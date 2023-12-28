using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Globalization;
using System.Text.RegularExpressions;

using System.Drawing;
using System.Drawing.Drawing2D;

public partial class credit_limit : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            fnLoadCombo_Zone(DropDownList1, "CatName", "CategoryID", "Zone");
            //this.BindGrid();
        }

    }


    private void BindGrid()
    {

        SqlConnection conn = DBConnectionDSM.GetConnection();

        //SqlCommand dataCommand = new SqlCommand();
        //dataCommand.Connection = conn;

        //string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
        
        string query = "";       
        query = "SELECT dbo.Zone.CategoryID AS ZoneID, dbo.Zone.CatName AS ZoneName, dbo.DelearInfo.Code,";
        query = query + " dbo.DelearInfo.Name, dbo.tbCreditLimitYearly.TAmount, dbo.tbCreditLimitYearly.TID";
        query = query + " FROM dbo.Zone INNER JOIN";
        query = query + " dbo.DelearInfo ON dbo.Zone.CategoryID = dbo.DelearInfo.CategoryID INNER JOIN";
        query = query + " dbo.tbCreditLimitYearly ON dbo.DelearInfo.DAID = dbo.tbCreditLimitYearly.DealerID";


        if (DropDownList1.SelectedItem.Text != "ALL")
        {
            query = query + " WHERE dbo.Zone.CatName='" + DropDownList1.SelectedItem.Text + "'";
        }



        //using (SqlConnection con = new SqlConnection(constr))
        //{
        //    using (SqlDataAdapter sda = new SqlDataAdapter(query, con))
        //    {
        //        using (DataTable dt = new DataTable())
        //        {
        //            sda.Fill(dt);
        //            GridView1.DataSource = dt;
        //            GridView1.DataBind();
        //        }
        //    }
        //}


        SqlCommand cmd = new SqlCommand(query, conn);
        //SqlDataReader dr = cmd.ExecuteReader();
        conn.Open();

        DataSet ds = new DataSet();
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        da.Fill(ds);
        //da.Fill(dt);

        GridView1.DataSource = ds;
        //GridView1.DataSource = dt;
        GridView1.DataBind();
        //dr.Close();
        conn.Close();


    }

    protected void Insert(object sender, EventArgs e)
    {
        //string name = txtName.Text;
        //string country = txtCountry.Text;
        //txtName.Text = "";
        //txtCountry.Text = "";
        //string query = "INSERT INTO tbProduct VALUES(@Name, @Country)";
        //string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
        //using (SqlConnection con = new SqlConnection(constr))
        //{
        //    using (SqlCommand cmd = new SqlCommand(query))
        //    {
        //        cmd.Parameters.AddWithValue("@Name", name);
        //        cmd.Parameters.AddWithValue("@Country", country);
        //        cmd.Connection = con;
        //        con.Open();
        //        cmd.ExecuteNonQuery();
        //        con.Close();
        //    }
        //}
        //this.BindGrid();
    }

    protected void OnRowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView1.EditIndex = e.NewEditIndex;
        this.BindGrid();
    }

    protected void OnRowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridViewRow row = GridView1.Rows[e.RowIndex];
        int productId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);

        string dCode = (row.FindControl("lblCode") as Label).Text;
        string mrp = (row.FindControl("txtMRP") as TextBox).Text;
        //string salePrice = (row.FindControl("txtPrice") as TextBox).Text;
        //string OrderBy = (row.FindControl("txtOrder") as TextBox).Text;

        //string query = "UPDATE tbProduct SET MRP=@MRP, SalePrice=@SalePrice, OrderBy=@OrderBy WHERE ProductId=@productId";
        //string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
        
        //using (SqlConnection con = new SqlConnection(constr))
        //{
        //    using (SqlCommand cmd = new SqlCommand(query))
        //    {
        //        cmd.Parameters.AddWithValue("@productId", productId);
        //        cmd.Parameters.AddWithValue("@MRP", mrp);
        //        cmd.Parameters.AddWithValue("@SalePrice", salePrice);
        //        cmd.Parameters.AddWithValue("@OrderBy", OrderBy);
        //        cmd.Connection = con;
        //        con.Open();
        //        cmd.ExecuteNonQuery();
        //        con.Close();
        //    }
        //}

        //-----------------------------------------------------
        SqlConnection conn = DBConnectionDSM.GetConnection();

        //SqlCommand dataCommand = new SqlCommand();
        //dataCommand.Connection = conn;

        //-----------------------------------------------------
        string gSql = "";
        gSql = "UPDATE tbCreditLimitYearly SET TAmount='" + mrp + "'";
        //gSql = gSql + " SalePrice='" + salePrice + "', OrderBy='" + OrderBy + "'";
        gSql = gSql + " WHERE TID='" + productId + "'";

        SqlCommand cmdIns = new SqlCommand(gSql, conn);
        conn.Open();
        cmdIns.ExecuteNonQuery();
        conn.Close();
        //-----------------------------------------------------

        //----------------------------------------------------------------------------------
        //INSERT INTO LOG TABLE
        gSql = "";
        gSql = "INSERT INTO tbCreditLimit_Log (TID,DCode,TAmount,";
        gSql = gSql + " UserID, EntryDate";
        gSql = gSql + " ) ";
        gSql = gSql + " VALUES ('" + productId + "','" + dCode + "', '" + mrp + "',";
        gSql = gSql + " '" + Session["sUser"].ToString() + "','" + DateTime.Now + "'";
        gSql = gSql + " )";

        SqlCommand cmdIns1 = new SqlCommand(gSql, conn);
        conn.Open();
        cmdIns1.ExecuteNonQuery();
        conn.Close();
        //-------------------------------------------------------------------------------------------


        GridView1.EditIndex = -1;
        this.BindGrid();

    }

    protected void OnRowCancelingEdit(object sender, EventArgs e)
    {
        GridView1.EditIndex = -1;
        this.BindGrid();
    }

    protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        //int customerId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
        //string query = "DELETE FROM tbProduct WHERE ProductId=@productId";
        //string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
        //using (SqlConnection con = new SqlConnection(constr))
        //{
        //    using (SqlCommand cmd = new SqlCommand(query))
        //    {
        //        cmd.Parameters.AddWithValue("@productId", customerId);
        //        cmd.Connection = con;
        //        con.Open();
        //        cmd.ExecuteNonQuery();
        //        con.Close();
        //    }
        //}

        //this.BindGrid();
    }

    protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex != GridView1.EditIndex)
        //{
        //    (e.Row.Cells[4].Controls[4] as LinkButton).Attributes["onclick"] = "return confirm('Do you want to delete this row?');";
        //}
    }

    protected void OnPaging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        this.BindGrid();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        this.BindGrid();
    }

    private void fnLoadCombo_Zone(DropDownList xCombo, string pField, string pFieldID, string pTable)
    {
        SqlConnection conn = DBConnectionDSM.GetConnection();

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
            //xCombo.Items.Insert(0, new ListItem("ALL", "ALL"));

            conn.Close();
        }
        catch (Exception ex)
        {
            //lblmsg.Text = ex.Message;
        }
        finally
        {
            conn.Close();
        }

    }
}