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

using System.Configuration;

public partial class Search_SlowItems1 : System.Web.UI.Page
{
    SqlConnection conn = DBConnection.GetConnection();
    long i;
    //string s = ConfigurationManager.ConnectionStrings["dbcs"].ConnectionString; //string of connection
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;

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
            //this.txtFrom.Text = DateTime.Today.ToString("MM/dd/yyyy");
            //this.txtToDate.Text = DateTime.Today.ToString("MM/dd/yyyy");

            //LOAD CATEGORY
            LoadDropDownList_Category();

            //this.BindGrid();

        }

    }


    //LOAD CTP IN DROPDOWN LIST
    protected void LoadDropDownList_Category()
    {
        SqlConnection conn = DBConnection.GetConnection();

        String strQuery = "Select GroupName from Product ";
        strQuery = strQuery + " WHERE (Discontinue = 'No')";
        strQuery = strQuery + " GROUP BY GroupName";
        strQuery = strQuery + " ORDER BY GroupName";
        //SqlConnection con = new SqlConnection("conn");
        SqlCommand cmd = new SqlCommand(strQuery, conn);
        //SqlCommand cmd = new SqlCommand();
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = strQuery;
        cmd.Connection = conn;
        try
        {
            conn.Open();
            ddlCategory.DataSource = cmd.ExecuteReader();
            ddlCategory.DataTextField = "GroupName";
            ddlCategory.DataValueField = "GroupName";
            ddlCategory.DataBind();

            //Add blank item at index 0.
            ddlCategory.Items.Insert(0, new ListItem("ALL", "0"));

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


    
    protected void PopupMessage(string Msg, Control controlID)
    {
        ScriptManager.RegisterClientScriptBlock(controlID, controlID.GetType(), "msg", "alert('" + Msg + "');", true);
    }

    protected void SearchData(object sender, EventArgs e)
    {
        //LOAD DATA IN GRID
        fnLoadData_SlowItems();
    }


    protected void fnLoadData_SlowItems()
    {

        SqlConnection con = DBConnection.GetConnection();
        con.Open();

        int iDay = Convert.ToInt16(txtDay.Text);
        DateTime tDate = DateTime.Today.AddDays(-iDay);
        //DateTime tDate1 = DateTime.Today.AddDays(iDay);

        //lblDateCTPWise.Text = tDate.ToString("dd-MMM-yyyy");
        //lblSlowItemCaption.Text = txtDay.Text;

        string sSql = "";

        sSql = "";
        sSql = "SELECT dbo.MRSRMaster.TrType, convert(varchar,MAX(dbo.MRSRMaster.TDate),5) AS maxDate, dbo.Product.Model,";
        sSql = sSql + " dbo.Product.ProductID, dbo.Product.Discontinue, dbo.Product.GroupName, ";
        sSql = sSql + " dbo.VW_Model_Stock.bQty";
        sSql = sSql + " FROM dbo.MRSRMaster INNER JOIN";
        sSql = sSql + " dbo.MRSRDetails ON dbo.MRSRMaster.MRSRMID = dbo.MRSRDetails.MRSRMID INNER JOIN";
        sSql = sSql + " dbo.Product ON dbo.MRSRDetails.ProductID = dbo.Product.ProductID INNER JOIN";
        sSql = sSql + " dbo.VW_Model_Stock ON dbo.Product.ProductID = dbo.VW_Model_Stock.ProductID";
        sSql = sSql + " GROUP BY dbo.MRSRMaster.TrType, dbo.Product.Model, dbo.Product.ProductID, ";
        sSql = sSql + " dbo.Product.Discontinue, dbo.Product.GroupName, dbo.VW_Model_Stock.bQty";
        sSql = sSql + " HAVING (dbo.MRSRMaster.TrType = 3) AND (dbo.Product.Discontinue = N'No')";
        sSql = sSql + " AND (dbo.VW_Model_Stock.bQty > 0)";

        if (txtDay.Text.Length > 0)
        {
            //sSql = sSql + " AND (MAX(dbo.MRSRMaster.TDate) < CONVERT(DATETIME, '2018-11-30 00:00:00', 102))";
            sSql = sSql + " AND (MAX(dbo.MRSRMaster.TDate)<'" + tDate + "')";
        }

        if (this.ddlCategory.SelectedItem.Text != "ALL")
        {
            sSql = sSql + " AND (dbo.Product.GroupName='" + this.ddlCategory.SelectedItem.Text + "')";
        }

        sSql = sSql + " ORDER BY dbo.Product.GroupName, dbo.Product.Model";

        SqlCommand cmd = new SqlCommand(sSql, con);
        SqlDataReader dr = cmd.ExecuteReader();

        GridView1.DataSource = dr;
        GridView1.DataBind();
        dr.Close();
        con.Close();

    }


    private void BindGrid(string sortExpression = null)
    {
        //SqlConnection conn = DBConnection.GetConnection();

        //string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
        using (SqlConnection con = new SqlConnection(constr))
        {

            int iDay = Convert.ToInt16(txtDay.Text);
            DateTime tDate = DateTime.Today.AddDays(-iDay);

            string sSql = "";
            sSql = "";
            sSql = "SELECT dbo.MRSRMaster.TrType, MAX(dbo.MRSRMaster.TDate) AS maxDate, dbo.Product.Model,";
            sSql = sSql + " dbo.Product.ProductID, dbo.Product.Discontinue, dbo.Product.GroupName, ";
            sSql = sSql + " dbo.VW_Model_Stock.bQty";
            sSql = sSql + " FROM dbo.MRSRMaster INNER JOIN";
            sSql = sSql + " dbo.MRSRDetails ON dbo.MRSRMaster.MRSRMID = dbo.MRSRDetails.MRSRMID INNER JOIN";
            sSql = sSql + " dbo.Product ON dbo.MRSRDetails.ProductID = dbo.Product.ProductID INNER JOIN";
            sSql = sSql + " dbo.VW_Model_Stock ON dbo.Product.ProductID = dbo.VW_Model_Stock.ProductID";
            sSql = sSql + " GROUP BY dbo.MRSRMaster.TrType, dbo.Product.Model, dbo.Product.ProductID, ";
            sSql = sSql + " dbo.Product.Discontinue, dbo.Product.GroupName, dbo.VW_Model_Stock.bQty";
            sSql = sSql + " HAVING (dbo.MRSRMaster.TrType = 3) AND (dbo.Product.Discontinue = N'No')";
            sSql = sSql + " AND (dbo.VW_Model_Stock.bQty > 0)";

            if (txtDay.Text.Length > 0)
            {
                //sSql = sSql + " AND (MAX(dbo.MRSRMaster.TDate) < CONVERT(DATETIME, '2018-11-30 00:00:00', 102))";
                sSql = sSql + " AND (MAX(dbo.MRSRMaster.TDate)<'" + tDate + "')";
            }

            if (this.ddlCategory.SelectedItem.Text != "ALL")
            {
                sSql = sSql + " AND (dbo.Product.GroupName='" + this.ddlCategory.SelectedItem.Text + "')";
            }

            sSql = sSql + " ORDER BY dbo.Product.GroupName, dbo.Product.Model";

            //using (SqlCommand cmd = new SqlCommand("SELECT CustomerId, ContactName, City, Country FROM Customers"))
            using (SqlCommand cmd = new SqlCommand(sSql))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        if (sortExpression != null)
                        {
                            DataView dv = dt.AsDataView();
                            this.SortDirection = this.SortDirection == "ASC" ? "DESC" : "ASC";

                            dv.Sort = sortExpression + " " + this.SortDirection;
                            GridView1.DataSource = dv;
                        }
                        else
                        {
                            GridView1.DataSource = dt;
                        }
                        GridView1.DataBind();
                    }
                }
            }
        }
    }

    private string SortDirection
    {
        get { return ViewState["SortDirection"] != null ? ViewState["SortDirection"].ToString() : "ASC"; }
        set { ViewState["SortDirection"] = value; }
    }

    protected void OnSorting(object sender, GridViewSortEventArgs e)
    {
        this.BindGrid(e.SortExpression);
    }


}