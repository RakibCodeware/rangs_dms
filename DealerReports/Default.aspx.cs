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

public partial class DealerReports_Default : System.Web.UI.Page
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
            Response.Redirect("~/LogIn.aspx");
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

            try
            {
                this.lblText.Text = "Welcome to " + Session["eName"].ToString() + " ... FY : " + FYs + "-" + FYe + "";
                //this.lblCTP.Text = Session["eName"].ToString();                       

                //LOAD YEARLY SALES AMOUNT
                //mycoment//
                //fnLoadYearlySalesAmnt();

                //LOAD MONTHLY SALES AMOUNT
                //mycoment//
                //fnLoadMonthlySalesAmnt();

                //LOAD DAILY SALES AMOUNT
                //mycoment//
                //fnLoadDailySalesAmnt();

                //LOAD Yearly Target
                //mycoment//
                //fnLoadTargetYearly();

                //LOAD CAMPAIGN INFO
                //fnLoadCampaign();

                //--------------------------------------------------------
                //fnLoadData_BrandWiseSales();
                //fnLoadData_CatWiseSales();
                //fnLoadData_ModelWiseSales();
                //fnLoadData_NewProduct();
                //fnLoadData_Stock();
                //--------------------------------------------------------

                //mycoment//

                //fnLoadYearlyDeposit();
                //fnLoadMonthlyDeposit();
                //fnLoadDailyDeposit();
                //mycoment//

                //--------------------------------------------------------
                //LOAD TOTAL OUTSTANDING
                //mycoment//
                //fnLoadTotalOutStanding();
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
        gSql = "SELECT dbo.MRSRMaster.TrType, SUM(dbo.MRSRMaster.NetSalesAmnt)/100000 AS NetAmnt";
        //gSql = gSql + " PARSENAME(CONVERT(VARCHAR, CAST(SUM(NetSalesAmnt)/100000  AS NetAmnt), 1),2)";
        //gSql = gSql + " FROM dbo.MRSRMaster";
        gSql = gSql + " FROM dbo.MRSRMaster INNER JOIN dbo.Entity ON dbo.MRSRMaster.InSource = dbo.Entity.EID";

        gSql = gSql + " WHERE (dbo.MRSRMaster.TrType = -3)";
        gSql = gSql + " AND (dbo.Entity.EntityType = 'Dealer')";
        gSql = gSql + " AND (dbo.MRSRMaster.TDate >= '" + sFYs + "')";
        gSql = gSql + " AND (dbo.MRSRMaster.TDate <= '" + sFYe + "')";
        //gSql = gSql + " AND (dbo.MRSRMaster.InSource = '" + Session["sBrId"] + "')";

        gSql = gSql + " GROUP BY dbo.MRSRMaster.TrType";

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
        gSql = "SELECT dbo.MRSRMaster.TrType, SUM(dbo.MRSRMaster.NetSalesAmnt)/100000 AS NetAmnt";
        //gSql = gSql + " PARSENAME(CONVERT(VARCHAR, CAST(SUM(NetSalesAmnt)/100000  AS NetAmnt), 1),2)";
        //gSql = gSql + " FROM dbo.MRSRMaster";
        gSql = gSql + " FROM dbo.MRSRMaster INNER JOIN dbo.Entity ON dbo.MRSRMaster.OutSource = dbo.Entity.EID";

        gSql = gSql + " WHERE (dbo.MRSRMaster.TrType = 3)";
        gSql = gSql + " AND (dbo.Entity.EntityType = 'Dealer')";
        gSql = gSql + " AND (dbo.MRSRMaster.TDate >= '" + sFYs + "')";
        gSql = gSql + " AND (dbo.MRSRMaster.TDate <= '" + sFYe + "')";
        //gSql = gSql + " AND (dbo.MRSRMaster.OutSource = '" + Session["sBrId"] + "')";

        gSql = gSql + " GROUP BY dbo.MRSRMaster.TrType";

        cmd = new SqlCommand(gSql, conn);
        conn.Open();
        dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            tAmnt = Convert.ToDouble(dr["NetAmnt"].ToString()) - wAmnt;
            lblYSales.Text = tAmnt.ToString("00.00", CultureInfo.InvariantCulture) + "L";
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
        gSql = "SELECT dbo.MRSRMaster.TrType, SUM(dbo.MRSRMaster.NetSalesAmnt)/100000 AS NetAmnt";
        //gSql = gSql + " PARSENAME(CONVERT(VARCHAR, CAST(SUM(NetSalesAmnt)/100000  AS NetAmnt), 1),2)";
        //gSql = gSql + " FROM dbo.MRSRMaster";
        gSql = gSql + " FROM dbo.MRSRMaster INNER JOIN dbo.Entity ON dbo.MRSRMaster.InSource = dbo.Entity.EID";

        gSql = gSql + " WHERE (dbo.MRSRMaster.TrType = -3)";
        gSql = gSql + " AND (dbo.Entity.EntityType = 'Dealer')";
        gSql = gSql + " AND (dbo.MRSRMaster.TDate >= '" + sDate + "')";
        gSql = gSql + " AND (dbo.MRSRMaster.TDate <= '" + eDate + "')";
        //gSql = gSql + " AND (InSource = '" + Session["sBrId"] + "')";

        gSql = gSql + " GROUP BY dbo.MRSRMaster.TrType";

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
        gSql = "SELECT dbo.MRSRMaster.TrType, SUM(dbo.MRSRMaster.NetSalesAmnt)/100000 AS NetAmnt";
        //gSql = gSql + " PARSENAME(CONVERT(VARCHAR, CAST(SUM(NetSalesAmnt)/100000  AS NetAmnt), 1),2)";
        //gSql = gSql + " FROM dbo.MRSRMaster";
        gSql = gSql + " FROM dbo.MRSRMaster INNER JOIN dbo.Entity ON dbo.MRSRMaster.OutSource = dbo.Entity.EID";

        gSql = gSql + " WHERE (dbo.MRSRMaster.TrType = 3)";
        gSql = gSql + " AND (dbo.Entity.EntityType = 'Dealer')";
        gSql = gSql + " AND (dbo.MRSRMaster.TDate >= '" + sDate + "')";
        gSql = gSql + " AND (dbo.MRSRMaster.TDate <= '" + eDate + "')";
        //gSql = gSql + " AND (dbo.MRSRMaster.OutSource = '" + Session["sBrId"] + "')";

        gSql = gSql + " GROUP BY dbo.MRSRMaster.TrType";

        cmd = new SqlCommand(gSql, conn);
        conn.Open();
        dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            tAmnt = Convert.ToDouble(dr["NetAmnt"].ToString()) - wAmnt;
            lblMSales.Text = tAmnt.ToString("00.00", CultureInfo.InvariantCulture) + "L";
            //lblMSales.Text = tAmnt.ToString("00", CultureInfo.InvariantCulture);
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
        gSql = "SELECT dbo.MRSRMaster.TrType, SUM(dbo.MRSRMaster.NetSalesAmnt) AS NetAmnt";
        //gSql = gSql + " PARSENAME(CONVERT(VARCHAR, CAST(SUM(NetSalesAmnt)/100000  AS NetAmnt), 1),2)";
        //gSql = gSql + " FROM dbo.MRSRMaster";
        gSql = gSql + " FROM dbo.MRSRMaster INNER JOIN dbo.Entity ON dbo.MRSRMaster.InSource = dbo.Entity.EID";

        gSql = gSql + " WHERE (dbo.MRSRMaster.TrType = -3)";
        gSql = gSql + " AND (dbo.Entity.EntityType = 'Dealer')";
        gSql = gSql + " AND (dbo.MRSRMaster.TDate = '" + tDate + "')";
        //gSql = gSql + " AND (dbo.MRSRMaster.InSource = '" + Session["sBrId"] + "')";

        gSql = gSql + " GROUP BY dbo.MRSRMaster.TrType";

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
        gSql = "SELECT dbo.MRSRMaster.TrType, SUM(dbo.MRSRMaster.NetSalesAmnt) AS NetAmnt";
        //gSql = gSql + " PARSENAME(CONVERT(VARCHAR, CAST(SUM(NetSalesAmnt)/100000  AS NetAmnt), 1),2)";
        //gSql = gSql + " FROM dbo.MRSRMaster";
        gSql = gSql + " FROM dbo.MRSRMaster INNER JOIN dbo.Entity ON dbo.MRSRMaster.OutSource = dbo.Entity.EID";

        gSql = gSql + " WHERE (dbo.MRSRMaster.TrType = 3)";
        gSql = gSql + " AND (dbo.Entity.EntityType = 'Dealer')";
        gSql = gSql + " AND (dbo.MRSRMaster.TDate = '" + tDate + "')";
        //gSql = gSql + " AND (dbo.MRSRMaster.OutSource = '" + Session["sBrId"] + "')";

        gSql = gSql + " GROUP BY TrType";

        cmd = new SqlCommand(gSql, conn);
        conn.Open();
        dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            tAmnt = Convert.ToDouble(dr["NetAmnt"].ToString()) - wAmnt;
            lblDSales.Text = tAmnt.ToString("00", CultureInfo.InvariantCulture);
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
        gSql = "SELECT SUM(dbo.tbTargetYearly.TAmount) AS NetAmnt";
        //gSql = gSql + " FROM dbo.tbTargetYearly";
        gSql = gSql + " FROM dbo.tbTargetYearly INNER JOIN dbo.Entity ON dbo.tbTargetYearly.EID = dbo.Entity.EID";

        //gSql = gSql + " WHERE (TrType = 3)";
        gSql = gSql + " WHERE (dbo.tbTargetYearly.TYear = '" + sFY + "')";
        gSql = gSql + " AND (dbo.Entity.EntityType = 'Dealer')";
        //gSql = gSql + " AND (EID = '" + Session["sBrId"] + "')";

        //gSql = gSql + " GROUP BY EntityName";

        SqlCommand cmd = new SqlCommand(gSql, conn);
        conn.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            tAmnt = Convert.ToDouble(dr["NetAmnt"].ToString());
            lblTargetY.Text = tAmnt.ToString("00", CultureInfo.InvariantCulture) + "L";
        }
        else
        {
            lblTargetY.Text = "0";
        }
        dr.Dispose();
        dr.Close();
        conn.Close();

    }


    //protected void fnLoadCampaign()
    //{
    //    DateTime tDate = DateTime.Today;

    //    SqlConnection conn = DBConnection.GetConnection();

    //    string gSql = "";
    //    gSql = "SELECT  CampaignNo, CampaignName, CONVERT(VARCHAR(10), EffectiveDate, 103) AS EffectiveDate,";
    //    gSql = gSql + " cTag, cStop, cStopDate";
    //    gSql = gSql + " FROM  dbo.tbCampaignMaster";
    //    gSql = gSql + " WHERE (cStop = 0)";
    //    gSql = gSql + " ORDER BY CamAID DESC";

    //    SqlCommand cmd = new SqlCommand(gSql, conn);
    //    conn.Open();
    //    SqlDataReader dr = cmd.ExecuteReader();
    //    if (dr.Read())
    //    {
    //        lblPromotion.Text = dr["CampaignName"].ToString();
    //        lblPromotionDate.Text = "Start From : " + dr["EffectiveDate"].ToString();

    //    }
    //    else
    //    {
    //        lblPromotion.Text = "";
    //        lblPromotionDate.Text = "";
    //    }
    //    dr.Dispose();
    //    dr.Close();
    //    conn.Close();


    //}



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
        //gvModelWiseSales.DataSource = ViewState["dt"] as DataTable;
        //gvModelWiseSales.DataBind();
    }


    //----------------------------------------------------------------------------------------

    //protected void fnLoadData_BrandWiseSales()
    //{

    //    SqlConnection con = DBConnection.GetConnection();
    //    con.Open();

    //    DateTime tDate = DateTime.Today;

    //    //lblDateBrand.Text = tDate.ToString("dd-MMM-yyyy");
    //    lblDateBrand.Text = tDate.ToString("MMM-yyyy");

    //    DateTime date = DateTime.Today;
    //    var fDayOfMonth = new DateTime(date.Year, date.Month, 1);
    //    var lDayOfMonth = fDayOfMonth.AddMonths(1).AddDays(-1);

    //    sDate = Convert.ToDateTime(fDayOfMonth);
    //    eDate = Convert.ToDateTime(lDayOfMonth);

    //    string sSql = "";

    //    sSql = "";
    //    sSql = "SELECT dbo.MRSRMaster.TrType,  dbo.Product.PCategory,";
    //    sSql = sSql + " SUM(ABS(dbo.MRSRDetails.Qty)) AS tQty, SUM(dbo.MRSRDetails.NetAmnt) AS tAmnt";

    //    sSql = sSql + " FROM dbo.MRSRMaster INNER JOIN";
    //    sSql = sSql + " dbo.MRSRDetails ON dbo.MRSRMaster.MRSRMID = dbo.MRSRDetails.MRSRMID INNER JOIN";
    //    sSql = sSql + " dbo.Product ON dbo.MRSRDetails.ProductID = dbo.Product.ProductID";
    //    //sSql = sSql + " dbo.Category ON dbo.Product.CategoryID = dbo.Category.CategoryID INNER JOIN";
    //    //sSql = sSql + " dbo.Category AS Category_1 ON dbo.Category.ParentID = Category_1.CategoryID";
    //    sSql = sSql + " INNER JOIN dbo.Entity ON dbo.MRSRMaster.OutSource = dbo.Entity.EID";

    //    sSql = sSql + " WHERE (dbo.MRSRMaster.TrType = 3)";
    //    sSql = sSql + " AND (dbo.Entity.EntityType = 'Dealer')";
    //    //sSql = sSql + " AND (dbo.MRSRMaster.TDate = '" + tDate + "')";
    //    sSql = sSql + " AND (dbo.MRSRMaster.TDate >= '" + sDate + "')";
    //    sSql = sSql + " AND (dbo.MRSRMaster.TDate <= '" + eDate + "')";

    //    //sSql = sSql + " AND (dbo.MRSRMaster.OutSource = '" + Session["sBrId"] + "')";

    //    sSql = sSql + " GROUP BY dbo.MRSRMaster.TrType, dbo.Product.PCategory";
    //    sSql = sSql + " ORDER BY dbo.Product.PCategory";



    //    SqlCommand cmd = new SqlCommand(sSql, con);
    //    dr = cmd.ExecuteReader();

    //    GridView1.DataSource = dr;
    //    GridView1.DataBind();
    //    dr.Close();
    //    con.Close();
    //}

    //protected void fnLoadData_CatWiseSales()
    //{

    //    SqlConnection con = DBConnection.GetConnection();
    //    con.Open();

    //    DateTime tDate = DateTime.Today;

    //    //lblDateCatWise.Text = tDate.ToString("dd-MMM-yyyy");
    //    lblDateCatWise.Text = tDate.ToString("MMM-yyyy");

    //    DateTime date = DateTime.Today;
    //    var fDayOfMonth = new DateTime(date.Year, date.Month, 1);
    //    var lDayOfMonth = fDayOfMonth.AddMonths(1).AddDays(-1);

    //    sDate = Convert.ToDateTime(fDayOfMonth);
    //    eDate = Convert.ToDateTime(lDayOfMonth);


    //    string sSql = "";

    //    sSql = "";
    //    sSql = "SELECT dbo.MRSRMaster.TrType, dbo.Product.GroupName,";
    //    sSql = sSql + " SUM(ABS(dbo.MRSRDetails.Qty)) AS tQty, SUM(dbo.MRSRDetails.NetAmnt) AS tAmnt";
    //    sSql = sSql + " FROM dbo.MRSRMaster INNER JOIN";
    //    sSql = sSql + " dbo.MRSRDetails ON dbo.MRSRMaster.MRSRMID = dbo.MRSRDetails.MRSRMID INNER JOIN";
    //    sSql = sSql + " dbo.Product ON dbo.MRSRDetails.ProductID = dbo.Product.ProductID";
    //    sSql = sSql + " INNER JOIN dbo.Entity ON dbo.MRSRMaster.OutSource = dbo.Entity.EID";

    //    sSql = sSql + " WHERE (dbo.MRSRMaster.TrType = 3)";
    //    sSql = sSql + " AND (dbo.Entity.EntityType = 'Dealer')";
    //    //sSql = sSql + " AND (dbo.MRSRMaster.TDate = '" + tDate + "')";
    //    sSql = sSql + " AND (dbo.MRSRMaster.TDate >= '" + sDate + "')";
    //    sSql = sSql + " AND (dbo.MRSRMaster.TDate <= '" + eDate + "')";

    //    //sSql = sSql + " AND (dbo.MRSRMaster.OutSource = '" + Session["sBrId"] + "')";

    //    sSql = sSql + " GROUP BY dbo.MRSRMaster.TrType, dbo.Product.GroupName";
    //    sSql = sSql + " ORDER BY dbo.Product.GroupName";


    //    SqlCommand cmd = new SqlCommand(sSql, con);
    //    dr = cmd.ExecuteReader();

    //    GridView2.DataSource = dr;
    //    GridView2.DataBind();
    //    dr.Close();
    //    con.Close();
    //}

    //protected void fnLoadData_ModelWiseSales()
    //{

    //    SqlConnection con = DBConnection.GetConnection();
    //    con.Open();

    //    DateTime tDate = DateTime.Today;

    //    //lblDateModelWise.Text = tDate.ToString("dd-MMM-yyyy");
    //    lblDateModelWise.Text = tDate.ToString("MMM-yyyy");

    //    DateTime date = DateTime.Today;
    //    var fDayOfMonth = new DateTime(date.Year, date.Month, 1);
    //    var lDayOfMonth = fDayOfMonth.AddMonths(1).AddDays(-1);

    //    sDate = Convert.ToDateTime(fDayOfMonth);
    //    eDate = Convert.ToDateTime(lDayOfMonth);


    //    string sSql = "";

    //    sSql = "";
    //    sSql = "SELECT dbo.MRSRMaster.TrType, dbo.Product.Model,";
    //    sSql = sSql + " SUM(ABS(dbo.MRSRDetails.Qty)) AS tQty, SUM(dbo.MRSRDetails.NetAmnt) AS tAmnt";
    //    sSql = sSql + " FROM dbo.MRSRMaster INNER JOIN";
    //    sSql = sSql + " dbo.MRSRDetails ON dbo.MRSRMaster.MRSRMID = dbo.MRSRDetails.MRSRMID INNER JOIN";
    //    sSql = sSql + " dbo.Product ON dbo.MRSRDetails.ProductID = dbo.Product.ProductID";
    //    sSql = sSql + " INNER JOIN dbo.Entity ON dbo.MRSRMaster.OutSource = dbo.Entity.EID";

    //    sSql = sSql + " WHERE (dbo.MRSRMaster.TrType = 3)";
    //    sSql = sSql + " AND (dbo.Entity.EntityType = 'Dealer')";
    //    //sSql = sSql + " AND (dbo.MRSRMaster.TDate = '" + tDate + "')";
    //    sSql = sSql + " AND (dbo.MRSRMaster.TDate >= '" + sDate + "')";
    //    sSql = sSql + " AND (dbo.MRSRMaster.TDate <= '" + eDate + "')";

    //    sSql = sSql + " AND (dbo.MRSRMaster.OutSource = '" + Session["sBrId"] + "')";

    //    sSql = sSql + " GROUP BY dbo.MRSRMaster.TrType, dbo.Product.Model";
    //    sSql = sSql + " ORDER BY dbo.Product.Model";


    //    SqlCommand cmd = new SqlCommand(sSql, con);
    //    dr = cmd.ExecuteReader();

    //    gvModelWiseSales.DataSource = dr;
    //    gvModelWiseSales.DataBind();
    //    dr.Close();
    //    con.Close();
    //}


    //protected void fnLoadData_NewProduct()
    //{

    //    SqlConnection con = DBConnection.GetConnection();
    //    con.Open();

    //    DateTime tDate = DateTime.Today;

    //    //lblDateStock.Text = tDate.ToString("dd-MMM-yyyy");

    //    string sSql = "";

    //    sSql = "";
    //    sSql = "SELECT TOP (10) ProductID, Code, Model, ProdName, GroupName, UnitPrice, Discontinue";
    //    sSql = sSql + " FROM  dbo.Product ";

    //    sSql = sSql + " WHERE (Discontinue = 'No')";
    //    //sSql = sSql + " AND (dbo.MRSRMaster.TDate = '" + tDate + "')";

    //    //sSql = sSql + " GROUP BY dbo.MRSRMaster.TrType, dbo.Entity.eName";
    //    sSql = sSql + " ORDER BY ProductID DESC";


    //    SqlCommand cmd = new SqlCommand(sSql, con);
    //    dr = cmd.ExecuteReader();

    //    GridView3.DataSource = dr;
    //    GridView3.DataBind();
    //    dr.Close();
    //    con.Close();
    //}


    protected void fnLoadYearlyDeposit()
    {

        SqlConnection conn = DBConnection.GetConnection();

        string gSql = "";
        double tAmnt = 0;


        //---------------------------------------------------------------------------------------------
        // SALES AMOUNT
        gSql = "";
        gSql = "SELECT SUM(dbo.tbDeposit.DepositAmnt)/100000 AS NetAmnt";
        //gSql = "SELECT EID, ISNULL(SUM(DepositAmnt),0)/10000000 AS NetAmnt";
        //gSql = gSql + " PARSENAME(CONVERT(VARCHAR, CAST(SUM(NetSalesAmnt)/100000  AS NetAmnt), 1),2)";
        //gSql = gSql + " FROM dbo.tbDeposit";
        gSql = gSql + " FROM dbo.tbDeposit INNER JOIN dbo.Entity ON dbo.tbDeposit.EID = dbo.Entity.EID";

        //gSql = gSql + " WHERE (TrType = 3)";
        gSql = gSql + " WHERE (dbo.tbDeposit.DepositDate >= '" + sFYs + "')";
        gSql = gSql + " AND (dbo.tbDeposit.DepositDate <= '" + sFYe + "')";
        gSql = gSql + " AND (dbo.Entity.EntityType = 'Dealer')";
        //gSql = gSql + " AND (dbo.tbDeposit.EID = '" + Session["sBrId"] + "')";

        //gSql = gSql + " GROUP BY dbo.tbDeposit.EID";

        SqlCommand cmd = new SqlCommand(gSql, conn);
        conn.Open();
        dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            tAmnt = Convert.ToDouble(dr["NetAmnt"].ToString());
            lblYDeposit.Text = tAmnt.ToString("00.00",
                CultureInfo.InvariantCulture) + "L";
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
        gSql = "SELECT ISNULL(SUM(dbo.tbDeposit.DepositAmnt),0) AS NetAmnt";
        //gSql = gSql + " PARSENAME(CONVERT(VARCHAR, CAST(SUM(NetSalesAmnt)/100000  AS NetAmnt), 1),2)";
        //gSql = gSql + " FROM dbo.tbDeposit";
        gSql = gSql + " FROM dbo.tbDeposit INNER JOIN dbo.Entity ON dbo.tbDeposit.EID = dbo.Entity.EID";

        //gSql = gSql + " WHERE (TrType = 3)";
        gSql = gSql + " WHERE (dbo.tbDeposit.DepositDate >= '" + sDate + "')";
        gSql = gSql + " AND (dbo.tbDeposit.DepositDate <= '" + eDate + "')";
        gSql = gSql + " AND (dbo.Entity.EntityType = 'Dealer')";
        //gSql = gSql + " AND (dbo.tbDeposit.EID = '" + Session["sBrId"] + "')";

        SqlCommand cmd = new SqlCommand(gSql, conn);
        conn.Open();
        dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            tAmnt = Convert.ToDouble(dr["NetAmnt"].ToString()) / 100000;
            lblMDeposit.Text = tAmnt.ToString("00", CultureInfo.InvariantCulture) + "L";
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
        gSql = "SELECT ISNULL(SUM(dbo.tbDeposit.DepositAmnt),0) AS NetAmnt";
        //gSql = gSql + " PARSENAME(CONVERT(VARCHAR, CAST(SUM(NetSalesAmnt)/100000  AS NetAmnt), 1),2)";
        //gSql = gSql + " FROM dbo.tbDeposit";
        gSql = gSql + " FROM dbo.tbDeposit INNER JOIN dbo.Entity ON dbo.tbDeposit.EID = dbo.Entity.EID";

        //gSql = gSql + " WHERE (TrType = 3)";
        gSql = gSql + " WHERE (dbo.tbDeposit.DepositDate = '" + tDate + "')";
        //gSql = gSql + " AND (EID = '" + Session["sBrId"] + "')";
        gSql = gSql + " AND (dbo.Entity.EntityType = 'Dealer')";

        //gSql = gSql + " GROUP BY EID";

        SqlCommand cmd = new SqlCommand(gSql, conn);
        conn.Open();
        dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            tAmnt = Convert.ToDouble(dr["NetAmnt"].ToString());
            lblDDeposit.Text = tAmnt.ToString("00", CultureInfo.InvariantCulture);
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