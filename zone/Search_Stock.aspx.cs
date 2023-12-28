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

public partial class Search_Stock : System.Web.UI.Page
{
    SqlConnection conn = DBConnection.GetConnection();
    long i;

    private double runningTotalQty = 0;
    private double runningTotalTP = 0;
    private double runningTotalCash = 0;
    private double runningTotalCard = 0;
    private double runningTotalCheque = 0;
    private double runningTotalReq = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (System.Convert.ToInt32(Session["Vis"]) == 0)
        {
            Response.Redirect("../Account/Login.aspx");
        }

        if (!IsPostBack)
        {
            this.txtFrom.Text = DateTime.Today.ToString("MM/dd/yyyy");
            //this.txtToDate.Text = DateTime.Today.ToString("MM/dd/yyyy");

            //LOAD CATEGORY IN DROPDOWN
            LoadDropDownList_Category();

        }

    }

    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GetModel(string prefixText)
    {
        DataTable dt = new DataTable();

        SqlConnection con = DBConnection.GetConnection();

        con.Open();
        SqlCommand cmd = new SqlCommand("Select TOP 10 * from Product where Discontinue='No' AND Model like @model+'%'", con);
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


    //LOAD CATEGORY IN DROPDOWN LIST
    protected void LoadDropDownList_Category()
    {
        if (System.Convert.ToInt32(Session["Vis"]) == 0)
        {
            Response.Redirect("../Account/Login.aspx");
        }

        SqlConnection conn = DBConnection.GetConnection();
        String strQuery = "select GroupName from Product GROUP BY GroupName Order By GroupName";
        //SqlConnection con = new SqlConnection("conn");
        SqlCommand cmd = new SqlCommand(strQuery, conn);
        //SqlCommand cmd = new SqlCommand();
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = strQuery;
        cmd.Connection = conn;
        try
        {
            conn.Open();
            ddlCategory.DataSource = cmd.ExecuteReader();
            ddlCategory.DataTextField = "GroupName";
            //ddlCategory.DataValueField = "ProductID";
            ddlCategory.DataValueField = "GroupName";
            ddlCategory.DataBind();

            //Add blank item at index 0.
            ddlCategory.Items.Insert(0, new ListItem("ALL", "ALL"));


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


    protected void PopupMessage(string Msg, Control controlID)
    {
        ScriptManager.RegisterClientScriptBlock(controlID, controlID.GetType(), "msg", "alert('" + Msg + "');", true);
    }

    protected void SearchData(object sender, EventArgs e)
    {

        //if (txtModel.Text == "")
        //{
        //    PopupMessage("Please select product Model.", btnSearch);
        //    txtModel.Focus();
        //    return;
        //}

        //lblModel.Text = "Model : " + txtModel.Text; 

        //LOAD STATEMENT DATA
        fnLoadStatementData();

        //LOAD DATA IN GRID
        fnLoadData();
    }


    //LOAD DATA IN GRID
    private void fnLoadData()
    {
        //s = WebConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        //con = new SqlConnection(s);
        SqlConnection con = DBConnection.GetConnection();
        con.Open();

        string sSql = "";
        sSql = "SELECT GroupName, Model, ProdName, " +            
            " ISNULL(QtyIN,0) - ISNULL(QtyOut,0) AS bQty" +

            " FROM TempRPTBalance_Online " +

            " WHERE (UserID = '" + Session["UserName"] + "') " +
            " AND (ISNULL(QtyIN,0) - ISNULL(QtyOut,0) <> 0) " +
            //" AND (EID='" + Session["sBrId"] + "')" +
                    
            //" AND (dbo.MRSRMaster.TDate >= '" + Convert.ToDateTime(this.txtFrom.Text) + "'" +
            //" AND dbo.MRSRMaster.TDate <= '" + Convert.ToDateTime(this.txtToDate.Text) + "')" +

            " ORDER BY GroupName, Model";
        

        SqlCommand cmd = new SqlCommand(sSql, con);        
        //OleDbDataReader dr = cmd.ExecuteReader();

        DataSet ds = new DataSet();
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        da.Fill(ds);

        GridView1.DataSource = ds;
        GridView1.DataBind();
        //dr.Close();
        con.Close();

    }

    //LOAD STATEMENT DATA
    
    private void fnLoadStatementData()
    {
        SqlConnection con = DBConnection.GetConnection();
        SqlConnection con1 = DBConnection.GetConnection();

        string gSQL = "";

        //*****************************************************************************************
        //DELETE PREVIOUS DATA
        gSQL = "";
        gSQL = "DELETE FROM RPTBalance_Online";       
        gSQL = gSQL + " WHERE UserID='" + Session["UserName"] + "'";
        //gSQL = gSQL + " AND EID='" + Session["sBrId"] + "'";

        SqlCommand cmdD = new SqlCommand(gSQL, con1);
        con1.Open();
        cmdD.ExecuteNonQuery();
        con1.Close();
        //*****************************************************************************************
        
        
        gSQL = "";
        gSQL = " SELECT dbo.Entity.sFlag,dbo.Entity.SerialNo,dbo.Entity.GroupSL AS eGroupSL,dbo.Entity.eName AS InSource, ";
        gSQL = gSQL + " dbo.Product.ProdName, dbo.Product.Model, dbo.Product.ModelSerial, ";
        gSQL = gSQL + " dbo.Product.GroupName, dbo.Product.GroupSL,dbo.Entity.EntityType,";
        gSQL = gSQL + " QtyIN = SUM(CASE WHEN (TrType = 1 OR TrType = 2 OR TrType = 3 OR";
        gSQL = gSQL + " TrType = 4 OR TrType=-1 OR TrType=-3) THEN ABS(dbo.MRSRDetails.QTy) ELSE 0 END),";
        gSQL = gSQL + " QtyOut =0";
            
        gSQL = gSQL + " FROM dbo.MRSRMaster INNER JOIN";
        gSQL = gSQL + " dbo.MRSRDetails ON dbo.MRSRMaster.MRSRMID = dbo.MRSRDetails.MRSRMID INNER JOIN";
        gSQL = gSQL + " dbo. Product ON dbo.MRSRDetails.ProductID = dbo. Product .ProductID INNER JOIN";
        gSQL = gSQL + " dbo.Entity ON dbo.MRSRMaster.InSource = dbo.Entity.EID";
        
        gSQL = gSQL + " WHERE dbo.MRSRMaster.TDate <'" + txtFrom.Text + "'";
        gSQL = gSQL + " AND  (dbo.Entity.EID='" + Session["EID"] + "') ";

        if (ddlCategory.SelectedItem.Text != "ALL") 
        {
            gSQL = gSQL + " AND dbo.Product.GroupName='" + ddlCategory.SelectedItem.Text + "'";
        }

        if (txtModel.Text != "")
        {
            gSQL = gSQL + " AND dbo.Product.Model='" + txtModel.Text + "'";
        }

        gSQL = gSQL + " AND dbo.Entity.ActiveDeactive = 1 AND dbo.Product.Discontinue = 'No'";
        //'gSQL = gSQL + " AND dbo.Product.Discontinue = 'No'";
    
        gSQL = gSQL + " GROUP BY dbo.Entity.sFlag,dbo.Entity.SerialNo,dbo.Entity.GroupSL,dbo.Entity.eName, ";
        gSQL = gSQL + " dbo.Product.ProdName, dbo.Product.Model, dbo.Product.ModelSerial,";
        gSQL = gSQL + " dbo.Product.GroupName, dbo.Product.GroupSL,dbo.Entity.EntityType" ;

        SqlCommand cmd = new SqlCommand(gSQL, con);
        con.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        while (dr.Read())
        {    
            gSQL = "";
            gSQL = "INSERT INTO RPTBalance_Online(sFlag,SerialNo,EGroupSL,Entity,ProdName,";
            gSQL = gSQL + " Model,ModelSerial,QtyIN,";
            gSQL = gSQL + " QtyOut,GroupName,GroupSL,";
            gSQL = gSQL + " EntityType,UserID)";
            gSQL = gSQL + " VALUES(";
            gSQL = gSQL + " " + dr["sFlag"] + "," + dr["SerialNo"] + "," + dr["EGroupSL"] + ",";
            gSQL = gSQL + " '" + dr["InSource"] + "','" + dr["ProdName"] + "',";
            gSQL = gSQL + " '" + dr["Model"] + "'," + dr["ModelSerial"] + "," + dr["QtyIN"] + ",";
            gSQL = gSQL + " " + dr["QtyOut"] + ",'" + dr["GroupName"] + "'," + dr["GroupSL"] + ",";
            gSQL = gSQL + " '" + dr["EntityType"] + "','" + Session["UserName"] + "'";
            gSQL = gSQL + " )";

            SqlCommand cmdS = new SqlCommand(gSQL, con1);
            con1.Open();
            cmdS.ExecuteNonQuery();
            con1.Close();

        }
        con.Close();
        dr.Close();
        //------------------------------------------------------------------------------------------------

        gSQL = "";
        gSQL = "SELECT dbo.Entity.sFlag,dbo.Entity.SerialNo,dbo.Entity.GroupSL AS eGroupSL,dbo.Entity.eName AS OutSource,";
        gSQL = gSQL + " dbo.Product.ProdName, dbo.Product.Model, dbo.Product.ModelSerial, ";
        gSQL = gSQL + " dbo.Product.GroupName, dbo.Product.GroupSL,dbo.Entity.EntityType,";
        gSQL = gSQL + " QtyIN =0,";
        gSQL = gSQL + " QtyOut = SUM(CASE WHEN (TrType = 1 OR TrType = 2 OR TrType = 3 OR ";
        gSQL = gSQL + " TrType = 4 OR TrType=-1 OR TrType=-3) THEN ABS(dbo.MRSRDetails.QTy) ELSE 0 END)";
        //gSql = gSql + " UserID='" & sUser & "',PCName='" & sPCName & "',";
        //gSql = gSql + " BranchName='" & sBranchName & "'";
        
        gSQL = gSQL + " FROM dbo.MRSRMaster INNER JOIN";
        gSQL = gSQL + " dbo.MRSRDetails ON dbo.MRSRMaster.MRSRMID = dbo.MRSRDetails.MRSRMID INNER JOIN";
        gSQL = gSQL + " dbo. Product ON dbo.MRSRDetails.ProductID = dbo. Product .ProductID INNER JOIN";
        gSQL = gSQL + " dbo.Entity ON dbo.MRSRMaster.OutSource = dbo.Entity.EID";
    
        gSQL = gSQL + " WHERE dbo.MRSRMaster.TDate <'" + txtFrom.Text + "'";
        gSQL = gSQL + " AND  (dbo.Entity.EID='" + Session["EID"] + "') ";

        if (ddlCategory.SelectedItem.Text != "ALL")
        {
            gSQL = gSQL + " AND dbo.Product.GroupName='" + ddlCategory.SelectedItem.Text + "'";
        }

        if (txtModel.Text != "")
        {
            gSQL = gSQL + " AND dbo.Product.Model='" + txtModel.Text + "'";
        }
    
        gSQL = gSQL + " AND dbo.Entity.ActiveDeactive = 1 AND dbo.Product.Discontinue = 'No'";
        //gSQL = gSQL & " AND dbo.Product.Discontinue = 'No'"
    
        gSQL = gSQL + " GROUP BY dbo.Entity.sFlag,dbo.Entity.SerialNo,dbo.Entity.GroupSL,dbo.Entity.eName,";
        gSQL = gSQL + " dbo.Product.ProdName, dbo.Product.Model, dbo.Product.ModelSerial,";
        gSQL = gSQL + " dbo.Product.GroupName, dbo.Product.GroupSL,dbo.Entity.EntityType";

        cmd = new SqlCommand(gSQL, con);
        con.Open();
        dr = cmd.ExecuteReader();
        while (dr.Read())
        {  
            gSQL = "";
            gSQL = "INSERT INTO RPTBalance_Online(sFlag,SerialNo,EGroupSL,Entity,ProdName,";
            gSQL = gSQL + " Model,ModelSerial,QtyIN,";
            gSQL = gSQL + " QtyOut,GroupName,GroupSL,";
            gSQL = gSQL + " EntityType,UserID)";
            gSQL = gSQL + " VALUES(";
            gSQL = gSQL + " " + dr["sFlag"] + "," + dr["SerialNo"] + "," + dr["EGroupSL"] + ",";
            gSQL = gSQL + " '" + dr["OutSource"] + "','" + dr["ProdName"] + "',";
            gSQL = gSQL + " '" + dr["Model"] + "'," + dr["ModelSerial"] + "," + dr["QtyIN"] + ",";
            gSQL = gSQL + " " + dr["QtyOut"] + ",'" + dr["GroupName"] + "'," + dr["GroupSL"] + ",";
            gSQL = gSQL + " '" + dr["EntityType"] + "','" + Session["UserName"] + "')";

            SqlCommand cmdS = new SqlCommand(gSQL, con1);
            con1.Open();
            cmdS.ExecuteNonQuery();
            con1.Close();

        }
        con.Close();
        dr.Close();

             
        //=================================================================================================
        // DELETE PREVIOUS DATA
        gSQL = "";
        gSQL = "DELETE FROM TempRPTBalance_Online";        
        gSQL = gSQL + " WHERE UserID='" + Session["UserName"] + "'";
        //gSQL = gSQL + " AND EID='" + Session["sBrId"] + "'";
        cmdD = new SqlCommand(gSQL, con1);
        con1.Open();
        cmdD.ExecuteNonQuery();
        con1.Close();


        // INSERT NEW DATA
        gSQL = "";
        gSQL = "SELECT  sFlag,SerialNo,EGroupSL,Entity,ProdName, Model,";
        gSQL = gSQL + " GroupName,GroupSL,EntityType,";
        gSQL = gSQL + " ModelSerial,SUM(QtyIN) as QtyIN,";
        gSQL = gSQL + " SUM(QtyOut) AS QtyOut";
    
        gSQL = gSQL + " From RPTBalance_Online";
        gSQL = gSQL + " WHERE UserID='" + Session["UserName"] + "'";
        
        gSQL = gSQL + " GROUP BY sFlag,SerialNo,EGroupSL,Entity,ProdName,";
        gSQL = gSQL + " GroupName,GroupSL,EntityType,";
        gSQL = gSQL + " Model,MOdelSerial";

        cmd = new SqlCommand(gSQL, con);
        con.Open();
        dr = cmd.ExecuteReader();
        while (dr.Read())
        {    
            gSQL = "";
            gSQL = "INSERT INTO TempRPTBalance_Online(sFlag,SerialNo,Entity,ProdName, ";
            gSQL = gSQL + " Model,ModelSerial,QtyIN,";
            gSQL = gSQL + " QtyOut,QtyBalance,";
            gSQL = gSQL + " GroupName,GroupSL,";
            gSQL = gSQL + " Tag,EGroupSL,EntityType,UserID)";
            gSQL = gSQL + " VALUES(" + dr["sFlag"] + "," + dr["SerialNo"] + ",";
            gSQL = gSQL + " '" + dr["Entity"] + "','" + dr["ProdName"] + "',";
            gSQL = gSQL + " '" + dr["Model"] + "'," + dr["ModelSerial"] + ",";
            gSQL = gSQL + " " + dr["QtyIN"] + "," + dr["QtyOut"] + ",";
            gSQL = gSQL + " '" + (Convert.ToDouble(dr["QtyIN"]) - Convert.ToDouble(dr["QtyOut"])) + "',";
            gSQL = gSQL + " '" + dr["GroupName"] + "'," + dr["GroupSL"] + ",";
            gSQL = gSQL + " 0," + dr["EGroupSL"] + ",'" + dr["EntityType"] + "','" + Session["UserName"] + "'";
            gSQL = gSQL + " )";
            SqlCommand cmdS = new SqlCommand(gSQL, con1);
            con1.Open();
            cmdS.ExecuteNonQuery();
            con1.Close();

        }
        //====================================================================================================


        }

   
    

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //CalcTotalQty(e.Row.Cells[4].Text);
            

            CalcTotal_Cash(e.Row.Cells[4].Text);
            //CalcTotal_Card(e.Row.Cells[3].Text);
            //CalcTotal_Cheque(e.Row.Cells[4].Text);
            //CalcTotal_Req(e.Row.Cells[5].Text);

            //CalcTotal_TP(e.Row.Cells[6].Text);

            // ALIGNMENT
            e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Left;

            e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Center;
            //e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
            //e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Center;
            //e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Center;

            //e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Center;
                                   

        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[1].Text = "Total";
            //e.Row.Cells[4].Text = runningTotalQty.ToString("0,0", CultureInfo.InvariantCulture);
            
            e.Row.Cells[4].Text = runningTotalCash.ToString("0,0", CultureInfo.InvariantCulture);
            //e.Row.Cells[3].Text = runningTotalCard.ToString("0,0", CultureInfo.InvariantCulture);
            //e.Row.Cells[4].Text = runningTotalCheque.ToString("0,0", CultureInfo.InvariantCulture);
            //e.Row.Cells[5].Text = runningTotalReq.ToString("0,0", CultureInfo.InvariantCulture);

            //e.Row.Cells[6].Text = runningTotalTP.ToString("0,0", CultureInfo.InvariantCulture);

            //ALIGNMENT
            e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Left;

            e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Center;
            //e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
            //e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Center;
            //e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Center;

            //e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Center;
            
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

    //CALCULATE TOTAL CHEQUE
    private void CalcTotal_Cheque(string _price)
    {
        try
        {
            runningTotalCheque += Double.Parse(_price);
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write(ex.ToString());
        }
    }

    //CALCULATE TOTAL REQUISITION
    private void CalcTotal_Req(string _price)
    {
        try
        {
            runningTotalReq += Double.Parse(_price);
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



    protected void OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        GridView1.DataBind();
        fnLoadData();
    }

}