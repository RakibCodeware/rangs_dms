using System;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Globalization;
using System.Text.RegularExpressions;

public partial class discount_code : System.Web.UI.Page
{
    SqlConnection conn = DBConnection.GetConnection();
    SqlConnection conn1 = DBConnection.GetConnection();
    SqlConnection _connStr = DBConnection.GetConnection();
    int iMRSRID = 0;
    

    protected void Page_Load(object sender, EventArgs e)
    {

        if (System.Convert.ToInt32(Session["Vis"]) == 0)
        {
            Response.Redirect("../Account/Login.aspx");
        }

        //btnSerch.Attributes.Add("OnClick", "return confirm_Add();");
        if (!IsPostBack)
        {
            txtRefBy.Text = Session["UserName"].ToString();

            //LOAD CTP
            LoadDropDownList_CTP();

            fnLoadAutoBillNo();
        }


    }

    protected void LoadDropDownList_CTP()
    {
        if (System.Convert.ToInt32(Session["Vis"]) == 0)
        {
            Response.Redirect("../Account/Login.aspx");
        }

        SqlConnection conn = DBConnection.GetConnection();

        //String strQuery = "select DISTINCT Dist from tbDistThana Order By Dist";
        string gSql = "";   
        gSql ="SELECT EID, eName, EntityType, ActiveDeactive";
        gSql = gSql + " FROM dbo.Entity";
        gSql = gSql + " WHERE (EntityType = 'Showroom') AND (ActiveDeactive = 1)";
        gSql = gSql + " ORDER BY eName";

        //SqlConnection con = new SqlConnection("conn");
        SqlCommand cmd = new SqlCommand(gSql, conn);
        //SqlCommand cmd = new SqlCommand();
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = gSql;
        cmd.Connection = conn;
        //try
        //{
            conn.Open();
            ddlCTP.DataSource = cmd.ExecuteReader();
            ddlCTP.DataTextField = "eName";
            //ddlModel.DataValueField = "ProductID";
            ddlCTP.DataValueField = "EID";
            ddlCTP.DataBind();

            //Add blank item at index 0.
            ddlCTP.Items.Insert(0, new ListItem("--Select--", "0"));

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



    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        if (txtDisAmnt.Text.Length == 0)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert",
                            "<script>alert('" + "Please enter discount amount ..." + "');</script>", false);
            txtDisAmnt.Focus();
            return;
        }

        if (txtDisAmnt.Text == "0")
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert",
                            "<script>alert('" + "Please enter discount amount ..." + "');</script>", false);
            txtDisAmnt.Focus();
            return;
        }


        //SqlConnection conn = DBConnection.GetConnection();

        try
        {
            fnLoadAutoBillNo();

            double tt = Convert.ToDouble(txtDisAmnt.Text) * 4 / 10;
            string tt1 = StringReverse(Convert.ToString(tt.ToString("00000")));
            txtDisCode.Text = lblDisID.Text + "" + tt1;
        }
        catch
        {
            //
        }
        

        /*
        int iCount = 0;
        this.Literal1.Text = "";

        conn.Open();

        string gSql = "";
        gSql = "SELECT dbo.MRSRDetails.SLNO, CONVERT(varchar(12), dbo.MRSRMaster.TDate, 105) AS TDate, ";
        gSql = gSql + " dbo.MRSRMaster.MRSRCode, dbo.MRSRDetails.ProductID, dbo.MRSRMaster.TrType, ";
        gSql = gSql +  " CASE dbo.MRSRMaster.TrType WHEN 1 THEN 'Receive' WHEN 2 THEN 'Issue' WHEN 3 THEN 'Sale'";
        gSql = gSql + " WHEN 4 THEN 'Transfer' WHEN -3 THEN 'Customer Withdrawn' END AS tStatus,";
        gSql = gSql +  " dbo.Entity.eName AS OutSource, Entity_1.eName AS InSource,";
        gSql = gSql +  " dbo.MRSRMaster.MRSRMID";
        gSql = gSql +  " FROM dbo.MRSRDetails INNER JOIN";
        gSql = gSql +  " dbo.MRSRMaster ON dbo.MRSRDetails.MRSRMID = dbo.MRSRMaster.MRSRMID INNER JOIN";
        gSql = gSql +  " dbo.Entity ON dbo.MRSRMaster.OutSource = dbo.Entity.EID INNER JOIN";
        gSql = gSql +  " dbo.Entity AS Entity_1 ON dbo.MRSRMaster.InSource = Entity_1.EID";
        
        gSql = gSql +  " WHERE  (dbo.MRSRDetails.SLNO = '" + txtSL.Text + "')";
        //gSql = gSql + " AND (dbo.MRSRDetails.ProductID = 5171)";
        gSql = gSql + " AND (dbo.MRSRDetails.ProductID = '" + ddlModel.SelectedItem.Value + "')";
        
        gSql = gSql +  " GROUP BY dbo.MRSRDetails.SLNO, dbo.MRSRMaster.TDate, dbo.MRSRMaster.MRSRCode,";
        gSql = gSql +  " dbo.MRSRDetails.ProductID, dbo.MRSRMaster.TrType, ";
        gSql = gSql + " dbo.Entity.eName, Entity_1.eName, Entity_1.eName, dbo.MRSRMaster.MRSRMID";

        SqlCommand cmd = new SqlCommand(gSql, conn);
        SqlDataReader dr2 = cmd.ExecuteReader();

        while (dr2.Read())
        {
            iCount = iCount + 1;
            this.Literal1.Text = this.Literal1.Text + @"<tr>";
            this.Literal1.Text = this.Literal1.Text + "<td>" + iCount + "</td>";
            this.Literal1.Text = this.Literal1.Text + "<td>" + dr2["TDate"] + "</td>";
            this.Literal1.Text = this.Literal1.Text + "<td>" + dr2["MRSRCode"] + "</td>";
            this.Literal1.Text = this.Literal1.Text + "<td>" + dr2["tStatus"] + "</td>";
            this.Literal1.Text = this.Literal1.Text + "<td>" + dr2["OutSource"] + "</td>";
            this.Literal1.Text = this.Literal1.Text + "<td>" + dr2["InSource"] + "</td>";
            this.Literal1.Text = this.Literal1.Text + "</tr>";
        }

        conn.Close();
        dr2.Close();
        */

    }


    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        //

        try
        {
            if (txtDisAmnt.Text.Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert",
                                "<script>alert('" + "Please enter discount amount ..." + "');</script>", false);
                txtDisAmnt.Focus();
                return;
            }

            if (txtDisAmnt.Text == "0")
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert",
                                "<script>alert('" + "Please enter discount amount ..." + "');</script>", false);
                txtDisAmnt.Focus();
                return;
            }

            if (txtDisCode.Text.Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert",
                                "<script>alert('" + "Please enter discount amount and generate code ..." + "');</script>", false);
                txtDisAmnt.Focus();
                return;
            }

            if (ddlCTP.SelectedItem.Value == "0")
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert",
                                "<script>alert('" + "Please select CTP Name ..." + "');</script>", false);
                //txtDisAmnt.Focus();
                return;
            }


            // SAVE DATA
            string gSQL = "";
            gSQL = "INSERT INTO tbKeyGen(";
            gSQL = gSQL + " DisCode,DisID,";
            gSQL = gSQL + " DisAmnt,DisRef,";
            gSQL = gSQL + " EID,dTag,UserID)";
            gSQL = gSQL + " VALUES(";
            gSQL = gSQL + " '" + txtDisCode.Text + "','" + lblDisID.Text + "',";
            gSQL = gSQL + " '" + txtDisAmnt.Text + "','" + txtRefBy.Text + "',";
            gSQL = gSQL + " '" + ddlCTP.SelectedItem.Value + "',0,'" + Session["UserName"].ToString() + "'";
            gSQL = gSQL + " )";
            SqlCommand cmdU = new SqlCommand(gSQL, conn);
            conn.Open();
            cmdU.ExecuteNonQuery();
            conn.Close();
        }
        catch
        {
            //
        }

        ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert",
         "<script>alert('" + "Submit Successfully." + "');</script>", false);

        txtDisCode.Text = "";
        txtDisAmnt.Text = "";
        //txtRefBy.Text = "";

        try
        {
            fnLoadAutoBillNo();
        }
        catch
        {
            //
        }
        txtDisAmnt.Focus();



    }


    protected void fnLoadAutoBillNo()
    {

        SqlConnection conn = DBConnection.GetConnection();

        SqlCommand dataCommand = new SqlCommand();
        dataCommand.Connection = conn;

        dataCommand.CommandType = CommandType.Text;
        //con.Open();

        int xMax = 0;
        string sAutoNo = "";

        string sSql = "";        
        //sSql = "SELECT IIF(MAX(RIGHT(DisID, 3)) Is Null,'0',MAX(RIGHT(DisID, 3))) AS aInvNo";
        sSql = "SELECT ISNULL(MAX(RIGHT(DisID, 3)), 0) AS aInvNo";
        sSql = sSql + " FROM tbKeyGen";
        //sSql = sSql + " WHERE (LEFT(DelNo, 5) = '" + DateTime.Now.ToString("yyyy") + "/" + "')";
        sSql = sSql + " WHERE (LEFT(DisID, 6) = '" + StringReverse(DateTime.Now.ToString("dd")) + "" + StringReverse(DateTime.Now.ToString("MM")) + "" + StringReverse(DateTime.Now.ToString("yy")) + "" + "')";
        //sSql = sSql + " AND TrType=2";

        SqlCommand cmd = new SqlCommand(sSql, conn);
        conn.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        //try
        //{
        if (dr.Read())
        {
            if (dr["aInvNo"] == null)
            {
                xMax = 1;
            }
            else
            {
                xMax = Convert.ToInt32(dr["aInvNo"]) + 1;
            }
            //sAutoNo = "" + DateTime.Now.ToString("yyyy") + "/" + xMax.ToString("00000");
            sAutoNo = "" + StringReverse(DateTime.Now.ToString("dd")) + "" + StringReverse(DateTime.Now.ToString("MM")) + "" + StringReverse(DateTime.Now.ToString("yy")) + "" + xMax.ToString("000");
            lblDisID.Text = sAutoNo;
        }
        else
        {
            xMax = Convert.ToInt32(dr["aInvNo"]) + 1;
            //sAutoNo = "" + DateTime.Now.ToString("yyyy") + "/" + xMax.ToString("00000");
            sAutoNo = "" + StringReverse(DateTime.Now.ToString("dd")) + "" + StringReverse(DateTime.Now.ToString("MM")) + "" + StringReverse(DateTime.Now.ToString("yy")) + "" + xMax.ToString("000");
            lblDisID.Text = sAutoNo;
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
    }


    public static string StringReverse(string str)
    {
        if (str.Length > 0)
            return str[str.Length - 1] + StringReverse(str.Substring(0, str.Length - 1));
        else
            return str;
    }


}