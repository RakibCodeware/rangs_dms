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

public partial class FormsReport_Admin_frmStockReport_Admin : System.Web.UI.Page
{
    SqlConnection conn = DBConnection.GetConnection();
    SqlConnection conn1 = DBConnection.GetConnection();
    SqlConnection _connStr = DBConnection.GetConnection();
    SqlConnection con;
    int iMRSRID = 0;
    DateTime tDate;
    SqlDataReader dr;

    private string s;

    private double runningTotal = 0;
    private double runningOBQty = 0;
    private double runningInQty = 0;
    private double runningOutQty = 0;
    private double runningStockQty = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (System.Convert.ToInt32(Session["Vis"]) == 0)
            {
                Response.Redirect("../Account/Login.aspx");
            }

            ddlModel.Items.Insert(0, new ListItem("ALL", "ALL"));
            LoadDropDownList_Model();

            //ddlGroup.Items.Insert(0, new ListItem("ALL", "ALL"));
            LoadDropDownList_Group();

            ddlEntity.Items.Insert(0, new ListItem("ALL", "ALL"));
            FillDropDownList_Entity();

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
            " WHERE (ActiveDeactive = 1) AND EID='" + Session["sBrId"] + "'" +
            //" (EntityType = 'Showroom' OR EntityType = 'Dealer')" +
            " ORDER BY eName";

        SqlCommand cmd = new SqlCommand(sSql, con);

        dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            ddlEntity.Items.Add(dr[0].ToString());
        }
        dr.Close();
        con.Close();
    }

    //LOAD PRODUCT GROUP IN DROPDOWN LIST
    protected void LoadDropDownList_Group()
    {
        String strQuery = "SELECT DISTINCT GroupName FROM dbo.Product ORDER BY GroupName";
        SqlDataAdapter da = new SqlDataAdapter(strQuery, _connStr);
        DataSet ds = new DataSet();
        da.Fill(ds);
        ddlGroup.DataSource = ds.Tables[0];
        ddlGroup.DataValueField = "GroupName";
        ddlGroup.DataTextField = "GroupName";
        ddlGroup.DataBind();

        ddlGroup.Items.Insert(0, new ListItem("", ""));

        ddlGroup1.DataSource = ds.Tables[0];
        ddlGroup1.DataValueField = "GroupName";
        ddlGroup1.DataTextField = "GroupName";
        ddlGroup1.DataBind();
        
        ddlGroup1.Items.Insert(0, new ListItem("ALL", "ALL"));

    }

    //LOAD PRODUCT MODEL IN DROPDOWN LIST
    protected void LoadDropDownList_Model()
    {
        //ddlContinents.AppendDataBoundItems = true;
        //String strConnString = ConfigurationManager
        //.ConnectionStrings["conn"].ConnectionString;
        String strQuery = "select Model from Product WHERE Discontinue='No' ORDER BY Model";
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
            //ddlContinents.DataValueField = "ProductID";
            ddlModel.DataValueField = "Model";
            ddlModel.DataBind();

            ddlModel.Items.Insert(0, new ListItem("ALL", "ALL"));
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


    //PRODUCT GROUP WISE STOCK
    public void fnLoadGroupWiseStock()
    {
        //s = WebConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        //con = new SqlConnection(s);
        SqlConnection con = DBConnection.GetConnection();
        con.Open();

        string sCHNo = Convert.ToString(this.lbl_id.Text);

        string sSql = "";
        sSql = "SELECT dbo.Product.Model, dbo.Product.ProdName AS [Product Description]," +
            " SUM(ISNULL(OB_In.OBInQty, 0))- SUM(ISNULL(OB_Out.OBOutQty, 0)) AS OBQty," +
            " SUM(ISNULL(a.InQty, 0)) AS InQty, SUM(ISNULL(b.OutQty, 0)) AS OutQty," +
            " SUM(ISNULL(OB_In.OBInQty,0)) - SUM(ISNULL(OB_Out.OBOutQty, 0))" +
            " + SUM(ISNULL(a.InQty, 0)) - SUM(ISNULL(b.OutQty, 0)) AS StockQty" +
            " FROM dbo.Product LEFT OUTER JOIN" +

            " (SELECT dbo.MRSRMaster.InSource, dbo.Product.Model, SUM(dbo.MRSRDetails.Qty) AS OBInQty" +
                " FROM dbo.MRSRMaster INNER JOIN" +
                " dbo.MRSRDetails ON dbo.MRSRMaster.MRSRMID = dbo.MRSRDetails.MRSRMID INNER JOIN" +
                " dbo.Product ON dbo.MRSRDetails.ProductID = dbo.Product.ProductID" +
                " WHERE (dbo.MRSRMaster.TDate < '" + Convert.ToDateTime(this.txtFrom.Text) + "') AND" +
                " (dbo.MRSRMaster.TrType = 1 OR dbo.MRSRMaster.TrType = 2 OR" +
                " dbo.MRSRMaster.TrType = 3 OR dbo.MRSRMaster.TrType = 4 OR" +
                " dbo.MRSRMaster.TrType = - 1 OR dbo.MRSRMaster.TrType = - 3)";
        if (ddlGroup.SelectedItem.Text != "ALL")
        {
            sSql = sSql + " AND (dbo.Product.GroupName = '" + this.ddlGroup.SelectedItem.Text + "')";
        }

        sSql = sSql +  " AND (dbo.MRSRMaster.InSource = '" + Session["sBrId"] + "')";
        //if (ddlEntity.SelectedItem.ToString() != "ALL")
        //{
        //    sSql = sSql + " AND (dbo.MRSRMaster.InSource='" + txtEID.Text + "')";
        //}
        sSql = sSql + "" +
        " GROUP BY dbo.MRSRMaster.InSource, dbo. Product .Model," +
        " dbo. Product .ModelSerial) OB_In ON " +
    " dbo.Product.Model = OB_In.Model LEFT OUTER JOIN" +
    " (SELECT dbo.MRSRMaster.OutSource, dbo.Product.Model,SUM(ABS(dbo.MRSRDetails.Qty)) AS OutQty" +
        " FROM dbo.MRSRMaster INNER JOIN" +
        " dbo.MRSRDetails ON dbo.MRSRMaster.MRSRMID = dbo.MRSRDetails.MRSRMID INNER JOIN" +
        " dbo. Product ON dbo.MRSRDetails.ProductID = dbo. Product .ProductID" +
        " WHERE (dbo.MRSRMaster.TDate >= '" + Convert.ToDateTime(this.txtFrom.Text) + "')" +
        " AND (dbo.MRSRMaster.TDate <= '" + Convert.ToDateTime(this.txtToDate.Text) + "')" +
        " AND (dbo.MRSRMaster.TrType = 1 OR dbo.MRSRMaster.TrType = 2 OR" +
        " dbo.MRSRMaster.TrType = 3 OR dbo.MRSRMaster.TrType = 4 OR" +
        " dbo.MRSRMaster.TrType = - 1 OR dbo.MRSRMaster.TrType = - 3)";
        if (ddlGroup.SelectedItem.Text != "ALL")
        {
            sSql = sSql + " AND (dbo.Product.GroupName = '" + this.ddlGroup.SelectedItem.Text + "')";
        }

        sSql = sSql + " AND (dbo.MRSRMaster.OutSource = '" + Session["sBrId"] + "')";
        //if (ddlEntity.SelectedItem.ToString() != "ALL")
        //{
        //    sSql = sSql + " AND (dbo.MRSRMaster.OutSource='" + txtEID.Text + "')";
        //}
        sSql = sSql + "" +
        " GROUP BY dbo.MRSRMaster.OutSource, dbo. Product .Model," +
        " dbo. Product .ModelSerial) b ON dbo.Product.Model = b.Model LEFT OUTER JOIN" +
    " (SELECT dbo.MRSRMaster.OutSource, dbo. Product .Model, SUM(ABS(dbo.MRSRDetails.Qty)) AS OBOutQty" +
        " FROM dbo.MRSRMaster INNER JOIN" +
        " dbo.MRSRDetails ON dbo.MRSRMaster.MRSRMID = dbo.MRSRDetails.MRSRMID INNER JOIN" +
        " dbo. Product ON dbo.MRSRDetails.ProductID = dbo. Product .ProductID" +
        " WHERE (dbo.MRSRMaster.TDate < '" + Convert.ToDateTime(this.txtFrom.Text) + "')" +
        " AND (dbo.MRSRMaster.TrType = 1 OR dbo.MRSRMaster.TrType = 2 OR" +
        " dbo.MRSRMaster.TrType = 3 OR dbo.MRSRMaster.TrType = 4 OR" +
        " dbo.MRSRMaster.TrType = - 1 OR dbo.MRSRMaster.TrType = - 3)";
        if (ddlGroup.SelectedItem.Text != "ALL")
        {
            sSql = sSql + " AND (dbo.Product.GroupName = '" + this.ddlGroup.SelectedItem.Text + "')";
        }
        sSql = sSql + " AND (dbo.MRSRMaster.OutSource = '" + Session["sBrId"] + "')";
        //if (ddlEntity.SelectedItem.ToString() != "ALL")
        //{
        //    sSql = sSql + " AND (dbo.MRSRMaster.OutSource='" + txtEID.Text + "')";
        //}
        sSql = sSql + "" +
        " GROUP BY dbo.MRSRMaster.OutSource, dbo.Product.Model," +
        " dbo.Product.ModelSerial) OB_Out ON dbo.Product.Model = OB_Out.Model LEFT OUTER JOIN" +
    " (SELECT dbo.MRSRMaster.InSource, dbo. Product .Model, SUM(dbo.MRSRDetails.Qty) AS InQty" +
        " FROM dbo.MRSRMaster INNER JOIN" +
        " dbo.MRSRDetails ON dbo.MRSRMaster.MRSRMID = dbo.MRSRDetails.MRSRMID INNER JOIN" +
        " dbo. Product ON dbo.MRSRDetails.ProductID = dbo. Product .ProductID" +
        " WHERE (dbo.MRSRMaster.TDate >= '" + Convert.ToDateTime(this.txtFrom.Text) + "')" +
        " AND (dbo.MRSRMaster.TDate <= '" + Convert.ToDateTime(this.txtToDate.Text) + "')" +
        " AND (dbo.MRSRMaster.TrType = 1 OR dbo.MRSRMaster.TrType = 2 OR" +
        " dbo.MRSRMaster.TrType = 3 OR dbo.MRSRMaster.TrType = 4 OR" +
        " dbo.MRSRMaster.TrType = - 1 OR dbo.MRSRMaster.TrType = - 3)";
        if (ddlGroup.SelectedItem.Text != "ALL")
        {
            sSql = sSql + " AND (dbo.Product.GroupName = '" + this.ddlGroup.SelectedItem.Text + "')";
        }
        sSql = sSql + " AND (dbo.MRSRMaster.InSource = '" + Session["sBrId"] + "')";
        //if (ddlEntity.SelectedItem.ToString() != "ALL")
        //{
        //    sSql = sSql + " AND (dbo.MRSRMaster.InSource='" + txtEID.Text + "')";
        //}

        sSql = sSql + "" +
        " GROUP BY dbo.MRSRMaster.InSource, dbo.Product.Model," +
        " dbo.Product.ModelSerial) a ON dbo.Product.Model = a.Model";

        if (ddlGroup.SelectedItem.Text != "ALL")
        {
            sSql = sSql + " WHERE (dbo.Product.GroupName = '" + this.ddlGroup.SelectedItem.Text + "')";
        }

    sSql = sSql + "" +
    " GROUP BY dbo.Product.Model, dbo.Product.ModelSerial, dbo.Product.ProdName" +
    " HAVING (SUM(ISNULL(OB_In.OBInQty, 0)) - SUM(ISNULL(OB_Out.OBOutQty, 0))" +
    " + SUM(ISNULL(a.InQty, 0)) - SUM(ISNULL(b.OutQty, 0)) <> 0)" +
    " ORDER BY CAST(dbo.Product.ModelSerial AS INT)";

        SqlCommand cmd = new SqlCommand(sSql, con);
        dr = cmd.ExecuteReader();
        GridView2.DataSource = dr;
        GridView2.DataBind();
        dr.Close();
        con.Close();

    }

    //PRODUCT MODEL WISE STOCK
    public void fnLoadModelWiseStock()
    {
        //s = WebConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        //con = new SqlConnection(s);
        SqlConnection con = DBConnection.GetConnection();
        con.Open();

        string sCHNo = Convert.ToString(this.lbl_id.Text);

        string sSql = "";
        sSql = "SELECT dbo.Product.Model, dbo.Product.ProdName AS [Product Description]," +
            " SUM(ISNULL(OB_In.OBInQty, 0))- SUM(ISNULL(OB_Out.OBOutQty, 0)) AS OBQty," +
            " SUM(ISNULL(a.InQty, 0)) AS InQty, SUM(ISNULL(b.OutQty, 0)) AS OutQty," +
            " SUM(ISNULL(OB_In.OBInQty,0)) - SUM(ISNULL(OB_Out.OBOutQty, 0))" +
            " + SUM(ISNULL(a.InQty, 0)) - SUM(ISNULL(b.OutQty, 0)) AS StockQty" +
            " FROM dbo.Product LEFT OUTER JOIN" +

            " (SELECT dbo.MRSRMaster.InSource, dbo.Product.Model, SUM(dbo.MRSRDetails.Qty) AS OBInQty" +
                " FROM dbo.MRSRMaster INNER JOIN" +
                " dbo.MRSRDetails ON dbo.MRSRMaster.MRSRMID = dbo.MRSRDetails.MRSRMID INNER JOIN" +
                " dbo.Product ON dbo.MRSRDetails.ProductID = dbo.Product.ProductID" +
                " WHERE (dbo.MRSRMaster.TDate < '" + Convert.ToDateTime(this.txtFrom.Text) + "') AND" +
                " (dbo.MRSRMaster.TrType = 1 OR dbo.MRSRMaster.TrType = 2 OR" +
                " dbo.MRSRMaster.TrType = 3 OR dbo.MRSRMaster.TrType = 4 OR" +
                " dbo.MRSRMaster.TrType = - 1 OR dbo.MRSRMaster.TrType = - 3)";

        sSql = sSql + " AND (dbo.MRSRMaster.InSource = '" + Session["EID"] + "')";
        //if (ddlEntity.SelectedItem.ToString() != "ALL")
        //{
        //    sSql = sSql + " AND (dbo.MRSRMaster.InSource='" + txtEID.Text + "')";
        //}

        sSql = sSql + "" +
        " GROUP BY dbo.MRSRMaster.InSource, dbo. Product .Model," +
        " dbo. Product .ModelSerial) OB_In ON " +
    " dbo.Product.Model = OB_In.Model LEFT OUTER JOIN" +
    " (SELECT dbo.MRSRMaster.OutSource, dbo.Product.Model,SUM(ABS(dbo.MRSRDetails.Qty)) AS OutQty" +
        " FROM dbo.MRSRMaster INNER JOIN" +
        " dbo.MRSRDetails ON dbo.MRSRMaster.MRSRMID = dbo.MRSRDetails.MRSRMID INNER JOIN" +
        " dbo. Product ON dbo.MRSRDetails.ProductID = dbo. Product .ProductID" +
        " WHERE (dbo.MRSRMaster.TDate >= '" + Convert.ToDateTime(this.txtFrom.Text) + "')" +
        " AND (dbo.MRSRMaster.TDate <= '" + Convert.ToDateTime(this.txtToDate.Text) + "')" +
        " AND (dbo.MRSRMaster.TrType = 1 OR dbo.MRSRMaster.TrType = 2 OR" +
        " dbo.MRSRMaster.TrType = 3 OR dbo.MRSRMaster.TrType = 4 OR" +
        " dbo.MRSRMaster.TrType = - 1 OR dbo.MRSRMaster.TrType = - 3)";

        sSql = sSql + " AND (dbo.MRSRMaster.OutSource = '" + Session["EID"] + "')";
        //if (ddlEntity.SelectedItem.ToString() != "ALL")
        //{
        //    sSql = sSql + " AND (dbo.MRSRMaster.OutSource='" + txtEID.Text + "')";
        //}

        sSql = sSql + "" +
        " GROUP BY dbo.MRSRMaster.OutSource, dbo. Product .Model," +
        " dbo. Product .ModelSerial) b ON dbo.Product.Model = b.Model LEFT OUTER JOIN" +
    " (SELECT dbo.MRSRMaster.OutSource, dbo. Product .Model, SUM(ABS(dbo.MRSRDetails.Qty)) AS OBOutQty" +
        " FROM dbo.MRSRMaster INNER JOIN" +
        " dbo.MRSRDetails ON dbo.MRSRMaster.MRSRMID = dbo.MRSRDetails.MRSRMID INNER JOIN" +
        " dbo. Product ON dbo.MRSRDetails.ProductID = dbo. Product .ProductID" +
        " WHERE (dbo.MRSRMaster.TDate < '" + Convert.ToDateTime(this.txtFrom.Text) + "')" +
        " AND (dbo.MRSRMaster.TrType = 1 OR dbo.MRSRMaster.TrType = 2 OR" +
        " dbo.MRSRMaster.TrType = 3 OR dbo.MRSRMaster.TrType = 4 OR" +
        " dbo.MRSRMaster.TrType = - 1 OR dbo.MRSRMaster.TrType = - 3)";

        sSql = sSql + " AND (dbo.MRSRMaster.OutSource = '" + Session["EID"] + "')";

        //if (ddlEntity.SelectedItem.ToString() != "ALL")
        //{
        //    sSql = sSql + " AND (dbo.MRSRMaster.OutSource='" + txtEID.Text + "')";
        //}

        sSql = sSql + "" +
        " GROUP BY dbo.MRSRMaster.OutSource, dbo.Product.Model," +
        " dbo.Product.ModelSerial) OB_Out ON dbo.Product.Model = OB_Out.Model LEFT OUTER JOIN" +
    " (SELECT dbo.MRSRMaster.InSource, dbo. Product .Model, SUM(dbo.MRSRDetails.Qty) AS InQty" +
        " FROM dbo.MRSRMaster INNER JOIN" +
        " dbo.MRSRDetails ON dbo.MRSRMaster.MRSRMID = dbo.MRSRDetails.MRSRMID INNER JOIN" +
        " dbo. Product ON dbo.MRSRDetails.ProductID = dbo. Product .ProductID" +
        " WHERE (dbo.MRSRMaster.TDate >= '" + Convert.ToDateTime(this.txtFrom.Text) + "')" +
        " AND (dbo.MRSRMaster.TDate <= '" + Convert.ToDateTime(this.txtToDate.Text) + "')" +
        " AND (dbo.MRSRMaster.TrType = 1 OR dbo.MRSRMaster.TrType = 2 OR" +
        " dbo.MRSRMaster.TrType = 3 OR dbo.MRSRMaster.TrType = 4 OR" +
        " dbo.MRSRMaster.TrType = - 1 OR dbo.MRSRMaster.TrType = - 3)";

        sSql = sSql + " AND (dbo.MRSRMaster.InSource = '" + Session["sBrId"] + "')";
        //if (ddlEntity.SelectedItem.ToString() != "ALL")
        //{
        //    sSql = sSql + " AND (dbo.MRSRMaster.InSource='" + txtEID.Text + "')";
        //}

        sSql = sSql + "" +
        " GROUP BY dbo.MRSRMaster.InSource, dbo.Product.Model," +
        " dbo.Product.ModelSerial) a ON dbo.Product.Model = a.Model";

        if (ddlModel.SelectedItem.Text != "ALL")
        {
            sSql = sSql + " WHERE (dbo.Product.Model = '" + this.ddlModel.SelectedItem.Text + "')";
        }
        sSql = sSql + "" +
    " GROUP BY dbo.Product.Model, dbo.Product.ModelSerial, dbo.Product.ProdName" +
    " HAVING (SUM(ISNULL(OB_In.OBInQty, 0)) - SUM(ISNULL(OB_Out.OBOutQty, 0))" +
    " + SUM(ISNULL(a.InQty, 0)) - SUM(ISNULL(b.OutQty, 0)) <> 0)" +
    " ORDER BY CAST(dbo.Product.ModelSerial AS INT)";

        SqlCommand cmd = new SqlCommand(sSql, con);
        dr = cmd.ExecuteReader();
        GridView2.DataSource = dr;
        GridView2.DataBind();
        dr.Close();
        con.Close();

    }


    //SEARCH
    protected void btnAdd_Click(object sender, EventArgs e)
    {

        if (RadioButton1.Checked == true)
        {
            GridView2.DataSource = null;
            GridView2.DataBind();

            if (ddlGroup.SelectedItem.Text == "")
            {
                PopupMessage("Please select Product Group.", btnAdd);
                ddlGroup.Focus();
                return;
            }

            lbl_id.Text = "( Stock From " + txtFrom.Text + " To " + txtToDate.Text + " )";

            fnLoadGroupWiseStock();
        }
        else if (RadioButton2.Checked == true)
        {
            GridView2.DataSource = null;
            GridView2.DataBind();

            if (ddlModel.SelectedItem.Text == "")
            {
                PopupMessage("Please select Product Model.", btnAdd);
                ddlModel.Focus();
                return;
            }

            lbl_id.Text = "( Stock From " + txtFrom.Text + " To " + txtToDate.Text + " )";

            fnLoadModelWiseStock();
        }


    }



    protected void PopupMessage(string Msg, Control controlID)
    {
        ScriptManager.RegisterClientScriptBlock(controlID, controlID.GetType(), "msg", "alert('" + Msg + "');", true);
    }

    //Grid View 2 Footer Total
    protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CalcTotalQtyOB(e.Row.Cells[2].Text);
            CalcTotalQtyIN(e.Row.Cells[3].Text);
            CalcTotalQtyOUT(e.Row.Cells[4].Text);
            CalcTotalQtyStok(e.Row.Cells[5].Text);

            double value = Convert.ToDouble(e.Row.Cells[2].Text);
            e.Row.Cells[2].Text = value.ToString("#,#.##");

            double value1 = Convert.ToDouble(e.Row.Cells[3].Text);
            e.Row.Cells[3].Text = value1.ToString("#,#.##");

            double value2 = Convert.ToDouble(e.Row.Cells[4].Text);
            e.Row.Cells[4].Text = value2.ToString("#,#.##");

            double value3 = Convert.ToDouble(e.Row.Cells[5].Text);
            e.Row.Cells[5].Text = value3.ToString("#,#.##");

            //RIGHT ALIGNMENT
            e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Right;

        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[0].Text = "Total";
            //e.Row.Cells[10].Text = string.Format("{0:c}", runningTotal);
            e.Row.Cells[2].Text = runningOBQty.ToString("0,0", CultureInfo.InvariantCulture);
            e.Row.Cells[3].Text = runningInQty.ToString("0,0", CultureInfo.InvariantCulture);
            e.Row.Cells[4].Text = runningOutQty.ToString("0,0", CultureInfo.InvariantCulture);
            e.Row.Cells[5].Text = runningStockQty.ToString("0,0", CultureInfo.InvariantCulture);

            e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Right;
        }
    }

    //CALCULATE TOTAL OB QTY
    private void CalcTotalQtyOB(string _qty)
    {
        try
        {
            runningOBQty += Double.Parse(_qty);
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write(ex.ToString());
        }
    }

    //CALCULATE TOTAL IN QTY
    private void CalcTotalQtyIN(string _qty)
    {
        try
        {
            runningInQty += Double.Parse(_qty);
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write(ex.ToString());
        }
    }

    //CALCULATE TOTAL OUT QTY
    private void CalcTotalQtyOUT(string _qty)
    {
        try
        {
            runningOutQty += Double.Parse(_qty);
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write(ex.ToString());
        }
    }

    //CALCULATE TOTAL STOCK QTY
    private void CalcTotalQtyStok(string _qty)
    {
        try
        {
            runningStockQty += Double.Parse(_qty);
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write(ex.ToString());
        }
    }

    protected void RadioButtonSales_CheckedChanged(object sender, EventArgs e)
    {
        if (RadioButton1.Checked == true)
        {
            this.ddlGroup.Visible = true;
            this.ddlModel.Visible = false;
            this.ddlGroup1.Visible = false;
        }
        else if (RadioButton2.Checked == true)
        {
            this.ddlGroup.Visible = false;
            this.ddlModel.Visible = true;
            this.ddlGroup1.Visible = false;
        }
        else if (RadioButton3.Checked == true)
        {
            this.ddlGroup.Visible = false;
            this.ddlModel.Visible = false;
            this.ddlGroup1.Visible = true;
        }

    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        GridView2.DataSource = null;
        GridView2.DataBind();
        GridView2.Visible = false;

        lbl_id.Text = "";

        RadioButton1.Checked = false;
        RadioButton2.Checked = false;

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

}