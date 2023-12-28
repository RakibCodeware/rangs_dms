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

public partial class Search_Sales_info : System.Web.UI.Page
{
    SqlConnection conn = DBConnection.GetConnection();
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
            LoadDropDownList_CTP();

            dt = new DataTable();
            MakeTable();

        }

        else
        {
            dt = (DataTable)ViewState["dt"];
        }
        ViewState["dt"] = dt;


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
            ddlEntity.Items.Insert(0, new ListItem("ALL", "ALL"));

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
        //LOAD DATA IN GRID
        fnLoadData();
    }


    //LOAD SALES SUMMARY CHALLAN WISE
    private void fnLoadData()
    {

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

        //s = WebConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        //con = new SqlConnection(s);
        SqlConnection con = DBConnection.GetConnection();
        con.Open();

        string sSql = "";
        sSql = "SELECT MRSRCode, CONVERT(varchar(12), TDate, 105) AS TDate," +
            " ISNULL(NetSalesAmnt,0) AS NetSalesAmnt,  dbo.Entity.eName as Entity," +
            " ISNULL(CashAmnt,0) AS CashAmnt, POCode," +
            " (ISNULL(CardAmnt1,0) + ISNULL(CardAmnt2,0)) AS CardAmnt, " +
            " dbo.Customer.CustID, dbo.Customer.CustName, dbo.Customer.Address, " +
            " dbo.Customer.Phone, dbo.Customer.Mobile, dbo.Customer.Email, " +
            " Entity_1.eName AS DelFrom" +

            //" FROM dbo.Entity INNER JOIN  dbo.MRSRMaster ON dbo.Entity.EID = dbo.MRSRMaster.OutSource LEFT OUTER JOIN " +
            //" dbo.Customer ON dbo.MRSRMaster.Customer = dbo.Customer.Mobile" +

            " FROM   dbo.MRSRMaster INNER JOIN" +
            " dbo.Entity ON dbo.MRSRMaster.OutSource = dbo.Entity.EID LEFT OUTER JOIN" +
            " dbo.Customer ON dbo.MRSRMaster.Customer = dbo.Customer.Mobile LEFT OUTER JOIN" +
            " dbo.Entity AS Entity_1 ON dbo.MRSRMaster.DeliveryFrom = Entity_1.EID" +

            " WHERE (dbo.MRSRMaster.TrType = 3) " +
            " AND (dbo.MRSRMaster.OutSource='" + Session["EID"] + "')" +

            " AND (dbo.MRSRMaster.TDate >= '" + Convert.ToDateTime(this.txtFrom.Text) + "'" +
            " AND dbo.MRSRMaster.TDate <= '" + Convert.ToDateTime(this.txtToDate.Text) + "')";

        if (this.ddlEntity.SelectedItem.Text != "ALL")
        {
            sSql = sSql + " AND (Entity_1.eName='" + this.ddlEntity.SelectedItem.Text + "')";
        }
        if (this.txtInvNo.Text.Length != 0)
        {
            sSql = sSql + " AND (MRSRCode='" + this.txtInvNo.Text + "')";
        }
        if (this.txtMobile.Text.Length != 0)
        {
            sSql = sSql + " AND (dbo.Customer.Mobile='" + this.txtMobile.Text + "')";
        }

        sSql = sSql + " ORDER BY TDate, MRSRCode";
        

        SqlCommand cmd = new SqlCommand(sSql, con);        
        //OleDbDataReader dr = cmd.ExecuteReader();

        DataSet ds = new DataSet();
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        da.Fill(ds);

        GridView1.DataSource = ds;
        GridView1.DataBind();
        //dr.Close();
        con.Close();

    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //CalcTotalQty(e.Row.Cells[4].Text);
            CalcTotal_TP(e.Row.Cells[3].Text);

            CalcTotal_Cash(e.Row.Cells[4].Text);
            CalcTotal_Card(e.Row.Cells[5].Text);

            // ALIGNMENT
            e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Left;

            e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Center;

            e.Row.Cells[8].HorizontalAlign = HorizontalAlign.Center;
            

        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[2].Text = "Total";
            //e.Row.Cells[4].Text = runningTotalQty.ToString("0,0", CultureInfo.InvariantCulture);
            e.Row.Cells[3].Text = runningTotalTP.ToString("0,0", CultureInfo.InvariantCulture);
            e.Row.Cells[4].Text = runningTotalCash.ToString("0,0", CultureInfo.InvariantCulture);
            e.Row.Cells[5].Text = runningTotalCard.ToString("0,0", CultureInfo.InvariantCulture);


            //ALIGNMENT
            e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Left;

            e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Center;
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


        SqlConnection conn = DBConnection.GetConnection();



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

        sSql = " SELECT dbo.MRSRMaster.MRSRMID, dbo.MRSRMaster.MRSRCode," +
            " CONVERT(varchar(12), TDate, 101) AS TDate, dbo.MRSRMaster.OutSource," +
            "NetSalesAmnt,CONVERT(varchar, CAST(NetSalesAmnt AS money), 1) AS tAmnt,POCode," +
            "PayAmnt,DueAmnt,PayMode," +
            "CashAmnt,CONVERT(varchar, CAST(CashAmnt AS money), 1) AS tCashAmnt," +
            "CardAmnt1,CONVERT(varchar, CAST(CardAmnt1 AS money), 1) AS tCardAmnt1," +
            "CardAmnt2,CONVERT(varchar, CAST(CardAmnt2 AS money), 1) AS tCardAmnt2," +
            "CardNo1,CardNo2,CardType1,CardType2," +
            "Bank1,Bank2,SecurityCode,SecurityCode2," +
            "AppovalCode1,AppovalCode2,PersonID," +
            "Remarks,TermsCondition," +

            " dbo.MRSRMaster.Customer, dbo.Customer.CustID," +
            " dbo.Customer.CustName, dbo.Customer.Address," +
            " dbo.Customer.CustSex, dbo.Customer.Profession," +
            " dbo.Customer.Mobile, dbo.Customer.Email," +
            " dbo.Customer.City, dbo.Customer.CustArea," +
            " dbo.Customer.DOBT, dbo.Customer.CustAge," +
            " dbo.Customer.Org, dbo.Customer.Desg" +
            " FROM dbo.MRSRMaster LEFT OUTER JOIN" +
            " dbo.Customer ON dbo.MRSRMaster.Customer = dbo.Customer.Mobile" +
            " WHERE (dbo.MRSRMaster.MRSRCode = '" + sPID + "')" +
            " AND (dbo.MRSRMaster.TrType = 3)";
        //" AND (dbo.MRSRMaster.OutSource='" + Session["sBrId"] + "')";

        //sSql = sSql + " WHERE tbMemberList.ID= " + sPID + "";
        SqlCommand cmd = new SqlCommand(sSql, conn);
        conn.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            this.lblID.Text = dr["MRSRMID"].ToString();
            this.lblInv.Text = dr["MRSRCode"].ToString();
            this.lblDate.Text = dr["TDate"].ToString();

            this.lblCustName.Text = dr["CustName"].ToString();
            this.lblContact.Text = dr["Mobile"].ToString();
            this.lblAdd.Text = dr["Address"].ToString();
            this.lblSex.Text = dr["CustSex"].ToString();
            this.lblProfession.Text = dr["Profession"].ToString();
            this.lblEmail.Text = dr["Email"].ToString();
            this.lblOrg.Text = dr["Org"].ToString();
            this.lblDesg.Text = dr["Desg"].ToString();

            this.lblCity.Text = dr["City"].ToString();
            this.lblLoc.Text = dr["CustArea"].ToString();
            this.lblDOB.Text = dr["DOBT"].ToString();
            this.lblAge.Text = dr["CustAge"].ToString();

            this.lblTotalAmnt.Text = dr["tAmnt"].ToString();
            this.lblCash.Text = dr["tCashAmnt"].ToString();
            this.lblCard1.Text = dr["tCardAmnt1"].ToString();
            this.lblCardType1.Text = dr["CardAmnt1"].ToString();
            this.lblCardBank1.Text = dr["Bank1"].ToString();

            this.lblCard2.Text = dr["tCardAmnt2"].ToString();
            this.lblCardType2.Text = dr["CardType2"].ToString();
            this.lblCardBank2.Text = dr["Bank2"].ToString();

            this.lblOnlineOrderNo.Text = dr["POCode"].ToString();
            this.lblWarrenty.Text = dr["TermsCondition"].ToString();

            lblEID.Text = dr["OutSource"].ToString();

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

        }

        conn.Close();

        //LOAD CTP INFORMATION
        sSql = "";
        sSql = "SELECT EID, eName, EDesc, EntityType, EntityCode, ContactPerson,";
        sSql = sSql + " Desg, PhoneNo, EmailAdd, ContactNo";
        sSql = sSql + " FROM dbo.Entity";
        sSql = sSql + " WHERE EID='" + lblEID.Text + "'";
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
            Response.Redirect("Default.aspx");
        }

        SqlConnection con = DBConnection.GetConnection();

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
            fnSendMail_Invoice();
        }
        //    }
        //    catch
        //    {
        //        //
        //    }
        //}


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


        //****************************************************************************************
        //-----------------------------------------------------------------------------------------------------
        // Mail to Customer------------------------------------------------------------------------------------

        MailMessage mM2 = new MailMessage();
        //mM2.From = new MailAddress(txtEmail.Text);        

        //mM2.From = new MailAddress("rangs.eshop@gmail.com");
        mM2.From = new MailAddress("dms@rangs.com.bd");
        //PW:Exampass@567

        //mM2.To.Add(new MailAddress(Session["sEmail"].ToString()));
        mM2.To.Add(new MailAddress(lblEmail.Text));
        mM2.CC.Add(new MailAddress(lblCTPEmail.Text));
        mM2.Bcc.Add(new MailAddress("zunayedqu10@gmail.com"));

        mM2.Subject = "Sony-Rangs Invoice No." + lblInv.Text + " ";
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
        mM2.Body = mM2.Body + "<p><b>Invoice No: " + lblInv.Text + "</b><br/>";
        mM2.Body = mM2.Body + "Invoice Date: " + lblDate.Text + "</p>";

        //mM2.Body = mM2.Body + "<br/>";
        mM2.Body = mM2.Body + "<p><u>Customer Details:</u><br/> Name: " + lblCustName.Text + "<br/>";
        mM2.Body = mM2.Body + "Contact # " + lblContact.Text + "<br/>";
        mM2.Body = mM2.Body + "Email: " + lblEmail.Text + "<br/>";
        mM2.Body = mM2.Body + "Address: " + lblAdd.Text + "</p>";

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
        string sSql = "";
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
        sSql = sSql + " WHERE (dbo.MRSRMaster.MRSRMID = '" + this.lblID.Text + "')";

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
        mM2.Body = mM2.Body + "<b>Total Amount: &#2547; " + lblTotalAmnt.Text + "</b><br/>";

        mM2.Body = mM2.Body + "<b>Payment Details: </b><br/>";
        if (lblCash.Text.Length > 0)
        {
            mM2.Body = mM2.Body + "Cash: " + lblCash.Text + "<br/>";
        }
        if (lblCard1.Text.Length > 0)
        {
            if (lblCard1.Text != "0.00")
            {
                mM2.Body = mM2.Body + "Card1: " + lblCard1.Text + "<br/>";
            }
        }
        if (lblCard2.Text.Length > 0)
        {
            if (lblCard2.Text != "0.00")
            {
                mM2.Body = mM2.Body + "Card2 " + lblCard2.Text + "<br/>";
            }
        }


        //mM2.Body = mM2.Body + "Payment Type: " + lblPaymentMethod.Text + "<br/>";

        if (lblOnlineOrderNo.Text.Length > 0)
        {
            mM2.Body = mM2.Body + "Online Order No.: " + lblOnlineOrderNo.Text + "<br/>";
        }

        //mM2.Body = mM2.Body + "Delivery Type: " + lblDelType.Text + "<br/>";
        //mM2.Body = mM2.Body + "Delivery From: " + lblEName.Text + " (" + lblCTPAdd.Text + ") " + "";
        mM2.Body = mM2.Body + "</p>";
        //mM2.Body = mM2.Body + "<p>&nbsp;</p>";

        if (lblWarrenty.Text.Length > 0)
        {
            mM2.Body = mM2.Body + "<p>Terms & Conditions: " + lblWarrenty.Text + "</p>";
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

}