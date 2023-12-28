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

using System.Net.Mail;

public partial class Forms_Requirement : System.Web.UI.Page
{

    SqlConnection conn = DBConnection.GetConnection();
    SqlConnection conn1 = DBConnection.GetConnection();
    SqlConnection _connStr = DBConnection.GetConnection();
    int iMRSRID = 0;
    DataTable dt;
    DateTime tDate;

    private double runningTotal = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        btnAdd.Attributes.Add("OnClick", "return confirm_Add();");
        btnSave.Attributes.Add("OnClick", "return confirm_Save();");
        btnCancel.Attributes.Add("OnClick", "return confirm_Cancel();");
        //ibtnDelete.Attributes.Add("OnClick", "return confirm_delete();");
        //ddlContinents.Attributes.Add("onChange", "DisplayText();");

        if (System.Convert.ToInt32(Session["Vis"]) == 0)
        {
            Response.Redirect("../Account/Login.aspx");
        }

        if (!IsPostBack)
        {
            dt = new DataTable();
            MakeTable();
            LoadDropDownList();

            //LOAD AUTO REQUEST NUMBER
            fnLoadAutoBillNo();
        }
        else
        {
            dt = (DataTable)ViewState["dt"];
        }
        ViewState["dt"] = dt;

        //txtDate.Text = String.Format("{0:t}", Now);       
        txtDate.Text = DateTime.Today.ToString("MM/dd/yyyy");
        
    }

    protected void fnLoadAutoBillNo()
    {

        SqlConnection con = DBConnection.GetConnection();
        //con.Open();

        int xMax = 0;
        string sAutoNo = "";
        string sSql = "";
        sSql = "SELECT ISNULL(MAX(RIGHT(ReqNo, 5)), 0) AS ReqNo" +
            " FROM dbo.RequirmentMaster" +
            " WHERE (LEFT(ReqNo, 11) = '" + "" + Session["sBrCode"] + "" + DateTime.Now.ToString("yyyy") + "-" + "')";
            //" AND TrType=3";

        SqlCommand cmd = new SqlCommand(sSql, con);
        con.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        try
        {
            if (dr.Read())
            {
                //xMax = dr["JobNo"].ToString();
                xMax = Convert.ToInt32(dr["ReqNo"]) + 1;
                sAutoNo = "" + Session["sBrCode"] + "" + DateTime.Now.ToString("yyyy") + "-" + xMax.ToString("00000");
                txtCHNo.Text = sAutoNo;
            }
            else
            {
                xMax = Convert.ToInt32(dr["ReqNo"]) + 1;
                sAutoNo = "" + Session["sBrCode"] + "" + DateTime.Now.ToString("yyyy") + "-" + xMax.ToString("00000");
                txtCHNo.Text = sAutoNo;
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
            con.Close();
        }
    }

    //LOAD PRODUCT IN DROPDOWN LIST
    protected void LoadDropDownList()
    {
        SqlConnection con = DBConnection.GetConnection();
        
        String strQuery = "select Model from Product WHERE Discontinue='No' Order By Model";
        //SqlConnection con = new SqlConnection("conn");
        SqlCommand cmd = new SqlCommand(strQuery, conn);
        //SqlCommand cmd = new SqlCommand();
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = strQuery;
        cmd.Connection = conn;
        try
        {
            conn.Open();
            ddlContinents.DataSource = cmd.ExecuteReader();
            ddlContinents.DataTextField = "Model";
            //ddlContinents.DataValueField = "ProductID";
            ddlContinents.DataValueField = "Model";
            ddlContinents.DataBind();

            //Add blank item at index 0.
            ddlContinents.Items.Insert(0, new ListItem("", ""));

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

    protected void MakeTable()
    {
        //dt.Columns.Add("ID").AutoIncrement = true;
        dt.Columns.Add("ProductID");
        //dt.Columns.Add("ProductID", typeof(SqlInt32));
        dt.Columns.Add("Model");
        dt.Columns.Add("Qty");        
        dt.Columns.Add("Remarks");

    }

    protected void PopupMessage(string Msg, Control controlID)
    {
        ScriptManager.RegisterClientScriptBlock(controlID, controlID.GetType(), "msg", "alert('" + Msg + "');", true);

    }

    protected void AddRows()
    {
        if (txtProdID.Text == "")
        {
            PopupMessage("Please select product Model.", btnAdd);
            txtProdID.Focus();
            return;
        }

        if (txtQty.Text == "")
        {
            PopupMessage("Please enter Quantity.", btnAdd);
            txtQty.Focus();
            return;
        }

        DataRow dr = dt.NewRow();
        dr["ProductID"] = txtProdID.Text;
        //dr["Model"] = ddlContinents.Text; //Model
        dr["Model"] = ddlContinents.SelectedItem.Text;
        dr["Qty"] = txtQty.Text;        
        dr["Remarks"] = txtRemarks.Text;
        //dr["CampDis"] = dr["MRP"].ToString() - dr["CampaignPrice"].ToString();
        dt.Rows.Add(dr);

        //CLEAR ALL TEXT
        txtProdID.Text = "";
        txtProdDesc.Text = "";
        txtQty.Text = "";        
        txtRemarks.Text = "";

    }

    //ADD DATA IN GRIDVIEW
    protected void btnAdd_Click(object sender, EventArgs e)
    {

        //FUNCTION FOR ADD ROW
        try
        {
            AddRows();
            gvUsers.DataSource = dt;
            gvUsers.DataBind();
        }
        catch (InvalidCastException err)
        {
            throw (err);
        }

    }

    //GRID ROW DELETE
    protected void gvUsers_RowDelating(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(e.RowIndex);
            DataTable dt = ViewState["dt"] as DataTable;
            dt.Rows[index].Delete();
            ViewState["dt"] = dt;
            BindGrid();
        }
        catch (InvalidCastException err)
        {
            throw (err);
        }

    }


    protected void BindGrid()
    {
        gvUsers.DataSource = ViewState["dt"] as DataTable;
        gvUsers.DataBind();
    }

    protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string item = e.Row.Cells[0].Text;
            foreach (Button button in e.Row.Cells[2].Controls.OfType<Button>())
            {
                if (button.CommandName == "Delete")
                {
                    button.Attributes["onclick"] = "if(!confirm('Do you want to delete " + item + "?')){ return false; };";
                }
            }
        }
    }

    //SELECT PRODUCT FROM Drop Down Menu
    protected void ddlContinents_SelectedIndexChanged(object sender, EventArgs e)
    {
        SqlConnection conn = DBConnection.GetConnection();
        string sSql = "";

        sSql = "";
        sSql = "SELECT * FROM Product" +
            " WHERE Model='" + this.ddlContinents.SelectedItem.Text + "'";
        SqlCommand cmd = new SqlCommand(sSql, conn);
        conn.Open();
        SqlDataReader dr = cmd.ExecuteReader();

        try
        {
            if (dr.Read())
            {
                this.txtProdID.Text = dr["ProductID"].ToString();
                this.txtProdDesc.Text = dr["ProdName"].ToString();
                this.txtCode.Text = dr["Code"].ToString();
            }
            else
            {
                this.txtProdDesc.Text = "";
                this.txtCode.Text = "";
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
        
    }

    //CLEAR ALL TEXT AND GRID
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        //CLEAR ALL TEXT
        txtCHNo.Text = "";
        txtDate.Text = "";
        txtDate.Text = DateTime.Today.ToString("dd/MM/yyyy");

        txtProdID.Text = "";        
        txtRemarks.Text = "";

        //CLEAR GRIDVIEW
        gvUsers.DataSource = null;
        gvUsers.DataBind();

        dt.Clear();

        txtCHNo.Focus();

        //LOAD AUTO REQUEST NUMBER
        fnLoadAutoBillNo();

    }

    //Grid View Footer Total
    protected void gvUsers_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CalcTotal(e.Row.Cells[2].Text);

            double value2 = Convert.ToDouble(e.Row.Cells[2].Text);
            e.Row.Cells[2].Text = value2.ToString("0");

            e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Center;

        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[1].Text = "Total";
            //e.Row.Cells[10].Text = string.Format("{0:c}", runningTotal);
            e.Row.Cells[2].Text = runningTotal.ToString("0,0", CultureInfo.InvariantCulture);
            this.lblNetAmnt.Text = runningTotal.ToString("0,0", CultureInfo.InvariantCulture);

            e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Right;
        }
    }

    private void CalcTotal(string _price)
    {
        try
        {
            runningTotal += Double.Parse(_price);
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write(ex.ToString());
        }
    }


    //FINALLY SAVE DATA
    protected void btnSave_Click(object sender, EventArgs e)
    {
        SqlConnection conn = DBConnection.GetConnection();
        //fnLoadMRSRNo();

        string sSql = "";

        //CHALLAN VALIDATION        
        if (txtCHNo.Text == "")
        {
            PopupMessage("Please enter Challan Number.", btnSave);
            txtCHNo.Focus();
            return;
        }

        //CHALLAN DATE VALIDATION        
        if (txtDate.Text == "")
        {
            PopupMessage("Please enter Date.", btnSave);
            txtDate.Focus();
            return;
        }

        tDate = Convert.ToDateTime(this.txtDate.Text);


        //LOAD AUTO REQUEST NUMBER
        fnLoadAutoBillNo();


        //----------------------------------------------------------------------
        //CHECK DUPLICATE CHALLAN NO.
        sSql = "";
        sSql = "SELECT ReqAID FROM RequirmentMaster" +
            " WHERE ReqNo='" + this.txtCHNo.Text + "'";
            //" WHERE MRSRCode='" + this.txtMRSR.Text + 'S' + "'" +
            //" AND TrType=4";
        SqlCommand cmdd = new SqlCommand(sSql, conn);
        conn.Open();
        SqlDataReader drd = cmdd.ExecuteReader();
        try
        {
            if (drd.Read())
            {
                //iMRSRID = Convert.ToInt32(dr["MRSRMID"].ToString());
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert",
                            "<script>alert('" + "This Challan no. already exists." + "');</script>", false);
                txtCHNo.Focus();
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


        //SAVE DATA IN MASTER TABLE
        sSql = "";
        sSql = "INSERT INTO RequirmentMaster(ReqNo,ReqDate," +
               "EID,UserID,EntryDate)" +    
                     " Values ('" + this.txtCHNo.Text + "'," +
                     " '" + tDate + "'," +                     
                     " '" + Session["sBrId"] + "'," +
                     " '" + Session["UserName"] + "', " +
                     " '" + DateTime.Today + "'" +            
                     " )";
        SqlCommand cmd = new SqlCommand(sSql, conn);
        conn.Open();
        cmd.ExecuteNonQuery();
        conn.Close();


        //RETRIVE MASTER ID         
        sSql = "";
        sSql = "SELECT ReqAID FROM RequirmentMaster" +
            " WHERE ReqNo='" + this.txtCHNo.Text + "'";            
        cmd = new SqlCommand(sSql, conn);
        conn.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            iMRSRID = Convert.ToInt32(dr["ReqAID"].ToString());
        }
        dr.Dispose();
        dr.Close();
        conn.Close();


        //------------------------------------------------------------------------------------------
        //SAVE DATA IN DETAILS TABLE
        foreach (GridViewRow g1 in gvUsers.Rows)
        {           
            string sRemarks = "";
            if (g1.Cells[3].Text.Trim() != "&nbsp;")
            {
                sRemarks = g1.Cells[3].Text.Trim();
            }
            else
            {
                sRemarks = g1.Cells[3].Text = "";
            }

            string gSql = "";
            gSql = "INSERT INTO RequirmentDetails(ReqAID," +
                 " ProductID,ReqQty,ProdRemarks" +
                 " )" +
                 " VALUES('" + iMRSRID + "'," +
                 " '" + g1.Cells[0].Text + "'," +
                 " '" + g1.Cells[2].Text + "'," +
                 " '" + g1.Cells[3].Text + "'" +                
                 " )";
            SqlCommand cmdIns = new SqlCommand(gSql, conn);

            conn.Open();
            cmdIns.ExecuteNonQuery();
            conn.Close();

        }

        //------------------------------------------------------------------------------------------
        //SEND MAIL
        try
        {
            fnSendMail_Invoice();
        }
        catch
        {
            //
        }

        //------------------------------------------------------------------------------------------

        //lblSaveMessage.Text = "Save Data Successfully.";

        ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert",
                        "<script>alert('" + "Submit Successfully." + "');</script>", false);

        //------------------------------------------------------------------------------------------
        //CLEAR ALL TEXT
        txtCHNo.Text = "";
        txtDate.Text = "";
        txtDate.Text = DateTime.Today.ToString("dd/MM/yyyy");

        txtProdID.Text = "";
        txtProdDesc.Text = "";        
        txtRemarks.Text = "";

        //CLEAR GRIDVIEW
        gvUsers.DataSource = null;
        gvUsers.DataBind();

        dt.Clear();

        //txtCHNo.Focus();
        //LOAD AUTO REQUEST NUMBER
        fnLoadAutoBillNo();

        //------------------------------------------------------------------------------------------

        return;

    }

    //FUNCTION FOR SEND MAIL
    private void fnSendMail_Invoice()
    {
        SqlConnection conn = DBConnection.GetConnection();
        SqlConnection conn1 = DBConnection.GetConnection();

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

        //LOAD CTP INFORMATION
        string sSql = "";
        sSql = "SELECT EID, eName, EDesc, EntityType, EntityCode, ContactPerson,";
        sSql = sSql + " Desg, PhoneNo, EmailAdd, ContactNo";
        sSql = sSql + " FROM dbo.Entity";
        sSql = sSql + " WHERE EID='" + Session["EID"].ToString() + "'";
        SqlCommand cmdC = new SqlCommand(sSql, conn);
        conn.Open();
        SqlDataReader drC = cmdC.ExecuteReader();
        if (drC.Read())
        {
            lblCTPName.Text = drC["eName"].ToString();
            lblCTPAdd.Text = drC["EDesc"].ToString();
            lblCTPEmail.Text = drC["EmailAdd"].ToString();
            lblCTPContact.Text = drC["PhoneNo"].ToString();
            if (drC["PhoneNo"].ToString().Length == 0)
            {
                lblCTPContact.Text = drC["ContactNo"].ToString();
            }
        }
        conn.Close();


        //****************************************************************************************
       
        MailMessage mM2 = new MailMessage();
        //mM2.From = new MailAddress(txtEmail.Text);        

        //mM2.From = new MailAddress("rangs.eshop@gmail.com");
        mM2.From = new MailAddress("dms@rangs.com.bd");
        //PW:Exampass@567

        //mM2.To.Add(new MailAddress(Session["sEmail"].ToString()));
        mM2.To.Add(new MailAddress("cidd@rangs.com.bd"));
        //mM2.To.Add(new MailAddress("zunayed@rangs.com.bd"));
        //mM2.To.Add(new MailAddress(txtEmail.Text));
        mM2.CC.Add(new MailAddress(lblCTPEmail.Text));
        mM2.Bcc.Add(new MailAddress("zunayed@rangs.com.bd"));

        mM2.Subject = "Product Requirements from " + lblCTPName.Text + " ";
        //mM2.Body = "<h1>Order Details</h1>";

        mM2.Body = "<h1 style='color: #FF0000'>Product Requirement</h1>";

        mM2.Body = mM2.Body + "<p>Dear Sir,</p>";
        mM2.Body = mM2.Body + "<p>Thank you for continue supporting us.<br/>";
        //mM2.Body = mM2.Body + "We really appreciate it and we are taking necessary steps to process this order.";
        //mM2.Body = mM2.Body + "as soon as possible. You will be updated about next step immediate.";
        mM2.Body = mM2.Body + "</p>";


        //mM2.Body = mM2.Body + "<p>";
        //mM2.Body = mM2.Body + "Your order will be on its way very shortly, in the meantime please check below ";
        //mM2.Body = mM2.Body + "to ensure we have the correct details for your order.";
        //mM2.Body = mM2.Body + "</p>";

        //mM2.Body = mM2.Body + "<p>";
        //mM2.Body = mM2.Body + "<b><u>Sales From</u><br/> " + lblCTPName.Text + "</b><br/>";
        //mM2.Body = mM2.Body + "" + lblCTPAdd.Text + "<br/>";
        //mM2.Body = mM2.Body + "Phone: " + lblCTPContact.Text + "";
        //mM2.Body = mM2.Body + "</p>";

        //mM2.Body = mM2.Body + "<br/>";
        mM2.Body = mM2.Body + "<p><b>Req. No: " + txtCHNo.Text + "</b><br/>";
        mM2.Body = mM2.Body + "Date: " + txtDate.Text + "</p>";

        
        //mM2.Body = mM2.Body + "<br/>";
        mM2.Body = mM2.Body + "<p><b>Product Details:</b> </p>";

        //------- Start Table ---------------
        mM2.Body = mM2.Body + "<table style='font-family: arial, sans-serif; border-collapse: collapse; width: 100%;'>";

        mM2.Body = mM2.Body + "<tr>";
        mM2.Body = mM2.Body + "<th width='5%' style='border: 1px solid orange; text-align: left; padding: 8px;'>SL#</th>";
        mM2.Body = mM2.Body + "<th style='border: 1px solid orange; text-align: left; padding: 8px;'>Item Name</th>";
        mM2.Body = mM2.Body + "<th width='10%' style='border: 1px solid orange; text-align: left; padding: 8px;'>Qty</th>";
        //mM2.Body = mM2.Body + "<th width='15%' style='border: 1px solid orange; text-align: left; padding: 8px;'>Amount (&#2547;)</th>";
        //mM2.Body = mM2.Body + "<th width='15%' style='border: 1px solid orange; text-align: left; padding: 8px;'>Product Serial</th>";
        mM2.Body = mM2.Body + "<th width='15%' style='border: 1px solid orange; text-align: left; padding: 8px;'>Remarks</th>";
        mM2.Body = mM2.Body + "</tr>";

        //-----------------------------------------------------------------------------
        sSql = "";
        //sSql = "SELECT dbo.tbCustomerDelDetails.DelDID, dbo.tbCustomerDelDetails.DelID, dbo.tbCustomerDelDetails.ProductID,";
        //sSql = sSql + " dbo.tbProduct.title, dbo.tbProduct.titleDesc, dbo.tbCustomerDelDetails.SalePrice,";
        //sSql = sSql + " dbo.tbCustomerDelDetails.tQty, dbo.tbCustomerDelDetails.tAmnt";
        //sSql = sSql + " FROM dbo.tbCustomerDelDetails INNER JOIN";
        //sSql = sSql + " dbo.tbProduct ON dbo.tbCustomerDelDetails.ProductID = dbo.tbProduct.ProductID";
        //sSql = sSql + " WHERE (dbo.tbCustomerDelDetails.DelID = '" + this.lblID.Text + "')";

       
        sSql = "SELECT dbo.RequirmentMaster.ReqAID, dbo.RequirmentMaster.ReqNo, dbo.RequirmentMaster.ReqDate,";
        sSql = sSql + " dbo.RequirmentMaster.EID, dbo.RequirmentMaster.ReqBy, dbo.RequirmentDetails.ProductID, ";
        sSql = sSql + " dbo.Product.Model, dbo.RequirmentDetails.ReqQty, dbo.RequirmentDetails.ProdRemarks, dbo.Product.ProdName";
        sSql = sSql + " FROM  dbo.Product INNER JOIN";
        sSql = sSql + " dbo.RequirmentDetails ON dbo.Product.ProductID = dbo.RequirmentDetails.ProductID INNER JOIN";
        sSql = sSql + " dbo.RequirmentMaster ON dbo.RequirmentDetails.ReqAID = dbo.RequirmentMaster.ReqAID";

        sSql = sSql + " WHERE (dbo.RequirmentMaster.ReqNo = '" + this.txtCHNo.Text + "')";

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
            mM2.Body = mM2.Body + "<td style='border: 1px solid orange; text-align: left; padding: 8px;'>" + dr["Model"].ToString() + " (" + dr["ProdName"].ToString() + ")</td>";
            mM2.Body = mM2.Body + "<td align='Right' style='border: 1px solid orange; text-align: left; padding: 8px;'>" + dr["ReqQty"].ToString() + "</td>";
            //mM2.Body = mM2.Body + "<td align='Right' style='border: 1px solid orange; text-align: left; padding: 8px;'>" + dr["tNetAmnt"].ToString() + "</td>";
            //mM2.Body = mM2.Body + "<td align='Right' style='border: 1px solid orange; text-align: left; padding: 8px;'>" + dr["SLNO"].ToString() + "</td>";
            mM2.Body = mM2.Body + "<td align='Right' style='border: 1px solid orange; text-align: left; padding: 8px;'>" + dr["ProdRemarks"].ToString() + "</td>";
            mM2.Body = mM2.Body + "</tr>";
            iSl = iSl + 1;
        }
        //dataCommand1.ExecuteNonQuery();
        conn1.Close();
        //-------------------------------------------------------------------------------------

        //mM2.Body = mM2.Body + "<tr><td>Booking Date/time:</td><td>" + string.Format("{0:D}", tfDate) + " at " + ddlTime.SelectedItem.Text + "</td></tr>";
        //mM2.Body = mM2.Body + "<tr><td>Secial Notes/Comments:</td><td>" + txtNote.Text + "</td></tr>";
        mM2.Body = mM2.Body + "</table>";

        mM2.Body = mM2.Body + "<p>";
        //mM2.Body = mM2.Body + "Sub Total: &#2547; " + lblTotalAmnt.Text + "<br/>";
        //mM2.Body = mM2.Body + "Shipping Fee: &#2547; " + lblShipping.Text + "<br/>";
        //mM2.Body = mM2.Body + "Tax Amount: &#2547; " + lblTax.Text + "<br/>";

        mM2.Body = mM2.Body + "<br/>";

        mM2.Body = mM2.Body + "<p>";
        mM2.Body = mM2.Body + "Kind Regards, <br/> ";

        mM2.Body = mM2.Body + "<b>" + lblCTPName.Text + "</b><br/>";
        mM2.Body = mM2.Body + "" + lblCTPAdd.Text + "<br/>";
        mM2.Body = mM2.Body + "" + lblCTPContact.Text + "";

        //mM2.Body = mM2.Body + "<a href='http://www.rangs.com.bd/'>Rangs Electronics Ltd</a>";
        mM2.Body = mM2.Body + "</p>";

        mM2.IsBodyHtml = true;
        mM2.Priority = MailPriority.High;
        SmtpClient sC1 = new SmtpClient("mail.rangs.com.bd");
        //sC1.Port = 587;
        sC1.Port = 2525;
        sC1.Credentials = new System.Net.NetworkCredential("dms@rangs.com.bd", "Exampass@567");
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

    protected void txtCode_TextChanged(object sender, EventArgs e)
    {
        SqlConnection conn = DBConnection.GetConnection();
        string sSql = "";

        sSql = "";
        sSql = "SELECT ProductID,Model,ProdName,Code FROM Product" +
            " WHERE Code='" + this.txtCode.Text + "'";
        SqlCommand cmd = new SqlCommand(sSql, conn);
        conn.Open();
        SqlDataReader dr = cmd.ExecuteReader();

        try
        {
            if (dr.Read())
            {
                this.txtProdID.Text = dr["ProductID"].ToString();
                this.txtProdDesc.Text = dr["ProdName"].ToString();
                //this.txtCode.Text = dr["Code"].ToString();
                this.ddlContinents.SelectedItem.Text = dr["Model"].ToString();
            }
            else
            {
                this.txtProdID.Text = "";
                this.txtProdDesc.Text = "";
                this.ddlContinents.SelectedItem.Text = "";
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
    }

    
}