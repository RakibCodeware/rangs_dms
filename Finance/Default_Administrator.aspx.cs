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

public partial class Default_Administrator : System.Web.UI.Page
{

    string currentMonth1 = DateTime.Now.Month.ToString();
    string currentYear1 = DateTime.Now.Year.ToString();
    SqlDataReader dr;

    int FYs, FYe;
    DateTime sFYs, sFYe, sDate, eDate;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (System.Convert.ToInt32(Session["Vis"]) == 0)
        {
            Response.Redirect("~/Default.aspx");
        }

        if (!IsPostBack)
        {

            int monthC = Convert.ToInt16(currentMonth1);
            int yearC = Convert.ToInt16(currentYear1);

            if (monthC >= 7)
            {
                FYs = yearC;
                FYe = yearC + 1;
            }
            else
            {
                FYs = yearC - 1;
                FYe = yearC;
            }

            sFYs = Convert.ToDateTime("7/1/" + FYs + "");
            sFYe = Convert.ToDateTime("6/30/" + FYe + "");

            //this.lblText.Text = "Welcome to " + Session["eName"].ToString() + " ... FY : " + FYs + "-" + FYe + "";
            this.lblText.Text = "Welcome to Rangs Electronics Ltd ... FY : " + FYs + "-" + FYe + "";
            //this.lblCTP.Text = Session["eName"].ToString();                       

            Session["sFY"] = FYs + "-" + FYe;

            //LOAD YEARLY SALES AMOUNT
            fnLoadYearlySalesAmnt();

            //LOAD MONTHLY SALES AMOUNT
            fnLoadMonthlySalesAmnt();

            //LOAD DAILY SALES AMOUNT
            fnLoadDailySalesAmnt();

            //LOAD Yearly Target
            fnLoadTargetYearly();

            //LOAD CAMPAIGN INFO
            fnLoadCampaign();

            //////--------------------------------------------------------
            ////fnLoadData_BrandWiseSales();
            ////fnLoadData_CatWiseSales();
            ////fnLoadData_ModelWiseSales();
            ////fnLoadData_NewProduct();
            //////fnLoadData_Stock();
            //--------------------------------------------------------
            fnLoadYearlyDeposit();
            fnLoadMonthlyDeposit();
            fnLoadDailyDeposit();

        }

    }

    protected void fnLoadYearlySalesAmnt()
    {

        SqlConnection conn = DBConnection.GetConnection();

        string gSql = "";
        double tAmnt = 0;
        double wAmnt = 0;

        //---------------------------------------------------------------------------------------------
        // WITHDRAWN AMOUNT
        gSql = "";
        gSql = "SELECT TrType, ISNULL(SUM(NetSalesAmnt),0)/10000000 AS NetAmnt";
        //gSql = gSql + " PARSENAME(CONVERT(VARCHAR, CAST(SUM(NetSalesAmnt)/100000  AS NetAmnt), 1),2)";
        gSql = gSql + " FROM dbo.MRSRMaster";

        gSql = gSql + " WHERE (TrType = -3)";
        gSql = gSql + " AND (TDate >= '" + sFYs + "')";
        gSql = gSql + " AND (TDate <= '" + sFYe + "')";
        //gSql = gSql + " AND (InSource = '" + Session["sBrId"] + "')";

        gSql = gSql + " GROUP BY TrType";

        SqlCommand cmd = new SqlCommand(gSql, conn);
        conn.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            wAmnt = Convert.ToDouble(dr["NetAmnt"].ToString());

        }
        else
        {
            wAmnt = 0;
        }
        dr.Dispose();
        dr.Close();
        conn.Close();

        //---------------------------------------------------------------------------------------------
        // SALES AMOUNT
        gSql = "";
        gSql = "SELECT TrType, ISNULL(SUM(NetSalesAmnt),0)/10000000 AS NetAmnt";
        //gSql = gSql + " PARSENAME(CONVERT(VARCHAR, CAST(SUM(NetSalesAmnt)/100000  AS NetAmnt), 1),2)";
        gSql = gSql + " FROM dbo.MRSRMaster";

        gSql = gSql + " WHERE (TrType = 3)";
        gSql = gSql + " AND (TDate >= '" + sFYs + "')";
        gSql = gSql + " AND (TDate <= '" + sFYe + "')";
        //gSql = gSql + " AND (OutSource = '" + Session["sBrId"] + "')";

        gSql = gSql + " GROUP BY TrType ";

        cmd = new SqlCommand(gSql, conn);
        conn.Open();
        dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            tAmnt = Convert.ToDouble(dr["NetAmnt"].ToString()) - wAmnt;
            //lblYSales.Text = tAmnt.ToString("00.00", CultureInfo.InvariantCulture) + "C";
        }
        else
        {
            //lblYSales.Text = "0";
        }
        dr.Dispose();
        dr.Close();
        conn.Close();
        //---------------------------------------------------------------------------------------------

    }

    protected void fnLoadMonthlySalesAmnt()
    {

        //int monthC = Convert.ToInt16(currentMonth1);
        //int yearC = Convert.ToInt16(currentYear1);

        //sDate = Convert.ToDateTime("" + monthC + "/1/" + yearC + "");
        //eDate = Convert.ToDateTime("" + monthC + "/30/" + yearC + "");
        DateTime date = DateTime.Today;
        var fDayOfMonth = new DateTime(date.Year, date.Month, 1);
        var lDayOfMonth = fDayOfMonth.AddMonths(1).AddDays(-1);

        sDate = Convert.ToDateTime(fDayOfMonth);
        eDate = Convert.ToDateTime(lDayOfMonth);

        SqlConnection conn = DBConnection.GetConnection();

        string gSql = "";
        double tAmnt = 0;
        double wAmnt = 0;

        //----------------------------------------------------------------------------------------------
        // WITHDRAWN AMOUNT
        gSql = "";
        gSql = "SELECT TrType, ISNULL(SUM(NetSalesAmnt),0)/100000 AS NetAmnt";
        //gSql = gSql + " PARSENAME(CONVERT(VARCHAR, CAST(SUM(NetSalesAmnt)/100000  AS NetAmnt), 1),2)";
        gSql = gSql + " FROM dbo.MRSRMaster";

        gSql = gSql + " WHERE (TrType = -3)";
        gSql = gSql + " AND (TDate >= '" + sDate + "')";
        gSql = gSql + " AND (TDate <= '" + eDate + "')";
        //gSql = gSql + " AND (InSource = '" + Session["sBrId"] + "')";

        gSql = gSql + " GROUP BY TrType";

        SqlCommand cmd = new SqlCommand(gSql, conn);
        conn.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            wAmnt = Convert.ToDouble(dr["NetAmnt"].ToString());

        }
        else
        {
            wAmnt = 0;
        }
        dr.Dispose();
        dr.Close();
        conn.Close();

        //----------------------------------------------------------------------------------------------
        // SALES AMOUNT
        gSql = "";
        gSql = "SELECT TrType, ISNULL(SUM(NetSalesAmnt),0)/100000 AS NetAmnt";
        //gSql = gSql + " PARSENAME(CONVERT(VARCHAR, CAST(SUM(NetSalesAmnt)/100000  AS NetAmnt), 1),2)";
        gSql = gSql + " FROM dbo.MRSRMaster";

        gSql = gSql + " WHERE (TrType = 3)";
        gSql = gSql + " AND (TDate >= '" + sDate + "')";
        gSql = gSql + " AND (TDate <= '" + eDate + "')";
        //gSql = gSql + " AND (OutSource = '" + Session["sBrId"] + "')";

        gSql = gSql + " GROUP BY TrType";

        cmd = new SqlCommand(gSql, conn);
        conn.Open();
        dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            tAmnt = Convert.ToDouble(dr["NetAmnt"].ToString()) - wAmnt;
            //lblMSales.Text = tAmnt.ToString("0,0", CultureInfo.InvariantCulture) + "L";
        }
        else
        {
            //lblMSales.Text = "0";
        }
        dr.Dispose();
        dr.Close();
        conn.Close();
        //----------------------------------------------------------------------------------------------


    }

    protected void fnLoadDailySalesAmnt()
    {
        DateTime tDate = DateTime.Today;

        SqlConnection conn = DBConnection.GetConnection();

        string gSql = "";
        double tAmnt = 0;
        double wAmnt = 0;

        //----------------------------------------------------------------------------------------------
        // Withdrawn AMOUNT
        gSql = "";
        gSql = "SELECT TrType, ISNULL(SUM(NetSalesAmnt),0)/100000 AS NetAmnt";
        //gSql = gSql + " PARSENAME(CONVERT(VARCHAR, CAST(SUM(NetSalesAmnt)/100000  AS NetAmnt), 1),2)";
        gSql = gSql + " FROM dbo.MRSRMaster";

        gSql = gSql + " WHERE (TrType = -3)";
        gSql = gSql + " AND (TDate = '" + tDate + "')";
        //gSql = gSql + " AND (InSource = '" + Session["sBrId"] + "')";

        gSql = gSql + " GROUP BY TrType";

        SqlCommand cmd = new SqlCommand(gSql, conn);
        conn.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            wAmnt = Convert.ToDouble(dr["NetAmnt"].ToString());
        }
        else
        {
            wAmnt = 0;
        }
        dr.Dispose();
        dr.Close();
        conn.Close();

        //----------------------------------------------------------------------------------------------
        // SALES AMOUNT
        gSql = "";
        gSql = "SELECT TrType, ISNULL(SUM(NetSalesAmnt),0)/100000 AS NetAmnt";
        //gSql = gSql + " PARSENAME(CONVERT(VARCHAR, CAST(SUM(NetSalesAmnt)/100000  AS NetAmnt), 1),2)";
        gSql = gSql + " FROM dbo.MRSRMaster";

        gSql = gSql + " WHERE (TrType = 3)";
        gSql = gSql + " AND (TDate = '" + tDate + "')";
        //gSql = gSql + " AND (OutSource = '" + Session["sBrId"] + "')";

        gSql = gSql + " GROUP BY TrType";

        cmd = new SqlCommand(gSql, conn);
        conn.Open();
        dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            tAmnt = Convert.ToDouble(dr["NetAmnt"].ToString()) - wAmnt;
            //lblDSales.Text = tAmnt.ToString("0,0", CultureInfo.InvariantCulture) + "L";
        }
        else
        {
            //lblDSales.Text = "0";
        }
        dr.Dispose();
        dr.Close();
        conn.Close();
        //----------------------------------------------------------------------------------------------

    }


    protected void fnLoadTargetYearly()
    {
        int monthC = Convert.ToInt16(currentMonth1);
        int yearC = Convert.ToInt16(currentYear1);

        if (monthC >= 7)
        {
            FYs = yearC;
            FYe = yearC + 1;
        }
        else
        {
            FYs = yearC - 1;
            FYe = yearC;
        }

        //sFYs = Convert.ToDateTime("7/1/" + FYs + "");
        //sFYe = Convert.ToDateTime("6/30/" + FYe + "");

        string sFY = FYs + "-" + FYe + "";

        SqlConnection conn = DBConnection.GetConnection();

        string gSql = "";
        double tAmnt = 0;
        gSql = "SELECT ISNULL(SUM(TAmount),0)/100 AS NetAmnt";
        gSql = gSql + " FROM dbo.tbTargetYearly";

        //gSql = gSql + " WHERE (TrType = 3)";
        gSql = gSql + " WHERE (TYear = '" + sFY + "')";
        //gSql = gSql + " AND (EID = '" + Session["sBrId"] + "')";

        //gSql = gSql + " GROUP BY EntityName";

        SqlCommand cmd = new SqlCommand(gSql, conn);
        conn.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            tAmnt = Convert.ToDouble(dr["NetAmnt"].ToString());
            //tAmnt = 0;
            //lblTargetY.Text = tAmnt.ToString("00", CultureInfo.InvariantCulture) + " C";
        }
        else
        {
            //lblTargetY.Text = "0";
        }
        dr.Dispose();
        dr.Close();
        conn.Close();


    }


    protected void fnLoadCampaign()
    {
        DateTime tDate = DateTime.Today;

        SqlConnection conn = DBConnection.GetConnection();

        string gSql = "";
        gSql = "SELECT  CampaignNo, CampaignName, CONVERT(VARCHAR(10), EffectiveDate, 103) AS EffectiveDate,";
        gSql = gSql + " cTag, cStop, cStopDate";
        gSql = gSql + " FROM  dbo.tbCampaignMaster";
        gSql = gSql + " WHERE (cStop = 0)";
        gSql = gSql + " ORDER BY CamAID DESC";

        SqlCommand cmd = new SqlCommand(gSql, conn);
        conn.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            lblPromotion.Text = dr["CampaignName"].ToString();
            lblPromotionDate.Text = "Start From : " + dr["EffectiveDate"].ToString();

        }
        else
        {
            lblPromotion.Text = "";
            lblPromotionDate.Text = "";
        }
        dr.Dispose();
        dr.Close();
        conn.Close();


    }



    //Grid View Row Format
    protected void gvModelWiseSales_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //CalcTotal(e.Row.Cells[3].Text);
            //CalcTotal1(e.Row.Cells[4].Text);

            //double value3 = Convert.ToDouble(e.Row.Cells[3].Text);
            //e.Row.Cells[3].Text = value3.ToString("0,0");

            //RIGHT ALIGNMENT            
            //e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Right;
        }

    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //CalcTotal(e.Row.Cells[3].Text);
            //CalcTotal1(e.Row.Cells[4].Text);

            //double value3 = Convert.ToDouble(e.Row.Cells[3].Text);
            //e.Row.Cells[3].Text = value3.ToString("0,0");

            //RIGHT ALIGNMENT            
            //e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Right;
        }

    }

    protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //CalcTotal(e.Row.Cells[3].Text);
            //CalcTotal1(e.Row.Cells[4].Text);

            //double value3 = Convert.ToDouble(e.Row.Cells[3].Text);
            //e.Row.Cells[3].Text = value3.ToString("0,0");

            //RIGHT ALIGNMENT            
            //e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Right;
        }

    }

    protected void GridView3_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //CalcTotal(e.Row.Cells[3].Text);
            //CalcTotal1(e.Row.Cells[4].Text);

            //double value3 = Convert.ToDouble(e.Row.Cells[3].Text);
            //e.Row.Cells[3].Text = value3.ToString("0,0");

            //RIGHT ALIGNMENT            
            //e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
            //e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Right;
        }

    }

    protected void BindGrid()
    {
        gvModelWiseSales.DataSource = ViewState["dt"] as DataTable;
        gvModelWiseSales.DataBind();
    }


    //----------------------------------------------------------------------------------------

    /*
    protected void fnLoadData_BrandWiseSales()
    {

        SqlConnection con = DBConnection.GetConnection();
        con.Open();

        DateTime tDate = DateTime.Today;

        //lblDateBrand.Text = tDate.ToString("dd-MMM-yyyy");
        //lblDateBrand.Text = tDate.ToString("MMM-yyyy");
        lblDateBrand.Text = txtFrom2.Text + " To " + txtToDate2.Text;

        DateTime date = DateTime.Today;
        var fDayOfMonth = new DateTime(date.Year, date.Month, 1);
        var lDayOfMonth = fDayOfMonth.AddMonths(1).AddDays(-1);

        sDate = Convert.ToDateTime(fDayOfMonth);
        eDate = Convert.ToDateTime(lDayOfMonth);

        string sSql = "";

        sSql = "";
        sSql = "SELECT dbo.MRSRMaster.TrType,  dbo.Product.PCategory,";
        sSql = sSql + " SUM(ABS(dbo.MRSRDetails.Qty)) AS tQty, SUM(dbo.MRSRDetails.NetAmnt) AS tAmnt";

        sSql = sSql + " FROM dbo.MRSRMaster INNER JOIN";
        sSql = sSql + " dbo.MRSRDetails ON dbo.MRSRMaster.MRSRMID = dbo.MRSRDetails.MRSRMID INNER JOIN";
        sSql = sSql + " dbo.Product ON dbo.MRSRDetails.ProductID = dbo.Product.ProductID";
        //sSql = sSql + " dbo.Category ON dbo.Product.CategoryID = dbo.Category.CategoryID INNER JOIN";
        //sSql = sSql + " dbo.Category AS Category_1 ON dbo.Category.ParentID = Category_1.CategoryID";

        sSql = sSql + " WHERE (dbo.MRSRMaster.TrType = 3)";
        //sSql = sSql + " AND (dbo.MRSRMaster.TDate = '" + tDate + "')";
        //sSql = sSql + " AND (dbo.MRSRMaster.TDate >= '" + sDate + "')";
        //sSql = sSql + " AND (dbo.MRSRMaster.TDate <= '" + eDate + "')";

        sSql = sSql + " AND (dbo.MRSRMaster.TDate >= '" + Convert.ToDateTime(this.txtFrom2.Text) + "')";
        sSql = sSql + " AND (dbo.MRSRMaster.TDate <= '" + Convert.ToDateTime(this.txtToDate2.Text) + "')";

        //sSql = sSql + " AND (dbo.MRSRMaster.OutSource = '" + Session["sBrId"] + "')";

        sSql = sSql + " GROUP BY dbo.MRSRMaster.TrType, dbo.Product.PCategory";
        sSql = sSql + " ORDER BY dbo.Product.PCategory";



        SqlCommand cmd = new SqlCommand(sSql, con);
        dr = cmd.ExecuteReader();

        GridView1.DataSource = dr;
        GridView1.DataBind();
        dr.Close();
        con.Close();
    }

    protected void fnLoadData_CatWiseSales()
    {

        SqlConnection con = DBConnection.GetConnection();
        con.Open();

        DateTime tDate = DateTime.Today;

        //lblDateCatWise.Text = tDate.ToString("dd-MMM-yyyy");
        //lblDateCatWise.Text = tDate.ToString("MMM-yyyy");

        lblDateCatWise.Text = txtFrom.Text + " To " + txtToDate.Text;


        DateTime date = DateTime.Today;
        var fDayOfMonth = new DateTime(date.Year, date.Month, 1);
        var lDayOfMonth = fDayOfMonth.AddMonths(1).AddDays(-1);

        sDate = Convert.ToDateTime(fDayOfMonth);
        eDate = Convert.ToDateTime(lDayOfMonth);


        string sSql = "";

        sSql = "";
        sSql = "SELECT dbo.MRSRMaster.TrType, dbo.Product.GroupName,";
        sSql = sSql + " SUM(ABS(dbo.MRSRDetails.Qty)) AS tQty, SUM(dbo.MRSRDetails.NetAmnt) AS tAmnt";
        sSql = sSql + " FROM dbo.MRSRMaster INNER JOIN";
        sSql = sSql + " dbo.MRSRDetails ON dbo.MRSRMaster.MRSRMID = dbo.MRSRDetails.MRSRMID INNER JOIN";
        sSql = sSql + " dbo.Product ON dbo.MRSRDetails.ProductID = dbo.Product.ProductID";

        sSql = sSql + " WHERE (dbo.MRSRMaster.TrType = 3)";
        //sSql = sSql + " AND (dbo.MRSRMaster.TDate = '" + tDate + "')";
        //sSql = sSql + " AND (dbo.MRSRMaster.TDate >= '" + sDate + "')";
        //sSql = sSql + " AND (dbo.MRSRMaster.TDate <= '" + eDate + "')";

        sSql = sSql + " AND (dbo.MRSRMaster.TDate >= '" + Convert.ToDateTime(this.txtFrom.Text) + "')";
        sSql = sSql + " AND (dbo.MRSRMaster.TDate <= '" + Convert.ToDateTime(this.txtToDate.Text) + "')";


        //sSql = sSql + " AND (dbo.MRSRMaster.OutSource = '" + Session["sBrId"] + "')";

        sSql = sSql + " GROUP BY dbo.MRSRMaster.TrType, dbo.Product.GroupName";
        sSql = sSql + " ORDER BY dbo.Product.GroupName";


        SqlCommand cmd = new SqlCommand(sSql, con);
        dr = cmd.ExecuteReader();

        GridView2.DataSource = dr;
        GridView2.DataBind();
        dr.Close();
        con.Close();
    }

    protected void fnLoadData_ModelWiseSales()
    {

        SqlConnection con = DBConnection.GetConnection();
        con.Open();

        DateTime tDate = DateTime.Today;

        //lblDateModelWise.Text = tDate.ToString("dd-MMM-yyyy");
        //lblDateModelWise.Text = tDate.ToString("MMM-yyyy");
        lblDateModelWise.Text = txtFrom1.Text + " To " + txtToDate1.Text;

        DateTime date = DateTime.Today;
        var fDayOfMonth = new DateTime(date.Year, date.Month, 1);
        var lDayOfMonth = fDayOfMonth.AddMonths(1).AddDays(-1);

        sDate = Convert.ToDateTime(fDayOfMonth);
        eDate = Convert.ToDateTime(lDayOfMonth);


        string sSql = "";

        sSql = "";
        sSql = "SELECT dbo.MRSRMaster.TrType, dbo.Product.Model,";
        sSql = sSql + " SUM(ABS(dbo.MRSRDetails.Qty)) AS tQty, SUM(dbo.MRSRDetails.NetAmnt) AS tAmnt";
        sSql = sSql + " FROM dbo.MRSRMaster INNER JOIN";
        sSql = sSql + " dbo.MRSRDetails ON dbo.MRSRMaster.MRSRMID = dbo.MRSRDetails.MRSRMID INNER JOIN";
        sSql = sSql + " dbo.Product ON dbo.MRSRDetails.ProductID = dbo.Product.ProductID";

        sSql = sSql + " WHERE (dbo.MRSRMaster.TrType = 3)";
        //sSql = sSql + " AND (dbo.MRSRMaster.TDate = '" + tDate + "')";
        //sSql = sSql + " AND (dbo.MRSRMaster.TDate >= '" + sDate + "')";
        //sSql = sSql + " AND (dbo.MRSRMaster.TDate <= '" + eDate + "')";

        sSql = sSql + " AND (dbo.MRSRMaster.TDate >= '" + Convert.ToDateTime(this.txtFrom1.Text) + "')";
        sSql = sSql + " AND (dbo.MRSRMaster.TDate <= '" + Convert.ToDateTime(this.txtToDate1.Text) + "')";

        //sSql = sSql + " AND (dbo.MRSRMaster.OutSource = '" + Session["sBrId"] + "')";

        sSql = sSql + " GROUP BY dbo.MRSRMaster.TrType, dbo.Product.Model";
        sSql = sSql + " ORDER BY dbo.Product.Model";

        SqlCommand cmd = new SqlCommand(sSql, con);
        dr = cmd.ExecuteReader();

        gvModelWiseSales.DataSource = dr;
        gvModelWiseSales.DataBind();
        dr.Close();
        con.Close();

    }
    */

    protected void fnLoadData_NewProduct()
    {

        SqlConnection con = DBConnection.GetConnection();
        con.Open();

        DateTime tDate = DateTime.Today;

        //lblDateStock.Text = tDate.ToString("dd-MMM-yyyy");

        string sSql = "";

        sSql = "";
        sSql = "SELECT TOP (10) ProductID, Code, Model, ProdName, GroupName, UnitPrice, Discontinue";
        sSql = sSql + " FROM  dbo.Product ";

        sSql = sSql + " WHERE (Discontinue = 'No')";
        //sSql = sSql + " AND (dbo.MRSRMaster.TDate = '" + tDate + "')";

        //sSql = sSql + " GROUP BY dbo.MRSRMaster.TrType, dbo.Entity.eName";
        sSql = sSql + " ORDER BY ProductID DESC";


        SqlCommand cmd = new SqlCommand(sSql, con);
        dr = cmd.ExecuteReader();

        GridView3.DataSource = dr;
        GridView3.DataBind();
        dr.Close();
        con.Close();
    }


    protected void fnLoadYearlyDeposit()
    {

        SqlConnection conn = DBConnection.GetConnection();

        string gSql = "";
        double tAmnt = 0;


        //---------------------------------------------------------------------------------------------
        // DEPOSIT AMOUNT
        gSql = "";
        //gSql = "SELECT SUM(ISNULL(DepositAmnt,0))/10000000 AS NetAmnt";
        gSql = "SELECT ISNULL(SUM(DepositAmnt),0)/10000000 AS NetAmnt";
        //gSql = gSql + " PARSENAME(CONVERT(VARCHAR, CAST(SUM(NetSalesAmnt)/100000  AS NetAmnt), 1),2)";
        gSql = gSql + " FROM dbo.tbDeposit";

        //gSql = gSql + " WHERE (TrType = 3)";
        gSql = gSql + " WHERE (DepositDate >= '" + sFYs + "')";
        gSql = gSql + " AND (DepositDate <= '" + sFYe + "')";
        //gSql = gSql + " AND (EID = '" + Session["sBrId"] + "')";

        //gSql = gSql + " GROUP BY EID";

        SqlCommand cmd = new SqlCommand(gSql, conn);
        conn.Open();
        dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            tAmnt = Convert.ToDouble(dr["NetAmnt"].ToString());
            lblYDeposit.Text = tAmnt.ToString("00.00", CultureInfo.InvariantCulture) + "C";
        }
        else
        {
            lblYDeposit.Text = "0";
        }
        dr.Dispose();
        dr.Close();
        conn.Close();
        //---------------------------------------------------------------------------------------------

    }

    protected void fnLoadMonthlyDeposit()
    {

        //int monthC = Convert.ToInt16(currentMonth1);
        //int yearC = Convert.ToInt16(currentYear1);

        //sDate = Convert.ToDateTime("" + monthC + "/1/" + yearC + "");
        //eDate = Convert.ToDateTime("" + monthC + "/30/" + yearC + "");
        DateTime date = DateTime.Today;
        var fDayOfMonth = new DateTime(date.Year, date.Month, 1);
        var lDayOfMonth = fDayOfMonth.AddMonths(1).AddDays(-1);

        sDate = Convert.ToDateTime(fDayOfMonth);
        eDate = Convert.ToDateTime(lDayOfMonth);

        SqlConnection conn = DBConnection.GetConnection();

        string gSql = "";
        double tAmnt = 0;

        //----------------------------------------------------------------------------------------------
        // DEPOSIT AMOUNT
        gSql = "";
        gSql = "SELECT ISNULL(SUM(DepositAmnt), 0)/10000000 AS NetAmnt";
        //gSql = gSql + " PARSENAME(CONVERT(VARCHAR, CAST(SUM(NetSalesAmnt)/100000  AS NetAmnt), 1),2)";
        gSql = gSql + " FROM dbo.tbDeposit";

        //gSql = gSql + " WHERE (TrType = 3)";
        gSql = gSql + " WHERE (DepositDate >= '" + sDate + "')";
        gSql = gSql + " AND (DepositDate <= '" + eDate + "')";


        //gSql = gSql + " AND (EID = '" + Session["sBrId"] + "')";

        //gSql = gSql + " GROUP BY EID";

        SqlCommand cmd = new SqlCommand(gSql, conn);
        conn.Open();
        dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            tAmnt = Convert.ToDouble(dr["NetAmnt"].ToString());
            lblMDeposit.Text = tAmnt.ToString("00", CultureInfo.InvariantCulture) + "C";
        }
        else
        {
            lblMDeposit.Text = "0";
        }
        dr.Dispose();
        dr.Close();
        conn.Close();
        //----------------------------------------------------------------------------------------------


    }

    protected void fnLoadDailyDeposit()
    {
        DateTime tDate = DateTime.Today;

        SqlConnection conn = DBConnection.GetConnection();

        string gSql = "";
        double tAmnt = 0;


        //----------------------------------------------------------------------------------------------
        // DEPOSIT AMOUNT
        gSql = "";
        gSql = "SELECT ISNULL(SUM(DepositAmnt), 0)/100000 AS NetAmnt";
        //gSql = gSql + " PARSENAME(CONVERT(VARCHAR, CAST(SUM(NetSalesAmnt)/100000  AS NetAmnt), 1),2)";
        gSql = gSql + " FROM dbo.tbDeposit";

        //gSql = gSql + " WHERE (TrType = 3)";
        gSql = gSql + " WHERE (DepositDate = '" + tDate + "')";
        //gSql = gSql + " AND (EID = '" + Session["sBrId"] + "')";

        //gSql = gSql + " GROUP BY EID";

        SqlCommand cmd = new SqlCommand(gSql, conn);
        conn.Open();
        dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            tAmnt = Convert.ToDouble(dr["NetAmnt"].ToString());
            lblDDeposit.Text = tAmnt.ToString("00", CultureInfo.InvariantCulture) + "L";
        }
        else
        {
            lblDDeposit.Text = "0";
        }
        dr.Dispose();
        dr.Close();
        conn.Close();
        //----------------------------------------------------------------------------------------------

    }



}