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
using System.Globalization;
using System.Text.RegularExpressions;

public partial class Admin_Search_Prod_history : System.Web.UI.Page
{
    SqlConnection conn = DBConnection.GetConnection();
    SqlConnection conn1 = DBConnection.GetConnection();
    SqlConnection _connStr = DBConnection.GetConnection();
    int iMRSRID = 0;
    DataTable dt;
    DateTime tDate;

    private double runningTotal = 0;
    private double runningTotalTP = 0;
    private double runningTotalDis = 0;
    private double runningTotalWith = 0;
    private double runningTotalQty = 0;

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
        //btnSerch.Attributes.Add("OnClick", "return confirm_Add();");



    }

    protected void btnRegister_Click(object sender, EventArgs e)
    {
        if (System.Convert.ToInt32(Session["Vis"]) == 0)
        {
            Response.Redirect("../Account/Login.aspx");
        }

        SqlConnection conn = DBConnection.GetConnection();

        //String strQuery = "select DISTINCT Dist from tbDistThana Order By Dist";
        string gSql = "";
        gSql = gSql + " SELECT dbo.MRSRDetails.SLNO, dbo.Product.Model, dbo.MRSRDetails.ProductID,";
        gSql = gSql + " dbo.Product.ModelSerial, dbo.Product.ProdName, dbo.Product.GroupName, ";
        gSql = gSql + " dbo.Product.GroupSL";
        gSql = gSql + " FROM dbo.MRSRDetails LEFT OUTER JOIN";
        gSql = gSql + " dbo.Product ON dbo.MRSRDetails.ProductID = dbo.Product.ProductID";

        gSql = gSql + " WHERE (dbo.MRSRDetails.SLNO = '" + txtSL.Text + "')";

        gSql = gSql + " GROUP BY dbo.MRSRDetails.SLNO, dbo.Product.Model, dbo.MRSRDetails.ProductID,";
        gSql = gSql + " dbo.Product.ModelSerial, dbo.Product.ProdName, dbo.Product.GroupName, ";
        gSql = gSql + " dbo.Product.GroupSL";
        
        //SqlConnection con = new SqlConnection("conn");
        SqlCommand cmd = new SqlCommand(gSql, conn);
        //SqlCommand cmd = new SqlCommand();
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = gSql;
        cmd.Connection = conn;
        //try
        //{
            conn.Open();
            ddlModel.DataSource = cmd.ExecuteReader();
            ddlModel.DataTextField = "Model";
            //ddlModel.DataValueField = "ProductID";
            ddlModel.DataValueField = "ProductID";
            ddlModel.DataBind();

            //Add blank item at index 0.
            //ddlModel.Items.Insert(0, new ListItem("", ""));

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

    protected void txtSL_TextChanged(object sender, EventArgs e)
    {
        if (System.Convert.ToInt32(Session["Vis"]) == 0)
        {
            Response.Redirect("../Account/Login.aspx");
        }

        SqlConnection conn = DBConnection.GetConnection();

        //String strQuery = "select DISTINCT Dist from tbDistThana Order By Dist";
        string gSql = "";
        gSql = gSql + " SELECT dbo.MRSRDetails.SLNO, dbo.Product.Model, dbo.MRSRDetails.ProductID,";
        gSql = gSql + " dbo.Product.ModelSerial, dbo.Product.ProdName, dbo.Product.GroupName, ";
        gSql = gSql + " dbo.Product.GroupSL";
        gSql = gSql + " FROM dbo.MRSRDetails LEFT OUTER JOIN";
        gSql = gSql + " dbo.Product ON dbo.MRSRDetails.ProductID = dbo.Product.ProductID";

        gSql = gSql + " WHERE (dbo.MRSRDetails.SLNO = '" + txtSL.Text + "')";

        gSql = gSql + " GROUP BY dbo.MRSRDetails.SLNO, dbo.Product.Model, dbo.MRSRDetails.ProductID,";
        gSql = gSql + " dbo.Product.ModelSerial, dbo.Product.ProdName, dbo.Product.GroupName, ";
        gSql = gSql + " dbo.Product.GroupSL";

        //SqlConnection con = new SqlConnection("conn");
        SqlCommand cmd = new SqlCommand(gSql, conn);
        //SqlCommand cmd = new SqlCommand();
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = gSql;
        cmd.Connection = conn;
        try
        {
            conn.Open();
            ddlModel.DataSource = cmd.ExecuteReader();
            ddlModel.DataTextField = "Model";
            //ddlModel.DataValueField = "ProductID";
            ddlModel.DataValueField = "ProductID";
            ddlModel.DataBind();

            //Add blank item at index 0.
            //ddlModel.Items.Insert(0, new ListItem("", ""));

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


    protected void btnSerch_Click(object sender, EventArgs e)
    {
        //
        SqlConnection conn = DBConnection.GetConnection();

        int iCount = 0;
        this.Literal1.Text = "";

        conn.Open();

        string gSql = "";
        gSql = "SELECT dbo.MRSRDetails.SLNO, CONVERT(varchar(12), dbo.MRSRMaster.TDate, 105) AS TDate, ";
        gSql = gSql + " dbo.MRSRMaster.MRSRCode, dbo.MRSRDetails.ProductID, dbo.MRSRMaster.TrType, ";
        gSql = gSql +  " CASE dbo.MRSRMaster.TrType WHEN 1 THEN 'Receive' WHEN 2 THEN 'Issue' WHEN 3 THEN 'Sale'";
        gSql = gSql + " WHEN 4 THEN 'Transfer' WHEN -3 THEN 'Customer Withdrawn' END AS tStatus,";
        gSql = gSql +  " dbo.Entity.eName AS OutSource, Entity_1.eName AS InSource,";
        gSql = gSql +  " dbo.MRSRMaster.MRSRMID";
        gSql = gSql +  " FROM dbo.MRSRDetails INNER JOIN";
        gSql = gSql +  " dbo.MRSRMaster ON dbo.MRSRDetails.MRSRMID = dbo.MRSRMaster.MRSRMID INNER JOIN";
        gSql = gSql +  " dbo.Entity ON dbo.MRSRMaster.OutSource = dbo.Entity.EID INNER JOIN";
        gSql = gSql +  " dbo.Entity AS Entity_1 ON dbo.MRSRMaster.InSource = Entity_1.EID";
        
        gSql = gSql +  " WHERE  (dbo.MRSRDetails.SLNO = '" + txtSL.Text + "')";
        //gSql = gSql + " AND (dbo.MRSRDetails.ProductID = 5171)";
        gSql = gSql + " AND (dbo.MRSRDetails.ProductID = '" + ddlModel.SelectedItem.Value + "')";
        
        gSql = gSql +  " GROUP BY dbo.MRSRDetails.SLNO, dbo.MRSRMaster.TDate, dbo.MRSRMaster.MRSRCode,";
        gSql = gSql +  " dbo.MRSRDetails.ProductID, dbo.MRSRMaster.TrType, ";
        gSql = gSql + " dbo.Entity.eName, Entity_1.eName, Entity_1.eName, dbo.MRSRMaster.MRSRMID";

        SqlCommand cmd = new SqlCommand(gSql, conn);
        SqlDataReader dr2 = cmd.ExecuteReader();

        while (dr2.Read())
        {
            iCount = iCount + 1;
            this.Literal1.Text = this.Literal1.Text + @"<tr>";
            this.Literal1.Text = this.Literal1.Text + "<td>" + iCount + "</td>";
            this.Literal1.Text = this.Literal1.Text + "<td>" + dr2["TDate"] + "</td>";
            this.Literal1.Text = this.Literal1.Text + "<td>" + dr2["MRSRCode"] + "</td>";
            this.Literal1.Text = this.Literal1.Text + "<td>" + dr2["tStatus"] + "</td>";
            this.Literal1.Text = this.Literal1.Text + "<td>" + dr2["OutSource"] + "</td>";
            this.Literal1.Text = this.Literal1.Text + "<td>" + dr2["InSource"] + "</td>";
            this.Literal1.Text = this.Literal1.Text + "</tr>";
        }

        conn.Close();
        dr2.Close();

    }
}