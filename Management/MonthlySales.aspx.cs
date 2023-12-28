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

public partial class CTP_MonthlySales : System.Web.UI.Page
{
    SqlConnection conn = DBConnection.GetConnection();
    SqlConnection conn1 = DBConnection.GetConnection();
    long i;

    string currentMonth1 = DateTime.Now.Month.ToString();
    string currentYear1 = DateTime.Now.Year.ToString();
    SqlDataReader dr;

    int FYs, FYe;
    DateTime sFYs, sFYe, sDate, eDate;

    private double runningTotalQty = 0;
    private double runningTotalTP = 0;
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

            lblMSales.Text = FYs + " - " + FYe;

            //string sMonth = DateTime.Now.ToString("MMM yyyy");
            //lblMSales.Text = sMonth;

            //LOAD & INSERT IN TEMP TABLE
            fnPick_Load();

            

        }


    }

    
    protected void PopupMessage(string Msg, Control controlID)
    {
        ScriptManager.RegisterClientScriptBlock(controlID, controlID.GetType(), "msg", "alert('" + Msg + "');", true);
    }

    //LOAD & INSERT IN TEMP TABLE
    private void fnPick_Load()
    {
        int monthC = Convert.ToInt16(currentMonth1);
        int yearC = Convert.ToInt16(currentYear1);

        if (monthC >= 7)
        {
            FYs = yearC;
            //FYe = yearC + 1;
        }
        else
        {
            //FYs = yearC - 1;
            //FYe = yearC;
            FYs = yearC + 1;
        }

        string sMonthName = "";
        int sOrderNo = 0;
        //string currentMonth = DateTime.Now.Month.ToString();
        //string currentYear = DateTime.Now.Year.ToString();

        string gSql = "";
        string sSql = "";

        //DLETE PREVIOUS SAME DATA
        sSql = "";
        sSql = "DELETE FROM Temp_MonthlySales_Graph";
        //sSql = sSql + " WHERE MonthName='" + sMonthName + "'";
        sSql = sSql + " WHERE SessionID='" + Session["sid"] + "'";
        SqlCommand cmdD = new SqlCommand(sSql, conn1);
        conn1.Open();
        cmdD.ExecuteNonQuery();
        conn1.Close();

        //---------------------------------------------------------------------------        
                            
        //------------------------------------------------------------------------------------------------
        for (int i= 1; i <= 12; i++)
        {
            if (i == 1)
            {
                sMonthName = "Jan";
                sOrderNo = 7;
            }
            else if (i == 2)
            {
                sMonthName = "Feb";
                sOrderNo = 8;
            }
            else if (i == 3)
            {
                sMonthName = "Mar";
                sOrderNo = 9;
            }
            else if (i == 4)
            {
                sMonthName = "Apr";
                sOrderNo = 10;
            }
            else if (i == 5)
            {
                sMonthName = "May";
                sOrderNo = 11;
            }
            else if (i == 6)
            {
                sMonthName = "Jun";
                sOrderNo = 12;
            }
            //--
            else if (i == 7)
            {
                sMonthName = "Jul";
                sOrderNo = 1;
            }
            else if (i == 8)
            {
                sMonthName = "Aug";
                sOrderNo = 2;
            }
            else if (i == 9)
            {
                sMonthName = "Sep";
                sOrderNo = 3;
            }
            else if (i == 10)
            {
                sMonthName = "Oct";
                sOrderNo = 4;
            }
            else if (i == 11)
            {
                sMonthName = "Nov";
                sOrderNo = 5;
            }
            else if (i == 12)
            {
                sMonthName = "Dec";
                sOrderNo = 6;
            }

            //-------------------------------------------
            if (i >= 7)
            {
                FYs = yearC - 1;
                //FYs = yearC;
            }
            else
            {
                FYs = yearC;
                //FYs = yearC + 1;
            }
            //-------------------------------------------
            gSql = "";
            //gSql = "SELECT dbo.MRSRMaster.TrType, SUM(dbo.MRSRDetails.NetAmnt) AS TAmnt";
            //gSql = gSql + " FROM dbo.MRSRMaster INNER JOIN";
            //gSql = gSql + " dbo.MRSRDetails ON dbo.MRSRMaster.MRSRMID = dbo.MRSRDetails.MRSRMID";
            //gSql = gSql + " WHERE (Month(dbo.MRSRMaster.TDate) = '" + i + "') AND (Year(dbo.MRSRMaster.TDate) = '" + FYs + "')";
            //gSql = gSql + " GROUP BY dbo.MRSRMaster.TrType";
            //gSql = gSql + " HAVING (dbo.MRSRMaster.TrType = 3)";

            gSql = "SELECT dbo.MRSRMaster.TrType, SUM(dbo.MRSRDetails.NetAmnt) AS TAmnt";
            gSql = gSql + " FROM dbo.MRSRMaster INNER JOIN";
            gSql = gSql + " dbo.MRSRDetails ON dbo.MRSRMaster.MRSRMID = dbo.MRSRDetails.MRSRMID INNER JOIN";
            gSql = gSql + " dbo.Entity ON dbo.MRSRMaster.OutSource = dbo.Entity.EID";
            gSql = gSql + " WHERE (Month(dbo.MRSRMaster.TDate) = '" + i + "') AND (Year(dbo.MRSRMaster.TDate) = '" + FYs + "')";
            gSql = gSql + " AND (dbo.Entity.EntityType <> 'Exclusive')";
            gSql = gSql + " GROUP BY dbo.MRSRMaster.TrType";
            gSql = gSql + " HAVING (dbo.MRSRMaster.TrType = 3)";
            
            SqlCommand cmd = new SqlCommand(gSql, conn);
            conn.Open();
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                sSql = "";
                sSql = "INSERT INTO Temp_MonthlySales_Graph(";
                sSql = sSql + " MonthName,SalesAmnt,OrderNo,UserID,SessionID";
                sSql = sSql + " )";
                sSql = sSql + " VALUES(";
                sSql = sSql + " '" + sMonthName + "','" + dr["TAmnt"] + "','" + sOrderNo + "',";
                sSql = sSql + " '" + Session["UserName"] + "','" + Session["sid"] + "'";
                sSql = sSql + " )";
                SqlCommand cmdS = new SqlCommand(sSql, conn1);
                conn1.Open();
                cmdS.ExecuteNonQuery();
                conn1.Close();
            }
            dr.Dispose();
            dr.Close();
            conn.Close();

            //WITHDRAWN
            gSql = "";
            //gSql = "SELECT dbo.MRSRMaster.TrType, SUM(dbo.MRSRDetails.NetAmnt) AS TAmnt";
            //gSql = gSql + " FROM dbo.MRSRMaster INNER JOIN";
            //gSql = gSql + " dbo.MRSRDetails ON dbo.MRSRMaster.MRSRMID = dbo.MRSRDetails.MRSRMID";
            //gSql = gSql + " WHERE (Month(dbo.MRSRMaster.TDate) = '" + i + "') AND (Year(dbo.MRSRMaster.TDate) = '" + FYs + "')";
            //gSql = gSql + " GROUP BY dbo.MRSRMaster.TrType";
            //gSql = gSql + " HAVING (dbo.MRSRMaster.TrType = -3)";

            gSql = "SELECT dbo.MRSRMaster.TrType, SUM(dbo.MRSRDetails.NetAmnt) AS TAmnt";
            gSql = gSql + " FROM dbo.MRSRMaster INNER JOIN";
            gSql = gSql + " dbo.MRSRDetails ON dbo.MRSRMaster.MRSRMID = dbo.MRSRDetails.MRSRMID INNER JOIN";
            gSql = gSql + " dbo.Entity ON dbo.MRSRMaster.InSource = dbo.Entity.EID";
            gSql = gSql + " WHERE (Month(dbo.MRSRMaster.TDate) = '" + i + "') AND (Year(dbo.MRSRMaster.TDate) = '" + FYs + "')";
            gSql = gSql + " AND (dbo.Entity.EntityType <> 'Exclusive')";
            gSql = gSql + " GROUP BY dbo.MRSRMaster.TrType";
            gSql = gSql + " HAVING (dbo.MRSRMaster.TrType = -3)";
            

            SqlCommand cmdW = new SqlCommand(gSql, conn);
            conn.Open();
            dr = cmdW.ExecuteReader();
            if (dr.Read())
            {
                sSql = "";
                sSql = "UPDATE Temp_MonthlySales_Graph SET WithAmnt= '" + dr["TAmnt"] + "'";
                sSql = sSql + " WHERE MonthName='" + sMonthName + "'";
                sSql = sSql + " AND SessionID='" + Session["sid"] + "'";
                
                SqlCommand cmdW2 = new SqlCommand(sSql, conn1);
                conn1.Open();
                cmdW2.ExecuteNonQuery();
                conn1.Close();
            }
            dr.Dispose();
            dr.Close();
            conn.Close();

        }
        //------------------------------------------------------------------------------------------------


        //LOAD DATA IN GRID
        fnLoadData_in_grid();

    }


    //LOAD MONTH WISE SALES DATA I GRID
    private void fnLoadData_in_grid()
    {
        //s = WebConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        //con = new SqlConnection(s);
        SqlConnection con = DBConnection.GetConnection();
        con.Open();

        string sSql = "";

        /*
        sSql = "SELECT MRSRCode, CONVERT(varchar(12), TDate, 105) AS TDate," +
            " ISNULL(NetSalesAmnt,0) AS NetSalesAmnt,  dbo.Entity.eName as Entity," +
            " ISNULL(CashAmnt,0) AS CashAmnt, " +
            " (ISNULL(CardAmnt1,0) + ISNULL(CardAmnt2,0)) AS CardAmnt, " +
            " dbo.Customer.CustID, dbo.Customer.CustName, dbo.Customer.Address, " +
            " dbo.Customer.Phone, dbo.Customer.Mobile, dbo.Customer.Email " +

            " FROM dbo.Entity INNER JOIN  dbo.MRSRMaster ON dbo.Entity.EID = dbo.MRSRMaster.OutSource LEFT OUTER JOIN " +
            " dbo.Customer ON dbo.MRSRMaster.Customer = dbo.Customer.Mobile" +

            " WHERE (dbo.MRSRMaster.TrType = 3) " +
            " AND (dbo.MRSRMaster.OutSource='" + Session["sBrId"] + "')" +

            " AND (dbo.MRSRMaster.TDate >= '" + DateTime.Today.ToString("MM/dd/yyyy") + "'" +
            " AND dbo.MRSRMaster.TDate <= '" + DateTime.Today.ToString("MM/dd/yyyy") + "')" +

            " ORDER BY TDate, MRSRCode";
        */


        //sSql = "SELECT CONVERT(varchar(12), V1.TDate, 105) AS TDate,";
        //sSql = sSql + " SUM(ISNULL(V1.tAmnt, 0)) AS SalesAmnt, SUM(ISNULL(V2.tAmnt, 0)) AS WithAmnt,";
        //sSql = sSql + " SUM(ISNULL(V1.tAmnt, 0)) - SUM(ISNULL(V2.tAmnt, 0)) AS NetAmnt";
        //sSql = sSql + " FROM (SELECT TDate, TrType, SUM(ISNULL(NetSalesAmnt, 0)) AS tAmnt, OutSource";
        //sSql = sSql + " FROM dbo.MRSRMaster";
        //sSql = sSql + " GROUP BY TDate, TrType, OutSource";
        //sSql = sSql + " HAVING (TrType = 3) AND (month(TDate) = '" + DateTime.Now.Month.ToString() + "')";
        //sSql = sSql + " AND (year(TDate) = '" + DateTime.Now.Year.ToString() + "')";
        //sSql = sSql + " AND (OutSource >'0')) AS V1 LEFT OUTER JOIN";
        //sSql = sSql + " (SELECT TDate, TrType, SUM(ISNULL(NetSalesAmnt, 0)) AS tAmnt, InSource";
        //sSql = sSql + " FROM dbo.MRSRMaster AS MRSRMaster_1";
        //sSql = sSql + " WHERE (InSource > '0')";
        //sSql = sSql + " GROUP BY TDate, TrType, InSource";
        //sSql = sSql + " HAVING (TrType = - 3)";
        //sSql = sSql + " AND (month(TDate) = '" + DateTime.Now.Month.ToString() + "')";
        //sSql = sSql + " AND (year(TDate) = '" + DateTime.Now.Year.ToString() + "'))";
        //sSql = sSql + " AS V2";
        //sSql = sSql + " ON V1.TDate = V2.TDate AND V1.OutSource = V2.InSource";
        //sSql = sSql + " GROUP BY V1.TDate";
        //sSql = sSql + " ORDER BY V1.TDate";

        sSql = "";
        sSql = "SELECT MonthName, SalesAmnt, WithAmnt, ISNULL(SalesAmnt, 0) - ISNULL(WithAmnt, 0) AS NetAmnt,";
        sSql = sSql + " UserID, SessionID, OrderNo";
        sSql = sSql + " FROM dbo.Temp_MonthlySales_Graph";
        sSql = sSql + " WHERE SessionID='" + Session["sid"] + "'";
        sSql = sSql + " ORDER BY OrderNo";

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
            CalcTotal_TP(e.Row.Cells[2].Text);

            CalcTotal_Cash(e.Row.Cells[3].Text);
            CalcTotal_Card(e.Row.Cells[4].Text);

            // ALIGNMENT
            e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Left;

            e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Center;
                        

        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[1].Text = "Total";
            //e.Row.Cells[4].Text = runningTotalQty.ToString("0,0", CultureInfo.InvariantCulture);
            e.Row.Cells[2].Text = runningTotalTP.ToString("0,0", CultureInfo.InvariantCulture);
            e.Row.Cells[3].Text = runningTotalCash.ToString("0,0", CultureInfo.InvariantCulture);
            e.Row.Cells[4].Text = runningTotalCard.ToString("0,0", CultureInfo.InvariantCulture);


            //ALIGNMENT
            e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Left;

            e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Center;

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



    protected void OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        GridView1.DataBind();
        fnLoadData_in_grid();
    }

}