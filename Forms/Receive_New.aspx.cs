using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Globalization;

public partial class Forms_Receive_New : System.Web.UI.Page
{
    SqlConnection conn = DBConnection.GetConnection();
    SqlConnection con;
    SqlCommand cmd;
    SqlDataReader dr;
    private string s;
    private double runningTotal = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        btnReceive.Attributes.Add("OnClick", "return confirm_Save();");
        btnCancel.Attributes.Add("OnClick", "return confirm_Cancel();");

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

    protected void PopupMessage(string Msg, Control controlID)
    {
        ScriptManager.RegisterClientScriptBlock(controlID, controlID.GetType(), "msg", "alert('" + Msg + "');", true);

    }

    // Show data in GridView
    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        //s = WebConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        //con = new SqlConnection(s);
        SqlConnection con = DBConnection.GetConnection();
        con.Open();
        
        //cmd = new SqlCommand("Select * from Student where StudentName='" + DropDownList1.SelectedItem.ToString() + "'", con);
        
        string sSql = "";
        sSql="SELECT dbo.MRSRMaster.MRSRCode AS Challan_No," +
            " dbo.Product.Model, dbo.Product.ProdName AS Description," +
            " dbo.MRSRDetails.SLNO,dbo.MRSRDetails.Qty" +
            " FROM  dbo.Product INNER JOIN" +
            " dbo.MRSRDetails ON dbo.Product.ProductID = dbo.MRSRDetails.ProductID INNER JOIN" +
            " dbo.MRSRMaster ON dbo.MRSRDetails.MRSRMID = dbo.MRSRMaster.MRSRMID" +
            " WHERE (dbo.MRSRMaster.MRSRCode = '" + DropDownList1.SelectedItem.ToString() + "')" +
            " AND (dbo.MRSRMaster.TrType = 2)" +
            " AND (dbo.MRSRMaster.InSource='" + Session["sBrId"] + "')" +
            " ORDER BY dbo.Product.ModelSerial";

        SqlCommand cmd = new SqlCommand(sSql, con);
        dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            this.txtCHNo.Text = dr["Challan_No"].ToString();           
        }
        else
        {
            this.txtCHNo.Text = "";
        }
        GridView1.DataSource = dr;
        GridView1.DataBind();
        dr.Close();
        con.Close();
    }

    // Fill Dropdownlist
    public void FillDropDownList()
    {
        //s = WebConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        //con = new SqlConnection(s);
        SqlConnection con = DBConnection.GetConnection();
        con.Open();
        //cmd = new SqlCommand("Select StudentName from Student", con);
        

        string sSql = "";
                
        sSql = "";
        sSql = "SELECT MRSRCode " +            
            " FROM dbo.MRSRMaster" +
            " WHERE " +
            " (dbo.MRSRMaster.TrType = 2) AND (dbo.MRSRMaster.Tag = 2)" +
            " AND (dbo.MRSRMaster.InSource='" + Session["sBrId"] + "')" +
            //" AND (dbo.MRSRMaster.TDate='" + DateTime.Today.ToString("MM/dd/yyyy") + "')" +
            //" ORDER BY dbo.MRSRMaster.MRSRCode, dbo.MRSRMaster.TDate";
            " ORDER BY MRSRCode";

        SqlCommand cmd = new SqlCommand(sSql, con);
                
        dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            DropDownList1.Items.Add(dr[0].ToString());
        }
        dr.Close();
        con.Close();
    }


    //Grid View Footer Total
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CalcTotal(e.Row.Cells[4].Text);
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {

            e.Row.Cells[3].Text = "Total";
            //e.Row.Cells[10].Text = string.Format("{0:c}", runningTotal);
            e.Row.Cells[4].Text = runningTotal.ToString("0,0", CultureInfo.InvariantCulture);
           
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

    //CLEAR ALL TEXT AND GRID
    protected void btnCancel_Click(object sender, EventArgs e)
    {

        //CLEAR ALL TEXT
        this.txtCHNo.Text = "";
        

        //CLEAR GRIDVIEW
        GridView1.DataSource = null;
        GridView1.DataBind();

        DropDownList1.Items.Insert(0, new ListItem("---Select---", "---Select---"));
        FillDropDownList();
        DropDownList1.Focus();

    }

    //FINALLY SAVE DATA
    protected void btnReceive_Click(object sender, EventArgs e)
    {
        SqlConnection conn = DBConnection.GetConnection();        
        string sSql = "";

        //CHALLAN DATE VALIDATION        
        if (txtCHNo.Text == "")
        {
            PopupMessage("Please select Challan #...", btnReceive);
            DropDownList1.Focus();
            return;
        }
        
        //-----------------------------------------------------------------------------------
        //SAVE DATA IN MASTER TABLE
        sSql = "";        
        sSql = "UPDATE MRSRMaster SET Tag=0" +
            " WHERE MRSRCode='" + DropDownList1.Text.ToString() + "'";
        SqlCommand cmd = new SqlCommand(sSql, conn);
        conn.Open();
        cmd.ExecuteNonQuery();
        conn.Close();
        
        ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert",
                        "<script>alert('" + "Save Successfully." + "');</script>", false);
        
        //-----------------------------------------------------------------------------------
        //CLEAR ALL TEXT
        this.txtCHNo.Text = "";


        //CLEAR GRIDVIEW
        GridView1.DataSource = null;
        GridView1.DataBind();

        DropDownList1.Items.Insert(0, new ListItem("---Select---", "---Select---"));
        FillDropDownList();
        DropDownList1.Focus();
        //-----------------------------------------------------------------------------------

        return;

    }

}