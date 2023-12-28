using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.SessionState;

public partial class Admin : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (System.Convert.ToInt32(Session["Vis"]) == 0)
        {
            Response.Redirect("~/Default.aspx");
        }

        //------------------------------------------------------
        Session["init"] = 0;
        HttpSessionState ss = HttpContext.Current.Session;
        string sid = ss.SessionID;
        Session["sid"] = ss.SessionID;
        //------------------------------------------------------

        if (!IsPostBack)
        {
            string[] path = HttpContext.Current.Request.Url.AbsolutePath.Split('/');
            string page = path[path.Length - 1].ToString();
            string[] pages = { "Requirement.aspx", "Requirement_Edit.aspx", "Receive_New.aspx", "Sales_Edit.aspx", "Sales_New.aspx", "Transfer_New.aspx",
                                 "Transfer_Edit.aspx", "Withdrawn_New.aspx", "Withdrawn_New.aspx#","tag_share_promocode.aspx",
                             "File_Uploadn.aspx","Product_Info.aspx","product_update.aspx","Ctp_Info.aspx","VendorInfo.aspx","UserInfo.aspx","ChangePassword.aspx","discount_code.aspx"};
            if (pages.Contains(page))
                Response.Redirect("~/Admin/Default_Administrator.aspx");

            this.lblUserName.Text = Session["UserName"].ToString();
            //this.lblCTP.Text = Session["eName"].ToString();
        }

    }


}
