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


public partial class Search_Installation : System.Web.UI.Page
{
    SqlConnection conn = DBConnection.GetConnection();
    long i;

    private double runningTotalQty = 0;
    private double runningTotalTP = 0;
    private double runningTotalCash = 0;
    private double runningTotalCard = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (System.Convert.ToInt32(Session["Vis"]) == 0)
        {
            Response.Redirect("../Account/Login.aspx");
        }

        if (!IsPostBack)
        {
            this.txtFrom.Text = DateTime.Today.ToString("MM/dd/yyyy");
            this.txtToDate.Text = DateTime.Today.ToString("MM/dd/yyyy");

            //LOAD CTP NAME
            LoadDropDownList_CTP();

            LoadDropDownList_Vendor();

            LoadDropDownList_Cat();

            LoadDropDownList_Prod();

        }

    }


    //LOAD CTP IN DROPDOWN LIST
    protected void LoadDropDownList_CTP()
    {
        SqlConnection conn = DBConnection.GetConnection();

        String strQuery = "select EID,eName from Entity ";
        strQuery = strQuery + " WHERE (ActiveDeactive = 1) AND";
        strQuery = strQuery + " (EntityType = 'showroom' OR EntityType = 'Dealer')";
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
            ddlEntity.DataSource = cmd.ExecuteReader();
            ddlEntity.DataTextField = "eName";
            ddlEntity.DataValueField = "EID";
            ddlEntity.DataBind();

            //Add blank item at index 0.
            ddlEntity.Items.Insert(0, new ListItem("ALL", "0"));

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


    //LOAD VENDOR NAME IN DROPDOWN LIST
    protected void LoadDropDownList_Vendor()
    {
        SqlConnection conn = DBConnection.GetConnection();

        String strQuery = "SELECT VAID, VName FROM dbo.tbVendorInfo";
        strQuery = strQuery + " WHERE (status = 1)";
        strQuery = strQuery + " ORDER BY VName";
        //SqlConnection con = new SqlConnection("conn");
        SqlCommand cmd = new SqlCommand(strQuery, conn);
        //SqlCommand cmd = new SqlCommand();
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = strQuery;
        cmd.Connection = conn;
        try
        {
            conn.Open();
            ddlVendor.DataSource = cmd.ExecuteReader();
            ddlVendor.DataTextField = "VName";
            ddlVendor.DataValueField = "VAID";
            ddlVendor.DataBind();

            //Add blank item at index 0.
            ddlVendor.Items.Insert(0, new ListItem("ALL", "0"));

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

    //LOAD CATEGORY IN DROPDOWN LIST
    protected void LoadDropDownList_Cat()
    {
        SqlConnection conn = DBConnection.GetConnection();

        String strQuery = "SELECT DISTINCT GroupName FROM VW_Installation_Service";
        //strQuery = strQuery + " WHERE (status = 1)";
        //strQuery = strQuery + " ORDER BY VName";
        //SqlConnection con = new SqlConnection("conn");
        SqlCommand cmd = new SqlCommand(strQuery, conn);
        //SqlCommand cmd = new SqlCommand();
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = strQuery;
        cmd.Connection = conn;
        try
        {
            conn.Open();
            ddlCat.DataSource = cmd.ExecuteReader();
            ddlCat.DataTextField = "GroupName";
            ddlCat.DataValueField = "GroupName";
            ddlCat.DataBind();

            //Add blank item at index 0.
            ddlCat.Items.Insert(0, new ListItem("ALL", "0"));

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

    //LOAD Product IN DROPDOWN LIST
    protected void LoadDropDownList_Prod()
    {
        SqlConnection conn = DBConnection.GetConnection();

        String strQuery = "SELECT DISTINCT Model FROM VW_Installation_Service";
        //strQuery = strQuery + " WHERE (status = 1)";
        //strQuery = strQuery + " ORDER BY VName";
        //SqlConnection con = new SqlConnection("conn");
        SqlCommand cmd = new SqlCommand(strQuery, conn);
        //SqlCommand cmd = new SqlCommand();
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = strQuery;
        cmd.Connection = conn;
        try
        {
            conn.Open();
            ddlModel.DataSource = cmd.ExecuteReader();
            ddlModel.DataTextField = "Model";
            ddlModel.DataValueField = "Model";
            ddlModel.DataBind();

            //Add blank item at index 0.
            ddlModel.Items.Insert(0, new ListItem("ALL", "0"));

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


    protected void lnkView_Click(object sender, EventArgs e)
    {
        LinkButton btnsubmit = sender as LinkButton;
        GridViewRow gRow = (GridViewRow)btnsubmit.NamingContainer;
        //txtPID.Text = gvCustomres.DataKeys[gRow.RowIndex].Value.ToString();

        //Session["AssAID"] = GridView1.DataKeys[gRow.RowIndex].Value.ToString();
        Session["sIAID"] = GridView1.DataKeys[gRow.RowIndex].Value.ToString();
        lblAID.Text = gRow.Cells[1].Text;

        //string rowNumber = gRow.Cells[0].Text;
        string sRefNo = gRow.Cells[1].Text;

        fnLoadData_ForView();

        this.ModalPopupExtender1.Show();   

    }

    protected void lnkDel_Click(object sender, EventArgs e)
    {
        LinkButton btnsubmit = sender as LinkButton;
        GridViewRow gRow = (GridViewRow)btnsubmit.NamingContainer;
        //txtPID.Text = gvCustomres.DataKeys[gRow.RowIndex].Value.ToString();

        //Session["AssAID"] = GridView1.DataKeys[gRow.RowIndex].Value.ToString();
        Session["sIAID"] = GridView1.DataKeys[gRow.RowIndex].Value.ToString();
        Session["sRefNo"] = gRow.Cells[1].Text;

        //string rowNumber = gRow.Cells[0].Text;
        string sRefNo = gRow.Cells[1].Text;

        string sSql = "";

        ////DELETE FROM Master Table
        //sSql = "";
        //sSql = "DELETE FROM MRSRMaster";
        //sSql = sSql + " WHERE MRSRMID='" + sMasterID + "'";

        //SqlCommand cmd = new SqlCommand(sSql, conn);
        //conn.Open();
        //cmd.ExecuteNonQuery();
        //conn.Close();
        
        
        //LOAD DATA IN GRID
        fnLoadData();


    }

    protected void lnkEdit_Click(object sender, EventArgs e)
    {

        //   


    }

    protected void fnLoadData_ForView()
    {

        SqlConnection conn = DBConnection.GetConnection();

        //int iDay = Convert.ToInt16(txtDay.Text);
        //DateTime tDate = DateTime.Today.AddDays(-iDay);
        //DateTime tDate1 = DateTime.Today.AddDays(iDay);

        //lblDateCTPWise.Text = tDate.ToString("dd-MMM-yyyy");
        //lblSlowItemCaption.Text = txtDay.Text;


        string sSql = "";               
        sSql ="SELECT dbo.tbInstallationMaster.IAID, dbo.tbInstallationMaster.RefNo, dbo.tbInstallationDetails.IDAID,";
        sSql = sSql + " dbo.tbInstallationDetails.ProductID, dbo.Product.Model, dbo.Product.ProdName, ";
        sSql = sSql + " dbo.Product.GroupName, dbo.Product.Size_Capacity, dbo.Product.Size_Capacity_Unit,";
        sSql = sSql + " dbo.tbInstallationDetails.tQty, dbo.tbInstallationDetails.ProdRemarks"; 
        sSql = sSql + " FROM   dbo.tbInstallationMaster INNER JOIN";
        sSql = sSql + " dbo.tbInstallationDetails ON dbo.tbInstallationMaster.IAID = dbo.tbInstallationDetails.IAID INNER JOIN";
        sSql = sSql + " dbo.Product ON dbo.tbInstallationDetails.ProductID = dbo.Product.ProductID";

        sSql = sSql + " WHERE (dbo.tbInstallationMaster.RefNo = '" + lblAID.Text + "')";

        SqlCommand cmd = new SqlCommand(sSql, conn);
        conn.Open();
        SqlDataReader dr = cmd.ExecuteReader();

        GridView2.DataSource = dr;
        GridView2.DataBind();
        dr.Close();
        conn.Close();

        //-------------------------------------------------------------------------------------------------------
        sSql = "";        
        sSql ="SELECT  dbo.tbInstallationMaster.IAID, dbo.tbInstallationMaster.EID, dbo.Entity.eName,";
        sSql = sSql + " dbo.tbInstallationMaster.VAID, dbo.tbVendorInfo.VName, dbo.tbVendorInfo.VContact, dbo.tbVendorInfo.VAddress, ";
        sSql = sSql + " dbo.tbInstallationMaster.RefNo, dbo.tbInstallationMaster.RefDate, dbo.tbInstallationMaster.InvNo,";
        sSql = sSql + " dbo.MRSRMaster.TDate, dbo.MRSRMaster.TrType, dbo.tbInstallationMaster.CustMobile, ";
        sSql = sSql + " dbo.tbInstallationMaster.CustName, dbo.tbInstallationMaster.CustAdd, dbo.tbInstallationMaster.CustDist,";
        sSql = sSql + " dbo.tbInstallationMaster.CustThana, dbo.tbInstallationMaster.InstDateAprx, ";
        sSql = sSql + " dbo.tbInstallationMaster.InstDateAprx1, dbo.tbInstallationMaster.InstTimeAprx,";
        sSql = sSql + " dbo.tbInstallationMaster.InstStatus, dbo.tbInstallationMaster.UserID, dbo.tbInstallationMaster.EntryDate,"; 
        sSql = sSql + " dbo.tbInstallationMaster.InstDate, dbo.tbInstallationMaster.InsTime, dbo.tbInstallationMaster.SpecNote,";
        sSql = sSql + " dbo.tbInstallationMaster.Remarks,";
        //dbo.tbInstallationMaster.iTag";
        sSql = sSql + " CASE dbo.tbInstallationMaster.iTag WHEN 0 THEN 'Pending' WHEN 1 THEN 'Done' WHEN 2 THEN 'Cancel' END AS InsTag";
                
        sSql = sSql + " FROM dbo.tbInstallationMaster LEFT OUTER JOIN";
        sSql = sSql + " dbo.Entity ON dbo.tbInstallationMaster.EID = dbo.Entity.EID LEFT OUTER JOIN";
        sSql = sSql + " dbo.tbVendorInfo ON dbo.tbInstallationMaster.VAID = dbo.tbVendorInfo.VAID LEFT OUTER JOIN";
        sSql = sSql + " dbo.MRSRMaster ON dbo.tbInstallationMaster.InvNo = dbo.MRSRMaster.MRSRCode";

        sSql = sSql + " WHERE (dbo.tbInstallationMaster.RefNo = '" + lblAID.Text + "')";

        SqlCommand cmd1 = new SqlCommand(sSql, conn);
        conn.Open();
        SqlDataReader dr1 = cmd1.ExecuteReader();
        if (dr1.Read())
        {
            lblRefNo.Text = dr1["RefNo"].ToString();
            lblRefDate.Text = dr1["RefDate"].ToString();
            lblInv.Text = dr1["InvNo"].ToString();
            lblInvDate.Text = dr1["TDate"].ToString();
            lblCTP.Text = dr1["eName"].ToString();
            lblVName.Text = dr1["VName"].ToString();
            lblVAdd.Text = dr1["VAddress"].ToString();
            lblVContact.Text = dr1["VContact"].ToString();
            lblCustName.Text = dr1["CustName"].ToString();
            lblCustAdd.Text = dr1["CustAdd"].ToString();
            lblContact.Text = dr1["CustMobile"].ToString();
            lblInsDate.Text = dr1["InstDateAprx1"].ToString();
            lblInsTime.Text = dr1["InstTimeAprx"].ToString();
            lblStatus.Text = dr1["InsTag"].ToString();
        }
        else
        {
            lblRefNo.Text = "";
            lblRefDate.Text = "";
            lblInv.Text = "";
            lblInvDate.Text = "";
            lblCTP.Text = "";
            lblVName.Text = "";
            lblVAdd.Text = "";
            lblVContact.Text = "";
            lblCustName.Text = "";
            lblCustAdd.Text = "";
            lblContact.Text = "";
            lblInsDate.Text = "";
            lblInsTime.Text = "";
            lblStatus.Text = "";
        }
        dr1.Dispose();
        dr1.Close();
        conn.Close();


    }

    protected void PopupMessage(string Msg, Control controlID)
    {
        ScriptManager.RegisterClientScriptBlock(controlID, controlID.GetType(), "msg", "alert('" + Msg + "');", true);
    }

    protected void SearchData(object sender, EventArgs e)
    {
        //LOAD DATA IN GRID
        fnLoadData();
    }


    //LOAD SALES SUMMARY CHALLAN WISE
    private void fnLoadData()
    {
        //s = WebConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        //con = new SqlConnection(s);
        SqlConnection con = DBConnection.GetConnection();
        con.Open();

        string sSql = "";
        sSql ="SELECT dbo.tbInstallationMaster.IAID, dbo.tbInstallationMaster.EID, dbo.Entity.eName,";
        sSql = sSql + " dbo.tbInstallationMaster.VAID, dbo.tbVendorInfo.VName, dbo.tbVendorInfo.VContact,";
        sSql = sSql + " dbo.tbInstallationMaster.RefNo, dbo.tbInstallationMaster.InvNo,";
        sSql = sSql + " convert(varchar, dbo.tbInstallationMaster.RefDate, 105) AS RefDate, dbo.tbInstallationMaster.MRNo, dbo.tbInstallationMaster.CustMobile,";
        sSql = sSql + " dbo.tbInstallationMaster.CustName, dbo.tbInstallationMaster.CustAdd, ";
        sSql = sSql + " dbo.tbInstallationMaster.CustDist, dbo.tbInstallationMaster.CustThana, dbo.tbInstallationMaster.InstDateAprx,";
        sSql = sSql + " dbo.tbInstallationMaster.InstDateAprx1, dbo.tbInstallationMaster.InstTimeAprx, ";
        sSql = sSql + " dbo.tbInstallationMaster.InstStatus, dbo.Product.Model, dbo.Product.GroupName, dbo.Product.Size_Capacity,";
        sSql = sSql + " dbo.Product.Size_Capacity_Unit, dbo.tbInstallationDetails.tQty, ";
        sSql = sSql + " dbo.tbInstallationDetails.ProdRemarks, dbo.tbInstallationMaster.SpecNote, dbo.tbInstallationMaster.Remarks,";
        sSql = sSql + " CASE dbo.tbInstallationMaster.iTag WHEN 0 THEN 'Pending' WHEN 1 THEN 'Done' WHEN 2 THEN 'Cancel' END AS InsTag";
        
        sSql = sSql + " FROM  dbo.tbInstallationMaster INNER JOIN";
        sSql = sSql + " dbo.tbInstallationDetails ON dbo.tbInstallationMaster.IAID = dbo.tbInstallationDetails.IAID INNER JOIN";
        sSql = sSql + " dbo.Entity ON dbo.tbInstallationMaster.EID = dbo.Entity.EID INNER JOIN";
        sSql = sSql + " dbo.Product ON dbo.tbInstallationDetails.ProductID = dbo.Product.ProductID INNER JOIN";
        sSql = sSql + " dbo.tbVendorInfo ON dbo.tbInstallationMaster.VAID = dbo.tbVendorInfo.VAID";
        
        sSql = sSql + " WHERE  (dbo.tbInstallationMaster.EID > 0) ";
        sSql = sSql + " AND (dbo.tbInstallationMaster.InstDateAprx >= '" + Convert.ToDateTime(this.txtFrom.Text) + "'";
        sSql = sSql + " AND dbo.tbInstallationMaster.InstDateAprx <= '" + Convert.ToDateTime(this.txtToDate.Text) + "')";

        if (this.ddlEntity.SelectedItem.Text != "ALL")
        {
            sSql = sSql + " AND (dbo.Entity.eName='" + this.ddlEntity.SelectedItem.Text + "')";
        }

        if (this.ddlVendor.SelectedItem.Text != "ALL")
        {
            sSql = sSql + " AND (dbo.tbVendorInfo.VName='" + this.ddlVendor.SelectedItem.Text + "')";
        }

        if (this.ddlCat.SelectedItem.Text != "ALL")
        {
            sSql = sSql + " AND (dbo.Product.GroupName='" + this.ddlCat.SelectedItem.Text + "')";
        }

        if (this.ddlModel.SelectedItem.Text != "ALL")
        {
            sSql = sSql + " AND (dbo.Product.Model='" + this.ddlModel.SelectedItem.Text + "')";
        }

        sSql = sSql + " GROUP BY dbo.tbInstallationMaster.IAID, dbo.tbInstallationMaster.EID, dbo.Entity.eName, ";
        sSql = sSql + " dbo.tbInstallationMaster.VAID, dbo.tbVendorInfo.VName, dbo.tbVendorInfo.VContact, ";
        sSql = sSql + " dbo.tbInstallationMaster.RefNo, dbo.tbInstallationMaster.InvNo,"; 
        sSql = sSql + " dbo.tbInstallationMaster.RefDate, dbo.tbInstallationMaster.MRNo, dbo.tbInstallationMaster.CustMobile, ";
        sSql = sSql + " dbo.tbInstallationMaster.CustName, dbo.tbInstallationMaster.CustAdd, ";
        sSql = sSql + " dbo.tbInstallationMaster.CustDist, dbo.tbInstallationMaster.CustThana, dbo.tbInstallationMaster.InstDateAprx,";
        sSql = sSql + " dbo.tbInstallationMaster.InstDateAprx1, dbo.tbInstallationMaster.InstTimeAprx, ";
        sSql = sSql + " dbo.tbInstallationMaster.InstStatus, dbo.Product.Model, dbo.Product.GroupName, dbo.Product.Size_Capacity,";
        sSql = sSql + " dbo.Product.Size_Capacity_Unit, dbo.tbInstallationDetails.tQty, ";
        sSql = sSql + " dbo.tbInstallationDetails.ProdRemarks, dbo.tbInstallationMaster.SpecNote, dbo.tbInstallationMaster.Remarks,";
        sSql = sSql + "  dbo.tbInstallationMaster.iTag";

        sSql = sSql + " ORDER BY dbo.tbInstallationMaster.RefNo, dbo.tbInstallationMaster.RefDate";


        SqlCommand cmd = new SqlCommand(sSql, con);        
        //OleDbDataReader dr = cmd.ExecuteReader();

        DataSet ds = new DataSet();
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        da.Fill(ds);

        GridView1.DataSource = ds;
        GridView1.DataBind();
        //dr.Close();
        con.Close();

    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CalcTotalQty(e.Row.Cells[10].Text);
            //CalcTotal_TP(e.Row.Cells[3].Text);

            //CalcTotal_Cash(e.Row.Cells[4].Text);
            //CalcTotal_Card(e.Row.Cells[5].Text);

            //// ALIGNMENT
            //e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Left;

            //e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
            //e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Center;
            //e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Center;

            e.Row.Cells[10].HorizontalAlign = HorizontalAlign.Right;
            

        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[1].Text = "Total";
            e.Row.Cells[10].Text = runningTotalQty.ToString("0,0", CultureInfo.InvariantCulture);
            //e.Row.Cells[3].Text = runningTotalTP.ToString("0,0", CultureInfo.InvariantCulture);
            //e.Row.Cells[4].Text = runningTotalCash.ToString("0,0", CultureInfo.InvariantCulture);
            //e.Row.Cells[5].Text = runningTotalCard.ToString("0,0", CultureInfo.InvariantCulture);


            ////ALIGNMENT
            //e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Left;

            //e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
            //e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[10].HorizontalAlign = HorizontalAlign.Right;
            
        }

    }

    //CALCULATE TOTAL CASH PAY
    private void CalcTotal_Cash(string _price)
    {
        try
        {
            runningTotalCash += Double.Parse(_price);
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write(ex.ToString());
        }
    }

    //CALCULATE TOTAL CARD PAY
    private void CalcTotal_Card(string _price)
    {
        try
        {
            runningTotalCard += Double.Parse(_price);
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write(ex.ToString());
        }
    }


    //CALCULATE TOTAL AMOUNT
    private void CalcTotal_TP(string _price)
    {
        try
        {
            runningTotalTP += Double.Parse(_price);
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write(ex.ToString());
        }
    }

    //CALCULATE TOTAL QTY
    private void CalcTotalQty(string _qty)
    {
        try
        {
            runningTotalQty += Double.Parse(_qty);
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write(ex.ToString());
        }
    }



    protected void OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        GridView1.DataBind();
        fnLoadData();
    }

    //EXPORT TO EXCEL
    protected void ExportToExcel(object sender, EventArgs e)
    {
        try
        {
            Response.Clear();
            Response.Buffer = true;
            //Response.AddHeader("content-disposition", "attachment;filename=GridViewExport.xls");
            //string filename = "Slow_Items_List_" + Session["UserName"].ToString() + "_on_" + DateTime.Now.ToString() + ".xls";
            string filename = "Search_Ins_on_" + DateTime.Now.ToString() + ".xls";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);

            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                //To Export all pages
                GridView1.AllowPaging = false;

                //FUNCTION FOR LOAD DATA                
                fnLoadData();
                

                GridView1.HeaderRow.BackColor = Color.White;
                foreach (TableCell cell in GridView1.HeaderRow.Cells)
                {
                    cell.BackColor = GridView1.HeaderStyle.BackColor;
                }
                foreach (GridViewRow row in GridView1.Rows)
                {
                    row.BackColor = Color.White;
                    foreach (TableCell cell in row.Cells)
                    {
                        if (row.RowIndex % 2 == 0)
                        {
                            cell.BackColor = GridView1.AlternatingRowStyle.BackColor;
                        }
                        else
                        {
                            cell.BackColor = GridView1.RowStyle.BackColor;
                        }
                        cell.CssClass = "textmode";
                    }
                }

                GridView1.RenderControl(hw);

                //style to format numbers to string
                string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                Response.Write(style);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
        }
        catch (Exception ex)
        {
            //
        }

    }



}