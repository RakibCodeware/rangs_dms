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

public partial class LogIn : System.Web.UI.Page
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

    protected void btnLogin_Click(object sender, EventArgs e)
    {
        SqlConnection conn = DBConnection.GetConnection();
        SqlConnection conn1 = DBConnection.GetConnection();
        SqlConnection connDSM = DBConnectionDSM.GetConnection();

        string sSql = "";

        //-----------------------------------------------------------------
        string ssFYSDate = "07/01/2019";
        DateTime sFYSDate = Convert.ToDateTime(ssFYSDate);
        Session["ssFYSDate"] = ssFYSDate;
        Session["sFYSDate"] = sFYSDate;

        string seFYSDate = "06/30/2019";
        DateTime eFYSDate = Convert.ToDateTime(seFYSDate);
        Session["seFYSDate"] = seFYSDate;
        Session["eFYSDate"] = eFYSDate;
        //-----------------------------------------------------------------

        string gSql;
        gSql = "";
        //gSql = "select RolesId,UserName,Passward,WebAccess,iVatUser," +
        //        " WebAccessType,UserType,EID,eName,BranchCode,ChngPass,iTagStock,iSpinWin " +
        //        " FROM SoftUser" +
        //        " where UserName='" + UserName1.Text + "'" +
        //        " and Passward='" + Password.Text + "' AND Active=1";
        gSql = "Sp_Login";
        SqlCommand cmd = new SqlCommand(gSql, conn);
        cmd.CommandType = CommandType.StoredProcedure;
        //cmd.CommandText = "Sp_Login";
        //cmd.Parameters.AddWithValue("@UserName", UserName1.Text.ToString());
        //cmd.Parameters.AddWithValue("@Passward", Password.Text.ToString());
        cmd.Parameters.Add("@UserName", SqlDbType.NVarChar).Value = UserName1.Text;
        cmd.Parameters.Add("@Passward", SqlDbType.NVarChar).Value = Password.Text;

        conn.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        try
        {
            if (dr.Read() == true)
            {
                Session["Vis"] = 1;
                Session["UserType"] = dr["UserType"].ToString();
                Session["UserName"] = UserName1.Text;
                Session["RolesId"] = dr["RolesId"].ToString();

                Session["UserID"] = UserName1.Text;
                Session["sUser"] = UserName1.Text;
                Session["ChangPW"] = dr["ChngPass"].ToString();
                Session["WebAccess"] = dr["WebAccess"].ToString();
                Session["WebAccessType"] = dr["WebAccessType"].ToString();

                Session["EID"] = dr["EID"].ToString();
                Session["eName"] = dr["eName"].ToString();

                Session["iTagStock"] = dr["iTagStock"].ToString();
                string ssaa = dr["iTagStock"].ToString();
                Session["iSpinWin"] = dr["iSpinWin"].ToString();

                Session["iVatUser"] = dr["iVatUser"].ToString();

                //vDeclare.sUser = LoginUser.UserName;
                //vDeclare.sWebAccess = dr["WebAccess"].ToString();
                //vDeclare.sWebAccessType = dr["WebAccessType"].ToString();

                string s = Request.UserHostAddress;

                //----------------------------------------------------------------------------
                sSql = "";
                sSql = "INSERT INTO LogUser(UserName,LogIn,LogINFrom,UserPC)" +
                    " VALUES('" + UserName1.Text + "','" + DateTime.Now + "','Online','" + s + "')";
                SqlCommand cmd12 = new SqlCommand(sSql, conn1);
                conn1.Open();
                cmd12.ExecuteNonQuery();
                conn1.Close();
                //----------------------------------------------------------------------------


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
                        //----------------------------------------------------------------------
                        else if (Session["WebAccessType"].ToString() == "Dealer")
                        {
                            //vDeclare.sBrId = Convert.ToInt32(dr["EID"]);       
                            //vDeclare.sBr = dr["eName"].ToString();
                            //vDeclare.sBrCode = dr["BranchCode"].ToString();

                            Session["sBrId"] = dr["EID"].ToString();
                            Session["sBr"] = dr["eName"].ToString();
                            Session["sBrCode"] = dr["BranchCode"].ToString();

                            //LOAD ZONE INFO
                            gSql = "";
                            gSql = "select CategoryID,Code,CatName,ZoneType,DMSEID,ZonalEmail,MailTo,SubStoreID from Zone";
                            gSql = gSql + " Where Code='" + dr["BranchCode"].ToString() + "'";

                            SqlCommand cmd2 = new SqlCommand(gSql, connDSM);
                            connDSM.Open();
                            SqlDataReader dr2 = cmd2.ExecuteReader();
                            if (dr2.Read() == true)
                            {
                                Session["sZoneID"] = dr2["CategoryID"].ToString();
                                Session["sZoneCode"] = dr2["Code"].ToString();
                                Session["sZoneName"] = dr2["CatName"].ToString();
                                Session["sDealerMktEID"] = dr2["DMSEID"].ToString();
                                Session["sZonalEmail"] = dr2["ZonalEmail"].ToString();
                                Session["sMailTo"] = dr2["MailTo"].ToString();
                                Session["sStoreID"] = dr2["SubStoreID"].ToString();
                            }
                            dr2.Dispose();
                            dr2.Close();
                            connDSM.Close();

                            Response.Redirect("~/Dealer/Default_Administrator.aspx");
                        }
                        //----------------------------------------------------------------------
                        else if (Session["WebAccessType"].ToString() == "Sub Store")
                        {
                            if (Session["EID"].ToString() == "370")
                            {
                                Session["sBrId"] = dr["EID"].ToString();
                                Session["sBr"] = dr["eName"].ToString();
                                Session["sBrCode"] = dr["BranchCode"].ToString();

                                //LOAD ZONE INFO
                                gSql = "";
                                gSql = "select CategoryID,Code,CatName,ZoneType,SubStoreCode,SubStoreID,DMSEID from Zone";
                                gSql = gSql + " Where SubStoreCode='" + dr["BranchCode"].ToString() + "'";

                                SqlCommand cmd2 = new SqlCommand(gSql, connDSM);
                                connDSM.Open();
                                SqlDataReader dr2 = cmd2.ExecuteReader();
                                if (dr2.Read() == true)
                                {
                                    //Session["sZoneID"] = dr2["CategoryID"].ToString();
                                    //Session["sZoneCode"] = dr2["Code"].ToString();
                                    //Session["sZoneName"] = dr2["CatName"].ToString();
                                    Session["sStoreID"] = dr2["SubStoreID"].ToString();
                                    Session["sStoreCode"] = dr2["SubStoreCode"].ToString();
                                    //Session["sDealerMktEID"] = dr2["DMSEID"].ToString(); //SUB STORE ID
                                }
                                dr2.Dispose();
                                dr2.Close();
                                connDSM.Close();

                                Response.Redirect("~/Zone/Default_Administrator.aspx");
                            }
                            else
                            {

                                Session["sBrId"] = dr["EID"].ToString();
                                Session["sBr"] = dr["eName"].ToString();
                                Session["sBrCode"] = dr["BranchCode"].ToString();

                                //LOAD ZONE INFO
                                gSql = "";
                                gSql = "select CategoryID,Code,CatName,ZoneType,SubStoreCode,SubStoreID,DMSEID from Zone";
                                gSql = gSql + " Where SubStoreCode='" + dr["BranchCode"].ToString() + "'";

                                SqlCommand cmd2 = new SqlCommand(gSql, connDSM);
                                connDSM.Open();
                                SqlDataReader dr2 = cmd2.ExecuteReader();
                                if (dr2.Read() == true)
                                {
                                    Session["sZoneID"] = dr2["CategoryID"].ToString();
                                    Session["sZoneCode"] = dr2["Code"].ToString();
                                    Session["sZoneName"] = dr2["CatName"].ToString();
                                    Session["sStoreID"] = dr2["SubStoreID"].ToString();
                                    Session["sStoreCode"] = dr2["SubStoreCode"].ToString();
                                    Session["sDealerMktEID"] = dr2["DMSEID"].ToString(); //SUB STORE ID
                                }
                                dr2.Dispose();
                                dr2.Close();
                                connDSM.Close();

                                Response.Redirect("~/Zone/Default_Administrator.aspx");
                            }
                        }
                        //----------------------------------------------------------------------
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

                            Response.Redirect("~/CIDD/Default_Administrator.aspx");

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
                        else if (Session["WebAccessType"].ToString() == "Management Sales")
                        {
                            Session["sBrId"] = dr["EID"].ToString();
                            Session["sBr"] = dr["eName"].ToString();
                            Session["sBrCode"] = dr["BranchCode"].ToString();

                            Response.Redirect("~/Management_Sales/Default_Administrator.aspx");
                        }
                        else if (Session["WebAccessType"].ToString() == "Dg")
                        {
                            Session["sBrId"] = dr["EID"].ToString();
                            Session["sBr"] = dr["eName"].ToString();
                            Session["sBrCode"] = dr["BranchCode"].ToString();

                            Response.Redirect("~/Digital_Marketing/Default_Administrator.aspx");
                        }
                        else if (Session["WebAccessType"].ToString() == "Management Dealer")
                        {
                            Session["sBrId"] = dr["EID"].ToString();
                            Session["sBr"] = dr["eName"].ToString();
                            Session["sBrCode"] = dr["BranchCode"].ToString();

                            Response.Redirect("~/Management_dealer/Default_Administrator.aspx");
                        }

                          //Dealer Report UserAcsess start
                        else if (Session["WebAccessType"].ToString() == "DealerReports")
                        {
                            Session["sBrId"] = dr["EID"].ToString();
                            Session["sBr"] = dr["eName"].ToString();
                            Session["sBrCode"] = dr["BranchCode"].ToString();

                            Response.Redirect("~/DealerReports/Default.aspx");
                        }
                        //Dealer Report UserAcsess end

                        else if (Session["WebAccessType"].ToString() == "MD")
                        {
                            Session["sBrId"] = dr["EID"].ToString();
                            Session["sBr"] = dr["eName"].ToString();
                            Session["sBrCode"] = dr["BranchCode"].ToString();

                            Response.Redirect("~/MD/Default_Administrator.aspx");
                        }
                        else if (Session["WebAccessType"].ToString() == "Corporate")
                        {
                            Session["sBrId"] = dr["EID"].ToString();
                            Session["sBr"] = dr["eName"].ToString();
                            Session["sBrCode"] = dr["BranchCode"].ToString();

                            Response.Redirect("~/Corporate/Default_Administrator.aspx");
                        }

                        else if (Session["WebAccessType"].ToString() == "Commercial")
                        {
                            Session["sBrId"] = dr["EID"].ToString();
                            Session["sBr"] = dr["eName"].ToString();
                            Session["sBrCode"] = dr["BranchCode"].ToString();

                            //Response.Redirect("~/Default_Mkt_Admin.aspx");
                            Response.Redirect("~/Commercial/Default_Administrator.aspx");
                        }

                        else if (Session["WebAccessType"].ToString() == "Accounts")
                        {
                            Session["sBrId"] = dr["EID"].ToString();
                            Session["sBr"] = dr["eName"].ToString();
                            Session["sBrCode"] = dr["BranchCode"].ToString();

                            //Response.Redirect("~/Default_Mkt_Admin.aspx");
                            Response.Redirect("~/Acc/Default_Administrator.aspx");
                        }
                        else if (Session["WebAccessType"].ToString() == "Finance")
                        {
                            Session["sBrId"] = dr["EID"].ToString();
                            Session["sBr"] = dr["eName"].ToString();
                            Session["sBrCode"] = dr["BranchCode"].ToString();

                            //Response.Redirect("~/Default_Mkt_Admin.aspx");
                            Response.Redirect("~/Finance/Default_Administrator.aspx");
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
                    this.lblMessage.Text = "Sorry!!! " + UserName1.Text + ",  You have no permission of Web Access.";
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

                UserName1.Text = "";
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