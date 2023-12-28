using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;

public partial class Admin_Att_Report_EmpWise : System.Web.UI.Page
{
    SqlConnection conn = DBConnectionHRM.GetConnection();
    //string strCon = "Data Source=103.4.144.183;Integrated Security=false;Initial Catalog=Attendance; User Id=sa";
    string strCon = "Data Source=MKTSERVER;Initial Catalog=HRMS;Integrated Security=SSPI;User Id=sa;Password=Adminn321;Min Pool Size=5;Max Pool Size=60;Connect Timeout=0";
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (System.Convert.ToInt32(Session["Vis"]) == 0)
            {
                Response.Redirect("../Account/Login.aspx");
            }

            //BindGridviewData();
            LoadDropDownList_Employee();
            this.txtFromDate.Text = DateTime.Today.ToString("dd-MMM-yyyy");
            this.txtToDate.Text = DateTime.Today.ToString("dd-MMM-yyyy");
        }
    }

    //LOAD LOCATION IN DROPDOWN LIST
    protected void LoadDropDownList_Employee()
    {
        if (System.Convert.ToInt32(Session["Vis"]) == 0)
        {
            Response.Redirect("../Account/Login.aspx");
        }

        SqlConnection conn = DBConnectionHRM.GetConnection();

        String strQuery = "";
        //strQuery="SELECT FstNm + ' ' + LstNm AS EmpName FROM dbo.Employee";
        //strQuery = strQuery + " WHERE (Active=1)";
        //strQuery = strQuery + " WHERE (Active=1)";
        //strQuery = strQuery + " GROUP BY FstNm + ' ' + LstNm";

        strQuery = "SELECT dbo.Employee.FstNm + ' ' + dbo.Employee.LstNm AS EmpName, ";
        strQuery = strQuery + " dbo.Organization.OrgName, dbo.Dept.DeptNm, dbo.Location.Location,"; 
        strQuery = strQuery + " dbo.Employee.Active";
        strQuery = strQuery + " FROM  dbo.Organization INNER JOIN";
        strQuery = strQuery + " dbo.Employee ON dbo.Organization.OrgID = dbo.Employee.OrgID INNER JOIN";
        strQuery = strQuery + " dbo.Dept ON dbo.Employee.DeptId = dbo.Dept.DeptId INNER JOIN";
        strQuery = strQuery + " dbo.Location ON dbo.Employee.LocationId = dbo.Location.LocationId";
        
        strQuery = strQuery + " WHERE (dbo.Employee.Active = '1')";
        //strQuery = strQuery + " AND (dbo.Organization.OrgName = '" + Session["OrgName"] + "')";
        //strQuery = strQuery + " AND (dbo.Dept.DeptNm = '" + Session["DeptNm"] + "')";
        //strQuery = strQuery + " AND (dbo.Location.Location = '" + Session["Location"] + "')";
        //strQuery = strQuery + " AND (dbo.Location.EntityCode = '" + Session["sBrCode"] + "')";

        strQuery = strQuery + " ORDER BY dbo.Employee.FstNm + ' ' + dbo.Employee.LstNm";
                
        SqlCommand cmd = new SqlCommand(strQuery, conn);        
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = strQuery;
        cmd.Connection = conn;

        try
        {
            conn.Open();
            DropDownList1.DataSource = cmd.ExecuteReader();
            DropDownList1.DataTextField = "EmpName";
            //ddlEmp.DataValueField = "EmpName";
            DropDownList1.DataValueField = "EmpName";
            DropDownList1.DataBind();

            //Add blank item at index 0.
            DropDownList1.Items.Insert(0, new ListItem("", "0"));
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

    //SELECT EMPLOYEE FROM COMBO
    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (System.Convert.ToInt32(Session["Vis"]) == 0)
        {
            Response.Redirect("../Account/Login.aspx");
        }

        string sSql = "";
        SqlConnection conn = DBConnectionHRM.GetConnection();
        sSql = "";        
        sSql="SELECT dbo.Employee.EmpCod, dbo.Employee.FstNm, dbo.Employee.LstNm,";
        sSql=sSql + " dbo.Dept.DeptNm, dbo.Designation.Desg, dbo.Location.Location";
        sSql=sSql + " FROM dbo.Employee INNER JOIN";
        sSql=sSql + " dbo.Designation ON dbo.Employee.DesgId = dbo.Designation.DesgId INNER JOIN";
        sSql=sSql + " dbo.Location ON dbo.Employee.LocationId = dbo.Location.LocationId INNER JOIN";
        sSql = sSql + " dbo.Dept ON dbo.Employee.DeptId = dbo.Dept.DeptId";
        sSql = sSql + " WHERE FstNm + ' ' + LstNm = '" + this.DropDownList1.SelectedValue + "'";
        
        SqlCommand cmd = new SqlCommand(sSql, conn);
        conn.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            this.txtJodID.Text = dr["EmpCod"].ToString();
            this.txtDesg.Text = dr["Desg"].ToString();
            this.txtLocation.Text = dr["Location"].ToString();
        }
        else
        {
            this.txtJodID.Text = "";
            this.txtDesg.Text = "";
            this.txtLocation.Text = "";
        }
        dr.Dispose();
        dr.Close();
        conn.Close();


    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (System.Convert.ToInt32(Session["Vis"]) == 0)
        {
            Response.Redirect("../Account/Login.aspx");
        }

        if (this.DropDownList1.Text.Length < 1)
        {
            string javaScript =
                "<script language=JavaScript>\n" +
                "alert('Please select employee name...');\n" +
                "</script>";
            RegisterStartupScript("WarningScript", javaScript);
            DropDownList1.Focus();
            return;
        }

        //DATE VALIDATION
        if (txtFromDate.Text.Length < 1)
        {
            string javaScript =
                "<script language=JavaScript>\n" +
                "alert('Please select from date.');\n" +
                "</script>";
            RegisterStartupScript("WarningScript", javaScript);
            txtFromDate.Focus();
            return;
        }
        if (txtToDate.Text.Length < 1)
        {
            string javaScript =
                "<script language=JavaScript>\n" +
                "alert('Please select to date.');\n" +
                "</script>";
            RegisterStartupScript("WarningScript", javaScript);
            txtToDate.Focus();
            return;
        }

        BindGridviewData();

    }

    // Bind Gridview Data
    private void BindGridviewData()
    {
        if (System.Convert.ToInt32(Session["Vis"]) == 0)
        {
            Response.Redirect("../Account/Login.aspx");
        }

        using (SqlConnection con = new SqlConnection(strCon))
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                string gSQL = "";
                gSQL = "";
                gSQL = "SELECT " +
                        " CONVERT(VARCHAR(8), dbo.AttndRegDet.Dt, 5) AS [Date], " +
                        " dbo.AttndRegDet.EntryTime AS [Entry Time], dbo.AttndRegDet.ExitTime AS [Exit Time]," +
                        " dbo.AttndRegDet.LateTime AS [Late]," +
                        " dbo.AttndRegDet.LateNote AS [Late Note], dbo.AttndRegDet.EarlyLeaveNote AS [Early Leave Note]" +

                        " FROM dbo.Employee INNER JOIN " +
                        " dbo.AttndRegDet ON dbo.Employee.EmpCod = dbo.AttndRegDet.EmpCod INNER JOIN " +
                        " dbo.Location ON dbo.Employee.LocationId = dbo.Location.LocationId INNER JOIN " +
                        " dbo.Designation ON dbo.Employee.DesgId = dbo.Designation.DesgId " +
                                                
                        " WHERE (dbo.AttndRegDet.Dt>='" + this.txtFromDate.Text + "') " +
                        " AND (dbo.AttndRegDet.Dt<='" + this.txtToDate.Text + "') " +
                        " AND (dbo.Employee.EmpCod = '" + this.txtJodID.Text + "') " +

                        " ORDER BY dbo.AttndRegDet.Dt";
                
                cmd.CommandText = gSQL;
                //cmd.CommandText = "SELECT AID,EmpCod,CONVERT(VARCHAR(8),Dt, 5) AS Dt,EntryTime,ExitTime,EmpInImg,EmpOutImg FROM AttndRegDet";
                cmd.Connection = con;
                con.Open();
                gvProd.DataSource = cmd.ExecuteReader();
                gvProd.DataBind();
                con.Close();
            }
        }

    }
    
    
}