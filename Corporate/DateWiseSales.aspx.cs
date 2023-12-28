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

public partial class DateWiseSales : System.Web.UI.Page
{
    SqlConnection conn = DBConnection.GetConnection();
    long i;

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
            string sMonth = DateTime.Now.ToString("MMM yyyy");
            lblMSales.Text = sMonth;

            //LOAD DATA IN GRID
            fnLoadData();
        }


    }

    
    protected void PopupMessage(string Msg, Control controlID)
    {
        ScriptManager.RegisterClientScriptBlock(controlID, controlID.GetType(), "msg", "alert('" + Msg + "');", true);
    }


    //LOAD SALES SUMMARY CHALLAN WISE
    private void fnLoadData()
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


        sSql = "SELECT CONVERT(varchar(12), V1.TDate, 105) AS TDate,";
        sSql = sSql + " SUM(ISNULL(V1.tAmnt, 0)) AS SalesAmnt, SUM(ISNULL(V2.tAmnt, 0)) AS WithAmnt,";
        sSql = sSql + " SUM(ISNULL(V1.tAmnt, 0)) - SUM(ISNULL(V2.tAmnt, 0)) AS NetAmnt";
        sSql = sSql + " FROM (SELECT TDate, TrType, SUM(ISNULL(NetSalesAmnt, 0)) AS tAmnt, OutSource";
        sSql = sSql + " FROM dbo.MRSRMaster";
        sSql = sSql + " GROUP BY TDate, TrType, OutSource";
        sSql = sSql + " HAVING (TrType = 3) AND (month(TDate) = '" + DateTime.Now.Month.ToString() + "')";
        sSql = sSql + " AND (year(TDate) = '" + DateTime.Now.Year.ToString() + "')";
        sSql = sSql + " AND (OutSource in (473,253))) AS V1 LEFT OUTER JOIN";
        sSql = sSql + " (SELECT TDate, TrType, SUM(ISNULL(NetSalesAmnt, 0)) AS tAmnt, InSource";
        sSql = sSql + " FROM dbo.MRSRMaster AS MRSRMaster_1";
        sSql = sSql + " WHERE (InSource in (473,253))"; // 431,
        sSql = sSql + " GROUP BY TDate, TrType, InSource";
        sSql = sSql + " HAVING (TrType = - 3)";
        sSql = sSql + " AND (month(TDate) = '" + DateTime.Now.Month.ToString() + "')";
        sSql = sSql + " AND (year(TDate) = '" + DateTime.Now.Year.ToString() + "'))";
        sSql = sSql + " AS V2";
        sSql = sSql + " ON V1.TDate = V2.TDate AND V1.OutSource = V2.InSource";
        sSql = sSql + " GROUP BY V1.TDate";
        sSql = sSql + " ORDER BY V1.TDate";
                
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
        fnLoadData();
    }

}