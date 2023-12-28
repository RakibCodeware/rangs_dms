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

public partial class Search_Incentive : System.Web.UI.Page
{
    SqlConnection conn = DBConnection.GetConnection();
    long i;

    int iMRSRID = 0;
    DataTable dt;
    DateTime tDate;

    private float runningTotal = 0;
    private float runningTotalTP = 0;
    private float runningTotalDis = 0;
    private float runningTotalWith = 0;
    private float runningTotalQty = 0;

    private float runningTotalIncentive = 0;

    private float runningTotalCash = 0;
    private float runningTotalCard = 0;

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

            //LOAD CTP NAME
            LoadDropDownList_CTP();

            dt = new DataTable();
            MakeTable();

            GridView1.Visible = false;
            GridView2.Visible = false;

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
        strQuery = strQuery + " (EntityType = 'showroom' OR EntityType = 'Dealer')";
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

        //Response.Redirect("Sales_Bill_Print.aspx");

    }

    protected void lnkView_Click(object sender, EventArgs e)
    {
        //if (System.Convert.ToInt32(Session["Vis"]) == 0)
        //{
        //    Response.Redirect("Default.aspx");
        //}

        //CLEAR GRIDVIEW
        gvUsers.DataSource = "";
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
        sSql = " SELECT dbo.MRSRMaster.MRSRMID, dbo.MRSRMaster.MRSRCode," +
            " CONVERT(varchar(12), TDate, 101) AS TDate, dbo.MRSRMaster.OutSource," +
            "NetSalesAmnt," +
            "PayAmnt,DueAmnt,PayMode," +
            "CashAmnt,CardAmnt1,CardAmnt2," +
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



        //LOAD DETAILS DATA
        sSql = "";
        sSql = "SELECT dbo.Product.ProductID, dbo.Product.Model," +
            " dbo.Product.MRP," +
            " dbo.MRSRDetails.UnitPrice AS CampaignPrice," +
            " ABS(dbo.MRSRDetails.Qty) AS Qty," +
            " dbo.MRSRDetails.TotalAmnt As TotalPrice," +
            " dbo.MRSRDetails.DiscountAmnt AS DisAmnt, " +
            " dbo.MRSRDetails.DisCode, dbo.MRSRDetails.DisRef," +
            " ISNULL(dbo.MRSRDetails.WithAdjAmnt,0) AS WithAdjAmnt, dbo.MRSRDetails.NetAmnt," +
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

    

    protected void PopupMessage(string Msg, Control controlID)
    {
        ScriptManager.RegisterClientScriptBlock(controlID, controlID.GetType(), "msg", "alert('" + Msg + "');", true);
    }

    protected void SearchData(object sender, EventArgs e)
    {
        GridView1.Visible = false;
        GridView2.Visible = false;

        //LOAD DATA IN GRID
        if (RadioButtonList1.SelectedIndex == 0)
        {
            fnLoadData_Summary();
            GridView1.Visible = false;
            GridView2.Visible = true;
        }
        else
        {
            fnLoadData_Details();
            GridView1.Visible = true;
            GridView2.Visible = false;
        }

    }

    private void fnLoadData_Summary()
    {
        //s = WebConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        //con = new SqlConnection(s);
        SqlConnection con = DBConnection.GetConnection();
        con.Open();

        string sSql = "";        
        sSql ="SELECT dbo.MRSRMaster.TrType, dbo.Product.Model, dbo.Product.ProdName, dbo.Product.GetIncentive,";
        sSql = sSql + " SUM(ABS(dbo.MRSRDetails.Qty)) AS tQty, dbo.Entity.eName,";
        sSql = sSql + " SUM(dbo.MRSRDetails.NetAmnt) AS tAmnt, ";
        sSql = sSql + " SUM(dbo.MRSRDetails.IncentiveAmnt) AS tIncentive ";
        //sSql = sSql + " SUM(dbo.MRSRDetails.NetAmnt - dbo.MRSRDetails.BLIPAmnt * ABS(dbo.MRSRDetails.Qty)) AS tIncentive";
        sSql = sSql + " FROM  dbo.Product INNER JOIN";
        sSql = sSql + " dbo.MRSRDetails ON dbo.Product.ProductID = dbo.MRSRDetails.ProductID INNER JOIN";
        sSql = sSql + " dbo.MRSRMaster ON dbo.MRSRDetails.MRSRMID = dbo.MRSRMaster.MRSRMID INNER JOIN";
        sSql = sSql + " dbo.Entity ON dbo.MRSRMaster.OutSource = dbo.Entity.EID";

        sSql = sSql + " WHERE (dbo.Product.GetIncentive = 1) AND (dbo.MRSRMaster.TrType = 3)";
        //sSql = sSql + " AND (dbo.MRSRMaster.TDate >= CONVERT(DATETIME, '2020-02-20 00:00:00', 102))";
        sSql = sSql + " AND (dbo.MRSRMaster.TDate >= '" + Convert.ToDateTime(this.txtFrom.Text) + "'";
        sSql = sSql + " AND dbo.MRSRMaster.TDate <= '" + Convert.ToDateTime(this.txtToDate.Text) + "')";

        sSql = sSql + " AND (dbo.MRSRMaster.OutSource = '" + Session["sBrId"] + "')";
        //if (ddlEntity.SelectedItem.Text != "ALL")
        //{
        //    sSql = sSql + " AND (dbo.Entity.eName='" + ddlEntity.SelectedItem.Text + "')";
        //}

        sSql = sSql + " GROUP BY dbo.MRSRMaster.TrType, dbo.Product.Model, dbo.Product.ProdName,";
        sSql = sSql + " dbo.Product.GetIncentive, dbo.Entity.eName";
        sSql = sSql + " ORDER BY dbo.Product.Model";


        //HAVING      (dbo.MRSRMaster.TrType = 3) 
        //AND (dbo.MRSRMaster.TDate >= CONVERT(DATETIME, '2020-02-17 00:00:00', 102))


        SqlCommand cmd = new SqlCommand(sSql, con);
        //OleDbDataReader dr = cmd.ExecuteReader();

        DataSet ds = new DataSet();
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        da.Fill(ds);

        GridView2.DataSource = ds;
        GridView2.DataBind();
        //dr.Close();
        con.Close();

    }
    
    private void fnLoadData_Details()
    {
        //s = WebConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        //con = new SqlConnection(s);
        SqlConnection con = DBConnection.GetConnection();
        con.Open();

        string sSql = "";        
        sSql = "SELECT dbo.MRSRMaster.MRSRMID, dbo.MRSRMaster.MRSRCode, CONVERT(varchar(12), TDate, 105) AS TDate,";
        sSql = sSql + " dbo.MRSRMaster.TrType, dbo.MRSRDetails.ProductID, ";
        sSql = sSql + " dbo.Product.Model, dbo.Product.ProdName, dbo.Product.GetIncentive, dbo.Entity.eName,";
        sSql = sSql + " SUM(ABS(dbo.MRSRDetails.Qty)) AS tQty, SUM(dbo.MRSRDetails.NetAmnt) AS tAmnt,";
        sSql = sSql + " SUM(ISNULL(dbo.MRSRDetails.IncentiveAmnt,0)) AS tIncentive, ";
        //sSql = sSql + " SUM(dbo.MRSRDetails.NetAmnt - dbo.MRSRDetails.BLIPAmnt * ABS(dbo.MRSRDetails.Qty)) AS tIncentive";
        sSql = sSql + " FROM  dbo.Product INNER JOIN";
        sSql = sSql + " dbo.MRSRDetails ON dbo.Product.ProductID = dbo.MRSRDetails.ProductID INNER JOIN";
        sSql = sSql + " dbo.MRSRMaster ON dbo.MRSRDetails.MRSRMID = dbo.MRSRMaster.MRSRMID INNER JOIN";
        sSql = sSql + " dbo.Entity ON dbo.MRSRMaster.OutSource = dbo.Entity.EID";
        
        sSql = sSql + " WHERE (dbo.MRSRMaster.TrType = 3) ";
        sSql = sSql + " AND  (dbo.Product.GetIncentive = 1)";
        sSql = sSql + " AND (dbo.MRSRMaster.OutSource = '" + Session["sBrId"] + "')";

        //if (ddlEntity.SelectedItem.Text != "ALL")
        //{
        //    sSql = sSql + " AND (dbo.Entity.eName='" + ddlEntity.SelectedItem.Text + "')";
        //}

        sSql = sSql + " AND (dbo.MRSRMaster.TDate >= '" + Convert.ToDateTime(this.txtFrom.Text) + "'";
        sSql = sSql + " AND dbo.MRSRMaster.TDate <= '" + Convert.ToDateTime(this.txtToDate.Text) + "')";
                        
        sSql = sSql + " GROUP BY dbo.MRSRMaster.MRSRMID, dbo.MRSRMaster.MRSRCode, dbo.MRSRMaster.TDate,";
        sSql = sSql + " dbo.MRSRMaster.TrType, dbo.MRSRDetails.ProductID, ";
        sSql = sSql + " dbo.Product.Model, dbo.Product.ProdName, dbo.Product.GetIncentive, dbo.Entity.eName";

        sSql = sSql + " ORDER BY dbo.MRSRMaster.TDate, dbo.MRSRMaster.MRSRCode";

        //HAVING      (dbo.MRSRMaster.TrType = 3) 
        //AND (dbo.MRSRMaster.TDate >= CONVERT(DATETIME, '2020-02-17 00:00:00', 102))


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
            CalcTotalQty(e.Row.Cells[4].Text);

            CalcTotal_TP(e.Row.Cells[5].Text);            
            CalcTotal_Incentive(e.Row.Cells[6].Text);
            //CalcTotal_Cash(e.Row.Cells[6].Text);

            // ALIGNMENT
            e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Left;
                                    

        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[1].Text = "Total";
            //e.Row.Cells[4].Text = runningTotalQty.ToString("0,0", CultureInfo.InvariantCulture);
            e.Row.Cells[4].Text = runningTotalQty.ToString("0,0", CultureInfo.InvariantCulture);
            e.Row.Cells[5].Text = runningTotalTP.ToString("0,0", CultureInfo.InvariantCulture);
            e.Row.Cells[6].Text = runningTotalIncentive.ToString("0,0", CultureInfo.InvariantCulture);
            //e.Row.Cells[6].Text = runningTotalCash.ToString("0,0", CultureInfo.InvariantCulture);
            
            //ALIGNMENT
            e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Left;

            e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Center;
            //01919401037--
        }

    }


    protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CalcTotalQty(e.Row.Cells[3].Text);

            CalcTotal_TP(e.Row.Cells[4].Text);
            CalcTotal_Incentive(e.Row.Cells[5].Text);
            //CalcTotal_Cash(e.Row.Cells[6].Text);

            // ALIGNMENT
            e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Left;
            e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Right;

        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[1].Text = "Total";
            e.Row.Cells[3].Text = runningTotalQty.ToString("0,0", CultureInfo.InvariantCulture);
            e.Row.Cells[4].Text = runningTotalTP.ToString("0,0", CultureInfo.InvariantCulture);
            e.Row.Cells[5].Text = runningTotalIncentive.ToString("0,0", CultureInfo.InvariantCulture);
            //e.Row.Cells[6].Text = runningTotalCash.ToString("0,0", CultureInfo.InvariantCulture);

            //ALIGNMENT
            e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Left;

            e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Right; 
            //01919401037--
        }

    }

    //CALCULATE TOTAL CASH PAY
    private void CalcTotal_Cash(string _price)
    {
        try
        {
            runningTotalCash += float.Parse(_price);
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
            runningTotalCard += float.Parse(_price);
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write(ex.ToString());
        }
    }

    //CALCULATE TOTAL INCENTIVE
    private void CalcTotal_Incentive(string _price)
    {
        try
        {
            runningTotalIncentive += float.Parse(_price);
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
            runningTotalQty += float.Parse(_qty);
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write(ex.ToString());
        }
    }
    */


    protected void OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        GridView1.DataBind();
        fnLoadData_Details();
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
            runningTotal += float.Parse(_price);
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
            runningTotalTP += float.Parse(_price);
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
            runningTotalDis += float.Parse(_price);
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
            runningTotalWith += float.Parse(_price);
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
            runningTotalQty += float.Parse(_qty);
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write(ex.ToString());
        }
    }

}