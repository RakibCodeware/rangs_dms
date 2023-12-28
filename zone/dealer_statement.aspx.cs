using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Globalization;


using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Text;
using System.Globalization;
using System.Text.RegularExpressions;

using System.Net.Mail;

public partial class dealer_statement : System.Web.UI.Page
{
    SqlConnection conn = DBConnectionDSM.GetConnection();
    long i;

    int iMRSRID = 0;
    DataTable dt;
    DateTime tDate;


    private double runningTotal = 0;
    private double runningTotalTP = 0;
    private double runningTotalDis = 0;
    private double runningTotalWith = 0;
    private double runningTotalQty = 0;

    private double runningTotalCash = 0;
    private double runningTotalCard = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (System.Convert.ToInt32(Session["Vis"]) == 0)
        {
            Response.Redirect("../Account/Login.aspx");
        }

        if (!IsPostBack)
        {
            this.txtFrom.Text = DateTime.Today.ToString("MM/dd/yyyy");
            this.txtToDate.Text = DateTime.Today.ToString("MM/dd/yyyy");

            //LOAD CTP
            //LoadDropDownList_CTP();

            LoadDropDownList_Dealer();

            dt = new DataTable();
            MakeTable();

        }

        else
        {
            dt = (DataTable)ViewState["dt"];
        }
        ViewState["dt"] = dt;


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
        ddlEntity.DataSource = cmd.ExecuteReader();
        ddlEntity.DataTextField = "Name";
        ddlEntity.DataValueField = "DAID";
        ddlEntity.DataBind();

        //Add blank item at index 0.
        //ddlEntity.Items.Insert(0, new ListItem("", "0"));
        ddlEntity.Items.Insert(0, new ListItem("ALL", "ALL"));
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

    


    protected void lnkPrint_Click(object sender, EventArgs e)
    {
        LinkButton btnsubmit = sender as LinkButton;
        GridViewRow gRow = (GridViewRow)btnsubmit.NamingContainer;
        //txtPID.Text = gvCustomres.DataKeys[gRow.RowIndex].Value.ToString();

        //Session["AssAID"] = GridView1.DataKeys[gRow.RowIndex].Value.ToString();
        Session["sBillNo"] = GridView1.DataKeys[gRow.RowIndex].Value.ToString();
        //Session["sBillNo"] = this.txtInvoiceNo.Text;
        Session["sReportType"] = "RPT_Sales_Bill";

        Response.Redirect("Sales_Bill_Print.aspx");

    }

    protected void PopupMessage(string Msg, Control controlID)
    {
        ScriptManager.RegisterClientScriptBlock(controlID, controlID.GetType(), "msg", "alert('" + Msg + "');", true);
    }

    protected void SearchData(object sender, EventArgs e)
    {
        lblOBSales.Text = "0";
        lblOBCollection.Text = "0";
        lblOBDis.Text = "0";
        lblOBWith.Text = "0";

        //LOAD DATA IN GRID
        fnLoadData();

        //LOAD STATEMENT
        fnLoadOutStanding();

        double dOutStanding = Convert.ToDouble(lblOBSales.Text) - Convert.ToDouble(lblOBCollection.Text) + Convert.ToDouble(lblOBDis.Text) - Convert.ToDouble(lblOBWith.Text);
        txtOB.Text = Convert.ToString(dOutStanding);

        if (txtSalesAmnt.Text.Length == 0)
        {
            txtSalesAmnt.Text = "0";
        }
        if (txtDeposit.Text.Length == 0)
        {
            txtDeposit.Text = "0";
        }
        if (txtWithAmnt.Text.Length == 0)
        {
            txtWithAmnt.Text = "0";
        }

        double dCB = dOutStanding + Convert.ToDouble(txtSalesAmnt.Text) - Convert.ToDouble(txtDeposit.Text) - Convert.ToDouble(txtWithAmnt.Text);
        txtCB.Text = Convert.ToString(dCB);

    }


    //LOAD SALES SUMMARY CHALLAN WISE
    private void fnLoadData()
    {
        txtSalesAmnt.Text = "0";
        txtDeposit.Text = "0";
        txtWithAmnt.Text = "0";

        //-----------------------------------------------------------------------------------------
        //CHECK FOR VAT USER
        if (Session["iVatUser"].ToString() == "1")
        {
            if (Convert.ToDateTime(this.txtFrom.Text) < Convert.ToDateTime(Session["eFYSDate"]))
            {
                this.txtFrom.Text = Session["ssFYSDate"].ToString();
            }
        }
        //-----------------------------------------------------------------------------------------

        lblDate1.Text = " (" + txtFrom.Text + " To " + txtToDate.Text + ")";
        lblDate2.Text = " (" + txtFrom.Text + " To " + txtToDate.Text + ")";
        lblDate3.Text = " (" + txtFrom.Text + " To " + txtToDate.Text + ")";

        //s = WebConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        //con = new SqlConnection(s);
        SqlConnection conn = DBConnectionDSM.GetConnection();
        
        //*****************************************************************************************
        //LOAD SALES DELIVERY
        conn.Open();

        string sSql = "";
        sSql = "";
        sSql = "SELECT dbo.MRSRMaster.MRSRCode, CONVERT(varchar(12), dbo.MRSRMaster.TDate, 101) AS TDate, dbo.MRSRMaster.TrType,";
        sSql = sSql + " dbo.VW_Delear_Info.Name AS InSource, dbo.VW_Delear_Info.ZoneName, dbo.Zone.CatName AS OutSource, ";
        sSql = sSql + " dbo.VW_Delear_Info.Address, dbo.VW_Delear_Info.DealerStatus, dbo.VW_Delear_Info.ContactNo,";
        sSql = sSql + " dbo.VW_Delear_Info.ContactPerson, dbo.VW_Delear_Info.Code, dbo.VW_Delear_Info.DAID, ";
        sSql = sSql + " dbo.VW_Delear_Info.EmailAdd, dbo.VW_Delear_Info.ZoneType, dbo.MRSRMaster.POCode,";
        sSql = sSql + " dbo.MRSRMaster.OnLineSales, dbo.MRSRMaster.TermsCondition, dbo.MRSRMaster.Remarks, ";
        sSql = sSql + " dbo.MRSRMaster.MRSRMID, dbo.MRSRMaster.NetSalesAmnt, dbo.MRSRMaster.SaleDeclar,";
        sSql = sSql + " CASE dbo.MRSRMaster.SaleDeclar WHEN 1 THEN 'Declared' ELSE 'Pending' END AS sStatus";
        sSql = sSql + " FROM dbo.VW_Delear_Info INNER JOIN";
        sSql = sSql + " dbo.MRSRMaster ON dbo.VW_Delear_Info.DAID = dbo.MRSRMaster.InSource INNER JOIN";
        sSql = sSql + " dbo.Zone ON dbo.MRSRMaster.OutSource = dbo.Zone.CategoryID";

        sSql = sSql + " WHERE (dbo.MRSRMaster.TrType = 3) AND (dbo.Zone.Code = '" + Session["sBrCode"].ToString() + "') ";
        sSql = sSql + " AND (dbo.MRSRMaster.TDate >= '" + Convert.ToDateTime(this.txtFrom.Text) + "'";
        sSql = sSql + " AND dbo.MRSRMaster.TDate <= '" + Convert.ToDateTime(this.txtToDate.Text) + "')";

        if (this.ddlEntity.SelectedItem.Text != "ALL")
        {
            sSql = sSql + " AND (dbo.VW_Delear_Info.Name ='" + this.ddlEntity.SelectedItem.Text + "')";
        }
        
        sSql = sSql + " ORDER BY dbo.MRSRMaster.TDate, dbo.MRSRMaster.MRSRCode DESC";

        SqlCommand cmd = new SqlCommand(sSql, conn);        
        //OleDbDataReader dr = cmd.ExecuteReader();

        DataSet ds = new DataSet();
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        da.Fill(ds);

        GridView1.DataSource = ds;
        GridView1.DataBind();
        //dr.Close();
        conn.Close();
        //*****************************************************************************************

        //*****************************************************************************************
        //LOAD SALES WITHDRAWN
        conn.Open();

        sSql = "";
        sSql = "";
        sSql = "SELECT dbo.MRSRMaster.MRSRCode, CONVERT(varchar(12), dbo.MRSRMaster.TDate, 101) AS TDate, dbo.MRSRMaster.TrType,";
        sSql = sSql + " dbo.VW_Delear_Info.Name AS InSource, dbo.VW_Delear_Info.ZoneName, dbo.Zone.CatName AS OutSource, ";
        sSql = sSql + " dbo.VW_Delear_Info.Address, dbo.VW_Delear_Info.DealerStatus, dbo.VW_Delear_Info.ContactNo,";
        sSql = sSql + " dbo.VW_Delear_Info.ContactPerson, dbo.VW_Delear_Info.Code, dbo.VW_Delear_Info.DAID, ";
        sSql = sSql + " dbo.VW_Delear_Info.EmailAdd, dbo.VW_Delear_Info.ZoneType, dbo.MRSRMaster.POCode,";
        sSql = sSql + " dbo.MRSRMaster.OnLineSales, dbo.MRSRMaster.TermsCondition, dbo.MRSRMaster.Remarks, ";
        sSql = sSql + " dbo.MRSRMaster.MRSRMID, dbo.MRSRMaster.NetSalesAmnt, dbo.MRSRMaster.SaleDeclar,";
        sSql = sSql + " CASE dbo.MRSRMaster.SaleDeclar WHEN 1 THEN 'Declared' ELSE 'Pending' END AS sStatus";
        sSql = sSql + " FROM dbo.VW_Delear_Info INNER JOIN";
        sSql = sSql + " dbo.MRSRMaster ON dbo.VW_Delear_Info.DAID = dbo.MRSRMaster.OutSource INNER JOIN";
        sSql = sSql + " dbo.Zone ON dbo.MRSRMaster.InSource = dbo.Zone.CategoryID";

        sSql = sSql + " WHERE (dbo.MRSRMaster.TrType = -3) AND (dbo.Zone.Code = '" + Session["sBrCode"].ToString() + "') ";
        sSql = sSql + " AND (dbo.MRSRMaster.TDate >= '" + Convert.ToDateTime(this.txtFrom.Text) + "'";
        sSql = sSql + " AND dbo.MRSRMaster.TDate <= '" + Convert.ToDateTime(this.txtToDate.Text) + "')";

        if (this.ddlEntity.SelectedItem.Text != "ALL")
        {
            sSql = sSql + " AND (dbo.VW_Delear_Info.Name ='" + this.ddlEntity.SelectedItem.Text + "')";
        }
        
        sSql = sSql + " ORDER BY dbo.MRSRMaster.TDate, dbo.MRSRMaster.MRSRCode DESC";

        cmd = new SqlCommand(sSql, conn);
        //OleDbDataReader dr = cmd.ExecuteReader();

        DataSet ds2 = new DataSet();
        SqlDataAdapter da2 = new SqlDataAdapter(cmd);
        da2.Fill(ds2);

        GridView2.DataSource = ds2;
        GridView2.DataBind();
        //dr.Close();
        conn.Close();
        //*****************************************************************************************

        //LOAD DEPOSIT DETAILS
        conn.Open();

        sSql = "";
        sSql = "SELECT   dbo.DepositAmnt.CANO, dbo.DepositAmnt.CollectionNo,";
        sSql = sSql + " CONVERT(VARCHAR(10), dbo.DepositAmnt.CDate, 105) AS DepositDate,";
        sSql = sSql + " dbo.DepositAmnt.DelearID, dbo.DelearInfo.Code, dbo.DelearInfo.Name, dbo.DelearInfo.Address, ";
        sSql = sSql + " ISNULL(dbo.DepositAmnt.DepositAmnt, 0) AS DepositAmnt, dbo.DepositAmnt.PayType,";
        sSql = sSql + " dbo.DepositAmnt.ChequeNo, dbo.DepositAmnt.BankName, dbo.DepositAmnt.BranchName, ";
        sSql = sSql + " dbo.DepositAmnt.cRemarks, dbo.Zone.CatName AS ZoneName, dbo.Zone.CategoryID, ";
        sSql = sSql + " dbo.DelearInfo.DAID, dbo.DepositAmnt.RefNo, dbo.DepositAmnt.BankID";
        sSql = sSql + " FROM dbo.DepositAmnt INNER JOIN";
        sSql = sSql + " dbo.DelearInfo ON dbo.DepositAmnt.DelearID = dbo.DelearInfo.DAID INNER JOIN";
        sSql = sSql + " dbo.Zone ON dbo.DelearInfo.CategoryID = dbo.Zone.CategoryID";

        sSql = sSql + " WHERE (dbo.DepositAmnt.CDate >= '" + Convert.ToDateTime(this.txtFrom.Text) + "'";
        sSql = sSql + " AND dbo.DepositAmnt.CDate <= '" + Convert.ToDateTime(this.txtToDate.Text) + "')";
        sSql = sSql + " AND (dbo.Zone.CategoryID='" + Session["sZoneID"] + "')";

        if (ddlEntity.SelectedItem.Text != "ALL")
        {
            sSql = sSql + " AND (dbo.DelearInfo.Name = '" + ddlEntity.SelectedItem.Text + "')";
        }
                
        sSql = sSql + " ORDER BY dbo.DepositAmnt.CDate, dbo.DepositAmnt.CollectionNo Desc";

        cmd = new SqlCommand(sSql, conn);
        //OleDbDataReader dr = cmd.ExecuteReader();

        DataSet ds3 = new DataSet();
        SqlDataAdapter da3 = new SqlDataAdapter(cmd);
        da3.Fill(ds3);

        GridView3.DataSource = ds3;
        GridView3.DataBind();
        //dr.Close();
        conn.Close();
        //*****************************************************************************************

    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //CalcTotalQty(e.Row.Cells[4].Text);
            CalcTotal_TP(e.Row.Cells[3].Text);

            //CalcTotal_Cash(e.Row.Cells[4].Text);
            //CalcTotal_Card(e.Row.Cells[5].Text);

            // ALIGNMENT
            e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Left;

            //e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
            //e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Center;

            //e.Row.Cells[7].HorizontalAlign = HorizontalAlign.Center;
            //e.Row.Cells[8].HorizontalAlign = HorizontalAlign.Center;
            

        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[2].Text = "Total";
            //e.Row.Cells[4].Text = runningTotalQty.ToString("0,0", CultureInfo.InvariantCulture);
            e.Row.Cells[3].Text = runningTotalTP.ToString("0,0", CultureInfo.InvariantCulture);
            txtSalesAmnt.Text = runningTotalTP.ToString("0,0", CultureInfo.InvariantCulture);
            //e.Row.Cells[4].Text = runningTotalCash.ToString("0,0", CultureInfo.InvariantCulture);
            //e.Row.Cells[5].Text = runningTotalCard.ToString("0,0", CultureInfo.InvariantCulture);


            //ALIGNMENT
            e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Left;

            e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
            //e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Center;
            //e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Center;
            
        }

    }

    protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //CalcTotalQty(e.Row.Cells[4].Text);
            CalcTotal_With(e.Row.Cells[3].Text);

            //CalcTotal_Cash(e.Row.Cells[4].Text);
            //CalcTotal_Card(e.Row.Cells[5].Text);

            // ALIGNMENT
            e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Left;

            //e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
            //e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Center;

            //e.Row.Cells[7].HorizontalAlign = HorizontalAlign.Center;
            //e.Row.Cells[8].HorizontalAlign = HorizontalAlign.Center;


        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[2].Text = "Total";
            //e.Row.Cells[4].Text = runningTotalQty.ToString("0,0", CultureInfo.InvariantCulture);
            e.Row.Cells[3].Text = runningTotalWith.ToString("0,0", CultureInfo.InvariantCulture);
            txtWithAmnt.Text = runningTotalWith.ToString("0,0", CultureInfo.InvariantCulture);
            //e.Row.Cells[4].Text = runningTotalCash.ToString("0,0", CultureInfo.InvariantCulture);
            //e.Row.Cells[5].Text = runningTotalCard.ToString("0,0", CultureInfo.InvariantCulture);


            //ALIGNMENT
            e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Left;

            e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
            //e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Center;
            //e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Center;

        }

    }

    protected void GridView3_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //CalcTotalQty(e.Row.Cells[4].Text);
            //CalcTotal_TP(e.Row.Cells[3].Text);

            //CalcTotal_Cash(e.Row.Cells[4].Text);
            CalcTotal_Card(e.Row.Cells[5].Text);

            // ALIGNMENT
            e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Left;

            //e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
            //e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Center;

            //e.Row.Cells[8].HorizontalAlign = HorizontalAlign.Center;


        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[1].Text = "Total";
            //e.Row.Cells[4].Text = runningTotalQty.ToString("0,0", CultureInfo.InvariantCulture);
            //e.Row.Cells[3].Text = runningTotalTP.ToString("0,0", CultureInfo.InvariantCulture);
            //e.Row.Cells[4].Text = runningTotalCash.ToString("0,0", CultureInfo.InvariantCulture);
            e.Row.Cells[5].Text = runningTotalCard.ToString("0,0", CultureInfo.InvariantCulture);
            txtDeposit.Text = runningTotalCard.ToString("0,0", CultureInfo.InvariantCulture);

            //ALIGNMENT
            e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Left;

            //e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
            //e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Center;

        }

    }

    //CALCULATE TOTAL CASH PAY
    private void CalcTotal_Cash(string _price)
    {
        try
        {
            runningTotalCash += Double.Parse(_price);
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write(ex.ToString());
        }
    }

    //CALCULATE TOTAL CARD PAY
    private void CalcTotal_Card(string _price)
    {
        try
        {
            runningTotalCard += Double.Parse(_price);
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write(ex.ToString());
        }
    }

    /*
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
    */


    protected void lnkView_Click(object sender, EventArgs e)
    {
        //if (System.Convert.ToInt32(Session["Vis"]) == 0)
        //{
        //    Response.Redirect("Default.aspx");
        //}

        //CLEAR GRIDVIEW
        gvUsers.DataSource = null;
        gvUsers.DataBind();

        //CLEAR DATA TABLE
        dt.Clear();


        LinkButton btnsubmit = sender as LinkButton;
        GridViewRow gRow = (GridViewRow)btnsubmit.NamingContainer;
        //txtPID.Text = gvCustomres.DataKeys[gRow.RowIndex].Value.ToString();
        //string sPID = Convert.ToString(gvCustomres.DataKeys[gRow.RowIndex].Value.ToString());
        string sPID = GridView1.DataKeys[gRow.RowIndex].Value.ToString();
        //txtPName.Text = gRow.Cells[0].Text;        
        //this.ModalPopupExtender1.Show();


        SqlConnection conn = DBConnectionDSM.GetConnection();



        string sSql = "";

        //sSql = " SELECT dbo.MRSRMaster.MRSRMID, dbo.MRSRMaster.MRSRCode," +
        //    " CONVERT(varchar(12), TDate, 101) AS TDate, dbo.MRSRMaster.OutSource," +
        //    "NetSalesAmnt," +
        //    "PayAmnt,DueAmnt,PayMode," +
        //    "CashAmnt,CardAmnt1,CardAmnt2," +
        //    "CardNo1,CardNo2,CardType1,CardType2," +
        //    "Bank1,Bank2,SecurityCode,SecurityCode2," +
        //    "AppovalCode1,AppovalCode2,PersonID," +
        //    "Remarks,TermsCondition," +
               
        sSql = "";
        sSql = "SELECT dbo.MRSRMaster.MRSRCode, CONVERT(varchar(12), dbo.MRSRMaster.TDate, 101) AS TDate, dbo.MRSRMaster.TrType,";
        sSql = sSql + " dbo.VW_Delear_Info.Name AS InSource, dbo.VW_Delear_Info.ZoneName, dbo.Zone.CatName AS OutSource,dbo.Zone.CategoryID, ";
        sSql = sSql + " dbo.VW_Delear_Info.Address, dbo.VW_Delear_Info.DealerStatus, dbo.VW_Delear_Info.ContactNo,";
        sSql = sSql + " dbo.VW_Delear_Info.ContactPerson, dbo.VW_Delear_Info.Code, dbo.VW_Delear_Info.DAID, ";
        sSql = sSql + " dbo.VW_Delear_Info.EmailAdd, dbo.VW_Delear_Info.ZoneType, dbo.MRSRMaster.POCode,";
        sSql = sSql + " dbo.MRSRMaster.OnLineSales, dbo.MRSRMaster.TermsCondition, dbo.MRSRMaster.Remarks, ";
        sSql = sSql + " dbo.MRSRMaster.MRSRMID, dbo.MRSRMaster.NetSalesAmnt, dbo.MRSRMaster.SaleDeclar,";
        sSql = sSql + " dbo.Zone.Code AS ZonalCode, dbo.Zone.ZonalEmail,";
        sSql = sSql + " CASE dbo.MRSRMaster.SaleDeclar WHEN 1 THEN 'Declared' ELSE 'Pending' END AS sStatus";
        sSql = sSql + " FROM dbo.VW_Delear_Info INNER JOIN";
        sSql = sSql + " dbo.MRSRMaster ON dbo.VW_Delear_Info.DAID = dbo.MRSRMaster.InSource INNER JOIN";
        sSql = sSql + " dbo.Zone ON dbo.MRSRMaster.OutSource = dbo.Zone.CategoryID";

        sSql = sSql + " WHERE (dbo.MRSRMaster.TrType = 3)";
        sSql = sSql + " AND (dbo.MRSRMaster.MRSRCode = '" + sPID + "')";

        //sSql = sSql + " WHERE tbMemberList.ID= " + sPID + "";
        SqlCommand cmd = new SqlCommand(sSql, conn);
        conn.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            this.lblID.Text = dr["MRSRMID"].ToString();
            this.lblInv.Text = dr["MRSRCode"].ToString();
            this.lblDate.Text = dr["TDate"].ToString();

            this.lblCustName.Text = dr["InSource"].ToString();
            this.lblContact.Text = dr["ContactNo"].ToString();
            this.lblAdd.Text = dr["Address"].ToString();

            this.lblEmail.Text = dr["EmailAdd"].ToString();

            this.lblTotalAmnt.Text = dr["NetSalesAmnt"].ToString();
            
            this.lblOnlineOrderNo.Text = dr["POCode"].ToString();
            this.lblWarrenty.Text = dr["TermsCondition"].ToString();

            lblZoneID.Text = dr["CategoryID"].ToString();
            lblZoneCode.Text = dr["ZonalCode"].ToString();
            lblZoneEmail.Text = dr["ZonalEmail"].ToString();

            lblDealerID.Text = dr["DAID"].ToString();
            lblDealerCode.Text = dr["Code"].ToString();
            lblDealerMobile.Text = dr["ContactNo"].ToString();
            lblDealerEmail.Text = dr["EmailAdd"].ToString();
            //Image1.ImageUrl = "img/photos/" + dr["path"].ToString();

        }
        else
        {
            this.lblID.Text = "";
            this.lblInv.Text = "";
            this.lblDate.Text = "";

            this.lblCustName.Text = "";
            this.lblContact.Text = "";
            this.lblAdd.Text = "";
            this.lblSex.Text = "";
            this.lblProfession.Text = "";
            this.lblEmail.Text = "";
            this.lblOrg.Text = "";
            this.lblDesg.Text = "";

            this.lblCity.Text = "";
            this.lblLoc.Text = "";
            this.lblDOB.Text = "";
            this.lblAge.Text = "";

            lblZoneID.Text = "0";
            lblZoneCode.Text = "";
            lblZoneEmail.Text = "";

            lblDealerID.Text = "0";
            lblDealerCode.Text = "";
            lblDealerMobile.Text = "";
            lblDealerEmail.Text = "";


        }

        conn.Close();

        ////LOAD CTP INFORMATION
        //sSql = "";
        //sSql = "SELECT EID, eName, EDesc, EntityType, EntityCode, ContactPerson,";
        //sSql = sSql + " Desg, PhoneNo, EmailAdd, ContactNo";
        //sSql = sSql + " FROM dbo.Entity";
        //sSql = sSql + " WHERE EID='" + lblEID.Text + "'";
        //SqlCommand cmdC = new SqlCommand(sSql, conn);
        //conn.Open();
        //SqlDataReader drC = cmdC.ExecuteReader();
        //if (drC.Read())
        //{
        //    lblCTPName.Text = drC["eName"].ToString();
        //    lblCTPAdd.Text = drC["EDesc"].ToString();
        //    lblCTPEmail.Text = drC["EmailAdd"].ToString();
        //    lblCTPContact.Text = drC["PhoneNo"].ToString();
        //    if (drC["PhoneNo"].ToString().Length == 0)
        //    {
        //        lblCTPContact.Text = drC["ContactNo"].ToString();
        //    }
        //}
        //conn.Close();
    

        //LOAD DETAILS DATA
        sSql = "";
        sSql = "SELECT dbo.Product.ProductID, dbo.Product.Model," +
            " dbo.Product.UnitPrice AS MRP," +
            " dbo.MRSRDetails.UnitPrice AS CampaignPrice," +
            " ABS(dbo.MRSRDetails.Qty) AS Qty," +
            " dbo.MRSRDetails.TotalAmnt As TotalPrice," +
            " dbo.MRSRDetails.DiscountAmnt AS DisAmnt, " +
            " dbo.MRSRDetails.DisCode, dbo.MRSRDetails.DisRef," +
            " dbo.MRSRDetails.WithAdjAmnt, dbo.MRSRDetails.NetAmnt," +
            " dbo.MRSRDetails.SLNO AS ProductSL," +
            " dbo.MRSRDetails.ProdRemarks as Remarks" +
            " FROM dbo.Product INNER JOIN" +
            " dbo.MRSRDetails ON dbo.Product.ProductID = dbo.MRSRDetails.ProductID" +
            " WHERE (dbo.MRSRDetails.MRSRMID = '" + this.lblID.Text + "')";

        cmd = new SqlCommand(sSql, conn);
        conn.Open();

        // Create a SqlDataAdapter to get the results as DataTable
        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sSql, conn);

        // Fill the DataTable with the result of the SQL statement
        sqlDataAdapter.Fill(dt);

        gvUsers.DataSource = dt;
        gvUsers.DataBind();
        

        this.ModalPopupExtender1.Show();


    }



    protected void OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        GridView1.DataBind();
        fnLoadData();
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

            //double value2 = Convert.ToDouble(e.Row.Cells[2].Text);
            //e.Row.Cells[2].Text = value2.ToString("0");

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

    protected void BindGrid()
    {
        gvUsers.DataSource = ViewState["dt"] as DataTable;
        gvUsers.DataBind();
    }

    protected void btnSendMail_Click(object sender, EventArgs e)
    {
        if (System.Convert.ToInt32(Session["Vis"]) == 0)
        {
            Response.Redirect("../Account/Login.aspx");
        }

        SqlConnection con = DBConnectionDSM.GetConnection();

        //string sSql = "";
        //sSql = "UPDATE tbCustomerDelivery Set dStatus='" + RadioButtonList1.SelectedIndex + "',";
        //sSql = sSql + " StatusNote='" + txtNote.Text + "' where DelNo='" + lblInv.Text + "'";
        //SqlCommand cmdIns = new SqlCommand(sSql, conn);
        //conn.Open();
        //cmdIns.ExecuteNonQuery();
        //conn.Close();

        //if (RadioButtonList1.SelectedIndex == 4)
        //{
        //    //SEND CANCEL MAIL
        //    try
        //    {
        if (lblEmail.Text.Length > 0)
        {
            //fnSendMail_Invoice();
        }
        //    }
        //    catch
        //    {
        //        //
        //    }
        //}


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
        gSQL = gSQL + " WHERE dbo.DelearInfo.Name = '" + ddlEntity.SelectedItem.Text + "'";
        //gSQL = gSQL + " AND dbo.DelearInfo.Code = '" + txtDealerCode.Text + "'";
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
        //gSQL = "";
        //gSQL = "SELECT TOP (1) dbo.tbCreditLimitYearly.TID, dbo.DelearInfo.DAID, dbo.DelearInfo.Code,";
        //gSQL = gSQL + " dbo.DelearInfo.Name, dbo.DelearInfo.ContactNo,";
        //gSQL = gSQL + " dbo.DelearInfo.EmailAdd, dbo.tbCreditLimitYearly.TAmount";
        //gSQL = gSQL + " FROM dbo.tbCreditLimitYearly INNER JOIN";
        //gSQL = gSQL + " dbo.DelearInfo ON dbo.tbCreditLimitYearly.DealerID = dbo.DelearInfo.DAID";
        //gSQL = gSQL + " WHERE dbo.DelearInfo.Name = '" + ddlDealerName.SelectedItem.Text + "'";
        //gSQL = gSQL + " AND dbo.DelearInfo.Code = '" + txtDealerCode.Text + "'";
        //gSQL = gSQL + " ORDER BY dbo.tbCreditLimitYearly.TID DESC";

        //SqlCommand cmd = new SqlCommand(gSQL, conn);
        //conn.Open();
        //SqlDataReader dr = cmd.ExecuteReader();

        //if (dr.Read())
        //{
        //    this.txtCreditLimit.Text = dr["TAmount"].ToString();
        //}
        //else
        //{
        //    this.txtCreditLimit.Text = "0";
        //}

        //    '========================================================================
        //    'OPENING
        //    '========================================================================
        //    '--------------------------------------------------------------------------------------------

        //'    gSQL = ""
        //'    gSQL = "DELETE FROM TempOpening"
        //'    'gSQL = gSQL & " WHERE UserID='" & sUser & "' AND PCName='" & ComputerName & "'"
        //'    Cnn.Execute gSQL


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

        gSQL = gSQL + " AND dbo.DelearInfo.Name='" + ddlEntity.SelectedItem.Text + "'";
        //gSQL = gSQL + " AND dbo.DelearInfo.Code = '" + txtDealerCode.Text + "'";
        gSQL = gSQL + " AND dbo.MRSRMaster.TDate<'" + txtFrom.Text + "'";
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


        gSQL = gSQL + " WHERE dbo.DelearInfo.Name='" + ddlEntity.SelectedItem.Text + "'";
        //gSQL = gSQL + " AND dbo.DelearInfo.Code = '" + txtDealerCode.Text + "'";
        gSQL = gSQL + " AND dbo.DepositAmnt.CDate<'" + txtFrom.Text + "'";

        //gSQL = gSQL + " WHERE dbo.DepositAmnt.CDate<'" & dtpReceive & "'";
        //    'gSQL = gSQL & " AND dbo.DepositAmnt.CDate<='" & dtpEDate & "'";
        //''    If cboZone.text <> "ALL" Then
        //''        gSQL = gSQL & " AND dbo.Zone.CatName='" & cboZone.text & "'"
        //''    End If
        //gSQL = gSQL + " AND dbo.DelearInfo.Name='" & cboIn.text & "'";

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
        //gSQL = gSQL + " WHERE CDate<'" & dtpReceive & "'";
        ////'    If cboZone.text <> "ALL" Then
        ////'        gSQL = gSQL & " AND ZoneName='" & cboZone.text & "'"
        ////'    End If
        //gSQL = gSQL + " AND Name='" & cboIn.text & "'";

        gSQL = gSQL + " WHERE Name='" + ddlEntity.SelectedItem.Text + "'";
        //gSQL = gSQL + " AND Code = '" + txtDealerCode.Text + "'";
        gSQL = gSQL + " AND CDate<'" + txtFrom.Text + "'";

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

        //'If cboZone.text <> "ALL" Then
        //'        gSQL = gSQL & " AND dbo.[Zone].CatName='" & cboZone.text & "'"
        //'    End If
        //gSQL = gSQL + " AND dbo.DelearInfo.Name='" & cboIn.text & "'";
        //gSQL = gSQL + " AND dbo.MRSRMaster.TDate<'" & dtpReceive & "'";

        gSQL = gSQL + " AND dbo.DelearInfo.Name='" + ddlEntity.SelectedItem.Text + "'";
        //gSQL = gSQL + " AND dbo.DelearInfo.Code = '" + txtDealerCode.Text + "'";
        gSQL = gSQL + " AND dbo.MRSRMaster.TDate<'" + txtFrom.Text + "'";

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