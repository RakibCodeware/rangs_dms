using System;
using System.Data;
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
using System.Data.SqlClient;
using System.Text;

public partial class Account_Login : System.Web.UI.Page
{
        
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Session["Vis"] = 0;
        }

        System.Threading.Thread.Sleep(100);
        string currenttime = DateTime.Now.ToLongTimeString();
        //lblcurrenttime.Text = currenttime;

        //RegisterHyperLink.NavigateUrl = "Register.aspx?ReturnUrl=" + HttpUtility.UrlEncode(Request.QueryString["ReturnUrl"]);
    }
    protected void LogIn_Click(object sender, EventArgs e)
    {        
        SqlConnection conn = DBConnection.GetConnection();
                
        string gSql;
        gSql = "";
        gSql = "select UserName,Passward,WebAccess,iVatUser," +
                " WebAccessType,UserType,EID,eName,BranchCode,ChngPass, iTagStock, iSpinWin " +
                " FROM SoftUser" +
                " where UserName='" + LoginUser.UserName + "'" +
                " and Passward='" + LoginUser.Password + "'";
        SqlCommand cmd = new SqlCommand(gSql, conn);
        conn.Open();   
        SqlDataReader dr = cmd.ExecuteReader();
        try
        {                     
            if (dr.Read() == true)
            {
                Session["Vis"] = 1;
                Session["UserType"] = dr["UserType"].ToString();
                Session["UserName"] = LoginUser.UserName;
                Session["ChangPW"] = dr["ChngPass"].ToString();
                Session["WebAccess"] = dr["WebAccess"].ToString();
                Session["WebAccessType"] = dr["WebAccessType"].ToString();

                Session["EID"] = dr["EID"].ToString();
                Session["eName"] = dr["eName"].ToString();

                Session["iVatUser"] = dr["iVatUser"].ToString();

                //vDeclare.sUser = LoginUser.UserName;
                //vDeclare.sWebAccess = dr["WebAccess"].ToString();
                //vDeclare.sWebAccessType = dr["WebAccessType"].ToString();


                if (Session["WebAccess"].ToString() == "YES")
                {
                    if (dr["EID"] != System.DBNull.Value)
                    {
                        if (Session["WebAccessType"].ToString() == "CTP")
                        {
                            //vDeclare.sBrId = Convert.ToInt32(dr["EID"]);                            
                            //vDeclare.sBr = dr["eName"].ToString();
                            //vDeclare.sBrCode = dr["BranchCode"].ToString();

                            Session["sBrId"] = dr["EID"].ToString();
                            Session["sBr"] = dr["eName"].ToString();
                            Session["sBrCode"] = dr["BranchCode"].ToString();

                            Response.Redirect("~/CTP/Default_Administrator.aspx");
                        }
                        else if (Session["WebAccessType"].ToString() == "PIC")
                        {
                            Session["sBrId"] = dr["EID"].ToString();
                            Session["sBr"] = dr["eName"].ToString();
                            Session["sBrCode"] = dr["BranchCode"].ToString();

                            //Response.Redirect("~/Default_Mkt_Admin.aspx");
                            Response.Redirect("~/PIC/Default_Administrator.aspx");
                        }
                        else if (Session["WebAccessType"].ToString() == "CIDD")
                        {
                            Session["sBrId"] = dr["EID"].ToString();
                            Session["sBr"] = dr["eName"].ToString();
                            Session["sBrCode"] = dr["BranchCode"].ToString();

                            //Response.Redirect("~/Default_CTP_admin.aspx");
                        }
                        else if (Session["WebAccessType"].ToString() == "Admin")
                        {
                            Session["sBrId"] = dr["EID"].ToString();
                            Session["sBr"] = dr["eName"].ToString();
                            Session["sBrCode"] = dr["BranchCode"].ToString();

                            Response.Redirect("~/Admin/Default_Administrator.aspx");
                        }

                        else if (Session["WebAccessType"].ToString() == "Management")
                        {
                            Session["sBrId"] = dr["EID"].ToString();
                            Session["sBr"] = dr["eName"].ToString();
                            Session["sBrCode"] = dr["BranchCode"].ToString();

                            Response.Redirect("~/Management/Default_Administrator.aspx");
                        }

                        else if (Session["WebAccessType"].ToString() == "MD")
                        {
                            Session["sBrId"] = dr["EID"].ToString();
                            Session["sBr"] = dr["eName"].ToString();
                            Session["sBrCode"] = dr["BranchCode"].ToString();

                            Response.Redirect("~/MD/Default_Administrator.aspx");
                        }

                        else if (Session["WebAccessType"].ToString() == "Commercial")
                        {
                            Session["sBrId"] = dr["EID"].ToString();
                            Session["sBr"] = dr["eName"].ToString();
                            Session["sBrCode"] = dr["BranchCode"].ToString();

                            //Response.Redirect("~/Default_Mkt_Admin.aspx");
                            Response.Redirect("~/Commercial/Default_Administrator.aspx");
                        }


                    }
                    else
                    {
                        //vDeclare.sBrId = 0;
                        //vDeclare.sBrCode = "000000";
                        Session["sBrId"] = 0;
                        Session["sBrCode"] = "000000";
                    }
                    
                }
                else
                {
                    this.lblMessage.Visible = true;
                    this.lblMessage.Text = "Sorry!!! " + LoginUser.UserName + ",  You have no permission of Web Access.";
                }
            }
            else
            {
                //string msg = "Invalid User or Password.";
                //Label lbl = new Label();
                //lbl.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('" + msg + "')</script>";
                //Page.Controls.Add(lbl);

                this.lblMessage.Visible = true;
                this.lblMessage.Text = "Invalid User or Password.";

                //Response.Write("<script> alert('Invalid User or Password...') </script>");

                //LoginUser.UserName.Focus();

                LoginUser.UserName = "";
                //LoginUser.Password = "";
                //this.lblMessage.Visible = false;
                return;
            }
        }

        catch (InvalidCastException err)
        {
            throw (err);
            //this.Session["exceptionMessage"] = err.Message;
            //Response.Redirect("ErrorDisplay.aspx");
            //log.Write(err.Message + err.StackTrace);
        }
        finally
        {
            dr.Dispose();
            dr.Close();
            conn.Close();
        }
        
    }

    
}
