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

using System.Net.Mail;

public partial class Forms_Sales_New : System.Web.UI.Page
{
    SqlConnection conn = DBConnection.GetConnection();
    SqlConnection conn1 = DBConnection.GetConnection();
    SqlConnection _connStr = DBConnection.GetConnection();
    int iMRSRID = 0;
    int iDelTag = 0;

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

            //LOAD PRODUCT MODEL
            LoadDropDownList();

            //LOAD CITY
            LoadDropDownList_City();

            //LOAD DEALER LIST
            LoadDropDownList_Dealer();

            //LOAD CTP
            LoadDropDownList_CTP();
            ddlEntity.SelectedItem.Text= Session["eName"].ToString();
            ddlEntity.SelectedItem.Value = Session["EID"].ToString();
            
            //LoadDropDownList_Model();

            //LOAD AUTO BILL NO.
            fnLoadAutoBillNo();

            //LOAD T & C
            fnLoadTC();

            //LOAD TERMS & CONDITIONS
            this.fnClaimList();


            //txtDate.Text = String.Format("{0:t}", Now);       
            txtDate.Text = DateTime.Today.ToString("MM/dd/yyyy");

        }
        else
        {
            dt = (DataTable)ViewState["dt"];
        }
        ViewState["dt"] = dt;

                       
    }
    #endregion

    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GetModel(string prefixText)
    {
        DataTable dt = new DataTable();

        SqlConnection con = DBConnection.GetConnection();

        con.Open();
        SqlCommand cmd = new SqlCommand("Select TOP 10 * from Product where Discontinue='No' AND Model like @model+'%'", con);
        cmd.Parameters.AddWithValue("@model", prefixText);
        SqlDataAdapter adp = new SqlDataAdapter(cmd);
        adp.Fill(dt);
        List<string> ModelNames = new List<string>();
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            ModelNames.Add(dt.Rows[i][5].ToString());
        }
        return ModelNames;
    }


    //LOAD CTP IN DROPDOWN LIST
    protected void LoadDropDownList_CTP()
    {
        SqlConnection conn = DBConnection.GetConnection();

        String strQuery = "Select EID,eName from Entity ";
        strQuery = strQuery + " WHERE (ActiveDeactive = 1) AND (EntityType = 'Dealer')";
        //strQuery = strQuery + " (EntityType = 'showroom' OR  EntityType = 'zone'";
        //strQuery = strQuery + " OR  EntityType = 'Dealer')";
        strQuery = strQuery + " ORDER BY eName";
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
            ddlEntity.DataTextField = "eName";
            ddlEntity.DataValueField = "EID";
            ddlEntity.DataBind();

            //Add blank item at index 0.
            ddlEntity.Items.Insert(0, new ListItem("", "0"));
            ddlEntity.Items.Insert(1, new ListItem("CI&DD (REL)", "370"));

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

    protected void fnLoadTC()
    {
        SqlConnection con = DBConnection.GetConnection();
                
        string sSql = "";
        sSql = "SELECT * FROM tbTermsCondition";            

        SqlCommand cmd = new SqlCommand(sSql, con);
        con.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        try
        {
            if (dr.Read())
            {
                txtTC.Text = dr["TermsCondition"].ToString();
            }
            else
            {
                txtTC.Text = "";
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
            " AND TrType=3 AND OnLineSales=1";

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

    //LOAD CITY IN DROPDOWN LIST
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
            ddlCity.DataSource = cmd.ExecuteReader();
            ddlCity.DataTextField = "Dist";
            //ddlCity.DataValueField = "ProductID";
            ddlCity.DataValueField = "Dist";
            ddlCity.DataBind();

            //Add blank item at index 0.
            ddlCity.Items.Insert(0, new ListItem("", ""));
            
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

        dt.Columns.Add("BLIPAmnt");
        dt.Columns.Add("IncentiveAmnt");
        dt.Columns.Add("IncentiveType");
        dt.Columns.Add("CustShowPrice");

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
        //if (Convert.ToInt16(txtQty.Text) >= Convert.ToInt16(this.lblWPMinQty.Text))
        //{
        //    dr["CampaignPrice"] = lblWPPrice.Text;
        //}
        //else
        //{
        //    dr["CampaignPrice"] = txtCP.Text;
        //}
        dr["Qty"] = txtQty.Text;
        dr["TotalPrice"] = txtTotalAmnt.Text;        
        dr["DisAmnt"] = txtDisAmnt.Text;
        dr["DisCode"] = txtDisCode.Text;
        dr["DisRef"] = txtDisRef.Text;        
        dr["WithAdjAmnt"] = txtWithAdj.Text;        
        dr["NetAmnt"] = Convert.ToDouble(txtNet.Text);
        dr["ProductSL"] = txtSL.Text;
        dr["Remarks"] = txtRemarks.Text;

        if (this.lblWPMinQty.Text.Length == 0)
        {
            this.lblWPMinQty.Text = "0";
        }

        if (Convert.ToInt16(this.lblWPMinQty.Text) > 0)
        {
            if (Convert.ToInt16(txtQty.Text) >= Convert.ToInt16(this.lblWPMinQty.Text))
            {
                dr["BLIPAmnt"] = lblBLIPofWP.Text;
                dr["IncentiveAmnt"] = lblWPIncentive.Text;
                dr["CampaignPrice"] = lblWPPrice.Text;
            }
            else
            {
                dr["BLIPAmnt"] = lblBLIPAmnt.Text;
                dr["IncentiveAmnt"] = lblIncentiveAmnt.Text;
                dr["CampaignPrice"] = txtCP.Text;
            }
        }
        else
        {
            dr["BLIPAmnt"] = lblBLIPAmnt.Text;
            dr["IncentiveAmnt"] = lblIncentiveAmnt.Text;
        }
        dr["IncentiveType"] = lblIncentiveType.Text;
        dr["CustShowPrice"] = lblUP.Text;

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

        lblBLIPAmnt.Text = "0";
        lblIncentiveAmnt.Text = "0";
        lblIncentiveType.Text = "";
        lblUP.Text = "0";

        ddlContinents.SelectedItem.Text="";
        txtCode.Text = "";
        txtProdDesc.Text = "";
        txtProdID.Text = "0";
        txtModel.Text = "";
        txtModel.Focus();

    }

    //ADD DATA IN GRIDVIEW
    protected void btnAdd_Click(object sender, EventArgs e)
    {        
        //FUNCTION FOR ADD ROW
        //try
        //{
            AddRows();
            gvUsers.DataSource = dt;
            gvUsers.DataBind();
        //}
        //catch (InvalidCastException err)
        //{
        //    throw (err);
        //}
        
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
        
        //sSql = "SELECT ProductID,ProdName,UnitPrice,Model,Code,";
        //sSql = sSql + " IncentiveType,BLIPAmnt,IncentiveAmnt,GetIncentive,WPPrice,BLIPofWP,WPIncentive,ISNULL(WPMinQty,0) AS WPMinQty";
        //sSql = sSql + " FROM Product";

        sSql = "SELECT ProductID,ProdName,UnitPrice,Model,Code,";
        sSql = sSql + " IncentiveType,BLIPAmnt,IncentiveAmnt,";
        sSql = sSql + " GetIncentive, ISNULL(WPPrice,0) AS WPPrice, ISNULL(BLIPofWP,0) AS BLIPofWP,";
        sSql = sSql + " ISNULL(WPIncentive,0) AS WPIncentive, ISNULL(WPMinQty,0) AS WPMinQty";
        sSql = sSql + " FROM Product";

        sSql = sSql + " WHERE Model='" + this.ddlContinents.SelectedItem.Text + "'";
        SqlCommand cmd = new SqlCommand(sSql, conn);
        conn.Open(); 
        SqlDataReader dr = cmd.ExecuteReader();
            
        try
        {                   
            if (dr.Read())
            {
                this.txtCode.Text = dr["Code"].ToString();
                this.txtProdID.Text = dr["ProductID"].ToString();
                this.txtProdDesc.Text = dr["ProdName"].ToString();
                UP = Convert.ToDouble(dr["UnitPrice"].ToString());
                this.txtUP.Text = Convert.ToString(UP);

                this.lblIncentiveType.Text = dr["IncentiveType"].ToString();
                this.lblBLIPAmnt.Text = dr["BLIPAmnt"].ToString();
                this.lblIncentiveAmnt.Text = dr["IncentiveAmnt"].ToString();
                this.lblGetIncentive.Text = dr["GetIncentive"].ToString();
                this.lblWPPrice.Text = dr["WPPrice"].ToString();
                this.lblBLIPofWP.Text = dr["BLIPofWP"].ToString();
                this.lblWPIncentive.Text = dr["WPIncentive"].ToString();
                this.lblWPMinQty.Text = dr["WPMinQty"].ToString();
            }
            else
            {
                UP = 0;
                this.txtUP.Text = Convert.ToString(UP);
                this.txtCode.Text = "";
                this.txtProdID.Text = "";
                this.txtProdDesc.Text = "";

                this.lblIncentiveType.Text = "";
                this.lblBLIPAmnt.Text = "0";
                this.lblIncentiveAmnt.Text = "0";
                this.lblGetIncentive.Text = "0";
                this.lblWPPrice.Text = "0";
                this.lblBLIPofWP.Text = "0";
                this.lblWPIncentive.Text = "0";
                this.lblWPMinQty.Text = "0";

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

        //-----------------------------------------------------------
        //lblUP.Text = txtCP.Text;
        //if (lblGetIncentive.Text == "True")
        //{
        //    if (lblIncentiveType.Text == "Instant")
        //    {
        //        this.txtCP.Text = lblBLIPAmnt.Text;
        //    }
        //    else
        //    {
        //        this.txtCP.Text = Convert.ToString(CampPrice);
        //    }
        //}
        //-----------------------------------------------------------

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

            if (Convert.ToInt16(this.lblWPMinQty.Text) > 0)
            {
                if (Convert.ToInt16(txtQty.Text) >= Convert.ToInt16(this.lblWPMinQty.Text))
                {
                    txtCP.Text = lblWPPrice.Text;
                }
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
        SqlConnection connSMS = DBConnectionSMS.GetConnection();

        //fnLoadMRSRNo();

        

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

        if (this.txtCustContact.Text.Length == 0)
        {
            PopupMessage("Please enter customer contact number.", btnSave);
            txtCustContact.Focus();
            return;
        }


        //DUE AMNT       
        if (txtDue.Text !="0")
        {
            PopupMessage("Please enter full payment ...", btnSave);
            txtPay.Focus();
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


        //**********************************************************************
        //LOAD AUTO BILL NO.
        fnLoadAutoBillNo();

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

        string sProfession = "";
        if (optProfession.SelectedItem.Text == "Business")
        {
            sProfession = "Business";
        }
        else if (optProfession.SelectedItem.Text == "Service")
        {
            sProfession = "Service";
        }
        else
        {
            sProfession = "Others";
        }

        string sSex = "";
        if (optSex.SelectedItem.Text == "Male")
        {
            sSex = "Male";
        }
        else if (optSex.SelectedItem.Text == "Female")
        {
            sSex = "Female";
        }        
        else
        {
            sSex = "N/A";
        }

        
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
                sSql = "";
                sSql = "UPDATE Customer SET";
                sSql = sSql + " CustName='" + this.txtCustName.Text.Replace("'", "''") + "',";
                sSql = sSql + " Address='" + this.txtCustAdd.Text.Replace("'", "''") + "',";
                sSql = sSql + " City='" + this.ddlCity.SelectedItem.Text + "',";
                sSql = sSql + " Email='" + this.txtEmail.Text + "',";
                sSql = sSql + " Profession='" + sProfession + "',";
                sSql = sSql + " Org='" + this.txtOrg.Text.Replace("'", "''") + "',";
                sSql = sSql + " Desg='" + this.txtDesg.Text.Replace("'", "''") + "',";
                sSql = sSql + " CustSex='" + sSex + "',";
                sSql = sSql + " DOBT='" + txtDOB.Text + "',";
                sSql = sSql + " eID='" + Session["EID"] + "',";
                sSql = sSql + " CustType='Regular',";
                sSql = sSql + " IdentityType='N/A', IdentityNo='N/A'";
                sSql = sSql + " WHERE Mobile='" + this.txtCustContact.Text + "'";
                SqlCommand cmdU = new SqlCommand(sSql, conn1);
                conn1.Open();
                cmdU.ExecuteNonQuery();
                conn1.Close();

            }
            else
            {
                sSql = "";
                sSql = "INSERT INTO Customer(Mobile,CustName,Address,City,eID,CustType," +
                       "Email,Profession, Org, Desg,CustSex,IdentityType,IdentityNo,DOBT)" +
                        " Values ('" + this.txtCustContact.Text + "','" + this.txtCustName.Text.Replace("'", "''") + "'," +
                        " '" + this.txtCustAdd.Text.Replace("'", "''") + "','" + this.ddlCity.SelectedItem.Text + "'," +
                        " '" + Session["EID"] + "','Regular'," +
                        " '" + this.txtEmail.Text + "','" + sProfession + "'," +
                        " '" + this.txtOrg.Text.Replace("'", "''") + "','" + this.txtDesg.Text.Replace("'", "''") + "'," +
                        " '" + sSex + "','N/A'," +
                        " 'N/A', '" + txtDOB.Text + "'" +                                          
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

        if (this.txtCardAmnt1.Text.Length == 0)
        {
            this.txtCardAmnt1.Text = "0";
        }

        if (this.txtCardAmnt2.Text.Length == 0)
        {
            this.txtCardAmnt2.Text = "0";
        }

        if (this.txtCash.Text.Length == 0)
        {
            this.txtCash.Text = "0";
        }

        if (this.txtDue.Text.Length == 0)
        {
            this.txtDue.Text = "0";
        }


        if (ddlEntity.SelectedItem.Value == Session["sBrId"].ToString())
        {
            iDelTag = 1;
        }
        else
        {
            iDelTag = 2;
        }

        //-----------------------------------------------------------------
        // T & C
        txtTC.Text = "";
        int iCount = 1;
        string k = "";
        for (int i = 0; i < chkTC.Items.Count; i++)
        {
            if (chkTC.Items[i].Selected)
            {
                k = k + iCount + ". " + chkTC.Items[i].Text + "\n";
                iCount = iCount + 1;
            }
        }
        txtTC.Text = k;
        //-----------------------------------------------------------------


        DateTime aDate = DateTime.Now;

        try
        {
            //LOAD AUTO BILL NO.
            fnLoadAutoBillNo();

            //SAVE DATA IN MASTER TABLE
            sSql = "";
            sSql = "INSERT INTO MRSRMaster(MRSRCode,TDate,TrType," +
                   "InvoiceNo,InSource,OutSource," +
                   "PayAmnt,DueAmnt,PayMode," +
                   "Customer,UserID,EntryDate," +
                   "NetSalesAmnt,TermsCondition," +
                   "CashAmnt,CardAmnt1,CardAmnt2," +
                   "CardNo1,CardNo2,CardType1,CardType2," +
                   "Bank1,Bank2,SecurityCode,SecurityCode2," +
                   "AppovalCode1,AppovalCode2,OnLineSales," +
                   "Authorby,Issby,DeliveryFrom,Remarks,Tag,RefCHNo,POCode" +
                   " )" +
                " Values ('" + this.txtCHNo.Text + "','" + tDate + "','3'," +
                " '" + this.txtCHNo.Text + "','230','" + Session["sBrId"] + "'," +
                " '" + dTPay + "','" + dTDue + "','" + this.ddlPayType.Text + "'," +
                " '" + this.txtCustContact.Text + "', '" + Session["UserName"] + "', '" + DateTime.Now + "'," +
                " '" + dTAmnt + "','" + this.txtTC.Text.Replace("'", "''") + "'," +
                " '" + this.txtCash.Text + "','" + this.txtCardAmnt1.Text + "','" + this.txtCardAmnt2.Text + "'," +
                " '" + this.txtChequeNo.Text + "','" + this.txtChequeNo2.Text + "','" + this.ddlCardType1.SelectedItem.Text + "','" + this.ddlCardType2.SelectedItem.Text + "'," +
                " '" + this.txtBankName.Text + "','" + this.txtBankName2.Text + "','" + this.txtSecurityCode.Text + "','" + this.txtSecurityCode2.Text + "'," +
                " '" + this.txtApprovalCode1.Text + "','" + this.txtApprovalCode2.Text + "',1," +
                " '" + this.txtRefBy.Text + "','" + this.txtJobID.Text + "'," +
                " '" + this.ddlEntity.SelectedItem.Value + "','" + this.txtNote.Text.Replace("'", "''") + "'," +
                " '" + iDelTag + "','" + txtRefChNo.Text + "','" + txtOrderNo.Text + "'" +
            " )";
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

                string sBLAMNT = "";
                if (g1.Cells[13].Text.Trim() != "&nbsp;")
                {
                    sBLAMNT = g1.Cells[13].Text.Trim();
                }
                else
                {
                    sBLAMNT = g1.Cells[13].Text = "0";
                }

                string sIncType = "";
                if (g1.Cells[15].Text.Trim() != "&nbsp;")
                {
                    sIncType = g1.Cells[15].Text.Trim();
                }
                else
                {
                    sIncType = g1.Cells[15].Text = "";
                }

                //string gSql = "";
                //gSql = "INSERT INTO MRSRDetails(MRSRMID,ProductID,Qty," +
                //     " UnitPrice,TotalAmnt,DiscountAmnt," +
                //     " SLNO,ProdRemarks," +
                //     " DisCode,DisRef," +
                //     " WithAdjAmnt,RetPrice,NetAmnt" +
                //     " )" +
                //     " VALUES('" + iMRSRID + "'," +
                //     " '" + g1.Cells[0].Text + "'," +
                //     " '" + '-' + g1.Cells[4].Text + "'," +
                //     " '" + g1.Cells[3].Text + "'," +
                //     " '" + g1.Cells[5].Text + "'," +
                //     " '" + g1.Cells[6].Text + "'," +
                //     " '" + sProdSL + "'," +
                //     " '" + sRemarks + "'," +
                //     " '" + sDisCode + "'," +
                //     " '" + sDisRef + "'," +
                //     " '" + g1.Cells[9].Text + "'," +
                //     " '" + g1.Cells[2].Text + "'," +
                //     " '" + g1.Cells[10].Text + "'" +
                //     " )";

                double dIncAmnt = 0;
                double dTAmnt1 = Convert.ToDouble(g1.Cells[3].Text) * Convert.ToDouble(g1.Cells[4].Text);
                double dTBLAmnt1 = Convert.ToDouble(sBLAMNT) * Convert.ToDouble(g1.Cells[4].Text);
                if (dTBLAmnt1 > 0)
                {
                    dIncAmnt = dTAmnt1 - dTBLAmnt1;
                }

                string gSql = "";
                gSql = "INSERT INTO MRSRDetails(MRSRMID,ProductID,Qty," +
                     " MRP,UnitPrice,TotalAmnt,DiscountAmnt," +
                     " SLNO,ProdRemarks,DisCode,DisRef," +
                     " WithAdjAmnt,RetPrice,NetAmnt," +
                     " BLIPAmnt,IncentiveAmnt,IncentiveType," +
                     " CustShowPrice)" +
                     " VALUES('" + iMRSRID + "','" + g1.Cells[0].Text + "','" + '-' + g1.Cells[4].Text + "'," +
                     " '" + g1.Cells[2].Text + "','" + g1.Cells[3].Text + "','" + g1.Cells[5].Text + "','" + g1.Cells[6].Text + "'," +
                     " '" + sProdSL + "','" + sRemarks + "','" + sDisCode + "','" + sDisRef + "'," +
                     " '" + g1.Cells[9].Text + "','" + g1.Cells[2].Text + "','" + g1.Cells[10].Text + "'," +
                     " '" + g1.Cells[13].Text + "','" + dIncAmnt + "','" + sIncType + "'," +
                     " '" + g1.Cells[16].Text + "')";
                SqlCommand cmdIns = new SqlCommand(gSql, conn);

                conn.Open();
                cmdIns.ExecuteNonQuery();
                conn.Close();

            }
        }
        catch
        {
            //
        }

        //######################################################################################
        //SAVE DATA IN HISTORY MASTER TABLE
        try
        {
            sSql = "";
            sSql = "INSERT INTO HistoryMaster(MRSRMID,MRSRCode,TDate,TrType," +
                   "InvoiceNo,InSource,OutSource," +
                   "PayAmnt,DueAmnt,PayMode," +
                   "Customer,UserID,EntryDate," +
                   "NetSalesAmnt,TermsCondition," +
                   "CashAmnt,CardAmnt1,CardAmnt2," +
                   "CardNo1,CardNo2,CardType1,CardType2," +
                   "Bank1,Bank2,SecurityCode,SecurityCode2," +
                   "AppovalCode1,AppovalCode2,OnLineSales," +
                   "Authorby,Issby,DeliveryFrom,Remarks,Tag,RefCHNo,POCode" +
                   " )" +
                " Values ('" + iMRSRID + "','" + this.txtCHNo.Text + "','" + tDate + "','3'," +
                " '" + this.txtCHNo.Text + "','230','" + Session["sBrId"] + "'," +
                " '" + dTPay + "','" + dTDue + "','" + this.ddlPayType.Text + "'," +
                " '" + this.txtCustContact.Text + "', '" + Session["UserName"] + "', '" + aDate.ToString("MM/dd/yyyy hh:mm tt") + "'," +
                " '" + dTAmnt + "','" + this.txtTC.Text.Replace("'", "''") + "'," +
                " '" + this.txtCash.Text + "','" + this.txtCardAmnt1.Text + "','" + this.txtCardAmnt2.Text + "'," +
                " '" + this.txtChequeNo.Text + "','" + this.txtChequeNo2.Text + "','" + this.ddlCardType1.SelectedItem.Text + "','" + this.ddlCardType2.SelectedItem.Text + "'," +
                " '" + this.txtBankName.Text + "','" + this.txtBankName2.Text + "','" + this.txtSecurityCode.Text + "','" + this.txtSecurityCode2.Text + "'," +
                " '" + this.txtApprovalCode1.Text + "','" + this.txtApprovalCode2.Text + "',1," +
                " '" + this.txtRefBy.Text + "','" + this.txtJobID.Text + "'," +
                " '" + this.ddlEntity.SelectedItem.Value + "','" + this.txtNote.Text.Replace("'", "''") + "'," +
                " '" + iDelTag + "','" + txtRefChNo.Text + "','" + txtOrderNo.Text + "'" +
            " )";
            //" CAST(" + this.lblNetAmnt.Text + " AS Numeric)";        
            // " )";
            SqlCommand cmdH = new SqlCommand(sSql, conn);
            conn.Open();
            cmdH.ExecuteNonQuery();
            conn.Close();
            //lblMessage.Text = "Done";


            //RETRIVE HISTORY MASTER ID  
            Int32 hMRSRID = 0;
            sSql = "";
            sSql = "SELECT  TOP (1) HisMID FROM HistoryMaster";
            sSql = sSql + " WHERE MRSRCode='" + this.txtCHNo.Text + "'";
            //sSql = sSql + " AND EntryDate='" + DateTime.Today + "'";
            //" WHERE MRSRCode='" + this.txtMRSR.Text + 'S' + "'" +
            sSql = sSql + " AND TrType=3";
            sSql = sSql + " GROUP BY HisMID, TrType";
            sSql = sSql + " ORDER BY HisMID DESC";
            SqlCommand cmd = new SqlCommand(sSql, conn);
            conn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                hMRSRID = Convert.ToInt32(dr["HisMID"].ToString());
                //Session["sBrId"] = Convert.ToInt16(dr["EID"].ToString());
            }
            dr.Dispose();
            dr.Close();
            conn.Close();


            //double dCampDis = 0;
            foreach (GridViewRow g1 in this.gvUsers.Rows)
            {
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

                string sIncType = "";
                if (g1.Cells[15].Text.Trim() != "&nbsp;")
                {
                    sIncType = g1.Cells[15].Text.Trim();
                }
                else
                {
                    sIncType = g1.Cells[15].Text = "";
                }

                double dIncAmnt = 0;
                double dTAmnt1 = Convert.ToDouble(g1.Cells[3].Text) * Convert.ToDouble(g1.Cells[4].Text);
                double dTBLAmnt1 = Convert.ToDouble(g1.Cells[13].Text) * Convert.ToDouble(g1.Cells[4].Text);
                if (dTBLAmnt1 > 0)
                {
                    dIncAmnt = dTAmnt1 - dTBLAmnt1;
                }

                string gSql = "";
                gSql = "INSERT INTO HistoryDetails(HisMID,MRSRMID,ProductID,Qty," +
                     " MRP,UnitPrice,TotalAmnt,DiscountAmnt," +
                     " SLNO,ProdRemarks,DisCode,DisRef," +
                     " WithAdjAmnt,RetPrice,NetAmnt," +
                     " BLIPAmnt,IncentiveAmnt,IncentiveType," +
                     " CustShowPrice)" +
                     " VALUES('" + hMRSRID + "','" + iMRSRID + "','" + g1.Cells[0].Text + "','" + '-' + g1.Cells[4].Text + "'," +
                     " '" + g1.Cells[2].Text + "','" + g1.Cells[3].Text + "','" + g1.Cells[5].Text + "','" + g1.Cells[6].Text + "'," +
                     " '" + sProdSL + "','" + sRemarks + "','" + sDisCode + "','" + sDisRef + "'," +
                     " '" + g1.Cells[9].Text + "','" + g1.Cells[2].Text + "','" + g1.Cells[10].Text + "'," +
                     " '" + g1.Cells[13].Text + "','" + dIncAmnt + "','" + sIncType + "'," +
                     " '" + g1.Cells[16].Text + "')";

                SqlCommand cmdInsH = new SqlCommand(gSql, conn);

                conn.Open();
                cmdInsH.ExecuteNonQuery();
                conn.Close();

            }

        }
        catch
        {
            //
        }

        //----------------------------------------------------------------
        //SAVE TERMS & CONDITIONS
        try
        {
            string strcbl1 = string.Empty;
            for (int i = 0; i < chkTC.Items.Count; i++)
            {
                //if (chkTC.Items[i].Selected == true)
                if (chkTC.Items[i].Selected)
                {
                    strcbl1 = strcbl1 + chkTC.Items[i].Text.ToString() + ",";

                    sSql = "";
                    sSql = "INSERT INTO tbTC_Customer(MRSRMID,InvoiceNo,TCText,tcAID)" +
                            " Values ('" + iMRSRID + "','" + this.txtCHNo.Text + "','" + chkTC.Items[i].Text + "','" + chkTC.Items[i].Value + "'" +
                            " )";
                    SqlCommand cmd2 = new SqlCommand(sSql, conn);
                    conn.Open();
                    cmd2.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }
        catch
        {
            //
        }
        //----------------------------------------------------------------


        //***************************************************************************************
        // FOR SMS
        /*
        if (txtCustContact.Text != "")
        {
            string smsText = "";
            smsText = "Dear Customer,\n";
            smsText = smsText + "Your Bill Details:\n";
            smsText = smsText + "Bill # " + this.txtCHNo.Text + ".\n";
            smsText = smsText + "Date: " + this.txtDate.Text + ".\n";
            smsText = smsText + "Bill Amount : " + this.txtNetAmnt.Text + ".\n";
            smsText = smsText + "Thank you for shopping with us.\n";
            smsText = smsText + "REL";

            sSql = "";
            sSql = "INSERT INTO tbSMS(ContactNo,SMSText,UserID,EntryDate,SMSSource)" +
                    " Values ('" + this.txtCustContact.Text + "','" + smsText + "'," +
                    " '" + Session["UserID"] + "','" + DateTime.Today + "'," +
                    " 'DMS'" +
                    " )";
            SqlCommand cmdSMS = new SqlCommand(sSql, connSMS);
            connSMS.Open();
            cmdSMS.ExecuteNonQuery();
            connSMS.Close();
        }
         */
        //***************************************************************************************

        /*
        //SMS FOR INCENTIVE
        try
        {
            fnSMSforIncentive();
        }
        catch (Exception ex)
        {
            //lblmsg.Text = ex.Message;
        }
        finally
        {
            //conn.Close();
        }
        */

        //------------------------------------------------------------------------------------------
        //SEND MAIL TO CUSTOMER
        if (txtEmail.Text.Length > 0)
        {
            try
            {
                fnSendMail_Invoice();
            }
            catch
            {
                //
            }
        }

        //------------------------------------------------------------------------------------------
        // UPDATE ONLINE STORE  --- dStatus=3 for Delivered
        if (txtOrderNo.Text.Length > 0)
        {
            try
            {
                SqlConnection connRos = DBConnection_ROS.GetConnection();
                sSql = "";
                sSql = "UPDATE tbCustomerDelivery SET dStatus=3 WHERE DelNo='" + txtOrderNo.Text + "'";
                SqlCommand cmdRR = new SqlCommand(sSql, connRos);
                connRos.Open();
                cmdRR.ExecuteNonQuery();
                connRos.Close();
            }
            catch
            {
                //
            }
        }
        //------------------------------------------------------------------------------------------


        //LOAD AUTO BILL NO.
        //fnLoadAutoBillNo();

        //vDeclare.sBillNo = this.txtCHNo.Text;
        Session["sBillNo"] = this.txtCHNo.Text;
        Response.Redirect("Sales_Bill_Print.aspx");

        //LOAD AUTO BILL NUMBER
        //fnLoadAutoBillNo();

        //------------------------------------------------------------------------------------------

        //lblSaveMessage.Text = "Save Data Successfully.";

        //ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert",
                        //"<script>alert('" + "Save Successfully." + "');</script>", false);
                
        return;

    }


    //FUNCTION FOR SEND MAIL
    private void fnSendMail_Invoice()
    {
        SqlConnection conn = DBConnection.GetConnection();
        SqlConnection conn1 = DBConnection.GetConnection();

        SqlCommand dataCommand = new SqlCommand();
        dataCommand.Connection = conn;
        SqlCommand dataCommand1 = new SqlCommand();
        dataCommand1.Connection = conn1;

        dataCommand.CommandType = CommandType.Text;
        dataCommand1.CommandType = CommandType.Text;

        int iSl = 1;
        //string tDate = DateTime.Today.ToString("dd/MM/yyyy");
        //string tDate = DateTime.Today.ToString();
        string tDate = string.Format("{0:D}", DateTime.Today);
        string tTime = DateTime.Now.ToString("T");

        //LOAD CTP INFORMATION
        string sSql = "";
        sSql = "SELECT EID, eName, EDesc, EntityType, EntityCode, ContactPerson,";
        sSql = sSql + " Desg, PhoneNo, EmailAdd, ContactNo";
        sSql = sSql + " FROM dbo.Entity";
        sSql = sSql + " WHERE EID='" + Session["EID"].ToString() + "'";
        SqlCommand cmdC = new SqlCommand(sSql, conn);
        conn.Open();
        SqlDataReader drC = cmdC.ExecuteReader();
        if (drC.Read())
        {
            lblCTPName.Text = drC["eName"].ToString();
            lblCTPAdd.Text = drC["EDesc"].ToString();
            lblCTPEmail.Text = drC["EmailAdd"].ToString();
            lblCTPContact.Text = drC["PhoneNo"].ToString();
            if (drC["PhoneNo"].ToString().Length == 0)
            {
                lblCTPContact.Text = drC["ContactNo"].ToString();
            }
        }
        conn.Close();


        //****************************************************************************************
        //-----------------------------------------------------------------------------------------------------
        // Mail to Customer------------------------------------------------------------------------------------

        MailMessage mM2 = new MailMessage();
        //mM2.From = new MailAddress(txtEmail.Text);        

        //mM2.From = new MailAddress("rangs.eshop@gmail.com");
        mM2.From = new MailAddress("dms@rangs.com.bd");
        //PW:Exampass@567

        //mM2.To.Add(new MailAddress(Session["sEmail"].ToString()));
        mM2.To.Add(new MailAddress(txtEmail.Text));
        mM2.CC.Add(new MailAddress(lblCTPEmail.Text));
        mM2.Bcc.Add(new MailAddress("zunayedqu10@gmail.com"));

        mM2.Subject = "Sony-Rangs Invoice No." + txtCHNo.Text + " ";
        //mM2.Body = "<h1>Order Details</h1>";
        mM2.Body = "<p>Dear Valued Customer,</p>";
        mM2.Body = mM2.Body + "<p>Thank you for shopping with us.<br/>";
        //mM2.Body = mM2.Body + "We really appreciate it and we are taking necessary steps to process this order.";
        //mM2.Body = mM2.Body + "as soon as possible. You will be updated about next step immediate.";
        mM2.Body = mM2.Body + "</p>";


        //mM2.Body = mM2.Body + "<p>";
        //mM2.Body = mM2.Body + "Your order will be on its way very shortly, in the meantime please check below ";
        //mM2.Body = mM2.Body + "to ensure we have the correct details for your order.";
        //mM2.Body = mM2.Body + "</p>";
        mM2.Body = mM2.Body + "<p>";
        mM2.Body = mM2.Body + "<b><u>Sales From</u><br/> " + lblCTPName.Text + "</b><br/>";
        mM2.Body = mM2.Body + "" + lblCTPAdd.Text + "<br/>";
        mM2.Body = mM2.Body + "Phone: " + lblCTPContact.Text + "";
        mM2.Body = mM2.Body + "</p>";

        //mM2.Body = mM2.Body + "<br/>";
        mM2.Body = mM2.Body + "<p><b>Invoice No: " + txtCHNo.Text + "</b><br/>";
        mM2.Body = mM2.Body + "Invoice Date: " + txtDate.Text + "</p>";

        //mM2.Body = mM2.Body + "<br/>";
        mM2.Body = mM2.Body + "<p><u>Customer Details:</u><br/> Name: " + txtCustName.Text + "<br/>";
        mM2.Body = mM2.Body + "Contact # " + txtCustContact.Text + "<br/>";
        mM2.Body = mM2.Body + "Email: " + txtEmail.Text + "<br/>";
        mM2.Body = mM2.Body + "Address: " + txtCustAdd.Text + "</p>";

        //if (Session["DelType1"] != "Collection")
        //{
        //    mM2.Body = mM2.Body + "<u><b>Delivery Address:</b></u><br/>";
        //    mM2.Body = mM2.Body + "" + Session["CustAdd1"].ToString() + "<br/>";
        //    mM2.Body = mM2.Body + "" + Session["CustAdd2"].ToString() + "<br/>";
        //    mM2.Body = mM2.Body + "" + Session["CustPostal"].ToString() + "</p>";
        //}

        //if (Session["DelAdd"].ToString() != null)
        //{
        //    mM2.Body = mM2.Body + "<p>Shipping Address: " + Session["DelAdd"].ToString() + "</p>";
        //}

        //mM2.Body = mM2.Body + "<br/>";
        mM2.Body = mM2.Body + "<p><b>Product Details:</b> </p>";

        //------- Start Table ---------------
        mM2.Body = mM2.Body + "<table style='font-family: arial, sans-serif; border-collapse: collapse; width: 100%;'>";

        mM2.Body = mM2.Body + "<tr>";
        mM2.Body = mM2.Body + "<th width='5%' style='border: 1px solid orange; text-align: left; padding: 8px;'>SL#</th>";
        mM2.Body = mM2.Body + "<th style='border: 1px solid orange; text-align: left; padding: 8px;'>Item Name</th>";
        mM2.Body = mM2.Body + "<th width='10%' style='border: 1px solid orange; text-align: left; padding: 8px;'>Qty</th>";
        mM2.Body = mM2.Body + "<th width='15%' style='border: 1px solid orange; text-align: left; padding: 8px;'>Amount (&#2547;)</th>";
        mM2.Body = mM2.Body + "<th width='15%' style='border: 1px solid orange; text-align: left; padding: 8px;'>Product Serial</th>";
        mM2.Body = mM2.Body + "<th width='15%' style='border: 1px solid orange; text-align: left; padding: 8px;'>Remarks</th>";
        mM2.Body = mM2.Body + "</tr>";

        //-----------------------------------------------------------------------------
        sSql = "";
        //sSql = "SELECT dbo.tbCustomerDelDetails.DelDID, dbo.tbCustomerDelDetails.DelID, dbo.tbCustomerDelDetails.ProductID,";
        //sSql = sSql + " dbo.tbProduct.title, dbo.tbProduct.titleDesc, dbo.tbCustomerDelDetails.SalePrice,";
        //sSql = sSql + " dbo.tbCustomerDelDetails.tQty, dbo.tbCustomerDelDetails.tAmnt";
        //sSql = sSql + " FROM dbo.tbCustomerDelDetails INNER JOIN";
        //sSql = sSql + " dbo.tbProduct ON dbo.tbCustomerDelDetails.ProductID = dbo.tbProduct.ProductID";
        //sSql = sSql + " WHERE (dbo.tbCustomerDelDetails.DelID = '" + this.lblID.Text + "')";

        sSql = "SELECT dbo.MRSRMaster.MRSRMID, dbo.MRSRDetails.ProductID, dbo.Product.Code, dbo.Product.Model,";
        sSql = sSql + " dbo.Product.ProdName, ABS(dbo.MRSRDetails.Qty) AS tQty, dbo.MRSRDetails.UnitPrice,";
        sSql = sSql + " dbo.MRSRDetails.TotalAmnt, dbo.MRSRDetails.DiscountAmnt, dbo.MRSRDetails.WithAdjAmnt, ";
        sSql = sSql + " dbo.MRSRDetails.NetAmnt, CONVERT(varchar, CAST(dbo.MRSRDetails.NetAmnt AS money), 1) AS tNetAmnt, dbo.MRSRDetails.ProdRemarks, dbo.MRSRMaster.MRSRCode, dbo.MRSRDetails.SLNO";
        sSql = sSql + " FROM  dbo.MRSRMaster INNER JOIN";
        sSql = sSql + " dbo.MRSRDetails ON dbo.MRSRMaster.MRSRMID = dbo.MRSRDetails.MRSRMID INNER JOIN";
        sSql = sSql + " dbo.Product ON dbo.MRSRDetails.ProductID = dbo.Product.ProductID";
        sSql = sSql + " WHERE (dbo.MRSRMaster.MRSRCode = '" + this.txtCHNo.Text + "')";

        SqlCommand cmd1 = new SqlCommand(sSql, conn1);
        dataCommand1.CommandText = sSql;

        iSl = 1;
        conn1.Open();
        SqlDataReader dr = dataCommand1.ExecuteReader();
        while (dr.Read())
        {
            mM2.Body = mM2.Body + "<tr>";
            mM2.Body = mM2.Body + "<td style='border: 1px solid orange; text-align: left; padding: 8px;'>" + iSl + ". </td>";
            //mM2.Body = mM2.Body + "<td style='border: 1px solid orange; text-align: left; padding: 8px;'>" + dr["ProductName"].ToString() + "</td>";
            mM2.Body = mM2.Body + "<td style='border: 1px solid orange; text-align: left; padding: 8px;'>" + dr["Model"].ToString() + " (" + dr["ProdName"].ToString() + ")</td>";
            mM2.Body = mM2.Body + "<td align='Right' style='border: 1px solid orange; text-align: left; padding: 8px;'>" + dr["tQty"].ToString() + "</td>";
            mM2.Body = mM2.Body + "<td align='Right' style='border: 1px solid orange; text-align: left; padding: 8px;'>" + dr["tNetAmnt"].ToString() + "</td>";
            mM2.Body = mM2.Body + "<td align='Right' style='border: 1px solid orange; text-align: left; padding: 8px;'>" + dr["SLNO"].ToString() + "</td>";
            mM2.Body = mM2.Body + "<td align='Right' style='border: 1px solid orange; text-align: left; padding: 8px;'>" + dr["ProdRemarks"].ToString() + "</td>";
            mM2.Body = mM2.Body + "</tr>";
            iSl = iSl + 1;
        }
        //dataCommand1.ExecuteNonQuery();
        conn1.Close();
        //-------------------------------------------------------------------------------------

        //mM2.Body = mM2.Body + "<tr><td>Booking Date/time:</td><td>" + string.Format("{0:D}", tfDate) + " at " + ddlTime.SelectedItem.Text + "</td></tr>";
        //mM2.Body = mM2.Body + "<tr><td>Secial Notes/Comments:</td><td>" + txtNote.Text + "</td></tr>";
        mM2.Body = mM2.Body + "</table>";

        mM2.Body = mM2.Body + "<p>";
        //mM2.Body = mM2.Body + "Sub Total: &#2547; " + lblTotalAmnt.Text + "<br/>";
        //mM2.Body = mM2.Body + "Shipping Fee: &#2547; " + lblShipping.Text + "<br/>";
        //mM2.Body = mM2.Body + "Tax Amount: &#2547; " + lblTax.Text + "<br/>";
        mM2.Body = mM2.Body + "<b>Total Amount: &#2547; " + txtNetAmnt.Text + "</b><br/>";

        mM2.Body = mM2.Body + "<b>Payment Details: </b><br/>";
        if (txtCash.Text.Length > 0)
        {
            mM2.Body = mM2.Body + "Cash: " + txtCash.Text + "<br/>";
        }
        if (txtCardAmnt1.Text.Length > 0)
        {
            if (txtCardAmnt1.Text != "0")
            {
                mM2.Body = mM2.Body + "Card1: " + txtCardAmnt1.Text + "<br/>";
            }
        }
        if (txtCardAmnt2.Text.Length > 0)
        {
            if (txtCardAmnt2.Text != "0")
            {
                mM2.Body = mM2.Body + "Card2 " + txtCardAmnt2.Text + "<br/>";
            }
        }


        //mM2.Body = mM2.Body + "Payment Type: " + lblPaymentMethod.Text + "<br/>";

        if (txtOrderNo.Text.Length > 0)
        {
            mM2.Body = mM2.Body + "Online Order No.: " + txtOrderNo.Text + "<br/>";
        }

        //mM2.Body = mM2.Body + "Delivery Type: " + lblDelType.Text + "<br/>";
        //mM2.Body = mM2.Body + "Delivery From: " + lblEName.Text + " (" + lblCTPAdd.Text + ") " + "";
        mM2.Body = mM2.Body + "</p>";
        //mM2.Body = mM2.Body + "<p>&nbsp;</p>";

        if (txtTC.Text.Length > 0)
        {
            mM2.Body = mM2.Body + "<p>Terms & Conditions: " + txtTC.Text + "</p>";
        }

        //mM2.Body = mM2.Body + "<p>&nbsp;</p>";

        //mM2.Body = mM2.Body + "<p>";
        //mM2.Body = mM2.Body + "Once item(s) has been sent out for your order another email will be sent to you ";
        //mM2.Body = mM2.Body + "to confirm the dispatch along with the tracking details of this order.";
        //mM2.Body = mM2.Body + "</p>";



        mM2.Body = mM2.Body + "<p>";
        mM2.Body = mM2.Body + "If you have any further enquiries <br/> please do contact us at - <br/> ";

        mM2.Body = mM2.Body + "<a href='mailto:marketing@rangs.com.bd'>marketing@rangs.com.bd</a>";
        mM2.Body = mM2.Body + "</p>";

        mM2.Body = mM2.Body + "<p>";
        mM2.Body = mM2.Body + "Kind Regards, <br/> ";
        mM2.Body = mM2.Body + "<a href='http://www.rangs.com.bd/'>Rangs Electronics Ltd</a>";
        mM2.Body = mM2.Body + "</p>";

        mM2.IsBodyHtml = true;
        mM2.Priority = MailPriority.High;
        SmtpClient sC1 = new SmtpClient("mail.rangs.com.bd");
        //sC1.Port = 587;
        sC1.Port = 2525;
        sC1.Credentials = new System.Net.NetworkCredential("dms@rangs.com.bd", "Exampass@567");
        //sC.EnableSsl = true;
        sC1.Send(mM2);


        //----------------------------------------------------------------------------------------
        //mM2.IsBodyHtml = true;
        //SmtpClient smtp2 = new SmtpClient();
        //smtp2.Host = "smtp.gmail.com";
        //smtp2.Credentials = new System.Net.NetworkCredential
        //     ("rangs.eshop@gmail.com", "Admin@321");

        //smtp.Port = 587;
        //smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
        //smtp.UseDefaultCredentials = false;

        //smtp2.EnableSsl = true;
        //smtp2.Send(mM2);
        //----------------------------------------------------------------------------------------

    }

    protected void fnSMSforIncentive()
    {
        SqlConnection conn = DBConnection.GetConnection();
        SqlConnection connS = DBConnection.GetConnection();
        SqlConnection connHR = DBConnectionHRM.GetConnection();
        SqlConnection connSMS = DBConnectionSMS.GetConnection();
        //-----------------------------------------------------------------
        double dIncAmnt = 0;
        double dTotalIncAmnt = 0;
        string sMonth = String.Format("{0:MMM}", DateTime.Now);
        string sYear = String.Format("{0:yyyy}", DateTime.Now);

        int FYs, FYe;
        DateTime sFYs, sFYe, sDate, eDate;
        DateTime date = DateTime.Today;
        var fDayOfMonth = new DateTime(date.Year, date.Month, 1);
        var lDayOfMonth = fDayOfMonth.AddMonths(1).AddDays(-1);

        sDate = Convert.ToDateTime(fDayOfMonth);
        eDate = Convert.ToDateTime(lDayOfMonth);
        //-----------------------------------------------------------------

        if (txtJobID.Text == "")
        {
            PopupMessage("Please enter Customer Contact #.", btnSave);
            txtJobID.Focus();
            return;
        }

        string sMobile = "";

        //RETRIVE MOBILE NUMBER
        string sSql = "";
        sSql = "SELECT * FROM vw_EmpInfo WHERE JobCod='" + this.txtJobID.Text + "'";
        SqlCommand cmdCust = new SqlCommand(sSql, connHR);
        connHR.Open();
        SqlDataReader drCust = cmdCust.ExecuteReader();

        if (drCust.Read())
        {
            sMobile = drCust["Phone"].ToString();
        }
        else
        {
            sMobile = "";
        }
        connHR.Close();
        //-----------------------------------------------------------------------

        //RETRIVE CURRENT INVOICE INCENTIVE
        sSql = "";
        sSql = "SELECT dbo.MRSRMaster.TrType, dbo.MRSRMaster.Issby, dbo.Product.Model, dbo.Product.UnitPrice,";
        sSql = sSql + " dbo.Product.GetIncentive, ISNULL(dbo.Product.BLIPAmnt,0) AS BLIPAmnt, dbo.MRSRDetails.UnitPrice, SUM(ABS(dbo.MRSRDetails.Qty)) AS tQty,";
        sSql = sSql + " dbo.MRSRDetails.UnitPrice - dbo.Product.BLIPAmnt AS dIncAmnt, dbo.MRSRMaster.MRSRCode,";
        sSql = sSql + " dbo.MRSRMaster.MRSRMID, dbo.MRSRMaster.OutSource, dbo.MRSRDetails.MRSRDID, dbo.MRSRDetails.ProductID, dbo.MRSRMaster.TDate";
        sSql = sSql + " FROM  dbo.Product INNER JOIN";
        sSql = sSql + " dbo.MRSRDetails ON dbo.Product.ProductID = dbo.MRSRDetails.ProductID INNER JOIN";
        sSql = sSql + " dbo.MRSRMaster ON dbo.MRSRDetails.MRSRMID = dbo.MRSRMaster.MRSRMID";

        sSql = sSql + " WHERE (dbo.MRSRMaster.TrType = 3) AND (dbo.MRSRMaster.Issby = '" + txtJobID.Text + "')";
        sSql = sSql + " AND (dbo.Product.GetIncentive = 1) AND (dbo.MRSRMaster.MRSRCode = '" + txtCHNo.Text + "')";

        sSql = sSql + " GROUP BY dbo.MRSRMaster.TrType, dbo.MRSRMaster.Issby, dbo.Product.Model, dbo.Product.UnitPrice, dbo.Product.GetIncentive, dbo.Product.BLIPAmnt,"; 
        sSql = sSql + " dbo.MRSRDetails.UnitPrice, dbo.MRSRMaster.MRSRCode, dbo.MRSRMaster.MRSRMID, dbo.MRSRMaster.OutSource, dbo.MRSRDetails.MRSRDID, ";
        sSql = sSql + " dbo.MRSRDetails.ProductID, dbo.MRSRMaster.TDate";

        SqlCommand cmdR = new SqlCommand(sSql, conn);
        conn.Open();
        SqlDataReader drR = cmdR.ExecuteReader();

        if (drR.Read())
        {
            dIncAmnt = Convert.ToDouble(drR["dIncAmnt"].ToString());

            sSql = "";
            sSql = "INSERT INTO tbIncentiveDetails(MRSRMID,MRSRCode,TDate,MRSRDID,";
            sSql = sSql + " ProductID,UnitPrice,BLIPAmnt,IncentiveAmnt,";
            sSql = sSql + " tQty,JobID,EID)";            
            sSql = sSql + " Values ('" + drR["MRSRMID"].ToString() + "','" + drR["MRSRCode"].ToString() + "',";
            sSql = sSql + " '" + drR["TDate"].ToString() + "','" + drR["MRSRDID"].ToString() + "',";
            sSql = sSql + " '" + drR["ProductID"].ToString() + "','" + drR["UnitPrice"].ToString() + "',";
            sSql = sSql + " '" + drR["BLIPAmnt"].ToString() + "','" + drR["dIncAmnt"].ToString() + "',";
            sSql = sSql + " '" + drR["tQty"].ToString() + "','" + drR["Issby"].ToString() + "','" + drR["OutSource"].ToString() + "'";
            sSql = sSql + " )";
            SqlCommand cmdS = new SqlCommand(sSql, connS);
            connS.Open();
            cmdS.ExecuteNonQuery();
            connS.Close();

        }
        else
        {
            dIncAmnt = 0;
            return;
        }
        conn.Close();
        //-----------------------------------------------------------------------

        //RETRIVE CURRENT MONTHLY INCENTIVE
        sSql = "";
        sSql = "SELECT dbo.MRSRMaster.TrType, dbo.MRSRMaster.Issby, dbo.Product.GetIncentive,";
        //sSql = sSql + " SUM(dbo.MRSRDetails.UnitPrice) - SUM(dbo.Product.BLIPAmnt) AS dIncAmnt";
        sSql = sSql + " SUM((dbo.MRSRDetails.UnitPrice - dbo.Product.BLIPAmnt) ";
        sSql = sSql + " * Abs(dbo.MRSRDetails.Qty)) AS dIncAmnt, SUM(Abs(dbo.MRSRDetails.Qty)) AS tQty";

        sSql = sSql + " FROM dbo.Product INNER JOIN";
        sSql = sSql + " dbo.MRSRDetails ON dbo.Product.ProductID = dbo.MRSRDetails.ProductID INNER JOIN";
        sSql = sSql + " dbo.MRSRMaster ON dbo.MRSRDetails.MRSRMID = dbo.MRSRMaster.MRSRMID";

        //sSql = sSql + " WHERE (dbo.Product.GetIncentive = 0)";
        sSql = sSql + " WHERE (dbo.MRSRMaster.TrType = 3) AND (dbo.MRSRMaster.Issby = '" + txtJobID.Text + "')";
        sSql = sSql + " AND (dbo.Product.GetIncentive = 1)";
        sSql = sSql + " AND (dbo.MRSRMaster.TDate >= '" + sDate + "') AND (dbo.MRSRMaster.TDate <= '" + eDate + "')";

        sSql = sSql + " GROUP BY dbo.MRSRMaster.TrType, dbo.MRSRMaster.Issby, dbo.Product.GetIncentive";
        //sSql = sSql + " HAVING  dbo.MRSRMaster.TrType = 3) AND (dbo.MRSRMaster.Issby > N'0')";

        SqlCommand cmdR1 = new SqlCommand(sSql, conn);
        conn.Open();
        SqlDataReader drR1 = cmdR1.ExecuteReader();

        if (drR1.Read())
        {
            dTotalIncAmnt = Convert.ToDouble(drR1["dIncAmnt"].ToString());
        }
        else
        {
            dTotalIncAmnt = 0;
        }
        conn.Close();
        //-----------------------------------------------------------------------


        //SEND SMS
        if (sMobile != "")
        {
            string smsText = "";
            smsText = "Dear " + txtJobID.Text + ",\n";
            //smsText = smsText + "Your Bill Details:\n";
            smsText = smsText + "You have earned Tk. " + dIncAmnt + " for Invoice# " + this.txtCHNo.Text + ".\n";
            //smsText = smsText + "Date: " + this.txtDate.Text + ".\n";
            smsText = smsText + "Your total earning Tk. " + dTotalIncAmnt + " in " + sMonth + "-" + sYear + ".\n";
            smsText = smsText + "Sale More. Earn More.\n";
            smsText = smsText + "REL-LGP";

            sSql = "";
            sSql = "INSERT INTO tbSMS(ContactNo,SMSText,UserID,EntryDate,SMSSource)" +
                    " Values ('" + sMobile + "','" + smsText + "'," +
                    " '" + Session["UserID"] + "','" + DateTime.Today + "'," +
                    " 'DMS'" +
                    " )";
            SqlCommand cmdSMS = new SqlCommand(sSql, connSMS);
            connSMS.Open();
            cmdSMS.ExecuteNonQuery();
            connSMS.Close();
        }


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
        txtEmail.Text = "";

        txtCode.Text = "";
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


        txtNetAmnt.Text = "0";
        txtCash.Text = "0";
        txtDue.Text = "0";
        txtCardAmnt1.Text = "0";
        txtCardAmnt2.Text = "0";

        txtChequeNo.Text = "";
        txtBankName.Text = "";
        txtSecurityCode.Text = "";
        txtApprovalCode1.Text = "";

        txtChequeNo2.Text = "";
        txtBankName2.Text = "";
        txtSecurityCode2.Text = "";
        txtApprovalCode2.Text = "";

        //LOAD AUTO BILL NUMBER
        fnLoadAutoBillNo();

        txtCustName.Focus();

        //vDeclare.sBillNo = "";

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

            this.txtCash.Text = runningTotal.ToString();

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
        
        Response.Redirect("Sales_Bill_Print.aspx");
                
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

    
    protected void txtCardAmnt1_TextChanged(object sender, EventArgs e)
    {
        if (this.lblNetAmnt.Text.Length == 0)
        {
            this.lblNetAmnt.Text = "0";
        }

        if (this.txtCardAmnt1.Text.Length == 0)
        {
            this.txtCardAmnt1.Text = "0";
        }

        if (this.txtCardAmnt2.Text.Length == 0)
        {
            this.txtCardAmnt2.Text = "0";
        }

        if (this.txtCash.Text.Length == 0)
        {
            this.txtCash.Text = "0";
        }

        double dTotalPay = 0;
        dTotalPay = Convert.ToDouble(this.txtCash.Text) + Convert.ToDouble(this.txtCardAmnt1.Text) + Convert.ToDouble(this.txtCardAmnt2.Text);
        this.txtPay.Text = Convert.ToString(dTotalPay);

        double dDue = 0;
        //dDue = Convert.ToDouble(this.lblNetAmnt.Text) - (Convert.ToDouble(this.txtCash.Text) + Convert.ToDouble(this.txtCardAmnt1.Text) + Convert.ToDouble(this.txtCardAmnt2.Text));
        dDue = Convert.ToDouble(this.lblNetAmnt.Text) - dTotalPay;
        this.txtDue.Text = Convert.ToString(dDue); 

    }
        
    protected void txtCardAmnt2_TextChanged(object sender, EventArgs e)
    {
        if (this.lblNetAmnt.Text.Length == 0)
        {
            this.lblNetAmnt.Text = "0";
        }

        if (this.txtCardAmnt1.Text.Length == 0)
        {
            this.txtCardAmnt1.Text = "0";
        }

        if (this.txtCardAmnt2.Text.Length == 0)
        {
            this.txtCardAmnt2.Text = "0";
        }

        if (this.txtCash.Text.Length == 0)
        {
            this.txtCash.Text = "0";
        }

        double dTotalPay = 0;
        dTotalPay = Convert.ToDouble(this.txtCash.Text) + Convert.ToDouble(this.txtCardAmnt1.Text) + Convert.ToDouble(this.txtCardAmnt2.Text);
        this.txtPay.Text = Convert.ToString(dTotalPay);

        double dDue = 0;
        //dDue = Convert.ToDouble(this.lblNetAmnt.Text) - (Convert.ToDouble(this.txtCash.Text) + Convert.ToDouble(this.txtCardAmnt1.Text) + Convert.ToDouble(this.txtCardAmnt2.Text));
        dDue = Convert.ToDouble(this.lblNetAmnt.Text) - dTotalPay;
        this.txtDue.Text = Convert.ToString(dDue); 

    }
    
    protected void txtCash_TextChanged(object sender, EventArgs e)
    {
        if (this.lblNetAmnt.Text.Length == 0)
        {
            this.lblNetAmnt.Text = "0";
        }

        if (this.txtCardAmnt1.Text.Length == 0)
        {
            this.txtCardAmnt1.Text = "0";
        }

        if (this.txtCardAmnt2.Text.Length == 0)
        {
            this.txtCardAmnt2.Text = "0";
        }

        if (this.txtCash.Text.Length == 0)
        {
            this.txtCash.Text = "0";
        }

        double dTotalPay = 0;
        dTotalPay = Convert.ToDouble(this.txtCash.Text) + Convert.ToDouble(this.txtCardAmnt1.Text) + Convert.ToDouble(this.txtCardAmnt2.Text);
        this.txtPay.Text = Convert.ToString(dTotalPay); 

        double dDue = 0;
        //dDue = Convert.ToDouble(this.lblNetAmnt.Text) - (Convert.ToDouble(this.txtCash.Text) + Convert.ToDouble(this.txtCardAmnt1.Text) + Convert.ToDouble(this.txtCardAmnt2.Text));
        dDue = Convert.ToDouble(this.lblNetAmnt.Text) - dTotalPay;
        this.txtDue.Text = Convert.ToString(dDue);  
    }

    protected void txtCode_TextChanged(object sender, EventArgs e)
    {
        SqlConnection conn = DBConnection.GetConnection();
        string sSql = "";
        double UP = 0;
        double CampPrice = 0;

        sSql = "";
        
        //sSql = "SELECT ProductID,ProdName,UnitPrice,Model,Code,";
        //sSql = sSql + " IncentiveType,BLIPAmnt,IncentiveAmnt,GetIncentive,WPPrice,BLIPofWP,WPIncentive,ISNULL(WPMinQty,0) AS WPMinQty";
        //sSql = sSql + " FROM Product";

        sSql = "SELECT ProductID,ProdName,UnitPrice,Model,Code,";
        sSql = sSql + " IncentiveType,BLIPAmnt,IncentiveAmnt,";
        sSql = sSql + " GetIncentive, ISNULL(WPPrice,0) AS WPPrice, ISNULL(BLIPofWP,0) AS BLIPofWP,";
        sSql = sSql + " ISNULL(WPIncentive,0) AS WPIncentive, ISNULL(WPMinQty,0) AS WPMinQty";
        sSql = sSql + " FROM Product";

        sSql = sSql + " WHERE Code='" + this.txtCode.Text + "'";
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
                                
                this.txtProdDesc.Text = dr["ProdName"].ToString();
                UP = Convert.ToDouble(dr["UnitPrice"].ToString());
                this.txtUP.Text = Convert.ToString(UP);

                this.lblIncentiveType.Text = dr["IncentiveType"].ToString();
                this.lblBLIPAmnt.Text = dr["BLIPAmnt"].ToString();
                this.lblIncentiveAmnt.Text = dr["IncentiveAmnt"].ToString();
                this.lblGetIncentive.Text = dr["GetIncentive"].ToString();
                this.lblWPPrice.Text = dr["WPPrice"].ToString();
                this.lblBLIPofWP.Text = dr["BLIPofWP"].ToString();
                this.lblWPIncentive.Text = dr["WPIncentive"].ToString();
                this.lblWPMinQty.Text = dr["WPMinQty"].ToString();
            }
            else
            {
                UP = 0;
                this.txtUP.Text = Convert.ToString(UP);
                this.txtProdID.Text = "";
                this.txtProdDesc.Text = "";
                this.ddlContinents.SelectedItem.Text = "";

                this.lblIncentiveType.Text = "";
                this.lblBLIPAmnt.Text = "0";
                this.lblIncentiveAmnt.Text = "0";
                this.lblGetIncentive.Text = "0";
                this.lblWPPrice.Text = "0";
                this.lblBLIPofWP.Text = "0";
                this.lblWPIncentive.Text = "0";
                this.lblWPMinQty.Text = "0";

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
        //------------------------------------------------------

        //lblUP.Text = txtCP.Text;
        //if (lblGetIncentive.Text == "True")
        //{
        //    if (lblIncentiveType.Text == "Instant")
        //    {
        //        this.txtCP.Text = lblBLIPAmnt.Text;
        //    }
        //    else
        //    {
        //        this.txtCP.Text = Convert.ToString(CampPrice);
        //    }
        //}
        //------------------------------------------------------

    }


    //LOAD CUSTOMER INFO
    protected void btnCustSearch_Click(object sender, ImageClickEventArgs e)
    {
        SqlConnection conn = DBConnection.GetConnection();

        if (txtCustContact.Text == "")
        {
            PopupMessage("Please enter Customer Contact #.", btnSave);
            txtCustContact.Focus();
            return;
        }

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
                this.ddlCity.SelectedItem.Text = drCust["City"].ToString();
                this.txtDOB.Text = drCust["DOBT"].ToString();
                this.txtOrg.Text = drCust["Org"].ToString();
                this.txtDesg.Text = drCust["Desg"].ToString();

                if (drCust["Profession"].ToString() == "Business")
                {
                    optProfession.SelectedIndex = 0;
                }
                else if (drCust["Profession"].ToString() == "Service")
                {
                    optProfession.SelectedIndex = 1;
                }
                else if (drCust["Profession"].ToString() == "Others")
                {
                    optProfession.SelectedIndex = 2;
                }


                if (drCust["CustSex"].ToString() == "Male")
                {
                    optSex.SelectedIndex = 0;
                }
                else if (drCust["CustSex"].ToString() == "Female")
                {
                    optSex.SelectedIndex = 1;
                }

            }
            else
            {
                this.txtCustName.Text = "";
                this.txtCustAdd.Text = "";
                this.txtEmail.Text = "";
                this.ddlCity.SelectedItem.Text = "";
                this.txtDOB.Text = "";
                this.txtOrg.Text = "";
                this.txtDesg.Text = "";
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


    protected void txtModel_TextChanged(object sender, EventArgs e)
    {
        SqlConnection conn = DBConnection.GetConnection();
        double UP = 0;
        double dBLIP = 0;
        double CampPrice = 0;
        string sSql = "";

        sSql = "";
        //sSql = "SELECT ProductID,ProdName,UnitPrice,Model,Code,BLIPAmnt FROM Product" +
        //    " WHERE Model='" + this.txtModel.Text + "'";
        sSql = "SELECT ProductID,ProdName,UnitPrice,Model,Code,";
        sSql = sSql + " IncentiveType,BLIPAmnt,IncentiveAmnt,";
        sSql = sSql + " GetIncentive, ISNULL(WPPrice,0) AS WPPrice, ISNULL(BLIPofWP,0) AS BLIPofWP,";
        sSql = sSql + " ISNULL(WPIncentive,0) AS WPIncentive, ISNULL(WPMinQty,0) AS WPMinQty";
        sSql = sSql + " FROM Product";
        sSql = sSql + " WHERE Model='" + this.txtModel.Text + "'";

        SqlCommand cmd = new SqlCommand(sSql, conn);
        conn.Open();
        SqlDataReader dr = cmd.ExecuteReader();

        try
        {
            if (dr.Read())
            {
                this.txtCode.Text = dr["Code"].ToString();
                this.ddlContinents.SelectedItem.Text = dr["Model"].ToString();
                this.txtProdID.Text = dr["ProductID"].ToString();
                this.txtProdDesc.Text = dr["ProdName"].ToString();
                UP = Convert.ToDouble(dr["UnitPrice"].ToString());
                this.txtUP.Text = Convert.ToString(UP);
                dBLIP = Convert.ToDouble(dr["BLIPAmnt"].ToString());
                this.txtBLIPAmnt.Text = Convert.ToString(dBLIP);

                this.lblIncentiveType.Text = dr["IncentiveType"].ToString();
                this.lblBLIPAmnt.Text = dr["BLIPAmnt"].ToString();
                this.lblIncentiveAmnt.Text = dr["IncentiveAmnt"].ToString();
                this.lblGetIncentive.Text = dr["GetIncentive"].ToString();
                this.lblWPPrice.Text = dr["WPPrice"].ToString();
                this.lblBLIPofWP.Text = dr["BLIPofWP"].ToString();
                this.lblWPIncentive.Text = dr["WPIncentive"].ToString();
                this.lblWPMinQty.Text = dr["WPMinQty"].ToString();

                
            }
            else
            {
                UP = 0;
                dBLIP = 0;
                this.txtUP.Text = Convert.ToString(UP);
                this.txtCode.Text = "";
                this.ddlContinents.SelectedItem.Text = "";
                this.txtProdID.Text = "";
                this.txtProdDesc.Text = "";

                this.lblIncentiveType.Text = "";
                this.lblBLIPAmnt.Text = "0";
                this.lblIncentiveAmnt.Text = "0";
                this.lblGetIncentive.Text = "0";
                this.lblWPPrice.Text = "0";
                this.lblBLIPofWP.Text = "0";
                this.lblWPIncentive.Text = "0";
                this.lblWPMinQty.Text = "0";

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
            " WHERE Model='" + txtModel.Text + "'" +
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
        //------------------------------------------------------

        //lblUP.Text = txtCP.Text;
        //if (lblGetIncentive.Text == "True")
        //{
        //    if (lblIncentiveType.Text == "Instant")
        //    {
        //        this.txtCP.Text = lblBLIPAmnt.Text;
        //    }
        //    else
        //    {
        //        this.txtCP.Text = Convert.ToString(CampPrice);
        //    }
        //}
        //------------------------------------------------------

    }


    protected void btnSearchSalesBy_Click(object sender, ImageClickEventArgs e)
    {
        SqlConnection conn = DBConnection.GetConnection();
        SqlConnection connHR = DBConnectionHRM.GetConnection();

        if (txtJobID.Text == "")
        {
            PopupMessage("Please enter Customer Contact #.", btnSave);
            txtJobID.Focus();
            return;
        }

        //CHECK & INSERT CUSTOMER INFO
        string sSql = "";
        sSql = "SELECT * FROM vw_EmpInfo WHERE JobCod='" + this.txtJobID.Text + "'";
        SqlCommand cmdCust = new SqlCommand(sSql, connHR);
        connHR.Open();
        SqlDataReader drCust = cmdCust.ExecuteReader();
        try
        {
            if (drCust.Read())
            {
                this.lblSalesBy.Text = drCust["FullName"].ToString() + ", " + drCust["Desg"].ToString() + ", " + drCust["DeptNm"].ToString() + ", " + drCust["Location"].ToString();
            }
            else
            {
                //this.lblSalesBy.Text = "";  
                sSql = "";
                sSql = "SELECT EID,EntityCode,eName,ContactPerson,ContactNo FROM Entity WHERE EntityCode='" + this.txtJobID.Text + "'";
                SqlCommand cmdE = new SqlCommand(sSql, conn);
                conn.Open();
                SqlDataReader drE = cmdE.ExecuteReader();
                if (drE.Read())
                {
                    this.lblSalesBy.Text = drE["ContactPerson"].ToString() + ", " + drE["eName"].ToString();
                }
                else
                {
                    this.lblSalesBy.Text = "";
                }
                drE.Dispose();
                drE.Close();
                conn.Close();

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
            connHR.Close();
        }
        //----------------------------------------------------------------------

    }

    private void fnClaimList()
    {
        SqlConnection conn = DBConnection.GetConnection();
        //using (SqlConnection conn = new SqlConnection())
        //{
        //conn.ConnectionString = ConfigurationManager
        //.ConnectionStrings["constr"].ConnectionString;

        using (SqlCommand cmd = new SqlCommand())
        {
            cmd.CommandText = "Select TAIC, TermsCondition from tbTC ORDER BY OrderBy";
            cmd.Connection = conn;
            conn.Open();
            using (SqlDataReader sdr = cmd.ExecuteReader())
            {
                while (sdr.Read())
                {
                    ListItem item = new ListItem();
                    item.Text = sdr["TermsCondition"].ToString();
                    item.Value = sdr["TAIC"].ToString();
                    //item.Selected = Convert.ToBoolean(sdr["IsSelected"]);
                    chkTC.Items.Add(item);
                }
            }
            conn.Close();
        }
        //}
    }

    protected void ddlDealerName_SelectedIndexChanged(object sender, EventArgs e)
    {
        SqlConnection conn = DBConnectionDSM.GetConnection();
        double UP = 0;
        double CampPrice = 0;
        string sSql = "";

        sSql = "";
        sSql = "SELECT CategoryID,DAID,Code,Name,ZoneName,Address, ContactNo, EmailAdd FROM VW_Delear_Info ";
        sSql = sSql + " WHERE (Name = '" + ddlDealerName.SelectedItem.Text + "') ";
        sSql = sSql + " AND (CategoryID='" + Session["sZoneID"].ToString() + "')";

        SqlCommand cmd = new SqlCommand(sSql, conn);
        conn.Open();
        SqlDataReader dr = cmd.ExecuteReader();

        //try
        //{
        if (dr.Read())
        {
            this.txtDealerCode.Text = dr["Code"].ToString();
            this.lblDealerID.Text = dr["DAID"].ToString();
            this.lblDealerCode.Text = dr["Code"].ToString();
            this.lblDealerName.Text = dr["Name"].ToString();
            this.lblDealerAdd.Text = dr["Address"].ToString();
            this.lblDealerMobile.Text = dr["ContactNo"].ToString();
            this.lblDealerEmail.Text = dr["EmailAdd"].ToString();

            this.txtCustContact.Text = dr["ContactNo"].ToString();
            //this.txtEmail.Text = dr["EmailAdd"].ToString();
            //this.txtProdDesc.Text = dr["ProdName"].ToString();
        }
        else
        {
            this.txtCode.Text = "";
            lblDealerID.Text = "0";
            //this.txtCustContact.Text = "";
            //this.txtEmail.Text = "";
            //this.txtCreditLimit.Text = "0";
            //this.txtOutstanding.Text = "0";

        }
        //}
        //catch (InvalidCastException err)
        //{
        //    throw (err);
        //}
        //finally
        //{
        dr.Dispose();
        dr.Close();
        conn.Close();
        //}


        ////LOAD CREDIT LIMIT
        //fnLoadCreditLimit();

        ////LOAD STATEMENT/OUTSTANDING
        //fnLoadOutStanding();
        //double dOutStanding = Convert.ToDouble(lblOBSales.Text) - Convert.ToDouble(lblOBCollection.Text) + Convert.ToDouble(lblOBDis.Text) - Convert.ToDouble(lblOBWith.Text);
        //txtOutstanding.Text = Convert.ToString(dOutStanding);

    }

    //LOAD Dealer IN DROPDOWN LIST
    protected void LoadDropDownList_Dealer()
    {
        SqlConnection conn = DBConnectionDSM.GetConnection();

        String strQuery = "SELECT CategoryID,DAID,Name,ZoneName FROM VW_Delear_Info ";
        strQuery = strQuery + " WHERE (Discontinue = 'No') AND (CategoryID='" + Session["sZoneID"].ToString() + "')";
        strQuery = strQuery + " ORDER BY Name";
        //SqlConnection con = new SqlConnection("conn");
        SqlCommand cmd = new SqlCommand(strQuery, conn);
        //SqlCommand cmd = new SqlCommand();
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = strQuery;
        cmd.Connection = conn;
        //try
        //{
        conn.Open();
        ddlDealerName.DataSource = cmd.ExecuteReader();
        ddlDealerName.DataTextField = "Name";
        ddlDealerName.DataValueField = "DAID";
        ddlDealerName.DataBind();

        //Add blank item at index 0.
        ddlDealerName.Items.Insert(0, new ListItem("", "0"));
        //ddlDealerName.Items.Insert(1, new ListItem("CI&DD (REL)", "370"));

        //}
        //catch (Exception ex)
        //{
        //    throw ex;
        //}
        //finally
        //{
        //    conn.Close();
        //    conn.Dispose();
        //}
    }

}
