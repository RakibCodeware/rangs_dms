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
using System.Text;

using System.IO;
//using System.Web.Mail;
using System.Net.Mail;
//using iTextSharp.text;
//using iTextSharp.text.pdf;
//using iTextSharp.text.html.simpleparser;


public partial class Admin_Att_Report_WithPhoto : System.Web.UI.Page
{
    SqlConnection conn = DBConnectionHRM.GetConnection();
    //string strCon = "Data Source=103.4.144.183;Integrated Security=false;Initial Catalog=Attendance; User Id=sa";
    string strCon = "Data Source=MKTSERVER;Initial Catalog=HRMS;Integrated Security=SSPI;User Id=sa;Password=Adminn321;Min Pool Size=5;Max Pool Size=60;Connect Timeout=0";
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (System.Convert.ToInt32(Session["Vis"]) == 0)
        {
            Response.Redirect("../Account/Login.aspx");
        }

        if (!IsPostBack)
        {
            //BindGridviewData();
            LoadDropDownList_Org();
            LoadDropDownList_Dept();
            LoadDropDownList_Location();
            this.txtFromDate.Text = DateTime.Today.ToString("dd-MMM-yyyy");
            this.txtToDate.Text = DateTime.Today.ToString("dd-MMM-yyyy");
 
        }
    }

    //LOAD Department IN DROPDOWN LIST
    protected void LoadDropDownList_Org()
    {
        if (System.Convert.ToInt32(Session["Vis"]) == 0)
        {
            Response.Redirect("../Account/Login.aspx");
        }
        SqlConnection conn = DBConnectionHRM.GetConnection();

        //String strQuery = "SELECT DISTINCT OrgName FROM dbo.Organization";
        String strQuery = "SELECT DISTINCT OrgName FROM dbo.Organization WHERE OrgName='REL'";

        //SqlConnection con = new SqlConnection("conn");
        SqlCommand cmd = new SqlCommand(strQuery, conn);
        //SqlCommand cmd = new SqlCommand();
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = strQuery;
        cmd.Connection = conn;

        try
        {
            conn.Open();
            ddlOrg.DataSource = cmd.ExecuteReader();
            ddlOrg.DataTextField = "OrgName";
            //ddlOrg.DataValueField = "ProductID";
            ddlOrg.DataValueField = "OrgName";
            ddlOrg.DataBind();

            //Add blank item at index 0.
            ddlOrg.Items.Insert(0, new ListItem("ALL", "ALL"));
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

    //LOAD Department IN DROPDOWN LIST
    protected void LoadDropDownList_Dept()
    {
        if (System.Convert.ToInt32(Session["Vis"]) == 0)
        {
            Response.Redirect("../Login.aspx");
        }
        SqlConnection conn = DBConnectionHRM.GetConnection();

        //String strQuery = "SELECT DISTINCT DeptNm FROM dbo.Dept";
        String strQuery = "SELECT DISTINCT DeptNm FROM dbo.Dept WHERE DeptNm='Sales'";

        //SqlConnection con = new SqlConnection("conn");
        SqlCommand cmd = new SqlCommand(strQuery, conn);
        //SqlCommand cmd = new SqlCommand();
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = strQuery;
        cmd.Connection = conn;

        try
        {
            conn.Open();
            ddlDept.DataSource = cmd.ExecuteReader();
            ddlDept.DataTextField = "DeptNm";
            //ddlEmp.DataValueField = "ProductID";
            ddlDept.DataValueField = "DeptNm";
            ddlDept.DataBind();

            //Add blank item at index 0.
            ddlDept.Items.Insert(0, new ListItem("ALL", "ALL"));
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

    //LOAD LOCATION IN DROPDOWN LIST
    protected void LoadDropDownList_Location()
    {
        if (System.Convert.ToInt32(Session["Vis"]) == 0)
        {
            Response.Redirect("../Login.aspx");
        }
        SqlConnection conn = DBConnectionHRM.GetConnection();

        String strQuery = "SELECT DISTINCT Location FROM dbo.Location";

        //SqlConnection con = new SqlConnection("conn");
        SqlCommand cmd = new SqlCommand(strQuery, conn);
        //SqlCommand cmd = new SqlCommand();
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = strQuery;
        cmd.Connection = conn;

        try
        {
            conn.Open();
            DropDownList1.DataSource = cmd.ExecuteReader();
            DropDownList1.DataTextField = "Location";
            //ddlEmp.DataValueField = "ProductID";
            DropDownList1.DataValueField = "Location";
            DropDownList1.DataBind();

            //Add blank item at index 0.
            DropDownList1.Items.Insert(0, new ListItem("ALL", "ALL"));
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

    // Bind Gridview Data
    private void BindGridviewData()
    {
        if (System.Convert.ToInt32(Session["Vis"]) == 0)
        {
            Response.Redirect("../Login.aspx");
        }

        using (SqlConnection con = new SqlConnection(strCon))
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                string gSQL = "";
                gSQL = "";
                gSQL = "SELECT dbo.AttndRegDet.AID,";
                gSQL = gSQL + " dbo.Employee.EmpCod, dbo.Employee.FstNm + ' ' + dbo.Employee.LstNm AS EmpName, ";
                gSQL = gSQL + " dbo.Designation.Desg, CONVERT(VARCHAR(8), dbo.AttndRegDet.Dt, 5) AS Dt, ";
                gSQL = gSQL + " dbo.AttndRegDet.EntryTime, dbo.AttndRegDet.ExitTime,";
                gSQL = gSQL + " dbo.AttndRegDet.EmpInImg,dbo.AttndRegDet.EmpOutImg,dbo.Dept.DeptNm,";
                gSQL = gSQL + " dbo.Location.Location";
                gSQL = gSQL + " FROM dbo.Employee LEFT OUTER JOIN ";
                gSQL = gSQL + " dbo.AttndRegDet ON dbo.Employee.EmpCod = dbo.AttndRegDet.EmpCod INNER JOIN ";
                gSQL = gSQL + " dbo.Location ON dbo.Employee.LocationId = dbo.Location.LocationId INNER JOIN ";
                gSQL = gSQL + " dbo.Designation ON dbo.Employee.DesgId = dbo.Designation.DesgId INNER JOIN";
                gSQL = gSQL + " dbo.Organization ON dbo.Employee.OrgID = dbo.Organization.OrgID INNER JOIN";
                gSQL = gSQL + " dbo.Dept ON dbo.Employee.DeptId = dbo.Dept.DeptId";

                gSQL = gSQL + " WHERE (dbo.Employee.Active = '1') ";
                if (this.ddlDept.SelectedValue != "ALL")
                {
                    gSQL = gSQL + " AND (dbo.Dept.DeptNm = '" + this.ddlDept.Text + "') ";
                }
                if (this.ddlOrg.SelectedValue != "ALL")
                {
                    gSQL = gSQL + " AND (dbo.Organization.OrgName = '" + this.ddlOrg.Text + "') ";
                }

                if (this.DropDownList1.SelectedValue != "ALL")
                {
                    gSQL = gSQL + " AND (dbo.Location.Location = '" + this.DropDownList1.Text + "') ";
                }

                if (this.ddlEntityType.SelectedItem.Text != "ALL")
                {
                    gSQL = gSQL + " AND (dbo.Location.EntityType = '" + this.ddlEntityType.Text + "') ";
                }

                gSQL = gSQL + " AND (dbo.AttndRegDet.Dt>='" + this.txtFromDate.Text + "') ";
                gSQL = gSQL + " AND (dbo.AttndRegDet.Dt<='" + this.txtToDate.Text + "') ";

                //gSQL = gSQL + " ORDER BY dbo.AttndRegDet.AID";
                gSQL = gSQL + " ORDER BY dbo.Location.Location, dbo.AttndRegDet.AID";

                //" GROUP BY " +                    
                //" dbo.Employee.EmpCod, dbo.Employee.FstNm + ' ' + dbo.Employee.LstNm, dbo.Designation.Desg, " +
                //" dbo.AttndRegDet.Dt, dbo.AttndRegDet.EntryTime, dbo.AttndRegDet.ExitTime," +
                ////" dbo.AttndRegDet.LateNote, dbo.AttndRegDet.TmEntr, " +
                //" dbo.AttndRegDet.EmpInImg, dbo.AttndRegDet.EmpOutImg ";


                cmd.CommandText = gSQL;
                //cmd.CommandText = "SELECT AID,EmpCod,CONVERT(VARCHAR(8),Dt, 5) AS Dt,EntryTime,ExitTime,EmpInImg,EmpOutImg FROM AttndRegDet";
                cmd.Connection = con;
                con.Open();
                gvImages.DataSource = cmd.ExecuteReader();
                gvImages.DataBind();
                con.Close();
            }
        }
    }
    
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (System.Convert.ToInt32(Session["Vis"]) == 0)
        {
            Response.Redirect("../Login.aspx");
        }
        //
        if (this.DropDownList1.Text.Length <1)
        {
            string javaScript =
                "<script language=JavaScript>\n" +
                "alert('Please select employee job location.');\n" +
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

        //this.lblDateTime.Text = DateTime.Today.ToString("dd-MMM-yyyy HH:MM"); 
        //this.lblDateTime.Text = DateTime.Now.ToString("dd-MMM-yyyy HH:MM");
        this.lblDateTime.Text = "(Print Date & Time: " + DateTime.Now.ToString("dd-MMM-yyyy") + "; " + DateTime.Now.ToString("hh:mm:ss tt") + ")";


        //--------------------------------------------------------------------
        //LOAD TOTAL EMPLOYEE COUNT        
        string sSql = "";
        SqlConnection conn = DBConnectionHRM.GetConnection();
        sSql = "";
        sSql = "SELECT COUNT(dbo.Employee.EmpCod) AS TEmp, dbo.Employee.Active";
        //, dbo.Dept.DeptNm, dbo.Location.Location";
        sSql = sSql + " FROM dbo.Employee INNER JOIN";
        sSql = sSql + " dbo.Organization ON dbo.Employee.OrgID = dbo.Organization.OrgID INNER JOIN";
        sSql = sSql + " dbo.Dept ON dbo.Employee.DeptId = dbo.Dept.DeptId INNER JOIN";
        sSql = sSql + " dbo.Location ON dbo.Employee.LocationId = dbo.Location.LocationId";

        //sSql = sSql + " WHERE (dbo.Location.Location = '" + this.DropDownList1.Text + "') ";
        sSql = sSql + " WHERE dbo.Employee.Active=1";
        if (this.ddlDept.SelectedValue != "ALL")
        {
            sSql = sSql + " AND (dbo.Dept.DeptNm = '" + this.ddlDept.Text + "') ";
        }
        if (this.ddlOrg.SelectedValue != "ALL")
        {
            sSql = sSql + " AND (dbo.Organization.OrgName = '" + this.ddlOrg.Text + "') ";
        }
        if (this.DropDownList1.SelectedValue != "ALL")
        {
            sSql = sSql + " AND (dbo.Location.Location = '" + this.DropDownList1.Text + "') ";
        }

        if (this.ddlEntityType.SelectedItem.Text != "ALL")
        {
            sSql = sSql + " AND (dbo.Location.EntityType = '" + this.ddlEntityType.Text + "') ";
        }

        sSql = sSql + " GROUP BY dbo.Employee.Active";
        //,dbo.Dept.DeptNm, dbo.Location.Location";

        SqlCommand cmd = new SqlCommand(sSql, conn);
        conn.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            this.txtTEmp.Text = dr["TEmp"].ToString();            
        }
        else
        {
            this.txtTEmp.Text = "0";            
        }
        dr.Dispose();
        dr.Close();
        conn.Close();
        //--------------------------------------------------------------------

        //TOTAL PRESENT
        sSql = "";        
        //sSql = " SELECT OrgName, DeptNm, Location, Active, Dt, COUNT(TEmp) AS TPresent";
        //sSql = sSql + " FROM ";
        //sSql = sSql + " (SELECT dbo.Organization.OrgName, dbo.Dept.DeptNm,";
        //sSql = sSql + " dbo.Location.Location, dbo.Employee.Active, dbo.AttndRegDet.Dt, ";
        //sSql = sSql + " dbo.Employee.EmpCod AS TEmp";
        //sSql = sSql + " FROM dbo.Organization INNER JOIN";
        //sSql = sSql + " dbo.Employee ON dbo.Organization.OrgID = dbo.Employee.OrgID INNER JOIN";
        //sSql = sSql + " dbo.Dept ON dbo.Employee.DeptId = dbo.Dept.DeptId INNER JOIN";
        //sSql = sSql + " dbo.Location ON dbo.Employee.LocationId = dbo.Location.LocationId INNER JOIN";
        //sSql = sSql + " dbo.AttndRegDet ON dbo.Employee.EmpCod = dbo.AttndRegDet.EmpCod";
        
        sSql = "SELECT dbo.AttndRegDet.Dt, COUNT(dbo.AttndRegDet.EmpCod) AS TPresent, dbo.Employee.Active";
        sSql = sSql + " FROM dbo.Employee INNER JOIN";
        sSql = sSql + " dbo.Organization ON dbo.Employee.OrgID = dbo.Organization.OrgID INNER JOIN";
        sSql = sSql + " dbo.Dept ON dbo.Employee.DeptId = dbo.Dept.DeptId INNER JOIN";
        sSql = sSql + " dbo.Location ON dbo.Employee.LocationId = dbo.Location.LocationId INNER JOIN";
        sSql = sSql + " dbo.AttndRegDet ON dbo.Employee.EmpCod = dbo.AttndRegDet.EmpCod";
        
        sSql = sSql + " WHERE (dbo.AttndRegDet.Dt>='" + this.txtFromDate.Text + "') ";
        sSql = sSql + " AND (dbo.AttndRegDet.Dt<='" + this.txtToDate.Text + "') ";

        sSql = sSql + " AND dbo.Employee.Active=1";

        //sSql = sSql + " AND (dbo.Location.Location = '" + this.DropDownList1.Text + "') ";
        if (this.DropDownList1.SelectedValue != "ALL")
        {
            sSql = sSql + " AND (dbo.Location.Location = '" + this.DropDownList1.Text + "') ";
        }
        
        if (this.ddlDept.SelectedValue != "ALL")
        {
            sSql = sSql + " AND (dbo.Dept.DeptNm = '" + this.ddlDept.Text + "') ";
        }
        if (this.ddlOrg.SelectedValue != "ALL")
        {
            sSql = sSql + " AND (dbo.Organization.OrgName = '" + this.ddlOrg.Text + "') ";
        }

        if (this.ddlEntityType.SelectedItem.Text != "ALL")
        {
            sSql = sSql + " AND (dbo.Location.EntityType = '" + this.ddlEntityType.Text + "') ";
        }

        //sSql = sSql + " GROUP BY dbo.Organization.OrgName, dbo.Dept.DeptNm, dbo.Location.Location,";
        //sSql = sSql + " dbo.Employee.Active, dbo.AttndRegDet.Dt, dbo.Employee.EmpCod) AS a";
        sSql = sSql + " GROUP BY dbo.AttndRegDet.Dt, dbo.Employee.Active";

        //sSql = sSql + " GROUP BY dbo.AttndRegDet.Dt";
        //sSql = sSql + " GROUP BY OrgName, DeptNm, Location, Active, Dt";


        cmd = new SqlCommand(sSql, conn);
        conn.Open();
        dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            this.txtTPresent.Text = dr["TPresent"].ToString();
        }
        else
        {
            this.txtTPresent.Text = "0";
        }
        dr.Dispose();
        dr.Close();
        conn.Close();
        //---------------------------------------------------------

        //Total Late Comer
        sSql = "";
        sSql ="SELECT COUNT(dbo.AttndRegDet.EmpCod) AS tLateComer, dbo.AttndRegDet.Dt";
        //sSql = sSql + " dbo.Organization.OrgName, dbo.Dept.DeptNm, dbo.Location.Location";
        sSql = sSql + " FROM dbo.AttndRegDet INNER JOIN";
        sSql = sSql + " dbo.Employee ON dbo.AttndRegDet.EmpCod = dbo.Employee.EmpCod INNER JOIN";
        sSql = sSql + " dbo.Organization ON dbo.Employee.OrgID = dbo.Organization.OrgID INNER JOIN";
        sSql = sSql + " dbo.Dept ON dbo.Employee.DeptId = dbo.Dept.DeptId INNER JOIN";
        sSql = sSql + " dbo.Location ON dbo.Employee.LocationId = dbo.Location.LocationId";
                
        sSql = sSql + " WHERE (dbo.AttndRegDet.Dt>='" + this.txtFromDate.Text + "') ";
        sSql = sSql + " AND (dbo.AttndRegDet.Dt<='" + this.txtToDate.Text + "') ";
        sSql = sSql + " AND (dbo.AttndRegDet.LateTime <> N'00:00') ";

        sSql = sSql + " AND dbo.Employee.Active=1"; 

        if (this.ddlDept.SelectedValue != "ALL")
        {
            sSql = sSql + " AND (dbo.Dept.DeptNm = '" + this.ddlDept.Text + "') ";
        }
        if (this.ddlOrg.SelectedValue != "ALL")
        {
            sSql = sSql + " AND (dbo.Organization.OrgName = '" + this.ddlOrg.Text + "') ";
        }

        if (this.DropDownList1.SelectedValue != "ALL")
        {
            sSql = sSql + " AND (dbo.Location.Location = '" + this.DropDownList1.Text + "') ";
        }

        if (this.ddlEntityType.SelectedItem.Text != "ALL")
        {
            sSql = sSql + " AND (dbo.Location.EntityType = '" + this.ddlEntityType.Text + "') ";
        }

        sSql = sSql + " GROUP BY dbo.AttndRegDet.Dt";
        //dbo.Organization.OrgName, dbo.Dept.DeptNm, dbo.Location.Location";

        cmd = new SqlCommand(sSql, conn);
        conn.Open();
        dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            this.txtTLate.Text = dr["tLateComer"].ToString();
        }
        else
        {
            this.txtTLate.Text = "0";
        }
        dr.Dispose();
        dr.Close();
        conn.Close();
        //----------------------------------------------------------

        double tAbs = 0;
        //TOTAL ABSENT
        tAbs = Convert.ToDouble(this.txtTEmp.Text) - (Convert.ToDouble(this.txtTPresent.Text) + Convert.ToDouble(this.txtTLeave.Text) + Convert.ToDouble(this.txtTTour.Text));
        this.txtTAbs.Text = Convert.ToString(tAbs);
        if (tAbs > 0)
        {
            this.lblAbsentCaption.Visible = true;
        }
        else
        {
            this.lblAbsentCaption.Visible = false;
        }

        //LOAD DATA IN GRIDVIEW for Present Employee
        BindGridviewData();

        //LOAD DATA IN GRIDVIEW for Absent Employee
        BindGridviewData_Abs();

    }

    // Bind Gridview Data
    private void BindGridviewData_Abs()
    {
        if (System.Convert.ToInt32(Session["Vis"]) == 0)
        {
            Response.Redirect("../Login.aspx");
        }

        using (SqlConnection con1 = new SqlConnection(strCon))
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                string gSQL = "";
                gSQL = "";

                gSQL = "SELECT dbo.Employee.EmpCod as [Job ID], dbo.Employee.FstNm + ' ' + dbo.Employee.LstNm AS [Employee Name],";
                gSQL = gSQL + " dbo.Designation.Desg as [Designation], dbo.Dept.DeptNm as [Department],";
                gSQL = gSQL + " dbo.Organization.OrgName as [Organization], dbo.Location.Location as [Location]";
                gSQL = gSQL + " FROM  dbo.Employee INNER JOIN";
                gSQL = gSQL + " dbo.Dept ON dbo.Employee.DeptId = dbo.Dept.DeptId INNER JOIN";
                gSQL = gSQL + " dbo.Designation ON dbo.Employee.DesgId = dbo.Designation.DesgId INNER JOIN";
                gSQL = gSQL + " dbo.Location ON dbo.Employee.LocationId = dbo.Location.LocationId INNER JOIN";
                gSQL = gSQL + " dbo.Organization ON dbo.Employee.OrgID = dbo.Organization.OrgID";
                
                gSQL = gSQL + " WHERE dbo.Employee.Active=1 AND (dbo.Employee.EmpCod NOT IN";
                gSQL = gSQL + " (SELECT EmpCod";
                gSQL = gSQL + " FROM dbo.AttndRegDet AS AttndRegDet_1";
                //gSQL = gSQL + " WHERE (CONVERT(VARCHAR(8), Dt, 5) = '22-05-16')))";

                gSQL = gSQL + " WHERE (Dt>='" + this.txtFromDate.Text + "') ";
                gSQL = gSQL + " AND (Dt<='" + this.txtToDate.Text + "')))";
                
                
                if (this.ddlDept.SelectedValue != "ALL")
                {
                    gSQL = gSQL + " AND (dbo.Dept.DeptNm = '" + this.ddlDept.Text + "') ";
                }
                if (this.ddlOrg.SelectedValue != "ALL")
                {
                    gSQL = gSQL + " AND (dbo.Organization.OrgName = '" + this.ddlOrg.Text + "')";
                }
                if (this.DropDownList1.SelectedValue != "ALL")
                {
                    gSQL = gSQL + " AND (dbo.Location.Location = '" + this.DropDownList1.Text + "') ";
                }

                if (this.ddlEntityType.SelectedItem.Text != "ALL")
                {
                    gSQL = gSQL + " AND (dbo.Location.EntityType = '" + this.ddlEntityType.Text + "') ";
                }

                gSQL = gSQL + " ORDER BY dbo.Location.Location, dbo.Employee.EmpCod";
                
                cmd.CommandText = gSQL;
                //cmd.CommandText = "SELECT AID,EmpCod,CONVERT(VARCHAR(8),Dt, 5) AS Dt,EntryTime,ExitTime,EmpInImg,EmpOutImg FROM AttndRegDet";
                cmd.Connection = con1;
                con1.Open();
                gvAbsent.DataSource = cmd.ExecuteReader();
                gvAbsent.DataBind();
                con1.Close();
            }
        }
    }

    protected void btnSendEmail_Click(object sender, EventArgs e)
    {        
        //MailMessage Msg = new MailMessage();
        //Msg.Body += "Attendance Report: <br/><br/>";
        //Msg.Body += GetGridviewData(gvImages);

        /*
        string to = "zunayed@rangs.com.bd";
        string From = "zunayedqu10@gmail.com";
        string subject = "Attendance Report";
        string Body = "Dear sir ,<br> Plz Check d Attachment <br><br>";
        Body += GridViewToHtml(gvImages); //Elaborate this function detail later
        Body += "<br><br>Regards,<br>Software";
        bool send = send_mail(to, From, subject, Body);//Elaborate this function detail later
        if (send == true)
        {
            string CloseWindow = "alert('Mail Sent Successfully!');";
            ClientScript.RegisterStartupScript(this.GetType(), "CloseWindow", CloseWindow, true);
        }
        else
        {
            string CloseWindow = "alert('Problem in Sending mail...try later!');";
            ClientScript.RegisterStartupScript(this.GetType(), "CloseWindow", CloseWindow, true);
        }
          
         */
    }

    private string GridViewToHtml(GridView gv)
    {
        StringBuilder sb = new StringBuilder();
        StringWriter sw = new StringWriter(sb);
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        gv.RenderControl(hw);
        return sb.ToString();
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        //Confirms that an HtmlForm control is rendered for the specified ASP.NET server control at run time.
    }

    public bool send_mail(string to, string from, string subject, string body)
    {
        MailMessage msg = new MailMessage(from, to);
        msg.Subject = subject;
        AlternateView view;
        SmtpClient client;
        StringBuilder msgText = new StringBuilder();
        msgText.Append(" <html><body><br></body></html> <br><br><br>  " + body);
        view = AlternateView.CreateAlternateViewFromString(msgText.ToString(), null, "text/html");

        msg.AlternateViews.Add(view);
        client = new SmtpClient();
        client.Host = "smtp.gmail.com";
        client.Port = 587;
        client.Credentials = new System.Net.NetworkCredential("zunayedqu10@gmail.com", "bertalaAdminn");
        client.EnableSsl = true; //Gmail works on Server Secured Layer
        client.Send(msg);
        bool k = true;
        return k;
    }

    public string GetGridviewData(GridView gv)
    {
        StringBuilder strBuilder = new StringBuilder();
        StringWriter strWriter = new StringWriter(strBuilder);
        HtmlTextWriter htw = new HtmlTextWriter(strWriter);
        gv.RenderControl(htw);
        return strBuilder.ToString();
    }



    protected void btnExportPDF_Click(object sender, EventArgs e)
    {
        //Response.ContentType = "application/pdf";
        //Response.AddHeader("content-disposition",
        // "attachment;filename=GridViewExport.pdf");
        //Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //StringWriter sw = new StringWriter();
        //HtmlTextWriter hw = new HtmlTextWriter(sw);
        //gvImages.AllowPaging = false;
        //gvImages.DataBind();
        //gvImages.RenderControl(hw);
        //StringReader sr = new StringReader(sw.ToString());
        //Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
        //HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
        //PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
        //pdfDoc.Open();
        //htmlparser.Parse(sr);
        //pdfDoc.Close();
        //Response.Write(pdfDoc);
        //Response.End(); 
    }
}