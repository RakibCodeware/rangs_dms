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

public partial class Admin_Forms_UserInfo : System.Web.UI.Page
{
    SqlConnection conn = DBConnection.GetConnection();
    SqlConnection conn1 = DBConnection.GetConnection();
    SqlConnection _connStr = DBConnection.GetConnection();
    SqlDataReader dr;

    protected void Page_Load(object sender, EventArgs e)
    {
        btnSave.Attributes.Add("OnClick", "return confirm_Save();");
        btnCancel.Attributes.Add("OnClick", "return confirm_Cancel();");

        if (!IsPostBack)
        {
            //LOAD DATA IN GRIDVIEW
            fnLoadData();
            LoadDropDownList();
        }
    }

    protected void fnLoadData()
    {

        SqlConnection con = DBConnection.GetConnection();
        con.Open();

        string sSql = "";                       
        sSql = "SELECT dbo.SoftUser.FullName, dbo.SoftUser.Designation, dbo.SoftUser.UserName," +
            " dbo.SoftUser.Passward, dbo.SoftUser.WebAccess, " +
            " dbo.SoftUser.UserType, dbo.SoftUser.eName, dbo.SoftUser.Active" +
            " FROM dbo.SoftUser LEFT OUTER JOIN" +
            " dbo.Entity ON dbo.SoftUser.EID = dbo.Entity.EID" +
            " ORDER BY dbo.SoftUser.FullName";
        
        SqlCommand cmd = new SqlCommand(sSql, con);
        dr = cmd.ExecuteReader();

        gvCustomres.DataSource = dr;
        gvCustomres.DataBind();
        dr.Close();
        con.Close();
    }

    //LOAD EMPLOYEE IN DROPDOWN LIST
    protected void LoadDropDownList()
    {
        SqlConnection conn = DBConnection.GetConnection();

        String strQuery = "SELECT EmpName FROM PersonalInformation ORDER BY EmpName";
            
        //SqlConnection con = new SqlConnection("conn");
        SqlCommand cmd = new SqlCommand(strQuery, conn);
        //SqlCommand cmd = new SqlCommand();
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = strQuery;
        cmd.Connection = conn;

        try
        {
            conn.Open();
            ddlEmp.DataSource = cmd.ExecuteReader();
            ddlEmp.DataTextField = "EmpName";
            //ddlEmp.DataValueField = "ProductID";
            ddlEmp.DataValueField = "EmpName";
            ddlEmp.DataBind();
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

    protected void ddlEmp_SelectedIndexChanged(object sender, EventArgs e)
    {
        SqlConnection conn = DBConnection.GetConnection();

        string sSql = "";

        sSql = "";        
        sSql = "SELECT dbo.PersonalInformation.EmpName, dbo.PersonalInformation.JobID," +
            " dbo.PersonalInformation.Designation,dbo.Entity.eName" +
            " FROM  dbo.Entity RIGHT OUTER JOIN" +
            " dbo.PersonalInformation ON dbo.Entity.EID = dbo.PersonalInformation.EID" +
            " WHERE dbo.PersonalInformation.EmpName='" + this.ddlEmp.SelectedItem.Text + "'";
        SqlCommand cmd = new SqlCommand(sSql, conn);
        conn.Open();
        SqlDataReader dr = cmd.ExecuteReader();

        try
        {
            if (dr.Read())
            {
                this.txtEmpID.Text = dr["JobID"].ToString();
                this.txtDesg.Text = dr["Designation"].ToString();
                this.txtBr.Text = dr["eName"].ToString();                
            }
            else
            {
                this.txtEmpID.Text = "";
                this.txtDesg.Text = "";
                this.txtBr.Text = "";
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
            conn.Close();
        }
    }

    //Grid View Row Format
    protected void gvCustomres_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //CalcTotal(e.Row.Cells[3].Text);
            //CalcTotal1(e.Row.Cells[4].Text);

            //double value3 = Convert.ToDouble(e.Row.Cells[3].Text);
            //e.Row.Cells[3].Text = value3.ToString("0,0");

            //RIGHT ALIGNMENT            
            //e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[7].HorizontalAlign = HorizontalAlign.Center;
        }

    }


    protected void PopupMessage(string Msg, Control controlID)
    {
        ScriptManager.RegisterClientScriptBlock(controlID, controlID.GetType(), "msg", "alert('" + Msg + "');", true);
    }


    protected void btnSave_Click(object sender, EventArgs e)
    {
        string sSql = "";


        if (txtEmpID.Text == "")
        {
            PopupMessage("Please enter Employee ID #.", btnSave);
            txtEmpID.Focus();
            return;
        }

        if (ddlEmp.Text == "")
        {
            PopupMessage("Please enter Employee Name.", btnSave);
            ddlEmp.Focus();
            return;
        }

        if (txtBr.Text == "")
        {
            PopupMessage("Please Select Branch Name.", btnSave);
            txtBr.Focus();
            return;
        }

        if (txtUser.Text == "")
        {
            PopupMessage("Please Enter User Name.", btnSave);
            txtUser.Focus();
            return;
        }

        if (txtPass.Text == "")
        {
            PopupMessage("Please Enter User Password.", btnSave);
            txtPass.Focus();
            return;
        }

        if (txtPassConfirm.Text == "")
        {
            PopupMessage("Please Enter User Confirm Password.", btnSave);
            txtPassConfirm.Focus();
            return;
        }


        if (txtPassConfirm.Text != txtPass.Text)
        {
            PopupMessage("Password Not Match ...", btnSave);
            txtPassConfirm.Focus();
            return;
        }

        SqlConnection conn = DBConnection.GetConnection();

        //----------------------------------------------------------------------
        //CHECK DUPLICATE USER.
        sSql = "";
        sSql = "SELECT UserName FROM SoftUser" +
            " WHERE UserName='" + this.txtUser.Text + "'";
        SqlCommand cmdd = new SqlCommand(sSql, conn);
        conn.Open();
        SqlDataReader drd = cmdd.ExecuteReader();
        try
        {
            if (drd.Read())
            {
                //iMRSRID = Convert.ToInt32(dr["MRSRMID"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert",
                            "<script>alert('" + "This User already created." + "');</script>", false);
                txtEmpID.Focus();
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


        //SAVE DATA
        sSql = "";
        sSql = "INSERT INTO SoftUser(EmpID,FullName,UserName," +
               "Passward,ConfirmPass,BranchCode,eName,UserType,EntryDate)" +
                     " Values ('" + this.txtEmpID.Text + "'," +
                     " '" + this.ddlEmp.Text + "'," +
                     " '" + this.txtUser.Text + "'," +
                     " '" + this.txtPass.Text + "'," +
                     " '" + this.txtPassConfirm.Text + "'," +
                     " '" + this.txtBrCode.Text + "'," +
                     " '" + this.txtBr.Text + "'," +
                     " '" + this.ddlUserType.Text + "'," +
                     " '" + DateTime.Today + "'" +
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
        txtEmpID.Text = "";
        txtDesg.Text = "";
        txtBr.Text = "";

        txtUser.Text = "";
        txtPass.Text = "";
        txtPassConfirm.Text = "";
        

        ddlEmp.Focus();
        //------------------------------------------------------------------------------------------

        //LOAD DATA IN GRIDVIEW
        fnLoadData();

        return;
    }


    protected void btnCancel_Click(object sender, EventArgs e)
    {
        //
        //CLEAR ALL TEXT
        txtEmpID.Text = "";
        txtDesg.Text = "";
        txtBr.Text = "";

        txtUser.Text = "";
        txtPass.Text = "";
        txtPassConfirm.Text = "";


        ddlEmp.Focus();
    }

    //GRID ROW DELETE
    protected void gvCustomres_RowDelating(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            SqlConnection conn = DBConnection.GetConnection();
            //int index = gvCustomres.SelectedIndex;

            /*
            string sJobNo = Convert.ToString(gvCustomres.DataKeys[e.RowIndex].Values["UserName"].ToString());
            SqlCommand com = new SqlCommand("DELETE FROM SoftUser WHERE UserName = '" + sJobNo + "'", conn);
            conn.Open();
            com.ExecuteNonQuery();
            conn.Close();
            */

            //LOAD DATA
            fnLoadData();

        }
        catch (InvalidCastException err)
        {
            throw (err);
        }

    }

    protected void BindGrid()
    {
        gvCustomres.DataSource = ViewState["dt"] as DataTable;
        gvCustomres.DataBind();
    }

    protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string item = e.Row.Cells[0].Text;
            foreach (Button button in e.Row.Cells[2].Controls.OfType<Button>())
            {
                if (button.CommandName == "Delete")
                {
                    button.Attributes["onclick"] = "if(!confirm('Do you want to delete " + item + "?')){ return false; };";
                }
            }
        }
    }


    protected void ddlUserType_SelectedIndexChanged(object sender, EventArgs e)
    {
        SqlConnection con = DBConnection.GetConnection();
        con.Open();

        string sSql = "";
        sSql = "SELECT dbo.SoftUser.FullName, dbo.SoftUser.Designation, dbo.SoftUser.UserName," +
            " dbo.SoftUser.Passward, dbo.SoftUser.WebAccess, " +
            " dbo.SoftUser.UserType, dbo.SoftUser.eName, dbo.SoftUser.Active" +
            " FROM dbo.SoftUser LEFT OUTER JOIN" +
            " dbo.Entity ON dbo.SoftUser.EID = dbo.Entity.EID" +

            " WHERE dbo.SoftUser.WebAccessType='" + ddlUserType.SelectedItem.Text  + "'" +

            " ORDER BY dbo.SoftUser.FullName";

        SqlCommand cmd = new SqlCommand(sSql, con);
        dr = cmd.ExecuteReader();

        gvCustomres.DataSource = dr;
        gvCustomres.DataBind();
        dr.Close();
        con.Close();
    }


}