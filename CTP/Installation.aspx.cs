using System;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;

using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Globalization;
using System.Text.RegularExpressions;

public partial class Installation : System.Web.UI.Page
{

    SqlConnection conn = DBConnection.GetConnection();
    SqlConnection conn1 = DBConnection.GetConnection();
    SqlConnection _connStr = DBConnection.GetConnection();
    int iMRSRID = 0;
    DataTable dt;
    DateTime tDate;

    private double runningTotal = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        btnAdd.Attributes.Add("OnClick", "return confirm_Add();");
        btnSave.Attributes.Add("OnClick", "return confirm_Save();");
        btnCancel.Attributes.Add("OnClick", "return confirm_Cancel();");
        //ibtnDelete.Attributes.Add("OnClick", "return confirm_delete();");
        //ddlContinents.Attributes.Add("onChange", "DisplayText();");

        if (System.Convert.ToInt32(Session["Vis"]) == 0)
        {
            Response.Redirect("../Account/Login.aspx");
        }

        if (!IsPostBack)
        {
            dt = new DataTable();
            MakeTable();
            LoadDropDownList();

            //LOAD AUTO REQUEST NUMBER
            fnLoadAutoBillNo();

            //LOAD VENDOR NAME
            LoadDropDownList_Vendor();

            //LOAD DISTRICT
            LoadDropDownList_City();

        }
        else
        {
            dt = (DataTable)ViewState["dt"];
        }
        ViewState["dt"] = dt;

        //txtDate.Text = String.Format("{0:t}", Now);       
        txtDate.Text = DateTime.Today.ToString("MM/dd/yyyy");
        txtInsDate.Text = DateTime.Today.ToString("MM/dd/yyyy");
        
    }

    protected void fnLoadAutoBillNo()
    {

        SqlConnection con = DBConnection.GetConnection();
        //con.Open();

        int xMax = 0;
        string sAutoNo = "";
        string sSql = "";
        sSql = "SELECT ISNULL(MAX(RIGHT(RefNo, 5)), 0) AS RefNo" +
            " FROM dbo.tbInstallationMaster" +
            " WHERE (LEFT(RefNo, 11) = '" + "" + Session["sBrCode"] + "" + DateTime.Now.ToString("yyyy") + "-" + "')";
            //" AND TrType=3";

        SqlCommand cmd = new SqlCommand(sSql, con);
        con.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        try
        {
            if (dr.Read())
            {
                //xMax = dr["JobNo"].ToString();
                xMax = Convert.ToInt32(dr["RefNo"]) + 1;
                sAutoNo = "" + Session["sBrCode"] + "" + DateTime.Now.ToString("yyyy") + "-" + xMax.ToString("00000");
                txtCHNo.Text = sAutoNo;
            }
            else
            {
                xMax = Convert.ToInt32(dr["RefNo"]) + 1;
                sAutoNo = "" + Session["sBrCode"] + "" + DateTime.Now.ToString("yyyy") + "-" + xMax.ToString("00000");
                txtCHNo.Text = sAutoNo;
            }
        }
        catch (InvalidCastException err)
        {
            throw (err);
        }
        finally
        {
            dr.Dispose();
            dr.Close();
            con.Close();
        }
    }

    //LOAD PRODUCT IN DROPDOWN LIST
    protected void LoadDropDownList()
    {
        SqlConnection con = DBConnection.GetConnection();
        
        String strQuery = "select Model from Product WHERE Discontinue='No' Order By Model";
        //SqlConnection con = new SqlConnection("conn");
        SqlCommand cmd = new SqlCommand(strQuery, conn);
        //SqlCommand cmd = new SqlCommand();
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = strQuery;
        cmd.Connection = conn;
        try
        {
            conn.Open();
            ddlContinents.DataSource = cmd.ExecuteReader();
            ddlContinents.DataTextField = "Model";
            //ddlContinents.DataValueField = "ProductID";
            ddlContinents.DataValueField = "Model";
            ddlContinents.DataBind();

            //Add blank item at index 0.
            ddlContinents.Items.Insert(0, new ListItem("", ""));

        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            conn.Close();
            conn.Dispose();
        }
    }

    protected void MakeTable()
    {
        //dt.Columns.Add("ID").AutoIncrement = true;
        dt.Columns.Add("ProductID");
        //dt.Columns.Add("ProductID", typeof(SqlInt32));
        dt.Columns.Add("Model");
        dt.Columns.Add("Qty");        
        dt.Columns.Add("Remarks");

    }

    protected void PopupMessage(string Msg, Control controlID)
    {
        ScriptManager.RegisterClientScriptBlock(controlID, controlID.GetType(), "msg", "alert('" + Msg + "');", true);

    }

    protected void AddRows()
    {
        if (txtProdID.Text == "")
        {
            PopupMessage("Please select product Model.", btnAdd);
            txtProdID.Focus();
            return;
        }

        if (txtQty.Text == "")
        {
            PopupMessage("Please enter Quantity.", btnAdd);
            txtQty.Focus();
            return;
        }

        DataRow dr = dt.NewRow();
        dr["ProductID"] = txtProdID.Text;
        //dr["Model"] = ddlContinents.Text; //Model
        dr["Model"] = ddlContinents.SelectedItem.Text;
        dr["Qty"] = txtQty.Text;        
        dr["Remarks"] = txtRemarks.Text;
        //dr["CampDis"] = dr["MRP"].ToString() - dr["CampaignPrice"].ToString();
        dt.Rows.Add(dr);

        //CLEAR ALL TEXT
        txtProdID.Text = "";
        txtProdDesc.Text = "";
        txtQty.Text = "";        
        txtRemarks.Text = "";

    }

    //ADD DATA IN GRIDVIEW
    protected void btnAdd_Click(object sender, EventArgs e)
    {

        //FUNCTION FOR ADD ROW
        try
        {
            AddRows();
            gvUsers.DataSource = dt;
            gvUsers.DataBind();
        }
        catch (InvalidCastException err)
        {
            throw (err);
        }

    }

    //GRID ROW DELETE
    protected void gvUsers_RowDelating(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(e.RowIndex);
            DataTable dt = ViewState["dt"] as DataTable;
            dt.Rows[index].Delete();
            ViewState["dt"] = dt;
            BindGrid();
        }
        catch (InvalidCastException err)
        {
            throw (err);
        }

    }


    protected void BindGrid()
    {
        gvUsers.DataSource = ViewState["dt"] as DataTable;
        gvUsers.DataBind();
    }

    protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string item = e.Row.Cells[0].Text;
            foreach (Button button in e.Row.Cells[2].Controls.OfType<Button>())
            {
                if (button.CommandName == "Delete")
                {
                    button.Attributes["onclick"] = "if(!confirm('Do you want to delete " + item + "?')){ return false; };";
                }
            }
        }
    }

    //SELECT PRODUCT FROM Drop Down Menu
    protected void ddlContinents_SelectedIndexChanged(object sender, EventArgs e)
    {
        SqlConnection conn = DBConnection.GetConnection();
        string sSql = "";

        sSql = "";
        sSql = "SELECT * FROM Product" +
            " WHERE Model='" + this.ddlContinents.SelectedItem.Text + "'";
        SqlCommand cmd = new SqlCommand(sSql, conn);
        conn.Open();
        SqlDataReader dr = cmd.ExecuteReader();

        try
        {
            if (dr.Read())
            {
                this.txtProdID.Text = dr["ProductID"].ToString();
                this.txtProdDesc.Text = dr["ProdName"].ToString();
                this.txtCode.Text = dr["Code"].ToString();
            }
            else
            {
                this.txtProdDesc.Text = "";
                this.txtCode.Text = "";
            }
        }
        catch (InvalidCastException err)
        {
            throw (err);
        }
        finally
        {
            dr.Dispose();
            dr.Close();
            conn.Close();
        }
        
    }

    //CLEAR ALL TEXT AND GRID
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        //CLEAR ALL TEXT
        txtCHNo.Text = "";
        txtDate.Text = "";
        txtDate.Text = DateTime.Today.ToString("dd/MM/yyyy");

        txtProdID.Text = "";        
        txtRemarks.Text = "";

        //CLEAR GRIDVIEW
        gvUsers.DataSource = null;
        gvUsers.DataBind();

        dt.Clear();

        txtCHNo.Focus();

        //LOAD AUTO REQUEST NUMBER
        fnLoadAutoBillNo();

    }

    //Grid View Footer Total
    protected void gvUsers_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CalcTotal(e.Row.Cells[2].Text);

            double value2 = Convert.ToDouble(e.Row.Cells[2].Text);
            e.Row.Cells[2].Text = value2.ToString("0");

            e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Center;

        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[1].Text = "Total";
            //e.Row.Cells[10].Text = string.Format("{0:c}", runningTotal);
            e.Row.Cells[2].Text = runningTotal.ToString("0,0", CultureInfo.InvariantCulture);
            this.lblNetAmnt.Text = runningTotal.ToString("0,0", CultureInfo.InvariantCulture);

            e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Right;
        }
    }

    private void CalcTotal(string _price)
    {
        try
        {
            runningTotal += Double.Parse(_price);
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write(ex.ToString());
        }
    }


    //FINALLY SAVE DATA
    protected void btnSave_Click(object sender, EventArgs e)
    {
        SqlConnection conn = DBConnection.GetConnection();
        //fnLoadMRSRNo();

        string sSql = "";

        //CHALLAN VALIDATION        
        if (txtCHNo.Text == "")
        {            
            ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert",
                "<script>alert('" + "Please enter Request Number." + "');</script>", false);
            txtCHNo.Focus();
            return;
        }

        //CHALLAN DATE VALIDATION        
        if (txtChallanNo.Text == "")
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert",
                "<script>alert('" + "Please enter Challan Number." + "');</script>", false);
            txtChallanNo.Focus();
            return;
        }

        if (txtCustContact.Text.Length == 0)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert",
                "<script>alert('" + "Please enter customer mobile number." + "');</script>", false);
            txtCustContact.Focus();
            return;
        }

        if (txtVAID.Text.Length == 0)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert",
                "<script>alert('" + "Please select vendor name." + "');</script>", false);
            //txtVAID.Focus();
            return;
        }
        if (txtVAID.Text == "0")
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert",
                "<script>alert('" + "Please select vendor name." + "');</script>", false);
            //txtVAID.Focus();
            return;
        }

        string sThana = "";
        if (this.ddlThana.SelectedItem.Text.Length == 0)
        {
            sThana = "N/A";
        }
        else
        {
            sThana = this.ddlThana.SelectedItem.Text;
        }

        tDate = Convert.ToDateTime(this.txtDate.Text);


        //----------------------------------------------------------------------
        //CHECK VENDOR JOB NUMBER 
        int vJobNo = 0;
        sSql = "";
        sSql = "SELECT COUNT(IAID) AS tJob FROM tbInstallationMaster" +
            " WHERE VAID='" + txtVAID.Text + "' AND RefDate='" + tDate + "'";
        //" WHERE MRSRCode='" + this.txtMRSR.Text + 'S' + "'" +
        //" AND TrType=4";
        SqlCommand cmdd = new SqlCommand(sSql, conn);
        conn.Open();
        SqlDataReader drd = cmdd.ExecuteReader();
        try
        {
            if (drd.Read())
            {
                vJobNo = Convert.ToInt32(drd["tJob"].ToString());
                if (vJobNo >= 5)
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert",
                                "<script>alert('" + "This Vendor's has already " + vJobNo + " job assign." + "');</script>", false);
                    //txtChallanNo.Focus();
                    return;
                }
            }
        }
        catch (InvalidCastException err)
        {
            throw (err);
        }
        finally
        {
            drd.Dispose();
            drd.Close();
            conn.Close();
        }
        //----------------------------------------------------------------------

        //LOAD AUTO REQUEST NUMBER
        fnLoadAutoBillNo();


        //----------------------------------------------------------------------
        //CHECK DUPLICATE CHALLAN NO.
        sSql = "";
        sSql = "SELECT IAID FROM tbInstallationMaster" +
            " WHERE InvNo='" + this.txtChallanNo.Text + "'";
            //" WHERE MRSRCode='" + this.txtMRSR.Text + 'S' + "'" +
            //" AND TrType=4";
        cmdd = new SqlCommand(sSql, conn);
        conn.Open();
        drd = cmdd.ExecuteReader();
        try
        {
            if (drd.Read())
            {
                //iMRSRID = Convert.ToInt32(dr["MRSRMID"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert",
                            "<script>alert('" + "This challan no. already exists." + "');</script>", false);
                txtChallanNo.Focus();
                return;
            }
        }
        catch (InvalidCastException err)
        {
            throw (err);
        }
        finally
        {
            drd.Dispose();
            drd.Close();
            conn.Close();
        }
        //----------------------------------------------------------------------


        //SAVE DATA IN MASTER TABLE
        sSql = "";
        sSql = "INSERT INTO tbInstallationMaster(RefNo,RefDate,";
        sSql = sSql + " InvNo, MRNo,";
        sSql = sSql + " CustMobile,CustName,";
        sSql = sSql + " CustAdd,VAID,";
        sSql = sSql + " CustDist,CustThana,";
        sSql = sSql + " InstDateAprx, InstDateAprx1, ";
        sSql = sSql + " InstTimeAprx, SpecNote,";
        sSql = sSql + " EID,UserID,EntryDate)";
        sSql = sSql + " Values ('" + this.txtCHNo.Text + "','" + tDate + "',";
        sSql = sSql + " '" + this.txtChallanNo.Text + "','N/A',";
        sSql = sSql + " '" + this.txtCustContact.Text + "','" + this.txtCustName.Text + "',";
        sSql = sSql + " '" + this.txtCustAdd.Text + "','" + txtVAID.Text + "',";
        sSql = sSql + " '" + this.ddlDist.SelectedItem.Text + "','" + sThana + "',";
        sSql = sSql + " '" + this.txtInsDate.Text + "','" + this.txtInsDate.Text + "',";
        sSql = sSql + " '" + this.txtInsTime.Text + "','" + txtNote.Text + "',";
        sSql = sSql + " '" + Session["EID"] + "','" + Session["UserName"] + "', ";
        sSql = sSql + " '" + DateTime.Today + "'";
        sSql = sSql + " )";
        SqlCommand cmd = new SqlCommand(sSql, conn);
        conn.Open();
        cmd.ExecuteNonQuery();
        conn.Close();


        //RETRIVE MASTER ID         
        sSql = "";
        sSql = "SELECT IAID FROM tbInstallationMaster" +
            " WHERE RefNo='" + this.txtCHNo.Text + "'";            
        cmd = new SqlCommand(sSql, conn);
        conn.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            iMRSRID = Convert.ToInt32(dr["IAID"].ToString());
        }
        dr.Dispose();
        dr.Close();
        conn.Close();


        //------------------------------------------------------------------------------------------
        //SAVE DATA IN DETAILS TABLE
        foreach (GridViewRow g1 in gvUsers.Rows)
        {           
            string sRemarks = "";
            if (g1.Cells[3].Text.Trim() != "&nbsp;")
            {
                sRemarks = g1.Cells[3].Text.Trim();
            }
            else
            {
                sRemarks = g1.Cells[3].Text = "";
            }

            string gSql = "";
            gSql = "INSERT INTO tbInstallationDetails(IAID," +
                 " ProductID,tQty,ProdRemarks" +
                 " )" +
                 " VALUES('" + iMRSRID + "'," +
                 " '" + g1.Cells[0].Text + "'," +
                 " '" + g1.Cells[2].Text + "'," +
                 " '" + g1.Cells[3].Text + "'" +                
                 " )";
            SqlCommand cmdIns = new SqlCommand(gSql, conn);

            conn.Open();
            cmdIns.ExecuteNonQuery();
            conn.Close();

        }

        //RETRIVE PRODUCT DETAILS  
        string sProd = "";
        sSql = "";        
        sSql = "SELECT dbo.Product.GroupName, dbo.Product.Size_Capacity, dbo.Product.Size_Capacity_Unit,";
        sSql = sSql + " SUM(dbo.tbInstallationDetails.tQty) AS tQty";
        sSql = sSql + " FROM dbo.tbInstallationDetails INNER JOIN";
        sSql = sSql + " dbo.Product ON dbo.tbInstallationDetails.ProductID = dbo.Product.ProductID";
        sSql = sSql + " WHERE dbo.tbInstallationDetails.IAID='" + iMRSRID + "'";
        sSql = sSql + " GROUP BY dbo.Product.GroupName, dbo.Product.Size_Capacity, dbo.Product.Size_Capacity_Unit";

        cmd = new SqlCommand(sSql, conn);
        conn.Open();
        dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            sProd = sProd + "" + dr["GroupName"].ToString() + "(" + dr["Size_Capacity"].ToString() + "" + dr["Size_Capacity_Unit"].ToString() + ")-" + dr["tQty"].ToString() + " \n";
        }
        dr.Dispose();
        dr.Close();
        conn.Close();

        //*******************************************************************************************
        // FOR SMS   
        SqlConnection connSMS = DBConnectionSMS.GetConnection();

        if (txtVMobile.Text != "")
        {
            string smsText = "";
            smsText = "Dear " + this.txtVNickName.Text + ",\n";
            smsText = smsText + "Installation requirement from: " + Session["eName"].ToString() + "\n";
            smsText = smsText + "Customer:\n" + this.txtCustName.Text + ".\n";
            smsText = smsText + "" + this.txtCustContact.Text + ".\n";
            smsText = smsText + "" + this.txtCustAdd.Text + ".\n";
            smsText = smsText + "Install date: " + this.txtInsDate.Text + "";
            //smsText = smsText + "Product:\n";
            //smsText = smsText + "" + sProd + "";
            //smsText = smsText + "Thank you for shopping with us.\n";
            //smsText = smsText + "Sony-Rangs";

            sSql = "";
            sSql = "INSERT INTO tbSMS(ContactNo,SMSText,UserID,EntryDate,SMSSource)" +
                    " Values ('" + this.txtVMobile.Text + "','" + smsText + "'," +
                    " '" + Session["UserID"] + "','" + DateTime.Today + "'," +
                    " 'DMS'" +
                    " )";
            SqlCommand cmdSMS = new SqlCommand(sSql, connSMS);
            connSMS.Open();
            cmdSMS.ExecuteNonQuery();
            connSMS.Close();
            //---------------------------------------------------------------------------------------
            smsText = "";
            //smsText = "Dear " + this.txtVNickName.Text + ",\n";
            smsText = "Product Details of: " + this.txtCustName.Text + ".\n";            
            smsText = smsText + "" + sProd + "";
            //smsText = smsText + "Thank you for shopping with us.\n";
            smsText = smsText + "Sony-Rangs";

            sSql = "";
            sSql = "INSERT INTO tbSMS(ContactNo,SMSText,UserID,EntryDate,SMSSource)" +
                    " Values ('" + this.txtVMobile.Text + "','" + smsText + "'," +
                    " '" + Session["UserID"] + "','" + DateTime.Today + "'," +
                    " 'DMS'" +
                    " )";
            cmdSMS = new SqlCommand(sSql, connSMS);
            connSMS.Open();
            cmdSMS.ExecuteNonQuery();
            connSMS.Close();

        }         
        //******************************************************************************************

        //------------------------------------------------------------------------------------------

        //lblSaveMessage.Text = "Save Data Successfully.";

        ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert",
                        "<script>alert('" + "Submit Successfully." + "');</script>", false);

        //------------------------------------------------------------------------------------------
        //CLEAR ALL TEXT
        txtCHNo.Text = "";
        txtDate.Text = "";
        txtDate.Text = DateTime.Today.ToString("dd/MM/yyyy");

        txtChallanNo.Text = "";
        txtChDate.Text = "";
        txtCustName.Text = "";
        txtCustContact.Text = "";
        txtCustAdd.Text = "";

        txtNote.Text = "";

        txtProdID.Text = "";
        txtProdDesc.Text = "";        
        txtRemarks.Text = "";

        //CLEAR GRIDVIEW
        gvUsers.DataSource = null;
        gvUsers.DataBind();

        dt.Clear();

        //txtCHNo.Focus();
        //LOAD AUTO REQUEST NUMBER
        fnLoadAutoBillNo();

        //------------------------------------------------------------------------------------------

        return;

    }


    protected void txtCode_TextChanged(object sender, EventArgs e)
    {
        SqlConnection conn = DBConnection.GetConnection();
        string sSql = "";

        sSql = "";
        sSql = "SELECT ProductID,Model,ProdName,Code FROM Product" +
            " WHERE Code='" + this.txtCode.Text + "'";
        SqlCommand cmd = new SqlCommand(sSql, conn);
        conn.Open();
        SqlDataReader dr = cmd.ExecuteReader();

        try
        {
            if (dr.Read())
            {
                this.txtProdID.Text = dr["ProductID"].ToString();
                this.txtProdDesc.Text = dr["ProdName"].ToString();
                //this.txtCode.Text = dr["Code"].ToString();
                this.ddlContinents.SelectedItem.Text = dr["Model"].ToString();
            }
            else
            {
                this.txtProdID.Text = "";
                this.txtProdDesc.Text = "";
                this.ddlContinents.SelectedItem.Text = "";
            }
        }
        catch (InvalidCastException err)
        {
            throw (err);
        }
        finally
        {
            dr.Dispose();
            dr.Close();
            conn.Close();
        }
    }


    //LOAD CUSTOMER INFO
    protected void btnCustSearch_Click(object sender, ImageClickEventArgs e)
    {
        SqlConnection conn = DBConnection.GetConnection();

        if (txtCustContact.Text == "")
        {            
            ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert",
                "<script>alert('" + "Please enter customer contact # ..." + "');</script>", false);
            txtCustContact.Focus();
            return;
        }

        //CHECK CUSTOMER INFO
        string sSql = "";
        sSql = "SELECT * FROM Customer" +
            " WHERE Mobile='" + this.txtCustContact.Text + "'";
        SqlCommand cmdCust = new SqlCommand(sSql, conn);
        conn.Open();
        SqlDataReader drCust = cmdCust.ExecuteReader();
        try
        {
            if (drCust.Read())
            {
                this.txtCustName.Text = drCust["CustName"].ToString();
                this.txtCustAdd.Text = drCust["Address"].ToString();
                //this.txtEmail.Text = drCust["Email"].ToString();
                //this.ddlCity.SelectedItem.Text = drCust["City"].ToString();                                               

            }
            else
            {
                this.txtCustName.Text = "";
                this.txtCustAdd.Text = "";
                //this.txtEmail.Text = "";
                //this.ddlCity.SelectedItem.Text = "";
                
            }
        }
        catch (InvalidCastException err)
        {
            throw (err);
        }
        finally
        {
            drCust.Dispose();
            drCust.Close();
            conn.Close();
        }
        //----------------------------------------------------------------------

    }

    //LOAD VENDOR NAME IN DROPDOWN LIST
    protected void LoadDropDownList_Vendor()
    {
        if (System.Convert.ToInt32(Session["Vis"]) == 0)
        {
            Response.Redirect("../Account/Login.aspx");
        }

        SqlConnection conn = DBConnection.GetConnection();
        String strQuery = "select VAID,VName from tbVendorInfo WHERE status=1 Order By VName";
        //SqlConnection con = new SqlConnection("conn");
        SqlCommand cmd = new SqlCommand(strQuery, conn);
        //SqlCommand cmd = new SqlCommand();
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = strQuery;
        cmd.Connection = conn;
        try
        {
            conn.Open();
            ddlVendorName.DataSource = cmd.ExecuteReader();
            ddlVendorName.DataTextField = "VName";
            //ddlVendorName.DataValueField = "ProductID";
            ddlVendorName.DataValueField = "VAID";
            ddlVendorName.DataBind();

            //Add blank item at index 0.
            ddlVendorName.Items.Insert(0, new ListItem("", "0"));


        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            conn.Close();
            conn.Dispose();
        }
    }

    //LOAD CITY/DIST IN DROPDOWN LIST
    protected void LoadDropDownList_City()
    {
        if (System.Convert.ToInt32(Session["Vis"]) == 0)
        {
            Response.Redirect("../Account/Login.aspx");
        }

        SqlConnection conn = DBConnection.GetConnection();

        String strQuery = "select DISTINCT Dist from tbDistThana Order By Dist";
        //SqlConnection con = new SqlConnection("conn");
        SqlCommand cmd = new SqlCommand(strQuery, conn);
        //SqlCommand cmd = new SqlCommand();
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = strQuery;
        cmd.Connection = conn;
        try
        {
            conn.Open();
            ddlDist.DataSource = cmd.ExecuteReader();
            ddlDist.DataTextField = "Dist";
            //ddlDist.DataValueField = "ProductID";
            ddlDist.DataValueField = "Dist";
            ddlDist.DataBind();

            //Add blank item at index 0.
            ddlDist.Items.Insert(0, new ListItem("", "0"));

        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            conn.Close();
            conn.Dispose();
        }
    }

    //LOAD CITY/DIST IN DROPDOWN LIST
    protected void LoadDropDownList_Thana()
    {
        if (System.Convert.ToInt32(Session["Vis"]) == 0)
        {
            Response.Redirect("../Account/Login.aspx");
        }

        SqlConnection conn = DBConnection.GetConnection();

        String strQuery = "Select DISTINCT Thana From tbDistThana";
        strQuery = strQuery + " WHERE Dist='" + this.ddlDist.SelectedItem.Text + "' Order By Thana";
        //SqlConnection con = new SqlConnection("conn");
        SqlCommand cmd = new SqlCommand(strQuery, conn);
        //SqlCommand cmd = new SqlCommand();
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = strQuery;
        cmd.Connection = conn;
        try
        {
            conn.Open();
            ddlThana.DataSource = cmd.ExecuteReader();
            ddlThana.DataTextField = "Thana";
            //ddlThana.DataValueField = "ProductID";
            ddlThana.DataValueField = "Thana";
            ddlThana.DataBind();

            //Add blank item at index 0.
            ddlThana.Items.Insert(0, new ListItem("", "0"));

        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            conn.Close();
            conn.Dispose();
        }
    }


    protected void ddlVendorName_SelectedIndexChanged(object sender, EventArgs e)
    {
        SqlConnection conn = DBConnection.GetConnection();
        string sSql = "";

        sSql = "";
        sSql = "SELECT VAID, VName, VAddress, VContact, NickName FROM tbVendorInfo" +
            " WHERE VName='" + this.ddlVendorName.SelectedItem.Text + "'";
        SqlCommand cmd = new SqlCommand(sSql, conn);
        conn.Open();
        SqlDataReader dr = cmd.ExecuteReader();

        try
        {
            if (dr.Read())
            {
                this.txtVAID.Text = dr["VAID"].ToString();
                this.txtVMobile.Text = dr["VContact"].ToString();
                this.txtVAdd.Text = dr["VAddress"].ToString();
                this.txtVNickName.Text = dr["NickName"].ToString(); 
            }
            else
            {
                this.txtVAID.Text = "";
                this.txtVMobile.Text = "";
                this.txtVAdd.Text = "";
                this.txtVNickName.Text = "";
            }
        }
        catch (InvalidCastException err)
        {
            throw (err);
        }
        finally
        {
            dr.Dispose();
            dr.Close();
            conn.Close();
        }
    }

    protected void ddlDist_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadDropDownList_Thana();
    }


    // SEARCH CHALLAN INFORMATION
    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {
        SqlConnection conn = DBConnection.GetConnection();

        if (txtChallanNo.Text == "")
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert",
                "<script>alert('" + "Please enter customer challan # ..." + "');</script>", false);
            txtChallanNo.Focus();
            return;
        }

        //CHECK CUSTOMER INFO
        string sSql = "";                
        sSql = "SELECT dbo.MRSRMaster.MRSRCode, CONVERT(varchar, dbo.MRSRMaster.TDate, 110) AS ttDate,";
        sSql = sSql + " dbo.Customer.CustName, dbo.Customer.Address, dbo.Customer.Mobile, dbo.Customer.City,"; 
        sSql = sSql + " dbo.Entity.eName, dbo.MRSRMaster.TrType";
        sSql = sSql + " FROM dbo.MRSRMaster INNER JOIN";
        sSql = sSql + " dbo.Customer ON dbo.MRSRMaster.Customer = dbo.Customer.Mobile INNER JOIN";
        sSql = sSql + " dbo.Entity ON dbo.MRSRMaster.OutSource = dbo.Entity.EID";
        sSql = sSql + " WHERE (dbo.MRSRMaster.TrType = 3)";
        sSql = sSql + " AND (dbo.MRSRMaster.MRSRCode='" + this.txtChallanNo.Text + "')";

        SqlCommand cmdCust = new SqlCommand(sSql, conn);
        conn.Open();
        SqlDataReader drCust = cmdCust.ExecuteReader();
        try
        {
            if (drCust.Read())
            {
                this.txtChDate.Text = drCust["ttDate"].ToString();                
                this.txtCustName.Text = drCust["CustName"].ToString();
                this.txtCustAdd.Text = drCust["Address"].ToString();
                //this.txtEmail.Text = drCust["Email"].ToString();
                this.ddlDist.SelectedItem.Text = drCust["City"].ToString();                                               

            }
            else
            {
                this.txtChDate.Text = "";
                this.txtCustName.Text = "";
                this.txtCustAdd.Text = "";
                //this.txtEmail.Text = "";
                this.ddlDist.SelectedItem.Text = "";

            }
        }
        catch (InvalidCastException err)
        {
            throw (err);
        }
        finally
        {
            drCust.Dispose();
            drCust.Close();
            conn.Close();
        }
        //----------------------------------------------------------------------
    }


}