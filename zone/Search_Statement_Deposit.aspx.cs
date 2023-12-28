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

public partial class Search_Statement_Deposit : System.Web.UI.Page
{
    SqlConnection conn = DBConnection.GetConnection();
    long i;

    private double runningTotalQty = 0;
    private double runningTotalTP = 0;
    private double runningTotalCash = 0;
    private double runningTotalCard = 0;
    private double runningTotalCheque = 0;
    private double runningTotalReq = 0;
    private double runningTotalOP = 0;
    private double runningTotalbKash = 0;

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
            " ISNULL(CashAmnt,0) AS CashAmnt, ISNULL(CardAmnt,0) AS CardAmnt, ISNULL(OnlinePaymentAmnt,0) AS OnlinePaymentAmnt, " +
            " ISNULL(ChequeAmnt,0) AS ChequeAmnt, ISNULL(ReqAmnt,0) AS ReqAmnt, ISNULL(bKashAmnt,0) AS bKashAmnt," +
            " ISNULL(CashAmnt,0) + ISNULL(CardAmnt,0) +  ISNULL(OnlinePaymentAmnt,0) + " +
            " ISNULL(ChequeAmnt,0) + ISNULL(ReqAmnt,0) + ISNULL(bKashAmnt,0) AS TAmnt" +

            " FROM Temp_DepositStatement " +

            " WHERE (UserID = '" + Session["UserName"] + "') " +
            " AND (EID='" + Session["EID"] + "')" +
                    
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
        sSql = sSql + " AND EID='" + Session["EID"] + "'";

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
        sSql = sSql + " AND (OutSource = '" + Session["EID"] + "') AND (TrType = 3)";
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
            sSql = sSql + " '" + dr["TDate"] + "','" + Session["UserName"] + "','" + Session["EID"] + "'";
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
        sSql = sSql + " WHERE (DepositDate >= '" + txtFrom.Text + "') AND (DepositDate <= '" + txtToDate.Text + "') AND (EID='" + Session["EID"] + "') AND";
        sSql = sSql + " (DepositDate NOT IN ";
        sSql = sSql + " (SELECT TDate";
        sSql = sSql + " FROM dbo.MRSRMaster ";
        sSql = sSql + " WHERE (TDate >= '" + txtFrom.Text + "') AND (TDate <= '" + txtToDate.Text + "') AND (OutSource='" + Session["EID"] + "')";
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
            sSql = sSql + " '" + dr["DepositDate"] + "','" + Session["UserName"] + "','" + Session["EID"] + "'";
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

        // INSERT DATE FROM DEPOSIT TABLE
        sSql = "";
        sSql = "SELECT DepositDate";
        sSql = sSql + " FROM dbo.tbDeposit";
        sSql = sSql + " WHERE (DepositDate >= '" + txtFrom.Text + "') AND (DepositDate <= '" + txtToDate.Text + "')";
        sSql = sSql + " AND (EID='" + Session["EID"] + "')";        
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
            sSql = sSql + " '" + dr["DepositDate"] + "','" + Session["UserName"] + "','" + Session["EID"] + "'";
            sSql = sSql + " )";
            SqlCommand cmdS = new SqlCommand(sSql, con1);
            con1.Open();
            cmdS.ExecuteNonQuery();
            con1.Close();

        }

        con.Close();
        dr.Close();

        //----------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------


        DateTime startDate = DateTime.ParseExact(txtFrom.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);
        DateTime stopDate = DateTime.ParseExact(txtToDate.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);
        double interval = (startDate.Date - stopDate.Date).TotalDays;

        //***************************************************************************************************
        //for (DateTime dateTime = startDate; dateTime < stopDate; dateTime += TimeSpan.FromDays(interval))
        for (DateTime dateTime = startDate; dateTime <= stopDate; dateTime = dateTime.AddDays(1))
        {
              
            //----------------------------------------------------------------------------------------
            // CASH DEPOSIT AMOUNT
            sSql = "";
            sSql = "SELECT EID, SUM(ISNULL(DepositAmnt,0)) AS DAmnt";
            sSql = sSql + " FROM dbo.tbDeposit";
            sSql = sSql + " WHERE (DepositDate = '" + dateTime + "')";
            sSql = sSql + " AND (EID = '" + Session["EID"] + "')";
            sSql = sSql + " AND (DepositType = 'CASH')";
            sSql = sSql + " GROUP BY EID";
            cmd = new SqlCommand(sSql, con);
            con.Open();
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                sSql = "";
                sSql = "UPDATE Temp_DepositStatement ";
                sSql = sSql + " SET CashAmnt='" + dr["DAmnt"] + "'";
                sSql = sSql + " WHERE DDate='" + dateTime + "'";
                sSql = sSql + " AND UserID='" + Session["UserName"] + "' AND EID='" + Session["EID"] + "'";
            
                SqlCommand cmdS = new SqlCommand(sSql, con1);
                con1.Open();
                cmdS.ExecuteNonQuery();
                con1.Close();
            }

            con.Close();
            dr.Close();

            // CARD DEPOSIT AMOUNT
            sSql = "";
            sSql = "SELECT EID, SUM(ISNULL(DepositAmnt,0)) AS DAmnt";
            sSql = sSql + " FROM dbo.tbDeposit";
            sSql = sSql + " WHERE (DepositDate = '" + dateTime + "')";
            sSql = sSql + " AND (EID = '" + Session["EID"] + "')";
            sSql = sSql + " AND (DepositType = 'CARD')";
            sSql = sSql + " GROUP BY EID";
            cmd = new SqlCommand(sSql, con);
            con.Open();
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                sSql = "";
                sSql = "UPDATE Temp_DepositStatement ";
                sSql = sSql + " SET CardAmnt='" + dr["DAmnt"] + "'";
                sSql = sSql + " WHERE DDate='" + dateTime + "'";
                sSql = sSql + " AND UserID='" + Session["UserName"] + "' AND EID='" + Session["EID"] + "'";

                SqlCommand cmdS = new SqlCommand(sSql, con1);
                con1.Open();
                cmdS.ExecuteNonQuery();
                con1.Close();
            }

            con.Close();
            dr.Close();


            // CHEQUE DEPOSIT AMOUNT
            sSql = "";
            sSql = "SELECT EID, SUM(ISNULL(DepositAmnt,0)) AS DAmnt";
            sSql = sSql + " FROM dbo.tbDeposit";
            sSql = sSql + " WHERE (DepositDate = '" + dateTime + "')";
            sSql = sSql + " AND (EID = '" + Session["EID"] + "')";
            sSql = sSql + " AND (DepositType = 'CHEQUE')";
            sSql = sSql + " GROUP BY EID";
            cmd = new SqlCommand(sSql, con);
            con.Open();
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                sSql = "";
                sSql = "UPDATE Temp_DepositStatement ";
                sSql = sSql + " SET ChequeAmnt='" + dr["DAmnt"] + "'";
                sSql = sSql + " WHERE DDate='" + dateTime + "'";
                sSql = sSql + " AND UserID='" + Session["UserName"] + "' AND EID='" + Session["EID"] + "'";

                SqlCommand cmdS = new SqlCommand(sSql, con1);
                con1.Open();
                cmdS.ExecuteNonQuery();
                con1.Close();
            }

            con.Close();
            dr.Close();


            // REQUISITION DEPOSIT AMOUNT
            sSql = "";
            sSql = "SELECT EID, SUM(ISNULL(DepositAmnt,0)) AS DAmnt";
            sSql = sSql + " FROM dbo.tbDeposit";
            sSql = sSql + " WHERE (DepositDate = '" + dateTime + "')";
            sSql = sSql + " AND (EID = '" + Session["EID"] + "')";
            sSql = sSql + " AND (DepositType = 'REQUISITION')";
            sSql = sSql + " GROUP BY EID";
            cmd = new SqlCommand(sSql, con);
            con.Open();
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                sSql = "";
                sSql = "UPDATE Temp_DepositStatement ";
                sSql = sSql + " SET ReqAmnt='" + dr["DAmnt"] + "'";
                sSql = sSql + " WHERE DDate='" + dateTime + "'";
                sSql = sSql + " AND UserID='" + Session["UserName"] + "' AND EID='" + Session["EID"] + "'";

                SqlCommand cmdS = new SqlCommand(sSql, con1);
                con1.Open();
                cmdS.ExecuteNonQuery();
                con1.Close();
            }

            con.Close();
            dr.Close();


            // bKASH DEPOSIT AMOUNT
            sSql = "";
            sSql = "SELECT EID, SUM(ISNULL(DepositAmnt,0)) AS DAmnt";
            sSql = sSql + " FROM dbo.tbDeposit";
            sSql = sSql + " WHERE (DepositDate = '" + dateTime + "')";
            sSql = sSql + " AND (EID = '" + Session["EID"] + "')";
            sSql = sSql + " AND (DepositType = 'bKash')";
            sSql = sSql + " GROUP BY EID";
            cmd = new SqlCommand(sSql, con);
            con.Open();
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                sSql = "";
                sSql = "UPDATE Temp_DepositStatement ";
                sSql = sSql + " SET bKashAmnt='" + dr["DAmnt"] + "'";
                sSql = sSql + " WHERE DDate='" + dateTime + "'";
                sSql = sSql + " AND UserID='" + Session["UserName"] + "' AND EID='" + Session["EID"] + "'";

                SqlCommand cmdS = new SqlCommand(sSql, con1);
                con1.Open();
                cmdS.ExecuteNonQuery();
                con1.Close();
            }

            con.Close();
            dr.Close();

            // ONLINE PAYMENT AMOUNT
            sSql = "";
            sSql = "SELECT EID, SUM(ISNULL(DepositAmnt,0)) AS DAmnt";
            sSql = sSql + " FROM dbo.tbDeposit";
            sSql = sSql + " WHERE (DepositDate = '" + dateTime + "')";
            sSql = sSql + " AND (EID = '" + Session["EID"] + "')";
            sSql = sSql + " AND (DepositType = 'Online Payment')";
            sSql = sSql + " GROUP BY EID";
            cmd = new SqlCommand(sSql, con);
            con.Open();
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                sSql = "";
                sSql = "UPDATE Temp_DepositStatement ";
                sSql = sSql + " SET OnlinePaymentAmnt='" + dr["DAmnt"] + "'";
                sSql = sSql + " WHERE DDate='" + dateTime + "'";
                sSql = sSql + " AND UserID='" + Session["UserName"] + "' AND EID='" + Session["EID"] + "'";

                SqlCommand cmdS = new SqlCommand(sSql, con1);
                con1.Open();
                cmdS.ExecuteNonQuery();
                con1.Close();
            }

            con.Close();
            dr.Close();

            //----------------------------------------------------------------------------------------

        }
        //***************************************************************************************************

    }


    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //CalcTotalQty(e.Row.Cells[4].Text);
            

            CalcTotal_Cash(e.Row.Cells[2].Text);
            CalcTotal_Card(e.Row.Cells[3].Text);
            CalcTotal_Cheque(e.Row.Cells[4].Text);
            CalcTotal_Req(e.Row.Cells[5].Text);

            CalcTotal_OP(e.Row.Cells[6].Text);

            CalcTotal_bKash(e.Row.Cells[7].Text);

            CalcTotal_TP(e.Row.Cells[8].Text);

            // ALIGNMENT
            e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Left;

            e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Center;

            e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[7].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[8].HorizontalAlign = HorizontalAlign.Center;

            //e.Row.Cells[8].HorizontalAlign = HorizontalAlign.Center;
            

        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[1].Text = "Total";
            //e.Row.Cells[4].Text = runningTotalQty.ToString("0,0", CultureInfo.InvariantCulture);
            
            e.Row.Cells[2].Text = runningTotalCash.ToString("0,0", CultureInfo.InvariantCulture);
            e.Row.Cells[3].Text = runningTotalCard.ToString("0,0", CultureInfo.InvariantCulture);
            e.Row.Cells[4].Text = runningTotalCheque.ToString("0,0", CultureInfo.InvariantCulture);
            e.Row.Cells[5].Text = runningTotalReq.ToString("0,0", CultureInfo.InvariantCulture);

            e.Row.Cells[6].Text = runningTotalOP.ToString("0,0", CultureInfo.InvariantCulture);
            e.Row.Cells[7].Text = runningTotalbKash.ToString("0,0", CultureInfo.InvariantCulture);

            e.Row.Cells[8].Text = runningTotalTP.ToString("0,0", CultureInfo.InvariantCulture);

            //ALIGNMENT
            e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Left;

            e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Center;

            e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[7].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[8].HorizontalAlign = HorizontalAlign.Center;
            
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

    //CALCULATE TOTAL CHEQUE
    private void CalcTotal_Cheque(string _price)
    {
        try
        {
            runningTotalCheque += Double.Parse(_price);
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write(ex.ToString());
        }
    }

    //CALCULATE TOTAL REQUISITION
    private void CalcTotal_Req(string _price)
    {
        try
        {
            runningTotalReq += Double.Parse(_price);
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write(ex.ToString());
        }
    }

    //CALCULATE TOTAL Online Payment
    private void CalcTotal_OP(string _price)
    {
        try
        {
            runningTotalOP += Double.Parse(_price);
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write(ex.ToString());
        }
    }

    //CALCULATE TOTAL BKASH
    private void CalcTotal_bKash(string _price)
    {
        try
        {
            runningTotalbKash += Double.Parse(_price);
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