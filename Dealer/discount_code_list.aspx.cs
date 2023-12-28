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

public partial class discount_code_list : System.Web.UI.Page
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
            //this.txtFrom.Text = DateTime.Today.ToString("MM/dd/yyyy");
            //this.txtToDate.Text = DateTime.Today.ToString("MM/dd/yyyy");

            //LOAD DATA IN GRID
            fnLoadData();
        }
        
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

        sSql = sSql + " WHERE (dbo.Entity.EID = '" + Session["sBrId"].ToString() + "')";
        sSql = sSql + " AND (dbo.tbKeyGen.dTag=0)";

        sSql = sSql + " ORDER BY dbo.tbKeyGen.EntryDate DESC";

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
            //CalcTotal_TP(e.Row.Cells[3].Text);

            //CalcTotal_Cash(e.Row.Cells[4].Text);
            //CalcTotal_Card(e.Row.Cells[5].Text);

            // ALIGNMENT
            e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Left;

            e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Left;
            e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Center;
                        

        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[2].Text = "";
            //e.Row.Cells[4].Text = runningTotalQty.ToString("0,0", CultureInfo.InvariantCulture);
            //e.Row.Cells[3].Text = runningTotalTP.ToString("0,0", CultureInfo.InvariantCulture);
            //e.Row.Cells[4].Text = runningTotalCash.ToString("0,0", CultureInfo.InvariantCulture);
            //e.Row.Cells[5].Text = runningTotalCard.ToString("0,0", CultureInfo.InvariantCulture);


            //ALIGNMENT
            e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Left;

            e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Center;
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

    
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        if (System.Convert.ToInt32(Session["Vis"]) == 0)
        {
            Response.Redirect("Default.aspx");
        }

        //SqlConnection con = DBConnection.GetConnection();

        //int iSS = 0;

        //if (CheckBox1.Checked == true)
        //    iSS = 1;
        //else
        //    iSS = 0;

        //string sSql = "";
        //sSql = "UPDATE tbKeyGen Set ActiveInactive='" + iSS + "'";
        //sSql = sSql + " where GAID='" + lblID.Text + "'";
        //SqlCommand cmdIns = new SqlCommand(sSql, conn);
        //conn.Open();
        //cmdIns.ExecuteNonQuery();
        //conn.Close();

        //fnLoadData();

    }

    

}