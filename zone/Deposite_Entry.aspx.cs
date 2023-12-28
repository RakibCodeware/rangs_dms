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

public partial class Deposite_Entry : System.Web.UI.Page
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

            //LOAD DEALER INFORMATION
            LoadDropDownList_Dealer();

            //LOAD TODAY DATA
            //fnLoadData();
            fnLoadData_DSM();

            txtFrom.Text = DateTime.Today.ToString("MM/dd/yyyy");

        }
        

    }

    //LOAD Dealer IN DROPDOWN LIST
    protected void LoadDropDownList_Dealer()
    {
        SqlConnection conn = DBConnectionDSM.GetConnection();

        String strQuery = "SELECT CategoryID,DAID,Name,ZoneName FROM VW_Delear_Info ";
        strQuery = strQuery + " WHERE (Discontinue = 'No') AND (CategoryID='" + Session["sZoneID"].ToString() + "')";
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
        ddlDealerName.DataSource = cmd.ExecuteReader();
        ddlDealerName.DataTextField = "Name";
        ddlDealerName.DataValueField = "DAID";
        ddlDealerName.DataBind();

        //Add blank item at index 0.
        //ddlEntity.Items.Insert(0, new ListItem("", "0"));
        ddlDealerName.Items.Insert(0, new ListItem("--Select--", "0"));
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

    protected void fnLoadData_DSM()
    {

        //string InvFDate = ddlYear.SelectedItem.Text + "" + ddlMonth.SelectedItem.Value + "" + ddlDay.SelectedItem.Text;
        //string InvSDate = ddlYear2.SelectedItem.Text + "" + ddlMonth2.SelectedItem.Value + "" + ddlDay2.SelectedItem.Text;

        //DateTime tDate;
        //tDate = Convert.ToDateTime(this.txtFrom.Text);

        SqlConnection conn = DBConnectionDSM.GetConnection();
        conn.Open();

        string sSql = "";
        //sSql = "SELECT RefNo, CONVERT(VARCHAR(10), DepositDate, 103) AS DepositDate, BankName, ";
        //sSql = sSql + " BranchName, DepositAmnt, DepositType, DepositBy, POSMachine, DepositeSlip, AccApproved, Remarks";
        //sSql = sSql + " FROM tbDeposit ";

        //sSql = sSql + " WHERE DepositDate='" + DateTime.Today.ToString("MM/dd/yyyy") + "'";
        //sSql = sSql + " AND EID='" + Session["EID"] + "'";

        //sSql = sSql + " ORDER BY DepositDate, RefNo";
        
        sSql = "SELECT dbo.DepositAmnt.CANO, dbo.DepositAmnt.CollectionNo,";
        sSql = sSql + " CONVERT(VARCHAR(10), dbo.DepositAmnt.CDate, 103) AS DepositDate,";
        sSql = sSql + " dbo.DepositAmnt.DelearID, dbo.DelearInfo.Code, dbo.DelearInfo.Name, dbo.DelearInfo.Address, ";
        sSql = sSql + " ISNULL(dbo.DepositAmnt.DepositAmnt, 0) AS cAmount, dbo.DepositAmnt.PayType,";
        sSql = sSql + " dbo.DepositAmnt.ChequeNo, dbo.DepositAmnt.BankName, dbo.DepositAmnt.BranchName, ";
        sSql = sSql + " dbo.DepositAmnt.cRemarks, dbo.Zone.CatName AS ZoneName, dbo.Zone.CategoryID, dbo.DelearInfo.DAID";
        sSql = sSql + " FROM  dbo.DepositAmnt INNER JOIN";
        sSql = sSql + " dbo.DelearInfo ON dbo.DepositAmnt.DelearID = dbo.DelearInfo.DAID INNER JOIN";
        sSql = sSql + " dbo.Zone ON dbo.DelearInfo.CategoryID = dbo.Zone.CategoryID";

        sSql = sSql + " WHERE dbo.DepositAmnt.CDate='" + DateTime.Today.ToString("MM/dd/yyyy") + "'";
        sSql = sSql + " AND dbo.Zone.CategoryID='" + Session["sZoneID"].ToString() + "'";

        sSql = sSql + " ORDER BY dbo.DepositAmnt.CDate, dbo.DepositAmnt.CollectionNo";

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
        //SqlConnection connDSM = DBConnectionDSM.GetConnection();

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
        sSql = sSql + " BranchName,cRemarks,MoneyReceiptNo,";
        sSql = sSql + " UserID,PCName,EntryDate) VALUES(";
        sSql = sSql + " '" + txtRefNo.Text + "','" + tDate + "','" + Session["sZoneID"] + "','" + this.txtRefNo.Text + "',";
        sSql = sSql + " '" + lblDealerID.Text + "','" + txtAmnt.Text + "','" + txtAmnt.Text + "',";
        sSql = sSql + " '" + ddlDepositeType.SelectedItem.Text + "','" + txtRemarks.Text + "','" + ddlBankName.SelectedItem.Text + "',";
        sSql = sSql + " '" + txtBranch.Text + "','" + txtRemarks.Text + "','" + txtMRNo.Text + "',";
        sSql = sSql + " '" + Session["UserID"] + "','Online','" + DateTime.Today + "')";
        SqlCommand cmd2 = new SqlCommand(sSql, connDSM);
        connDSM.Open();
        cmd2.ExecuteNonQuery();
        connDSM.Close();
        //--------------------------------------------------------------------------

        //***************************************************************************************
        try
        {
            SqlConnection connSMS = DBConnectionSMS.GetConnection();
            // FOR SMS        
            if (lblDealerMobile.Text != "")
            {
                string smsText = "";
                double dOutstanding = Convert.ToDouble(txtOutstanding.Text) - Convert.ToDouble(txtAmnt.Text);

                smsText = "Dear Sir,\n";
                //smsText = smsText + "Your Code: " + lblDealerCode.Text + ".\n";
                smsText = smsText + "Thanks for your Deposit.\n";
                //smsText = smsText + "Invoice# " + this.txtCHNo.Text + ".\n";
                smsText = smsText + "Date: " + this.txtFrom.Text + ".\n";
                smsText = smsText + "Deposit Amnt: " + this.txtAmnt.Text + ".\n";
                smsText = smsText + "Total Outstanding: " + dOutstanding + ".\n";
                //smsText = smsText + "Thank You.\n";
                smsText = smsText + "Sony-Rangs";

                sSql = "";
                sSql = "INSERT INTO tbSMS(ContactNo,SMSText,UserID,EntryDate,SMSSource)" +
                        " Values ('" + this.lblDealerMobile.Text + "','" + smsText + "'," +
                        " '" + Session["UserID"] + "','" + DateTime.Now + "'," +
                        " 'DSM'" +
                        " )";
                SqlCommand cmdSMS = new SqlCommand(sSql, connSMS);
                connSMS.Open();
                cmdSMS.ExecuteNonQuery();
                connSMS.Close();
            }
        }
        catch
        {
            //
        }
        //***************************************************************************************

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
        fnLoadData_DSM();

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
            Response.Redirect("../Account/Login.aspx");
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

        //SqlConnection conn = DBConnection.GetConnection();
        SqlConnection conn = DBConnectionDSM.GetConnection();

        string sSql = "";
        //sSql = "SELECT RefAID, RefNo, DepositDate, CollectionNo, BankName, BankID, BranchName, DepositAmnt,";
        //sSql = sSql + " DepositType, DepositBy, Remarks, POSMachine, DepositeSlip, AccApproved, EntryDate, UserID, EID";
        //sSql = sSql + " FROM dbo.tbDeposit";
        //sSql = sSql + " WHERE RefNo='" + sPID + "'";
        
        sSql = "SELECT dbo.DepositAmnt.CANO, dbo.DepositAmnt.CollectionNo,";
        sSql = sSql + " CONVERT(VARCHAR(10), dbo.DepositAmnt.CDate, 103) AS DepositDate,";
        sSql = sSql + " dbo.DepositAmnt.DelearID, dbo.DelearInfo.Code, dbo.DelearInfo.Name, dbo.DelearInfo.Address, ";
        sSql = sSql + " ISNULL(dbo.DepositAmnt.DepositAmnt, 0) AS cAmount, dbo.DepositAmnt.PayType,";
        sSql = sSql + " dbo.DepositAmnt.ChequeNo, dbo.DepositAmnt.BankName, dbo.DepositAmnt.BranchName, ";
        sSql = sSql + "  dbo.DepositAmnt.RefNo, dbo.DepositAmnt.BankID,";
        sSql = sSql + " dbo.DepositAmnt.cRemarks, dbo.Zone.CatName AS ZoneName, dbo.Zone.CategoryID, dbo.DelearInfo.DAID";
        sSql = sSql + " FROM  dbo.DepositAmnt INNER JOIN";
        sSql = sSql + " dbo.DelearInfo ON dbo.DepositAmnt.DelearID = dbo.DelearInfo.DAID INNER JOIN";
        sSql = sSql + " dbo.Zone ON dbo.DelearInfo.CategoryID = dbo.Zone.CategoryID";

        sSql = sSql + " WHERE dbo.DepositAmnt.CollectionNo ='" + sPID + "'";
        //sSql = sSql + " AND dbo.Zone.CategoryID='" + Session["sZoneID"].ToString() + "'";

        //sSql = sSql + " ORDER BY dbo.DepositAmnt.CDate, dbo.DepositAmnt.CollectionNo";
        
        SqlCommand cmd = new SqlCommand(sSql, conn);
        conn.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            this.lblID.Text = dr["CANO"].ToString();
            this.lblRefNo.Text = dr["CollectionNo"].ToString();
            this.lblDDate.Text = dr["DepositDate"].ToString();
            this.txtMRNo1.Text = dr["CollectionNo"].ToString();

            this.DropDownList1.SelectedItem.Text = dr["BankName"].ToString();
            this.DropDownList1.SelectedItem.Value = dr["BankID"].ToString();

            //this.DropDownList2.SelectedItem.Text = dr["POSMachine"].ToString();
            //this.DropDownList2.SelectedItem.Value = dr["BankID"].ToString();

            this.txtBrName.Text = dr["BranchName"].ToString();

            this.txtAmnt1.Text = dr["cAmount"].ToString();
            this.ddlDType.SelectedItem.Text = dr["PayType"].ToString();

            //this.txtDBy.Text = dr["DepositBy"].ToString();
            this.txtRemarks1.Text = dr["cRemarks"].ToString();
            
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
            Response.Redirect("../Account/Login.aspx");
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
        //sSql = sSql + " SET CollectionNo='" + txtMRNo1.Text + "',";
        sSql = sSql + " SET DepositAmnt='" + this.txtAmnt1.Text + "',";
        sSql = sSql + " PayType='" + this.ddlDType.SelectedItem.Text + "',BankName='" + this.DropDownList1.SelectedItem.Text + "',";
        sSql = sSql + " BranchName='" + this.txtBrName.Text + "',cRemarks='" + this.txtRemarks1.Text + "',";
        sSql = sSql + " UserID='" + Session["UserID"] + "','Online',EntryDate='" + DateTime.Today + "'";
        sSql = sSql + " WHERE CollectionNo='" + lblRefNo.Text + "'";
        SqlCommand cmd2 = new SqlCommand(sSql, connDSM);
        connDSM.Open();
        cmd2.ExecuteNonQuery();
        connDSM.Close();
        //--------------------------------------------------------------------------


        //LOAD TODAY DATA
        fnLoadData_DSM();


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

        //DELETE FROM DMS Master Table
        sSql = "";
        sSql = "DELETE FROM tbDeposit";
        sSql = sSql + " WHERE RefNo='" + sBillNo + "'";

        SqlCommand cmd = new SqlCommand(sSql, conn);
        conn.Open();
        cmd.ExecuteNonQuery();
        conn.Close();



        //DELETE FROM DEALER Master Table
        SqlConnection connDSM = DBConnectionDSM.GetConnection();
        sSql = "";
        sSql = "DELETE FROM DepositAmnt";
        sSql = sSql + " WHERE CollectionNo='" + sBillNo + "'";

        cmd = new SqlCommand(sSql, connDSM);
        connDSM.Open();
        cmd.ExecuteNonQuery();
        connDSM.Close();


        //LOAD DATA IN GRID
        fnLoadData_DSM();

       
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


    protected void ddlDealerName_SelectedIndexChanged(object sender, EventArgs e)
    {
        SqlConnection conn = DBConnectionDSM.GetConnection();
        double UP = 0;
        double CampPrice = 0;
        string sSql = "";

        sSql = "";
        sSql = "SELECT CategoryID,DAID,Code,Name,ZoneName,Address, ContactNo, EmailAdd FROM VW_Delear_Info ";
        sSql = sSql + " WHERE (Name = '" + ddlDealerName.SelectedItem.Text + "') ";
        sSql = sSql + " AND (CategoryID='" + Session["sZoneID"].ToString() + "')";

        SqlCommand cmd = new SqlCommand(sSql, conn);
        conn.Open();
        SqlDataReader dr = cmd.ExecuteReader();

        //try
        //{
        if (dr.Read())
        {            
            this.lblDealerID.Text = dr["DAID"].ToString();
            this.lblDealerCode.Text = dr["Code"].ToString();
            txtCode.Text = dr["Code"].ToString();
            this.lblDealerName.Text = dr["Name"].ToString();
            this.lblDealerAdd.Text = dr["Address"].ToString();
            this.lblDealerMobile.Text = dr["ContactNo"].ToString();
            this.lblDealerEmail.Text = dr["EmailAdd"].ToString();
                        
            //this.txtEmail.Text = dr["EmailAdd"].ToString();
            //this.txtProdDesc.Text = dr["ProdName"].ToString();
        }
        else
        {
            this.txtCode.Text = "";
            lblDealerID.Text = "0";
            //this.txtCustContact.Text = "";
            //this.txtEmail.Text = "";
            //this.txtCreditLimit.Text = "0";
            //this.txtOutstanding.Text = "0";

        }
        //}
        //catch (InvalidCastException err)
        //{
        //    throw (err);
        //}
        //finally
        //{
        dr.Dispose();
        dr.Close();
        conn.Close();
        //}


        //LOAD CREDIT LIMIT
        fnLoadCreditLimit();

        //LOAD STATEMENT/OUTSTANDING
        fnLoadOutStanding();
        double dOutStanding = Convert.ToDouble(lblOBSales.Text) - Convert.ToDouble(lblOBCollection.Text) + Convert.ToDouble(lblOBDis.Text) - Convert.ToDouble(lblOBWith.Text);
        txtOutstanding.Text = Convert.ToString(dOutStanding);

    }

    //FUNCTION FOR LOAD CREDIT LIMIT
    private void fnLoadCreditLimit()
    {
        SqlConnection conn = DBConnectionDSM.GetConnection();

        txtCreditLimit.Text = "0";

        string gSQL = "";
        gSQL = "";
        gSQL = "SELECT TOP (1) dbo.tbCreditLimitYearly.TID, dbo.DelearInfo.DAID, dbo.DelearInfo.Code,";
        gSQL = gSQL + " dbo.DelearInfo.Name, dbo.DelearInfo.ContactNo,";
        gSQL = gSQL + " dbo.DelearInfo.EmailAdd, dbo.tbCreditLimitYearly.TAmount";
        gSQL = gSQL + " FROM dbo.tbCreditLimitYearly INNER JOIN";
        gSQL = gSQL + " dbo.DelearInfo ON dbo.tbCreditLimitYearly.DealerID = dbo.DelearInfo.DAID";
        gSQL = gSQL + " WHERE dbo.DelearInfo.Name = '" + ddlDealerName.SelectedItem.Text + "'";
        gSQL = gSQL + " AND dbo.DelearInfo.Code = '" + lblDealerCode.Text + "'";
        gSQL = gSQL + " ORDER BY dbo.tbCreditLimitYearly.TID DESC";

        SqlCommand cmd = new SqlCommand(gSQL, conn);
        conn.Open();
        SqlDataReader dr = cmd.ExecuteReader();

        if (dr.Read())
        {
            this.txtCreditLimit.Text = dr["TAmount"].ToString();
        }
        else
        {
            this.txtCreditLimit.Text = "0";
        }
        dr.Dispose();
        dr.Close();
        conn.Close();


    }

    //FUNCTION FOR LOAD STATEMENT/OUTSTANDING
    private void fnLoadOutStanding()
    {
        SqlConnection conn = DBConnectionDSM.GetConnection();

        txtOutstanding.Text = "0";

        string gSQL = "";
        //gSQL = "";
        //gSQL = "SELECT TOP (1) dbo.tbCreditLimitYearly.TID, dbo.DelearInfo.DAID, dbo.DelearInfo.Code,";
        //gSQL = gSQL + " dbo.DelearInfo.Name, dbo.DelearInfo.ContactNo,";
        //gSQL = gSQL + " dbo.DelearInfo.EmailAdd, dbo.tbCreditLimitYearly.TAmount";
        //gSQL = gSQL + " FROM dbo.tbCreditLimitYearly INNER JOIN";
        //gSQL = gSQL + " dbo.DelearInfo ON dbo.tbCreditLimitYearly.DealerID = dbo.DelearInfo.DAID";
        //gSQL = gSQL + " WHERE dbo.DelearInfo.Name = '" + ddlDealerName.SelectedItem.Text + "'";
        //gSQL = gSQL + " AND dbo.DelearInfo.Code = '" + txtDealerCode.Text + "'";
        //gSQL = gSQL + " ORDER BY dbo.tbCreditLimitYearly.TID DESC";

        //SqlCommand cmd = new SqlCommand(gSQL, conn);
        //conn.Open();
        //SqlDataReader dr = cmd.ExecuteReader();

        //if (dr.Read())
        //{
        //    this.txtCreditLimit.Text = dr["TAmount"].ToString();
        //}
        //else
        //{
        //    this.txtCreditLimit.Text = "0";
        //}

        //    '========================================================================
        //    'OPENING
        //    '========================================================================
        //    '--------------------------------------------------------------------------------------------

        //'    gSQL = ""
        //'    gSQL = "DELETE FROM TempOpening"
        //'    'gSQL = gSQL & " WHERE UserID='" & sUser & "' AND PCName='" & ComputerName & "'"
        //'    Cnn.Execute gSQL


        //OPENING
        //LOAD SALES DATA
        gSQL = "";
        gSQL = "SELECT dbo.DelearInfo.Code, ";
        gSQL = gSQL + " dbo.DelearInfo.Name, dbo.DelearInfo.Address, ";
        gSQL = gSQL + " dbo.[Zone].CatName AS ZoneName, SUM(ISNULL(dbo.MRSRMaster.NetSalesAmnt,0)) AS NetSalesAmnt";
        gSQL = gSQL + " FROM dbo.DelearInfo INNER JOIN";
        gSQL = gSQL + " dbo.MRSRMaster ON dbo.DelearInfo.DAID = dbo.MRSRMaster.InSource INNER JOIN";
        gSQL = gSQL + " dbo.[Zone] ON dbo.DelearInfo.CategoryID = dbo.[Zone].CategoryID";

        gSQL = gSQL + " WHERE (dbo.MRSRMaster.TrType = 3)";

        gSQL = gSQL + " AND dbo.DelearInfo.Name='" + ddlDealerName.SelectedItem.Text + "'";
        gSQL = gSQL + " AND dbo.DelearInfo.Code = '" + lblDealerCode.Text + "'";
        gSQL = gSQL + " AND dbo.MRSRMaster.TDate<='" + txtFrom.Text + "'";
        //'gSQL = gSQL & " AND dbo.MRSRMaster.TDate<='" & dtpEDate & "'"

        gSQL = gSQL + " GROUP BY  ";
        gSQL = gSQL + " dbo.DelearInfo.Code,";
        gSQL = gSQL + " dbo.DelearInfo.Name , dbo.DelearInfo.Address, dbo.[Zone].CatName ";
        SqlCommand cmd = new SqlCommand(gSQL, conn);
        conn.Open();
        SqlDataReader dr = cmd.ExecuteReader();

        if (dr.Read())
        {
            this.lblOBSales.Text = dr["NetSalesAmnt"].ToString();
        }
        else
        {
            this.lblOBSales.Text = "0";
        }
        dr.Dispose();
        dr.Close();
        conn.Close();
        //'-------------------------------------------------------------------------------------------


        //'-------------------------------------------------------------------------------------------
        //'LOAD OPENING COLLECTION
        gSQL = "";
        gSQL = "SELECT dbo.DepositAmnt.DelearID, dbo.DelearInfo.Code, dbo.DelearInfo.Name, ";
        gSQL = gSQL + " dbo.DelearInfo.Address, SUM(ISNULL(dbo.DepositAmnt.DepositAmnt, 0)) AS cAmount,";
        gSQL = gSQL + " dbo.Zone.CatName AS ZoneName";
        gSQL = gSQL + " FROM dbo.DepositAmnt INNER JOIN";
        gSQL = gSQL + " dbo.DelearInfo ON dbo.DepositAmnt.DelearID = dbo.DelearInfo.DAID INNER JOIN";
        gSQL = gSQL + " dbo.Zone ON dbo.DelearInfo.CategoryID = dbo.Zone.CategoryID";


        gSQL = gSQL + " WHERE dbo.DelearInfo.Name='" + ddlDealerName.SelectedItem.Text + "'";
        gSQL = gSQL + " AND dbo.DelearInfo.Code = '" + lblDealerCode.Text + "'";
        gSQL = gSQL + " AND dbo.DepositAmnt.CDate<='" + txtFrom.Text + "'";

        //gSQL = gSQL + " WHERE dbo.DepositAmnt.CDate<'" & dtpReceive & "'";
        //    'gSQL = gSQL & " AND dbo.DepositAmnt.CDate<='" & dtpEDate & "'";
        //''    If cboZone.text <> "ALL" Then
        //''        gSQL = gSQL & " AND dbo.Zone.CatName='" & cboZone.text & "'"
        //''    End If
        //gSQL = gSQL + " AND dbo.DelearInfo.Name='" & cboIn.text & "'";

        gSQL = gSQL + " GROUP BY dbo.DepositAmnt.DelearID, ";
        gSQL = gSQL + " dbo.DelearInfo.Code, dbo.DelearInfo.Name, dbo.DelearInfo.Address,";
        gSQL = gSQL + " dbo.Zone.CatName";

        cmd = new SqlCommand(gSQL, conn);
        conn.Open();
        dr = cmd.ExecuteReader();

        if (dr.Read())
        {
            this.lblOBCollection.Text = dr["cAmount"].ToString();
        }
        else
        {
            this.lblOBCollection.Text = "0";
        }
        dr.Dispose();
        dr.Close();
        conn.Close();

        //'-------------------------------------------------------------------------------------------
        //'-------------------------------------------------------------------------------------------
        //'LOAD OPENING DISHONOUR
        gSQL = "";
        gSQL = "SELECT ZoneName,Name, SUM(cAmount) AS cAmount";
        gSQL = gSQL + " From dbo.VW_DishonourAmnt";
        //gSQL = gSQL + " WHERE CDate<'" & dtpReceive & "'";
        ////'    If cboZone.text <> "ALL" Then
        ////'        gSQL = gSQL & " AND ZoneName='" & cboZone.text & "'"
        ////'    End If
        //gSQL = gSQL + " AND Name='" & cboIn.text & "'";

        gSQL = gSQL + " WHERE Name='" + ddlDealerName.SelectedItem.Text + "'";
        gSQL = gSQL + " AND Code = '" + lblDealerCode.Text + "'";
        gSQL = gSQL + " AND CDate<='" + txtFrom.Text + "'";

        gSQL = gSQL + " GROUP BY ZoneName,Name";
        cmd = new SqlCommand(gSQL, conn);
        conn.Open();
        dr = cmd.ExecuteReader();

        if (dr.Read())
        {
            this.lblOBDis.Text = dr["cAmount"].ToString();
        }
        else
        {
            this.lblOBDis.Text = "0";
        }
        dr.Dispose();
        dr.Close();
        conn.Close();
        //'-------------------------------------------------------------------------------------------


        //'-------------------------------------------------------------------------------------------
        //'LOAD OPENING WITHDRAWN
        gSQL = "";
        gSQL = "SELECT dbo.DelearInfo.Code, ";
        gSQL = gSQL + " dbo.DelearInfo.Name, dbo.DelearInfo.Address, ";
        gSQL = gSQL + " dbo.[Zone].CatName AS ZoneName, SUM(ISNULL(dbo.MRSRMaster.NetSalesAmnt,0)) AS NetSalesAmnt";
        gSQL = gSQL + " FROM dbo.DelearInfo INNER JOIN";
        gSQL = gSQL + " dbo.MRSRMaster ON dbo.DelearInfo.DAID = dbo.MRSRMaster.OutSource INNER JOIN";
        gSQL = gSQL + " dbo.[Zone] ON dbo.DelearInfo.CategoryID = dbo.[Zone].CategoryID";

        gSQL = gSQL + " WHERE (dbo.MRSRMaster.TrType = -3)";

        //'If cboZone.text <> "ALL" Then
        //'        gSQL = gSQL & " AND dbo.[Zone].CatName='" & cboZone.text & "'"
        //'    End If
        //gSQL = gSQL + " AND dbo.DelearInfo.Name='" & cboIn.text & "'";
        //gSQL = gSQL + " AND dbo.MRSRMaster.TDate<'" & dtpReceive & "'";

        gSQL = gSQL + " AND dbo.DelearInfo.Name='" + ddlDealerName.SelectedItem.Text + "'";
        gSQL = gSQL + " AND dbo.DelearInfo.Code = '" + lblDealerCode.Text + "'";
        gSQL = gSQL + " AND dbo.MRSRMaster.TDate<='" + txtFrom.Text + "'";

        gSQL = gSQL + " GROUP BY ";
        gSQL = gSQL + " dbo.DelearInfo.Code,";
        gSQL = gSQL + " dbo.DelearInfo.Name , dbo.DelearInfo.Address, dbo.[Zone].CatName ";

        cmd = new SqlCommand(gSQL, conn);
        conn.Open();
        dr = cmd.ExecuteReader();

        if (dr.Read())
        {
            this.lblOBWith.Text = dr["NetSalesAmnt"].ToString();
        }
        else
        {
            this.lblOBWith.Text = "0";
        }
        dr.Dispose();
        dr.Close();
        conn.Close();
        //'-------------------------------------------------------------------------------------------

    } 


}