using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Globalization;

using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Text;
using System.Globalization;
using System.Text.RegularExpressions;

using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports;

public partial class Search_Sales_info : System.Web.UI.Page
{
    //SqlConnection conn = DBConnection.GetConnection();
    SqlConnection conn = DBConnection_Log.GetConnection();
    //private ReportDocument report = new ReportDocument();
    ReportDocument myReportDocument = new ReportDocument();
    ReportDocument oRpt = new ReportDocument();
    DataTable dt = new DataTable();

    long i;

    private double runningTotalQty = 0;
    private double runningTotalTP = 0;
    private double runningTotalCash = 0;
    private double runningTotalCard = 0;

    int FYs, FYe;
    DateTime sFYs, sFYe, sDate, eDate;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (System.Convert.ToInt32(Session["Vis"]) == 0)
        {
            Response.Redirect("../Account/Login.aspx");
        }

        if (!IsPostBack)
        {
            this.txtFrom.Text = DateTime.Today.ToString("MM/dd/yyyy");
            this.txtToDate.Text = DateTime.Today.ToString("MM/dd/yyyy");

            /*
            DateTime date = DateTime.Today;
            var fDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var lDayOfMonth = fDayOfMonth.AddMonths(1).AddDays(-1);

            sDate = Convert.ToDateTime(fDayOfMonth);
            eDate = Convert.ToDateTime(lDayOfMonth);
            
            this.txtFrom.Text = sDate.ToString("MM/dd/yyyy");
            this.txtFrom.Text = DateTime.Today.ToString("MM/dd/yyyy");
            */

            //LOAD CTP NAME
            //LoadDropDownList_CTP();

            //LOAD USER NAME
            LoadDropDownList_User();
                        
        }                    
        

    }

    //LOAD CTP IN DROPDOWN LIST
    protected void LoadDropDownList_User()
    {
        SqlConnection conn = DBConnection.GetConnection();

        String strQuery = "select ID, UserName from SoftUser ";
        strQuery = strQuery + " WHERE (Active = 1)";
        strQuery = strQuery + " ORDER BY UserName";
        //SqlConnection con = new SqlConnection("conn");
        SqlCommand cmd = new SqlCommand(strQuery, conn);
        //SqlCommand cmd = new SqlCommand();
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = strQuery;
        cmd.Connection = conn;
        try
        {
            conn.Open();
            ddlUser.DataSource = cmd.ExecuteReader();
            ddlUser.DataTextField = "UserName";
            ddlUser.DataValueField = "ID";
            ddlUser.DataBind();

            //Add blank item at index 0.
            ddlUser.Items.Insert(0, new ListItem("ALL", "0"));

        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            conn.Close();
            conn.Dispose();
        }
    }

    
    protected void PopupMessage(string Msg, Control controlID)
    {
        ScriptManager.RegisterClientScriptBlock(controlID, controlID.GetType(), "msg", "alert('" + Msg + "');", true);
    }

    protected void SearchData(object sender, EventArgs e)
    {
        //LOAD DATA IN GRID
        //fnLoadData();

        //LOAD Report
        //fnLoadReport();

        DateTime eDate = Convert.ToDateTime(txtToDate.Text).AddDays(1);
        string ssDate = "(" + txtFrom.Text + " To " + txtToDate.Text + ")";

        Session["lSDate"] = Convert.ToDateTime(txtFrom.Text);
        Session["lEDate"] = Convert.ToDateTime(txtToDate.Text).AddDays(1);
        Session["ssDate"] = "(" + txtFrom.Text + " To " + txtToDate.Text + ")";
        Session["slUser"] = this.ddlUser.SelectedItem.Text;

        Response.Redirect("print.aspx");

    }

    private void fnLoadReport()
    {
        
        if (conn.State == ConnectionState.Open)
        {
            conn.Close();
        }

        string str = "";
        string ssDate = "(" + txtFrom.Text + " To " + txtToDate.Text + ")";

        DateTime eDate = Convert.ToDateTime(txtToDate.Text).AddDays(1);

        str = "";
        str = "SELECT SName, PSName, PPSName, LogInTime,";
        str = str + " LogOutTime, UserName, PCName, LogIn, LogOut ";
        str = str + " FROM VW_User_Log_Information";
        str = str + " WHERE LogInTime>='" + txtFrom.Text + "'";
        str = str + " AND LogInTime<='" + eDate + "'";

        if (this.ddlUser.SelectedItem.Text != "ALL")
        {
            str = str + " AND UserName='" + this.ddlUser.SelectedItem.Text + "'";
        }

        conn.Open();
        SqlCommand cmd = new SqlCommand(str, conn);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();        
        dsLogReport1 ds1 = new dsLogReport1(); 

        //da.Fill(ds, "VW_User_Log_Information");
        da.Fill(dt);
        ds1.Tables[0].Merge(dt);
        
        myReportDocument.Load(Server.MapPath("Reports/rptLogReport.rpt"));

        //myReportDocument.SetDataSource(ds);
        myReportDocument.SetDataSource(ds1);
        myReportDocument.SetParameterValue("pmCTPName", ddlUser.SelectedItem.Text);
        myReportDocument.SetParameterValue("pmDate", ssDate);
        //myReportDocument.SetParameterValue("add1", Session["CompA1"].ToString());

        myReportDocument.ReportOptions.Dispose();

        crv.ReportSource = myReportDocument;
        crv.DataBind();
    }

    private void SetDBLogonForReport(ConnectionInfo connectionInfo)
    {
        TableLogOnInfos tableLogOnInfos = crv.LogOnInfo;
        foreach (TableLogOnInfo tableLogOnInfo in tableLogOnInfos)
        {
            tableLogOnInfo.ConnectionInfo = connectionInfo;
        }
    }

    protected void Page_UnLoad(object sender, EventArgs e)
    {
        this.crv.Dispose();
        this.crv = null;
        // CrystalReportViewer1.Close();
        // CrystalReportViewer1.Dispose();
        GC.Collect();
    }
        

}