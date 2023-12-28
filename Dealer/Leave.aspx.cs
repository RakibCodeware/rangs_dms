using System;
using System.Collections;
using System.Configuration;
using System.Data;
//using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
//using System.Xml.Linq;
using System.Net.Mail;
using System.Data.SqlClient;


public partial class Leave : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        btnSend.Attributes.Add("OnClick", "return confirm_Save();");
    }

    /*
    protected void Btn_SendMail_Click(object sender, EventArgs e)
    {
        //SmtpClient smtpClient = new SmtpClient();
        MailMessage mailObj = new MailMessage(
            txtFrom.Text, txtTo.Text, txtSubject.Text, txtBody.Text);
        SmtpClient SMTPServer = new SmtpClient("localhost");
        try
        {
            SMTPServer.Send(mailObj);
        }
        catch (Exception ex)
        {
            Label1.Text = ex.ToString();
        }
    }*/
    //-----------------------------------------------------------

    protected void btnSend_Click(object sender, EventArgs e)
    {
        //USER JOB ID VALIDATION
        if (txtJobId.Text.Length < 1)
        {
            string javaScript =
                "<script language=JavaScript>\n" +
                "alert('Employee ID is blank.');\n" +
                "</script>";
            RegisterStartupScript("WarningScript", javaScript);
            //Response.Write("<script language='javascript'> window.alert('Submitted Successfully...')</script>"); 
            txtJobId.Focus();
            return;
        }

        //USER PASSWORD VALIDATION
        if (txtPassword.Text.Length < 1)
        {
            string javaScript =
                "<script language=JavaScript>\n" +
                "alert('Employee Password is blank.');\n" +
                "</script>";
            RegisterStartupScript("WarningScript", javaScript);
            //Response.Write("<script language='javascript'> window.alert('Submitted Successfully...')</script>"); 
            txtPassword.Focus();
            return;
        }

        //USER NAME VALIDATION
        if (txtName.Text.Length < 1)
        {
            string javaScript =
                "<script language=JavaScript>\n" +
                "alert('Employee Name is blank.');\n" +
                "</script>";
            RegisterStartupScript("WarningScript", javaScript);
            //Response.Write("<script language='javascript'> window.alert('Submitted Successfully...')</script>"); 
            txtPassword.Focus();
            return;
        }

        //FROM DATE
        if (txtFromDate.Text.Length < 1)
        {
            string javaScript =
                "<script language=JavaScript>\n" +
                "alert('Please enter From Date.');\n" +
                "</script>";
            RegisterStartupScript("WarningScript", javaScript);
            //Response.Write("<script language='javascript'> window.alert('Submitted Successfully...')</script>"); 
            txtFromDate.Focus();
            return;
        }

        //TO DATE
        if (txtToDate.Text.Length < 1)
        {
            string javaScript =
                "<script language=JavaScript>\n" +
                "alert('Please enter To Date.');\n" +
                "</script>";
            RegisterStartupScript("WarningScript", javaScript);
            //Response.Write("<script language='javascript'> window.alert('Submitted Successfully...')</script>"); 
            txtToDate.Focus();
            return;
        }

        //PURPOSE
        if (txtPurpose.Text.Length < 1)
        {
            string javaScript =
                "<script language=JavaScript>\n" +
                "alert('Please enter leave purpose.');\n" +
                "</script>";
            RegisterStartupScript("WarningScript", javaScript);
            //Response.Write("<script language='javascript'> window.alert('Submitted Successfully...')</script>"); 
            txtPurpose.Focus();
            return;
        }

        //Leave Period
        if (txtAdd.Text.Length < 1)
        {
            string javaScript =
                "<script language=JavaScript>\n" +
                "alert('Please enter address during leave period.');\n" +
                "</script>";
            RegisterStartupScript("WarningScript", javaScript);
            //Response.Write("<script language='javascript'> window.alert('Submitted Successfully...')</script>"); 
            txtAdd.Focus();
            return;
        }

        //Joining After Leave
        if (txtJoinDate.Text.Length < 1)
        {
            string javaScript =
                "<script language=JavaScript>\n" +
                "alert('Please enter date of joining after leave.');\n" +
                "</script>";
            RegisterStartupScript("WarningScript", javaScript);
            //Response.Write("<script language='javascript'> window.alert('Submitted Successfully...')</script>"); 
            txtJoinDate.Focus();
            return;
        }



        try
        {
            string sMessage = "";
        
            sMessage ="Dear Sir, \r\n" +
                "\r\n" +
                "Kindly grant me " + this.DropDownList1.Text  + " Leave " +
                "from " + this.txtFromDate.Text + "" +
                " to " + this.txtToDate.Text + " for the purpose of " + this.txtPurpose.Text + "." +
                " In this period I will be in " + this.txtAdd.Text + " and join on " + this.txtJoinDate.Text + "." +
                "\r\n" +
                "\r\n" +
                "Best Regards,\r\n" +
                "" + this.txtName.Text  + "\r\n" +
                "" + this.txtDesg.Text + ", " + this.txtDept.Text + "\r\n" +
                "" + this.txtLocation.Text + "\r\n" +
                "" + this.txtEmail.Text + "\r\n" +
                "" + this.txtphone.Text + "" +
                "";
         
            //MailSender.SendEmail(txtGmailId.Text + "@gmail.com", txtPassword.Text, txtTo.Text, txtSubject.Text, txtMessage.Text, System.Web.Mail.MailFormat.Text, "");
            //MailSender.SendEmail("relleave@gmail.com", "relleave123", "zunayed@rangs.org", "Leave Notification", sMessage, System.Web.Mail.MailFormat.Text, "");
            //lblError.Text = "Mail sent successfully.";


            //FUNCTION FOR SAVE DATA
            //fnSaveData();

        }
        catch (Exception ex)
        {
            lblError.Text = ex.Message;
        }
    }

    protected void fnSaveData()
    {

        SqlConnection conn1 = DBConnectionHRM.GetConnection();

        string gSql = "";
        gSql = "INSERT INTO LevApp(Dt,EmpCod,StDate," +
                " EndDate,LevType,LevPurpose,LevAdd,Remarks,Status)" +
                " VALUES('" + DateTime.Today + "'," +
                " '" + this.txtJobId.Text + "'," +
                " '" + this.txtFromDate.Text + "','" + this.txtToDate.Text + "'," +
                " '" + this.DropDownList1.Text + "','" + this.txtPurpose.Text + "'," +
                " '" + this.txtAdd.Text + "','" + this.txtMessage.Text + "','Sent'" +
                ")";

        SqlCommand cmd1 = new SqlCommand(gSql, conn1);
        conn1.Open();
        cmd1.ExecuteNonQuery();
        conn1.Close();
        
        Response.Write("<script language='javascript'> window.alert('Submitted Successfully...')</script>");


        //TEXT CLEAR-------------------
        this.txtJobId.Text = "";
        this.txtPassword.Text = ""; 
        this.txtName.Text = "";
        this.txtDesg.Text = "";
        this.txtDept.Text = "";
        this.txtLocation.Text = "";
        this.txtEmail.Text = "";
        this.txtphone.Text = "";
        this.txtFromDate.Text = "";
        this.txtToDate.Text = "";
        this.txtPurpose.Text = "";
        this.txtAdd.Text = "";
        this.txtMessage.Text = "";
        this.txtJoinDate.Text = "";
        //-----------------------------

        txtJobId.Focus();

    }

    
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        //USER NAME VALIDATION
        if (txtJobId.Text.Length < 1)
        {            
            string javaScript =
                "<script language=JavaScript>\n" +
                "alert('Employee ID is blank.');\n" +
                "</script>";
                RegisterStartupScript("WarningScript", javaScript);            
            //Response.Write("<script language='javascript'> window.alert('Submitted Successfully...')</script>"); 
            txtJobId.Focus(); 
            return;
        }

        //USER NAME VALIDATION
        if (txtPassword.Text.Length < 1)
        {            
            string javaScript =
                "<script language=JavaScript>\n" +
                "alert('Employee Password is blank.');\n" +
                "</script>";
                RegisterStartupScript("WarningScript", javaScript);            
            //Response.Write("<script language='javascript'> window.alert('Submitted Successfully...')</script>"); 
            txtPassword.Focus(); 
            return;
        }
              
        SqlConnection conn = DBConnectionHRM.GetConnection();
        
        string gSql;
        gSql = "";             
        gSql = "SELECT dbo.Employee.EmpCod, dbo.Employee.FstNm + ' ' + dbo.Employee.LstNm AS EmpName, dbo.Organization.OrgName, dbo.Dept.DeptNm," +
            " dbo.Designation.Desg, dbo.Location.Location, dbo.Employee.Mail, dbo.Employee.Phone, dbo.EmpPass.Pass " +
            " FROM  dbo.Employee INNER JOIN " +
            " dbo.Designation ON dbo.Employee.DesgId = dbo.Designation.DesgId INNER JOIN " +
            " dbo.Dept ON dbo.Employee.DeptId = dbo.Dept.DeptId INNER JOIN " +
            " dbo.Location ON dbo.Employee.LocationId = dbo.Location.LocationId INNER JOIN " +
            " dbo.Organization ON dbo.Employee.OrgID = dbo.Organization.OrgID LEFT OUTER JOIN " +
            " dbo.EmpPass ON dbo.Employee.EmpCod = dbo.EmpPass.EmpCod " +

            " where dbo.Employee.EmpCod='" + this.txtJobId.Text + "'" +
            " AND dbo.EmpPass.Pass='" + this.txtPassword.Text + "'" +
            " AND Active=1" +

            " GROUP BY dbo.Employee.EmpCod, dbo.Employee.FstNm + ' ' + dbo.Employee.LstNm, dbo.Organization.OrgName, dbo.Dept.DeptNm, dbo.Designation.Desg, " +
            " dbo.Location.Location, dbo.Employee.Mail, dbo.Employee.Phone, dbo.EmpPass.Pass ";
        
        SqlCommand cmd = new SqlCommand(gSql, conn);        
        conn.Open();
                    
        SqlDataReader dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            this.txtName.Text = dr["EmpName"].ToString();
            this.txtDesg.Text = dr["Desg"].ToString();
            this.txtDept.Text = dr["DeptNm"].ToString();
            this.txtLocation.Text = dr["Location"].ToString();
            this.txtEmail.Text = dr["Mail"].ToString();
            this.txtphone.Text = dr["Phone"].ToString();
        }
        else
        {
            this.txtName.Text = "";
            this.txtDesg.Text = "";
            this.txtDept.Text = "";
            this.txtLocation.Text = "";
            this.txtEmail.Text = "";
            this.txtphone.Text = "";
	    Response.Write("<script language='javascript'> window.alert('Job ID or Password not correct ...')</script>");
        }

        dr.Dispose();
        dr.Close();
        conn.Close();

    }

}
