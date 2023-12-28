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

public partial class CTP_Receive_Cart_Details : System.Web.UI.Page
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
            //LOAD DATA IN GRID
            fnLoadData();
        }

    }

    protected void gvShoppingCart_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Remove")
        {
            int pId = int.Parse(e.CommandArgument.ToString());

            SqlConnection conn = DBConnection.GetConnection();

            HttpSessionState ss = HttpContext.Current.Session;
            string sid = ss.SessionID;

            String gSql = "";
            //gSql = "";
            //gSql = "DELETE FROM tbBaskets";
            //gSql = gSql + " WHERE ProductID=" + pId + "";
            ////gSql = gSql + " AND SessionNo='" + sid + "'";
            ////gSql = gSql + " AND TableNo=" + this.lblTableNo.Text + "";
            //gSql = gSql + " AND PrintTag=0";

            //SqlCommand cmd = new SqlCommand(gSql, conn);
            //conn.Open();
            //cmd.ExecuteNonQuery();
            //conn.Close();

            //GvReportResults.DeleteRow(id);
        }

        //LOAD DATA IN GRID
        //fnLoadData();

        
    }

    protected void fnLoadData()
    {
        if (System.Convert.ToInt32(Session["Vis"]) == 0)
        {
            Response.Redirect("../Account/Login.aspx");
        }

        SqlConnection conn = DBConnection.GetConnection();
        conn.Open();

        this.txtInvoiceNo.Text = Session["sChNo"].ToString();

        this.lblFrom.Text = Session["sChFrom"].ToString();
        this.lblInvNo.Text = Session["sChNo"].ToString();
        this.lblInvDate.Text = Session["sChDate"].ToString();

        HttpSessionState ss = HttpContext.Current.Session;
        string sid = ss.SessionID;

        string sSql = "";
       
        sSql = "SELECT  dbo.MRSRMaster.MRSRMID, dbo.MRSRMaster.MRSRCode, dbo.MRSRDetails.ProductID,";
        sSql = sSql + " dbo.Product.Code, dbo.Product.Model, dbo.Product.ProdName, dbo.MRSRDetails.Qty, dbo.MRSRDetails.SLNO";
        sSql = sSql + " FROM dbo.Product INNER JOIN";
        sSql = sSql + " dbo.MRSRDetails ON dbo.Product.ProductID = dbo.MRSRDetails.ProductID INNER JOIN";
        sSql = sSql + " dbo.MRSRMaster ON dbo.MRSRDetails.MRSRMID = dbo.MRSRMaster.MRSRMID";

        sSql = sSql + " WHERE dbo.MRSRMaster.MRSRCode='" + Session["sChNo"] + "'";

        sSql = sSql + " ORDER BY dbo.Product.Model";

        SqlCommand cmd = new SqlCommand(sSql, conn);
        //OleDbDataReader dr = cmd.ExecuteReader();

        DataSet ds = new DataSet();
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        da.Fill(ds);

        gvShoppingCart.DataSource = ds;
        gvShoppingCart.DataBind();
        //dr.Close();
        conn.Close();

    }



    protected void gvShoppingCart_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //CalcTotalQty(e.Row.Cells[3].Text);
            //CalcTotal_TP(e.Row.Cells[4].Text);

            //TextBox txt = (TextBox)sender;
            string tQty = ((TextBox)e.Row.FindControl("txtQuantity")).Text;
            Double totalQty = Convert.ToDouble(tQty);
            //e.Row.Cells[2].Text = totalQty.ToString();
            runningTotalQty += totalQty;

          
            // ALIGNMENT
            e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Left;
            e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Center;
            
            string item = e.Row.Cells[1].Text;
            foreach (Button button in e.Row.Cells[0].Controls.OfType<Button>())
            {
                if (button.CommandName == "Delete")
                {
                    button.Attributes["onclick"] = "if(!confirm('Do you want to remove " + item + "?')){ return false; };";
                }
            }
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[1].Text = "Total";
            //e.Row.Cells[3].Text = runningTotalQty.ToString("0,0", CultureInfo.InvariantCulture);
            //e.Row.Cells[4].Text = runningTotalTP.ToString("0,0", CultureInfo.InvariantCulture);

            Label lbl = (Label)e.Row.FindControl("lblTQty");
            lbl.Text = runningTotalQty.ToString();
            lblTItem.Text = runningTotalQty.ToString();
            lblTTQty.Text = runningTotalQty.ToString();

            
            //ALIGNMENT
            e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Left;
            e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Center;
            
        }
    }

    // TextBox Change Event.
    protected void txtQty_TextChanged(object sender, EventArgs e)
    {
        TextBox txt = (TextBox)sender;
        GridViewRow gvRow = (GridViewRow)txt.Parent.Parent;
        Label lblRs = (Label)gvRow.FindControl("lblRs");
        
        /*
        //________________________________________________________________________________
        //UPDATE BUSKET TABLE
        Label lblProdName = (Label)gvRow.FindControl("lblPName");

        SqlConnection conn = DBConnection.GetConnection();
        SqlConnection conn1 = DBConnection.GetConnection();

        HttpSessionState ss = HttpContext.Current.Session;
        string sid = ss.SessionID;

        int iQty = 0;
        String gSql = "";
        gSql = "";
        gSql = "SELECT ProductID,ProductName,tQty FROM tbBaskets";
        gSql = gSql + " WHERE ProductName='" + lblProdName.Text + "'";
        //gSql = gSql + " AND SessionNo='" + sid + "'";
        //gSql = gSql + " AND TableNo=" + this.lblTableNo.Text + "";
        gSql = gSql + " AND PrintTag=0";

        SqlCommand cmd = new SqlCommand(gSql, conn);
        conn.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            //iQty = Convert.ToInt16(dr["tQty"].ToString()) + 1;
            iQty = Convert.ToInt32(txt.Text);
            //pID = Convert.ToInt16(dr["ProductID"].ToString());

            //TEMP INSERT 
            gSql = "";
            gSql = "Update tbBaskets SET ";
            gSql = gSql + " tQty=" + iQty + "";
            //gSql = gSql + " Where SessionNo='" + sid + "'";
            gSql = gSql + " Where ProductName='" + lblProdName.Text + "'";
            //gSql = gSql + " AND TableNo=" + this.lblTableNo.Text + "";

            cmd = new SqlCommand(gSql, conn1);
            conn1.Open();
            cmd.ExecuteNonQuery();
            conn1.Close();

        }
        //________________________________________________________________________________

         
         */

        //LOAD DATA IN GRID
        fnLoadData();

       
    }



    protected void SaveData(object sender, EventArgs e)
    {
        //
    }
    protected void btnDenyCall(object sender, EventArgs e)
    {
        //
    }
    protected void btnBackCall(object sender, EventArgs e)
    {
        Response.Redirect("Receive_CartList.aspx");
    }

}