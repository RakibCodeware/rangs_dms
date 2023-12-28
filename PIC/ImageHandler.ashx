<%@ WebHandler Language="C#" Class="ImageHandler" %>

using System;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

public class ImageHandler : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {                

        string imageid = context.Request.QueryString["ImID"];
        //SqlConnection con = new SqlConnection("Data Source=ZUNAYED-LT\\SQLEXPRESS;Integrated Security=false;Initial Catalog=dbTestImage; User Id=sa; password=Adminn321");
        //SqlConnection con = new SqlConnection("Data Source=ZUNAYED-LT;Integrated Security=false;Initial Catalog=dbTestImage; User Id=sa");
        SqlConnection con = DBConnection.GetConnection();
        con.Open();
        SqlCommand cmd = new SqlCommand("Select EmpInImg from AttndRegDet where AID=" + imageid, con);
        SqlDataReader dr = cmd.ExecuteReader();
        dr.Read();
        context.Response.BinaryWrite((byte[])dr[0]);
        con.Close();
        context.Response.End();                
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}