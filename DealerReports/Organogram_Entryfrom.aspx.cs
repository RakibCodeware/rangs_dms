using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

public partial class DealerReports_Organogram_Entryfrom : System.Web.UI.Page
{

    SqlConnection conn = DBConnection.GetConnection();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            filldrop();
            GridViewGetData();
            gettreeView();
        }
    }

    private void GridViewGetData()
    {
        //if (conn.State != null)
        //    conn.Close();
        //conn.Open();
        getConnection();
        string gridviewQuery = "Select z.ZoneId,z.ZoneName,IsNull(p.ZoneName,'No Parent') as ParentName,z.Note from DealerOrganogram z left join DealerOrganogram  p on z.ParentZoneId=p.ZoneId ";

        SqlCommand cd = new SqlCommand(gridviewQuery, conn);

        gridZone.DataSource = cd.ExecuteReader();
        gridZone.DataBind();
        conn.Close();

    }



    protected void Savebutton_Click(object sender, EventArgs e)
    {
        if (Savebutton.Text == "Save")
        {

            if (conn.State != null)
                conn.Close();
            conn.Open();
            string sqlquery = "Insert into DealerOrganogram(ZoneName,ParentZoneId,Note)Values('" + ZoneName.Text + "','" + Parent_zone.SelectedValue + "','" + Note.Text + "')";
            SqlCommand cmd = new SqlCommand(sqlquery, conn);
           int result=int.Parse( cmd.ExecuteNonQuery().ToString());
            conn.Close();
            if (result > 0)
            {
                Response.Write("Data Saved Succesfully");
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else {
                Response.Write("Data Saved Failed");
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            
           
        }
        else
        {
            ViewState["__ZoneId__"].ToString();
            //if (conn.State != null)
            //    conn.Close();
            //conn.Open();
            getConnection();
            string sqlquery = "update DealerOrganogram set ZoneName='" + ZoneName.Text.Trim() + "',ParentZoneId='" + Parent_zone.SelectedValue + "', Note='" + Note.Text + "' where ZoneId="+ViewState["__ZoneId__"].ToString();
            SqlCommand cmd = new SqlCommand(sqlquery, conn);
           int result=int.Parse( cmd.ExecuteNonQuery().ToString());
            conn.Close();
            if (result > 0)
            {
                Savebutton.Text = "Save";
                Response.Write("Data Saved");
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else 
              {
                  Response.Write("Data Updated Faild");
                  Response.Redirect(Request.Url.AbsoluteUri);
              }
            
        }
    }
    private void filldrop()
    {
        Parent_zone.DataSource = getZoneData();
        Parent_zone.DataTextField = "ZoneName";
        Parent_zone.DataValueField = "ZoneId";
        Parent_zone.DataBind();
        Parent_zone.Items.Insert(0, new ListItem("--Select--", "-1"));
        Parent_zone.Items.Insert(1, new ListItem("No Parent", "0"));
    }

    public object getZoneData()
    {
        //if (conn.State != null)
        //    conn.Close();
        //conn.Open();
        getConnection();
        string sqlquery = "Select ZoneName,ZoneId from DealerOrganogram ";
        SqlCommand cmd = new SqlCommand(sqlquery, conn);
        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
        DataTable dt = new DataTable();
        adapter.Fill(dt);
        return dt;
        conn.Close();

    }




    protected void Parent_zone_SelectedIndexChanged(object sender, EventArgs e)
    {

    }



    protected void gridZone_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        int rIndex = int.Parse(e.CommandArgument.ToString());
        if (e.CommandName == "change")
        {
            string zoneId = gridZone.DataKeys[rIndex].Values[0].ToString();



            //if (conn.State != null)
            //    conn.Close();
            //conn.Open();
            getConnection(); 
            string sqlquery = "Select * from DealerOrganogram where ZoneId=" + zoneId.ToString();
            SqlCommand cmd = new SqlCommand(sqlquery, conn);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            conn.Close();
            adapter.Fill(dt);
            conn.Close();

            if (dt != null && dt.Rows.Count > 0)
            {
                Parent_zone.SelectedValue = dt.Rows[0]["ParentZoneId"].ToString();
                ZoneName.Text = dt.Rows[0]["ZoneName"].ToString();
                Note.Text = dt.Rows[0]["Note"].ToString();
                Savebutton.Text = "Update";
                ViewState["__ZoneId__"] = dt.Rows[0]["ZoneId"].ToString();
            }

        }
        if (e.CommandName == "remove")
        {
            string dealerId = gridZone.DataKeys[rIndex].Values[0].ToString();
            //if (conn.State != null)
            //    conn.Close();
            //conn.Open();
            getConnection();
            string gridviewQuery = "Delete from DealerOrganogram where ZoneId= " + dealerId;
            SqlCommand cd = new SqlCommand(gridviewQuery, conn);
            int row = cd.ExecuteNonQuery();
            if (row > 0)
            {
                getZoneData();
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            conn.Close();
        }


    }

    public void getConnection() 
        {
            if (conn.State != null)
                conn.Close();
            conn.Open();
        }



    //Tree view organogram

    public void gettreeView() 
    {
        DataTable dt = new DataTable();
        string query = "SELECT ZoneId, ZoneName, ParentZoneId FROM DealerOrganogram";
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
        PopulateTreeView(0, null, dt);
    }

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
                TreeView1.Nodes.Add(node);
            }
            else
            {
                parentNode.ChildNodes.Add(node);
            }
            PopulateTreeView(Convert.ToInt32(row["ZoneId"]), node, dt);
        }

    }  

 
}
