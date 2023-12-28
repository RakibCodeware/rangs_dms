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
using System.Globalization;


public partial class DealerReports_Reports : System.Web.UI.Page
{
    SqlConnection conn = DBConnection.GetConnection();
    SqlConnection conn1 = DBConnectionDSM.GetConnection();
    SqlConnection conn2 = DBConnectionDSM.GetConnection();
    DataTable dt;

    //for calculate total create global method start

    private double runningTotalTP = 0;

    //for calculate total create global method end
    //for deposit start
    private double runningTotalCard = 0;
    //for deposit

    private double runningTotalUnit = 0;
    private double runningTotalNetAmnt = 0;
    private double runningTotalQty = 0;
    //zonewise Sales delivery
    private double runningOpeningBalance = 0;
    private double runningSalesAmount = 0;
    private double runningWithdrawnAmount = 0;
    private double runningActualSales = 0;
    private double runningBankDeposit = 0;
    private double runningClosingBalance = 0;



    //group by




    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            if (Session["UserName"] != null)
            {
                string username = Session["UserName"].ToString();
                gettreeView(getUserName(username));
                BindData();
            }
            
        }
    }


    //connection delearsales databse
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

    //connection dbcid database
    public void getConnection()
    {
        if (conn.State != null)
            conn.Close();
        conn.Open();
    }


    //get zone name like tree view start

    public void gettreeView(int UserID)
    {
        try
        {
            string ZoneIds = getPermissionzone(UserID);
            int[] integerArray = ZoneIds
            .Split(',')
            .Select(int.Parse)
            .ToArray();
            DataTable dt = new DataTable();
            string query = "SELECT ZoneId, ZoneName, ParentZoneId FROM DealerOrganogram where ZoneID in(" + string.Join(",", integerArray)+ ")";
            using (conn)
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    getConnection();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    conn.Close();
                }
            }
            PopulateTreeView(int.Parse(dt.Rows[0]["ParentZoneId"].ToString()), null, dt);
        }
        catch (Exception ex) 
         {
        
         }
    }

    //tree view data

    public void PopulateTreeView(int parentId, TreeNode parentNode, DataTable dt)
    {
        DataRow[] rows = dt.Select(string.Format("ParentZoneId = {0}", parentId));
        foreach (DataRow row in rows)
        {
            TreeNode node = new TreeNode();
            node.Text = row["ZoneName"].ToString();
            node.Value = row["ZoneId"].ToString();
            if (parentNode == null)
            {
                tree_view.Nodes.Add(node);
            }
            else
            {
                parentNode.ChildNodes.Add(node);
            }
            PopulateTreeView(Convert.ToInt32(row["ZoneId"]), node, dt);
        }

    }



    protected void btn_preview_Click(object sender, EventArgs e)
    {

        getdataforExport();

    }


    private void getdataforExport()
    {
        Session["__ZoneNameForReport__"] = "";
        reportCompanyanme.InnerText = "Rangs Electronics Limited";
        hcompanyAdreess.InnerText = "Sonartori tower(4th floor,12 Sonargaon Road)";
        hcompanyrodadress.InnerText = "Dhaka-1000 Bangladesh";
        getReporName();

        hReportsDateRange.InnerText = "(" + txtFrom.Text.Trim() + " To " + txtToDate.Text.Trim() + ")";

        runningTotalTP = 0;
        runningTotalCard = 0;
        runningTotalUnit = 0;
        runningTotalNetAmnt = 0;
        runningTotalQty = 0;
        //zonewisesaleStatement

        runningOpeningBalance = 0;
        runningSalesAmount = 0;
        runningWithdrawnAmount = 0;
        runningActualSales = 0;
        runningBankDeposit = 0;
        runningClosingBalance = 0;




        List<string> myList = getCheckdValues(tree_view.Nodes);
        hZoneName.InnerText = Session["__ZoneNameForReport__"].ToString();
        string zoneIds = string.Join(",", myList.ToArray());

        getConnectiondealer();


        if (btn_radio_sales_delivary.Checked)
        {
            try
            {
                string sSql = "";
                sSql = "";
                sSql = "SELECT dbo.MRSRMaster.MRSRCode, CONVERT(varchar(12), dbo.MRSRMaster.TDate, 101) AS TDate, dbo.MRSRMaster.TrType,";
                sSql = sSql + " dbo.VW_Delear_Info.Name AS InSource, dbo.VW_Delear_Info.ZoneName, dbo.Zone.CatName AS OutSource, ";
                sSql = sSql += " dbo.VW_Delear_Info.Address, dbo.VW_Delear_Info.DealerStatus, dbo.VW_Delear_Info.ContactNo,";
                sSql = sSql + " dbo.VW_Delear_Info.ContactPerson, dbo.VW_Delear_Info.Code, dbo.VW_Delear_Info.DAID, ";
                sSql = sSql + " dbo.VW_Delear_Info.EmailAdd, dbo.VW_Delear_Info.ZoneType, dbo.MRSRMaster.POCode,";
                sSql = sSql + " dbo.MRSRMaster.OnLineSales, dbo.MRSRMaster.TermsCondition, dbo.MRSRMaster.Remarks, ";
                sSql = sSql + " dbo.MRSRMaster.MRSRMID, dbo.MRSRMaster.NetSalesAmnt, dbo.MRSRMaster.SaleDeclar,";
                sSql = sSql + " CASE dbo.MRSRMaster.SaleDeclar WHEN 1 THEN 'Declared' WHEN 2 THEN 'Proceed' ELSE 'Pending' END AS sStatus, dbo.MRSRMaster.RefCHNo";
                sSql = sSql + " FROM dbo.VW_Delear_Info INNER JOIN";
                sSql = sSql + " dbo.MRSRMaster ON dbo.VW_Delear_Info.DAID = dbo.MRSRMaster.InSource INNER JOIN";
                sSql = sSql + " dbo.Zone ON dbo.MRSRMaster.OutSource = dbo.Zone.CategoryID";

                sSql = sSql + " WHERE dbo.VW_Delear_Info.DAID IN(select Dealer from dbCID.dbo.ZoneWisDealer where Zone in(" + zoneIds + "))";

                sSql = sSql + " AND (dbo.MRSRMaster.TDate >= '" + Convert.ToDateTime(this.txtFrom.Text) + "'";
                sSql = sSql + " AND dbo.MRSRMaster.TDate <= '" + Convert.ToDateTime(this.txtToDate.Text) + "')";


                sSql = sSql + " ORDER BY dbo.MRSRMaster.TDate, dbo.MRSRMaster.MRSRCode DESC";



                //SqlCommand cmd = new SqlCommand(sSql, conn1);
                //DataTable dt = new DataTable();
                //SqlDataAdapter da = new SqlDataAdapter(cmd);
                //da.Fill(dt);

                //gv_sales_delivery.DataSource = dt;
                //gv_sales_delivery.DataBind();
                ////dr.Close();
                //conn1.Close();

                dt = new DataTable();
                dt = ExecuteReturnDataTable(sSql, conn1);
                gv_sales_delivery.DataSource = dt;
                gv_sales_delivery.DataBind();
            }

            catch (Exception ex)
            {
                Response.Write("Somthing went wrong please try again");
            }
        }


          //deposit

        else if (btn_radio_deposit.Checked)
        {
            try
            {
                string sSql = "";
                sSql = "SELECT dbo.DepositAmnt.CANO, dbo.DepositAmnt.CollectionNo,";
                sSql = sSql + " CONVERT(VARCHAR(10), dbo.DepositAmnt.CDate, 105) AS DepositDate,";
                sSql = sSql + " dbo.DepositAmnt.DelearID, dbo.DelearInfo.Code, dbo.DelearInfo.Name, dbo.DelearInfo.Address, ";
                sSql = sSql + " ISNULL(dbo.DepositAmnt.DepositAmnt, 0) AS DepositAmnt, dbo.DepositAmnt.PayType,";
                sSql = sSql + " dbo.DepositAmnt.ChequeNo, dbo.DepositAmnt.BankName, dbo.DepositAmnt.BranchName, ";
                sSql = sSql + " dbo.DepositAmnt.cRemarks, dbo.Zone.CatName AS ZoneName, dbo.Zone.CategoryID, ";
                sSql = sSql + " dbo.DelearInfo.DAID, dbo.DepositAmnt.RefNo, dbo.DepositAmnt.BankID";
                sSql = sSql + " FROM dbo.DepositAmnt INNER JOIN";
                sSql = sSql + " dbo.DelearInfo ON dbo.DepositAmnt.DelearID = dbo.DelearInfo.DAID INNER JOIN";
                sSql = sSql + " dbo.Zone ON dbo.DelearInfo.CategoryID = dbo.Zone.CategoryID";
                sSql = sSql + " WHERE dbo.DelearInfo.DAID IN(select Dealer from dbCID.dbo.ZoneWisDealer where Zone in(" + zoneIds + "))";

                sSql = sSql + " AND (dbo.DepositAmnt.CDate >= '" + Convert.ToDateTime(this.txtFrom.Text) + "'";
                sSql = sSql + " AND dbo.DepositAmnt.CDate <= '" + Convert.ToDateTime(this.txtToDate.Text) + "')";




                sSql = sSql + " ORDER BY dbo.DepositAmnt.CDate, dbo.DepositAmnt.CollectionNo Desc";

                //SqlCommand cmd = new SqlCommand(sSql, conn1);
                //DataTable dt = new DataTable();
                //SqlDataAdapter da = new SqlDataAdapter(cmd);
                //da.Fill(dt);

                //gv_deposit.DataSource = dt;
                //gv_deposit.DataBind();
                ////dr.Close();
                //conn1.Close();

                dt = new DataTable();
                dt = ExecuteReturnDataTable(sSql, conn1);
                gv_deposit.DataSource = dt;
                gv_deposit.DataBind();

            }
            catch (Exception ex)
            {
                Response.Write("Somthing went wrong please try again");
            }
        }


        //sales withdrawn

        else if (btn_radio_sales_withdrawn.Checked)
        {
            string sSql = "";
            sSql = "SELECT MRSRCode,dbo.VW_Delear_Info.ZoneName,dbo.VW_Delear_Info.Name, CONVERT(varchar(12), TDate, 105) AS TDate," +
                " ISNULL(NetSalesAmnt,0) AS NetSalesAmnt," +
                " InvoiceNo, " +
                " dbo.Customer.CustID, dbo.Customer.CustName, dbo.Customer.Address, " +
                " dbo.Customer.Phone, dbo.Customer.Mobile, dbo.Customer.Email" +

                " FROM dbo.VW_Delear_Info INNER JOIN  dbo.MRSRMaster ON dbo.VW_Delear_Info.DAID = dbo.MRSRMaster.OutSource LEFT OUTER JOIN " +
                " dbo.Customer ON dbo.MRSRMaster.Customer = dbo.Customer.Mobile" +

                " WHERE (dbo.MRSRMaster.TrType = -3) " +

                "AND dbo.VW_Delear_Info.DAID IN(select Dealer from dbCID.dbo.ZoneWisDealer where Zone in(" + zoneIds + "))" +

                " AND (dbo.MRSRMaster.TDate >= '" + Convert.ToDateTime(this.txtFrom.Text) + "'" +
                " AND dbo.MRSRMaster.TDate <= '" + Convert.ToDateTime(this.txtToDate.Text) + "')";

            sSql = sSql + " ORDER BY TDate, MRSRCode";


            //SqlCommand cmd = new SqlCommand(sSql, conn1);
            ////OleDbDataReader dr = cmd.ExecuteReader();

            //DataSet ds = new DataSet();
            //SqlDataAdapter da = new SqlDataAdapter(cmd);
            //da.Fill(ds);

            //gv_sales_withd.DataSource = ds;
            //gv_sales_withd.DataBind();
            ////dr.Close();
            //conn1.Close();


            dt = new DataTable();
            dt = ExecuteReturnDataTable(sSql, conn1);
            gv_sales_withd.DataSource = dt;
            gv_sales_withd.DataBind();
        }

        else if (btn_radio_zonewise_sales.Checked)
        {
            string sPC = Request.UserHostAddress;
            fnLoadDealerStatement();

            string sSql = "";


            sSql = "select ZoneName,DelearName,DelearID,";
            sSql = sSql + " (OpeningSalesAmnt - OpeningCollection - OpenigWithdrawn + OpeningDishonour)  AS OB,";

            sSql = sSql + " SalesAmnt,Withdrawn,(SalesAmnt-Withdrawn)as ActualSales,Collection,DishonourAmnt,";
            sSql = sSql + " ((OpeningSalesAmnt - OpeningCollection - OpenigWithdrawn + OpeningDishonour) + (SalesAmnt - Collection - Withdrawn + DishonourAmnt)) AS CB";
            sSql = sSql + " from TempOpening";
            sSql = sSql + " WHERE UserID='" + Session["sUser"].ToString() + "' AND PCName='" + sPC + "'";


            //SqlCommand cmd = new SqlCommand(sSql, conn1);
            ////OleDbDataReader dr = cmd.ExecuteReader();

            //DataSet ds = new DataSet();
            //SqlDataAdapter da = new SqlDataAdapter(cmd);
            //da.Fill(ds);

            //gv_Zone_statement.DataSource = ds;
            //gv_Zone_statement.DataBind();
            ////dr.Close();
            //conn1.Close();

            dt = new DataTable();
            dt = ExecuteReturnDataTable(sSql, conn1);
            gv_Zone_statement.DataSource = dt;
            gv_Zone_statement.DataBind();


        }

        else if (btn_radio_country_dealer_stmnt.Checked)
        {
            string sPC = Request.UserHostAddress;
            fnLoadDealerStatement_();

            string sSql = "";


            sSql = "select ZoneName,";
            sSql = sSql + " SUM(OpeningSalesAmnt - OpeningCollection - OpenigWithdrawn + OpeningDishonour)  AS OB,";

            sSql = sSql + " SUM(SalesAmnt)as SalesAmnt,SUM(Withdrawn)as Withdrawn,SUM(SalesAmnt-Withdrawn)as ActualSales,SUM(Collection) as Collection,SUM(DishonourAmnt) as DishonourAmnt,";
            sSql = sSql + " SUM((OpeningSalesAmnt - OpeningCollection - OpenigWithdrawn + OpeningDishonour) + (SalesAmnt - Collection - Withdrawn + DishonourAmnt)) AS CB";
            sSql = sSql + " from TempCountryWideStock";
            sSql = sSql + " WHERE UserID='" + Session["sUser"].ToString() + "' AND PCName='" + sPC + "'";
            sSql = sSql + " Group by ZoneName";


            //SqlCommand cmd = new SqlCommand(sSql, conn1);
            ////OleDbDataReader dr = cmd.ExecuteReader();

            //DataSet ds = new DataSet();
            //SqlDataAdapter da = new SqlDataAdapter(cmd);
            //da.Fill(ds);

            //gv_Zone_statement.DataSource = ds;
            //gv_Zone_statement.DataBind();
            ////dr.Close();
            //conn1.Close();

            dt = new DataTable();
            dt = ExecuteReturnDataTable(sSql, conn1);
            gv_countryWise.DataSource = dt;
            gv_countryWise.DataBind();


        }





        if (btn_radio_salesdetails_brandcat.Checked)
        {
            string querySql = "";
            querySql = @"select m.MRSRCode,df.Name,p.PCategory as Brand,cat.CatName as Catagory,p.Model,
                                 CONVERT(varchar(12), m.TDate, 105) AS Date,md.UnitPrice,abs(md.Qty) as Qty,md.TotalAmnt,md.DiscountAmnt,md.NetAmnt from 
                                  VW_Delear_Info df inner join MRSRMaster m on df.DAID=m.InSource inner join MRSRDetails md
                                  on m.MRSRMID=md.MRSRMID inner join Product p on p.ProductID=md.ProductID 
                                  inner join Category cat on p.CategoryID=cat.CategoryID 
                                  where TrType='3'
                                  and df.DAID In(select Dealer from dbCID.dbo.ZoneWisDealer where Zone in(" + zoneIds + ")) and m.TDate >= '" + Convert.ToDateTime(this.txtFrom.Text) + "' and m.TDate <= '" + Convert.ToDateTime(this.txtToDate.Text) + "' order by m.MRSRCode desc";

            //SqlCommand cmd = new SqlCommand(querySql, conn1);


            //DataSet ds = new DataSet();
            //SqlDataAdapter da = new SqlDataAdapter(cmd);
            //da.Fill(ds);

            //gv_brand_cat.DataSource = ds;
            //gv_brand_cat.DataBind();

            //conn1.Close();

            dt = new DataTable();
            dt = ExecuteReturnDataTable(querySql, conn1);
            gv_brand_cat.DataSource = dt;
            gv_brand_cat.DataBind();


        }


    }

    //ViewState[""]

    public static List<string> getCheckdValues(TreeNodeCollection nodes)
    {
        try
        {

            List<string> values = new List<string>();
            foreach (TreeNode tn in nodes)
            {
                if (tn.Checked)
                {
                    if (values.Count == 0)
                        HttpContext.Current.Session["__ZoneNameForReport__"] = tn.Text.Trim();
                    values.Add(tn.Value);

                }
                if (tn.ChildNodes.Count > 0)
                    getChildValues(tn, values);
            }
            return values;
        }
        catch (Exception ex) { return null; }

    }


    private static void getChildValues(TreeNode tn, List<string> values)
    {

        foreach (TreeNode cn in tn.ChildNodes)
        {
            if (cn.Checked)
            {
                if (values.Count == 0)
                    HttpContext.Current.Session["__ZoneNameForReport__"] = cn.Text.Trim();
                values.Add(cn.Value);


            }
            if (cn.ChildNodes.Count > 0)
                getChildValues(cn, values);
        }
    }







    //export data


    private void BindData()
    {
        getdataforExport();
    }


    //Export to Excel Button
    protected void btn_export_excel_Click(object sender, EventArgs e)
    {
        if (btn_radio_sales_delivary.Checked)
            Export_Excel(gv_sales_delivery);
        else if (btn_radio_deposit.Checked)
            Export_Excel(gv_deposit);
        else if (btn_radio_sales_withdrawn.Checked)
            Export_Excel(gv_sales_withd);
        else if (btn_radio_salesdetails_brandcat.Checked)
            Export_Excel(gv_brand_cat);
        else if (btn_radio_zonewise_sales.Checked)
            Export_Excel(gv_Zone_statement);
        else if (btn_radio_country_dealer_stmnt.Checked)
            Export_Excel(gv_countryWise);

    }

    //rendermethod control for export pdf and excel
    public override void VerifyRenderingInServerForm(Control control)
    {

    }


    //Export to Pdf Button
    protected void btn_export_pdf_Click(object sender, EventArgs e)
    {
        if (btn_radio_sales_delivary.Checked)
            Export_pdf(gv_sales_delivery);
        else if (btn_radio_deposit.Checked)
            Export_pdf(gv_deposit);
        else if (btn_radio_sales_withdrawn.Checked)
            Export_pdf(gv_sales_withd);
        else if (btn_radio_salesdetails_brandcat.Checked)
            Export_pdf(gv_brand_cat);
        else if (btn_radio_zonewise_sales.Checked)
            Export_pdf(gv_Zone_statement);
        else if (btn_radio_country_dealer_stmnt.Checked)
            Export_pdf(gv_countryWise);

    }







    //footer load sales delivery
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CalcTotal_TP(e.Row.Cells[6].Text);
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[3].Text = "Total Pay";
            e.Row.Cells[6].Text = e.Row.Cells[6].Text = runningTotalTP.ToString("0,0", CultureInfo.InvariantCulture);

            e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Left;
        }
    }

    //footer load for deposit
    protected void gv_deposit_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CalcTotal_Card(e.Row.Cells[6].Text);
        }

        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[2].Text = "Total";
            e.Row.Cells[6].Text = runningTotalCard.ToString("0,0", CultureInfo.InvariantCulture);


            //ALIGNMENT

            e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Center;

        }
    }

    //footer load for withdrawn

    protected void gv_sales_withd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CalcTotal_TP(e.Row.Cells[5].Text);
        }

        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[2].Text = "Total";
            e.Row.Cells[5].Text = runningTotalTP.ToString("0,0", CultureInfo.InvariantCulture);


            //ALIGNMENT

            e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Center;

        }
    }

    //footer load for catagory and brandwise
    protected void gv_brand_cat_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Unit_Price(e.Row.Cells[7].Text);
            CalcTotal_Qty(e.Row.Cells[8].Text);
            CalcTotal_TP(e.Row.Cells[9].Text);
            CalcTotal_NetAmount(e.Row.Cells[11].Text);


        }

        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[1].Text = "Total";
            e.Row.Cells[7].Text = runningTotalUnit.ToString("0,0", CultureInfo.InvariantCulture);
            e.Row.Cells[8].Text = runningTotalQty.ToString("0,0", CultureInfo.InvariantCulture);
            e.Row.Cells[9].Text = runningTotalTP.ToString("0,0", CultureInfo.InvariantCulture);
            e.Row.Cells[11].Text = runningTotalNetAmnt.ToString("0,0", CultureInfo.InvariantCulture);


            //ALIGNMENT

            e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Center;

        }
    }

    //footer load for zonewisesales
    protected void gv_Zone_statement_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CalcTotal_OpeningBalance(e.Row.Cells[2].Text);
            CalcTotal_SalesAmount(e.Row.Cells[3].Text);
            CalcTotal_WidthAmount(e.Row.Cells[4].Text);
            CalcTotal_ActualSales(e.Row.Cells[5].Text);
            CalcTotal_BankDeposit(e.Row.Cells[6].Text);
            CalcTotal_ClosingBalance(e.Row.Cells[8].Text);


        }

        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[1].Text = "Total";
            e.Row.Cells[2].Text = runningOpeningBalance.ToString("0,0", CultureInfo.InvariantCulture);
            e.Row.Cells[3].Text = runningSalesAmount.ToString("0,0", CultureInfo.InvariantCulture);
            e.Row.Cells[4].Text = runningWithdrawnAmount.ToString("0,0", CultureInfo.InvariantCulture);
            e.Row.Cells[5].Text = runningActualSales.ToString("0,0", CultureInfo.InvariantCulture);
            e.Row.Cells[6].Text = runningBankDeposit.ToString("0,0", CultureInfo.InvariantCulture);
            e.Row.Cells[8].Text = runningClosingBalance.ToString("0,0", CultureInfo.InvariantCulture);



            //ALIGNMENT

            e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Center;

        }

    }


    protected void gv_countryWise_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CalcTotal_OpeningBalance(e.Row.Cells[2].Text);
            CalcTotal_SalesAmount(e.Row.Cells[3].Text);
            CalcTotal_WidthAmount(e.Row.Cells[4].Text);
            CalcTotal_ActualSales(e.Row.Cells[5].Text);
            CalcTotal_BankDeposit(e.Row.Cells[6].Text);
            CalcTotal_ClosingBalance(e.Row.Cells[8].Text);


        }

        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[1].Text = "Total";
            e.Row.Cells[2].Text = runningOpeningBalance.ToString("0,0", CultureInfo.InvariantCulture);
            e.Row.Cells[3].Text = runningSalesAmount.ToString("0,0", CultureInfo.InvariantCulture);
            e.Row.Cells[4].Text = runningWithdrawnAmount.ToString("0,0", CultureInfo.InvariantCulture);
            e.Row.Cells[5].Text = runningActualSales.ToString("0,0", CultureInfo.InvariantCulture);
            e.Row.Cells[6].Text = runningBankDeposit.ToString("0,0", CultureInfo.InvariantCulture);
            e.Row.Cells[8].Text = runningClosingBalance.ToString("0,0", CultureInfo.InvariantCulture);



            //ALIGNMENT

            e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Center;

        }

    }



    //calculate totalpay for zonewise sales delevery
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

    //calculate totalcard pay for deposit

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

    //unitprice
    private void Unit_Price(string _price)
    {
        try
        {
            runningTotalUnit += Double.Parse(_price);
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write(ex.ToString());
        }
    }
    //Net Amount
    private void CalcTotal_NetAmount(string _price)
    {
        try
        {
            runningTotalNetAmnt += Double.Parse(_price);
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write(ex.ToString());
        }
    }

    //runnigquanity
    private void CalcTotal_Qty(string _price)
    {
        try
        {
            runningTotalQty += Double.Parse(_price);
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write(ex.ToString());
        }
    }

    //zonewisesals Statement
    private void CalcTotal_OpeningBalance(string _price)
    {
        try
        {
            runningOpeningBalance += Double.Parse(_price);
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write(ex.ToString());
        }
    }

    private void CalcTotal_SalesAmount(string _price)
    {
        try
        {
            runningSalesAmount += Double.Parse(_price);
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write(ex.ToString());
        }
    }

    private void CalcTotal_WidthAmount(string _price)
    {
        try
        {
            runningWithdrawnAmount += Double.Parse(_price);
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write(ex.ToString());
        }
    }

    private void CalcTotal_ActualSales(string _price)
    {
        try
        {
            runningActualSales += Double.Parse(_price);
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write(ex.ToString());
        }
    }

    private void CalcTotal_BankDeposit(string _price)
    {
        try
        {
            runningBankDeposit += Double.Parse(_price);
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write(ex.ToString());
        }
    }

    private void CalcTotal_ClosingBalance(string _price)
    {
        try
        {
            runningClosingBalance += Double.Parse(_price);
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

        Divpoint.RenderControl(htw);
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

        Divpoint.RenderControl(hw);
        //gvPdf.RenderControl(hw);

        //gvPdf.HeaderRow.Style.Add("width", "25%");
        //gvPdf.HeaderRow.Style.Add("font-size", "40px");

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




    //ZoneWise dealer Statement

    private void fnLoadDealerStatement()
    {
        List<string> myList = getCheckdValues(tree_view.Nodes);

        string zoneIds = string.Join(",", myList.ToArray());


        getConnectiondealer();

        string gSQL = "";

        string fromDate = DateTime.Today.ToString("MM/dd/yyyy");
        string toDate = DateTime.Today.ToString("MM/dd/yyyy");

        string sPC = Request.UserHostAddress;

        if (Session["sUser"] == null)
        {
            Session["sUser"] = "0";
        }
        if (Session["sZoneID"] == null)
        {
            Session["sZoneID"] = "0";
        }

        gSQL = "";
        gSQL = "DELETE FROM TempOpening";
        gSQL = gSQL + " WHERE UserID='" + Session["sUser"].ToString() + "' AND PCName='" + sPC + "'";
        SqlCommand cmd2 = new SqlCommand(gSQL, conn1);
        getConnectiondealer();
        cmd2.ExecuteNonQuery();
        conn1.Close();

        //'----------------------------------------------------------------------------
        //'LOAD DEALER NAME ZONE WISE
        gSQL = "";
        gSQL = @"select zd.Dealer as DAID, df.Name as DelearName,zd.Zone as ZoneId,do.ZoneName from  dbCID.dbo.ZonewisDealer zd 
inner join dbo.DelearInfo df on zd.Dealer=df.DAID inner join dbCID.dbo.DealerOrganogram do on zd.Zone=do.ZoneId

where df.Discontinue='No' and zd.Zone in(" + zoneIds + ") ORDER BY df.Name";

        getConnectiondealer();
        SqlCommand cmd = new SqlCommand(gSQL, conn1);

        SqlDataReader dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            string OpeningSalesAmnt = getOppeningSalse(dr["DAID"].ToString());
            string OpDepositAmnt = getOpeningDeposit(dr["DAID"].ToString());
            string OpeningDishonorAmnt = getOpeningDisHonour(dr["DAID"].ToString());
            string OpWithdrawnAmnt = getOpeningWithdrawn(dr["DAID"].ToString());
            string CurrentgSalesAmnt = getCurrentSales(dr["DAID"].ToString());
            string CurrentDepositAmnt = getCurrentCollection(dr["DAID"].ToString());
            string CurrentgDishonorAmnt = getCurrentDisHonour(dr["DAID"].ToString());
            string CurrentWithdrawnAmnt = getCurrentWithdrawn(dr["DAID"].ToString());

            gSQL = @"INSERT INTO TempOpening(ZoneName,DelearName,DelearID,[OpeningSalesAmnt],[OpeningCollection],
                 [OpenigWithdrawn],[OpeningDishonour],[SalesAmnt],[Collection],[Withdrawn],[DishonourAmnt],UserID,PCName) 
            VALUES('" + dr["ZoneId"].ToString() + "','" + dr["DelearName"].ToString() + "','" + dr["DAID"].ToString() +
                     "'," + OpeningSalesAmnt + "," + OpDepositAmnt + "," + OpWithdrawnAmnt + "," + OpeningDishonorAmnt + "," + CurrentgSalesAmnt + "," + CurrentDepositAmnt + "," + CurrentWithdrawnAmnt + "," + CurrentgDishonorAmnt + ",'" + Session["sUser"].ToString() + "','" + sPC + "')";
            cmd2 = new SqlCommand(gSQL, conn2);
            getConnectiondealer2();
            cmd2.ExecuteNonQuery();
            conn2.Close();
            //dr.Dispose();
        }
        conn1.Close();





    }


    private void fnLoadDealerStatement_()
    {
        List<string> myList = getCheckdValues(tree_view.Nodes);
        Session["__ZoneNameForReport__"].ToString();
        string zoneIds = string.Join(",", myList.ToArray());


        getConnectiondealer();

        string gSQL = "";

        string fromDate = DateTime.Today.ToString("MM/dd/yyyy");
        string toDate = DateTime.Today.ToString("MM/dd/yyyy");

        string sPC = Request.UserHostAddress;

        if (Session["sUser"] == null)
        {
            Session["sUser"] = "0";
        }
        if (Session["sZoneID"] == null)
        {
            Session["sZoneID"] = "0";
        }

        gSQL = "";
        gSQL = "DELETE FROM TempCountryWideStock";
        gSQL = gSQL + " WHERE UserID='" + Session["sUser"].ToString() + "' AND PCName='" + sPC + "'";
        SqlCommand cmd2 = new SqlCommand(gSQL, conn1);
        getConnectiondealer();
        cmd2.ExecuteNonQuery();
        conn1.Close();

        //'----------------------------------------------------------------------------
        //'LOAD DEALER NAME ZONE WISE
        gSQL = "";
        gSQL = @"select zd.Dealer as DAID, df.Name as DelearName,zd.Zone as ZoneId,do.ZoneName from  dbCID.dbo.ZonewisDealer zd 
inner join dbo.DelearInfo df on zd.Dealer=df.DAID inner join dbCID.dbo.DealerOrganogram do on zd.Zone=do.ZoneId

where df.Discontinue='No' and zd.Zone in(" + zoneIds + ") ORDER BY df.Name";

        getConnectiondealer();
        SqlCommand cmd = new SqlCommand(gSQL, conn1);

        SqlDataReader dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            string OpeningSalesAmnt = getOppeningSalse(dr["DAID"].ToString());
            string OpDepositAmnt = getOpeningDeposit(dr["DAID"].ToString());
            string OpeningDishonorAmnt = getOpeningDisHonour(dr["DAID"].ToString());
            string OpWithdrawnAmnt = getOpeningWithdrawn(dr["DAID"].ToString());
            string CurrentgSalesAmnt = getCurrentSales(dr["DAID"].ToString());
            string CurrentDepositAmnt = getCurrentCollection(dr["DAID"].ToString());
            string CurrentgDishonorAmnt = getCurrentDisHonour(dr["DAID"].ToString());
            string CurrentWithdrawnAmnt = getCurrentWithdrawn(dr["DAID"].ToString());

            gSQL = @"INSERT INTO TempCountryWideStock(ZoneName,DelearName,DelearID,[OpeningSalesAmnt],[OpeningCollection],
                 [OpenigWithdrawn],[OpeningDishonour],[SalesAmnt],[Collection],[Withdrawn],[DishonourAmnt],UserID,PCName) 
            VALUES('" + dr["ZoneName"].ToString() + "','" + dr["DelearName"].ToString() + "','" + dr["DAID"].ToString() +
                     "'," + OpeningSalesAmnt + "," + OpDepositAmnt + "," + OpWithdrawnAmnt + "," + OpeningDishonorAmnt + "," + CurrentgSalesAmnt + "," + CurrentDepositAmnt + "," + CurrentWithdrawnAmnt + "," + CurrentgDishonorAmnt + ",'" + Session["sUser"].ToString() + "','" + sPC + "')";
            cmd2 = new SqlCommand(gSQL, conn2);
            getConnectiondealer2();
            cmd2.ExecuteNonQuery();
            conn2.Close();
            //dr.Dispose();
        }
        conn1.Close();





    }






    private string getOppeningSalse(string DAID)
    {
        try
        {
            // getConnectiondealer2();
            DataTable dt = new DataTable();
            dt = ExecuteReturnDataTable(@"SELECT ISNULL( SUM(ISNULL(dbo.MRSRMaster.NetSalesAmnt, 0)),0) AS NetSalesAmnt 
                FROM dbo.MRSRMaster INNER JOIN  dbo.DelearInfo ON dbo.MRSRMaster.InSource = dbo.DelearInfo.DAID 
                Left Join dbCID.dbo.ZonewisDealer zd on  dbo.DelearInfo.DAID =zd.Dealer 
                Where (dbo.MRSRMaster.TrType = 3) AND dbo.MRSRMaster.TDate<'" + Convert.ToDateTime(this.txtFrom.Text) + @"' AND  dbo.DelearInfo.DAID =" + DAID, conn2);
            return dt.Rows[0]["NetSalesAmnt"].ToString();

        }
        catch (Exception e) { return "0"; }
    }

    private string getOpeningDeposit(string DAID)
    {
        try
        {
            // getConnectiondealer2();
            DataTable dt = new DataTable();
            dt = ExecuteReturnDataTable(@"SELECT DelearID, ISNULL(SUM(cAmount),0) AS cAmount From dbo.VW_Deposit_Info 
            WHERE DelearID='" + DAID + "' And CDate<'" + Convert.ToDateTime(this.txtFrom.Text) + "' GROUP BY DelearID", conn2);
            return dt.Rows[0]["cAmount"].ToString();
        }
        catch (Exception ex)
        {
            return "0";
        }
    }

    private string getOpeningDisHonour(string DAID)
    {
        try
        {
            //getConnectiondealer2();
            DataTable dt = new DataTable();
            dt = ExecuteReturnDataTable(@"SELECT DelearID, SUM(cAmount) AS cAmount 
                  From dbo.VW_DishonourAmnt WHERE DelearID='" + DAID + "' And CDate<'" + Convert.ToDateTime(this.txtFrom.Text) + "' GROUP BY DelearID", conn2);
            return dt.Rows[0]["cAmount"].ToString();
        }
        catch (Exception ex)
        {
            return "0";
        }

    }

    private string getOpeningWithdrawn(string DAID)
    {
        try
        {
            //getConnectiondealer2();
            DataTable dt = new DataTable();
            dt = ExecuteReturnDataTable(@"SELECT SUM(ISNULL(dbo.MRSRMaster.NetSalesAmnt, 0)) AS NetSalesAmnt,dbo.DelearInfo.DAID,dbo.DelearInfo.Name,zd.Zone as ZoneName 
                FROM dbo.MRSRMaster INNER JOIN  dbo.DelearInfo ON dbo.MRSRMaster.OutSource = dbo.DelearInfo.DAID 
                Left Join dbCID.dbo.ZonewisDealer zd on  dbo.DelearInfo.DAID =zd.Dealer  Where (dbo.MRSRMaster.TrType = -3) And dbo.DelearInfo.DAID='" + DAID + @"'
                AND dbo.MRSRMaster.TDate<'" + Convert.ToDateTime(this.txtFrom.Text) + @"'  
                GROUP BY dbo.DelearInfo.DAID, dbo.DelearInfo.Name, zd.Zone", conn2);
            return dt.Rows[0]["NetSalesAmnt"].ToString();

        }
        catch (Exception ex)
        {
            return "0";
        }
    }


    private string getCurrentSales(string DAID)
    {
        try
        {
            //getConnectiondealer2();
            DataTable dt = new DataTable();
            dt = ExecuteReturnDataTable(@"SELECT SUM(ISNULL(dbo.MRSRMaster.NetSalesAmnt, 0)) AS NetSalesAmnt,dbo.DelearInfo.DAID,dbo.DelearInfo.Name,zd.Zone as ZoneName 
                FROM dbo.MRSRMaster INNER JOIN  dbo.DelearInfo ON dbo.MRSRMaster.InSource = dbo.DelearInfo.DAID 
                Left Join dbCID.dbo.ZonewisDealer zd on  dbo.DelearInfo.DAID =zd.Dealer  Where (dbo.MRSRMaster.TrType = 3) 
                And dbo.DelearInfo.DAID ='" + DAID + @"'
                AND dbo.MRSRMaster.TDate>='" + Convert.ToDateTime(this.txtFrom.Text) + @"' 
                AND dbo.MRSRMaster.TDate<='" + Convert.ToDateTime(this.txtToDate.Text) + @"' 
                GROUP BY dbo.DelearInfo.DAID, dbo.DelearInfo.Name, zd.Zone", conn2);
            return dt.Rows[0]["NetSalesAmnt"].ToString();

        }
        catch (Exception e) { return "0"; }
    }


    private string getCurrentCollection(string DAID)
    {
        try
        {
            //getConnectiondealer2();
            DataTable dt = new DataTable();
            dt = ExecuteReturnDataTable(@"SELECT DelearID, SUM(cAmount) AS cAmount From dbo.VW_Deposit_Info WHERE DelearID='" + DAID + "' And CDate>='" + Convert.ToDateTime(this.txtFrom.Text) + @"'
          AND CDate<='" + Convert.ToDateTime(this.txtToDate.Text) + "'GROUP BY DelearID", conn2);
            return dt.Rows[0]["cAmount"].ToString();
        }
        catch (Exception e) { return "0"; }
    }

    private string getCurrentDisHonour(string DAID)
    {
        try
        {
            // getConnectiondealer2();
            DataTable dt = new DataTable();
            dt = ExecuteReturnDataTable(@"SELECT DelearID,SUM(cAmount) AS cAmount From dbo.VW_DishonourAmnt WHERE DelearID='" + DAID + "' And CDate>='" + Convert.ToDateTime(this.txtFrom.Text) + @"'
              AND CDate<='" + Convert.ToDateTime(this.txtToDate.Text) + "'GROUP BY DelearID", conn2);
            return dt.Rows[0]["cAmount"].ToString();
        }
        catch (Exception e) { return "0"; }
    }

    private string getCurrentWithdrawn(string DAID)
    {
        try
        {
            //getConnectiondealer2();
            DataTable dt = new DataTable();
            dt = ExecuteReturnDataTable(@"SELECT SUM(ISNULL(dbo.MRSRMaster.NetSalesAmnt, 0)) AS NetSalesAmnt,dbo.DelearInfo.DAID,dbo.DelearInfo.Name,
                zd.Zone as ZoneName 
                FROM dbo.MRSRMaster INNER JOIN  dbo.DelearInfo ON dbo.MRSRMaster.OutSource = dbo.DelearInfo.DAID 
                Left Join dbCID.dbo.ZonewisDealer zd on  dbo.DelearInfo.DAID =zd.Dealer  Where (dbo.MRSRMaster.TrType = -3) And dbo.DelearInfo.DAID ='" + DAID + @"' 
                AND dbo.MRSRMaster.TDate>='" + Convert.ToDateTime(this.txtFrom.Text) + "'  AND dbo.MRSRMaster.TDate<='" + Convert.ToDateTime(this.txtToDate.Text) + @"' 
                GROUP BY dbo.DelearInfo.DAID, dbo.DelearInfo.Name, zd.Zone", conn2);
            return dt.Rows[0]["NetSalesAmnt"].ToString();
        }
        catch (Exception e) { return "0"; }
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


    public string getReporName()
    {

        if (btn_radio_zonewise_sales.Checked)
        {
            repotname.InnerText = btn_radio_zonewise_sales.Text.Trim();
        }
        else if (btn_radio_deposit.Checked)
        {
            repotname.InnerText = btn_radio_deposit.Text.Trim();
        }
        else if (btn_radio_salesdetails_brandcat.Checked)
        {
            repotname.InnerText = btn_radio_salesdetails_brandcat.Text.Trim();
        }
        else if (btn_radio_sales_delivary.Checked)
        {
            repotname.InnerText = btn_radio_sales_delivary.Text.Trim();
        }
        else if (btn_radio_sales_withdrawn.Checked)
        {
            repotname.InnerText = btn_radio_sales_withdrawn.Text.Trim();
        }
        else if (btn_radio_country_dealer_stmnt.Checked)
        {
            repotname.InnerText = btn_radio_country_dealer_stmnt.Text.Trim();
        }
        return "";
    }

    //get userid using currentlogin username
    private int getUserName(string username)
    {
        int UserId=0;
        string query = "SELECT Id FROM Softuser WHERE UserName = '" + username + "'";
        using (SqlConnection conn = DBConnection.GetConnection())
        {
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                try
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            UserId = Convert.ToInt32(reader["Id"]);
                        }
                        reader.Close();
                    }
                }
                catch (Exception ex) 
                 { 
                
                 }
                
                
            }
            
        }

        return UserId;
        
    }


    private string getPermissionzone(int UserId) 
     {

        string permissionId="";
        using (SqlConnection conn = DBConnection.GetConnection())
        {

            string query = "select PermissionZone from DealerRole  where UserId='" + UserId + "'";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        permissionId = reader["PermissionZone"].ToString();


                    }
                }
            }
            return permissionId;
        }

     }







  }




