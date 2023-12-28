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
//using System.Data.OleDb;
using System.IO;
using System.Text;
using System.Globalization;
using System.Text.RegularExpressions;

using System.Drawing;
using System.Drawing.Drawing2D;

using System.Data.SqlClient;


public partial class VendorInfo : System.Web.UI.Page
{
    long i;
    DataTable dt;
    //SqlDataAdapter da;
    //DataSet ds = new DataSet();
    //DataTable dt1 = new DataTable();

    protected void Page_Load(object sender, EventArgs e)
    {
        //errorMsg.Text = "ssssssss"+Request.QueryString["action"];
        if (!IsPostBack)
        {

            //LOAD CATEGORY
            //fnLoadCombo_Item(ddlCategory, "CatName", "CategoryID", "Customer");

            //fnLoadCombo_MainCat(ddlType, "CatName", "CategoryID", "Customer");

            //fnLoadCombo_MainCat(DropDownList1, "CatName", "CategoryID", "Customer");
            //DropDownList1.Items.Insert(0, new ListItem("ALL", "ALL"));
            int role = Convert.ToInt32(Session["RolesId"]);

            if (role != 1)
            {
                Response.Redirect("~/Login.aspx");
            }
            // LOAD ALL DATA IN GRID
            fnLoadData();

            fnLoadDay(ddlDay);
            fnLoadMonth(ddlMonth);
            fnLoadYear(ddlYear);

            //fnLoadCombo(ddlCity, "CatName", "CategoryID", "Customer");
            
            //LOAD CTP
            //LoadDropDownList_CTP();


            if (Request.QueryString["action"] == "delete" && Request.QueryString["id"] != "")
            {

                SqlConnection conn = DBConnection.GetConnection();

                try
                {
                    /*
                    SqlCommand dataCommand = new SqlCommand();
                    dataCommand.Connection = conn;
                    dataCommand.CommandType = CommandType.Text;

                    dataCommand.CommandText = "DELETE FROM tbVendorInfo WHERE VAID=" + Request.QueryString["id"];

                    conn.Open();
                    dataCommand.ExecuteNonQuery();
                    conn.Close();
                    Response.Redirect("VendorInfo.aspx");
                     * */

                }
                catch (Exception ex)
                {
                    errorMsg.Text = ex.Message;
                    //Label1.Text = Label1.Text+"INSERT INTO categories (title, description, parent,status,order) VALUES ('" + txttitle.Text + "', '" + description.Text + "', '" + parent.Value + "','" + "0" + "','" + order.Text + "')";

                }
            }
            else if (Request.QueryString["action"] == "edit" && Request.QueryString["id"] != "")
            {
                SqlConnection conn = DBConnection.GetConnection();

                string sSql = "";

                //try
                //{
                    SqlCommand dataCommand = new SqlCommand();
                    dataCommand.Connection = conn;
                    dataCommand.CommandType = CommandType.Text;

                    sSql = "Select VAID, VName, NickName, VAddress, VContact, ";
                    sSql = sSql + " VPhoto, VNID, VRef, VType, status, VNIDNo, VContact2, UserID";
                    sSql = sSql + " FROM tbVendorInfo WHERE VAID=" + Request.QueryString["id"];
                    dataCommand.CommandText = sSql;

                    conn.Open();
                    SqlDataReader dr = dataCommand.ExecuteReader();
                    while (dr.Read())
                    {
                        ID.Text = dr["VAID"].ToString();
                        txtAID.Text = dr["VAID"].ToString();
                        txtName.Text = dr["VName"].ToString();
                        txtNickName.Text = dr["NickName"].ToString();
                        txtMobile.Text = dr["VContact"].ToString();
                        Session["vContact"] = dr["VContact"].ToString();
                        txtAdd.Text = dr["VAddress"].ToString();
                        txtRef.Text = dr["VRef"].ToString();
                        //ddlCity.SelectedItem.Text = dr["City"].ToString();
                        this.ddlType.SelectedItem.Text = dr["VType"].ToString();

                        txtNIDNo.Text = dr["VNIDNo"].ToString();

                        if (dr["status"].ToString() == "True")
                        {
                            status.Checked = true;
                        }
                                                

                    }

                    conn.Close();
                    dr.Close();

                //}
                //catch (Exception ex)
                //{
                //    errorMsg.Text = ex.Message;
                //    //Label1.Text = Label1.Text+"INSERT INTO categories (title, description, parent,status,order) VALUES ('" + txttitle.Text + "', '" + description.Text + "', '" + parent.Value + "','" + "0" + "','" + order.Text + "')";

                //}
            }

        }
    }


    protected void btnSave_Click(object sender, EventArgs e)
    {
        string gSql = "";

        SqlConnection conn = DBConnection.GetConnection();

        SqlCommand dataCommand = new SqlCommand();
        dataCommand.Connection = conn;        
        dataCommand.CommandType = CommandType.Text;

        SqlCommand dataCommand1 = new SqlCommand();
        dataCommand1.Connection = conn;
        dataCommand1.CommandType = CommandType.Text;

        string st;
        string filename = "";
        string filename2 = "";
        string thumfilename = "";

        DateTime nm = DateTime.Now;
        string date = nm.ToString("yyyyMMddhhmmss");        
        string date1 = nm.ToString("yyyyMMddhhmmss") + "1";
        string date2 = nm.ToString("yyyyMMddhhmmss") + "2";

        string path, path2, file, file2;
        string NewPath;
        string cSex = "";
        


        //try
        //{
        if (order.Text.Length == 0)
        {
            order.Text = "0";
        }

        if (status.Checked)
        {
            st = "1";
        }
        else
        {
            st = "0";
        }


        if (txtName.Text.Length == 0)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert",
                        "<script>alert('" + "Please enter Vendor Name..." + "');</script>", false);
            txtName.Focus();
            return;
        }

        if (txtMobile.Text.Length == 0)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert",
                        "<script>alert('" + "Please enter Vendor Mobile number..." + "');</script>", false);
            txtMobile.Focus();
            return;
        }

        //-------------------------------- IMAGE 1 -------------------------------------------------------------
        if (ImageUpload.PostedFile != null)
        {
            try
            {
                // Check the extension of image
                string extension = Path.GetExtension(ImageUpload.FileName);
                if (extension.ToLower() == ".png" || extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg")
                {
                    Stream strm = ImageUpload.PostedFile.InputStream;
                    using (var image = System.Drawing.Image.FromStream(strm))
                    {
                        // Print Original Size of file (Height or Width) 
                        //Label1.Text = image.Size.ToString();
                        int newWidth = 450; // New Width of Image in Pixel
                        int newHeight = 570; // New Height of Image in Pixel
                        var thumbImg = new Bitmap(newWidth, newHeight);
                        var thumbGraph = Graphics.FromImage(thumbImg);
                        thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
                        thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
                        thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        var imgRectangle = new Rectangle(0, 0, newWidth, newHeight);
                        thumbGraph.DrawImage(image, imgRectangle);

                        //---------------------------------------------------------------------
                        path = Path.GetFileName(ImageUpload.FileName);
                        file = Path.GetFileNameWithoutExtension(path);
                        string ext = Path.GetExtension(path);
                        NewPath = path.Replace(file, date1);
                        filename = path.Replace(file, date1);
                        //---------------------------------------------------------------------

                        // Save the file
                        //string targetPath = Server.MapPath(@"~\Images\") + FileUpload1.FileName;                    
                        string targetPath = Server.MapPath("~/") + "/image/vendor/" + filename;
                        thumbImg.Save(targetPath, image.RawFormat);

                    }
                }
            }
            catch
            {
                //
            }
        }

        //-------------------------------- IMAGE 2 -------------------------------------------------------------
        if (ImageUpload2.PostedFile != null)
        {
            try
            {
                // Check the extension of image
                string extension = Path.GetExtension(ImageUpload2.FileName);
                if (extension.ToLower() == ".png" || extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg")
                {     
                    filename2 = Path.GetFileName(ImageUpload2.FileName);
                    ImageUpload2.SaveAs(Server.MapPath("~/") + "/image/vendor/doc/" + filename2);
                }
            }
            catch
            {
                //
            }
        }

        string DOBDate = ddlYear.SelectedItem.Text + "" + ddlMonth.SelectedItem.Value + "" + ddlDay.SelectedItem.Text;
        string stDate = ddlYear.SelectedItem.Text + "/" + ddlMonth.SelectedItem.Value + "/" + ddlDay.SelectedItem.Text;
        DateTime dtDOB = Convert.ToDateTime(stDate);


        if (ID.Text == "")
        {
            gSql = "";
            gSql = "INSERT INTO tbVendorInfo(VName, NickName, VContact, VAddress, VPhoto, VNID, VNIDNo, ";
            gSql = gSql + " VRef, VType, status)";
            gSql = gSql + " VALUES ('" + txtName.Text.Replace("'", "''") + "','" + txtNickName.Text + "',";
            gSql = gSql + " '" + txtMobile.Text + "','" + txtAdd.Text.Replace("'", "''") + "',";
            gSql = gSql + " '" + filename + "','" + filename2 + "','" + txtNIDNo.Text + "',";
            gSql = gSql + " '" + txtRef.Text + "','" + ddlType.SelectedItem.Text + "','" + st + "'";                       
            gSql = gSql + " )";

            dataCommand.CommandText = gSql;
        }

        else
        {
            if (filename != "" && filename2 != "")
            {
                gSql = "";
                gSql = "UPDATE tbVendorInfo SET VName='" + txtName.Text + "', VAddress='" + txtAdd.Text + "',";
                gSql = gSql + " NickName='" + txtNickName.Text + "',";
                gSql = gSql + " VContact='" + txtMobile.Text + "',";
                gSql = gSql + " VPhoto='" + filename + "', VNID='" + filename2 + "',";
                gSql = gSql + " VNIDNo='" + txtNIDNo.Text + "',";
                gSql = gSql + " VRef='" + txtRef.Text + "', VType='" + ddlType.SelectedItem.Text + "',";
                gSql = gSql + " status='" + st + "'";

                gSql = gSql + " WHERE VAID=" + ID.Text;

                dataCommand.CommandText = gSql;

            }
            else if (filename != "")
            {
                gSql = "";
                gSql = "UPDATE tbVendorInfo SET VName='" + txtName.Text + "', VAddress='" + txtAdd.Text + "',";
                gSql = gSql + " NickName='" + txtNickName.Text + "',";
                gSql = gSql + " VContact='" + txtMobile.Text + "',";
                gSql = gSql + " VPhoto='" + filename + "',";
                gSql = gSql + " VNIDNo='" + txtNIDNo.Text + "',";
                gSql = gSql + " VRef='" + txtRef.Text + "', VType='" + ddlType.SelectedItem.Text + "',";
                gSql = gSql + " status='" + st + "'";

                gSql = gSql + " WHERE VAID=" + ID.Text;

                dataCommand.CommandText = gSql;

            }
            else if (filename2 != "")
            {
                gSql = "";
                gSql = "UPDATE tbVendorInfo SET VName='" + txtName.Text + "', VAddress='" + txtAdd.Text + "',";
                gSql = gSql + " NickName='" + txtNickName.Text + "',";
                gSql = gSql + " VContact='" + txtMobile.Text + "',";
                gSql = gSql + " VNID='" + filename2 + "',";
                gSql = gSql + " VNIDNo='" + txtNIDNo.Text + "',";
                gSql = gSql + " VRef='" + txtRef.Text + "', VType='" + ddlType.SelectedItem.Text + "',";
                gSql = gSql + " status='" + st + "'";

                gSql = gSql + " WHERE VAID=" + ID.Text;

                dataCommand.CommandText = gSql;

            }

            else
            {
                gSql = "";
                gSql = "UPDATE tbVendorInfo SET VName='" + txtName.Text + "', VAddress='" + txtAdd.Text + "',";
                gSql = gSql + " NickName='" + txtNickName.Text + "',";
                gSql = gSql + " VContact='" + txtMobile.Text + "',";
                gSql = gSql + " VNIDNo='" + txtNIDNo.Text + "',";
                gSql = gSql + " VRef='" + txtRef.Text + "', VType='" + ddlType.SelectedItem.Text + "',";
                gSql = gSql + " status='" + st + "'";

                gSql = gSql + " WHERE VAID=" + ID.Text;

                dataCommand.CommandText = gSql;

            }

        }
                
        //Label1.Text = dataCommand.CommandText;
        conn.Open();
        dataCommand.ExecuteNonQuery();
        conn.Close();

        //----------------------------------------------------------------------------
        //if (ID.Text != "")
        //{
        //    gSql = "";
        //    gSql = "UPDATE MRSRMaster SET tbVendorInfo='" + txtMobile.Text + "'";
        //    gSql = gSql + " WHERE Customer='" + Session["cMobile"].ToString() + "'";

        //    dataCommand1.CommandText = gSql;
        //}
        //conn.Open();
        //dataCommand1.ExecuteNonQuery();
        //conn.Close();
        //----------------------------------------------------------------------------

        Response.Redirect("VendorInfo.aspx");

        //}
        //catch (Exception ex)
        //{
        //   // Label1.Text = ex.Message;
        //  //Label1.Text = Label1.Text+"INSERT INTO tbVendorInfo (title, description, parent,status,order) VALUES ('" + txttitle.Text + "', '" + description.Text + "', '" + parent.Value + "','" + "0" + "','" + order.Text + "')";

        //}


    }


    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("VendorInfo.aspx");
    }


    private void fnLoadCombo_MainCat(DropDownList xCombo, string pField, string pFieldID, string pTable)
    {
        SqlConnection conn = DBConnection.GetConnection();

        try
        {
            conn.Open();
            string sqlfns = "SELECT * FROM " + pTable + "  ORDER BY " + pField + " ASC";
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

    private void fnLoadCombo(DropDownList xCombo, string pField, string pFieldID, string pTable)
    {
        SqlConnection conn = DBConnection.GetConnection();

        try
        {
            conn.Open();
            string sqlfns = "SELECT * FROM " + pTable + "  ORDER BY " + pField + " ASC";
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

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        //fnLoadData();
        fnLoadData_BySearch();
    }

    protected void fnLoadData()
    {
        //if (System.Convert.ToInt32(Session["Vis"]) == 0)
        //{
        //    Response.Redirect("Default.aspx");
        //}

        SqlConnection conn = DBConnection.GetConnection();
        
        string sSql = "";

        sSql = "SELECT VAID, VName, NickName, VContact, VAddress, VPhoto, VNID, VNIDNo, VRef, VType, status, VContact2, UserID";
        sSql = sSql + " FROM  dbo.tbVendorInfo";
        sSql = sSql + " WHERE status=1";


        SqlCommand cmd = new SqlCommand(sSql, conn);
        //OleDbDataReader dr = cmd.ExecuteReader();
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

    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        if (txtSearchByPhone.Text.Length == 0)
        {
            fnLoadData(); //bindgridview will get the data source and bind it again
        }
        else
        {
            fnLoadData_BySearch();
        }
    }

    //private void bindGridView()
    //{
    //    GridView1.DataSource = fnLoadData();
    //    GridView1.DataBind();
    //}

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
        for (i = (DateTime.Now.Year - 1); (i <= (DateTime.Now.Year + 2)); i++)
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
        cBox.Items.FindByText(DateTime.Now.Year.ToString()).Selected = true;

    }

    

    protected void txtSearchByPhone_TextChanged(object sender, EventArgs e)
    {
        //fnLoadData_BySearch();
    }


    protected void fnLoadData_BySearch()
    {
        //if (System.Convert.ToInt32(Session["Vis"]) == 0)
        //{
        //    Response.Redirect("Default.aspx");
        //}

        SqlConnection conn = DBConnection.GetConnection();

        string sSql = "";
        sSql = "SELECT VAID, VName,NickName, VContact, VAddress, VPhoto, VNID, VNIDNo, VRef, VType, status, VContact2, UserID";
        sSql = sSql + " FROM  dbo.tbVendorInfo";
        sSql = sSql + " WHERE status=1";
        sSql = sSql + " AND ((VContact  Like '%" + txtSearchByPhone.Text + "%')";
        sSql = sSql + " OR (VName  Like '%" + txtSearchByPhone.Text + "%'))";

        sSql = sSql + " ORDER BY VName";

        SqlCommand cmd = new SqlCommand(sSql, conn);
        //OleDbDataReader dr = cmd.ExecuteReader();
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

        if (ds.Tables[0].Rows.Count > 0)
        {
            GridView1.DataSource = ds.Tables[0];
            GridView1.DataBind();
        }


    }

    protected void GridView1_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[4].Text = Regex.Replace(e.Row.Cells[4].Text, txtSearchByPhone.Text.Trim(), delegate(Match match)
            {
                return string.Format("<span style = 'background-color:green; color:white;'>{0}</span>", match.Value);
            }, RegexOptions.IgnoreCase);

            e.Row.Cells[2].Text = Regex.Replace(e.Row.Cells[2].Text, txtSearchByPhone.Text.Trim(), delegate(Match match)
            {
                return string.Format("<span style = 'background-color:green; color:white;'>{0}</span>", match.Value);
            }, RegexOptions.IgnoreCase);

        }
    }

    protected void btnGo1_Click(object sender, EventArgs e)
    {
        fnLoadData_BySearch();
    }

    protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        fnLoadData_SearchBySex();
    }


    protected void fnLoadData_SearchBySex()
    {
        //if (System.Convert.ToInt32(Session["Vis"]) == 0)
        //{
        //    Response.Redirect("Default.aspx");
        //}

        SqlConnection conn = DBConnection.GetConnection();

        string sSql = "";
        sSql = "SELECT VAID, VName, NickName, VContact, VAddress, VPhoto, VNID, VRef, VType, status";
        sSql = sSql + " FROM  dbo.tbVendorInfo";
        sSql = sSql + " WHERE status=1";

        if (RadioButtonList1.SelectedItem.Text=="Own")
        {
            sSql = sSql + " AND (VType='Own')";
        }
        else if (RadioButtonList1.SelectedItem.Text == "Franchise")
        {
            sSql = sSql + " AND (VType='Franchise')";
        }

        sSql = sSql + " ORDER BY VName";

        SqlCommand cmd = new SqlCommand(sSql, conn);
        //OleDbDataReader dr = cmd.ExecuteReader();
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

        if (ds.Tables[0].Rows.Count > 0)
        {
            GridView1.DataSource = ds.Tables[0];
            GridView1.DataBind();
        }


    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        //
    }

    

}