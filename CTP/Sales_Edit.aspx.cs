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

public partial class Forms_Sales_Edit : System.Web.UI.Page
{
    SqlConnection conn = DBConnection.GetConnection();
    SqlConnection conn1 = DBConnection.GetConnection();
    SqlConnection _connStr = DBConnection.GetConnection();
    int iMRSRID = 0;
    int iDelTag = 0;

    DataTable dt;
    DateTime tDate;

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

            //LOAD CTP
            LoadDropDownList_CTP();
            ddlEntity.SelectedItem.Text = Session["eName"].ToString();
            ddlEntity.SelectedItem.Value = Session["EID"].ToString();

            //LOAD T & C
            //fnLoadTC();

            //LOAD TERMS & CONDITIONS
            this.fnClaimList();


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


    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (System.Convert.ToInt32(Session["Vis"]) == 0)
        {
            Response.Redirect("../Account/Login.aspx");
        }

        SqlConnection conn = DBConnection.GetConnection();
        string sSql = "";

        //CLEAR GRIDVIEW
        gvUsers.DataSource = null;
        gvUsers.DataBind();

        //CLEAR DATA TABLE
        dt.Clear();


        //LOAD MASTER DATA
        sSql = "";
        sSql = " SELECT dbo.MRSRMaster.MRSRMID, dbo.MRSRMaster.MRSRCode," +
            " CONVERT(varchar(12), TDate, 101) AS TDate, dbo.MRSRMaster.OutSource," +

            "dbo.MRSRMaster.NetSalesAmnt," +
            "dbo.MRSRMaster.PayAmnt,dbo.MRSRMaster.DueAmnt,dbo.MRSRMaster.PayMode," +
            "dbo.MRSRMaster.CashAmnt,dbo.MRSRMaster.CardAmnt1,dbo.MRSRMaster.CardAmnt2," +
            "dbo.MRSRMaster.CardNo1,dbo.MRSRMaster.CardNo2,dbo.MRSRMaster.CardType1,dbo.MRSRMaster.CardType2," +
            "dbo.MRSRMaster.Bank1,dbo.MRSRMaster.Bank2,dbo.MRSRMaster.SecurityCode,dbo.MRSRMaster.SecurityCode2," +
            "dbo.MRSRMaster.AppovalCode1,dbo.MRSRMaster.AppovalCode2,dbo.MRSRMaster.Issby," +
            "dbo.MRSRMaster.Authorby,dbo.MRSRMaster.DeliveryFrom, dbo.MRSRMaster.POCode," +
            "dbo.MRSRMaster.Remarks,dbo.MRSRMaster.TermsCondition, dbo.MRSRMaster.RefCHNo," +
            
            " dbo.MRSRMaster.Customer, dbo.Customer.CustID," +
            " dbo.Customer.CustName, dbo.Customer.Address," +
            " dbo.Customer.CustSex, dbo.Customer.Profession," +
            " dbo.Customer.Mobile, dbo.Customer.Email," +
            " dbo.Customer.City, dbo.Customer.DOBT," +            
            " dbo.Customer.Org, dbo.Customer.Desg" +

            " FROM dbo.MRSRMaster LEFT OUTER JOIN" +
            " dbo.Customer ON dbo.MRSRMaster.Customer = dbo.Customer.Mobile" +
            " WHERE (dbo.MRSRMaster.MRSRCode = '" + this.txtCHNo.Text + "')" +
            " AND (dbo.MRSRMaster.TrType = 3)" +
            " AND (dbo.MRSRMaster.TDate = '" + DateTime.Today + "')" +
            " AND (dbo.MRSRMaster.OutSource='" + Session["EID"] + "')";

        SqlCommand cmd = new SqlCommand(sSql, conn);
        conn.Open();
        SqlDataReader dr = cmd.ExecuteReader();

        if (dr.Read())
        {
            this.txtMRSRID.Text = Convert.ToString(dr["MRSRMID"].ToString());
            Session["iMRSRID"] = dr["MRSRMID"].ToString();
            this.txtDate.Text = Convert.ToString(dr["TDate"].ToString());
                        
            this.txtNetAmnt.Text = Convert.ToString(dr["NetSalesAmnt"].ToString());
            this.txtPay.Text = Convert.ToString(dr["PayAmnt"].ToString());
            this.txtDue.Text = Convert.ToString(dr["DueAmnt"].ToString());
            this.ddlPayType.SelectedItem.Text = Convert.ToString(dr["PayMode"].ToString());
            this.txtCash.Text = Convert.ToString(dr["CashAmnt"].ToString());
            this.txtCardAmnt1.Text = Convert.ToString(dr["CardAmnt1"].ToString());
            this.txtCardAmnt2.Text = Convert.ToString(dr["CardAmnt2"].ToString());
            this.txtChequeNo.Text = Convert.ToString(dr["CardNo1"].ToString());
            this.txtChequeNo2.Text = Convert.ToString(dr["CardNo2"].ToString());
            this.ddlCardType1.SelectedItem.Text = Convert.ToString(dr["CardType1"].ToString());
            this.ddlCardType2.SelectedItem.Text = Convert.ToString(dr["CardType2"].ToString());
            this.txtBankName.Text = Convert.ToString(dr["Bank1"].ToString());
            this.txtBankName2.Text = Convert.ToString(dr["Bank2"].ToString());
            this.txtSecurityCode.Text = Convert.ToString(dr["SecurityCode"].ToString());
            this.txtSecurityCode2.Text = Convert.ToString(dr["SecurityCode2"].ToString());
            this.txtApprovalCode1.Text = Convert.ToString(dr["AppovalCode1"].ToString());
            this.txtApprovalCode2.Text = Convert.ToString(dr["AppovalCode2"].ToString());

            this.txtRefBy.Text = Convert.ToString(dr["Authorby"].ToString());
            this.txtJobID.Text = Convert.ToString(dr["Issby"].ToString());
            this.txtNote.Text = Convert.ToString(dr["Remarks"].ToString());
            this.txtTC.Text = Convert.ToString(dr["TermsCondition"].ToString());

            this.txtCustContact.Text = Convert.ToString(dr["Customer"].ToString());
            this.txtCustName.Text = Convert.ToString(dr["CustName"].ToString());
            this.txtCustAdd.Text = Convert.ToString(dr["Address"].ToString());

            if (dr["CustSex"].ToString() == "Male")
            {
                optSex.SelectedIndex = 0;
            }
            else if (dr["CustSex"].ToString() == "Female")
            {
                optSex.SelectedIndex = 1;
            }

            if (dr["Profession"].ToString() == "Business")
            {
                optProfession.SelectedIndex = 0;
            }
            else if (dr["Profession"].ToString() == "Service")
            {
                optProfession.SelectedIndex = 1;
            }
            else if (dr["Profession"].ToString() == "Others")
            {
                optProfession.SelectedIndex = 2;
            }
            this.txtEmail.Text = dr["Email"].ToString();

            this.ddlCity.SelectedItem.Text = dr["City"].ToString();            
            this.txtDOB.Text = dr["DOBT"].ToString();            
            this.txtOrg.Text = dr["Org"].ToString();
            this.txtDesg.Text = dr["Desg"].ToString();

            this.txtNote.Text = dr["Remarks"].ToString();
            this.txtRefChNo.Text = dr["RefCHNo"].ToString();
            this.txtOrderNo.Text = dr["POCode"].ToString();

        }
        else
        {
            this.txtMRSRID.Text = "";
            this.txtDate.Text = "";
            this.txtCustContact.Text = "";

            this.txtNetAmnt.Text = "";
            this.txtPay.Text = "";
            this.txtDue.Text = "";
            this.ddlPayType.SelectedItem.Text = "";
            this.txtCash.Text = "";
            this.txtCardAmnt1.Text = "";
            this.txtCardAmnt2.Text = "";
            this.txtChequeNo.Text = "";
            this.txtChequeNo2.Text = "";
            this.ddlCardType1.SelectedItem.Text = "";
            this.ddlCardType2.SelectedItem.Text = "";
            this.txtBankName.Text = "";
            this.txtBankName2.Text = "";
            this.txtSecurityCode.Text = "";
            this.txtSecurityCode2.Text = "";
            this.txtApprovalCode1.Text = "";
            this.txtApprovalCode2.Text = "";

            this.txtRefBy.Text = "";
            this.txtJobID.Text = "";
            this.txtNote.Text = "";
            this.txtTC.Text = "";

            this.txtCustContact.Text = "";

            this.txtCustName.Text = "";
            this.txtCustAdd.Text = "";
            this.txtEmail.Text = "";
            this.ddlCity.SelectedItem.Text = "";            
            this.txtDOB.Text = "";           
            this.txtOrg.Text = "";
            this.txtDesg.Text = "";

            //CLEAR GRIDVIEW
            gvUsers.DataSource = null;
            gvUsers.DataBind();

            //CLEAR DATA TABLE
            dt.Clear();

            ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert",
                        "<script>alert('" + "Sorry!!! No data found... / Time Expire ..." + "');</script>", false);
            return;

        }
        dr.Dispose();
        dr.Close();
        conn.Close();


        //LOAD DETAILS DATA
        sSql = "";        
        sSql = "SELECT dbo.Product.ProductID, dbo.Product.Model," +
            " dbo.MRSRDetails.RetPrice AS MRP," +
            " dbo.MRSRDetails.UnitPrice AS CampaignPrice," +
            " ABS(dbo.MRSRDetails.Qty) AS Qty," +
            " dbo.MRSRDetails.TotalAmnt As TotalPrice," +
            " dbo.MRSRDetails.DiscountAmnt AS DisAmnt, " +
            " dbo.MRSRDetails.DisCode, dbo.MRSRDetails.DisRef," +
            " dbo.MRSRDetails.WithAdjAmnt, dbo.MRSRDetails.NetAmnt," +
            " dbo.MRSRDetails.SLNO AS ProductSL,dbo.MRSRDetails.ProdRemarks as Remarks," +
            " dbo.MRSRDetails.BLIPAmnt,dbo.MRSRDetails.IncentiveAmnt,dbo.MRSRDetails.IncentiveType,dbo.MRSRDetails.CustShowPrice" +
            " FROM dbo.Product INNER JOIN" +
            " dbo.MRSRDetails ON dbo.Product.ProductID = dbo.MRSRDetails.ProductID" +
            " WHERE (dbo.MRSRDetails.MRSRMID = '" + this.txtMRSRID.Text + "')";

        cmd = new SqlCommand(sSql, conn);
        conn.Open();

        // Create a SqlDataAdapter to get the results as DataTable
        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sSql, conn);

        // Fill the DataTable with the result of the SQL statement
        sqlDataAdapter.Fill(dt);

        gvUsers.DataSource = dt;
        gvUsers.DataBind();

        conn.Close();

        //------------------------------------------------------------------------------------------
        // LOAD T & C
        sSql = "";
        sSql = "SELECT CTCAID, MRSRMID, InvoiceNo, tcAID, TCText";
        sSql = sSql + " FROM dbo.tbTC_Customer";
        sSql = sSql + " Where MRSRMID = '" + this.txtMRSRID.Text + "'";
        SqlCommand cmdTC = new SqlCommand(sSql, conn);
        conn.Open();
        SqlDataReader drTC = cmdTC.ExecuteReader();

        while (drTC.Read())
        {
            //this.ddlEntity.SelectedItem.Text = drTC["eName"].ToString();
            for (int l = 0; l < chkTC.Items.Count; l++)
            {
                if (chkTC.Items[l].Value == drTC["tcAID"].ToString())
                {
                    chkTC.Items[l].Selected = true;
                }
            }

        }
        drTC.Dispose();
        drTC.Close();
        conn.Close();
        //------------------------------------------------------------------------------------------

        /*
        DataTable dt = new DataTable("MyTable");
        dt.Columns.Add(new DataColumn("ProductID", typeof(SqlCommand)));
        dt.Columns.Add(new DataColumn("Model", typeof(SqlCommand)));
        dt.Columns.Add(new DataColumn("Qty", typeof(SqlCommand)));
        dt.Columns.Add(new DataColumn("MRP", typeof(SqlCommand)));
        dt.Columns.Add(new DataColumn("CampaignPrice", typeof(SqlCommand)));
        dt.Columns.Add(new DataColumn("TotalAmnt", typeof(SqlCommand)));
        dt.Columns.Add(new DataColumn("DiscountAmnt", typeof(SqlCommand)));
        dt.Columns.Add(new DataColumn("DisCode", typeof(SqlCommand)));
        dt.Columns.Add(new DataColumn("DisRef", typeof(SqlCommand)));
        dt.Columns.Add(new DataColumn("WithAdjAmnt", typeof(SqlCommand)));
        dt.Columns.Add(new DataColumn("NetAmnt", typeof(SqlCommand)));
        dt.Columns.Add(new DataColumn("SLNO", typeof(SqlCommand)));
        dt.Columns.Add(new DataColumn("ProdRemarks", typeof(SqlCommand)));

        DataRow row = dt.NewRow();
        row["ProductID"] = cmd;
        row["Model"] = cmd;
        row["Qty"] = cmd;
        row["MRP"] = cmd;
        row["CampaignPrice"] = cmd;
        row["TotalAmnt"] = cmd;
        row["DiscountAmnt"] = cmd;
        row["DisCode"] = cmd;
        row["DisRef"] = cmd;
        row["WithAdjAmnt"] = cmd;
        row["NetAmnt"] = cmd;
        row["SLNO"] = cmd;
        row["ProdRemarks"] = cmd;
        dt.Rows.Add(row);

        conn.Close();
        */

        //return dt;

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

    //LOAD CTP IN DROPDOWN LIST
    protected void LoadDropDownList_CTP()
    {
        SqlConnection conn = DBConnection.GetConnection();

        String strQuery = "select EID,eName from Entity ";
        strQuery = strQuery + " WHERE (ActiveDeactive = 1) AND";
        strQuery = strQuery + " (EntityType = 'showroom' OR  EntityType = 'zone' OR  EntityType = 'Dealer')";
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
            ddlEntity.Items.Insert(0, new ListItem("", ""));

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
        dr["Qty"] = txtQty.Text;
        dr["TotalPrice"] = txtTotalAmnt.Text;
        dr["DisAmnt"] = txtDisAmnt.Text;
        dr["DisCode"] = txtDisCode.Text;
        dr["DisRef"] = txtDisRef.Text;
        dr["WithAdjAmnt"] = txtWithAdj.Text;
        dr["NetAmnt"] = Convert.ToDouble(txtNet.Text);
        dr["ProductSL"] = txtSL.Text;
        dr["Remarks"] = txtRemarks.Text;

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
        //sSql = "SELECT ProductID,ProdName,UnitPrice,Model,Code FROM Product" +
        //    " WHERE Model='" + this.ddlContinents.SelectedItem.Text + "'";

        sSql = "SELECT ProductID,ProdName,UnitPrice,Model,Code,";
        sSql = sSql + " IncentiveType,BLIPAmnt,IncentiveAmnt,GetIncentive,WPPrice,BLIPofWP,WPIncentive,ISNULL(WPMinQty,0) AS WPMinQty";
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

            if (lblWPMinQty.Text.Length == 0)
            {
                lblWPMinQty.Text = "0";
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

        //GRIDVIEW DATA VALIDATION
        int totalRowsCount = gvUsers.Rows.Count;
        if (totalRowsCount == 0)
        {
            PopupMessage("There is no product in list. Please add product.", btnSave);
            return;
        }


        tDate = Convert.ToDateTime(this.txtDate.Text);

        //----------------------------------------------------------------------
        //DELETE PREVIOUS DATA.
        sSql = "";
        sSql = "DELETE FROM MRSRMaster" +
            " WHERE MRSRMID='" + this.txtMRSRID.Text + "'";
            //" AND OutSource='" + Session["sBrId"] + "'" +
            //" AND TrType=3";
        SqlCommand cmdd = new SqlCommand(sSql, conn);
        conn.Open();
        cmdd.ExecuteNonQuery();
        conn.Close();

        //DELETE FROM Details Table
        sSql = "";
        sSql = "DELETE FROM MRSRDetails" +
            " WHERE MRSRMID='" + this.txtMRSRID.Text + "'";
            //" AND OutSource='" + Session["sBrId"] + "'" +
            //" AND TrType=3";
        cmdd = new SqlCommand(sSql, conn);
        conn.Open();
        cmdd.ExecuteNonQuery();
        conn.Close();

        //DELETE FROM TC Table
        sSql = "";
        sSql = "DELETE FROM tbTC_Customer";
        sSql = sSql + " WHERE MRSRMID='" + this.txtMRSRID.Text + "'";

        cmdd = new SqlCommand(sSql, conn);
        conn.Open();
        cmdd.ExecuteNonQuery();
        conn.Close();
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
                sSql = sSql + " CustName='" + this.txtCustName.Text + "',";
                sSql = sSql + " Address='" + this.txtCustAdd.Text + "',";
                sSql = sSql + " City='" + this.ddlCity.SelectedItem.Text + "',";
                sSql = sSql + " Email='" + this.txtEmail.Text + "',";
                sSql = sSql + " Profession='" + sProfession + "',";
                sSql = sSql + " Org='" + this.txtOrg.Text + "',";
                sSql = sSql + " Desg='" + this.txtDesg.Text + "',";
                sSql = sSql + " CustSex='" + sSex + "',";
                sSql = sSql + " DOBT='" + txtDOB.Text + "',";
                sSql = sSql + " IdentityType='N/A', IdentityNo='N/A'";
                sSql = sSql + " WHERE Mobile='" + this.txtCustContact.Text + "'";
                SqlCommand cmdC = new SqlCommand(sSql, conn1);
                conn1.Open();
                cmdC.ExecuteNonQuery();
                conn1.Close();

            }
            else
            {
                sSql = "";
                sSql = "INSERT INTO Customer(Mobile,CustName,Address,City," +
                       "Email,Profession, Org, Desg,CustSex,IdentityType,IdentityNo,DOBT)" +
                        " Values ('" + this.txtCustContact.Text + "','" + this.txtCustName.Text + "'," +
                        " '" + this.txtCustAdd.Text + "','" + this.ddlCity.SelectedItem.Text + "'," +
                        " '" + this.txtEmail.Text + "','" + sProfession + "'," +
                        " '" + this.txtOrg.Text + "','" + this.txtDesg.Text + "'," +
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

        if (ddlEntity.SelectedItem.Value == Session["sBrId"].ToString())
        {
            iDelTag = 1;
        }
        else
        {
            iDelTag = 2;
        }

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
        //" '" + this.txtCustContact.Text + "', '" + Session["UserName"] + "', '" + DateTime.Now + "'," +
        //" '" + this.txtCustContact.Text + "', '" + lblUserID.Text + "', '" + lblEntryDate.Text + "'," +

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
            " '" + dTAmnt + "','" + this.txtTC.Text + "'," +
            " '" + this.txtCash.Text + "','" + this.txtCardAmnt1.Text + "','" + this.txtCardAmnt2.Text + "'," +
            " '" + this.txtChequeNo.Text + "','" + this.txtChequeNo2.Text + "','" + this.ddlCardType1.SelectedItem.Text + "','" + this.ddlCardType2.SelectedItem.Text + "'," +
            " '" + this.txtBankName.Text + "','" + this.txtBankName2.Text + "','" + this.txtSecurityCode.Text + "','" + this.txtSecurityCode2.Text + "'," +
            " '" + this.txtApprovalCode1.Text + "','" + this.txtApprovalCode2.Text + "',1," +
            " '" + this.txtRefBy.Text + "','" + this.txtJobID.Text + "'," +
            " '" + this.ddlEntity.SelectedItem.Value + "','" + this.txtNote.Text + "'," +
            " '" + iDelTag + "','" + txtRefChNo.Text + "','" + txtOrderNo.Text + "'" +
        " )";

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

        //----------------------------------------------------------------
        //SAVE TERMS & CONDITIONS
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
        //----------------------------------------------------------------

        //######################################################################################
        //SAVE DATA IN HISTORY MASTER TABLE
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
            " '" + this.txtCustContact.Text + "', '" + Session["UserName"] + "', '" + DateTime.Now + "'," +
            " '" + dTAmnt + "','" + this.txtTC.Text + "'," +
            " '" + this.txtCash.Text + "','" + this.txtCardAmnt1.Text + "','" + this.txtCardAmnt2.Text + "'," +
            " '" + this.txtChequeNo.Text + "','" + this.txtChequeNo2.Text + "','" + this.ddlCardType1.SelectedItem.Text + "','" + this.ddlCardType2.SelectedItem.Text + "'," +
            " '" + this.txtBankName.Text + "','" + this.txtBankName2.Text + "','" + this.txtSecurityCode.Text + "','" + this.txtSecurityCode2.Text + "'," +
            " '" + this.txtApprovalCode1.Text + "','" + this.txtApprovalCode2.Text + "',1," +
            " '" + this.txtRefBy.Text + "','" + this.txtJobID.Text + "'," +
            " '" + this.ddlEntity.SelectedItem.Value + "','" + this.txtNote.Text + "'," +
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
        cmd = new SqlCommand(sSql, conn);
        conn.Open();
        dr = cmd.ExecuteReader();
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

        //------------------------------------------------------------------------------------------

        //lblSaveMessage.Text = "Save Data Successfully.";

        //ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert",
                        //"<script>alert('" + "Save Successfully." + "');</script>", false);

        //Session["sBillNo"] = this.txtCHNo.Text;
        //Response.Redirect("Sales_Bill_Print.aspx");

        //-------------------------------------------------------------------------------------------
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
        //----------------------------------------------------------------------------------------

        return;

    }



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

        txtNote.Text = "";

        //LOAD AUTO BILL NUMBER
        //fnLoadAutoBillNo();

        txtCHNo.Focus();

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

            //double value5 = Convert.ToDouble(e.Row.Cells[5].Text);
            //e.Row.Cells[5].Text = value5.ToString("0");

            double value6 = Convert.ToDouble(e.Row.Cells[6].Text);
            e.Row.Cells[6].Text = value6.ToString("0");

            double value9 = Convert.ToDouble(e.Row.Cells[9].Text);
            e.Row.Cells[9].Text = value9.ToString("0");

            double value10 = Convert.ToDouble(e.Row.Cells[10].Text);
            e.Row.Cells[10].Text = value10.ToString("0");

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
            e.Row.Cells[5].Text = runningTotalTP.ToString("0,0", CultureInfo.InvariantCulture);
            e.Row.Cells[6].Text = runningTotalDis.ToString("0,0", CultureInfo.InvariantCulture);
            e.Row.Cells[9].Text = runningTotalWith.ToString("0,0", CultureInfo.InvariantCulture);
            e.Row.Cells[10].Text = runningTotal.ToString("0,0", CultureInfo.InvariantCulture);
            //this.lblNetAmnt.Text = runningTotal.ToString("0,0", CultureInfo.InvariantCulture);
            this.lblNetAmnt.Text = runningTotal.ToString();
            this.txtNetAmnt.Text = runningTotal.ToString();

            //this.txtPay.Text = runningTotal.ToString();
            //this.txtCash.Text = runningTotal.ToString();

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
        //sSql = "SELECT ProductID,ProdName,UnitPrice,Model,Code FROM Product" +
        //    " WHERE Code='" + this.txtCode.Text + "'";

        sSql = "SELECT ProductID,ProdName,UnitPrice,Model,Code,";
        sSql = sSql + " IncentiveType,BLIPAmnt,IncentiveAmnt,GetIncentive,WPPrice,BLIPofWP,WPIncentive,ISNULL(WPMinQty,0) AS WPMinQty";
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
        double CampPrice = 0;
        string sSql = "";

        sSql = "";
        //sSql = "SELECT ProductID,ProdName,UnitPrice,Model,Code FROM Product" +
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

}
