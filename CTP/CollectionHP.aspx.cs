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

public partial class CollectionHP : System.Web.UI.Page
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

    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GetSalesInv(string prefixText)
    {
        DataTable dt = new DataTable();

        SqlConnection con = DBConnection.GetConnection();

        con.Open();
        SqlCommand cmd = new SqlCommand("Select TOP 10 * from MRSRMaster where SalesTypeHP=1 AND HPStatus=0 AND MRSRCode like @mrsrcode+'%'", con);
        cmd.Parameters.AddWithValue("@mrsrcode", prefixText);
        SqlDataAdapter adp = new SqlDataAdapter(cmd);
        adp.Fill(dt);
        List<string> ModelNames = new List<string>();
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            ModelNames.Add(dt.Rows[i][5].ToString());
        }
        return ModelNames;
    }

    protected void fnLoadData()
    {
        
        //string InvFDate = ddlYear.SelectedItem.Text + "" + ddlMonth.SelectedItem.Value + "" + ddlDay.SelectedItem.Text;
        //string InvSDate = ddlYear2.SelectedItem.Text + "" + ddlMonth2.SelectedItem.Value + "" + ddlDay2.SelectedItem.Text;

        DateTime tDate;
        //tDate = Convert.ToDateTime(this.txtFrom.Text);

        SqlConnection conn = DBConnection.GetConnection();
        conn.Open();

        string sSql = "";
        sSql = "SELECT MRNO, CONVERT(VARCHAR(10), MRDate, 103) AS MRDate, BankName, ";
        sSql = sSql + " BranchName, cAmnt,cType,ChequeNo, CollectBy, Remarks";
        sSql = sSql + " FROM tbCollection ";

        sSql = sSql + " WHERE MRDate='" + DateTime.Today.ToString("MM/dd/yyyy") + "'";

        sSql = sSql + " ORDER BY MRDate, MRNO";
        

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
            xCombo.Items.Insert(0, new ListItem("", ""));

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
            PopupMessage("Please enter Collect Amount.", btnSave);
            txtAmnt.Focus();
            return;
        }

        if (txtAmnt.Text == "")
        {
            PopupMessage("Please enter Collect Amount.", btnSave);
            txtAmnt.Focus();
            return;
        }

        if (txtInv.Text == "")
        {
            PopupMessage("Please Select Sales Invoice #", btnSave);
            txtInv.Focus();
            return;
        }

        if (txtMobile.Text == "")
        {
            PopupMessage("Please Select Customer Info ...", btnSave);
            txtMobile.Focus();
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
        sSql = "SELECT CAID FROM tbCollection" +
            " WHERE MRNO='" + this.txtRefNo.Text + "'";
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
                            "<script>alert('" + "This MR # already exists." + "');</script>", false);
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
        sSql = "INSERT INTO tbCollection(MRNO,MRDate,AppID,MRSRMID,CustAID,cAmnt," +
               "cType,ChequeNo,BankName,BranchName,CollectBy,Remarks,UserID,EntryDate)" +
                     " Values ('" + this.txtRefNo.Text + "','" + tDate + "'," +
                     " '" + this.txtAppID.Text + "','" + this.txtMRSRID.Text + "'," +
                     " '" + this.txtCustID.Text + "','" + this.txtAmnt.Text + "'," +
                     " '" + ddlType.SelectedItem.Text + "','" + txtChequeNo.Text + "'," +
                     " '" + ddlBankName.SelectedItem.Value + "','" + txtBranch.Text + "'," +
                     " '" + txtCollectBy.Text + "', '" + txtRemarks.Text + "'," +
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
        txtFrom.Text = DateTime.Today.ToString("dd/MM/yyyy");

        txtInv.Text = "";
        txtCustName.Text = "";
        txtTDue.Text = "";
        txtMobile.Text = "";
        txtRDue.Text = "";
        txtAppID.Text = "";
        txtMRSRID.Text = "";
        txtCustID.Text = "";

        txtAmnt.Text = "0";
        txtChequeNo.Text = "";
        ddlBankName.SelectedItem.Text = "";
        txtBranch.Text = "";

        txtCollectBy.Text = "";
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
        sSql = "SELECT ISNULL(MAX(RIGHT(MRNO, 5)), 0) AS RefNo" +
            " FROM dbo.tbCollection" +
            " WHERE (LEFT(MRNO, 11) = '" + "" + Session["sBrCode"] + "" + DateTime.Now.ToString("yyyy") + "-" + "')";
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

    }

    protected void lnkDel_Click(object sender, EventArgs e)
    {
        LinkButton btnsubmit = sender as LinkButton;
        GridViewRow gRow = (GridViewRow)btnsubmit.NamingContainer;
        //txtPID.Text = gvCustomres.DataKeys[gRow.RowIndex].Value.ToString();
        string sJobNo = Convert.ToString(GridView1.DataKeys[gRow.RowIndex].Value);

       
    }
    

    protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlType.SelectedItem.Text == "CASH")
        {
            lblNo.Visible = false;
            txtChequeNo.Visible = false;
            lblBank.Visible = false;
            ddlBankName.Visible = false;
            lblBranch.Visible = false;
            txtBranch.Visible = false;
        }

        else if (ddlType.SelectedItem.Text == "CHEQUE")
        {
            lblNo.Visible = true;
            txtChequeNo.Visible = true;
            lblBank.Visible = true;
            ddlBankName.Visible = true;
            lblBranch.Visible = true;
            txtBranch.Visible = true;
            lblNo.Text = "Cheque #";
        }

        else if (ddlType.SelectedItem.Text == "VISA")
        {
            lblNo.Visible = true;
            txtChequeNo.Visible = true;
            lblBank.Visible = true;
            ddlBankName.Visible = true;
            lblBranch.Visible = true;
            txtBranch.Visible = true;
            lblNo.Text = "Card #";
        }

        else if (ddlType.SelectedItem.Text == "MASTER")
        {
            lblNo.Visible = true;
            txtChequeNo.Visible = true;
            lblBank.Visible = true;
            ddlBankName.Visible = true;
            lblBranch.Visible = true;
            txtBranch.Visible = true;
            lblNo.Text = "Card #";
        }

        else if (ddlType.SelectedItem.Text == "AMEX")
        {
            lblNo.Visible = true;
            txtChequeNo.Visible = true;
            lblBank.Visible = true;
            ddlBankName.Visible = true;
            lblBranch.Visible = true;
            txtBranch.Visible = true;
            lblNo.Text = "Card #";
        }

        else if (ddlType.SelectedItem.Text == "bKash")
        {
            lblNo.Visible = true;
            txtChequeNo.Visible = true;
            lblBank.Visible = false;
            ddlBankName.Visible = false;
            lblBranch.Visible = false;
            txtBranch.Visible = false;
            lblNo.Text = "Ref #";
        }

        else if (ddlType.SelectedItem.Text == "Roket")
        {
            lblNo.Visible = true;
            txtChequeNo.Visible = true;
            lblBank.Visible = false;
            ddlBankName.Visible = false;
            lblBranch.Visible = false;
            txtBranch.Visible = false;
            lblNo.Text = "Ref #";
        }

        else if (ddlType.SelectedItem.Text == "Others")
        {
            lblNo.Visible = true;
            txtChequeNo.Visible = true;
            lblBank.Visible = true;
            ddlBankName.Visible = true;
            lblBranch.Visible = true;
            txtBranch.Visible = true;
            lblNo.Text = "Others #";
        }


    }


}