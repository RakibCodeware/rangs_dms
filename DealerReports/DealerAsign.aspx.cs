using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;



public partial class DealerReports_DealerAsign : System.Web.UI.Page
{
    SqlConnection conn = DBConnection.GetConnection();
    SqlConnection conn1 = DBConnectionDSM.GetConnection();

    protected void Page_Load(object sender, EventArgs e)
    {
      

        if (!IsPostBack)
         {
             Zonefill();
            Dealerfill();
            getDealerName();
            gridViewGetData();
         }

    }


    //ZoneNmae form dbcid DataBase

    private void Zonefill()
    {
        ddl_ZoneName.DataSource = getZoneName();
        ddl_ZoneName.DataTextField = "ZoneName";
        ddl_ZoneName.DataValueField = "ZoneId";
        ddl_ZoneName.DataBind();
        ddl_ZoneName.Items.Insert(0, new ListItem("--Select--", "-1"));
        ddl_ZoneName.Items.Insert(1, new ListItem("No Parent", "0"));
 
     }


 public object getZoneName() 
    {

        if (conn.State != null)
            conn.Close();
        conn.Open();
        string sqlquery = "Select ZoneId,ZoneName from DealerOrganogram d left join ZoneWisDealer zd on d.ZoneId=zd.Zone";
        SqlCommand cmd = new SqlCommand(sqlquery, conn);
        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
        DataTable dt = new DataTable();
        adapter.Fill(dt);
        return dt;
        conn.Close();

    }


    // dealer Name from delear sales database

 private void Dealerfill()
 {
     ddl_dealerName.DataSource = getDealerName();
     ddl_dealerName.DataTextField = "Name";
     ddl_dealerName.DataValueField="DAID";
     ddl_dealerName.DataBind();
     ddl_dealerName.Items.Insert(0, new ListItem("--Select--", "-1"));
   
}

    public object getDealerName() 
    {
        if (conn.State != null)
            conn.Close();
        conn.Open();
        string sqlqury = "select DAID,Name+' ['+Code+']' as Name from DelearSales.dbo.DelearInfo  df left join ZoneWisDealer zd on df.DAID=zd.Dealer ";
        SqlCommand cmd = new SqlCommand(sqlqury, conn);
        SqlDataAdapter adaptr = new SqlDataAdapter(cmd);
        DataTable dte = new DataTable();
        adaptr.Fill(dte);
        return dte;
        conn.Close();
    }

    //data save into Dbcid Database ZoneWiseDealer table

    protected void btn_save_Click(object sender, EventArgs e)
    {
        if (btn_save.Text == "Save")
         {
            conn.Open();
            string checkQuey = "Select Dealer from ZoneWisDealer where Dealer=" + ddl_dealerName.SelectedValue + " ";
            SqlCommand cmd1 = new SqlCommand(checkQuey, conn);
            SqlDataAdapter sd = new SqlDataAdapter(cmd1);

            DataTable dt = new DataTable();
            sd.Fill(dt);
           if (dt.Rows.Count > 0)
            {
                lbl_message.Text = "Duplicate Data Not Alowed";
            }
            else {

                string sqlquery = "Insert into ZoneWisDealer(Zone,Dealer) Values('" + ddl_ZoneName.SelectedValue + "','" + ddl_dealerName.SelectedValue + "')";
                SqlCommand cmd = new SqlCommand(sqlquery, conn);
                int result = int.Parse(cmd.ExecuteNonQuery().ToString());

                conn.Close();
                if (result > 0)
                {

                    Response.Redirect(Request.Url.AbsoluteUri);
                }
                else
                {
                    Response.Redirect(Request.Url.AbsoluteUri);
                }
            }


        }
        else 
           {
               ViewState["__DealerId__"].ToString();
               if (conn.State != null)
                   conn.Close();
               conn.Open();

               string checkQuey = "Select Dealer from ZoneWisDealer where Dealer=" + ddl_dealerName.SelectedValue + " ";
               SqlCommand cmd1 = new SqlCommand(checkQuey, conn);
               SqlDataAdapter sd = new SqlDataAdapter(cmd1);

               DataTable dt = new DataTable();
               sd.Fill(dt);
               if (dt.Rows.Count > 0)
               {
                   lbl_message.Text = "Duplicate Data Not Alowed";
               }
               else 
                {
                    string updatQuery = ("update ZoneWisDealer set Zone='" + ddl_ZoneName.SelectedValue + "', Dealer='" + ddl_dealerName.SelectedValue + "' where Id=" + ViewState["__DealerId__"].ToString());
                    SqlCommand cmd = new SqlCommand(updatQuery, conn);
                    int result = int.Parse(cmd.ExecuteNonQuery().ToString());

                    conn.Close();
                    if (result > 0)
                    {
                        btn_save.Text = "Save";

                        Response.Redirect(Request.Url.AbsoluteUri);
                    }
                    else
                    {

                        Response.Redirect(Request.Url.AbsoluteUri);
                    }
                }
              
           }
           


    }


    private void gridViewGetData()  
        {
            if (conn.State != null)
                conn.Close();
            conn.Open();
            string gridviewQuery = "Select zd.Id,do.ZoneName as Zone,df.Name as Dealer from ZoneWisDealer zd left join DelearSales.dbo.DelearInfo df on zd.Dealer=df.DAID left join DealerOrganogram do on do.ZoneId=zd.Zone order by zd.Id ";
             
            SqlCommand cd = new SqlCommand(gridviewQuery, conn);

            gridView_Zonewisedealer.DataSource = cd.ExecuteReader();
            gridView_Zonewisedealer.DataBind();
            conn.Close();
        }


    //protected void gridView_Zonewisedealer_RowDeleting(object sender, GridViewDeleteEventArgs e)
    //{
    //    int id = Convert.ToInt32(gridView_Zonewisedealer.DataKeys.DataKeys[e.RowIndex].Value);
    //   if (conn.State != null)
    //        conn.Close();
    //    conn.Open();
    //    string gridviewQuery = "Delete from DealerOrganogram where DealerId= " + id;
    //    SqlCommand cd = new SqlCommand(gridviewQuery, conn);
    //    int row = cd.ExecuteNonQuery();
    //    if (row > 0)
    //    {
    //        gridViewGetData();
    //    }
    //    conn.Close();

    //}


    protected void gridView_Zonewisedealer_RowCommand(object sender, GridViewCommandEventArgs e)
     {
         int rIndex = int.Parse(e.CommandArgument.ToString());
             if (e.CommandName == "change")
             {
                 string dealerId = gridView_Zonewisedealer.DataKeys[rIndex].Values[0].ToString();

            if (conn.State != null)
                 conn.Close();
             conn.Open();
             string sqlquery = "Select * from ZoneWisDealer where Id=" + dealerId.ToString();
             SqlCommand cmd = new SqlCommand(sqlquery, conn);
             SqlDataAdapter adapter = new SqlDataAdapter(cmd);
             DataTable dt = new DataTable();
             conn.Close();
             adapter.Fill(dt);
             conn.Close();

             if (dt != null && dt.Rows.Count > 0)
             {
                 ddl_ZoneName.SelectedValue = dt.Rows[0]["Zone"].ToString();
                 ddl_dealerName.SelectedValue = dt.Rows[0]["Dealer"].ToString();
                 btn_save.Text = "Update";
                 ViewState["__DealerId__"] = dt.Rows[0]["Id"].ToString();
             }

         }

             if (e.CommandName == "remove")
             {
                 string dealerId = gridView_Zonewisedealer.DataKeys[rIndex].Values[0].ToString();
                 if (conn.State != null)
                     conn.Close();
                 conn.Open();
                 string gridviewQuery = "Delete from ZoneWisDealer where Id= " + dealerId;
                 SqlCommand cd = new SqlCommand(gridviewQuery, conn);
                 int row = cd.ExecuteNonQuery();
                 if (row > 0)
                 {
                     gridViewGetData();
                 }
                 conn.Close();
             }
     }


}