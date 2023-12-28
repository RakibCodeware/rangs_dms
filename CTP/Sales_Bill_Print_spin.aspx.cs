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

public partial class Sales_Bill_Print_spin : System.Web.UI.Page
{
    SqlConnection conn = DBConnection.GetConnection();
    private ReportDocument report = new ReportDocument();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (System.Convert.ToInt32(Session["Vis"]) == 0)
            {
                Response.Redirect("../Default.aspx");
            }

            string reportPath = "";
            ConnectionInfo cnn = ReportDBConnectionSpin.GetConnection();
            //SqlConnection cnn = DBConnection.GetConnection();
            //crv.SelectionFormula = "{VW_Sales_BILL.MRSRCode} ='" + this.ddlContinents.SelectedValue + "'";
            crv.SelectionFormula = "{VW_Sales_BILL.ChNo} ='" + Session["sChNoSpin"].ToString() + "'";
            //rptDoc.Load(reportPath); 
            reportPath = Server.MapPath("Bill_N_spin.rpt");
            //SetDateRangeForOrders("",vDeclare.sDate,vDeclare.eDate);  
            //txtDate.Text = ro.i "Zunayed"; 
            

            crv.ReportSource = reportPath;
            SetDBLogonForReport(cnn);
            //--
        }
      
    }
    
    
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("Search_Sales_spin.aspx");
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