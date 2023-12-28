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

public partial class Search_Sales_spin : System.Web.UI.Page
{
    SqlConnection conn = DBConnectionSpin.GetConnection();
    long i;

    private double runningTotalQty = 0;
    private double runningTotalTP = 0;
    private double runningTotalCash = 0;
    private double runningTotalCard = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (System.Convert.ToInt32(Session["Vis"]) == 0)
        {
            Response.Redirect("../Default.aspx");
        }

        if (!IsPostBack)
        {
            this.txtFrom.Text = DateTime.Today.ToString("MM/dd/yyyy");
            this.txtToDate.Text = DateTime.Today.ToString("MM/dd/yyyy");

            //LOAD CTP
            LoadDropDownList_Model();

        }

    }

    //LOAD CTP IN DROPDOWN LIST
    protected void LoadDropDownList_Model()
    {
        SqlConnection conn = DBConnectionSpin.GetConnection();

        String strQuery = "Select ProductID, Model  from tbProduct ";
        strQuery = strQuery + " ORDER BY Model";
        //SqlConnection con = new SqlConnection("conn");
        SqlCommand cmd = new SqlCommand(strQuery, conn);
        //SqlCommand cmd = new SqlCommand();
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = strQuery;
        cmd.Connection = conn;
        try
        {
            conn.Open();
            ddlModel.DataSource = cmd.ExecuteReader();
            ddlModel.DataTextField = "Model";
            ddlModel.DataValueField = "ProductID";
            ddlModel.DataBind();

            //Add blank item at index 0.
            ddlModel.Items.Insert(0, new ListItem("ALL", "0"));

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
        //s = WebConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        //con = new SqlConnection(s);
        SqlConnection con = DBConnectionSpin.GetConnection();
        con.Open();

        string sSql = "";                
        sSql ="SELECT  dbo.tbCustomer.CustAID, dbo.tbCustomer.CustName, dbo.tbCustomer.CustAdd,";
        sSql = sSql + " dbo.tbCustomer.CustMobile, dbo.tbCustomer.ProductID, dbo.tbCustomer.DisCode,";
        sSql = sSql + " dbo.tbProduct.Model, dbo.tbProduct.Description, dbo.tbProduct.MinDis,";
        sSql = sSql + " dbo.tbProduct.MaxDis, dbo.tbCustomer.Dis_Gift, dbo.tbCustomer.ChNo, ";
        sSql = sSql + "  CASE dbo.tbCustomer.sTag WHEN 0 THEN 'Pending Toss & Win' ELSE 'Done Toss & Win' END AS sStatus,";
        sSql = sSql + " dbo.tbCustomer.sTag, dbo.tbCustomer.EntryDate, dbo.tbCustomer.EntryTime";

        sSql = sSql + " FROM  dbo.tbCustomer INNER JOIN";
        sSql = sSql + " dbo.tbProduct ON dbo.tbCustomer.ProductID = dbo.tbProduct.ProductID";

        sSql = sSql + " WHERE dbo.tbCustomer.EID='" + Session["EID"].ToString() + "'";
        sSql = sSql + " AND (dbo.tbCustomer.EntryDate >= '" + Convert.ToDateTime(this.txtFrom.Text) + "'";
        sSql = sSql + " AND dbo.tbCustomer.EntryDate <= '" + Convert.ToDateTime(this.txtToDate.Text) + "')";

        if (this.ddlModel.SelectedItem.Text != "ALL")
        {
            sSql = sSql + " AND (dbo.tbProduct.Model ='" + this.ddlModel.SelectedItem.Text + "')";
        }
        if (this.txtInvNo.Text.Length != 0)
        {
            sSql = sSql + " AND (dbo.tbCustomer.ChNo='" + this.txtInvNo.Text + "')";
        }
        if (this.txtMobile.Text.Length != 0)
        {
            sSql = sSql + " AND (dbo.tbCustomer.CustMobile='" + this.txtMobile.Text + "')";
        }

        sSql = sSql + " ORDER BY dbo.tbCustomer.CustAID";
        

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
            //CalcTotal_Card(e.Row.Cells[5].Text);

            // ALIGNMENT
            //e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Left;

            //e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
            //e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Center;
            //e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Center;

            //e.Row.Cells[8].HorizontalAlign = HorizontalAlign.Center;
            

        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            //e.Row.Cells[2].Text = "Total";
            ////e.Row.Cells[4].Text = runningTotalQty.ToString("0,0", CultureInfo.InvariantCulture);
            //e.Row.Cells[3].Text = runningTotalTP.ToString("0,0", CultureInfo.InvariantCulture);
            //e.Row.Cells[4].Text = runningTotalCash.ToString("0,0", CultureInfo.InvariantCulture);
            //e.Row.Cells[5].Text = runningTotalCard.ToString("0,0", CultureInfo.InvariantCulture);


            ////ALIGNMENT
            //e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Left;

            //e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
            //e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Center;
            //e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Center;
            
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