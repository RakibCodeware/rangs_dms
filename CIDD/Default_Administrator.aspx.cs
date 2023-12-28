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

    private double runningTotalQtyCat = 0;
    private double runningTotalAmntCat = 0;
    private double runningTotalAmntSumm=0;
    private double runningTotalQtyM = 0;
    private double runningTotalAmntM = 0;
    private double runningTotalQtyB = 0;
    private double runningTotalAmntB = 0;

    private double runningTotalQtyCTP1 = 0;
    private double runningTotalAmntCTP1 = 0;

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

            try
            {
                sFYs = Convert.ToDateTime("7/1/" + FYs + "");
                sFYe = Convert.ToDateTime("6/30/" + FYe + "");

                //this.lblText.Text = "Welcome to " + Session["eName"].ToString() + " ... FY : " + FYs + "-" + FYe + "";
                this.lblText.Text = "Welcome to Rangs Electronics Ltd ... FY : " + FYs + "-" + FYe + "";
                //this.lblCTP.Text = Session["eName"].ToString();                       

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

                //--------------------------------------------------------

                DateTime date = DateTime.Today;
                var fDayOfMonth = new DateTime(date.Year, date.Month, 1);
                var lDayOfMonth = fDayOfMonth.AddMonths(1).AddDays(-1);

                sDate = Convert.ToDateTime(fDayOfMonth);
                eDate = Convert.ToDateTime(lDayOfMonth);


                this.txtFrom2.Text = sDate.ToString("MM/dd/yyyy");
                this.txtToDate2.Text = DateTime.Today.ToString("MM/dd/yyyy");
                fnLoadData_BrandWiseSales();


                this.txtFrom.Text = sDate.ToString("MM/dd/yyyy");
                //this.txtToDate.Text = eDate.ToString("MM/dd/yyyy");
                this.txtToDate.Text = DateTime.Today.ToString("MM/dd/yyyy");
                fnLoadData_CatWiseSales();

                this.txtFrom1.Text = sDate.ToString("MM/dd/yyyy");
                this.txtToDate1.Text = DateTime.Today.ToString("MM/dd/yyyy");
                fnLoadData_ModelWiseSales();

                this.txtFromDateCTP1.Text = DateTime.Today.ToString("MM/dd/yyyy");
                this.txtToDateCTP1.Text = DateTime.Today.ToString("MM/dd/yyyy");
                fnLoadData_CTPWiseSales();


                this.txtFromDateSS1.Text = DateTime.Today.ToString("MM/dd/yyyy");
                this.txtToDateSS1.Text = DateTime.Today.ToString("MM/dd/yyyy");
                fnLoadData_ChannelTypeWiseSales();


                this.txtFromDateDealer1.Text = DateTime.Today.ToString("MM/dd/yyyy");
                this.txtToDateDealer1.Text = DateTime.Today.ToString("MM/dd/yyyy");
                fnLoadDealerSale_ctpWise();


                fnLoadData_NewProduct();
                //fnLoadData_Stock();
                //--------------------------------------------------------
                fnLoadYearlyDeposit();
                fnLoadMonthlyDeposit();
                fnLoadDailyDeposit();


                //LOAD DEALER OUTSTANDING
                fnLoadTotalOutStanding();


            }
            catch
            {
                //
            }

        }
    }

    protected void fnLoadTotalOutStanding()
    {
        if (System.Convert.ToInt32(Session["Vis"]) == 0)
        {
            Response.Redirect("~/LogIn.aspx");
        }

        SqlConnection conn = DBConnectionDSM.GetConnection();
        SqlConnection conn2 = DBConnectionDSM.GetConnection();

        string gSql = "";

        ////---------------------------------------------------------------------------------------------
        //// WITHDRAWN AMOUNT
        //gSql = "";
        //gSql = "SELECT TrType, InSource, SUM(NetSalesAmnt)/100000 AS NetAmnt";
        ////gSql = gSql + " PARSENAME(CONVERT(VARCHAR, CAST(SUM(NetSalesAmnt)/100000  AS NetAmnt), 1),2)";
        //gSql = gSql + " FROM dbo.MRSRMaster";

        //gSql = gSql + " WHERE (TrType = -3)";
        //gSql = gSql + " AND (TDate >= '" + sFYs + "')";
        //gSql = gSql + " AND (TDate <= '" + sFYe + "')";
        //gSql = gSql + " AND (InSource = '" + Session["sBrId"] + "')";

        //gSql = gSql + " GROUP BY TrType, InSource";

        //SqlCommand cmd = new SqlCommand(gSql, conn);
        //conn.Open();
        //SqlDataReader dr = cmd.ExecuteReader();
        //if (dr.Read())
        //{
        //    wAmnt = Convert.ToDouble(dr["NetAmnt"].ToString());
        //}
        //else
        //{
        //    wAmnt = 0;
        //}
        //dr.Dispose();
        //dr.Close();
        //conn.Close();

        ////---------------------------------------------------------------------------------------------
        //// SALES AMOUNT
        //gSql = "";
        //gSql = "SELECT TrType, OutSource, SUM(NetSalesAmnt)/100000 AS NetAmnt";
        ////gSql = gSql + " PARSENAME(CONVERT(VARCHAR, CAST(SUM(NetSalesAmnt)/100000  AS NetAmnt), 1),2)";
        //gSql = gSql + " FROM dbo.MRSRMaster";

        //gSql = gSql + " WHERE (TrType = 3)";
        //gSql = gSql + " AND (TDate >= '" + sFYs + "')";
        //gSql = gSql + " AND (TDate <= '" + sFYe + "')";
        //gSql = gSql + " AND (OutSource = '" + Session["sBrId"] + "')";

        //gSql = gSql + " GROUP BY TrType, OutSource";

        //cmd = new SqlCommand(gSql, conn);
        //conn.Open();
        //dr = cmd.ExecuteReader();
        //if (dr.Read())
        //{
        //    tAmnt = Convert.ToDouble(dr["NetAmnt"].ToString()) - wAmnt;
        //    lblYSales.Text = tAmnt.ToString("00.00", CultureInfo.InvariantCulture) + "L";
        //}
        //else
        //{
        //    lblYSales.Text = "0";
        //}
        //dr.Dispose();
        //dr.Close();
        //conn.Close();
        //---------------------------------------------------------------------------------------------
        //0000000000000000

        string toDate = DateTime.Today.ToString("MM/dd/yyyy");
        string sPC = Request.UserHostAddress;

        if (Session["sUser"] == null)
        {
            Session["sUser"] = "0";
        }
        if (Session["sZoneID"] == null)
        {
            Session["sZoneID"] = "0";
        }

        string gSQL = "";
        gSQL = "DELETE FROM TempOpening";
        gSQL = gSQL + " WHERE UserID='" + Session["sUser"].ToString() + "'";
        //gSQL = gSQL + " WHERE UserID='" + Session["sUser"].ToString() + "' AND PCName='" + sPC + "'";
        SqlCommand cmd2 = new SqlCommand(gSQL, conn2);
        conn2.Open();
        cmd2.ExecuteNonQuery();
        conn2.Close();

        //'------------------------------------------------------------------------------------------
        gSQL = "";
        gSQL = "SELECT  CatName AS ZoneName, CategoryID ";
        gSQL = gSQL + " FROM Zone";
        gSQL = gSQL + " WHERE ZoneType='1'";
        //gSQL = gSQL + " WHERE CategoryID='" + Session["sZoneID"].ToString() + "'";      
        gSQL = gSQL + " GROUP BY CatName, CategoryID";
        SqlCommand cmd = new SqlCommand(gSQL, conn);
        conn.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            gSQL = "";
            gSQL = "INSERT INTO TempOpening(ZoneName,DelearID,";
            gSQL = gSQL + " UserID,PCName) VALUES(";
            gSQL = gSQL + " '" + dr["ZoneName"].ToString() + "','" + dr["CategoryID"].ToString() + "',";
            gSQL = gSQL + " '" + Session["sUser"].ToString() + "','" + sPC + "')";
            cmd2 = new SqlCommand(gSQL, conn2);
            conn2.Open();
            cmd2.ExecuteNonQuery();
            conn2.Close();
        }
        conn.Close();
        //'-------------------------------------------------------------------------------------------


        //'-------------------------------------------------------------------------------------------
        //'OPENING
        //'===========================================================================================
        //'LOAD SALES DATA
        gSQL = "";
        gSQL = "SELECT SUM(ISNULL(dbo.MRSRMaster.NetSalesAmnt, 0)) AS NetSalesAmnt, dbo.[Zone].CatName AS ZoneName";
        gSQL = gSQL + " FROM dbo.MRSRMaster INNER JOIN";
        gSQL = gSQL + " dbo.[Zone] ON dbo.MRSRMaster.OutSource = dbo.[Zone].CategoryID";
        gSQL = gSQL + " Where (dbo.MRSRMaster.TrType = 3)";
        gSQL = gSQL + " AND dbo.MRSRMaster.TDate<'" + toDate + "'";
        gSQL = gSQL + " AND dbo.[Zone].ZoneType='1'";
        //gSQL = gSQL + " AND dbo.[Zone].CategoryID='" + Session["sZoneID"].ToString() + "'"; 
        gSQL = gSQL + " GROUP BY dbo.[Zone].CatName";
        cmd = new SqlCommand(gSQL, conn);
        conn.Open();
        dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            gSQL = "";
            gSQL = "UPDATE TempOpening SET OpeningSalesAmnt=" + dr["NetSalesAmnt"].ToString() + "";
            gSQL = gSQL + " WHERE ZoneName='" + dr["ZoneName"].ToString() + "'";
            gSQL = gSQL + " AND UserID='" + Session["sUser"].ToString() + "'";
            gSQL = gSQL + " AND PCName='" + sPC + "'";
            cmd2 = new SqlCommand(gSQL, conn2);
            conn2.Open();
            cmd2.ExecuteNonQuery();
            conn2.Close();
        }
        conn.Close();
        //'-------------------------------------------------------------------------------------------


        //'-------------------------------------------------------------------------------------------        
        //'LOAD OPENING DEPOSIT
        gSQL = "";
        gSQL = "SELECT ZoneName, SUM(cAmount) AS cAmount";
        gSQL = gSQL + " From dbo.VW_Deposit_Info";
        gSQL = gSQL + " WHERE CDate<'" + toDate + "'";
        //gSQL = gSQL + " AND CategoryID='" + Session["sZoneID"].ToString() + "'"; 
        gSQL = gSQL + " GROUP BY ZoneName";
        cmd = new SqlCommand(gSQL, conn);
        conn.Open();
        dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            gSQL = "";
            gSQL = " UPDATE TempOpening SET OpeningCollection=" + dr["cAmount"].ToString() + "";
            gSQL = gSQL + " WHERE ZoneName='" + dr["ZoneName"].ToString() + "'";
            gSQL = gSQL + " AND UserID='" + Session["sUser"].ToString() + "'";
            gSQL = gSQL + " AND PCName='" + sPC + "'";
            cmd2 = new SqlCommand(gSQL, conn2);
            conn2.Open();
            cmd2.ExecuteNonQuery();
            conn2.Close();
        }
        conn.Close();
        //'-------------------------------------------------------------------------------------------

        //'-------------------------------------------------------------------------------------------
        //'LOAD DISHONOUR
        gSQL = "";
        gSQL = "SELECT ZoneName, SUM(cAmount) AS cAmount";
        gSQL = gSQL + " From VW_DishonourAmnt";
        gSQL = gSQL + " WHERE CDate<'" + toDate + "'";
        //gSQL = gSQL + " AND CategoryID='" + Session["sZoneID"].ToString() + "'"; 
        gSQL = gSQL + " GROUP BY ZoneName";
        cmd = new SqlCommand(gSQL, conn);
        conn.Open();
        dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            gSQL = "";
            gSQL = " UPDATE TempOpening SET OpeningDishonour=" + dr["cAmount"].ToString() + "";
            gSQL = gSQL + " WHERE ZoneName='" + dr["ZoneName"].ToString() + "'";
            gSQL = gSQL + " AND UserID='" + Session["sUser"].ToString() + "'";
            gSQL = gSQL + " AND PCName='" + sPC + "'";
            cmd2 = new SqlCommand(gSQL, conn2);
            conn2.Open();
            cmd2.ExecuteNonQuery();
            conn2.Close();
        }
        conn.Close();
        //'-------------------------------------------------------------------------------------------


        //'-------------------------------------------------------------------------------------------
        //'LOAD WITHDRAWN
        gSQL = "";
        gSQL = "SELECT SUM(ISNULL(dbo.MRSRMaster.NetSalesAmnt, 0)) AS NetSalesAmnt, ";
        gSQL = gSQL + " dbo.[Zone].CatName AS ZoneName";
        gSQL = gSQL + " FROM dbo.MRSRMaster INNER JOIN";
        gSQL = gSQL + " dbo.[Zone] ON dbo.MRSRMaster.InSource = dbo.[Zone].CategoryID";
        gSQL = gSQL + " Where (dbo.MRSRMaster.TrType = -3)";
        gSQL = gSQL + " AND dbo.[Zone].ZoneType='1'";
        //gSQL = gSQL + " AND dbo.[Zone].CategoryID='" + Session["sZoneID"].ToString() + "'"; 
        gSQL = gSQL + " AND dbo.MRSRMaster.TDate<'" + toDate + "'";
        gSQL = gSQL + " GROUP BY dbo.[Zone].CatName";
        cmd = new SqlCommand(gSQL, conn);
        conn.Open();
        dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            gSQL = "";
            gSQL = " UPDATE TempOpening SET OpenigWithdrawn=" + dr["NetSalesAmnt"].ToString() + "";
            gSQL = gSQL + " WHERE ZoneName='" + dr["ZoneName"].ToString() + "'";
            gSQL = gSQL + " AND UserID='" + Session["sUser"].ToString() + "'";
            gSQL = gSQL + " AND PCName='" + sPC + "'";
            cmd2 = new SqlCommand(gSQL, conn2);
            conn2.Open();
            cmd2.ExecuteNonQuery();
            conn2.Close();
        }
        conn.Close();


        //'==================================================================================================
        //'CURRENT TRANSACTION
        //'LOAD SALES DATA
        gSQL = "";
        gSQL = "SELECT SUM(ISNULL(dbo.MRSRMaster.NetSalesAmnt, 0)) AS NetSalesAmnt, dbo.[Zone].CatName AS ZoneName";
        gSQL = gSQL + " FROM dbo.MRSRMaster INNER JOIN";
        gSQL = gSQL + " dbo.[Zone] ON dbo.MRSRMaster.OutSource = dbo.[Zone].CategoryID";
        gSQL = gSQL + " Where (dbo.MRSRMaster.TrType = 3)";
        //gSQL = gSQL + " AND dbo.[Zone].CategoryID='" + Session["sZoneID"].ToString() + "'"; 
        gSQL = gSQL + " AND dbo.MRSRMaster.TDate>='" + toDate + "'";
        gSQL = gSQL + " AND dbo.MRSRMaster.TDate<='" + toDate + "'";
        gSQL = gSQL + " AND dbo.[Zone].ZoneType='1'";
        gSQL = gSQL + " GROUP BY dbo.[Zone].CatName";
        cmd = new SqlCommand(gSQL, conn);
        conn.Open();
        dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            gSQL = "";
            gSQL = " UPDATE TempOpening SET SalesAmnt=" + dr["NetSalesAmnt"].ToString() + "";
            gSQL = gSQL + " WHERE ZoneName='" + dr["ZoneName"].ToString() + "'";
            gSQL = gSQL + " AND UserID='" + Session["sUser"].ToString() + "'";
            gSQL = gSQL + " AND PCName='" + sPC + "'";
            cmd2 = new SqlCommand(gSQL, conn2);
            conn2.Open();
            cmd2.ExecuteNonQuery();
            conn2.Close();
        }
        conn.Close();
        //'-------------------------------------------------------------------------------------------


        //'-------------------------------------------------------------------------------------------        
        //'LOAD DEPOSIT
        gSQL = "";
        gSQL = "SELECT ZoneName, SUM(cAmount) AS cAmount";
        gSQL = gSQL + " From dbo.VW_Deposit_Info";
        gSQL = gSQL + " WHERE CDate>='" + toDate + "'";
        //gSQL = gSQL + " AND CategoryID='" + Session["sZoneID"].ToString() + "'"; 
        gSQL = gSQL + " AND CDate<='" + toDate + "'";
        gSQL = gSQL + " GROUP BY ZoneName";
        cmd = new SqlCommand(gSQL, conn);
        conn.Open();
        dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            gSQL = "";
            gSQL = " UPDATE TempOpening SET Collection=" + dr["cAmount"].ToString() + "";
            gSQL = gSQL + " WHERE ZoneName='" + dr["ZoneName"].ToString() + "'";
            gSQL = gSQL + " AND UserID='" + Session["sUser"].ToString() + "'";
            gSQL = gSQL + " AND PCName='" + sPC + "'";
            cmd2 = new SqlCommand(gSQL, conn2);
            conn2.Open();
            cmd2.ExecuteNonQuery();
            conn2.Close();
        }
        conn.Close();
        //'-------------------------------------------------------------------------------------------

        //'-------------------------------------------------------------------------------------------
        //'LOAD DISHONOUR
        gSQL = "";
        gSQL = "SELECT ZoneName, SUM(cAmount) AS cAmount";
        gSQL = gSQL + " From dbo.VW_DishonourAmnt";
        gSQL = gSQL + " WHERE CDate>='" + toDate + "'";
        gSQL = gSQL + " AND CDate<='" + toDate + "'";
        //gSQL = gSQL + " AND CategoryID='" + Session["sZoneID"].ToString() + "'"; 
        gSQL = gSQL + " GROUP BY ZoneName";
        cmd = new SqlCommand(gSQL, conn);
        conn.Open();
        dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            gSQL = "";
            gSQL = " UPDATE TempOpening SET DishonourAmnt=" + dr["cAmount"].ToString() + "";
            gSQL = gSQL + " WHERE ZoneName='" + dr["ZoneName"].ToString() + "'";
            gSQL = gSQL + " AND UserID='" + Session["sUser"].ToString() + "'";
            gSQL = gSQL + " AND PCName='" + sPC + "'";
            cmd2 = new SqlCommand(gSQL, conn2);
            conn2.Open();
            cmd2.ExecuteNonQuery();
            conn2.Close();
        }
        conn.Close();
        //'-------------------------------------------------------------------------------------------


        //'-------------------------------------------------------------------------------------------
        //'LOAD WITHDRAWN
        gSQL = "";
        gSQL = "SELECT SUM(ISNULL(dbo.MRSRMaster.NetSalesAmnt, 0)) AS NetSalesAmnt, ";
        gSQL = gSQL + " dbo.[Zone].CatName AS ZoneName";
        gSQL = gSQL + " FROM dbo.MRSRMaster INNER JOIN";
        gSQL = gSQL + " dbo.[Zone] ON dbo.MRSRMaster.InSource = dbo.[Zone].CategoryID";
        gSQL = gSQL + " Where (dbo.MRSRMaster.TrType = -3)";
        //gSQL = gSQL + " AND dbo.[Zone].CategoryID='" + Session["sZoneID"].ToString() + "'"; 
        gSQL = gSQL + " AND dbo.MRSRMaster.TDate>='" + toDate + "'";
        gSQL = gSQL + " AND dbo.MRSRMaster.TDate<='" + toDate + "'";
        gSQL = gSQL + " AND dbo.[Zone].ZoneType='1'";
        gSQL = gSQL + " GROUP BY dbo.[Zone].CatName";
        cmd = new SqlCommand(gSQL, conn);
        conn.Open();
        dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            gSQL = "";
            gSQL = " UPDATE TempOpening SET Withdrawn=" + dr["NetSalesAmnt"].ToString() + "";
            gSQL = gSQL + " WHERE ZoneName='" + dr["ZoneName"].ToString() + "'";
            gSQL = gSQL + " AND UserID='" + Session["sUser"].ToString() + "'";
            gSQL = gSQL + " AND PCName='" + sPC + "'";
            cmd2 = new SqlCommand(gSQL, conn2);
            conn2.Open();
            cmd2.ExecuteNonQuery();
            conn2.Close();
        }
        conn.Close();
        //'-------------------------------------------------------------------------------------------


        double dOB = 0;
        double dCB = 0;
        double ddCB = 0;
        //LOAD TOTAL
        gSQL = "";
        //gSQL = "select * from TempOpening";
        //gSQL = gSQL + " WHERE UserID='" + Session["sUser"].ToString() + "' AND PCName='" + sPC + "'";

        gSQL = "SELECT UserID, SUM(OpeningSalesAmnt) AS OpeningSalesAmnt, SUM(OpeningCollection) AS OpeningCollection,";
        gSQL = gSQL + " SUM(OpenigWithdrawn) AS OpenigWithdrawn, SUM(OpeningDishonour) AS OpeningDishonour,";
        gSQL = gSQL + " SUM(SalesAmnt) AS SalesAmnt, SUM(Collection) AS Collection, SUM(Withdrawn) AS Withdrawn,";
        gSQL = gSQL + " SUM(DishonourAmnt) AS DishonourAmnt";
        gSQL = gSQL + " FROM dbo.TempOpening";
        gSQL = gSQL + " WHERE UserID='" + Session["sUser"].ToString() + "' AND PCName='" + sPC + "'";
        gSQL = gSQL + " GROUP BY UserID";

        cmd = new SqlCommand(gSQL, conn);
        conn.Open();
        dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            dOB = Convert.ToDouble(dr["OpeningSalesAmnt"].ToString()) - Convert.ToDouble(dr["OpeningCollection"].ToString()) - Convert.ToDouble(dr["OpenigWithdrawn"].ToString()) + Convert.ToDouble(dr["OpeningDishonour"].ToString());
            dCB = dOB + Convert.ToDouble(dr["SalesAmnt"].ToString()) - Convert.ToDouble(dr["Collection"].ToString()) - Convert.ToDouble(dr["Withdrawn"].ToString()) + Convert.ToDouble(dr["DishonourAmnt"].ToString());
            ddCB = dCB / 10000000;
            //lblOutstanding.Text = Convert.ToString(dCB / 100000) + " L";
            lblOutstanding.Text = ddCB.ToString("00.00", CultureInfo.InvariantCulture) + "C";

        }
        conn.Close();

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
            lblYSales.Text = tAmnt.ToString("00.00", CultureInfo.InvariantCulture) + "C";
        }
        else
        {
            lblYSales.Text = "0";
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
            lblMSales.Text = tAmnt.ToString("0,0", CultureInfo.InvariantCulture) + "L";
        }
        else
        {
            lblMSales.Text = "0";
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
            lblDSales.Text = tAmnt.ToString("0,0", CultureInfo.InvariantCulture) + "L";
        }
        else
        {
            lblDSales.Text = "0";
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
        gSql = "";
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
            lblTargetY.Text = tAmnt.ToString("00", CultureInfo.InvariantCulture) + " C";
        }
        else
        {
            lblTargetY.Text = "0";
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
        gSql = "SELECT  CampaignNo, CampaignName,";
        gSql = gSql + " CONVERT(VARCHAR(10), EffectiveDate, 103) AS EffectiveDate,";
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
        sSql = sSql + " SUM(ABS(dbo.MRSRDetails.Qty)) AS tQty, SUM(ISNULL(dbo.MRSRDetails.NetAmnt,0)) AS tAmnt";

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
        sSql = sSql + " SUM(ABS(dbo.MRSRDetails.Qty)) AS tQty, SUM(ISNULL(dbo.MRSRDetails.NetAmnt,0)) AS tAmnt";
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
        sSql = sSql + " SUM(ABS(dbo.MRSRDetails.Qty)) AS tQty, SUM(ISNULL(dbo.MRSRDetails.NetAmnt,0)) AS tAmnt";
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
        gSql = "SELECT SUM(ISNULL(DepositAmnt,0))/10000000 AS NetAmnt";
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



    protected void btnCatSearch_Click(object sender, ImageClickEventArgs e)
    {
        fnLoadData_CatWiseSales();
    }



    protected void btnBrandSearch_Click(object sender, ImageClickEventArgs e)
    {
        fnLoadData_BrandWiseSales();
    }


    protected void btnModelSearch_Click(object sender, ImageClickEventArgs e)
    {
        fnLoadData_ModelWiseSales();
    }


    //Grid View Footer Total FOR CATEGORY
    protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CalcTotalQty_cat(e.Row.Cells[2].Text);
            CalcTotal_TP_cat(e.Row.Cells[3].Text);            
                       
            double value4 = Convert.ToDouble(e.Row.Cells[2].Text);
            e.Row.Cells[2].Text = value4.ToString("0");

            double value5 = Convert.ToDouble(e.Row.Cells[3].Text);
            e.Row.Cells[3].Text = value5.ToString("0");

           
            //RIGHT ALIGNMENT            
            e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Right;
            
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {

            e.Row.Cells[1].Text = "Total";
            //e.Row.Cells[10].Text = string.Format("{0:c}", runningTotal);
            e.Row.Cells[2].Text = runningTotalQtyCat.ToString("0,0", CultureInfo.InvariantCulture);
            e.Row.Cells[3].Text = runningTotalAmntCat.ToString("0,0", CultureInfo.InvariantCulture);
            
            //this.lblNetAmnt.Text = runningTotal.ToString();
            
            //RIGHT ALIGNMENT
            e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Right;
            
        }
    }

    //CALCULATE NET CATEGORY AMOUNT
    private void CalcTotalQty_cat(string _price)
    {
        try
        {
            runningTotalQtyCat += Double.Parse(_price);
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write(ex.ToString());
        }
    }

    //CALCULATE TOTAL  CATEGORY AMOUNT
    private void CalcTotal_TP_cat(string _price)
    {
        try
        {
            runningTotalAmntCat += Double.Parse(_price);
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write(ex.ToString());
        }
    }


    //Grid View Footer Total FOR MODEL
    protected void gvModelWiseSales_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CalcTotalQty_M(e.Row.Cells[2].Text);
            CalcTotal_TP_M(e.Row.Cells[3].Text);

            double value4 = Convert.ToDouble(e.Row.Cells[2].Text);
            e.Row.Cells[2].Text = value4.ToString("0");

            double value5 = Convert.ToDouble(e.Row.Cells[3].Text);
            e.Row.Cells[3].Text = value5.ToString("0");


            //RIGHT ALIGNMENT            
            e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Right;

        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {

            e.Row.Cells[1].Text = "Total";
            //e.Row.Cells[10].Text = string.Format("{0:c}", runningTotal);
            e.Row.Cells[2].Text = runningTotalQtyM.ToString("0,0", CultureInfo.InvariantCulture);
            e.Row.Cells[3].Text = runningTotalAmntM.ToString("0,0", CultureInfo.InvariantCulture);

            //this.lblNetAmnt.Text = runningTotal.ToString();

            //RIGHT ALIGNMENT
            e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Right;

        }

    }


    //CALCULATE NET MODEL AMOUNT
    private void CalcTotalQty_M(string _price)
    {
        try
        {
            runningTotalQtyM += Double.Parse(_price);
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write(ex.ToString());
        }
    }

    //CALCULATE TOTAL  MODEL AMOUNT
    private void CalcTotal_TP_M(string _price)
    {
        try
        {
            runningTotalAmntM += Double.Parse(_price);
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write(ex.ToString());
        }
    }


    //Grid View Footer Total for BRAND
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CalcTotalQty_B(e.Row.Cells[2].Text);
            CalcTotal_TP_B(e.Row.Cells[3].Text);

            double value4 = Convert.ToDouble(e.Row.Cells[2].Text);
            e.Row.Cells[2].Text = value4.ToString("0");

            double value5 = Convert.ToDouble(e.Row.Cells[3].Text);
            e.Row.Cells[3].Text = value5.ToString("0");


            //RIGHT ALIGNMENT            
            e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Right;

        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[1].Text = "Total";
            //e.Row.Cells[10].Text = string.Format("{0:c}", runningTotal);
            e.Row.Cells[2].Text = runningTotalQtyB.ToString("0,0", CultureInfo.InvariantCulture);
            e.Row.Cells[3].Text = runningTotalAmntB.ToString("0,0", CultureInfo.InvariantCulture);

            //this.lblNetAmnt.Text = runningTotal.ToString();

            //RIGHT ALIGNMENT
            e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Right;

        }

    }


    //CALCULATE NET BRAND AMOUNT
    private void CalcTotalQty_B(string _price)
    {
        try
        {
            runningTotalQtyB += Double.Parse(_price);
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write(ex.ToString());
        }
    }

    //CALCULATE TOTAL  BRAND AMOUNT
    private void CalcTotal_TP_B(string _price)
    {
        try
        {
            runningTotalAmntB += Double.Parse(_price);
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write(ex.ToString());
        }
    }

    protected void fnLoadData_CTPWiseSales()
    {

        SqlConnection con = DBConnection.GetConnection();
        con.Open();

        DateTime tDate = DateTime.Today;

        //lblDateCTPWise.Text = tDate.ToString("dd-MMM-yyyy");
        lblCTP1.Text = txtFromDateCTP1.Text + " To " + txtToDateCTP1.Text;

        string sSql = "";

        sSql = "";
        sSql = "SELECT dbo.MRSRMaster.TrType, dbo.Entity.eName,";
        sSql = sSql + " SUM(ABS(dbo.MRSRDetails.Qty)) AS tQty, SUM(dbo.MRSRDetails.NetAmnt) AS tAmnt";

        sSql = sSql + " FROM dbo.MRSRMaster INNER JOIN";
        sSql = sSql + " dbo.MRSRDetails ON dbo.MRSRMaster.MRSRMID = dbo.MRSRDetails.MRSRMID INNER JOIN";
        sSql = sSql + " dbo.Entity ON dbo.MRSRMaster.OutSource = dbo.Entity.EID";

        sSql = sSql + " WHERE (dbo.MRSRMaster.TrType = 3)";
        //sSql = sSql + " AND (dbo.MRSRMaster.TDate = '" + tDate + "')";
        sSql = sSql + " AND (dbo.MRSRMaster.TDate >= '" + Convert.ToDateTime(this.txtFromDateCTP1.Text) + "')";
        sSql = sSql + " AND (dbo.MRSRMaster.TDate <= '" + Convert.ToDateTime(this.txtToDateCTP1.Text) + "')";

        sSql = sSql + " GROUP BY dbo.MRSRMaster.TrType, dbo.Entity.eName";
        sSql = sSql + " ORDER BY dbo.Entity.eName";


        SqlCommand cmd = new SqlCommand(sSql, con);
        dr = cmd.ExecuteReader();

        GridView4.DataSource = dr;
        GridView4.DataBind();
        dr.Close();
        con.Close();

    }

    protected void btnCTP1Search_Click(object sender, ImageClickEventArgs e)
    {
        fnLoadData_CTPWiseSales();
    }


    protected void btnCTPSearch_Click(object sender, ImageClickEventArgs e)
    {
        //
    }

    //Grid View Footer Total FOR CTP Wise
    protected void GridView4_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CalcTotalQty_CTP1(e.Row.Cells[2].Text);
            CalcTotal_TP_CTP1(e.Row.Cells[3].Text);

            double value4 = Convert.ToDouble(e.Row.Cells[2].Text);
            e.Row.Cells[2].Text = value4.ToString("0,0");

            double value5 = Convert.ToDouble(e.Row.Cells[3].Text);
            e.Row.Cells[3].Text = value5.ToString("0,0");


            //RIGHT ALIGNMENT            
            e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Right;

        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {

            e.Row.Cells[1].Text = "Total";
            //e.Row.Cells[10].Text = string.Format("{0:c}", runningTotal);
            e.Row.Cells[2].Text = runningTotalQtyCTP1.ToString("0,0", CultureInfo.InvariantCulture);
            e.Row.Cells[3].Text = runningTotalAmntCTP1.ToString("0,0", CultureInfo.InvariantCulture);

            //this.lblNetAmnt.Text = runningTotal.ToString();

            //RIGHT ALIGNMENT
            e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Right;

        }

    }

    //Grid View Footer Total FOR CTP Wise
    protected void GridView5_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CalcTotalQty_CTP1(e.Row.Cells[2].Text);
            CalcTotal_TP_CTP1(e.Row.Cells[3].Text);

            double value4 = Convert.ToDouble(e.Row.Cells[2].Text);
            e.Row.Cells[2].Text = value4.ToString("0,0");

            double value5 = Convert.ToDouble(e.Row.Cells[3].Text);
            e.Row.Cells[3].Text = value5.ToString("0,0");


            //RIGHT ALIGNMENT            
            e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Right;

        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {

            e.Row.Cells[1].Text = "Total";
            //e.Row.Cells[10].Text = string.Format("{0:c}", runningTotal);
            e.Row.Cells[2].Text = runningTotalQtyM.ToString("0,0", CultureInfo.InvariantCulture);
            e.Row.Cells[3].Text = runningTotalAmntM.ToString("0,0", CultureInfo.InvariantCulture);

            //this.lblNetAmnt.Text = runningTotal.ToString();

            //RIGHT ALIGNMENT
            e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Right;

        }

    }


    //CALCULATE NET CTP QTY
    private void CalcTotalQty_CTP1(string _price)
    {
        try
        {
            runningTotalQtyCTP1 += Double.Parse(_price);
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write(ex.ToString());
        }
    }

    //CALCULATE TOTAL CTP AMOUNT
    private void CalcTotal_TP_CTP1(string _price)
    {
        try
        {
            runningTotalAmntCTP1 += Double.Parse(_price);
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write(ex.ToString());
        }
    }


    protected void imgBtnSS_Click(object sender, ImageClickEventArgs e)
    {
        fnLoadData_ChannelTypeWiseSales();
    }

    protected void fnLoadData_ChannelTypeWiseSales()
    {

        SqlConnection con = DBConnection.GetConnection();
        con.Open();

        DateTime tDate = DateTime.Today;

        //lblDateCTPWise.Text = tDate.ToString("dd-MMM-yyyy");
        lblSummaryDate.Text = txtFromDateSS1.Text + " To " + txtToDateSS1.Text;

        string sSql = "";

        sSql = "";
        //sSql = "SELECT dbo.MRSRMaster.TrType, ";
        //sSql = sSql + " dbo.Entity.EntityType, SUM(dbo.MRSRMaster.NetSalesAmnt) AS tAmnt";

        //sSql = sSql + " FROM dbo.MRSRMaster INNER JOIN";
        ////sSql = sSql + " dbo.MRSRDetails ON dbo.MRSRMaster.MRSRMID = dbo.MRSRDetails.MRSRMID INNER JOIN";
        //sSql = sSql + " dbo.Entity ON dbo.MRSRMaster.OutSource = dbo.Entity.EID";

        //sSql = sSql + " WHERE (dbo.MRSRMaster.TrType = 3)";
        ////sSql = sSql + " AND (dbo.MRSRMaster.TDate = '" + tDate + "')";
        //sSql = sSql + " AND (dbo.MRSRMaster.TDate >= '" + Convert.ToDateTime(this.txtFromDateSS1.Text) + "')";
        //sSql = sSql + " AND (dbo.MRSRMaster.TDate <= '" + Convert.ToDateTime(this.txtToDateSS1.Text) + "')";

        //sSql = sSql + " GROUP BY dbo.MRSRMaster.TrType, dbo.Entity.EntityType";
        //sSql = sSql + " ORDER BY dbo.Entity.EntityType";

        sSql = "SELECT TOP (100) PERCENT dbo.MRSRMaster.TrType, SUM(dbo.MRSRMaster.NetSalesAmnt) AS tAmnt, ";
        sSql = sSql + " CASE dbo.Entity.SalesOrShowroom WHEN 0 THEN 'Showroom' WHEN 1 THEN 'Coporate' WHEN 2 THEN 'Dealer' END AS EntityType";
        sSql = sSql + " FROM  dbo.MRSRMaster INNER JOIN";
        sSql = sSql + " dbo.Entity ON dbo.MRSRMaster.OutSource = dbo.Entity.EID";
        //sSql = sSql + " WHERE     (dbo.MRSRMaster.TDate = CONVERT(DATETIME, '2019-12-15 00:00:00', 102)) AND (dbo.MRSRMaster.TrType = 3)";

        sSql = sSql + " WHERE (dbo.MRSRMaster.TrType = 3)";
        //sSql = sSql + " AND (dbo.MRSRMaster.TDate = '" + tDate + "')";
        sSql = sSql + " AND (dbo.MRSRMaster.TDate >= '" + Convert.ToDateTime(this.txtFromDateSS1.Text) + "')";
        sSql = sSql + " AND (dbo.MRSRMaster.TDate <= '" + Convert.ToDateTime(this.txtToDateSS1.Text) + "')";

        sSql = sSql + " GROUP BY dbo.MRSRMaster.TrType, dbo.Entity.SalesOrShowroom";
        sSql = sSql + " ORDER BY tAmnt DESC";

        SqlCommand cmd = new SqlCommand(sSql, con);
        dr = cmd.ExecuteReader();

        GridView6.DataSource = dr;
        GridView6.DataBind();
        dr.Close();
        con.Close();

    }

    protected void GridView6_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //CalcTotalQty_cat(e.Row.Cells[2].Text);
            CalcTotal_TP_Summ(e.Row.Cells[2].Text);

            double value4 = Convert.ToDouble(e.Row.Cells[2].Text);
            e.Row.Cells[2].Text = value4.ToString("0,0");

            //double value5 = Convert.ToDouble(e.Row.Cells[3].Text);
            //e.Row.Cells[3].Text = value5.ToString("0,0");


            //RIGHT ALIGNMENT            
            e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Right;
            //e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Right;

        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {

            e.Row.Cells[1].Text = "Total";
            //e.Row.Cells[10].Text = string.Format("{0:c}", runningTotal);
            //e.Row.Cells[2].Text = runningTotalQtyCat.ToString("0,0", CultureInfo.InvariantCulture);
            e.Row.Cells[2].Text = runningTotalAmntSumm.ToString("0,0", CultureInfo.InvariantCulture);

            //this.lblNetAmnt.Text = runningTotal.ToString();

            //RIGHT ALIGNMENT
            e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Right;
            //e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Right;

        }
    }


    //CALCULATE TOTAL  SUMMARY AMOUNT
    private void CalcTotal_TP_Summ(string _price)
    {
        try
        {
            runningTotalAmntSumm += Double.Parse(_price);
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write(ex.ToString());
        }
    }


    protected void fnLoadDealerSale_ctpWise()
    {
        SqlConnection con = DBConnectionDSM.GetConnection();
        con.Open();

        DateTime tDate = DateTime.Today;

        //lblDateCTPWise.Text = tDate.ToString("dd-MMM-yyyy");
        lblDealerWiseDate.Text = txtFromDateDealer1.Text + " To " + txtToDateDealer1.Text;

        string sSql = "";
        sSql = "SELECT dbo.MRSRMaster.TrType, dbo.VW_Delear_Info.Name AS InSource, dbo.VW_Delear_Info.ZoneName,";
        sSql = sSql + " dbo.Zone.CatName AS OutSource, dbo.VW_Delear_Info.Address, ";
        sSql = sSql + " dbo.VW_Delear_Info.DealerStatus, dbo.VW_Delear_Info.ContactNo, dbo.VW_Delear_Info.ContactPerson,";
        sSql = sSql + " dbo.VW_Delear_Info.Code, dbo.VW_Delear_Info.DAID, dbo.VW_Delear_Info.EmailAdd, ";
        sSql = sSql + " dbo.VW_Delear_Info.ZoneType, SUM(dbo.MRSRMaster.NetSalesAmnt) AS tAmnt";
        sSql = sSql + " FROM dbo.VW_Delear_Info INNER JOIN";
        sSql = sSql + " dbo.MRSRMaster ON dbo.VW_Delear_Info.DAID = dbo.MRSRMaster.InSource INNER JOIN";
        sSql = sSql + " dbo.Zone ON dbo.MRSRMaster.OutSource = dbo.Zone.CategoryID";
        sSql = sSql + " WHERE dbo.MRSRMaster.TrType=3";

        sSql = sSql + " AND (dbo.MRSRMaster.TDate >= '" + Convert.ToDateTime(this.txtFromDateDealer1.Text) + "')";
        sSql = sSql + " AND (dbo.MRSRMaster.TDate <= '" + Convert.ToDateTime(this.txtToDateDealer1.Text) + "')";

        sSql = sSql + " GROUP BY dbo.MRSRMaster.TrType, dbo.VW_Delear_Info.Name, dbo.VW_Delear_Info.ZoneName,";
        sSql = sSql + " dbo.Zone.CatName, dbo.VW_Delear_Info.Address, dbo.VW_Delear_Info.DealerStatus, ";
        sSql = sSql + " dbo.VW_Delear_Info.ContactNo, dbo.VW_Delear_Info.ContactPerson, dbo.VW_Delear_Info.Code,";
        sSql = sSql + " dbo.VW_Delear_Info.DAID, dbo.VW_Delear_Info.EmailAdd, dbo.VW_Delear_Info.ZoneType";
        //sSql = sSql + " HAVING      (dbo.MRSRMaster.TrType = 3)";
        sSql = sSql + " ORDER BY dbo.VW_Delear_Info.ZoneName, dbo.VW_Delear_Info.Name";
        SqlCommand cmd = new SqlCommand(sSql, con);
        dr = cmd.ExecuteReader();

        GridView7.DataSource = dr;
        GridView7.DataBind();
        dr.Close();
        con.Close();


    }


    protected void btnDealerWise_Click(object sender, ImageClickEventArgs e)
    {
        fnLoadDealerSale_ctpWise();
    }

}