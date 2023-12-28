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

using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;

public partial class tag_share_promocode : System.Web.UI.Page
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
            //fnLoadCombo_Item(ddlProduct, "Model", "ProductID", "tbProduct");

            //fnLoadDay(ddlDay);
            //fnLoadMonth(ddlMonth);
            //fnLoadYear(ddlYear);

            //fnLoadPromoCode();

        }

    }

    private void fnLoadCombo_Item(DropDownList xCombo, string pField, string pFieldID, string pTable)
    {
        //if (System.Convert.ToInt32(Session["Vis"]) == 0)
        //{
        //    Response.Redirect("~/Default.aspx");
        //}

        SqlConnection conn = DBConnectionSpin.GetConnection();

        try
        {
            conn.Open();
            //string sqlfns = "SELECT * FROM " + pTable + " WHERE CompID=" + Session["CompID"] + " ORDER BY " + pField + " ASC";
            string sqlfns = "SELECT * FROM " + pTable + " ORDER BY " + pField + " ASC";
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

        //if ((ddlDay.SelectedItem.Text == "") && (ddlMonth.SelectedItem.Text == "") && (ddlYear.Text == ""))
        //{
        //    ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert",
        //            "<script>alert('" + "Invalid Date format." + "');</script>", false);
        //    return;
        //}

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

        if (DropDownList1.SelectedItem.Text == "0")
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert",
                    "<script>alert('" + "Please select number of Tag & Share !!!" + "');</script>", false);
            //txtChNo.Focus();
            return;
        }

        if (ImageUpload.HasFile == false)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert",
                    "<script>alert('" + "Please Select Photo !!!" + "');</script>", false);
            //txtChNo.Focus();
            return;
        }

        SqlConnection conn = DBConnection.GetConnection();
        

        //----------------------------------------------------------------------
        //CHECK DUPLICATE CHALLAN NO.
        string sSql = "";
        sSql = "SELECT CustMobile, ChNo FROM tbTagPromocode" ;
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

        string filename = "";
        string path = "", file = "", NewPath="";
        DateTime nm = DateTime.Now;
        string date = nm.ToString("yyyyMMddhhmmss");        
        string date1 = date + "1";

        //if (ImageUpload.HasFile)
        //{
        //    try
        //    {
        //        filename = Path.GetFileName(ImageUpload.FileName);
        //        ImageUpload.SaveAs(Server.MapPath("~/") + "/images/tag_share/" + filename);
        //        // StatusLabel.Text = "Upload status: File uploaded!";
        //    }
        //    catch (Exception ex)
        //    {
        //        //StatusLabel.Text = "Upload status: The file could not be uploaded. The following error occured: " + ex.Message;
        //    }
        //}

        try
        {
            //------------------- IMAGE 1 --------------------------------------------------------
            if (ImageUpload.PostedFile != null)
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
                        int newWidth = 1200; // New Width of Image in Pixel
                        int newHeight = 667; // New Height of Image in Pixel
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
                        string targetPath = Server.MapPath("~/") + "/images/tag_share/" + filename;
                        thumbImg.Save(targetPath, image.RawFormat);

                    }
                }
            }
        }
        catch
        {
            //
        }


        //string sDOB = ddlYear.SelectedItem.Text + "" + ddlMonth.SelectedItem.Value + "" + ddlDay.SelectedItem.Text;
        //string stDate = ddlYear.SelectedItem.Text + "/" + ddlMonth.SelectedItem.Value + "/" + ddlDay.SelectedItem.Text;
        //DateTime tDate = Convert.ToDateTime(stDate);

        try
        {
            // SAVE DATA
            sSql = "";
            sSql = "INSERT INTO tbTagPromocode(CustName,CustMobile,CustDOB,";
            sSql = sSql + " ProductID, Model, ChNo, DisCode, EntryDate, EID, NoOfTag, path)";
            sSql = sSql + " VALUES(";
            sSql = sSql + " '" + txtName.Text + "','" + txtContact.Text + "','" + DateTime.Today + "',";
            sSql = sSql + " '" + lblProdID.Text + "','" + txtModel.Text + "','" + txtChNo.Text + "', '" + txtChNo.Text + "',";
            sSql = sSql + " '" + DateTime.Today + "', '" + Session["EID"].ToString() + "',";
            sSql = sSql + " '" + DropDownList1.SelectedItem.Text + "','" + filename + "')";
            //sSql = sSql + " WHERE AID='" + lblAID.Text + "'";

            SqlCommand cmd = new SqlCommand(sSql, conn);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        catch
        {
            //
        }


        //******************************************************************************************
        // FOR SMS   
        try
        {
            SqlConnection connSMS = DBConnectionSMS.GetConnection();

            if (txtContact.Text != "")
            {
                string smsText = "";
                smsText = "Dear " + this.txtName.Text + ",\n";
                smsText = smsText + "Your promocode is: " + txtChNo.Text + "\n";
                //smsText = smsText + "for Sony Flagship Anniversary Toss & Win.\n";            
                smsText = smsText + "for discount by Tag & Share.\n";
                //smsText = smsText + "Product:\n";
                //smsText = smsText + "" + sProd + "";
                smsText = smsText + "Thank you.\n";
                //smsText = smsText + "Sony-Rangs";
                smsText = smsText + "" + Session["eName"].ToString() + "";

                sSql = "";
                sSql = "INSERT INTO tbSMS(ContactNo,SMSText,UserID,EntryDate,SMSSource)" +
                        " Values ('" + this.txtContact.Text + "','" + smsText + "'," +
                        " '" + Session["UserID"] + "','" + DateTime.Today + "'," +
                        " 'Tag-Share'" +
                        " )";
                SqlCommand cmdSMS11 = new SqlCommand(sSql, connSMS);
                connSMS.Open();
                cmdSMS11.ExecuteNonQuery();
                connSMS.Close();

                // SEND TO SAJAL
                sSql = "";
                sSql = "INSERT INTO tbSMS(ContactNo,SMSText,UserID,EntryDate,SMSSource)" +
                        " Values ('01708151444','" + smsText + "'," +
                        " '" + Session["UserID"] + "','" + DateTime.Today + "'," +
                        " 'Tag-Share'" +
                        " )";
                SqlCommand cmdSMS = new SqlCommand(sSql, connSMS);
                connSMS.Open();
                cmdSMS.ExecuteNonQuery();
                connSMS.Close();


                // SEND TO ME
                sSql = "";
                sSql = "INSERT INTO tbSMS(ContactNo,SMSText,UserID,EntryDate,SMSSource)" +
                        " Values ('01707000037','" + smsText + "'," +
                        " '" + Session["UserID"] + "','" + DateTime.Today + "'," +
                        " 'Tag-Share'" +
                        " )";
                cmdSMS = new SqlCommand(sSql, connSMS);
                connSMS.Open();
                cmdSMS.ExecuteNonQuery();
                connSMS.Close();

            }
        }
        catch
        {
            //
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
                "<script>alert('" + "Submit Success ...." + "');</script>", false);

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
        SqlConnection conn = DBConnectionSpin.GetConnection();
        string sSql = "";

        double UP = 0, bUP = 0;
        
        sSql = "";
        sSql = "SELECT ProductID, Code, Model, Description, MRP, SellingPrice, MinDis, MaxDis";        
        sSql = sSql + " FROM tbProduct ";
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
                this.txtDesc.Text = dr["Description"].ToString();
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
        fnLoadPromoCode();
    }


    protected void UploadFile(object sender, EventArgs e)
    {
        //
    }

    private void fnLoadPromoCode()
    {
        SqlConnection conn = DBConnection.GetConnection();

        Random generator = new Random();
        String r = generator.Next(0, 999999).ToString("D6");

        // single random character in ascii range a-z
        const string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        Random rnd = new Random((DateTime.Now.Millisecond));
        string alphaChar = alphabet.Substring(rnd.Next(0, alphabet.Length - 1), 4) + r;

        try
        {
            string gSql = "";
            gSql = "SELECT DisCode ";
            gSql = gSql + " FROM tbTagPromocode WHERE DisCode='" + alphaChar + "'";
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
                txtChNo.Text = alphaChar;
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