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

public partial class Forms_Sales_Bill_Print1 : System.Web.UI.Page
{
    SqlConnection conn = DBConnection.GetConnection();
    private ReportDocument report = new ReportDocument();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (System.Convert.ToInt32(Session["Vis"]) == 0)
            {
                Response.Redirect("../Account/Login.aspx");
            }

            LoadDropDownList();
        }
    }

    //LOAD PRODUCT IN DROPDOWN LIST
    protected void LoadDropDownList()
    {
        SqlConnection conn = DBConnection.GetConnection();
        String strQuery = "select MRSRCode from MRSRMaster" +
            " WHERE TrType=3 AND TDate>'1/1/2015'" +
            " Order By MRSRCode";
        //SqlConnection con = new SqlConnection("conn");
        SqlCommand cmd = new SqlCommand(strQuery, conn);
        //SqlCommand cmd = new SqlCommand();
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = strQuery;
        cmd.Connection = conn;
        try
        {
            conn.Open();
            ddlContinents.DataSource = cmd.ExecuteReader();
            ddlContinents.DataTextField = "MRSRCode";
            //ddlContinents.DataValueField = "ProductID";
            ddlContinents.DataValueField = "MRSRCode";
            ddlContinents.DataBind();
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


    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Forms/Sales_New.aspx");
    }
    protected void btnPrintClick(object sender, EventArgs e)
    {
        //fnDataPrint();

        string reportPath = "";
        ConnectionInfo cnn = ReportDBConnection.GetConnection();
        //SqlConnection cnn = DBConnection.GetConnection();
        crv.SelectionFormula = "{VW_Sales_BILL.MRSRCode} ='" + this.ddlContinents.SelectedValue + "'";
        //rptDoc.Load(reportPath); 
        reportPath = Server.MapPath("~/Reports/Bill_N.rpt");
        //SetDateRangeForOrders("",vDeclare.sDate,vDeclare.eDate);  
        //txtDate.Text = ro.i "Zunayed"; 

        crv.ReportSource = reportPath;
        SetDBLogonForReport(cnn);
        //--

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