using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System.Data.SqlClient;
using System.Data;

public partial class DealerReports_AcountsReport : System.Web.UI.Page
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




    public void getConnection()
    {
        if (conn.State != null)
            conn.Close();
        conn.Open();
    }

    public void getConnection1()
    {
        if (conn1.State != null)
            conn1.Close();
        conn1.Open();
    }




    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GetModel(string prefixText)
    {
        SqlConnection conn = DBConnection.GetConnection();
        DataTable dt = new DataTable();
        conn.Open();

        SqlCommand cmd = new SqlCommand("Select TOP 10 * from Product where Discontinue='No' AND Model like @model+'%'", conn);
        cmd.Parameters.AddWithValue("@model", prefixText);
        SqlDataAdapter adp = new SqlDataAdapter(cmd);
        adp.Fill(dt);
        List<string> ModelNames = new List<string>();
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            ModelNames.Add(dt.Rows[i][5].ToString());
        }
        return ModelNames;
    }




    protected void btn_preview_Click(object sender, EventArgs e)
    {
        getData();
        getdataforExport();

    }


    //protected void gv_salesReport_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
        
    //}


    protected void btn_export_excel_Click(object sender, EventArgs e)
    {
       if (btn_radio_salesReport.Checked)
           Export_Excel(gv_salesReport);
       
       else if (btn_radio_inventory.Checked)
           Export_Excel(gvInventoryReport);
  

    }

    //rendermethod control for export pdf and excel
    public override void VerifyRenderingInServerForm(Control control)
    {

    }


    //Export to Pdf Button
    protected void btn_export_pdf_Click(object sender, EventArgs e)
    {
       if (btn_radio_salesReport.Checked)
           Export_pdf(gv_salesReport);
       else if (btn_radio_inventory.Checked)
           Export_pdf(gvInventoryReport);
      
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
         getdataforExport();
     }
    private void getdataforExport() 
     {
         getConnection();
         if (btn_radio_salesReport.Checked) 
          {
              try
               {


                   string query = "";
                   query += @"select * into #temp 
                 from 
                 (
                  select ISNULL(e1.Amount,0)amountS,ISNULL(e2.Amount,0)amountW,isnull(e1.Qty,0)qtyS,isnull(e2.Qty,0) qtyW,
                  ISNULL(e1.Model,e2.Model)Model,ISNULL(e1.GroupName,e2.GroupName)GroupName,ISNULL(e1.eName,e2.eName)eName,
                  ISNULL(e1.TDate,e2.TDate)TDate,
                  ISNULL(e1.SalesOrShowroom,e2.SalesOrShowroom)SalesOrShowroom

                  from 
                  (
	                  select sum(abs(md.NetAmnt))Amount,sum(abs(md.Qty))Qty,p.Model,p.GroupName,en.eName,cast(M.TDate as date)Tdate
	                  ,en.SalesOrShowroom
	                  from MRSRMaster m 
	                  inner join MRSRDetails md on md.MRSRMID=m.MRSRMID 
	                  inner join Product p on p.ProductID = md.ProductID and p.Discontinue='No'
	                  inner join Entity en on en.EID = m.OutSource
	                  where TDate>='" + Convert.ToDateTime(this.txtFrom.Text) + "' and TDate<='" + Convert.ToDateTime(this.txtToDate.Text) + @"' and m.TrType=3 and en.ActiveDeactive = 1 
	                    and en.SalesOrShowroom in ('0','1','2')
			                  group by p.GroupName,p.Model,en.eName,M.TDate,en.SalesOrShowroom
                  )
                   as e1 
                  full join 
                  (
                  select sum(abs(md.NetAmnt))Amount,sum(abs(md.Qty))Qty,p.Model,p.GroupName,en.eName,cast(M.TDate as date)Tdate,en.SalesOrShowroom
 
                  from MRSRMaster m 
                  inner join MRSRDetails md on md.MRSRMID=m.MRSRMID 
                  inner join Product p on p.ProductID = md.ProductID and p.Discontinue='No'
                  inner join Entity en on en.EID = m.InSource
                  where TDate>='" + Convert.ToDateTime(this.txtFrom.Text) + "' and TDate<='" + Convert.ToDateTime(this.txtToDate.Text) + @"' and en.ActiveDeactive = 1--and en.EntityType in('Customer') 
 
                  and en.SalesOrShowroom in ('0','1','2')
                   and m.TrType=-3 group by p.GroupName,p.Model,en.eName,M.TDate,en.SalesOrShowroom
                  ) as e2 on e1.Model=e2.Model and e1.eName=e2.eName and e1.Tdate=e2.Tdate) as e
                   select CASE
                    WHEN SalesOrShowroom = '0' THEN 'Showroom'
                    WHEN SalesOrShowroom = '1' THEN 'Corporate'
                    ELSE 'Dealer'end as CTP 
	                ,eName As Location,Model
                   ,GroupName,DATENAME(MONTH, DATEADD(MONTH, 0, TDate))+'-'+cast(YEAR(TDate) as varchar) Month
                  ,sum(qtyS)SalesQty,sum(amountS)SalesAmnt,sum(qtyW)WithQty,sum(amountW)WithdrawnAmnt,sum(qtyS-qtyW)Qty
                 ,sum(amountS-amountW)GrossSales

 
                 from #temp	 

                group by GroupName,TDate,SalesOrShowroom,Model,eName 
                 order by Qty desc

                   drop table #temp";

                   dt = new DataTable();
                   dt = ExecuteReturnDataTable(query, conn);
                   gv_salesReport.DataSource = dt;
                   gv_salesReport.DataBind();
              
               }
              catch (Exception ex) 
               { 
              
               }
         
          }

         if (btn_radio_inventory.Checked) 
          {
              try
              {
                  fnLoadStatementData();
                  //fnLoadData();
                  getData();
                  string finalQuery="";
                  finalQuery = @"select model,BrandName,OpCIDD,OpCTP,OpCKDSKD,OpCBU,(OpCIDD+OpCTP+OpCKDSKD+OpCBU)as OpeningTotalQty,PurchaseQty,WithDrawnQty,SalesQty,(OpCIDD+PurchaseQty+WithDrawnQty) as ClosCidd,(OpCTP-SalesQty)as ClCtp,OpCKDSKD as cloCKDSKD,OpCBU as CloCBU,(OpCIDD+PurchaseQty+WithDrawnQty)+(OpCTP-SalesQty) as closingstock from tempStockReport
";

                  dt = new DataTable();
                  dt = ExecuteReturnDataTable(finalQuery, conn);
                  gvInventoryReport.DataSource = dt;
                  gvInventoryReport.DataBind();
              

                   
              }
              catch (Exception ex) 
              {

              }

          }
    
     }

    private void fnLoadData() 
     {

         try
         {

             string sSql = @"SELECT GroupName, Model, ProdName, SUM(QtyIN) AS tIn, SUM(QtyOut) AS tOut, SUM(QtyIN) - SUM(QtyOut) AS bQty, UserID FROM dbo.RPTBalance_Online
                GROUP BY GroupName, Model, ProdName, UserID HAVING (UserID = '" + Session["UserName"] + "') AND (SUM(QtyIN) - SUM(QtyOut) <> 0) ORDER BY GroupName, Model";
 

         }
         catch (Exception ex)
         {

         }

     }


    private void fnLoadStatementData()
    {


        string gSQL = "";

        //*****************************************************************************************
        //DELETE PREVIOUS DATA
    
        gSQL = "DELETE FROM RPTBalance_Online WHERE UserID='" + Session["UserName"] + "'";

        SqlCommand cmdD = new SqlCommand(gSQL, conn1);
        getConnection1();
        cmdD.ExecuteNonQuery();
        conn1.Close();
        //*****************************************************************************************



        gSQL = @" SELECT dbo.Entity.sFlag,dbo.Entity.SerialNo,dbo.Entity.GroupSL AS eGroupSL,dbo.Entity.eName AS InSource,  dbo.Product.ProdName, dbo.Product.Model,dbo.Product.ModelSerial, 
 dbo.Product.GroupName, dbo.Product.GroupSL,dbo.Entity.EntityType,QtyIN = SUM(CASE WHEN (TrType = 1 OR TrType = 2 OR TrType = 3 OR
        TrType = 4 OR TrType=-1 OR TrType=-3) THEN ABS(dbo.MRSRDetails.QTy) ELSE 0 END),QtyOut =0
         FROM dbo.MRSRMaster INNER JOIN dbo.MRSRDetails ON dbo.MRSRMaster.MRSRMID = dbo.MRSRDetails.MRSRMID INNER JOIN
        dbo. Product ON dbo.MRSRDetails.ProductID = dbo. Product .ProductID INNER JOIN dbo.Entity ON dbo.MRSRMaster.InSource = dbo.Entity.EID
        WHERE dbo.MRSRMaster.TDate <='" + Convert.ToDateTime(this.txtFrom.Text) + @"' AND dbo.Product.Model='" + txtModel.Text + @"'
        AND dbo.Entity.ActiveDeactive = 1 AND dbo.Product.Discontinue = 'No'GROUP BY dbo.Entity.sFlag,dbo.Entity.SerialNo,dbo.Entity.GroupSL,dbo.Entity.eName, dbo.Product.ProdName, dbo.Product.Model, dbo.Product.ModelSerial,
         dbo.Product.GroupName, dbo.Product.GroupSL,dbo.Entity.EntityType";


        SqlCommand cmd = new SqlCommand(gSQL, conn);
        getConnection();
        SqlDataReader dr = cmd.ExecuteReader();
        DataTable dtTest = new DataTable();
        //dtTest.Load(dr);
        //int c = dtTest.Rows.Count;
        int c = 0;
        while (dr.Read())
        {
            c++;
        
            gSQL = @"INSERT INTO RPTBalance_Online(sFlag,SerialNo,EGroupSL,Entity,ProdName, Model,ModelSerial,QtyIN,QtyOut,GroupName,GroupSL, EntityType,UserID)
                   VALUES('" + dr["sFlag"] + "','" + dr["SerialNo"] + "','" + dr["EGroupSL"] + "', '" + dr["InSource"] + "','" + dr["ProdName"] + @"',
                 '" + dr["Model"] + "','" + dr["ModelSerial"] + "','" + dr["QtyIN"] + "', '" + dr["QtyOut"] + "','" + dr["GroupName"] + "','" + dr["GroupSL"] + @"','" + dr["EntityType"] + "','" + Session["UserName"] + "')";
   

            SqlCommand cmdS = new SqlCommand(gSQL, conn1);
            getConnection1();
            cmdS.ExecuteNonQuery();
            conn1.Close();

        }
        conn.Close();
        dr.Close();
        //------------------------------------------------------------------------------------------------

        gSQL = @"SELECT dbo.Entity.sFlag,dbo.Entity.SerialNo,dbo.Entity.GroupSL AS eGroupSL,dbo.Entity.eName AS OutSource,dbo.Product.ProdName, dbo.Product.Model, dbo.Product.ModelSerial,
               dbo.Product.GroupName, dbo.Product.GroupSL,dbo.Entity.EntityType,QtyIN =0,QtyOut = SUM(CASE WHEN (TrType = 1 OR TrType = 2 OR TrType = 3 OR 
              TrType = 4 OR TrType=-1 OR TrType=-3) THEN ABS(dbo.MRSRDetails.QTy) ELSE 0 END) FROM dbo.MRSRMaster INNER JOIN
              dbo.MRSRDetails ON dbo.MRSRMaster.MRSRMID = dbo.MRSRDetails.MRSRMID INNER JOIN
             dbo. Product ON dbo.MRSRDetails.ProductID = dbo. Product .ProductID INNER JOIN dbo.Entity ON dbo.MRSRMaster.OutSource = dbo.Entity.EID
           WHERE dbo.MRSRMaster.TDate <='" + Convert.ToDateTime(this.txtFrom.Text) + @"' AND dbo.Entity.ActiveDeactive = 1  AND dbo.Product.Model='" + txtModel.Text + @"' AND dbo.Product.Discontinue = 'No'
            GROUP BY dbo.Entity.sFlag,dbo.Entity.SerialNo,dbo.Entity.GroupSL,dbo.Entity.eName, dbo.Product.ProdName, dbo.Product.Model,   dbo.Product.ModelSerial, dbo.Product.GroupName, dbo.Product.GroupSL,dbo.Entity.EntityType";

        cmd = new SqlCommand(gSQL, conn);
        getConnection();
        dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            gSQL = @"INSERT INTO RPTBalance_Online(sFlag,SerialNo,EGroupSL,Entity,ProdName,Model,ModelSerial,QtyIN, QtyOut,GroupName,GroupSL,EntityType,UserID)
            VALUES('" + dr["sFlag"] + "','" + dr["SerialNo"] + "','" + dr["EGroupSL"] + "','" + dr["OutSource"] + "','" + dr["ProdName"] + "','" + dr["Model"] + "','" + dr["ModelSerial"] + "','" + dr["QtyIN"] + @"',
           '" + dr["QtyOut"] + "','" + dr["GroupName"] + "','" + dr["GroupSL"] + "', '" + dr["EntityType"] + "','" + Session["UserName"] + "')";

            SqlCommand cmdS = new SqlCommand(gSQL, conn1);
            getConnection1();
            cmdS.ExecuteNonQuery();
            conn1.Close();

        }
        conn.Close();
        dr.Close();


        //=================================================================================================
        // DELETE PREVIOUS DATA
      
        gSQL = @"DELETE FROM TempRPTBalance_Online WHERE UserID='" + Session["UserName"] + "'";

        cmdD = new SqlCommand(gSQL, conn1);
        getConnection1();
        cmdD.ExecuteNonQuery();
        conn1.Close();


        // INSERT NEW DATA
      
        gSQL = @"SELECT  sFlag,SerialNo,EGroupSL,Entity,ProdName, Model, GroupName,GroupSL,EntityType, ModelSerial,SUM(QtyIN) as QtyIN, SUM(QtyOut) AS QtyOut From RPTBalance_Online  WHERE UserID='" + Session["UserName"] + "' GROUP BY sFlag,SerialNo,EGroupSL,Entity,ProdName, GroupName,GroupSL,EntityType, Model,MOdelSerial";

        cmd = new SqlCommand(gSQL, conn);
        getConnection();
        dr = cmd.ExecuteReader();
        while (dr.Read())
        {
         gSQL = "INSERT INTO TempRPTBalance_Online(sFlag,SerialNo,Entity,ProdName, Model,ModelSerial,QtyIN,         QtyOut,QtyBalance,GroupName,GroupSL,Tag,EGroupSL,EntityType,UserID) VALUES(" + dr["sFlag"] + "," + dr["SerialNo"] + ", '" + dr["Entity"] + "','" + dr["ProdName"] + "', '" + dr["Model"] + "'," + dr["ModelSerial"] + "," + dr["QtyIN"] + "," + dr["QtyOut"] + ",'" + (Convert.ToDouble(dr["QtyIN"]) - Convert.ToDouble(dr["QtyOut"])) + "', '" + dr["GroupName"] + "'," + dr["GroupSL"] + ", 0," + dr["EGroupSL"] + ",'" + dr["EntityType"] + "','" + Session["UserName"] + "')";
     
            SqlCommand cmdS = new SqlCommand(gSQL, conn1);
            getConnection1();
            cmdS.ExecuteNonQuery();
            conn1.Close();

        }
        //====================================================================================================


    }



    public void getData() 
     {
         try
         {
             string sqlqury = "";
             sqlqury = "DELETE FROM tempStockReport WHERE UserID='" + Session["UserName"] + "'";

             SqlCommand cmdD = new SqlCommand(sqlqury, conn1);
             getConnection1();
             cmdD.ExecuteNonQuery();
             conn1.Close();
           
            sqlqury= @"SELECT Entity,EntityType,
GroupName, Model, ProdName, SUM(QtyIN) AS tIn, SUM(QtyOut) AS tOut, SUM(QtyIN) - SUM(QtyOut) AS bQty, UserID FROM dbo.RPTBalance_Online where Entity='CI&DD (REL)' 
                GROUP BY Entity,EntityType, GroupName, Model, ProdName, UserID HAVING (UserID = '" + Session["UserName"] + "') AND (SUM(QtyIN) - SUM(QtyOut) <> 0) ORDER BY GroupName, Model";

            SqlCommand cmd = new SqlCommand(sqlqury, conn);
            getConnection();
            SqlDataReader dr = cmd.ExecuteReader();
            DataTable dtTest = new DataTable();

            while (dr.Read())
            {
                string getdelear = getDealer().ToString();
                string getCtp = getCTP().ToString();
                string getskd = getCKD().ToString();
                string getcbu = getCBU().ToString();
                string getzone = getZone().ToString();
                string getreciveqty = getReciveQty().ToString();
                string getsalesQty = getSalesQty().ToString();
                string getwithQty = getWithdrawnQty().ToString();


                sqlqury = "INSERT INTO tempStockReport(ProductCatagory,Model,BrandName,OpCIDD,OpDealer,OpZone,OpCTP,OpCKDSKD,OpCBU,PurchaseQty,SalesQty,WithDrawnQty,CLZone,UserID) VALUES('" + dr["ProdName"] + "','" + dr["Model"] + "','" + dr["GroupName"] + "','" + dr["bQty"] + "','" + getdelear + "','" + getzone + "','" + getCtp + "','" + getskd + "','" + getcbu + "','" + getreciveqty + "','" + getsalesQty + "','" + getwithQty + "','" + dr["Entity"]+ "','" + Session["UserName"] + "')";
                SqlCommand cmdS = new SqlCommand(sqlqury, conn1);
                getConnection1();
                cmdS.ExecuteNonQuery();
                conn1.Close();
            }

       
         }
         catch (Exception ex) 
          {
              
          }
     }




    public string getCTP() 
     {
         try
         {
             DataTable dt = new DataTable();
             dt = ExecuteReturnDataTable(@"SELECT GroupName, Model, ProdName, SUM(QtyIN) AS tIn, SUM(QtyOut) AS tOut, SUM(QtyIN) - SUM(QtyOut) AS bQty, UserID FROM dbo.RPTBalance_Online  where EntityType='showroom' 
                GROUP BY GroupName, Model, ProdName, UserID HAVING (UserID = '" + Session["UserName"] + "') AND (SUM(QtyIN) - SUM(QtyOut) <> 0) ORDER BY GroupName, Model", conn1);
             return dt.Rows[0]["bQty"].ToString();
         }
         catch (Exception ex) 
         {
             return "0";
         }
    
     }


    public string getDealer()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = ExecuteReturnDataTable(@"SELECT GroupName, Model, ProdName, SUM(QtyIN) AS tIn, SUM(QtyOut) AS tOut, SUM(QtyIN) - SUM(QtyOut) AS bQty, UserID FROM dbo.RPTBalance_Online where EntityType='Dealer' 
                GROUP BY GroupName, Model, ProdName, UserID HAVING (UserID = '" + Session["UserName"] + "') AND (SUM(QtyIN) - SUM(QtyOut) <> 0) ORDER BY GroupName, Model", conn1);
            return dt.Rows[0]["bQty"].ToString();
        }
        catch (Exception ex)
        {
            return "0";
        }

    }

    public string getCKD()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = ExecuteReturnDataTable(@"SELECT GroupName, Model, ProdName, SUM(QtyIN) AS tIn, SUM(QtyOut) AS tOut, SUM(QtyIN) - SUM(QtyOut) AS bQty, UserID FROM dbo.RPTBalance_Online where Entity='CKD/SKD' 
                GROUP BY GroupName, Model, ProdName, UserID HAVING (UserID = '" + Session["UserName"] + "') AND (SUM(QtyIN) - SUM(QtyOut) <> 0) ORDER BY GroupName, Model", conn1);
            return dt.Rows[0]["bQty"].ToString();
        }
        catch (Exception ex)
        {
            return "0";
        }

    }

    public string getCBU()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = ExecuteReturnDataTable(@"SELECT GroupName, Model, ProdName, SUM(QtyIN) AS tIn, SUM(QtyOut) AS tOut, SUM(QtyIN) - SUM(QtyOut) AS bQty, UserID FROM dbo.RPTBalance_Online where Entity='CBU(FACTORY)'
                GROUP BY GroupName, Model, ProdName, UserID HAVING (UserID = '" + Session["UserName"] + "') AND (SUM(QtyIN) - SUM(QtyOut) <> 0) ORDER BY GroupName, Model", conn1);
            return dt.Rows[0]["bQty"].ToString();
        }
        catch (Exception ex)
        {
            return "0";
        }

    }

    public string getZone()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = ExecuteReturnDataTable(@"SELECT GroupName, Model, ProdName, SUM(QtyIN) AS tIn, SUM(QtyOut) AS tOut, SUM(QtyIN) - SUM(QtyOut) AS bQty, UserID FROM dbo.RPTBalance_Online where EntityType='sub store' and Entity not in('CI&DD (REL)','CKD/SKD')
                GROUP BY GroupName, Model, ProdName, UserID HAVING (UserID = '" + Session["UserName"] + "') AND (SUM(QtyIN) - SUM(QtyOut) <> 0) ORDER BY GroupName, Model", conn1);
            return dt.Rows[0]["bQty"].ToString();
        }
        catch (Exception ex)
        {
            return "0";
        }

    }

    public string getReciveQty() 
    {
        try
        {
            DataTable dt = new DataTable();
            dt = ExecuteReturnDataTable(@"select p.GroupName,p.Model,p.ProdName,m.TrType,sum(abs(md.Qty))Qty from MRSRMaster m inner join MRSRDetails md on m.MRSRMID=md.MRSRMID inner join Product p on md.ProductID=p.ProductID 
  where m.TDate>='" + Convert.ToDateTime(this.txtFrom.Text) + "' and m.TDate<='" + Convert.ToDateTime(this.txtToDate.Text) + @"'and m.TrType='1' and p.Model='" + txtModel.Text + @"'
  group by p.GroupName,p.Model,p.ProdName,m.TrType", conn1);
            return dt.Rows[0]["Qty"].ToString();
        }
        catch (Exception ex) 
        {
            return "0";
        }
    }
    public string getSalesQty()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = ExecuteReturnDataTable(@"select p.GroupName,p.Model,p.ProdName,m.TrType,sum(abs(md.Qty))Qty from MRSRMaster m inner join MRSRDetails md on m.MRSRMID=md.MRSRMID inner join Product p on md.ProductID=p.ProductID inner join Entity e on m.OutSource=e.EID
  where m.TDate>='" + Convert.ToDateTime(this.txtFrom.Text) + "' and m.TDate<='" + Convert.ToDateTime(this.txtToDate.Text) + @"'and m.TrType='3' and p.Model='" + txtModel.Text + @"' and e.SalesOrShowroom='0'
  group by p.GroupName,p.Model,p.ProdName,m.TrType", conn1);
            return dt.Rows[0]["Qty"].ToString();
        }
        catch (Exception ex)
        {
            return "0";
        }
    }
    public string getWithdrawnQty()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = ExecuteReturnDataTable(@"select p.GroupName,p.Model,p.ProdName,m.TrType,sum(abs(md.Qty))Qty from MRSRMaster m inner join MRSRDetails md on m.MRSRMID=md.MRSRMID inner join Product p on md.ProductID=p.ProductID 
  where m.TDate>='" + Convert.ToDateTime(this.txtFrom.Text) + "' and m.TDate<='" + Convert.ToDateTime(this.txtToDate.Text) + @"'and m.TrType='-3' and p.Model='"+txtModel.Text +@"'
  group by p.GroupName,p.Model,p.ProdName,m.TrType", conn1);
            return dt.Rows[0]["Qty"].ToString();
        }
        catch (Exception ex)
        {
            return "0";
        }
    }


    protected void gvInventoryReport_RowDataBound(object sender, GridViewRowEventArgs e)
    { 
    
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