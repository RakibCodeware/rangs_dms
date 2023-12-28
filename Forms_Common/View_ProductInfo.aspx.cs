using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Globalization;
using System.Drawing;
using System.IO;
using System.Data;
using System.Configuration;

public partial class Forms_Common_View_ProductInfo : System.Web.UI.Page
{
    SqlConnection conn = DBConnection.GetConnection();
    SqlConnection con;
    SqlCommand cmd;
    SqlDataReader dr;
    private string s;
    private double runningTotal = 0;
    private double runningTotal1 = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (System.Convert.ToInt32(Session["Vis"]) == 0)
            {
                Response.Redirect("../Account/Login.aspx");
            }

            DropDownList1.Items.Insert(0, new ListItem("---Select---", "---Select---"));
            FillDropDownList();
        }
        
    }

    // Fill Dropdownlist
    public void FillDropDownList()
    {
        //s = WebConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        //con = new SqlConnection(s);
        //con.Open();

        SqlConnection con = DBConnection.GetConnection();

        //cmd = new SqlCommand("Select StudentName from Student", con);

        string sSql = "";

        sSql = "";
        sSql = "SELECT DISTINCT GroupName " +
            " FROM dbo.Product" +                     
            " ORDER BY GroupName";

        con.Open();  
        SqlCommand cmd = new SqlCommand(sSql, con);

        dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            DropDownList1.Items.Add(dr[0].ToString());
        }
        dr.Close();
        con.Close();
    }


    // Show data in GridView
    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        fnLoadData();        
    }

    protected void fnLoadData()
    {
        //s = WebConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        //con = new SqlConnection(s);
        SqlConnection con = DBConnection.GetConnection();
        con.Open();

        string sSql = "";
        //sSql = "SELECT Code,Model,ProdName FROM Product" +
        //" WHERE (GroupName = '" + DropDownList1.SelectedItem.ToString() + "')" +            
        //" ORDER BY dbo.Product.ModelSerial";

        sSql = "SELECT a.Code, a.Model, a.ProdName, a.UnitPrice, " +
            " a.UnitPrice - ISNULL(b.DisAmnt, 0) AS CampPrice, a.GroupName " +
            " FROM (SELECT Code, Model, ProdName, UnitPrice, ProductID, " +
            " Discontinue, ModelSerial, GroupName, GroupSL " +
            " FROM dbo. Product" +
            " WHERE (Discontinue = 'No')) a LEFT OUTER JOIN" +
            " (SELECT dbo.tbCampaignDetails.ProductID, dbo.tbCampaignDetails.DisAmnt" +
            " FROM dbo.tbCampaignMaster INNER JOIN dbo.tbCampaignDetails ON" +
            " dbo.tbCampaignMaster.CampaignNo = dbo.tbCampaignDetails.CampaignNo" +
            " WHERE (dbo.tbCampaignMaster.cStop = 0)) b ON a.ProductID = b.ProductID" +
            " WHERE (a.GroupName = '" + DropDownList1.SelectedItem.ToString() + "')" +
            " ORDER BY a.GroupSL, CAST(a.ModelSerial AS INT)";

        SqlCommand cmd = new SqlCommand(sSql, con);
        dr = cmd.ExecuteReader();

        gvCustomres.DataSource = dr;
        gvCustomres.DataBind();
        dr.Close();
        con.Close();
    }

    //Grid View Row Format
    protected void gvCustomres_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CalcTotal(e.Row.Cells[3].Text);
            CalcTotal1(e.Row.Cells[4].Text);

            double value3 = Convert.ToDouble(e.Row.Cells[3].Text);
            e.Row.Cells[3].Text = value3.ToString("0,0");

            double value4 = Convert.ToDouble(e.Row.Cells[4].Text);
            e.Row.Cells[4].Text = value4.ToString("0,0");
                        
            //RIGHT ALIGNMENT            
            e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Right;
            
        }
        
    }

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

    private void CalcTotal1(string _price)
    {
        try
        {
            runningTotal1 += Double.Parse(_price);
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write(ex.ToString());
        }
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        /*
        try
        {
            this.gvCustomres.AllowPaging = false;
            this.gvCustomres.AllowSorting = false;
            this.gvCustomres.EditIndex = -1;

            // Let's bind data to GridView
            this.fnLoadData();

            // Let's output HTML of GridView
            Response.Clear();
            Response.ContentType = "application/vnd.xls";
            Response.AddHeader("content-disposition",
                    "attachment;filename=MyList.xls");
            Response.Charset = "";
            StringWriter swriter = new StringWriter();
            HtmlTextWriter hwriter = new HtmlTextWriter(swriter);
            gvCustomres.RenderControl(hwriter);
            Response.Write(swriter.ToString());
            Response.End();
        }
        catch (Exception exe)
        {
            throw exe;
        }
        */
    }

    /*
     OnRowDataBound="gvCustomres_RowDataBound"  
    onpageindexchanging="gvCustomres_pageIndexChanged" 
    protected void gvCustomres_pageIndexChanged(object sender, GridViewPageEventArgs e)
    {
        gvCustomres.PageIndex = e.NewPageIndex;
        gvCustomres.DataBind();
        //BindDataGrid()                           
    }
    */

    protected void gvCustomres_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes.Add("onmouseover", "this.className='highlight'");
            e.Row.Attributes.Add("onmouseout", "this.className='normal'");
        }
    }

}