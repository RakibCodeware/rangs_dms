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

public partial class Search_DisCode : System.Web.UI.Page
{
    SqlConnection conn = DBConnection.GetConnection();
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

    protected void Page_Load(object sender, EventArgs e)
    {
        if (System.Convert.ToInt32(Session["Vis"]) == 0)
        {
            Response.Redirect("../Account/Login.aspx");
        }

        if (!IsPostBack)
        {
            this.txtFrom.Text = DateTime.Today.ToString("MM/dd/yyyy");
            this.txtToDate.Text = DateTime.Today.ToString("MM/dd/yyyy");

            //LOAD CTP NAME
            LoadDropDownList_CTP();

            dt = new DataTable();
            //MakeTable();

        }
        else
        {
            dt = (DataTable)ViewState["dt"];
        }
        ViewState["dt"] = dt;

    }


    //LOAD CTP IN DROPDOWN LIST
    protected void LoadDropDownList_CTP()
    {
        SqlConnection conn = DBConnection.GetConnection();

        String strQuery = "select EID,eName from Entity ";
        strQuery = strQuery + " WHERE (ActiveDeactive = 1) AND";
        strQuery = strQuery + " (EntityType = 'showroom' OR EntityType = 'Dealer')";
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


    protected void lnkPrint_Click(object sender, EventArgs e)
    {
        LinkButton btnsubmit = sender as LinkButton;
        GridViewRow gRow = (GridViewRow)btnsubmit.NamingContainer;
        //txtPID.Text = gvCustomres.DataKeys[gRow.RowIndex].Value.ToString();

        ////Session["AssAID"] = GridView1.DataKeys[gRow.RowIndex].Value.ToString();
        //Session["sBillNo"] = GridView1.DataKeys[gRow.RowIndex].Value.ToString();
        ////Session["sBillNo"] = this.txtInvoiceNo.Text;
        //Session["sReportType"] = "RPT_Sales_Bill";

        //Response.Redirect("Sales_Bill_Print.aspx");

    }

    protected void lnkView_Click(object sender, EventArgs e)
    {
        //if (System.Convert.ToInt32(Session["Vis"]) == 0)
        //{
        //    Response.Redirect("Default.aspx");
        //}

       
        //CLEAR DATA TABLE
        dt.Clear();
        
        LinkButton btnsubmit = sender as LinkButton;
        GridViewRow gRow = (GridViewRow)btnsubmit.NamingContainer;
        //txtPID.Text = gvCustomres.DataKeys[gRow.RowIndex].Value.ToString();
        //string sPID = Convert.ToString(gvCustomres.DataKeys[gRow.RowIndex].Value.ToString());
        string sPID = GridView1.DataKeys[gRow.RowIndex].Value.ToString();
        //txtPName.Text = gRow.Cells[0].Text;        
        //this.ModalPopupExtender1.Show();
        
        SqlConnection conn = DBConnection.GetConnection();
        
        string sSql = "";
        
        //LOAD CTP INFORMATION
        sSql = "";
        sSql ="SELECT dbo.tbKeyGen.GAID, dbo.tbKeyGen.DisCode, dbo.tbKeyGen.DisID, dbo.tbKeyGen.DisAmnt,";
        sSql = sSql + " dbo.tbKeyGen.DisRef, dbo.tbKeyGen.dTag, ";
        sSql = sSql + " dbo.tbKeyGen.ActiveInactive, dbo.tbKeyGen.UserID, dbo.tbKeyGen.EntryDate,";
        sSql = sSql + " dbo.Entity.EID, dbo.Entity.eName";
        sSql = sSql + " FROM dbo.tbKeyGen INNER JOIN";
        sSql = sSql + " dbo.Entity ON dbo.tbKeyGen.EID = dbo.Entity.EID";
        sSql = sSql + " Where dbo.tbKeyGen.GAID='" + sPID + "'";
        
        SqlCommand cmdC = new SqlCommand(sSql, conn);
        conn.Open();
        SqlDataReader drC = cmdC.ExecuteReader();
        if (drC.Read())
        {
            lblID.Text = drC["GAID"].ToString();
            lblInv.Text = drC["DisCode"].ToString();
            lblDate.Text = drC["EntryDate"].ToString();

            if (drC["ActiveInactive"].ToString() == "1")
            {
                CheckBox1.Checked = true;
            }
            else
            {
                CheckBox1.Checked = false;
            }
        }
        conn.Close();


        this.ModalPopupExtender1.Show();


    }

    protected void lnkDel_Click(object sender, EventArgs e)
    {
        
        GridViewRow grdrow = (GridViewRow)((LinkButton)sender).NamingContainer;
        string rowNumber = grdrow.Cells[0].Text;
        string sBillNo = grdrow.Cells[1].Text;
        string sMasterID = grdrow.Cells[8].Text;

        string sSql = "";

        ////DELETE FROM Master Table
        //sSql = "";
        //sSql = "DELETE FROM MRSRMaster";
        //sSql = sSql + " WHERE MRSRMID='" + sMasterID + "'";

        //SqlCommand cmd = new SqlCommand(sSql, conn);
        //conn.Open();
        //cmd.ExecuteNonQuery();
        //conn.Close();


        ////DELETE FROM Details Table
        //sSql = "";
        //sSql = "DELETE FROM MRSRDetails";
        //sSql = sSql + " WHERE MRSRMID='" + sMasterID + "'";

        //SqlCommand cmd1 = new SqlCommand(sSql, conn);
        //conn.Open();
        //cmd1.ExecuteNonQuery();
        //conn.Close();


        ////LOAD DATA IN GRID
        //fnLoadData();


    }

    protected void lnkEdit_Click(object sender, EventArgs e)
    {
        
        GridViewRow grdrow = (GridViewRow)((LinkButton)sender).NamingContainer;
        string rowNumber = grdrow.Cells[0].Text;
        Session["sBillNoS"] = grdrow.Cells[1].Text;
        Session["sMasterID"] = grdrow.Cells[8].Text;
                
        //Response.Redirect("Sales_EditS.aspx");       


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
        //s = WebConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        //con = new SqlConnection(s);
        SqlConnection con = DBConnection.GetConnection();
        con.Open();

        string sSql = "";                
        sSql = "SELECT dbo.tbKeyGen.GAID, dbo.tbKeyGen.DisCode, dbo.tbKeyGen.DisAmnt,";
        sSql = sSql + " dbo.tbKeyGen.DisRef, dbo.tbKeyGen.UserID, dbo.Entity.eName,";
        sSql = sSql + " CONVERT(varchar(12), dbo.tbKeyGen.EntryDate, 105) AS TDate,"; 
        //sSql = sSql + " dbo.Entity.EID, dbo.Entity.eName, dbo.tbKeyGen.dTag, dbo.tbKeyGen.ActiveInactive";
        sSql = sSql + " CASE dTag WHEN 0 THEN 'Not Use' ELSE 'Used' END AS tStatus,";
        sSql = sSql + " CASE dbo.tbKeyGen.ActiveInactive WHEN 0 THEN 'Inactive' ELSE 'Active' END AS dActive";
        sSql = sSql + " FROM dbo.tbKeyGen INNER JOIN";
        sSql = sSql + " dbo.Entity ON dbo.tbKeyGen.EID = dbo.Entity.EID";

        sSql = sSql + " WHERE (CONVERT(date, dbo.tbKeyGen.EntryDate, 101) >= '" + Convert.ToDateTime(this.txtFrom.Text) + "'";
        sSql = sSql + " AND CONVERT(date, dbo.tbKeyGen.EntryDate, 101) <= '" + Convert.ToDateTime(this.txtToDate.Text) + "')";

        if (ddlEntity.SelectedItem.Text != "ALL")
        {
            sSql = sSql + " AND (dbo.Entity.eName = '" + ddlEntity.SelectedItem.Text + "')";
        }

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

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //CalcTotalQty(e.Row.Cells[4].Text);
            CalcTotal_TP(e.Row.Cells[3].Text);

            //CalcTotal_Cash(e.Row.Cells[4].Text);
            //CalcTotal_Card(e.Row.Cells[5].Text);

            // ALIGNMENT
            e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Left;

            e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Left;
            e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Left;

            e.Row.Cells[7].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[8].HorizontalAlign = HorizontalAlign.Center;
            

        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[2].Text = "Total";
            //e.Row.Cells[4].Text = runningTotalQty.ToString("0,0", CultureInfo.InvariantCulture);
            e.Row.Cells[3].Text = runningTotalTP.ToString("0,0", CultureInfo.InvariantCulture);
            //e.Row.Cells[4].Text = runningTotalCash.ToString("0,0", CultureInfo.InvariantCulture);
            //e.Row.Cells[5].Text = runningTotalCard.ToString("0,0", CultureInfo.InvariantCulture);


            //ALIGNMENT
            e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Left;

            e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
            //e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Center;
            //e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Center;
            
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


    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        if (System.Convert.ToInt32(Session["Vis"]) == 0)
        {
            Response.Redirect("Default.aspx");
        }

        SqlConnection con = DBConnection.GetConnection();

        int iSS = 0;

        if (CheckBox1.Checked == true)
            iSS = 1;
        else
            iSS = 0;

        string sSql = "";
        sSql = "UPDATE tbKeyGen Set ActiveInactive='" + iSS + "'";
        sSql = sSql + " where GAID='" + lblID.Text + "'";
        SqlCommand cmdIns = new SqlCommand(sSql, conn);
        conn.Open();
        cmdIns.ExecuteNonQuery();
        conn.Close();

        fnLoadData();

    }

    

}