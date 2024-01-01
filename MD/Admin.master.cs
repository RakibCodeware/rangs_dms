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

using System.Text.RegularExpressions;
using System.Web.SessionState;

public partial class Admin : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (System.Convert.ToInt32(Session["Vis"]) == 0)
        {
            Response.Redirect("~/Default.aspx");
        }

        if (!IsPostBack)
        {

            if (DateTime.Now > StaticInfo.FrezeeDmsEntryTime)
            {
                string[] path = HttpContext.Current.Request.Url.AbsolutePath.Split('/');
                string page = path[path.Length - 1].ToString();
                string[] pages = { "discount_code.aspx"};
                if (pages.Contains(page))
                    Response.Redirect("~/MD/Default_Administrator.aspx");
            }

            this.lblUserName.Text = Session["UserName"].ToString();
            //this.lblCTP.Text = Session["eName"].ToString();

            fnLoadAppListCount();

        }

    }


    protected void fnLoadAppListCount()
    {
        String gSql = "";
        SqlConnection conn = DBConnection.GetConnection();

        HttpSessionState ss = HttpContext.Current.Session;
        string sid = ss.SessionID;

        //SHOW IN TOTAL CART IMTEMS
        gSql = "";

        gSql = "SELECT COUNT(RAID) AS tCH";
        gSql = gSql + " FROM dbo.tbRequest";
        gSql = gSql + " WHERE (GrantDeny = 0)";
        //gSql = gSql + " AND (dbo.MRSRMaster.DeliveryFrom = '" + Session["sBrId"] + "')";

        SqlCommand cmd = new SqlCommand(gSql, conn);
        conn.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            lblInbox.Text = "(" + dr["tCH"].ToString() + ")";
        }
        else
        {
            lblInbox.Text = "(0)";
        }
        dr.Dispose();
        dr.Close();
        conn.Close();

    }


}
