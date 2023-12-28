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

public partial class Admin_Forms_Employee_Info : System.Web.UI.Page
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

    //LOAD BRANCH IN DROPDOWN LIST
    protected void LoadDropDownList()
    {
        SqlConnection conn = DBConnection.GetConnection();
                
        string gSql = "";
        gSql="SELECT EID, eName";
        gSql=gSql + " FROM dbo.Entity";
        gSql=gSql + " WHERE (EntityType = 'Showroom' OR";
        gSql=gSql + " EntityType = 'Dealer' OR EntityType = 'Sub Store')";
        gSql = gSql + " AND (ActiveDeactive = 1)";

        //SqlConnection con = new SqlConnection("conn");
        SqlCommand cmd = new SqlCommand(gSql, conn);
        //SqlCommand cmd = new SqlCommand();
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = gSql;
        cmd.Connection = conn;
        
        try
        {
            conn.Open();
            ddlBranch1.DataSource = cmd.ExecuteReader();
            ddlBranch1.DataTextField = "eName";
            //ddlBranch.DataValueField = "ProductID";
            ddlBranch1.DataValueField = "EID";
            ddlBranch1.DataBind();
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

    protected void PopupMessage(string Msg, Control controlID)
    {
        ScriptManager.RegisterClientScriptBlock(controlID, controlID.GetType(), "msg", "alert('" + Msg + "');", true);
    }

   
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        txtProdID.Text = "0";
        txtJobID.Text = "";
        txtEmpName.Text = "";
        txtAdd.Text = "";
        txtContactNo.Text = "";
        txtDesg.Text = "";
        txtDept.Text = "";

        ddlBranch1.Text = "";
        txtBrCode.Text = "";
        txtBrAdd.Text = "";
        txtEID.Text = "";

        txtJobID.Focus();
    }

    //GRID ROW DELETE
    protected void gvCustomres_RowDelating(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            SqlConnection conn = DBConnection.GetConnection();
            //int index = gvCustomres.SelectedIndex;

            string sJobNo = Convert.ToString(gvCustomres.DataKeys[e.RowIndex].Values["JobID"].ToString());
            SqlCommand com = new SqlCommand("DELETE FROM PersonalInformation WHERE JobID = '" + sJobNo + "'", conn);
            conn.Open();
            com.ExecuteNonQuery();
            conn.Close();

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

    protected void btnSave_Click(object sender, EventArgs e)
    {
        string sSql = "";


        if (txtJobID.Text == "")
        {
            PopupMessage("Please enter Employee Job #.", btnSave);
            txtJobID.Focus();
            return;
        }

        if (txtEmpName.Text == "")
        {
            PopupMessage("Please enter Employee Name.", btnSave);
            txtEmpName.Focus();
            return;
        }

        if (txtContactNo.Text == "")
        {
            PopupMessage("Please enter Employee Contact #.", btnSave);
            txtContactNo.Focus();
            return;
        }

        SqlConnection conn = DBConnection.GetConnection();

        //----------------------------------------------------------------------
        //CHECK DUPLICATE JOB NO.
        sSql = "";
        sSql = "SELECT EmpID FROM PersonalInformation" +
            " WHERE JobID='" + this.txtJobID.Text + "'";
        SqlCommand cmdd = new SqlCommand(sSql, conn);
        conn.Open();
        SqlDataReader drd = cmdd.ExecuteReader();
        try
        {
            if (drd.Read())
            {
                //iMRSRID = Convert.ToInt32(dr["MRSRMID"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert",
                            "<script>alert('" + "This Job # already exists." + "');</script>", false);
                txtJobID.Focus();
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
        sSql = "INSERT INTO PersonalInformation(JobID,EmpName,eAddress," +
               "ContactNo,Designation,Department,EID,EntryDate)" +
                     " Values ('" + this.txtJobID.Text + "'," +
                     " '" + this.txtEmpName.Text + "'," +
                     " '" + this.txtAdd.Text + "'," +
                     " '" + this.txtContactNo.Text + "'," +
                     " '" + this.txtDesg.Text + "'," +
                     " '" + this.txtDept.Text + "'," +
                     " '" + this.txtBrCode.Text + "'," +                    
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
        txtJobID.Text = "";
        txtEmpName.Text = "";
        txtAdd.Text = "";
        txtContactNo.Text = "";
        txtDesg.Text = "";
        txtDept.Text = "";

        ddlBranch1.Text = "";
        txtBrCode.Text = "";
        txtBrAdd.Text = "";
        txtEID.Text = "";

        txtJobID.Focus();
        //------------------------------------------------------------------------------------------

        //LOAD DATA IN GRIDVIEW
        fnLoadData();

        return;
    }

    protected void fnLoadData()
    {

        SqlConnection con = DBConnection.GetConnection();
        con.Open();

        string sSql = ""; 
        sSql = "SELECT dbo.PersonalInformation.JobID, dbo.PersonalInformation.EmpName," +
            " dbo.PersonalInformation.eAddress, dbo.PersonalInformation.ContactNo, " +
            " dbo.PersonalInformation.Designation, dbo.PersonalInformation.Department," +
            " dbo.Entity.eName" +
            " FROM dbo.PersonalInformation LEFT OUTER JOIN" +
            " dbo.Entity ON dbo.PersonalInformation.EID = dbo.Entity.EID";

        SqlCommand cmd = new SqlCommand(sSql, con);
        dr = cmd.ExecuteReader();

        gvCustomres.DataSource = dr;
        gvCustomres.DataBind();
        dr.Close();
        con.Close();
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
            e.Row.Cells[8].HorizontalAlign = HorizontalAlign.Center;
        }

    }

    protected void ddlBranch1_SelectedIndexChanged(object sender, EventArgs e)
    {
        SqlConnection conn = DBConnection.GetConnection();

        string sSql = "";

        sSql = "";
        sSql = "SELECT EID, eName, EntityCode, EDesc FROM dbo.Entity" +
            " WHERE eName='" + this.ddlBranch1.SelectedItem.Text + "'";
        SqlCommand cmd = new SqlCommand(sSql, conn);
        conn.Open();
        SqlDataReader dr = cmd.ExecuteReader();

        try
        {
            if (dr.Read())
            {
                this.txtBrCode.Text = dr["EntityCode"].ToString();
                this.txtBrAdd.Text = dr["EDesc"].ToString();
                this.txtEID.Text = dr["EID"].ToString();
            }
            else
            {
                this.txtBrCode.Text = "";
                this.txtBrAdd.Text = "";
                this.txtEID.Text = "";
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

}