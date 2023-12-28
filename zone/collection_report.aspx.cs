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

public partial class CTP_collection_report : System.Web.UI.Page
{
    SqlConnection conn = DBConnection.GetConnection();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (System.Convert.ToInt32(Session["Vis"]) == 0)
        {
            Response.Redirect("../Account/Login.aspx");
        }

        if (!IsPostBack)
        {            
            
        }

    }


    protected void SearchData(object sender, EventArgs e)
    {
        //
    }

    protected void PopupMessage(string Msg, Control controlID)
    {
        ScriptManager.RegisterClientScriptBlock(controlID, controlID.GetType(), "msg", "alert('" + Msg + "');", true);
    }

    protected void OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        GridView1.DataBind();
        //fnLoadData();
    }


    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //e.Row.Cells[1].Visible = false;
        }

    }


    protected void lnkPrint_Click(object sender, EventArgs e)
    {
        LinkButton btnsubmit = sender as LinkButton;
        GridViewRow gRow = (GridViewRow)btnsubmit.NamingContainer;
        //txtPID.Text = gvCustomres.DataKeys[gRow.RowIndex].Value.ToString();

        //OleDbConnection conn = DBConnection.Connection();


        //Session["AssAID"] = GridView1.DataKeys[gRow.RowIndex].Value.ToString();
        Session["sMID"] = GridView1.DataKeys[gRow.RowIndex].Value.ToString();
        //Session["sBillNo"] = this.txtInvoiceNo.Text;
        Session["ReportType"] = "RPT_Sales_Bill";

        //Response.Redirect("Print.aspx");

    }

    protected void lnkEdit_Click(object sender, EventArgs e)
    {
        if (System.Convert.ToInt32(Session["Vis"]) == 0)
        {
            Response.Redirect("../Account/Login.aspx");
        }

        //CLEAR PREVIOUS DATA
        //fnClearText();

        LinkButton btnsubmit = sender as LinkButton;
        GridViewRow gRow = (GridViewRow)btnsubmit.NamingContainer;
        //txtPID.Text = gvCustomres.DataKeys[gRow.RowIndex].Value.ToString();
        //string sPID = Convert.ToString(gvCustomres.DataKeys[gRow.RowIndex].Value.ToString());
        string sPID = GridView1.DataKeys[gRow.RowIndex].Value.ToString();
        //txtPName.Text = gRow.Cells[0].Text;        
        //this.ModalPopupExtender1.Show();

        Session["sMID"] = sPID;
        Session["sInvNo"] = gRow.Cells[2].Text;
        Session["sInvDate"] = gRow.Cells[3].Text;
        Session["CustName"] = gRow.Cells[8].Text;
        Session["CustID"] = gRow.Cells[9].Text;

        //string sID= gRow.Cells[9].Text;

        //Response.Redirect("Sales_Edit.aspx");


    }

    protected void lnkDel_Click(object sender, EventArgs e)
    {
        LinkButton btnsubmit = sender as LinkButton;
        GridViewRow gRow = (GridViewRow)btnsubmit.NamingContainer;
        Int16 sJobNo = Convert.ToInt16(GridView1.DataKeys[gRow.RowIndex].Value);

        //SqlConnection conn = db.ConnectDB();

        try
        {
            //int index = gvCustomres.SelectedIndex;
            //string sJobNo = Convert.ToString(gvCustomres.DataKeys[e.RowIndex].Values["EmpCod"].ToString());
            SqlCommand com = new SqlCommand("DELETE * FROM tbMRSRMaster WHERE MID = " + sJobNo + "", conn);
            conn.Open();
            com.ExecuteNonQuery();
            conn.Close();

            com = new SqlCommand("DELETE * FROM tbMRSRDetails WHERE MID = " + sJobNo + "", conn);
            conn.Open();
            com.ExecuteNonQuery();
            conn.Close();

            //DELETE FROM PAYMENT TABLE
            com = new SqlCommand("DELETE * FROM tbPayment WHERE MID = " + sJobNo + "", conn);
            conn.Open();
            com.ExecuteNonQuery();
            conn.Close();

            //LOAD DATA
            //fnLoadData();

        }
        catch (InvalidCastException err)
        {
            throw (err);
        }

    }



}