using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_Report : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //LoginName UserName = (LoginName)HeadLoginView.FindControl("HeadLoginName");
        // UserName.Text = Session["UserName"].ToString();

        //var welcomeLabel = Page.Master.FindControl("UserName") as Label;
        // welcomeLabel.Text = Session["UserName"].ToString();

        if (!IsPostBack)
        {            
            lblUser.Text = Session["UserName"].ToString();
            lblCTP.Text = Session["eName"].ToString();
        }
    }
}
