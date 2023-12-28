using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Globalization;

public partial class FormsReport_Admin_frmTransferReport_Admin : System.Web.UI.Page
{
    SqlConnection conn = DBConnection.GetConnection();
    SqlConnection con;
    //SqlCommand cmd;
    SqlDataReader dr;
    private string s;
    //private double runningTotal = 0;
    private double runningTotalQty = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (System.Convert.ToInt32(Session["Vis"]) == 0)
            {
                Response.Redirect("../Account/Login.aspx");
            }

            ddlGroup.Items.Insert(0, new ListItem("---Select Group---", "---Select Group---"));
            FillDropDownList_Group();

            ddlModel.Items.Insert(0, new ListItem("---Select Model---", "---Select Model---"));
            FillDropDownList_Model();

            txtFrom.Text = DateTime.Today.ToString("MM/dd/yyyy");
            txtToDate.Text = DateTime.Today.ToString("MM/dd/yyyy");

        }
    }

    // Fill PRODUCT GROUP in Dropdownlist 
    public void FillDropDownList_Group()
    {
        //s = WebConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        //con = new SqlConnection(s);
        SqlConnection con = DBConnection.GetConnection();
        con.Open();
        //cmd = new SqlCommand("Select StudentName from Student", con);

        string sSql = "";

        sSql = "";
        sSql = "SELECT DISTINCT GroupName " +
            " FROM dbo.Product" +
            " ORDER BY GroupName";

        SqlCommand cmd = new SqlCommand(sSql, con);

        dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            ddlGroup.Items.Add(dr[0].ToString());
        }
        dr.Close();
        con.Close();
    }

    // Fill PRODUCT MODEL in Dropdownlist 
    public void FillDropDownList_Model()
    {
        //s = WebConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        //con = new SqlConnection(s);
        SqlConnection con = DBConnection.GetConnection();
        con.Open();
        //cmd = new SqlCommand("Select StudentName from Student", con);

        string sSql = "";

        sSql = "";
        sSql = "SELECT DISTINCT Model " +
            " FROM dbo.Product WHERE Discontinue='No'" +
            " ORDER BY Model";

        SqlCommand cmd = new SqlCommand(sSql, con);

        dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            ddlModel.Items.Add(dr[0].ToString());
        }
        dr.Close();
        con.Close();
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (RadioButton1.Checked == true)
        {
            lb1.Visible = true;
            lbl_id.Visible = true;

            GridView1.DataSource = null;
            GridView1.DataBind();
            GridView1.Visible = true;

            GridView2.DataSource = null;
            GridView2.DataBind();
            GridView2.Visible = true;

            GridView3.DataSource = null;
            GridView3.DataBind();
            GridView3.Visible = false;

            fnLoadReceiveSummary_ChallanWise();

        }
        else if (RadioButton2.Checked == true)
        {
            lb1.Visible = true;
            lbl_id.Visible = true;

            GridView1.DataSource = null;
            GridView1.DataBind();
            GridView1.Visible = true;

            GridView2.DataSource = null;
            GridView2.DataBind();
            GridView2.Visible = true;

            GridView3.DataSource = null;
            GridView3.DataBind();
            GridView3.Visible = false;

            fnLoadReceiveSummary_ChallanWise();

        }
        else if (RadioButton3.Checked == true)
        {
            lb1.Visible = false;
            lbl_id.Visible = false;

            GridView1.DataSource = null;
            GridView1.DataBind();
            GridView1.Visible = false;

            GridView2.DataSource = null;
            GridView2.DataBind();
            GridView2.Visible = false;

            GridView3.DataSource = null;
            GridView3.DataBind();
            GridView3.Visible = true;

            fnLoadReceiveSummary_GroupWise();
        }

        else if (RadioButton4.Checked == true)
        {
            lb1.Visible = false;
            lbl_id.Visible = false;

            GridView1.DataSource = null;
            GridView1.DataBind();
            GridView1.Visible = false;

            GridView2.DataSource = null;
            GridView2.DataBind();
            GridView2.Visible = true;

            GridView3.DataSource = null;
            GridView3.DataBind();
            GridView3.Visible = false;

            fnLoadReceiveSummary_ModelWise();

        }

        else if (RadioButton5.Checked == true)
        {
            lb1.Visible = false;
            lbl_id.Visible = false;

            GridView1.DataSource = null;
            GridView1.DataBind();
            GridView1.Visible = false;

            GridView2.DataSource = null;
            GridView2.DataBind();
            GridView2.Visible = true;

            GridView3.DataSource = null;
            GridView3.DataBind();
            GridView3.Visible = false;

            fnLoadReceiveSummary_SLWise();
        }
    }

    //LOAD RECEIVE PRODUCT GROUP SUMMARY
    private void fnLoadReceiveSummary_SLWise()
    {
        //s = WebConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        //con = new SqlConnection(s);
        SqlConnection con = DBConnection.GetConnection();
        con.Open();

        string sSql = "";

        sSql = "SELECT  dbo.MRSRMaster.MRSRCode AS [Challan #...], " +
            " CONVERT(varchar(12), dbo.MRSRMaster.TDate, 105) AS [Receive Date...], " +
            " SUM(dbo.MRSRDetails.Qty) AS [...Quantity], dbo.Product.Model as [Product Model..]" +
            " FROM dbo.Product INNER JOIN" +
            " dbo.MRSRDetails ON dbo.Product.ProductID = dbo.MRSRDetails.ProductID INNER JOIN" +
            " dbo.MRSRMaster ON dbo.MRSRDetails.MRSRMID = dbo.MRSRMaster.MRSRMID" +

            " WHERE " +
            " (dbo.MRSRMaster.TrType = 4) " +
            " AND (dbo.MRSRDetails.SLNO = '" + this.txtSL.Text + "') " +
            //" AND (dbo.MRSRMaster.OutSource='" + Session["sBrId"] + "')" +
            " AND (dbo.MRSRMaster.InSource='370')" +

            " GROUP BY dbo.MRSRMaster.MRSRCode, dbo.MRSRMaster.TDate,dbo.Product.Model" +

            " ORDER BY dbo.MRSRMaster.TDate";

        SqlCommand cmd = new SqlCommand(sSql, con);
        dr = cmd.ExecuteReader();
        GridView2.DataSource = dr;
        GridView2.DataBind();
        dr.Close();
        con.Close();



    }

    //LOAD RECEIVE PRODUCT GROUP SUMMARY
    private void fnLoadReceiveSummary_ModelWise()
    {
        //s = WebConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        //con = new SqlConnection(s);
        SqlConnection con = DBConnection.GetConnection();
        con.Open();

        string sSql = "";

        sSql = "SELECT  dbo.MRSRMaster.MRSRCode AS [Challan #...], " +
            " CONVERT(varchar(12), dbo.MRSRMaster.TDate, 105) AS [Receive Date...], " +
            " SUM(dbo.MRSRDetails.Qty) AS [Quantity]" +
            " FROM dbo.Product INNER JOIN" +
            " dbo.MRSRDetails ON dbo.Product.ProductID = dbo.MRSRDetails.ProductID INNER JOIN" +
            " dbo.MRSRMaster ON dbo.MRSRDetails.MRSRMID = dbo.MRSRMaster.MRSRMID" +

            " WHERE (dbo.MRSRMaster.TDate >= '" + Convert.ToDateTime(this.txtFrom.Text) + "'" +
            " AND dbo.MRSRMaster.TDate <= '" + Convert.ToDateTime(this.txtToDate.Text) + "')" +
            " AND (dbo.MRSRMaster.TrType = 4) " +
            " AND (dbo.Product.Model = '" + ddlModel.SelectedItem.ToString() + "') " +
            //" AND (dbo.MRSRMaster.OutSource='" + Session["sBrId"] + "')" +
            " AND (dbo.MRSRMaster.InSource='370')" +

            " GROUP BY dbo.MRSRMaster.MRSRCode, dbo.MRSRMaster.TDate" +

            " ORDER BY dbo.MRSRMaster.TDate";

        SqlCommand cmd = new SqlCommand(sSql, con);
        dr = cmd.ExecuteReader();
        GridView2.DataSource = dr;
        GridView2.DataBind();
        dr.Close();
        con.Close();
        
    }

    //LOAD RECEIVE PRODUCT GROUP SUMMARY
    private void fnLoadReceiveSummary_GroupWise()
    {
        //s = WebConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        //con = new SqlConnection(s);
        SqlConnection con = DBConnection.GetConnection();
        con.Open();

        string sSql = "";

        sSql = "SELECT  dbo.Product.Model, dbo.Product.ProdName AS ProdDesc,dbo.MRSRDetails.SLNO," +
            " SUM(dbo.MRSRDetails.Qty) AS Qty" +
            " FROM dbo.Product INNER JOIN" +
            " dbo.MRSRDetails ON dbo.Product.ProductID = dbo.MRSRDetails.ProductID" +
            " INNER JOIN dbo.MRSRMaster ON dbo.MRSRDetails.MRSRMID = dbo.MRSRMaster.MRSRMID" +

            " WHERE (dbo.MRSRMaster.TDate >= '" + Convert.ToDateTime(this.txtFrom.Text) + "'" +
            " AND dbo.MRSRMaster.TDate <= '" + Convert.ToDateTime(this.txtToDate.Text) + "')" +
            " AND (dbo.MRSRMaster.TrType = 4) " +
            " AND (dbo.Product.GroupName = '" + ddlGroup.SelectedItem.ToString() + "') " +
            //" AND (dbo.MRSRMaster.OutSource='" + Session["sBrId"] + "')" +
            " AND (dbo.MRSRMaster.InSource='370')" +

            " GROUP BY dbo.Product.Model, dbo.Product.ProdName,dbo.MRSRDetails.SLNO " +

            " ORDER BY dbo.Product.Model";

        SqlCommand cmd = new SqlCommand(sSql, con);
        dr = cmd.ExecuteReader();
        GridView3.DataSource = dr;
        GridView3.DataBind();
        dr.Close();
        con.Close();
        
    }

    //FOR VIEW DETAILS CHALLAN
    protected void GridView1_SelectedIndexChanged(Object sender, EventArgs e)
    {
        if (RadioButton1.Checked == true)
        {
            lbl_id.Text = GridView1.SelectedRow.Cells[1].Text;
            fnLoadSalesDetails();
        }
        else if (RadioButton2.Checked == true)
        {
            lbl_id.Text = GridView1.SelectedRow.Cells[1].Text;
            fnLoadSalesDetails();
        }

    }


    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        Control ctrl = new Control();
        ctrl = GridView1.FindControl("lnk_Select");
        if (ctrl != null)
        {
            e.Row.Attributes.Add("onClick", Page.ClientScript.GetPostBackClientHyperlink(ctrl, String.Empty));
        }
    }

    //Grid View 2 Footer Total
    protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CalcTotalQty(e.Row.Cells[3].Text);

            double value = Convert.ToDouble(e.Row.Cells[3].Text);
            e.Row.Cells[3].Text = value.ToString("#,#.##");

            //RIGHT ALIGNMENT
            e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Right;

        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[1].Text = "Total";
            //e.Row.Cells[10].Text = string.Format("{0:c}", runningTotal);
            e.Row.Cells[3].Text = runningTotalQty.ToString("0,0", CultureInfo.InvariantCulture);
            e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Right;
        }
    }


    //Grid View 3 Footer Total
    protected void GridView3_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CalcTotalQty1(e.Row.Cells[3].Text);

            double value = Convert.ToDouble(e.Row.Cells[3].Text);
            e.Row.Cells[3].Text = value.ToString("#,#.##");

            //RIGHT ALIGNMENT
            e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Right;

        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[1].Text = "Total";
            //e.Row.Cells[10].Text = string.Format("{0:c}", runningTotal);
            e.Row.Cells[3].Text = runningTotalQty.ToString("0,0", CultureInfo.InvariantCulture);
            e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Right;
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

    //CALCULATE TOTAL QTY1
    private void CalcTotalQty1(string _qty)
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

    public void fnLoadSalesDetails()
    {
        //s = WebConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        //con = new SqlConnection(s);
        SqlConnection con = DBConnection.GetConnection();
        con.Open();

        string sCHNo = Convert.ToString(this.lbl_id.Text);

        string sSql = "";
        sSql = "SELECT dbo.Product.Model, " +
            " dbo.MRSRDetails.SLNO as [Product SL#]," +
            " ABS(dbo.MRSRDetails.Qty) AS Qty, " +
            " dbo.MRSRDetails.ProdRemarks As [Product Remarks]" +
            " FROM dbo.Product INNER JOIN" +
            " dbo.MRSRDetails ON dbo.Product.ProductID = dbo.MRSRDetails.ProductID INNER JOIN" +
            " dbo.MRSRMaster ON dbo.MRSRDetails.MRSRMID = dbo.MRSRMaster.MRSRMID" +
           // " WHERE (dbo.MRSRMaster.TrType = 4)" +
            " WHERE (dbo.MRSRMaster.MRSRCode = '" + lbl_id.Text + "')" +
            " ORDER BY CAST(dbo.Product.ModelSerial AS INT)";

        SqlCommand cmd = new SqlCommand(sSql, con);
        dr = cmd.ExecuteReader();
        GridView2.DataSource = dr;
        GridView2.DataBind();
        dr.Close();
        con.Close();

    }

    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView2.PageIndex = e.NewPageIndex;
        fnLoadReceiveSummary_ChallanWise();
    }

    //LOAD RECEIVE CHALLAN WISE SUMMARY
    private void fnLoadReceiveSummary_ChallanWise()
    {
        //s = WebConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        //con = new SqlConnection(s);
        SqlConnection con = DBConnection.GetConnection();
        con.Open();
        //cmd = new SqlCommand("Select * from Student where StudentName='" + DropDownList1.SelectedItem.ToString() + "'", con);
        //tDate = Convert.ToDateTime(this.txtDate.Text);

        string sSql = "";
        sSql = "SELECT MRSRCode, CONVERT(varchar(12), TDate, 105) AS TDate," +
            " Remarks" +
            " FROM dbo.MRSRMaster" +
            " WHERE (dbo.MRSRMaster.TrType = 4) " +
            //" AND (dbo.MRSRMaster.OutSource='" + Session["sBrId"] + "')";
        " AND (dbo.MRSRMaster.InSource='370')";

        if (RadioButton1.Checked == true)
        {
            sSql = sSql + " AND (dbo.MRSRMaster.MRSRCode = '" + this.txtCHNo.Text + "')";
        }
        else
        {
            sSql = sSql + " AND (dbo.MRSRMaster.TDate >= '" + Convert.ToDateTime(this.txtFrom.Text) + "'";
            sSql = sSql + " AND dbo.MRSRMaster.TDate <= '" + Convert.ToDateTime(this.txtToDate.Text) + "')";
        }
        sSql = sSql + " ORDER BY TDate";

        SqlCommand cmd = new SqlCommand(sSql, con);
        dr = cmd.ExecuteReader();
        GridView1.DataSource = dr;
        GridView1.DataBind();
        dr.Close();
        con.Close();

    }

    protected void RadioButtonSales_CheckedChanged(object sender, EventArgs e)
    {
        if (RadioButton1.Checked == true)
        {
            this.txtCHNo.Visible = true;
            this.ddlGroup.Visible = false;
            this.ddlModel.Visible = false;
            this.txtSL.Visible = false;
            txtCHNo.Focus();
        }
        else if (RadioButton2.Checked == true)
        {
            this.txtCHNo.Visible = false;
            this.ddlGroup.Visible = false;
            this.ddlModel.Visible = false;
            this.txtSL.Visible = false;
        }
        else if (RadioButton3.Checked == true)
        {
            this.ddlGroup.Visible = true;
            this.txtCHNo.Visible = false;
            this.ddlModel.Visible = false;
            this.txtSL.Visible = false;
        }
        else if (RadioButton4.Checked == true)
        {
            this.ddlModel.Visible = true;
            this.txtCHNo.Visible = false;
            this.ddlGroup.Visible = false;
            this.txtSL.Visible = false;
        }
        else if (RadioButton5.Checked == true)
        {
            this.txtSL.Visible = true;
            this.txtCHNo.Visible = false;
            this.ddlGroup.Visible = false;
            this.ddlModel.Visible = false;
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        GridView1.DataSource = null;
        GridView1.DataBind();
        GridView1.Visible = false;

        GridView2.DataSource = null;
        GridView2.DataBind();
        GridView2.Visible = false;

        GridView3.DataSource = null;
        GridView3.DataBind();
        GridView3.Visible = false;

        lb1.Visible = false;
        lbl_id.Text = "";

        RadioButton1.Checked = false;
        RadioButton2.Checked = false;
        RadioButton3.Checked = false;
        RadioButton4.Checked = false;
        RadioButton5.Checked = false;
    }


}