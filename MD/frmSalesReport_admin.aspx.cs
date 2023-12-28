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


public partial class FormsReport_Admin_frmSalesReport_Admin : System.Web.UI.Page
{

    SqlConnection conn = DBConnection.GetConnection();
    SqlConnection con;
    //SqlCommand cmd;
    SqlDataReader dr;
    private string s;
    //private double runningTotal = 0;

    private double runningTotalSum = 0;
    private double runningTotal = 0;
    private double runningTotalQty = 0;
    private double runningTotalDis = 0;
    private double runningTotalWith = 0;

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

            //ddlEntity.Items.Insert(0, new ListItem("ALL", "ALL"));
            //FillDropDownList_Entity();

            LoadDropDownList_CTP();

            txtFrom.Text = DateTime.Today.ToString("MM/dd/yyyy");
            txtToDate.Text = DateTime.Today.ToString("MM/dd/yyyy");

        }
    }

    // Fill Entity in Dropdownlist 
    public void FillDropDownList_Entity()
    {
        //s = WebConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        //con = new SqlConnection(s);
        SqlConnection con = DBConnection.GetConnection();
        con.Open();
        //cmd = new SqlCommand("Select StudentName from Student", con);

        string sSql = "";

        sSql = "";
        sSql = "SELECT eName, EntityType, ActiveDeactive" +
            " FROM  dbo.Entity" +
            //WHERE EID=" + Session["sBrID"] + "" +
            " WHERE (ActiveDeactive = 1) AND" +
            " (EntityType = 'Showroom' OR EntityType = 'Dealer')" +
            " ORDER BY eName";

        SqlCommand cmd = new SqlCommand(sSql, con);

        dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            ddlEntity.Items.Add(dr[0].ToString());
        }
        dr.Close();
        con.Close();

        //Add blank item at index 0.
        ddlEntity.Items.Insert(0, new ListItem("ALL", "0"));

    }

    //LOAD CTP IN DROPDOWN LIST
    protected void LoadDropDownList_CTP()
    {
        SqlConnection conn = DBConnection.GetConnection();

        String strQuery = "select EID,eName from Entity ";
        strQuery = strQuery + " WHERE (ActiveDeactive = 1) AND";
        strQuery = strQuery + " (EntityType = 'showroom' OR EntityType = 'Dealer')";
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
            ddlEntity.Items.Insert(0, new ListItem("ALL", "0"));

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
            " FROM dbo.Product" +
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

    protected void PopupMessage(string Msg, Control controlID)
    {
        ScriptManager.RegisterClientScriptBlock(controlID, controlID.GetType(), "msg", "alert('" + Msg + "');", true);
    }
    
    //FOR SEARCH DATA
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (RadioButton1.Checked == true)
        {
            ddlGroup.Visible = false;
            ddlModel.Visible = false;

            GridView1.DataSource = null;
            GridView1.DataBind();
            GridView1.Visible = true;

            GridView2.DataSource = null;
            GridView2.DataBind();
            GridView2.Visible = true;

            GridView3.DataSource = null;
            GridView3.DataBind();
            GridView3.Visible = false;

            btnExport.Visible = false;

            if (txtCHNo.Text == "")
            {
                PopupMessage("Please select Challan #.", btnAdd);
                txtCHNo.Focus();
                return;
            }
            fnLoadSalesSummary();
        }
        else if (RadioButton2.Checked == true)
        {
            if (txtFrom.Text == "")
            {
                PopupMessage("Please enter From Date.", btnAdd);
                txtFrom.Focus();
                return;
            }
            if (txtToDate.Text == "")
            {
                PopupMessage("Please enter From Date.", btnAdd);
                txtToDate.Focus();
                return;
            }

            ddlGroup.Visible = false;
            ddlModel.Visible = false;

            GridView1.DataSource = null;
            GridView1.DataBind();
            GridView1.Visible = true;

            GridView2.DataSource = null;
            GridView2.DataBind();
            GridView2.Visible = true;

            GridView3.DataSource = null;
            GridView3.DataBind();
            GridView3.Visible = false;

            btnExport.Visible = false;

            fnLoadSalesSummary();

        }
        else if (RadioButton6.Checked == true)
        {
            if (txtFrom.Text == "")
            {
                PopupMessage("Please enter From Date.", btnAdd);
                txtFrom.Focus();
                return;
            }
            if (txtToDate.Text == "")
            {
                PopupMessage("Please enter From Date.", btnAdd);
                txtToDate.Focus();
                return;
            }

            GridView1.DataSource = null;
            GridView1.DataBind();
            GridView1.Visible = false;

            GridView2.DataSource = null;
            GridView2.DataBind();
            GridView2.Visible = true;

            GridView3.DataSource = null;
            GridView3.DataBind();
            GridView3.Visible = false;

            lb1.Visible = false;
            btnExport.Visible = true;

            lbl_id.Text = "( Sales From " + txtFrom.Text + " To " + txtToDate.Text + " )";

            fnLoadSalesSummary_ProdWise();
        }

        else if (RadioButton7.Checked == true)
        {
            if (txtFrom.Text == "")
            {
                PopupMessage("Please enter From Date.", btnAdd);
                txtFrom.Focus();
                return;
            }
            if (txtToDate.Text == "")
            {
                PopupMessage("Please enter From Date.", btnAdd);
                txtToDate.Focus();
                return;
            }
            
            GridView1.DataSource = null;
            GridView1.DataBind();
            GridView1.Visible = false;

            GridView2.DataSource = null;
            GridView2.DataBind();
            GridView2.Visible = false;

            GridView3.DataSource = null;
            GridView3.DataBind();
            GridView3.Visible = true;

            lb1.Visible = false;
            btnExport.Visible = false;

            lbl_id.Text = "( Sales From " + txtFrom.Text + " To " + txtToDate.Text + " )";

            fnLoadSalesSummary_CategoryWise();
        }

        else if (RadioButton3.Checked == true)
        {
            if (txtFrom.Text == "")
            {
                PopupMessage("Please enter From Date.", btnAdd);
                txtFrom.Focus();
                return;
            }
            if (txtToDate.Text == "")
            {
                PopupMessage("Please enter From Date.", btnAdd);
                txtToDate.Focus();
                return;
            }
                        
            GridView1.DataSource = null;
            GridView1.DataBind();
            GridView1.Visible = false;

            GridView2.DataSource = null;
            GridView2.DataBind();
            GridView2.Visible = false;

            GridView3.DataSource = null;
            GridView3.DataBind();
            GridView3.Visible = true;

            lb1.Visible = false;
            btnExport.Visible = false;

            lbl_id.Text = "( Sales From " + txtFrom.Text + " To " + txtToDate.Text + " )";

            fnLoadSalesSummary_GroupWise();
        }

        else if (RadioButton4.Checked == true)
        {
            if (txtFrom.Text == "")
            {
                PopupMessage("Please enter From Date.", btnAdd);
                txtFrom.Focus();
                return;
            }
            if (txtToDate.Text == "")
            {
                PopupMessage("Please enter From Date.", btnAdd);
                txtToDate.Focus();
                return;
            }

            GridView1.DataSource = null;
            GridView1.DataBind();
            GridView1.Visible = false;

            GridView2.DataSource = null;
            GridView2.DataBind();
            GridView2.Visible = false;

            GridView3.DataSource = null;
            GridView3.DataBind();
            GridView3.Visible = true;

            lb1.Visible = false;
            btnExport.Visible = false;

            lbl_id.Text = "( Sales From " + txtFrom.Text + " To " + txtToDate.Text + " )";

            fnLoadSalesSummary_ModelWise();
        }

    }

    //LOAD SALES SUMMARY Group Wise
    private void fnLoadSalesSummary_ModelWise()
    {
        //s = WebConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        //con = new SqlConnection(s);
        SqlConnection con = DBConnection.GetConnection();
        con.Open();

        string sSql = "";
        sSql = "SELECT dbo.MRSRMaster.MRSRCode AS [Challan #]," +
            " CONVERT(varchar(12), dbo.MRSRMaster.TDate, 105) AS TDate," +
            " dbo.Product.Model, " +
            " SUM(ABS(dbo.MRSRDetails.Qty)) AS Qty," +
            " SUM(dbo.MRSRDetails.NetAmnt) AS [Sales Value]" +
            " FROM  dbo.MRSRMaster INNER JOIN" +
            " dbo.MRSRDetails ON dbo.MRSRMaster.MRSRMID = dbo.MRSRDetails.MRSRMID INNER JOIN" +
            " dbo.Product ON dbo.MRSRDetails.ProductID = dbo.Product.ProductID" +

            " WHERE (dbo.MRSRMaster.TrType = 3)" +
            //" AND (dbo.MRSRMaster.OutSource='" + Session["sBrId"] + "')" +
            " AND (dbo.MRSRMaster.TDate >= '" + Convert.ToDateTime(this.txtFrom.Text) + "'" +
            " AND dbo.MRSRMaster.TDate <= '" + Convert.ToDateTime(this.txtToDate.Text) + "')" +
            " AND (dbo.Product.Model='" + ddlModel.SelectedItem.ToString() + "')";
            
            if (ddlEntity.SelectedItem.ToString() != "ALL")
            {
                sSql = sSql + " AND (dbo.MRSRMaster.OutSource='" + txtEID.Text + "')";
            }

            sSql = sSql + "" +

            " GROUP BY dbo.MRSRMaster.MRSRCode, dbo.MRSRMaster.TDate, dbo.Product.Model" +
            " ORDER BY dbo.MRSRMaster.TDate, dbo.MRSRMaster.MRSRCode";

        SqlCommand cmd = new SqlCommand(sSql, con);
        dr = cmd.ExecuteReader();
        GridView3.DataSource = dr;
        GridView3.DataBind();
        dr.Close();
        con.Close();

    }

    //LOAD SALES SUMMARY Group Wise
    private void fnLoadSalesSummary_GroupWise()
    {
        //s = WebConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        //con = new SqlConnection(s);
        SqlConnection con = DBConnection.GetConnection();
        con.Open();

        string sSql = "";
        sSql = "SELECT dbo.Product.Model as [Product Model], dbo.Product.ProdName as [Product Description]," +
            " SUM(ABS(dbo.MRSRDetails.Qty)) AS [Sales Qty]," +
            " SUM(dbo.MRSRDetails.NetAmnt) AS [Sales  Value]" +
            " FROM  dbo.MRSRMaster INNER JOIN" +
            " dbo.MRSRDetails ON dbo.MRSRMaster.MRSRMID = dbo.MRSRDetails.MRSRMID INNER JOIN" +
            " dbo.Product ON dbo.MRSRDetails.ProductID = dbo.Product.ProductID" +

            " WHERE (dbo.MRSRMaster.TrType = 3)" +
            //" AND (dbo.MRSRMaster.OutSource='" + Session["sBrId"] + "')" +
            " AND (dbo.MRSRMaster.TDate >= '" + Convert.ToDateTime(this.txtFrom.Text) + "'" +
            " AND dbo.MRSRMaster.TDate <= '" + Convert.ToDateTime(this.txtToDate.Text) + "')" +
            " AND (dbo.Product.GroupName='" + ddlGroup.SelectedItem.ToString() + "')";

            if (ddlEntity.SelectedItem.ToString() != "ALL")
            {
                sSql = sSql + " AND (dbo.MRSRMaster.OutSource='" + txtEID.Text + "')";
            }
            sSql = sSql + "" +

            " GROUP BY dbo.Product.Model, dbo.Product.ProdName" +
            " ORDER BY dbo.Product.Model";

        SqlCommand cmd = new SqlCommand(sSql, con);
        dr = cmd.ExecuteReader();
        GridView3.DataSource = dr;
        GridView3.DataBind();
        dr.Close();
        con.Close();

    }

    //LOAD SALES SUMMARY Category Wise
    private void fnLoadSalesSummary_CategoryWise()
    {
        //s = WebConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        //con = new SqlConnection(s);
        SqlConnection con = DBConnection.GetConnection();
        con.Open();

        string sSql = "";
        sSql = "SELECT dbo.Product.GroupSL, dbo.Product.GroupName AS [Product Category Name]," +
            " SUM(ABS(dbo.MRSRDetails.Qty)) AS [Sales Qty]," +
            " SUM(dbo.MRSRDetails.NetAmnt) AS [Sales  Value]" +
            " FROM  dbo.MRSRMaster INNER JOIN" +
            " dbo.MRSRDetails ON dbo.MRSRMaster.MRSRMID = dbo.MRSRDetails.MRSRMID INNER JOIN" +
            " dbo.Product ON dbo.MRSRDetails.ProductID = dbo.Product.ProductID" +

            " WHERE (dbo.MRSRMaster.TrType = 3)" +
            //" AND (dbo.MRSRMaster.OutSource='" + Session["sBrId"] + "')" +
            " AND (dbo.MRSRMaster.TDate >= '" + Convert.ToDateTime(this.txtFrom.Text) + "'" +
            " AND dbo.MRSRMaster.TDate <= '" + Convert.ToDateTime(this.txtToDate.Text) + "')";

            if (ddlEntity.SelectedItem.ToString() != "ALL")
            {
                sSql = sSql + " AND (dbo.MRSRMaster.OutSource='" + txtEID.Text + "')";
            }
            sSql = sSql + "" +
            " GROUP BY dbo.Product.GroupSL, dbo.Product.GroupName" +
            " ORDER BY dbo.Product.GroupSL";

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
            
            lb1.Visible = true;
            fnLoadSalesDetails();

            btnExport.Visible = true;
            
        }
        if (RadioButton2.Checked == true)
        {
            //GridView1.Visible = true;
            //GridView2.Visible = true;

            lbl_id.Text = GridView1.SelectedRow.Cells[1].Text;
            lb1.Visible = true;
            fnLoadSalesDetails();

            btnExport.Visible = true;
        }


    }

    //Grid View 1 Footer Total
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        Control ctrl = new Control();
        ctrl = GridView1.FindControl("lnk_Select");
                
        if (ctrl != null)
        {
            e.Row.Attributes.Add("onClick", Page.ClientScript.GetPostBackClientHyperlink(ctrl, String.Empty));
                     
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CalcTotalSum(e.Row.Cells[3].Text);

            double value = Convert.ToDouble(e.Row.Cells[3].Text);
            e.Row.Cells[3].Text = value.ToString("#,#.##");          
            e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Right;
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[2].Text = "Total";
            e.Row.Cells[3].Text = runningTotalSum.ToString("0,0", CultureInfo.InvariantCulture);
            e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Right;
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

    //Grid View 2 Footer Total
    protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
    {        
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CalcTotalQty(e.Row.Cells[2].Text);
            CalcTotalDis(e.Row.Cells[4].Text);
            CalcTotalWith(e.Row.Cells[6].Text);
            CalcTotal(e.Row.Cells[7].Text);
            
            double value = Convert.ToDouble(e.Row.Cells[2].Text);
            e.Row.Cells[2].Text = value.ToString("#,#.##");

            double value1 = Convert.ToDouble(e.Row.Cells[3].Text);
            e.Row.Cells[3].Text = value1.ToString("#,#.##");

            double value2 = Convert.ToDouble(e.Row.Cells[4].Text);
            e.Row.Cells[4].Text = value2.ToString("#,#.##");

            double value3 = Convert.ToDouble(e.Row.Cells[6].Text);
            e.Row.Cells[6].Text = value3.ToString("#,#.##");

            double value4 = Convert.ToDouble(e.Row.Cells[7].Text);
            e.Row.Cells[7].Text = value4.ToString("#,#.##");

            //RIGHT ALIGNMENT
            e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Left;
            e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[7].HorizontalAlign = HorizontalAlign.Right;

        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[0].Text = "Total";
            //e.Row.Cells[10].Text = string.Format("{0:c}", runningTotal);
            e.Row.Cells[2].Text = runningTotalQty.ToString("0,0", CultureInfo.InvariantCulture);
            e.Row.Cells[4].Text = runningTotalDis.ToString("0,0", CultureInfo.InvariantCulture);
            e.Row.Cells[6].Text = runningTotalWith.ToString("0,0", CultureInfo.InvariantCulture);
            e.Row.Cells[7].Text = runningTotal.ToString("0,0", CultureInfo.InvariantCulture);
            
            e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[7].HorizontalAlign = HorizontalAlign.Right;
        }
    }

    //CALCULATE NET Summary Total
    private void CalcTotalSum(string _price)
    {
        try
        {
            runningTotalSum += Double.Parse(_price);
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write(ex.ToString());
        }
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

    //CALCULATE TOTAL DISCOUNT
    private void CalcTotalDis(string _price)
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

    //CALCULATE TOTAL WITH/ADJ
    private void CalcTotalWith(string _price)
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
    
    //SALES DETAILS
    public void fnLoadSalesDetails()
    {
        //s = WebConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        //con = new SqlConnection(s);
        SqlConnection con = DBConnection.GetConnection();
        con.Open();

        string sCHNo = Convert.ToString(this.lbl_id.Text);

        string sSql = "";
        sSql = "SELECT dbo.Product.Model, dbo.MRSRDetails.UnitPrice AS [Unit Price], " +
            " ABS(dbo.MRSRDetails.Qty) AS Qty, dbo.MRSRDetails.TotalAmnt as [Total Price]," +
            " dbo.MRSRDetails.DiscountAmnt AS Discount," +
            " dbo.MRSRDetails.DisRef, dbo.MRSRDetails.WithAdjAmnt as WithAdj, dbo.MRSRDetails.NetAmnt as [Net Amount]," +
            " dbo.MRSRDetails.SLNO as [Product SL#], dbo.MRSRDetails.ProdRemarks As [Product Remarks]" +
            " FROM dbo.Product INNER JOIN" +
            " dbo.MRSRDetails ON dbo.Product.ProductID = dbo.MRSRDetails.ProductID INNER JOIN" +
            " dbo.MRSRMaster ON dbo.MRSRDetails.MRSRMID = dbo.MRSRMaster.MRSRMID" +
            " WHERE (dbo.MRSRMaster.TrType = 3)" +
            //" AND (dbo.MRSRMaster.OutSource='" + Session["sBrId"] + "')" +
            " AND (dbo.MRSRMaster.MRSRCode = '" + lbl_id.Text + "')" +
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
        fnLoadSalesSummary();
    }


    //LOAD SALES SUMMARY CHALLAN WISE
    private void fnLoadSalesSummary()
    {
        //s = WebConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        //con = new SqlConnection(s);
        SqlConnection con = DBConnection.GetConnection();
        con.Open();
                        
        string sSql = "";
        sSql = "SELECT MRSRCode, CONVERT(varchar(12), TDate, 105) AS TDate," +
            " NetSalesAmnt,  dbo.Entity.eName as Entity" +
            " FROM dbo.Entity INNER JOIN dbo.MRSRMaster" +
            " ON dbo.Entity.EID = dbo.MRSRMaster.OutSource" +
            " WHERE (dbo.MRSRMaster.TrType = 3) ";
            //" AND (dbo.MRSRMaster.OutSource='" + Session["sBrId"] + "')";
            
            if (ddlEntity.SelectedItem.ToString() != "ALL")
            {
                sSql = sSql + " AND (dbo.MRSRMaster.OutSource='" + txtEID.Text + "')";
            }
            
            if (RadioButton1.Checked == true)
            {
                sSql = sSql + " AND (dbo.MRSRMaster.MRSRCode='" + txtCHNo.Text + "')" +                    
                    " ORDER BY TDate";            
            }
            else if (RadioButton2.Checked == true)
            {                
                sSql = sSql + ""+
                    " AND (dbo.MRSRMaster.TDate >= '" + Convert.ToDateTime(this.txtFrom.Text) + "'" +
                    " AND dbo.MRSRMaster.TDate <= '" + Convert.ToDateTime(this.txtToDate.Text) + "')" +
                    " ORDER BY TDate";
            }

        SqlCommand cmd = new SqlCommand(sSql, con);
        dr = cmd.ExecuteReader();
        GridView1.DataSource = dr;
        GridView1.DataBind();
        dr.Close();
        con.Close();

    }


    //LOAD SALES SUMMARY Product Wise
    private void fnLoadSalesSummary_ProdWise()
    {
        //s = WebConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        //con = new SqlConnection(s);
        SqlConnection con = DBConnection.GetConnection();
        con.Open();

        string sSql = "";                
        sSql="SELECT dbo.Product.Model,  dbo.MRSRDetails.UnitPrice," +
            " SUM(ABS(dbo.MRSRDetails.Qty)) AS Qty, SUM(dbo.MRSRDetails.TotalAmnt) as [Total Price]," +
            " SUM(dbo.MRSRDetails.DiscountAmnt) AS [Dis Amnt],dbo.MRSRDetails.DisRef," +
            " SUM(dbo.MRSRDetails.WithAdjAmnt) AS [With/Adj Amnt]," +
            " SUM(dbo.MRSRDetails.NetAmnt) AS [Net Amnt]" +
            " FROM dbo.MRSRMaster INNER JOIN" +
            " dbo.MRSRDetails ON dbo.MRSRMaster.MRSRMID = dbo.MRSRDetails.MRSRMID INNER JOIN" +
            " dbo.Product ON dbo.MRSRDetails.ProductID = dbo.Product.ProductID" +
            
            " WHERE (dbo.MRSRMaster.TrType = 3)" +
            //" AND (dbo.MRSRMaster.OutSource='" + Session["sBrId"] + "')" +
            " AND (dbo.MRSRMaster.TDate >= '" + Convert.ToDateTime(this.txtFrom.Text) + "'" +
            " AND dbo.MRSRMaster.TDate <= '" + Convert.ToDateTime(this.txtToDate.Text) + "')" ;
            
            if (ddlEntity.SelectedItem.ToString() != "ALL")
            {
                sSql = sSql + " AND (dbo.MRSRMaster.OutSource='" + txtEID.Text + "')";
            }
            
            sSql = sSql + ""+
            " GROUP BY dbo.Product.Model, dbo.Product.ProdName," +
            " dbo.MRSRDetails.UnitPrice, dbo.MRSRDetails.DisRef, dbo.Product.ModelSerial" +
            " ORDER BY CAST(dbo.Product.ModelSerial AS INT)";
               
        SqlCommand cmd = new SqlCommand(sSql, con);
        dr = cmd.ExecuteReader();
        GridView2.DataSource = dr;
        GridView2.DataBind();
        dr.Close();
        con.Close();

    }


    protected void RadioButtonSales_CheckedChanged(object sender, EventArgs e)
    {
        if (RadioButton1.Checked == true)
        {
            this.txtCHNo.Visible = true;
            this.txtCHNo.Text = "";
            this.ddlGroup.Visible = false;
            this.ddlModel.Visible = false;
            this.txtSL.Visible = false;
            txtCHNo.Focus();

            GridView1.DataSource = null;
            GridView1.DataBind();
            GridView1.Visible = true;

            GridView2.DataSource = null;
            GridView2.DataBind();
            GridView2.Visible = true;

            lb1.Visible = true;
            lbl_id.Text = "";

            btnExport.Visible = false;

        }
        else if (RadioButton2.Checked == true)
        {
            this.txtCHNo.Visible = false;
            this.txtCHNo.Text = "";
            this.ddlGroup.Visible = false;
            this.ddlModel.Visible = false;
            this.txtSL.Visible = false;

            GridView1.DataSource = null;
            GridView1.DataBind();
            GridView1.Visible = true;

            GridView2.DataSource = null;
            GridView2.DataBind();
            GridView2.Visible = true;

            lb1.Visible = true;
            lbl_id.Text = "";

            btnExport.Visible = false;

        }
        else if (RadioButton3.Checked == true)
        {
            this.ddlGroup.Visible = true;
            this.txtCHNo.Visible = false;
            this.txtCHNo.Text = "";
            this.ddlModel.Visible = false;
            this.txtSL.Visible = false;

            lb1.Visible = false;
            lbl_id.Text = "";

            btnExport.Visible = true;

        }
        else if (RadioButton4.Checked == true)
        {
            this.ddlModel.Visible = true;
            this.txtCHNo.Visible = false;
            this.txtCHNo.Text = "";
            this.ddlGroup.Visible = false;
            this.txtSL.Visible = false;

            lb1.Visible = false;
            lbl_id.Text = "";
            
            btnExport.Visible = true;

        }
        else if (RadioButton5.Checked == true)
        {
            this.txtSL.Visible = true;
            this.txtCHNo.Visible = false;
            this.txtCHNo.Text = "";
            this.ddlGroup.Visible = false;
            this.ddlModel.Visible = false;

            lb1.Visible = false;
            lbl_id.Text = "";

            btnExport.Visible = true;

        }
        else if (RadioButton6.Checked == true)
        {
            this.txtSL.Visible = false;
            this.txtCHNo.Visible = false;
            this.txtCHNo.Text = "";
            this.ddlGroup.Visible = false;
            this.ddlModel.Visible = false;

            GridView1.DataSource = null;
            GridView1.DataBind();
            GridView1.Visible = false;

            GridView2.DataSource = null;
            GridView2.DataBind();
            GridView2.Visible = true;

            GridView3.DataSource = null;
            GridView3.DataBind();
            GridView3.Visible = true;

            lb1.Visible = false;
            lbl_id.Text = "";

            btnExport.Visible = true;

        }

        else if (RadioButton7.Checked == true)
        {
            this.txtSL.Visible = false;
            this.txtCHNo.Visible = false;
            this.txtCHNo.Text = "";
            this.ddlGroup.Visible = false;
            this.ddlModel.Visible = false;

            GridView1.DataSource = null;
            GridView1.DataBind();
            GridView1.Visible = false;

            GridView2.DataSource = null;
            GridView2.DataBind();
            GridView2.Visible = true;

            GridView3.DataSource = null;
            GridView3.DataBind();
            GridView3.Visible = true;

            lb1.Visible = false;
            lbl_id.Text = "";

            btnExport.Visible = true;

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

        lb1.Visible = false;
        lbl_id.Text = "";

        RadioButton1.Checked = false;
        RadioButton2.Checked = false;
        RadioButton3.Checked = false;
        RadioButton4.Checked = false;
        RadioButton5.Checked = false;

        btnExport.Visible = false;

    }


    //EXPORT TO EXCEL
    protected void ExportToExcel(object sender, EventArgs e)
    {
               
        Response.Clear();
        Response.Buffer = true;
        //Response.AddHeader("content-disposition", "attachment;filename=GridViewExport.xls");
        string filename = "" + Session["sBr"].ToString() + "_" + DateTime.Now.ToString() + ".xls";
        Response.AddHeader("content-disposition", "attachment;filename=" + filename);

        Response.Charset = "";
        Response.ContentType = "application/vnd.ms-excel";
        using (StringWriter sw = new StringWriter())
        {
            HtmlTextWriter hw = new HtmlTextWriter(sw);

            //To Export all pages
            GridView2.AllowPaging = false;

            if (RadioButton6.Checked == true)
            {
                this.fnLoadSalesSummary_ProdWise();
            }

            
            GridView2.HeaderRow.BackColor = Color.White;
            foreach (TableCell cell in GridView2.HeaderRow.Cells)
            {
                cell.BackColor = GridView2.HeaderStyle.BackColor;
            }
            foreach (GridViewRow row in GridView2.Rows)
            {
                row.BackColor = Color.White;
                foreach (TableCell cell in row.Cells)
                {
                    if (row.RowIndex % 2 == 0)
                    {
                        cell.BackColor = GridView2.AlternatingRowStyle.BackColor;
                    }
                    else
                    {
                        cell.BackColor = GridView2.RowStyle.BackColor;
                    }
                    cell.CssClass = "textmode";
                }
            }

            GridView2.RenderControl(hw);

            //style to format numbers to string
            string style = @"<style> .textmode { mso-number-format:\@; } </style>";
            Response.Write(style);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
        }
        
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Verifies that the control is rendered */
    }

    protected void ddlEntity_SelectedIndexChanged(object sender, EventArgs e)
    {
        //s = WebConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        //con = new SqlConnection(s);
        SqlConnection con = DBConnection.GetConnection();
        con.Open();

        string sSql = "";
        //sSql = "SELECT Code,Model,ProdName FROM Product" +
        //" WHERE (GroupName = '" + DropDownList1.SelectedItem.ToString() + "')" +            
        //" ORDER BY dbo.Product.ModelSerial";
               
        sSql = "SELECT EID, eName FROM dbo.Entity" +
            " WHERE (eName = '" + ddlEntity.SelectedItem.ToString() + "')" +
            " GROUP BY EID, eName" +
            " ORDER BY eName";

        SqlCommand cmd = new SqlCommand(sSql, con);
        dr = cmd.ExecuteReader();

        if (dr.Read() == true)
        {
            txtEID.Text = dr["EID"].ToString();
        }
        else
        {
            txtEID.Text = "0";
        }
       
        dr.Close();
        con.Close();
    }


    protected void ddlEntity_SelectedIndexChanged1(object sender, EventArgs e)
    {

    }
}