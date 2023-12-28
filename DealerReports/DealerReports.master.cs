using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class DealerReports_DealerReports : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        {
            if (System.Convert.ToInt32(Session["Vis"]) == 0)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!IsPostBack)
            {
                string[] path = HttpContext.Current.Request.Url.AbsolutePath.Split('/');
                string page = path[path.Length - 1].ToString();
                string[] pages = { "Organogram_Entryfrom.aspx", "DealerAsign.aspx", "PermisssionEntryPanel.aspx"};
                if (pages.Contains(page))
                    Response.Redirect("~/DealerReports/Default.aspx");

                this.lblUserName.Text = Session["UserName"].ToString();
                //this.lblCTP.Text = Session["eName"].ToString();
            }

        }

    }
}
