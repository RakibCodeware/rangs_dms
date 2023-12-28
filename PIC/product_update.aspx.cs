using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Globalization;
using System.Text.RegularExpressions;

using System.Drawing;
using System.Drawing.Drawing2D;
using System.Net.Mail;

public partial class admin_product_update : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (System.Convert.ToInt32(Session["Vis"]) == 0)
        {
            Response.Redirect("../Login.aspx");
        }

        if (!this.IsPostBack)
        {
            //fnLoadCombo_Category(DropDownList1, "CatName", "CategoryID", "tbCategory");
            //this.BindGrid();

            //if (ddlCampaign.SelectedItem.Text=="--Select--")
            //{
            //    ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert",
            //            "<script>alert('" + "Please select running Campaign..." + "');</script>", false);
            //    return;
            //}

            fnLoadCombo_ProdGroup();

            fnLoadCombo_Campaign();

        }

    }


    private void BindGrid()
    {

        SqlConnection conn = DBConnection.GetConnection();

        //SqlCommand dataCommand = new SqlCommand();
        //dataCommand.Connection = conn;

        //string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;

        //string query = "";
        //query = "SELECT dbo.tbCategory.CategoryID, dbo.tbCategory.CatName, dbo.tbProduct.ProductID,";
        //query = query + " dbo.tbProduct.DelChargeInDhk, dbo.tbProduct.DelChargeOutDhk,";
        //query = query + " dbo.tbProduct.title, dbo.tbProduct.MRP, dbo.tbProduct.SalePrice, dbo.tbProduct.OrderBy";
        //query = query + " FROM  dbo.tbCategory INNER JOIN";
        //query = query + " dbo.tbProduct ON dbo.tbCategory.CategoryID = dbo.tbProduct.CategoryID";

        //if (DropDownList1.SelectedItem.Text != "ALL")
        //{
        //    query = query + " WHERE dbo.tbCategory.CatName='" + DropDownList1.SelectedItem.Text + "'";
        //}

        string sSql = "";
        sSql = "SELECT dbo.Product.ProductID, dbo.Product.Model, dbo.Product.MRP, dbo.Product.UnitPrice, dbo.Product.DealerPrice,";
        sSql = sSql + " dbo.Product.GroupName, dbo.Product.GroupSL, dbo.Product.Discontinue, ";
        sSql = sSql + " ISNULL(dbo.VW_CampaingProduct_List.CampPrice, 0) AS ttCampPrice,";
        sSql = sSql + " (ISNULL(dbo.Product.UnitPrice, 0) - ISNULL(dbo.VW_CampaingProduct_List.DisAmnt, 0)) AS tCampPrice,";
        sSql = sSql + " ISNULL(dbo.VW_CampaingProduct_List.DisAmnt, 0) AS tDisAmnt, dbo.VW_CampaingProduct_List.CamAID,";
        sSql = sSql + " dbo.VW_CampaingProduct_List.CampaignNo, dbo.VW_CampaingProduct_List.CampaignName";
        sSql = sSql + " FROM dbo.Product LEFT OUTER JOIN";
        sSql = sSql + " dbo.VW_CampaingProduct_List ON dbo.Product.ProductID = dbo.VW_CampaingProduct_List.ProductID";
        sSql = sSql + " WHERE (dbo.Product.Discontinue = N'No')";

        if (DropDownList1.SelectedItem.Text != "ALL")
        {
            sSql = sSql + " AND dbo.Product.GroupName ='" + DropDownList1.SelectedItem.Text + "'";
        }

        sSql = sSql + " ORDER BY dbo.Product.Model";

        //using (SqlConnection con = new SqlConnection(constr))
        //{
        //    using (SqlDataAdapter sda = new SqlDataAdapter(query, con))
        //    {
        //        using (DataTable dt = new DataTable())
        //        {
        //            sda.Fill(dt);
        //            GridView1.DataSource = dt;
        //            GridView1.DataBind();
        //        }
        //    }
        //}


        SqlCommand cmd = new SqlCommand(sSql, conn);
        //SqlDataReader dr = cmd.ExecuteReader();
        conn.Open();

        DataSet ds = new DataSet();
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        da.Fill(ds);
        //da.Fill(dt);

        GridView1.DataSource = ds;
        //GridView1.DataSource = dt;
        GridView1.DataBind();
        //dr.Close();
        conn.Close();


    }

    protected void Insert(object sender, EventArgs e)
    {
        //string name = txtName.Text;
        //string country = txtCountry.Text;
        //txtName.Text = "";
        //txtCountry.Text = "";
        //string query = "INSERT INTO tbProduct VALUES(@Name, @Country)";
        //string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
        //using (SqlConnection con = new SqlConnection(constr))
        //{
        //    using (SqlCommand cmd = new SqlCommand(query))
        //    {
        //        cmd.Parameters.AddWithValue("@Name", name);
        //        cmd.Parameters.AddWithValue("@Country", country);
        //        cmd.Connection = con;
        //        con.Open();
        //        cmd.ExecuteNonQuery();
        //        con.Close();
        //    }
        //}

        //this.BindGrid();

    }


    protected void OnRowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView1.EditIndex = e.NewEditIndex;
        this.BindGrid();
    }
    private void fnSendMail_Price_Changed(int pId, string mrp, string specialPrice, string dealerPrice, string userName)
    {
        try
        {
            SqlConnection conn = DBConnection.GetConnection();
            string gSql = "";
            string model = "";
            //-------------------------------------------------------------------------------------------

            gSql = "";
            gSql = "select Model from Product";
            gSql = gSql + " WHERE ProductID='" + pId + "'";

            SqlCommand cmdIns = new SqlCommand(gSql, conn);
            conn.Open();
            SqlDataReader dr = cmdIns.ExecuteReader();
            if (dr.Read())
            {
                model = dr["Model"].ToString();
            }
            conn.Close();

            //****************************************************************************************
            //-----------------------------------------------------------------------------------------------------
            // Mail to Customer------------------------------------------------------------------------------------
            MailMessage mM2 = new MailMessage();
            //mM2.From = new MailAddress(txtEmail.Text);        
            //mM2.From = new MailAddress("rangs.eshop@gmail.com");
            mM2.From = new MailAddress("dms@rangs.com.bd");
            //PW:Exampass@567
            //mM2.To.Add(new MailAddress(Session["sEmail"].ToString()));            
            mM2.To.Add(new MailAddress("minto@rangs.com.bd"));
            mM2.Bcc.Add(new MailAddress("mohiuddin@rangs.com.bd"));

            mM2.Subject = "Product Price Update DMS " + model + " ";
            //mM2.Body = "<h1>Order Details</h1>";
            mM2.Body = "<p>Updated By:mr. " + userName + ",</p>";
            mM2.Body = mM2.Body + "<p>IP: " + Request.UserHostAddress.ToString() + ",</p>";
            mM2.Body = mM2.Body + "<p>Updated Time:" + DateTime.Now.ToString() + "<br/>";
            //mM2.Body = mM2.Body + "We really appreciate it and we are taking necessary steps to process this order.";
            //mM2.Body = mM2.Body + "as soon as possible. You will be updated about next step immediate.";
            mM2.Body = mM2.Body + "</p>";





            mM2.Body = mM2.Body + "<p><b>Changed Product Details:</b> </p>";

            //------- Start Table ---------------
            mM2.Body = mM2.Body + "<table style='font-family: arial, sans-serif; border-collapse: collapse; width: 100%;'>";

            mM2.Body = mM2.Body + "<tr>";

            mM2.Body = mM2.Body + "<th style='border: 1px solid orange; text-align: left; padding: 8px;'>Model</th>";
            mM2.Body = mM2.Body + "<th width='15%' style='border: 1px solid orange; text-align: left; padding: 8px;'>MRP</th>";
            mM2.Body = mM2.Body + "<th width='15%' style='border: 1px solid orange; text-align: left; padding: 8px;'>Special/Camp Price</th>";
            mM2.Body = mM2.Body + "<th width='15%' style='border: 1px solid orange; text-align: left; padding: 8px;'>Dealer Price</th>";
            mM2.Body = mM2.Body + "</tr>";

            //-----------------------------------------------------------------------------          

            mM2.Body = mM2.Body + "<tr>";
            //mM2.Body = mM2.Body + "<td style='border: 1px solid orange; text-align: left; padding: 8px;'>" + dr["ProductName"].ToString() + "</td>";
            mM2.Body = mM2.Body + "<td style='border: 1px solid orange; text-align: left; padding: 8px;'>" + model + "</td>";
            mM2.Body = mM2.Body + "<td align='Right' style='border: 1px solid orange; text-align: left; padding: 8px;'>" + mrp + "</td>";
            mM2.Body = mM2.Body + "<td align='Right' style='border: 1px solid orange; text-align: left; padding: 8px;'>" + specialPrice + "</td>";
            mM2.Body = mM2.Body + "<td align='Right' style='border: 1px solid orange; text-align: left; padding: 8px;'>" + dealerPrice + "</td>";
            mM2.Body = mM2.Body + "</tr>";
            mM2.Body = mM2.Body + "</table>";
            mM2.Body = mM2.Body + "<p>";
            mM2.Body = mM2.Body + "<p>";
            mM2.Body = mM2.Body + "</p>";

            mM2.Body = mM2.Body + "<p>";
            mM2.Body = mM2.Body + "Kind Regards, <br/> ";
            mM2.Body = mM2.Body + "<a href='#'>DMS</a>";
            mM2.Body = mM2.Body + "</p>";

            mM2.IsBodyHtml = true;
            mM2.Priority = MailPriority.High;
            SmtpClient sC1 = new SmtpClient("mail.rangs.com.bd");
            sC1.Port = 587;
            //sC1.Port = 2525;
            sC1.Credentials = new System.Net.NetworkCredential("dms@rangs.com.bd", "Exampass@567");
            //sC.EnableSsl = true;
            sC1.Send(mM2);
        }
        catch (Exception ex)
        {

        }

    }
    protected void OnRowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridViewRow row = GridView1.Rows[e.RowIndex];
        int productId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);

        string mrp = (row.FindControl("txtMRP") as TextBox).Text;
        string salePrice = (row.FindControl("txtPrice") as TextBox).Text;
        string sDP = (row.FindControl("txtDP") as TextBox).Text;
        //string OrderBy = (row.FindControl("txtOrder") as TextBox).Text;

        double dDisAmnt = Convert.ToDouble(mrp) - Convert.ToDouble(salePrice);

        //string query = "UPDATE tbProduct SET MRP=@MRP, SalePrice=@SalePrice, OrderBy=@OrderBy WHERE ProductId=@productId";
        //string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;

        //using (SqlConnection con = new SqlConnection(constr))
        //{
        //    using (SqlCommand cmd = new SqlCommand(query))
        //    {
        //        cmd.Parameters.AddWithValue("@productId", productId);
        //        cmd.Parameters.AddWithValue("@MRP", mrp);
        //        cmd.Parameters.AddWithValue("@SalePrice", salePrice);
        //        cmd.Parameters.AddWithValue("@OrderBy", OrderBy);
        //        cmd.Connection = con;
        //        con.Open();
        //        cmd.ExecuteNonQuery();
        //        con.Close();
        //    }
        //}

        //-----------------------------------------------------
        SqlConnection conn = DBConnection.GetConnection();

        string gSql = "";
        //----------------------------------------------------------------------------------

        string sSql = "";
        
        //DELETE SAME DATA
        sSql = "";
        sSql = "DELETE FROM tbCampaignDetails";
        sSql = sSql + " WHERE CampaignNo='" + lblCampCode.Text + "'";
        sSql = sSql + " AND ProductID='" + productId + "'";

        SqlCommand cmd = new SqlCommand(sSql, conn);
        conn.Open();
        //cmd.ExecuteNonQuery();
        conn.Close();


        //INSERT INTO TABLE
        gSql = "";
        gSql = "INSERT INTO tbCampaignDetails (CampaignNo,ProductID,MRP,CampPrice,DisAmnt,";
        gSql = gSql + " UserID, EntryDate";
        gSql = gSql + " ) ";
        gSql = gSql + " VALUES ('" + lblCampCode.Text + "','" + productId + "','" + mrp + "', ";
        gSql = gSql + " '" + salePrice + "', '" + dDisAmnt + "',";
        gSql = gSql + " '" + Session["sUser"].ToString() + "','" + DateTime.Now + "'";
        gSql = gSql + " )";

        SqlCommand cmdIns1 = new SqlCommand(gSql, conn);
        conn.Open();
        //cmdIns1.ExecuteNonQuery();
        conn.Close();
        //-------------------------------------------------------------------------------------------

        gSql = "";
        gSql = "UPDATE Product SET MRP='" + mrp + "',";
        gSql = gSql + " UnitPrice='" + mrp + "',";
        gSql = gSql + " DealerPrice='" + sDP + "'";
        gSql = gSql + " WHERE ProductID='" + productId + "'";

        SqlCommand cmdIns = new SqlCommand(gSql, conn);
        conn.Open();
        //cmdIns.ExecuteNonQuery();
        conn.Close();

        fnSendMail_Price_Changed(productId, mrp, salePrice, sDP, Session["UserName"].ToString());
        //SqlCommand dataCommand = new SqlCommand();
        //dataCommand.Connection = conn;

        ////
        ////gSql = "UPDATE tbProduct SET MRP='" + mrp + "',";
        ////gSql = gSql + " SalePrice='" + salePrice + "',";
        ////gSql = gSql + " DelChargeInDhk='" + DelChargeInDhk + "', DelChargeOutDhk='" + DelChargeOutDhk + "',";
        ////gSql = gSql + " OrderBy='" + OrderBy + "'";
        ////gSql = gSql + " WHERE ProductId='" + productId + "'";

        ////SqlCommand cmdIns = new SqlCommand(gSql, conn);
        ////conn.Open();
        ////cmdIns.ExecuteNonQuery();
        ////conn.Close();

        //-----------------------------------------------------

        ////----------------------------------------------------------------------------------
        ////INSERT INTO LOG TABLE
        //gSql = "";
        //gSql = "INSERT INTO tbProduct_log (ProductID,MRP,SalePrice,OrderBy,";
        //gSql = gSql + " DelChargeInDhk, DelChargeOutDhk,";
        //gSql = gSql + " status,UserID, EntryDate";
        //gSql = gSql + " ) ";
        //gSql = gSql + " VALUES ('" + productId + "','" + mrp + "', '" + salePrice + "', '" + OrderBy + "',";
        //gSql = gSql + " '" + DelChargeInDhk + "', '" + DelChargeOutDhk + "',";
        //gSql = gSql + " 1,'" + Session["sUser"].ToString() + "','" + DateTime.Now + "'";               
        //gSql = gSql + " )";

        //SqlCommand cmdIns1 = new SqlCommand(gSql, conn);
        //conn.Open();
        //cmdIns1.ExecuteNonQuery();
        //conn.Close();
        ////-------------------------------------------------------------------------------------------
        GridView1.EditIndex = -1;
        this.BindGrid();
    }

    protected void OnRowCancelingEdit(object sender, EventArgs e)
    {
        GridView1.EditIndex = -1;
        this.BindGrid();
    }

    protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        //int customerId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
        //string query = "DELETE FROM tbProduct WHERE ProductId=@productId";
        //string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
        //using (SqlConnection con = new SqlConnection(constr))
        //{
        //    using (SqlCommand cmd = new SqlCommand(query))
        //    {
        //        cmd.Parameters.AddWithValue("@productId", customerId);
        //        cmd.Connection = con;
        //        con.Open();
        //        cmd.ExecuteNonQuery();
        //        con.Close();
        //    }
        //}

        //this.BindGrid();
    }

    protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex != GridView1.EditIndex)
        //{
        //    (e.Row.Cells[4].Controls[4] as LinkButton).Attributes["onclick"] = "return confirm('Do you want to delete this row?');";
        //}
    }

    protected void OnPaging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        this.BindGrid();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        SqlConnection conn = DBConnection.GetConnection();

        string sqlfns = "SELECT CamAID, CampaignNo, CampaignName, cStop";
        sqlfns = sqlfns + " FROM  dbo.tbCampaignMaster";
        sqlfns = sqlfns + " GROUP BY CamAID, CampaignNo, CampaignName, cStop";
        sqlfns = sqlfns + " HAVING (cStop = 0)";
        SqlCommand cmd = new SqlCommand(sqlfns, conn);
        conn.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            this.lblCampID.Text = dr["CamAID"].ToString();
            this.lblCampCode.Text = dr["CampaignNo"].ToString();
        }
        else
        {
            this.lblCampID.Text = "0";
            this.lblCampCode.Text = "";
        }
        dr.Dispose();
        dr.Close();
        conn.Close();


        this.BindGrid();
    }

    private void fnLoadCombo_Category(DropDownList xCombo, string pField, string pFieldID, string pTable)
    {
        SqlConnection conn = DBConnection.GetConnection();

        try
        {
            conn.Open();
            string sqlfns = "SELECT * FROM " + pTable + " WHERE parent<>0 ORDER BY " + pField + " ASC";
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
            //xCombo.Items.Insert(0, new ListItem("ALL", "ALL"));

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

    private void fnLoadCombo_ProdGroup()
    {
        SqlConnection conn = DBConnection.GetConnection();

        try
        {
            conn.Open();
            string sqlfns = "SELECT  GroupName, GroupSL FROM dbo.Product GROUP BY GroupName, GroupSL ORDER BY GroupName ";
            SqlCommand cmd = new SqlCommand(sqlfns, conn);

            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sqlfns;
            cmd.Connection = conn;

            DropDownList1.DataSource = cmd.ExecuteReader();
            DropDownList1.DataTextField = "GroupName";
            //ddlEmp.DataValueField = "SupName";
            DropDownList1.DataValueField = "GroupSL";
            DropDownList1.DataBind();

            //Add blank item at index 0.
            DropDownList1.Items.Insert(0, new ListItem("--Select--", "0"));

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

    private void fnLoadCombo_Campaign()
    {
        SqlConnection conn = DBConnection.GetConnection();

        try
        {
            conn.Open();
            string sqlfns = "SELECT CamAID, CampaignNo, CampaignName, cStop";
            sqlfns = sqlfns + " FROM  dbo.tbCampaignMaster";
            sqlfns = sqlfns + " GROUP BY CamAID, CampaignNo, CampaignName, cStop";
            sqlfns = sqlfns + " HAVING (cStop = 0)";
            SqlCommand cmd = new SqlCommand(sqlfns, conn);

            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sqlfns;
            cmd.Connection = conn;

            ddlCampaign.DataSource = cmd.ExecuteReader();
            ddlCampaign.DataTextField = "CampaignName";
            //ddlEmp.DataValueField = "SupName";
            ddlCampaign.DataValueField = "CamAID";
            ddlCampaign.DataBind();

            //Add blank item at index 0.
            //ddlCampaign.Items.Insert(0, new ListItem("--Select--", "0"));

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

}