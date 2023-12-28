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

public partial class Search_Statement : System.Web.UI.Page
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
            this.txtFrom.Text = DateTime.Today.ToString("MM/dd/yyyy");
            this.txtToDate.Text = DateTime.Today.ToString("MM/dd/yyyy");
        }

    }

    
    protected void PopupMessage(string Msg, Control controlID)
    {
        ScriptManager.RegisterClientScriptBlock(controlID, controlID.GetType(), "msg", "alert('" + Msg + "');", true);
    }

    protected void SearchData(object sender, EventArgs e)
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

        try
        {
            //LOAD STATEMENT DATA
            fnLoadStatementData();

            //LOAD DATA IN GRID
            fnLoadData();
        }
        catch (Exception ex)
        {
            //
        }

    }


    //LOAD DATA IN GRID
    private void fnLoadData()
    {
        //s = WebConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        //con = new SqlConnection(s);
        SqlConnection con = DBConnection.GetConnection();
        con.Open();

        string sSql = "";
        sSql = "SELECT CONVERT(varchar(12), DDate, 105) AS DDate," +
            " (ISNULL(OBSales,0) - ISNULL(OBWith,0) -ISNULL(OBDeposit,0)) AS OBAmnt," +
            " ISNULL(SalesAmnt,0) AS SalesAmnt, ISNULL(WithAmnt,0) AS WithAmnt, ISNULL(DepositAmnt,0) AS DepositAmnt," +
            " (ISNULL(OBSales,0) - ISNULL(OBWith,0) -ISNULL(OBDeposit,0)) + " +
            " ISNULL(SalesAmnt,0) - ISNULL(WithAmnt,0) - ISNULL(DepositAmnt,0) AS CBAmnt" +

            " FROM Temp_DepositStatement " +

            " WHERE (UserID = '" + Session["UserName"] + "') " +
            " AND (EID='" + Session["sBrId"] + "')" +
                    
            //" AND (dbo.MRSRMaster.TDate >= '" + Convert.ToDateTime(this.txtFrom.Text) + "'" +
            //" AND dbo.MRSRMaster.TDate <= '" + Convert.ToDateTime(this.txtToDate.Text) + "')" +

            " ORDER BY DDate";
        

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

    //LOAD STATEMENT DATA
    private void fnLoadStatementData()
    {
        SqlConnection con = DBConnection.GetConnection();
        SqlConnection con1 = DBConnection.GetConnection();

        string sSql = "";

        //*****************************************************************************************
        //DELETE PREVIOUS DATA
        sSql = "";
        sSql = "DELETE FROM Temp_DepositStatement ";        
        sSql = sSql + " WHERE ";
        sSql = sSql + " UserID='" + Session["UserName"] + "'";
        //sSql = sSql + " AND EID='" + Session["sBrId"] + "'";

        SqlCommand cmdD = new SqlCommand(sSql, con1);
        con1.Open();
        cmdD.ExecuteNonQuery();
        con1.Close();
        //*****************************************************************************************

        /*
        //----------------------------------------------------------------------------------------
        // INSERT DATE FROM MASTER TABLE
        sSql = "";
        sSql = "SELECT TrType, TDate";
        sSql = sSql + " FROM dbo.MRSRMaster";
        sSql = sSql + " WHERE (TDate >= '" + txtFrom.Text + "')";
        sSql = sSql + " AND (TDate <= '" + txtToDate.Text + "')";
        sSql = sSql + " AND (OutSource = '" + Session["sBrId"] + "') AND (TrType = 3)";
        sSql = sSql + " GROUP BY TrType, TDate";
        SqlCommand cmd = new SqlCommand(sSql, con);
        con.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            sSql = "";
            sSql = "INSERT INTO Temp_DepositStatement(";
            sSql = sSql + " DDate,UserID,EID";
            sSql = sSql + " )";
            sSql = sSql + " VALUES(";
            sSql = sSql + " '" + dr["TDate"] + "','" + Session["UserName"] + "','" + Session["sBrId"] + "'";
            sSql = sSql + " )";
            SqlCommand cmdS = new SqlCommand(sSql, con1);
            con1.Open();
            cmdS.ExecuteNonQuery();
            con1.Close();

        }

        con.Close();
        dr.Close();


        // INSERT DATE FROM DEPOSIT TABLE
        sSql = "";
        sSql = "SELECT DepositDate";
        sSql = sSql + " FROM dbo.tbDeposit";
        sSql = sSql + " WHERE (DepositDate >= '" + txtFrom.Text + "') AND (DepositDate <= '" + txtToDate.Text + "') AND ";
        sSql = sSql + " (DepositDate NOT IN ";
        sSql = sSql + " (SELECT TDate";
        sSql = sSql + " FROM dbo.MRSRMaster ";
        sSql = sSql + " WHERE (TDate >= '" + txtFrom.Text + "') AND (TDate <= '" + txtToDate.Text + "')";
        sSql = sSql + " GROUP BY TDate))";
        sSql = sSql + " GROUP BY DepositDate";
        cmd = new SqlCommand(sSql, con);
        con.Open();
        dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            sSql = "";
            sSql = "INSERT INTO Temp_DepositStatement(";
            sSql = sSql + " DDate,UserID,EID";
            sSql = sSql + " )";
            sSql = sSql + " VALUES(";
            sSql = sSql + " '" + dr["DepositDate"] + "','" + Session["UserName"] + "','" + Session["sBrId"] + "'";
            sSql = sSql + " )";
            SqlCommand cmdS = new SqlCommand(sSql, con1);
            con1.Open();
            cmdS.ExecuteNonQuery();
            con1.Close();

        }

        con.Close();
        dr.Close();
        */

        //-------
        /*
        sSql = "";
        sSql = "SELECT TDate FROM dbo.MRSRMaster";
        sSql = sSql + " WHERE (OutSource = '" + Session["sBrId"] + "')";
        sSql = sSql + " AND (TrType = 3) AND (TDate >= '" + txtFrom.Text + "') AND (TDate <= '" + txtToDate.Text + "')";
        sSql = sSql + " GROUP BY TDate";
        sSql = sSql + " UNION";
        sSql = sSql + " SELECT DepositDate AS TDate FROM dbo.tbDeposit";
        sSql = sSql + " WHERE (DepositDate >= '" + txtFrom.Text + "') AND (DepositDate <= '" + txtToDate.Text + "') AND ";
        sSql = sSql + " (EID = '" + Session["sBrId"] + "')";
        sSql = sSql + " GROUP BY DepositDate";
        SqlCommand cmd = new SqlCommand(sSql, con);
        con.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            sSql = "";
            sSql = "INSERT INTO Temp_DepositStatement(";
            sSql = sSql + " DDate,UserID,EID";
            sSql = sSql + " )";
            sSql = sSql + " VALUES(";
            sSql = sSql + " '" + dr["TDate"] + "','" + Session["UserName"] + "','" + Session["sBrId"] + "'";
            sSql = sSql + " )";
            SqlCommand cmdS = new SqlCommand(sSql, con1);
            con1.Open();
            cmdS.ExecuteNonQuery();
            con1.Close();

        }

        con.Close();
        dr.Close();
         */ 
        //----------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------


        DateTime startDate = DateTime.ParseExact(txtFrom.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);
        DateTime stopDate = DateTime.ParseExact(txtToDate.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);
        double interval = (startDate.Date - stopDate.Date).TotalDays;

        //for (DateTime dateTime = startDate; dateTime < stopDate; dateTime += TimeSpan.FromDays(interval))
        for (DateTime dateTime = startDate; dateTime <= stopDate; dateTime = dateTime.AddDays(1))
        {
            //INSERT DATE
            sSql = "";
            sSql = "INSERT INTO Temp_DepositStatement(";
            sSql = sSql + " DDate,UserID,EID";
            sSql = sSql + " )";
            sSql = sSql + " VALUES(";
            sSql = sSql + " '" + dateTime + "','" + Session["UserName"] + "','" + Session["sBrId"] + "'";
            sSql = sSql + " )";
            SqlCommand cmdDt = new SqlCommand(sSql, con1);
            con1.Open();
            cmdDt.ExecuteNonQuery();
            con1.Close();
        
            //----------------------------------------------------------------------------------------
            // OPENING SALES AMOUNT
            sSql = "";
            sSql = "SELECT OutSource, SUM(ISNULL(NetSalesAmnt,0)) AS tAmnt";
            sSql = sSql + " FROM dbo.MRSRMaster";
            sSql = sSql + " WHERE (TDate >= '7/1/2019') AND (TDate < '" + dateTime + "')";
            sSql = sSql + " AND (OutSource = '" + Session["sBrId"] + "') AND (TrType = 3)";
            sSql = sSql + " GROUP BY OutSource";
            SqlCommand cmd = new SqlCommand(sSql, con);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                sSql = "";
                sSql = "UPDATE Temp_DepositStatement ";            
                sSql = sSql + " SET OBSales='" + dr["tAmnt"] + "'";
                sSql = sSql + " WHERE DDate='" + dateTime + "'";
                sSql = sSql + " AND UserID='" + Session["UserName"] + "' AND EID='" + Session["sBrId"] + "'";
            
                SqlCommand cmdS = new SqlCommand(sSql, con1);
                con1.Open();
                cmdS.ExecuteNonQuery();
                con1.Close();
            }

            con.Close();
            dr.Close();


            // CURRENT SALES AMOUNT
            sSql = "";
            sSql = "SELECT OutSource, SUM(ISNULL(NetSalesAmnt,0)) AS tAmnt, TDate";
            sSql = sSql + " FROM dbo.MRSRMaster";
            sSql = sSql + " WHERE (TDate = '" + dateTime + "') ";
            sSql = sSql + " AND (OutSource = '" + Session["sBrId"] + "') AND (TrType = 3)";
            sSql = sSql + " GROUP BY OutSource, TDate";
            cmd = new SqlCommand(sSql, con);
            con.Open();
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                sSql = "";
                sSql = "UPDATE Temp_DepositStatement ";
                sSql = sSql + " SET SalesAmnt='" + dr["tAmnt"] + "'";
                sSql = sSql + " WHERE DDate='" + dateTime + "'";
                sSql = sSql + " AND UserID='" + Session["UserName"] + "' AND EID='" + Session["sBrId"] + "'";
            
                SqlCommand cmdS = new SqlCommand(sSql, con1);
                con1.Open();
                cmdS.ExecuteNonQuery();
                con1.Close();
            }

            con.Close();
            dr.Close();
            //----------------------------------------------------------------------------------------


            //----------------------------------------------------------------------------------------
            // OPENING WITHDRAWN AMOUNT
            sSql = "";
            sSql = "SELECT InSource, SUM(ISNULL(NetSalesAmnt,0)) AS tAmnt, TDate";
            sSql = sSql + " FROM dbo.MRSRMaster";
            sSql = sSql + " WHERE (TDate >= '7/1/2019') AND (TDate < '" + dateTime + "')";
            sSql = sSql + " AND (InSource = '" + Session["sBrId"] + "') AND (TrType = -3)";
            sSql = sSql + " GROUP BY InSource, TDate";
            cmd = new SqlCommand(sSql, con);
            con.Open();
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                sSql = "";
                sSql = "UPDATE Temp_DepositStatement ";
                sSql = sSql + " SET OBWith='" + dr["tAmnt"] + "'";
                sSql = sSql + " WHERE DDate='" + dateTime + "'";
                sSql = sSql + " AND UserID='" + Session["UserName"] + "' AND EID='" + Session["sBrId"] + "'";
            
                SqlCommand cmdS = new SqlCommand(sSql, con1);
                con1.Open();
                cmdS.ExecuteNonQuery();
                con1.Close();
            }

            con.Close();
            dr.Close();


            // CURRENT WITHDRAWN AMOUNT
            sSql = "";
            sSql = "SELECT InSource, SUM(ISNULL(NetSalesAmnt,0)) AS tAmnt, TDate";
            sSql = sSql + " FROM dbo.MRSRMaster";
            sSql = sSql + " WHERE (TDate = '" + dateTime + "') ";
            sSql = sSql + " AND (InSource = '" + Session["sBrId"] + "') AND (TrType = -3)";
            sSql = sSql + " GROUP BY InSource, TDate";
            cmd = new SqlCommand(sSql, con);
            con.Open();
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                sSql = "";
                sSql = "UPDATE Temp_DepositStatement ";
                sSql = sSql + " SET WithAmnt='" + dr["tAmnt"] + "'";
                sSql = sSql + " WHERE DDate='" + dateTime + "'";
                sSql = sSql + " AND UserID='" + Session["UserName"] + "' AND EID='" + Session["sBrId"] + "'";
            
                SqlCommand cmdS = new SqlCommand(sSql, con1);
                con1.Open();
                cmdS.ExecuteNonQuery();
                con1.Close();
            }

            con.Close();
            dr.Close();
            //----------------------------------------------------------------------------------------


            //----------------------------------------------------------------------------------------
            // OPENING DEPOSIT AMOUNT
            sSql = "";
            sSql = "SELECT EID, SUM(ISNULL(DepositAmnt,0)) AS DAmnt";
            sSql = sSql + " FROM dbo.tbDeposit";
            sSql = sSql + " WHERE (DepositDate >= '7/1/2019') AND (DepositDate < '" + dateTime + "')";
            sSql = sSql + " AND (EID = '" + Session["sBrId"] + "')";
            sSql = sSql + " GROUP BY EID";
            cmd = new SqlCommand(sSql, con);
            con.Open();
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                sSql = "";
                sSql = "UPDATE Temp_DepositStatement ";
                sSql = sSql + " SET OBDeposit='" + dr["DAmnt"] + "'";
                sSql = sSql + " WHERE DDate='" + dateTime + "'";
                sSql = sSql + " AND UserID='" + Session["UserName"] + "' AND EID='" + Session["sBrId"] + "'";
            
                SqlCommand cmdS = new SqlCommand(sSql, con1);
                con1.Open();
                cmdS.ExecuteNonQuery();
                con1.Close();
            }

            con.Close();
            dr.Close();


            // CURRENT DEPOSIT AMOUNT
            sSql = "";
            sSql = "SELECT EID, SUM(ISNULL(DepositAmnt,0)) AS DAmnt";
            sSql = sSql + " FROM dbo.tbDeposit";
            sSql = sSql + " WHERE (DepositDate = '" + dateTime + "') ";
            sSql = sSql + " AND (EID = '" + Session["sBrId"] + "')";
            sSql = sSql + " GROUP BY EID";
            cmd = new SqlCommand(sSql, con);
            con.Open();
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                sSql = "";
                sSql = "UPDATE Temp_DepositStatement ";
                sSql = sSql + " SET DepositAmnt='" + dr["DAmnt"] + "'";
                sSql = sSql + " WHERE DDate='" + dateTime + "'";
                sSql = sSql + " AND UserID='" + Session["UserName"] + "' AND EID='" + Session["sBrId"] + "'";
            
                SqlCommand cmdS = new SqlCommand(sSql, con1);
                con1.Open();
                cmdS.ExecuteNonQuery();
                con1.Close();
            }

            con.Close();
            dr.Close();
            //----------------------------------------------------------------------------------------

        }

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

            //e.Row.Cells[8].HorizontalAlign = HorizontalAlign.Center;
            

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