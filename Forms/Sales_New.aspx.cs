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
using System.Data.SqlTypes;

public partial class Forms_Sales_New : System.Web.UI.Page
{
    SqlConnection conn = DBConnection.GetConnection();
    SqlConnection conn1 = DBConnection.GetConnection();
    SqlConnection _connStr = DBConnection.GetConnection();
    int iMRSRID = 0;
    DataTable dt;
    DateTime tDate;
    DateTime tChequeDate;

    private double runningTotal = 0;
    private double runningTotalTP = 0;
    private double runningTotalDis = 0;
    private double runningTotalWith = 0;
    private double runningTotalQty = 0;

    #region[Page Load event]
    protected void Page_Load(object sender, EventArgs e)
    {
        if (System.Convert.ToInt32(Session["Vis"]) == 0)
        {
            Response.Redirect("../Account/Login.aspx");
        }

        btnAdd.Attributes.Add("OnClick", "return confirm_Add();");
        btnSave.Attributes.Add("OnClick", "return confirm_Save();");
        btnCancel.Attributes.Add("OnClick", "return confirm_Cancel();");
        //ibtnDelete.Attributes.Add("OnClick", "return confirm_delete();");
        if (!IsPostBack)
        {
            dt = new DataTable();
            MakeTable();
            LoadDropDownList();
            fnLoadAutoBillNo();
        }
        else
        {
            dt = (DataTable)ViewState["dt"];
        }
        ViewState["dt"] = dt;
        //txtDate.Text = String.Format("{0:t}", Now);       
        //txtDate.Text = DateTime.Today.ToString("MM/dd/yyyy");
               
    }
    #endregion

    protected void fnLoadAutoBillNo()
    {

        SqlConnection con = DBConnection.GetConnection();
        //con.Open();

        int xMax = 0;
        string sAutoNo = "";
        string sSql = "";
        sSql = "SELECT ISNULL(MAX(RIGHT(MRSRCode, 5)), 0) AS BillNo" +
            " FROM dbo.MRSRMaster" +
            " WHERE (LEFT(MRSRCode, 12) = '" + "" + Session["sBrCode"] + "/" + DateTime.Now.ToString("yyyy") + "/" + "')" +
            " AND TrType=3";

        SqlCommand cmd = new SqlCommand(sSql, con);
        con.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        try
        {
            if (dr.Read())
            {
                //xMax = dr["JobNo"].ToString();
                xMax = Convert.ToInt32(dr["BillNo"]) + 1;
                sAutoNo = "" + Session["sBrCode"] + "/" + DateTime.Now.ToString("yyyy") + "/" + xMax.ToString("00000");
                txtCHNo.Text = sAutoNo;
            }
            else
            {
                xMax = Convert.ToInt32(dr["BillNo"]) + 1;
                sAutoNo = "" + Session["sBrCode"] + "/" + DateTime.Now.ToString("yyyy") + "/" + xMax.ToString("00000");
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
        if (System.Convert.ToInt32(Session["Vis"]) == 0)
        {
            Response.Redirect("../Account/Login.aspx");
        }

        SqlConnection conn = DBConnection.GetConnection();
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
        dt.Columns.Add("MRP");
        dt.Columns.Add("CampaignPrice");
        dt.Columns.Add("Qty");
        dt.Columns.Add("TotalPrice");
        dt.Columns.Add("DisAmnt");
        dt.Columns.Add("DisCode");
        dt.Columns.Add("DisRef");
        dt.Columns.Add("WithAdjAmnt");
        dt.Columns.Add("NetAmnt");
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
        dr["MRP"] = txtUP.Text;
        dr["CampaignPrice"] = txtCP.Text;
        dr["Qty"] = txtQty.Text;
        dr["TotalPrice"] = txtTotalAmnt.Text;        
        dr["DisAmnt"] = txtDisAmnt.Text;
        dr["DisCode"] = txtDisCode.Text;
        dr["DisRef"] = txtDisRef.Text;        
        dr["WithAdjAmnt"] = txtWithAdj.Text;        
        dr["NetAmnt"] = Convert.ToDouble(txtNet.Text);
        dr["ProductSL"] = txtSL.Text;
        dr["Remarks"] = txtRemarks.Text;
        //dr["CampDis"] = dr["MRP"].ToString() - dr["CampaignPrice"].ToString();
        dt.Rows.Add(dr);
        
        //CLEAR ALL TEXT
        txtProdID.Text = "";
        txtProdDesc.Text = "";
        txtUP.Text = "";
        txtCP.Text = "";
        txtQty.Text = "";
        txtTotalAmnt.Text = "";
        txtDisAmnt.Text = "";
        txtDisCode.Text = "";
        txtDisRef.Text = "";
        txtWithAdj.Text = "";
        txtNet.Text = "";
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
        double UP = 0;
        double CampPrice = 0;
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
                UP = Convert.ToDouble(dr["UnitPrice"].ToString());
                this.txtUP.Text = Convert.ToString(UP);
            }
            else
            {
                UP = 0;
                this.txtUP.Text = Convert.ToString(UP);
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
        

        //LOAD CAMPAIGN PRICE
        sSql = "";
        sSql = "SELECT TOP 1 ProductID,Model,DisAmnt " +
            " FROM VW_CampaignInfo" +
            " WHERE Model='" + this.ddlContinents.SelectedValue + "'" +
            " AND (EffectiveDate<='" + DateTime.Today + "' AND cStop=0)" +
            " ORDER BY EffectiveDate DESC";
        cmd = new SqlCommand(sSql, conn);
        conn.Open();
        dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            CampPrice = UP - Convert.ToDouble(dr["DisAmnt"].ToString());
            this.txtCP.Text = Convert.ToString(CampPrice);
        }
        else
        {
            CampPrice = UP;
            this.txtCP.Text = Convert.ToString(CampPrice);
        }
        dr.Dispose();
        dr.Close();
        conn.Close();

    }
       
    //PRODUCT QUANTITY
    protected void txtQty_TextChanged(object sender, EventArgs e)
    {
        double tAmnt = 0;
        if (this.txtQty.Text == "")
        {
             //Response.Write("Please enter Quantity"); 
            //lblQty.Text = "Please enter Quantity";
        }
        else
        {
            //lblQty.Text = "";
            if (txtCP.Text.Length == 0)
            {
                this.txtCP.Text = "0";
            }
            if (txtTotalAmnt.Text.Length == 0)
            {
                this.txtTotalAmnt.Text = "0";
            }
            if (txtDisAmnt.Text.Length == 0)
            {
                this.txtDisAmnt.Text = "0";
            }
            if (txtWithAdj.Text.Length == 0)
            {
                this.txtWithAdj.Text = "0";
            }

            //if (txtCP.Text.Length > 0)
            //{
                tAmnt = Convert.ToDouble(this.txtQty.Text) * Convert.ToDouble(this.txtCP.Text);
                this.txtTotalAmnt.Text = Convert.ToString(tAmnt);
                //this.txtDisAmnt.Text = "0";
                //this.txtWithAdj.Text = "0";

                double dNet = 0;
                dNet = Convert.ToDouble(this.txtTotalAmnt.Text) - Convert.ToDouble(this.txtDisAmnt.Text) - Convert.ToDouble(this.txtWithAdj.Text);
                this.txtNet.Text = Convert.ToString(dNet);
            //}
           
        }
    }

    //DISCOUNT AMOUNT
    protected void txtDisAmnt_TextChanged(object sender, EventArgs e)
    {
        double dNet = 0;
        if (this.txtDisAmnt.Text == "")
        {
             Response.Write("Please enter Quantity"); 
            //lblQty.Text = "Please enter discount amount.";
        }
        else
        {
            //lblQty.Text = "";
            if (txtDisAmnt.Text.Length == 0)
            {
                this.txtDisAmnt.Text = "0";
            }
            if (txtTotalAmnt.Text.Length == 0)
            {
                this.txtTotalAmnt.Text = "0";
            }
            if (txtWithAdj.Text.Length == 0)
            {
                this.txtWithAdj.Text = "0";
            }

            dNet = Convert.ToDouble(this.txtTotalAmnt.Text) - Convert.ToDouble(this.txtDisAmnt.Text) - Convert.ToDouble(this.txtWithAdj.Text);
            this.txtNet.Text = Convert.ToString(dNet);
        }
    }

    //WITHDRAWN OR ADJUSTMENT AMOUNT
    protected void txtWithAdj_TextChanged(object sender, EventArgs e)
    {
        double dNet = 0;
        if (this.txtWithAdj.Text == "")
        {
            Response.Write("Please enter Withdrawn or Adjustment Amount."); 
        }
        else
        {
            if (txtDisAmnt.Text.Length == 0)
            {
                this.txtDisAmnt.Text = "0";
            }
            if (txtTotalAmnt.Text.Length == 0)
            {
                this.txtTotalAmnt.Text = "0";
            }
            if (txtWithAdj.Text.Length == 0)
            {
                this.txtWithAdj.Text = "0";
            }
            dNet = Convert.ToDouble(this.txtTotalAmnt.Text) - Convert.ToDouble(this.txtDisAmnt.Text) - Convert.ToDouble(this.txtWithAdj.Text);
            this.txtNet.Text = Convert.ToString(dNet);
        }
    }

    //FUNCTION FOR LOAD MRSR NO.
    protected void fnLoadMRSRNo()
    {
        SqlConnection conn = DBConnection.GetConnection();
        string sSql = "";
        Double sMRSRNo = 0;
        
        sSql = "";
        //sSql = "SELECT MAX(CAST(InvoiceNO AS INT)) AS SLNmbr FROM MRSRMaster" +
        sSql = "SELECT MAX(CAST(LEFT(MRSRCode,LEN(MRSRCode)-1) AS INT)) AS SLNmbr FROM MRSRMaster" +
            " WHERE TrType=3";
        // AND RIGHT(MRSRCode,1)<>'S'
        //AND OutSource='" + Session["sBrId"] + "'";

        SqlCommand cmd = new SqlCommand(sSql, conn);
        conn.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        try
        {
            if (dr.Read())
            {
                sMRSRNo = Convert.ToDouble(dr["SLNmbr"].ToString()) + 1;
                this.txtMRSR.Text = Convert.ToString(sMRSRNo);
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
    
    //FINALLY SAVE DATA
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (System.Convert.ToInt32(Session["Vis"]) == 0)
        {
            Response.Redirect("../Account/Login.aspx");
        }

        SqlConnection conn = DBConnection.GetConnection();
        //fnLoadMRSRNo();

        //LOAD AUTO BILL NO.
        fnLoadAutoBillNo();

        string sSql = "";

        if (Session["sBrId"] == "0")
        {
            PopupMessage("Please LogIn again.", btnSave);
            return;
        }

        //CHALLAN NUMBER       
        if (txtCHNo.Text == "")
        {
            PopupMessage("Please enter Challan #.", btnSave);
            txtCHNo.Focus();
            return;
        }

        //CHALLAN DATE VALIDATION        
        if (txtDate.Text == "")
        {
            PopupMessage("Please enter Date.", btnSave);
            txtDate.Focus();
            return;
        }

        //GRIDVIEW DATA VALIDATION
        int totalRowsCount = gvUsers.Rows.Count;
        if (totalRowsCount == 0)
        {
            PopupMessage("There is no product in list. Please add product.", btnSave);
            return;
        }
        SqlDateTime sqldatenull ;
        sqldatenull = SqlDateTime.Null;
        tDate = Convert.ToDateTime(this.txtDate.Text);
        if (this.txtIssueDate.Text.Length == 0)
        {
            tChequeDate = DateTime.Now;
            //tChequeDate = sqldatenull;
        }
        else
        {
            tChequeDate = Convert.ToDateTime(this.txtIssueDate.Text);
        }
        //----------------------------------------------------------------------
        //CHECK DUPLICATE CHALLAN NO.
        sSql = "";
        sSql = "SELECT MRSRMID FROM MRSRMaster" +
            " WHERE MRSRCode='" + this.txtCHNo.Text + "'" +
            //" WHERE MRSRCode='" + this.txtMRSR.Text + 'S' + "'" +
            " AND TrType=3";
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

        //----------------------------------------------------------------------
        //CHECK & INSERT CUSTOMER INFO
        sSql = "";
        sSql = "SELECT * FROM Customer" +
            " WHERE Mobile='" + this.txtCustContact.Text + "'";            
        SqlCommand cmdCust = new SqlCommand(sSql, conn);
        conn.Open();
        SqlDataReader drCust = cmdCust.ExecuteReader();
        try
        {            
            if (drCust.Read())
            {
            }
            else
            {
                sSql = "";
                sSql = "INSERT INTO Customer(Mobile,CustName,Address," +
                       "Email)" +
                        " Values ('" + this.txtCustContact.Text + "'," +
                        " '" + this.txtCustName.Text + "'," +                        
                        " '" + this.txtCustAdd.Text + "'," +                       
                        " '" + this.txtEmail.Text + "'" +                                          
                        " )";
                SqlCommand cmdC = new SqlCommand(sSql, conn1);
                conn1.Open();
                cmdC.ExecuteNonQuery();
                conn1.Close();
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

        double dTAmnt = 0;
        if (this.txtNetAmnt.Text == "")
        {
            dTAmnt = 0;
        }
        else
        {
            dTAmnt = Convert.ToDouble(this.txtNetAmnt.Text);
        }
        double dTPay = 0;
        if (this.txtPay.Text == "")
        {
            dTPay = 0;
        }
        else
        {
            dTPay = Convert.ToDouble(this.txtPay.Text);
        }
        double dTDue = 0;
        if (this.txtDue.Text == "")
        {
            dTDue = 0;
        }
        else
        {
            dTDue = Convert.ToDouble(this.txtDue.Text);
        }

        //SAVE DATA IN MASTER TABLE
        sSql = "";
        sSql = "INSERT INTO MRSRMaster(MRSRCode,TDate,TrType," +
               "InvoiceNo,InSource,OutSource," +
               "PayAmnt,DueAmnt,PayMode," +
               "ChequeNmbr,BankName," +
               "ChequeIssueDate,SecurityCode," +
               "Customer," +
               "UserID,EntryDate," +
                " NetSalesAmnt)" +            
            " Values ('" + this.txtCHNo.Text + "','" + tDate + "','3'," +
            " '" + this.txtCHNo.Text + "','230','" + Session["sBrId"] + "'," +
            " '" + dTPay + "','" + dTDue + "'," +            
            " '" + this.ddlPayType.Text + "'," +
            " '" + this.txtChequeNo.Text + "','" + this.txtBankName.Text + "'," +
            " '" + tChequeDate + "','" + this.txtSecurityCode.Text + "'," +
            " '" + this.txtCustContact.Text + "'," +                     
            " '" + Session["UserName"] + "', " +
            " '" + DateTime.Today + "'," +
             " '" + dTAmnt + "')";
        //" CAST(" + this.lblNetAmnt.Text + " AS Numeric)";        
        // " )";
        SqlCommand cmd = new SqlCommand(sSql, conn);
        conn.Open();
        cmd.ExecuteNonQuery();
        conn.Close();
        //lblMessage.Text = "Done";


        //RETRIVE MASTER ID         
        sSql = "";
        sSql = "SELECT MRSRMID FROM MRSRMaster" +
            " WHERE MRSRCode='" + this.txtCHNo.Text + "'" +
            //" WHERE MRSRCode='" + this.txtMRSR.Text + 'S' + "'" +
            " AND TrType=3";
        cmd = new SqlCommand(sSql, conn);
        conn.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            iMRSRID = Convert.ToInt32(dr["MRSRMID"].ToString());
            //Session["sBrId"] = Convert.ToInt16(dr["EID"].ToString());
        }
        dr.Dispose();
        dr.Close();
        conn.Close();


        //------------------------------------------------------------------------------------------
        /*
        //RETRIVE & SAVE DATA IN DETAILS TABLE        
        foreach (GridViewRow row in this.gvUsers.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                this.SaveDetails(row);
            }
        } */

        //double dCampDis = 0;
        foreach (GridViewRow g1 in this.gvUsers.Rows)
        {            
            //dCampDis=Convert.ToDouble(g1.Cells[2].Text) - Convert.ToDouble(g1.Cells[3].Text);
                        
            //GridView1.Rows[i].Cells[3].Text = Convert.ToString(Convert.ToDecimal(GridView1.Rows[i].Cells[1].Text) * Convert.ToDecimal(GridView1.Rows[i].Cells[2].Text));

            string sDisCode = "";
            if (g1.Cells[7].Text.Trim() != "&nbsp;")
            {
                sDisCode = g1.Cells[7].Text.Trim();
            }
            else
            {
                sDisCode = g1.Cells[7].Text = "";
            }

            string sDisRef = "";
            if (g1.Cells[8].Text.Trim() != "&nbsp;")
            {
                sDisRef = g1.Cells[8].Text.Trim();
            }
            else
            {
                sDisRef = g1.Cells[8].Text = "";
            }

            string sProdSL = "";
            if (g1.Cells[11].Text.Trim() != "&nbsp;")
            {
                sProdSL = g1.Cells[11].Text.Trim();
            }
            else
            {
                sProdSL = g1.Cells[11].Text = "";
            }

            string sRemarks = "";
            if (g1.Cells[12].Text.Trim() != "&nbsp;")
            {
                sRemarks = g1.Cells[12].Text.Trim();
            }
            else
            {
                sRemarks = g1.Cells[12].Text = "";
            }
                        
            string gSql = "";
            gSql = "INSERT INTO MRSRDetails(MRSRMID,ProductID,Qty," +
                 " UnitPrice,TotalAmnt,DiscountAmnt," +
                 " SLNO,ProdRemarks," +
                 " DisCode,DisRef," +
                 " WithAdjAmnt,RetPrice,NetAmnt" +
                 " )" +
                 " VALUES('" + iMRSRID + "'," +
                 " '" + g1.Cells[0].Text + "'," +
                 " '" + '-' + g1.Cells[4].Text + "'," +
                 " '" + g1.Cells[3].Text + "'," +
                 " '" + g1.Cells[5].Text + "'," +
                 " '" + g1.Cells[6].Text + "'," +
                 " '" + sProdSL + "'," +
                 " '" + sRemarks + "'," +
                 " '" + sDisCode + "'," +
                 " '" + sDisRef + "'," +
                 " '" + g1.Cells[9].Text + "'," +
                 " '" + g1.Cells[2].Text + "'," +
                 " '" + g1.Cells[10].Text + "'" +
                 " )";
            SqlCommand cmdIns = new SqlCommand(gSql, conn);
                       
            conn.Open();
            cmdIns.ExecuteNonQuery();
            conn.Close();

        }

        vDeclare.sBillNo = this.txtCHNo.Text;

        //------------------------------------------------------------------------------------------

        //lblSaveMessage.Text = "Save Data Successfully.";

        ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert",
                        "<script>alert('" + "Save Successfully." + "');</script>", false);

        //------------------------------------------------------------------------------------------
        //CLEAR ALL TEXT
        
        //txtCHNo.Text = "";
        //txtDate.Text = "";
        //txtDate.Text = DateTime.Today.ToString("dd/MM/yyyy");

        //txtCustContact.Text = "";
        //txtCustName.Text = "";
        //txtCustAdd.Text = "";

        //txtProdID.Text = "";
        //txtProdDesc.Text = "";
        //txtUP.Text = "";
        //txtCP.Text = "";
        //txtQty.Text = "";
        //txtTotalAmnt.Text = "";
        //txtDisAmnt.Text = "";
        //txtDisCode.Text = "";
        //txtDisRef.Text = "";
        //txtWithAdj.Text = "";
        //txtNet.Text = "";
        //txtSL.Text = "";
        //txtRemarks.Text = "";

        ////CLEAR GRIDVIEW
        //gvUsers.DataSource = null;
        //gvUsers.DataBind();

        //dt.Clear();

        //txtCHNo.Focus();
        //------------------------------------------------------------------------------------------

        return;

    }
    
    /*
    private void SaveDetails(GridViewRow row)
    {
        string sSql = "";
        sSql = "INSERT INTO MRSRDetails(MRSRMID,ProductID,Qty," +
             " UnitPrice,TotalAmnt,DiscountAmnt," +
             " SLNO,ProdRemarks," +
             " DisCode,DisRef," +
             " WithAdjAmnt,CampDisAmnt,NetAmnt" +
             " )" +
             " VALUES('" + iMRSRID + "', @ProductID, @Qty," +
             " @UnitPrice, @TotalAmnt, @DiscountAmnt," +
             " @SLNO, @ProdRemarks," +
             " @DisCode, @DisRef," +
             " @WithAdjAmnt, @CampDisAmnt, @NetAmnt" +
             " )";
        SqlCommand cmdIns = new SqlCommand(sSql, conn);
        
        cmdIns.Parameters.AddWithValue("@MRSRMID", iMRSRID);        
        //cmdIns.Parameters.AddWithValue("@ProductID", SqlDbType.Int).Value=row.Cells[0].Text;
        cmdIns.Parameters.AddWithValue("@ProductID", Convert.ToDouble(row.Cells[0].Text)).ToString();
        cmdIns.Parameters.AddWithValue("@Qty", Convert.ToDouble(row.Cells[4].Text)).ToString();
        cmdIns.Parameters.AddWithValue("@UnitPrice", Convert.ToDouble(row.Cells[3].Text)).ToString();
        cmdIns.Parameters.AddWithValue("@TotalAmnt", Convert.ToDouble(row.Cells[5].Text)).ToString();
        cmdIns.Parameters.AddWithValue("@DiscountAmnt", Convert.ToDouble(row.Cells[6].Text).ToString());
        cmdIns.Parameters.AddWithValue("@SLNO", Convert.ToString(row.Cells[11].Text));
        cmdIns.Parameters.AddWithValue("@ProdRemarks", Convert.ToString(row.Cells[12].Text));
        cmdIns.Parameters.AddWithValue("@DisCode", row.Cells[7].Text);
        cmdIns.Parameters.AddWithValue("@DisRef", Convert.ToString(row.Cells[8].Text));
        cmdIns.Parameters.AddWithValue("@WithAdjAmnt", Convert.ToDouble(row.Cells[9].Text).ToString());
        cmdIns.Parameters.AddWithValue("@CampDisAmnt", Convert.ToDouble(row.Cells[2].Text) - Convert.ToDouble(row.Cells[3].Text));
        cmdIns.Parameters.AddWithValue("@NetAmnt", Convert.ToDouble(row.Cells[10].Text)).ToString();
        
        conn.Open();
        cmdIns.ExecuteScalar();        
        conn.Close();
    }

     */
     
    //CLEAR ALL TEXT AND GRID
    protected void btnCancel_Click(object sender, EventArgs e)
    {

        //CLEAR ALL TEXT
        txtCHNo.Text = "";
        txtDate.Text = "";
        txtDate.Text = DateTime.Today.ToString("dd/MM/yyyy");

        txtCustContact.Text = "";
        txtCustName.Text = "";
        txtCustAdd.Text = "";

        txtProdID.Text = "";
        txtProdDesc.Text = "";
        txtUP.Text = "";
        txtCP.Text = "";
        txtQty.Text = "";
        txtTotalAmnt.Text = "";
        txtDisAmnt.Text = "";
        txtDisCode.Text = "";
        txtDisRef.Text = "";
        txtWithAdj.Text = "";
        txtNet.Text = "";
        txtSL.Text = "";
        txtRemarks.Text = "";

        //CLEAR GRIDVIEW
        gvUsers.DataSource = null;
        gvUsers.DataBind();
        
        //CLEAR DATA TABLE
        dt.Clear();

        
        fnLoadAutoBillNo();
        txtCustName.Focus();

        //vDeclare.sBillNo = "";

    }

    //LOAD CUSTOMER INFO
    protected void txtCustContact_TextChanged(object sender, EventArgs e)
    {
        SqlConnection conn = DBConnection.GetConnection();
        //CHECK & INSERT CUSTOMER INFO
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
                this.txtEmail.Text = drCust["Email"].ToString();
            }
            else
            {
                this.txtCustName.Text = "";
                this.txtCustAdd.Text = "";
                this.txtEmail.Text = "";
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


    //Grid View Footer Total
    protected void gvUsers_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CalcTotalQty(e.Row.Cells[4].Text);
            CalcTotal_TP(e.Row.Cells[5].Text);
            CalcTotal_Dis(e.Row.Cells[6].Text);
            CalcTotal_With(e.Row.Cells[9].Text);
            CalcTotal(e.Row.Cells[10].Text);

            double value2 = Convert.ToDouble(e.Row.Cells[2].Text);
            e.Row.Cells[2].Text = value2.ToString("0");

            double value3 = Convert.ToDouble(e.Row.Cells[3].Text);
            e.Row.Cells[3].Text = value3.ToString("0");

            double value4 = Convert.ToDouble(e.Row.Cells[4].Text);
            e.Row.Cells[4].Text = value4.ToString("0");

            double value5 = Convert.ToDouble(e.Row.Cells[5].Text);
            e.Row.Cells[5].Text = value5.ToString("0");

            double value6 = Convert.ToDouble(e.Row.Cells[6].Text);
            e.Row.Cells[6].Text = value6.ToString("0");

            double value9 = Convert.ToDouble(e.Row.Cells[9].Text);
            e.Row.Cells[9].Text = value9.ToString("0");

            double value10 = Convert.ToDouble(e.Row.Cells[10].Text);
            e.Row.Cells[10].Text = value10.ToString("0");
            this.lblNetAmnt.Text = value10.ToString("0");
            this.txtNetAmnt.Text = value10.ToString("0");
            this.txtPay.Text = value10.ToString("0");

            //RIGHT ALIGNMENT
            e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[9].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[10].HorizontalAlign = HorizontalAlign.Right;

        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
           
            e.Row.Cells[1].Text = "Total";
            //e.Row.Cells[10].Text = string.Format("{0:c}", runningTotal);
            e.Row.Cells[4].Text = runningTotalQty.ToString("0,0", CultureInfo.InvariantCulture);
            e.Row.Cells[5].Text = runningTotalTP.ToString("0,0",CultureInfo.InvariantCulture);
            e.Row.Cells[6].Text = runningTotalDis.ToString("0,0", CultureInfo.InvariantCulture);
            e.Row.Cells[9].Text = runningTotalWith.ToString("0,0", CultureInfo.InvariantCulture);
            e.Row.Cells[10].Text = runningTotal.ToString("0,0", CultureInfo.InvariantCulture);
            //this.lblNetAmnt.Text = runningTotal.ToString("0,0", CultureInfo.InvariantCulture);
            this.lblNetAmnt.Text = runningTotal.ToString();
            this.txtNetAmnt.Text = runningTotal.ToString();
            this.txtPay.Text = runningTotal.ToString();

            //RIGHT ALIGNMENT
            e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[9].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[10].HorizontalAlign = HorizontalAlign.Right;
        }
    }

    //CALCULATE NET AMOUNT
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

    //CALCULATE TOTAL AMOUNT
    private void CalcTotal_TP(string _price)
    {
        try
        {
            runningTotalTP += Double.Parse(_price);
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write(ex.ToString());
        }
    }

    //CALCULATE DISCOUNT AMOUNT
    private void CalcTotal_Dis(string _price)
    {
        try
        {
            runningTotalDis += Double.Parse(_price);
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write(ex.ToString());
        }
    }

    //CALCULATE WITH/Adj AMOUNT
    private void CalcTotal_With(string _price)
    {
        try
        {
            runningTotalWith += Double.Parse(_price);
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write(ex.ToString());
        }
    }


    //CALCULATE TOTAL QTY
    private void CalcTotalQty(string _qty)
    {
        try
        {
            runningTotalQty += Double.Parse(_qty);
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write(ex.ToString());
        }
    }


    protected void btnPrint_Click(object sender, EventArgs e)
    {
        if (vDeclare.sBillNo.Length == 0)
        {
        }
        else
        {
            Response.Redirect("~/Forms/Sales_Bill_Print.aspx");
        }
        
    }

    protected void ddlPayType_SelectedIndexChanged(object sender, EventArgs e)
    {
        //
        if (this.ddlPayType.SelectedValue == "CASH")
        {
            this.lblNo.Visible = false;
            this.txtChequeNo.Visible = false;
            this.lblBankName.Visible = false;
            this.txtBankName.Visible = false;
            this.lblIssueDate.Visible = false;
            this.txtIssueDate.Visible = false;
            this.lblSecurityCode.Visible = false;
            this.txtSecurityCode.Visible = false;
        }
        else if (this.ddlPayType.SelectedValue == "CHEQUE")
        {
            this.lblNo.Visible = true;
            this.lblNo.Text = "Cheque #";
            this.txtChequeNo.Visible = true;
            this.lblBankName.Visible = true;
            this.txtBankName.Visible = true;
            this.lblIssueDate.Visible = true;
            this.lblIssueDate.Text = "Cheque Date";
            this.txtIssueDate.Visible = true;
            this.lblSecurityCode.Visible = true;
            this.txtSecurityCode.Visible = true;
        }
        else if (this.ddlPayType.SelectedValue == "AMEX")
        {
            this.lblNo.Visible = true;
            this.lblNo.Text = "AMEX Card #";
            this.txtChequeNo.Visible = true;
            this.lblBankName.Visible = true;
            this.txtBankName.Visible = true;
            this.lblIssueDate.Visible = true;
            this.txtIssueDate.Visible = true;
            this.lblSecurityCode.Visible = true;
            this.txtSecurityCode.Visible = true;
        }
        else if (this.ddlPayType.SelectedValue == "VISA CARD")
        {
            this.lblNo.Visible = true;
            this.lblNo.Text = "VISA Card #";
            this.txtChequeNo.Visible = true;
            this.lblBankName.Visible = true;
            this.txtBankName.Visible = true;
            this.lblIssueDate.Visible = true;
            this.txtIssueDate.Visible = true;
            this.lblSecurityCode.Visible = true;
            this.txtSecurityCode.Visible = true;
        }
        else if (this.ddlPayType.SelectedValue == "MASTER CARD")
        {
            this.lblNo.Visible = true;
            this.lblNo.Text = "MASTER Card #";
            this.txtChequeNo.Visible = true;
            this.lblBankName.Visible = true;
            this.txtBankName.Visible = true;
            this.lblIssueDate.Visible = true;
            this.txtIssueDate.Visible = true;
            this.lblSecurityCode.Visible = true;
            this.txtSecurityCode.Visible = true;
        }
        else if (this.ddlPayType.SelectedValue == "DD")
        {
            this.lblNo.Visible = true;
            this.lblNo.Text = "DD #";
            this.txtChequeNo.Visible = true;
            this.lblBankName.Visible = true;
            this.txtBankName.Visible = true;
            this.lblIssueDate.Visible = true;
            this.lblIssueDate.Text = "DD Date";
            this.txtIssueDate.Visible = true;
            this.lblSecurityCode.Visible = true;
            this.txtSecurityCode.Visible = true;
        }
        else if (this.ddlPayType.SelectedValue == "TT")
        {
            this.lblNo.Visible = true;
            this.lblNo.Text = "TT #";
            this.txtChequeNo.Visible = true;
            this.lblBankName.Visible = true;
            this.txtBankName.Visible = true;
            this.lblIssueDate.Visible = true;
            this.lblIssueDate.Text = "TT Date";
            this.txtIssueDate.Visible = true;
            this.lblSecurityCode.Visible = true;
            this.txtSecurityCode.Visible = true;
        }
        else
        {
            this.lblNo.Visible = false;
            this.txtChequeNo.Visible = false;
            this.lblBankName.Visible = false;
            this.txtBankName.Visible = false;
            this.lblIssueDate.Visible = false;
            this.txtIssueDate.Visible = false;
            this.lblSecurityCode.Visible = false;
            this.txtSecurityCode.Visible = false;
        }
    }

    protected void txtPay_TextChanged(object sender, EventArgs e)
    {      
        if (this.lblNetAmnt.Text.Length == 0)
        {
            this.lblNetAmnt.Text = "0";
        }
                        
        double dDue = 0;
        dDue = Convert.ToDouble(this.lblNetAmnt.Text) - Convert.ToDouble(this.txtPay.Text);
        this.txtDue.Text = Convert.ToString(dDue);            
        
    }

}
