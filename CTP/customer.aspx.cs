using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Globalization;

using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Text;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;

public partial class CTP_customer : System.Web.UI.Page
{
    long i;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (System.Convert.ToInt32(Session["Vis"]) == 0)
        {
            Response.Redirect("../Account/Login.aspx");
        }

        if (!IsPostBack)
        {
            fnLoadCombo_Item(ddlProduct, "Model", "ProductID", "tbProduct");

            fnLoadDay(ddlDay);
            fnLoadMonth(ddlMonth);
            fnLoadYear(ddlYear);
        }

    }

    private void fnLoadCombo_Item(DropDownList xCombo, string pField, string pFieldID, string pTable)
    {
        //if (System.Convert.ToInt32(Session["Vis"]) == 0)
        //{
        //    Response.Redirect("~/Default.aspx");
        //}

        //SqlConnection conn = DBConnectionSpin.GetConnection();
        SqlConnection conn = DBConnection.GetConnection();

        try
        {
            conn.Open();
            //string sqlfns = "SELECT * FROM " + pTable + " WHERE CompID=" + Session["CompID"] + " ORDER BY " + pField + " ASC";
            //string sqlfns = "SELECT * FROM " + pTable + " ORDER BY " + pField + " ASC";
            //string sqlfns = "select Product.ProductID,Product.Model,Product.MRP,camd.CampPrice from Product left join tbCampaignDetails camd on camd.ProductID=Product.ProductID left join tbCampaignMaster camp  on camp.CampaignNo=camd.CampaignNo where camp.cStop=0 order by Product.Model";
            string sqlfns = "select * from Product WHERE Discontinue='No' and Model NOT IN ('GS-X6172NS', 'GS-Q6472NS', 'GS-B6432WB') and GroupName in ('KELVINATOR REFRIGERATOR','LG LCD TV','LG-REFRIGERATOR','LG-WASHING MACHINE'"+
              ",'RANGS LCD TV','KELVINATOR FREEZER','FREEZER',"+
              "'RANGS FREEZER','REFRIGERATOR(FROST)','REFRIGERATOR(NON FROST)','SONY LCD TV','WASHING MACHINE','MICROWAVE OVEN','LG-OVEN') Order By Model";
            
            
            SqlCommand cmd = new SqlCommand(sqlfns, conn);

            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sqlfns;
            cmd.Connection = conn;

            xCombo.DataSource = cmd.ExecuteReader();
            xCombo.DataTextField = pField;
            //ddlEmp.DataValueField = "SupName";
            xCombo.DataValueField = pFieldID;
            xCombo.DataBind();

            //Add blank item at index 0.
            xCombo.Items.Insert(0, new ListItem("", "0"));

            conn.Close();
        }
        catch (Exception ex)
        {
            //lblmsg.Text = ex.Message;
        }
        finally
        {
            conn.Close();
        }

    }


    protected void SaveData(object sender, EventArgs e)
    {
        //if (System.Convert.ToInt32(Session["Vis"]) == 0)
        //{
        //    Response.Redirect("~/Default.aspx");
        //}

        if ((ddlDay.SelectedItem.Text == "") && (ddlMonth.SelectedItem.Text == "") && (ddlYear.Text == ""))
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert",
                    "<script>alert('" + "Invalid Date format." + "');</script>", false);
            return;
        }

        if (txtName.Text == "")
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert",
                    "<script>alert('" + "Please enter your name." + "');</script>", false);
            txtName.Focus();
            return;
        }

        if (txtContact.Text == "")
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert",
                    "<script>alert('" + "Please enter your contact number." + "');</script>", false);
            txtContact.Focus();
            return;
        }


        if (txtChNo.Text == "")
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert",
                    "<script>alert('" + "Please generate promo code." + "');</script>", false);
            txtChNo.Focus();
            return;
        }


        SqlConnection conn = DBConnectionSpin.GetConnection();


        ////------------------------
        //string gSql = "SELECT DisCode ";
        //gSql = gSql + " FROM tbCustomer WHERE CustMobile='" + this.txtContact.Text.ToString() + "'";
        //gSql = gSql + " AND sTag=0";
        //SqlCommand cmd1 = new SqlCommand(gSql, conn);
        //conn.Open();
        //SqlDataReader drCheckCHNoIsUsed = cmd1.ExecuteReader();
        //if (drCheckCHNoIsUsed.Read() == true)
        //{
        //    ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert",
        //        "<script>alert('" + "You have already a promo code you didnt used yet please use this code first=" + drCheckCHNoIsUsed["DisCode"].ToString() + "');</script>", false);
        //    txtChNo.Text = "";
        //    return;
        //}
        ////else
        ////{
        ////    txtChNo.Text = r;
        ////}
        //conn.Close();
        //drCheckCHNoIsUsed.Close();
        ////=======================

        //------------------------
        string gSql = "SELECT DisCode,Model ";
        gSql = gSql + " FROM tbCustomer WHERE CustMobile='" + this.txtContact.Text.ToString() + "'";
        gSql = gSql + " AND sTag=0 And Model='" + txtModel.Text + "'";
        SqlCommand cmd1 = new SqlCommand(gSql, conn);
        conn.Open();
        SqlDataReader drCheckCHNoIsUsed = cmd1.ExecuteReader();
        if (drCheckCHNoIsUsed.Read() == true)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert",
                "<script>alert('" + "You have already a promo code you didnt spinned yet please use this code first=" + drCheckCHNoIsUsed["DisCode"].ToString() + " ,Model=" + drCheckCHNoIsUsed["Model"].ToString() + "');</script>", false);
            txtChNo.Text = "";
            return;
        }

        conn.Close();
        drCheckCHNoIsUsed.Close();
        //=======================

        //------------------------
        gSql = "";
        gSql = "SELECT DisCode,Model,DisPer ";
        gSql = gSql + " FROM tbCustomer WHERE CustMobile='" + this.txtContact.Text.ToString() + "'";
        gSql = gSql + " AND sTag=1 and DisCodeTag=0 And Model='" + txtModel.Text + "'";
        cmd1 = new SqlCommand(gSql, conn);
        conn.Open();
        drCheckCHNoIsUsed = cmd1.ExecuteReader();
        if (drCheckCHNoIsUsed.Read() == true)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert",
                "<script>alert('" + "You have already a promo code you played, please claim that discount on dms first=" + drCheckCHNoIsUsed["DisCode"].ToString() + " ,Model=" + drCheckCHNoIsUsed["Model"].ToString() + " ,Discount=" + drCheckCHNoIsUsed["DisPer"].ToString() + "');</script>", false);
            txtChNo.Text = "";
            return;
        }

        conn.Close();
        drCheckCHNoIsUsed.Close();
        //=======================

        //----------------------------------------------------------------------
        //CHECK DUPLICATE CHALLAN NO.
        string sSql = "";
        sSql = "SELECT CustMobile, ChNo FROM tbCustomer";
        sSql = sSql + " WHERE ChNo='" + this.txtChNo.Text + "'";

        SqlCommand cmdd = new SqlCommand(sSql, conn);
        conn.Open();
        SqlDataReader drd = cmdd.ExecuteReader();
        try
        {
            if (drd.Read())
            {
                //iMRSRID = Convert.ToInt32(dr["MRSRMID"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert",
                            "<script>alert('" + "This Code already exists." + "');</script>", false);
                txtChNo.Focus();
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


        string sDOB = ddlYear.SelectedItem.Text + "" + ddlMonth.SelectedItem.Value + "" + ddlDay.SelectedItem.Text;
        string stDate = ddlYear.SelectedItem.Text + "/" + ddlMonth.SelectedItem.Value + "/" + ddlDay.SelectedItem.Text;
        DateTime tDate = Convert.ToDateTime(stDate);


        // SAVE DATA
        sSql = "";
        sSql = "INSERT INTO tbCustomer(CustName,CustMobile,CustDOB,";
        sSql = sSql + " ProductID, Model, ChNo, DisCode, EntryDate, EID)";
        sSql = sSql + " VALUES(";
        sSql = sSql + " '" + txtName.Text + "','" + txtContact.Text + "','" + sDOB + "',";
        sSql = sSql + " '" + lblProdID.Text + "','" + txtModel.Text + "','" + txtChNo.Text + "', '" + txtChNo.Text + "',";
        sSql = sSql + " '" + DateTime.Today + "', '" + Session["EID"].ToString() + "')";
        //sSql = sSql + " WHERE AID='" + lblAID.Text + "'";

        SqlCommand cmd = new SqlCommand(sSql, conn);
        conn.Open();
        cmd.ExecuteNonQuery();
        conn.Close();

        //******************************************************************************************
        // FOR SMS   
        SqlConnection connSMS = DBConnectionSMS.GetConnection();
        string smsText = "";
        if (txtContact.Text != "")
        {
            
            smsText = "Dear " + this.txtName.Text + ",\n";
            smsText = smsText + "Your promocode is " + txtChNo.Text + "\n";
            //smsText = smsText + "for Sony Flagship Anniversary Toss & Win.\n";            
            smsText = smsText + "for discount by Spin & Win.\n";
            //smsText = smsText + "Product:\n";
            //smsText = smsText + "" + sProd + "";
            smsText = smsText + "Thank you.\n";
            //smsText = smsText + "Sony-Rangs";
            smsText = smsText + "" + Session["eName"].ToString() + "";

            sSql = "";
            sSql = "INSERT INTO tbSMS(ContactNo,SMSText,UserID,EntryDate,SMSSource)" +
                    " Values ('" + this.txtContact.Text + "','" + smsText + "'," +
                    " '" + Session["UserID"] + "','" + DateTime.Today + "'," +
                    " 'Toss-Win'" +
                    " )";
            SqlCommand cmdSMS = new SqlCommand(sSql, connSMS);
            connSMS.Open();
            cmdSMS.ExecuteNonQuery();
            connSMS.Close();

            //// SEND TO GM SIR
            //sSql = "";
            //sSql = "INSERT INTO tbSMS(ContactNo,SMSText,UserID,EntryDate,SMSSource)" +
            //        " Values ('01711546006','" + smsText + "'," +
            //        " '" + Session["UserID"] + "','" + DateTime.Today + "'," +
            //        " 'Toss-Win'" +
            //        " )";
            //cmdSMS = new SqlCommand(sSql, connSMS);
            //connSMS.Open();
            //cmdSMS.ExecuteNonQuery();
            //connSMS.Close();


            //// SEND TO ME
            //sSql = "";
            //sSql = "INSERT INTO tbSMS(ContactNo,SMSText,UserID,EntryDate,SMSSource)" +
            //        " Values ('01707000037','" + smsText + "'," +
            //        " '" + Session["UserID"] + "','" + DateTime.Today + "'," +
            //        " 'Toss-Win'" +
            //        " )";
            //cmdSMS = new SqlCommand(sSql, connSMS);
            //connSMS.Open();
            //cmdSMS.ExecuteNonQuery();
            //connSMS.Close(); 

        }
        //******************************************************************************************


        //Session["CustName"] = txtName.Text;
        //Session["CustPhone"] = txtContact.Text;
        //Session["CustDOB"] = sDOB;
        //Session["ProdID"] = lblProdID.Text;
        //Session["sModel"] = txtModel.Text;
        //Session["sChNo"] = txtChNo.Text;

        //Response.Redirect("Default.aspx");


        ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert",
                "<script>alert('" + "Submit Success ....An sms sent to the customer with promo code" + "');</script>", false);
        string myParameters = "";
        try
        {
            string sms = smsText;
            //saveLogs("sms string " + sms, true);
            string msisdn = txtContact.Text.Trim();
            //string msisdn = "";
            string apitoken = "j5iv07qb-t32ljykq-2su04uuz-xnwxdrjd-pgz8wd4h";

            ServicePointManager.Expect100Continue = false;
            myParameters = @"data=SENDSMS&cpuid=isms&sid=BRANDSONYRANGS" + "&msisdn=" + msisdn + "&api_token=" + apitoken + "&sms=" + System.Web.HttpUtility.UrlEncode(sms) + "&csms_id=123456";
            //saveLogs(myParameters, true);
            byte[] datatosend = Encoding.ASCII.GetBytes(myParameters);
            HttpWebRequest httpreq = (HttpWebRequest)WebRequest.Create("https://smsplus.sslwireless.com/api/v3/send-sms");
            httpreq.Method = "post";
            httpreq.ContentType = "application/x-www-form-urlencoded";
            httpreq.ContentLength = datatosend.Length;
            Stream st = httpreq.GetRequestStream();
            st.Write(datatosend, 0, datatosend.Length);
            st.Close();
            HttpWebResponse myresponse = (HttpWebResponse)httpreq.GetResponse();
            StreamReader myreader = new StreamReader(myresponse.GetResponseStream());
            //fnSendMail_Invoice_For_Redeem();
            string sr = myreader.ReadToEnd().ToString();
        }
        catch (Exception err)
        {
            //saveLogs("Error Occured when inserting into sms api", true);
            //saveLogs("Error Occured when inserting into sms api, Error=>> " + err.ToString(), false);
            //return "<SMS_STATUS>ERROR</SMS_STATUS>";
        }

        txtName.Text = "";
        txtContact.Text = "";
        ddlProduct.SelectedItem.Text = "";
        txtModel.Text = "";
        txtDesc.Text = "";
        txtChNo.Text = "";

        txtName.Focus();

    }

    protected void ddlProduct_SelectedIndexChanged(object sender, EventArgs e)
    {
        //SqlConnection conn = DBConnectionSpin.GetConnection();
        //string sSql = "";

        //double UP = 0, bUP = 0;

        //sSql = "";
        //sSql = "SELECT ProductID, Model, Description, MRP, SellingPrice, MinDis, MaxDis";
        //sSql = sSql + " FROM tbProduct ";
        //sSql = sSql + " WHERE Model='" + this.ddlProduct.SelectedItem.Text + "'";
        SqlConnection conn = DBConnection.GetConnection();
        string sSql = "";

        double UP = 0, bUP = 0;

        sSql = "";
        sSql = "SELECT ProductID, Model, ProdName";
        sSql = sSql + " FROM Product ";
        sSql = sSql + " WHERE Model='" + this.ddlProduct.SelectedItem.Text + "'";

        SqlCommand cmd = new SqlCommand(sSql, conn);
        conn.Open();
        SqlDataReader dr = cmd.ExecuteReader();

        try
        {
            if (dr.Read())
            {
                this.lblProdID.Text = dr["ProductID"].ToString();
                this.txtModel.Text = dr["Model"].ToString();
                this.lblModel.Text = dr["Model"].ToString();
                this.txtDesc.Text = dr["ProdName"].ToString();
                //UP = Convert.ToDouble(dr["SalePrice"].ToString());
                //this.txtUP.Text = Convert.ToString(UP);

            }
            else
            {
                this.lblProdID.Text = "";
                this.txtModel.Text = "";
                this.lblModel.Text = "";
                this.txtDesc.Text = "";
                //UP = 0;
                //this.txtUP.Text = Convert.ToString(UP);
                //bUP = 0;
                //this.txtBuyPrice.Text = Convert.ToString(bUP);
            }
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        finally
        {
            dr.Dispose();
            dr.Close();
            conn.Close();
        }

    }

    private void fnLoadDay(DropDownList cBox)
    {
        string j;
        for (i = 1; (i <= 31); i++)
        {
            if ((i < 10))
            {
                j = ("0" + i);
            }
            else
            {
                j = Convert.ToString(i);
            }

            cBox.Items.Add(j);
            if ((i == DateTime.Now.Day))
            {
                cBox.SelectedValue = j;
            }

        }

    }

    private void fnLoadMonth(DropDownList cBox)
    {
        string[] m_Name = new string[13];
        //{ "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
        m_Name[0] = "";
        m_Name[1] = "January";
        m_Name[2] = "February";
        m_Name[3] = "March";
        m_Name[4] = "April";
        m_Name[5] = "May";
        m_Name[6] = "June";
        m_Name[7] = "July";
        m_Name[8] = "August";
        m_Name[9] = "September";
        m_Name[10] = "October";
        m_Name[11] = "November";
        m_Name[12] = "December";

        string j;
        for (i = 1; (i <= 12); i++)
        {
            if ((i < 10))
            {
                j = ("0" + i);
            }
            else
            {
                j = Convert.ToString(i);
            }

            cBox.Items.Add(new ListItem(m_Name[i], j));
            if ((i == DateTime.Now.Month))
            {
                cBox.SelectedValue = j;
                //cBox.SelectedValue =m_Name[i].ToString();
            }

        }
        //cBox.SelectedIndex = DateTime.Now.Month-1;

    }

    private void fnLoadYear(DropDownList cBox)
    {
        for (i = (DateTime.Now.Year - 70); (i <= (DateTime.Now.Year)); i++)
        {
            if ((i == DateTime.Now.Year))
            {
                //cBox.Items.Add[i];
                cBox.Items.Add(new ListItem(Convert.ToString(DateTime.Now.Year), "0", true));
                //cBox.SelectedValue = Convert.ToString(DateTime.Now.Year);
            }
            else
            {
                cBox.Items.Add(new ListItem(Convert.ToString(i)));
            }
        }
        //cBox.Items.FindByText(DateTime.Now.Year.ToString()).Selected = true;
        cBox.Items.FindByText(DateTime.Now.Year.ToString()).Selected = true;

    }


    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        SqlConnection conn = DBConnectionSpin.GetConnection();

        Random generator = new Random();
        String r = generator.Next(0, 999999).ToString("D6");

        try
        {
            //check is used ch code
            string gSql = "";



            gSql = "";
            gSql = "SELECT DisCode ";
            gSql = gSql + " FROM tbCustomer WHERE DisCode='" + r + "'";
            //gSql = gSql + " AND sTag=1";
            SqlCommand cmd1 = new SqlCommand(gSql, conn);
            conn.Open();
            SqlDataReader dr1 = cmd1.ExecuteReader();
            if (dr1.Read() == true)
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert",
                    "<script>alert('" + "This Code already generated !!! Pls try again ...." + "');</script>", false);
                txtChNo.Text = "";
                return;
            }
            else
            {
                txtChNo.Text = r;
            }
            //conn.Close();
            dr1.Close();
        }

        catch (Exception ex)
        {
            throw (ex);
        }
        finally
        {
            //dr1.Dispose();
            //dr1.Close();
            conn.Close();
        }

    }
}