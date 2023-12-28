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

public partial class Sales_Delivery_List : System.Web.UI.Page
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
      
        sSql = " SELECT dbo.MRSRMaster.MRSRMID, dbo.MRSRMaster.MRSRCode,";
        sSql = sSql + " CONVERT(varchar(12), dbo.MRSRMaster.TDate, 105) AS TDate,";
        sSql = sSql + " dbo.MRSRMaster.TrType, dbo.Entity.eName AS SalesFrom, ";
        sSql = sSql + " Entity_1.eName AS DelFrom, dbo.MRSRMaster.OnLineSales, dbo.MRSRMaster.Remarks,";
        sSql = sSql + " dbo.Customer.CustName, dbo.Customer.Mobile,";
        sSql = sSql + " SUM(abs(dbo.MRSRDetails.Qty)) AS tQty, SUM(dbo.MRSRDetails.NetAmnt) AS tAmnt";

        sSql = sSql + " FROM dbo.MRSRMaster INNER JOIN";
        sSql = sSql + " dbo.MRSRDetails ON dbo.MRSRMaster.MRSRMID = dbo.MRSRDetails.MRSRMID INNER JOIN";
        sSql = sSql + " dbo.Product ON dbo.MRSRDetails.ProductID = dbo.Product.ProductID INNER JOIN";
        sSql = sSql + " dbo.Entity ON dbo.MRSRMaster.OutSource = dbo.Entity.EID INNER JOIN";
        sSql = sSql + " dbo.Entity AS Entity_1 ON dbo.MRSRMaster.DeliveryFrom = Entity_1.EID";
        sSql = sSql + " LEFT OUTER JOIN dbo.Customer ON dbo.MRSRMaster.Customer = dbo.Customer.Mobile";

        sSql = sSql + " WHERE (dbo.MRSRMaster.OnLineSales = 1) AND (dbo.MRSRMaster.TrType = 3)";
        sSql = sSql + " AND (dbo.MRSRMaster.DeliveryFrom = '" + Session["sBrId"] + "')";
        sSql = sSql + " AND (dbo.MRSRMaster.Tag = 2)";

        sSql = sSql + " GROUP BY dbo.MRSRMaster.MRSRMID, dbo.MRSRMaster.MRSRCode, dbo.MRSRMaster.TDate,";
        sSql = sSql + " dbo.MRSRMaster.TrType, dbo.Entity.eName, Entity_1.eName, ";
        sSql = sSql + " dbo.MRSRMaster.OnLineSales, dbo.MRSRMaster.Remarks,";
        sSql = sSql + " dbo.Customer.CustName, dbo.Customer.Mobile";
                
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
            Session["sDelInvNo"] = row.Cells[2].Text;
            Session["sDelInvDate"] = row.Cells[3].Text;
            Session["sDelInvFrom"] = row.Cells[4].Text;

            Response.Redirect("Sales_Del_Details.aspx");
            
        }

    }

}