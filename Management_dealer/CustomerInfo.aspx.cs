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


public partial class CTP_CustomerInfo : System.Web.UI.Page
{
    long i;


    int iMRSRID = 0;
    DataTable dt;
    DateTime tDate;

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

            // LOAD ALL DATA IN GRID
            fnLoadData();

            fnLoadDay(ddlDay);
            fnLoadMonth(ddlMonth);
            fnLoadYear(ddlYear);

            //fnLoadCombo(ddlCity, "CatName", "CategoryID", "Customer");
            //LOAD CITY
            LoadDropDownList_City();

            //LOAD CTP
            LoadDropDownList_CTP();

            dt = new DataTable();
            MakeTable();


            if (Request.QueryString["action"] == "delete" && Request.QueryString["id"] != "")
            {

                SqlConnection conn = DBConnection.GetConnection();

                try
                {
                    SqlCommand dataCommand = new SqlCommand();
                    dataCommand.Connection = conn;
                    dataCommand.CommandType = CommandType.Text;

                    dataCommand.CommandText = "DELETE FROM Customer WHERE CategoryID=" + Request.QueryString["id"];

                    conn.Open();
                    dataCommand.ExecuteNonQuery();
                    conn.Close();
                    Response.Redirect("CustomerInfo.aspx");

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

                    sSql = "Select CustAID, CustName, Address, Mobile, Email, City, Profession, Org,";
                    sSql = sSql + " Desg, CustSex, CustRef, DOB, DOBT, CustType";
                    sSql = sSql + " FROM Customer WHERE CustAID=" + Request.QueryString["id"];
                    dataCommand.CommandText = sSql;

                    conn.Open();
                    SqlDataReader dr = dataCommand.ExecuteReader();
                    while (dr.Read())
                    {
                        ID.Text = dr["CustAID"].ToString();
                        txtAID.Text = dr["CustAID"].ToString();
                        txtName.Text = dr["CustName"].ToString();
                        txtMobile.Text = dr["Mobile"].ToString();
                        Session["cMobile"] = dr["Mobile"].ToString();
                        txtAdd.Text = dr["Address"].ToString();
                        //ddlCity.SelectedItem.Text = dr["City"].ToString();
                        this.ddlCity.SelectedItem.Text = dr["City"].ToString();

                        txtEmail.Text = dr["Email"].ToString();
                        txtProfession.Text = dr["Profession"].ToString();
                        txtOrg.Text = dr["Org"].ToString();
                        txtDesg.Text = dr["Desg"].ToString();
                        txtDOB.Text = dr["DOBT"].ToString();

                        //if (dr["status"].ToString() == "True")
                        //{
                        //    status.Checked = true;
                        //}

                        ddlType.SelectedItem.Text = dr["CustType"].ToString();
                        //ddlType.SelectedItem.Text = dr["CatType"].ToString();
                        //ddlType.SelectedItem.Value = dr["CategoryID"].ToString();

                        if (dr["CustSex"].ToString() == "Male")
                        {
                            optSex.SelectedIndex = 0;
                        }
                        else if (dr["CustSex"].ToString() == "Female")
                        {
                            optSex.SelectedIndex = 1;
                        }
                        else if (dr["CustSex"].ToString() == "Others")
                        {
                            optSex.SelectedIndex = 2;
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
            else if (Request.QueryString["action"] == "view" && Request.QueryString["id"] != "")
            {
                //CLEAR GRIDVIEW
                gvUsers.DataSource = null;
                gvUsers.DataBind();

                //CLEAR DATA TABLE
                dt.Clear();

                //LinkButton btnsubmit = sender as LinkButton;
                //GridViewRow gRow = (GridViewRow)btnsubmit.NamingContainer;
                ////txtPID.Text = gvCustomres.DataKeys[gRow.RowIndex].Value.ToString();
                ////string sPID = Convert.ToString(gvCustomres.DataKeys[gRow.RowIndex].Value.ToString());
                //string sPID = GridView1.DataKeys[gRow.RowIndex].Value.ToString();
                ////txtPName.Text = gRow.Cells[0].Text;        
                ////this.ModalPopupExtender1.Show();
                //string sMobile = gRow.Cells[5].Text;

                SqlConnection conn = DBConnection.GetConnection();

                string sSql = "";

                sSql = " SELECT CustName,Mobile,Address,CustSex,Profession,Email,Org,Desg,City,CustArea,";
                sSql=sSql + " DOBT,CustAge FROM Customer WHERE CustAID='" + Request.QueryString["id"] + "'";
                //" CONVERT(varchar(12), TDate, 101) AS TDate, dbo.MRSRMaster.OutSource," +

                //sSql = sSql + " WHERE tbMemberList.ID= " + sPID + "";
                SqlCommand cmd = new SqlCommand(sSql, conn);
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    //this.lblID.Text = dr["MRSRMID"].ToString();
                    //this.lblInv.Text = dr["MRSRCode"].ToString();
                    //this.lblDate.Text = dr["TDate"].ToString();

                    this.lblCustName.Text = dr["CustName"].ToString();
                    this.lblContact.Text = dr["Mobile"].ToString();
                    this.lblAdd.Text = dr["Address"].ToString();
                    this.lblSex.Text = dr["CustSex"].ToString();
                    this.lblProfession.Text = dr["Profession"].ToString();
                    this.lblEmail.Text = dr["Email"].ToString();
                    this.lblOrg.Text = dr["Org"].ToString();
                    this.lblDesg.Text = dr["Desg"].ToString();

                    this.lblCity.Text = dr["City"].ToString();
                    this.lblLoc.Text = dr["CustArea"].ToString();
                    this.lblDOB.Text = dr["DOBT"].ToString();
                    this.lblAge.Text = dr["CustAge"].ToString();

                    //Image1.ImageUrl = "img/photos/" + dr["path"].ToString();

                }
                else
                {
                    this.lblID.Text = "";
                    //this.lblInv.Text = "";
                    //this.lblDate.Text = "";

                    this.lblCustName.Text = "";
                    this.lblContact.Text = "";
                    this.lblAdd.Text = "";
                    this.lblSex.Text = "";
                    this.lblProfession.Text = "";
                    this.lblEmail.Text = "";
                    this.lblOrg.Text = "";
                    this.lblDesg.Text = "";

                    this.lblCity.Text = "";
                    this.lblLoc.Text = "";
                    this.lblDOB.Text = "";
                    this.lblAge.Text = "";

                }

                conn.Close();

                //LOAD DETAILS DATA
                sSql = "";
                sSql = "SELECT dbo.MRSRMaster.TrType, dbo.MRSRMaster.MRSRMID, dbo.MRSRMaster.MRSRCode,";
                sSql = sSql + " CONVERT(varchar, dbo.MRSRMaster.TDate, 110) AS TDate, dbo.Entity.eName, dbo.Entity.EID, ";
                sSql = sSql + " dbo.MRSRDetails.ProductID, dbo.Product.Model, dbo.MRSRDetails.UnitPrice,";
                sSql = sSql + " ABS(dbo.MRSRDetails.Qty) AS Qty, dbo.MRSRDetails.TotalAmnt, ";
                sSql = sSql + " dbo.MRSRDetails.DiscountAmnt, dbo.MRSRDetails.WithAdjAmnt, dbo.MRSRDetails.NetAmnt,";
                sSql = sSql + " dbo.Customer.CustID, dbo.Customer.CustName, ";
                sSql = sSql + " dbo.Customer.Address, dbo.Customer.Mobile, dbo.Customer.Email, dbo.Customer.City,";
                sSql = sSql + " dbo.Customer.Profession, dbo.Customer.Org, ";
                sSql = sSql + " dbo.Customer.Desg, dbo.Customer.CustSex, dbo.Customer.DOB, dbo.Customer.DOBT,";
                sSql = sSql + " dbo.Customer.CustType, dbo.Customer.CustArea, ";
                sSql = sSql + " dbo.Customer.CustAge, dbo.MRSRDetails.ProdRemarks, dbo.MRSRDetails.SLNO";
                sSql = sSql + " FROM  dbo.Product INNER JOIN";
                sSql = sSql + " dbo.MRSRDetails ON dbo.Product.ProductID = dbo.MRSRDetails.ProductID INNER JOIN";
                sSql = sSql + " dbo.MRSRMaster ON dbo.MRSRDetails.MRSRMID = dbo.MRSRMaster.MRSRMID INNER JOIN";
                sSql = sSql + " dbo.Entity ON dbo.MRSRMaster.OutSource = dbo.Entity.EID INNER JOIN";
                sSql = sSql + " dbo.Customer ON dbo.MRSRMaster.Customer = dbo.Customer.Mobile";

                sSql = sSql + " WHERE  (dbo.MRSRMaster.TrType = 3) AND (dbo.Customer.CustAID='" + Request.QueryString["id"] + "')";


                cmd = new SqlCommand(sSql, conn);
                conn.Open();

                // Create a SqlDataAdapter to get the results as DataTable
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sSql, conn);

                // Fill the DataTable with the result of the SQL statement
                sqlDataAdapter.Fill(dt);

                gvUsers.DataSource = dt;
                gvUsers.DataBind();
            }

            

        }

        else
        {
            dt = (DataTable)ViewState["dt"];
        }
        ViewState["dt"] = dt;
        
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
        string thumfilename = "";
        DateTime nm = DateTime.Now;
        string date = nm.ToString("yyyyMMddhhmmss");
        //Session["Imagename"] = date + ".jpg";

        string path, path2, file;
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

        if (optSex.SelectedIndex == 0)
        {
            cSex = "Male";
        }
        else if (optSex.SelectedIndex == 1)
        {
            cSex = "Female";
        }
        else if (optSex.SelectedIndex == 2)
        {
            cSex = "Others";
        }

        string DOBDate = ddlYear.SelectedItem.Text + "" + ddlMonth.SelectedItem.Value + "" + ddlDay.SelectedItem.Text;
        string stDate = ddlYear.SelectedItem.Text + "/" + ddlMonth.SelectedItem.Value + "/" + ddlDay.SelectedItem.Text;
        DateTime dtDOB = Convert.ToDateTime(stDate);


        if (ID.Text == "")
        {
            gSql = "";
            gSql = "INSERT INTO Customer(CustName, Address, City, Mobile,Email,Profession,";
            gSql = gSql + " Org,Desg,CustSex,CustRef,DOB,DOBT,CustType)";
            gSql = gSql + " VALUES ('" + txtName.Text.Replace("'", "''") + "',";
            gSql = gSql + " '" + txtAdd.Text.Replace("'", "''") + "','" + ddlCity.SelectedItem.Text + "',";
            gSql = gSql + " '" + txtMobile.Text + "','" + txtEmail.Text + "','" + txtProfession.Text + "',";
            gSql = gSql + " '" + txtOrg.Text + "','" + txtDesg.Text + "','" + cSex + "','N/A',";
            gSql = gSql + " '" + dtDOB + "','" + DOBDate + "','" + ddlType.SelectedItem.Text + "'";            
            gSql = gSql + " )";

            dataCommand.CommandText = gSql;
        }

        else
        {           
            
            gSql = "";
            gSql = "UPDATE Customer SET CustName='" + txtName.Text + "', Address='" + txtAdd.Text + "',";
            gSql = gSql + " City='" + ddlCity.SelectedItem.Text + "', Mobile='" + txtMobile.Text + "',";
            gSql = gSql + " Email='" + txtEmail.Text + "', Profession='" + txtProfession.Text + "',";
            gSql = gSql + " Org='" + txtOrg.Text + "', Desg='" + txtDesg.Text + "',";
            gSql = gSql + " CustSex='" + cSex + "', DOB='" + dtDOB + "',";
            gSql = gSql + " DOBT='" + DOBDate + "', CustType='" + ddlType.SelectedItem.Text + "'";

            gSql = gSql + " WHERE CustAID=" + ID.Text;

            dataCommand.CommandText = gSql;

        }
                
        //Label1.Text = dataCommand.CommandText;
        conn.Open();
        dataCommand.ExecuteNonQuery();
        conn.Close();

        //----------------------------------------------------------------------------
        if (ID.Text != "")
        {
            gSql = "";
            gSql = "UPDATE MRSRMaster SET Customer='" + txtMobile.Text + "'";
            gSql = gSql + " WHERE Customer='" + Session["cMobile"].ToString() + "'";

            dataCommand1.CommandText = gSql;
        }
        conn.Open();
        dataCommand1.ExecuteNonQuery();
        conn.Close();
        //----------------------------------------------------------------------------

        Response.Redirect("CustomerInfo.aspx");

        //}
        //catch (Exception ex)
        //{
        //   // Label1.Text = ex.Message;
        //  //Label1.Text = Label1.Text+"INSERT INTO Customer (title, description, parent,status,order) VALUES ('" + txttitle.Text + "', '" + description.Text + "', '" + parent.Value + "','" + "0" + "','" + order.Text + "')";

        //}


    }


    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("CustomerInfo.aspx");
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
        fnLoadData();
    }

    protected void fnLoadData()
    {
        //if (System.Convert.ToInt32(Session["Vis"]) == 0)
        //{
        //    Response.Redirect("Default.aspx");
        //}

        SqlConnection conn = DBConnection.GetConnection();
        
        string sSql = "";

        //sSql = "SELECT Customer.CustAID, Customer.CustName, Customer.Address, Customer.CustSex,";
        //sSql = sSql + " Customer.Mobile, Customer.Email, Customer.City, Customer.DOBT,";
        //sSql = sSql + " Customer.CustType";
        //sSql = sSql + " FROM Customer";
        //sSql = sSql + " WHERE ((Customer.CustAID)>0)";

        sSql = "SELECT  dbo.MRSRMaster.TrType, dbo.Entity.eName, dbo.Customer.CustAID, dbo.Customer.CustID,";
        sSql = sSql + " dbo.Customer.CustName, dbo.Customer.CustFatherName, dbo.Customer.Address,";
        sSql = sSql + " dbo.Customer.Phone,dbo.Entity.EID,"; 
        sSql = sSql + " dbo.Customer.Mobile, dbo.Customer.Email, dbo.Customer.ContPer1, dbo.Customer.ContPer2,";
        sSql = sSql + " dbo.Customer.City, dbo.Customer.Country, dbo.Customer.Profession, dbo.Customer.Org, ";
        sSql = sSql + " dbo.Customer.Desg, dbo.Customer.CustSex, dbo.Customer.CustRef, dbo.Customer.DOB,";
        sSql = sSql + " dbo.Customer.DOBT, dbo.Customer.CustType";
        sSql = sSql + " FROM  dbo.Customer INNER JOIN";
        sSql = sSql + " dbo.MRSRMaster ON dbo.Customer.Mobile = dbo.MRSRMaster.Customer INNER JOIN";
        sSql = sSql + " dbo.Entity ON dbo.MRSRMaster.OutSource = dbo.Entity.EID";
        sSql = sSql + " WHERE (dbo.MRSRMaster.TrType = 3)";

        //sSql = sSql + " AND (dbo.Entity.EID = '" + Session["EID"] + "')";
        if (ddlCTP.SelectedValue != "0")
        {
            sSql = sSql + " AND ((dbo.Entity.EID)='" + ddlCTP.SelectedValue + "')";
        }

        if (DropDownList1.SelectedItem.Text != "ALL")
        {
            sSql = sSql + " AND ((dbo.Customer.CustType)='" + DropDownList1.SelectedItem.Text + "')";
        }

        sSql = sSql + " GROUP BY dbo.MRSRMaster.TrType, dbo.Entity.eName, dbo.Customer.CustAID, dbo.Customer.CustID,";
        sSql = sSql + " dbo.Customer.CustName, dbo.Customer.CustFatherName, dbo.Customer.Address, dbo.Customer.Phone,";
        sSql = sSql + " dbo.Entity.EID,dbo.Customer.Mobile, dbo.Customer.Email, dbo.Customer.ContPer1, dbo.Customer.ContPer2,";
        sSql = sSql + " dbo.Customer.City, dbo.Customer.Country, dbo.Customer.Profession, dbo.Customer.Org, ";
        sSql = sSql + " dbo.Customer.Desg, dbo.Customer.CustSex, dbo.Customer.CustRef, dbo.Customer.DOB,";
        sSql = sSql + " dbo.Customer.DOBT, dbo.Customer.CustType";


        sSql = sSql + " ORDER BY dbo.Customer.CustName";

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

    //LOAD CITY IN DROPDOWN LIST
    protected void LoadDropDownList_City()
    {
        //if (System.Convert.ToInt32(Session["Vis"]) == 0)
        //{
        //    Response.Redirect("../Account/Login.aspx");
        //}

        SqlConnection conn = DBConnection.GetConnection();

        String strQuery = "select DISTINCT Dist from tbDistThana Order By Dist";
        //SqlConnection con = new SqlConnection("conn");
        SqlCommand cmd = new SqlCommand(strQuery, conn);
        //SqlCommand cmd = new SqlCommand();
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = strQuery;
        cmd.Connection = conn;
        try
        {
            conn.Open();
            ddlCity.DataSource = cmd.ExecuteReader();
            ddlCity.DataTextField = "Dist";
            //ddlCity.DataValueField = "ProductID";
            ddlCity.DataValueField = "Dist";
            ddlCity.DataBind();

            //Add blank item at index 0.
            ddlCity.Items.Insert(0, new ListItem("", ""));

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

    protected void txtSearchByPhone_TextChanged(object sender, EventArgs e)
    {
        fnLoadData_BySearch();
    }


    protected void fnLoadData_BySearch()
    {
        //if (System.Convert.ToInt32(Session["Vis"]) == 0)
        //{
        //    Response.Redirect("Default.aspx");
        //}

        SqlConnection conn = DBConnection.GetConnection();

        string sSql = "";

        //sSql = "SELECT Customer.CustAID, Customer.CustName, Customer.Address, Customer.CustSex,";
        //sSql = sSql + " Customer.Mobile, Customer.Email, Customer.City, Customer.DOBT,";
        //sSql = sSql + " Customer.CustType";
        //sSql = sSql + " FROM Customer";
        //sSql = sSql + " WHERE ((Customer.CustAID)>0)";

        sSql = "SELECT  dbo.MRSRMaster.TrType, dbo.Entity.eName, dbo.Customer.CustAID, dbo.Customer.CustID,";
        sSql = sSql + " dbo.Customer.CustName, dbo.Customer.CustFatherName, dbo.Customer.Address,";
        sSql = sSql + " dbo.Customer.Phone,dbo.Entity.EID,";
        sSql = sSql + " dbo.Customer.Mobile, dbo.Customer.Email, dbo.Customer.ContPer1, dbo.Customer.ContPer2,";
        sSql = sSql + " dbo.Customer.City, dbo.Customer.Country, dbo.Customer.Profession, dbo.Customer.Org, ";
        sSql = sSql + " dbo.Customer.Desg, dbo.Customer.CustSex, dbo.Customer.CustRef, dbo.Customer.DOB,";
        sSql = sSql + " dbo.Customer.DOBT, dbo.Customer.CustType";
        sSql = sSql + " FROM  dbo.Customer INNER JOIN";
        sSql = sSql + " dbo.MRSRMaster ON dbo.Customer.Mobile = dbo.MRSRMaster.Customer INNER JOIN";
        sSql = sSql + " dbo.Entity ON dbo.MRSRMaster.OutSource = dbo.Entity.EID";
        sSql = sSql + " WHERE (dbo.MRSRMaster.TrType = 3)";
        //sSql = sSql + " AND (dbo.Entity.EID = '" + Session["EID"] + "')";

        if (ddlCTP.SelectedValue != "0")
        {
            sSql = sSql + " AND ((dbo.Entity.EID)='" + ddlCTP.SelectedValue + "')";
        }

        sSql = sSql + " AND ((dbo.Customer.Mobile  Like '%" + txtSearchByPhone.Text + "%')";
        sSql = sSql + " OR (dbo.Customer.CustName  Like '%" + txtSearchByPhone.Text + "%'))";
        
        sSql = sSql + " GROUP BY dbo.MRSRMaster.TrType, dbo.Entity.eName, dbo.Customer.CustAID, dbo.Customer.CustID,";
        sSql = sSql + " dbo.Customer.CustName, dbo.Customer.CustFatherName, dbo.Customer.Address, dbo.Customer.Phone,";
        sSql = sSql + " dbo.Entity.EID,dbo.Customer.Mobile, dbo.Customer.Email, dbo.Customer.ContPer1, dbo.Customer.ContPer2,";
        sSql = sSql + " dbo.Customer.City, dbo.Customer.Country, dbo.Customer.Profession, dbo.Customer.Org, ";
        sSql = sSql + " dbo.Customer.Desg, dbo.Customer.CustSex, dbo.Customer.CustRef, dbo.Customer.DOB,";
        sSql = sSql + " dbo.Customer.DOBT, dbo.Customer.CustType";


        sSql = sSql + " ORDER BY dbo.Customer.CustName";

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
            e.Row.Cells[5].Text = Regex.Replace(e.Row.Cells[5].Text, txtSearchByPhone.Text.Trim(), delegate(Match match)
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

        //sSql = "SELECT Customer.CustAID, Customer.CustName, Customer.Address, Customer.CustSex,";
        //sSql = sSql + " Customer.Mobile, Customer.Email, Customer.City, Customer.DOBT,";
        //sSql = sSql + " Customer.CustType";
        //sSql = sSql + " FROM Customer";
        //sSql = sSql + " WHERE ((Customer.CustAID)>0)";

        sSql = "SELECT  dbo.MRSRMaster.TrType, dbo.Entity.eName, dbo.Customer.CustAID, dbo.Customer.CustID,";
        sSql = sSql + " dbo.Customer.CustName, dbo.Customer.CustFatherName, dbo.Customer.Address,";
        sSql = sSql + " dbo.Customer.Phone,dbo.Entity.EID,";
        sSql = sSql + " dbo.Customer.Mobile, dbo.Customer.Email, dbo.Customer.ContPer1, dbo.Customer.ContPer2,";
        sSql = sSql + " dbo.Customer.City, dbo.Customer.Country, dbo.Customer.Profession, dbo.Customer.Org, ";
        sSql = sSql + " dbo.Customer.Desg, dbo.Customer.CustSex, dbo.Customer.CustRef, dbo.Customer.DOB,";
        sSql = sSql + " dbo.Customer.DOBT, dbo.Customer.CustType";
        sSql = sSql + " FROM  dbo.Customer INNER JOIN";
        sSql = sSql + " dbo.MRSRMaster ON dbo.Customer.Mobile = dbo.MRSRMaster.Customer INNER JOIN";
        sSql = sSql + " dbo.Entity ON dbo.MRSRMaster.OutSource = dbo.Entity.EID";
        sSql = sSql + " WHERE (dbo.MRSRMaster.TrType = 3)";
        sSql = sSql + " AND (dbo.Entity.EID = '" + Session["EID"] + "')";

        if (RadioButtonList1.SelectedIndex == 1)
        {
            sSql = sSql + " AND (dbo.Customer.CustSex = 'Male')";
        }
        if (RadioButtonList1.SelectedIndex == 2)
        {
            sSql = sSql + " AND (dbo.Customer.CustSex = 'Female')";
        }
        if (RadioButtonList1.SelectedIndex == 3)
        {
            sSql = sSql + " AND (dbo.Customer.CustSex = 'Others')";
        }

        //sSql = sSql + " AND ((dbo.Customer.Mobile  Like '%" + txtSearchByPhone.Text + "%')";
        //sSql = sSql + " OR (dbo.Customer.CustName  Like '%" + txtSearchByPhone.Text + "%'))";

        sSql = sSql + " GROUP BY dbo.MRSRMaster.TrType, dbo.Entity.eName, dbo.Customer.CustAID, dbo.Customer.CustID,";
        sSql = sSql + " dbo.Customer.CustName, dbo.Customer.CustFatherName, dbo.Customer.Address, dbo.Customer.Phone,";
        sSql = sSql + " dbo.Entity.EID,dbo.Customer.Mobile, dbo.Customer.Email, dbo.Customer.ContPer1, dbo.Customer.ContPer2,";
        sSql = sSql + " dbo.Customer.City, dbo.Customer.Country, dbo.Customer.Profession, dbo.Customer.Org, ";
        sSql = sSql + " dbo.Customer.Desg, dbo.Customer.CustSex, dbo.Customer.CustRef, dbo.Customer.DOB,";
        sSql = sSql + " dbo.Customer.DOBT, dbo.Customer.CustType";


        sSql = sSql + " ORDER BY dbo.Customer.CustName";

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

    

    //LOAD CTP IN DROPDOWN LIST
    protected void LoadDropDownList_CTP()
    {
        SqlConnection conn = DBConnection.GetConnection();

        String strQuery = "Select EID,eName from Entity ";
        strQuery = strQuery + " WHERE (ActiveDeactive = 1) AND";
        strQuery = strQuery + " (EntityType = 'showroom')";
        //strQuery = strQuery + " OR  EntityType = 'Dealer')";
        strQuery = strQuery + " ORDER BY eName";
        //SqlConnection con = new SqlConnection("conn");
        SqlCommand cmd = new SqlCommand(strQuery, conn);
        //SqlCommand cmd = new SqlCommand();
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = strQuery;
        cmd.Connection = conn;
        try
        {
            conn.Open();
            ddlCTP.DataSource = cmd.ExecuteReader();
            ddlCTP.DataTextField = "eName";
            ddlCTP.DataValueField = "EID";
            ddlCTP.DataBind();

            //Add blank item at index 0.
            ddlCTP.Items.Insert(0, new ListItem("ALL", "0"));
            //ddlEntity.Items.Insert(1, new ListItem("CI&DD (REL)", "370"));

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
        dt.Columns.Add("MRSRCode");
        //dt.Columns.Add("ProductID", typeof(SqlInt32));
        dt.Columns.Add("TDate");
        dt.Columns.Add("eName");
        dt.Columns.Add("Model");
        dt.Columns.Add("UnitPrice");
        dt.Columns.Add("Qty");
        dt.Columns.Add("TotalAmnt");
        dt.Columns.Add("DiscountAmnt");
        dt.Columns.Add("WithAdjAmnt");
        dt.Columns.Add("NetAmnt");
        dt.Columns.Add("SLNO");
        dt.Columns.Add("ProdRemarks");

    }


    protected void Button1_Click(object sender, EventArgs e)
    {
        Response.Redirect("CustomerInfo.aspx");
    }

}