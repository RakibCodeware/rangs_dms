

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System.Globalization;


public partial class DealerReports_PermisssionEntryPanel : System.Web.UI.Page
{
    SqlConnection conn = DBConnection.GetConnection();

    DataTable dt;




    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
           
            gettreeView();
            getUser();
            UserFill();
          

        }
    }




    //connection dbcid database
    public void getConnection()
    {
        if (conn.State != null)
            conn.Close();
        conn.Open();
    }


    //get zone name like tree view start

    public void gettreeView()
    {
        DataTable dt = new DataTable();
        string query = "SELECT ZoneId, ZoneName, ParentZoneId FROM DealerOrganogram ";
        using (conn)
        {
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                getConnection();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                conn.Close();
            }
        }
        PopulateTreeView(int.Parse(dt.Rows[0]["ParentZoneId"].ToString()), null, dt);
    }

    //tree view data

    public void PopulateTreeView(int parentId, TreeNode parentNode, DataTable dt)
    {
        DataRow[] rows = dt.Select(string.Format("ParentZoneId = {0}", parentId));
        foreach (DataRow row in rows)
        {
            TreeNode node = new TreeNode();
            node.Text = row["ZoneName"].ToString();
            node.Value = row["ZoneId"].ToString();
            if (parentNode == null)
            {
                treeview.Nodes.Add(node);
            }
            else
            {
                parentNode.ChildNodes.Add(node);
            }
            PopulateTreeView(Convert.ToInt32(row["ZoneId"]), node, dt);
        }

    }




    public static List<string> getCheckdValues(TreeNodeCollection nodes)
    {
        try
        {

            List<string> values = new List<string>();
            foreach (TreeNode tn in nodes)
            {
                if (tn.Checked)
                {
                    if (values.Count == 0)
                        HttpContext.Current.Session["__ZoneNameForReport__"] = tn.Text.Trim();
                    values.Add(tn.Value);

                }
                if (tn.ChildNodes.Count > 0)
                    getChildValues(tn, values);
            }
            return values;
        }
        catch (Exception ex) { return null; }

    }


    private static void getChildValues(TreeNode tn, List<string> values)
    {

        foreach (TreeNode cn in tn.ChildNodes)
        {
            if (cn.Checked)
            {
                if (values.Count == 0)
                    HttpContext.Current.Session["__ZoneNameForReport__"] = cn.Text.Trim();
                values.Add(cn.Value);


            }
            if (cn.ChildNodes.Count > 0)
                getChildValues(cn, values);
        }
    }

  private void UserFill()
    {
        ddlUserName.DataSource = getUser();
        ddlUserName.DataTextField = "UserName";
        ddlUserName.DataValueField = "ID";
        ddlUserName.DataBind();
        ddlUserName.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", "-1"));

    }

    public object getUser()
    {
        SqlConnection conn = DBConnection.GetConnection();
        conn.Open();
        string sqlqury = "select ID,UserName from SoftUser where WebAccessType='DealerReports' order by UserName asc";
        SqlCommand cmd = new SqlCommand(sqlqury, conn);
        SqlDataAdapter adaptr = new SqlDataAdapter(cmd);
        DataTable dte = new DataTable();
        adaptr.Fill(dte);
        return dte;
        conn.Close();
    }

    protected void btn_save_Click(object sender, EventArgs e)
     {
        List<string> myList = getCheckdValues(treeview.Nodes);
        string zoneIds = string.Join(",", myList.ToArray());
        string getpermitZoneId = GetPermissionZoneID().ToString();
        if (conn.State != null)
            conn.Close();
        conn.Open();

        

        //string sqlquery = "Delete  DealerRole Where UserId='" + ddlUserName.SelectedValue + "'";
        string sqlquery = @"Delete  DealerRole Where UserId='" + ddlUserName.SelectedValue + 
            "';Insert into DealerRole(UserId,PermissionZone) values('" + ddlUserName.SelectedValue + "','" + zoneIds + "')";
        SqlCommand cmd = new SqlCommand(sqlquery, conn);
        int result = int.Parse(cmd.ExecuteNonQuery().ToString());
        conn.Close();
        if (result > 0)
        {
           // Response.Write("Data Saved Succesfully");
          

            Response.Redirect(Request.Url.AbsoluteUri);
        }
        else
        {
            
            Response.Write("Data Saved Failed");
            Response.Redirect(Request.Url.AbsoluteUri);
        }
       

    }




    public string getUserName(string username)
    {
        string UserId = "";
        string query = "SELECT Id FROM Softuser WHERE userId = '" + username + "'";
        using (SqlConnection conn = DBConnection.GetConnection())
        {
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string Id = reader.GetString(0);
                        UserId = Id;
                    }
                }
            }
        }

        return UserId;
    }


   private string getPermissionzone(int UserId)
    {

        string permissionId = "";
        using (SqlConnection conn = DBConnection.GetConnection())
        {

            string query = "select PermissionZone from DealerRole  where UserId='" + UserId + "'";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        permissionId = reader["PermissionZone"].ToString();


                    }
                }
            }
            return permissionId;
        }

    }

   private string GetPermissionZoneID()
   {

       string permissionZoneID = "";
       using (SqlConnection conn = DBConnection.GetConnection())
       {
           string query = "select PermissionZone from DealerRole  where UserId='" + ddlUserName.SelectedValue + "'";
 
           using (SqlCommand cmd = new SqlCommand(query, conn))
           {
               try
               {
                   conn.Open();
                   using (SqlDataReader reader = cmd.ExecuteReader())
                   {
                       if (reader.Read())
                       {
                           permissionZoneID = reader["PermissionZone"].ToString();
                       }
                       reader.Close();
                   }
               }
               catch (Exception ex)
               {

               }


           }
           return permissionZoneID;
           
       }

   }
   protected void ddlUserName_SelectedIndexChanged(object sender, EventArgs e)
   {
       string permissionZoneID = GetPermissionZoneID().ToString();
       if (permissionZoneID != "")
           FindNodeByValue(treeview.Nodes, permissionZoneID
               .Split(',')
               .ToArray());
       else
           CheckUncheckNodes(treeview.Nodes,false);
     
   }
   private void FindNodeByValue(TreeNodeCollection nodes, string[] values)
   {
       foreach (TreeNode node in nodes)
       {
           if (values.Contains(node.Value))
           {
               node.Checked = true;
           }
           else
               node.Checked = false;
           FindNodeByValue(node.ChildNodes, values);
           
       }

       
   }

   private void CheckUncheckNodes(TreeNodeCollection nodes, bool value)
   {
       foreach (TreeNode node in nodes)
       {
           node.Checked = value;
           CheckUncheckNodes(node.ChildNodes, value);

       }


   }

   


}

  

       


   

