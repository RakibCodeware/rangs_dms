using System;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;


public partial class Forms_Receive_List : System.Web.UI.Page
{
    SqlConnection conn = DBConnection.GetConnection();

    protected void Page_Load(object sender, EventArgs e)
    {
        /*
        // code provided by getcodesnippet.com
        SqlConnection conn = DBConnection.GetConnection();
        string sSql = "";

        sSql = "";
        sSql = "SELECT dbo.MRSRMaster.MRSRCode, dbo.MRSRMaster.TDate," +
            " dbo.Entity.eName" +
            " FROM dbo.MRSRMaster INNER JOIN  dbo.Entity ON dbo.MRSRMaster.OutSource = dbo.Entity.EID" +
            " WHERE " +
            " (dbo.MRSRMaster.TrType = 2) AND (dbo.MRSRMaster.Tag = 2)" +
            " AND (dbo.MRSRMaster.InSource='" + vDeclare.eID + "')" +
            " ORDER BY dbo.MRSRMaster.MRSRCode, dbo.MRSRMaster.TDate";

        SqlCommand cmd = new SqlCommand(sSql, conn);
        conn.Open();
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataTable dt = new DataTable();
        da.Fill(dt);
        conn.Close();

        ListView1.DataSource = dt;
        ListView1.DataBind();
         */

        if (!IsPostBack)
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
            BindGrid();
        }

    }

    protected void BindGrid()
    {
        SqlConnection conn = DBConnection.GetConnection();
        DataSet ds = new DataSet();
        conn.Open();
        //string cmdstr = "Select * from EmployeeDetails";
        string sSql = "";

        sSql = "";
        sSql = "SELECT " +
            " (SELECT  COUNT(*) FROM MRSRMaster AS t1" +
            " WHERE " +
            " (t1.TrType = 2) AND (t1.Tag = 2)" +
            " AND (t1.InSource='" + Session["sBrId"] + "')" +
            " AND t1.MRSRCode <= MRSRMaster.MRSRCode) AS sno," +
            " dbo.MRSRMaster.MRSRCode, " +
            " CONVERT(VARCHAR(12), dbo.MRSRMaster.TDate, 105) AS TDate," +
            " dbo.Entity.eName" +            
            " FROM dbo.MRSRMaster INNER JOIN  dbo.Entity ON dbo.MRSRMaster.OutSource = dbo.Entity.EID" +
            " WHERE " +
            " (dbo.MRSRMaster.TrType = 2) AND (dbo.MRSRMaster.Tag = 2)" +
            " AND (dbo.MRSRMaster.InSource='" + Session["sBrId"] + "')" +
            //" ORDER BY dbo.MRSRMaster.MRSRCode, dbo.MRSRMaster.TDate";
            " ORDER BY sno";

        SqlCommand cmd = new SqlCommand(sSql, conn);
        SqlDataAdapter adp = new SqlDataAdapter(cmd);
        adp.Fill(ds);
        cmd.ExecuteNonQuery();
        conn.Close();
        gvEmployeeDetails.DataSource = ds;
        gvEmployeeDetails.DataBind();

    }

    protected void gvEmployeeDetails_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        SqlConnection conn = DBConnection.GetConnection();
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblCHNo = (Label)e.Row.FindControl("lblCHNo");
            GridView gv_Child = (GridView)e.Row.FindControl("gv_Child");

            string txtCHNo = lblCHNo.Text;

            DataSet ds = new DataSet();
            conn.Open();
            //string cmdstr = "Select * from Salary_Details where empid=@empid";
            string sSql = "";
            sSql="SELECT dbo.MRSRMaster.MRSRCode, dbo.MRSRMaster.TDate, dbo.Product.Code, " +
                "  dbo.Product.ProductID, dbo.Product.Model, dbo.Product.ProdName, " +
                " dbo.MRSRDetails.SLNO," +
                " SUM(dbo.MRSRDetails.Qty) AS tQty" +

                " FROM dbo.MRSRMaster INNER JOIN" +                
                " dbo.MRSRDetails ON dbo.MRSRMaster.MRSRMID = dbo.MRSRDetails.MRSRMID INNER JOIN" +
                " dbo.Product ON dbo.MRSRDetails.ProductID = dbo.Product.ProductID" +

                " WHERE (dbo.MRSRMaster.TrType = 2) AND (dbo.MRSRMaster.Tag = 2)" +
                " AND (dbo.MRSRMaster.MRSRCode = '" + txtCHNo.ToString() + "')" +

                " GROUP BY dbo.MRSRMaster.MRSRCode, dbo.MRSRMaster.TDate, dbo.Product.Code," +
                " dbo.Product.ProductID, dbo.Product.Model, dbo.Product.ProdName," + 
                " dbo.MRSRDetails.SLNO" +                
                " ORDER BY dbo.MRSRMaster.MRSRCode";


            SqlCommand cmd = new SqlCommand(sSql, conn);
            cmd.Parameters.AddWithValue("@MRSRCode", txtCHNo);
            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            adp.Fill(ds);
            cmd.ExecuteNonQuery();
            conn.Close();
            gv_Child.DataSource = ds;
            gv_Child.DataBind();

        }
    }

    protected void gv_Child_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        SqlConnection conn = DBConnection.GetConnection();
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblProdID = (Label)e.Row.FindControl("lblProdID");
            //Label lblCHNo = (Label)e.Row.FindControl("lblProdID");
            GridView gv_NestedChild = (GridView)e.Row.FindControl("gv_NestedChild");

            string txtProdId = lblProdID.Text;
            //string txtCHNo = lblCHNo.Text;

            DataSet ds = new DataSet();
            conn.Open();
            //string cmdstr = "Select * from Leave_Details where salary_id=@salary_id";

            string sSql = "";
            //sSql = "SELECT SUM(dbo.MRSRDetails.Qty) AS tQty, dbo.MRSRMaster.MRSRCode" +
              //  " FROM dbo.MRSRMaster INNER JOIN" +
                //" dbo.MRSRDetails ON dbo.MRSRMaster.MRSRMID = dbo.MRSRDetails.MRSRMID" +
                //" WHERE (dbo.MRSRMaster.TrType = 2) AND (dbo.MRSRMaster.Tag = 2)" +
                //" AND (dbo.MRSRMaster.MRSRCode = '" + txtCHNo.ToString() + "')" +
                //" GROUP BY dbo.MRSRMaster.MRSRCode";

            sSql = "Select * from Product where ProductID=@ProductID";

            SqlCommand cmd = new SqlCommand(sSql, conn);
            cmd.Parameters.AddWithValue("@ProductID", txtProdId);
            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            adp.Fill(ds);
            cmd.ExecuteNonQuery();
            conn.Close();
            gv_NestedChild.DataSource = ds;
            gv_NestedChild.DataBind();
        }

    }

}