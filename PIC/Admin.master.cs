using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web.SessionState;

public partial class Admin : System.Web.UI.MasterPage
{
    
    public string labeltext1
    {
        get
        {
            return lblTItem.Text;
        }
        set
        {
            lblTItem.Text = value;
        }
    }



    protected void Page_Load(object sender, EventArgs e)
    {
        if (System.Convert.ToInt32(Session["Vis"]) == 0)
        {
            Response.Redirect("~/Default.aspx");
        }

        if (!IsPostBack)
        {

            string[] path = HttpContext.Current.Request.Url.AbsolutePath.Split('/');
            string page = path[path.Length - 1].ToString();
            string[] pages = { "VendorInfo.aspx", "product_update.aspx", "FOB_Entry.aspx", "File_Uploadn.aspx" };
            if (pages.Contains(page))
                Response.Redirect("~/PIC/Default_Administrator.aspx");


            this.lblUserName.Text = Session["UserName"].ToString();
            //this.lblCTP.Text = Session["eName"].ToString();

            //LOAD CART ITEMS
            //fnLoadCartItems();

            //LOAD INBOX CHALLAN COUNT
            //fnLoadChallanCount();

        }

    }


    protected void fnLoadCartItems()
    {
        String gSql = "";
        SqlConnection conn = DBConnection.GetConnection();

        HttpSessionState ss = HttpContext.Current.Session;
        string sid = ss.SessionID;

        //SHOW IN TOTAL CART IMTEMS
        gSql = "";
      
        gSql="SELECT dbo.MRSRMaster.InSource, SUM(dbo.MRSRDetails.Qty) AS tQty";
        gSql = gSql + " FROM dbo.MRSRMaster INNER JOIN";
        gSql = gSql + " dbo.MRSRDetails ON dbo.MRSRMaster.MRSRMID = dbo.MRSRDetails.MRSRMID";
        gSql = gSql + " WHERE (dbo.MRSRMaster.TrType = 2) AND (dbo.MRSRMaster.Tag = 2)";
        gSql = gSql + " GROUP BY dbo.MRSRMaster.InSource";
        gSql = gSql + " HAVING (dbo.MRSRMaster.InSource = '" + Session["sBrId"] + "')";
        
        SqlCommand cmd = new SqlCommand(gSql, conn);
        conn.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            lblTItem.Text = "My Cart (" + dr["tQty"].ToString() + ")";
        }
        else
        {
            lblTItem.Text = "My Cart (0)";
        }
        dr.Dispose();
        dr.Close();
        conn.Close();

    }

    protected void fnLoadChallanCount()
    {
        String gSql = "";
        SqlConnection conn = DBConnection.GetConnection();

        HttpSessionState ss = HttpContext.Current.Session;
        string sid = ss.SessionID;

        //SHOW IN TOTAL CART IMTEMS
        gSql = "";

        gSql = "SELECT COUNT(MRSRCode) AS tCH";
        gSql = gSql + " FROM dbo.MRSRMaster";        
        gSql = gSql + " WHERE (dbo.MRSRMaster.TrType = 2) AND (dbo.MRSRMaster.Tag = 2)";        
        gSql = gSql + " AND (dbo.MRSRMaster.InSource = '" + Session["sBrId"] + "')";

        SqlCommand cmd = new SqlCommand(gSql, conn);
        conn.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            //lblInbox.Text = "(" + dr["tCH"].ToString() + ")";
        }
        else
        {
            //lblInbox.Text = "(0)";
        }
        dr.Dispose();
        dr.Close();
        conn.Close();

    }


}
