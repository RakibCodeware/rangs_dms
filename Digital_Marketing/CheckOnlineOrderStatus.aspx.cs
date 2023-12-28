using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

public partial class Digital_Marketing_CheckOnlineOrderStatus : System.Web.UI.Page
{


    SqlConnection conn = DBConnection.GetConnection();
    SqlConnection conn1 = DBConnection.GetConnection();
    DataTable dt;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DataBind();
        }
    }

    public void getConnection()
    {
        if (conn.State != null)
            conn.Close();
        conn.Open();
    }

    protected void btn_preview_Click(object sender, EventArgs e)
    {
        GetData();

    }


    private void DataBind() 
     {
         GetData();
     }

    private void GetData() 
    {
        try
        {
            getConnection();
            string query = "";
            query = @"select OrderNumber,InvoiceNumber from api_logData where UpdatedAt >= '" + Convert.ToDateTime(this.txtFrom.Text) + "'  AND UpdatedAt <= '" + Convert.ToDateTime(this.txtToDate.Text + " 23:59:59") + "' and OrderNumber<>'' and IsDeliveredFromOnline is null and IsDeliveredFromDMS='1' ";


            dt = new DataTable();
            dt = ExecuteReturnDataTable(query, conn);
            gvOnlineOrderStatus.DataSource = dt;
            gvOnlineOrderStatus.DataBind();
        }
        catch (Exception ex) 
         {
        
        }
       


    }


    public DataTable ExecuteReturnDataTable(string sqlCmd, SqlConnection _conn)
    {
        try
        {

            if (_conn == null)
            {
                _conn = new SqlConnection();
                _conn.Open();
            }
            else
            {
                if (_conn.State == ConnectionState.Open)
                    _conn.Close();
                _conn.Open();

            }



            DataTable dt = new DataTable();
            // cmd = new SqlCommand(sqlCmd, con);
            SqlDataAdapter da = new SqlDataAdapter(sqlCmd, _conn);
            //   da.SelectCommand.CommandTimeout =300;  // seconds
            da.SelectCommand.CommandTimeout = 0;  // seconds              
            da.Fill(dt);
            _conn.Close();
            return dt;
        }
        catch (Exception ex) { return null; }
    }

}