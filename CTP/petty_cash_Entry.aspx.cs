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

public partial class petty_cash_Entry : System.Web.UI.Page
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
            fnLoadCombo_Item(ddlParticulars, "LeadgerName", "LeadgerID", "AccLeadger");
            //fnLoadCombo_Item(DropDownList1, "BName", "ID", "tbBankList");
            //fnLoadCombo_Item(ddlBranch, "Branch", "BankID", "tbBank");

            if (chkAutoNo.Checked == true)
            {
                //LOAD AUTO REQUEST NUMBER
                fnLoadAutoBillNo();
                                
            }

            //LOAD TODAY DATA
            fnLoadData();

            txtFrom.Text = DateTime.Today.ToString("MM/dd/yyyy");

        }
        

    }


    protected void fnLoadData()
    {
        
        //string InvFDate = ddlYear.SelectedItem.Text + "" + ddlMonth.SelectedItem.Value + "" + ddlDay.SelectedItem.Text;
        //string InvSDate = ddlYear2.SelectedItem.Text + "" + ddlMonth2.SelectedItem.Value + "" + ddlDay2.SelectedItem.Text;

        //DateTime tDate;
        //tDate = Convert.ToDateTime(this.txtFrom.Text);

        SqlConnection conn = DBConnection.GetConnection();
        conn.Open();

        string sSql = "";
        sSql = "SELECT RefAID, RefNo, CONVERT(VARCHAR(10), ExpenseDate, 103) AS ExpenseDate, LeadgerName, ";
        sSql = sSql + " LDesc, ExpenseAmnt, ExpenseBy, Remarks";
        sSql = sSql + " FROM tbPettyCash ";

        sSql = sSql + " WHERE ExpenseDate='" + DateTime.Today.ToString("MM/dd/yyyy") + "'";
        sSql = sSql + " AND EID='" + Session["EID"] + "'";

        sSql = sSql + " ORDER BY ExpenseDate, RefNo";
        

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
            //xCombo.Items.Insert(0, new ListItem("N/A", "0"));

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
    
    
    protected void btnSaveClick(object sender, EventArgs e)
    {
        SqlConnection conn = DBConnection.GetConnection();
        //fnLoadMRSRNo();

        string sSql = "";
        DateTime tDate;

        if (txtRefNo.Text == "")
        {
            PopupMessage("Please enter Ref. number.", btnSave);
            txtRefNo.Focus();
            return;
        }

        //CHALLAN DATE VALIDATION        
        if (txtFrom.Text == "")
        {
            PopupMessage("Please enter Date.", btnSave);
            txtFrom.Focus();
            return;
        }

        if (txtAmnt.Text.Length == 0)
        {
            PopupMessage("Please enter Amount.", btnSave);
            txtAmnt.Focus();
            return;
        }

        if (txtAmnt.Text == "")
        {
            PopupMessage("Please enter Amount.", btnSave);
            txtAmnt.Focus();
            return;
        }

        tDate = Convert.ToDateTime(this.txtFrom.Text);


        if (chkAutoNo.Checked == true)
        {
            //LOAD AUTO REQUEST NUMBER
            fnLoadAutoBillNo();
        }


        //----------------------------------------------------------------------
        //CHECK DUPLICATE CHALLAN NO.
        sSql = "";
        sSql = "SELECT RefAID FROM tbPettyCash" +
            " WHERE RefNo='" + this.txtRefNo.Text + "'";
        //" WHERE MRSRCode='" + this.txtMRSR.Text + 'S' + "'" +
        //" AND TrType=4";
        SqlCommand cmdd = new SqlCommand(sSql, conn);
        conn.Open();
        SqlDataReader drd = cmdd.ExecuteReader();
        try
        {
            if (drd.Read())
            {
                //iMRSRID = Convert.ToInt32(dr["MRSRMID"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert",
                            "<script>alert('" + "This Ref. no. already exists." + "');</script>", false);
                txtRefNo.Focus();
                return;
            }
        }
        catch (InvalidCastException err)
        {
            throw (err);
        }
        finally
        {
            drd.Dispose();
            drd.Close();
            conn.Close();
        }
        //----------------------------------------------------------------------


        //SAVE DATA IN MASTER TABLE
        sSql = "";
        sSql = "INSERT INTO tbPettyCash(RefNo,ExpenseDate,EID," +
               "LeadgerID,LeadgerName,LDesc,ExpenseAmnt,ExpenseBy,Remarks,UserID,EntryDate)" +
                     " Values ('" + this.txtRefNo.Text + "','" + tDate + "','" + Session["EID"] + "', " +
                     " '" + ddlParticulars.SelectedItem.Value + "','" + ddlParticulars.SelectedItem.Text + "'," +
                     " '" + txtBranch.Text + "','" + txtAmnt.Text + "'," +
                     " '" + txtDepositBy.Text + "', '" + txtRemarks.Text + "'," +
                     " '" + Session["UserID"] + "', '" + DateTime.Today + "'" +
                     " )";
        SqlCommand cmd = new SqlCommand(sSql, conn);
        conn.Open();
        cmd.ExecuteNonQuery();
        conn.Close();
                      
        
        //lblSaveMessage.Text = "Save Data Successfully.";

        ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert",
                        "<script>alert('" + "Save Successfully." + "');</script>", false);

        //------------------------------------------------------------------------------------------
        //CLEAR ALL TEXT
        txtRefNo.Text = "";
        txtFrom.Text = "";        
        txtFrom.Text = DateTime.Today.ToString("MM/dd/yyyy");

        txtBranch.Text = "";
        txtAmnt.Text = "";
        txtDepositBy.Text = "";
        txtRemarks.Text = "";

        if (chkAutoNo.Checked == true)
        {
            //LOAD AUTO REQUEST NUMBER
            fnLoadAutoBillNo();
        }

        //LOAD TODAY DATA
        fnLoadData();

        //------------------------------------------------------------------------------------------

        return;
    }

    protected void PopupMessage(string Msg, Control controlID)
    {
        ScriptManager.RegisterClientScriptBlock(controlID, controlID.GetType(), "msg", "alert('" + Msg + "');", true);

    }

    protected void fnLoadAutoBillNo()
    {

        SqlConnection con = DBConnection.GetConnection();
        //con.Open();

        int xMax = 0;
        string sAutoNo = "";
        string sSql = "";
        sSql = "SELECT ISNULL(MAX(RIGHT(RefNo, 5)), 0) AS RefNo" +
            " FROM dbo.tbPettyCash" +
            " WHERE (LEFT(RefNo, 11) = '" + "" + Session["sBrCode"] + "" + DateTime.Now.ToString("yyyy") + "-" + "')";
        //" AND TrType=3";

        SqlCommand cmd = new SqlCommand(sSql, con);
        con.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        try
        {
            if (dr.Read())
            {
                //xMax = dr["JobNo"].ToString();
                xMax = Convert.ToInt32(dr["RefNo"]) + 1;
                sAutoNo = "" + Session["sBrCode"] + "" + DateTime.Now.ToString("yyyy") + "-" + xMax.ToString("00000");
                txtRefNo.Text = sAutoNo;
            }
            else
            {
                xMax = Convert.ToInt32(dr["RefNo"]) + 1;
                sAutoNo = "" + Session["sBrCode"] + "" + DateTime.Now.ToString("yyyy") + "-" + xMax.ToString("00000");
                txtRefNo.Text = sAutoNo;
            }
        }
        catch (InvalidCastException err)
        {
            throw (err);
        }
        finally
        {
            dr.Dispose();
            dr.Close();
            con.Close();
        }
    }

    protected void chkAutoNo_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAutoNo.Checked == true)
        {
            //LOAD AUTO REQUEST NUMBER
            fnLoadAutoBillNo();
        }
        else
        {
            txtRefNo.Text = "";
        }
    }

    protected void OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        GridView1.DataBind();
                
        fnLoadData();
        
    }


    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //e.Row.Cells[1].Visible = false;
        }

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

        //fnLoadCombo_Item(DropDownList1, "BName", "ID", "tbBankList");
        fnLoadCombo_Item(DropDownList1, "LeadgerName", "LeadgerID", "AccLeadger");


        SqlConnection conn = DBConnection.GetConnection();


        string sSql = "";
        sSql = "SELECT RefAID, RefNo, ExpenseDate, LeadgerID, LeadgerName, LDesc, ExpenseAmnt,";
        sSql = sSql + " ExpenseBy, Remarks, EntryDate, UserID, EID";
        sSql = sSql + " FROM dbo.tbPettyCash";
        sSql = sSql + " WHERE RefAID='" + sPID + "'";
        SqlCommand cmd = new SqlCommand(sSql, conn);
        conn.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            this.lblID.Text = dr["RefAID"].ToString();
            this.lblRefNo.Text = dr["RefNo"].ToString();
            this.lblDDate.Text = dr["ExpenseDate"].ToString();
            this.DropDownList1.SelectedItem.Text = dr["LeadgerName"].ToString();
            this.DropDownList1.SelectedItem.Value = dr["LeadgerID"].ToString();

            this.txtBrName.Text = dr["LDesc"].ToString();

            this.txtAmnt1.Text = dr["ExpenseAmnt"].ToString();

            this.txtDBy.Text = dr["ExpenseBy"].ToString();
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
        sSql = "UPDATE tbPettyCash SET LeadgerName='" + this.DropDownList1.SelectedItem.Text + "',";
        sSql = sSql + " LDesc='" + this.txtBrName.Text + "',";
        sSql = sSql + " ExpenseAmnt='" + this.txtAmnt1.Text + "',";
        //sSql = sSql + " DepositType='" + this.ddlDType.SelectedItem.Text + "',";
        sSql = sSql + " ExpenseBy='" + this.txtDBy.Text + "',";
        sSql = sSql + " Remarks='" + this.txtRemarks1.Text + "'";
        
        sSql = sSql + " WHERE RefNo='" + lblRefNo.Text + "'";

        SqlCommand cmd = new SqlCommand(sSql, conn);
        cmd.ExecuteNonQuery();
        conn.Close();

        //LOAD TODAY DATA
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
        string sMasterID = grdrow.Cells[8].Text;

        string sSql = "";

        //DELETE FROM Master Table
        sSql = "";
        sSql = "DELETE FROM tbPettyCash";
        sSql = sSql + " WHERE RefNo='" + sBillNo + "'";

        SqlCommand cmd = new SqlCommand(sSql, conn);
        conn.Open();
        cmd.ExecuteNonQuery();
        conn.Close();

        
        //LOAD DATA IN GRID
        fnLoadData();

       
    }



}