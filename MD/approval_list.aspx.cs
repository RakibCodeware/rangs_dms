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

public partial class MD_approval_list : System.Web.UI.Page
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
            //this.txtFrom.Text = DateTime.Today.ToString("MM/dd/yyyy");
            //this.txtToDate.Text = DateTime.Today.ToString("MM/dd/yyyy");

            //LOAD DATA IN GRID
            fnLoadData();

        }
    }

    protected void lnkView_Click(object sender, EventArgs e)
    {
        LinkButton btnsubmit = sender as LinkButton;
        GridViewRow gRow = (GridViewRow)btnsubmit.NamingContainer;
        //txtPID.Text = gvCustomres.DataKeys[gRow.RowIndex].Value.ToString();

        //Session["AssAID"] = GridView1.DataKeys[gRow.RowIndex].Value.ToString();
        Session["sInvAID"] = GridView1.DataKeys[gRow.RowIndex].Value.ToString();
        //lblDelNo.Text = gRow.Cells[0].Text;

        //string rowNumber = gRow.Cells[0].Text;
        string sInvNo = gRow.Cells[0].Text;

        fnLoadData_View();

        this.ModalPopupExtender1.Show();

    }

    protected void fnLoadData_View()
    {
        SqlConnection conn = DBConnection.GetConnection();
        
        string sSql = "";
        sSql = "";
        sSql = "SELECT dbo.tbRequest.RAID, dbo.tbRequest.ReqNo, CONVERT(VARCHAR(10),dbo.tbRequest.ReqDate, 110) AS ReqDate, dbo.tbRequest.MRSRCode,";
        sSql = sSql + " dbo.tbRequest.TransactionType,  CONVERT(VARCHAR(10), dbo.MRSRMaster.TDate, 110) AS TDate, dbo.Entity.eName AS eFrom, ";
        sSql = sSql + " Entity_1.eName AS eTo, dbo.tbRequest.ReqFor, dbo.tbRequest.ReqReason, dbo.tbRequest.ReqBy,";
        sSql = sSql + " dbo.tbRequest.RRemarks, dbo.tbRequest.UserID, dbo.tbRequest.EntryDate, ";
        sSql = sSql + " dbo.MRSRMaster.MRSRMID, dbo.MRSRMaster.TrType";
        sSql = sSql + " FROM  dbo.tbRequest INNER JOIN";
        sSql = sSql + " dbo.MRSRMaster ON dbo.tbRequest.MRSRCode = dbo.MRSRMaster.MRSRCode INNER JOIN";
        sSql = sSql + " dbo.Entity AS Entity_1 ON dbo.MRSRMaster.InSource = Entity_1.EID INNER JOIN";
        sSql = sSql + " dbo.Entity ON dbo.MRSRMaster.OutSource = dbo.Entity.EID";

        sSql = sSql + " WHERE dbo.tbRequest.RAID='" + Session["sInvAID"].ToString() + "'";

        SqlCommand cmd1 = new SqlCommand(sSql, conn);
        conn.Open();
        SqlDataReader dr1 = cmd1.ExecuteReader();
        if (dr1.Read())
        {
            lblInvNo.Text = dr1["MRSRCode"].ToString();
            lblInvDate.Text = dr1["TDate"].ToString();

            lblTrType.Text = dr1["TransactionType"].ToString();
            lblFrom.Text = dr1["eFrom"].ToString();
            lblTo.Text = dr1["eTo"].ToString();
            lblReqFor.Text = dr1["ReqFor"].ToString();
            lblReason.Text = dr1["ReqReason"].ToString();
            lblReqBy.Text = dr1["ReqBy"].ToString();
            lblReqDate.Text = dr1["EntryDate"].ToString();
        }
        else
        {
            lblInvNo.Text = "";
            lblInvDate.Text = "";

            lblTrType.Text = "";
            lblFrom.Text = "";
            lblTo.Text = "";
            lblReqFor.Text = "";
            lblReason.Text = "";
            lblReqBy.Text = "";
            lblReqDate.Text = "";

        }
        dr1.Dispose();
        dr1.Close();
        conn.Close();
        
    }

    protected void lnkDel_Click(object sender, EventArgs e)
    {

        LinkButton btnsubmit = sender as LinkButton;
        GridViewRow gRow = (GridViewRow)btnsubmit.NamingContainer;
        //txtPID.Text = gvCustomres.DataKeys[gRow.RowIndex].Value.ToString();

        //Session["AssAID"] = GridView1.DataKeys[gRow.RowIndex].Value.ToString();
        Session["sInvAID"] = GridView1.DataKeys[gRow.RowIndex].Value.ToString();
        //lblDelNo.Text = gRow.Cells[0].Text;

        //string rowNumber = gRow.Cells[0].Text;
        string sInvNo = gRow.Cells[0].Text;

        string sSql = "";

        ////DELETE FROM Master Table
        //sSql = "";
        //sSql = "DELETE FROM MRSRMaster";
        //sSql = sSql + " WHERE MRSRMID='" + sMasterID + "'";

        //SqlCommand cmd = new SqlCommand(sSql, conn);
        //conn.Open();
        //cmd.ExecuteNonQuery();
        //conn.Close();


        fnLoadData_Deny();

        this.ModalPopupExtender2.Show();   

        //LOAD DATA IN GRID
        fnLoadData();


    }

    protected void lnkApp_Click(object sender, EventArgs e)
    {
        LinkButton btnsubmit = sender as LinkButton;
        GridViewRow gRow = (GridViewRow)btnsubmit.NamingContainer;
        //txtPID.Text = gvCustomres.DataKeys[gRow.RowIndex].Value.ToString();

        //Session["AssAID"] = GridView1.DataKeys[gRow.RowIndex].Value.ToString();
        Session["sInvAID"] = GridView1.DataKeys[gRow.RowIndex].Value.ToString();
        //lblDelNo.Text = gRow.Cells[0].Text;

        //string rowNumber = gRow.Cells[0].Text;
        string sInvNo = gRow.Cells[0].Text;
                  

        SqlConnection con = DBConnection.GetConnection();
        string sSql = "";

        //UPDATE tbRequest Table
        sSql = "";
        sSql = "UPDATE tbRequest SET GrantDeny=1";
        sSql = sSql + " WHERE RAID='" + Session["sInvAID"] + "'";

        SqlCommand cmd = new SqlCommand(sSql, conn);
        conn.Open();
        cmd.ExecuteNonQuery();
        conn.Close();

        //LOAD DATA IN GRID
        fnLoadData();

    }


    protected void fnLoadData_Deny()
    {
        SqlConnection conn = DBConnection.GetConnection();

        string sSql = "";
        sSql = "";
        sSql = "SELECT dbo.tbRequest.RAID, dbo.tbRequest.ReqNo, CONVERT(VARCHAR(10),dbo.tbRequest.ReqDate, 110) AS ReqDate, dbo.tbRequest.MRSRCode,";
        sSql = sSql + " dbo.tbRequest.TransactionType,  CONVERT(VARCHAR(10), dbo.MRSRMaster.TDate, 110) AS TDate, dbo.Entity.eName AS eFrom, ";
        sSql = sSql + " Entity_1.eName AS eTo, dbo.tbRequest.ReqFor, dbo.tbRequest.ReqReason, dbo.tbRequest.ReqBy,";
        sSql = sSql + " dbo.tbRequest.RRemarks, dbo.tbRequest.UserID, dbo.tbRequest.EntryDate, ";
        sSql = sSql + " dbo.MRSRMaster.MRSRMID, dbo.MRSRMaster.TrType";
        sSql = sSql + " FROM  dbo.tbRequest INNER JOIN";
        sSql = sSql + " dbo.MRSRMaster ON dbo.tbRequest.MRSRCode = dbo.MRSRMaster.MRSRCode INNER JOIN";
        sSql = sSql + " dbo.Entity AS Entity_1 ON dbo.MRSRMaster.InSource = Entity_1.EID INNER JOIN";
        sSql = sSql + " dbo.Entity ON dbo.MRSRMaster.OutSource = dbo.Entity.EID";

        sSql = sSql + " WHERE dbo.tbRequest.RAID='" + Session["sInvAID"].ToString() + "'";

        SqlCommand cmd1 = new SqlCommand(sSql, conn);
        conn.Open();
        SqlDataReader dr1 = cmd1.ExecuteReader();
        if (dr1.Read())
        {
            lblAID.Text = dr1["RAID"].ToString();
            lblInvNo1.Text = dr1["MRSRCode"].ToString();
            lblInvDate1.Text = dr1["TDate"].ToString();

            lblTrType1.Text = dr1["TransactionType"].ToString();
            lblFrom1.Text = dr1["eFrom"].ToString();
            lblTo1.Text = dr1["eTo"].ToString();
            lblReqFor1.Text = dr1["ReqFor"].ToString();
            lblReason1.Text = dr1["ReqReason"].ToString();
            lblReqBy1.Text = dr1["ReqBy"].ToString();
            lblReqDate1.Text = dr1["EntryDate"].ToString();
        }
        else
        {
            lblInvNo1.Text = "";
            lblInvDate1.Text = "";

            lblTrType1.Text = "";
            lblFrom1.Text = "";
            lblTo1.Text = "";
            lblReqFor1.Text = "";
            lblReason1.Text = "";
            lblReqBy1.Text = "";
            lblReqDate1.Text = "";

        }
        dr1.Dispose();
        dr1.Close();
        conn.Close();

    }

    protected void PopupMessage(string Msg, Control controlID)
    {
        ScriptManager.RegisterClientScriptBlock(controlID, controlID.GetType(), "msg", "alert('" + Msg + "');", true);
    }

    protected void OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        GridView1.DataBind();
        fnLoadData();
    }

    private void fnLoadData()
    {
        //s = WebConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        //con = new SqlConnection(s);
        SqlConnection con = DBConnection.GetConnection();
        con.Open();

        string sSql = "";

        sSql = "SELECT dbo.tbRequest.RAID, dbo.tbRequest.ReqNo, CONVERT(VARCHAR(10),dbo.tbRequest.ReqDate, 110) AS ReqDate, dbo.tbRequest.MRSRCode,";
        sSql = sSql + " dbo.tbRequest.TransactionType, CONVERT(VARCHAR(10), dbo.MRSRMaster.TDate, 110) AS TDate, dbo.Entity.eName AS eFrom, ";
        sSql = sSql + " Entity_1.eName AS eTo, dbo.tbRequest.ReqFor, dbo.tbRequest.ReqReason, dbo.tbRequest.ReqBy,";
        sSql = sSql + " dbo.tbRequest.RRemarks, dbo.tbRequest.UserID, dbo.tbRequest.EntryDate, ";
        sSql = sSql + " dbo.MRSRMaster.MRSRMID, dbo.MRSRMaster.TrType";
        sSql = sSql + " FROM  dbo.tbRequest INNER JOIN";
        sSql = sSql + " dbo.MRSRMaster ON dbo.tbRequest.MRSRCode = dbo.MRSRMaster.MRSRCode INNER JOIN";
        sSql = sSql + " dbo.Entity AS Entity_1 ON dbo.MRSRMaster.InSource = Entity_1.EID INNER JOIN";
        sSql = sSql + " dbo.Entity ON dbo.MRSRMaster.OutSource = dbo.Entity.EID";

        sSql = sSql + " WHERE dbo.tbRequest.GrantDeny=0";

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


    protected void btnDConfirm_Click(object sender, EventArgs e)
    {

        SqlConnection conn = DBConnection.GetConnection();
        string sSql = "";

        //UPDATE tbRequest Table
        sSql = "";
        sSql = "UPDATE tbRequest SET GrantDeny=2";
        sSql = sSql + " WHERE RAID='" + lblAID.Text + "'";

        SqlCommand cmd = new SqlCommand(sSql, conn);
        conn.Open();
        cmd.ExecuteNonQuery();
        conn.Close();


    }

}