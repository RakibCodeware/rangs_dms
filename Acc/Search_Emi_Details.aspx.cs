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

public partial class Search_Emi_Details : System.Web.UI.Page
{
    SqlConnection conn = DBConnection.GetConnection();
    long i;

    private double runningTotalQty = 0;
    private double runningTotalTP = 0;
    private double runningTotalCash = 0;
    private double runningTotalCard = 0;
    private double runningTotalCheque = 0;
    private double runningTotalReq = 0;

    DataTable dt;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (System.Convert.ToInt32(Session["Vis"]) == 0)
        {
            Response.Redirect("../Account/Login.aspx");
        }
        int role = Convert.ToInt32(Session["RolesId"]);

        //if (role != 1)
        //{
        //    Response.Redirect("~/Login.aspx");
        //}
        if (!IsPostBack)
        {
            dt = new DataTable();
            MakeTable();
        }
        else
        {
            dt = new DataTable();
            MakeTable();
        }
        ViewState["dtEmiInfo"] = dt;
    }
    protected void MakeTable()
    {

        dt.Columns.Add("Model");
        dt.Columns.Add("PurchaseMode");
        dt.Columns.Add("MRP");
        dt.Columns.Add("SellingPrice");
        dt.Columns.Add("Discount");
        dt.Columns.Add("DiscountPercent");
        dt.Columns.Add("MonthlyEMI");

    }
    protected void SearchData(object sender, EventArgs e)
    {

        //if (txtModel.Text == "")
        //{
        //    PopupMessage("Please select product Model.", btnSearch);
        //    txtModel.Focus();
        //    return;
        //}

        //lblModel.Text = "Model : " + txtModel.Text; 

        //LOAD STATEMENT DATA
        //fnLoadStatementData();

        //LOAD DATA IN GRID
        //fnLoadData();


        AddRows();
        GridView1.DataSource = dt;
        GridView1.DataBind();
    }

    private double[] txtProductInfo(string model)
    {
        SqlConnection conn = DBConnection.GetConnection();
        double UP = 0;
        double CampPrice = 0;
        string sSql = "";

        sSql = "";
        sSql = "SELECT ProductID,ProdName,UnitPrice,Model";
        sSql = sSql + " FROM Product";
        sSql = sSql + " WHERE Model='" + model + "'";

        SqlCommand cmd = new SqlCommand(sSql, conn);
        conn.Open();
        SqlDataReader dr = cmd.ExecuteReader();

        try
        {
            if (dr.Read())
            {

                UP = Convert.ToDouble(dr["UnitPrice"].ToString());

            }
            else
            {
                UP = 0;
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


        //LOAD CAMPAIGN PRICE
        sSql = "";
        sSql = "SELECT TOP 1 ProductID,Model,DisAmnt " +
            " FROM VW_CampaignInfo" +
            " WHERE Model='" + txtModel.Text + "'" +
            " AND (EffectiveDate<='" + DateTime.Today + "' AND cStop=0)" +
            " ORDER BY EffectiveDate DESC";
        cmd = new SqlCommand(sSql, conn);
        conn.Open();
        dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            CampPrice = UP - Convert.ToDouble(dr["DisAmnt"].ToString());
        }
        else
        {
            CampPrice = UP;
        }
        dr.Dispose();
        dr.Close();
        conn.Close();


        double[] d = new double[2];
        d[0] = UP;
        d[1] = CampPrice;

        return d;
    }
    protected void AddRows()
    {
        if (System.Convert.ToInt32(Session["Vis"]) == 0)
        {
            Response.Redirect("../Account/Login.aspx");
        }

        if (txtModel.Text == "")
        {
            PopupMessage("Please select product Model.", btnSearch);
            txtModel.Focus();
            return;
        }




        double[] d = txtProductInfo(txtModel.Text);


        for (int i = 1; i <= 7; i++)
        {
            if (i == 1)
            {
                double disPer = 0;

                if (d[0] == d[1])
                {
                    disPer = 0;
                }
                else
                {
                    disPer = ((d[0] - d[1]) / d[0]) * 100;
                }

                DataRow dr = dt.NewRow();
                dr["Model"] = txtModel.Text;
                dr["PurchaseMode"] = "Cash";
                dr["MRP"] = d[0].ToString();
                dr["SellingPrice"] = d[1].ToString();
                dr["Discount"] = (d[0] - d[1]).ToString();
                dr["DiscountPercent"] = disPer.ToString();
                dr["MonthlyEMI"] = "N/A";
                dt.Rows.Add(dr);
            }
            else if (i == 2)
            {   //for 6month emi
                double disPer = 0;
                if (d[0] == d[1])
                {
                    disPer = 0;
                }
                else
                {
                    disPer = ((d[0] - d[1]) / d[0]) * 100;
                }
                double cp = d[0] - d[0] * (disPer - 0) / 100;

                DataRow dr = dt.NewRow();
                dr["Model"] = txtModel.Text;
                dr["PurchaseMode"] = "6 Month EMI";
                dr["MRP"] = d[0].ToString();
                dr["SellingPrice"] = cp.ToString();
                dr["Discount"] = (d[0] - cp).ToString();
                dr["DiscountPercent"] = disPer.ToString();
                dr["MonthlyEMI"] = Math.Round((cp / 6), 2).ToString();
                dt.Rows.Add(dr);
            }
            else if (i == 3)
            {   //for 9month emi
                double disPer = 0;
                if (d[0] == d[1])
                {
                    disPer = 0;
                }
                else
                {
                    disPer = ((d[0] - d[1]) / d[0]) * 100;
                }
                double cp = d[0] - d[0] * (disPer - 0) / 100;

                DataRow dr = dt.NewRow();
                dr["Model"] = txtModel.Text;
                dr["PurchaseMode"] = "9 Month EMI";
                dr["MRP"] = d[0].ToString();
                dr["SellingPrice"] = cp.ToString();
                dr["Discount"] = (d[0] - cp).ToString();
                dr["DiscountPercent"] = disPer.ToString();
                dr["MonthlyEMI"] = Math.Round((cp / 9), 2).ToString();
                dt.Rows.Add(dr);
            }
            else if (i == 4)
            {   //for 12month emi
                double disPer = 0;
                if (d[0] == d[1])
                {
                    disPer = 0;
                }
                else
                {
                    disPer = ((d[0] - d[1]) / d[0]) * 100;
                }
                double cp = d[0] - d[0] * (disPer - 0) / 100;

                DataRow dr = dt.NewRow();
                dr["Model"] = txtModel.Text;
                dr["PurchaseMode"] = "12 Month EMI";
                dr["MRP"] = d[0].ToString();
                dr["SellingPrice"] = cp.ToString();
                dr["Discount"] = (d[0] - cp).ToString();
                dr["DiscountPercent"] = disPer.ToString();
                dr["MonthlyEMI"] = Math.Round((cp / 12), 2).ToString();
                dt.Rows.Add(dr);
            }
            else if (i == 5)
            {   //for 18month emi
                double disPer = 0;
                double cp = 0;
                double emiChargePer = 0;
                if (d[0] == d[1])
                {
                    disPer = 0;
                    cp = d[0] - d[0] * (disPer - 0) / 100;
                }
                else
                {
                    disPer = ((d[0] - d[1]) / d[0]) * 100;
                    emiChargePer = 4;
                    cp = d[0] - d[0] * (disPer - emiChargePer) / 100;
                }


                DataRow dr = dt.NewRow();
                dr["Model"] = txtModel.Text;
                dr["PurchaseMode"] = "18 Month EMI";
                dr["MRP"] = d[0].ToString();
                dr["SellingPrice"] = cp.ToString();
                dr["Discount"] = (d[0] - cp).ToString();
                dr["DiscountPercent"] = disPer - emiChargePer;
                dr["MonthlyEMI"] = Math.Round((cp / 18), 2).ToString();
                dt.Rows.Add(dr);
            }
            else if (i == 6)
            {
                //for 24month emi
                double disPer = 0;
                double cp = 0;
                double emiChargePer = 0;
                if (d[0] == d[1])
                {
                    disPer = 0;
                    cp = d[0] - d[0] * (disPer - 0) / 100;
                }
                else
                {
                    disPer = ((d[0] - d[1]) / d[0]) * 100;
                    emiChargePer = 6;
                    cp = d[0] - d[0] * (disPer - emiChargePer) / 100;
                }


                DataRow dr = dt.NewRow();
                dr["Model"] = txtModel.Text;
                dr["PurchaseMode"] = "24 Month EMI";
                dr["MRP"] = d[0].ToString();
                dr["SellingPrice"] = cp.ToString();
                dr["Discount"] = (d[0] - cp).ToString();
                dr["DiscountPercent"] = disPer - emiChargePer;
                dr["MonthlyEMI"] = Math.Round((cp / 24), 2).ToString();
                dt.Rows.Add(dr);
            }
            else if (i == 7)
            {
                //for 36month emi
                double disPer = 0;
                double cp = 0;
                double emiChargePer = 0;
                if (d[0] == d[1])
                {
                    disPer = 0;
                    cp = d[0] - d[0] * (disPer - 0) / 100;
                }
                else
                {
                    disPer = ((d[0] - d[1]) / d[0]) * 100;
                    emiChargePer = 10;
                    cp = d[0] - d[0] * (disPer - emiChargePer) / 100;
                }


                DataRow dr = dt.NewRow();
                dr["Model"] = txtModel.Text;
                dr["PurchaseMode"] = "36 Month EMI";
                dr["MRP"] = d[0].ToString();
                dr["SellingPrice"] = cp.ToString();
                dr["Discount"] = (d[0] - cp).ToString();
                dr["DiscountPercent"] = disPer - emiChargePer;
                dr["MonthlyEMI"] = Math.Round((cp / 36), 2).ToString();
                dt.Rows.Add(dr);
            }
        }

    }



    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    //CalcTotalQty(e.Row.Cells[4].Text);


        //    //CalcTotal_Cash(e.Row.Cells[4].Text);
        //    //CalcTotal_Card(e.Row.Cells[3].Text);
        //    //CalcTotal_Cheque(e.Row.Cells[4].Text);
        //    //CalcTotal_Req(e.Row.Cells[5].Text);

        //    //CalcTotal_TP(e.Row.Cells[6].Text);

        //    // ALIGNMENT
        //    e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Left;

        //    e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Center;
        //    //e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
        //    //e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Center;
        //    //e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Center;

        //    //e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Center;


        //}
        //else if (e.Row.RowType == DataControlRowType.Footer)
        //{
        //    e.Row.Cells[1].Text = "Total";
        //    //e.Row.Cells[4].Text = runningTotalQty.ToString("0,0", CultureInfo.InvariantCulture);

        //    e.Row.Cells[4].Text = runningTotalCash.ToString("0,0", CultureInfo.InvariantCulture);
        //    //e.Row.Cells[3].Text = runningTotalCard.ToString("0,0", CultureInfo.InvariantCulture);
        //    //e.Row.Cells[4].Text = runningTotalCheque.ToString("0,0", CultureInfo.InvariantCulture);
        //    //e.Row.Cells[5].Text = runningTotalReq.ToString("0,0", CultureInfo.InvariantCulture);

        //    //e.Row.Cells[6].Text = runningTotalTP.ToString("0,0", CultureInfo.InvariantCulture);

        //    //ALIGNMENT
        //    e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Left;

        //    e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Center;
        //    //e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
        //    //e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Center;
        //    //e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Center;

        //    //e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Center;

        //}

    }


    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GetModel(string prefixText)
    {
        DataTable dt = new DataTable();
        SqlConnection con = DBConnection.GetConnection();
        con.Open();
        SqlCommand cmd = new SqlCommand("Select TOP 10 * from Product where Discontinue='No' AND Model like @model+'%'", con);
        cmd.Parameters.AddWithValue("@model", prefixText);
        SqlDataAdapter adp = new SqlDataAdapter(cmd);
        adp.Fill(dt);
        List<string> ModelNames = new List<string>();
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            ModelNames.Add(dt.Rows[i][5].ToString());
        }
        return ModelNames;
    }

    protected void PopupMessage(string Msg, Control controlID)
    {
        ScriptManager.RegisterClientScriptBlock(controlID, controlID.GetType(), "msg", "alert('" + Msg + "');", true);
    }
}