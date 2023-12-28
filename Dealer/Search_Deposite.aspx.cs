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

public partial class Search_Deposite_info : System.Web.UI.Page
{
    SqlConnection conn = DBConnectionDSM.GetConnection();
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
            fnLoadCombo_Item(ddlBank, "BName", "ID", "tbBankList");
            //fnLoadCombo_Item(ddlBank, "BankName", "BankName", "DepositAmnt");

            this.txtFrom.Text = DateTime.Today.ToString("MM/dd/yyyy");
            this.txtToDate.Text = DateTime.Today.ToString("MM/dd/yyyy");

            //LoadDropDownList_BankName();
            LoadDropDownList_Dealer();

        }

        

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
        ddlDealerName.DataSource = cmd.ExecuteReader();
        ddlDealerName.DataTextField = "Name";
        ddlDealerName.DataValueField = "DAID";
        ddlDealerName.DataBind();

        //Add blank item at index 0.
        //ddlEntity.Items.Insert(0, new ListItem("", "0"));
        ddlDealerName.Items.Insert(0, new ListItem("ALL", "ALL"));
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

    //LOAD Dealer IN DROPDOWN LIST
    protected void LoadDropDownList_BankName()
    {
        SqlConnection conn = DBConnectionDSM.GetConnection();

        String strQuery = "SELECT BankName FROM DepositAmnt ";
        //strQuery = strQuery + " WHERE (Discontinue = 'No') AND (CategoryID='" + Session["sZoneID"].ToString() + "')";
        strQuery = strQuery + " ORDER BY BankName ";
        //SqlConnection con = new SqlConnection("conn");
        SqlCommand cmd = new SqlCommand(strQuery, conn);
        //SqlCommand cmd = new SqlCommand();
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = strQuery;
        cmd.Connection = conn;
        //try
        //{
        conn.Open();
        ddlDealerName.DataSource = cmd.ExecuteReader();
        ddlDealerName.DataTextField = "BankName";
        ddlDealerName.DataValueField = "BankName";
        ddlDealerName.DataBind();

        //Add blank item at index 0.
        //ddlEntity.Items.Insert(0, new ListItem("", "0"));
        ddlDealerName.Items.Insert(0, new ListItem("ALL", "ALL"));
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
        SqlConnection con = DBConnectionDSM.GetConnection();
        con.Open();
                
        string sSql = "";
        //sSql = "SELECT CANO,CollectionNo, CONVERT(VARCHAR(10), CDate, 105) AS DepositDate, BankName, ";
        //sSql = sSql + " BranchName, cAmount, DepositType, DepositBy, Remarks";
        //sSql = sSql + " FROM tbDeposit ";

        //sSql = sSql + " WHERE (DepositDate >= '" + Convert.ToDateTime(this.txtFrom.Text) + "'";
        //sSql = sSql + " AND DepositDate <= '" + Convert.ToDateTime(this.txtToDate.Text) + "')";
        //sSql = sSql + " AND (EID='" + Session["EID"] + "')";


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

        if (ddlDealerName.SelectedItem.Text != "ALL")
        {
            sSql = sSql + " AND (dbo.DelearInfo.Name = '" + ddlDealerName.SelectedItem.Text + "')";
        }

        if (ddlBank.SelectedItem.Text != "ALL")
        {
            sSql = sSql + " AND (dbo.DepositAmnt.BankName = '" + ddlBank.SelectedItem.Text + "')";
        }

        if (ddlDepositeType.SelectedItem.Text != "ALL")
        {
            sSql = sSql + " AND (dbo.DepositAmnt.PayType = '" + ddlDepositeType.SelectedItem.Text + "')";
        }


        sSql = sSql + " ORDER BY dbo.DepositAmnt.CDate, dbo.DepositAmnt.CollectionNo Desc";

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


    private void fnLoadCombo_Item(DropDownList xCombo, string pField, string pFieldID, string pTable)
    {
        SqlConnection conn = DBConnection.GetConnection();

        try
        {
            conn.Open();
            string sqlfns = "SELECT * FROM " + pTable + " ORDER BY " + pField + " ASC";
            SqlCommand cmd = new SqlCommand(sqlfns, conn);

            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sqlfns;
            cmd.Connection = conn;

            xCombo.DataSource = cmd.ExecuteReader();
            xCombo.DataTextField = pField;
            //ddlEmp.DataValueField = "SupName";
            xCombo.DataValueField = pFieldID;
            xCombo.DataBind();

            //Add blank item at index 0.
            xCombo.Items.Insert(0, new ListItem("ALL", "ALL"));

            conn.Close();
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
        finally
        {
            conn.Close();
        }

    }



}