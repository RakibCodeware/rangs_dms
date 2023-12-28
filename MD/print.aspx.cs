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

using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports;

public partial class MD_print : System.Web.UI.Page
{
    SqlConnection conn = DBConnection_Log.GetConnection();
    private ReportDocument report = new ReportDocument();

    ReportDocument myReportDocument = new ReportDocument();
    ReportDocument oRpt = new ReportDocument();
    DataTable dt = new DataTable();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (System.Convert.ToInt32(Session["Vis"]) == 0)
            {
                Response.Redirect("../Account/Login.aspx");
            }
        }
            string reportPath = "";
            string str = "";
            //string ssDate = "(" + txtFrom.Text + " To " + txtToDate.Text + ")";

            //DateTime eDate = Convert.ToDateTime(txtToDate.Text).AddDays(1);

            str = "";
            str = "SELECT SName, PSName, PPSName, LogInTime,";
            str = str + " LogOutTime, UserName, PCName, LogIn, LogOut ";
            str = str + " FROM VW_User_Log_Information";
            str = str + " WHERE LogInTime>='" + Session["lSDate"] + "'";
            str = str + " AND LogInTime<='" + Session["lEDate"] + "'";

            if (Session["slUser"] != "ALL")
            {
                str = str + " AND UserName='" + Session["slUser"].ToString() + "'";
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
            myReportDocument.SetParameterValue("pmCTPName", Session["slUser"].ToString());
            myReportDocument.SetParameterValue("pmDate", Session["ssDate"].ToString());
            //myReportDocument.SetParameterValue("add1", Session["CompA1"].ToString());

            myReportDocument.ReportOptions.Dispose();

            crv.ReportSource = myReportDocument;
            crv.DataBind();

            /*
            ConnectionInfo cnn = ReportDBConnection.GetConnection();
            //SqlConnection cnn = DBConnection.GetConnection();
            //crv.SelectionFormula = "{VW_Sales_BILL.MRSRCode} ='" + this.ddlContinents.SelectedValue + "'";
            //crv.SelectionFormula = "{VW_Sales_BILL.MRSRCode} ='" + Session["sBillNo"].ToString() + "'";

            crv.SelectionFormula = str;

            //rptDoc.Load(reportPath); 
            reportPath = Server.MapPath("Reports/rptLogReport.rpt");
            //SetDateRangeForOrders("",vDeclare.sDate,vDeclare.eDate);  
            //txtDate.Text = ro.i "Zunayed"; 


            crv.ReportSource = reportPath;
            SetDBLogonForReport(cnn);
            //--
             */ 
        

    }


    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("Search_LogReport.aspx");
    }

    protected void btnPrintClick(object sender, EventArgs e)
    {
        //fnDataPrint();               

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