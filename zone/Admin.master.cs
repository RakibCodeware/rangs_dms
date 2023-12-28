using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web.SessionState;

public partial class Admin : System.Web.UI.MasterPage
{
    
    public string labeltext1
    {
        get
        {
            return lblTItem.Text;
        }
        set
        {
            lblTItem.Text = value;
        }
    }



    protected void Page_Load(object sender, EventArgs e)
    {
        if (System.Convert.ToInt32(Session["Vis"]) == 0)
        {
            Response.Redirect("~/Default.aspx");
        }

        if (!IsPostBack)
        {
            string[] path = HttpContext.Current.Request.Url.AbsolutePath.Split('/');
            string page = path[path.Length - 1].ToString();
            string[] pages = { "issue_delivery.aspx", "sales_delivery.aspx", "Dhaka_sales_delivery.aspx", "File_Uploadn.aspx" };
            if (pages.Contains(page))
                Response.Redirect("~/Zone/Default_Administrator.aspx");


            this.lblUserName.Text = Session["UserName"].ToString();
            //this.lblCTP.Text = Session["eName"].ToString();

            //LOAD CART ITEMS
            fnLoadCartItems();

            //LOAD INBOX CHALLAN COUNT
            fnLoadChallanCount();


            fnLoadSalesInvDelCount();

            //TOTAL DISCOUNT CODE
            fnLoadDisCodeCount();

            //TOTAL PENDING ORDER
            fnLoadOnlineOrder();


            //DECLARATION PENDING LIST
            fnLoadSales_declarationPending();

            //LOAD REQUIREMENT PENDING LIST
            fnLoadRequirementPendingList();


            SqlConnection conn = DBConnection.GetConnection();

            //LOAD STOCK MENU            
            //string gSql = "";
            //gSql = "SELECT CategoryID, CatName, ShowTag, parent";
            //gSql = gSql + " FROM tbCategory WHERE parent=0";
            //gSql = gSql + " ORDER BY order_text";

            //SqlCommand cmd = new SqlCommand(gSql, conn);
            //conn.Open();
            //SqlDataReader dr = cmd.ExecuteReader();

            Literal1.Text = "";
            //if (dr.Read())
            if (Session["iTagStock"].ToString()=="1")
            {
                Literal1.Text = Literal1.Text + @"";
                //Literal1.Text = Literal1.Text + "<li><a href='menu.aspx?cat_id=" + dr["CategoryID"].ToString() + "' class='active'>" + dr["CatName"].ToString() + "</a></li>";

                Literal1.Text = Literal1.Text + "<li>";
                Literal1.Text = Literal1.Text + "<a href='Search_Stock.aspx'>My Stock</a> ";
                Literal1.Text = Literal1.Text + "</li>";

                Literal1.Text = Literal1.Text + "<li>";
                Literal1.Text = Literal1.Text + "<a href='Search_DP.aspx'>Countrywide Stock</a>";
                Literal1.Text = Literal1.Text + "</li>";

                //STOCk
                ltlStockReport.Text = ltlStockReport.Text + "<li>";
                ltlStockReport.Text = ltlStockReport.Text + "<a href='frmStockReport_admin.aspx'>Stock Report</a>";
                ltlStockReport.Text = ltlStockReport.Text + "</li>";

            }
            else
            {
                Literal1.Text = "";

            }

            //conn.Close();
            //dr.Close();


            ltlSpin.Text = "";
            //if (dr.Read())
            if (Session["iSpinWin"].ToString() == "1")
            {
                ltlSpin.Text = ltlSpin.Text + @"";
                //Literal1.Text = Literal1.Text + "<li><a href='menu.aspx?cat_id=" + dr["CategoryID"].ToString() + "' class='active'>" + dr["CatName"].ToString() + "</a></li>";

                ltlSpin.Text = ltlSpin.Text + "<li>";
                ltlSpin.Text = ltlSpin.Text + "<a href='customer.aspx'><i class='fa fa-files-o fa-fw'></i> Customer for Toss & Win<span class='fa arrow'></span></a> ";
                ltlSpin.Text = ltlSpin.Text + "</li>";
                                
            }


        }

    }


    protected void fnLoadCartItems()
    {
        String gSql = "";
        SqlConnection conn = DBConnection.GetConnection();

        HttpSessionState ss = HttpContext.Current.Session;
        string sid = ss.SessionID;

        //SHOW IN TOTAL CART IMTEMS
        gSql = "";
      
        gSql="SELECT dbo.MRSRMaster.InSource, SUM(dbo.MRSRDetails.Qty) AS tQty";
        gSql = gSql + " FROM dbo.MRSRMaster INNER JOIN";
        gSql = gSql + " dbo.MRSRDetails ON dbo.MRSRMaster.MRSRMID = dbo.MRSRDetails.MRSRMID";
        gSql = gSql + " WHERE (dbo.MRSRMaster.TrType = 2) AND (dbo.MRSRMaster.Tag = 2)";
        gSql = gSql + " GROUP BY dbo.MRSRMaster.InSource";
        gSql = gSql + " HAVING (dbo.MRSRMaster.InSource = '" + Session["sBrId"] + "')";
        
        SqlCommand cmd = new SqlCommand(gSql, conn);
        conn.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            lblTItem.Text = "My Cart (" + dr["tQty"].ToString() + ")";
        }
        else
        {
            lblTItem.Text = "My Cart (0)";
        }
        dr.Dispose();
        dr.Close();
        conn.Close();

    }

    protected void fnLoadChallanCount()
    {
        String gSql = "";
        SqlConnection conn = DBConnection.GetConnection();

        HttpSessionState ss = HttpContext.Current.Session;
        string sid = ss.SessionID;

        //SHOW IN TOTAL CART IMTEMS
        gSql = "";

        gSql = "SELECT COUNT(MRSRCode) AS tCH";
        gSql = gSql + " FROM dbo.MRSRMaster";        
        gSql = gSql + " WHERE (dbo.MRSRMaster.TrType = 2) AND (dbo.MRSRMaster.Tag = 2)";        
        gSql = gSql + " AND (dbo.MRSRMaster.InSource = '" + Session["sBrId"] + "')";

        SqlCommand cmd = new SqlCommand(gSql, conn);
        conn.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            lblInbox.Text = "(" + dr["tCH"].ToString() + ")";
        }
        else
        {
            lblInbox.Text = "(0)";
        }
        dr.Dispose();
        dr.Close();
        conn.Close();

    }

    protected void fnLoadSalesInvDelCount()
    {
        String gSql = "";
        SqlConnection conn = DBConnection.GetConnection();

        HttpSessionState ss = HttpContext.Current.Session;
        string sid = ss.SessionID;

        //SHOW IN TOTAL CART IMTEMS
        gSql = "";

        gSql = "SELECT COUNT(MRSRCode) AS tCH";
        gSql = gSql + " FROM dbo.MRSRMaster";
        gSql = gSql + " WHERE (dbo.MRSRMaster.TrType = 3) AND (dbo.MRSRMaster.Tag = 2)";
        gSql = gSql + " AND (dbo.MRSRMaster.DeliveryFrom = '" + Session["sBrId"] + "')";

        SqlCommand cmd = new SqlCommand(gSql, conn);
        conn.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            lblSalesDel.Text = "(" + dr["tCH"].ToString() + ")";
        }
        else
        {
            lblSalesDel.Text = "(0)";
        }
        dr.Dispose();
        dr.Close();
        conn.Close();

    }


    protected void fnLoadSales_declarationPending()
    {
        String gSql = "";
        SqlConnection conn = DBConnectionDSM.GetConnection();

        HttpSessionState ss = HttpContext.Current.Session;
        string sid = ss.SessionID;

        //SHOW IN TOTAL CART IMTEMS
        gSql = "";

        //gSql = "SELECT COUNT(MRSRCode) AS tCH";
        //gSql = gSql + " FROM dbo.MRSRMaster";
        //gSql = gSql + " WHERE (dbo.MRSRMaster.TrType = 3) AND (dbo.MRSRMaster.Tag = 2)";
        //gSql = gSql + " AND (dbo.MRSRMaster.DeliveryFrom = '" + Session["sBrId"] + "')";


        gSql = "";
        gSql = "SELECT COUNT(dbo.MRSRMaster.MRSRCode) AS tCH ";

        gSql = gSql + " FROM dbo.VW_Delear_Info INNER JOIN";
        gSql = gSql + " dbo.MRSRMaster ON dbo.VW_Delear_Info.DAID = dbo.MRSRMaster.InSource INNER JOIN";
        gSql = gSql + " dbo.Zone ON dbo.MRSRMaster.OutSource = dbo.Zone.CategoryID";

        gSql = gSql + " WHERE (dbo.MRSRMaster.TrType = 3) AND (dbo.Zone.Code = '" + Session["sBrCode"].ToString() + "') ";
        gSql = gSql + " AND (dbo.MRSRMaster.SaleDeclar = 0)";
                
        SqlCommand cmd = new SqlCommand(gSql, conn);
        conn.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            lblDeclarationPending.Text = "(" + dr["tCH"].ToString() + ")";
        }
        else
        {
            lblDeclarationPending.Text = "(0)";
        }
        dr.Dispose();
        dr.Close();
        conn.Close();

    }


    protected void fnLoadDisCodeCount()
    {
        String gSql = "";
        SqlConnection conn = DBConnection.GetConnection();

        HttpSessionState ss = HttpContext.Current.Session;
        string sid = ss.SessionID;

        //SHOW IN TOTAL CART IMTEMS
        gSql = "";

        gSql = "SELECT COUNT(DisCode) AS tCode";
        gSql = gSql + " FROM dbo.tbKeyGen";
        gSql = gSql + " WHERE (dbo.tbKeyGen.dTag = 0)";
        gSql = gSql + " AND (dbo.tbKeyGen.EID = '" + Session["sBrId"] + "')";

        SqlCommand cmd = new SqlCommand(gSql, conn);
        conn.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            //lblTDisCode.Text = "(" + dr["tCode"].ToString() + ")";
        }
        else
        {
            //lblTDisCode.Text = "(0)";
        }
        dr.Dispose();
        dr.Close();
        conn.Close();

    }

    protected void fnLoadOnlineOrder()
    {
        String gSql = "";
        SqlConnection conn = DBConnection_ROS.GetConnection();

        //SHOW IN TOTAL CART IMTEMS
        gSql = "";

        gSql = "SELECT COUNT(DelID) AS tCode";
        gSql = gSql + " FROM dbo.tbCustomerDelivery";
        gSql = gSql + " WHERE (dStatus = 0)";
        gSql = gSql + " AND (EID = '" + Session["sBrId"] + "')";

        SqlCommand cmd = new SqlCommand(gSql, conn);
        conn.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            lblTOrder.Text = "(" + dr["tCode"].ToString() + ")";
        }
        else
        {
            lblTOrder.Text = "(0)";
        }
        dr.Dispose();
        dr.Close();
        conn.Close();

    }


    protected void fnLoadRequirementPendingList()
    {
        String gSql = "";
        SqlConnection conn = DBConnection.GetConnection();

        //SHOW IN TOTAL REQUIREMNET LIST
        gSql = "";

        //gSql = "SELECT COUNT(ReqAID) AS tCode";
        //gSql = gSql + " FROM dbo.RequirmentMaster";
        //gSql = gSql + " WHERE (dTag = 0)";
        //gSql = gSql + " AND (EID = '" + Session["sBrId"] + "')";

        gSql ="SELECT dbo.Entity.EID, dbo.Entity.eName AS SubStoreName,";
        gSql = gSql + " COUNT(dbo.RequirmentMaster.ReqNo) AS tReqNo";
        gSql = gSql + " FROM  dbo.Entity INNER JOIN";
        gSql = gSql + " dbo.Entity AS Entity_1 ON dbo.Entity.ZoneID = Entity_1.EID INNER JOIN";
        gSql = gSql + " dbo.Entity AS Entity_2 ON Entity_1.EID = Entity_2.ParentEntity INNER JOIN";
        gSql = gSql + " dbo.RequirmentMaster ON Entity_2.EID = dbo.RequirmentMaster.EID";
        gSql = gSql + " WHERE (dbo.Entity.EntityType = 'Sub Store') AND (dbo.RequirmentMaster.Delivered = 0)";
        gSql = gSql + " AND (dbo.Entity.EID = '" + Session["sBrId"] + "')";
        gSql = gSql + " GROUP BY dbo.Entity.EID, dbo.Entity.eName";
        //gSql = gSql + " HAVING      (dbo.Entity.EID = 370)";
        
        SqlCommand cmd = new SqlCommand(gSql, conn);
        conn.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            lblTReq.Text = "(" + dr["tReqNo"].ToString() + ")";
        }
        else
        {
            lblTReq.Text = "(0)";
        }
        dr.Dispose();
        dr.Close();
        conn.Close();

    }

}
