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

public partial class shipment : System.Web.UI.Page
{

    SqlConnection conn = DBConnection.GetConnection();
    SqlConnection conn1 = DBConnection.GetConnection();
    SqlConnection _connStr = DBConnection.GetConnection();
    long iMRSRID = 0;
    DataTable dt;
    DateTime tDate;
    DateTime tSDate;

    private double runningTotal = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        //btnAdd.Attributes.Add("OnClick", "return confirm_Add();");
        btnSave.Attributes.Add("OnClick", "return confirm_Save();");
        btnCancel.Attributes.Add("OnClick", "return confirm_Cancel();");
        //ibtnDelete.Attributes.Add("OnClick", "return confirm_delete();");
        if (!IsPostBack)
        {
            if (System.Convert.ToInt32(Session["Vis"]) == 0)
            {
                Response.Redirect("../Account/Login.aspx");
            }

                       
            //LOAD CTP
            LoadDropDownList_CTP();

            //LOAD AUTO CHALLAN NUMBER
            //fnLoadAutoBillNo();

            txtDate.Text = DateTime.Today.ToString("MM/dd/yyyy");
            txtShipDate.Text = DateTime.Today.ToString("MM/dd/yyyy");

        }
        
        //txtDate.Text = String.Format("{0:t}", Now);       


    }

    

    //LOAD CTP IN DROPDOWN LIST
    protected void LoadDropDownList_CTP()
    {
        SqlConnection conn = DBConnection.GetConnection();

        String strQuery = "select EID,eName from Entity ";
        strQuery = strQuery + " WHERE (ActiveDeactive = 1) AND";
        strQuery = strQuery + " (EntityType = 'showroom' OR  EntityType = 'zone' OR  EntityType = 'Dealer')";
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
            ddlEntity.Items.Insert(0, new ListItem("CI&DD (REL)", "370"));

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


    

    protected void PopupMessage(string Msg, Control controlID)
    {
        ScriptManager.RegisterClientScriptBlock(controlID, controlID.GetType(), "msg", "alert('" + Msg + "');", true);

    }

    
    //CLEAR ALL TEXT AND GRID
    protected void btnCancel_Click(object sender, EventArgs e)
    {

        //CLEAR ALL TEXT
        txtCHNo.Text = "";
        txtDate.Text = "";
        txtDate.Text = DateTime.Today.ToString("dd/MM/yyyy");

        txtProdID.Text = "";
                
        //LOAD AUTO CHALLAN NUMBER
        fnLoadAutoBillNo();

        txtCHNo.Focus();

    }




    //FINALLY SAVE DATA
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (System.Convert.ToInt32(Session["Vis"]) == 0)
        {
            Response.Redirect("../Account/Login.aspx");
        }

        SqlConnection conn = DBConnection.GetConnection();
        //fnLoadMRSRNo();

        string sSql = "";

        //CHALLAN VALIDATION        
        if (txtCHNo.Text == "")
        {
            PopupMessage("Please enter L/C Number.", btnSave);
            txtCHNo.Focus();
            return;
        }

        //if (ddlEntity.SelectedItem.Text == "")
        //{
        //    PopupMessage("Please Select Entity Name.", btnSave);
        //    //txtDate.Focus();
        //    return;
        //}

        //CHALLAN DATE VALIDATION        
        if (txtDate.Text == "")
        {
            PopupMessage("Please enter L/C Date.", btnSave);
            txtDate.Focus();
            return;
        }

        tDate = Convert.ToDateTime(this.txtDate.Text);
        tSDate = Convert.ToDateTime(this.txtShipDate.Text);

        //LOAD AUTO CHALLAN NUMBER
        //fnLoadAutoBillNo();

        //----------------------------------------------------------------------
        //CHECK DUPLICATE CHALLAN NO.
        sSql = "";
        sSql = "SELECT MRSRMID FROM MRSRMaster_TM" +
            " WHERE MRSRCode='" + this.txtCHNo.Text + "'" +
            " AND TrType=1";
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
        sSql = "INSERT INTO MRSRMaster_TM(MRSRCode,LCNO,TDate,LCDate," +
                " TrType,InvoiceNo,ShipFrom,ShipDate,UserID,EntryDate" +
                " )" +
                " Values ('" + this.txtCHNo.Text + "','" + this.txtCHNo.Text + "'," +
                " '" + tDate + "','" + tDate + "'," +
                " '1','" + this.txtCHNo.Text + "'," +
                " '" + txtShipFrom.Text + "','" + tSDate + "'," +
                " '" + Session["UserName"] + "','" + DateTime.Today + "'" +
                " )";
        SqlCommand cmd = new SqlCommand(sSql, conn);
        conn.Open();
        cmd.ExecuteNonQuery();
        conn.Close();


        //RETRIVE MASTER ID         
        sSql = "";
        sSql = "SELECT MRSRMID FROM MRSRMaster_TM" +
            " WHERE MRSRCode='" + this.txtCHNo.Text + "'" +
            " AND TrType=1";
        cmd = new SqlCommand(sSql, conn);
        conn.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            iMRSRID = Convert.ToInt32(dr["MRSRMID"].ToString());
        }
        dr.Dispose();
        dr.Close();
        conn.Close();

               

        //------------------------------------------------------------------------------------------

        //lblSaveMessage.Text = "Save Data Successfully.";

        ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert",
                        "<script>alert('" + "Save Successfully." + "');</script>", false);

        //------------------------------------------------------------------------------------------
        //CLEAR ALL TEXT
        txtCHNo.Text = "";
        txtDate.Text = "";
        txtDate.Text = DateTime.Today.ToString("dd/MM/yyyy");

        txtProdID.Text = "";
        
        
        //LOAD AUTO CHALLAN NUMBER
        //fnLoadAutoBillNo();
                
        //------------------------------------------------------------------------------------------

        return;

    }


    protected void fnLoadAutoBillNo()
    {

        SqlConnection con = DBConnection.GetConnection();
        //con.Open();

        int xMax = 0;
        string sAutoNo = "";
        string sSql = "";
        sSql = "SELECT ISNULL(MAX(RIGHT(MRSRCode, 4)), 0) AS MRSRCode" +
            " FROM dbo.MRSRMaster_TM WHERE TrType=1" +
            " AND (LEFT(MRSRCode, 6) = '" + "1" + "" + DateTime.Now.ToString("yyyy") + "-" + "')";
        //" AND TrType=3";

        SqlCommand cmd = new SqlCommand(sSql, con);
        con.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        try
        {
            if (dr.Read())
            {
                //xMax = dr["JobNo"].ToString();
                xMax = Convert.ToInt32(dr["MRSRCode"]) + 1;
                sAutoNo = "1" + "" + DateTime.Now.ToString("yyyy") + "-" + xMax.ToString("0000");
                txtCHNo.Text = sAutoNo;
            }
            else
            {
                xMax = Convert.ToInt32(dr["MRSRCode"]) + 1;
                sAutoNo = "1" + "" + DateTime.Now.ToString("yyyy") + "-" + xMax.ToString("0000");
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


    


}