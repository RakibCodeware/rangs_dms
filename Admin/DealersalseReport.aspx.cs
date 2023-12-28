using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;

public partial class DealerReports_DealersalseReport : System.Web.UI.Page
{
    SqlConnection conn = DBConnection.GetConnection();
    SqlConnection conn1 = DBConnection.GetConnection();
    DataTable dt;

    protected void Page_Load(object sender, EventArgs e)
    {


        if (!IsPostBack) 
         {
             DataBind();
         }
    }

    //check connection state 

    public void getConnection()
    {
        if (conn.State != null)
            conn.Close();
        conn.Open();
    }



    protected void btn_export_excel_Click(object sender, EventArgs e)
    {
        Export_Excel(gvdealerSalesReport);


    }

    //rendermethod control for export pdf and excel
    public override void VerifyRenderingInServerForm(Control control)
    {

    }


    //Export to Pdf Button
    protected void btn_export_pdf_Click(object sender, EventArgs e)
    {
        Export_pdf(gvdealerSalesReport);

    }

    private void Export_Excel(GridView gvExcel)
    {
        // for excel
        // Clear all content output from the buffer stream
        Response.ClearContent();
        // Specify the default file name using "content-disposition" RESPONSE header
        Response.AppendHeader("content-disposition", "attachment; filename=Download_Report.xls");
        // Set excel as the HTTP MIME type
        Response.ContentType = "application/excel";
        // Create an instance of stringWriter for writing information to a string
        System.IO.StringWriter stringWriter = new System.IO.StringWriter();
        // Create an instance of HtmlTextWriter class for writing markup 
        // characters and text to an ASP.NET server control output stream
        HtmlTextWriter htw = new HtmlTextWriter(stringWriter);

        // Set white color as the background color for gridview header row

        gvExcel.HeaderRow.Style.Add("background-color", "#FFFFFF");

        // Set background color of each cell of GridView1 header row
        foreach (TableCell tableCell in gvExcel.HeaderRow.Cells)
        {
            tableCell.Style["background-color"] = "#A55129";
        }

        // Set background color of each cell of each data row of GridView1
        foreach (GridViewRow gridViewRow in gvExcel.Rows)
        {
            gridViewRow.BackColor = System.Drawing.Color.White;
            foreach (TableCell gridViewRowTableCell in gridViewRow.Cells)
            {
                gridViewRowTableCell.Style["background-color"] = "#FFF7E7";
            }
        }

        gvExcel.RenderControl(htw);
        Response.Write(stringWriter.ToString());
        Response.End();
    }

    //Export to Pdf Oparetion Method
    private void Export_pdf(GridView gvPdf)
    {
        Response.ContentType = "aplication/ms-excel";
        Response.AddHeader("content-disposition",
        String.Format("attachment;filename={0}", "Download_Report.pdf"));
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        gvPdf.RenderControl(hw);

        gvPdf.HeaderRow.Style.Add("width", "15%");
        gvPdf.HeaderRow.Style.Add("font-size", "10px");


        Document doc = new Document(PageSize.A2, 40f, 30f, 40f, 40f);
        HTMLWorker htmlworker = new HTMLWorker(doc);
        PdfWriter.GetInstance(doc, Response.OutputStream);
        doc.Open();
        StringReader str = new StringReader(sw.ToString());
        htmlworker.Parse(str);
        doc.Close();
        Response.Write(doc);
        Response.End();
    }

    private void DataBind() 
     {
         getData();
     }

    protected void btn_preview_Click(object sender, EventArgs e) 
     {
         getData();
     }

    private void getData() 
     {
         try
         {
             getConnection();
             string query = "";
             query += @"select do.ZoneName,do.Note as Name,Entity.eName ShowroomName,di.Name as DelearName,MRSRMaster.MRSRCode InvNo--,MRSRDetails.MRSRMID
,MRSRMaster.TrType,convert(varchar, MRSRMaster.TDate, 23) Date,Product.Model,Product.GroupName,abs(MRSRDetails.Qty)Qty,MRSRDetails.MRP,MRSRDetails.UnitPrice,MRSRDetails.TotalAmnt,
MRSRDetails.DiscountAmnt,MRSRDetails.DisRef,MRSRDetails.ProdRemarks,MRSRMaster.Remarks,MRSRDetails.RedeemAmnt,MRSRDetails.WithAdjAmnt,MRSRDetails.IncentiveAmnt
,(dbo.MRSRDetails.NetAmnt) AS SalesAmnt

,MRSRMaster.NetSalesAmnt InvValue,(MRSRDetails.NetAmnt-MRSRDetails.RedeemAmnt )difference
--into #temp1  
from MRSRMaster 
inner join MRSRDetails on MRSRMaster.MRSRMID=MRSRDetails.MRSRMID 
inner join Entity on MRSRMaster.OutSource=Entity.EID 
inner join Product on Product.ProductID=MRSRDetails.ProductID 
inner join DelearSales.dbo.MRSRMaster dm on MRSRMaster.MRSRCode=dm.MRSRCode left join DelearSales.dbo.DelearInfo di on dm.InSource=di.DAID inner join ZonewisDealer zd on di.DAID=zd.Dealer inner join DealerOrganogram do on do.ZoneId=zd.Zone

where MRSRMaster.TrType in (3)  and Entity.ActiveDeactive=1 --and Entity.SalesOrShowroom in (0,1) 
 --and Entity.eName='BOGURA CTP'--and Product.Discontinue='No' 

and  MRSRMaster.TDate>='" + Convert.ToDateTime(this.txtFrom.Text) + @"' and  MRSRMaster.TDate<='" + Convert.ToDateTime(this.txtToDate.Text) + @"'  and Entity.SalesOrShowroom=2

order by MRSRMaster.TDate desc--,Entity.eName,MRSRMaster.MRSRCode ";


             dt = new DataTable();
             dt = ExecuteReturnDataTable(query, conn);
             gvdealerSalesReport.DataSource = dt;
             gvdealerSalesReport.DataBind();
              

         }
         catch (Exception ex)
         {
             
            
         }
         

     }





    public DataTable ExecuteReturnDataTable(string sqlCmd, SqlConnection _conn)
    {
        try
        {

            if (_conn == null)
            {
                _conn = new SqlConnection();
                _conn.Open();
            }
            else
            {
                if (_conn.State == ConnectionState.Open)
                    _conn.Close();
                _conn.Open();

            }



            DataTable dt = new DataTable();
            // cmd = new SqlCommand(sqlCmd, con);
            SqlDataAdapter da = new SqlDataAdapter(sqlCmd, _conn);
            //   da.SelectCommand.CommandTimeout =300;  // seconds
            da.SelectCommand.CommandTimeout = 0;  // seconds              
            da.Fill(dt);
            _conn.Close();
            return dt;
        }
        catch (Exception ex) { return null; }
    }

}