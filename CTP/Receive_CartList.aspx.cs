using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.IO;
using System.Text;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web.SessionState;

public partial class CTP_Receive_CartList : System.Web.UI.Page
{
    private double runningTotalTP = 0;
    private double runningTotalQty = 0;
    long i;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (System.Convert.ToInt32(Session["Vis"]) == 0)
        {
            Response.Redirect("../Account/Login.aspx");
        }

        if (!IsPostBack)
        {            
            //fnLoadCombo_Item(ddlProduct, "BrandName", "BrandID", "tbBrand");

            //LOAD DATA IN GRID
            fnLoadData();
                        
        }

    }

    protected void fnLoadData()
    {
        if (System.Convert.ToInt32(Session["Vis"]) == 0)
        {
            Response.Redirect("../Account/Login.aspx");
        }

        SqlConnection conn = DBConnection.GetConnection();
        conn.Open();

        string sSql = "";

        sSql = "SELECT  dbo.MRSRMaster.MRSRMID, dbo.MRSRMaster.MRSRCode, CONVERT(VARCHAR(10), dbo.MRSRMaster.TDate, 103) AS TDate,";
        sSql = sSql + " dbo.Entity.eName,  dbo.MRSRMaster.Remarks, SUM(dbo.MRSRDetails.Qty) AS tQty";
        sSql = sSql + " FROM dbo.MRSRMaster INNER JOIN";
        sSql = sSql + " dbo.MRSRDetails ON dbo.MRSRMaster.MRSRMID = dbo.MRSRDetails.MRSRMID INNER JOIN";
        sSql = sSql + " dbo.Entity ON dbo.MRSRMaster.OutSource = dbo.Entity.EID";

        sSql = sSql + " WHERE (dbo.MRSRMaster.TrType = 2) AND (dbo.MRSRMaster.Tag = 2)";
        sSql = sSql + " AND (dbo.MRSRMaster.InSource = '" + Session["sBrId"] + "')";

        sSql = sSql + " GROUP BY dbo.MRSRMaster.MRSRMID, dbo.MRSRMaster.MRSRCode, dbo.MRSRMaster.TDate,";
        sSql = sSql + " dbo.Entity.eName, dbo.MRSRMaster.Remarks";

        sSql = sSql + " ORDER BY dbo.MRSRMaster.TDate DESC, dbo.MRSRMaster.MRSRCode DESC";

        SqlCommand cmd = new SqlCommand(sSql, conn);
        //OleDbDataReader dr = cmd.ExecuteReader();

        DataSet ds = new DataSet();
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        da.Fill(ds);

        GridView1.DataSource = ds;
        GridView1.DataBind();
        //dr.Close();
        conn.Close();
    }

    protected void OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        GridView1.DataBind();
        fnLoadData();
    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //e.Row.Cells[1].Visible = false;
        }

    }

    protected void PopupMessage(string Msg, Control controlID)
    {
        ScriptManager.RegisterClientScriptBlock(controlID, controlID.GetType(), "msg", "alert('" + Msg + "');", true);
    }

    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "ShoppingCart")
        {
            SqlConnection conn = DBConnection.GetConnection();                                   

            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = GridView1.Rows[index];

            string sChNo = row.Cells[2].Text;
            Session["sChNo"] = row.Cells[2].Text;
            Session["sChDate"] = row.Cells[3].Text;
            Session["sChFrom"] = row.Cells[4].Text;

            Response.Redirect("Receive_Cart_Details.aspx");
            
        }

    }

}