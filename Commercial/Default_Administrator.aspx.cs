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
using System.Globalization;
using System.Text.RegularExpressions;

public partial class Default_Administrator : System.Web.UI.Page
{

    string currentMonth1 = DateTime.Now.Month.ToString();
    string currentYear1 = DateTime.Now.Year.ToString();
    SqlDataReader dr;

    int FYs, FYe;
    DateTime sFYs, sFYe, sDate, eDate;

    private double runningTotalQtyCat = 0;
    private double runningTotalAmntCat = 0;
    private double runningTotalQtyM = 0;
    private double runningTotalAmntM = 0;
    private double runningTotalQtyB = 0;
    private double runningTotalAmntB = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (System.Convert.ToInt32(Session["Vis"]) == 0)
        {
            //Response.Redirect("~/Default.aspx");
            Response.Redirect("../Account/Login.aspx");
        }

        if (!IsPostBack)
        {

            int monthC = Convert.ToInt16(currentMonth1);
            int yearC = Convert.ToInt16(currentYear1);

            if (monthC >= 7)
            {
                FYs = yearC;
                FYe = yearC + 1;
            }
            else
            {
                FYs = yearC - 1;
                FYe = yearC;
            }

            sFYs = Convert.ToDateTime("7/1/" + FYs + "");
            sFYe = Convert.ToDateTime("6/30/" + FYe + "");

            //this.lblText.Text = "Welcome to " + Session["eName"].ToString() + " ... FY : " + FYs + "-" + FYe + "";
            this.lblText.Text = "Welcome to Rangs Electronics Ltd ... FY : " + FYs + "-" + FYe + "";
            //this.lblCTP.Text = Session["eName"].ToString();                       

            
            
            //--------------------------------------------------------

            
                                   

        }
    }







}