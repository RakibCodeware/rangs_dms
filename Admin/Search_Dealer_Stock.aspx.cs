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

public partial class Search_Dealer_Stock : System.Web.UI.Page
{
    SqlConnection conn = DBConnectionDSM.GetConnection();
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
        int role = Convert.ToInt32(Session["RolesId"]);

        if (role != 1)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            this.txtFrom.Text = DateTime.Today.ToString("MM/dd/yyyy");
            this.txtToDate.Text = DateTime.Today.ToString("MM/dd/yyyy");

            //LOAD CTP
            //LoadDropDownList_CTP();

            LoadDropDownList_Dealer();
            LoadDropDownList_Category();

            dt = new DataTable();
            MakeTable();

        }

        else
        {
            dt = (DataTable)ViewState["dt"];
        }
        ViewState["dt"] = dt;


    }

    //LOAD CATEGORY IN DROPDOWN LIST
    protected void LoadDropDownList_Category()
    {
        if (System.Convert.ToInt32(Session["Vis"]) == 0)
        {
            Response.Redirect("../Account/Login.aspx");
        }

        SqlConnection conn = DBConnection.GetConnection();
        String strQuery = "select GroupName from Product GROUP BY GroupName Order By GroupName";
        //SqlConnection con = new SqlConnection("conn");
        SqlCommand cmd = new SqlCommand(strQuery, conn);
        //SqlCommand cmd = new SqlCommand();
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = strQuery;
        cmd.Connection = conn;
        try
        {
            conn.Open();
            ddlCat.DataSource = cmd.ExecuteReader();
            ddlCat.DataTextField = "GroupName";
            //ddlCat.DataValueField = "ProductID";
            ddlCat.DataValueField = "GroupName";
            ddlCat.DataBind();

            //Add blank item at index 0.
            ddlCat.Items.Insert(0, new ListItem("ALL", "ALL"));


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
        ddlEntity.DataSource = cmd.ExecuteReader();
        ddlEntity.DataTextField = "Name";
        ddlEntity.DataValueField = "DAID";
        ddlEntity.DataBind();

        //Add blank item at index 0.
        //ddlEntity.Items.Insert(0, new ListItem("", "0"));
        ddlEntity.Items.Insert(0, new ListItem("ALL", "ALL"));
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
        SqlConnection conn = DBConnectionDSM.GetConnection();
        conn.Open();

        string sSql = "";
        
        sSql = "SELECT dbo.Product.ProductID, dbo.Product.Code, dbo.Product.ProdName, dbo.Product.Model, dbo.Product.GroupName,";
        sSql = sSql + " SUM(ISNULL(a.QtyOut, 0)) AS tDealerOut, SUM(ISNULL(b.QtyIN, 0)) AS tDealerIn,";
        sSql = sSql + " SUM(ISNULL(b.QtyIN, 0)) - SUM(ISNULL(a.QtyOut, 0)) AS tStock";
        sSql = sSql + " FROM dbo.Product LEFT OUTER JOIN";
        sSql = sSql + " (SELECT dbo.DelearInfo.Name, Product_2.Model AS Model1, Product_2.GroupName,";
        sSql = sSql + " SUM(CASE WHEN (TrType = 1 OR TrType = 2 OR TrType = 3 OR TrType = 4 OR TrType = - 1 OR";
        sSql = sSql + " TrType = - 3) THEN ABS(d2.Qty) ELSE 0 END) AS QtyIN, 0 AS QtyOut";
        sSql = sSql + " FROM dbo.MRSRMaster INNER JOIN";
        sSql = sSql + " dbo.MRSRDetails AS d2 ON dbo.MRSRMaster.MRSRMID = d2.MRSRMID INNER JOIN";
        sSql = sSql + " dbo.Product AS Product_2 ON d2.ProductID = Product_2.ProductID INNER JOIN";
        sSql = sSql + " dbo.DelearInfo ON dbo.MRSRMaster.InSource = dbo.DelearInfo.DAID";
        sSql = sSql + " WHERE (dbo.DelearInfo.DAID IS NOT NULL) ";
        //sSql = sSql + " AND (dbo.DelearInfo.CategoryID='" + Session["sZoneID"].ToString() + "') ";
        if (ddlEntity.SelectedItem.Text !="ALL")
        {
            sSql = sSql + " AND (dbo.DelearInfo.Name = '" + ddlEntity.SelectedItem.Text + "')";
        }
        if (ddlCat.SelectedItem.Text != "ALL")
        {
            sSql = sSql + " AND (Product_2.GroupName = '" + ddlCat.SelectedItem.Text + "')";
        }
        sSql = sSql + " GROUP BY dbo.DelearInfo.Name, Product_2.Model, Product_2.GroupName) AS b ON dbo.Product.Model = b.Model1 LEFT OUTER JOIN";
        sSql = sSql + " (SELECT DelearInfo_1.Name, DelearInfo_1.CategoryID, Product_1.Model AS Model1, Product_1.GroupName, SUM(CASE WHEN (TrType = 1 OR";
        sSql = sSql + " TrType = 2 OR TrType = 3 OR TrType = 4 OR TrType = - 1 OR TrType = - 3)";
        sSql = sSql + " THEN ABS(d1.Qty) ELSE 0 END) AS QtyOut, 0 AS QtyIN";
        sSql = sSql + " FROM  dbo.MRSRMaster AS MRSRMaster_1 INNER JOIN";
        sSql = sSql + " dbo.MRSRDetails AS d1 ON MRSRMaster_1.MRSRMID = d1.MRSRMID INNER JOIN";
        sSql = sSql + " dbo.Product AS Product_1 ON d1.ProductID = Product_1.ProductID INNER JOIN";
        sSql = sSql + " dbo.DelearInfo AS DelearInfo_1 ON MRSRMaster_1.OutSource = DelearInfo_1.DAID";
        sSql = sSql + " WHERE (DelearInfo_1.DAID IS NOT NULL)";
        //sSql = sSql + " AND (DelearInfo_1.CategoryID='" + Session["sZoneID"].ToString() + "') ";
        if (ddlEntity.SelectedItem.Text != "ALL")
        {
            sSql = sSql + " AND (DelearInfo_1.Name = '" + ddlEntity.SelectedItem.Text + "')";
        }
        if (ddlCat.SelectedItem.Text != "ALL")
        {
            sSql = sSql + " AND (Product_1.GroupName = '" + ddlCat.SelectedItem.Text + "')";
        }
        sSql = sSql + " GROUP BY DelearInfo_1.Name, DelearInfo_1.CategoryID, Product_1.Model, Product_1.GroupName) AS a ON dbo.Product.Model = a.Model1";
        sSql = sSql + " GROUP BY dbo.Product.ProductID, dbo.Product.Code, dbo.Product.ProdName, dbo.Product.Model, dbo.Product.GroupName";
        sSql = sSql + " HAVING (SUM(ISNULL(b.QtyIn, 0)) - SUM(ISNULL(a.QtyOut, 0)) <> 0)";
        sSql = sSql + " ORDER BY dbo.Product.GroupName, dbo.Product.Model";


        SqlCommand cmd = new SqlCommand(sSql, conn);        
        //OleDbDataReader dr = cmd.ExecuteReader();

        DataSet ds = new DataSet();
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        da.Fill(ds);

        GridView1.DataSource = ds;
        GridView1.DataBind();
        //dr.Close();
        conn.Close();

    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CalcTotalQty(e.Row.Cells[2].Text);
            CalcTotal_TP(e.Row.Cells[3].Text);

            CalcTotal_Cash(e.Row.Cells[4].Text);
            //CalcTotal_Card(e.Row.Cells[5].Text);

            // ALIGNMENT
            e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Left;

            e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Right;
                                    

        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[1].Text = "Total";
            e.Row.Cells[2].Text = runningTotalQty.ToString("0,0", CultureInfo.InvariantCulture);
            e.Row.Cells[3].Text = runningTotalTP.ToString("0,0", CultureInfo.InvariantCulture);
            e.Row.Cells[4].Text = runningTotalCash.ToString("0,0", CultureInfo.InvariantCulture);
            //e.Row.Cells[5].Text = runningTotalCard.ToString("0,0", CultureInfo.InvariantCulture);


            //ALIGNMENT
            e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Left;

            e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Right;
            
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


 
    protected void OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        GridView1.DataBind();
        fnLoadData();
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

    

    
}