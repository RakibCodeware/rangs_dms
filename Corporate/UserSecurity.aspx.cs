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

public partial class Admin_Forms_UserSecurity : System.Web.UI.Page
{

    SqlConnection conn = DBConnection.GetConnection();
    //SqlConnection conn1 = DBConnection.GetConnection();
    //SqlConnection _connStr = DBConnection.GetConnection();
    SqlDataReader dr;

    protected void Page_Load(object sender, EventArgs e)
    {
        //TreeView1.Attributes.Add("onclick", "postBackByObject()");
        btnSave.Attributes.Add("OnClick", "return confirm_Save();");
        btnCancel.Attributes.Add("OnClick", "return confirm_Cancel();");

        if (!IsPostBack)
        {            
            LoadDropDownList();
        }

    }

    //LOAD EMPLOYEE IN DROPDOWN LIST
    protected void LoadDropDownList()
    {
        SqlConnection conn = DBConnection.GetConnection();

        String strQuery = "SELECT UserName FROM SoftUser ORDER BY UserName";

        //SqlConnection con = new SqlConnection("conn");
        SqlCommand cmd = new SqlCommand(strQuery, conn);
        //SqlCommand cmd = new SqlCommand();
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = strQuery;
        cmd.Connection = conn;
        
        try
        {
            conn.Open();
            ddlUser.DataSource = cmd.ExecuteReader();
            ddlUser.DataTextField = "UserName";
            //ddlEmp.DataValueField = "ProductID";
            ddlUser.DataValueField = "UserName";
            ddlUser.DataBind();

            //Add blank item at index 0.
            ddlUser.Items.Insert(0, new ListItem("", ""));
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

    protected void fnLoadUserData()
    {

        SqlConnection con = DBConnection.GetConnection();
        con.Open();

        //FOR CLEAR TREEVIEW
        foreach (TreeNode tn in TreeView1.Nodes)
        {            
            tn.Checked = false;            
            foreach (TreeNode child in tn.ChildNodes)
            {
                child.Checked = false;                
            }
        }


        string sSql = "";       
        sSql = "SELECT dbo.tbMenus.MenuId, dbo.tbMenus.MenuCode, dbo.tbMenus.Title, dbo.tbMenus_User.UserName" +
            " FROM dbo.tbMenus INNER JOIN" +
            " dbo.tbMenus_User ON dbo.tbMenus.MenuCode = dbo.tbMenus_User.MenuCode" +
            " WHERE dbo.tbMenus_User.UserName='" + ddlUser.SelectedItem.Text   + "'" +
            " ORDER BY dbo.tbMenus.MenuId";

        SqlCommand cmd = new SqlCommand(sSql, con);
        //dr = cmd.ExecuteReader();

        SqlDataAdapter da = new SqlDataAdapter(cmd); 
        DataTable dt = new DataTable(); 
        da.Fill(dt); 
        
        foreach (DataRow dr in dt.Rows)
        {
            foreach (TreeNode tn in TreeView1.Nodes)
            {
                string strTreeValue;
                strTreeValue = tn.Value;
                if (strTreeValue == dr["MenuCode"].ToString())
                {
                    tn.Checked = true;
                }
                foreach (TreeNode child in tn.ChildNodes)
                {
                    if (child.Value == dr["MenuCode"].ToString())
                    {
                        child.Checked = true;
                    }
                }                
            }
        }   
     
        //dr.Close();
        con.Close();

    }

    
    protected void TreeView1_TreeNodeCheckChanged(object sender, TreeNodeEventArgs e)
    {
        //if (e.Node.Text == "New Node 1")
        //{
        //    ClientScript.RegisterStartupScript(GetType(), "alertme", "alert('The first node is selected...');", true);
        //}

        ////if ((sender as TreeView).SelectedNode.Checked == true)
        ////    foreach (TreeNode tr in (sender as TreeView).SelectedNode.ChildNodes)
        ////    {
        ////        foreach (TreeNode child in tr.ChildNodes)
        ////            child.Checked = true;
        ////        tr.Checked = true;
        ////    }
        ////else
        ////    foreach (TreeNode tr in (sender as TreeView).SelectedNode.ChildNodes)
        ////    {
        ////        foreach (TreeNode child in tr.ChildNodes)
        ////            child.Checked = false;
        ////        tr.Checked = false;
        ////    }

    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        //

        SqlConnection con = DBConnection.GetConnection();

        //-----------------------------------------------------------------
        //DELETE PREVIOUS SAME DATA
        string sSqlDel = "";
        sSqlDel = "DELETE FROM tbMenus_User";
        sSqlDel = sSqlDel + " WHERE UserName='" + this.ddlUser.SelectedItem.Text + "'";
        SqlCommand cmd = new SqlCommand(sSqlDel, conn);
        conn.Open();
        cmd.ExecuteNonQuery();
        conn.Close();
        //-----------------------------------------------------------------

        //SAVE DATA
        if (TreeView1.CheckedNodes.Count > 0)
        {
            foreach (TreeNode tn in TreeView1.CheckedNodes)
            {
                if (tn.Checked)
                {                    
                    String sSql = "";
                    sSql = "INSERT INTO tbMenus_User(UserName,Title,MenuCode,EntryDate)" +
                                 " Values ('" + this.ddlUser.SelectedItem.Text  + "'," +
                                 " '" + tn.Text  + "'," +
                                 " '" + tn.Value + "'," +                                 
                                 " '" + DateTime.Today + "'" +
                                 " )";
                    cmd = new SqlCommand(sSql, conn);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();

                }
            }
        }

        //-----------------------------------------------------------------
        //CLEAR ALL TEXT & OTHERS
        this.ddlUser.SelectedItem.Text  = "";
        this.txtFullName.Text = "";
        this.txtJobID.Text = "";
        this.txtDesg.Text = "";
        this.txtBranch.Text = "";
        this.txtBrID.Text = "";
        //foreach (TreeNode tn in TreeView1.CheckedNodes)
        //{
        //    tn.Checked = false;
        //}
        foreach (TreeNode tn in TreeView1.Nodes)
        {            
            tn.Checked = false;            
            foreach (TreeNode child in tn.ChildNodes)
            {
                child.Checked = false;                
            }
        }
        //-----------------------------------------------------------------

    }
        

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        //
    }

    protected void ddlUser_SelectedIndexChanged(object sender, EventArgs e)
    {
        SqlConnection conn = DBConnection.GetConnection();

        string sSql = "";

        sSql = "";
        //sSql = "SELECT BranchCode,BranchName FROM tbBranchInfo" +
        //" WHERE BranchName='" + this.ddlEmp.SelectedItem.Text + "'";
        sSql = "SELECT * FROM SoftUser" +
            " WHERE UserName='" + this.ddlUser.SelectedItem.Text + "'";
        SqlCommand cmd = new SqlCommand(sSql, conn);
        conn.Open();
        SqlDataReader dr = cmd.ExecuteReader();

        try
        {
            if (dr.Read())
            {
                this.txtFullName.Text = dr["FullName"].ToString();
                this.txtJobID.Text = dr["UserID"].ToString();
                this.txtDesg.Text = dr["Designation"].ToString();
                this.txtBranch.Text = dr["eName"].ToString();
                this.txtBrID.Text = dr["EID"].ToString();
            }
            else
            {
                this.txtFullName.Text = "";
                this.txtJobID.Text = "";
                this.txtDesg.Text = "";
                this.txtBranch.Text = "";
                this.txtBrID.Text = "";
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

        //LOAD SECURITY PROFILE
        fnLoadUserData();

    }


}