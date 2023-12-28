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

using System.Net.Mail;

public partial class CustomerOrder : System.Web.UI.Page
{
    SqlConnection conn = DBConnection_ROS.GetConnection();

    //SqlConnection conn = DBConnection.GetConnection();
    long i;
    int iMRSRID = 0;
    DataTable dt;
    DateTime tDate;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (System.Convert.ToInt32(Session["Vis"]) == 0)
        {
            Response.Redirect("../Account/Login.aspx");
        }

        //errorMsg.Text = "ssssssss"+Request.QueryString["action"];
        if (!IsPostBack)
        {
            this.txtFrom.Text = DateTime.Today.ToString("MM/dd/yyyy");
            this.txtToDate.Text = DateTime.Today.ToString("MM/dd/yyyy");

            //LOAD CTP INFO
            fnLoadCTP();

            //LOAD ORDER DETAILS DATA
            fnLoadOrderData();
        }

    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        fnLoadOrderData_Search();
    }

    protected void fnLoadOrderData_Search()
    {

        //string InvFDate = ddlYear.SelectedItem.Text + "" + ddlMonth.SelectedItem.Value + "" + ddlDay.SelectedItem.Text;
        //string InvSDate = ddlYear2.SelectedItem.Text + "" + ddlMonth2.SelectedItem.Value + "" + ddlDay2.SelectedItem.Text;

        //DateTime tDate;
        //tDate = Convert.ToDateTime(this.txtFrom.Text);

        SqlConnection conn = DBConnection_ROS.GetConnection();

        conn.Open();

        string sSql = "";
        sSql = "SELECT DelID, DelNo, DelDate, TotalAmnt, TotalQty, DelFrom, EID,";
        sSql = sSql + " CustName, CustContact, DelAddress, DelContact, CustEmail, PayType,";
        sSql = sSql + " CASE dStatus WHEN 0 THEN 'Processing' WHEN 1 THEN 'Quality Check'";
        sSql = sSql + " WHEN 2 THEN 'Dispatched' WHEN 3 THEN 'Delivered' WHEN 4 THEN 'Cancel' END AS tStatus";
        sSql = sSql + " FROM dbo.tbCustomerDelivery";

        sSql = sSql + " Where EID='" + Session["sBrId"].ToString() + "'";

        if (txtContact.Text.Length > 0)
        {
            sSql = sSql + " AND (CustName LIKE '%" + txtContact.Text + "%'";
            sSql = sSql + " OR CustContact LIKE '%" + txtContact.Text + "%'";
            sSql = sSql + " OR CustEmail LIKE '%" + txtContact.Text + "%'";
            sSql = sSql + " OR DelNo LIKE '%" + txtContact.Text + "%')";
        }
        else
        {
            sSql = sSql + " AND (CONVERT(date, DelDate1, 101)>='" + Convert.ToDateTime(this.txtFrom.Text) + "'";
            sSql = sSql + " AND CONVERT(date, DelDate1, 101)<='" + Convert.ToDateTime(this.txtToDate.Text) + "')";
        }

        //if (ddlCTP.SelectedItem.Text != "ALL")
        //{
        //    sSql = sSql + " AND (DelFrom ='" + ddlCTP.SelectedItem.Text + "')";
        //}

        if (ddlStatus.SelectedItem.Text != "ALL")
        {
            sSql = sSql + " AND (dStatus ='" + ddlStatus.SelectedItem.Value + "')";
        }


        sSql = sSql + " Order By DelID Desc";

        SqlCommand cmd = new SqlCommand(sSql, conn);
        //OleDbDataReader dr = cmd.ExecuteReader();

        DataSet ds = new DataSet();
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        da.Fill(ds);

        GridView1.DataSource = ds;
        GridView1.DataBind();
        //dr.Close();
        conn.Close();

    }

    protected void fnLoadOrderData()
    {

        //string InvFDate = ddlYear.SelectedItem.Text + "" + ddlMonth.SelectedItem.Value + "" + ddlDay.SelectedItem.Text;
        //string InvSDate = ddlYear2.SelectedItem.Text + "" + ddlMonth2.SelectedItem.Value + "" + ddlDay2.SelectedItem.Text;

        //DateTime tDate;
        //tDate = Convert.ToDateTime(this.txtFrom.Text);

        SqlConnection conn = DBConnection_ROS.GetConnection();

        conn.Open();

        string sSql = "";
        sSql = "SELECT DelID, DelNo, DelDate, TotalAmnt, TotalQty, DelFrom, EID,";
        sSql = sSql + " CustName, CustContact, DelAddress, DelContact, CustEmail, PayType,";
        sSql = sSql + " CASE dStatus WHEN 0 THEN 'Processing' WHEN 1 THEN 'Quality Check'";
        sSql = sSql + " WHEN 2 THEN 'Dispatched' WHEN 3 THEN 'Delivered' WHEN 4 THEN 'Cancel' END AS tStatus";
        sSql = sSql + " FROM dbo.tbCustomerDelivery";

        sSql = sSql + " Where (EID='" + Session["sBrId"].ToString() + "')";
        sSql = sSql + " AND (dStatus = 0)";
                
        sSql = sSql + " Order By DelID Desc";

        SqlCommand cmd = new SqlCommand(sSql, conn);
        //OleDbDataReader dr = cmd.ExecuteReader();

        DataSet ds = new DataSet();
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        da.Fill(ds);

        GridView1.DataSource = ds;
        GridView1.DataBind();
        //dr.Close();
        conn.Close();

    }

    protected void lnkView_Click(object sender, EventArgs e)
    {
        //if (System.Convert.ToInt32(Session["Vis"]) == 0)
        //{
        //    Response.Redirect("Default.aspx");
        //}

        //CLEAR GRIDVIEW
        gvUsers.DataSource = "";
        gvUsers.DataBind();

        
        LinkButton btnsubmit = sender as LinkButton;
        GridViewRow gRow = (GridViewRow)btnsubmit.NamingContainer;
        //txtPID.Text = gvCustomres.DataKeys[gRow.RowIndex].Value.ToString();
        //string sPID = Convert.ToString(gvCustomres.DataKeys[gRow.RowIndex].Value.ToString());
        string sPID = GridView1.DataKeys[gRow.RowIndex].Value.ToString();
        //txtPName.Text = gRow.Cells[0].Text;        
        //this.ModalPopupExtender1.Show();


        SqlConnection conn = DBConnection_ROS.GetConnection();

        string sSql = "";                
        sSql = "";
        sSql = "SELECT dbo.tbCustomerDelivery.DelID, dbo.tbCustomerDelivery.DelNo, dbo.tbCustomerDelivery.DelDate,";
        sSql = sSql + " dbo.tbCustomerDelivery.DelDate1, dbo.tbCustomerInfo.CustName, dbo.tbCustomerInfo.ContactNo,";
        sSql = sSql + " dbo.tbCustomerInfo.EmailAdd, dbo.tbCustomerInfo.Address, dbo.tbCustomerInfo.fname,";
        sSql = sSql + " dbo.tbCustomerInfo.lname, dbo.tbCustomerInfo.Dist, dbo.tbCustomerInfo.Thana,";
        sSql = sSql + " dbo.tbCustomerInfo.Add1, dbo.tbCustomerInfo.Add2, dbo.tbCustomerInfo.thum, ";
        sSql = sSql + " dbo.tbCustomerInfo.path, dbo.tbCustomerDelivery.TotalQty, dbo.tbCustomerDelivery.TotalAmnt,";
        sSql = sSql + " dbo.tbCustomerDelivery.dStatus, dbo.tbCustomerDelivery.PayType, dbo.tbCustomerDelivery.StatusNote,";
        sSql = sSql + " dbo.tbCustomerDelivery.DelFee, dbo.tbCustomerDelivery.DelTax,dbo.tbCustomerDelivery.PayType,";
        sSql = sSql + " dbo.tbCustomerDelivery.TotalAmnt, dbo.tbCustomerDelivery.TotalQty,dbo.tbCustomerDelivery.DelType,";
        sSql = sSql + " dbo.Entity.eName, dbo.Entity.EDesc, dbo.Entity.EmailAdd AS CTPEmail";

        sSql = sSql + " FROM dbo.tbCustomerDelivery INNER JOIN";
        sSql = sSql + " dbo.tbCustomerInfo ON dbo.tbCustomerDelivery.CustEmail = dbo.tbCustomerInfo.EmailAdd";
        sSql = sSql + " INNER JOIN dbo.Entity ON dbo.tbCustomerDelivery.EID = dbo.Entity.EID";

        sSql = sSql + " WHERE dbo.tbCustomerDelivery.DelID = '" + sPID + "'";


        //sSql = sSql + " WHERE tbMemberList.ID= " + sPID + "";
        SqlCommand cmd = new SqlCommand(sSql, conn);
        conn.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            this.lblID.Text = dr["DelID"].ToString();
            this.lblInv.Text = dr["DelNo"].ToString();
            this.lblDate.Text = dr["DelDate"].ToString();

            this.lblCustName.Text = dr["CustName"].ToString();
            this.lblContact.Text = dr["ContactNo"].ToString();
            this.lblAdd.Text = dr["Address"].ToString();
            this.lblEmail.Text = dr["EmailAdd"].ToString();

            this.lblDist.Text = dr["Dist"].ToString();
            this.lblThana.Text = dr["Thana"].ToString();

            this.lblTotalAmnt.Text = dr["TotalAmnt"].ToString();
            this.lblPaymentMethod.Text = dr["PayType"].ToString();
            this.lblDelType.Text = dr["DelType"].ToString();
            this.lblEName.Text = dr["eName"].ToString();
            this.lblCTPEmail.Text = dr["CTPEmail"].ToString();
            this.lblCTPAdd.Text = dr["EDesc"].ToString();

            txtNote.Text = dr["StatusNote"].ToString();
            //Image1.ImageUrl = "img/photos/" + dr["path"].ToString();

            if (dr["dStatus"].ToString() == "0")
            {
                RadioButtonList1.SelectedIndex = 0;
            }
            else if (dr["dStatus"].ToString() == "1")
            {
                RadioButtonList1.SelectedIndex = 1;
            }
            else if (dr["dStatus"].ToString() == "2")
            {
                RadioButtonList1.SelectedIndex = 2;
            }
            else if (dr["dStatus"].ToString() == "3")
            {
                RadioButtonList1.SelectedIndex = 3;
            }
            else if (dr["dStatus"].ToString() == "4")
            {
                RadioButtonList1.SelectedIndex = 4;
            }
            else
            {
                RadioButtonList1.SelectedIndex = 0;
            }


        }
        else
        {
            this.lblID.Text = "";
            this.lblInv.Text = "";
            this.lblDate.Text = "";

            this.lblCustName.Text = "";
            this.lblContact.Text = "";
            this.lblAdd.Text = "";
            this.lblEmail.Text = "";

            this.lblDist.Text = "";
            this.lblThana.Text = "";
            
        }

        conn.Close();



        //LOAD DETAILS DATA
        sSql = "";        
        sSql ="SELECT dbo.tbCustomerDelDetails.DelDID, dbo.tbCustomerDelDetails.DelID, dbo.tbCustomerDelDetails.ProductID,";
        sSql = sSql + " dbo.tbProduct.title, dbo.tbProduct.titleDesc, dbo.tbCustomerDelDetails.SalePrice,";
        sSql = sSql + " dbo.tbCustomerDelDetails.tQty, dbo.tbCustomerDelDetails.tAmnt";
        sSql = sSql + " FROM dbo.tbCustomerDelDetails INNER JOIN";
        sSql = sSql + " dbo.tbProduct ON dbo.tbCustomerDelDetails.ProductID = dbo.tbProduct.ProductID";
        sSql = sSql + " WHERE (dbo.tbCustomerDelDetails.DelID = '" + this.lblID.Text + "')";

        cmd = new SqlCommand(sSql, conn);
        conn.Open();

        // Create a SqlDataAdapter to get the results as DataTable
        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sSql, conn);

        //// Fill the DataTable with the result of the SQL statement
        //sqlDataAdapter.Fill(dt);

        //gvUsers.DataSource = dt;
        //gvUsers.DataBind();

        DataSet ds = new DataSet();
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        da.Fill(ds);

        gvUsers.DataSource = ds;
        gvUsers.DataBind();
        //dr.Close();
        conn.Close();

        this.ModalPopupExtender1.Show();


    }


    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        if (System.Convert.ToInt32(Session["Vis"]) == 0)
        {
            Response.Redirect("Default.aspx");
        }

        SqlConnection conn = DBConnection_ROS.GetConnection();

        string sSql = "";
        sSql = "UPDATE tbCustomerDelivery Set dStatus='" + RadioButtonList1.SelectedIndex + "',";
        sSql = sSql + " StatusNote='" + txtNote.Text + "' where DelNo='" + lblInv.Text + "'";
        SqlCommand cmdIns = new SqlCommand(sSql, conn);
        conn.Open();
        cmdIns.ExecuteNonQuery();
        conn.Close();

        if (RadioButtonList1.SelectedIndex == 4)
        {
            //SEND CANCEL MAIL
            try
            {
                fnSendMail();
            }
            catch
            {
                //
            }
        }

        //LOAD ORDER DETAILS DATA
        fnLoadOrderData();

    }

    protected void btnSendMail_Click(object sender, EventArgs e)
    {
        if (System.Convert.ToInt32(Session["Vis"]) == 0)
        {
            Response.Redirect("Default.aspx");
        }

        SqlConnection conn = DBConnection_ROS.GetConnection();

        //string sSql = "";
        //sSql = "UPDATE tbCustomerDelivery Set dStatus='" + RadioButtonList1.SelectedIndex + "',";
        //sSql = sSql + " StatusNote='" + txtNote.Text + "' where DelNo='" + lblInv.Text + "'";
        //SqlCommand cmdIns = new SqlCommand(sSql, conn);
        //conn.Open();
        //cmdIns.ExecuteNonQuery();
        //conn.Close();

        //if (RadioButtonList1.SelectedIndex == 4)
        //{
        //    //SEND CANCEL MAIL
        //    try
        //    {
                fnSendMail_Resend();
        //    }
        //    catch
        //    {
        //        //
        //    }
        //}

        
    }

    protected void fnSendSMS()
    {
        //***************************************************************************************
        // FOR SMS   

        string sMobile = "";
        sMobile = lblContact.Text;

        SqlConnection conn = DBConnectionSMS.GetConnection();

        //SqlConnection connSMS = DBConnection.GetConnectionSMS();
        if (sMobile != "")
        {
            String smsText = "";
            //smsText = "প্রিয় গ্রাহক,\n";
            //smsText = smsText + "র‌্যাংগ্স অনলাইন স্টোরটিতে আপনার অর্ডারটি নিশ্চিত হয়ে গেছে। \n";
            //smsText = smsText + "পণ্য বিতরণের সময় আপনার সাথে যোগাযোগ করা হবে।\n";
            ////smsText = smsText + "Bill # " + this.txtCHNo.Text + ".\n";
            ////smsText = smsText + "Date: " + this.txtDate.Text + ".\n";
            ////smsText = smsText + "Bill Amount : " + this.txtNetAmnt.Text + ".\n";
            //smsText = smsText + "ধন্যবাদ।\n";
            //smsText = smsText + "যোগাযোগ: 02-9663551-3.";

            smsText = "Dear Customer,\n";
            smsText = smsText + "Your order# " + this.lblInv.Text + " has been Cancel from Rangs Online Store.\n";
            //smsText = smsText + "You will be contacted during product delivery.\n";
            //smsText = smsText + "Order # " + this.lblInv.Text + ".\n";
            //smsText = smsText + "Date: " + this.txtDate.Text + ".\n";
            //smsText = smsText + "Bill Amount : " + this.txtNetAmnt.Text + ".\n";
            smsText = smsText + "Thanks.\n";
            smsText = smsText + "Contact: 02-9663551-3";

            string ssSql = "";
            ssSql = "INSERT INTO tbSMS(ContactNo,SMSText,UserID,EntryDate,SMSSource)" +
                    " Values ('" + sMobile + "','" + smsText + "'," +
                    " 'Online-Store','" + DateTime.Now + "'," +
                    " 'Online-Store'" +
                    " )";
            SqlCommand cmdSMS = new SqlCommand(ssSql, conn);
            conn.Open();
            cmdSMS.ExecuteNonQuery();
            conn.Close();
        }

        //***************************************************************************************
    }

    protected void fnLoadCTP()
    {
        //if (System.Convert.ToInt32(Session["Vis"]) == 0)
        //{
        //    Response.Redirect("Default.aspx");
        //}

        SqlConnection conn = DBConnection_ROS.GetConnection();

        SqlConnection conn1 = DBConnection_ROS.GetConnection();

        //try
        //{
            
                conn.Open();
                
                string sqlfns = "";
                sqlfns = "SELECT Entity_1.eName AS ZoneName, dbo.Entity.eName AS CTP,";
                sqlfns = sqlfns + " dbo.Entity.EntityType, dbo.Entity.EID";
                sqlfns = sqlfns + " FROM dbo.Entity INNER JOIN";
                sqlfns = sqlfns + " dbo.Entity AS Entity_1 ON dbo.Entity.ParentEntity = Entity_1.EID";
                sqlfns = sqlfns + " WHERE (dbo.Entity.EntityType = 'showroom')";
                sqlfns = sqlfns + " AND (dbo.Entity.ActiveDeactive = 1) AND (dbo.Entity.SalesOrShowroom = 0)";
                //sqlfns = sqlfns + " AND Entity_1.eName='" + ddlZone.SelectedItem.Text + "'";
                sqlfns = sqlfns + " ORDER BY dbo.Entity.eName";

                SqlCommand cmd = new SqlCommand(sqlfns, conn);

                //SqlCommand cmd1 = new SqlCommand(sqlfns, conn1);
                //conn1.Open();
                //SqlDataReader dr = cmd1.ExecuteReader();
                //string sCTP = "", sAdd = "";
                //if (dr.Read() == true)
                //{
                //    sCTP = dr["CTP"].ToString();
                //    sAdd = dr["EDesc"].ToString();
                //}
                //conn1.Close();

                //string sTT = sCTP + " (" + sAdd + ")";    

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sqlfns;
                cmd.Connection = conn;

                ddlCTP.DataSource = cmd.ExecuteReader();
                ddlCTP.DataTextField = "CTP";
                //ddlCTP.DataValueField = "SupName";
                //xCombo.DataValueField = pFieldID;
                ddlCTP.DataValueField = "EID";
                ddlCTP.DataBind();

                //Add blank item at index 0.
                ddlCTP.Items.Insert(0, new ListItem("ALL", "0"));

                conn.Close();
           

        //}
        //catch (Exception ex)
        //{
        //    //lblmsg.Text = ex.Message;
        //}
        //finally
        //{
        //    conn.Close();
        //}

    }


    //FUNCTION FOR fnSendMail for CANCEL
    private void fnSendMail()
    {
        SqlConnection conn = DBConnection_ROS.GetConnection();
        SqlConnection conn1 = DBConnection_ROS.GetConnection();

        SqlCommand dataCommand = new SqlCommand();
        dataCommand.Connection = conn;
        SqlCommand dataCommand1 = new SqlCommand();
        dataCommand1.Connection = conn1;

        dataCommand.CommandType = CommandType.Text;
        dataCommand1.CommandType = CommandType.Text;

        int iSl = 1;
        //string tDate = DateTime.Today.ToString("dd/MM/yyyy");
        //string tDate = DateTime.Today.ToString();
        string tDate = string.Format("{0:D}", DateTime.Today);
        string tTime = DateTime.Now.ToString("T");

        //-----------------------------------------------------------------------------------------------------
        // Mail to Admin---------------------------------------------------------------------------------------
        MailMessage mM = new MailMessage();
        //mM.From = new MailAddress(txtEmail.Text);        
        ////mM.From = new MailAddress("online@noorrestaurant.co.uk");
        ////mM.To.Add(new MailAddress("online@noorrestaurant.co.uk"));

        //mM.From = new MailAddress("rangs.eshop@gmail.com");
        mM.From = new MailAddress("shop@rangs.com.bd");

        if (lblCTPEmail.Text.Length > 0)
        {
            mM.To.Add(new MailAddress(lblCTPEmail.Text));
        }
        else
        {
            mM.To.Add(new MailAddress("sajal@rangs.com.bd"));
        }
        mM.CC.Add(new MailAddress("marketing@rangs.com.bd"));

        //mM.To.Add(new MailAddress("zunayed@rangs.com.bd"));

        //mM.To.Add(new MailAddress("zunayedqu10@gmail.com"));

        //mM.CC.Add(new MailAddress(txtEmail.Text));
        mM.Subject = "Cancel Order No. " + lblInv.Text + " ";
        mM.Body = "<h1 style='color: #FF0000'>Cancel Order</h1>";
        //mM.Body = mM.Body + "<p>You have received a new order with following details:</p>";
        //mM.Body = mM.Body + "<br/>";
        mM.Body = mM.Body + "<p style='color: #FF0000'><b>Order No: " + lblInv.Text + "</b><br/>";
        mM.Body = mM.Body + "Order Date: " + lblDate.Text + "<br/>";
        //mM.Body = mM.Body + "Order Time: " + tTime + "</p>";

        //mM.Body = mM.Body + "<br/>";
        mM.Body = mM.Body + "<p><u>Customer Details:</u><br/> Name: " + lblCustName.Text + "<br/>";
        mM.Body = mM.Body + "Address: " + lblAdd.Text + "<br/>";
        mM.Body = mM.Body + "" + lblThana.Text + ", " + lblDist.Text + "<br/>";
        mM.Body = mM.Body + "Contact: " + lblContact.Text + "<br/>";
        mM.Body = mM.Body + "Email: " + lblEmail.Text + "<br/>";

        //mM.Body = mM.Body + "Delivery Address: " + Session["CustAdd"].ToString() + "</p>";

        //if (Session["DelType1"] != "Collection")
        //{
        //mM.Body = mM.Body + "<u><b>Delivery Address:</b></u><br/>";
        //mM.Body = mM.Body + "" + Session["CustAdd1"].ToString() + "<br/>";
        //mM.Body = mM.Body + "" + Session["CustAdd2"].ToString() + "<br/>";
        //mM.Body = mM.Body + "" + Session["CustPostal"].ToString() + "</p>";
        //}

        //if (Session["DelAdd"].ToString() != null)
        //{
        //    mM.Body = mM.Body + "<p>Shipping Address: " + Session["DelAdd"].ToString() + "</p>";
        //}

        mM.Body = mM.Body + "<br/>";
        mM.Body = mM.Body + "<p><b>Order Details:</b> </p>";

        //------- Start Table ---------------
        mM.Body = mM.Body + "<table style='font-family: arial, sans-serif; border-collapse: collapse; width: 100%;'>";

        mM.Body = mM.Body + "<tr>";
        mM.Body = mM.Body + "<th width='5%' style='border: 1px solid orange; text-align: left; padding: 8px;'>SL#</th>";
        mM.Body = mM.Body + "<th style='border: 1px solid orange; text-align: left; padding: 8px;'>Item Name</th>";
        mM.Body = mM.Body + "<th width='15%' style='border: 1px solid orange; text-align: left; padding: 8px;'>Qty</th>";
        mM.Body = mM.Body + "<th width='15%' style='border: 1px solid orange; text-align: left; padding: 8px;'>Amount (&#2547;)</th>";
        mM.Body = mM.Body + "</tr>";

        //-----------------------------------------------------------------------------
        string sSql = "";
        //sSql = "SELECT tbBaskets.ProductID, tbBaskets.ProductName, tbProduct.thum, tbProduct.titleDesc,";
        //sSql = sSql + " abs(tbBaskets.tQty) AS tQty, tbBaskets.SalePrice, ";
        //sSql = sSql + " Format(abs(tbBaskets.tQty) * tbBaskets.SalePrice,'0.00')  AS TAmnt";
        //sSql = sSql + " FROM tbBaskets INNER JOIN tbProduct ON tbBaskets.ProductID = tbProduct.ProductID";
        //sSql = sSql + " WHERE tbBaskets.SessionNo='" + Session["sid"] + "'";
        //sSql = sSql + " ORDER BY tbBaskets.ProductName";

        sSql = "";
        sSql = "SELECT dbo.tbCustomerDelDetails.DelDID, dbo.tbCustomerDelDetails.DelID, dbo.tbCustomerDelDetails.ProductID,";
        sSql = sSql + " dbo.tbProduct.title, dbo.tbProduct.titleDesc, dbo.tbCustomerDelDetails.SalePrice,";
        sSql = sSql + " dbo.tbCustomerDelDetails.tQty, dbo.tbCustomerDelDetails.tAmnt";
        sSql = sSql + " FROM dbo.tbCustomerDelDetails INNER JOIN";
        sSql = sSql + " dbo.tbProduct ON dbo.tbCustomerDelDetails.ProductID = dbo.tbProduct.ProductID";
        sSql = sSql + " WHERE (dbo.tbCustomerDelDetails.DelID = '" + this.lblID.Text + "')";


        SqlCommand cmd = new SqlCommand(sSql, conn);
        dataCommand.CommandText = sSql;

        conn.Open();
        SqlDataReader dr2 = dataCommand.ExecuteReader();
        while (dr2.Read())
        {
            mM.Body = mM.Body + "<tr>";
            mM.Body = mM.Body + "<td style='border: 1px solid orange; text-align: left; padding: 8px;'>" + iSl + ". </td>";
            mM.Body = mM.Body + "<td style='border: 1px solid orange; text-align: left; padding: 8px;'>" + dr2["title"].ToString() + " (" + dr2["titleDesc"].ToString() + ")</td>";
            mM.Body = mM.Body + "<td align='Right' style='border: 1px solid orange; text-align: left; padding: 8px;'>" + dr2["tQty"].ToString() + "</td>";
            mM.Body = mM.Body + "<td align='Right' style='border: 1px solid orange; text-align: left; padding: 8px;'>" + dr2["TAmnt"].ToString() + "</td>";
            mM.Body = mM.Body + "</tr>";
            iSl = iSl + 1;
        }
        //dataCommand.ExecuteNonQuery();
        conn.Close();
        //-------------------------------------------------------------------------------------

        //mM.Body = mM.Body + "<tr><td>Booking Date/time:</td><td>" + string.Format("{0:D}", tfDate) + " at " + ddlTime.SelectedItem.Text + "</td></tr>";
        //mM.Body = mM.Body + "<tr><td>Secial Notes/Comments:</td><td>" + txtNote.Text + "</td></tr>";
        mM.Body = mM.Body + "</table>";


        //mM.Body = mM.Body + "<p>Sub Total: &#2547; " + lblTotalAmnt.Text + "<br/>";
        //mM.Body = mM.Body + "Shipping Charge: &#2547; " + lblShipping.Text + "<br/>";
        //mM.Body = mM.Body + "Tax Amount: &#2547; " + lblTax.Text + "<br/>";
        mM.Body = mM.Body + "Total Amount: &#2547; " + lblTotalAmnt.Text + "<br/>";
        mM.Body = mM.Body + "Payment Type: " + lblPaymentMethod.Text + "<br/>";
        mM.Body = mM.Body + "Delivery Type: " + lblDelType.Text + "<br/>";
        mM.Body = mM.Body + "Delivery From: " + lblEName.Text + "</p>";
        mM.Body = mM.Body + "<p>&nbsp;</p>";

        mM.Body = mM.Body + "<p>";
        mM.Body = mM.Body + "Kind Regards, <br/> ";
        mM.Body = mM.Body + "<a href='http://shop.rangs.com.bd/'>Rangs Online Store</a>";
        mM.Body = mM.Body + "</p>";

        mM.IsBodyHtml = true;
        mM.Priority = MailPriority.High;
        SmtpClient sC = new SmtpClient("mail.rangs.com.bd");
        sC.Port = 587;
        //sC.Port = 25;
        sC.Credentials = new System.Net.NetworkCredential("shop@rangs.com.bd", "Exampass@321");
        //sC.EnableSsl = true;
        sC.Send(mM);

        //----------------------------------------------------------------------------------------
        //mM.IsBodyHtml = true;
        //SmtpClient smtp = new SmtpClient();
        //smtp.Host = "smtp.gmail.com";
        //smtp.Credentials = new System.Net.NetworkCredential
        //     ("rangs.eshop@gmail.com", "Admin@321");

        //smtp.Port = 587;
        //smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
        //smtp.UseDefaultCredentials = false;
        //smtp.EnableSsl = true;
        //smtp.Send(mM);       
        //----------------------------------------------------------------------------------------

        //----------------------------------------------------------------------------------------


        //****************************************************************************************
        //-----------------------------------------------------------------------------------------------------
        // Mail to Customer------------------------------------------------------------------------------------

        MailMessage mM2 = new MailMessage();
        //mM2.From = new MailAddress(txtEmail.Text);        

        //mM2.From = new MailAddress("rangs.eshop@gmail.com");
        mM2.From = new MailAddress("shop@rangs.com.bd");


        //mM2.To.Add(new MailAddress(Session["sEmail"].ToString()));
        
        mM2.To.Add(new MailAddress(lblEmail.Text));
        //mM2.To.Add(new MailAddress("zunayed@rangs.com.bd"));
        //mM2.CC.Add(new MailAddress(txtEmail.Text));
        //mM2.Bcc.Add(new MailAddress("zunayedqu10@gmail.com"));

        mM2.Subject = "Cancel Your order with Sony-Rangs";

        mM2.Body = "<h1 style='color: #FF0000'>Cancel Order</h1>";
        mM2.Body = mM2.Body + "<p>Dear Valued Customer,</p>";
        mM2.Body = mM2.Body + "<p>Greeting from Sony-Rangs Online Store.<br/>";
        mM2.Body = mM2.Body + "Your order has been canceled.";
        //mM2.Body = mM2.Body + "as soon as possible. You will be updated about next step immediate.";
        mM2.Body = mM2.Body + "</p>";
               
        mM2.Body = mM2.Body + "<p>";
        mM2.Body = mM2.Body + "<b>Your order details: </b>";
        mM2.Body = mM2.Body + "</p>";
        //--------
        //mM2.Body = mM2.Body + "<p>";
        //mM2.Body = mM2.Body + "Your order will be on its way very shortly, in the meantime please check below ";
        //mM2.Body = mM2.Body + "to ensure we have the correct details for your order.";
        //mM2.Body = mM2.Body + "</p>";

        //mM2.Body = mM2.Body + "<br/>";
        mM2.Body = mM2.Body + "<p><b>Order No: " + lblInv.Text + "</b><br/>";
        mM2.Body = mM2.Body + "Order Date: " + lblDate.Text + "</p>";

        //mM2.Body = mM2.Body + "<br/>";
        mM2.Body = mM2.Body + "<p><u>Your Details:</u><br/> Name: " + lblCustName.Text + "<br/>";
        mM2.Body = mM2.Body + "Contact # " + lblContact.Text + "<br/>";
        mM2.Body = mM2.Body + "Email: " + lblEmail.Text + "<br/>";
        mM2.Body = mM2.Body + "Address: " + lblAdd.Text + "</p>";

        //if (Session["DelType1"] != "Collection")
        //{
        //    mM2.Body = mM2.Body + "<u><b>Delivery Address:</b></u><br/>";
        //    mM2.Body = mM2.Body + "" + Session["CustAdd1"].ToString() + "<br/>";
        //    mM2.Body = mM2.Body + "" + Session["CustAdd2"].ToString() + "<br/>";
        //    mM2.Body = mM2.Body + "" + Session["CustPostal"].ToString() + "</p>";
        //}

        //if (Session["DelAdd"].ToString() != null)
        //{
        //    mM2.Body = mM2.Body + "<p>Shipping Address: " + Session["DelAdd"].ToString() + "</p>";
        //}

        //mM2.Body = mM2.Body + "<br/>";
        //mM2.Body = mM2.Body + "<p><b>Order Details:</b> </p>";

        //------- Start Table ---------------
        mM2.Body = mM2.Body + "<table style='font-family: arial, sans-serif; border-collapse: collapse; width: 100%;'>";

        mM2.Body = mM2.Body + "<tr>";
        mM2.Body = mM2.Body + "<th width='5%' style='border: 1px solid orange; text-align: left; padding: 8px;'>SL#</th>";
        mM2.Body = mM2.Body + "<th style='border: 1px solid orange; text-align: left; padding: 8px;'>Item Name</th>";
        mM2.Body = mM2.Body + "<th width='15%' style='border: 1px solid orange; text-align: left; padding: 8px;'>Qty</th>";
        mM2.Body = mM2.Body + "<th width='15%' style='border: 1px solid orange; text-align: left; padding: 8px;'>Amount (&#2547;)</th>";
        mM2.Body = mM2.Body + "</tr>";

        //-----------------------------------------------------------------------------
        //sSql = "";
        //sSql = "SELECT tbBaskets.ProductID, tbBaskets.ProductName, tbProduct.thum, tbProduct.titleDesc,";
        //sSql = sSql + " abs(tbBaskets.tQty) AS tQty, tbBaskets.SalePrice, ";
        //sSql = sSql + " Format(abs(tbBaskets.tQty) * tbBaskets.SalePrice,'0.00')  AS TAmnt";
        //sSql = sSql + " FROM tbBaskets INNER JOIN tbProduct ON tbBaskets.ProductID = tbProduct.ProductID";
        //sSql = sSql + " WHERE tbBaskets.SessionNo='" + Session["sid"] + "'";
        //sSql = sSql + " ORDER BY tbBaskets.ProductName";

        sSql = "";
        sSql = "SELECT dbo.tbCustomerDelDetails.DelDID, dbo.tbCustomerDelDetails.DelID, dbo.tbCustomerDelDetails.ProductID,";
        sSql = sSql + " dbo.tbProduct.title, dbo.tbProduct.titleDesc, dbo.tbCustomerDelDetails.SalePrice,";
        sSql = sSql + " dbo.tbCustomerDelDetails.tQty, dbo.tbCustomerDelDetails.tAmnt";
        sSql = sSql + " FROM dbo.tbCustomerDelDetails INNER JOIN";
        sSql = sSql + " dbo.tbProduct ON dbo.tbCustomerDelDetails.ProductID = dbo.tbProduct.ProductID";
        sSql = sSql + " WHERE (dbo.tbCustomerDelDetails.DelID = '" + this.lblID.Text + "')";

        SqlCommand cmd1 = new SqlCommand(sSql, conn1);
        dataCommand1.CommandText = sSql;

        iSl = 1;
        conn1.Open();
        SqlDataReader dr = dataCommand1.ExecuteReader();
        while (dr.Read())
        {
            mM2.Body = mM2.Body + "<tr>";
            mM2.Body = mM2.Body + "<td style='border: 1px solid orange; text-align: left; padding: 8px;'>" + iSl + ". </td>";
            //mM2.Body = mM2.Body + "<td style='border: 1px solid orange; text-align: left; padding: 8px;'>" + dr["ProductName"].ToString() + "</td>";
            mM2.Body = mM2.Body + "<td style='border: 1px solid orange; text-align: left; padding: 8px;'>" + dr["title"].ToString() + " (" + dr["titleDesc"].ToString() + ")</td>";
            mM2.Body = mM2.Body + "<td align='Right' style='border: 1px solid orange; text-align: left; padding: 8px;'>" + dr["tQty"].ToString() + "</td>";
            mM2.Body = mM2.Body + "<td align='Right' style='border: 1px solid orange; text-align: left; padding: 8px;'>" + dr["TAmnt"].ToString() + "</td>";
            mM2.Body = mM2.Body + "</tr>";
            iSl = iSl + 1;
        }
        //dataCommand1.ExecuteNonQuery();
        conn1.Close();
        //-------------------------------------------------------------------------------------

        //mM2.Body = mM2.Body + "<tr><td>Booking Date/time:</td><td>" + string.Format("{0:D}", tfDate) + " at " + ddlTime.SelectedItem.Text + "</td></tr>";
        //mM2.Body = mM2.Body + "<tr><td>Secial Notes/Comments:</td><td>" + txtNote.Text + "</td></tr>";
        mM2.Body = mM2.Body + "</table>";

        //mM2.Body = mM2.Body + "<p>Sub Total: &#2547; " + lblCartTotal.Text + "<br/>";
        //mM2.Body = mM2.Body + "Shipping Fee: &#2547; " + lblShipping.Text + "<br/>";
        //mM2.Body = mM2.Body + "Tax Amount: &#2547; " + lblTax.Text + "<br/>";
        mM2.Body = mM2.Body + "Total Amount: &#2547; " + lblTotalAmnt.Text + "<br/>";
        mM2.Body = mM2.Body + "Payment Type: " + lblPaymentMethod.Text + "<br/>";
        mM2.Body = mM2.Body + "Delivery Type: " + lblDelType.Text + "<br/>";
        mM2.Body = mM2.Body + "Delivery From: " + lblEName.Text + "</p>";
        mM2.Body = mM2.Body + "<p>&nbsp;</p>";
        
        mM2.Body = mM2.Body + "<p>";
        mM2.Body = mM2.Body + "If you have any further enquiries <br/> please do contact us at - <br/> ";

        mM2.Body = mM2.Body + "<a href='mailto:marketing@rangs.com.bd'>marketing@rangs.com.bd</a>";
        mM2.Body = mM2.Body + "</p>";

        mM2.Body = mM2.Body + "<p>";
        mM2.Body = mM2.Body + "Kind Regards, <br/> ";
        mM2.Body = mM2.Body + "<a href='http://shop.rangs.com.bd/'>Rangs Online Store</a>";
        mM2.Body = mM2.Body + "</p>";

        mM2.IsBodyHtml = true;
        mM2.Priority = MailPriority.High;
        SmtpClient sC1 = new SmtpClient("mail.rangs.com.bd");
        sC1.Port = 587;
        //sC.Port = 2525;
        sC1.Credentials = new System.Net.NetworkCredential("shop@rangs.com.bd", "Exampass@321");
        //sC.EnableSsl = true;
        sC1.Send(mM2);


        //----------------------------------------------------------------------------------------
        //mM2.IsBodyHtml = true;
        //SmtpClient smtp2 = new SmtpClient();
        //smtp2.Host = "smtp.gmail.com";
        //smtp2.Credentials = new System.Net.NetworkCredential
        //     ("rangs.eshop@gmail.com", "Admin@321");

        //smtp.Port = 587;
        //smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
        //smtp.UseDefaultCredentials = false;

        //smtp2.EnableSsl = true;
        //smtp2.Send(mM2);
        //----------------------------------------------------------------------------------------

    }


    //FUNCTION FOR RE-SEND MAIL
    private void fnSendMail_Resend()
    {
        SqlConnection conn = DBConnection_ROS.GetConnection();
        SqlConnection conn1 = DBConnection_ROS.GetConnection();
        
        SqlCommand dataCommand = new SqlCommand();
        dataCommand.Connection = conn;
        SqlCommand dataCommand1 = new SqlCommand();
        dataCommand1.Connection = conn1;

        dataCommand.CommandType = CommandType.Text;
        dataCommand1.CommandType = CommandType.Text;

        int iSl = 1;
        //string tDate = DateTime.Today.ToString("dd/MM/yyyy");
        //string tDate = DateTime.Today.ToString();
        string tDate = string.Format("{0:D}", DateTime.Today);
        string tTime = DateTime.Now.ToString("T");

        //-----------------------------------------------------------------------------------------------------
        // Mail to Admin---------------------------------------------------------------------------------------
        MailMessage mM = new MailMessage();
        //mM.From = new MailAddress(txtEmail.Text);        
        ////mM.From = new MailAddress("online@noorrestaurant.co.uk");
        ////mM.To.Add(new MailAddress("online@noorrestaurant.co.uk"));

        //mM.From = new MailAddress("rangs.eshop@gmail.com");
        mM.From = new MailAddress("shop@rangs.com.bd");

        if (lblCTPEmail.Text.Length > 0)
        {
            mM.To.Add(new MailAddress(lblCTPEmail.Text));
        }
        else
        {
            mM.To.Add(new MailAddress("sajal@rangs.com.bd"));
        }
        mM.CC.Add(new MailAddress("marketing@rangs.com.bd"));



        //mM.To.Add(new MailAddress("zunayedqu10@gmail.com"));

        //mM.CC.Add(new MailAddress(txtEmail.Text));
        mM.Subject = "New Order No. " + lblInv.Text + " ";
        mM.Body = "<h1>Order Details</h1>";
        mM.Body = mM.Body + "<p>You have received a new order with following details:</p>";
        //mM.Body = mM.Body + "<br/>";
        mM.Body = mM.Body + "<p><b>Order No: " + lblInv.Text + "</b><br/>";
        mM.Body = mM.Body + "Order Date: " + lblDate.Text + "<br/>";
        //mM.Body = mM.Body + "Order Time: " + tTime + "</p>";

        //mM.Body = mM.Body + "<br/>";
        mM.Body = mM.Body + "<p><u>Customer Details:</u><br/> Name: " + lblCustName.Text + "<br/>";
        mM.Body = mM.Body + "Address: " + lblAdd.Text + "<br/>";
        mM.Body = mM.Body + "" + lblThana.Text + ", " + lblDist.Text + "<br/>";
        mM.Body = mM.Body + "Contact: " + lblContact.Text + "<br/>";
        mM.Body = mM.Body + "Email: " + lblEmail.Text + "<br/>";

        //mM.Body = mM.Body + "Delivery Address: " + Session["CustAdd"].ToString() + "</p>";

        //if (Session["DelType1"] != "Collection")
        //{
        //mM.Body = mM.Body + "<u><b>Delivery Address:</b></u><br/>";
        //mM.Body = mM.Body + "" + Session["CustAdd1"].ToString() + "<br/>";
        //mM.Body = mM.Body + "" + Session["CustAdd2"].ToString() + "<br/>";
        //mM.Body = mM.Body + "" + Session["CustPostal"].ToString() + "</p>";
        //}

        //if (Session["DelAdd"].ToString() != null)
        //{
        //    mM.Body = mM.Body + "<p>Shipping Address: " + Session["DelAdd"].ToString() + "</p>";
        //}

        mM.Body = mM.Body + "<br/>";
        mM.Body = mM.Body + "<p><b>Order Details:</b> </p>";

        //------- Start Table ---------------
        mM.Body = mM.Body + "<table style='font-family: arial, sans-serif; border-collapse: collapse; width: 100%;'>";

        mM.Body = mM.Body + "<tr>";
        mM.Body = mM.Body + "<th width='5%' style='border: 1px solid orange; text-align: left; padding: 8px;'>SL#</th>";
        mM.Body = mM.Body + "<th style='border: 1px solid orange; text-align: left; padding: 8px;'>Item Name</th>";
        mM.Body = mM.Body + "<th width='15%' style='border: 1px solid orange; text-align: left; padding: 8px;'>Qty</th>";
        mM.Body = mM.Body + "<th width='15%' style='border: 1px solid orange; text-align: left; padding: 8px;'>Amount (&#2547;)</th>";
        mM.Body = mM.Body + "</tr>";

        //-----------------------------------------------------------------------------
        string sSql = "";
        sSql = "SELECT dbo.tbCustomerDelDetails.DelDID, dbo.tbCustomerDelDetails.DelID, dbo.tbCustomerDelDetails.ProductID,";
        sSql = sSql + " dbo.tbProduct.title, dbo.tbProduct.titleDesc, dbo.tbCustomerDelDetails.SalePrice,";
        sSql = sSql + " dbo.tbCustomerDelDetails.tQty, dbo.tbCustomerDelDetails.tAmnt";
        sSql = sSql + " FROM dbo.tbCustomerDelDetails INNER JOIN";
        sSql = sSql + " dbo.tbProduct ON dbo.tbCustomerDelDetails.ProductID = dbo.tbProduct.ProductID";
        sSql = sSql + " WHERE (dbo.tbCustomerDelDetails.DelID = '" + this.lblID.Text + "')";

        SqlCommand cmd = new SqlCommand(sSql, conn);
        dataCommand.CommandText = sSql;

        conn.Open();
        SqlDataReader dr2 = dataCommand.ExecuteReader();
        while (dr2.Read())
        {
            mM.Body = mM.Body + "<tr>";
            mM.Body = mM.Body + "<td style='border: 1px solid orange; text-align: left; padding: 8px;'>" + iSl + ". </td>";
            mM.Body = mM.Body + "<td style='border: 1px solid orange; text-align: left; padding: 8px;'>" + dr2["title"].ToString() + " (" + dr2["titleDesc"].ToString() + ")</td>";
            mM.Body = mM.Body + "<td align='Right' style='border: 1px solid orange; text-align: left; padding: 8px;'>" + dr2["tQty"].ToString() + "</td>";
            mM.Body = mM.Body + "<td align='Right' style='border: 1px solid orange; text-align: left; padding: 8px;'>" + dr2["TAmnt"].ToString() + "</td>";
            mM.Body = mM.Body + "</tr>";
            iSl = iSl + 1;
        }
        //dataCommand.ExecuteNonQuery();
        conn.Close();
        //-------------------------------------------------------------------------------------

        //mM.Body = mM.Body + "<tr><td>Booking Date/time:</td><td>" + string.Format("{0:D}", tfDate) + " at " + ddlTime.SelectedItem.Text + "</td></tr>";
        //mM.Body = mM.Body + "<tr><td>Secial Notes/Comments:</td><td>" + txtNote.Text + "</td></tr>";
        mM.Body = mM.Body + "</table>";


        mM.Body = mM.Body + "<p>Sub Total: &#2547; " + lblTotalAmnt.Text + "<br/>";
        mM.Body = mM.Body + "Shipping Charge: &#2547; " + lblShipping.Text + "<br/>";
        mM.Body = mM.Body + "Tax Amount: &#2547; " + lblTax.Text + "<br/>";
        mM.Body = mM.Body + "Total Amount: &#2547; " + lblTotalAmnt.Text + "<br/>";
        mM.Body = mM.Body + "Payment Type: " + lblPaymentMethod.Text + "<br/>";
        mM.Body = mM.Body + "Delivery Type: " + lblDelType.Text + "<br/>";
        mM.Body = mM.Body + "Delivery From: " + lblEName.Text + " (" + lblCTPAdd.Text + ") " + "</p>";
        mM.Body = mM.Body + "<p>&nbsp;</p>";

        mM.Body = mM.Body + "<p>";
        mM.Body = mM.Body + "Kind Regards, <br/> ";
        mM.Body = mM.Body + "<a href='http://shop.rangs.com.bd/'>Rangs Online Store</a>";
        mM.Body = mM.Body + "</p>";

        mM.IsBodyHtml = true;
        mM.Priority = MailPriority.High;
        SmtpClient sC = new SmtpClient("mail.rangs.com.bd");
        sC.Port = 587;
        //sC.Port = 25;
        sC.Credentials = new System.Net.NetworkCredential("shop@rangs.com.bd", "Exampass@321");
        //sC.EnableSsl = true;
        sC.Send(mM);

        //----------------------------------------------------------------------------------------
        //mM.IsBodyHtml = true;
        //SmtpClient smtp = new SmtpClient();
        //smtp.Host = "smtp.gmail.com";
        //smtp.Credentials = new System.Net.NetworkCredential
        //     ("rangs.eshop@gmail.com", "Admin@321");

        //smtp.Port = 587;
        //smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
        //smtp.UseDefaultCredentials = false;
        //smtp.EnableSsl = true;
        //smtp.Send(mM);       
        //----------------------------------------------------------------------------------------

        //----------------------------------------------------------------------------------------


        //****************************************************************************************
        //-----------------------------------------------------------------------------------------------------
        // Mail to Customer------------------------------------------------------------------------------------

        MailMessage mM2 = new MailMessage();
        //mM2.From = new MailAddress(txtEmail.Text);        

        //mM2.From = new MailAddress("rangs.eshop@gmail.com");
        mM2.From = new MailAddress("shop@rangs.com.bd");


        //mM2.To.Add(new MailAddress(Session["sEmail"].ToString()));
        mM2.To.Add(new MailAddress(lblEmail.Text));
        //mM2.CC.Add(new MailAddress(txtEmail.Text));
        mM2.Bcc.Add(new MailAddress("zunayedqu10@gmail.com"));

        mM2.Subject = "Your order with Sony-Rangs";
        //mM2.Body = "<h1>Order Details</h1>";
        mM2.Body = "<p>Dear Valued Customer,</p>";
        mM2.Body = mM2.Body + "<p>Thank you for your order with us.<br/>";
        mM2.Body = mM2.Body + "We really appreciate it and we are taking necessary steps to process this order.";
        //mM2.Body = mM2.Body + "as soon as possible. You will be updated about next step immediate.";
        mM2.Body = mM2.Body + "</p>";

        //--------
        mM2.Body = mM2.Body + "<p>";
        mM2.Body = mM2.Body + "Please take note that this special pricing & offer valid as per T&C of this unique promotion , published in newspaper & online store promotion banner display under Covid-19 Free Showroom, Special Offer.";
        mM2.Body = mM2.Body + "</p>";

        mM2.Body = mM2.Body + "<p>";
        //mM2.Body = mM2.Body + "1. This Pricing is valid from 28th March till 11th April or until Country Quarantine situation is over.";
        mM2.Body = mM2.Body + "1. This pricing is valid only for when you place the order, authority keeps the right to change products price at any time without giving any notice.";
        //mM2.Body = mM2.Body + "1. This Pricing is valid from after Country Quarantine situation is over.";
        mM2.Body = mM2.Body + "</p>";

        mM2.Body = mM2.Body + "<p>";
        mM2.Body = mM2.Body + "2. This offer  is valid for Cash on Delivery from your nearest showroom.<br/>";
        mM2.Body = mM2.Body + "You are kindly require to visit nearest showroom as per next mail/call / direction send to you to verify your item within 3 working days after quarantine situation is over.";
        mM2.Body = mM2.Body + "</p>";

        mM2.Body = mM2.Body + "<p>";
        mM2.Body = mM2.Body + "3. To get that special pricing , you have to produce order detail / number / mail from Rangs online store.";
        mM2.Body = mM2.Body + "</p>";

        mM2.Body = mM2.Body + "<p>";
        mM2.Body = mM2.Body + "4. You have to clear your payments in that showroom after verification & full satisfaction for your ordered items.";
        mM2.Body = mM2.Body + "</p>";

        mM2.Body = mM2.Body + "<p>";
        mM2.Body = mM2.Body + "5. We will start delivery process instantly once payment received in that designated showroom.";
        mM2.Body = mM2.Body + "</p>";

        mM2.Body = mM2.Body + "<p>";
        mM2.Body = mM2.Body + "6. For small , handy items , you may instantly take or can drop for delivery by us max in 7 days.";
        mM2.Body = mM2.Body + "</p>";

        mM2.Body = mM2.Body + "<p>";
        mM2.Body = mM2.Body + "7. For bulky items , we will arrange delivery & installation in your preferred location.";
        mM2.Body = mM2.Body + "</p>";

        mM2.Body = mM2.Body + "<p>";
        mM2.Body = mM2.Body + "<b>Here is your order details - </b>";
        mM2.Body = mM2.Body + "</p>";
        //--------
        //mM2.Body = mM2.Body + "<p>";
        //mM2.Body = mM2.Body + "Your order will be on its way very shortly, in the meantime please check below ";
        //mM2.Body = mM2.Body + "to ensure we have the correct details for your order.";
        //mM2.Body = mM2.Body + "</p>";

        //mM2.Body = mM2.Body + "<br/>";
        mM2.Body = mM2.Body + "<p><b>Order No: " + lblInv.Text + "</b><br/>";
        mM2.Body = mM2.Body + "Order Date: " + lblDate.Text + "</p>";

        //mM2.Body = mM2.Body + "<br/>";
        mM2.Body = mM2.Body + "<p><u>Your Details:</u><br/> Name: " + lblCustName.Text + "<br/>";
        mM2.Body = mM2.Body + "Contact # " + lblContact.Text + "<br/>";
        mM2.Body = mM2.Body + "Email: " + lblEmail.Text + "<br/>";
        mM2.Body = mM2.Body + "Address: " + lblAdd.Text + "</p>";

        //if (Session["DelType1"] != "Collection")
        //{
        //    mM2.Body = mM2.Body + "<u><b>Delivery Address:</b></u><br/>";
        //    mM2.Body = mM2.Body + "" + Session["CustAdd1"].ToString() + "<br/>";
        //    mM2.Body = mM2.Body + "" + Session["CustAdd2"].ToString() + "<br/>";
        //    mM2.Body = mM2.Body + "" + Session["CustPostal"].ToString() + "</p>";
        //}

        //if (Session["DelAdd"].ToString() != null)
        //{
        //    mM2.Body = mM2.Body + "<p>Shipping Address: " + Session["DelAdd"].ToString() + "</p>";
        //}

        //mM2.Body = mM2.Body + "<br/>";
        mM2.Body = mM2.Body + "<p><b>Order Details:</b> </p>";

        //------- Start Table ---------------
        mM2.Body = mM2.Body + "<table style='font-family: arial, sans-serif; border-collapse: collapse; width: 100%;'>";

        mM2.Body = mM2.Body + "<tr>";
        mM2.Body = mM2.Body + "<th width='5%' style='border: 1px solid orange; text-align: left; padding: 8px;'>SL#</th>";
        mM2.Body = mM2.Body + "<th style='border: 1px solid orange; text-align: left; padding: 8px;'>Item Name</th>";
        mM2.Body = mM2.Body + "<th width='15%' style='border: 1px solid orange; text-align: left; padding: 8px;'>Qty</th>";
        mM2.Body = mM2.Body + "<th width='15%' style='border: 1px solid orange; text-align: left; padding: 8px;'>Amount (&#2547;)</th>";
        mM2.Body = mM2.Body + "</tr>";

        //-----------------------------------------------------------------------------
        sSql = "";
        sSql = "SELECT dbo.tbCustomerDelDetails.DelDID, dbo.tbCustomerDelDetails.DelID, dbo.tbCustomerDelDetails.ProductID,";
        sSql = sSql + " dbo.tbProduct.title, dbo.tbProduct.titleDesc, dbo.tbCustomerDelDetails.SalePrice,";
        sSql = sSql + " dbo.tbCustomerDelDetails.tQty, dbo.tbCustomerDelDetails.tAmnt";
        sSql = sSql + " FROM dbo.tbCustomerDelDetails INNER JOIN";
        sSql = sSql + " dbo.tbProduct ON dbo.tbCustomerDelDetails.ProductID = dbo.tbProduct.ProductID";
        sSql = sSql + " WHERE (dbo.tbCustomerDelDetails.DelID = '" + this.lblID.Text + "')";

        SqlCommand cmd1 = new SqlCommand(sSql, conn1);
        dataCommand1.CommandText = sSql;

        iSl = 1;
        conn1.Open();
        SqlDataReader dr = dataCommand1.ExecuteReader();
        while (dr.Read())
        {
            mM2.Body = mM2.Body + "<tr>";
            mM2.Body = mM2.Body + "<td style='border: 1px solid orange; text-align: left; padding: 8px;'>" + iSl + ". </td>";
            //mM2.Body = mM2.Body + "<td style='border: 1px solid orange; text-align: left; padding: 8px;'>" + dr["ProductName"].ToString() + "</td>";
            mM2.Body = mM2.Body + "<td style='border: 1px solid orange; text-align: left; padding: 8px;'>" + dr["title"].ToString() + " (" + dr["titleDesc"].ToString() + ")</td>";
            mM2.Body = mM2.Body + "<td align='Right' style='border: 1px solid orange; text-align: left; padding: 8px;'>" + dr["tQty"].ToString() + "</td>";
            mM2.Body = mM2.Body + "<td align='Right' style='border: 1px solid orange; text-align: left; padding: 8px;'>" + dr["TAmnt"].ToString() + "</td>";
            mM2.Body = mM2.Body + "</tr>";
            iSl = iSl + 1;
        }
        //dataCommand1.ExecuteNonQuery();
        conn1.Close();
        //-------------------------------------------------------------------------------------

        //mM2.Body = mM2.Body + "<tr><td>Booking Date/time:</td><td>" + string.Format("{0:D}", tfDate) + " at " + ddlTime.SelectedItem.Text + "</td></tr>";
        //mM2.Body = mM2.Body + "<tr><td>Secial Notes/Comments:</td><td>" + txtNote.Text + "</td></tr>";
        mM2.Body = mM2.Body + "</table>";

        mM2.Body = mM2.Body + "<p>Sub Total: &#2547; " + lblTotalAmnt.Text + "<br/>";
        mM2.Body = mM2.Body + "Shipping Fee: &#2547; " + lblShipping.Text + "<br/>";
        mM2.Body = mM2.Body + "Tax Amount: &#2547; " + lblTax.Text + "<br/>";
        mM2.Body = mM2.Body + "Total Amount: &#2547; " + lblTotalAmnt.Text + "<br/>";
        mM2.Body = mM2.Body + "Payment Type: " + lblPaymentMethod.Text + "<br/>";
        mM2.Body = mM2.Body + "Delivery Type: " + lblDelType.Text + "<br/>";
        mM2.Body = mM2.Body + "Delivery From: " + lblEName.Text + " (" + lblCTPAdd.Text + ") " + "</p>";
        mM2.Body = mM2.Body + "<p>&nbsp;</p>";


        //mM2.Body = mM2.Body + "<p>";
        //mM2.Body = mM2.Body + "Once item(s) has been sent out for your order another email will be sent to you ";
        //mM2.Body = mM2.Body + "to confirm the dispatch along with the tracking details of this order.";
        //mM2.Body = mM2.Body + "</p>";
        mM2.Body = mM2.Body + "<p>";
        mM2.Body = mM2.Body + "Our next Step will be executed once you visit our nearest showroom, verify your product & settle the payments.";
        mM2.Body = mM2.Body + "</p>";

        mM2.Body = mM2.Body + "<p>";
        mM2.Body = mM2.Body + "If you do not visit within next 3 opening days after quarantine situation over, your order will be treated invalid & we will have no obligation to deliver the item at that special price , you have to pay regular price to get the items after defined period.";
        mM2.Body = mM2.Body + "</p>";

        mM2.Body = mM2.Body + "<p>";
        mM2.Body = mM2.Body + "In case you failed to visit within 3 days after country’s quarantine situation, you may send us email with reason & order details.";
        mM2.Body = mM2.Body + "</p>";


        mM2.Body = mM2.Body + "<p>";
        mM2.Body = mM2.Body + "If you have any further enquiries <br/> please do contact us at - <br/> ";

        mM2.Body = mM2.Body + "<a href='mailto:marketing@rangs.com.bd'>marketing@rangs.com.bd</a>";
        mM2.Body = mM2.Body + "</p>";

        mM2.Body = mM2.Body + "<p>";
        mM2.Body = mM2.Body + "Kind Regards, <br/> ";
        mM2.Body = mM2.Body + "<a href='http://www.rangs.com.bd/'>Rangs Electronics Ltd</a>";
        mM2.Body = mM2.Body + "</p>";

        mM2.IsBodyHtml = true;
        mM2.Priority = MailPriority.High;
        SmtpClient sC1 = new SmtpClient("mail.rangs.com.bd");
        sC1.Port = 587;
        //sC.Port = 2525;
        sC1.Credentials = new System.Net.NetworkCredential("shop@rangs.com.bd", "Exampass@321");
        //sC.EnableSsl = true;
        sC1.Send(mM2);


        //----------------------------------------------------------------------------------------
        //mM2.IsBodyHtml = true;
        //SmtpClient smtp2 = new SmtpClient();
        //smtp2.Host = "smtp.gmail.com";
        //smtp2.Credentials = new System.Net.NetworkCredential
        //     ("rangs.eshop@gmail.com", "Admin@321");

        //smtp.Port = 587;
        //smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
        //smtp.UseDefaultCredentials = false;

        //smtp2.EnableSsl = true;
        //smtp2.Send(mM2);
        //----------------------------------------------------------------------------------------

    }

}
