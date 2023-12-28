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

public partial class Report : System.Web.UI.Page
{
    SqlConnection conn = DBConnection.GetConnection();
    //private ReportDocument report = new ReportDocument();
    ReportDocument myReportDocument = new ReportDocument();
    ReportDocument oRpt = new ReportDocument();

    protected void Page_Load(object sender, EventArgs e)
    {
       
            string str = "";
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
            

            if (Session["ReportType"] == "RawData")
            {
                str = "";
                str = "SELECT * FROM VW_Sales_BILL";
                str = str + " WHERE VW_Sales_BILL.TDate>='" + Session["sDate"].ToString() + "'";
                str = str + " AND VW_Sales_BILL.TDate<='" + Session["eDate"].ToString() + "'";
                str = str + " AND VW_Sales_BILL.eName='" + Session["eName"].ToString() + "'";

                conn.Open();
                SqlCommand cmd = new SqlCommand(str, conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds, "VW_Sales_BILL");

                myReportDocument.Load(Server.MapPath("Reports/rptRawData.rpt"));

                myReportDocument.SetDataSource(ds);
                myReportDocument.SetParameterValue("pmCTPName", Session["eName"].ToString());
                myReportDocument.SetParameterValue("pmDate", Session["sDate"].ToString());
                //myReportDocument.SetParameterValue("add1", Session["CompA1"].ToString());

                myReportDocument.ReportOptions.Dispose();

                crv.ReportSource = myReportDocument;
                crv.DataBind();

            }
                  
              
    }
    
    
    protected void fnLoadSalesBill()
    {
        string reportPath = "";
        ConnectionInfo cnn = ReportDBConnection.GetConnection();
        //SqlConnection cnn = DBConnection.GetConnection();
        //crv.SelectionFormula = "{VW_Sales_BILL.MRSRCode} ='" + this.ddlContinents.SelectedValue + "'";
        crv.SelectionFormula = "{VW_Sales_BILL.MRSRCode} ='" + Session["sBillNo"].ToString() + "'";
        //rptDoc.Load(reportPath); 
        reportPath = Server.MapPath("Bill_N.rpt");
        //SetDateRangeForOrders("",vDeclare.sDate,vDeclare.eDate);  
        //txtDate.Text = ro.i "Zunayed"; 


        crv.ReportSource = reportPath;
        SetDBLogonForReport(cnn);
        //--
    }
    
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("Default_Administrator.aspx");
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