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

public partial class rawdata_print : System.Web.UI.Page
{
    SqlConnection conn = DBConnectionDSM.GetConnection();
    //private ReportDocument report = new ReportDocument();
    ReportDocument myReportDocument = new ReportDocument();
    ReportDocument oRpt = new ReportDocument();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string str = "";
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }



           // str = "";
           // str = "SELECT * FROM VW_RawData";
           // str = str + " WHERE VW_RawData.TDate>='" + Convert.ToDateTime(Session["sDate"].ToString()) + "'";
           // str = str + " AND VW_RawData.TDate<='" + Convert.ToDateTime(Session["eDate"].ToString()) + "'";
           // str = str + " AND VW_RawData.ZoneName='" + Session["sZoneName"].ToString() + "'";

           // conn.Open();
           // SqlCommand cmd = new SqlCommand(str, conn);
           // SqlDataAdapter da = new SqlDataAdapter(cmd);
           // DataSet ds = new DataSet();
           // da.Fill(ds, "VW_RawData");

           // myReportDocument.Load(Server.MapPath("~/Dealer/Reports/rptRawData.rpt"));

           // myReportDocument.SetDataSource(ds);
           //// myReportDocument.SetParameterValue("pmCTPName", Session["sZoneName"].ToString());
           // //myReportDocument.SetParameterValue("pmDate", Session["sDate"].ToString() + '-' + Session["eDate"].ToString());
           // //myReportDocument.SetParameterValue("add1", Session["CompA1"].ToString());

           // myReportDocument.ReportOptions.Dispose();

           // crv.ReportSource = myReportDocument;
           // crv.DataBind();
                
            //-------------------------------------------------------------------------

            string reportPath = "";
            ConnectionInfo cnn = ReportDBConnectionDSM.GetConnection();
            //SqlConnection cnn = DBConnection.GetConnection();
            //crv.SelectionFormula = "{VW_Sales_BILL.MRSRCode} ='" + this.ddlContinents.SelectedValue + "'";
            //crv.SelectionFormula = "{VW_RawData.ZoneName} ='" + Session["sZoneName"].ToString() + "' AND {VW_RawData.TDate} ='" + Convert.ToDateTime(Session["sDate"].ToString()) + "' ";
            crv.SelectionFormula = "{VW_RawData.TDate} ='" + Session["sDate"].ToString() + "'";
            //rptDoc.Load(reportPath); 
            reportPath = Server.MapPath("Reports/rptRawData.rpt");
            //SetDateRangeForOrders("",vDeclare.sDate,vDeclare.eDate);  
            //txtDate.Text = ro.i "Zunayed"; 


            crv.ReportSource = reportPath;
            SetDBLogonForReport(cnn);

            
        }
      
    }
    
    
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("frmSalesReport_admin.aspx");
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