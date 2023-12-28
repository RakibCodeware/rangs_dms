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

public partial class ToDayDeposit : System.Web.UI.Page
{
    SqlConnection conn = DBConnection.GetConnection();
    long i;

    private double runningTotalQty = 0;
    private double runningTotalTP = 0;
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
            //LOAD DATA IN GRID
            fnLoadData();
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

        ////Response.Redirect("Sales_Bill_Print.aspx");


    }

    protected void PopupMessage(string Msg, Control controlID)
    {
        ScriptManager.RegisterClientScriptBlock(controlID, controlID.GetType(), "msg", "alert('" + Msg + "');", true);
    }


    //LOAD SALES SUMMARY CHALLAN WISE
    private void fnLoadData()
    {
        //s = WebConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        //con = new SqlConnection(s);
        SqlConnection con = DBConnection.GetConnection();
        con.Open();

        string sSql = "";
        sSql = "SELECT dbo.tbDeposit.RefNo, CONVERT(VARCHAR(10), dbo.tbDeposit.DepositDate, 103) AS DepositDate, ";
        sSql = sSql + " dbo.tbDeposit.BranchName, dbo.tbDeposit.DepositAmnt, ";
        sSql = sSql + " dbo.tbDeposit.DepositBy, dbo.tbDeposit.Remarks, dbo.tbDeposit.EntryDate,";
        sSql = sSql + " dbo.tbDeposit.DepositType, dbo.tbBankList.BName, ";
        sSql = sSql + " dbo.Entity.eName";
        sSql = sSql + " FROM  dbo.tbDeposit INNER JOIN";
        sSql = sSql + " dbo.tbBankList ON dbo.tbDeposit.BankID = dbo.tbBankList.ID INNER JOIN";
        sSql = sSql + " dbo.Entity ON dbo.tbDeposit.EID = dbo.Entity.EID";

        sSql = sSql + " WHERE (dbo.tbDeposit.DepositDate >= '" + DateTime.Today.ToString("MM/dd/yyyy") + "')";
        sSql = sSql + " AND dbo.tbDeposit.DepositDate <= '" + DateTime.Today.ToString("MM/dd/yyyy") + "'";

        sSql = sSql + " ORDER BY dbo.Entity.eName, dbo.tbDeposit.RefNo";


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
            
            //e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Center;
            //e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Center;

            e.Row.Cells[9].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[10].HorizontalAlign = HorizontalAlign.Center;


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



    protected void lnkEdit_Click(object sender, EventArgs e)
    {
        if (System.Convert.ToInt32(Session["Vis"]) == 0)
        {
            Response.Redirect("Default.aspx");
        }

        LinkButton btnsubmit = sender as LinkButton;
        GridViewRow gRow = (GridViewRow)btnsubmit.NamingContainer;
        //txtPID.Text = gvCustomres.DataKeys[gRow.RowIndex].Value.ToString();
        //string sPID = Convert.ToString(gvCustomres.DataKeys[gRow.RowIndex].Value.ToString());
        string sPID = GridView1.DataKeys[gRow.RowIndex].Value.ToString();
        //txtPName.Text = gRow.Cells[0].Text;        
        //this.ModalPopupExtender1.Show();

        fnLoadCombo_Item(DropDownList1, "BName", "ID", "tbBankList");

        SqlConnection conn = DBConnection.GetConnection();


        string sSql = "";
        sSql = "SELECT RefAID, RefNo, DepositDate, BankName, BankID, BranchName, DepositAmnt,";
        sSql = sSql + " DepositType, DepositBy, Remarks, EntryDate, UserID, EID";
        sSql = sSql + " FROM dbo.tbDeposit";
        sSql = sSql + " WHERE RefNo='" + sPID + "'";
        SqlCommand cmd = new SqlCommand(sSql, conn);
        conn.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            this.lblID.Text = dr["RefAID"].ToString();
            this.lblRefNo.Text = dr["RefNo"].ToString();
            this.lblDDate.Text = dr["DepositDate"].ToString();
            this.DropDownList1.SelectedItem.Text = dr["BankName"].ToString();
            this.DropDownList1.SelectedItem.Value = dr["BankID"].ToString();

            this.txtBrName.Text = dr["BranchName"].ToString();

            this.txtAmnt1.Text = dr["DepositAmnt"].ToString();
            this.ddlDType.SelectedItem.Text = dr["DepositType"].ToString();

            this.txtDBy.Text = dr["DepositBy"].ToString();
            this.txtRemarks1.Text = dr["Remarks"].ToString();

        }
        else
        {
            this.lblID.Text = "";
            this.lblRefNo.Text = "";
            this.lblDDate.Text = "";

            this.txtBrName.Text = "";
            this.txtAmnt1.Text = "";

            this.txtDBy.Text = "";
            this.txtRemarks1.Text = "";

        }

        this.ModalPopupExtender1.Show();


    }


    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        if (System.Convert.ToInt32(Session["Vis"]) == 0)
        {
            Response.Redirect("Default.aspx");
        }
        SqlConnection conn = DBConnection.GetConnection();
        conn.Open();

        //UPDATE DATA
        string sSql = "";
        sSql = "UPDATE tbDeposit SET BankName='" + this.DropDownList1.SelectedItem.Text + "',";
        sSql = sSql + " BranchName='" + this.txtBrName.Text + "',";
        sSql = sSql + " DepositAmnt='" + this.txtAmnt1.Text + "',";
        sSql = sSql + " DepositType='" + this.ddlDType.SelectedItem.Text + "',";
        sSql = sSql + " DepositBy='" + this.txtDBy.Text + "',";
        sSql = sSql + " Remarks='" + this.txtRemarks1.Text + "'";

        sSql = sSql + " WHERE RefNo='" + lblRefNo.Text + "'";

        SqlCommand cmd = new SqlCommand(sSql, conn);
        cmd.ExecuteNonQuery();
        conn.Close();



    }

    protected void lnkDel_Click(object sender, EventArgs e)
    {
        /*
        LinkButton btnsubmit = sender as LinkButton;
        GridViewRow gRow = (GridViewRow)btnsubmit.NamingContainer;
        //txtPID.Text = gvCustomres.DataKeys[gRow.RowIndex].Value.ToString();
        string sJobNo = Convert.ToString(GridView1.DataKeys[gRow.RowIndex].Value);
        */

        GridViewRow grdrow = (GridViewRow)((LinkButton)sender).NamingContainer;
        string rowNumber = grdrow.Cells[0].Text;
        string sBillNo = grdrow.Cells[1].Text;
        string sMasterID = grdrow.Cells[8].Text;

        string sSql = "";

        //DELETE FROM Master Table
        sSql = "";
        sSql = "DELETE FROM tbDeposit";
        sSql = sSql + " WHERE RefNo='" + sBillNo + "'";

        SqlCommand cmd = new SqlCommand(sSql, conn);
        conn.Open();
        cmd.ExecuteNonQuery();
        conn.Close();


        //LOAD DATA IN GRID
        fnLoadData();


    }

    private void fnLoadCombo_Item(DropDownList xCombo, string pField, string pFieldID, string pTable)
    {
        SqlConnection conn = DBConnection.GetConnection();

        try
        {
            conn.Open();
            string sqlfns = "SELECT * FROM " + pTable + " ORDER BY " + pField + " ASC";
            SqlCommand cmd = new SqlCommand(sqlfns, conn);

            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sqlfns;
            cmd.Connection = conn;

            xCombo.DataSource = cmd.ExecuteReader();
            xCombo.DataTextField = pField;
            //ddlEmp.DataValueField = "SupName";
            xCombo.DataValueField = pFieldID;
            xCombo.DataBind();

            //Add blank item at index 0.
            xCombo.Items.Insert(0, new ListItem("N/A", "0"));

            conn.Close();

        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.Message;
        }
        finally
        {
            conn.Close();
        }

    }

}