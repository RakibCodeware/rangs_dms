using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

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
            string[] path = HttpContext.Current.Request.Url.AbsolutePath.Split('/');
            string page = path[path.Length - 1].ToString();
            string[] pages = { "File_Uploadn.aspx", "discount_code.aspx" };
            if (pages.Contains(page))
                Response.Redirect("~/Management_Sales/Default_Administrator.aspx");

            this.lblUserName.Text = Session["UserName"].ToString();
            //this.lblCTP.Text = Session["eName"].ToString();
        }

    }


}
