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

public partial class Dishonour_Entry : System.Web.UI.Page
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
            fnLoadCombo_Item(ddlBankName, "BName", "ID", "tbBankList");
            fnLoadCombo_Item(ddlBankName1, "BName", "ID", "tbBankList");
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
        sSql = "SELECT RefNo, CONVERT(VARCHAR(10), DepositDate, 103) AS DepositDate, BankName, ";
        sSql = sSql + " BranchName, DepositAmnt, DepositType, DepositBy, POSMachine, DepositeSlip, AccApproved, Remarks";
        sSql = sSql + " FROM tbDeposit ";

        sSql = sSql + " WHERE DepositDate='" + DateTime.Today.ToString("MM/dd/yyyy") + "'";
        sSql = sSql + " AND EID='" + Session["EID"] + "'";

        sSql = sSql + " ORDER BY DepositDate, RefNo";
        

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
    
    
    protected void btnSaveClick(object sender, EventArgs e)
    {
        SqlConnection conn = DBConnection.GetConnection();
        //fnLoadMRSRNo();

        string sSql = "";
        DateTime tDate;

        if (txtRefNo.Text == "")
        {
            PopupMessage("Please enter Deposit Slip/Ref. number.", btnSave);
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
            PopupMessage("Please enter Deposite Amount.", btnSave);
            txtAmnt.Focus();
            return;
        }

        if (txtAmnt.Text == "")
        {
            PopupMessage("Please enter Deposite Amount.", btnSave);
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
        sSql = "SELECT RefAID FROM tbDeposit" +
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


        //SAVE DATA IN MASTER TABLE IN DMS
        sSql = "";
        sSql = "INSERT INTO tbDeposit(RefNo,DepositDate,EID,CollectionNo," +
               "BankID,BankName,BranchName,DepositAmnt,DepositType,DepositBy,Remarks,POSMachine,UserID,EntryDate)" +
                     " Values ('" + this.txtRefNo.Text + "','" + tDate + "','" + Session["EID"] + "', '" + this.txtMRNo.Text + "'," +
                     " '" + ddlBankName.SelectedItem.Value + "','" + ddlBankName.SelectedItem.Text  + "'," +
                     " '" + txtBranch.Text + "','" + txtAmnt.Text + "','" + ddlDepositeType.SelectedItem.Text + "'," +
                     " '" + txtDepositBy.Text + "', '" + txtRemarks.Text + "','" + ddlBankName1.SelectedItem.Text + "'," +
                     " '" + Session["UserID"] + "', '" + DateTime.Today + "'" +
                     " )";
        SqlCommand cmd = new SqlCommand(sSql, conn);
        conn.Open();
        cmd.ExecuteNonQuery();
        conn.Close();

        //--------------------------------------------------------------------------
        SqlConnection connDSM = DBConnectionDSM.GetConnection();

        //SAVE DATA IN MASTER TABLE IN DEALER
        sSql = "";    
        sSql = "INSERT INTO DepositAmnt(";
        sSql = sSql + " CollectionNo,CDate,CollectionTo,RefNo,";
        sSql = sSql + " DelearID,DepositAmnt,ChequeAmount,";
        sSql = sSql + " PayType,ChequeNo,BankName,";
        sSql = sSql + " BranchName,cRemarks,";
        sSql = sSql + " UserID,PCName,EntryDate) VALUES(";
        sSql = sSql + " '" + txtMRNo.Text + "','" + tDate + "','" + Session["EID"] + "','" + this.txtRefNo.Text + "',";
        sSql = sSql + " '" + lblDealerID.Text + "','" + txtAmnt.Text + "','" + txtAmnt.Text + "',";
        sSql = sSql + " '" + ddlDepositeType.SelectedItem.Text + "','" + txtRemarks.Text + "','" + ddlBankName.SelectedItem.Text + "',";
        sSql = sSql + " '" + txtBranch.Text + "','" + txtRemarks.Text + "',";
        sSql = sSql + " '" + Session["UserID"] + "','Online','" + DateTime.Today + "')";
        SqlCommand cmd2 = new SqlCommand(sSql, connDSM);
        connDSM.Open();
        cmd2.ExecuteNonQuery();
        connDSM.Close();
        //--------------------------------------------------------------------------
        
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
            " FROM dbo.tbDeposit" +
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

        fnLoadCombo_Item(DropDownList1, "BName", "ID", "tbBankList");
        fnLoadCombo_Item(DropDownList2, "BName", "ID", "tbBankList");

        SqlConnection conn = DBConnection.GetConnection();


        string sSql = "";
        sSql = "SELECT RefAID, RefNo, DepositDate, CollectionNo, BankName, BankID, BranchName, DepositAmnt,";
        sSql = sSql + " DepositType, DepositBy, Remarks, POSMachine, DepositeSlip, AccApproved, EntryDate, UserID, EID";
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
            this.txtMRNo1.Text = dr["CollectionNo"].ToString();

            this.DropDownList1.SelectedItem.Text = dr["BankName"].ToString();
            this.DropDownList1.SelectedItem.Value = dr["BankID"].ToString();

            this.DropDownList2.SelectedItem.Text = dr["POSMachine"].ToString();
            //this.DropDownList2.SelectedItem.Value = dr["BankID"].ToString();

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

        sSql = sSql + " POSMachine='" + this.DropDownList2.SelectedItem.Text + "',";

        sSql = sSql + " Remarks='" + this.txtRemarks1.Text + "'";
        
        sSql = sSql + " WHERE RefNo='" + lblRefNo.Text + "'";

        SqlCommand cmd = new SqlCommand(sSql, conn);
        cmd.ExecuteNonQuery();
        conn.Close();


        //--------------------------------------------------------------------------
        SqlConnection connDSM = DBConnectionDSM.GetConnection();

        //SAVE DATA IN MASTER TABLE IN DEALER
        sSql = "";
        sSql = "UPDATE DepositAmnt";
        sSql = sSql + " SET CollectionNo='" + txtMRNo1.Text + "',";
        sSql = sSql + " DepositAmnt='" + this.txtAmnt1.Text + "',ChequeAmount,";
        sSql = sSql + " PayType='" + this.ddlDType.SelectedItem.Text + "',ChequeNo,BankName='" + this.DropDownList1.SelectedItem.Text + "',";
        sSql = sSql + " BranchName='" + this.txtBrName.Text + "',cRemarks='" + this.txtRemarks1.Text + "',";
        sSql = sSql + " UserID='" + Session["UserID"] + "','Online',EntryDate='" + DateTime.Today + "'";
        sSql = sSql + " WHERE RefNo='" + lblRefNo.Text + "'";
        SqlCommand cmd2 = new SqlCommand(sSql, connDSM);
        connDSM.Open();
        cmd2.ExecuteNonQuery();
        connDSM.Close();
        //--------------------------------------------------------------------------


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
        sSql = "DELETE FROM tbDeposit";
        sSql = sSql + " WHERE RefNo='" + sBillNo + "'";

        SqlCommand cmd = new SqlCommand(sSql, conn);
        conn.Open();
        cmd.ExecuteNonQuery();
        conn.Close();

        
        //LOAD DATA IN GRID
        fnLoadData();

       
    }



    protected void ddlDepositeType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlDepositeType.SelectedItem.Text == "CARD")
        {
            Panel1.Visible = true;
        }
        else
        {
            Panel1.Visible = false;
        }

    }
}