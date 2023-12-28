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

public partial class Dhaka_sales_delivery : System.Web.UI.Page
{
    SqlConnection conn = DBConnection.GetConnection();
    SqlConnection conn1 = DBConnection.GetConnection();
    SqlConnection _connStr = DBConnection.GetConnection();
    int iMRSRID = 0;
    int iMRSRIDdms = 0;
    int iMRSRIDdms2 = 0;
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

            //LOAD PRODUCT GROUP & MODEL
            LoadProductGroupList();
            //LoadDropDownList();
            //LOAD CTP
            LoadDropDownList_CTP();
            LoadDropDownList_Zone();
            fnLoadTC();

            //LOAD TERMS & CONDITIONS
            this.fnClaimList();
            txtDate.Text = DateTime.Today.ToString("MM/dd/yyyy");

        }
        else
        {
            dt = (DataTable)ViewState["dt"];
        }
        ViewState["dt"] = dt;


    }

    //LOAD PRODUCT GROUP IN DROPDOWN LIST
    protected void LoadProductGroupList()
    {
        if (System.Convert.ToInt32(Session["Vis"]) == 0)
        {
            Response.Redirect("../Account/Login.aspx");
        }

        SqlConnection conn = DBConnection.GetConnection();
        String strQuery = "select GroupSL, GroupName from Product" +
            " where GroupSL not in (2,3,4,7,8,9,10,11,12,13,14,15,20,21,22,26,27,31,32,33,34,38,40,41,43,52,53,54,55,56,57,59," +
            "60,61,71,81,82,90,91,92,94,95,103,105,106,108,109,110,111,112)" +
            "GROUP BY GroupSL, GroupName ORDER BY GroupName";
        // 1,19,50,58,24,
        // 62,63,64,65,66,67,101,102,

        SqlCommand cmd = new SqlCommand(strQuery, conn);
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = strQuery;
        cmd.Connection = conn;
        try
        {
            conn.Open();
            ddlProductGroup.DataSource = cmd.ExecuteReader();
            ddlProductGroup.DataTextField = "GroupName";
            ddlProductGroup.DataValueField = "GroupSL";
            ddlProductGroup.DataBind();

            //Add blank item at index 0.
            ddlProductGroup.Items.Insert(0, new ListItem("", ""));


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

        SqlConnection con = DBConnectionDSM.GetConnection();
        //con.Open();

        int xMax = 0;
        string sAutoNo = "";
        string sSql = "";
        sSql = "SELECT ISNULL(MAX(RIGHT(MRSRCode, 5)), 0) AS BillNo" +
            " FROM dbo.MRSRMaster" +
            " WHERE (LEFT(MRSRCode, 12) = '" + "" + Session["sZoneCode"] + "/" + DateTime.Now.ToString("yyyy") + "/" + "')" +
            " AND (TrType=3)";
        //AND OnLineSales=1
        SqlCommand cmd = new SqlCommand(sSql, con);
        con.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        try
        {
            if (dr.Read())
            {
                //xMax = dr["JobNo"].ToString();
                xMax = Convert.ToInt32(dr["BillNo"]) + 1;
                sAutoNo = "" + Session["sZoneCode"] + "/" + DateTime.Now.ToString("yyyy") + "/" + xMax.ToString("00000");
                txtCHNo.Text = sAutoNo;
            }
            else
            {
                xMax = Convert.ToInt32(dr["BillNo"]) + 1;
                sAutoNo = "" + Session["sZoneCode"] + "/" + DateTime.Now.ToString("yyyy") + "/" + xMax.ToString("00000");
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

    protected void fnLoadAutoBillNo_ChallanNo()
    {
        SqlConnection con = DBConnection.GetConnection();
        //con.Open();

        int xMax = 0;
        string sAutoNo = "";
        string sSql = "";
        sSql = "SELECT ISNULL(MAX(RIGHT(MRSRCode, 3)), 0) AS BillNo" +
            " FROM dbo.MRSRMaster" +
            " WHERE (LEFT(MRSRCode, 7) = '" + "" + DateTime.Now.ToString("dd") + "" + DateTime.Now.ToString("MM") + "" + DateTime.Now.ToString("yy") + "-" + "')" +
            " AND (TrType=2)";
        //AND OnLineSales=1
        SqlCommand cmd = new SqlCommand(sSql, con);
        con.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        //try
        //{
        if (dr.Read())
        {
            //xMax = dr["JobNo"].ToString();
            xMax = Convert.ToInt32(dr["BillNo"]) + 1;
            sAutoNo = "" + DateTime.Now.ToString("dd") + "" + DateTime.Now.ToString("MM") + "" + DateTime.Now.ToString("yy") + "-" + xMax.ToString("000");
            txtCHNo2.Text = sAutoNo;
        }
        else
        {
            xMax = Convert.ToInt32(dr["BillNo"]) + 1;
            sAutoNo = "" + DateTime.Now.ToString("dd") + "" + DateTime.Now.ToString("MM") + "" + DateTime.Now.ToString("yy") + "-" + xMax.ToString("000");
            txtCHNo2.Text = sAutoNo;
        }
        //}
        //catch (InvalidCastException err)
        //{
        //    throw (err);
        //}
        //finally
        //{
        //    dr.Dispose();
        //    dr.Close();
        //    con.Close();
        //}

        con.Close();
    }

    //LOAD PRODUCT IN DROPDOWN LIST
    protected void LoadDropDownList()
    {
        if (System.Convert.ToInt32(Session["Vis"]) == 0)
        {
            Response.Redirect("../Account/Login.aspx");
        }
        int groupSL = 0;
        if (!string.IsNullOrEmpty(ddlProductGroup.SelectedValue))
        {
            groupSL = Convert.ToInt16(ddlProductGroup.SelectedValue);
        }
        SqlConnection conn = DBConnection.GetConnection();
        String strQuery = "select Model from Product WHERE Discontinue='No' and GroupSL in (113,107, " + groupSL + ") Order By Model";
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

    //LOAD Dealer by Zone IN DROPDOWN LIST
    protected void LoadDropDownList_Zone()
    {
        try
        {
            SqlConnection conn = DBConnectionDSM.GetConnection();

            String gSql = "";
            gSql = "select CategoryID,Code,CatName from Zone";
            gSql = gSql + " Where SubStoreCode='" + Session["sStoreCode"].ToString() + "'";
            //gSql = gSql + " or CategoryID in(69) ";
            if (Session["sBrCode"].ToString() == "907001")
            {
                gSql = gSql + "  or CategoryID in(69)";
            }


            SqlCommand cmd = new SqlCommand(gSql, conn);

            cmd.CommandType = CommandType.Text;
            cmd.CommandText = gSql;
            cmd.Connection = conn;

            conn.Open();
            ddlZone.DataSource = cmd.ExecuteReader();
            ddlZone.DataTextField = "CatName";
            ddlZone.DataValueField = "CategoryID";
            ddlZone.DataBind();

            //Add blank item at index 0.
            ddlZone.Items.Insert(0, new ListItem("", "0"));
        }
        catch (InvalidCastException err)
        {
            throw (err);
        }
        finally
        {
            conn.Close();
        }
    }

    //LOAD CTP IN DROPDOWN LIST
    protected void LoadDropDownList_CTP()
    {
        SqlConnection conn = DBConnection.GetConnection();

        String strQuery = "Select EID,eName from Entity ";
        strQuery = strQuery + " WHERE (ActiveDeactive = 1) AND (EntityType = 'Dealer')";
        strQuery = strQuery + " ORDER BY eName";
        SqlCommand cmd = new SqlCommand(strQuery, conn);
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

    protected void ddlProductGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadDropDownList();

        ddlProductGroup.Enabled = false;
    }

    //LOAD Dealer Zonewise IN DROPDOWN LIST
    protected void ddlZone_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            SqlConnection conn = DBConnectionDSM.GetConnection();

            // Bind Dealer Dropdown

            String strQuery = "SELECT CategoryID,DAID,Name,ZoneName FROM VW_Delear_Info ";
            strQuery = strQuery + " WHERE (Discontinue = 'No') AND (CategoryID='" + ddlZone.SelectedItem.Value + "')";
            strQuery = strQuery + " ORDER BY Name";

            SqlCommand cmd = new SqlCommand(strQuery, conn);
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = strQuery;
            cmd.Connection = conn;

            conn.Open();
            SqlDataReader drDealer = cmd.ExecuteReader();
            ddlDealerName.DataSource = drDealer;
            ddlDealerName.DataTextField = "Name";
            ddlDealerName.DataValueField = "DAID";
            ddlDealerName.DataBind();

            //Add blank item at index 0.
            ddlDealerName.Items.Insert(0, new ListItem("", "0"));
            this.txtCode.Text = "";
            lblDealerID.Text = "0";
            this.txtCustContact.Text = "";
            this.txtEmail.Text = "";
            this.txtOutstanding.Text = "0";
            this.txtCreditLimit.Text = "0";

            drDealer.Dispose();
            drDealer.Close();
            conn.Close();

            // Get Zonel code
            string zonalSQL = "SELECT top 1 CategoryID,ZonalCode,DAID, DMSEID, Name,ZoneName FROM VW_Delear_Info WHERE (Discontinue = 'No') AND (CategoryID='" + ddlZone.SelectedItem.Value + "')";
            SqlCommand cmd1 = new SqlCommand(zonalSQL, conn);
            conn.Open();
            SqlDataReader dr = cmd1.ExecuteReader();
            if (dr.Read())
            {
                Session["sZoneCode"] = dr["ZonalCode"];
                Session["sZoneEID"] = dr["DMSEID"];
                Session["sDealerMktEID"] = dr["DAID"];
            }

            //LOAD AUTO BILL NO.
            fnLoadAutoBillNo();

            ddlEntity.SelectedItem.Text = Session["eName"].ToString();
            ddlEntity.SelectedItem.Value = Session["sDealerMktEID"].ToString();

            dr.Dispose();
            dr.Close();
        }
        catch (InvalidCastException err)
        {
            throw (err);
        }
        finally
        {
            conn.Close();
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

        ddlContinents.SelectedItem.Text = "";
        txtCode.Text = "";
        txtProdDesc.Text = "";
        txtProdID.Text = "0";
        txtModel.Text = "";
        //txtModel.Focus();

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
        double dBLIP = 0;
        double CampPrice = 0;
        string sSql = "";

        sSql = "";
        sSql = "SELECT ProductID,ProdName,UnitPrice,DealerPrice,Model,Code,";
        sSql = sSql + " IncentiveType,BLIPAmnt,IncentiveAmnt,";
        sSql = sSql + " GetIncentive, ISNULL(WPPrice,0) AS WPPrice, ISNULL(BLIPofWP,0) AS BLIPofWP,";
        sSql = sSql + " ISNULL(WPIncentive,0) AS WPIncentive, ISNULL(WPMinQty,0) AS WPMinQty";
        sSql = sSql + " FROM Product";
        sSql = sSql + " WHERE Model='" + this.ddlContinents.SelectedValue + "'";

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
                this.txtUP.Text = dr["UnitPrice"].ToString();
                this.txtCP.Text = dr["DealerPrice"].ToString();

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

                this.txtCP.Text = "0";

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

    //FINALLY SAVE DATA
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (System.Convert.ToInt32(Session["Vis"]) == 0)
        {
            Response.Redirect("../Account/Login.aspx");
        }

        SqlConnection conn = DBConnectionDSM.GetConnection();
        SqlConnection connSMS = DBConnectionSMS.GetConnection();
        SqlConnection connDMS = DBConnection.GetConnection();

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

        //REF. CHALLAN NUMBER       
        //if (txtRefChNo.Text == "")
        //{
        //    PopupMessage("Please enter Reference Challan #.", btnSave);
        //    txtRefChNo.Focus();
        //    return;
        //}

        //CHALLAN DATE VALIDATION        
        if (txtDate.Text == "")
        {
            PopupMessage("Please enter Date.", btnSave);
            txtDate.Focus();
            return;
        }

        if (this.txtDealerCode.Text.Length == 0)
        {
            PopupMessage("Please Select Dealer Name.", btnSave);
            txtDealerCode.Focus();
            return;
        }

        //GRIDVIEW DATA VALIDATION
        int totalRowsCount = gvUsers.Rows.Count;
        if (totalRowsCount == 0)
        {
            PopupMessage("There is no product in list. Please add product.", btnSave);
            return;
        }
        SqlDateTime sqldatenull;
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
            " AND TrType=3";
        SqlCommand cmdd = new SqlCommand(sSql, conn);
        conn.Open();
        SqlDataReader drd = cmdd.ExecuteReader();
        try
        {
            if (drd.Read())
            {
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

        int iTrType = 0;
        if (Session["sBrId"].ToString() == null)
        {
            iTrType = 6;
        }
        else if (Session["sBrId"].ToString() == "370")
        {
            iTrType = 3;
        }
        else
        {
            iTrType = 6;
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


        string ssNote = "";
        ssNote = ddlDealerName.SelectedItem.Text + ", (" + txtDealerCode.Text + ")";

        DateTime aDate = DateTime.Now;

        fnLoadAutoBillNo();
        fnLoadAutoBillNo_ChallanNo(); //LOAD AUTO CHALLAN NUMBER

        Session["sBillNo"] = this.txtCHNo.Text;


        //SAVE DATA IN MASTER TABLE OF DEALER
        sSql = "";
        sSql = "INSERT INTO MRSRMaster(MRSRCode,TDate,TrType,ProductGroup," +
               "InvoiceNo,InSource,OutSource," +
               "PayAmnt,DueAmnt,PayMode," +
               "Customer,UserID,EntryDate," +
               "NetSalesAmnt,TermsCondition," +
               "CashAmnt,CardAmnt1,CardAmnt2," +
               "CardNo1,CardNo2,CardType1,CardType2," +
               "Bank1,Bank2,SecurityCode,SecurityCode2," +
               "AppovalCode1,AppovalCode2,OnLineSales," +
               "tCreditLimit,tOutStanding," +
               "Authorby,Issby,DeliveryFrom,Remarks,Tag,RefCHNo,POCode,SaleDeclar" +
               " )" +
            " Values ('" + this.txtCHNo.Text + "','" + tDate + "','3','" + this.ddlProductGroup.SelectedItem.Text + "'," +
            " '" + this.txtCHNo.Text + "','" + lblDealerID.Text + "','" + this.ddlZone.SelectedItem.Value + "'," +
            " '" + dTPay + "','" + dTDue + "','" + this.ddlPayType.Text + "'," +
            " '" + this.txtCustContact.Text + "', '" + Session["UserName"] + "', '" + DateTime.Now + "'," +
            " '" + dTAmnt + "','" + this.txtTC.Text.Replace("'", "''") + "'," +
            " '" + this.txtCash.Text + "','" + this.txtCardAmnt1.Text + "','" + this.txtCardAmnt2.Text + "'," +
            " '" + this.txtChequeNo.Text + "','" + this.txtChequeNo2.Text + "','" + this.ddlCardType1.SelectedItem.Text + "','" + this.ddlCardType2.SelectedItem.Text + "'," +
            " '" + this.txtBankName.Text + "','" + this.txtBankName2.Text + "','" + this.txtSecurityCode.Text + "','" + this.txtSecurityCode2.Text + "'," +
            " '" + this.txtApprovalCode1.Text + "','" + this.txtApprovalCode2.Text + "',1," +
            " '" + this.txtCreditLimit.Text + "','" + this.txtOutstanding.Text + "'," +
            " '" + this.txtRefBy.Text + "','" + this.txtJobID.Text + "'," +
            " '" + this.ddlZone.SelectedItem.Value + "','" + this.txtNote.Text.Replace("'", "''") + "'," +
            " '" + iDelTag + "','" + this.txtCHNo2.Text + "','" + txtOrderNo.Text + "',2" +
        " )";
        //" CAST(" + this.lblNetAmnt.Text + " AS Numeric)";        
        // " )";
        SqlCommand cmd = new SqlCommand(sSql, conn);
        conn.Open();
        cmd.ExecuteNonQuery();
        conn.Close();
        //lblMessage.Text = "Done";


        //SAVE DATA IN MASTER TABLE OF DMS (MARKET TO CUSTOMER)
        sSql = "";
        sSql = "INSERT INTO MRSRMaster(MRSRCode,TDate,TrType,ProductGroup," +
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
            " Values ('" + this.txtCHNo.Text + "','" + tDate + "','6','" + this.ddlProductGroup.SelectedItem.Text + "'," +
            //" '" + this.txtCHNo.Text + "','230','" + Session["sDealerMktEID"] + "'," +
            " '" + this.txtCHNo.Text + "','230','" + Session["sZoneEID"] + "'," +
            " '" + dTPay + "','" + dTDue + "','" + this.ddlPayType.Text + "'," +
            " '" + this.txtCustContact.Text + "', '" + Session["UserName"] + "', '" + DateTime.Now + "'," +
            " '" + dTAmnt + "','" + this.txtTC.Text.Replace("'", "''") + "'," +
            " '" + this.txtCash.Text + "','" + this.txtCardAmnt1.Text + "','" + this.txtCardAmnt2.Text + "'," +
            " '" + this.txtChequeNo.Text + "','" + this.txtChequeNo2.Text + "','" + this.ddlCardType1.SelectedItem.Text + "','" + this.ddlCardType2.SelectedItem.Text + "'," +
            " '" + this.txtBankName.Text + "','" + this.txtBankName2.Text + "','" + this.txtSecurityCode.Text + "','" + this.txtSecurityCode2.Text + "'," +
            " '" + this.txtApprovalCode1.Text + "','" + this.txtApprovalCode2.Text + "',1," +
            " '" + this.txtRefBy.Text + "','" + this.txtJobID.Text + "'," +
            " '" + Session["sZoneEID"] + "','" + this.txtNote.Text.Replace("'", "''") + "'," +
            " '" + iDelTag + "','" + this.txtCHNo2.Text + "','" + txtOrderNo.Text + "'" +
        " )";
        //" CAST(" + this.lblNetAmnt.Text + " AS Numeric)";  
        // " )";
        SqlCommand cmdDMS = new SqlCommand(sSql, connDMS);
        connDMS.Open();
        cmdDMS.ExecuteNonQuery();
        connDMS.Close();
        //lblMessage.Text = "Done";

        //----------------------------------
        if (Session["sBrId"].ToString() == "370")
        {

            //SAVE DATA IN MASTER TABLE OF DMS (ISSUE: CIDD TO DHAKA MARKET)
            sSql = "";
            sSql = "INSERT INTO MRSRMaster(MRSRCode,TDate,TrType,ProductGroup," +
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
                " Values ('" + this.txtCHNo2.Text + "','" + tDate + "','2','" + this.ddlProductGroup.SelectedItem.Text + "'," +
                " '" + this.txtCHNo2.Text + "','" + Session["sZoneEID"] + "','370'," +
                " '" + dTPay + "','" + dTDue + "','" + this.ddlPayType.Text + "'," +
                " '" + this.txtCustContact.Text + "', '" + Session["UserName"] + "', '" + DateTime.Now + "'," +
                " '" + dTAmnt + "','" + this.txtTC.Text.Replace("'", "''") + "'," +
                " '" + this.txtCash.Text + "','" + this.txtCardAmnt1.Text + "','" + this.txtCardAmnt2.Text + "'," +
                " '" + this.txtChequeNo.Text + "','" + this.txtChequeNo2.Text + "','" + this.ddlCardType1.SelectedItem.Text + "','" + this.ddlCardType2.SelectedItem.Text + "'," +
                " '" + this.txtBankName.Text + "','" + this.txtBankName2.Text + "','" + this.txtSecurityCode.Text + "','" + this.txtSecurityCode2.Text + "'," +
                " '" + this.txtApprovalCode1.Text + "','" + this.txtApprovalCode2.Text + "',1," +
                " '" + this.txtRefBy.Text + "','" + this.txtJobID.Text + "'," +
                " '" + Session["sZoneEID"] + "','" + ssNote + ' ' + this.txtNote.Text.Replace("'", "''") + "'," +
                " '" + iDelTag + "','" + this.txtCHNo.Text + "','" + txtOrderNo.Text + "'" +
            " )";
            //" CAST(" + this.lblNetAmnt.Text + " AS Numeric)";  
            // " )";
            cmdDMS = new SqlCommand(sSql, connDMS);
            connDMS.Open();
            cmdDMS.ExecuteNonQuery();
            connDMS.Close();
            //lblMessage.Text = "Done";
        }


        //************************************************************************************************
        //RETRIVE MASTER ID FROM DEALER DB        
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

        //RETRIVE MASTER ID FROM DMS        
        sSql = "";
        sSql = "SELECT MRSRMID FROM MRSRMaster" +
            " WHERE MRSRCode='" + this.txtCHNo.Text + "'" +
            //" WHERE MRSRCode='" + this.txtMRSR.Text + 'S' + "'" +
            " AND TrType='6'";
        cmdDMS = new SqlCommand(sSql, connDMS);
        connDMS.Open();
        dr = cmdDMS.ExecuteReader();
        if (dr.Read())
        {
            iMRSRIDdms = Convert.ToInt32(dr["MRSRMID"].ToString());
            //Session["sBrId"] = Convert.ToInt16(dr["EID"].ToString());
        }
        dr.Dispose();
        dr.Close();
        connDMS.Close();

        //RETRIVE MASTER ID FROM DMS (CIDD - DHK MKT)       
        sSql = "";
        sSql = "SELECT MRSRMID FROM MRSRMaster" +
            " WHERE MRSRCode='" + this.txtCHNo2.Text + "'" +
            //" WHERE MRSRCode='" + this.txtMRSR.Text + 'S' + "'" +
            " AND TrType='2'";
        cmdDMS = new SqlCommand(sSql, connDMS);
        connDMS.Open();
        dr = cmdDMS.ExecuteReader();
        if (dr.Read())
        {
            iMRSRIDdms2 = Convert.ToInt32(dr["MRSRMID"].ToString());
            //Session["sBrId"] = Convert.ToInt16(dr["EID"].ToString());
        }
        dr.Dispose();
        dr.Close();
        connDMS.Close();
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


            double dIncAmnt = 0;
            double dTAmnt1 = Convert.ToDouble(g1.Cells[3].Text) * Convert.ToDouble(g1.Cells[4].Text);
            double dTBLAmnt1 = Convert.ToDouble(sBLAMNT) * Convert.ToDouble(g1.Cells[4].Text);
            if (dTBLAmnt1 > 0)
            {
                dIncAmnt = dTAmnt1 - dTBLAmnt1;
            }

            string gSql = "";

            //SAVE IN DEALER DATABASE
            gSql = "INSERT INTO MRSRDetails(MRSRMID,ProductID,Qty," +
                 " MRP,UnitPrice,TotalAmnt,DiscountAmnt," +
                 " SLNO,ProdRemarks,DisCode,DisRef," +
                 " WithAdjAmnt,RetPrice,NetAmnt," +
                 " BLIPAmnt,IncentiveAmnt,IncentiveType," +
                 " CustShowPrice,InEID,OutEID)" +
                 " VALUES('" + iMRSRID + "','" + g1.Cells[0].Text + "','" + '-' + g1.Cells[4].Text + "'," +
                 " '" + g1.Cells[2].Text + "','" + g1.Cells[3].Text + "','" + g1.Cells[5].Text + "','" + g1.Cells[6].Text + "'," +
                 " '" + sProdSL + "','" + sRemarks + "','" + sDisCode + "','" + sDisRef + "'," +
                 " '" + g1.Cells[9].Text + "','" + g1.Cells[2].Text + "','" + g1.Cells[10].Text + "'," +
                 " '" + g1.Cells[13].Text + "','" + dIncAmnt + "','" + sIncType + "'," +
                 " '" + g1.Cells[16].Text + "','" + lblDealerID.Text + "','" + ddlZone.SelectedItem.Value + "')";
            SqlCommand cmdIns = new SqlCommand(gSql, conn);

            conn.Open();
            cmdIns.ExecuteNonQuery();
            conn.Close();


            //SAVE IN DMS DATABASE
            gSql = "";
            gSql = "INSERT INTO MRSRDetails(MRSRMID,ProductID,Qty," +
                 " MRP,UnitPrice,TotalAmnt,DiscountAmnt," +
                 " SLNO,ProdRemarks,DisCode,DisRef," +
                 " WithAdjAmnt,RetPrice,NetAmnt," +
                 " BLIPAmnt,IncentiveAmnt,IncentiveType," +
                 " CustShowPrice,InEID,OutEID)" +
                 " VALUES('" + iMRSRIDdms + "','" + g1.Cells[0].Text + "','" + '-' + g1.Cells[4].Text + "'," +
                 " '" + g1.Cells[2].Text + "','" + g1.Cells[3].Text + "','" + g1.Cells[5].Text + "','" + g1.Cells[6].Text + "'," +
                 " '" + sProdSL + "','" + sRemarks + "','" + sDisCode + "','" + sDisRef + "'," +
                 " '" + g1.Cells[9].Text + "','" + g1.Cells[2].Text + "','" + g1.Cells[10].Text + "'," +
                 " '" + g1.Cells[13].Text + "','" + dIncAmnt + "','" + sIncType + "'," +
                 " '" + g1.Cells[16].Text + "','230','" + Session["sZoneEID"] + "')";
            cmdIns = new SqlCommand(gSql, connDMS);

            connDMS.Open();
            cmdIns.ExecuteNonQuery();
            connDMS.Close();

            //SAVE IN DMS DATABASE (CIDD-DHAKA.MKT)
            gSql = "";
            gSql = "INSERT INTO MRSRDetails(MRSRMID,ProductID,Qty," +
                 " MRP,UnitPrice,TotalAmnt,DiscountAmnt," +
                 " SLNO,ProdRemarks,DisCode,DisRef," +
                 " WithAdjAmnt,RetPrice,NetAmnt," +
                 " BLIPAmnt,IncentiveAmnt,IncentiveType," +
                 " CustShowPrice,InEID,OutEID)" +
                 " VALUES('" + iMRSRIDdms2 + "','" + g1.Cells[0].Text + "','" + g1.Cells[4].Text + "'," +
                 " '" + g1.Cells[2].Text + "','" + g1.Cells[3].Text + "','" + g1.Cells[5].Text + "','" + g1.Cells[6].Text + "'," +
                 " '" + sProdSL + "','" + sRemarks + "','" + sDisCode + "','" + sDisRef + "'," +
                 " '" + g1.Cells[9].Text + "','" + g1.Cells[2].Text + "','" + g1.Cells[10].Text + "'," +
                 " '" + g1.Cells[13].Text + "','" + dIncAmnt + "','" + sIncType + "'," +
                 " '" + g1.Cells[16].Text + "','370','" + Session["sZoneEID"] + "')";
            cmdIns = new SqlCommand(gSql, connDMS);

            connDMS.Open();
            cmdIns.ExecuteNonQuery();
            connDMS.Close();

        }
        //}
        //catch
        //{
        //    //
        //}

        //######################################################################################
        //SAVE DATA IN HISTORY MASTER TABLE
        //try
        //{
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
            " '" + this.txtCHNo.Text + "','" + lblDealerID.Text + "','" + ddlZone.SelectedItem.Value + "'," +
            " '" + dTPay + "','" + dTDue + "','" + this.ddlPayType.Text + "'," +
            " '" + this.txtCustContact.Text + "', '" + Session["UserName"] + "', '" + aDate.ToString("MM/dd/yyyy hh:mm tt") + "'," +
            " '" + dTAmnt + "','" + this.txtTC.Text.Replace("'", "''") + "'," +
            " '" + this.txtCash.Text + "','" + this.txtCardAmnt1.Text + "','" + this.txtCardAmnt2.Text + "'," +
            " '" + this.txtChequeNo.Text + "','" + this.txtChequeNo2.Text + "','" + this.ddlCardType1.SelectedItem.Text + "','" + this.ddlCardType2.SelectedItem.Text + "'," +
            " '" + this.txtBankName.Text + "','" + this.txtBankName2.Text + "','" + this.txtSecurityCode.Text + "','" + this.txtSecurityCode2.Text + "'," +
            " '" + this.txtApprovalCode1.Text + "','" + this.txtApprovalCode2.Text + "',1," +
            " '" + this.txtRefBy.Text + "','" + this.txtJobID.Text + "'," +
            " '" + ddlZone.SelectedItem.Value + "','" + this.txtNote.Text.Replace("'", "''") + "'," +
            " '" + iDelTag + "','" + this.txtCHNo2.Text + "','" + txtOrderNo.Text + "'" +
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

        //}
        //catch
        //{
        //    //
        //}

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
        if (txtCustContact.Text != "")
        {
            string smsText = "";
            double dOutstanding = Convert.ToDouble(txtNetAmnt.Text) + Convert.ToDouble(txtOutstanding.Text);

            //smsText = "Dear Sir (Code:" + txtDealerCode.Text + "),\n";
            smsText = "Dear Sir,\n";
            smsText = smsText + "Thanks for new Invoice.\n";
            smsText = smsText + "Invoice# " + this.txtCHNo.Text + ".\n";
            //smsText = smsText + "Date: " + this.txtDate.Text + ".\n";
            smsText = smsText + "Inv.Amnt: " + this.txtNetAmnt.Text + ".\n";
            smsText = smsText + "Total Outstanding: " + dOutstanding + ".\n";
            //smsText = smsText + "Thank You.\n";
            smsText = smsText + "Sony-Rangs";

            sSql = "";
            sSql = "INSERT INTO tbSMS(ContactNo,SMSText,UserID,EntryDate,SMSSource)" +
                    " Values ('" + this.txtCustContact.Text + "','" + smsText + "'," +
                    " '" + Session["UserID"] + "','" + DateTime.Now + "'," +
                    " 'DSM'" +
                    " )";
            SqlCommand cmdSMS = new SqlCommand(sSql, connSMS);
            connSMS.Open();
            cmdSMS.ExecuteNonQuery();
            connSMS.Close();
        }
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

        Response.Redirect("Sales_Bill_Print.aspx");

        return;

    }


    //FUNCTION FOR SEND MAIL
    private void fnSendMail_Invoice()
    {
        SqlConnection conn = DBConnectionDSM.GetConnection();
        SqlConnection conn1 = DBConnectionDSM.GetConnection();

        SqlConnection connDMS = DBConnection.GetConnection();

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

        //LOAD ZONE INFORMATION
        string sSql = "";
        sSql = "SELECT EID, eName, EDesc, EntityType, EntityCode, ContactPerson,";
        sSql = sSql + " Desg, PhoneNo, EmailAdd, ContactNo";
        sSql = sSql + " FROM dbo.Entity";
        sSql = sSql + " WHERE EID='" + ddlZone.SelectedItem.Value + "'";

        //sSql = "SELECT dbo.Zone.CatName AS AreaName, dbo.Zone.ZoneType, dbo.Zone.CategoryID AS AreaID,";
        //sSql = sSql + " dbo.DelearInfo.DAID, dbo.DelearInfo.Code, dbo.DelearInfo.Name, dbo.DelearInfo.Address, ";
        //sSql = sSql + " dbo.DelearInfo.ContactNo, dbo.DelearInfo.ContactPerson, dbo.DelearInfo.EmailAdd";
        //sSql = sSql + " FROM  dbo.Zone INNER JOIN";
        //sSql = sSql + " dbo.DelearInfo ON dbo.Zone.CategoryID = dbo.DelearInfo.CategoryID";
        //sSql = sSql + " WHERE dbo.DelearInfo.Code='" + txtDealerCode.Text + "'";

        SqlCommand cmdC = new SqlCommand(sSql, connDMS);
        connDMS.Open();
        SqlDataReader drC = cmdC.ExecuteReader();
        if (drC.Read())
        {
            lblCTPName.Text = drC["eName"].ToString();
            lblCTPAdd.Text = drC["EDesc"].ToString();
            lblCTPEmail.Text = drC["EmailAdd"].ToString(); //ZONAL EMAIL
            lblCTPContact.Text = drC["PhoneNo"].ToString();
            if (drC["PhoneNo"].ToString().Length == 0)
            {
                lblCTPContact.Text = drC["ContactNo"].ToString();
            }
        }
        connDMS.Close();


        double dTOutStanding = Convert.ToDouble(txtNetAmnt.Text) + Convert.ToDouble(txtOutstanding.Text);

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
        mM2.CC.Add(new MailAddress("jahangir@rangs.com.bd"));
        mM2.CC.Add(new MailAddress("minto@rangs.com.bd"));
        //mM2.Bcc.Add(new MailAddress("zunayedqu10@gmail.com"));

        mM2.Subject = "Sony-Rangs Invoice No." + txtCHNo.Text + " ";
        //mM2.Body = "<h1>Order Details</h1>";
        mM2.Body = "<p>Dear Valued Dealer,</p>";
        mM2.Body = mM2.Body + "<p>Thank you for continue with us.<br/>";
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
        mM2.Body = mM2.Body + "<p><u>Dealer Details:</u><br/> Name: " + lblDealerName.Text + "<br/>";
        mM2.Body = mM2.Body + "Contact # " + lblDealerMobile.Text + "<br/>";
        mM2.Body = mM2.Body + "Email: " + lblDealerEmail.Text + "<br/>";
        mM2.Body = mM2.Body + "Address: " + lblDealerAdd.Text + "</p>";

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

        //mM2.Body = mM2.Body + "<b>Payment Details: </b><br/>";
        //if (txtCash.Text.Length > 0)
        //{
        //    mM2.Body = mM2.Body + "Cash: " + txtCash.Text + "<br/>";
        //}
        //if (txtCardAmnt1.Text.Length > 0)
        //{
        //    if (txtCardAmnt1.Text != "0")
        //    {
        //        mM2.Body = mM2.Body + "Card1: " + txtCardAmnt1.Text + "<br/>";
        //    }
        //}
        //if (txtCardAmnt2.Text.Length > 0)
        //{
        //    if (txtCardAmnt2.Text != "0")
        //    {
        //        mM2.Body = mM2.Body + "Card2 " + txtCardAmnt2.Text + "<br/>";
        //    }
        //}


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
        mM2.Body = mM2.Body + "Your Total Outstanding: " + dTOutStanding + ". <br/> ";
        mM2.Body = mM2.Body + "Please pay your outstanding on time.";
        mM2.Body = mM2.Body + "</p>";


        mM2.Body = mM2.Body + "<p>";
        mM2.Body = mM2.Body + "If you have any further enquiries <br/> please do contact us at - <br/> ";

        mM2.Body = mM2.Body + "<a href='mailto:marketing@rangs.com.bd'>marketing@rangs.com.bd</a>";
        mM2.Body = mM2.Body + "</p>";

        mM2.Body = mM2.Body + "<p>";
        mM2.Body = mM2.Body + "Kind Regards, <br/> ";
        mM2.Body = mM2.Body + "<a href='http://www.rangs.com.bd/'>Rangs Electronics Ltd.</a>";
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

        sSql = sSql + " WHERE (dbo.MRSRMaster.TrType = 6) AND (dbo.MRSRMaster.Issby = '" + txtJobID.Text + "')";
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
        sSql = sSql + " WHERE (dbo.MRSRMaster.TrType = 6) AND (dbo.MRSRMaster.Issby = '" + txtJobID.Text + "')";
        sSql = sSql + " AND (dbo.Product.GetIncentive = 1)";
        sSql = sSql + " AND (dbo.MRSRMaster.TDate >= '" + sDate + "') AND (dbo.MRSRMaster.TDate <= '" + eDate + "')";

        sSql = sSql + " GROUP BY dbo.MRSRMaster.TrType, dbo.MRSRMaster.Issby, dbo.Product.GetIncentive";
        //sSql = sSql + " HAVING  dbo.MRSRMaster.TrType = 6) AND (dbo.MRSRMaster.Issby > N'0')";

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
            e.Row.Cells[5].Text = runningTotalTP.ToString("0,0", CultureInfo.InvariantCulture);
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
    }


    //LOAD CUSTOMER INFO
    protected void btnCustSearch_Click(object sender, ImageClickEventArgs e)
    {
        SqlConnection conn = DBConnectionDSM.GetConnection();

        if (txtCustContact.Text == "")
        {
            PopupMessage("Please enter Dealer Code #.", btnSave);
            txtDealerCode.Focus();
            return;
        }

        double UP = 0;
        double CampPrice = 0;
        string sSql = "";

        sSql = "";
        sSql = "SELECT CategoryID,DAID,Code,Name,ZoneName,Address, ContactNo, EmailAdd FROM VW_Delear_Info ";
        sSql = sSql + " WHERE (Code = '" + txtDealerCode.Text + "') ";
        sSql = sSql + " AND (CategoryID='" + ddlZone.SelectedItem.Value + "')";

        SqlCommand cmd = new SqlCommand(sSql, conn);
        conn.Open();
        SqlDataReader dr = cmd.ExecuteReader();

        if (dr.Read())
        {
            this.ddlDealerName.SelectedItem.Text = dr["Name"].ToString();
            this.ddlDealerName.SelectedItem.Value = dr["DAID"].ToString();
            //this.txtDealerCode.Text = dr["Code"].ToString();
            this.lblDealerID.Text = dr["DAID"].ToString();
            this.lblDealerCode.Text = dr["Code"].ToString();
            this.lblDealerName.Text = dr["Name"].ToString();
            this.lblDealerAdd.Text = dr["Address"].ToString();
            this.lblDealerMobile.Text = dr["ContactNo"].ToString();
            this.lblDealerEmail.Text = dr["EmailAdd"].ToString();

            this.txtCustContact.Text = dr["ContactNo"].ToString();
            this.txtEmail.Text = dr["EmailAdd"].ToString();
            //this.txtProdDesc.Text = dr["ProdName"].ToString();
        }
        else
        {
            this.txtCode.Text = "";
            lblDealerID.Text = "0";
            this.txtCustContact.Text = "";
            this.txtEmail.Text = "";
            //this.txtCreditLimit.Text = "0";
            //this.txtOutstanding.Text = "0";

        }
        dr.Dispose();
        dr.Close();
        conn.Close();


        //LOAD CREDIT LIMIT
        fnLoadCreditLimit();

        //LOAD STATEMENT/OUTSTANDING
        fnLoadOutStanding();
        double dOutStanding = Convert.ToDouble(lblOBSales.Text) - Convert.ToDouble(lblOBCollection.Text) + Convert.ToDouble(lblOBDis.Text) - Convert.ToDouble(lblOBWith.Text);
        txtOutstanding.Text = Convert.ToString(dOutStanding);

    }


    protected void txtModel_TextChanged(object sender, EventArgs e)
    {
        SqlConnection conn = DBConnection.GetConnection();
        double UP = 0;
        double dBLIP = 0;
        double CampPrice = 0;
        string sSql = "";

        sSql = "";
        sSql = "SELECT ProductID,ProdName,UnitPrice,DealerPrice,Model,Code,";
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
                this.txtUP.Text = dr["UnitPrice"].ToString();
                this.txtCP.Text = dr["DealerPrice"].ToString();

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

                this.txtCP.Text = "0";

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
        sSql = sSql + " AND (CategoryID='" + ddlZone.SelectedItem.Value + "')";

        SqlCommand cmd = new SqlCommand(sSql, conn);
        conn.Open();
        SqlDataReader dr = cmd.ExecuteReader();

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
            this.txtEmail.Text = dr["EmailAdd"].ToString();
        }
        else
        {
            this.txtCode.Text = "";
            lblDealerID.Text = "0";
            this.txtCustContact.Text = "";
            this.txtEmail.Text = "";
            //this.txtCreditLimit.Text = "0";
            //this.txtOutstanding.Text = "0";

        }
        dr.Dispose();
        dr.Close();
        conn.Close();

        //LOAD CREDIT LIMIT
        fnLoadCreditLimit();

        //LOAD STATEMENT/OUTSTANDING
        fnLoadOutStanding();
        double dOutStanding = Convert.ToDouble(lblOBSales.Text) - Convert.ToDouble(lblOBCollection.Text) + Convert.ToDouble(lblOBDis.Text) - Convert.ToDouble(lblOBWith.Text);
        txtOutstanding.Text = Convert.ToString(dOutStanding);

    }


    //FUNCTION FOR LOAD CREDIT LIMIT
    private void fnLoadCreditLimit()
    {
        SqlConnection conn = DBConnectionDSM.GetConnection();

        txtCreditLimit.Text = "0";

        string gSQL = "";
        gSQL = "";
        gSQL = "SELECT TOP (1) dbo.tbCreditLimitYearly.TID, dbo.DelearInfo.DAID, dbo.DelearInfo.Code,";
        gSQL = gSQL + " dbo.DelearInfo.Name, dbo.DelearInfo.ContactNo,";
        gSQL = gSQL + " dbo.DelearInfo.EmailAdd, dbo.tbCreditLimitYearly.TAmount";
        gSQL = gSQL + " FROM dbo.tbCreditLimitYearly INNER JOIN";
        gSQL = gSQL + " dbo.DelearInfo ON dbo.tbCreditLimitYearly.DealerID = dbo.DelearInfo.DAID";
        gSQL = gSQL + " WHERE dbo.DelearInfo.Name = '" + ddlDealerName.SelectedItem.Text + "'";
        gSQL = gSQL + " AND dbo.DelearInfo.Code = '" + txtDealerCode.Text + "'";
        gSQL = gSQL + " ORDER BY dbo.tbCreditLimitYearly.TID DESC";

        SqlCommand cmd = new SqlCommand(gSQL, conn);
        conn.Open();
        SqlDataReader dr = cmd.ExecuteReader();

        if (dr.Read())
        {
            this.txtCreditLimit.Text = dr["TAmount"].ToString();
        }
        else
        {
            this.txtCreditLimit.Text = "0";
        }
        dr.Dispose();
        dr.Close();
        conn.Close();


    }

    //FUNCTION FOR LOAD STATEMENT/OUTSTANDING
    private void fnLoadOutStanding()
    {
        SqlConnection conn = DBConnectionDSM.GetConnection();

        txtOutstanding.Text = "0";

        string gSQL = "";
        //OPENING
        //LOAD SALES DATA
        gSQL = "";
        gSQL = "SELECT dbo.DelearInfo.Code, ";
        gSQL = gSQL + " dbo.DelearInfo.Name, dbo.DelearInfo.Address, ";
        gSQL = gSQL + " dbo.[Zone].CatName AS ZoneName, SUM(ISNULL(dbo.MRSRMaster.NetSalesAmnt,0)) AS NetSalesAmnt";
        gSQL = gSQL + " FROM dbo.DelearInfo INNER JOIN";
        gSQL = gSQL + " dbo.MRSRMaster ON dbo.DelearInfo.DAID = dbo.MRSRMaster.InSource INNER JOIN";
        gSQL = gSQL + " dbo.[Zone] ON dbo.DelearInfo.CategoryID = dbo.[Zone].CategoryID";

        gSQL = gSQL + " WHERE (dbo.MRSRMaster.TrType = 3)";

        gSQL = gSQL + " AND dbo.DelearInfo.Name='" + ddlDealerName.SelectedItem.Text + "'";
        gSQL = gSQL + " AND dbo.DelearInfo.Code = '" + txtDealerCode.Text + "'";
        gSQL = gSQL + " AND dbo.MRSRMaster.TDate<='" + txtDate.Text + "'";
        //'gSQL = gSQL & " AND dbo.MRSRMaster.TDate<='" & dtpEDate & "'"

        gSQL = gSQL + " GROUP BY  ";
        gSQL = gSQL + " dbo.DelearInfo.Code,";
        gSQL = gSQL + " dbo.DelearInfo.Name , dbo.DelearInfo.Address, dbo.[Zone].CatName ";
        SqlCommand cmd = new SqlCommand(gSQL, conn);
        conn.Open();
        SqlDataReader dr = cmd.ExecuteReader();

        if (dr.Read())
        {
            this.lblOBSales.Text = dr["NetSalesAmnt"].ToString();
        }
        else
        {
            this.lblOBSales.Text = "0";
        }
        dr.Dispose();
        dr.Close();
        conn.Close();
        //'-------------------------------------------------------------------------------------------


        //'-------------------------------------------------------------------------------------------
        //'LOAD OPENING COLLECTION
        gSQL = "";
        gSQL = "SELECT dbo.DepositAmnt.DelearID, dbo.DelearInfo.Code, dbo.DelearInfo.Name, ";
        gSQL = gSQL + " dbo.DelearInfo.Address, SUM(ISNULL(dbo.DepositAmnt.DepositAmnt, 0)) AS cAmount,";
        gSQL = gSQL + " dbo.Zone.CatName AS ZoneName";
        gSQL = gSQL + " FROM dbo.DepositAmnt INNER JOIN";
        gSQL = gSQL + " dbo.DelearInfo ON dbo.DepositAmnt.DelearID = dbo.DelearInfo.DAID INNER JOIN";
        gSQL = gSQL + " dbo.Zone ON dbo.DelearInfo.CategoryID = dbo.Zone.CategoryID";


        gSQL = gSQL + " WHERE dbo.DelearInfo.Name='" + ddlDealerName.SelectedItem.Text + "'";
        gSQL = gSQL + " AND dbo.DelearInfo.Code = '" + txtDealerCode.Text + "'";
        gSQL = gSQL + " AND dbo.DepositAmnt.CDate<='" + txtDate.Text + "'";

        gSQL = gSQL + " GROUP BY dbo.DepositAmnt.DelearID, ";
        gSQL = gSQL + " dbo.DelearInfo.Code, dbo.DelearInfo.Name, dbo.DelearInfo.Address,";
        gSQL = gSQL + " dbo.Zone.CatName";

        cmd = new SqlCommand(gSQL, conn);
        conn.Open();
        dr = cmd.ExecuteReader();

        if (dr.Read())
        {
            this.lblOBCollection.Text = dr["cAmount"].ToString();
        }
        else
        {
            this.lblOBCollection.Text = "0";
        }
        dr.Dispose();
        dr.Close();
        conn.Close();

        //'-------------------------------------------------------------------------------------------
        //'-------------------------------------------------------------------------------------------
        //'LOAD OPENING DISHONOUR
        gSQL = "";
        gSQL = "SELECT ZoneName,Name, SUM(cAmount) AS cAmount";
        gSQL = gSQL + " From dbo.VW_DishonourAmnt";
        gSQL = gSQL + " WHERE Name='" + ddlDealerName.SelectedItem.Text + "'";
        gSQL = gSQL + " AND Code = '" + txtDealerCode.Text + "'";
        gSQL = gSQL + " AND CDate<='" + txtDate.Text + "'";

        gSQL = gSQL + " GROUP BY ZoneName,Name";
        cmd = new SqlCommand(gSQL, conn);
        conn.Open();
        dr = cmd.ExecuteReader();

        if (dr.Read())
        {
            this.lblOBDis.Text = dr["cAmount"].ToString();
        }
        else
        {
            this.lblOBDis.Text = "0";
        }
        dr.Dispose();
        dr.Close();
        conn.Close();
        //'-------------------------------------------------------------------------------------------


        //'-------------------------------------------------------------------------------------------
        //'LOAD OPENING WITHDRAWN
        gSQL = "";
        gSQL = "SELECT dbo.DelearInfo.Code, ";
        gSQL = gSQL + " dbo.DelearInfo.Name, dbo.DelearInfo.Address, ";
        gSQL = gSQL + " dbo.[Zone].CatName AS ZoneName, SUM(ISNULL(dbo.MRSRMaster.NetSalesAmnt,0)) AS NetSalesAmnt";
        gSQL = gSQL + " FROM dbo.DelearInfo INNER JOIN";
        gSQL = gSQL + " dbo.MRSRMaster ON dbo.DelearInfo.DAID = dbo.MRSRMaster.OutSource INNER JOIN";
        gSQL = gSQL + " dbo.[Zone] ON dbo.DelearInfo.CategoryID = dbo.[Zone].CategoryID";

        gSQL = gSQL + " WHERE (dbo.MRSRMaster.TrType = -3)";

        gSQL = gSQL + " AND dbo.DelearInfo.Name='" + ddlDealerName.SelectedItem.Text + "'";
        gSQL = gSQL + " AND dbo.DelearInfo.Code = '" + txtDealerCode.Text + "'";
        gSQL = gSQL + " AND dbo.MRSRMaster.TDate<='" + txtDate.Text + "'";

        gSQL = gSQL + " GROUP BY ";
        gSQL = gSQL + " dbo.DelearInfo.Code,";
        gSQL = gSQL + " dbo.DelearInfo.Name , dbo.DelearInfo.Address, dbo.[Zone].CatName ";

        cmd = new SqlCommand(gSQL, conn);
        conn.Open();
        dr = cmd.ExecuteReader();

        if (dr.Read())
        {
            this.lblOBWith.Text = dr["NetSalesAmnt"].ToString();
        }
        else
        {
            this.lblOBWith.Text = "0";
        }
        dr.Dispose();
        dr.Close();
        conn.Close();
        //'-------------------------------------------------------------------------------------------

    }
}
