using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;


public partial class DealerReports_IndividualDealerStatementReport : System.Web.UI.Page
{


    SqlConnection conn = DBConnection.GetConnection();

    SqlConnection conn1 = DBConnectionDSM.GetConnection();
    SqlConnection conn2 = DBConnectionDSM.GetConnection();
    DataTable dt;



    private double runningTotal = 0;
    private double runningTotalTP = 0;
    private double runningTotalDis = 0;
    private double runningTotalWith = 0;
    private double runningTotalQty = 0;

    private double runningTotalCash = 0;
    private double runningTotalCard = 0;


    public void getConnectiondealer()
    {
        if (conn1.State != null)
            conn1.Close();
        conn1.Open();
    }

    public void getConnectiondealer2()
    {
        if (conn2.State != null)
            conn2.Close();
        conn2.Open();
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) 
        {
            Dealerfill();
            getDealerName();
            //BindData();
        }
    }



    public object getDealerName()
    {
        if (conn.State != null)
            conn.Close();
        conn.Open();
        string sqlqury = "select Name as Name from DelearSales.dbo.DelearInfo ";
        SqlCommand cmd = new SqlCommand(sqlqury, conn);
        SqlDataAdapter adaptr = new SqlDataAdapter(cmd);
        DataTable dte = new DataTable();
        adaptr.Fill(dte);
        return dte;
        conn.Close();
    }


    private void Dealerfill()
    {
        ddlDealerName.DataSource = getDealerName();
        ddlDealerName.DataTextField = "Name";
        ddlDealerName.DataBind();
        ddlDealerName.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", "-1"));

    }

    protected void btn_export_excel_Click(object sender, EventArgs e)
    {
        Export_Excel(gvSales);
    }

    //rendermethod control for export pdf and excel
    public override void VerifyRenderingInServerForm(Control control)
    {

    }


    //Export to Pdf Button
    protected void btn_export_pdf_Click(object sender, EventArgs e)
    {
        Export_pdf(gvSales);
       

    }

   


    protected void btn_preview_Click(object sender, EventArgs e)
    {
        reportCompanyanme.InnerText = "Rangs Electronics Limited";
        hcompanyAdreess.InnerText = "Sonartori tower(4th floor,12 Sonargaon Road)";
        hcompanyrodadress.InnerText = "Dhaka-1000 Bangladesh";
        hReportsDateRange.InnerText = "(" + txtFrom.Text.Trim() + " To " + txtToDate.Text.Trim() + ")";
        dealername.InnerText = "(" + ddlDealerName.Text + ")";

        lblOBSales.Text = "0";
        lblOBCollection.Text = "0";
        lblOBDis.Text = "0";
        lblOBWith.Text = "0";
        CurrentData();
        FnloadOpeningdata();

        double dOutStanding = Convert.ToDouble(lblOBSales.Text) - Convert.ToDouble(lblOBCollection.Text) + Convert.ToDouble(lblOBDis.Text) - Convert.ToDouble(lblOBWith.Text);
        lblOb.Text = Convert.ToString(dOutStanding);

        if (lblSalesAmount.Text.Length == 0)
        {
            lblSalesAmount.Text = "0";
        }
        if (lblDeposit.Text.Length == 0)
        {
            lblDeposit.Text = "0";
        }
        if (lblWithdrawn.Text.Length == 0)
        {
            lblWithdrawn.Text = "0";
        }

        double dCB = dOutStanding + Convert.ToDouble(lblSalesAmount.Text) - Convert.ToDouble(lblDeposit.Text) - Convert.ToDouble(lblWithdrawn.Text);
        lblClosing.Text = Convert.ToString(dCB);


    }


    private void BindData() 
    {
        CurrentData();
    }







    private void CurrentData()
    {
        lblSalesAmount.Text = "0";
        lblDeposit.Text = "0";
        lblWithdrawn.Text = "0";

        string SqlQuery = "";
        SqlQuery = "SELECT dbo.MRSRMaster.MRSRCode, CONVERT(varchar(12), dbo.MRSRMaster.TDate, 101) AS TDate, dbo.MRSRMaster.TrType, dbo.VW_Delear_Info.Name AS InSource, dbo.VW_Delear_Info.ZoneName, dbo.Zone.CatName AS OutSource,  dbo.VW_Delear_Info.Address, dbo.VW_Delear_Info.DealerStatus, dbo.VW_Delear_Info.ContactNo, dbo.VW_Delear_Info.ContactPerson, dbo.VW_Delear_Info.Code, dbo.VW_Delear_Info.DAID,  dbo.VW_Delear_Info.EmailAdd, dbo.VW_Delear_Info.ZoneType, dbo.MRSRMaster.POCode, dbo.MRSRMaster.OnLineSales, dbo.MRSRMaster.TermsCondition, dbo.MRSRMaster.Remarks,  dbo.MRSRMaster.MRSRMID, dbo.MRSRMaster.NetSalesAmnt, dbo.MRSRMaster.SaleDeclar, CASE dbo.MRSRMaster.SaleDeclar WHEN 1 THEN 'Declared' ELSE 'Pending' END AS sStatus FROM dbo.VW_Delear_Info INNER JOIN dbo.MRSRMaster ON dbo.VW_Delear_Info.DAID = dbo.MRSRMaster.InSource INNER JOIN dbo.Zone ON dbo.MRSRMaster.OutSource = dbo.Zone.CategoryID WHERE (dbo.MRSRMaster.TrType = 3) and dbo.MRSRMaster.NetSalesAmnt>0 AND (dbo.MRSRMaster.TDate>='" + Convert.ToDateTime(this.txtFrom.Text) + "' AND dbo.MRSRMaster.TDate<='" + Convert.ToDateTime(this.txtToDate.Text) + "') AND (dbo.VW_Delear_Info.Name ='" + ddlDealerName.Text + "') ORDER BY dbo.MRSRMaster.TDate, dbo.MRSRMaster.MRSRCode DESC";

        dt = new DataTable();
        dt = ExecuteReturnDataTable(SqlQuery, conn1);
        gvSales.DataSource = dt;
        gvSales.DataBind();
        conn1.Close();



        SqlQuery = @"SELECT dbo.MRSRMaster.MRSRCode, CONVERT(varchar(12), dbo.MRSRMaster.TDate, 101) AS TDate, dbo.MRSRMaster.TrType, dbo.VW_Delear_Info.Name AS InSource, dbo.VW_Delear_Info.ZoneName, dbo.Zone.CatName AS OutSource,  dbo.VW_Delear_Info.Address, dbo.VW_Delear_Info.DealerStatus, dbo.VW_Delear_Info.ContactNo, dbo.VW_Delear_Info.ContactPerson, dbo.VW_Delear_Info.Code, dbo.VW_Delear_Info.DAID,  dbo.VW_Delear_Info.EmailAdd, dbo.VW_Delear_Info.ZoneType, dbo.MRSRMaster.POCode, dbo.MRSRMaster.OnLineSales, dbo.MRSRMaster.TermsCondition, dbo.MRSRMaster.Remarks,  dbo.MRSRMaster.MRSRMID, dbo.MRSRMaster.NetSalesAmnt, dbo.MRSRMaster.SaleDeclar, CASE dbo.MRSRMaster.SaleDeclar WHEN 1 THEN 'Declared' ELSE 'Pending' END AS sStatus FROM dbo.VW_Delear_Info INNER JOIN dbo.MRSRMaster ON dbo.VW_Delear_Info.DAID = dbo.MRSRMaster.OutSource INNER JOIN dbo.Zone ON dbo.MRSRMaster.InSource = dbo.Zone.CategoryID WHERE (dbo.MRSRMaster.TrType = -3)   AND (dbo.MRSRMaster.TDate>='" + Convert.ToDateTime(this.txtFrom.Text) + "' AND dbo.MRSRMaster.TDate<='" + Convert.ToDateTime(this.txtToDate.Text) + "') AND (dbo.VW_Delear_Info.Name ='" + ddlDealerName.Text + "') ORDER BY dbo.MRSRMaster.TDate, dbo.MRSRMaster.MRSRCode DESC ";
        dt = new DataTable();
        dt = ExecuteReturnDataTable(SqlQuery, conn1);
        gvWithdran.DataSource = dt;
        gvWithdran.DataBind();
        conn1.Close();





        SqlQuery = @"SELECT   dbo.DepositAmnt.CANO, dbo.DepositAmnt.CollectionNo, CONVERT(VARCHAR(10), dbo.DepositAmnt.CDate, 105) AS DepositDate, dbo.DepositAmnt.DelearID, dbo.DelearInfo.Code, dbo.DelearInfo.Name, dbo.DelearInfo.Address,  ISNULL(dbo.DepositAmnt.DepositAmnt, 0) AS DepositAmnt, dbo.DepositAmnt.PayType, dbo.DepositAmnt.ChequeNo, dbo.DepositAmnt.BankName, dbo.DepositAmnt.BranchName,  dbo.DepositAmnt.cRemarks, dbo.Zone.CatName AS ZoneName, dbo.Zone.CategoryID,  dbo.DelearInfo.DAID, dbo.DepositAmnt.RefNo, dbo.DepositAmnt.BankID FROM dbo.DepositAmnt INNER JOIN dbo.DelearInfo ON dbo.DepositAmnt.DelearID = dbo.DelearInfo.DAID INNER JOIN dbo.Zone ON dbo.DelearInfo.CategoryID = dbo.Zone.CategoryID WHERE (dbo.DepositAmnt.CDate>='" + Convert.ToDateTime(this.txtFrom.Text) + "' AND dbo.DepositAmnt.CDate<='" + Convert.ToDateTime(this.txtToDate.Text) + "') AND (dbo.DelearInfo.Name  ='" + ddlDealerName.Text + "') ORDER BY dbo.DepositAmnt.CDate, dbo.DepositAmnt.CollectionNo Desc ";

        dt = new DataTable();
        dt = ExecuteReturnDataTable(SqlQuery, conn1);
        gvDeposit.DataSource = dt;
        gvDeposit.DataBind();
        conn1.Close();
    
    }




    private void FnloadOpeningdata()  
    {
       
        string SqlQuery = "";
        SqlQuery = @"SELECT dbo.DelearInfo.Code,  dbo.DelearInfo.Name, SUM(ISNULL(dbo.MRSRMaster.NetSalesAmnt,0)) AS NetSalesAmnt FROM dbo.DelearInfo INNER JOIN dbo.MRSRMaster ON dbo.DelearInfo.DAID = dbo.MRSRMaster.InSource WHERE (dbo.MRSRMaster.TrType = 3) AND dbo.DelearInfo.Name='" + ddlDealerName.Text + "' AND dbo.MRSRMaster.TDate<'" + Convert.ToDateTime(this.txtFrom.Text) + "' GROUP BY   dbo.DelearInfo.Code, dbo.DelearInfo.Name ";
        getConnectiondealer();
        SqlCommand cmd = new SqlCommand(SqlQuery, conn1);
        SqlDataReader dr = cmd.ExecuteReader();

        if (dr.Read())
        {
            this.lblOBSales.Text = dr["NetSalesAmnt"].ToString();
        }
        else
        {
            this.lblOBSales.Text = "0";
        }
    
        dr.Dispose();
         dr.Close();
         conn1.Close();


  
          SqlQuery = "";
         SqlQuery = @"SELECT dbo.DepositAmnt.DelearID, dbo.DelearInfo.Code, dbo.DelearInfo.Name, SUM(ISNULL(dbo.DepositAmnt.DepositAmnt, 0)) AS cAmount FROM dbo.DepositAmnt INNER JOIN dbo.DelearInfo ON dbo.DepositAmnt.DelearID = dbo.DelearInfo.DAID WHERE dbo.DelearInfo.Name='" + ddlDealerName.Text + "' AND dbo.DepositAmnt.CDate<'" + Convert.ToDateTime(this.txtFrom.Text) + "' GROUP BY dbo.DepositAmnt.DelearID,  dbo.DelearInfo.Code, dbo.DelearInfo.Name";

          getConnectiondealer();
          cmd = new SqlCommand(SqlQuery, conn1);
         
          dr = cmd.ExecuteReader();

         if (dr.Read())
         {
             this.lblOBCollection.Text = dr["cAmount"].ToString();
         }
         else
         {
             this.lblOBCollection.Text = "0";
         }
         dr.Dispose();
         dr.Close();
         conn1.Close();
    


 
   
        SqlQuery = @"SELECT Name, SUM(cAmount) AS cAmount From dbo.VW_DishonourAmnt WHERE Name='" + ddlDealerName.Text + "' AND     CDate<'" + Convert.ToDateTime(this.txtFrom.Text) + "' GROUP BY Name";
        getConnectiondealer();
        cmd = new SqlCommand(SqlQuery, conn1);
       
         dr = cmd.ExecuteReader();

        if (dr.Read())
        {
            this.lblOBDis.Text = dr["cAmount"].ToString();
        }
        else
        {
            this.lblOBDis.Text = "0";
        }
        dr.Dispose();
        dr.Close();
        conn1.Close();
   
   
 

         SqlQuery = @"SELECT dbo.DelearInfo.Code,  dbo.DelearInfo.Name, SUM(ISNULL(dbo.MRSRMaster.NetSalesAmnt,0)) AS NetSalesAmnt FROM dbo.DelearInfo INNER JOIN dbo.MRSRMaster ON dbo.DelearInfo.DAID = dbo.MRSRMaster.OutSource  WHERE (dbo.MRSRMaster.TrType = -3) AND dbo.DelearInfo.Name='" + ddlDealerName.Text + "' AND dbo.MRSRMaster.TDate<'" + Convert.ToDateTime(this.txtFrom.Text) + "' GROUP BY  dbo.DelearInfo.Code, dbo.DelearInfo.Name ";

          getConnectiondealer();
          cmd = new SqlCommand(SqlQuery, conn1);
          dr = cmd.ExecuteReader();

         if (dr.Read())
         {
             this.lblOBWith.Text = dr["NetSalesAmnt"].ToString();
         }
         else
         {
             this.lblOBWith.Text = "0";
         }
         dr.Dispose();
         dr.Close();
         conn1.Close();

    }


    protected void gvSales_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //CalcTotalQty(e.Row.Cells[4].Text);
            CalcTotal_TP(e.Row.Cells[3].Text);

            //CalcTotal_Cash(e.Row.Cells[4].Text);
            //CalcTotal_Card(e.Row.Cells[5].Text);

            // ALIGNMENT
            e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Left;

            //e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
            //e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Center;

            //e.Row.Cells[7].HorizontalAlign = HorizontalAlign.Center;
            //e.Row.Cells[8].HorizontalAlign = HorizontalAlign.Center;


        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[2].Text = "Total";
            //e.Row.Cells[4].Text = runningTotalQty.ToString("0,0", CultureInfo.InvariantCulture);
            e.Row.Cells[3].Text = runningTotalTP.ToString("0,0", CultureInfo.InvariantCulture);
            lblSalesAmount.Text = runningTotalTP.ToString("0,0", CultureInfo.InvariantCulture);
            //e.Row.Cells[4].Text = runningTotalCash.ToString("0,0", CultureInfo.InvariantCulture);
            //e.Row.Cells[5].Text = runningTotalCard.ToString("0,0", CultureInfo.InvariantCulture);


            //ALIGNMENT
            e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Left;

            e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
            //e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Center;
            //e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Center;

        }

    
    }
    protected void gvWithdran_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //CalcTotalQty(e.Row.Cells[4].Text);
            CalcTotal_With(e.Row.Cells[3].Text);

            //CalcTotal_Cash(e.Row.Cells[4].Text);
            //CalcTotal_Card(e.Row.Cells[5].Text);

            // ALIGNMENT
            e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Left;

            //e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
            //e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Center;

            //e.Row.Cells[7].HorizontalAlign = HorizontalAlign.Center;
            //e.Row.Cells[8].HorizontalAlign = HorizontalAlign.Center;


        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[2].Text = "Total";
            //e.Row.Cells[4].Text = runningTotalQty.ToString("0,0", CultureInfo.InvariantCulture);
            e.Row.Cells[3].Text = runningTotalWith.ToString("0,0", CultureInfo.InvariantCulture);
            lblWithdrawn.Text = runningTotalWith.ToString("0,0", CultureInfo.InvariantCulture);
            //e.Row.Cells[4].Text = runningTotalCash.ToString("0,0", CultureInfo.InvariantCulture);
            //e.Row.Cells[5].Text = runningTotalCard.ToString("0,0", CultureInfo.InvariantCulture);


            //ALIGNMENT
            e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Left;

            e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
            //e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Center;
            //e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Center;

        }

    }
    protected void gvDeposit_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //CalcTotalQty(e.Row.Cells[4].Text);
            //CalcTotal_TP(e.Row.Cells[3].Text);

            //CalcTotal_Cash(e.Row.Cells[4].Text);
            CalcTotal_Card(e.Row.Cells[3].Text);

            // ALIGNMENT
            e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Left;

            //e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
            //e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;

            //e.Row.Cells[8].HorizontalAlign = HorizontalAlign.Center;


        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[1].Text = "Total";
            //e.Row.Cells[4].Text = runningTotalQty.ToString("0,0", CultureInfo.InvariantCulture);
            //e.Row.Cells[3].Text = runningTotalTP.ToString("0,0", CultureInfo.InvariantCulture);
            //e.Row.Cells[4].Text = runningTotalCash.ToString("0,0", CultureInfo.InvariantCulture);
            e.Row.Cells[3].Text = runningTotalCard.ToString("0,0", CultureInfo.InvariantCulture);
            lblDeposit.Text = runningTotalCard.ToString("0,0", CultureInfo.InvariantCulture);

            //ALIGNMENT
            e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Left;

            //e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
            //e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;

        }

    }

    //CalculateTotalcash Amount
    private void CalcTotal_Cash(string _price)
    {
        try
        {
            runningTotalCash += Double.Parse(_price);
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write(ex.ToString());
        }
    }

    //CALCULATE TOTAL CARD PAY
    private void CalcTotal_Card(string _price)
    {
        try
        {
            runningTotalCard += Double.Parse(_price);
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write(ex.ToString());
        }
    }
    //Calculate Total Withdrawn
    private void CalcTotal_With(string _price)
    {
        try
        {
            runningTotalWith += Double.Parse(_price);
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write(ex.ToString());
        }
    }
    //Calculate totalpay
    private void CalcTotal_TP(string _price)
    {
        try
        {
            runningTotalTP += Double.Parse(_price);
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write(ex.ToString());
        }
    }





    //Export to Excel Oparetion method
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

        divPoint.RenderControl(htw);
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

        PagePdf.RenderControl(hw);
        // gvPdf.RenderControl(hw);

        //gvPdf.HeaderRow.Style.Add("width", "25%");
        //gvPdf.HeaderRow.Style.Add("font-size", "40px");

        Document doc = new Document(PageSize.A3, 40f, 30f, 40f, 40f);
        HTMLWorker htmlworker = new HTMLWorker(doc);
        PdfWriter.GetInstance(doc, Response.OutputStream);
        doc.Open();
        StringReader str = new StringReader(sw.ToString());
        htmlworker.Parse(str);
        doc.Close();
        Response.Write(doc);
        Response.End();
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