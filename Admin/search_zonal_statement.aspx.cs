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

using System.Net.Mail;

public partial class search_zonal_statement : System.Web.UI.Page
{
    SqlConnection conn = DBConnectionDSM.GetConnection();
    long i;

    int iMRSRID = 0;
    DataTable dt;
    DateTime tDate;


    private double runningTotal = 0;
    private double runningTotalTP = 0;
    private double runningTotalDis = 0;
    private double runningTotalWith = 0;
    private double runningTotalQty = 0;

    private double runningTotalCash = 0;
    private double runningTotalCard = 0;
    private double runningTotal6 = 0;
    private double runningTotal7 = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (System.Convert.ToInt32(Session["Vis"]) == 0)
        {
            Response.Redirect("../Account/Login.aspx");
        }
        int role = Convert.ToInt32(Session["RolesId"]);

        if (role != 1)
        {
            Response.Redirect("~/Login.aspx");
        }
        if (!IsPostBack)
        {
            this.txtFrom.Text = DateTime.Today.ToString("MM/dd/yyyy");
            this.txtToDate.Text = DateTime.Today.ToString("MM/dd/yyyy");

            //LOAD CTP
            //LoadDropDownList_CTP();

            //LoadDropDownList_Dealer();
            LoadDropDownList_DealerMkt();

            dt = new DataTable();
            MakeTable();

        }

        else
        {
            dt = (DataTable)ViewState["dt"];
        }
        ViewState["dt"] = dt;


    }

    //LOAD CTP IN DROPDOWN LIST
    protected void LoadDropDownList_DealerMkt()
    {
        SqlConnection conn = DBConnection.GetConnection();

        String strQuery = "select EID,eName from Entity ";
        strQuery = strQuery + " WHERE (ActiveDeactive = 1) AND";
        strQuery = strQuery + " (EntityType = 'Dealer')";
        strQuery = strQuery + " ORDER BY eName";
        //SqlConnection con = new SqlConnection("conn");
        SqlCommand cmd = new SqlCommand(strQuery, conn);
        //SqlCommand cmd = new SqlCommand();
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = strQuery;
        cmd.Connection = conn;
        try
        {
            conn.Open();
            ddlEntity.DataSource = cmd.ExecuteReader();
            ddlEntity.DataTextField = "eName";
            ddlEntity.DataValueField = "EID";
            ddlEntity.DataBind();

            //Add blank item at index 0.
            ddlEntity.Items.Insert(0, new ListItem("ALL", "ALL"));

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

    //LOAD Dealer IN DROPDOWN LIST
    protected void LoadDropDownList_Dealer()
    {
        SqlConnection conn = DBConnectionDSM.GetConnection();

        String strQuery = "SELECT CategoryID,DAID,Name,ZoneName FROM VW_Delear_Info ";
        strQuery = strQuery + " WHERE (Discontinue = 'No')";
        //strQuery = strQuery + " AND (CategoryID='" + Session["sZoneID"].ToString() + "')";
        strQuery = strQuery + " ORDER BY Name";
        //SqlConnection con = new SqlConnection("conn");
        SqlCommand cmd = new SqlCommand(strQuery, conn);
        //SqlCommand cmd = new SqlCommand();
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = strQuery;
        cmd.Connection = conn;
        //try
        //{
        conn.Open();
        ddlEntity.DataSource = cmd.ExecuteReader();
        ddlEntity.DataTextField = "Name";
        ddlEntity.DataValueField = "DAID";
        ddlEntity.DataBind();

        //Add blank item at index 0.
        //ddlEntity.Items.Insert(0, new ListItem("", "0"));
        ddlEntity.Items.Insert(0, new ListItem("ALL", "ALL"));
        //ddlDealerName.Items.Insert(1, new ListItem("CI&DD (REL)", "370"));

        //}
        //catch (Exception ex)
        //{
        //    throw ex;
        //}
        //finally
        //{
        //    conn.Close();
        //    conn.Dispose();
        //}
    }

   
    protected void PopupMessage(string Msg, Control controlID)
    {
        ScriptManager.RegisterClientScriptBlock(controlID, controlID.GetType(), "msg", "alert('" + Msg + "');", true);
    }

    protected void SearchData(object sender, EventArgs e)
    {
        //LOAD DATA IN GRID
        fnLoadData();
    }


    //LOAD SALES SUMMARY CHALLAN WISE
    private void fnLoadData()
    {

        //-----------------------------------------------------------------------------------------
        //CHECK FOR VAT USER
        if (Session["iVatUser"].ToString() == "1")
        {
            if (Convert.ToDateTime(this.txtFrom.Text) < Convert.ToDateTime(Session["eFYSDate"]))
            {
                this.txtFrom.Text = Session["ssFYSDate"].ToString();
            }
        }
        //-----------------------------------------------------------------------------------------

        //s = WebConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        //con = new SqlConnection(s);
        SqlConnection conn = DBConnectionDSM.GetConnection();
        conn.Open();

        string sSql = "";
        //sSql = "";
        //sSql = "SELECT dbo.MRSRMaster.MRSRCode, CONVERT(varchar(12), dbo.MRSRMaster.TDate, 101) AS TDate, dbo.MRSRMaster.TrType,";
        //sSql = sSql + " dbo.VW_Delear_Info.Name AS InSource, dbo.VW_Delear_Info.ZoneName, dbo.Zone.CatName AS OutSource, ";
        //sSql = sSql + " dbo.VW_Delear_Info.Address, dbo.VW_Delear_Info.DealerStatus, dbo.VW_Delear_Info.ContactNo,";
        //sSql = sSql + " dbo.VW_Delear_Info.ContactPerson, dbo.VW_Delear_Info.Code, dbo.VW_Delear_Info.DAID, ";
        //sSql = sSql + " dbo.VW_Delear_Info.EmailAdd, dbo.VW_Delear_Info.ZoneType, dbo.MRSRMaster.POCode,";
        //sSql = sSql + " dbo.MRSRMaster.OnLineSales, dbo.MRSRMaster.TermsCondition, dbo.MRSRMaster.Remarks, ";
        //sSql = sSql + " dbo.MRSRMaster.MRSRMID, dbo.MRSRMaster.NetSalesAmnt, dbo.MRSRMaster.SaleDeclar,";
        //sSql = sSql + " CASE dbo.MRSRMaster.SaleDeclar WHEN 1 THEN 'Declared' ELSE 'Pending' END AS sStatus";
        //sSql = sSql + " FROM dbo.VW_Delear_Info INNER JOIN";
        //sSql = sSql + " dbo.MRSRMaster ON dbo.VW_Delear_Info.DAID = dbo.MRSRMaster.InSource INNER JOIN";
        //sSql = sSql + " dbo.Zone ON dbo.MRSRMaster.OutSource = dbo.Zone.CategoryID";

        //sSql = sSql + " WHERE (dbo.MRSRMaster.TrType = 3) AND (dbo.Zone.Code = '" + Session["sBrCode"].ToString() + "') ";

        //if (txtInvNo.Text.Length > 0)
        //{
        //    sSql = sSql + " AND (dbo.MRSRMaster.MRSRCode LIKE '%" + txtInvNo.Text + "%'";
        //    sSql = sSql + " OR dbo.MRSRMaster.POCode LIKE '%" + txtInvNo.Text + "%'";
        //    sSql = sSql + " OR dbo.VW_Delear_Info.ContactNo LIKE '%" + txtInvNo.Text + "%')";
        //}
        //else
        //{

        //    sSql = sSql + " AND (dbo.MRSRMaster.TDate >= '" + Convert.ToDateTime(this.txtFrom.Text) + "'";
        //    sSql = sSql + " AND dbo.MRSRMaster.TDate <= '" + Convert.ToDateTime(this.txtToDate.Text) + "')";

        //    if (this.ddlEntity.SelectedItem.Text != "ALL")
        //    {
        //        sSql = sSql + " AND (dbo.VW_Delear_Info.Name ='" + this.ddlEntity.SelectedItem.Text + "')";
        //    }
        //}
        
        //sSql = sSql + " ORDER BY dbo.MRSRMaster.TDate, dbo.MRSRMaster.MRSRCode DESC";

        string sPC = Request.UserHostAddress;

        //LOAD DATA
        fnLoadTotalOutStanding();

        //LOAD SUMMARY
        sSql = "";
        ////sSql = "select ZoneName,DelearName,DelearID,";
        ////sSql = sSql + " (OpeningSalesAmnt - OpeningCollection - OpenigWithdrawn + OpeningDishonour)  AS OB,";
        ////sSql = sSql + " SalesAmnt,Collection,Withdrawn,DishonourAmnt,";
        ////sSql = sSql + " ((OpeningSalesAmnt - OpeningCollection - OpenigWithdrawn + OpeningDishonour) + (SalesAmnt - Collection - Withdrawn + DishonourAmnt)) AS CB";
        ////sSql = sSql + " from TempOpening";
        ////sSql = sSql + " WHERE UserID='" + Session["sUser"].ToString() + "' AND PCName='" + sPC + "'";

        double dOB = 0;
        double dCB = 0;
        double ddCB = 0;
        //LOAD TOTAL
        sSql = "";
        sSql = "SELECT ZoneName,UserID, SUM(OpeningSalesAmnt) AS OpeningSalesAmnt, SUM(OpeningCollection) AS OpeningCollection,";
        sSql = sSql + " SUM(OpenigWithdrawn) AS OpenigWithdrawn, SUM(OpeningDishonour) AS OpeningDishonour,";
        sSql = sSql + " SUM(OpeningSalesAmnt)- SUM(OpeningCollection) - SUM(OpenigWithdrawn) + SUM(OpeningDishonour) AS OB ,";
        sSql = sSql + " SUM(SalesAmnt) AS SalesAmnt, SUM(Collection) AS Collection, SUM(Withdrawn) AS Withdrawn,";
        sSql = sSql + " SUM(DishonourAmnt) AS DishonourAmnt,";
        sSql = sSql + " (SUM(OpeningSalesAmnt)- SUM(OpeningCollection) - SUM(OpenigWithdrawn) + SUM(OpeningDishonour) + SUM(SalesAmnt) - SUM(Collection) - SUM(Withdrawn) + SUM(DishonourAmnt)) AS CB";
        sSql = sSql + " FROM dbo.TempOpening";
        sSql = sSql + " WHERE UserID='" + Session["sUser"].ToString() + "' AND PCName='" + sPC + "'";
        sSql = sSql + " GROUP BY ZoneName, UserID";

        ////cmd = new SqlCommand(gSQL, conn);
        ////conn.Open();
        ////dr = cmd.ExecuteReader();
        ////if (dr.Read())
        ////{
        ////    dOB = Convert.ToDouble(dr["OpeningSalesAmnt"].ToString()) - Convert.ToDouble(dr["OpeningCollection"].ToString()) - Convert.ToDouble(dr["OpenigWithdrawn"].ToString()) + Convert.ToDouble(dr["OpeningDishonour"].ToString());
        ////    dCB = dOB + Convert.ToDouble(dr["SalesAmnt"].ToString()) - Convert.ToDouble(dr["Collection"].ToString()) - Convert.ToDouble(dr["Withdrawn"].ToString()) + Convert.ToDouble(dr["DishonourAmnt"].ToString());
        ////    ddCB = dCB / 10000000;
        ////    //lblOutstanding.Text = Convert.ToString(dCB / 100000) + " L";
        ////    lblOutstanding.Text = ddCB.ToString("00.00", CultureInfo.InvariantCulture) + "C";

        ////}
        ////conn.Close();

        SqlCommand cmd = new SqlCommand(sSql, conn);        
        //OleDbDataReader dr = cmd.ExecuteReader();

        DataSet ds = new DataSet();
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        da.Fill(ds);

        GridView1.DataSource = ds;
        GridView1.DataBind();
        //dr.Close();
        conn.Close();

    }


    protected void fnLoadTotalOutStanding()
    {
        if (System.Convert.ToInt32(Session["Vis"]) == 0)
        {
            Response.Redirect("~/LogIn.aspx");
        }

        SqlConnection conn = DBConnectionDSM.GetConnection();
        SqlConnection conn2 = DBConnectionDSM.GetConnection();

        string gSql = "";

        ////---------------------------------------------------------------------------------------------
        //// WITHDRAWN AMOUNT
        //gSql = "";
        //gSql = "SELECT TrType, InSource, SUM(NetSalesAmnt)/100000 AS NetAmnt";
        ////gSql = gSql + " PARSENAME(CONVERT(VARCHAR, CAST(SUM(NetSalesAmnt)/100000  AS NetAmnt), 1),2)";
        //gSql = gSql + " FROM dbo.MRSRMaster";

        //gSql = gSql + " WHERE (TrType = -3)";
        //gSql = gSql + " AND (TDate >= '" + sFYs + "')";
        //gSql = gSql + " AND (TDate <= '" + sFYe + "')";
        //gSql = gSql + " AND (InSource = '" + Session["sBrId"] + "')";

        //gSql = gSql + " GROUP BY TrType, InSource";

        //SqlCommand cmd = new SqlCommand(gSql, conn);
        //conn.Open();
        //SqlDataReader dr = cmd.ExecuteReader();
        //if (dr.Read())
        //{
        //    wAmnt = Convert.ToDouble(dr["NetAmnt"].ToString());
        //}
        //else
        //{
        //    wAmnt = 0;
        //}
        //dr.Dispose();
        //dr.Close();
        //conn.Close();

        ////---------------------------------------------------------------------------------------------
        //// SALES AMOUNT
        //gSql = "";
        //gSql = "SELECT TrType, OutSource, SUM(NetSalesAmnt)/100000 AS NetAmnt";
        ////gSql = gSql + " PARSENAME(CONVERT(VARCHAR, CAST(SUM(NetSalesAmnt)/100000  AS NetAmnt), 1),2)";
        //gSql = gSql + " FROM dbo.MRSRMaster";

        //gSql = gSql + " WHERE (TrType = 3)";
        //gSql = gSql + " AND (TDate >= '" + sFYs + "')";
        //gSql = gSql + " AND (TDate <= '" + sFYe + "')";
        //gSql = gSql + " AND (OutSource = '" + Session["sBrId"] + "')";

        //gSql = gSql + " GROUP BY TrType, OutSource";

        //cmd = new SqlCommand(gSql, conn);
        //conn.Open();
        //dr = cmd.ExecuteReader();
        //if (dr.Read())
        //{
        //    tAmnt = Convert.ToDouble(dr["NetAmnt"].ToString()) - wAmnt;
        //    lblYSales.Text = tAmnt.ToString("00.00", CultureInfo.InvariantCulture) + "L";
        //}
        //else
        //{
        //    lblYSales.Text = "0";
        //}
        //dr.Dispose();
        //dr.Close();
        //conn.Close();
        //---------------------------------------------------------------------------------------------
        //0000000000000000

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

        string gSQL = "";
        gSQL = "DELETE FROM TempOpening";
        gSQL = gSQL + " WHERE UserID='" + Session["sUser"].ToString() + "'";
        //gSQL = gSQL + " WHERE UserID='" + Session["sUser"].ToString() + "' AND PCName='" + sPC + "'";
        SqlCommand cmd2 = new SqlCommand(gSQL, conn2);
        conn2.Open();
        cmd2.ExecuteNonQuery();
        conn2.Close();

        //'------------------------------------------------------------------------------------------
        gSQL = "";
        gSQL = "SELECT  CatName AS ZoneName, CategoryID ";
        gSQL = gSQL + " FROM Zone";
        gSQL = gSQL + " WHERE ZoneType='1'";
        //gSQL = gSQL + " WHERE CategoryID='" + Session["sZoneID"].ToString() + "'";      
        gSQL = gSQL + " GROUP BY CatName, CategoryID";
        SqlCommand cmd = new SqlCommand(gSQL, conn);
        conn.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            gSQL = "";
            gSQL = "INSERT INTO TempOpening(ZoneName,DelearID,";
            gSQL = gSQL + " UserID,PCName) VALUES(";
            gSQL = gSQL + " '" + dr["ZoneName"].ToString() + "','" + dr["CategoryID"].ToString() + "',";
            gSQL = gSQL + " '" + Session["sUser"].ToString() + "','" + sPC + "')";
            cmd2 = new SqlCommand(gSQL, conn2);
            conn2.Open();
            cmd2.ExecuteNonQuery();
            conn2.Close();
        }
        conn.Close();
        //'-------------------------------------------------------------------------------------------


        //'-------------------------------------------------------------------------------------------
        //'OPENING
        //'===========================================================================================
        //'LOAD SALES DATA
        gSQL = "";
        gSQL = "SELECT SUM(ISNULL(dbo.MRSRMaster.NetSalesAmnt, 0)) AS NetSalesAmnt, dbo.[Zone].CatName AS ZoneName";
        gSQL = gSQL + " FROM dbo.MRSRMaster INNER JOIN";
        gSQL = gSQL + " dbo.[Zone] ON dbo.MRSRMaster.OutSource = dbo.[Zone].CategoryID";
        gSQL = gSQL + " Where (dbo.MRSRMaster.TrType = 3)";
        gSQL = gSQL + " AND dbo.MRSRMaster.TDate<'" + this.txtFrom.Text + "'";
        gSQL = gSQL + " AND dbo.[Zone].ZoneType='1'";
        //gSQL = gSQL + " AND dbo.[Zone].CategoryID='" + Session["sZoneID"].ToString() + "'"; 
        gSQL = gSQL + " GROUP BY dbo.[Zone].CatName";
        cmd = new SqlCommand(gSQL, conn);
        conn.Open();
        dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            gSQL = "";
            gSQL = "UPDATE TempOpening SET OpeningSalesAmnt=" + dr["NetSalesAmnt"].ToString() + "";
            gSQL = gSQL + " WHERE ZoneName='" + dr["ZoneName"].ToString() + "'";
            gSQL = gSQL + " AND UserID='" + Session["sUser"].ToString() + "'";
            gSQL = gSQL + " AND PCName='" + sPC + "'";
            cmd2 = new SqlCommand(gSQL, conn2);
            conn2.Open();
            cmd2.ExecuteNonQuery();
            conn2.Close();
        }
        conn.Close();
        //'-------------------------------------------------------------------------------------------


        //'-------------------------------------------------------------------------------------------        
        //'LOAD OPENING DEPOSIT
        gSQL = "";
        gSQL = "SELECT ZoneName, SUM(cAmount) AS cAmount";
        gSQL = gSQL + " From dbo.VW_Deposit_Info";
        gSQL = gSQL + " WHERE CDate<'" + this.txtFrom.Text + "'";
        //gSQL = gSQL + " AND CategoryID='" + Session["sZoneID"].ToString() + "'"; 
        gSQL = gSQL + " GROUP BY ZoneName";
        cmd = new SqlCommand(gSQL, conn);
        conn.Open();
        dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            gSQL = "";
            gSQL = " UPDATE TempOpening SET OpeningCollection=" + dr["cAmount"].ToString() + "";
            gSQL = gSQL + " WHERE ZoneName='" + dr["ZoneName"].ToString() + "'";
            gSQL = gSQL + " AND UserID='" + Session["sUser"].ToString() + "'";
            gSQL = gSQL + " AND PCName='" + sPC + "'";
            cmd2 = new SqlCommand(gSQL, conn2);
            conn2.Open();
            cmd2.ExecuteNonQuery();
            conn2.Close();
        }
        conn.Close();
        //'-------------------------------------------------------------------------------------------

        //'-------------------------------------------------------------------------------------------
        //'LOAD DISHONOUR
        gSQL = "";
        gSQL = "SELECT ZoneName, SUM(cAmount) AS cAmount";
        gSQL = gSQL + " From VW_DishonourAmnt";
        gSQL = gSQL + " WHERE CDate<'" + this.txtFrom.Text + "'";
        //gSQL = gSQL + " AND CategoryID='" + Session["sZoneID"].ToString() + "'"; 
        gSQL = gSQL + " GROUP BY ZoneName";
        cmd = new SqlCommand(gSQL, conn);
        conn.Open();
        dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            gSQL = "";
            gSQL = " UPDATE TempOpening SET OpeningDishonour=" + dr["cAmount"].ToString() + "";
            gSQL = gSQL + " WHERE ZoneName='" + dr["ZoneName"].ToString() + "'";
            gSQL = gSQL + " AND UserID='" + Session["sUser"].ToString() + "'";
            gSQL = gSQL + " AND PCName='" + sPC + "'";
            cmd2 = new SqlCommand(gSQL, conn2);
            conn2.Open();
            cmd2.ExecuteNonQuery();
            conn2.Close();
        }
        conn.Close();
        //'-------------------------------------------------------------------------------------------


        //'-------------------------------------------------------------------------------------------
        //'LOAD OPENING WITHDRAWN
        gSQL = "";
        gSQL = "SELECT SUM(ISNULL(dbo.MRSRMaster.NetSalesAmnt, 0)) AS NetSalesAmnt, ";
        gSQL = gSQL + " dbo.[Zone].CatName AS ZoneName";
        gSQL = gSQL + " FROM dbo.MRSRMaster INNER JOIN";
        gSQL = gSQL + " dbo.[Zone] ON dbo.MRSRMaster.InSource = dbo.[Zone].CategoryID";
        gSQL = gSQL + " Where (dbo.MRSRMaster.TrType = -3)";
        gSQL = gSQL + " AND dbo.[Zone].ZoneType='1'";
        //gSQL = gSQL + " AND dbo.[Zone].CategoryID='" + Session["sZoneID"].ToString() + "'"; 
        gSQL = gSQL + " AND dbo.MRSRMaster.TDate<'" + this.txtFrom.Text + "'";
        gSQL = gSQL + " GROUP BY dbo.[Zone].CatName";
        cmd = new SqlCommand(gSQL, conn);
        conn.Open();
        dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            gSQL = "";
            gSQL = " UPDATE TempOpening SET OpenigWithdrawn=" + dr["NetSalesAmnt"].ToString() + "";
            gSQL = gSQL + " WHERE ZoneName='" + dr["ZoneName"].ToString() + "'";
            gSQL = gSQL + " AND UserID='" + Session["sUser"].ToString() + "'";
            gSQL = gSQL + " AND PCName='" + sPC + "'";
            cmd2 = new SqlCommand(gSQL, conn2);
            conn2.Open();
            cmd2.ExecuteNonQuery();
            conn2.Close();
        }
        conn.Close();


        //'==================================================================================================
        //'CURRENT TRANSACTION
        //'LOAD SALES DATA
        gSQL = "";
        gSQL = "SELECT SUM(ISNULL(dbo.MRSRMaster.NetSalesAmnt, 0)) AS NetSalesAmnt, dbo.[Zone].CatName AS ZoneName";
        gSQL = gSQL + " FROM dbo.MRSRMaster INNER JOIN";
        gSQL = gSQL + " dbo.[Zone] ON dbo.MRSRMaster.OutSource = dbo.[Zone].CategoryID";
        gSQL = gSQL + " Where (dbo.MRSRMaster.TrType = 3)";
        //gSQL = gSQL + " AND dbo.[Zone].CategoryID='" + Session["sZoneID"].ToString() + "'"; 
        gSQL = gSQL + " AND dbo.MRSRMaster.TDate>='" + this.txtFrom.Text + "'";
        gSQL = gSQL + " AND dbo.MRSRMaster.TDate<='" + this.txtToDate.Text + "'";
        gSQL = gSQL + " AND dbo.[Zone].ZoneType='1'";
        gSQL = gSQL + " GROUP BY dbo.[Zone].CatName";
        cmd = new SqlCommand(gSQL, conn);
        conn.Open();
        dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            gSQL = "";
            gSQL = " UPDATE TempOpening SET SalesAmnt=" + dr["NetSalesAmnt"].ToString() + "";
            gSQL = gSQL + " WHERE ZoneName='" + dr["ZoneName"].ToString() + "'";
            gSQL = gSQL + " AND UserID='" + Session["sUser"].ToString() + "'";
            gSQL = gSQL + " AND PCName='" + sPC + "'";
            cmd2 = new SqlCommand(gSQL, conn2);
            conn2.Open();
            cmd2.ExecuteNonQuery();
            conn2.Close();
        }
        conn.Close();
        //'-------------------------------------------------------------------------------------------


        //'-------------------------------------------------------------------------------------------        
        //'LOAD DEPOSIT
        gSQL = "";
        gSQL = "SELECT ZoneName, SUM(cAmount) AS cAmount";
        gSQL = gSQL + " From dbo.VW_Deposit_Info";
        gSQL = gSQL + " WHERE CDate>='" + this.txtFrom.Text + "'";
        //gSQL = gSQL + " AND CategoryID='" + Session["sZoneID"].ToString() + "'"; 
        gSQL = gSQL + " AND CDate<='" + this.txtToDate.Text + "'";
        gSQL = gSQL + " GROUP BY ZoneName";
        cmd = new SqlCommand(gSQL, conn);
        conn.Open();
        dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            gSQL = "";
            gSQL = " UPDATE TempOpening SET Collection=" + dr["cAmount"].ToString() + "";
            gSQL = gSQL + " WHERE ZoneName='" + dr["ZoneName"].ToString() + "'";
            gSQL = gSQL + " AND UserID='" + Session["sUser"].ToString() + "'";
            gSQL = gSQL + " AND PCName='" + sPC + "'";
            cmd2 = new SqlCommand(gSQL, conn2);
            conn2.Open();
            cmd2.ExecuteNonQuery();
            conn2.Close();
        }
        conn.Close();
        //'-------------------------------------------------------------------------------------------

        //'-------------------------------------------------------------------------------------------
        //'LOAD DISHONOUR
        gSQL = "";
        gSQL = "SELECT ZoneName, SUM(cAmount) AS cAmount";
        gSQL = gSQL + " From dbo.VW_DishonourAmnt";
        gSQL = gSQL + " WHERE CDate>='" + this.txtFrom.Text + "'";
        gSQL = gSQL + " AND CDate<='" + this.txtToDate.Text + "'";
        //gSQL = gSQL + " AND CategoryID='" + Session["sZoneID"].ToString() + "'"; 
        gSQL = gSQL + " GROUP BY ZoneName";
        cmd = new SqlCommand(gSQL, conn);
        conn.Open();
        dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            gSQL = "";
            gSQL = " UPDATE TempOpening SET DishonourAmnt=" + dr["cAmount"].ToString() + "";
            gSQL = gSQL + " WHERE ZoneName='" + dr["ZoneName"].ToString() + "'";
            gSQL = gSQL + " AND UserID='" + Session["sUser"].ToString() + "'";
            gSQL = gSQL + " AND PCName='" + sPC + "'";
            cmd2 = new SqlCommand(gSQL, conn2);
            conn2.Open();
            cmd2.ExecuteNonQuery();
            conn2.Close();
        }
        conn.Close();
        //'-------------------------------------------------------------------------------------------


        //'-------------------------------------------------------------------------------------------
        //'LOAD WITHDRAWN
        gSQL = "";
        gSQL = "SELECT SUM(ISNULL(dbo.MRSRMaster.NetSalesAmnt, 0)) AS NetSalesAmnt, ";
        gSQL = gSQL + " dbo.[Zone].CatName AS ZoneName";
        gSQL = gSQL + " FROM dbo.MRSRMaster INNER JOIN";
        gSQL = gSQL + " dbo.[Zone] ON dbo.MRSRMaster.InSource = dbo.[Zone].CategoryID";
        gSQL = gSQL + " Where (dbo.MRSRMaster.TrType = -3)";
        //gSQL = gSQL + " AND dbo.[Zone].CategoryID='" + Session["sZoneID"].ToString() + "'"; 
        gSQL = gSQL + " AND dbo.MRSRMaster.TDate>='" + this.txtFrom.Text + "'";
        gSQL = gSQL + " AND dbo.MRSRMaster.TDate<='" + this.txtToDate.Text + "'";
        gSQL = gSQL + " AND dbo.[Zone].ZoneType='1'";
        gSQL = gSQL + " GROUP BY dbo.[Zone].CatName";
        cmd = new SqlCommand(gSQL, conn);
        conn.Open();
        dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            gSQL = "";
            gSQL = " UPDATE TempOpening SET Withdrawn=" + dr["NetSalesAmnt"].ToString() + "";
            gSQL = gSQL + " WHERE ZoneName='" + dr["ZoneName"].ToString() + "'";
            gSQL = gSQL + " AND UserID='" + Session["sUser"].ToString() + "'";
            gSQL = gSQL + " AND PCName='" + sPC + "'";
            cmd2 = new SqlCommand(gSQL, conn2);
            conn2.Open();
            cmd2.ExecuteNonQuery();
            conn2.Close();
        }
        conn.Close();
        //'-------------------------------------------------------------------------------------------


        //double dOB = 0;
        //double dCB = 0;
        //double ddCB = 0;
        ////LOAD TOTAL
        //gSQL = "";
        ////gSQL = "select * from TempOpening";
        ////gSQL = gSQL + " WHERE UserID='" + Session["sUser"].ToString() + "' AND PCName='" + sPC + "'";

        //gSQL = "SELECT UserID, SUM(OpeningSalesAmnt) AS OpeningSalesAmnt, SUM(OpeningCollection) AS OpeningCollection,";
        //gSQL = gSQL + " SUM(OpenigWithdrawn) AS OpenigWithdrawn, SUM(OpeningDishonour) AS OpeningDishonour,";
        //gSQL = gSQL + " SUM(SalesAmnt) AS SalesAmnt, SUM(Collection) AS Collection, SUM(Withdrawn) AS Withdrawn,";
        //gSQL = gSQL + " SUM(DishonourAmnt) AS DishonourAmnt";
        //gSQL = gSQL + " FROM dbo.TempOpening";
        //gSQL = gSQL + " WHERE UserID='" + Session["sUser"].ToString() + "' AND PCName='" + sPC + "'";
        //gSQL = gSQL + " GROUP BY UserID";

        //cmd = new SqlCommand(gSQL, conn);
        //conn.Open();
        //dr = cmd.ExecuteReader();
        //if (dr.Read())
        //{
        //    dOB = Convert.ToDouble(dr["OpeningSalesAmnt"].ToString()) - Convert.ToDouble(dr["OpeningCollection"].ToString()) - Convert.ToDouble(dr["OpenigWithdrawn"].ToString()) + Convert.ToDouble(dr["OpeningDishonour"].ToString());
        //    dCB = dOB + Convert.ToDouble(dr["SalesAmnt"].ToString()) - Convert.ToDouble(dr["Collection"].ToString()) - Convert.ToDouble(dr["Withdrawn"].ToString()) + Convert.ToDouble(dr["DishonourAmnt"].ToString());
        //    ddCB = dCB / 10000000;
        //    //lblOutstanding.Text = Convert.ToString(dCB / 100000) + " L";
        //    lblOutstanding.Text = ddCB.ToString("00.00", CultureInfo.InvariantCulture) + "C";

        //}
        //conn.Close();

    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CalcTotalQty(e.Row.Cells[2].Text);
            CalcTotal_TP(e.Row.Cells[3].Text);

            CalcTotal_Cash(e.Row.Cells[4].Text);
            CalcTotal_Card(e.Row.Cells[5].Text);

            CalcTotal_6(e.Row.Cells[6].Text);
            CalcTotal_7(e.Row.Cells[7].Text);

            // ALIGNMENT
            e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Left;

            e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[7].HorizontalAlign = HorizontalAlign.Right;

        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[1].Text = "Total";

            e.Row.Cells[2].Text = runningTotalQty.ToString("0,0", CultureInfo.InvariantCulture);
            e.Row.Cells[3].Text = runningTotalTP.ToString("0,0", CultureInfo.InvariantCulture);
            e.Row.Cells[4].Text = runningTotalCash.ToString("0,0", CultureInfo.InvariantCulture);
            e.Row.Cells[5].Text = runningTotalCard.ToString("0,0", CultureInfo.InvariantCulture);

            e.Row.Cells[6].Text = runningTotal6.ToString("0,0", CultureInfo.InvariantCulture);
            e.Row.Cells[7].Text = runningTotal7.ToString("0,0", CultureInfo.InvariantCulture);


            //ALIGNMENT
            e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Left;

            e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[7].HorizontalAlign = HorizontalAlign.Right;
            
        }

    }

    //CALCULATE TOTAL CASH PAY
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

    private void CalcTotal_6(string _price)
    {
        try
        {
            runningTotal6 += Double.Parse(_price);
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write(ex.ToString());
        }
    }

    private void CalcTotal_7(string _price)
    {
        try
        {
            runningTotal7 += Double.Parse(_price);
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write(ex.ToString());
        }
    }


    protected void OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        GridView1.DataBind();
        fnLoadData();
    }

 

    //CALCULATE NET AMOUNT
    private void CalcTotal(string _price)
    {
        try
        {
            runningTotal += Double.Parse(_price);
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write(ex.ToString());
        }
    }

    //CALCULATE TOTAL AMOUNT
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

    //CALCULATE DISCOUNT AMOUNT
    private void CalcTotal_Dis(string _price)
    {
        try
        {
            runningTotalDis += Double.Parse(_price);
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write(ex.ToString());
        }
    }

    //CALCULATE WITH/Adj AMOUNT
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


    //CALCULATE TOTAL QTY
    private void CalcTotalQty(string _qty)
    {
        try
        {
            runningTotalQty += Double.Parse(_qty);
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write(ex.ToString());
        }
    }

    protected void MakeTable()
    {
        //dt.Columns.Add("ID").AutoIncrement = true;
        dt.Columns.Add("ProductID");
        //dt.Columns.Add("ProductID", typeof(SqlInt32));
        dt.Columns.Add("Model");
        dt.Columns.Add("MRP");
        dt.Columns.Add("CampaignPrice");
        dt.Columns.Add("Qty");
        dt.Columns.Add("TotalPrice");
        dt.Columns.Add("DisAmnt");
        dt.Columns.Add("DisCode");
        dt.Columns.Add("DisRef");
        dt.Columns.Add("WithAdjAmnt");
        dt.Columns.Add("NetAmnt");
        dt.Columns.Add("ProductSL");
        dt.Columns.Add("Remarks");

    }

   
   

   

    protected void fnLoadDealerStatement1()
    {
        SqlConnection conn = DBConnectionDSM.GetConnection();
        SqlConnection conn2 = DBConnectionDSM.GetConnection();

        //'============================================================================
        //'OPENING
        //'============================================================================
        
        string gSQL="";

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
        SqlCommand cmd2 = new SqlCommand(gSQL, conn2);
        conn2.Open();
        cmd2.ExecuteNonQuery();
        conn2.Close();
    
        //'----------------------------------------------------------------------------
        //'LOAD DEALER NAME ZONE WISE
        gSQL = "";
        gSQL = "SELECT dbo.[Zone].CatName AS ZoneName, dbo.DelearInfo.DAID, ";
        gSQL = gSQL + " dbo.DelearInfo.Code, dbo.DelearInfo.Name, ";
        gSQL = gSQL + " dbo.DelearInfo.Address,";
        gSQL = gSQL + " dbo.DelearInfo.Discontinue";
    
        gSQL = gSQL + " FROM dbo.DelearInfo INNER JOIN";
        gSQL = gSQL + " dbo.[Zone] ON dbo.DelearInfo.CategoryID = dbo.[Zone].CategoryID";
    
        gSQL = gSQL + " WHERE dbo.DelearInfo.Discontinue='No'";
        //gSQL = gSQL + " AND dbo.[Zone].CategoryID='" + Session["sZoneID"].ToString() + "'";

        if (ddlEntity.SelectedItem.Text != "ALL")
        {
            gSQL = gSQL + " AND dbo.DelearInfo.Name='" + ddlEntity.SelectedItem.Text + "'";
        }
    
        gSQL = gSQL + " ORDER BY dbo.DelearInfo.Name";
        SqlCommand cmd = new SqlCommand(gSQL, conn);
        conn.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            gSQL = "";
            gSQL = "INSERT INTO TempOpening(ZoneName,DelearName,DelearID,";
            gSQL = gSQL + " UserID,PCName) VALUES(";
            gSQL = gSQL + " '" + dr["ZoneName"].ToString() + "','" + dr["Name"].ToString() + "','" + dr["DAID"].ToString() + "',";
            gSQL = gSQL + " '" + Session["sUser"].ToString() + "','" + sPC + "')";
            cmd2 = new SqlCommand(gSQL, conn2);
            conn2.Open();
            cmd2.ExecuteNonQuery();
            conn2.Close();
        }
        conn.Close();                 
        //'----------------------------------------------------------------------------
    
        //'OPENING
        //'LOAD SALES DATA
        gSQL = "";
        gSQL = "SELECT SUM(ISNULL(dbo.MRSRMaster.NetSalesAmnt, 0)) AS NetSalesAmnt, ";
        gSQL = gSQL + " dbo.VW_Delear_Info.Name, dbo.VW_Delear_Info.ZoneName";
        gSQL = gSQL + " FROM dbo.MRSRMaster INNER JOIN";
        gSQL = gSQL + " dbo.VW_Delear_Info ON dbo.MRSRMaster.InSource = dbo.VW_Delear_Info.DAID";
    
        gSQL = gSQL + " Where (dbo.MRSRMaster.TrType = 3)";
        gSQL = gSQL + " AND dbo.MRSRMaster.TDate<'" + Convert.ToDateTime(this.txtFrom.Text) + "'";
        //gSQL = gSQL + " AND dbo.VW_Delear_Info.CategoryID='" + Session["sZoneID"].ToString() + "'";
        if (ddlEntity.SelectedItem.Text != "ALL")
        {
            gSQL = gSQL + " AND dbo.VW_Delear_Info.Name='" + ddlEntity.SelectedItem.Text + "'";
        }

        gSQL = gSQL + " GROUP BY dbo.VW_Delear_Info.Name, dbo.VW_Delear_Info.ZoneName";
        cmd = new SqlCommand(gSQL, conn);
        conn.Open();
        dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            gSQL = "";
            gSQL = "UPDATE TempOpening SET OpeningSalesAmnt=" + dr["NetSalesAmnt"].ToString() + "";
            gSQL = gSQL + " WHERE ZoneName='" + dr["ZoneName"].ToString() + "'";
            gSQL = gSQL + " AND DelearName='" + dr["Name"].ToString() + "'";
            gSQL = gSQL + " AND UserID='" + Session["sUser"].ToString() + "'";
            gSQL = gSQL + " AND PCName='" + sPC + "'";
            cmd2 = new SqlCommand(gSQL, conn2);
            conn2.Open();
            cmd2.ExecuteNonQuery();
            conn2.Close();
        }   
        conn.Close(); 
        //'-------------------------------------------------------------------------------------------
    
    
        //'-------------------------------------------------------------------------------------------        
        //'LOAD OPENING DEPOSIT
        gSQL = "";
        gSQL = "SELECT ZoneName,Name, SUM(cAmount) AS cAmount";
        gSQL = gSQL + " From dbo.VW_Deposit_Info";
        gSQL = gSQL + " WHERE CDate<'" + Convert.ToDateTime(this.txtFrom.Text) + "'";
        //gSQL = gSQL + " AND CategoryID='" + Session["sZoneID"].ToString() + "'";
        if (ddlEntity.SelectedItem.Text != "ALL")
        {
            gSQL = gSQL + " AND Name='" + ddlEntity.SelectedItem.Text + "'";
        }

        gSQL = gSQL + " GROUP BY ZoneName,Name";
        cmd = new SqlCommand(gSQL, conn);
        conn.Open();
        dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            gSQL = "";
            gSQL = " UPDATE TempOpening SET OpeningCollection=" + dr["cAmount"].ToString() + "";
            gSQL = gSQL + " WHERE ZoneName='" + dr["ZoneName"].ToString() + "'";
            gSQL = gSQL + " AND DelearName='" + dr["Name"].ToString() + "'";
            gSQL = gSQL + " AND UserID='" + Session["sUser"].ToString() + "'";
            gSQL = gSQL + " AND PCName='" + sPC + "'";
            cmd2 = new SqlCommand(gSQL, conn2);
            conn2.Open();
            cmd2.ExecuteNonQuery();
            conn2.Close();

        }
        conn.Close();                
        //'-------------------------------------------------------------------------------------------
        
    
        //'-------------------------------------------------------------------------------------------
        //'LOAD DISHONOUR
        gSQL = "";
        gSQL = "SELECT ZoneName,Name, SUM(cAmount) AS cAmount";
        gSQL = gSQL + " From dbo.VW_DishonourAmnt";
        gSQL = gSQL + " WHERE CDate<'" + Convert.ToDateTime(this.txtFrom.Text) + "'";
        //gSQL = gSQL + " AND CategoryID='" + Session["sZoneID"].ToString() + "'";
        if (ddlEntity.SelectedItem.Text != "ALL")
        {
            gSQL = gSQL + " AND Name='" + ddlEntity.SelectedItem.Text + "'";
        }

        gSQL = gSQL + " GROUP BY ZoneName,Name";
        cmd = new SqlCommand(gSQL, conn);
        conn.Open();
        dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            gSQL = "";
            gSQL = " UPDATE TempOpening SET OpeningDishonour=" + dr["cAmount"].ToString() + "";
            gSQL = gSQL + " WHERE ZoneName='" + dr["ZoneName"].ToString() + "'";
            gSQL = gSQL + " AND DelearName='" + dr["Name"].ToString() + "'";
            gSQL = gSQL + " AND UserID='" + Session["sUser"].ToString() + "'";
            gSQL = gSQL + " AND PCName='" + sPC + "'";
            cmd2 = new SqlCommand(gSQL, conn2);
            conn2.Open();
            cmd2.ExecuteNonQuery();
            conn2.Close();

        }
        conn.Close();
        //'-------------------------------------------------------------------------------------------

    
        //'-------------------------------------------------------------------------------------------
        //'LOAD WITHDRAWN
        gSQL = "";
        gSQL = "SELECT SUM(ISNULL(dbo.MRSRMaster.NetSalesAmnt, 0)) AS NetSalesAmnt, ";
        gSQL = gSQL + " dbo.VW_Delear_Info.Name, dbo.VW_Delear_Info.ZoneName";
        gSQL = gSQL + " FROM dbo.MRSRMaster INNER JOIN";
        gSQL = gSQL + " dbo.VW_Delear_Info ON dbo.MRSRMaster.OutSource = dbo.VW_Delear_Info.DAID";
    
        gSQL = gSQL + " Where (dbo.MRSRMaster.TrType = -3)";
        gSQL = gSQL + " AND dbo.MRSRMaster.TDate<'" + Convert.ToDateTime(this.txtFrom.Text) + "'";
        //gSQL = gSQL + " AND dbo.VW_Delear_Info.CategoryID='" + Session["sZoneID"].ToString() + "'";

        if (ddlEntity.SelectedItem.Text != "ALL")
        {
            gSQL = gSQL + " AND dbo.VW_Delear_Info.Name='" + ddlEntity.SelectedItem.Text + "'";
        }

        gSQL = gSQL + " GROUP BY dbo.VW_Delear_Info.Name, dbo.VW_Delear_Info.ZoneName";
        cmd = new SqlCommand(gSQL, conn);
        conn.Open();
        dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            gSQL = "";
            gSQL = "UPDATE TempOpening SET OpenigWithdrawn=" + dr["NetSalesAmnt"].ToString() + "";
            gSQL = gSQL + " WHERE ZoneName='" + dr["ZoneName"].ToString() + "'";
            gSQL = gSQL + " AND DelearName='" + dr["Name"].ToString() + "'";
            gSQL = gSQL + " AND UserID='" + Session["sUser"].ToString() + "'";
            gSQL = gSQL + " AND PCName='" + sPC + "'";
            cmd2 = new SqlCommand(gSQL, conn2);
            conn2.Open();
            cmd2.ExecuteNonQuery();
            conn2.Close();

        }
        conn.Close();
    

        //'==================================================================================================
        //'CURRENT TRANSACTION
        //'LOAD SALES DATA
        gSQL = "";
        gSQL = "SELECT SUM(ISNULL(dbo.MRSRMaster.NetSalesAmnt, 0)) AS NetSalesAmnt, ";
        gSQL = gSQL + " dbo.VW_Delear_Info.Name, dbo.VW_Delear_Info.ZoneName";
        gSQL = gSQL + " FROM dbo.MRSRMaster INNER JOIN";
        gSQL = gSQL + " dbo.VW_Delear_Info ON dbo.MRSRMaster.InSource = dbo.VW_Delear_Info.DAID";
    
        gSQL = gSQL + " Where (dbo.MRSRMaster.TrType = 3)";
        gSQL = gSQL + " AND dbo.MRSRMaster.TDate>='" + Convert.ToDateTime(this.txtFrom.Text) + "'";
        gSQL = gSQL + " AND dbo.MRSRMaster.TDate<='" + Convert.ToDateTime(this.txtToDate.Text) + "'";
        //gSQL = gSQL + " AND dbo.VW_Delear_Info.CategoryID='" + Session["sZoneID"].ToString() + "'";

        if (ddlEntity.SelectedItem.Text != "ALL")
        {
            gSQL = gSQL + " AND dbo.VW_Delear_Info.Name='" + ddlEntity.SelectedItem.Text + "'";
        }
    
        gSQL = gSQL + " GROUP BY dbo.VW_Delear_Info.Name, dbo.VW_Delear_Info.ZoneName";
        cmd = new SqlCommand(gSQL, conn);
        conn.Open();
        dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            gSQL = "";
            gSQL = " UPDATE TempOpening SET SalesAmnt=" + dr["NetSalesAmnt"].ToString() + "";
            gSQL = gSQL + " WHERE ZoneName='" + dr["ZoneName"].ToString() + "'";
            gSQL = gSQL + " AND DelearName='" + dr["Name"].ToString() + "'";
            gSQL = gSQL + " AND UserID='" + Session["sUser"].ToString() + "'";
            gSQL = gSQL + " AND PCName='" + sPC + "'";
            cmd2 = new SqlCommand(gSQL, conn2);
            conn2.Open();
            cmd2.ExecuteNonQuery();
            conn2.Close();

        }
        conn.Close();
        //'-------------------------------------------------------------------------------------------
    
    
        //'-------------------------------------------------------------------------------------------        
        //'LOAD DEPOSIT
        gSQL = "";
        gSQL = "SELECT ZoneName, Name, SUM(cAmount) AS cAmount";
        gSQL = gSQL + " From dbo.VW_Deposit_Info";
        gSQL = gSQL + " WHERE CDate>='" + Convert.ToDateTime(this.txtFrom.Text) + "'";
        gSQL = gSQL + " AND CDate<='" + Convert.ToDateTime(this.txtToDate.Text) + "'";
        //gSQL = gSQL + " AND CategoryID='" + Session["sZoneID"].ToString() + "'";
        if (ddlEntity.SelectedItem.Text != "ALL")
        {
            gSQL = gSQL + " AND Name='" + ddlEntity.SelectedItem.Text + "'";
        }

        gSQL = gSQL + " GROUP BY ZoneName,Name";
        cmd = new SqlCommand(gSQL, conn);
        conn.Open();
        dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            gSQL = "";
            gSQL = " UPDATE TempOpening SET Collection=" + dr["cAmount"].ToString() + "";
            gSQL = gSQL + " WHERE ZoneName='" + dr["ZoneName"].ToString() + "'";
            gSQL = gSQL + " AND DelearName='" + dr["Name"].ToString() + "'";
            gSQL = gSQL + " AND UserID='" + Session["sUser"].ToString() + "'";
            gSQL = gSQL + " AND PCName='" + sPC + "'";
            cmd2 = new SqlCommand(gSQL, conn2);
            conn2.Open();
            cmd2.ExecuteNonQuery();
            conn2.Close();

        }
        conn.Close();     
        //'-------------------------------------------------------------------------------------------
    
    
        //'-------------------------------------------------------------------------------------------
        //'LOAD DISHONOUR
        gSQL = "";
        gSQL = "SELECT ZoneName, Name,SUM(cAmount) AS cAmount";
        gSQL = gSQL + " From dbo.VW_DishonourAmnt";
        gSQL = gSQL + " WHERE CDate>='" + Convert.ToDateTime(this.txtFrom.Text) + "'";
        gSQL = gSQL + " AND CDate<='" + Convert.ToDateTime(this.txtToDate.Text) + "'";
        //gSQL = gSQL + " AND CategoryID='" + Session["sZoneID"].ToString() + "'";

        if (ddlEntity.SelectedItem.Text != "ALL")
        {
            gSQL = gSQL + " AND Name='" + ddlEntity.SelectedItem.Text + "'";
        }

        gSQL = gSQL + " GROUP BY ZoneName,Name";
        cmd = new SqlCommand(gSQL, conn);
        conn.Open();
        dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            gSQL = "";
            gSQL = " UPDATE TempOpening SET DishonourAmnt=" + dr["cAmount"].ToString() + "";
            gSQL = gSQL + " WHERE ZoneName='" + dr["ZoneName"].ToString() + "'";
            gSQL = gSQL + " AND DelearName='" + dr["Name"].ToString() + "'";
            gSQL = gSQL + " AND UserID='" + Session["sUser"].ToString() + "'";
            gSQL = gSQL + " AND PCName='" + sPC + "'";
            cmd2 = new SqlCommand(gSQL, conn2);
            conn2.Open();
            cmd2.ExecuteNonQuery();
            conn2.Close();

        }
        conn.Close();         
        //'-------------------------------------------------------------------------------------------

    
        //'-------------------------------------------------------------------------------------------
        //'LOAD WITHDRAWN
        gSQL = "";
        gSQL = "SELECT SUM(ISNULL(dbo.MRSRMaster.NetSalesAmnt, 0)) AS NetSalesAmnt, ";
        gSQL = gSQL + " dbo.VW_Delear_Info.Name, dbo.VW_Delear_Info.ZoneName";
        gSQL = gSQL + " FROM dbo.MRSRMaster INNER JOIN";
        gSQL = gSQL + " dbo.VW_Delear_Info ON dbo.MRSRMaster.OutSource = dbo.VW_Delear_Info.DAID";
    
        gSQL = gSQL + " Where (dbo.MRSRMaster.TrType = -3)";
        gSQL = gSQL + " AND dbo.MRSRMaster.TDate>='" + Convert.ToDateTime(this.txtFrom.Text) + "'";
        gSQL = gSQL + " AND dbo.MRSRMaster.TDate<='" + Convert.ToDateTime(this.txtToDate.Text) + "'";
        //gSQL = gSQL + " AND dbo.VW_Delear_Info.CategoryID='" + Session["sZoneID"].ToString() + "'";

        if (ddlEntity.SelectedItem.Text != "ALL")
        {
            gSQL = gSQL + " AND dbo.VW_Delear_Info.Name='" + ddlEntity.SelectedItem.Text + "'";
        }

        gSQL = gSQL + " GROUP BY dbo.VW_Delear_Info.Name, dbo.VW_Delear_Info.ZoneName";
        cmd = new SqlCommand(gSQL, conn);
        conn.Open();
        dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            gSQL = "";
            gSQL = " UPDATE TempOpening SET Withdrawn=" + dr["NetSalesAmnt"].ToString() + "";
            gSQL = gSQL + " WHERE ZoneName='" + dr["ZoneName"].ToString() + "'";
            gSQL = gSQL + " AND DelearName='" + dr["Name"].ToString() + "'";
            gSQL = gSQL + " AND UserID='" + Session["sUser"].ToString() + "'";
            gSQL = gSQL + " AND PCName='" + sPC + "'";
            cmd2 = new SqlCommand(gSQL, conn2);
            conn2.Open();
            cmd2.ExecuteNonQuery();
            conn2.Close();
        }
        conn.Close();              
        //'-------------------------------------------------------------------------------------------




    }

}