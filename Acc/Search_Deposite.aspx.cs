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

using System.IO;

using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;



public partial class Search_Deposite_info : System.Web.UI.Page
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
            fnLoadCombo_Item(ddlBank, "BName", "ID", "tbBankList");
            fnLoadCombo_Item(ddlBankName1, "BName", "ID", "tbBankList");

            this.txtFrom.Text = DateTime.Today.ToString("MM/dd/yyyy");
            this.txtToDate.Text = DateTime.Today.ToString("MM/dd/yyyy");

            //LOAD CTP NAME
            LoadDropDownList_CTP();

        }

        

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
            ddlEntity.Items.Insert(0, new System.Web.UI.WebControls.ListItem("ALL", "ALL"));

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


        int sEType = 0;
        if (rbEntityType.SelectedIndex == 0)
        {
            sEType = 3; //ALL
        }
        else if (rbEntityType.SelectedIndex == 1)
        {
            sEType = 0; //CTP
        }
        else if (rbEntityType.SelectedIndex == 2)
        {
            sEType = 2; //DEALER
        }
        else if (rbEntityType.SelectedIndex == 3)
        {
            sEType = 1; //CORPORATE
        }

        //string sSql = "";
        //sSql = "SELECT RefNo, CONVERT(VARCHAR(10), DepositDate, 103) AS DepositDate, BankName, POSMachine,";
        //sSql = sSql + " BranchName, DepositAmnt, DepositType, CardNo, POSMachine, ApprovalCode,EMEITenor,DepositBy, Remarks, eName,";
        //sSql = sSql + " CASE AccApproved WHEN 0 THEN 'Pending' WHEN 1 THEN 'Approved' WHEN 2 THEN 'Dishonour' END AS aStatus,";
        //sSql = sSql + " AccNote, AccNoteBy, AccUpdateDate";
        string sSql = "";
        if (rbEntityType.SelectedIndex == 0 || rbEntityType.SelectedIndex == 2)
        {
            sSql = sSql + "SELECT CollectionNo,DelearID,Name as DealerName into #temp1 " +
        "FROM [DelearSales].[dbo].[DepositAmnt] " +
        "inner join [DelearSales].[dbo].DelearInfo on DelearID=DAID ";//+
        //"where [DelearSales].[dbo].[DepositAmnt].EntryDate>= '" + Convert.ToDateTime(this.txtFrom.Text) + "' AND [DelearSales].[dbo].[DepositAmnt].EntryDate <= '" + Convert.ToDateTime(this.txtToDate.Text) + "' ";

        }


        sSql = sSql + " SELECT RefNo, CONVERT(VARCHAR(10), DepositDate, 103) AS DepositDate, BankName, POSMachine,";
        sSql = sSql + " BranchName, DepositAmnt,IpdcAmnt, DepositType, CardNo, POSMachine, ApprovalCode,EMEITenor,DepositBy, Remarks, eName,";
        sSql = sSql + " CASE AccApproved WHEN 0 THEN 'Pending' WHEN 1 THEN 'Approved' WHEN 2 THEN 'Dishonour' END AS aStatus,";
        sSql = sSql + " AccNote, AccNoteBy, AccUpdateDate";
        if (rbEntityType.SelectedIndex == 0 || rbEntityType.SelectedIndex == 2)
        {
            sSql = sSql + " ,DealerName";
        }
        else
        {
            sSql = sSql + " ,('')DealerName";
        }
        sSql = sSql + " FROM  dbo.tbDeposit INNER JOIN ";
        sSql = sSql + " dbo.Entity ON dbo.tbDeposit.EID = dbo.Entity.EID";
        if (rbEntityType.SelectedIndex == 0 || rbEntityType.SelectedIndex == 2)
        {
            sSql = sSql + " left join #temp1 on #temp1.CollectionNo = dbo.tbDeposit.RefNo ";
        }
        if (txtInvNo.Text.Length > 0)
        {
            sSql = sSql + " WHERE (RefNo LIKE '%" + txtInvNo.Text + "%'";
            sSql = sSql + " OR DepositAmnt LIKE '%" + txtInvNo.Text + "%'";
            sSql = sSql + " OR CardNo LIKE '%" + txtInvNo.Text + "%')";
        }

        else
        {
            sSql = sSql + " WHERE (DepositDate >= '" + Convert.ToDateTime(this.txtFrom.Text) + "'";
            sSql = sSql + " AND DepositDate <= '" + Convert.ToDateTime(this.txtToDate.Text) + "')";
            //sSql = sSql + " AND (EID='" + Session["EID"] + "')";

            if (ddlBank.SelectedItem.Text != "ALL")
            {
                sSql = sSql + " AND (BankName = '" + ddlBank.SelectedItem.Text + "')";
            }

            if (ddlDepositeType.SelectedItem.Text != "ALL")
            {
                sSql = sSql + " AND (DepositType = '" + ddlDepositeType.SelectedItem.Text + "')";
            }

            if (ddlEntity.SelectedItem.Text != "ALL")
            {
                sSql = sSql + " AND (eName='" + ddlEntity.SelectedItem.Text + "')";
            }

            if (ddlApproval.SelectedItem.Text != "ALL")
            {
                sSql = sSql + " AND (AccApproved='" + ddlApproval.SelectedItem.Value + "')";
            }

            if (sEType!=3)
            {
                sSql = sSql + " AND (SalesOrShowroom='" + sEType + "')";
            }

            if (ddlBankName1.SelectedItem.Text != "ALL")
            {
                sSql = sSql + " AND (POSMachine = '" + ddlBankName1.SelectedItem.Text + "')";
            }

        }

        sSql = sSql + " ORDER BY DepositDate Desc";
        if (rbEntityType.SelectedIndex == 0 || rbEntityType.SelectedIndex == 2)
        {
            sSql = sSql + " drop table #temp1";
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
            //CalcTotal_TP(e.Row.Cells[3].Text);

            //CalcTotal_Cash(e.Row.Cells[4].Text);
            CalcTotal_Card(e.Row.Cells[5].Text);

            // ALIGNMENT
            e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Left;

            //e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
            //e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Center;

            //e.Row.Cells[8].HorizontalAlign = HorizontalAlign.Center;


        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[1].Text = "Total";
            //e.Row.Cells[4].Text = runningTotalQty.ToString("0,0", CultureInfo.InvariantCulture);
            //e.Row.Cells[3].Text = runningTotalTP.ToString("0,0", CultureInfo.InvariantCulture);
            //e.Row.Cells[4].Text = runningTotalCash.ToString("0,0", CultureInfo.InvariantCulture);
            e.Row.Cells[5].Text = runningTotalCard.ToString("0,0", CultureInfo.InvariantCulture);


            //ALIGNMENT
            e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Left;

            //e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
            //e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Center;

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
            xCombo.Items.Insert(0, new System.Web.UI.WebControls.ListItem("ALL", "ALL"));

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


    protected void lnkEdit_Click(object sender, EventArgs e)
    {
        if (System.Convert.ToInt32(Session["Vis"]) == 0)
        {
            Response.Redirect("../login.aspx");
        }

        LinkButton btnsubmit = sender as LinkButton;
        GridViewRow gRow = (GridViewRow)btnsubmit.NamingContainer;
        //txtPID.Text = gvCustomres.DataKeys[gRow.RowIndex].Value.ToString();
        //string sPID = Convert.ToString(gvCustomres.DataKeys[gRow.RowIndex].Value.ToString());
        string sPID = GridView1.DataKeys[gRow.RowIndex].Value.ToString();
        //txtPName.Text = gRow.Cells[0].Text;        
        //this.ModalPopupExtender1.Show();

        //fnLoadCombo_Item(DropDownList1, "BName", "ID", "tbBankList");

        SqlConnection conn = DBConnection.GetConnection();


        string sSql = "";
        sSql = "SELECT RefAID, RefNo, DepositDate, BankName, BankID, BranchName, DepositAmnt,";
        sSql = sSql + " DepositType, DepositBy, Remarks, EntryDate, UserID, EID, AccApproved,";
        sSql = sSql + " AccNote, AccNoteBy, AccUpdateDate, POSMachine, CardNo, ApprovalCode";
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
            this.lblBankName.Text = dr["BankName"].ToString();

            this.lblBrName.Text = dr["BranchName"].ToString();

            this.lblAmnt1.Text = dr["DepositAmnt"].ToString();
            this.lblDepositeType.Text = dr["DepositType"].ToString();

            this.lblDBy.Text = dr["DepositBy"].ToString();
            this.lblRemarks1.Text = dr["Remarks"].ToString();

            this.lblCardNo.Text = dr["CardNo"].ToString();
            this.lblAppCode.Text = dr["ApprovalCode"].ToString();
            this.lblPOSName.Text = dr["POSMachine"].ToString();


            if (dr["AccApproved"].ToString() == "1")
            {
                RadioButtonList1.SelectedIndex = 0;
            }
            else if (dr["AccApproved"].ToString() == "2")
            {
                RadioButtonList1.SelectedIndex = 1;
            }
            else
            {
                RadioButtonList1.Items[0].Selected = false;
                RadioButtonList1.Items[1].Selected = false; 
            }
            this.txtNote.Text = dr["AccNote"].ToString();


        }
        else
        {
            this.lblID.Text = "";
            this.lblRefNo.Text = "";
            this.lblDDate.Text = "";

            this.lblBrName.Text = "";
            this.lblAmnt1.Text = "";
            this.lblDepositeType.Text = "";

            this.lblDBy.Text = "";
            this.lblRemarks1.Text = "";

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

        int iType = 0;

        if (RadioButtonList1.SelectedIndex == 0)
        {
            iType = 1;
        }
        else
        {
            iType = 2;
        }

        //UPDATE DATA
        if (Session["UserName"].ToString()!="finance")
        {
            string sSql = "";
            sSql = "UPDATE tbDeposit SET AccApproved='" + iType + "',";
            sSql = sSql + " AccNote='" + this.txtNote.Text + "',";
            //sSql = sSql + " AccNoteBy='" + Session["sUser"].ToString() + "',";//Session["UserName"].ToString()
            sSql = sSql + " AccNoteBy='" + Session["UserName"].ToString() + "',";
            sSql = sSql + " AccUpdateDate='" + DateTime.Today.ToString("MM/dd/yyyy") + "'";

            sSql = sSql + " WHERE RefNo='" + lblRefNo.Text + "'";

            SqlCommand cmd = new SqlCommand(sSql, conn);
            cmd.ExecuteNonQuery();
        }
        
        conn.Close();

        //LOAD DATA IN GRID
        fnLoadData();


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
        //string sMasterID = grdrow.Cells[8].Text;

        string sSql = "";

        ////DELETE FROM Master Table
        //sSql = "";
        //sSql = "DELETE FROM tbDeposit";
        //sSql = sSql + " WHERE RefNo='" + sBillNo + "'";

        //SqlCommand cmd = new SqlCommand(sSql, conn);
        //conn.Open();
        //cmd.ExecuteNonQuery();
        //conn.Close();


        //LOAD DATA IN GRID
        fnLoadData();


    }

    private void ExportGridToExcel()
    {
        try
        {
            Response.Clear();
            Response.Buffer = true;
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Charset = "";
            string FileName = "CustOrder_" + DateTime.Now + ".xls";
            StringWriter strwritter = new StringWriter();
            HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
            GridView1.GridLines = GridLines.Both;
            GridView1.HeaderStyle.Font.Bold = true;
            GridView1.RenderControl(htmltextwrtter);
            Response.Write(strwritter.ToString());
            Response.End();
        }
        catch
        {
            //
        }

    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        //required to avoid the run time error "  
        //Control 'GridView1' of type 'Grid View' must be placed inside a form tag with runat=server."  
    }

    protected void btnDownloadToExcel_Click(object sender, EventArgs e)
    {
        ExportGridToExcel();
    }

    protected void btnExportToPDF_Click(object sender, EventArgs e)
    {
        try
        {
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=acc_report.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            GridView1.RenderControl(hw);
            StringReader sr = new StringReader(sw.ToString());
            Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);

#pragma warning disable CS0612 // Type or member is obsolete
            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
#pragma warning restore CS0612 // Type or member is obsolete

            PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();
            htmlparser.Parse(sr);
            pdfDoc.Close();
            Response.Write(pdfDoc);
            Response.End();
            GridView1.AllowPaging = true;
            GridView1.DataBind();


            
        }
        catch
        {
            //
        }

    }

    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}