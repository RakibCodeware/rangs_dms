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

public partial class issue_delivery : System.Web.UI.Page
{
    SqlConnection conn = DBConnection.GetConnection();
    SqlConnection conn1 = DBConnection.GetConnection();
    SqlConnection _connStr = DBConnection.GetConnection();
    long iMRSRID = 0;
    DataTable dt;
    DateTime tDate;

    private double runningTotal = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        btnAdd.Attributes.Add("OnClick", "return confirm_Add();");
        btnSave.Attributes.Add("OnClick", "return confirm_Save();");
        btnCancel.Attributes.Add("OnClick", "return confirm_Cancel();");
        //ibtnDelete.Attributes.Add("OnClick", "return confirm_delete();");
        if (!IsPostBack)
        {
            if (System.Convert.ToInt32(Session["Vis"]) == 0)
            {
                Response.Redirect("../Account/Login.aspx");
            }

            dt = new DataTable();
            MakeTable();

            //LOAD PRODUCT MODEL
            LoadDropDownList();

            //LOAD CTP
            LoadDropDownList_CTP();

            //LOAD AUTO CHALLAN NUMBER
            if (Session["sBrId"].ToString() == "370")
            {
                fnLoadAutoBillNo_Issue_CIDD();
            }
            else
            {
                fnLoadAutoBillNo();
            }

            txtDate.Text = DateTime.Today.ToString("MM/dd/yyyy");

        }
        else
        {
            dt = (DataTable)ViewState["dt"];
        }
        ViewState["dt"] = dt;
        //txtDate.Text = String.Format("{0:t}", Now);       
        
               
    }

    //LOAD PRODUCT IN DROPDOWN LIST
    protected void LoadDropDownList()
    {
        SqlConnection conn = DBConnection.GetConnection();
        String strQuery = "select Model from Product WHERE Discontinue='No' ORDER BY Model";
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

    //LOAD CTP IN DROPDOWN LIST
    protected void LoadDropDownList_CTP()
    {
        SqlConnection conn = DBConnection.GetConnection();

        //string strQuery = "select EID,eName from Entity ";
        //strQuery = strQuery + " WHERE (ActiveDeactive = 1) AND";
        //strQuery = strQuery + " (EntityType = 'showroom') AND (EID='" + Session["sBrId"].ToString() + "')";
        //strQuery = strQuery + " ORDER BY eName";

        string strQuery = "SELECT dbo.Entity.EID, dbo.Entity.eName, dbo.Entity.EntityType,";
        strQuery = strQuery + " Entity_1.eName AS ZonaName, Entity_2.eName AS CTPName, Entity_2.EID AS CTPID,";
        strQuery = strQuery + " Entity_2.EntityType AS CTPType, Entity_2.ActiveDeactive";
        strQuery = strQuery + " FROM dbo.Entity INNER JOIN";
        strQuery = strQuery + " dbo.Entity AS Entity_1 ON dbo.Entity.ZoneID = Entity_1.EID INNER JOIN";
        strQuery = strQuery + " dbo.Entity AS Entity_2 ON Entity_1.EID = Entity_2.ParentEntity";
        strQuery = strQuery + " WHERE (dbo.Entity.EntityType = 'Sub Store')";
        strQuery = strQuery + " AND (dbo.Entity.EID = '" + Session["sBrId"].ToString() + "')";
        strQuery = strQuery + " AND (Entity_2.EntityType = 'Showroom') AND (Entity_2.ActiveDeactive = 1)";


        //SqlConnection con = new SqlConnection("conn");
        SqlCommand cmd = new SqlCommand(strQuery, conn);
        //SqlCommand cmd = new SqlCommand();
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = strQuery;
        cmd.Connection = conn;
        try
        {
            conn.Open();
            ddlEntity.DataSource = cmd.ExecuteReader();
            ddlEntity.DataTextField = "CTPName";
            ddlEntity.DataValueField = "CTPID";
            ddlEntity.DataBind();

            //Add blank item at index 0.
            //ddlEntity.Items.Insert(0, new ListItem("CI&DD (REL)","370"));

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
        dt.Columns.Add("ProductSL");
        dt.Columns.Add("Remarks");

    }

    protected void PopupMessage(string Msg, Control controlID)
    {
        ScriptManager.RegisterClientScriptBlock(controlID, controlID.GetType(), "msg", "alert('" + Msg + "');", true);

    }

    protected void AddRows()
    {
        if (System.Convert.ToInt32(Session["Vis"]) == 0)
        {
            Response.Redirect("../Account/Login.aspx");
        }

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
        dr["ProductSL"] = txtSL.Text;
        dr["Remarks"] = txtRemarks.Text;
        //dr["CampDis"] = dr["MRP"].ToString() - dr["CampaignPrice"].ToString();
        dt.Rows.Add(dr);

        //lblProdSerial.Text = lblProdSerial + "" + txtSL.Text + ", ";

        //CLEAR ALL TEXT
        //txtProdID.Text = "";
        //txtProdDesc.Text = "";
        txtQty.Text = "";       
        txtSL.Text = "";
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
                this.txtProdID.Text = "";
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
        txtSL.Text = "";
        txtRemarks.Text = "";

        //CLEAR GRIDVIEW
        gvUsers.DataSource = null;
        gvUsers.DataBind();

        dt.Clear();

        //LOAD AUTO CHALLAN NUMBER
        fnLoadAutoBillNo();

        txtCHNo.Focus();

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
        if (System.Convert.ToInt32(Session["Vis"]) == 0)
        {
            Response.Redirect("../Account/Login.aspx");
        }

        SqlConnection conn = DBConnection.GetConnection();
        //fnLoadMRSRNo();

        string sSql = "";

        //CHALLAN VALIDATION        
        if (txtCHNo.Text == "")
        {
            PopupMessage("Please enter Transfer Challan Number.", btnSave);
            txtCHNo.Focus();
            return;
        }

        if (ddlEntity.SelectedItem.Text == "")
        {
            PopupMessage("Please Select Entity Name.", btnSave);
            //txtDate.Focus();
            return;
        }

        //CHALLAN DATE VALIDATION        
        if (txtDate.Text == "")
        {
            PopupMessage("Please enter Date.", btnSave);
            txtDate.Focus();
            return;
        }

        tDate = Convert.ToDateTime(this.txtDate.Text);

        //LOAD AUTO CHALLAN NUMBER
        //fnLoadAutoBillNo();

        int iTrType = 0;
        //LOAD AUTO CHALLAN NUMBER
        if (Session["sBrId"].ToString() == "370")
        {
            iTrType = 2;
            fnLoadAutoBillNo_Issue_CIDD();
        }
        else
        {
            iTrType = 4;
            fnLoadAutoBillNo();
        }

        //----------------------------------------------------------------------
        //CHECK DUPLICATE CHALLAN NO.
        sSql = "";
        sSql = "SELECT MRSRMID FROM MRSRMaster";
        sSql = sSql + " WHERE MRSRCode='" + this.txtCHNo.Text + "'";
        sSql = sSql + " AND TrType='" + iTrType + "'";
        
        SqlCommand cmdd = new SqlCommand(sSql, conn);
        conn.Open();
        SqlDataReader drd = cmdd.ExecuteReader();
        try
        {
            if (drd.Read())
            {
                //iMRSRID = Convert.ToInt32(dr["MRSRMID"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert",
                            "<script>alert('" + "This Challan no. already exists." + "');</script>", false);
                txtCHNo.Focus();
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
        sSql = "INSERT INTO MRSRMaster(MRSRCode,TDate,TrType," +
               "InvoiceNo,InSource,OutSource,UserID,EntryDate," +
                " Tag,OnLineSales,Remarks)" +
                " Values ('" + this.txtCHNo.Text + "','" + tDate + "','" + iTrType + "'," +
                " '" + this.txtCHNo.Text + "','" + ddlEntity.SelectedItem.Value + "'," +
                //" '370'," +
                " '" + Session["sBrId"] + "','" + Session["UserName"] + "'," +
                " '" + DateTime.Today + "','2','1','" + this.txtNote.Text + "'" +                    
                " )";
        SqlCommand cmd = new SqlCommand(sSql, conn);
        conn.Open();
        cmd.ExecuteNonQuery();
        conn.Close();
        

        //RETRIVE MASTER ID         
        sSql = "";
        sSql = "SELECT MRSRMID FROM MRSRMaster" +
            " WHERE MRSRCode='" + this.txtCHNo.Text + "'" +
            " AND TrType='" + iTrType + "'";
        cmd = new SqlCommand(sSql, conn);
        conn.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            iMRSRID = Convert.ToInt32(dr["MRSRMID"].ToString());           
        }
        dr.Dispose();
        dr.Close();
        conn.Close();


        //------------------------------------------------------------------------------------------
        //SAVE DATA IN DETAILS TABLE
        foreach (GridViewRow g1 in this.gvUsers.Rows)
        {                       
            string sProdSL = "";
            if (g1.Cells[3].Text.Trim() != "&nbsp;")
            {
                sProdSL = g1.Cells[3].Text.Trim();
            }
            else
            {
                sProdSL = g1.Cells[3].Text = "";
            }

            string sRemarks = "";
            if (g1.Cells[4].Text.Trim() != "&nbsp;")
            {
                sRemarks = g1.Cells[4].Text.Trim();
            }
            else
            {
                sRemarks = g1.Cells[4].Text = "";
            }

            string gSql = "";
            gSql = "INSERT INTO MRSRDetails(MRSRMID," +
                 " ProductID,Qty,SLNO,ProdRemarks" +                 
                 " )" +
                 " VALUES('" + iMRSRID + "'," +
                 " '" + g1.Cells[0].Text + "'," +
                 " '" + g1.Cells[2].Text + "'," +
                 " '" + g1.Cells[3].Text + "'," +
                 " '" + g1.Cells[4].Text + "'" +                 
                 " )";
            SqlCommand cmdIns = new SqlCommand(gSql, conn);

            conn.Open();
            cmdIns.ExecuteNonQuery();
            conn.Close();

        }

        //------------------------------------------------------------------------------------------

        //lblSaveMessage.Text = "Save Data Successfully.";

        //ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert",
        //                "<script>alert('" + "Save Successfully." + "');</script>", false);

        ////------------------------------------------------------------------------------------------
        ////CLEAR ALL TEXT
        //txtCHNo.Text = "";
        //txtDate.Text = "";
        //txtDate.Text = DateTime.Today.ToString("dd/MM/yyyy");

        //txtProdID.Text = "";
        //txtProdDesc.Text = "";
        //txtSL.Text = "";
        //txtRemarks.Text = "";

        ////CLEAR GRIDVIEW
        //gvUsers.DataSource = null;
        //gvUsers.DataBind();

        //dt.Clear();

        ////LOAD AUTO CHALLAN NUMBER
        //fnLoadAutoBillNo();

        //txtCode.Focus();
        //------------------------------------------------------------------------------------------

        if (iTrType == 2)
        {
            Session["sIssueChNo"] = txtCHNo.Text;

            Response.Redirect("Issue_Ch_Print.aspx");
        }
        else
        {
            Session["sTrChNo"] = txtCHNo.Text;

            Response.Redirect("Transfer_Ch_Print.aspx");
        }
        return;

    }


    protected void fnLoadAutoBillNo()
    {

        SqlConnection con = DBConnection.GetConnection();
        //con.Open();

        int xMax = 0;
        string sAutoNo = "";
        string sSql = "";
        sSql = "SELECT ISNULL(MAX(RIGHT(MRSRCode, 4)), 0) AS MRSRCode" +
            " FROM dbo.MRSRMaster WHERE TrType=4" +
            " AND (LEFT(MRSRCode, 12) = '" + "4" + Session["sBrCode"] + "" + DateTime.Now.ToString("yyyy") + "-" + "')";
        //" AND TrType=3";

        SqlCommand cmd = new SqlCommand(sSql, con);
        con.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        try
        {
            if (dr.Read())
            {
                //xMax = dr["JobNo"].ToString();
                xMax = Convert.ToInt32(dr["MRSRCode"]) + 1;
                sAutoNo = "4" + Session["sBrCode"] + "" + DateTime.Now.ToString("yyyy") + "-" + xMax.ToString("0000");
                txtCHNo.Text = sAutoNo;
            }
            else
            {
                xMax = Convert.ToInt32(dr["MRSRCode"]) + 1;
                sAutoNo = "4" + Session["sBrCode"] + "" + DateTime.Now.ToString("yyyy") + "-" + xMax.ToString("0000");
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

    protected void fnLoadAutoBillNo_Issue_CIDD()
    {

        SqlConnection con = DBConnection.GetConnection();
        //con.Open();

        int xMax = 0;
        string sAutoNo = "";
        string sSql = "";
        sSql = "SELECT ISNULL(MAX(RIGHT(MRSRCode, 3)), 0) AS MRSRCode" +
            " FROM dbo.MRSRMaster WHERE TrType=2" +
            " AND (LEFT(MRSRCode, 6) = '" + DateTime.Now.ToString("dd") + "" + DateTime.Now.ToString("MM") + "" + DateTime.Now.ToString("yy") + "')";
        //" AND TrType=3";

        SqlCommand cmd = new SqlCommand(sSql, con);
        con.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        try
        {
            if (dr.Read())
            {
                //xMax = dr["JobNo"].ToString();
                xMax = Convert.ToInt32(dr["MRSRCode"]) + 1;
                sAutoNo = "" + DateTime.Now.ToString("dd") + "" + DateTime.Now.ToString("MM") + "" + DateTime.Now.ToString("yy") + "-" + xMax.ToString("000");
                txtCHNo.Text = sAutoNo;
            }
            else
            {
                xMax = Convert.ToInt32(dr["MRSRCode"]) + 1;
                sAutoNo = "" + DateTime.Now.ToString("dd") + "" + DateTime.Now.ToString("MM") + "" + DateTime.Now.ToString("yy") + "-" + xMax.ToString("000");
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

    protected void txtCode_TextChanged(object sender, EventArgs e)
    {
        SqlConnection conn = DBConnection.GetConnection();
        string sSql = "";

        sSql = "";
        sSql = "SELECT * FROM Product" +
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

}
