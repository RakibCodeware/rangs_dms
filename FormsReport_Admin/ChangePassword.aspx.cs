using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Globalization;
using System.Drawing;
using System.IO;
using System.Data;
using System.Configuration;

public partial class Forms_ChangePassword : System.Web.UI.Page
{
    SqlConnection conn = DBConnection.GetConnection();
    SqlConnection conn1 = DBConnection.GetConnection();
    SqlConnection _connStr = DBConnection.GetConnection();
    int iMRSRID = 0;
    DataTable dt;
    DateTime tDate;

    protected void Page_Load(object sender, EventArgs e)
    {
        btnSave.Attributes.Add("OnClick", "return confirm_ChangePW();");
        btnCancel.Attributes.Add("OnClick", "return confirm_Cancel();");

        if (!IsPostBack)
        {
            if (System.Convert.ToInt32(Session["Vis"]) == 0)
            {
                Response.Redirect("../Account/Login.aspx");
            }
            txtUser.Text = Session["UserName"].ToString();
        }
    }

    protected void PopupMessage(string Msg, Control controlID)
    {
        ScriptManager.RegisterClientScriptBlock(controlID, controlID.GetType(), "msg", "alert('" + Msg + "');", true);

    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        string sSql = "";

        if (txtOldPass.Text == "")
        {
            PopupMessage("Please enter your old password.", btnSave);
            txtOldPass.Focus();
            return;
        }

        if (txtNewPass.Text == "")
        {
            PopupMessage("Please enter your new password.", btnSave);
            txtNewPass.Focus();
            return;
        }

        if (txtConfirmPass.Text == "")
        {
            PopupMessage("Please enter your confirm new password.", btnSave);
            txtConfirmPass.Focus();
            return;
        }

        if (txtNewPass.Text.Equals(txtNewPass.Text, StringComparison.Ordinal))
        {
            lblPasswordMatch.Visible = true;
            lblPasswordMatch.Text = "Password ok";
        }
        else
        {
            lblPasswordMatch.Visible = true;
            lblPasswordMatch.Text = "Password not match";
            return;
        }

        //----------------------------------------------------------------------
        //CHECK OLD PASSWORD.
        sSql = "";
        sSql = "SELECT * FROM SoftUser" +
            " WHERE UserName='" + this.txtUser.Text + "'" +
            " AND Passward='" + this.txtOldPass.Text + "'";
        SqlCommand cmdd = new SqlCommand(sSql, conn);
        conn.Open();
        SqlDataReader drd = cmdd.ExecuteReader();
        try
        {
            if (drd.Read())
            {                
                
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert",
                            "<script>alert('" + "Your old password is not correct." + "');</script>", false);
                txtOldPass.Focus();
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


        //SAVE DATA IN TABLE
        sSql = "";        
        sSql="UPDATE SoftUser SET Passward='" + this.txtConfirmPass.Text + "'" +
            " WHERE UserName='" + Session["UserName"] + "'";
        SqlCommand cmd = new SqlCommand(sSql, conn);
        conn.Open();
        cmd.ExecuteNonQuery();
        conn.Close();


        ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert",
                        "<script>alert('" + "Password Change Successfully." + "');</script>", false);

        Session["sBrId"] = 0;
        Session["sBr"] = "";
        Session["UserName"] = "";

        Response.Redirect("~/Account/LogOut.aspx");


    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        txtOldPass.Text = "";
        txtNewPass.Text = "";
        txtConfirmPass.Text = "";
        txtOldPass.Focus();
    }
    

}