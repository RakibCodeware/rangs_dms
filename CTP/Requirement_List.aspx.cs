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

public partial class CTP_Requirement_List : System.Web.UI.Page
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

            this.txtFrom.Text = DateTime.Today.ToString("MM/dd/yyyy");
            this.txtToDate.Text = DateTime.Today.ToString("MM/dd/yyyy");
            
            //LOAD Data
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

        sSql = "SELECT  dbo.RequirmentMaster.ReqAID, dbo.RequirmentMaster.ReqNo, CONVERT(VARCHAR(10), dbo.RequirmentMaster.ReqDate, 103) AS ReqDate,";
        sSql = sSql + " dbo.Entity.eName, (CASE dbo.RequirmentMaster.Delivered WHEN 0 THEN 'Pending' ELSE 'Delivered' END) AS dStatus, ";
        sSql = sSql + " dbo.RequirmentMaster.ReqBy,";
        sSql = sSql + " dbo.RequirmentMaster.Remarks, SUM(dbo.RequirmentDetails.ReqQty) AS tQty";
        sSql = sSql + " FROM dbo.RequirmentMaster INNER JOIN";
        sSql = sSql + " dbo.RequirmentDetails ON dbo.RequirmentMaster.ReqAID = dbo.RequirmentDetails.ReqAID INNER JOIN";
        sSql = sSql + " dbo.Entity ON dbo.RequirmentMaster.EID = dbo.Entity.EID";

        sSql = sSql + " WHERE (dbo.RequirmentMaster.EID = '" + Session["sBrId"] + "')";
        sSql = sSql + " AND dbo.RequirmentMaster.ReqDate>='" + Convert.ToDateTime(this.txtFrom.Text) + "'";
        sSql = sSql + " AND dbo.RequirmentMaster.ReqDate<='" + Convert.ToDateTime(this.txtToDate.Text) + "'";

        if (RadioButtonList1.SelectedIndex == 1)
        {
            sSql = sSql + " AND (dbo.RequirmentMaster.Delivered = '0')";
        }
        else if (RadioButtonList1.SelectedIndex == 2)
        {
            sSql = sSql + " AND (dbo.RequirmentMaster.Delivered = '1')";
        }

        sSql = sSql + " GROUP BY dbo.RequirmentMaster.ReqAID, dbo.RequirmentMaster.ReqNo, dbo.RequirmentMaster.ReqDate,";
        sSql = sSql + " dbo.Entity.eName, dbo.RequirmentMaster.Delivered, dbo.RequirmentMaster.ReqBy, dbo.RequirmentMaster.Remarks";

        sSql = sSql + " ORDER BY dbo.RequirmentMaster.ReqDate DESC, dbo.RequirmentMaster.ReqNo DESC";

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
            Session["sReqNo"] = row.Cells[2].Text;
            Session["sReqDate"] = row.Cells[3].Text;
            //Session["sChFrom"] = row.Cells[4].Text;

            Response.Redirect("Requirement_List_Details.aspx");

        }

    }

    protected void SearchData(object sender, EventArgs e)
    {
        //LOAD DATA IN GRID
        fnLoadData();
    }


}