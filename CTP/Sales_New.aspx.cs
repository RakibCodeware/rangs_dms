using System;
using System.Activities.Expressions;
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
using System.Data.SqlTypes;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.Web.Services;
//using System.Collections.Generic;
//using System.Linq;


public partial class Forms_Sales_New : System.Web.UI.Page
{
    SqlConnection conn = DBConnection.GetConnection();
    SqlConnection conn1 = DBConnection.GetConnection();
    SqlConnection _connStr = DBConnection.GetConnection();
    int iMRSRID = 0;
    int iDelTag = 0;
    int newShopOrderStatus = 0;
    DataTable dt;
    DateTime tDate; 
    DateTime tChequeDate;

    private double runningTotal = 0;
    private double runningTotalTP = 0;
    private double runningTotalDis = 0;
    private double runningTotalWith = 0;
    private double runningTotalQty = 0;


    string _mailFrom = "dms@rangs.com.bd";
    string _mailSmtpClient = "mail.rangs.com.bd";
    string _mailUserName = "dms@rangs.com.bd";
    string _mailPassword = "ExamP@ss#321";

    string _mailErrorTo = "nayem.codeware@gmail.com";


    #region[Page Load event]
    protected void Page_Load(object sender, EventArgs e)
    {
        if (System.Convert.ToInt32(Session["Vis"]) == 0)
        {
            Response.Redirect("../Account/Login.aspx");
        }

        btnAdd.Attributes.Add("OnClick", "return confirm_Add();");
        btnSave.Attributes.Add("OnClick", "return confirm_Save();");
        btnCancel.Attributes.Add("OnClick", "return confirm_Cancel();");
        //ibtnDelete.Attributes.Add("OnClick", "return confirm_delete();");
        if (!IsPostBack)
        {
            dt = new DataTable();
            MakeTable();

            //LOAD PRODUCT MODEL
            LoadDropDownList();

            //LOAD CITY
            LoadDropDownList_City();

            // Load Ref.Discount
            LoadDiscountReferenceList();
            LoadReferenceList();

            //LOAD CTP
            LoadDropDownList_CTP();
            ddlEntity.SelectedItem.Text = Session["eName"].ToString();
            ddlEntity.SelectedItem.Value = Session["EID"].ToString();

            //LoadDropDownList_Model();

            //LOAD AUTO BILL NO.
            fnLoadAutoBillNo();

            //LOAD T & C
            fnLoadTC();

            //LOAD TERMS & CONDITIONS
            this.fnClaimList();


            //txtDate.Text = String.Format("{0:t}", Now);       
            txtDate.Text = DateTime.Today.ToString("MM/dd/yyyy");
            lblDelivary_charge.Visible = false;
            chkTblOffer.Visible = false;

        }
        else
        {
            dt = (DataTable)ViewState["dt"];
        }
        ViewState["dt"] = dt;


    }
    #endregion

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
            //if (dt.Rows[i][5].ToString() != "KSV-18BDINV (1.5 ton Inverter)")
            //{
            ModelNames.Add(dt.Rows[i][5].ToString());//blocking KSV-18BDINV (1.5 ton Inverter)
            //}
        }
        return ModelNames;
    }
    [WebMethod]
    public static Data.MainData Newshopapi(string orderNo, int t)
    {

        string myParameters = "";
        string order_no = orderNo;
        string customer_contact = "";
        int type = t;

        try
        {


            string apitoken = "rangs123658975558745hkeaffHldeD";

            ServicePointManager.Expect100Continue = false;
            myParameters = @"key=" + apitoken + "&order_number=" + order_no + "&customer_contact=" + customer_contact + "&type=" + type;
            //saveLogs(myParameters, true);
            byte[] datatosend = Encoding.ASCII.GetBytes(myParameters);
            //HttpWebRequest httpreq = (HttpWebRequest)WebRequest.Create("http://36.255.70.152/status-delivered/");
            //HttpWebRequest httpreq = (HttpWebRequest)WebRequest.Create("http://36.255.70.152/status-delivered?key=rangs%26%26Raysul@3456@)9$^)*%26&order_number=30082022000001&type=0");

            string link = "https://ecom.rangs.com.bd/status-delivered?key=" + apitoken + "&order_number=" + order_no + "&type=" + type;
            if (type == 1)
                LogFile(order_no, "", "set request before call api", link, "", true,false,false);
            
            HttpWebRequest httpreq = (HttpWebRequest)WebRequest.Create(link);
            httpreq.Method = "post";
            httpreq.ContentType = "application/json";
            //httpreq.ContentLength = datatosend.Length;
            Stream st = httpreq.GetRequestStream();
            st.Write(datatosend, 0, datatosend.Length);
            st.Close();


            HttpWebResponse myresponse = (HttpWebResponse)httpreq.GetResponse();



            StreamReader myreader = new StreamReader(myresponse.GetResponseStream());
            JavaScriptSerializer js = new JavaScriptSerializer();


            dynamic objText = myreader.ReadToEnd();
            //MainData md = new MainData();
            //md = js.Deserialize<MainData>(myreader.ReadToEnd());

            //   var items = JsonConvert.DeserializeObject<Data.MainData>(myreader.ReadToEnd());

            var items = JsonConvert.DeserializeObject<Data.MainData>(objText);

            string response = objText.ToString();
            if (type == 1)
                LogFile(order_no, "", "set response", link, response, true,false,false);
            

            // Console.WriteLine(items);

            //JsonConvert.DeserializeObject<Movie>(json);
            //MainData m = myresponse.con;


            //Console.WriteLine(myreader.ReadToEnd());

            //foreach (var item in items.data.products)
            //{
            //    Console.WriteLine(item.product_model);
            //}

            return items;


        }


        catch (Exception e)
        {

            LogFile(order_no, "", "Error in Api:" + e.Message.ToString(), "", "", false,false,false);
            // PopupMessage("Does not hit api properly please deliverd it manually in online store",);
            //Console.WriteLine(count);
            Console.WriteLine("Error: " + e.Message);
            return null;
        }
    }

    [WebMethod]
    public static Data.MainDataFinal _Newshopapi(string orderNo, int t)
    {

        string myParameters = "";
        string order_no = orderNo;
        string customer_contact = "";
        int type = t;
       
        try
        {
           

            string apitoken = "rangs123658975558745hkeaffHldeD";

            ServicePointManager.Expect100Continue = false;
            myParameters = @"key=" + apitoken + "&order_number=" + order_no + "&customer_contact=" + customer_contact + "&type=" + type;
            //saveLogs(myParameters, true);
            byte[] datatosend = Encoding.ASCII.GetBytes(myParameters);
            //HttpWebRequest httpreq = (HttpWebRequest)WebRequest.Create("http://36.255.70.152/status-delivered/");
            //HttpWebRequest httpreq = (HttpWebRequest)WebRequest.Create("http://36.255.70.152/status-delivered?key=rangs%26%26Raysul@3456@)9$^)*%26&order_number=30082022000001&type=0");
            
            string link = "https://ecom.rangs.com.bd/status-delivered?key=" + apitoken + "&order_number=" + order_no + "&type=" + type;
            if(type==1)
             LogFile(order_no, "", "set request before call api", link , "",true,false,false);

            HttpWebRequest httpreq = (HttpWebRequest)WebRequest.Create(link);
            httpreq.Method = "post";
            httpreq.ContentType = "application/json";
            //httpreq.ContentLength = datatosend.Length;
            Stream st = httpreq.GetRequestStream();
            st.Write(datatosend, 0, datatosend.Length);
            st.Close();


            HttpWebResponse myresponse = (HttpWebResponse)httpreq.GetResponse();



            StreamReader myreader = new StreamReader(myresponse.GetResponseStream());
            JavaScriptSerializer js = new JavaScriptSerializer();


            dynamic objText = myreader.ReadToEnd();
            //MainData md = new MainData();
            //md = js.Deserialize<MainData>(myreader.ReadToEnd());

         //   var items = JsonConvert.DeserializeObject<Data.MainData>(myreader.ReadToEnd());

            var items = JsonConvert.DeserializeObject<Data.MainData>(objText);
            
            string response = objText.ToString();
            if (type == 1)
                LogFile(order_no, "", "set response", link, response,true,false,false);
            
           
            // Console.WriteLine(items);
             
            //JsonConvert.DeserializeObject<Movie>(json);
            //MainData m = myresponse.con;


            //Console.WriteLine(myreader.ReadToEnd());

            //foreach (var item in items.data.products)
            //{
            //    Console.WriteLine(item.product_model);
            //}
            Data.MainDataFinal _mainDataFinal = new Data.MainDataFinal();
            _mainDataFinal.mainData=items;
            _mainDataFinal.status = true;

            return _mainDataFinal;
           

        }

         
        catch (Exception e)
        {
            Data.MainDataFinal _mainDataFinal = new Data.MainDataFinal();
            _mainDataFinal.mainData = null;
            _mainDataFinal.status = false;
            _mainDataFinal.massage = "Does not hit api properly please deliverd it manually in online store";
            LogFile(order_no, "", "Error in Api:"+e.Message.ToString(),"", "", false,false,false);
           // PopupMessage("Does not hit api properly please deliverd it manually in online store",);
            //Console.WriteLine(count);
            Console.WriteLine("Error: " + e.Message);
            return _mainDataFinal;
        }
    }
    // Redeem Voucher Point

    protected void btnVoucherSearch_Click(object sender, ImageClickEventArgs e)
    {
        //return;
        SqlConnection conn = DBConnection.GetConnection();

        if (txtCustomerMobile.Text == "")
        {
            PopupMessage("Please enter Customer Contact #.", btnVoucherSearch);
            txtCustomerMobile.Focus();
            return;
        }

        if (txtCustomerMobile.Text.Length < 6)
        {
            PopupMessage("Please enter valid Customer Contact #.", btnVoucherSearch);
            txtCustomerMobile.Focus();
            return;
        }
        if (txtCustomerMobile.Text != txtCustContact.Text || txtCustContact.Text == "")
        {
            PopupMessage("Contact No Dont Match with Customer information you have added to customer information or fill up customer information first", btnVoucherSearch);
            //txtRedeemPoint.Text = "0";
            txtCustomerMobile.Focus();
            return;
        }
        //  CHECK & INSERT CUSTOMER INFO
        string sSql = "";
        sSql = "select SUM(EarnedVoucherPoint)EarnedVoucherPoint from [tbCustomerVoucher]" +
            " WHERE CustomerContact like '%" + this.txtCustomerMobile.Text + "%'" +
            " GROUP BY CustomerContact";
        SqlCommand cmdCust = new SqlCommand(sSql, conn);
        conn.Open();
        SqlDataReader drRedeem = cmdCust.ExecuteReader();
        try
        {
            if (drRedeem.Read())
            {
                this.txtAvailablePoint.Text = drRedeem["EarnedVoucherPoint"].ToString();
                this.txtAvailablePoint.Font.Bold = true;
            }
            else
            {
                this.txtAvailablePoint.Text = "";
            }
        }
        catch (InvalidCastException err)
        {
            throw (err);
        }
        finally
        {
            drRedeem.Dispose();
            drRedeem.Close();
            conn.Close();
        }
    }


    public int gcd(int a, int b)
    {
        if (a == 0)
            return b;
        return gcd(b % a, a);
    }
    public int findGCD(List<int> arr, int n)
    {
        int result = arr[0];
        for (int i = 1; i < n; i++)
        {
            result = gcd(arr[i], result);

            if (result == 1)
            {
                return 1;
            }
        }

        return result;
    }
    //    If A, B, and C are integer numbers, then

    //A : B : C = (A/GCF) : B/(GCF) : C/(GCF) . . . . . . . (1)

    //where GCF is the greatest common factor of A, B, and C.

    //If A=m/n, B = p/q, and C = r/s are fractions where m, n, p, q, r, and s are integers then

    //A : B : C = D : E : F where D, E, and F are integer numbers given by

    //D = (m/n)*LCM(nqs), E = (p/q)LCM(nqs), and F = (r/s)LCM(nqs) . . . . (2)

    //where LCM(nqs) is the Least Common Multiple of n, q, and s

    //(1) and (2) are guarantee that the ratios are integer ratios with no further reduction possible.
    //for default redeem plan


    protected void btnRedeem_Click(object sender, EventArgs e)
    {
        return;
        try
        {

           

            if (Convert.ToInt16(txtRedeemPoint.Text) > 6)
            {
                PopupMessage("Maximum 6 point can be redeem in 1 voucher", btnRedeem);
                txtRedeemPoint.Text = "0";
                txtRedeemPoint.Focus();
                return;
            }

            if (txtCustomerMobile.Text != txtCustContact.Text || txtCustContact.Text == "")
            {
                PopupMessage("Contact No Dont Match with Customer information you have added to customer information or fill up customer information first", btnRedeem);
                //txtRedeemPoint.Text = "0";
                txtRedeemPoint.Focus();
                return;
            }



            if (txtCustomerMobile.Text != "" && Convert.ToInt16(txtAvailablePoint.Text) > 0
                 && Convert.ToInt16(txtAvailablePoint.Text) >= Convert.ToInt16(txtRedeemPoint.Text))
            {
                PopupMessage("A confirmation SMS with redeem information will sent to this customer you entered for this challan.", btnRedeem);
                int totalPoint = 0, redeemPoint = 0; double dTAmnt = 0;
                if (this.txtNetAmnt.Text == "")
                {
                    dTAmnt = 0;
                }
                else
                {
                    dTAmnt = Convert.ToDouble(this.txtNetAmnt.Text);
                }
                //dTotalPay = Convert.ToDouble(this.txtCash.Text);
                if (dTAmnt > 14999)//Convert.ToInt16(txtRedeemPoint.Text)
                //if (dTAmnt > (Convert.ToInt16(txtRedeemPoint.Text) * 500))
                {

                    redeemPoint = Convert.ToInt16(this.txtRedeemPoint.Text);
                    totalPoint = redeemPoint * 500;


                    //start work here to fix the bug with calculation of redeem voucher
                    List<int> arr = new List<int>();
                    List<decimal> discountByRatio = new List<decimal>();
                    //List<float> arr = new List<float>();
                    //double decPoints = 0;
                    int giftItemCount = 0;
                    foreach (GridViewRow g1 in this.gvUsers.Rows)
                    {
                        arr.Add(Convert.ToInt32(g1.Cells[10].Text));

                        if (g1.Cells[8].Text == "Free Gift" || g1.Cells[5].Text == "0")
                        {
                            giftItemCount++;
                        }

                    }

                    int gcdValue = findGCD(arr, arr.Count);
                    decimal sumOfRatioValues = 0;
                    for (int i = 0; i < arr.Count; i++)
                    {
                        arr[i] = arr[i] / gcdValue;
                        sumOfRatioValues += arr[i];

                    }
                    float sumOfArr = 0;
                    for (int i = 0; i < arr.Count; i++)
                    {
                        //discountByRatio[i] = arr[i] * totalPoint / sumOfRatioValues;
                        var value = arr[i] * totalPoint / sumOfRatioValues;
                        arr[i] = Convert.ToInt32(Math.Floor(value));
                        sumOfArr += arr[i];
                        //int n = Math.Abs(discountByRatio[i]); // Change to positive
                        //decPoints += discountByRatio[i] - arr[i];
                    }
                    //if ((arr.Count - giftItemCount) > 1)
                    if (sumOfArr < totalPoint)
                    {
                        //Response.Write("<script>console.log('" + arr + "');</script>");

                        for (int i = 0; i < arr.Count; i++)
                        {

                            if (arr[i] > 0)
                            {
                                arr[i] = arr[i] + 1;
                                break;
                            }
                        }

                    }

                    int count = 0;
                    foreach (GridViewRow g1 in this.gvUsers.Rows)
                    {

                        int redeemByRatioValue = arr[count++];
                        int discountValue = Convert.ToInt32(g1.Cells[6].Text);
                        g1.Cells[6].Text = Convert.ToString(discountValue + redeemByRatioValue);
                        if (Convert.ToInt32(g1.Cells[10].Text) != 0)
                        {
                            g1.Cells[10].Text = (Convert.ToInt32(g1.Cells[10].Text) - (redeemByRatioValue)).ToString();
                        }
                        else
                        {
                            g1.Cells[10].Text = "0";
                        }

                    }

                    //end
                    dTAmnt = dTAmnt - totalPoint;
                    this.gvUsers.FooterRow.Cells[10].Text = dTAmnt.ToString();
                    //this.gvUsers.FooterRow.Cells[6].Text = dTAmnt.ToString();

                    this.txtNetAmnt.Text = dTAmnt.ToString();
                    this.txtCash.Text = dTAmnt.ToString();
                    this.txtPay.Text = dTAmnt.ToString();




                    btnAdd.Enabled = false;
                    btnRedeem.Enabled = false;
                    btnSpinCoupon.Enabled = false;
                    txtCustContact.Enabled = false;

                    ddlReference.Enabled = false;
                    btnAvailOffer.Enabled = false;
                }
                else
                {
                    //PopupMessage("Invoice Value Should Be Greater Than 12000 ", btnRedeem);
                    PopupMessage("Invoice Value Should Be Greater Than Reedem Amount ", btnRedeem);
                    txtRedeemPoint.Text = "0";
                    txtNetAmnt.Focus();
                    return;

                }
            }
            else
            {
                PopupMessage("Please Fill Up Customer Mobile No for Seeing Available Point", btnRedeem);
                txtCustomerMobile.Focus();
                txtAvailablePoint.Focus();
                return;
            }

        }
        catch { }
    }



    //this redeem mechanism applied for in case of only refrigerator



    //protected void btnRedeem_Click(object sender, EventArgs e)
    //{
    //    try
    //    {

    //        //List<string> groupName = new List<string>();

    //        //check product group name if it is refrigerator or not
    //        SqlConnection conn = DBConnection.GetConnection();
    //        string sSql = "";
    //        sSql = "select ProductID,GroupName into #temp1 from Product where ";
    //        foreach (GridViewRow g1 in this.gvUsers.Rows)
    //        {
    //            sSql += "or ProductID=" + Convert.ToInt32(g1.Cells[0].Text) + " ";
    //        }
    //        string d = sSql.Replace("where or", "where");
    //        d += "select ProductID,GroupName from #temp1 where GroupName like 'KELVINATOR AC' or GroupName like '%RANGS AC%' drop table #temp1";
    //        SqlCommand cmdCust = new SqlCommand(d, conn);
    //        conn.Open();
    //        Dictionary<int, string> pGroupList = new Dictionary<int, string>();
    //        SqlDataReader drRedeem = cmdCust.ExecuteReader();
    //        try
    //        {
    //            while (drRedeem.Read())
    //            {
    //                //groupName.Add(drRedeem["GroupName"].ToString());
    //                pGroupList.Add(Convert.ToInt32(drRedeem["ProductID"].ToString()), drRedeem["GroupName"].ToString());
    //            }
    //        }
    //        catch (InvalidCastException err)
    //        {
    //            throw (err);
    //        }
    //        finally
    //        {
    //            drRedeem.Dispose();
    //            drRedeem.Close();
    //            conn.Close();
    //        }
    //        //end....
    //        if (txtCustomerMobile.Text != "" && Convert.ToInt16(txtAvailablePoint.Text) > 0
    //             && Convert.ToInt16(txtAvailablePoint.Text) >= Convert.ToInt16(txtRedeemPoint.Text))
    //        {
    //            int totalPoint = 0, redeemPoint = 0; double dTAmnt = 0;
    //            if (this.txtNetAmnt.Text == "")
    //            {
    //                dTAmnt = 0;
    //            }
    //            else
    //            {
    //                dTAmnt = Convert.ToDouble(this.txtNetAmnt.Text);
    //            }
    //            //dTotalPay = Convert.ToDouble(this.txtCash.Text);

    //            if (dTAmnt > 15999)
    //            {
    //                redeemPoint = Convert.ToInt16(this.txtRedeemPoint.Text);
    //                totalPoint = redeemPoint * 500;


    //                //start work here to fix the bug with calculation of redeem voucher
    //                List<int> arr = new List<int>();
    //                List<decimal> discountByRatio = new List<decimal>();

    //                int giftItemCount = 0;

    //                foreach (GridViewRow g1 in this.gvUsers.Rows)
    //                {

    //                    if (pGroupList.ContainsKey(Convert.ToInt32(g1.Cells[0].Text)))
    //                    {
    //                        arr.Add(Convert.ToInt32(g1.Cells[10].Text));
    //                    }
    //                    if (g1.Cells[8].Text == "Free Gift" || g1.Cells[5].Text == "0")
    //                    {
    //                        giftItemCount++;
    //                    }

    //                }

    //                if (arr.Count == 0)
    //                {
    //                    PopupMessage("This offer(voucher redeem) is only available for KELVINATOR and Rangs Ac model models...", btnRedeem);
    //                    txtRedeemPoint.Text = "0";
    //                    txtNetAmnt.Focus();
    //                    return;
    //                }
    //                int gcdValue = findGCD(arr, arr.Count);
    //                decimal sumOfRatioValues = 0;
    //                for (int i = 0; i < arr.Count; i++)
    //                {
    //                    arr[i] = arr[i] / gcdValue;
    //                    sumOfRatioValues += arr[i];
    //                }
    //                float sumOfArr = 0;
    //                for (int i = 0; i < arr.Count; i++)
    //                {

    //                    var value = arr[i] * totalPoint / sumOfRatioValues;
    //                    arr[i] = Convert.ToInt32(Math.Floor(value));
    //                    sumOfArr += arr[i];

    //                }

    //                if (sumOfArr < totalPoint)
    //                {
    //                    for (int i = 0; i < arr.Count; i++)
    //                    {
    //                        if (arr[i] > 0)
    //                        {
    //                            arr[i] = arr[i] + 1;
    //                            break;
    //                        }
    //                    }
    //                }
    //                int count = 0;
    //                //int checkRefrigeretor = 0;

    //                foreach (GridViewRow g1 in this.gvUsers.Rows)
    //                {
    //                    if (pGroupList.ContainsKey(Convert.ToInt32(g1.Cells[0].Text)))
    //                    {
    //                        int redeemByRatioValue = arr[count++];
    //                        int discountValue = Convert.ToInt32(g1.Cells[6].Text);
    //                        g1.Cells[6].Text = Convert.ToString(discountValue + redeemByRatioValue);
    //                        if (Convert.ToInt32(g1.Cells[10].Text) != 0)
    //                        {
    //                            g1.Cells[10].Text = (Convert.ToInt32(g1.Cells[10].Text) - (redeemByRatioValue)).ToString();
    //                        }
    //                        else
    //                        {
    //                            g1.Cells[10].Text = "0";
    //                        }

    //                    }

    //                }
    //                //end
    //                dTAmnt = dTAmnt - totalPoint;
    //                this.gvUsers.FooterRow.Cells[10].Text = dTAmnt.ToString();
    //                //this.gvUsers.FooterRow.Cells[6].Text = dTAmnt.ToString();

    //                this.txtNetAmnt.Text = dTAmnt.ToString();
    //                this.txtCash.Text = dTAmnt.ToString();
    //                this.txtPay.Text = dTAmnt.ToString();




    //                btnAdd.Enabled = false;
    //                btnRedeem.Enabled = false;
    //                btnSpinCoupon.Enabled = false;
    //                txtCustContact.Enabled = false;

    //                ddlReference.Enabled = false;
    //                btnAvailOffer.Enabled = false;
    //            }
    //            else
    //            {
    //                PopupMessage("Invoice Value Should Be Greter Than ", btnRedeem);
    //                txtRedeemPoint.Text = "0";
    //                txtNetAmnt.Focus();
    //                return;

    //            }

    //        }
    //        else
    //        {
    //            PopupMessage("Please Fill Up Customer Mobile No for Seeing Available Point", btnRedeem);
    //            txtCustomerMobile.Focus();
    //            txtAvailablePoint.Focus();
    //            return;
    //        }

    //    }
    //    catch { }
    //}
    public void RedeemPoint()
    {

        SqlConnection conn = DBConnection.GetConnection();

        if (txtCustomerMobile.Text == "")
        {
            PopupMessage("Please enter Customer Contact #.", btnVoucherSearch);
            txtCustomerMobile.Focus();
            return;
        }

        //CHECK & INSERT CUSTOMER INFO
        string sSql = "";
        sSql = "select Id, [CustomerContact],[InvoiceNo],[EarnedVoucherPoint],[RedeemVoucherPoint] FROM [dbCID].[dbo].[tbCustomerVoucher]" +
            " WHERE CustomerContact like '%" + this.txtCustomerMobile.Text + "%'" +
            " AND EarnedVoucherPoint > 0";
        SqlCommand cmdCust = new SqlCommand(sSql, conn);
        conn.Open();
        SqlDataReader drRedeem = cmdCust.ExecuteReader();

        try
        {
            List<Voucher> resultList = new List<Voucher>();
            while (drRedeem.Read())
            {
                Voucher entity = new Voucher();
                entity.Id = (int)drRedeem["Id"];
                entity.CustomerContact = (string)drRedeem["CustomerContact"];
                entity.InvoiceNo = (string)drRedeem["InvoiceNo"];
                entity.EarnedVoucherPoint = (int)drRedeem["EarnedVoucherPoint"];
                entity.RedeemVoucherPoint = (int)drRedeem["RedeemVoucherPoint"];

                resultList.Add(entity);
            }

            conn.Close();

            int redeemPoint = Convert.ToInt32(this.txtRedeemPoint.Text);
            string refInvoice = Convert.ToString(this.txtCHNo.Text);

            foreach (var item in resultList)
            {
                if (item.EarnedVoucherPoint >= redeemPoint)
                {
                    sSql = "";
                    sSql = "Update [dbCID].[dbo].[tbCustomerVoucher] SET EarnedVoucherPoint = (EarnedVoucherPoint - '" + redeemPoint + "'), RedeemVoucherPoint= (RedeemVoucherPoint + '" + redeemPoint + "')" +
                            " ,RefInvoiceNo = '" + refInvoice + "' WHERE Id='" + item.Id + "'";
                    SqlCommand cmdRR = new SqlCommand(sSql, conn);
                    conn.Open();
                    cmdRR.ExecuteNonQuery();
                    conn.Close();
                    // Send SMS
                    SendRedeemSMS(refInvoice, Convert.ToInt32(this.txtRedeemPoint.Text));
                    return;
                }
                else if (item.EarnedVoucherPoint > 0 && item.EarnedVoucherPoint < redeemPoint)
                {
                    sSql = "";
                    sSql = "Update [dbCID].[dbo].[tbCustomerVoucher] SET EarnedVoucherPoint = (EarnedVoucherPoint - '" + item.EarnedVoucherPoint + "'), RedeemVoucherPoint = (RedeemVoucherPoint + '" + item.EarnedVoucherPoint + "')" +
                             " ,RefInvoiceNo = '" + refInvoice + "' WHERE Id='" + item.Id + "'";
                    SqlCommand cmdRR = new SqlCommand(sSql, conn);
                    conn.Open();
                    cmdRR.ExecuteNonQuery();
                    conn.Close();

                    redeemPoint = redeemPoint - item.EarnedVoucherPoint;
                }
            }


        }
        catch { }
    }

    //protected void btnGenerateCoupon_OnClick(object sender, ImageClickEventArgs e)
    //{
    //    try
    //    {
    //        if (txtCustContact.Text == "")
    //        {
    //            PopupMessage("Please enter Customer Contact first", btnGenerateCoupon);
    //            txtCustContact.Focus();
    //            return;
    //        }

    //        if (this.gvUsers.Rows.Count < 1)
    //        {
    //            PopupMessage("Please add product into cart first", btnGenerateCoupon);
    //            ddlContinents.Focus();
    //            return;
    //        }


    //        int totalVoucherAmount = 0;
    //        int nextAvailAmount = 0;
    //        string customerMobile = txtCustContact.Text;
    //        string mobileCode = customerMobile.Substring(customerMobile.Length - 4);

    //        Random r = new Random();
    //        int randNum = r.Next(1000000);
    //        string sixDigitNumber = randNum.ToString("D6");

    //        string voucherCode = sixDigitNumber + mobileCode;

    //        foreach (GridViewRow g1 in this.gvUsers.Rows)
    //        {
    //            //string sDisRef = "";
    //            //if (g1.Cells[8].Text.Trim() != "&nbsp;")
    //            //{
    //            //    sDisRef = g1.Cells[8].Text.Trim();
    //            //}
    //            //else
    //            //{
    //            //    sDisRef = g1.Cells[8].Text = "";
    //            //}

    //            string productModel = "";
    //            if (g1.Cells[8].Text.Trim() != "&nbsp;")
    //            {
    //                productModel = g1.Cells[1].Text.Trim();
    //            }
    //            else
    //            {
    //                productModel = g1.Cells[1].Text = "";
    //            }


    //            string productPrice = "";
    //            if (g1.Cells[8].Text.Trim() != "&nbsp;")
    //            {
    //                productPrice = g1.Cells[10].Text.Trim();
    //            }
    //            else
    //            {
    //                productPrice = g1.Cells[10].Text = "";
    //            }

    //            if (Convert.ToInt32(productPrice) >= 10000)
    //            {
    //                SqlConnection conn = DBConnection.GetConnection();

    //                //  CHECK MODEL VOUCHER AMOUNT
    //                string sSql = "";
    //                sSql = "select DigitalCashReturn, NextPurchaseVoucher from tbDigitalVoucher" +
    //                       " WHERE Model like '%" + productModel + "%'";
    //                SqlCommand cmdVoucher = new SqlCommand(sSql, conn);

    //                conn.Open();
    //                SqlDataReader drRedeem = cmdVoucher.ExecuteReader();
    //                try
    //                {
    //                    if (drRedeem.Read())
    //                    {
    //                        totalVoucherAmount += Convert.ToInt32(drRedeem["DigitalCashReturn"].ToString());
    //                        nextAvailAmount += Convert.ToInt32(drRedeem["NextPurchaseVoucher"].ToString());
    //                    }
    //                    else
    //                    {
    //                        totalVoucherAmount += 0;
    //                        nextAvailAmount += 0;
    //                    }
    //                }
    //                catch (InvalidCastException err)
    //                {
    //                    throw (err);
    //                }
    //                finally
    //                {
    //                    drRedeem.Dispose();
    //                    drRedeem.Close();
    //                    conn.Close();
    //                }
    //            }
    //        }

    //        if (totalVoucherAmount > 0)
    //        {
    //            txtCouponCode.Text = voucherCode;
    //            txtDiscountAmount.Text = totalVoucherAmount.ToString();

    //            sendInitialRedeemSMS(txtCHNo.Text, totalVoucherAmount, voucherCode);

    //            /// Next Purchase Coupon
    //            /// 
    //            //if (nextAvailAmount > 0)
    //            //{
    //            //    sendNextRedeemSMS(txtCHNo.Text, nextAvailAmount, voucherCode);
    //            //}

    //            int redeemAmnt = 0; double dTAmnt = 0;
    //            if (this.txtNetAmnt.Text == "")
    //            {
    //                dTAmnt = 0;
    //            }
    //            else
    //            {
    //                dTAmnt = Convert.ToDouble(this.txtNetAmnt.Text);
    //            }
    //            redeemAmnt = Convert.ToInt16(this.txtDiscountAmount.Text);
    //            dTAmnt = dTAmnt - redeemAmnt;

    //            this.txtNetAmnt.Text = dTAmnt.ToString();
    //            this.txtCash.Text = dTAmnt.ToString();
    //            this.txtPay.Text = dTAmnt.ToString();

    //            btnAdd.Enabled = false;
    //            btnRedeem.Enabled = false;
    //            btnGenerateCoupon.Enabled = false;
    //        }
    //        else
    //        {
    //            PopupMessage("Those models not eligible for generate coupon", btnGenerateCoupon);
    //            //ddlContinents.Focus();
    //            return;
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        throw (ex);
    //    }
    //}

    //protected void btnRedeem_OnClick(object sender, EventArgs e)
    //{
    //    if (txtCouponCode.Text == "")
    //    {
    //        PopupMessage("Please generate coupon first.", btnGenerateCoupon);
    //        btnGenerateCoupon.Focus();
    //        return;
    //    }

    //    if (txtDiscountAmount.Text == "")
    //    {
    //        PopupMessage("Please generate valid coupon first.", btnGenerateCoupon);
    //        btnGenerateCoupon.Focus();
    //        return;
    //    }

    //    try
    //    {
    //        int redeemAmnt = 0; double dTAmnt = 0;
    //        if (this.txtNetAmnt.Text == "")
    //        {
    //            dTAmnt = 0;
    //        }
    //        else
    //        {
    //            dTAmnt = Convert.ToDouble(this.txtNetAmnt.Text);
    //        }
    //        redeemAmnt = Convert.ToInt16(this.txtDiscountAmount.Text);
    //        dTAmnt = dTAmnt - redeemAmnt;

    //        this.txtNetAmnt.Text = dTAmnt.ToString();
    //        this.txtCash.Text = dTAmnt.ToString();
    //        this.txtPay.Text = dTAmnt.ToString();

    //        btnAdd.Enabled = false;
    //        btnRedeem.Enabled = false;
    //    }
    //    catch (Exception exception)
    //    {
    //        throw exception;
    //    }
    //}
    private string sendUtsab_raffle(string invNo)
    {
        string myParameters = "";
        try
        {
            string sms = "Congratulations! Your SONY-RANGS Eid Utsab raffle draw coupon number is " + invNo + ".";
            //saveLogs("sms string " + sms, true);
            string msisdn = this.txtCustContact.Text.Trim();
            //string msisdn = "";
            string apitoken = "j5iv07qb-t32ljykq-2su04uuz-xnwxdrjd-pgz8wd4h";
            ServicePointManager.Expect100Continue = false;
            myParameters = @"data=SENDSMS&cpuid=isms&sid=BRANDSONYRANGS" + "&msisdn=" + msisdn + "&api_token=" + apitoken + "&sms=" + System.Web.HttpUtility.UrlEncode(sms) + "&csms_id=123456";
            //saveLogs(myParameters, true);
            byte[] datatosend = Encoding.ASCII.GetBytes(myParameters);
            HttpWebRequest httpreq = (HttpWebRequest)WebRequest.Create("https://smsplus.sslwireless.com/api/v3/send-sms");
            httpreq.Method = "post";
            httpreq.ContentType = "application/x-www-form-urlencoded";
            httpreq.ContentLength = datatosend.Length;
            Stream st = httpreq.GetRequestStream();
            st.Write(datatosend, 0, datatosend.Length);
            st.Close();
            HttpWebResponse myresponse = (HttpWebResponse)httpreq.GetResponse();
            StreamReader myreader = new StreamReader(myresponse.GetResponseStream());
            return myreader.ReadToEnd().ToString();
        }
        catch (Exception err)
        {
            //saveLogs("Error Occured when inserting into sms api", true);
            //saveLogs("Error Occured when inserting into sms api, Error=>> " + err.ToString(), false);
            return "<SMS_STATUS>ERROR</SMS_STATUS>";
        }
    }
    private string sendReferenceSMS(string refName, int discountAmount)
    {
        string myParameters = "";
        try
        {
            string sms = "CTP# " + Session["eName"] + " ;Customer# " + txtCustName.Text + " ;Customer Mobile# " + txtCustContact.Text +
               " ;Invoice# " + txtCHNo.Text + " ;Get Tk " + discountAmount + " discount by using your reference; Dated: " + String.Format("{0:dd/MM/yyyy}", DateTime.Now) + "";
            string msisdn = "";

            //if (refName == "GM Sir")
            //{
            //    msisdn = "01722547625";
            //}
            //else if (refName == "DGM Sir")
            //{
            //    msisdn = "01722547625";
            //}
            //else if (refName == "Nipa Madam")
            //{
            //    msisdn = "01722547625";
            //}


            if (refName == "GM Sir")
            {
                msisdn = "01727417288";
            }
            else if (refName == "DGM Sir")
            {
                msisdn = "01727417288";
            }
            else if (refName == "Nipa Madam")
            {
                msisdn = "01708151439";
            }

            this.txtCustContact.Text.Trim();
            //string msisdn = "";
            string apitoken = "j5iv07qb-t32ljykq-2su04uuz-xnwxdrjd-pgz8wd4h";

            ServicePointManager.Expect100Continue = false;
            myParameters = @"data=SENDSMS&cpuid=isms&sid=BRANDSONYRANGS" + "&msisdn=" + msisdn + "&api_token=" + apitoken + "&sms=" + System.Web.HttpUtility.UrlEncode(sms) + "&csms_id=123456";
            //saveLogs(myParameters, true);
            byte[] datatosend = Encoding.ASCII.GetBytes(myParameters);
            HttpWebRequest httpreq = (HttpWebRequest)WebRequest.Create("https://smsplus.sslwireless.com/api/v3/send-sms");
            httpreq.Method = "post";
            httpreq.ContentType = "application/x-www-form-urlencoded";
            httpreq.ContentLength = datatosend.Length;
            Stream st = httpreq.GetRequestStream();
            st.Write(datatosend, 0, datatosend.Length);
            st.Close();
            HttpWebResponse myresponse = (HttpWebResponse)httpreq.GetResponse();
            StreamReader myreader = new StreamReader(myresponse.GetResponseStream());
            return myreader.ReadToEnd().ToString();
        }
        catch (Exception err)
        {
            //saveLogs("Error Occured when inserting into sms api", true);
            //saveLogs("Error Occured when inserting into sms api, Error=>> " + err.ToString(), false);
            return "<SMS_STATUS>ERROR</SMS_STATUS>";
        }
    }

    private string sendInitialRedeemSMS(string refInvoice, int redeemAmount, string coupon)
    {
        string myParameters = "";
        try
        {
            string sms = "Coupon# " + coupon + " ;Tk " + redeemAmount + " received as Cash Return for your purchase Invoice # " + refInvoice + "; Dated: " + String.Format("{0:dd/MM/yyyy}", DateTime.Now) + "";
            //saveLogs("sms string " + sms, true);
            string msisdn = this.txtCustContact.Text.Trim();
            //string msisdn = "";
            string apitoken = "j5iv07qb-t32ljykq-2su04uuz-xnwxdrjd-pgz8wd4h";

            ServicePointManager.Expect100Continue = false;
            myParameters = @"data=SENDSMS&cpuid=isms&sid=BRANDSONYRANGS" + "&msisdn=" + msisdn + "&api_token=" + apitoken + "&sms=" + System.Web.HttpUtility.UrlEncode(sms) + "&csms_id=123456";
            //saveLogs(myParameters, true);
            byte[] datatosend = Encoding.ASCII.GetBytes(myParameters);
            HttpWebRequest httpreq = (HttpWebRequest)WebRequest.Create("https://smsplus.sslwireless.com/api/v3/send-sms");
            httpreq.Method = "post";
            httpreq.ContentType = "application/x-www-form-urlencoded";
            httpreq.ContentLength = datatosend.Length;
            Stream st = httpreq.GetRequestStream();
            st.Write(datatosend, 0, datatosend.Length);
            st.Close();
            HttpWebResponse myresponse = (HttpWebResponse)httpreq.GetResponse();
            StreamReader myreader = new StreamReader(myresponse.GetResponseStream());
            return myreader.ReadToEnd().ToString();
        }
        catch (Exception err)
        {
            //saveLogs("Error Occured when inserting into sms api", true);
            //saveLogs("Error Occured when inserting into sms api, Error=>> " + err.ToString(), false);
            return "<SMS_STATUS>ERROR</SMS_STATUS>";
        }
    }

    private string sendNextRedeemSMS(string refInvoice, int redeemAmount, string coupon)
    {
        string myParameters = "";
        try
        {
            string sms = "Congratulations! You have received Tk " + redeemAmount + " Cash Voucher for your purchase Invoice # " + refInvoice + "; Dated:" + String.Format("{0:dd/MM/yyyy}", DateTime.Now) +
                         " This Voucher can be used on your next purchase of Tk 15000 or more amount."; // . Valid till from 01/09/2021 to 30/09/20021.
            //saveLogs("sms string " + sms, true);
            string msisdn = this.txtCustContact.Text.Trim();
            //string msisdn = "";
            string apitoken = "j5iv07qb-t32ljykq-2su04uuz-xnwxdrjd-pgz8wd4h";

            ServicePointManager.Expect100Continue = false;
            myParameters = @"data=SENDSMS&cpuid=isms&sid=BRANDSONYRANGS" + "&msisdn=" + msisdn + "&api_token=" + apitoken + "&sms=" + System.Web.HttpUtility.UrlEncode(sms) + "&csms_id=123456";
            //saveLogs(myParameters, true);
            byte[] datatosend = Encoding.ASCII.GetBytes(myParameters);
            HttpWebRequest httpreq = (HttpWebRequest)WebRequest.Create("https://smsplus.sslwireless.com/api/v3/send-sms");
            httpreq.Method = "post";
            httpreq.ContentType = "application/x-www-form-urlencoded";
            httpreq.ContentLength = datatosend.Length;
            Stream st = httpreq.GetRequestStream();
            st.Write(datatosend, 0, datatosend.Length);
            st.Close();
            HttpWebResponse myresponse = (HttpWebResponse)httpreq.GetResponse();
            StreamReader myreader = new StreamReader(myresponse.GetResponseStream());
            return myreader.ReadToEnd().ToString();
        }
        catch (Exception err)
        {
            //saveLogs("Error Occured when inserting into sms api", true);
            //saveLogs("Error Occured when inserting into sms api, Error=>> " + err.ToString(), false);
            return "<SMS_STATUS>ERROR</SMS_STATUS>";
        }
    }

    private string sendConfirmRedeemSMS(string refInvoice, int redeemAmount, string coupon)
    {
        string myParameters = "";
        try
        {
            string sms = "Coupon# " + coupon + " ;Tk " + redeemAmount + " redeemed on Invoice# " + refInvoice + "; Dated: " + String.Format("{0:dd/MM/yyyy}", DateTime.Now) + "";
            //saveLogs("sms string " + sms, true);
            string msisdn = this.txtCustContact.Text.Trim();
            //string msisdn = "";
            string apitoken = "j5iv07qb-t32ljykq-2su04uuz-xnwxdrjd-pgz8wd4h";

            ServicePointManager.Expect100Continue = false;
            myParameters = @"data=SENDSMS&cpuid=isms&sid=BRANDSONYRANGS" + "&msisdn=" + msisdn + "&api_token=" + apitoken + "&sms=" + System.Web.HttpUtility.UrlEncode(sms) + "&csms_id=123456";
            //saveLogs(myParameters, true);
            byte[] datatosend = Encoding.ASCII.GetBytes(myParameters);
            HttpWebRequest httpreq = (HttpWebRequest)WebRequest.Create("https://smsplus.sslwireless.com/api/v3/send-sms");
            httpreq.Method = "post";
            httpreq.ContentType = "application/x-www-form-urlencoded";
            httpreq.ContentLength = datatosend.Length;
            Stream st = httpreq.GetRequestStream();
            st.Write(datatosend, 0, datatosend.Length);
            st.Close();
            HttpWebResponse myresponse = (HttpWebResponse)httpreq.GetResponse();
            StreamReader myreader = new StreamReader(myresponse.GetResponseStream());
            return myreader.ReadToEnd().ToString();
        }
        catch (Exception err)
        {
            //saveLogs("Error Occured when inserting into sms api", true);
            //saveLogs("Error Occured when inserting into sms api, Error=>> " + err.ToString(), false);
            return "<SMS_STATUS>ERROR</SMS_STATUS>";
        }
    }

    private string SendRedeemSMS(string refInvoice, int redeemPoint)
    {
        string myParameters = "";
        try
        {
            string sms = //"চালান নং : " + refInvoice + "";
            "চালান নং: " + refInvoice +
                     " এর জন্য আপনার অ্যাকাউন্ট থেকে " + redeemPoint + " টি ভাউচার পয়েন্ট রিডিম করা হয়েছে";
            //saveLogs("sms string " + sms, true);
            string msisdn = this.txtCustomerMobile.Text.Trim();
            //string msisdn = "";
            string apitoken = "j5iv07qb-t32ljykq-2su04uuz-xnwxdrjd-pgz8wd4h";

            ServicePointManager.Expect100Continue = false;
            myParameters = @"data=SENDSMS&cpuid=isms&sid=BRANDSONYRANGS" + "&msisdn=" + msisdn + "&api_token=" + apitoken + "&sms=" + System.Web.HttpUtility.UrlEncode(sms) + "&csms_id=123456";
            //saveLogs(myParameters, true);
            byte[] datatosend = Encoding.ASCII.GetBytes(myParameters);
            HttpWebRequest httpreq = (HttpWebRequest)WebRequest.Create("https://smsplus.sslwireless.com/api/v3/send-sms");
            httpreq.Method = "post";
            httpreq.ContentType = "application/x-www-form-urlencoded";
            httpreq.ContentLength = datatosend.Length;
            Stream st = httpreq.GetRequestStream();
            st.Write(datatosend, 0, datatosend.Length);
            st.Close();
            HttpWebResponse myresponse = (HttpWebResponse)httpreq.GetResponse();
            StreamReader myreader = new StreamReader(myresponse.GetResponseStream());
           // fnSendMail_Invoice_For_Redeem();
            return myreader.ReadToEnd().ToString();
        }
        catch (Exception err)
        {
            //saveLogs("Error Occured when inserting into sms api", true);
            //saveLogs("Error Occured when inserting into sms api, Error=>> " + err.ToString(), false);
            return "<SMS_STATUS>ERROR</SMS_STATUS>";
        }
    }


    protected void LoadDiscountReferenceList()
    {
        ddlRefDiscount.Items.Clear();
        ddlRefDiscount.Items.Insert(0, new ListItem("Select Ref. Source", "0"));
        ddlRefDiscount.Items.Insert(1, new ListItem("Online Order", "Online Order"));
        ddlRefDiscount.Items.Insert(2, new ListItem("Free Gift", "Free Gift"));
        ddlRefDiscount.Items.Insert(3, new ListItem("GM Sir (Marketing)", "GM Sir"));
        ddlRefDiscount.Items.Insert(4, new ListItem("DGM Sir (Sales)", "DGM Sir"));
        ddlRefDiscount.Items.Insert(5, new ListItem("PWP Offer 10%", "PWP Offer 10%"));
        ddlRefDiscount.Items.Insert(6, new ListItem("PWP Offer 15%", "PWP Offer 15%"));
        //ddlRefDiscount.Items.Insert(7, new ListItem("New LG TV Launching Offer", "New LG TV Launching Offer"));
        //PWP Offer 15%

        //ddlRefDiscount.Items.Insert(5, new ListItem("..", ".."));
        //if (Session["eName"].ToString() == "TANGAIL NEW CTP")
        //{
        //    ddlRefDiscount.Items.Insert(6, new ListItem("Tangail CTP Opening", "Tangail CTP Opening"));
        //}

        //if (Session["eName"].ToString() == "PALLABI CTP")
        //{
        //    ddlRefDiscount.Items.Insert(6, new ListItem("Pallabi CTP Opening", "Pallabi CTP Opening"));
        //}

        ddlRefDiscount.Items.Insert(7, new ListItem("Customer Withdrawal", "Customer Withdrawal"));
       // ddlRefDiscount.Items.Insert(8, new ListItem("Exchange Offer", "Exchange Offer"));
        ddlRefDiscount.Items.Insert(8, new ListItem("Package-01", "Package-01"));
        ddlRefDiscount.Items.Insert(9, new ListItem("Package-02", "Package-02"));
        ddlRefDiscount.Items.Insert(10, new ListItem("Package-03", "Package-03"));
        ddlRefDiscount.Items.Insert(11, new ListItem("Package-04", "Package-04"));
        ddlRefDiscount.Items.Insert(12, new ListItem("Package-05", "Package-05"));
        ddlRefDiscount.Items.Insert(13, new ListItem("Package-06", "Package-06"));
        

       



        if (Session["UserName"].ToString() == "ashraf" || Session["UserName"].ToString() == "mamoon" || Session["UserName"].ToString() == "admin@inst1")//admin@inst1
        {
            ddlRefDiscount.Items.Insert(14, new ListItem("Tendar or Quotation", "Tendar or Quotation"));

        }

        if (Session["UserName"].ToString() == "gomes")
        {
            ddlRefDiscount.Items.Insert(14, new ListItem("Daraz Online Sale", "Daraz Online Sale"));
        }

        if (Session["UserName"].ToString() == "admin@ditf23")
        {
            ddlRefDiscount.Items.Insert(14, new ListItem("DITF-2023", "DITF-2023"));
        }

        //LG LCD TV speacial discount
        ddlRefDiscount.Items.Insert(ddlRefDiscount.Items.Count, new ListItem("LG TV 6%", "LG Tv 6%"));
        ddlRefDiscount.Items.Insert(ddlRefDiscount.Items.Count, new ListItem("LG TV 8%", "LG Tv 8%"));
        ddlRefDiscount.Items.Insert(ddlRefDiscount.Items.Count, new ListItem("LG TV 10%", "LG Tv 10%"));
        ddlRefDiscount.Items.Insert(ddlRefDiscount.Items.Count, new ListItem("LG Instant Cashback Offer", "LG TV Cashback Offer"));
        ddlRefDiscount.Items.Insert(ddlRefDiscount.Items.Count, new ListItem("Sony Instant Cashback Offer", "Sony TV Cashback Offer"));
        ddlRefDiscount.Items.Insert(ddlRefDiscount.Items.Count, new ListItem("Ac Exchange offer 10%", "Ac Exchange dis 10%"));
        ddlRefDiscount.Items.Insert(ddlRefDiscount.Items.Count, new ListItem("Mosque/ Relagious Institution/ Muktijoddha Offer 5%", "Mosque/Relagious/Muktijoddha Offer 5%"));
        ddlRefDiscount.Items.Insert(ddlRefDiscount.Items.Count, new ListItem("Great Exchange Offer", "Great Exchange Offer"));
        ddlRefDiscount.Items.Insert(ddlRefDiscount.Items.Count, new ListItem("Facebook Special Campeign", "Facebook Special Campeign"));
        ddlRefDiscount.Items.Insert(ddlRefDiscount.Items.Count, new ListItem("GP Star Offer", "GP Star Offer"));
        ddlRefDiscount.Items.Insert(ddlRefDiscount.Items.Count, new ListItem("Pickaboo", "Pickaboo"));
        ddlRefDiscount.Items.Insert(ddlRefDiscount.Items.Count, new ListItem("GP Prime Postpaid", "GP Prime Postpaid"));
        ddlRefDiscount.Items.Insert(ddlRefDiscount.Items.Count, new ListItem("Fantastic Four Showroom Special", "Fantastic Four Showroom Special"));
        ddlRefDiscount.Items.Insert(ddlRefDiscount.Items.Count, new ListItem("EBL 5% Discount DEC-2023", "EBL 5% Discount DEC-2023"));


      



        

        //ddlRefDiscount.Items.Insert(7, new ListItem("Price Exchange", "Price Exchange"));
        //ddlRefDiscount.Items.Insert(8, new ListItem("Cash Voucher", "Cash Voucher"));
        //ddlRefDiscount.Items.Insert(9, new ListItem("Bank Voucher", "Bank Voucher"));
        //ddlRefDiscount.Items.Insert(10, new ListItem("Facebook Tag", "Facebook Tag"));
        ////ddlRefDiscount.Items.Insert(11, new ListItem("T20 Scratch Card", "T20 Scratch Card"));

        ddlRefDiscount.Items.Insert(12, new ListItem("GP Star Coupon", "GP Star Coupon"));
        //ddlRefDiscount.Items.Insert(13, new ListItem("Clearance Sale", "Clearance Sale"));
        ////ddlRefDiscount.Items.Insert(15, new ListItem("Launcing Offer", "Launcing Offer"));
        ////ddlRefDiscount.Items.Insert(14, new ListItem("..", ".."));
        ////ddlRefDiscount.Items.Insert(15, new ListItem("..", ".."));
        ////ddlRefDiscount.Items.Insert(16, new ListItem("..", ".."));
        //ddlRefDiscount.Items.Insert(14, new ListItem("Vacation Package", "Vacation Package"));
        //ddlRefDiscount.Items.Insert(15, new ListItem("EID UTSAB", "EID UTSAB"));//Local Micro Campaign
        //ddlRefDiscount.Items.Insert(16, new ListItem("Local Micro Campaign", "Local Micro Campaign"));//
        //ddlRefDiscount.Items.Insert(17, new ListItem("Scratch Card", "Scratch Card"));//
        //ddlRefDiscount.Items.Insert(18, new ListItem("LG EXPO GIFT VOUCHER", "LG EXPO GIFT VOUCHER"));
    }

    protected void LoadReferenceList()
    {
        //<asp:ListItem>0 Month</asp:ListItem>
        //                            <asp:ListItem>3 Month</asp:ListItem>
        //                            <asp:ListItem>6 Month</asp:ListItem>
        //                            <asp:ListItem>12 Month</asp:ListItem>
        //                            <asp:ListItem>18 Month</asp:ListItem>
        //                            <asp:ListItem>24 Month</asp:ListItem>
        //                            <asp:ListItem>36 Month</asp:ListItem>
        //                        </asp:DropDownList>--%>
        ddlEMEIInfo.Items.Clear();
        ddlEMEIInfo.Items.Insert(0, new ListItem("0 Month", "0 Month"));
        ddlEMEIInfo.Items.Insert(1, new ListItem("3 Months", "3 Months"));
        ddlEMEIInfo.Items.Insert(2, new ListItem("6 Months", "6 Months"));
        ddlEMEIInfo.Items.Insert(3, new ListItem("7 Months", "7 Months"));
        ddlEMEIInfo.Items.Insert(4, new ListItem("9 Months", "9 Months"));
        ddlEMEIInfo.Items.Insert(5, new ListItem("12 Months", "12 Months"));
        ddlEMEIInfo.Items.Insert(6, new ListItem("18 Months", "18 Months"));
        ddlEMEIInfo.Items.Insert(7, new ListItem("24 Months", "24 Months"));
        ddlEMEIInfo.Items.Insert(8, new ListItem("36 Months", "36 Months"));




        ddlReference.Items.Clear();
        ddlReference.Items.Insert(0, new ListItem("Select Reference", "0"));
        ddlReference.Items.Insert(1, new ListItem("GP Star/Banglalink Orange Club", "GP Star"));//GP Star,Bank Card Discount,Exchange Offer(Active),Exchange Offer(In-Active)
        //ddlReference.Items.Insert(2, new ListItem("Bank Card Discount", "Bank Card Discount"));
        //ddlReference.Items.Insert(3, new ListItem("Exchange Offer(Active)", "Exchange Offer(Active)"));
        //ddlReference.Items.Insert(4, new ListItem("Exchange Offer(In-Active)", "Exchange Offer(In-Active)"));//
        //ddlReference.Items.Insert(1, new ListItem("LG EXPO GIFT VOUCHER", "LG EXPO GIFT VOUCHER"));
        //ddlReference.Items.Insert(2, new ListItem("EBL/AMEX/BRAC Bank", "EBL/AMEX/BRAC Bank"));
        //ddlReference.Items.Insert(3, new ListItem("BKash Discount", "BKash Discount"));
        //ddlReference.Items.Insert(4, new ListItem("Freedom Fighter", "Freedom Fighter"));
        //ddlReference.Items.Insert(2, new ListItem("Nagad/Bkash", "Nagad/Bkash"));
        

        //if (Session["eName"].ToString() == "SONY CENTER (B.C)")
        //{
        //    ddlReference.Items.Insert(3, new ListItem("LG TV EXPO 5%", "LG TV EXPO 5%"));
        //}

        //if (Session["eName"].ToString() == "DITF-2023")
        //{
        //    ddlReference.Items.Insert(3, new ListItem("DITF-2023", "DITF-2023"));//ditf-2023
        //}
        

    }

    protected void ddlReference_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (txtNetAmnt.Text == "")
        {
            PopupMessage("Please add the product first.", ddlReference);
            btnAdd.Focus();
            return;
        }
        int totalAmnt = Convert.ToInt32(txtNetAmnt.Text);
        int availAmount = 0;


        //        55ONED80, 65QNED80, 75QNED80, 55NANO75, 65NANO86, 75NANO75 - 15000

        //GS-X6172NS, GS-Q6472NS, KHV-422WDNFSBS2D, 
        //KHV-418NF2DBG, KHV-590NFSBS2DBG, KHV-516FF, KHV-401FFGI (Inverter), KHV-645NFSBS2DMG - 10000

        //FV1409S4W, FV1409H3W, KWM-KT8222GDS, KWM-KT9222GDS, KMW-KT1021B, EW6F5722BB, EWF8241SS5-5000

        //All Alpha Camera & Lens Models - 2500

        //Reference wise discount bkash/nagad/gp star

        double dTAmnt = 0;
        if (this.txtNetAmnt.Text == "")
        {
            dTAmnt = 0;
        }
        else
        {
            dTAmnt = Convert.ToDouble(this.txtNetAmnt.Text);
        }
        var discountListOnGpStar = new[]
        {
            new { Discount=20000, Model = "XR-55A80J" },
            new { Discount=20000, Model = "XR-65A80J" },
            new { Discount=20000,  Model = "XR-55A80K" },
            new { Discount=20000, Model = "XR-65A80K" },
            new { Discount=20000, Model = "XR-77A80K" },

             new { Discount=20000, Model = "OLED65A1" },
            new { Discount=20000,  Model = "OLED55C2" },
            new { Discount=20000, Model = "OLED65C2" },
            new { Discount=20000, Model = "OLED65G2" },



            new { Discount=15000, Model = "55QNED80" },
            new { Discount=15000, Model = "65QNED80" },
            new { Discount=15000,  Model = "75QNED80" },
            new { Discount=15000, Model = "GS-Q6472NS" },
            new { Discount=15000, Model = "KHV-422WDNFSBS2D" },

            new { Discount=10000,  Model = "KHV-418NFSBS2DBG" },//KHV-418NFSBS2DBG
            new { Discount=10000, Model = "KHV-590NFSBS2DBG" },
            new { Discount=10000,  Model = "KHV-516FF" },
            new { Discount=10000,  Model = "KHV-401FFGI (Inverter)" },
            new { Discount=10000,  Model = "KHV-645NFSBS2DMG" },

            new { Discount=5000,  Model = "FV1409S4W" },
            new { Discount=5000,  Model = "FV1409H3W" },
            new { Discount=5000, Model = "KWM-KT8222GDS" },
            new { Discount=5000, Model = "KWM-KT9222GDS" },//KWM-KT9222GDS
            new { Discount=5000,  Model = "KMW-KT1021B" },//KMW-KT1021B
            new { Discount=5000,  Model = "EW6F5722BB" },
            new { Discount=5000,  Model = "EWF8241SS5" },
            //FV1409S4W, FV1409H3W, KWM-KT8222GDS, KWMKT9222GDS, KMW-KT1021B, EW6F5722BB, EWF8241SS5
            //, , , , , 
//, , , 
            //new { Discount=2000,  Model = "" },
            //new { Discount=3000,  Model = "" },
            //new { Discount=1000,  Model = "" },
            //new { Discount=1000,  Model = "" },
            //new { Discount=1000,  Model = "" },
            //new { Discount=750,  Model = "" },
            //new { Discount=1000,  Model = "" },
            //new { Discount=750,  Model = "" },
            //new { Discount=500, Model = "" },              
        }.ToList();
        //
     

        if (ddlReference.SelectedValue == "GP Star")
        {
            string selectedModel = "";
            int selectedDiscount = 0;
            string options = "option 1";

            foreach (var item in discountListOnGpStar)
            {
                foreach (GridViewRow g1 in this.gvUsers.Rows)
                {
                    if (item.Model == g1.Cells[1].Text)
                    {
                        if (item.Discount > selectedDiscount)
                        {
                            selectedDiscount = item.Discount;
                            selectedModel = item.Model;
                        }
                    }
                }
            }
            foreach (GridViewRow g1 in this.gvUsers.Rows)
            {

                if (selectedModel == g1.Cells[1].Text)
                {
                    options = "option 2";
                    int qty = Convert.ToInt32(g1.Cells[4].Text);
                    int discountValue = Convert.ToInt32(g1.Cells[6].Text);
                    g1.Cells[6].Text = Convert.ToString(discountValue + selectedDiscount);
                    if (Convert.ToInt32(g1.Cells[10].Text) != 0)
                    {
                        g1.Cells[10].Text = (Convert.ToInt32(g1.Cells[10].Text) - (selectedDiscount)).ToString();
                    }
                    else
                    {
                        g1.Cells[10].Text = "0";
                    }
                    availAmount = selectedDiscount;
                    dTAmnt = dTAmnt - selectedDiscount;
                    break;
                }
            }

            //end

            if (options == "option 2")
            {
                this.gvUsers.FooterRow.Cells[10].Text = dTAmnt.ToString();
                //this.gvUsers.FooterRow.Cells[6].Text = (selectedDiscount + Convert.ToInt32(this.gvUsers.FooterRow.Cells[6].Text.Replace(',', ''))).ToString();

                this.txtNetAmnt.Text = dTAmnt.ToString();
                this.txtCash.Text = dTAmnt.ToString();
                this.txtPay.Text = dTAmnt.ToString();
                //this.txt

                btnAdd.Enabled = false;
                btnRedeem.Enabled = false;
                btnSpinCoupon.Enabled = false;
                txtCustContact.Enabled = false;
                ddlReference.Enabled = false;
                btnAvailOffer.Enabled = false;
            }

            if (totalAmnt >= 0 && options == "option 1")
            {
                int availPercent = (totalAmnt / 100) * 10;
                if (availPercent > 1500) { availAmount = 1000; }
                else { availAmount = availPercent; }
            }
        }
        else if (ddlReference.SelectedValue == "Bank Card Discount")
        {
            if (totalAmnt >= 15000)
            {
                int availPercent = (totalAmnt / 100) * 2;
                if (availPercent > 1000) { availAmount = 1000; }
                else { availAmount = availPercent; }
            }
        }
        else if (ddlReference.SelectedValue == "DITF-2023")
        {
            if (totalAmnt >= 15000)
            {
                int availPercent = (totalAmnt / 100) * 2;
                if (availPercent > 1000) { availAmount = 1000; }
                else { availAmount = availPercent; }
            }


            //foreach (GridViewRow g1 in this.gvUsers.Rows)
            //{

                
                    
            //        int qty = Convert.ToInt32(g1.Cells[4].Text);
            //        int discountValue = Convert.ToInt32(g1.Cells[6].Text);
            //        g1.Cells[6].Text = Convert.ToString(discountValue + selectedDiscount);
            //        if (Convert.ToInt32(g1.Cells[10].Text) != 0)
            //        {
            //            g1.Cells[10].Text = (Convert.ToInt32(g1.Cells[10].Text) - (selectedDiscount)).ToString();
            //        }
            //        else
            //        {
            //            g1.Cells[10].Text = "0";
            //        }
            //        dTAmnt = dTAmnt - selectedDiscount;
            //        break;
                
            //}
        }
        else if (ddlReference.SelectedValue == "Exchange Offer(Active)")
        {
            if (totalAmnt >= 15000)
            {
                int availPercent = (totalAmnt / 100) * 15;
                if (availPercent > 1500) { availAmount = 1500; }
                else { availAmount = availPercent; }
            }
        }
        else if (ddlReference.SelectedValue == "Exchange Offer(In-Active)")
        {
            if (totalAmnt >= 15000)
            {
                int availPercent = (totalAmnt / 100) * 10;
                if (availPercent > 1000) { availAmount = 1000; }
                else { availAmount = availPercent; }
            }
        }
        else if (ddlReference.SelectedValue == "LG EXPO GIFT VOUCHER")
        {
            var list = new[]
            {
                new { OfferPrice = 50900,Discount=2000, Model = "43UN731C" },
                new { OfferPrice = 79900,Discount=2000, Model = "50UP7750" },
                new { OfferPrice = 109900,Discount=3000,  Model = "55UN731C" },
                new { OfferPrice = 159900,Discount=4000, Model = "65UN731C" },
                new { OfferPrice = 194900,Discount=5000, Model = "55NANO75" },
                new { OfferPrice = 299900,Discount=7000,  Model = "65NANO86" },
                new { OfferPrice = 339900,Discount=6000, Model = "75NANO75" },
                new { OfferPrice = 449900,Discount=9000,  Model = "86NANO75" },
                new { OfferPrice = 224900,Discount=5000,  Model = "OLED55C1" },
                new { OfferPrice = 384900,Discount=8000,  Model = "OLED65C1" },
                new { OfferPrice = 374900,Discount=8000,  Model = "OLEC65A1" },
                new { OfferPrice = 649900,Discount=10000,  Model = "OLED77C1" },
                new { OfferPrice = 269900,Discount=10000, Model = "GS-Q6472NS" },
                new { OfferPrice = 229900,Discount=5000,  Model = "GS-B6432WB" },
                new { OfferPrice = 89900,Discount=2000,  Model = "2B502HXHL" },
                new { OfferPrice = 83900,Discount=2000,  Model = "2B432HLHL" },
                new { OfferPrice = 79900,Discount=2000,  Model = "FV1409S4W" },
                new { OfferPrice = 99900,Discount=3000,  Model = "FV1409H3W" },
                new { OfferPrice = 37990,Discount=1000,  Model = "WW-172EP" },
                new { OfferPrice = 29990,Discount=1000,  Model = "WW-151NP" },
                new { OfferPrice = 25990,Discount=1000,  Model = "WW-140NP" },
                new { OfferPrice = 16900,Discount=750,  Model = "MH-6565DIS" },
                new { OfferPrice = 24900,Discount=1000,  Model = "MS-2336GIB" },
                new { OfferPrice = 10900,Discount=750,  Model = "HBS-FN7/B,W-10" },
                new { OfferPrice = 9900, Discount=500, Model = "HBS-FN6/B,W-9" },              
            }.ToList();

            dTAmnt = 0;
            int totalDiscount = 0;
            if (this.txtNetAmnt.Text == "")
            {
                dTAmnt = 0;
            }
            else
            {
                dTAmnt = Convert.ToDouble(this.txtNetAmnt.Text);
            }

            foreach (GridViewRow g1 in this.gvUsers.Rows)
            {

                foreach (var item in list)
                {
                    if (g1.Cells[1].Text == item.Model)
                    {
                        if (Convert.ToInt32(g1.Cells[6].Text) > 0)
                        {
                            dTAmnt = dTAmnt + Convert.ToInt32(g1.Cells[6].Text);
                        }
                        int qty = Convert.ToInt32(g1.Cells[4].Text);
                        g1.Cells[6].Text = (item.Discount * qty).ToString();
                        g1.Cells[10].Text = (Convert.ToInt32(g1.Cells[3].Text) * qty - (item.Discount * qty)).ToString();
                        g1.Cells[8].Text = "LG EXPO GIFT VOUCHER";
                        g1.Cells[12].Text = "LG EXPO GIFT VOUCHER";
                        g1.Cells[9].Text = "0";

                        dTAmnt = dTAmnt - (item.Discount * qty);
                    }
                }
                totalDiscount += Convert.ToInt32(g1.Cells[6].Text);
            }
            this.gvUsers.FooterRow.Cells[10].Text = dTAmnt.ToString();
            this.gvUsers.FooterRow.Cells[6].Text = totalDiscount.ToString();
            this.txtNetAmnt.Text = dTAmnt.ToString();
            this.txtCash.Text = dTAmnt.ToString();
            this.txtPay.Text = dTAmnt.ToString();

            ddlReference.Enabled = false;
            btnAvailOffer.Enabled = false;
            btnRedeem.Enabled = false;
        }
        else if (ddlReference.SelectedValue == "Nagad/Bkash")
        {
            if (totalAmnt > 0)
            {
                int availPercent = (totalAmnt / 100) * 10;
                if (availPercent > 1500) { availAmount = 1500; }
                else { availAmount = availPercent; }
            }
        }
        else if (ddlReference.SelectedValue == "LG TV EXPO 5%")
        {
            if (totalAmnt >= 0)
            {
                int availPercent = (totalAmnt / 100) * 5;
                //if (availPercent > 1000) { availAmount = 1000; }
                //else { availAmount = availPercent; }

                availAmount = availPercent;
                //availAmount = txt;
            }
        }
        //else if (ddlReference.SelectedValue == "Freedom Fighter")
        //{
        //    int availPercent = (totalAmnt / 100) * 5;
        //    availAmount = availPercent;
        //}
        //else
        //{
        //    int availPercent = (totalAmnt / 100) * 2;
        //    availAmount = availPercent;
        //}

        txtAvailAmount.Text = Convert.ToString(availAmount);
    }

    protected void btnAvailOffer_Click(object sender, EventArgs e)
    {
        if (txtAvailAmount.Text == "")
        {
            //PopupMessage("Please avail the offer first.", btnGenerateCoupon);
            //btnGenerateCoupon.Focus();
            return;
        }
        try
        {
            int availAmnt = 0; double dTAmnt = 0;
            if (this.txtNetAmnt.Text == "")
            {
                dTAmnt = 0;
            }
            else
            {
                dTAmnt = Convert.ToDouble(this.txtNetAmnt.Text);
            }
            availAmnt = Convert.ToInt16(this.txtAvailAmount.Text);
            dTAmnt = dTAmnt - availAmnt;

            this.txtNetAmnt.Text = dTAmnt.ToString();
            this.txtCash.Text = dTAmnt.ToString();
            this.txtPay.Text = dTAmnt.ToString();

            ddlReference.Enabled = false;
            btnAvailOffer.Enabled = false;
            btnRedeem.Enabled = false;
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    protected void LoadDropDownList_CTP()
    {
        SqlConnection conn = DBConnection.GetConnection();

        String strQuery = "Select EID,eName from Entity ";
        strQuery = strQuery + " WHERE (ActiveDeactive = 1) AND";
        strQuery = strQuery + " (EntityType = 'showroom' OR  EntityType = 'zone'";
        strQuery = strQuery + " OR  EntityType = 'Dealer')";
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
            ddlEntity.Items.Insert(0, new ListItem("", "0"));
            ddlEntity.Items.Insert(1, new ListItem("CI&DD (REL)", "370"));

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

    protected void fnLoadTC()
    {
        SqlConnection con = DBConnection.GetConnection();

        string sSql = "";
        sSql = "SELECT * FROM tbTermsCondition";

        SqlCommand cmd = new SqlCommand(sSql, con);
        con.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        try
        {
            if (dr.Read())
            {
                txtTC.Text = dr["TermsCondition"].ToString();
            }
            else
            {
                txtTC.Text = "";
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

    protected void fnLoadAutoBillNo()
    {

        SqlConnection con = DBConnection.GetConnection();
        //con.Open();

        int xMax = 0;
        string sAutoNo = "";
        string sSql = "";
        //sSql =
        //    "SELECT ISNULL(MAX(RIGHT(MRSRCode, 5)), 0) AS BillNo" +
        //    //"SELECT COUNT(MRSRCode) AS BillNo" +
        //    " FROM dbo.MRSRMaster" +
        //    " WHERE (LEFT(MRSRCode, 12) = '" + "" + Session["sBrCode"] + "/" + DateTime.Now.ToString("yyyy") + "/" + "')" +
        //    " AND TrType=3 AND OnLineSales=1";
        sSql =
            "SELECT max(cast(isnull(RIGHT(MRSRCode,5),0) as int)) AS BillNo" +
            //"SELECT COUNT(MRSRCode) AS BillNo" +
            " FROM dbo.MRSRMaster" +
            " WHERE (LEFT(MRSRCode, 12) = '" + "" + Session["sBrCode"] + "/" + DateTime.Now.ToString("yyyy") + "/" + "')" +
            " AND TrType=3 AND OnLineSales=1";

        SqlCommand cmd = new SqlCommand(sSql, con);
        con.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        try
        {
            if (dr.Read())
            {
                //xMax = dr["JobNo"].ToString();
                if (string.IsNullOrEmpty(dr["BillNo"].ToString()))
                {
                    xMax = 1;
                }
                else
                {
                    xMax = Convert.ToInt32(dr["BillNo"]) + 1;
                }
                //xMax = Convert.ToInt32(dr["BillNo"]);
                sAutoNo = "" + Session["sBrCode"] + "/" + DateTime.Now.ToString("yyyy") + "/" + xMax.ToString("00000");
                //txtCHNo.Text = sAutoNo;
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

        try
        {
            string sSql1 = "";
            sSql1 =
                "SELECT CASE WHEN EXISTS (SELECT  * FROM   dbo.MRSRMaster WHERE  MRSRCode = '" + sAutoNo + "' AND TrType=3 AND OnLineSales=1)" +
                " THEN (Select COUNT(MRSRCode) + 1 AS BillNo from dbo.MRSRMaster WHERE (LEFT(MRSRCode, 12) = LEFT('" + sAutoNo + "', 12)) AND TrType=3 AND OnLineSales=1)" +
                " ELSE CAST(ISNULL(MAX(RIGHT('" + sAutoNo + "', 5)), 0) as int) END  BillNo";

            SqlCommand cmd1 = new SqlCommand(sSql1, con);
            con.Open();

            SqlDataReader dr1 = cmd1.ExecuteReader();

            if (dr1.Read())
            {
                var max = dr1["BillNo"].ToString();
                xMax = Convert.ToInt16(max);
                sAutoNo = "" + Session["sBrCode"] + "/" + DateTime.Now.ToString("yyyy") + "/" + xMax.ToString("00000");
                txtCHNo.Text = sAutoNo;
            }
            else
            {
                dr1.Dispose();
                dr1.Close();
                con.Close();
            }
        }
        catch (Exception ex) { throw ex; }

    }

    //LOAD PRODUCT IN DROPDOWN LIST
    protected void LoadDropDownList()
    {
        if (System.Convert.ToInt32(Session["Vis"]) == 0)
        {
            Response.Redirect("../Account/Login.aspx");
        }

        SqlConnection conn = DBConnection.GetConnection();
        String strQuery = "select Model from Product WHERE Discontinue='No' Order By Model";//and Model!='KSV-18BDINV (1.5 ton Inverter)'
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

    //LOAD CITY IN DROPDOWN LIST
    protected void LoadDropDownList_City()
    {
        if (System.Convert.ToInt32(Session["Vis"]) == 0)
        {
            Response.Redirect("../Account/Login.aspx");
        }

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

    protected void MakeTable()
    {
        //dt.Columns.Add("ID").AutoIncrement = true;
        dt.Columns.Add("ProductID");
        //dt.Columns.Add("ProductID", typeof(SqlInt32));
        dt.Columns.Add("Model");
        dt.Columns.Add("MRP");
        dt.Columns.Add("CampaignPrice");
        dt.Columns.Add("Qty");
        dt.Columns.Add("TotalPrice");
        dt.Columns.Add("DisAmnt");
        dt.Columns.Add("DisCode");
        dt.Columns.Add("DisRef");
        dt.Columns.Add("WithAdjAmnt");
        dt.Columns.Add("NetAmnt");
        dt.Columns.Add("ProductSL");
        dt.Columns.Add("Remarks");

        dt.Columns.Add("BLIPAmnt");
        dt.Columns.Add("IncentiveAmnt");
        dt.Columns.Add("IncentiveType");
        dt.Columns.Add("CustShowPrice");

    }

    protected void PopupMessage(string Msg, Control controlID)
    {
        ScriptManager.RegisterClientScriptBlock(controlID, controlID.GetType(), "msg", "alert('" + Msg + "');", true);
    }
    public void modelwiseproductinfo(string model)
    {

        SqlConnection conn = DBConnection.GetConnection();
        double UP = 0;
        double dBLIP = 0;
        double CampPrice = 0;
        string sSql = "";

        //p.title Model
        string tempStr = model;
        int pos = tempStr.IndexOf(" (");
        if (pos >= 0)
        {
            // String after founder  
            model = tempStr.Remove(pos);
            //Console.WriteLine(afterFounder);
            // Remove everything before founder but include founder  
            //string beforeFounder = founder.Remove(0, pos);
            //Console.Write(beforeFounder);
        }

        sSql = "";
        sSql = "SELECT ProductID,ProdName,UnitPrice,Model,Code,";
        sSql = sSql + " IncentiveType,BLIPAmnt,IncentiveAmnt,GroupName, ";
        sSql = sSql + " GetIncentive, ISNULL(WPPrice,0) AS WPPrice, ISNULL(BLIPofWP,0) AS BLIPofWP,";
        sSql = sSql + " ISNULL(WPIncentive,0) AS WPIncentive, ISNULL(WPMinQty,0) AS WPMinQty";
        sSql = sSql + " FROM Product";
        sSql = sSql + " WHERE Model like '%" + model + "%'";

        SqlCommand cmd = new SqlCommand(sSql, conn);
        conn.Open();
        SqlDataReader dr = cmd.ExecuteReader();

        try
        {
            if (dr.Read())
            {
                this.txtCode.Text = dr["Code"].ToString();
                this.ddlContinents.SelectedItem.Text = dr["Model"].ToString();
                this.txtProdID.Text = dr["ProductID"].ToString();
                this.txtProdDesc.Text = dr["ProdName"].ToString();
                UP = Convert.ToDouble(dr["UnitPrice"].ToString());
                this.txtUP.Text = Convert.ToString(UP);
                dBLIP = Convert.ToDouble(dr["BLIPAmnt"].ToString());
                this.txtBLIPAmnt.Text = Convert.ToString(dBLIP);
                this.GroupName.Text = dr["GroupName"].ToString();
                this.lblIncentiveType.Text = dr["IncentiveType"].ToString();
                this.lblBLIPAmnt.Text = dr["BLIPAmnt"].ToString();
                this.lblIncentiveAmnt.Text = dr["IncentiveAmnt"].ToString();
                this.lblGetIncentive.Text = dr["GetIncentive"].ToString();
                this.lblWPPrice.Text = dr["WPPrice"].ToString();
                this.lblBLIPofWP.Text = dr["BLIPofWP"].ToString();
                this.lblWPIncentive.Text = dr["WPIncentive"].ToString();
                this.lblWPMinQty.Text = dr["WPMinQty"].ToString();
            }
            else
            {
                UP = 0;
                dBLIP = 0;
                this.txtUP.Text = Convert.ToString(UP);
                this.txtCode.Text = "";
                this.ddlContinents.SelectedItem.Text = "";
                this.txtProdID.Text = "";
                this.txtProdDesc.Text = "";

                this.lblIncentiveType.Text = "";
                this.lblBLIPAmnt.Text = "0";
                this.lblIncentiveAmnt.Text = "0";
                this.lblGetIncentive.Text = "0";
                this.lblWPPrice.Text = "0";
                this.lblBLIPofWP.Text = "0";
                this.lblWPIncentive.Text = "0";
                this.lblWPMinQty.Text = "0";

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

            " WHERE Model like '%" + model + "%'" +
            " AND (EffectiveDate<='" + DateTime.Today + "' AND cStop=0)" +
            " ORDER BY EffectiveDate DESC";
        //sSql = "SELECT TOP 1 ProductID,Model,DisAmnt " +
        //    " FROM VW_CampaignInfo" +
        //    " WHERE Model='" + txtModel.Text + "'" +
        //    " AND (EffectiveDate<='" + DateTime.Today + "' AND cStop=0)" +
        //    " ORDER BY EffectiveDate DESC";
        cmd = new SqlCommand(sSql, conn);
        conn.Open();
        dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            CampPrice = UP - Convert.ToDouble(dr["DisAmnt"].ToString());
            this.txtCP.Text = Convert.ToString(CampPrice);
        }
        else
        {
            CampPrice = UP;
            this.txtCP.Text = Convert.ToString(CampPrice);
        }
        dr.Dispose();
        dr.Close();
        conn.Close();

    }
    protected void AddGiftItemToRowsRows(string model, int qty)
    {
        //---this btnAvailOffer is stoped--
        model = "";
        qty = 0;
        ///////////////////////////////////
        modelwiseproductinfo(model);
        DataRow dr = dt.NewRow();
        dr["ProductID"] = txtProdID.Text;
        //dr["Model"] = ddlContinents.Text; //Model
        dr["Model"] = ddlContinents.SelectedItem.Text;
        dr["MRP"] = txtUP.Text;
        dr["CampaignPrice"] = txtCP.Text;
        //if (Convert.ToInt16(txtQty.Text) >= Convert.ToInt16(this.lblWPMinQty.Text))
        //{
        //    dr["CampaignPrice"] = lblWPPrice.Text;
        //}
        //else
        //{
        //    dr["CampaignPrice"] = txtCP.Text;
        //}
        dr["Qty"] = qty.ToString();
        dr["TotalPrice"] = "0";
        dr["DisAmnt"] = "0";
        //dr["DisCode"] = txtDisCode.Text;


        //String[] blockedForBkashNagadGp = new String[] { "GS-X6172NS","GS-Q6472NS",
        //        "GS-B6432WB" };
        //if (blockedForBkashNagadGp.Contains(ddlContinents.SelectedItem.Text))
        //{
        //    this.btnAvailOffer.Enabled = false;
        //}


        dr["DisRef"] = ddlRefDiscount.SelectedValue;
        dr["WithAdjAmnt"] = "0";
        dr["NetAmnt"] = "0";
        dr["ProductSL"] = txtSL.Text;
        //dr["Remarks"] = txtRemarks.Text;

        if (this.lblWPMinQty.Text.Length == 0)
        {
            this.lblWPMinQty.Text = "0";
        }

        if (Convert.ToInt16(this.lblWPMinQty.Text) > 0)
        {
            if (Convert.ToInt16(txtQty.Text) >= Convert.ToInt16(this.lblWPMinQty.Text))
            {
                dr["BLIPAmnt"] = lblBLIPofWP.Text;
                dr["IncentiveAmnt"] = lblWPIncentive.Text;
                dr["CampaignPrice"] = lblWPPrice.Text;
            }
            else
            {
                dr["BLIPAmnt"] = lblBLIPAmnt.Text;
                dr["IncentiveAmnt"] = lblIncentiveAmnt.Text;
                dr["CampaignPrice"] = txtCP.Text;
            }
        }
        else
        {
            dr["BLIPAmnt"] = lblBLIPAmnt.Text;
            dr["IncentiveAmnt"] = lblIncentiveAmnt.Text;
        }
        dr["IncentiveType"] = lblIncentiveType.Text;
        dr["CustShowPrice"] = lblUP.Text;

        //dr["CampDis"] = dr["MRP"].ToString() - dr["CampaignPrice"].ToString();
        dt.Rows.Add(dr);



        txtProdID.Text = "";
        txtProdDesc.Text = "";
        txtUP.Text = "";
        txtCP.Text = "";

        txtTotalAmnt.Text = "";
        txtDisAmnt.Text = "";
        //txtDisCode.Text = "";
        //ddlRefDiscount.SelectedItem.Text = "";
        txtWithAdj.Text = "";
        txtNet.Text = "";
        txtSL.Text = "";
        //txtRemarks.Text = "";

        lblBLIPAmnt.Text = "0";
        lblIncentiveAmnt.Text = "0";
        lblIncentiveType.Text = "";
        lblUP.Text = "0";
        txtCode.Text = "";
        txtProdDesc.Text = "";
        txtProdID.Text = "0";
        txtModel.Text = "";
        txtModel.Focus();
        // Load Ref.Discount
        txtOnlineOrder.Text = "";
        ddlContinents.SelectedItem.Text = "";
        txtQty.Text = "";
        LoadDiscountReferenceList();
    }
    protected void AddRows()
    {
        if (System.Convert.ToInt32(Session["Vis"]) == 0)
        {
            Response.Redirect("../Account/Login.aspx");
        }

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

        // Check Discount Reference
        if (Convert.ToInt32(txtDisAmnt.Text) != 0)
        {
            if (ddlRefDiscount.SelectedValue == "0")
            {
                PopupMessage("Please select Reference name.", btnAdd);
                ddlRefDiscount.Focus();
                return;
            }
            else if (ddlRefDiscount.SelectedValue == "Online Order" && txtOnlineOrder.Text == "")
            {
                PopupMessage("Please enter Online Order No.", btnAdd);
                txtOnlineOrder.Focus();
                return;
            }
            else if (ddlRefDiscount.SelectedValue == "Cash Voucher" && txtOnlineOrder.Text == "")
            {
                PopupMessage("Please enter cash voucher code.", btnAdd);
                txtOnlineOrder.Focus();
                return;
            }
            else if (ddlRefDiscount.SelectedValue == "Bank Voucher" && txtOnlineOrder.Text == "")
            {
                PopupMessage("Please enter bank voucher code.", btnAdd);
                txtOnlineOrder.Focus();
                return;
            }
            else if (ddlRefDiscount.SelectedValue == "GP Star Coupon" && txtOnlineOrder.Text == "")
            {
                PopupMessage("Please enter GP Star Coupon.", btnAdd);
                txtOnlineOrder.Focus();
                return;
            }
            //else if (ddlRefDiscount.SelectedValue == "T20 Scratch Card" && txtOnlineOrder.Text == "")
            //{
            //    PopupMessage("Please enter T20 Scratch Card Coupon.", btnAdd);
            //    txtOnlineOrder.Focus();
            //    return;
            //}
            else if (ddlRefDiscount.SelectedValue == "GM Sir")
            {
                PopupMessage("A confirmation SMS sent to Honorable GM Sir for this challan.", ddlRefDiscount);
            }
            else if (ddlRefDiscount.SelectedValue == "DGM Sir")
            {
                PopupMessage("A confirmation SMS sent to Honorable DGM Sir for this challan.", ddlRefDiscount);
                //throw new HttpException(404, "");
            }
            else if (ddlRefDiscount.SelectedValue == "Nipa Madam")
            {
                PopupMessage("A confirmation SMS sent to Honorable Nipa Madam for this challan.", ddlRefDiscount);
            }
        }

        Data.MainData items = Newshopapi(txtOnlineOrder.Text.Trim().ToString(), 0);

        if (txtOnlineOrder.Text != "" && ddlRefDiscount.SelectedValue == "Online Order" && items.data != null)
        {
            string orderNo = txtOnlineOrder.Text.Trim().ToString();

            if (txtOnlineOrder.Text.Trim().Length != 14)
            {
                
            }
            //Data.MainData items = Newshopapi(txtOnlineOrder.Text);
            //1208202200020
            if (items != null)
            {
                foreach (var item in items.data.products)
                {
                    modelwiseproductinfo(item.product_model);
                    DataRow dr1 = dt.NewRow();
                    dr1["ProductID"] = txtProdID.Text;
                    //dr["Model"] = ddlContinents.Text; //Model
                    dr1["Model"] = item.product_model;
                    dr1["MRP"] = txtUP.Text;
                    dr1["CampaignPrice"] = txtCP.Text;
                    //if (Convert.ToInt16(txtQty.Text) >= Convert.ToInt16(this.lblWPMinQty.Text))
                    //{
                    //    dr["CampaignPrice"] = lblWPPrice.Text;
                    //}
                    //else
                    //{
                    //    dr["CampaignPrice"] = txtCP.Text;
                    //}
                    dr1["Qty"] = item.order_quantity.ToString();

                    //for ebl offer
                    if (chkTblOffer.Checked)
                    {
                        dr1["TotalPrice"] = (Convert.ToInt32(txtCP.Text) * Convert.ToInt32(item.order_quantity)).ToString();
                        string dicountAmnt = (Convert.ToInt32(dr1["TotalPrice"]) - Convert.ToInt32(item.subtotal)).ToString();
                        if (item.subtotal > 10000) 
                        {

                            decimal discountAmntDecimal = Convert.ToDecimal(item.subtotal) * 0.05m;

                            if (Convert.ToInt32(discountAmntDecimal) > 6000)
                            {
                                discountAmntDecimal = 6000;
                                dr1["DisAmnt"] = (Convert.ToInt32(dicountAmnt) + discountAmntDecimal).ToString();
                               
                            }
                            else 
                            {
                                dr1["DisAmnt"] = (Convert.ToInt32(dicountAmnt) + Convert.ToInt32(discountAmntDecimal)).ToString();
                            }

                            if (txtOnlineOrder.Text != "")
                            {
                                dr1["DisCode"] = orderNo;
                            }
                            dr1["DisRef"] = "Online Order + Ebl 5%=" + discountAmntDecimal.ToString();
                            dr1["WithAdjAmnt"] = 0;
                            dr1["NetAmnt"] = (Convert.ToInt32(item.subtotal) - Convert.ToInt32(discountAmntDecimal)).ToString();
                        }
                        



                    }
                    else 
                    {
                        dr1["TotalPrice"] = (Convert.ToInt32(txtCP.Text) * Convert.ToInt32(item.order_quantity)).ToString();
                        dr1["DisAmnt"] = (Convert.ToInt32(dr1["TotalPrice"]) - Convert.ToInt32(item.subtotal)).ToString();
                        //dr["DisCode"] = txtDisCode.Text;

                        if (txtOnlineOrder.Text != "")
                        {
                            dr1["DisCode"] = orderNo;
                        }
                        dr1["DisRef"] = "Online Order";
                        dr1["WithAdjAmnt"] = 0;
                        // dr1["DeliveryCost"] = Convert.ToInt32(items.data.delivery_fee).ToString();
                        dr1["NetAmnt"] = Convert.ToInt32(item.subtotal).ToString();

                    }
                   

                    dr1["ProductSL"] = txtSL.Text;
                    //dr["Remarks"] = txtRemarks.Text;

                    if (this.lblWPMinQty.Text.Length == 0)
                    {
                        this.lblWPMinQty.Text = "0";
                    }

                    if (Convert.ToInt16(this.lblWPMinQty.Text) > 0)
                    {
                        if (txtQty.Text == "")
                        {
                            txtQty.Text = "0";
                        }
                        if (Convert.ToInt16(txtQty.Text) >= Convert.ToInt16(this.lblWPMinQty.Text))
                        {
                            dr1["BLIPAmnt"] = lblBLIPofWP.Text;
                            dr1["IncentiveAmnt"] = lblWPIncentive.Text;
                            dr1["CampaignPrice"] = lblWPPrice.Text;
                        }
                        else
                        {
                            dr1["BLIPAmnt"] = lblBLIPAmnt.Text;
                            dr1["IncentiveAmnt"] = lblIncentiveAmnt.Text;
                            dr1["CampaignPrice"] = txtCP.Text;
                        }
                    }
                    else
                    {
                        dr1["BLIPAmnt"] = lblBLIPAmnt.Text;
                        dr1["IncentiveAmnt"] = lblIncentiveAmnt.Text;
                    }
                    dr1["IncentiveType"] = lblIncentiveType.Text;
                    dr1["CustShowPrice"] = lblUP.Text;
                    dt.Rows.Add(dr1);
                    txtProdID.Text = "";
                    txtProdDesc.Text = "";
                    txtUP.Text = "";
                    txtCP.Text = "";
                    txtQty.Text = "";
                    txtTotalAmnt.Text = "";
                    txtDisAmnt.Text = "";
                    //txtDisCode.Text = "";
                    //ddlRefDiscount.SelectedItem.Text = "";
                    txtWithAdj.Text = "";
                    txtNet.Text = "";
                    txtSL.Text = "";
                    //txtRemarks.Text = "";

                    lblBLIPAmnt.Text = "0";
                    lblIncentiveAmnt.Text = "0";
                    lblIncentiveType.Text = "";
                    lblUP.Text = "0";

                    ddlContinents.SelectedItem.Text = "";
                    txtCode.Text = "";
                    txtProdDesc.Text = "";
                    txtProdID.Text = "0";
                    txtModel.Text = "";
                    txtModel.Focus();
                    // Load Ref.Discount
                    //txtOnlineOrder.Text = "";
                    LoadDiscountReferenceList();

                    //CustContact,CustEmail,CustName
                    txtCustName.Text = items.data.customer_name.ToString();
                    txtCustContact.Text = items.data.customer_mobile.ToString().Trim();

                    if (items.data.customer_email != null)
                    {
                        txtEmail.Text = items.data.customer_email.ToString();
                        this.txtEmail.Enabled = false;
                    }
                    else
                    {
                        txtEmail.Text = "";
                    }

                    if (items.data.billing_address != null)
                    {

                        txtCustAdd.Text = items.data.billing_address.address_one.ToString();
                    }
                    else
                    {
                        txtCustAdd.Text = "";
                    }

                   
                    //this.ddlContinents.Enabled = false;
                    //this.txtCode.Enabled = false;
                    //this.txtModel.Enabled = false;
                    this.txtOrderNo.Enabled = false;
                    this.txtOnlineOrder.Enabled = false;
                    this.txtCustName.Enabled = false;
                    this.txtCustContact.Enabled = false;

                    this.btnCustSearch.Enabled = false;
                    lblDelivary_charge.Visible = true;
                    lblDelivary_charge.Text = "Shipping Charge = "+ViewState["__DelivariCharge__"].ToString();
                }

            }
        }
        //else if (txtOnlineOrder.Text != "" && ddlRefDiscount.SelectedValue == "Online Order" && txtOnlineOrder.Text.Trim().Length == 13)
        //{
        //    string orderNo = txtOnlineOrder.Text.Trim().ToString();
        //    //1208202200020
        //    SqlConnection connRos = DBConnection_ROS.GetConnection();//CustContact,CustEmail,CustName
        //    string sSql = "";
        //    sSql = "select det.ProductID ProductID,p.title Model,det.tQty tQty,det.SalePrice SalePrice,det.tAmnt tAmnt,main.CustContact CustContact,main.DelAddress,main.CustEmail CustEmail,main.CustName CustName,main.CartTotal CartTotal,main.TotalQty TotalQty from tbCustomerDelivery as main " +
        //    " inner join tbCustomerDelDetails det on main.DelID=det.DelID" +
        //    " inner join tbProduct p on det.ProductID=p.ProductID" +
        //    " WHERE main.DelNo='" + txtOnlineOrder.Text.Trim() + "'";
        //    SqlCommand cmdRR = new SqlCommand(sSql, connRos);
        //    connRos.Open();
        //    SqlDataReader shopdr = cmdRR.ExecuteReader();

        //    while (shopdr.Read())
        //    {
        //        modelwiseproductinfo(shopdr["Model"].ToString());
        //        DataRow dr1 = dt.NewRow();
        //        dr1["ProductID"] = txtProdID.Text;
        //        //dr["Model"] = ddlContinents.Text; //Model
        //        dr1["Model"] = shopdr["Model"].ToString();
        //        dr1["MRP"] = txtUP.Text;
        //        dr1["CampaignPrice"] = txtCP.Text;
        //        //if (Convert.ToInt16(txtQty.Text) >= Convert.ToInt16(this.lblWPMinQty.Text))
        //        //{
        //        //    dr["CampaignPrice"] = lblWPPrice.Text;
        //        //}
        //        //else
        //        //{
        //        //    dr["CampaignPrice"] = txtCP.Text;
        //        //}
        //        dr1["Qty"] = shopdr["tQty"].ToString();
        //        dr1["TotalPrice"] = (Convert.ToInt32(txtCP.Text) * Convert.ToInt32(shopdr["tQty"])).ToString();
        //        dr1["DisAmnt"] = (Convert.ToInt32(dr1["TotalPrice"]) - Convert.ToInt32(shopdr["tAmnt"])).ToString();
        //        //dr["DisCode"] = txtDisCode.Text;
        //        if (txtOnlineOrder.Text != "")
        //        {
        //            dr1["DisCode"] = orderNo;
        //        }
        //        dr1["DisRef"] = "Online Order";
        //        dr1["WithAdjAmnt"] = 0;
        //        dr1["NetAmnt"] = Convert.ToInt32(shopdr["tAmnt"]).ToString();
        //        dr1["ProductSL"] = txtSL.Text;
        //        //dr["Remarks"] = txtRemarks.Text;

        //        if (this.lblWPMinQty.Text.Length == 0)
        //        {
        //            this.lblWPMinQty.Text = "0";
        //        }

        //        if (Convert.ToInt16(this.lblWPMinQty.Text) > 0)
        //        {
        //            if (Convert.ToInt16(txtQty.Text) >= Convert.ToInt16(this.lblWPMinQty.Text))
        //            {
        //                dr1["BLIPAmnt"] = lblBLIPofWP.Text;
        //                dr1["IncentiveAmnt"] = lblWPIncentive.Text;
        //                dr1["CampaignPrice"] = lblWPPrice.Text;
        //            }
        //            else
        //            {
        //                dr1["BLIPAmnt"] = lblBLIPAmnt.Text;
        //                dr1["IncentiveAmnt"] = lblIncentiveAmnt.Text;
        //                dr1["CampaignPrice"] = txtCP.Text;
        //            }
        //        }
        //        else
        //        {
        //            dr1["BLIPAmnt"] = lblBLIPAmnt.Text;
        //            dr1["IncentiveAmnt"] = lblIncentiveAmnt.Text;
        //        }
        //        dr1["IncentiveType"] = lblIncentiveType.Text;
        //        dr1["CustShowPrice"] = lblUP.Text;

        //        //dr["CampDis"] = dr["MRP"].ToString() - dr["CampaignPrice"].ToString();

        //        dt.Rows.Add(dr1);
        //        txtProdID.Text = "";
        //        txtProdDesc.Text = "";
        //        txtUP.Text = "";
        //        txtCP.Text = "";
        //        txtQty.Text = "";
        //        txtTotalAmnt.Text = "";
        //        txtDisAmnt.Text = "";
        //        //txtDisCode.Text = "";
        //        //ddlRefDiscount.SelectedItem.Text = "";
        //        txtWithAdj.Text = "";
        //        txtNet.Text = "";
        //        txtSL.Text = "";
        //        //txtRemarks.Text = "";

        //        lblBLIPAmnt.Text = "0";
        //        lblIncentiveAmnt.Text = "0";
        //        lblIncentiveType.Text = "";
        //        lblUP.Text = "0";

        //        ddlContinents.SelectedItem.Text = "";
        //        txtCode.Text = "";
        //        txtProdDesc.Text = "";
        //        txtProdID.Text = "0";
        //        txtModel.Text = "";
        //        txtModel.Focus();
        //        // Load Ref.Discount

        //        LoadDiscountReferenceList();

        //        //CustContact,CustEmail,CustName
        //        txtCustName.Text = shopdr["CustName"].ToString();
        //        txtCustContact.Text = shopdr["CustContact"].ToString().Trim();
        //        txtEmail.Text = shopdr["CustEmail"].ToString();
        //        txtCustAdd.Text = shopdr["DelAddress"].ToString();

        //        //this.ddlContinents.Enabled = false;
        //        //this.txtCode.Enabled = false;
        //        //this.txtModel.Enabled = false;
        //        this.txtOrderNo.Enabled = false;
        //        this.txtOnlineOrder.Enabled = false;
        //        this.txtCustName.Enabled = false;
        //        this.txtCustContact.Enabled = false;
        //        this.txtEmail.Enabled = false;
        //        this.btnCustSearch.Enabled = false;
        //    }

        //    txtOnlineOrder.Text = "";
        //}
        else if (ddlRefDiscount.SelectedValue.Contains("Package"))
        {
            var discountListOnPackage = new[]
            {
                new { Package="Package-01",SpecialPrice=21900,PackagePrice=19200,Discount=15000, Model = "RL-32K405" },
                new { Package="Package-01",SpecialPrice=35500,PackagePrice=35500,Discount=15000, Model = "RR-221BD" },
                new { Package="Package-01",SpecialPrice=2990,PackagePrice=2990,Discount=15000,  Model = "RB-96" },
                new { Package="Package-01",SpecialPrice=0,PackagePrice=1490,Discount=15000, Model = "GC090" },
                
                new { Package="Package-02",SpecialPrice=34900,PackagePrice=34900,Discount=15000, Model = "32LQ636B" },
                new { Package="Package-02",SpecialPrice=43900,PackagePrice=43900,Discount=15000, Model = "RR-275BD" },
                new { Package="Package-02",SpecialPrice=12500,PackagePrice=12500,Discount=15000,  Model = "KMW-20" },
                new { Package="Package-02",SpecialPrice=0,PackagePrice=3150,Discount=15000, Model = "RB-96" },

                new { Package="Package-03",SpecialPrice=59900,PackagePrice=59900,Discount=0, Model = "43UP7550" },
                new { Package="Package-03",SpecialPrice=43900,PackagePrice=43900,Discount=15000, Model = "RR-275BD" },
                new { Package="Package-03",SpecialPrice=18500,PackagePrice=18500,Discount=15000,  Model = "KMW-31CS" },
                new { Package="Package-03",SpecialPrice=0,PackagePrice=2990,Discount=15000, Model = "RB-96" },

                new { Package="Package-04",SpecialPrice=75900,PackagePrice=75900,Discount=15000, Model = "KD-43X75K" },
                new { Package="Package-04",SpecialPrice=68900,PackagePrice=68900,Discount=15000, Model = "KHV-428NFIN" },
                new { Package="Package-04",SpecialPrice=34900,PackagePrice=34900,Discount=15000,  Model = "KWM-KT8222GDS" },
                new { Package="Package-04",SpecialPrice=41400,PackagePrice=41400,Discount=15000, Model = "KSV-12NVBD" },
                new { Package="Package-04",SpecialPrice=0,PackagePrice=12500,Discount=15000, Model = "KMW-20" },

                new { Package="Package-05",SpecialPrice=109900,PackagePrice=109900,Discount=15000, Model = "55UN731C" },
                new { Package="Package-05",SpecialPrice=96900,PackagePrice=99900,Discount=15000, Model = "2B502HXHL" },
                new { Package="Package-05",SpecialPrice=46900,PackagePrice=49900,Discount=15000,  Model = "T2311VSAB" },
                new { Package="Package-05",SpecialPrice=21900,PackagePrice=21900,Discount=15000, Model = "MH-6565DIS" },
                new { Package="Package-05",SpecialPrice=0,PackagePrice=5200,Discount=15000, Model = "HD7431/20" },

                new { Package="Package-06",SpecialPrice=129900,PackagePrice=129900,Discount=15000, Model = "KD-55X75K" },
                new { Package="Package-06",SpecialPrice=101500,PackagePrice=101500,Discount=15000, Model = "KHV-422WDNFSBS2D" },
                new { Package="Package-06",SpecialPrice=56900,PackagePrice=56900,Discount=15000,  Model = "EW6F5722BB" },
                new { Package="Package-06",SpecialPrice=12500,PackagePrice=12500,Discount=15000, Model = "KMW-20" },
                new { Package="Package-06",SpecialPrice=52400,PackagePrice=52400,Discount=15000, Model = "KSV-12TPINV-SW" },
                new { Package="Package-06",SpecialPrice=0,PackagePrice=5200,Discount=15000, Model = "HD7431/20" }
                        
            }.ToList();


            foreach (var item in discountListOnPackage)
            {
                if (item.Package == ddlRefDiscount.SelectedValue)
                {
                    modelwiseproductinfo(item.Model);
                    DataRow dr1 = dt.NewRow();
                    dr1["ProductID"] = txtProdID.Text;
                    //dr["Model"] = ddlContinents.Text; //Model
                    dr1["Model"] = item.Model;
                    dr1["MRP"] = txtUP.Text;
                    dr1["CampaignPrice"] = txtCP.Text;
                    //if (Convert.ToInt16(txtQty.Text) >= Convert.ToInt16(this.lblWPMinQty.Text))
                    //{
                    //    dr["CampaignPrice"] = lblWPPrice.Text;
                    //}
                    //else
                    //{
                    //    dr["CampaignPrice"] = txtCP.Text;
                    //}
                    dr1["Qty"] = "1";
                    dr1["TotalPrice"] = (Convert.ToInt32(txtCP.Text) * Convert.ToInt32(1)).ToString();
                    if (Convert.ToInt32(item.PackagePrice) > Convert.ToInt32(txtCP.Text))
                    {
                        dr1["DisAmnt"] = "0";
                    }
                    else {
                        dr1["DisAmnt"] = (Convert.ToInt32(dr1["TotalPrice"]) - Convert.ToInt32(item.PackagePrice)).ToString();
                    }
                    
                    //dr["DisCode"] = txtDisCode.Text;
                    if (txtOnlineOrder.Text != "")
                    {
                        //dr1["DisCode"] = orderNo;
                    }
                    dr1["DisRef"] = item.Package;
                    dr1["WithAdjAmnt"] = 0;
                    dr1["NetAmnt"] = (Convert.ToInt32(dr1["TotalPrice"]) - Convert.ToInt32(dr1["DisAmnt"])).ToString();
                    dr1["ProductSL"] = txtSL.Text;
                    //dr["Remarks"] = txtRemarks.Text;

                    if (this.lblWPMinQty.Text.Length == 0)
                    {
                        this.lblWPMinQty.Text = "0";
                    }

                    if (Convert.ToInt16(this.lblWPMinQty.Text) > 0)
                    {
                        if (Convert.ToInt16(txtQty.Text) >= Convert.ToInt16(this.lblWPMinQty.Text))
                        {
                            dr1["BLIPAmnt"] = lblBLIPofWP.Text;
                            dr1["IncentiveAmnt"] = lblWPIncentive.Text;
                            dr1["CampaignPrice"] = lblWPPrice.Text;
                        }
                        else
                        {
                            dr1["BLIPAmnt"] = lblBLIPAmnt.Text;
                            dr1["IncentiveAmnt"] = lblIncentiveAmnt.Text;
                            dr1["CampaignPrice"] = txtCP.Text;
                        }
                    }
                    else
                    {
                        dr1["BLIPAmnt"] = lblBLIPAmnt.Text;
                        dr1["IncentiveAmnt"] = lblIncentiveAmnt.Text;
                    }
                    dr1["IncentiveType"] = lblIncentiveType.Text;
                    dr1["CustShowPrice"] = lblUP.Text;

                    //dr["CampDis"] = dr["MRP"].ToString() - dr["CampaignPrice"].ToString();

                    dt.Rows.Add(dr1);
                    txtProdID.Text = "";
                    txtProdDesc.Text = "";
                    txtUP.Text = "";
                    txtCP.Text = "";
                    txtQty.Text = "";
                    txtTotalAmnt.Text = "";
                    txtDisAmnt.Text = "";
                    //txtDisCode.Text = "";
                    //ddlRefDiscount.SelectedItem.Text = "";
                    txtWithAdj.Text = "";
                    txtNet.Text = "";
                    txtSL.Text = "";
                    //txtRemarks.Text = "";

                    lblBLIPAmnt.Text = "0";
                    lblIncentiveAmnt.Text = "0";
                    lblIncentiveType.Text = "";
                    lblUP.Text = "0";

                    ddlContinents.SelectedItem.Text = "";
                    txtCode.Text = "";
                    txtProdDesc.Text = "";
                    txtProdID.Text = "0";
                    txtModel.Text = "";
                    txtModel.Focus();
                    // Load Ref.Discount

                    //LoadDiscountReferenceList();

                    //CustContact,CustEmail,CustName
                    //txtCustName.Text = shopdr["CustName"].ToString();
                    //txtCustContact.Text = shopdr["CustContact"].ToString().Trim();
                    //txtEmail.Text = shopdr["CustEmail"].ToString();
                    //txtCustAdd.Text = shopdr["DelAddress"].ToString();

                    //this.ddlContinents.Enabled = false;
                    //this.txtCode.Enabled = false;
                    //this.txtModel.Enabled = false;
                    //this.txtOrderNo.Enabled = false;
                    //this.txtOnlineOrder.Enabled = false;
                    //this.txtCustName.Enabled = false;
                    //this.txtCustContact.Enabled = false;
                    //this.txtEmail.Enabled = false;
                    //this.btnCustSearch.Enabled = false;
                }
            }
            LoadDiscountReferenceList();
        }
        else
        {
            DataRow dr = dt.NewRow();
            dr["ProductID"] = txtProdID.Text;
            //dr["Model"] = ddlContinents.Text; //Model
            dr["Model"] = ddlContinents.SelectedItem.Text;
            dr["MRP"] = txtUP.Text;
            dr["CampaignPrice"] = txtCP.Text;
            //if (Convert.ToInt16(txtQty.Text) >= Convert.ToInt16(this.lblWPMinQty.Text))
            //{
            //    dr["CampaignPrice"] = lblWPPrice.Text;
            //}
            //else
            //{
            //    dr["CampaignPrice"] = txtCP.Text;
            //}
            dr["Qty"] = txtQty.Text;
            dr["TotalPrice"] = txtTotalAmnt.Text;
            dr["DisAmnt"] = txtDisAmnt.Text;
            //dr["DisCode"] = txtDisCode.Text;
            if (txtOnlineOrder.Text != "")
            {
                dr["DisCode"] = txtOnlineOrder.Text;
            }

            //String[] blockedForBkashNagadGp = new String[] { "GS-X6172NS","GS-Q6472NS",
            //        "GS-B6432WB" };
            //if (blockedForBkashNagadGp.Contains(ddlContinents.SelectedItem.Text))
            //{
            //    this.btnAvailOffer.Enabled = false;
            //}
            //if (this.ddlContinents.SelectedItem.Text == "KD-65X80J" || this.ddlContinents.SelectedItem.Text == "KD-75X80J" ||
            //    this.ddlContinents.SelectedItem.Text == "KD-85X8000H" || this.ddlContinents.SelectedItem.Text == "KD-55A80J" ||
            //    this.ddlContinents.SelectedItem.Text == "KD-65A80J" || this.ddlContinents.SelectedItem.Text == "KD-55A80K"
            //    || this.ddlContinents.SelectedItem.Text == "KD-65A80K" || this.ddlContinents.SelectedItem.Text == "GS-Q6472NS"
            //    || this.ddlContinents.SelectedItem.Text == "GS-X6172NS" || this.ddlContinents.SelectedItem.Text == "GS-B6432WB"
            //    )
            //{
            //    //if (this.GroupName.Text == "SONY LCD TV")
            //    //{
            //        this.btnAvailOffer.Enabled = false;
            //    //}
            //}
            if (TxtSpinCouponNumber.Text != "")
            {
                dr["DisCode"] = TxtSpinCouponNumber.Text;
            }
            dr["DisRef"] = ddlRefDiscount.SelectedValue;
            dr["WithAdjAmnt"] = txtWithAdj.Text;
            dr["NetAmnt"] = Convert.ToDouble(txtNet.Text);

            if (TxtSpinCouponNumber.Text != "")
            {
                if (SpinModelProdSLNo.Text != "")
                {
                    txtSL.Text = SpinModelProdSLNo.Text;
                }

            }
            dr["ProductSL"] = txtSL.Text;
            //dr["Remarks"] = txtRemarks.Text;

            if (this.lblWPMinQty.Text.Length == 0)
            {
                this.lblWPMinQty.Text = "0";
            }

            if (Convert.ToInt16(this.lblWPMinQty.Text) > 0)
            {
                if (Convert.ToInt16(txtQty.Text) >= Convert.ToInt16(this.lblWPMinQty.Text))
                {
                    dr["BLIPAmnt"] = lblBLIPofWP.Text;
                    dr["IncentiveAmnt"] = lblWPIncentive.Text;
                    dr["CampaignPrice"] = lblWPPrice.Text;
                }
                else
                {
                    dr["BLIPAmnt"] = lblBLIPAmnt.Text;
                    dr["IncentiveAmnt"] = lblIncentiveAmnt.Text;
                    dr["CampaignPrice"] = txtCP.Text;
                }
            }
            else
            {
                dr["BLIPAmnt"] = lblBLIPAmnt.Text;
                dr["IncentiveAmnt"] = lblIncentiveAmnt.Text;
            }
            dr["IncentiveType"] = lblIncentiveType.Text;
            dr["CustShowPrice"] = lblUP.Text;

            //dr["CampDis"] = dr["MRP"].ToString() - dr["CampaignPrice"].ToString();
            dt.Rows.Add(dr);

            txtProdID.Text = "";
            txtProdDesc.Text = "";
            txtUP.Text = "";
            txtCP.Text = "";

            txtTotalAmnt.Text = "";
            txtDisAmnt.Text = "";
            //txtDisCode.Text = "";
            //ddlRefDiscount.SelectedItem.Text = "";
            txtWithAdj.Text = "";
            txtNet.Text = "";
            txtSL.Text = "";
            //txtRemarks.Text = "";

            lblBLIPAmnt.Text = "0";
            lblIncentiveAmnt.Text = "0";
            lblIncentiveType.Text = "";
            lblUP.Text = "0";


            txtCode.Text = "";
            txtProdDesc.Text = "";
            txtProdID.Text = "0";
            txtModel.Text = "";
            txtModel.Focus();
            // Load Ref.Discount
            txtOnlineOrder.Text = "";

            try
            {
                var giftsForModels = new[]
                {
                    new { GiftModel = "none", Model = "none"},
                    //new { GiftModel = "32LQ636B", Model = "GS-X6172NS"},
                    //new { GiftModel = "32LQ636B", Model = "GS-Q6472NS" },
                    //new { GiftModel = "32LQ636B", Model = "GS-B6432WB" },   
                    //new { GiftModel = "CMU-BC1", Model = "XR-77A80K" },
                    //new { GiftModel = "CMU-BC1", Model = "XR-65X90K" }, 
                    //new { GiftModel = "CMU-BC1", Model = "XR-55X90K" }, 
                
                }.ToList();
                foreach (var item in giftsForModels)
                {
                    if (item.Model == this.ddlContinents.SelectedItem.Text)
                    {
                        AddGiftItemToRowsRows(item.GiftModel, Convert.ToInt32(txtQty.Text));
                    }
                }
            }
            catch (Exception)
            {

                //throw;
            }
            ddlContinents.SelectedItem.Text = "";
            txtQty.Text = "";
            LoadDiscountReferenceList();
        }
        //CLEAR ALL TEXT
    }

    //ADD DATA IN GRIDVIEW
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (!NetAmntCalculation())
            return;
         Data.MainData items = Newshopapi(txtOnlineOrder.Text.Trim().ToString(), 0);       
        //try
        //{
         ViewState["__DelivariCharge__"] = items.data.delivery_fee.ToString();
         string delivary_charge = ViewState["__DelivariCharge__"].ToString();  
         




        if (ddlRefDiscount.SelectedValue == "Online Order" && txtOnlineOrder.Text.Trim().Length == 14 && (items.data != null))
        {
            try
            {
                SqlConnection connRos = DBConnection_ROS.GetConnection();
                string sSql = "";
                sSql = "select DelNo,dStatus from tbCustomerDelivery WHERE DelNo='" + txtOnlineOrder.Text.Trim() + "'";
                SqlCommand cmdRR = new SqlCommand(sSql, connRos);
                connRos.Open();
                SqlDataReader shopdr = cmdRR.ExecuteReader();

                Data.MainData data = Newshopapi(txtOnlineOrder.Text.Trim(), 0);
                if (data == null || data.massage == "Invalid order number")
                {
                    //Invalid order number
                    PopupMessage("Please enter a valid Online Order No..This Order No didnt found on Online store.Please contact to Mohiuddin(Software Engineer)", btnAdd);
                    txtOnlineOrder.Focus();
                    return;
                }
                else if (data.massage == "Already delivered")
                {
                    PopupMessage("This Online Order No already delivered..If anything found wrong..Please contact to Mohiuddin(Software Engineer)", btnAdd);
                    txtOnlineOrder.Focus();
                    return;
                }
                else if (data.order_status == 7)
                {
                    PopupMessage("This Online Order No already cencelled..If anything found wrong..Please contact to Mohiuddin(Software Engineer)", btnAdd);
                    txtOnlineOrder.Focus();
                    return;
                }
                //else {
                //    PopupMessage("Invalid Key..Please contact to Mohiuddin(Software Engineer)", btnAdd);
                //    txtOnlineOrder.Focus();
                //    return;
                //}
                //if (shopdr.Read())
                //{
                //    if (shopdr["DelNo"].ToString() != txtOnlineOrder.Text.Trim().ToString())
                //    {
                //        PopupMessage("Please enter a valid Online Order No..This Order No didnt found on Online store.Please contact to Mohiuddin(Software Engineer)", btnAdd);
                //        txtOnlineOrder.Focus();
                //        return;
                //    }
                //    else if (shopdr["DelNo"].ToString() == txtOnlineOrder.Text.Trim() && shopdr["dStatus"].ToString() == "3")
                //    {
                //        PopupMessage("This Online Order No already delivered..If anything found wrong..Please contact to Mohiuddin(Software Engineer)", btnAdd);
                //        txtOnlineOrder.Focus();
                //        return;
                //    }
                //    else if (shopdr["DelNo"].ToString() == txtOnlineOrder.Text.Trim() && shopdr["dStatus"].ToString() == "4")
                //    {
                //        PopupMessage("This Online Order No already cencelled..If anything found wrong..Please contact to Mohiuddin(Software Engineer)", btnAdd);
                //        txtOnlineOrder.Focus();
                //        return;
                //    }
                //    //CampPrice = UP - Convert.ToDouble(dr["DisAmnt"].ToString());
                //    //this.txtCP.Text = Convert.ToString(CampPrice);
                //}
                //else
                //{
                //    PopupMessage("Please enter a valid Online Order No..This Order No didnt found on Online store.Please contact to Mohiuddin(Software Engineer)", btnAdd);
                //    txtOnlineOrder.Focus();
                //    return;
                //    //CampPrice = UP;
                //    //this.txtCP.Text = Convert.ToString(CampPrice);
                //}
                shopdr.Dispose();
                shopdr.Close();
                connRos.Close();
                this.btnAvailOffer.Enabled = false;

            }
            catch
            {
                PopupMessage("Something went wrong.Please contact to Software Engineer", btnAdd);
                txtOnlineOrder.Focus();
                return;
                //
            }
        }
        //else if (ddlRefDiscount.SelectedValue == "Online Order" && txtOnlineOrder.Text.Trim().Length == 13)
        //{
        //    try
        //    {
        //        SqlConnection connRos = DBConnection_ROS.GetConnection();
        //        string sSql = "";
        //        sSql = "select DelNo,dStatus from tbCustomerDelivery WHERE DelNo='" + txtOnlineOrder.Text.Trim() + "'";
        //        SqlCommand cmdRR = new SqlCommand(sSql, connRos);
        //        connRos.Open();
        //        SqlDataReader shopdr = cmdRR.ExecuteReader();


        //        if (shopdr.Read())
        //        {
        //            if (shopdr["DelNo"].ToString() != txtOnlineOrder.Text.Trim().ToString())
        //            {
        //                PopupMessage("Please enter a valid Online Order No..This Order No didnt found on Online store.Please contact to Mohiuddin(Software Engineer)", btnAdd);
        //                txtOnlineOrder.Focus();
        //                return;
        //            }
        //            else if (shopdr["DelNo"].ToString() == txtOnlineOrder.Text.Trim() && shopdr["dStatus"].ToString() == "3")
        //            {
        //                PopupMessage("This Online Order No already delivered..If anything found wrong..Please contact to Mohiuddin(Software Engineer)", btnAdd);
        //                txtOnlineOrder.Focus();
        //                return;
        //            }
        //            else if (shopdr["DelNo"].ToString() == txtOnlineOrder.Text.Trim() && shopdr["dStatus"].ToString() == "4")
        //            {
        //                PopupMessage("This Online Order No already cencelled..If anything found wrong..Please contact to Mohiuddin(Software Engineer)", btnAdd);
        //                txtOnlineOrder.Focus();
        //                return;
        //            }
        //            //CampPrice = UP - Convert.ToDouble(dr["DisAmnt"].ToString());
        //            //this.txtCP.Text = Convert.ToString(CampPrice);
        //        }
        //        else
        //        {
        //            PopupMessage("Please enter a valid Online Order No..This Order No didnt found on Online store.Please contact to Mohiuddin(Software Engineer)", btnAdd);
        //            txtOnlineOrder.Focus();
        //            return;
        //            //CampPrice = UP;
        //            //this.txtCP.Text = Convert.ToString(CampPrice);
        //        }
        //        shopdr.Dispose();
        //        shopdr.Close();
        //        connRos.Close();
        //        this.btnAvailOffer.Enabled = false;
        //        this.btnRedeem.Enabled = false;

        //    }
        //    catch
        //    {
        //        PopupMessage("Something went wrong.Please contact to Mohiuddin(Software Engineer)", btnAdd);
        //        txtOnlineOrder.Focus();
        //        return;
        //        //
        //    }
        //}
        else if (ddlRefDiscount.SelectedValue == "Free Gift" || ddlRefDiscount.SelectedValue == "GM Sir"
            || ddlRefDiscount.SelectedValue == "DGM Sir" || ddlRefDiscount.SelectedValue == "Customer Withdrawal"
            || ddlRefDiscount.SelectedValue == "PWP Offer 15%"
            || ddlRefDiscount.SelectedValue == "PWP Offer 10%"
            || ddlRefDiscount.SelectedValue == "New LG TV Launching Offer"
            || ddlRefDiscount.SelectedValue == "")
        {
            if (ddlRefDiscount.SelectedValue == "Free Gift")
            {
                //String[] giftItems = new String[] { "19-22 WALL HANG BRACKET",
                //    "19-24 WALL HANG BRACKET", "26-32 WALL HANG BRACKET",
                //    "32-85 WALL HANG BRACKET","40-46 WALL HANG BRACKET",
                //    "50-100 WALL HANG BRACKET","50-60 WALL HANG BRACKET",
                //    "52-60 WALL HANG BRACKET","55-60 WALL HANG BRACKET",
                //    "65-85 WALL HANG BRACKET","70-90 WALL HANG BRACKET",
                //    "CALENDER-2009","CALENDER-2010","CALENDER-2007",
                //    "CALENDER-2011","CALENDER-2012","RL-32G404K","RL-32K450S","RL-32K404B","cmu-bc1"
                //    ,"HT-S400F","BRAVIA Cam","SMART JHILIK","INSTA-600W","HP-8120","32 COUNTRY FLAG",
                //    "WORLD CUP TROPHY","BRAZIL FLAG","BRAZIL SCARF","ARGENTINA FLAG","ARGENTINA SCARF","BEAMER",
                //    "CALENDER-2020","Desk Calender 2020","Desk Calender-2021","DESK CALENDER-2022","BAG","SONY BRAVIA XR T-SHIRT","World Cup T-Shirt 2022" };

                //if (!giftItems.Contains(ddlContinents.SelectedItem.Text))
                //{
                //    PopupMessage("This model is not enlisted as free gift item)", btnAdd);
                //    txtModel.Focus();
                //    return;
                //}
            }



            String[] pwpItems = new String[] { "HT-S100F",
                    "HT-S40R", "HT-S400",
                    "HT-G700","HT-S350" };

            //HT-S100F,HT-S40R,HT-S400,HT-G700,HT-S350
            if (ddlRefDiscount.SelectedValue == "PWP Offer 15%")
            {
                if (pwpItems.Contains(ddlContinents.SelectedItem.Text))
                {
                    this.txtDisAmnt.Text = (Math.Ceiling(Convert.ToInt32(this.txtTotalAmnt.Text) * 0.15)).ToString();
                    this.txtNet.Text = (Convert.ToInt32(this.txtTotalAmnt.Text) - Convert.ToInt32(this.txtDisAmnt.Text)).ToString();

                }
                else
                {
                    PopupMessage("This model is not enlisted as free PWP offer item)", btnAdd);
                    txtModel.Focus();
                    return;
                }

            }

            if (ddlRefDiscount.SelectedValue == "PWP Offer 10%")
            {

                if (pwpItems.Contains(ddlContinents.SelectedItem.Text))
                {
                    this.txtDisAmnt.Text = (Math.Ceiling(Convert.ToInt32(this.txtTotalAmnt.Text) * 0.10)).ToString();
                    this.txtNet.Text = (Convert.ToInt32(this.txtTotalAmnt.Text) - Convert.ToInt32(this.txtDisAmnt.Text)).ToString();

                }
                else
                {
                    PopupMessage("This model is not enlisted as free PWP offer item)", btnAdd);
                    txtModel.Focus();
                    return;
                }
            }


            String[] lgTvLaunchingOfferItem = new String[] { "43NANO75",
                    "55QNED80", "65QNED80",
                    "75QNED80","OLED55C2",
                     "OLED65C2","OLED65G2"};
            if (ddlRefDiscount.SelectedValue == "New LG TV Launching Offer")
            {
                if (lgTvLaunchingOfferItem.Contains(ddlContinents.SelectedItem.Text))
                {

                    if (ddlContinents.SelectedItem.Text == "43NANO75")
                    {
                        this.txtDisAmnt.Text = (Math.Ceiling(Convert.ToInt32(this.txtTotalAmnt.Text) * 0.05)).ToString();
                    }
                    else
                    {
                        this.txtDisAmnt.Text = (Math.Ceiling(Convert.ToInt32(this.txtTotalAmnt.Text) * 0.08)).ToString();
                    }
                    this.txtNet.Text = (Convert.ToInt32(this.txtTotalAmnt.Text) - Convert.ToInt32(this.txtDisAmnt.Text)).ToString();
                    btnSpinCoupon.Enabled = false;
                }
                else
                {
                    PopupMessage("This model is not enlisted as LG TV Launching Offer offer item)", btnAdd);
                    txtModel.Focus();
                    return;
                }
            }
            if (ddlRefDiscount.SelectedValue == "GM Sir"
            || ddlRefDiscount.SelectedValue == "DGM Sir")
            {
                btnRedeem.Enabled = false;
                btnAvailOffer.Enabled = false;
                ddlReference.Enabled = false;
            }


            AddRows();
            gvUsers.DataSource = dt;
            gvUsers.DataBind();
            return;
            //,GM Sir,DGM Sir,DGM Sir,Customer Withdrawal
        }
        else
        {
            if (ddlRefDiscount.SelectedValue == "Online Order")
            {
                PopupMessage("Order No:" + txtOnlineOrder.Text + ".Please enter a valid Online Order No..Order no should be length of 14 digit..This Order No didnt found on Online store.Please contact to Mohiuddin(Software Engineer)", btnAdd);
                txtOnlineOrder.Focus();
                return;
            }
        }

        //if (txtOrderNo.Text == "" && txtOnlineOrder.Text != "")
        //{
        //    txtOrderNo.Text = txtOnlineOrder.Text;
        //}

        if (txtOnlineOrder.Text != "")
        {
            txtOrderNo.Text = txtOnlineOrder.Text;
        }
        //sony tv cashback

        String[] sony_tv_discount = new String[]{"5X85K","KD-85X85J","KD-85X8000H","KD-75X80J","XR-65A80K","XR-65A80J","KD-65X85K","KD-65X85J","KD-65X80K",	
                      "KD-65X80J","KD-65X75K","XR-55A80K","XR-55A80J","XR-55X90K","KD-55X85K","KD-55X80K","KD-55X75K"};



        if (ddlRefDiscount.SelectedValue == "Sony TV Cashback Offer")
        {
            if (sony_tv_discount.Contains(ddlContinents.SelectedItem.Text))
            {
                //KD-85X8000H,KD-75X80J,XR-65A80K
                if (ddlContinents.SelectedItem.Text == "KD-85X8000H" || ddlContinents.SelectedItem.Text == "KD-75X80J" || ddlContinents.SelectedItem.Text == "XR-65A80K")
                {
                    this.txtDisAmnt.Text = (20000).ToString();
                    this.txtNet.Text = (Convert.ToInt32(this.txtTotalAmnt.Text) - Convert.ToInt32(this.txtDisAmnt.Text)).ToString();
                }

                  //KD-85X85K,KD-55X85K
                else if (ddlContinents.SelectedItem.Text == "KD-85X8000H" || ddlContinents.SelectedItem.Text == "KD-75X80J")
                {
                    this.txtDisAmnt.Text = (25000).ToString();
                    this.txtNet.Text = (Convert.ToInt32(this.txtTotalAmnt.Text) - Convert.ToInt32(this.txtDisAmnt.Text)).ToString();
                }

                //XR-65A80J,KD-65X85K
                else if (ddlContinents.SelectedItem.Text == "XR-65A80J" || ddlContinents.SelectedItem.Text == "KD-65X85K")
                {
                    this.txtDisAmnt.Text = (40000).ToString();
                    this.txtNet.Text = (Convert.ToInt32(this.txtTotalAmnt.Text) - Convert.ToInt32(this.txtDisAmnt.Text)).ToString();
                }

                //KD-65X85J,KD-65X75K
                else if (ddlContinents.SelectedItem.Text == "KD-65X85J" || ddlContinents.SelectedItem.Text == "KD-65X75K")
                {
                    this.txtDisAmnt.Text = (5000).ToString();
                    this.txtNet.Text = (Convert.ToInt32(this.txtTotalAmnt.Text) - Convert.ToInt32(this.txtDisAmnt.Text)).ToString();
                }


                //XR-55A80K,XR-55A80J
                else if (ddlContinents.SelectedItem.Text == "XR-55A80K" || ddlContinents.SelectedItem.Text == "XR-55A80J")
                {
                    this.txtDisAmnt.Text = (35000).ToString();
                    this.txtNet.Text = (Convert.ToInt32(this.txtTotalAmnt.Text) - Convert.ToInt32(this.txtDisAmnt.Text)).ToString();
                }


                //KD-85X85J
                else if (ddlContinents.SelectedItem.Text == "KD-85X85J")
                {
                    this.txtDisAmnt.Text = (100000).ToString();
                    this.txtNet.Text = (Convert.ToInt32(this.txtTotalAmnt.Text) - Convert.ToInt32(this.txtDisAmnt.Text)).ToString();
                }

                //KD-65X80K
                else if (ddlContinents.SelectedItem.Text == "KD-65X80K")
                {
                    this.txtDisAmnt.Text = (30000).ToString();
                    this.txtNet.Text = (Convert.ToInt32(this.txtTotalAmnt.Text) - Convert.ToInt32(this.txtDisAmnt.Text)).ToString();
                }

               //KD-65X80J
                else if (ddlContinents.SelectedItem.Text == "KD-65X80J")
                {
                    this.txtDisAmnt.Text = (10000).ToString();
                    this.txtNet.Text = (Convert.ToInt32(this.txtTotalAmnt.Text) - Convert.ToInt32(this.txtDisAmnt.Text)).ToString();
                }


               //XR-55X90K
                else if (ddlContinents.SelectedItem.Text == "XR-55X90K")
                {
                    this.txtDisAmnt.Text = (15000).ToString();
                    this.txtNet.Text = (Convert.ToInt32(this.txtTotalAmnt.Text) - Convert.ToInt32(this.txtDisAmnt.Text)).ToString();
                }

               //KD-55X80K
                else if (ddlContinents.SelectedItem.Text == "KD-55X80K")
                {
                    this.txtDisAmnt.Text = (24000).ToString();
                    this.txtNet.Text = (Convert.ToInt32(this.txtTotalAmnt.Text) - Convert.ToInt32(this.txtDisAmnt.Text)).ToString();
                }

              //KD-55X75K
                else if (ddlContinents.SelectedItem.Text == "KD-55X75K")
                {
                    this.txtDisAmnt.Text = (22000).ToString();
                    this.txtNet.Text = (Convert.ToInt32(this.txtTotalAmnt.Text) - Convert.ToInt32(this.txtDisAmnt.Text)).ToString();
                }

            }
            else
            {
                PopupMessage("This model is not Available Sony Instant  Cashback Offer offer)", btnAdd);
                txtModel.Focus();
                return;
            }



        }




        //lg tv instant cashback

        String[] lg_tv_model_cashback = new String[]{"32LQ636B","43UN731C","43UP7550","50UP7750","55UN731C","55UP7550","65UN731C",
            "43NANO75","55NANO75","65NANO86","75NANO75","86NANO75","55QNED80","65QNED80","75QNED80","OLED65A1","OLED55C2","OLED65C2","OLED65G2","GS-X6172NS","GS-Q6472NS","GS-B6432WB","T5107BM"};

        /// logic impliment
      
            //43UN731C,43UP7550,43NANO75
            if (ddlRefDiscount.SelectedValue == "LG TV Cashback Offer")
            {
                if (lg_tv_model_cashback.Contains(ddlContinents.SelectedItem.Text))
                {
                    if (ddlContinents.SelectedItem.Text == "43UN731C" || ddlContinents.SelectedItem.Text == "43UP7550" || ddlContinents.SelectedItem.Text == "43NANO75")
                    {
                        this.txtDisAmnt.Text = (3000).ToString();
                        this.txtNet.Text = (Convert.ToInt32(this.txtTotalAmnt.Text) - Convert.ToInt32(this.txtDisAmnt.Text)).ToString();
                    }
                }
                else
                {
                    PopupMessage("This model is not Available LG Cashback Offer offer)", btnAdd);
                    txtModel.Focus();
                    return;
                }
              
            }

            //32LQ636B 

            if (ddlRefDiscount.SelectedValue == "LG TV Cashback Offer")
            {
                if (lg_tv_model_cashback.Contains(ddlContinents.SelectedItem.Text))
                {
                    if (ddlContinents.SelectedItem.Text == "32LQ636B")
                    {
                        this.txtDisAmnt.Text = (4000).ToString();
                        this.txtNet.Text = (Convert.ToInt32(this.txtTotalAmnt.Text) - Convert.ToInt32(this.txtDisAmnt.Text)).ToString();
                    }
                }
                else
                {
                    PopupMessage("This model is not Available LG Cashback Offer offer)", btnAdd);
                    txtModel.Focus();
                    return;
                }
   
               
            }


            //50UP7750
            if (ddlRefDiscount.SelectedValue == "LG TV Cashback Offer")
            {
                if (lg_tv_model_cashback.Contains(ddlContinents.SelectedItem.Text))
                {
                    if (ddlContinents.SelectedItem.Text == "50UP7750")
                    {
                        this.txtDisAmnt.Text = (8000).ToString();
                        this.txtNet.Text = (Convert.ToInt32(this.txtTotalAmnt.Text) - Convert.ToInt32(this.txtDisAmnt.Text)).ToString();
                    }
                }
                else
                {
                    PopupMessage("This model is not Available LG Cashback Offer offer)", btnAdd);
                    txtModel.Focus();
                    return;
                }

           
            }

            //55UN731C,55UP7550,OLED65C2

            if (ddlRefDiscount.SelectedValue == "LG TV Cashback Offer")
            {
                if (lg_tv_model_cashback.Contains(ddlContinents.SelectedItem.Text))
                {
                    if (ddlContinents.SelectedItem.Text == "55UN731C" || ddlContinents.SelectedItem.Text == "55UP7550" || ddlContinents.SelectedItem.Text == "OLED65C2")
                    {
                        this.txtDisAmnt.Text = (20000).ToString();
                        this.txtNet.Text = (Convert.ToInt32(this.txtTotalAmnt.Text) - Convert.ToInt32(this.txtDisAmnt.Text)).ToString();
                    }
                }
                else
                {
                    PopupMessage("This model is not Available LG  Cashback Offer offer)", btnAdd);
                    txtModel.Focus();
                    return;
                }

               
            }


            //65UN731C,55QNED80

            if (ddlRefDiscount.SelectedValue == "LG TV Cashback Offer")
            {
                if (lg_tv_model_cashback.Contains(ddlContinents.SelectedItem.Text))
                {
                    if (ddlContinents.SelectedItem.Text == "65UN731C" || ddlContinents.SelectedItem.Text == "55QNED80")
                    {
                        this.txtDisAmnt.Text = (15000).ToString();
                        this.txtNet.Text = (Convert.ToInt32(this.txtTotalAmnt.Text) - Convert.ToInt32(this.txtDisAmnt.Text)).ToString();
                    }
                }
                else
                {
                    PopupMessage("This model is not Available LG  Cashback Offer offer)", btnAdd);
                    txtModel.Focus();
                    return;
                }

             
            }

            //65QNED80,OLED65A1
            if (ddlRefDiscount.SelectedValue == "LG TV Cashback Offer")
            {
                if (lg_tv_model_cashback.Contains(ddlContinents.SelectedItem.Text))
                {
                    if (ddlContinents.SelectedItem.Text == "65QNED80" || ddlContinents.SelectedItem.Text == "OLED65A1")
                    {
                        this.txtDisAmnt.Text = (100000).ToString();
                        this.txtNet.Text = (Convert.ToInt32(this.txtTotalAmnt.Text) - Convert.ToInt32(this.txtDisAmnt.Text)).ToString();
                    }
                }
                else
                {
                    PopupMessage("This model is not Available LG  Cashback Offer offer)", btnAdd);
                    txtModel.Focus();
                    return;
                }
   
              
            }

            //65NANO86,75QNED80
            if (ddlRefDiscount.SelectedValue == "LG TV Cashback Offer")
            {
                if (lg_tv_model_cashback.Contains(ddlContinents.SelectedItem.Text))
                {
                    if (ddlContinents.SelectedItem.Text == "65NANO86" || ddlContinents.SelectedItem.Text == "75QNED80")
                    {
                        this.txtDisAmnt.Text = (75000).ToString();
                        this.txtNet.Text = (Convert.ToInt32(this.txtTotalAmnt.Text) - Convert.ToInt32(this.txtDisAmnt.Text)).ToString();
                    }
                }
                else
                {
                    PopupMessage("This model is not Available LG Cashback Offer offer)", btnAdd);
                    txtModel.Focus();
                    return;
                }
            }

            //OLED65G2
            if (ddlRefDiscount.SelectedValue == "LG TV Cashback Offer")
            {
                if (lg_tv_model_cashback.Contains(ddlContinents.SelectedItem.Text))
                {
                    if (ddlContinents.SelectedItem.Text == "OLED65G2")
                    {
                        this.txtDisAmnt.Text = (37000).ToString();
                        this.txtNet.Text = (Convert.ToInt32(this.txtTotalAmnt.Text) - Convert.ToInt32(this.txtDisAmnt.Text)).ToString();
                    }
                }
                else
                {
                    PopupMessage("This model is not Available LG Cashback Offer offer)", btnAdd);
                    txtModel.Focus();
                    return;
                }
               
            }

            //OLED55C2
            if (ddlRefDiscount.SelectedValue == "LG TV Cashback Offer")
            {
                if (lg_tv_model_cashback.Contains(ddlContinents.SelectedItem.Text))
                {
                    if (ddlContinents.SelectedItem.Text == "OLED55C2")
                    {
                        this.txtDisAmnt.Text = (10000).ToString();
                        this.txtNet.Text = (Convert.ToInt32(this.txtTotalAmnt.Text) - Convert.ToInt32(this.txtDisAmnt.Text)).ToString();
                    }
                }
                else
                {
                    PopupMessage("This model is not Available LG Cashback Offer offer)", btnAdd);
                    txtModel.Focus();
                    return;
                }

                
            }

            //86NANO75

            if (ddlRefDiscount.SelectedValue == "LG TV Cashback Offer")
            {
                if (lg_tv_model_cashback.Contains(ddlContinents.SelectedItem.Text))
                {
                    if (ddlContinents.SelectedItem.Text == "86NANO75")
                    {
                        this.txtDisAmnt.Text = (0).ToString();
                        this.txtNet.Text = (Convert.ToInt32(this.txtTotalAmnt.Text) - Convert.ToInt32(this.txtDisAmnt.Text)).ToString();
                    }
                }
                else
                {
                    PopupMessage("This model is not Available LG Cashback Offer offer)", btnAdd);
                    txtModel.Focus();
                    return;
                }

               
            }

            //75NANO75
            if (ddlRefDiscount.SelectedValue == "LG TV Cashback Offer")
            {
                if (lg_tv_model_cashback.Contains(ddlContinents.SelectedItem.Text))
                {
                    if (ddlContinents.SelectedItem.Text == "75NANO75")
                    {
                        this.txtDisAmnt.Text = (76000).ToString();
                        this.txtNet.Text = (Convert.ToInt32(this.txtTotalAmnt.Text) - Convert.ToInt32(this.txtDisAmnt.Text)).ToString();
                    }
                }
                else
                {
                    PopupMessage("This model is not Available LG  Cashback Offer offer)", btnAdd);
                    txtModel.Focus();
                    return;
                }
                
               
            }

            //55NANO75
            if (ddlRefDiscount.SelectedValue == "LG TV Cashback Offer")
            {
                if (lg_tv_model_cashback.Contains(ddlContinents.SelectedItem.Text))
                {
                    if (ddlContinents.SelectedItem.Text == "55NANO75")
                    {
                        this.txtDisAmnt.Text = (25000).ToString();
                        this.txtNet.Text = (Convert.ToInt32(this.txtTotalAmnt.Text) - Convert.ToInt32(this.txtDisAmnt.Text)).ToString();
                    }
                }
                else
                {
                    PopupMessage("This model is not Available LG  Cashback Offer offer)", btnAdd);
                    txtModel.Focus();
                    return;
                }
             
            }

            //GS-X6172NS,GS-Q6472NS,T5107BM

            if (ddlRefDiscount.SelectedValue == "LG TV Cashback Offer")
            {
                if (lg_tv_model_cashback.Contains(ddlContinents.SelectedItem.Text))
                {
                    if (ddlContinents.SelectedItem.Text == "T5107BM" || ddlContinents.SelectedItem.Text == "GS-Q6472NS" || ddlContinents.SelectedItem.Text == "GS-X6172NS")
                    {
                        this.txtDisAmnt.Text = (10000).ToString();
                        this.txtNet.Text = (Convert.ToInt32(this.txtTotalAmnt.Text) - Convert.ToInt32(this.txtDisAmnt.Text)).ToString();
                    }
                }
                else
                {
                    PopupMessage("This model is not Available LG  Cashback Offer offer)", btnAdd);
                    txtModel.Focus();
                    return;
                }
               
            }


            //GS-B6432WB

            if (ddlRefDiscount.SelectedValue == "LG TV Cashback Offer")
            {
                if (lg_tv_model_cashback.Contains(ddlContinents.SelectedItem.Text))
                {
                    if (ddlContinents.SelectedItem.Text == "GS-B6432WB")
                    {
                        this.txtDisAmnt.Text = (30000).ToString();
                        this.txtNet.Text = (Convert.ToInt32(this.txtTotalAmnt.Text) - Convert.ToInt32(this.txtDisAmnt.Text)).ToString();
                    }
                }
                else
                {
                    PopupMessage("This model is not Available LG  Cashback Offer offer)", btnAdd);
                    txtModel.Focus();
                    return;
                }

            }




              //Ac exchange Offer and Religion offer start

             String[] newdis = new string[] {"RANGS AC","KELVINATOR AC"};

             if (ddlRefDiscount.SelectedValue == "Ac Exchange dis 10%") 
              {
                  if (newdis.Contains(GetProductGroupName(ddlContinents.SelectedItem.Text).Trim()))
                  {
                      this.txtDisAmnt.Text = (Math.Ceiling(Convert.ToInt32(this.txtTotalAmnt.Text) * 0.1)).ToString();
                      this.txtNet.Text = (Convert.ToInt32(this.txtTotalAmnt.Text) - Convert.ToInt32(this.txtDisAmnt.Text)).ToString();
                  }

                  else
                  {
                      PopupMessage("This model is not enlisted as Ac Exchange dis 10% item)", btnAdd);
                      txtModel.Focus();
                      return;
                  }

               }

             if (ddlRefDiscount.SelectedValue == "Mosque/Relagious/Muktijoddha Offer 5%")
             {
                 if (newdis.Contains(GetProductGroupName(ddlContinents.SelectedItem.Text)))
                 {
                     this.txtDisAmnt.Text = (Math.Ceiling(Convert.ToInt32(this.txtTotalAmnt.Text) * 0.05)).ToString();
                     this.txtNet.Text = (Convert.ToInt32(this.txtTotalAmnt.Text) - Convert.ToInt32(this.txtDisAmnt.Text)).ToString();
                 }

                 else
                 {
                     PopupMessage("This model is not enlisted as Mosque/Relagious/Muktijoddha Offer 5% item)", btnAdd);
                     txtModel.Focus();
                     return;
                 }

             }




            //Ac exchange Offer and Religion offer End//

            //// Speacial Discount for Lg LCD Product
            String[] lg_lcd_model = new String[] { "32LQ636B", "43UP7550", "50UP7750", "43NANO75", "75NANO75", "75QNED80" };

            // //32LQ636B,43U7550,50UP7750,43NANO75,75NANO75,75QNED80

            //LG LCD TV Special Offer 6%


            if (ddlRefDiscount.SelectedValue == "LG Tv 6%")
            {
                if (lg_lcd_model.Contains(ddlContinents.SelectedItem.Text))
                {
                    this.txtDisAmnt.Text = (Math.Ceiling(Convert.ToInt32(this.txtTotalAmnt.Text) * 0.06)).ToString();
                    this.txtNet.Text = (Convert.ToInt32(this.txtTotalAmnt.Text) - Convert.ToInt32(this.txtDisAmnt.Text)).ToString();


                }
                else
                {
                    PopupMessage("This model is not enlisted as LG TV (6%) item)", btnAdd);
                    txtModel.Focus();
                    return;
                }
            }
            //LG LCD TV Special Offer 8%
            if (ddlRefDiscount.SelectedValue == "LG Tv 8%")
            {
                if (lg_lcd_model.Contains(ddlContinents.SelectedItem.Text))
                {


                    this.txtDisAmnt.Text = (Math.Ceiling(Convert.ToInt32(this.txtTotalAmnt.Text) * 0.08)).ToString();
                    this.txtNet.Text = (Convert.ToInt32(this.txtTotalAmnt.Text) - Convert.ToInt32(this.txtDisAmnt.Text)).ToString();


                }
                else
                {
                    PopupMessage("This model is not enlisted as LG TV (8%) item)", btnAdd);
                    txtModel.Focus();
                    return;
                }
            }
            //LG LCD TV Special Offer 10%
            if (ddlRefDiscount.SelectedValue == "LG Tv 10%")
            {
                if (lg_lcd_model.Contains(ddlContinents.SelectedItem.Text))
                {


                    this.txtDisAmnt.Text = (Math.Ceiling(Convert.ToInt32(this.txtTotalAmnt.Text) * 0.1)).ToString();
                    this.txtNet.Text = (Convert.ToInt32(this.txtTotalAmnt.Text) - Convert.ToInt32(this.txtDisAmnt.Text)).ToString();


                }
                else
                {
                    PopupMessage("This model is not enlisted as LG TV (10%) item)", btnAdd);
                    txtModel.Focus();
                    return;
                }
            }


            AddRows();
            gvUsers.DataSource = dt;
            gvUsers.DataBind();
         


       




        //}
        //catch (InvalidCastException err)
        //{
        //    throw (err);
        //}

    }

    //GRID ROW DELETE
    //protected void gvUsers_RowDelating(object sender, GridViewDeleteEventArgs e)
    //{
    //    try
    //    {
    //        int index = Convert.ToInt32(e.RowIndex);
    //        DataTable dt = ViewState["dt"] as DataTable;
    //        dt.Rows[index].Delete();
    //        ViewState["dt"] = dt;
    //        BindGrid();
    //        if (this.txtOrderNo.Text != "")
    //        {
    //            //DataTable dt1 = new DataTable();
    //            //MakeTable();
    //            dt.Rows.Clear();
    //            ViewState["dt"] = dt;
    //            BindGrid();
    //            this.ddlContinents.Enabled = true;
    //            this.txtCode.Enabled = true;
    //            this.txtModel.Enabled = true;
    //            this.txtOrderNo.Enabled = true;
    //            this.txtOnlineOrder.Enabled = true;
    //            this.txtCustName.Enabled = true;
    //            this.txtCustContact.Enabled = true;
    //            this.txtEmail.Enabled = true;
    //            this.txtOrderNo.Text = "";
    //            this.btnCustSearch.Enabled = true;
    //            this.txtCustName.Text = "";
    //            this.txtCustContact.Text = "";
    //            this.txtEmail.Text = "";
    //            this.txtCustAdd.Text = "";
    //        }

    //    }
    //    catch (InvalidCastException err)
    //    {
    //        throw (err);
    //    }

    //}
protected void gvUsers_RowDelating(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(e.RowIndex);
            DataTable dt = ViewState["dt"] as DataTable;



            string package = "";
            foreach (DataRow row in dt.Rows)
            {
                if (dt.Rows.IndexOf(row) == index)
                {
                    package = row["DisRef"].ToString();
                }
            }

            dt.Rows[index].Delete();
            ViewState["dt"] = dt;
            BindGrid();
            if (this.txtOrderNo.Text != "")
            {
                //DataTable dt1 = new DataTable();
                //MakeTable();
                dt.Rows.Clear();
                ViewState["dt"] = dt;
                BindGrid();
                this.ddlContinents.Enabled = true;
                this.txtCode.Enabled = true;
                this.txtModel.Enabled = true;
                this.txtOrderNo.Enabled = true;
                this.txtOnlineOrder.Enabled = true;
                this.txtCustName.Enabled = true;
                this.txtCustContact.Enabled = true;
                this.txtEmail.Enabled = true;
                this.txtOrderNo.Text = "";
                this.btnCustSearch.Enabled = true;
                this.txtCustName.Text = "";
                this.txtCustContact.Text = "";
                this.txtEmail.Text = "";
                this.txtCustAdd.Text = "";
            }


            //List<DataRow> rowsToDelete = new List<DataRow>();
            DataTable dt1 = ViewState["dt"] as DataTable;
            //foreach( DataRow row in dt1.Rows )
            //{
            //    if (row["DisRef"].ToString()==package)
            //    {
            //        dt.Rows[dt.Rows.IndexOf(row)].Delete();
            //        ViewState["dt"] = dt1;
            //        BindGrid();
            //        dt1 = ViewState["dt"] as DataTable;

            //    }
            //}
            List<DataRow> rowsToDelete = new List<DataRow>();
            

            if (package.Contains("Package"))
            {
                foreach (DataRow row in dt1.Rows)
                {
                    if (row["DisRef"].ToString() == package)
                    {
                        rowsToDelete.Add(row);
                    }
                }
                foreach (DataRow row in rowsToDelete)
                {
                    dt1.Rows.Remove(row);
                }
                ViewState["dt"] = dt1;
                BindGrid();
            }




            //foreach( DataRow row in rowsToDelete )
            //{
            //    this.gvUsers.Rows.Remove( row );
            //}
            //foreach (GridViewRow g1 in this.gvUsers.Rows)
            //{
            //    //g1.row
            //    //if (g1.Cells[3].Text.Contains("package"))
            //    //{

            //    //}

            //}

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
        double UP = 0;
        double CampPrice = 0;
        string sSql = "";

        sSql = "";

        //sSql = "SELECT ProductID,ProdName,UnitPrice,Model,Code,";
        //sSql = sSql + " IncentiveType,BLIPAmnt,IncentiveAmnt,GetIncentive,WPPrice,BLIPofWP,WPIncentive,ISNULL(WPMinQty,0) AS WPMinQty";
        //sSql = sSql + " FROM Product";

        sSql = "SELECT ProductID,ProdName,UnitPrice,Model,Code,";
        sSql = sSql + " IncentiveType,BLIPAmnt,IncentiveAmnt,";
        sSql = sSql + " GetIncentive, ISNULL(WPPrice,0) AS WPPrice, ISNULL(BLIPofWP,0) AS BLIPofWP,";
        sSql = sSql + " ISNULL(WPIncentive,0) AS WPIncentive, ISNULL(WPMinQty,0) AS WPMinQty";
        sSql = sSql + " FROM Product";

        sSql = sSql + " WHERE Model='" + this.ddlContinents.SelectedItem.Text + "'";
        SqlCommand cmd = new SqlCommand(sSql, conn);
        conn.Open();
        SqlDataReader dr = cmd.ExecuteReader();

        try
        {
            if (dr.Read())
            {
                this.txtCode.Text = dr["Code"].ToString();
                this.txtProdID.Text = dr["ProductID"].ToString();
                this.txtProdDesc.Text = dr["ProdName"].ToString();
                UP = Convert.ToDouble(dr["UnitPrice"].ToString());
                this.txtUP.Text = Convert.ToString(UP);

                this.lblIncentiveType.Text = dr["IncentiveType"].ToString();
                this.lblBLIPAmnt.Text = dr["BLIPAmnt"].ToString();
                this.lblIncentiveAmnt.Text = dr["IncentiveAmnt"].ToString();
                this.lblGetIncentive.Text = dr["GetIncentive"].ToString();
                this.lblWPPrice.Text = dr["WPPrice"].ToString();
                this.lblBLIPofWP.Text = dr["BLIPofWP"].ToString();
                this.lblWPIncentive.Text = dr["WPIncentive"].ToString();
                this.lblWPMinQty.Text = dr["WPMinQty"].ToString();
            }
            else
            {
                UP = 0;
                this.txtUP.Text = Convert.ToString(UP);
                this.txtCode.Text = "";
                this.txtProdID.Text = "";
                this.txtProdDesc.Text = "";

                this.lblIncentiveType.Text = "";
                this.lblBLIPAmnt.Text = "0";
                this.lblIncentiveAmnt.Text = "0";
                this.lblGetIncentive.Text = "0";
                this.lblWPPrice.Text = "0";
                this.lblBLIPofWP.Text = "0";
                this.lblWPIncentive.Text = "0";
                this.lblWPMinQty.Text = "0";

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
            " WHERE Model='" + this.ddlContinents.SelectedValue + "'" +
            " AND (EffectiveDate<='" + DateTime.Today + "' AND cStop=0)" +
            " ORDER BY EffectiveDate DESC";
        cmd = new SqlCommand(sSql, conn);
        conn.Open();
        dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            CampPrice = UP - Convert.ToDouble(dr["DisAmnt"].ToString());
            this.txtCP.Text = Convert.ToString(CampPrice);
        }
        else
        {
            CampPrice = UP;
            this.txtCP.Text = Convert.ToString(CampPrice);
        }
        dr.Dispose();
        dr.Close();
        conn.Close();

        //-----------------------------------------------------------
        //lblUP.Text = txtCP.Text;
        //if (lblGetIncentive.Text == "True")
        //{
        //    if (lblIncentiveType.Text == "Instant")
        //    {
        //        this.txtCP.Text = lblBLIPAmnt.Text;
        //    }
        //    else
        //    {
        //        this.txtCP.Text = Convert.ToString(CampPrice);
        //    }
        //}
        //-----------------------------------------------------------

    }

    //PRODUCT QUANTITY
    protected void txtQty_TextChanged(object sender, EventArgs e)
    {
        NetAmntCalculation();
        double tAmnt = 0;
        if (this.txtQty.Text == "")
        {
            //Response.Write("Please enter Quantity"); 
            //lblQty.Text = "Please enter Quantity";
        }
        else
        {
            //lblQty.Text = "";
            if (txtCP.Text.Length == 0)
            {
                this.txtCP.Text = "0";
            }

            if (Convert.ToInt16(this.lblWPMinQty.Text) > 0)
            {
                if (Convert.ToInt16(txtQty.Text) >= Convert.ToInt16(this.lblWPMinQty.Text))
                {
                    txtCP.Text = lblWPPrice.Text;
                }
            }

            if (txtTotalAmnt.Text.Length == 0)
            {
                this.txtTotalAmnt.Text = "0";
            }
            if (txtDisAmnt.Text.Length == 0)
            {
                this.txtDisAmnt.Text = "0";
            }
            if (txtWithAdj.Text.Length == 0)
            {
                this.txtWithAdj.Text = "0";
            }

            //if (txtCP.Text.Length > 0)
            //{
            tAmnt = Convert.ToDouble(this.txtQty.Text) * Convert.ToDouble(this.txtCP.Text);
            this.txtTotalAmnt.Text = Convert.ToString(tAmnt);
            //this.txtDisAmnt.Text = "0";
            //this.txtWithAdj.Text = "0";

            double dNet = 0;
            dNet = Convert.ToDouble(this.txtTotalAmnt.Text) - Convert.ToDouble(this.txtDisAmnt.Text) - Convert.ToDouble(this.txtWithAdj.Text);
            this.txtNet.Text = Convert.ToString(dNet);
            //}

        }
       
    }

    //DISCOUNT AMOUNT
    protected void txtDisAmnt_TextChanged(object sender, EventArgs e)
    {
        txtDisAmnt_TextChanged();
    }
    private void txtDisAmnt_TextChanged()
    {
        double dNet = 0;
        if (this.txtDisAmnt.Text == "")
        {
            Response.Write("Please enter Quantity");
            //lblQty.Text = "Please enter discount amount.";
        }
        else
        {
            //lblQty.Text = "";
            if (txtDisAmnt.Text.Length == 0)
            {
                this.txtDisAmnt.Text = "0";
            }
            if (txtTotalAmnt.Text.Length == 0)
            {
                this.txtTotalAmnt.Text = "0";
            }
            if (txtWithAdj.Text.Length == 0)
            {
                this.txtWithAdj.Text = "0";
            }

            dNet = Convert.ToDouble(this.txtTotalAmnt.Text) - Convert.ToDouble(this.txtDisAmnt.Text) - Convert.ToDouble(this.txtWithAdj.Text);
            this.txtNet.Text = Convert.ToString(dNet);
        }
    }
    //WITHDRAWN OR ADJUSTMENT AMOUNT
    protected void txtWithAdj_TextChanged(object sender, EventArgs e)
    {
        double dNet = 0;
        if (this.txtWithAdj.Text == "")
        {
            Response.Write("Please enter Withdrawn or Adjustment Amount.");
        }
        else
        {
            if (txtDisAmnt.Text.Length == 0)
            {
                this.txtDisAmnt.Text = "0";
            }
            if (txtTotalAmnt.Text.Length == 0)
            {
                this.txtTotalAmnt.Text = "0";
            }
            if (txtWithAdj.Text.Length == 0)
            {
                this.txtWithAdj.Text = "0";
            }
            dNet = Convert.ToDouble(this.txtTotalAmnt.Text) - Convert.ToDouble(this.txtDisAmnt.Text) - Convert.ToDouble(this.txtWithAdj.Text);
            this.txtNet.Text = Convert.ToString(dNet);
        }
    }

    //FUNCTION FOR LOAD MRSR NO.
    protected void fnLoadMRSRNo()
    {
        SqlConnection conn = DBConnection.GetConnection();
        string sSql = "";
        Double sMRSRNo = 0;

        sSql = "";
        //sSql = "SELECT MAX(CAST(InvoiceNO AS INT)) AS SLNmbr FROM MRSRMaster" +
        sSql = "SELECT MAX(CAST(LEFT(MRSRCode,LEN(MRSRCode)-1) AS INT)) AS SLNmbr FROM MRSRMaster" +
            " WHERE TrType=3";
        // AND RIGHT(MRSRCode,1)<>'S'
        //AND OutSource='" + Session["sBrId"] + "'";

        SqlCommand cmd = new SqlCommand(sSql, conn);
        conn.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        try
        {
            if (dr.Read())
            {
                sMRSRNo = Convert.ToDouble(dr["SLNmbr"].ToString()) + 1;
                this.txtMRSR.Text = Convert.ToString(sMRSRNo);
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

    //FINALLY SAVE DATA
    protected void btnSave_Click(object sender, EventArgs e)
    {
        Session["__logID__"] = "0";
        LogFile(txtOnlineOrder.Text.Trim().ToString(), txtCHNo.Text.Trim().ToString(), "You click save button for invoice", "", "", false, false, false);
       
        if (System.Convert.ToInt32(Session["Vis"]) == 0)
        {  Session["__logID__"] = "0";
            Response.Redirect("../Account/Login.aspx");
        }

        SqlConnection conn = DBConnection.GetConnection();
        SqlConnection connSMS = DBConnectionSMS.GetConnection();

        string sSql = "";

        if (Session["sBrId"] == "0")
        {
            PopupMessage("Please LogIn again.", btnSave);
            return;
        }

        //CHALLAN NUMBER       
        if (txtCHNo.Text == "")
        {
            PopupMessage("Please enter Challan #.", btnSave);
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

        if (this.txtCustContact.Text.Length == 0)
        {
            PopupMessage("Please enter customer contact number.", btnSave);
            txtCustContact.Focus();
            return;
        }


        //DUE AMNT       
        if (txtDue.Text != "0")
        {
            PopupMessage("Please enter full payment ...", btnSave);
            txtPay.Focus();
            return;
        }

        //DUE AMNT       
        if (ddlSource.SelectedItem.Text == "None")
        {
            PopupMessage("Please select source of information field ...", btnSave);
            txtPay.Focus();
            return;
        }
        else if (txtOrderNo.Text.Length > 0 && ddlSource.SelectedItem.Text != "None")
        {
            ddlSource.SelectedItem.Text = "Facebook/Instagram/Twitter";
        }

        //Avail Amnt for GP, Banglalink, Nagad       
        //if (txtAvailAmount.Text != "")
        //{
        //    if (btnAvailOffer.Enabled)
        //    {

        //        PopupMessage("Please Avail Offer first ...", btnAvailOffer);
        //        btnAvailOffer.Focus();
        //        return;
        //    }
        //}

        //GRIDVIEW DATA VALIDATION
        int totalRowsCount = gvUsers.Rows.Count;
        if (totalRowsCount == 0)
        {
            PopupMessage("There is no product in list. Please add product.", btnSave);
            return;
        }
        SqlDateTime sqldatenull;
        sqldatenull = SqlDateTime.Null;
        tDate = Convert.ToDateTime(this.txtDate.Text);
        if (this.txtIssueDate.Text.Length == 0)
        {
            tChequeDate = DateTime.Now;
            //tChequeDate = sqldatenull;
        }
        else
        {
            tChequeDate = Convert.ToDateTime(this.txtIssueDate.Text);
        }


        //**********************************************************************
        //LOAD AUTO BILL NO.
        fnLoadAutoBillNo();

        //----------------------------------------------------------------------
        //CHECK DUPLICATE CHALLAN NO.
        sSql = "";
        sSql = "SELECT MRSRMID FROM MRSRMaster" +
            " WHERE MRSRCode='" + this.txtCHNo.Text + "'" +
            //" WHERE MRSRCode='" + this.txtMRSR.Text + 'S' + "'" +
            " AND TrType=3";
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
       
        LogFile(txtOnlineOrder.Text.Trim().ToString(), txtCHNo.Text.Trim().ToString(), "Check Ordernum already have in db", "", "", false, false, false);
        if (txtOrderNo.Text.Length > 0)
        {
            //----------------------------------------------------------------------
            //CHECK DUPLICATE ORDER NO.
            sSql = "";
            sSql = "SELECT POCode FROM MRSRMaster" +
                " WHERE POCode='" + this.txtOrderNo.Text + "'" +
                //" WHERE MRSRCode='" + this.txtMRSR.Text + 'S' + "'" +
                " AND TrType=3";
            cmdd = new SqlCommand(sSql, conn);
            conn.Open();
            drd = cmdd.ExecuteReader();
            try
            {
                if (drd.Read())
                {
                    //iMRSRID = Convert.ToInt32(dr["MRSRMID"].ToString());
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert",
                                "<script>alert('" + "This Order No. already exists." + "');</script>", false);
                    txtOrderNo.Focus();
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
        }


        //----------------------------------------------------------------------

        string sProfession = "";
        if (optProfession.SelectedItem.Text == "Business")
        {
            sProfession = "Business";
        }
        else if (optProfession.SelectedItem.Text == "Service")
        {
            sProfession = "Service";
        }
        else
        {
            sProfession = "Others";
        }

        string sSex = "";
        if (optSex.SelectedItem.Text == "Male")
        {
            sSex = "Male";
        }
        else if (optSex.SelectedItem.Text == "Female")
        {
            sSex = "Female";
        }
        else
        {
            sSex = "N/A";
        }


        //CHECK & INSERT CUSTOMER INFO
        sSql = "";
        sSql = "SELECT * FROM Customer" +
            " WHERE Mobile='" + this.txtCustContact.Text + "'";
        SqlCommand cmdCust = new SqlCommand(sSql, conn);
        conn.Open();
        SqlDataReader drCust = cmdCust.ExecuteReader();
        try
        {
            if (drCust.Read())
            {
                sSql = "";
                sSql = "UPDATE Customer SET";
                sSql = sSql + " CustName='" + this.txtCustName.Text.Replace("'", "''") + "',";
                sSql = sSql + " Address='" + this.txtCustAdd.Text.Replace("'", "''") + "',";
                sSql = sSql + " City='" + this.ddlCity.SelectedItem.Text + "',";
                sSql = sSql + " Email='" + this.txtEmail.Text + "',";
                sSql = sSql + " Profession='" + sProfession + "',";
                sSql = sSql + " Org='" + this.txtOrg.Text.Replace("'", "''") + "',";
                sSql = sSql + " Desg='" + this.txtDesg.Text.Replace("'", "''") + "',";
                sSql = sSql + " CustSex='" + sSex + "',";
                sSql = sSql + " DOBT='" + txtDOB.Text + "',";
                sSql = sSql + " eID='" + Session["EID"] + "',";
                sSql = sSql + " CustType='Regular',";
                sSql = sSql + " IdentityType='N/A', IdentityNo='N/A'";
                sSql = sSql + " WHERE Mobile='" + this.txtCustContact.Text + "'";
                SqlCommand cmdU = new SqlCommand(sSql, conn1);
                conn1.Open();
                cmdU.ExecuteNonQuery();
                conn1.Close();

            }
            else
            {
                sSql = "";
                sSql = "INSERT INTO Customer(Mobile,CustName,Address,City,eID,CustType," +
                       "Email,Profession, Org, Desg,CustSex,IdentityType,IdentityNo,DOBT)" +
                        " Values ('" + this.txtCustContact.Text + "','" + this.txtCustName.Text.Replace("'", "''") + "'," +
                        " '" + this.txtCustAdd.Text.Replace("'", "''") + "','" + this.ddlCity.SelectedItem.Text + "'," +
                        " '" + Session["EID"] + "','Regular'," +
                        " '" + this.txtEmail.Text + "','" + sProfession + "'," +
                        " '" + this.txtOrg.Text.Replace("'", "''") + "','" + this.txtDesg.Text.Replace("'", "''") + "'," +
                        " '" + sSex + "','N/A'," +
                        " 'N/A', '" + txtDOB.Text + "'" +
                        " )";
                SqlCommand cmdC = new SqlCommand(sSql, conn1);
                conn1.Open();
                cmdC.ExecuteNonQuery();
                conn1.Close();
            }
        }
        catch (InvalidCastException err)
        {
            throw (err);
        }
        finally
        {
            drCust.Dispose();
            drCust.Close();
            conn.Close();
        }
        //----------------------------------------------------------------------

        double dTAmnt = 0;
        if (this.txtNetAmnt.Text == "")
        {
            dTAmnt = 0;
        }
        else
        {
            dTAmnt = Convert.ToDouble(this.txtNetAmnt.Text);
        }
        double dTPay = 0;
        if (this.txtPay.Text == "")
        {
            dTPay = 0;
        }
        else
        {
            dTPay = Convert.ToDouble(this.txtPay.Text);
        }
        double dTDue = 0;

        if (this.txtDue.Text == "")
        {
            dTDue = 0;
        }
        else
        {
            dTDue = Convert.ToDouble(this.txtDue.Text);
        }

        if (this.txtCardAmnt1.Text.Length == 0)
        {
            this.txtCardAmnt1.Text = "0";
        }

        if (this.txtCardAmnt2.Text.Length == 0)
        {
            this.txtCardAmnt2.Text = "0";
        }

        if (this.txtCash.Text.Length == 0)
        {
            this.txtCash.Text = "0";
        }

        if (this.txtDue.Text.Length == 0)
        {
            this.txtDue.Text = "0";
        }


        if (ddlEntity.SelectedItem.Value == Session["sBrId"].ToString())
        {
            iDelTag = 1;
        }
        else
        {
            iDelTag = 2;
        }

        //-----------------------------------------------------------------
        // T & C
        txtTC.Text = "";
        int iCount = 1;
        string k = "";
        for (int i = 0; i < chkTC.Items.Count; i++)
        {
            if (chkTC.Items[i].Selected)
            {
                k = k + iCount + ". " + chkTC.Items[i].Text + "\n";
                iCount = iCount + 1;
            }
        }
        txtTC.Text = k;
        //-----------------------------------------------------------------


        DateTime aDate = DateTime.Now;
        try
        {
            //LOAD AUTO BILL NO.
            fnLoadAutoBillNo();

            int emeiCharge = 0;
            double EMEITenure = 0;

            double EMIDownPayment = 0;
            if (this.txtEMIDownPayment.Text == "")
            {
                this.txtEMIDownPayment.Text = "0";
            }
            if (Convert.ToInt32(this.txtEMIDownPayment.Text) > 0)
            {
                EMIDownPayment = Convert.ToDouble(this.txtEMIDownPayment.Text);
            }
            double EMIMonthlyToPay = 0;

            if (ddlEMEIInfo.SelectedItem.Text == "3 Months")
            {
                emeiCharge = 0;
                EMEITenure = 3;
            }
            if (ddlEMEIInfo.SelectedItem.Text == "6 Months")
            {
                emeiCharge = 0;
                EMEITenure = 6;
            }
            if (ddlEMEIInfo.SelectedItem.Text == "7 Months")
            {
                emeiCharge = 0;
                EMEITenure = 7;
            }
            if (ddlEMEIInfo.SelectedItem.Text == "9 Months")
            {
                emeiCharge = 0;
                EMEITenure = 9;
            }
            else if (ddlEMEIInfo.SelectedItem.Text == "12 Months")
            {
                emeiCharge = 0;
                EMEITenure = 12;
            }
            else if (ddlEMEIInfo.SelectedItem.Text == "18 Months")
            {
                emeiCharge = 4;
                EMEITenure = 18;
            }
            else if (ddlEMEIInfo.SelectedItem.Text == "24 Months")
            {
                emeiCharge = 6;
                EMEITenure = 24;
            }
            else if (ddlEMEIInfo.SelectedItem.Text == "36 Months")
            {
                emeiCharge = 10;
                EMEITenure = 36;
            }
            else if (ddlEMEIInfo.SelectedItem.Text == "0 Month")
            {
                emeiCharge = 0;
                EMEITenure = 0;
            }

            if (EMEITenure > 0)
            {
                EMIMonthlyToPay = (dTAmnt - EMIDownPayment) / EMEITenure;
            }
            else
            {
                EMIMonthlyToPay = 0;
            }



            //      ,[EMEI]
            //,[EMEIChargeOnPercent]
            //,[EMIDownPayment],,[EMIDownPayment],[EMEITenure]
            //,[EMIMonthyToPay]
            //SAVE DATA IN MASTER TABLE
            
            LogFile(txtOnlineOrder.Text.Trim().ToString(), txtCHNo.Text.Trim().ToString(), " before Save data in MrsrMaster Table", "", "", false, false, false);

            sSql = "";
            sSql = "INSERT INTO MRSRMaster(MRSRCode,TDate,TrType," +
                   "InvoiceNo,InSource,OutSource," +
                   "PayAmnt,DueAmnt,PayMode," +
                   "Customer,UserID,EntryDate," +
                   "NetSalesAmnt,TermsCondition," +
                   "CashAmnt,CardAmnt1,CardAmnt2," +
                   "CardNo1,CardNo2,CardType1,CardType2," +
                   "Bank1,Bank2,SecurityCode,SecurityCode2," +
                   "AppovalCode1,AppovalCode2,OnLineSales," +
                   "Authorby,Issby,DeliveryFrom,Remarks,Tag,RefCHNo,POCode,SourceOfInfo,EMEI,EMEIChargeOnPercent,EMIDownPayment,EMIMonthlyToPay,EMEITenure,DelivaryCharge" +
                   " )" +
                " Values ('" + this.txtCHNo.Text + "','" + tDate + "','3'," +
                " '" + this.txtCHNo.Text + "','230','" + Session["sBrId"] + "'," +
                " '" + dTPay + "','" + dTDue + "','" + this.ddlPayType.Text + "'," +
                " '" + this.txtCustContact.Text + "', '" + Session["UserName"] + "', '" + DateTime.Now + "'," +
                " '" + dTAmnt + "','" + this.txtTC.Text.Replace("'", "''") + "'," +
                " '" + this.txtCash.Text + "','" + this.txtCardAmnt1.Text + "','" + this.txtCardAmnt2.Text + "'," +
                " '" + this.txtChequeNo.Text + "','" + this.txtChequeNo2.Text + "','" + this.ddlCardType1.SelectedItem.Text + "','" + this.ddlCardType2.SelectedItem.Text + "'," +
                " '" + this.txtBankName.Text + "','" + this.txtBankName2.Text + "','" + this.txtSecurityCode.Text + "','" + this.txtSecurityCode2.Text + "'," +
                " '" + this.txtApprovalCode1.Text + "','" + this.txtApprovalCode2.Text + "',1," +
                " '" + this.txtRefBy.Text + "','" + this.txtJobID.Text + "'," +
                " '" + this.ddlEntity.SelectedItem.Value + "','" + this.txtNote.Text.Replace("'", "''") + "'," +
                " '" + iDelTag + "','" + txtRefChNo.Text + "','" + txtOrderNo.Text + "','" + ddlSource.SelectedItem.Text + "'," +
                " '" + ddlEMEIInfo.SelectedItem.Text + "','" + emeiCharge + "','" + EMIDownPayment + "','" + EMIMonthlyToPay + "','" + EMEITenure + "','" + ViewState["__DelivariCharge__"].ToString()+ "'" +
            " )";

            SqlCommand cmd = new SqlCommand(sSql, conn);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            //lblMessage.Text = "Done";
          
            LogFile(txtOnlineOrder.Text.Trim().ToString(), txtCHNo.Text.Trim().ToString(), "Save data in MrsrMaster Table", "", "", false, false, false);

            //RETRIVE MASTER ID         
            sSql = "";
            sSql = "SELECT MRSRMID FROM MRSRMaster" +
                " WHERE MRSRCode='" + this.txtCHNo.Text + "'" +
                " AND TrType=3";
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


            //---------------------- SAVE DATA MRSRDetais --------------------------------------

            int redeemAmount = 0;

            // SAVE Discount Info
            //if (txtDiscountAmount.Text != "")
            //{
            //    redeemAmount = Convert.ToInt32(txtDiscountAmount.Text);
            //}

            //string redeemCode = txtCouponCode.Text;

            string reference = "yes"; string extraRef = "yes";

            List<int> productIds = new List<int>();//this is for lg some specific model wise voucher redeem

            String[] newdis = new string[] {"RANGS AC", "KELVINATOR AC"};


            String[] kelventorRef = new string[] { "KHV-516FF", "KHV-401FFG1", "KHV-475NF4DBG", "KHV-418NFSBS2DBG", "KHV-422WDNFSBS2D", "KHV-645NFSBS2DMG"};

            String[] rangsKelventor = new string[] { "43UP7550", "55UN731C", "OLED65A1", "OLED55C2", "KD-43X75K", "KHV-428NFIN", "KHV-365FF", "KHV-333FF" };

            String[] whirpoolgroup = new string[] { "Magicook Pro 30GE", "Magicook Elite 30L", "Fresh Magic Pro 236L Chromium Steel", "Fresh Magic Pro 257L Crystal Black", "Fresh Magic Pro 278L Crystal Black", "Neo fresh Inverter 257L Crystal Black", "Intellifresh Inverter 278 Steel Onyx", "Stainwash ULTRA 7.5KG", "PRO H Graphite 360 Bloomwash 9.5KG" };



            //bool IsAllowedForRedeem = false;

            //bool isAllowed = false;
            //bool isAllowedreidm = false;
            //bool isAllowedWhilpool = false;

            foreach (GridViewRow g1 in this.gvUsers.Rows)
            {
                //if (newdis.Contains(GetProductGroupName(g1.Cells[1].Text.Trim()))) 
                //{
                //    IsAllowedForRedeem = true;
                //}

                //if (g1.Cells[8].Text.Trim() == "Online Order")
                //{

                //    if (kelventorRef.Contains((g1.Cells[1].Text.Trim())))
                //    {
                //        isAllowed = true;
                //    }
                //    else if (rangsKelventor.Contains((g1.Cells[1].Text.Trim())))
                //    {
                //        isAllowedreidm = true;
                //    }

                //    else if (whirpoolgroup.Contains((g1.Cells[1].Text.Trim())))
                //    {
                //        isAllowedWhilpool = true;
                //    }

                //}
               



                string sdiscode = "";
                if (g1.Cells[7].Text.Trim() != "&nbsp;")
                {
                    sdiscode = g1.Cells[7].Text.Trim();
                }
                else
                {
                    sdiscode = g1.Cells[7].Text = "";
                }

                string sDisRef = "";
                if (g1.Cells[8].Text.Trim() != "&nbsp;")
                {
                    sDisRef = g1.Cells[8].Text.Trim();
                }
                else
                {
                    sDisRef = g1.Cells[8].Text = "";
                }

                string sProdSL = "";
                if (g1.Cells[11].Text.Trim() != "&nbsp;")
                {
                    sProdSL = g1.Cells[11].Text.Trim();
                }
                else
                {
                    sProdSL = g1.Cells[11].Text = "";
                }

                string sRemarks = "";
                if (g1.Cells[12].Text.Trim() != "&nbsp;")
                {
                    sRemarks = g1.Cells[12].Text.Trim();
                }
                else
                {
                    sRemarks = g1.Cells[12].Text = "";
                }

                string sBLAMNT = "";
                if (g1.Cells[13].Text.Trim() != "&nbsp;")
                {
                    sBLAMNT = g1.Cells[13].Text.Trim();
                }
                else
                {
                    sBLAMNT = g1.Cells[13].Text = "0";
                }

                string sIncType = "";
                if (g1.Cells[15].Text.Trim() != "&nbsp;")
                {
                    sIncType = g1.Cells[15].Text.Trim();
                }
                else
                {
                    sIncType = g1.Cells[15].Text = "";
                }
                // ------------- Get Discount Amount ------------------------
                string discountAmnt = "";
                if (g1.Cells[6].Text.Trim() != "&nbsp;")
                {
                    discountAmnt = g1.Cells[6].Text.Trim();
                }
                else
                {
                    discountAmnt = g1.Cells[6].Text = "0";
                }

                // ------------- Send SMS to Reference ------------------------

                if (reference == "yes" && sDisRef != "")
                {
                    if (sDisRef == "GM Sir" || sDisRef == "DGM Sir" || sDisRef == "Nipa Madam")
                    {
                        sendReferenceSMS(sDisRef, Convert.ToInt32(g1.Cells[6].Text));
                        reference = "no";
                    }
                }


                //-------------- Compare Redeem price with Model price ----------------------------
                string productPrice = "";
                int redeemAmt = 0;
                string discountCode = "";
                if (g1.Cells[10].Text.Trim() != "&nbsp;")
                {
                    productPrice = g1.Cells[10].Text.Trim();
                }
                else   
                {
                    productPrice = g1.Cells[10].Text = "";
                }

                if (Convert.ToInt32(productPrice) >= redeemAmount && redeemAmount > 0)
                {
                    //redeemAmt = redeemAmount;
                    //discountCode = redeemCode;

                    //// Send SMS confirmation
                    //sendConfirmRedeemSMS(txtCHNo.Text, redeemAmount, redeemCode);

                    //redeemAmount = 0;
                    //redeemCode = "";
                }

                // ------------- Add GP/B.Link & Nagad Reference ------------------------

                if (extraRef == "yes" && ddlReference.SelectedValue != "0")
                {
                    if (Convert.ToInt32(productPrice) >= Convert.ToInt32(txtAvailAmount.Text))
                    {
                        if (ddlReference.SelectedValue == "GP Star" && Convert.ToInt32(txtAvailAmount.Text) > 1000)
                        {
                            string sSqlGp = "";
                            sSqlGp = 
                                    @"select TDate,Customer,MRSRDetails.ProdRemarks,MRSRDetails.DiscountAmnt,MRSRDetails.DisRef from MRSRMaster inner join MRSRDetails on MRSRMaster.MRSRMID=MRSRDetails.MRSRMID 
                                    where ProdRemarks like '%GP Star%' and MRSRDetails.DiscountAmnt>1000 
                                    and cast(MRSRMaster.TDate as date)>='2023-01-05' 
                                    and DATEDIFF(DAY,GETDATE(),MRSRMaster.TDate)<=30 
                                    and Customer like '%" + this.txtCustContact + "%' order by TDate desc";
                            //2023-01-05 : gp statr started on this date.in 1 month a customer cant avail gp star twice
                            //may it need to change depends on campaign circular

                            SqlCommand cmdGP = new SqlCommand(sSqlGp, conn);
                            conn.Open();
                            SqlDataReader drGP = cmdGP.ExecuteReader();
                            if (drGP.Read())
                            {
                                PopupMessage("This customer: "+this.txtCustContact.ToString()+" already availed gp star discount.Thanks #.", btnSave);
                                btnSave.Focus();
                                return;
                                //hMRSRID = Convert.ToInt32(dr["HisMID"].ToString());
                                //Session["sBrId"] = Convert.ToInt16(dr["EID"].ToString());
                            }
                            else
                            {
                                sRemarks = ddlReference.SelectedValue + ".";//added dot to identify model wise big amount gp star discount
                                extraRef = "no";
                            }
                            drGP.Dispose();
                            drGP.Close();
                            conn.Close();                            
                        }
                        else 
                        {
                            int totalDiscount = Convert.ToInt32(discountAmnt) + Convert.ToInt32(txtAvailAmount.Text);
                            sRemarks = ddlReference.SelectedItem.Text;
                            extraRef = "no";

                            discountAmnt = totalDiscount.ToString();

                            int netAmnt = Convert.ToInt32(productPrice) - Convert.ToInt32(txtAvailAmount.Text);
                            productPrice = netAmnt.ToString();
                        }
                        
                    }
                }

                // ------------------------------ End Comparing ------------------------------------

                double dIncAmnt = 0;
                double dTAmnt1 = Convert.ToDouble(g1.Cells[3].Text) * Convert.ToDouble(g1.Cells[4].Text);
                double dTBLAmnt1 = Convert.ToDouble(sBLAMNT) * Convert.ToDouble(g1.Cells[4].Text);
                if (dTBLAmnt1 > 0)
                {
                    dIncAmnt = dTAmnt1 - dTBLAmnt1;
                }
             
                LogFile(txtOnlineOrder.Text.Trim().ToString(), txtCHNo.Text.Trim().ToString(), " before Save data in MrsrMaster Details Table", "", "", false, false, false);
                string gSql = "";
                gSql = "INSERT INTO MRSRDetails(MRSRMID,ProductID,Qty," +
                     " MRP,UnitPrice,TotalAmnt,DiscountAmnt," +
                     " SLNO,ProdRemarks,DisCode,DisRef," +
                     " WithAdjAmnt,RetPrice,NetAmnt," +
                     " BLIPAmnt,IncentiveAmnt,IncentiveType," +
                     " CustShowPrice,RedeemAmnt)" +
                     " VALUES('" + iMRSRID + "','" + g1.Cells[0].Text + "','" + '-' + g1.Cells[4].Text + "'," +
                      " '" + g1.Cells[2].Text + "','" + g1.Cells[3].Text + "','" + g1.Cells[5].Text + "','" + Convert.ToInt32(discountAmnt) + "'," +
                    //" '" + g1.Cells[2].Text + "','" + g1.Cells[3].Text + "','" + g1.Cells[5].Text + "','" + g1.Cells[6].Text + "'," +
                     " '" + sProdSL + "','" + sRemarks + "','" + sdiscode + "','" + sDisRef + "'," +
                     " '" + g1.Cells[9].Text + "','" + g1.Cells[2].Text + "','" + productPrice + "'," +
                    //" '" + g1.Cells[9].Text + "','" + g1.Cells[2].Text + "','" + g1.Cells[10].Text + "'," +
                     " '" + g1.Cells[13].Text + "','" + dIncAmnt + "','" + sIncType + "'," +
                     " '" + g1.Cells[16].Text + "','" + redeemAmt + "')";
                SqlCommand cmdIns = new SqlCommand(gSql, conn);

                productIds.Add(Convert.ToInt32(g1.Cells[0].Text));

                conn.Open();
                cmdIns.ExecuteNonQuery();
                conn.Close();

            }

            LogFile(txtOnlineOrder.Text.Trim().ToString(), txtCHNo.Text.Trim().ToString(), "Save data in MrsrMaster Details Table", "", "", false, false, false);
            //SAVE DATA IN CUSTOMER VOUCHER TABLE
            //normal approch


            string custSql = "";

            ///  Reddem oparetion is off 2023-07-09 /////////
            //int earnedVoucherPoint = 0, redeemVoucherPoint = 0; string refInvoiceNo = ""; int earnVoucher=0,earnVoucerforRedeem=0,earnVoucherWhilpool=0;
            string cardType = ddlCardType1.SelectedItem.Text;
            ViewState["__earnedVoucherPoint__"] = "0";

            ///  Reddem oparetion is off 2023-07-09 /////////

            //if (IsAllowedForRedeem)
            //{
            //    earnedVoucherPoint = 2;
            //    ViewState["__earnedVoucherPoint__"] = earnedVoucherPoint;

                //if (dTAmnt > 14999)
                //{

                //    if (dTAmnt >= 15000 && dTAmnt <= 24999) { earnedVoucherPoint = 1; }//1
                //    else if (dTAmnt >= 25000 && dTAmnt <= 44999) { earnedVoucherPoint = 2; }//2
                //    else if (dTAmnt >= 45000 && dTAmnt <= 64999) { earnedVoucherPoint = 2; }   //3             
                //    else if (dTAmnt >= 65000 && dTAmnt <= 84999) { earnedVoucherPoint = 2; }//4
                //    else if (dTAmnt >= 85000 && dTAmnt <= 104999) { earnedVoucherPoint = 2; }//5
                //    //else if () { earnedVoucherPoint = 6; }
                //    //else if (dTAmnt >= 70000 && dTAmnt <= 79999) { earnedVoucherPoint = 7; }
                //    //else if (dTAmnt >= 80000 && dTAmnt <= 89999) { earnedVoucherPoint = 8; }
                //    //else if (dTAmnt >= 90000 && dTAmnt <= 99999) { earnedVoucherPoint = 9; }
                //    else if (dTAmnt >= 105000 && dTAmnt <= 04999) { earnedVoucherPoint = 2; }//6
                //    //else if (cardType == "BKash" || cardType == "NAGAD") { earnedVoucherPoint = earnedVoucherPoint + 1; }
                //    else { earnedVoucherPoint = 0; }
                //}

            //}

            ///  Reddem oparetion is off 2023-07-09 /////////


           // if (isAllowed) 
           //   {
           //       earnVoucher = 4;
           //       ViewState["__earnedVoucherPoint__"] = earnVoucher;
           //   }

           //else if (isAllowedreidm)
           //   {
           //     earnVoucerforRedeem = 2;
           //     ViewState["__earnedVoucherPoint__"] = earnVoucerforRedeem;
           //   }
           // else if (isAllowedWhilpool) 
           //  {
           //      earnVoucherWhilpool = 1;
           //      ViewState["__earnedVoucherPoint__"] = earnVoucherWhilpool;
           //  }




           
            //currently normal feature is off
            //only for some specific lg models (6062,6022,6063,6002,5845,4934)
            //special case for some specific model
            //string custSql = ""; int earnedVoucherPoint = 0, redeemVoucherPoint = 0; string refInvoiceNo = "";
            //string cardType = ddlCardType1.SelectedItem.Text;
            ////6002,6018,6001,6019

            ////--1 43UN731C,6022 4K UHD Smart TV 79900 36 50900 Cash Voucher of BDT 2000/- 
            ////--2 50UP7750,6219 4K UHD Smart TV 99900 20 79900 Cash Voucher of BDT 3000/- 
            ////--3 55UN731C,6062 4K UHD Smart TV 144900 31 99900 Cash Voucher of BDT 5000/- 
            ////--4 65UN731C,6063 4K UHD Smart TV 209900 38 129900 Cash Voucher of BDT 10000/- 
            ////--5 55NANO75,6202 4K NanoCell TV 239900 40 144900 Cash Voucher of BDT 2000/- 
            ////--6 65NANO86,6220 4K NanoCell TV 374900 36 239900 Cash Voucher of BDT 7500/- 
            ////--7 75NANO75,6205 4K NanoCell TV 424900 35 274900 Cash Voucher of BDT 10000/- 
            ////--8 OLED65A1,6204 OLED 464900 35 299900 Cash Voucher of BDT 10000/- 
            ////--9 OLED77C1,6222 OLED 849900 24 649900 Cash Voucher of BDT 10000/
            //if (dTAmnt > 14999)
            //{
            //    //6202,6062,6321,6220,6063,6205,6204
            //    //55UN731C-6062, 55UP7550-6321, 65UN731C -6063- 2000/- 
            //    //55NANO75 -6202 - 3000/- 
            //    //65NANO86-6220, OLED65A1-6204= 4000/- 
            //    //75NANO75 - 6205 - 5000/-
            //    foreach (var item in productIds)
            //    {
            //        if (item == 6202 || item == 6062 || item == 6321 || item == 6220
            //            || item == 6063 || item == 6205 || item == 6204)//3701,//|| item == 6018 || item == 6001 || item == 6019
            //        {
            //            //if (item == 60220000) { earnedVoucherPoint += 4; }
            //            if (item == 6062) { earnedVoucherPoint += 4; } 
            //            else if (item == 6321) { earnedVoucherPoint += 4; }
            //            else if (item == 6063) { earnedVoucherPoint += 4; }
            //            else if (item == 6202) { earnedVoucherPoint += 6; }
            //            else if (item == 6220) { earnedVoucherPoint += 8; }
            //            else if (item == 6204) { earnedVoucherPoint += 8; }
            //            else if (item == 6205) { earnedVoucherPoint += 10; }
            //            //else if (item == 6204) { earnedVoucherPoint += 20; }//OLED65A1
            //            //else if (item == 6222) { earnedVoucherPoint += 20; }//OLED77C1
            //            //else if (item == 6320) { earnedVoucherPoint += 4; }//43UP7550
            //            //else if (item == 6002) { earnedVoucherPoint += 10; }
            //            //else if (item == 6018) { earnedVoucherPoint += 10; }
            //            //else if (item == 6001) { earnedVoucherPoint += 20; }
            //            //else if (item == 6019) { earnedVoucherPoint += 20; }
            //            //else if () { earnedVoucherPoint = 6; }
            //            //else if (dTAmnt >= 70000 && dTAmnt <= 79999) { earnedVoucherPoint = 7; }
            //            //else if (dTAmnt >= 80000 && dTAmnt <= 89999) { earnedVoucherPoint = 8; }
            //            //else if (dTAmnt >= 90000 && dTAmnt <= 99999) { earnedVoucherPoint = 9; }
            //            //else if (dTAmnt >= 105000) { earnedVoucherPoint = 6; }
            //            //else if (cardType == "BKash" || cardType == "NAGAD") { earnedVoucherPoint = earnedVoucherPoint + 1; }
            //            else { earnedVoucherPoint = 0; }
            //        }
            //    }
            //    if (earnedVoucherPoint > 0)
            //    {
            //        //custSql = "INSERT INTO tbCustomerVoucher(CustomerName, CustomerAddress, CustomerContact, InvoiceNo," +
            //        //    "BillAmount, EarnedVoucherPoint, RedeemVoucherPoint, RefInvoiceNo )" +
            //        //    " Values ('" + this.txtCustName.Text + "','" + this.txtCustAdd.Text + "', '" + this.txtCustContact.Text + "', '" + this.txtCHNo.Text + "'," +
            //        //    " '" + dTAmnt + "','" + earnedVoucherPoint + "','" + redeemVoucherPoint + "','" + refInvoiceNo + "')";

            //        //SqlCommand cmdCustomer = new SqlCommand(custSql, conn);
            //        //conn.Open();
            //        //cmdCustomer.ExecuteNonQuery();
            //        //conn.Close();
            //    }
            //}

            //break;




            ////normal procedure  for all model


            ///  Reddem oparetion is off 2023-07-09 /////////
            ///  

            //if (earnedVoucherPoint > 0)
            //{
            //    custSql = "INSERT INTO tbCustomerVoucher(CustomerName, CustomerAddress, CustomerContact, InvoiceNo," +
            //           "BillAmount, EarnedVoucherPoint, RedeemVoucherPoint, RefInvoiceNo )" +
            //        " Values ('" + this.txtCustName.Text + "','" + this.txtCustAdd.Text + "', '" + this.txtCustContact.Text + "', '" + this.txtCHNo.Text + "'," +
            //        " '" + dTAmnt + "','" + earnedVoucherPoint + "','" + redeemVoucherPoint + "','" + refInvoiceNo + "')";

            //    SqlCommand cmdCustomer = new SqlCommand(custSql, conn);
            //    conn.Open();
            //    cmdCustomer.ExecuteNonQuery();
            //    conn.Close();
               
            //}

            //if (earnVoucher > 0)
            //{
            //    custSql = "INSERT INTO tbCustomerVoucher(CustomerName, CustomerAddress, CustomerContact, InvoiceNo," +
            //           "BillAmount, EarnedVoucherPoint, RedeemVoucherPoint, RefInvoiceNo )" +
            //        " Values ('" + this.txtCustName.Text + "','" + this.txtCustAdd.Text + "', '" + this.txtCustContact.Text + "', '" + this.txtCHNo.Text + "'," +
            //        " '" + dTAmnt + "','" + earnVoucher + "','" + redeemVoucherPoint + "','" + refInvoiceNo + "')";

            //    SqlCommand cmdCustomer = new SqlCommand(custSql, conn);
            //    conn.Open();
            //    cmdCustomer.ExecuteNonQuery();
            //    conn.Close();

            //}
            //if (earnVoucerforRedeem > 0)
            //{
            //    custSql = "INSERT INTO tbCustomerVoucher(CustomerName, CustomerAddress, CustomerContact, InvoiceNo," +
            //           "BillAmount, EarnedVoucherPoint, RedeemVoucherPoint, RefInvoiceNo )" +
            //        " Values ('" + this.txtCustName.Text + "','" + this.txtCustAdd.Text + "', '" + this.txtCustContact.Text + "', '" + this.txtCHNo.Text + "'," +
            //        " '" + dTAmnt + "','" + earnVoucerforRedeem + "','" + redeemVoucherPoint + "','" + refInvoiceNo + "')";

            //    SqlCommand cmdCustomer = new SqlCommand(custSql, conn);
            //    conn.Open();
            //    cmdCustomer.ExecuteNonQuery();
            //    conn.Close();

            //}

            //if (earnVoucherWhilpool > 0)
            //{
            //    custSql = "INSERT INTO tbCustomerVoucher(CustomerName, CustomerAddress, CustomerContact, InvoiceNo," +
            //           "BillAmount, EarnedVoucherPoint, RedeemVoucherPoint, RefInvoiceNo )" +
            //        " Values ('" + this.txtCustName.Text + "','" + this.txtCustAdd.Text + "', '" + this.txtCustContact.Text + "', '" + this.txtCHNo.Text + "'," +
            //        " '" + dTAmnt + "','" + earnVoucherWhilpool + "','" + redeemVoucherPoint + "','" + refInvoiceNo + "')";

            //    SqlCommand cmdCustomer = new SqlCommand(custSql, conn);
            //    conn.Open();
            //    cmdCustomer.ExecuteNonQuery();
            //    conn.Close();

            //}

        }
        catch
        {
            //
        }

        //######################################################################################
        //SAVE DATA IN HISTORY MASTER TABLE
        try
        {
            sSql = "";
            sSql = "INSERT INTO HistoryMaster(MRSRMID,MRSRCode,TDate,TrType," +
                   "InvoiceNo,InSource,OutSource," +
                   "PayAmnt,DueAmnt,PayMode," +
                   "Customer,UserID,EntryDate," +
                   "NetSalesAmnt,TermsCondition," +
                   "CashAmnt,CardAmnt1,CardAmnt2," +
                   "CardNo1,CardNo2,CardType1,CardType2," +
                   "Bank1,Bank2,SecurityCode,SecurityCode2," +
                   "AppovalCode1,AppovalCode2,OnLineSales," +
                   "Authorby,Issby,DeliveryFrom,Remarks,Tag,RefCHNo,POCode,SourceOfInfo" +
                   " )" +
                " Values ('" + iMRSRID + "','" + this.txtCHNo.Text + "','" + tDate + "','3'," +
                " '" + this.txtCHNo.Text + "','230','" + Session["sBrId"] + "'," +
                " '" + dTPay + "','" + dTDue + "','" + this.ddlPayType.Text + "'," +
                " '" + this.txtCustContact.Text + "', '" + Session["UserName"] + "', '" + aDate.ToString("MM/dd/yyyy hh:mm tt") + "'," +
                " '" + dTAmnt + "','" + this.txtTC.Text.Replace("'", "''") + "'," +
                " '" + this.txtCash.Text + "','" + this.txtCardAmnt1.Text + "','" + this.txtCardAmnt2.Text + "'," +
                " '" + this.txtChequeNo.Text + "','" + this.txtChequeNo2.Text + "','" + this.ddlCardType1.SelectedItem.Text + "','" + this.ddlCardType2.SelectedItem.Text + "'," +
                " '" + this.txtBankName.Text + "','" + this.txtBankName2.Text + "','" + this.txtSecurityCode.Text + "','" + this.txtSecurityCode2.Text + "'," +
                " '" + this.txtApprovalCode1.Text + "','" + this.txtApprovalCode2.Text + "',1," +
                " '" + this.txtRefBy.Text + "','" + this.txtJobID.Text + "'," +
                " '" + this.ddlEntity.SelectedItem.Value + "','" + this.txtNote.Text.Replace("'", "''") + "'," +
                " '" + iDelTag + "','" + txtRefChNo.Text + "','" + txtOrderNo.Text + "','" + ddlSource.SelectedItem.Text + "'" +
            " )";
            //" CAST(" + this.lblNetAmnt.Text + " AS Numeric)";        
            // " )";
            SqlCommand cmdH = new SqlCommand(sSql, conn);
            conn.Open();
            cmdH.ExecuteNonQuery();
            conn.Close();
            //lblMessage.Text = "Done";


            //RETRIVE HISTORY MASTER ID  
            Int32 hMRSRID = 0;
            sSql = "";
            sSql = "SELECT  TOP (1) HisMID FROM HistoryMaster";
            sSql = sSql + " WHERE MRSRCode='" + this.txtCHNo.Text + "'";
            //sSql = sSql + " AND EntryDate='" + DateTime.Today + "'";
            //" WHERE MRSRCode='" + this.txtMRSR.Text + 'S' + "'" +
            sSql = sSql + " AND TrType=3";
            sSql = sSql + " GROUP BY HisMID, TrType";
            sSql = sSql + " ORDER BY HisMID DESC";
            SqlCommand cmd = new SqlCommand(sSql, conn);
            conn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                hMRSRID = Convert.ToInt32(dr["HisMID"].ToString());
                //Session["sBrId"] = Convert.ToInt16(dr["EID"].ToString());
            }
            dr.Dispose();
            dr.Close();
            conn.Close();


            //double dCampDis = 0;
            
            foreach (GridViewRow g1 in this.gvUsers.Rows)
            {
                
                string sDisCode = "";
                if (g1.Cells[7].Text.Trim() != "&nbsp;")
                {
                    sDisCode = g1.Cells[7].Text.Trim();
                }
                else
                {
                    sDisCode = g1.Cells[7].Text = "";
                }

                string sDisRef = "";
                if (g1.Cells[8].Text.Trim() != "&nbsp;")
                {
                    sDisRef = g1.Cells[8].Text.Trim();
                }
                else
                {
                    sDisRef = g1.Cells[8].Text = "";
                }

                string sProdSL = "";
                if (g1.Cells[11].Text.Trim() != "&nbsp;")
                {
                    sProdSL = g1.Cells[11].Text.Trim();
                }
                else
                {
                    sProdSL = g1.Cells[11].Text = "";
                }

                string sRemarks = "";
                if (g1.Cells[12].Text.Trim() != "&nbsp;")
                {
                    sRemarks = g1.Cells[12].Text.Trim();
                }
                else
                {
                    sRemarks = g1.Cells[12].Text = "";
                }

                string sIncType = "";
                if (g1.Cells[15].Text.Trim() != "&nbsp;")
                {
                    sIncType = g1.Cells[15].Text.Trim();
                }
                else
                {
                    sIncType = g1.Cells[15].Text = "";
                }

                double dIncAmnt = 0;
                double dTAmnt1 = Convert.ToDouble(g1.Cells[3].Text) * Convert.ToDouble(g1.Cells[4].Text);
                double dTBLAmnt1 = Convert.ToDouble(g1.Cells[13].Text) * Convert.ToDouble(g1.Cells[4].Text);
                if (dTBLAmnt1 > 0)
                {
                    dIncAmnt = dTAmnt1 - dTBLAmnt1;
                }

                string gSql = "";
                gSql = "INSERT INTO HistoryDetails(HisMID,MRSRMID,ProductID,Qty," +
                     " MRP,UnitPrice,TotalAmnt,DiscountAmnt," +
                     " SLNO,ProdRemarks,DisCode,DisRef," +
                     " WithAdjAmnt,RetPrice,NetAmnt," +
                     " BLIPAmnt,IncentiveAmnt,IncentiveType," +
                     " CustShowPrice)" +
                     " VALUES('" + hMRSRID + "','" + iMRSRID + "','" + g1.Cells[0].Text + "','" + '-' + g1.Cells[4].Text + "'," +
                     " '" + g1.Cells[2].Text + "','" + g1.Cells[3].Text + "','" + g1.Cells[5].Text + "','" + g1.Cells[6].Text + "'," +
                     " '" + sProdSL + "','" + sRemarks + "','" + sDisCode + "','" + sDisRef + "'," +
                     " '" + g1.Cells[9].Text + "','" + g1.Cells[2].Text + "','" + g1.Cells[10].Text + "'," +
                     " '" + g1.Cells[13].Text + "','" + dIncAmnt + "','" + sIncType + "'," +
                     " '" + g1.Cells[16].Text + "')";

                SqlCommand cmdInsH = new SqlCommand(gSql, conn);

                conn.Open();
                cmdInsH.ExecuteNonQuery();
                conn.Close();


                if (sDisRef != "")
                {
                    if (sDisRef == "GM Sir" || sDisRef == "DGM Sir" || sDisRef == "Nipa Madam")
                    {
                        fnSendMail_Invoice_test_marketing();
                    }
                }
            }

        }
        catch
        {
            //
        }

        //----------------------------------------------------------------
        //SAVE TERMS & CONDITIONS
        try
        {
            string strcbl1 = string.Empty;
            for (int i = 0; i < chkTC.Items.Count; i++)
            {
                //if (chkTC.Items[i].Selected == true)
                if (chkTC.Items[i].Selected)
                {
                    strcbl1 = strcbl1 + chkTC.Items[i].Text.ToString() + ",";

                    sSql = "";
                    sSql = "INSERT INTO tbTC_Customer(MRSRMID,InvoiceNo,TCText,tcAID)" +
                            " Values ('" + iMRSRID + "','" + this.txtCHNo.Text + "','" + chkTC.Items[i].Text + "','" + chkTC.Items[i].Value + "'" +
                            " )";
                    SqlCommand cmd2 = new SqlCommand(sSql, conn);
                    conn.Open();
                    cmd2.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }
        catch
        {
            //
        }
        //----------------------------------------------------------------
        //------------------------------------------------------------------------------------------


        //SEND MAIL TO CUSTOMER
        if (txtEmail.Text.Length > 0)
        {
            try
            {
               // fnSendMail_Invoice_test();
                //fnSendMail_Invoice();

                //sendUtsab_raffle(this.txtCHNo.Text.ToString());

            }
            catch (Exception ex)
            {
                MailMessage mM2 = new MailMessage();
                //mM2.From = new MailAddress(txtEmail.Text);        

                //mM2.From = new MailAddress("rangs.eshop@gmail.com");
                mM2.From = new MailAddress("dms@rangs.com.bd");
                //PW:Exampass@567

                //mM2.To.Add(new MailAddress(Session["sEmail"].ToString()));
                //mM2.To.Add(new MailAddress("golummohammedmohiuddin@gmail.com"));
                //mM2.CC.Add(new MailAddress(lblCTPEmail.Text));
                //mM2.Bcc.Add(new MailAddress("zunayedqu10@gmail.com"));
                //mM2.Bcc.Add(new MailAddress("minto@rangs.com.bd"));

                //mM2.Bcc.Add(new MailAddress("sofiqul@rangs.com.bd"));

                mM2.Subject = "error_email_sending";
                //mM2.Body = "<h1>Order Details</h1>";
                mM2.Body = "1." + ex.Message + "...2." + ex.InnerException;

                mM2.Body = mM2.Body + "<p>" + txtCHNo.Text.ToString() + "<br/>";
                //mM2.Body = mM2.Body + "We really appreciate it and we are taking necessary steps to process this order.";
                //mM2.Body = mM2.Body + "as soon as possible. You will be updated about next step immediate.";
                mM2.Body = mM2.Body + "</p>";
                mM2.IsBodyHtml = true;
                mM2.Priority = MailPriority.High;
                SmtpClient sC1 = new SmtpClient("mail.rangs.com.bd");
                sC1.Port = 587;
                //sC1.Port = 2525;
                sC1.Credentials = new System.Net.NetworkCredential("dms@rangs.com.bd", "Exampass@567");
                //sC.EnableSsl = true;
                sC1.Send(mM2);
                // Deal with an exception if one is thrown by the code in the try block
                //
            }
        }
        else
        {
          //  fnSendMail_Invoice_test();
        }

        //------------------------------------------------------------------------------------------
        // UPDATE ONLINE STORE  --- dStatus=3 for Delivered

        LogFile(txtOnlineOrder.Text.Trim().ToString(), txtCHNo.Text.Trim().ToString(), "Before Update Online Store", "", "", false, false, false);
        
        if (txtOrderNo.Text.Length > 0)
        {
            try
            {


  
                LogFile(txtOnlineOrder.Text.Trim().ToString(), txtCHNo.Text.Trim().ToString(), "Hit 5206 line in before call api", "", "", false,false,false);

                SqlConnection connRos = DBConnection_ROS.GetConnection();
                sSql = "";
                sSql = "UPDATE tbCustomerDelivery SET dStatus=3 WHERE DelNo='" + txtOrderNo.Text + "'";
                SqlCommand cmdRR = new SqlCommand(sSql, connRos);
                connRos.Open();
                cmdRR.ExecuteNonQuery();
                connRos.Close();

                Data.MainData response = Newshopapi(txtOrderNo.Text.Trim().ToString(), 1);

                if (response == null)
                {
                    PopupMessage("Does not hit api properly please deliverd it manually in online store", btnSave);
                }

                else
                {
                    if (response.massage == "delivered" && response.status=="200")
                    {
                        LogFile(txtOnlineOrder.Text.Trim().ToString(), txtCHNo.Text.Trim().ToString(), "Hit api and take correct response from api", "", "", false,false,true);
                    }
                    else
                    {
                        PopupMessage("Online order does not delivered", btnSave);
                        LogFile(txtOnlineOrder.Text.Trim().ToString(), txtCHNo.Text.Trim().ToString(), "Wrong response form api", "", "", false,     true,false);
                    }
 
                }

 




                //call logmethod after call api
               LogFile(txtOnlineOrder.Text.Trim().ToString(), txtCHNo.Text.Trim().ToString(), "Operation succesfully end", "", "", false,true,false);
                return;
            }
            catch
            {
                //
            }
        }


        if (TxtSpinCouponNumber.Text.Length > 0)
        {
            try
            {
                SqlConnection connSpin = DBConnectionSpin.GetConnection();
                sSql = "";
                sSql = "Update tbCustomer set DisCodeTag=1 where ChNo='" + TxtSpinCouponNumber.Text.Trim() + "'";
                SqlCommand cmdSpin = new SqlCommand(sSql, connSpin);
                connSpin.Open();
                cmdSpin.ExecuteNonQuery();
                connSpin.Close();


            }
            catch
            {
                //
            }
        }
        //------------------------------------------------------------------------------------------


        RedeemPoint();

        //LOAD AUTO BILL NO.
        //fnLoadAutoBillNo();

        //vDeclare.sBillNo = this.txtCHNo.Text;
        Session["sBillNo"] = this.txtCHNo.Text;

        if (ddlCardType1.SelectedItem.Text == "IPDC")
        {
            Response.Redirect("IPDC_Sales_Bill_Print.aspx");
        }
        else
        {
            Response.Redirect("Sales_Bill_Print.aspx");
        }
        //Response.Redirect("Sales_Bill_Print.aspx");

        return;

    }

    //FUNCTION FOR SEND MAIL
    private void fnSendMail_Invoice()
    {
        try
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
            //-----------------------------------------------------------------------------------------------------
            // Mail to Customer------------------------------------------------------------------------------------

            MailMessage mM2 = new MailMessage();
            //mM2.From = new MailAddress(txtEmail.Text);        

            //mM2.From = new MailAddress("rangs.eshop@gmail.com");
            mM2.From = new MailAddress("dms@rangs.com.bd");
            //PW:Exampass@567

            //mM2.To.Add(new MailAddress(Session["sEmail"].ToString()));
             
            
           
            if (txtEmail.Text.Trim() != "" && isMailValid(txtEmail.Text))
            {
                mM2.To.Add(new MailAddress(txtEmail.Text));
                mM2.CC.Add(new MailAddress(lblCTPEmail.Text));
            }
            else
            {
                //mM2.To.Add(new MailAddress(txtEmail.Text));
                mM2.To.Add(new MailAddress(lblCTPEmail.Text));
            }

            //mM2.Bcc.Add(new MailAddress("zunayedqu10@gmail.com"));
            //mM2.Bcc.Add(new MailAddress("minto@rangs.com.bd"));
            //mM2.Bcc.Add(new MailAddress("mohiuddin@rangs.com.bd"));
            //mM2.Bcc.Add(new MailAddress("sofiqul@rangs.com.bd"));

            mM2.Subject = "Sony-Rangs Invoice No." + txtCHNo.Text + " ";
            //mM2.Body = "<h1>Order Details</h1>";
            mM2.Body = "<p>Dear Valued Customer,</p>";
            mM2.Body = mM2.Body + "<p>Thank you for shopping with us.<br/>";
            //mM2.Body = mM2.Body + "We really appreciate it and we are taking necessary steps to process this order.";
            //mM2.Body = mM2.Body + "as soon as possible. You will be updated about next step immediate.";
            mM2.Body = mM2.Body + "</p>";


            //mM2.Body = mM2.Body + "<p>";
            //mM2.Body = mM2.Body + "Your order will be on its way very shortly, in the meantime please check below ";
            //mM2.Body = mM2.Body + "to ensure we have the correct details for your order.";
            //mM2.Body = mM2.Body + "</p>";
            mM2.Body = mM2.Body + "<p>";
            mM2.Body = mM2.Body + "<b><u>Sales From</u><br/> " + lblCTPName.Text + "</b><br/>";
            mM2.Body = mM2.Body + "" + lblCTPAdd.Text + "<br/>";
            mM2.Body = mM2.Body + "Phone: " + lblCTPContact.Text + "";
            mM2.Body = mM2.Body + "</p>";

            //mM2.Body = mM2.Body + "<br/>";
            mM2.Body = mM2.Body + "<p><b>Invoice No: " + txtCHNo.Text + "</b><br/>";
            mM2.Body = mM2.Body + "Invoice Date: " + txtDate.Text + "</p>";

            //mM2.Body = mM2.Body + "<br/>";
            mM2.Body = mM2.Body + "<p><u>Customer Details:</u><br/> Name: " + txtCustName.Text + "<br/>";
            mM2.Body = mM2.Body + "Contact # " + txtCustContact.Text + "<br/>";
            mM2.Body = mM2.Body + "Email: " + txtEmail.Text + "<br/>";
            mM2.Body = mM2.Body + "Address: " + txtCustAdd.Text + "</p>";

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
            mM2.Body = mM2.Body + "<p><b>Product Details:</b> </p>";

            //------- Start Table ---------------
            mM2.Body = mM2.Body + "<table style='font-family: arial, sans-serif; border-collapse: collapse; width: 100%;'>";

            mM2.Body = mM2.Body + "<tr>";
            mM2.Body = mM2.Body + "<th width='5%' style='border: 1px solid orange; text-align: left; padding: 8px;'>SL#</th>";
            mM2.Body = mM2.Body + "<th style='border: 1px solid orange; text-align: left; padding: 8px;'>Item Name</th>";
            mM2.Body = mM2.Body + "<th width='10%' style='border: 1px solid orange; text-align: left; padding: 8px;'>Qty</th>";
            mM2.Body = mM2.Body + "<th width='15%' style='border: 1px solid orange; text-align: left; padding: 8px;'>Amount (&#2547;)</th>";
            mM2.Body = mM2.Body + "<th width='15%' style='border: 1px solid orange; text-align: left; padding: 8px;'>Product Serial</th>";
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

            sSql = "SELECT dbo.MRSRMaster.MRSRMID, dbo.MRSRDetails.ProductID, dbo.Product.Code, dbo.Product.Model,";
            sSql = sSql + " dbo.Product.ProdName, ABS(dbo.MRSRDetails.Qty) AS tQty, dbo.MRSRDetails.UnitPrice,";
            sSql = sSql + " dbo.MRSRDetails.TotalAmnt, dbo.MRSRDetails.DiscountAmnt, dbo.MRSRDetails.WithAdjAmnt, ";
            sSql = sSql + " dbo.MRSRDetails.NetAmnt, CONVERT(varchar, CAST(dbo.MRSRDetails.NetAmnt AS money), 1) AS tNetAmnt, dbo.MRSRDetails.ProdRemarks, dbo.MRSRMaster.MRSRCode, dbo.MRSRDetails.SLNO";
            sSql = sSql + " FROM  dbo.MRSRMaster INNER JOIN";
            sSql = sSql + " dbo.MRSRDetails ON dbo.MRSRMaster.MRSRMID = dbo.MRSRDetails.MRSRMID INNER JOIN";
            sSql = sSql + " dbo.Product ON dbo.MRSRDetails.ProductID = dbo.Product.ProductID";
            sSql = sSql + " WHERE (dbo.MRSRMaster.MRSRCode = '" + this.txtCHNo.Text + "')";

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
                mM2.Body = mM2.Body + "<td align='Right' style='border: 1px solid orange; text-align: left; padding: 8px;'>" + dr["tQty"].ToString() + "</td>";
                mM2.Body = mM2.Body + "<td align='Right' style='border: 1px solid orange; text-align: left; padding: 8px;'>" + dr["tNetAmnt"].ToString() + "</td>";
                mM2.Body = mM2.Body + "<td align='Right' style='border: 1px solid orange; text-align: left; padding: 8px;'>" + dr["SLNO"].ToString() + "</td>";
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
            mM2.Body = mM2.Body + "<b>Total Amount: &#2547; " + txtNetAmnt.Text + "</b><br/>";

            mM2.Body = mM2.Body + "<b>Payment Details: </b><br/>";
            if (txtCash.Text.Length > 0)
            {
                mM2.Body = mM2.Body + "Cash: " + txtCash.Text + "<br/>";
            }
            if (txtCardAmnt1.Text.Length > 0)
            {
                if (txtCardAmnt1.Text != "0")
                {
                    mM2.Body = mM2.Body + "Card1: " + txtCardAmnt1.Text + "<br/>";
                }
            }
            if (txtCardAmnt2.Text.Length > 0)
            {
                if (txtCardAmnt2.Text != "0")
                {
                    mM2.Body = mM2.Body + "Card2 " + txtCardAmnt2.Text + "<br/>";
                }
            }


            //mM2.Body = mM2.Body + "Payment Type: " + lblPaymentMethod.Text + "<br/>";
            if (txtOrderNo.Text != "")
            {
                if (txtOrderNo.Text.Length > 0)
                {
                    mM2.Body = mM2.Body + "Online Order No.: " + txtOrderNo.Text.ToString() + "<br/>";
                }
            }


            //mM2.Body = mM2.Body + "Delivery Type: " + lblDelType.Text + "<br/>";
            //mM2.Body = mM2.Body + "Delivery From: " + lblEName.Text + " (" + lblCTPAdd.Text + ") " + "";
            mM2.Body = mM2.Body + "</p>";
            //mM2.Body = mM2.Body + "<p>&nbsp;</p>";

            if (txtTC.Text.Length > 0)
            {
                mM2.Body = mM2.Body + "<p>Terms & Conditions: " + txtTC.Text + "</p>";
            }

            //mM2.Body = mM2.Body + "<p>&nbsp;</p>";

            //mM2.Body = mM2.Body + "<p>";
            //mM2.Body = mM2.Body + "Once item(s) has been sent out for your order another email will be sent to you ";
            //mM2.Body = mM2.Body + "to confirm the dispatch along with the tracking details of this order.";
            //mM2.Body = mM2.Body + "</p>";

            //it add codeware for notify earn redeem
            mM2.Body = mM2.Body + "<strong style='color: green;'> Earn Voucher Point : " + ViewState["__earnedVoucherPoint__"].ToString() + "</strong><br/>";
            
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
            //sC1.Port = 2525;
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
        catch (Exception ex)
        {
            MailMessage mM2 = new MailMessage();
            //mM2.From = new MailAddress(txtEmail.Text);        

            //mM2.From = new MailAddress("rangs.eshop@gmail.com");
            mM2.From = new MailAddress("dms@rangs.com.bd");
            //PW:Exampass@567

            //mM2.To.Add(new MailAddress(Session["sEmail"].ToString()));
            mM2.To.Add(new MailAddress("golummohammedmohiuddin@gmail.com"));
            //mM2.CC.Add(new MailAddress(lblCTPEmail.Text));
            //mM2.Bcc.Add(new MailAddress("zunayedqu10@gmail.com"));
            //mM2.Bcc.Add(new MailAddress("minto@rangs.com.bd"));

            //mM2.Bcc.Add(new MailAddress("sofiqul@rangs.com.bd"));

            mM2.Subject = "error_email_sending";
            //mM2.Body = "<h1>Order Details</h1>";
            mM2.Body = "1." + ex.Message + "...2." + ex.InnerException;

            mM2.Body = mM2.Body + "<p>" + txtCHNo.Text.ToString() + "<br/>";
            //mM2.Body = mM2.Body + "We really appreciate it and we are taking necessary steps to process this order.";
            //mM2.Body = mM2.Body + "as soon as possible. You will be updated about next step immediate.";
            mM2.Body = mM2.Body + "</p>";
            mM2.IsBodyHtml = true;
            mM2.Priority = MailPriority.High;
            SmtpClient sC1 = new SmtpClient("mail.rangs.com.bd");
            sC1.Port = 587;
            //sC1.Port = 2525;
            sC1.Credentials = new System.Net.NetworkCredential("dms@rangs.com.bd", "Exampass@567");
            //sC.EnableSsl = true;
            sC1.Send(mM2);
            // Deal with an exception if one is thrown by the code in the try block
            //
        }

    }

    private void _fnSendMail_Invoice_test()
    {
        try
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
            //-----------------------------------------------------------------------------------------------------
            // Mail to Customer------------------------------------------------------------------------------------

            MailMessage mM2 = new MailMessage();
            //mM2.From = new MailAddress(txtEmail.Text);        

            //mM2.From = new MailAddress("rangs.eshop@gmail.com");
            mM2.From = new MailAddress("dms@rangs.com.bd");
            //PW:Exampass@567

            //mM2.To.Add(new MailAddress(Session["sEmail"].ToString()));
            //if (txtEmail.Text.Trim() != "")
            //{
            //    mM2.To.Add(new MailAddress(txtEmail.Text));
            //    mM2.CC.Add(new MailAddress(lblCTPEmail.Text));
            //}
            //else
            //{
            //    //mM2.To.Add(new MailAddress(txtEmail.Text));
            //    mM2.To.Add(new MailAddress(lblCTPEmail.Text));
            //}

            //mM2.Bcc.Add(new MailAddress("zunayedqu10@gmail.com"));
            mM2.To.Add(new MailAddress("minto@rangs.com.bd"));
            mM2.Bcc.Add(new MailAddress("mohiuddin@rangs.com.bd"));
            mM2.Bcc.Add(new MailAddress("sofiqul@rangs.com.bd"));
            mM2.Bcc.Add(new MailAddress("ertaza@rangs.com.bd"));


            //mM2.Bcc.Add(new MailAddress("zanealam@rangs.com.bd"));

            mM2.Subject = "Sony-Rangs Invoice No." + txtCHNo.Text + " ";
            //mM2.Body = "<h1>Order Details</h1>";
            mM2.Body = "<p>Dear Valued Customer,</p>";
            mM2.Body = mM2.Body + "<p>Thank you for shopping with us.<br/>";
            //mM2.Body = mM2.Body + "We really appreciate it and we are taking necessary steps to process this order.";
            //mM2.Body = mM2.Body + "as soon as possible. You will be updated about next step immediate.";
            mM2.Body = mM2.Body + "</p>";


            //mM2.Body = mM2.Body + "<p>";
            //mM2.Body = mM2.Body + "Your order will be on its way very shortly, in the meantime please check below ";
            //mM2.Body = mM2.Body + "to ensure we have the correct details for your order.";
            //mM2.Body = mM2.Body + "</p>";
            mM2.Body = mM2.Body + "<p>";
            mM2.Body = mM2.Body + "<b><u>Sales From</u><br/> " + lblCTPName.Text + "</b><br/>";
            mM2.Body = mM2.Body + "" + lblCTPAdd.Text + "<br/>";
            mM2.Body = mM2.Body + "Phone: " + lblCTPContact.Text + "";
            mM2.Body = mM2.Body + "</p>";

            //mM2.Body = mM2.Body + "<br/>";
            mM2.Body = mM2.Body + "<p><b>Invoice No: " + txtCHNo.Text + "</b><br/>";
            mM2.Body = mM2.Body + "Invoice Date: " + txtDate.Text + "</p>";

            //mM2.Body = mM2.Body + "<br/>";
            mM2.Body = mM2.Body + "<p><u>Customer Details:</u><br/> Name: " + txtCustName.Text + "<br/>";
            mM2.Body = mM2.Body + "Contact # " + txtCustContact.Text + "<br/>";
            mM2.Body = mM2.Body + "Email: " + txtEmail.Text + "<br/>";
            mM2.Body = mM2.Body + "Address: " + txtCustAdd.Text + "</p>";

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
            mM2.Body = mM2.Body + "<p><b>Product Details:</b> </p>";

            //------- Start Table ---------------
            mM2.Body = mM2.Body + "<table style='font-family: arial, sans-serif; border-collapse: collapse; width: 100%;'>";

            mM2.Body = mM2.Body + "<tr>";
            mM2.Body = mM2.Body + "<th width='5%' style='border: 1px solid orange; text-align: left; padding: 8px;'>SL#</th>";
            mM2.Body = mM2.Body + "<th style='border: 1px solid orange; text-align: left; padding: 8px;'>Item Name</th>";
            mM2.Body = mM2.Body + "<th width='10%' style='border: 1px solid orange; text-align: left; padding: 8px;'>Qty</th>";
            mM2.Body = mM2.Body + "<th width='15%' style='border: 1px solid orange; text-align: left; padding: 8px;'>Amount (&#2547;)</th>";
            mM2.Body = mM2.Body + "<th width='15%' style='border: 1px solid orange; text-align: left; padding: 8px;'>Product Serial</th>";
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

            sSql = "SELECT dbo.MRSRMaster.MRSRMID, dbo.MRSRDetails.ProductID, dbo.Product.Code, dbo.Product.Model,";
            sSql = sSql + " dbo.Product.ProdName, ABS(dbo.MRSRDetails.Qty) AS tQty, dbo.MRSRDetails.UnitPrice,";
            sSql = sSql + " dbo.MRSRDetails.TotalAmnt, dbo.MRSRDetails.DiscountAmnt, dbo.MRSRDetails.WithAdjAmnt, ";
            sSql = sSql + " dbo.MRSRDetails.NetAmnt, CONVERT(varchar, CAST(dbo.MRSRDetails.NetAmnt AS money), 1) AS tNetAmnt, dbo.MRSRDetails.ProdRemarks, dbo.MRSRMaster.MRSRCode, dbo.MRSRDetails.SLNO";
            sSql = sSql + " FROM  dbo.MRSRMaster INNER JOIN";
            sSql = sSql + " dbo.MRSRDetails ON dbo.MRSRMaster.MRSRMID = dbo.MRSRDetails.MRSRMID INNER JOIN";
            sSql = sSql + " dbo.Product ON dbo.MRSRDetails.ProductID = dbo.Product.ProductID";
            sSql = sSql + " WHERE (dbo.MRSRMaster.MRSRCode = '" + this.txtCHNo.Text + "')";

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
                mM2.Body = mM2.Body + "<td align='Right' style='border: 1px solid orange; text-align: left; padding: 8px;'>" + dr["tQty"].ToString() + "</td>";
                mM2.Body = mM2.Body + "<td align='Right' style='border: 1px solid orange; text-align: left; padding: 8px;'>" + dr["tNetAmnt"].ToString() + "</td>";
                mM2.Body = mM2.Body + "<td align='Right' style='border: 1px solid orange; text-align: left; padding: 8px;'>" + dr["SLNO"].ToString() + "</td>";
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
            mM2.Body = mM2.Body + "<b>Total Amount: &#2547; " + txtNetAmnt.Text + "</b><br/>";

            mM2.Body = mM2.Body + "<b>Payment Details: </b><br/>";
            if (txtCash.Text.Length > 0)
            {
                mM2.Body = mM2.Body + "Cash: " + txtCash.Text + "<br/>";
            }
            if (txtCardAmnt1.Text.Length > 0)
            {
                if (txtCardAmnt1.Text != "0")
                {
                    mM2.Body = mM2.Body + "Card1: " + txtCardAmnt1.Text + "<br/>";
                }
            }
            if (txtCardAmnt2.Text.Length > 0)
            {
                if (txtCardAmnt2.Text != "0")
                {
                    mM2.Body = mM2.Body + "Card2 " + txtCardAmnt2.Text + "<br/>";
                }
            }


            //mM2.Body = mM2.Body + "Payment Type: " + lblPaymentMethod.Text + "<br/>";
            if (txtOrderNo.Text != "")
            {
                if (txtOrderNo.Text.Length > 0)
                {
                    mM2.Body = mM2.Body + "Online Order No.: " + txtOrderNo.Text.ToString() + "<br/>";
                }
            }


            //mM2.Body = mM2.Body + "Delivery Type: " + lblDelType.Text + "<br/>";
            //mM2.Body = mM2.Body + "Delivery From: " + lblEName.Text + " (" + lblCTPAdd.Text + ") " + "";
            mM2.Body = mM2.Body + "</p>";
            //mM2.Body = mM2.Body + "<p>&nbsp;</p>";

            if (txtTC.Text.Length > 0)
            {
                mM2.Body = mM2.Body + "<p>Terms & Conditions: " + txtTC.Text + "</p>";
            }

            //mM2.Body = mM2.Body + "<p>&nbsp;</p>";

            //mM2.Body = mM2.Body + "<p>";
            //mM2.Body = mM2.Body + "Once item(s) has been sent out for your order another email will be sent to you ";
            //mM2.Body = mM2.Body + "to confirm the dispatch along with the tracking details of this order.";
            //mM2.Body = mM2.Body + "</p>";

            //it add codeware for notify earnvoucher
            mM2.Body = mM2.Body + "<strong style='color: green;'> Earn Voucher Point : " + ViewState["__earnedVoucherPoint__"].ToString() + "</strong><br/>";

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
            //sC1.Port = 2525;
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
        catch (Exception ex)
        {
            MailMessage mM2 = new MailMessage();
            //mM2.From = new MailAddress(txtEmail.Text);        

            //mM2.From = new MailAddress("rangs.eshop@gmail.com");
            mM2.From = new MailAddress("dms@rangs.com.bd");
            //PW:Exampass@567

            //mM2.To.Add(new MailAddress(Session["sEmail"].ToString()));
            mM2.To.Add(new MailAddress("sofiqul@rangs.com.bd"));
            //mM2.CC.Add(new MailAddress(lblCTPEmail.Text));
            //mM2.Bcc.Add(new MailAddress("zunayedqu10@gmail.com"));
            //mM2.Bcc.Add(new MailAddress("minto@rangs.com.bd"));

            //mM2.Bcc.Add(new MailAddress("sofiqul@rangs.com.bd"));

            mM2.Subject = "error_email_sending";
            //mM2.Body = "<h1>Order Details</h1>";
            mM2.Body = "1." + ex.Message + "...2." + ex.InnerException;

            mM2.Body = mM2.Body + "<p>" + txtCHNo.Text.ToString() + "<br/>";
            //mM2.Body = mM2.Body + "We really appreciate it and we are taking necessary steps to process this order.";
            //mM2.Body = mM2.Body + "as soon as possible. You will be updated about next step immediate.";
            mM2.Body = mM2.Body + "</p>";
            mM2.IsBodyHtml = true;
            mM2.Priority = MailPriority.High;
            SmtpClient sC1 = new SmtpClient("mail.rangs.com.bd");
            sC1.Port = 587;
            //sC1.Port = 2525;
            sC1.Credentials = new System.Net.NetworkCredential("dms@rangs.com.bd", "Exampass@567");
            //sC.EnableSsl = true;
            sC1.Send(mM2);
            // Deal with an exception if one is thrown by the code in the try block
            //
        }

    }

    private void fnSendMail_Invoice_test()
    {
        try
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
            //-----------------------------------------------------------------------------------------------------
            // Mail to Customer------------------------------------------------------------------------------------

            MailMessage mM2 = new MailMessage();
            //mM2.From = new MailAddress(txtEmail.Text);        

            //mM2.From = new MailAddress("rangs.eshop@gmail.com");
            mM2.From = new MailAddress("dms@rangs.com.bd");
            //PW:Exampass@567

            //mM2.To.Add(new MailAddress(Session["sEmail"].ToString()));
            //if (txtEmail.Text.Trim() != "")
            //{
            //    mM2.To.Add(new MailAddress(txtEmail.Text));
            //    mM2.CC.Add(new MailAddress(lblCTPEmail.Text));
            //}
            //else
            //{
            //    //mM2.To.Add(new MailAddress(txtEmail.Text));
            //    mM2.To.Add(new MailAddress(lblCTPEmail.Text));
            //}

            //mM2.Bcc.Add(new MailAddress("zunayedqu10@gmail.com"));
            mM2.To.Add(new MailAddress("rakibul.codeware@gmail.com"));
           


            //mM2.Bcc.Add(new MailAddress("zanealam@rangs.com.bd"));

            mM2.Subject = "Sony-Rangs Invoice No." + txtCHNo.Text + " ";
            //mM2.Body = "<h1>Order Details</h1>";
            mM2.Body = "<p>Dear Valued Customer,</p>";
            mM2.Body = mM2.Body + "<p>Thank you for shopping with us.<br/>";
            //mM2.Body = mM2.Body + "We really appreciate it and we are taking necessary steps to process this order.";
            //mM2.Body = mM2.Body + "as soon as possible. You will be updated about next step immediate.";
            mM2.Body = mM2.Body + "</p>";


            //mM2.Body = mM2.Body + "<p>";
            //mM2.Body = mM2.Body + "Your order will be on its way very shortly, in the meantime please check below ";
            //mM2.Body = mM2.Body + "to ensure we have the correct details for your order.";
            //mM2.Body = mM2.Body + "</p>";
            mM2.Body = mM2.Body + "<p>";
            mM2.Body = mM2.Body + "<b><u>Sales From</u><br/> " + lblCTPName.Text + "</b><br/>";
            mM2.Body = mM2.Body + "" + lblCTPAdd.Text + "<br/>";
            mM2.Body = mM2.Body + "Phone: " + lblCTPContact.Text + "";
            mM2.Body = mM2.Body + "</p>";


          
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
            mM2.Body = mM2.Body + "<p><b>Product Details:</b> </p>";

            //------- Start Table ---------------
         

            //-----------------------------------------------------------------------------
            sSql = "";
            //sSql = "SELECT dbo.tbCustomerDelDetails.DelDID, dbo.tbCustomerDelDetails.DelID, dbo.tbCustomerDelDetails.ProductID,";
            //sSql = sSql + " dbo.tbProduct.title, dbo.tbProduct.titleDesc, dbo.tbCustomerDelDetails.SalePrice,";
            //sSql = sSql + " dbo.tbCustomerDelDetails.tQty, dbo.tbCustomerDelDetails.tAmnt";
            //sSql = sSql + " FROM dbo.tbCustomerDelDetails INNER JOIN";
            //sSql = sSql + " dbo.tbProduct ON dbo.tbCustomerDelDetails.ProductID = dbo.tbProduct.ProductID";
            //sSql = sSql + " WHERE (dbo.tbCustomerDelDetails.DelID = '" + this.lblID.Text + "')";

          

            
            //-------------------------------------------------------------------------------------

            //mM2.Body = mM2.Body + "<tr><td>Booking Date/time:</td><td>" + string.Format("{0:D}", tfDate) + " at " + ddlTime.SelectedItem.Text + "</td></tr>";
            //mM2.Body = mM2.Body + "<tr><td>Secial Notes/Comments:</td><td>" + txtNote.Text + "</td></tr>";
           
          

            //mM2.Body = mM2.Body + "Delivery Type: " + lblDelType.Text + "<br/>";
            //mM2.Body = mM2.Body + "Delivery From: " + lblEName.Text + " (" + lblCTPAdd.Text + ") " + "";
          
            //mM2.Body = mM2.Body + "<p>&nbsp;</p>";

            //mM2.Body = mM2.Body + "<p>";
            //mM2.Body = mM2.Body + "Once item(s) has been sent out for your order another email will be sent to you ";
            //mM2.Body = mM2.Body + "to confirm the dispatch along with the tracking details of this order.";
            //mM2.Body = mM2.Body + "</p>";

            //it add codeware for notify earnvoucher
         

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
            //sC1.Port = 2525;
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
        catch (Exception ex)
        {
            MailMessage mM2 = new MailMessage();
            //mM2.From = new MailAddress(txtEmail.Text);        

            //mM2.From = new MailAddress("rangs.eshop@gmail.com");
            mM2.From = new MailAddress("dms@rangs.com.bd");
            //PW:Exampass@567

            //mM2.To.Add(new MailAddress(Session["sEmail"].ToString()));
            mM2.To.Add(new MailAddress("sofiqul@rangs.com.bd"));
            //mM2.CC.Add(new MailAddress(lblCTPEmail.Text));
            //mM2.Bcc.Add(new MailAddress("zunayedqu10@gmail.com"));
            //mM2.Bcc.Add(new MailAddress("minto@rangs.com.bd"));

            //mM2.Bcc.Add(new MailAddress("sofiqul@rangs.com.bd"));

            mM2.Subject = "error_email_sending";
            //mM2.Body = "<h1>Order Details</h1>";
            mM2.Body = "1." + ex.Message + "...2." + ex.InnerException;

            mM2.Body = mM2.Body + "<p>" + txtCHNo.Text.ToString() + "<br/>";
            //mM2.Body = mM2.Body + "We really appreciate it and we are taking necessary steps to process this order.";
            //mM2.Body = mM2.Body + "as soon as possible. You will be updated about next step immediate.";
            mM2.Body = mM2.Body + "</p>";
            mM2.IsBodyHtml = true;
            mM2.Priority = MailPriority.High;
            SmtpClient sC1 = new SmtpClient("mail.rangs.com.bd");
            sC1.Port = 587;
            //sC1.Port = 2525;
            sC1.Credentials = new System.Net.NetworkCredential("dms@rangs.com.bd", "Exampass@567");
            //sC.EnableSsl = true;
            sC1.Send(mM2);
            // Deal with an exception if one is thrown by the code in the try block
            //
        }

    }
    private void fnSendMail_Invoice_For_Redeem()
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
        //-----------------------------------------------------------------------------------------------------
        // Mail to Customer------------------------------------------------------------------------------------

        MailMessage mM2 = new MailMessage();

        mM2.From = new MailAddress("dms@rangs.com.bd");//PW:Exampass@567        
        mM2.To.Add(new MailAddress("minto@rangs.com.bd"));

        //mM2.Bcc.Add(new MailAddress("md.mohiiuddiin@gmail.com"));
        mM2.Bcc.Add(new MailAddress("sofiqul@rangs.com.bd"));
        mM2.Bcc.Add(new MailAddress("ertaza@rangs.com.bd"));
        mM2.Bcc.Add(new MailAddress("mohiuddin@rangs.com.bd"));

        //mM2.Bcc.Add(new MailAddress("minto@rangs.com.bd"));

        mM2.Subject = "Sony-Rangs(Redeem) Invoice No." + txtCHNo.Text + " ";

        mM2.Body = "<p>Dear Valued Admin,</p>";
        mM2.Body = mM2.Body + "<p>Voucher Redeem Information<br/>";

        mM2.Body = mM2.Body + "</p>";



        mM2.Body = mM2.Body + "<p>";
        mM2.Body = mM2.Body + "<b><u>Sales From</u><br/> " + lblCTPName.Text + "</b><br/>";
        mM2.Body = mM2.Body + "</p>";

        //mM2.Body = mM2.Body + "<br/>";
        mM2.Body = mM2.Body + "<p><b>Invoice No: " + txtCHNo.Text + "</b><br/>";
        mM2.Body = mM2.Body + "Invoice Date: " + txtDate.Text + "</p>";

        //mM2.Body = mM2.Body + "<br/>";
        mM2.Body = mM2.Body + "<p><u>Customer Details:</u><br/> Name: " + txtCustName.Text + "<br/>";
        mM2.Body = mM2.Body + "Contact # " + txtCustContact.Text + "<br/>";


        //LOAD Redeem INFORMATION
        sSql = "";
        sSql = " SELECT [CustomerContact]," +
        "sum([EarnedVoucherPoint]*500)avlRdm,sum([RedeemVoucherPoint]*500)ttlusedRdm " +
        "FROM [dbCID].[dbo].[tbCustomerVoucher] where CustomerContact='" + txtCustContact.Text + "' " +
        "group by [CustomerContact]";
        SqlCommand cmdRdm = new SqlCommand(sSql, conn);
        conn.Open();
        SqlDataReader drRdm = cmdRdm.ExecuteReader();
        if (drRdm.Read())
        {
            if (string.IsNullOrEmpty(this.txtRedeemPoint.Text))
            {
                this.txtRedeemPoint.Text = "0";
            }
            
            mM2.Body = mM2.Body + "<strong style='color: green;'>Discount Over Redeem Voucher # " + Convert.ToInt32(this.txtRedeemPoint.Text) * 500 + "</strong><br/>";
            mM2.Body = mM2.Body + "<strong style='color: green;'>Avalilable Voucher Amount # " + drRdm["avlRdm"].ToString() + "</strong><br/>";
            mM2.Body = mM2.Body + "<strong style='color: green;'>Used Total Voucher Amount # " + drRdm["ttlusedRdm"].ToString() + "</strong><br/>";
        }
         mM2.Body = mM2.Body + "<strong style='color: green;'> Earn Voucher Point : " + ViewState["__earnedVoucherPoint__"].ToString() + "</strong><br/>";


        conn.Close();




        mM2.Body = mM2.Body + "<p><b>Product Details:</b> </p>";

        //------- Start Table ---------------
        mM2.Body = mM2.Body + "<table style='font-family: arial, sans-serif; border-collapse: collapse; width: 100%;'>";

        mM2.Body = mM2.Body + "<tr>";
        mM2.Body = mM2.Body + "<th width='5%' style='border: 1px solid orange; text-align: left; padding: 8px;'>SL#</th>";
        mM2.Body = mM2.Body + "<th style='border: 1px solid orange; text-align: left; padding: 8px;'>Item Name</th>";
        mM2.Body = mM2.Body + "<th width='10%' style='border: 1px solid orange; text-align: left; padding: 8px;'>Qty</th>";
        mM2.Body = mM2.Body + "<th width='15%' style='border: 1px solid orange; text-align: left; padding: 8px;'>Dis Amount (&#2547;)</th>";
        mM2.Body = mM2.Body + "<th width='15%' style='border: 1px solid orange; text-align: left; padding: 8px;'>Total Amnt</th>";
        mM2.Body = mM2.Body + "<th width='15%' style='border: 1px solid orange; text-align: left; padding: 8px;'>Net Amount</th>";
        mM2.Body = mM2.Body + "</tr>";

        //-----------------------------------------------------------------------------
        sSql = "";


        sSql = "SELECT dbo.MRSRMaster.MRSRMID, dbo.MRSRDetails.ProductID, dbo.Product.Code, dbo.Product.Model,";
        sSql = sSql + " dbo.Product.ProdName, ABS(dbo.MRSRDetails.Qty) AS tQty, dbo.MRSRDetails.UnitPrice,";
        sSql = sSql + " dbo.MRSRDetails.TotalAmnt, dbo.MRSRDetails.DiscountAmnt, dbo.MRSRDetails.WithAdjAmnt, ";
        sSql = sSql + " dbo.MRSRDetails.NetAmnt, CONVERT(varchar, CAST(dbo.MRSRDetails.NetAmnt AS money), 1) AS tNetAmnt, dbo.MRSRDetails.ProdRemarks, dbo.MRSRMaster.MRSRCode, dbo.MRSRDetails.SLNO";
        sSql = sSql + " FROM  dbo.MRSRMaster INNER JOIN";
        sSql = sSql + " dbo.MRSRDetails ON dbo.MRSRMaster.MRSRMID = dbo.MRSRDetails.MRSRMID INNER JOIN";
        sSql = sSql + " dbo.Product ON dbo.MRSRDetails.ProductID = dbo.Product.ProductID";
        sSql = sSql + " WHERE (dbo.MRSRMaster.MRSRCode = '" + this.txtCHNo.Text + "')";

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
            mM2.Body = mM2.Body + "<td align='Right' style='border: 1px solid orange; text-align: left; padding: 8px;'>" + dr["tQty"].ToString() + "</td>";
            mM2.Body = mM2.Body + "<td align='Right' style='border: 1px solid orange; text-align: left; padding: 8px;'>" + dr["DiscountAmnt"].ToString() + "</td>";
            mM2.Body = mM2.Body + "<td align='Right' style='border: 1px solid orange; text-align: left; padding: 8px;'>" + dr["TotalAmnt"].ToString() + "</td>";
            mM2.Body = mM2.Body + "<td align='Right' style='border: 1px solid orange; text-align: left; padding: 8px;'>" + dr["tNetAmnt"].ToString() + "</td>";
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
        mM2.Body = mM2.Body + "<b>Total Amount: &#2547; " + txtNetAmnt.Text + "</b><br/>";

        mM2.Body = mM2.Body + "<b>Payment Details: </b><br/>";
        if (txtCash.Text.Length > 0)
        {
            mM2.Body = mM2.Body + "Cash: " + txtCash.Text + "<br/>";
        }
        if (txtCardAmnt1.Text.Length > 0)
        {
            if (txtCardAmnt1.Text != "0")
            {
                mM2.Body = mM2.Body + "Card1: " + txtCardAmnt1.Text + "<br/>";
            }
        }
        if (txtCardAmnt2.Text.Length > 0)
        {
            if (txtCardAmnt2.Text != "0")
            {
                mM2.Body = mM2.Body + "Card2 " + txtCardAmnt2.Text + "<br/>";
            }
        }


        //mM2.Body = mM2.Body + "Payment Type: " + lblPaymentMethod.Text + "<br/>";

        if (txtOrderNo.Text.Length > 0)
        {
            mM2.Body = mM2.Body + "Online Order No.: " + txtOrderNo.Text + "<br/>";
        }

        //mM2.Body = mM2.Body + "Delivery Type: " + lblDelType.Text + "<br/>";
        //mM2.Body = mM2.Body + "Delivery From: " + lblEName.Text + " (" + lblCTPAdd.Text + ") " + "";
        mM2.Body = mM2.Body + "</p>";
        //mM2.Body = mM2.Body + "<p>&nbsp;</p>";

        if (txtTC.Text.Length > 0)
        {
            mM2.Body = mM2.Body + "<p>Terms & Conditions: " + txtTC.Text + "</p>";
        }

        mM2.IsBodyHtml = true;
        mM2.Priority = MailPriority.High;
        SmtpClient sC1 = new SmtpClient("mail.rangs.com.bd");
        //sC1.Port = 587;
        sC1.Port = 2525;
        sC1.Credentials = new System.Net.NetworkCredential("dms@rangs.com.bd", "Exampass@567");
        //sC.EnableSsl = true;
        sC1.Send(mM2);


    }
    protected void fnSMSforIncentive()
    {
        SqlConnection conn = DBConnection.GetConnection();
        SqlConnection connS = DBConnection.GetConnection();
        SqlConnection connHR = DBConnectionHRM.GetConnection();
        SqlConnection connSMS = DBConnectionSMS.GetConnection();
        //-----------------------------------------------------------------
        double dIncAmnt = 0;
        double dTotalIncAmnt = 0;
        string sMonth = String.Format("{0:MMM}", DateTime.Now);
        string sYear = String.Format("{0:yyyy}", DateTime.Now);

        int FYs, FYe;
        DateTime sFYs, sFYe, sDate, eDate;
        DateTime date = DateTime.Today;
        var fDayOfMonth = new DateTime(date.Year, date.Month, 1);
        var lDayOfMonth = fDayOfMonth.AddMonths(1).AddDays(-1);

        sDate = Convert.ToDateTime(fDayOfMonth);
        eDate = Convert.ToDateTime(lDayOfMonth);
        //-----------------------------------------------------------------

        if (txtJobID.Text == "")
        {
            PopupMessage("Please enter Customer Contact #.", btnSave);
            txtJobID.Focus();
            return;
        }

        string sMobile = "";

        //RETRIVE MOBILE NUMBER
        string sSql = "";
        sSql = "SELECT * FROM vw_EmpInfo WHERE JobCod='" + this.txtJobID.Text + "'";
        SqlCommand cmdCust = new SqlCommand(sSql, connHR);
        connHR.Open();
        SqlDataReader drCust = cmdCust.ExecuteReader();

        if (drCust.Read())
        {
            sMobile = drCust["Phone"].ToString();
        }
        else
        {
            sMobile = "";
        }
        connHR.Close();
        //-----------------------------------------------------------------------

        //RETRIVE CURRENT INVOICE INCENTIVE
        sSql = "";
        sSql = "SELECT dbo.MRSRMaster.TrType, dbo.MRSRMaster.Issby, dbo.Product.Model, dbo.Product.UnitPrice,";
        sSql = sSql + " dbo.Product.GetIncentive, ISNULL(dbo.Product.BLIPAmnt,0) AS BLIPAmnt, dbo.MRSRDetails.UnitPrice, SUM(ABS(dbo.MRSRDetails.Qty)) AS tQty,";
        sSql = sSql + " dbo.MRSRDetails.UnitPrice - dbo.Product.BLIPAmnt AS dIncAmnt, dbo.MRSRMaster.MRSRCode,";
        sSql = sSql + " dbo.MRSRMaster.MRSRMID, dbo.MRSRMaster.OutSource, dbo.MRSRDetails.MRSRDID, dbo.MRSRDetails.ProductID, dbo.MRSRMaster.TDate";
        sSql = sSql + " FROM  dbo.Product INNER JOIN";
        sSql = sSql + " dbo.MRSRDetails ON dbo.Product.ProductID = dbo.MRSRDetails.ProductID INNER JOIN";
        sSql = sSql + " dbo.MRSRMaster ON dbo.MRSRDetails.MRSRMID = dbo.MRSRMaster.MRSRMID";

        sSql = sSql + " WHERE (dbo.MRSRMaster.TrType = 3) AND (dbo.MRSRMaster.Issby = '" + txtJobID.Text + "')";
        sSql = sSql + " AND (dbo.Product.GetIncentive = 1) AND (dbo.MRSRMaster.MRSRCode = '" + txtCHNo.Text + "')";

        sSql = sSql + " GROUP BY dbo.MRSRMaster.TrType, dbo.MRSRMaster.Issby, dbo.Product.Model, dbo.Product.UnitPrice, dbo.Product.GetIncentive, dbo.Product.BLIPAmnt,";
        sSql = sSql + " dbo.MRSRDetails.UnitPrice, dbo.MRSRMaster.MRSRCode, dbo.MRSRMaster.MRSRMID, dbo.MRSRMaster.OutSource, dbo.MRSRDetails.MRSRDID, ";
        sSql = sSql + " dbo.MRSRDetails.ProductID, dbo.MRSRMaster.TDate";

        SqlCommand cmdR = new SqlCommand(sSql, conn);
        conn.Open();
        SqlDataReader drR = cmdR.ExecuteReader();

        if (drR.Read())
        {
            dIncAmnt = Convert.ToDouble(drR["dIncAmnt"].ToString());

            sSql = "";
            sSql = "INSERT INTO tbIncentiveDetails(MRSRMID,MRSRCode,TDate,MRSRDID,";
            sSql = sSql + " ProductID,UnitPrice,BLIPAmnt,IncentiveAmnt,";
            sSql = sSql + " tQty,JobID,EID)";
            sSql = sSql + " Values ('" + drR["MRSRMID"].ToString() + "','" + drR["MRSRCode"].ToString() + "',";
            sSql = sSql + " '" + drR["TDate"].ToString() + "','" + drR["MRSRDID"].ToString() + "',";
            sSql = sSql + " '" + drR["ProductID"].ToString() + "','" + drR["UnitPrice"].ToString() + "',";
            sSql = sSql + " '" + drR["BLIPAmnt"].ToString() + "','" + drR["dIncAmnt"].ToString() + "',";
            sSql = sSql + " '" + drR["tQty"].ToString() + "','" + drR["Issby"].ToString() + "','" + drR["OutSource"].ToString() + "'";
            sSql = sSql + " )";
            SqlCommand cmdS = new SqlCommand(sSql, connS);
            connS.Open();
            cmdS.ExecuteNonQuery();
            connS.Close();

        }
        else
        {
            dIncAmnt = 0;
            return;
        }
        conn.Close();
        //-----------------------------------------------------------------------

        //RETRIVE CURRENT MONTHLY INCENTIVE
        sSql = "";
        sSql = "SELECT dbo.MRSRMaster.TrType, dbo.MRSRMaster.Issby, dbo.Product.GetIncentive,";
        //sSql = sSql + " SUM(dbo.MRSRDetails.UnitPrice) - SUM(dbo.Product.BLIPAmnt) AS dIncAmnt";
        sSql = sSql + " SUM((dbo.MRSRDetails.UnitPrice - dbo.Product.BLIPAmnt) ";
        sSql = sSql + " * Abs(dbo.MRSRDetails.Qty)) AS dIncAmnt, SUM(Abs(dbo.MRSRDetails.Qty)) AS tQty";

        sSql = sSql + " FROM dbo.Product INNER JOIN";
        sSql = sSql + " dbo.MRSRDetails ON dbo.Product.ProductID = dbo.MRSRDetails.ProductID INNER JOIN";
        sSql = sSql + " dbo.MRSRMaster ON dbo.MRSRDetails.MRSRMID = dbo.MRSRMaster.MRSRMID";

        //sSql = sSql + " WHERE (dbo.Product.GetIncentive = 0)";
        sSql = sSql + " WHERE (dbo.MRSRMaster.TrType = 3) AND (dbo.MRSRMaster.Issby = '" + txtJobID.Text + "')";
        sSql = sSql + " AND (dbo.Product.GetIncentive = 1)";
        sSql = sSql + " AND (dbo.MRSRMaster.TDate >= '" + sDate + "') AND (dbo.MRSRMaster.TDate <= '" + eDate + "')";

        sSql = sSql + " GROUP BY dbo.MRSRMaster.TrType, dbo.MRSRMaster.Issby, dbo.Product.GetIncentive";
        //sSql = sSql + " HAVING  dbo.MRSRMaster.TrType = 3) AND (dbo.MRSRMaster.Issby > N'0')";

        SqlCommand cmdR1 = new SqlCommand(sSql, conn);
        conn.Open();
        SqlDataReader drR1 = cmdR1.ExecuteReader();

        if (drR1.Read())
        {
            dTotalIncAmnt = Convert.ToDouble(drR1["dIncAmnt"].ToString());
        }
        else
        {
            dTotalIncAmnt = 0;
        }
        conn.Close();
        //-----------------------------------------------------------------------


        //SEND SMS
        if (sMobile != "")
        {
            string smsText = "";
            smsText = "Dear " + txtJobID.Text + ",\n";
            //smsText = smsText + "Your Bill Details:\n";
            smsText = smsText + "You have earned Tk. " + dIncAmnt + " for Invoice# " + this.txtCHNo.Text + ".\n";
            //smsText = smsText + "Date: " + this.txtDate.Text + ".\n";
            smsText = smsText + "Your total earning Tk. " + dTotalIncAmnt + " in " + sMonth + "-" + sYear + ".\n";
            smsText = smsText + "Sale More. Earn More.\n";
            smsText = smsText + "REL-LGP";

            sSql = "";
            sSql = "INSERT INTO tbSMS(ContactNo,SMSText,UserID,EntryDate,SMSSource)" +
                    " Values ('" + sMobile + "','" + smsText + "'," +
                    " '" + Session["UserID"] + "','" + DateTime.Today + "'," +
                    " 'DMS'" +
                    " )";
            SqlCommand cmdSMS = new SqlCommand(sSql, connSMS);
            connSMS.Open();
            cmdSMS.ExecuteNonQuery();
            connSMS.Close();
        }


    }
    //send email into marketing team
    private void fnSendMail_Invoice_test_marketing()
    {
        try
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
            //-----------------------------------------------------------------------------------------------------
            // Mail to Customer------------------------------------------------------------------------------------

            MailMessage mM2 = new MailMessage();
            //mM2.From = new MailAddress(txtEmail.Text);        

            //mM2.From = new MailAddress("rangs.eshop@gmail.com");
            mM2.From = new MailAddress(_mailFrom);
            //PW:Exampass@567

            //mM2.To.Add(new MailAddress(Session["sEmail"].ToString()));
            //if (txtEmail.Text.Trim() != "")
            //{
            //    mM2.To.Add(new MailAddress(txtEmail.Text));
            //    mM2.CC.Add(new MailAddress(lblCTPEmail.Text));
            //}
            //else
            //{
            //    //mM2.To.Add(new MailAddress(txtEmail.Text));
            //    mM2.To.Add(new MailAddress(lblCTPEmail.Text));
            //}

            //mM2.Bcc.Add(new MailAddress("zunayedqu10@gmail.com"));            
            //mM2.Bcc.Add(new MailAddress("mohiuddin@rangs.com.bd"));

            mM2.To.Add(new MailAddress("rakibul.codeware@gmail.com"));
            mM2.Bcc.Add(new MailAddress("parvez.codeware@gmail.com"));
            


            //mM2.Bcc.Add(new MailAddress("zanealam@rangs.com.bd"));

            mM2.Subject = "Sony-Rangs Invoice No." + txtCHNo.Text + " ";
            //mM2.Body = "<h1>Order Details</h1>";
            mM2.Body = "<p>Dear Valued Customer,</p>";
            mM2.Body = mM2.Body + "<p>Thank you for shopping with us.<br/>";
            //mM2.Body = mM2.Body + "We really appreciate it and we are taking necessary steps to process this order.";
            //mM2.Body = mM2.Body + "as soon as possible. You will be updated about next step immediate.";
            mM2.Body = mM2.Body + "</p>";


            //mM2.Body = mM2.Body + "<p>";
            //mM2.Body = mM2.Body + "Your order will be on its way very shortly, in the meantime please check below ";
            //mM2.Body = mM2.Body + "to ensure we have the correct details for your order.";
            //mM2.Body = mM2.Body + "</p>";
            mM2.Body = mM2.Body + "<p>";
            mM2.Body = mM2.Body + "<b><u>Sales From</u><br/> " + lblCTPName.Text + "</b><br/>";
            mM2.Body = mM2.Body + "" + lblCTPAdd.Text + "<br/>";
            mM2.Body = mM2.Body + "Phone: " + lblCTPContact.Text + "";
            mM2.Body = mM2.Body + "</p>";

            //mM2.Body = mM2.Body + "<br/>";
            mM2.Body = mM2.Body + "<p><b>Invoice No: " + txtCHNo.Text + "</b><br/>";
            mM2.Body = mM2.Body + "Invoice Date: " + txtDate.Text + "</p>";

            //mM2.Body = mM2.Body + "<br/>";
            mM2.Body = mM2.Body + "<p><u>Customer Details:</u><br/> Name: " + txtCustName.Text + "<br/>";
            mM2.Body = mM2.Body + "Contact # " + txtCustContact.Text + "<br/>";
            mM2.Body = mM2.Body + "Email: " + txtEmail.Text + "<br/>";
            mM2.Body = mM2.Body + "Address: " + txtCustAdd.Text + "</p>";

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
            mM2.Body = mM2.Body + "<p><b>Product Details:</b> </p>";

            //------- Start Table ---------------
            mM2.Body = mM2.Body + "<table style='font-family: arial, sans-serif; border-collapse: collapse; width: 100%;'>";

            mM2.Body = mM2.Body + "<tr>";
            mM2.Body = mM2.Body + "<th width='5%' style='border: 1px solid orange; text-align: left; padding: 8px;'>SL#</th>";
            mM2.Body = mM2.Body + "<th style='border: 1px solid orange; text-align: left; padding: 8px;'>Item Name</th>";
            mM2.Body = mM2.Body + "<th width='10%' style='border: 1px solid orange; text-align: left; padding: 8px;'>MRP</th>";
            mM2.Body = mM2.Body + "<th width='10%' style='border: 1px solid orange; text-align: left; padding: 8px;'>Campaign Price</th>";
            mM2.Body = mM2.Body + "<th width='10%' style='border: 1px solid orange; text-align: left; padding: 8px;'>Qty</th>";
            mM2.Body = mM2.Body + "<th width='15%' style='border: 1px solid orange; text-align: left; padding: 8px;'>Total Amount (&#2547;)</th>";
            mM2.Body = mM2.Body + "<th width='15%' style='border: 1px solid orange; text-align: left; padding: 8px;'>Discount Amount (&#2547;)</th>";
            mM2.Body = mM2.Body + "<th width='15%' style='border: 1px solid orange; text-align: left; padding: 8px;'>NetAmount (&#2547;)</th>";
            mM2.Body = mM2.Body + "<th width='15%' style='border: 1px solid orange; text-align: left; padding: 8px;'>Product Serial</th>";
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

            sSql = "SELECT dbo.MRSRMaster.MRSRMID, dbo.MRSRDetails.ProductID, dbo.Product.Code, dbo.Product.Model,";
            sSql = sSql + " dbo.Product.ProdName, ABS(dbo.MRSRDetails.Qty) AS tQty, dbo.MRSRDetails.UnitPrice,dbo.MRSRDetails.MRP,";
            sSql = sSql + " dbo.MRSRDetails.TotalAmnt, dbo.MRSRDetails.DiscountAmnt, dbo.MRSRDetails.WithAdjAmnt, ";
            sSql = sSql + " dbo.MRSRDetails.NetAmnt, CONVERT(varchar, CAST(dbo.MRSRDetails.NetAmnt AS money), 1) AS tNetAmnt, dbo.MRSRDetails.ProdRemarks, dbo.MRSRMaster.MRSRCode, dbo.MRSRDetails.SLNO,dbo.MRSRDetails.DisRef";
            sSql = sSql + " FROM  dbo.MRSRMaster INNER JOIN";
            sSql = sSql + " dbo.MRSRDetails ON dbo.MRSRMaster.MRSRMID = dbo.MRSRDetails.MRSRMID INNER JOIN";
            sSql = sSql + " dbo.Product ON dbo.MRSRDetails.ProductID = dbo.Product.ProductID";
            sSql = sSql + " WHERE (dbo.MRSRMaster.MRSRCode = '" + this.txtCHNo.Text + "')";

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
                mM2.Body = mM2.Body + "<td align='Right' style='border: 1px solid orange; text-align: left; padding: 8px;'>" + dr["MRP"].ToString() + "</td>";
                mM2.Body = mM2.Body + "<td align='Right' style='border: 1px solid orange; text-align: left; padding: 8px;'>" + dr["UnitPrice"].ToString() + "</td>";
                mM2.Body = mM2.Body + "<td align='Right' style='border: 1px solid orange; text-align: left; padding: 8px;'>" + dr["tQty"].ToString() + "</td>";
                mM2.Body = mM2.Body + "<td align='Right' style='border: 1px solid orange; text-align: left; padding: 8px;'>" + dr["TotalAmnt"].ToString() + "</td>";
                mM2.Body = mM2.Body + "<td align='Right' style='border: 1px solid orange; text-align: left; padding: 8px;'>" + dr["DiscountAmnt"].ToString() + "</td>";
                mM2.Body = mM2.Body + "<td align='Right' style='border: 1px solid orange; text-align: left; padding: 8px;'>" + dr["tNetAmnt"].ToString() + "</td>";
                mM2.Body = mM2.Body + "<td align='Right' style='border: 1px solid orange; text-align: left; padding: 8px;'>" + dr["SLNO"].ToString() + "</td>";
                mM2.Body = mM2.Body + "<td align='Right' style='border: 1px solid orange; text-align: left; padding: 8px;'>" + dr["DisRef"].ToString() + "</td>";
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
            mM2.Body = mM2.Body + "<b>Total Amount: &#2547; " + txtNetAmnt.Text + "</b><br/>";

            mM2.Body = mM2.Body + "<b>Payment Details: </b><br/>";
            if (txtCash.Text.Length > 0)
            {
                mM2.Body = mM2.Body + "Cash: " + txtCash.Text + "<br/>";
            }
            if (txtCardAmnt1.Text.Length > 0)
            {
                if (txtCardAmnt1.Text != "0")
                {
                    mM2.Body = mM2.Body + "Card1: " + txtCardAmnt1.Text + "<br/>";
                }
            }
            if (txtCardAmnt2.Text.Length > 0)
            {
                if (txtCardAmnt2.Text != "0")
                {
                    mM2.Body = mM2.Body + "Card2 " + txtCardAmnt2.Text + "<br/>";
                }
            }


            //mM2.Body = mM2.Body + "Payment Type: " + lblPaymentMethod.Text + "<br/>";
            if (txtOrderNo.Text != "")
            {
                if (txtOrderNo.Text.Length > 0)
                {
                    mM2.Body = mM2.Body + "Online Order No.: " + txtOrderNo.Text.ToString() + "<br/>";
                }
            }


            //mM2.Body = mM2.Body + "Delivery Type: " + lblDelType.Text + "<br/>";
            //mM2.Body = mM2.Body + "Delivery From: " + lblEName.Text + " (" + lblCTPAdd.Text + ") " + "";
            mM2.Body = mM2.Body + "</p>";
            //mM2.Body = mM2.Body + "<p>&nbsp;</p>";

            if (txtTC.Text.Length > 0)
            {
                mM2.Body = mM2.Body + "<p>Terms & Conditions: " + txtTC.Text + "</p>";
            }

            //mM2.Body = mM2.Body + "<p>&nbsp;</p>";

            //mM2.Body = mM2.Body + "<p>";
            //mM2.Body = mM2.Body + "Once item(s) has been sent out for your order another email will be sent to you ";
            //mM2.Body = mM2.Body + "to confirm the dispatch along with the tracking details of this order.";
            //mM2.Body = mM2.Body + "</p>";


            //it add codeware for notify earn redeem
           // mM2.Body = mM2.Body + "<strong style='color: green;'> Earn Voucher Point : " + ViewState["__earnedVoucherPoint__"].ToString() + "</strong><br/>";

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
            SmtpClient sC1 = new SmtpClient(_mailSmtpClient);
            sC1.Port = 587;
            //sC1.Port = 2525;
            sC1.Credentials = new System.Net.NetworkCredential(_mailUserName, _mailPassword);
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
        catch (Exception ex)
        {
            MailMessage mM2 = new MailMessage();
            //mM2.From = new MailAddress(txtEmail.Text);        

            //mM2.From = new MailAddress("rangs.eshop@gmail.com");
            mM2.From = new MailAddress(_mailFrom);
            //PW:Exampass@567

            //mM2.To.Add(new MailAddress(Session["sEmail"].ToString()));
            // mM2.To.Add(new MailAddress("golummohammedmohiuddin@gmail.com"));
            mM2.To.Add(new MailAddress("md.sofiqulislamce@gmail.com"));
            // mM2.CC.Add(new MailAddress(_mailErrorTo));
            //mM2.CC.Add(new MailAddress(lblCTPEmail.Text));
            //mM2.Bcc.Add(new MailAddress("zunayedqu10@gmail.com"));
            //mM2.Bcc.Add(new MailAddress("minto@rangs.com.bd"));

            mM2.Bcc.Add(new MailAddress("sofiqul@rangs.com.bd"));

            mM2.Subject = "error_email_sending";
            //mM2.Body = "<h1>Order Details</h1>";
            mM2.Body = "1." + ex.Message + "...2." + ex.InnerException;

            mM2.Body = mM2.Body + "<p>" + txtCHNo.Text.ToString() + "<br/>";
            //mM2.Body = mM2.Body + "We really appreciate it and we are taking necessary steps to process this order.";
            //mM2.Body = mM2.Body + "as soon as possible. You will be updated about next step immediate.";
            mM2.Body = mM2.Body + "</p>";
            mM2.IsBodyHtml = true;
            mM2.Priority = MailPriority.High;
            SmtpClient sC1 = new SmtpClient(_mailSmtpClient);
            sC1.Port = 587;
            //sC1.Port = 2525;
            sC1.Credentials = new System.Net.NetworkCredential(_mailUserName, _mailPassword);
            //sC.EnableSsl = true;
            sC1.Send(mM2);
            // Deal with an exception if one is thrown by the code in the try block
            //
        }

    }


    //CLEAR ALL TEXT AND GRID
    protected void btnCancel_Click(object sender, EventArgs e)
    {

        //CLEAR ALL TEXT
        txtCHNo.Text = "";
        txtDate.Text = "";
        txtDate.Text = DateTime.Today.ToString("dd/MM/yyyy");

        txtCustContact.Text = "";
        txtCustName.Text = "";
        txtCustAdd.Text = "";
        txtEmail.Text = "";

        txtCode.Text = "";
        txtProdID.Text = "";
        txtProdDesc.Text = "";
        txtUP.Text = "";
        txtCP.Text = "";
        txtQty.Text = "";
        txtTotalAmnt.Text = "";
        txtDisAmnt.Text = "";
        //txtDisCode.Text = "";
        //ddlRefDiscount.SelectedItem.Text = "";
        txtWithAdj.Text = "";
        txtNet.Text = "";
        txtSL.Text = "";
        //txtRemarks.Text = "";

        //CLEAR GRIDVIEW
        gvUsers.DataSource = null;
        gvUsers.DataBind();

        //CLEAR DATA TABLE
        dt.Clear();


        txtNetAmnt.Text = "0";
        txtCash.Text = "0";
        txtDue.Text = "0";
        txtCardAmnt1.Text = "0";
        txtCardAmnt2.Text = "0";

        txtChequeNo.Text = "";
        txtBankName.Text = "";
        txtSecurityCode.Text = "";
        txtApprovalCode1.Text = "";

        txtChequeNo2.Text = "";
        txtBankName2.Text = "";
        txtSecurityCode2.Text = "";
        txtApprovalCode2.Text = "";

        //LOAD AUTO BILL NUMBER
        fnLoadAutoBillNo();

        txtCustName.Focus();

        //vDeclare.sBillNo = "";
        //LoadDiscountReferenceList();

    }


    //Grid View Footer Total
    protected void gvUsers_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {


            CalcTotalQty(e.Row.Cells[4].Text);
            CalcTotal_TP(e.Row.Cells[5].Text);
            CalcTotal_Dis(e.Row.Cells[6].Text);
            CalcTotal_With(e.Row.Cells[9].Text);
            CalcTotal(e.Row.Cells[10].Text);

            double value2 = Convert.ToDouble(e.Row.Cells[2].Text);
            e.Row.Cells[2].Text = value2.ToString("0");

            double value3 = Convert.ToDouble(e.Row.Cells[3].Text);
            e.Row.Cells[3].Text = value3.ToString("0");

            double value4 = Convert.ToDouble(e.Row.Cells[4].Text);
            e.Row.Cells[4].Text = value4.ToString("0");

            double value5 = Convert.ToDouble(e.Row.Cells[5].Text);
            e.Row.Cells[5].Text = value5.ToString("0");

            double value6 = Convert.ToDouble(e.Row.Cells[6].Text);
            e.Row.Cells[6].Text = value6.ToString("0");

            double value9 = Convert.ToDouble(e.Row.Cells[9].Text);
            e.Row.Cells[9].Text = value9.ToString("0");

            double value10 = Convert.ToDouble(e.Row.Cells[10].Text);
            e.Row.Cells[10].Text = value10.ToString("0") ;
            this.lblNetAmnt.Text = value10.ToString("0");
            this.txtNetAmnt.Text = value10.ToString("0");
            this.txtPay.Text = value10.ToString("0");

            //RIGHT ALIGNMENT
            e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[9].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[10].HorizontalAlign = HorizontalAlign.Right;

        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {

            e.Row.Cells[1].Text = "Total";
            //e.Row.Cells[10].Text = string.Format("{0:c}", runningTotal);
            e.Row.Cells[4].Text = runningTotalQty.ToString("0,0", CultureInfo.InvariantCulture);
            e.Row.Cells[5].Text = runningTotalTP.ToString("0,0", CultureInfo.InvariantCulture);
            e.Row.Cells[6].Text = runningTotalDis.ToString("0,0", CultureInfo.InvariantCulture);
            e.Row.Cells[9].Text = runningTotalWith.ToString("0,0", CultureInfo.InvariantCulture);


           
            //only for online order
        
                e.Row.Cells[10].Text = runningTotal.ToString("0,0", CultureInfo.InvariantCulture);
                string cellValue = e.Row.Cells[10].Text;
                string delivar = ViewState["__DelivariCharge__"].ToString();

                double cellNumericValue = double.Parse(cellValue, CultureInfo.InvariantCulture);
                double delivarNumericValue = double.Parse(delivar, CultureInfo.InvariantCulture);

                // Calculate the sum
                double TotalWithDelivary = cellNumericValue + delivarNumericValue;

                // Convert the sum back to a formatted string 
                string formattedSum = TotalWithDelivary.ToString("0,0", CultureInfo.InvariantCulture);
                e.Row.Cells[10].Text = formattedSum;

                this.lblNetAmnt.Text = TotalWithDelivary.ToString();
                this.txtNetAmnt.Text = TotalWithDelivary.ToString();
                this.txtPay.Text = TotalWithDelivary.ToString();

                this.txtCash.Text = TotalWithDelivary.ToString();
            
          
            



        

            //all sells and without online order

                e.Row.Cells[10].Text = runningTotal.ToString("0,0", CultureInfo.InvariantCulture);
           //// string delivar = ViewState["__DelivariCharge__"].ToString();
           // //this.lblNetAmnt.Text = runningTotal.ToString("0,0", CultureInfo.InvariantCulture);
                //this.lblNetAmnt.Text = runningTotal.ToString();
                //this.txtNetAmnt.Text = runningTotal.ToString();
                //this.txtPay.Text = runningTotal.ToString();
                //this.txtCash.Text = runningTotal.ToString();



            //RIGHT ALIGNMENT
            e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[9].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[10].HorizontalAlign = HorizontalAlign.Right;
        }

    }

    //CALCULATE NET AMOUNT
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

    //CALCULATE DISCOUNT AMOUNT
    private void CalcTotal_Dis(string _price)
    {
        try
        {
            runningTotalDis += Double.Parse(_price);
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write(ex.ToString());
        }
    }

    //CALCULATE WITH/Adj AMOUNT
    private void CalcTotal_With(string _price)
    {
        try
        {
            runningTotalWith += Double.Parse(_price);
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


    protected void btnPrint_Click(object sender, EventArgs e)
    {

        Response.Redirect("Sales_Bill_Print.aspx");

    }

    protected void ddlPayType_SelectedIndexChanged(object sender, EventArgs e)
    {
        //
        if (this.ddlPayType.SelectedValue == "CASH")
        {
            this.lblNo.Visible = false;
            this.txtChequeNo.Visible = false;
            this.lblBankName.Visible = false;
            this.txtBankName.Visible = false;
            this.lblIssueDate.Visible = false;
            this.txtIssueDate.Visible = false;
            this.lblSecurityCode.Visible = false;
            this.txtSecurityCode.Visible = false;
        }
        else if (this.ddlPayType.SelectedValue == "CHEQUE")
        {
            this.lblNo.Visible = true;
            this.lblNo.Text = "Cheque #";
            this.txtChequeNo.Visible = true;
            this.lblBankName.Visible = true;
            this.txtBankName.Visible = true;
            this.lblIssueDate.Visible = true;
            this.lblIssueDate.Text = "Cheque Date";
            this.txtIssueDate.Visible = true;
            this.lblSecurityCode.Visible = true;
            this.txtSecurityCode.Visible = true;
        }
        else if (this.ddlPayType.SelectedValue == "AMEX")
        {
            this.lblNo.Visible = true;
            this.lblNo.Text = "AMEX Card #";
            this.txtChequeNo.Visible = true;
            this.lblBankName.Visible = true;
            this.txtBankName.Visible = true;
            this.lblIssueDate.Visible = true;
            this.txtIssueDate.Visible = true;
            this.lblSecurityCode.Visible = true;
            this.txtSecurityCode.Visible = true;
        }
        else if (this.ddlPayType.SelectedValue == "VISA CARD")
        {
            this.lblNo.Visible = true;
            this.lblNo.Text = "VISA Card #";
            this.txtChequeNo.Visible = true;
            this.lblBankName.Visible = true;
            this.txtBankName.Visible = true;
            this.lblIssueDate.Visible = true;
            this.txtIssueDate.Visible = true;
            this.lblSecurityCode.Visible = true;
            this.txtSecurityCode.Visible = true;
        }
        else if (this.ddlPayType.SelectedValue == "MASTER CARD")
        {
            this.lblNo.Visible = true;
            this.lblNo.Text = "MASTER Card #";
            this.txtChequeNo.Visible = true;
            this.lblBankName.Visible = true;
            this.txtBankName.Visible = true;
            this.lblIssueDate.Visible = true;
            this.txtIssueDate.Visible = true;
            this.lblSecurityCode.Visible = true;
            this.txtSecurityCode.Visible = true;
        }
        else if (this.ddlPayType.SelectedValue == "IPDC")
        {
            this.lblNo.Visible = true;
            this.lblNo.Text = "IPDC #";
            this.txtChequeNo.Visible = true;
            this.lblBankName.Visible = true;
            this.txtBankName.Visible = true;
            this.lblIssueDate.Visible = true;
            this.txtIssueDate.Visible = true;
            this.lblSecurityCode.Visible = true;
            this.txtSecurityCode.Visible = true;
        }
        else if (this.ddlPayType.SelectedValue == "DD")
        {
            this.lblNo.Visible = true;
            this.lblNo.Text = "DD #";
            this.txtChequeNo.Visible = true;
            this.lblBankName.Visible = true;
            this.txtBankName.Visible = true;
            this.lblIssueDate.Visible = true;
            this.lblIssueDate.Text = "DD Date";
            this.txtIssueDate.Visible = true;
            this.lblSecurityCode.Visible = true;
            this.txtSecurityCode.Visible = true;
        }
        else if (this.ddlPayType.SelectedValue == "TT")
        {
            this.lblNo.Visible = true;
            this.lblNo.Text = "TT #";
            this.txtChequeNo.Visible = true;
            this.lblBankName.Visible = true;
            this.txtBankName.Visible = true;
            this.lblIssueDate.Visible = true;
            this.lblIssueDate.Text = "TT Date";
            this.txtIssueDate.Visible = true;
            this.lblSecurityCode.Visible = true;
            this.txtSecurityCode.Visible = true;
        }
        else
        {
            this.lblNo.Visible = false;
            this.txtChequeNo.Visible = false;
            this.lblBankName.Visible = false;
            this.txtBankName.Visible = false;
            this.lblIssueDate.Visible = false;
            this.txtIssueDate.Visible = false;
            this.lblSecurityCode.Visible = false;
            this.txtSecurityCode.Visible = false;
        }
    }


    protected void txtCardAmnt1_TextChanged(object sender, EventArgs e)
    {

        if (this.txtNetAmnt.Text == "")
        {
            this.txtNetAmnt.Text = "0";
        }

        if (this.txtCardAmnt1.Text.Length == 0)
        {
            this.txtCardAmnt1.Text = "0";
        }

        if (this.txtCardAmnt2.Text.Length == 0)
        {
            this.txtCardAmnt2.Text = "0";
        }

        if (this.txtCash.Text.Length == 0)
        {
            this.txtCash.Text = "0";
        }

        double dTotalPay = 0;
        dTotalPay = Convert.ToDouble(this.txtCash.Text) + Convert.ToDouble(this.txtCardAmnt1.Text) + Convert.ToDouble(this.txtCardAmnt2.Text);
        this.txtPay.Text = Convert.ToString(dTotalPay);

        double dDue = 0;
        // Redeem Point
        //dDue = Convert.ToDouble(this.lblNetAmnt.Text) - dTotalPay;
        dDue = Convert.ToDouble(this.txtNetAmnt.Text) - dTotalPay;
        this.txtDue.Text = Convert.ToString(dDue);

    }

    protected void txtCardAmnt2_TextChanged(object sender, EventArgs e)
    {
        if (this.txtNetAmnt.Text == "")
        {
            this.txtNetAmnt.Text = "0";
        }

        if (this.txtCardAmnt1.Text.Length == 0)
        {
            this.txtCardAmnt1.Text = "0";
        }

        if (this.txtCardAmnt2.Text.Length == 0)
        {
            this.txtCardAmnt2.Text = "0";
        }

        if (this.txtCash.Text.Length == 0)
        {
            this.txtCash.Text = "0";
        }

        double dTotalPay = 0;
        dTotalPay = Convert.ToDouble(this.txtCash.Text) + Convert.ToDouble(this.txtCardAmnt1.Text) + Convert.ToDouble(this.txtCardAmnt2.Text);
        this.txtPay.Text = Convert.ToString(dTotalPay);

        double dDue = 0;
        // Redeem Point
        //dDue = Convert.ToDouble(this.lblNetAmnt.Text) - dTotalPay;
        dDue = Convert.ToDouble(this.txtNetAmnt.Text) - dTotalPay;
        this.txtDue.Text = Convert.ToString(dDue);
    }

    protected void txtCash_TextChanged(object sender, EventArgs e)
    {

        if (this.txtNetAmnt.Text == "")
        {
            this.txtNetAmnt.Text = "0";
        }

        if (this.txtCardAmnt1.Text.Length == 0)
        {
            this.txtCardAmnt1.Text = "0";
        }

        if (this.txtCardAmnt2.Text.Length == 0)
        {
            this.txtCardAmnt2.Text = "0";
        }

        if (this.txtCash.Text.Length == 0)
        {
            this.txtCash.Text = "0";
        }

        double dTotalPay = 0;
        dTotalPay = Convert.ToDouble(this.txtCash.Text) + Convert.ToDouble(this.txtCardAmnt1.Text) + Convert.ToDouble(this.txtCardAmnt2.Text);
        this.txtPay.Text = Convert.ToString(dTotalPay);

        double dDue = 0;
        //dDue = Convert.ToDouble(this.lblNetAmnt.Text) - (Convert.ToDouble(this.txtCash.Text) + Convert.ToDouble(this.txtCardAmnt1.Text) + Convert.ToDouble(this.txtCardAmnt2.Text));
        // Redeem Point
        //dDue = Convert.ToDouble(this.lblNetAmnt.Text) - dTotalPay;
        dDue = Convert.ToDouble(this.txtNetAmnt.Text) - dTotalPay;
        this.txtDue.Text = Convert.ToString(dDue);
    }

    protected void txtCode_TextChanged(object sender, EventArgs e)
    {
        if (this.txtCode.Text == "27010024")
        {
            this.txtCode.Text = "";
        }
        SqlConnection conn = DBConnection.GetConnection();
        string sSql = "";
        double UP = 0;
        double CampPrice = 0;

        sSql = "";

        sSql = "SELECT ProductID,ProdName,UnitPrice,Model,Code,";
        sSql = sSql + " IncentiveType,BLIPAmnt,IncentiveAmnt,";
        sSql = sSql + " GetIncentive, ISNULL(WPPrice,0) AS WPPrice, ISNULL(BLIPofWP,0) AS BLIPofWP,";
        sSql = sSql + " ISNULL(WPIncentive,0) AS WPIncentive, ISNULL(WPMinQty,0) AS WPMinQty";
        sSql = sSql + " FROM Product";

        sSql = sSql + " WHERE Code='" + this.txtCode.Text + "'";
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

                this.txtProdDesc.Text = dr["ProdName"].ToString();
                UP = Convert.ToDouble(dr["UnitPrice"].ToString());
                this.txtUP.Text = Convert.ToString(UP);

                this.lblIncentiveType.Text = dr["IncentiveType"].ToString();
                this.lblBLIPAmnt.Text = dr["BLIPAmnt"].ToString();
                this.lblIncentiveAmnt.Text = dr["IncentiveAmnt"].ToString();
                this.lblGetIncentive.Text = dr["GetIncentive"].ToString();
                this.lblWPPrice.Text = dr["WPPrice"].ToString();
                this.lblBLIPofWP.Text = dr["BLIPofWP"].ToString();
                this.lblWPIncentive.Text = dr["WPIncentive"].ToString();
                this.lblWPMinQty.Text = dr["WPMinQty"].ToString();
            }
            else
            {
                UP = 0;
                this.txtUP.Text = Convert.ToString(UP);
                this.txtProdID.Text = "";
                this.txtProdDesc.Text = "";
                this.ddlContinents.SelectedItem.Text = "";

                this.lblIncentiveType.Text = "";
                this.lblBLIPAmnt.Text = "0";
                this.lblIncentiveAmnt.Text = "0";
                this.lblGetIncentive.Text = "0";
                this.lblWPPrice.Text = "0";
                this.lblBLIPofWP.Text = "0";
                this.lblWPIncentive.Text = "0";
                this.lblWPMinQty.Text = "0";

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
            " WHERE Model='" + this.ddlContinents.SelectedValue + "'" +
            " AND (EffectiveDate<='" + DateTime.Today + "' AND cStop=0)" +
            " ORDER BY EffectiveDate DESC";
        cmd = new SqlCommand(sSql, conn);
        conn.Open();
        dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            CampPrice = UP - Convert.ToDouble(dr["DisAmnt"].ToString());
            this.txtCP.Text = Convert.ToString(CampPrice);
        }
        else
        {
            CampPrice = UP;
            this.txtCP.Text = Convert.ToString(CampPrice);
        }
        dr.Dispose();
        dr.Close();
        conn.Close();
        //------------------------------------------------------

        //lblUP.Text = txtCP.Text;
        //if (lblGetIncentive.Text == "True")
        //{
        //    if (lblIncentiveType.Text == "Instant")
        //    {
        //        this.txtCP.Text = lblBLIPAmnt.Text;
        //    }
        //    else
        //    {
        //        this.txtCP.Text = Convert.ToString(CampPrice);
        //    }
        //}
        //------------------------------------------------------

    }

    //LOAD CUSTOMER INFO
    protected void btnCustSearch_Click(object sender, ImageClickEventArgs e)
    {
        SqlConnection conn = DBConnection.GetConnection();

        if (txtCustContact.Text == "")
        {
            PopupMessage("Please enter Customer Contact #.", btnSave);
            txtCustContact.Focus();
            return;
        }

        //CHECK & INSERT CUSTOMER INFO
        string sSql = "";
        sSql = "SELECT * FROM Customer" +
            " WHERE Mobile='" + this.txtCustContact.Text + "'";
        SqlCommand cmdCust = new SqlCommand(sSql, conn);
        conn.Open();
        SqlDataReader drCust = cmdCust.ExecuteReader();
        try
        {
            if (drCust.Read())
            {
                this.txtCustomerMobile.Text = this.txtCustContact.Text;
                this.txtCustomerMobile.Enabled = false;
                this.txtCustName.Text = drCust["CustName"].ToString();
                this.txtCustAdd.Text = drCust["Address"].ToString();
                this.txtEmail.Text = drCust["Email"].ToString();
                this.ddlCity.SelectedItem.Text = drCust["City"].ToString();
                this.txtDOB.Text = drCust["DOBT"].ToString();
                this.txtOrg.Text = drCust["Org"].ToString();
                this.txtDesg.Text = drCust["Desg"].ToString();

                if (drCust["Profession"].ToString() == "Business")
                {
                    optProfession.SelectedIndex = 0;
                }
                else if (drCust["Profession"].ToString() == "Service")
                {
                    optProfession.SelectedIndex = 1;
                }
                else if (drCust["Profession"].ToString() == "Others")
                {
                    optProfession.SelectedIndex = 2;
                }


                if (drCust["CustSex"].ToString() == "Male")
                {
                    optSex.SelectedIndex = 0;
                }
                else if (drCust["CustSex"].ToString() == "Female")
                {
                    optSex.SelectedIndex = 1;
                }

            }
            else
            {
                this.txtCustName.Text = "";
                this.txtCustAdd.Text = "";
                this.txtEmail.Text = "";
                this.ddlCity.SelectedItem.Text = "";
                this.txtDOB.Text = "";
                this.txtOrg.Text = "";
                this.txtDesg.Text = "";
            }
        }
        catch (InvalidCastException err)
        {
            throw (err);
        }
        finally
        {
            drCust.Dispose();
            drCust.Close();
            conn.Close();
        }
        //----------------------------------------------------------------------

    }

    protected void txtModel_TextChanged(object sender, EventArgs e)
    {
        SqlConnection conn = DBConnection.GetConnection();
        double UP = 0;
        double dBLIP = 0;
        double CampPrice = 0;
        string sSql = "";

        sSql = "";
        sSql = "SELECT ProductID,ProdName,UnitPrice,Model,Code,";
        sSql = sSql + " IncentiveType,BLIPAmnt,IncentiveAmnt,";
        sSql = sSql + " GetIncentive, ISNULL(WPPrice,0) AS WPPrice, ISNULL(BLIPofWP,0) AS BLIPofWP,";
        sSql = sSql + " ISNULL(WPIncentive,0) AS WPIncentive, ISNULL(WPMinQty,0) AS WPMinQty";
        sSql = sSql + " FROM Product";
        sSql = sSql + " WHERE Model='" + this.txtModel.Text + "'";

        SqlCommand cmd = new SqlCommand(sSql, conn);
        conn.Open();
        SqlDataReader dr = cmd.ExecuteReader();

        try
        {
            if (dr.Read())
            {
                this.txtCode.Text = dr["Code"].ToString();
                this.ddlContinents.SelectedItem.Text = dr["Model"].ToString();
                this.txtProdID.Text = dr["ProductID"].ToString();
                this.txtProdDesc.Text = dr["ProdName"].ToString();
                UP = Convert.ToDouble(dr["UnitPrice"].ToString());
                this.txtUP.Text = Convert.ToString(UP);
                dBLIP = Convert.ToDouble(dr["BLIPAmnt"].ToString());
                this.txtBLIPAmnt.Text = Convert.ToString(dBLIP);

                this.lblIncentiveType.Text = dr["IncentiveType"].ToString();
                this.lblBLIPAmnt.Text = dr["BLIPAmnt"].ToString();
                this.lblIncentiveAmnt.Text = dr["IncentiveAmnt"].ToString();
                this.lblGetIncentive.Text = dr["GetIncentive"].ToString();
                this.lblWPPrice.Text = dr["WPPrice"].ToString();
                this.lblBLIPofWP.Text = dr["BLIPofWP"].ToString();
                this.lblWPIncentive.Text = dr["WPIncentive"].ToString();
                this.lblWPMinQty.Text = dr["WPMinQty"].ToString();
            }
            else
            {
                UP = 0;
                dBLIP = 0;
                this.txtUP.Text = Convert.ToString(UP);
                this.txtCode.Text = "";
                this.ddlContinents.SelectedItem.Text = "";
                this.txtProdID.Text = "";
                this.txtProdDesc.Text = "";

                this.lblIncentiveType.Text = "";
                this.lblBLIPAmnt.Text = "0";
                this.lblIncentiveAmnt.Text = "0";
                this.lblGetIncentive.Text = "0";
                this.lblWPPrice.Text = "0";
                this.lblBLIPofWP.Text = "0";
                this.lblWPIncentive.Text = "0";
                this.lblWPMinQty.Text = "0";

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
            this.txtCP.Text = Convert.ToString(CampPrice);
        }
        else
        {
            CampPrice = UP;
            this.txtCP.Text = Convert.ToString(CampPrice);
        }
        dr.Dispose();
        dr.Close();
        conn.Close();
        //------------------------------------------------------

        //lblUP.Text = txtCP.Text;
        //if (lblGetIncentive.Text == "True")
        //{
        //    if (lblIncentiveType.Text == "Instant")
        //    {
        //        this.txtCP.Text = lblBLIPAmnt.Text;
        //    }
        //    else
        //    {
        //        this.txtCP.Text = Convert.ToString(CampPrice);
        //    }
        //}
        //------------------------------------------------------

    }

    protected void btnSearchSalesBy_Click(object sender, ImageClickEventArgs e)
    {
        SqlConnection conn = DBConnection.GetConnection();
        SqlConnection connHR = DBConnectionHRM.GetConnection();

        if (txtJobID.Text == "")
        {
            PopupMessage("Please enter Customer Contact #.", btnSave);
            txtJobID.Focus();
            return;
        }

        //CHECK & INSERT CUSTOMER INFO
        string sSql = "";
        sSql = "SELECT * FROM vw_EmpInfo WHERE JobCod='" + this.txtJobID.Text + "'";
        SqlCommand cmdCust = new SqlCommand(sSql, connHR);
        connHR.Open();
        SqlDataReader drCust = cmdCust.ExecuteReader();
        try
        {
            if (drCust.Read())
            {
                this.lblSalesBy.Text = drCust["FullName"].ToString() + ", " + drCust["Desg"].ToString() + ", " + drCust["DeptNm"].ToString() + ", " + drCust["Location"].ToString();
            }
            else
            {
                //this.lblSalesBy.Text = "";  
                sSql = "";
                sSql = "SELECT EID,EntityCode,eName,ContactPerson,ContactNo FROM Entity WHERE EntityCode='" + this.txtJobID.Text + "'";
                SqlCommand cmdE = new SqlCommand(sSql, conn);
                conn.Open();
                SqlDataReader drE = cmdE.ExecuteReader();
                if (drE.Read())
                {
                    this.lblSalesBy.Text = drE["ContactPerson"].ToString() + ", " + drE["eName"].ToString();
                }
                else
                {
                    this.lblSalesBy.Text = "";
                }
                drE.Dispose();
                drE.Close();
                conn.Close();

            }
        }
        catch (InvalidCastException err)
        {
            throw (err);
        }
        finally
        {
            drCust.Dispose();
            drCust.Close();
            connHR.Close();
        }
        //----------------------------------------------------------------------

    }

    private void fnClaimList()
    {
        SqlConnection conn = DBConnection.GetConnection();
        //using (SqlConnection conn = new SqlConnection())
        //{
        //conn.ConnectionString = ConfigurationManager
        //.ConnectionStrings["constr"].ConnectionString;

        using (SqlCommand cmd = new SqlCommand())
        {
            cmd.CommandText = "Select TAIC, TermsCondition from tbTC where status='True' ORDER BY OrderBy";
            cmd.Connection = conn;
            conn.Open();
            using (SqlDataReader sdr = cmd.ExecuteReader())
            {
                while (sdr.Read())
                {
                    ListItem item = new ListItem();
                    item.Text = sdr["TermsCondition"].ToString();
                    item.Value = sdr["TAIC"].ToString();
                    //item.Selected = Convert.ToBoolean(sdr["IsSelected"]);
                    if (item.Value == "37" && Session["UserName"].ToString() == "admin@ditf23")
                    {
                        chkTC.Items.Add(item);
                    }
                    else 
                    {
                        if (item.Value != "37")
                        {
                            chkTC.Items.Add(item);
                        }
                        
                    }
                    
                }
            }
            conn.Close();
        }
        //}
    }

    //protected void TextBox1_TextChanged(object sender, EventArgs e)
    //{

    //}
    protected void btnCouponSearch_Click(object sender, ImageClickEventArgs e)
    {
        SqlConnection conn = DBConnectionSpin.GetConnection();

        if (TxtSpinCouponNumber.Text == "")
        {
            PopupMessage("Please enter Coupon Number #.", btnCouponSearch);
            TxtSpinCouponNumber.Focus();
            return;
        }


        //if (txtCustomerMobile.Text != txtCustContact.Text || txtCustContact.Text == "")
        //{
        //    PopupMessage("Contact No Dont Match with Customer information you have added to customer information or fill up customer information first", btnVoucherSearch);
        //    //txtRedeemPoint.Text = "0";
        //    txtCustomerMobile.Focus();
        //    return;
        //}
        //  CHECK & INSERT CUSTOMER INFO
        string sSql = "";
        sSql = @"SELECT [CustName],[CustAdd],[CustMobile],[ProductID],[Model]      
                ,[DisPer],[ChNo],[sTag],[EID],[DisCodeTag]     
                FROM [dbSpinToWin].[dbo].[tbCustomer] where ChNo='" + TxtSpinCouponNumber.Text.Trim() + "' --and sTag=0 and DisCode = 0;";
        SqlCommand cmdCust = new SqlCommand(sSql, conn);
        conn.Open();
        SqlDataReader drSpin = cmdCust.ExecuteReader();
        try
        {
            if (drSpin.Read())
            {
                if (drSpin["sTag"].ToString() == "0")
                {
                    PopupMessage("You didnt completed all steps of Goal and Score game.Please go to the Goal and Score game then play and submit the discount#.", btnCouponSearch);
                    TxtSpinCouponNumber.Focus();
                    return;
                }
                else if (drSpin["sTag"].ToString() == "1" && drSpin["DisCodeTag"].ToString() == "0")
                {
                    this.txtSpinCpouponDiscountAmnt.Text = drSpin["DisPer"].ToString();
                    this.txtSpinCpouponDiscountAmnt.Font.Bold = true;
                    this.txtCustName.Text = drSpin["CustName"].ToString();
                    this.txtCustContact.Text = drSpin["CustMobile"].ToString();
                    this.txtCustAdd.Text = drSpin["CustAdd"].ToString();
                    this.txtSpinModel.Text = drSpin["Model"].ToString();
                }
                else
                {
                    PopupMessage("You already used availed this coupon on dms sales declare #.", btnCouponSearch);
                    TxtSpinCouponNumber.Focus();
                    return;
                }

            }
            else
            {
                this.txtSpinCpouponDiscountAmnt.Text = "";
            }


        }
        catch (InvalidCastException err)
        {
            throw (err);
        }
        finally
        {
            drSpin.Dispose();
            drSpin.Close();
            conn.Close();
        }
    }
    protected void btnSpinCoupon_Click(object sender, EventArgs e)
    {
        txtModel.Text = txtSpinModel.Text;
        modelwiseproductinfo(txtSpinModel.Text);
        txtQty.Text = "1";
        txtDisAmnt.Text = txtSpinCpouponDiscountAmnt.Text;


        if (txtCP.Text.Length == 0)
        {
            this.txtCP.Text = "0";
        }

        if (Convert.ToInt16(this.lblWPMinQty.Text) > 0)
        {
            if (Convert.ToInt16(txtQty.Text) >= Convert.ToInt16(this.lblWPMinQty.Text))
            {
                txtCP.Text = lblWPPrice.Text;
            }
        }

        if (txtTotalAmnt.Text.Length == 0)
        {
            this.txtTotalAmnt.Text = "0";
        }
        if (txtDisAmnt.Text.Length == 0)
        {
            this.txtDisAmnt.Text = "0";
        }
        if (txtWithAdj.Text.Length == 0)
        {
            this.txtWithAdj.Text = "0";
        }
        //if (txtCP.Text.Length > 0)
        //{

        this.txtTotalAmnt.Text = Convert.ToString(Convert.ToDouble(this.txtQty.Text) * Convert.ToDouble(this.txtCP.Text));
        //this.txtDisAmnt.Text = "0";
        //this.txtWithAdj.Text = "0";

        double dNet = 0;
        dNet = Convert.ToDouble(this.txtTotalAmnt.Text) - Convert.ToDouble(this.txtDisAmnt.Text) - Convert.ToDouble(this.txtWithAdj.Text);
        this.txtNet.Text = Convert.ToString(dNet);

        ddlRefDiscount.Items.Insert(5, new ListItem("Goal and Score", "Goal and Score"));
        ddlRefDiscount.SelectedValue = "Goal and Score";
        AddRows();
        gvUsers.DataSource = dt;
        gvUsers.DataBind();

        //AddRows();
        ddlRefDiscount.Items.RemoveAt(5);
        this.TxtSpinCouponNumber.Enabled = false;
        this.txtSpinCpouponDiscountAmnt.Enabled = false;
        this.txtCustName.Enabled = false;
        this.txtCustContact.Enabled = false;
        this.btnCouponSearch.Enabled = false;
        this.btnSpinCoupon.Enabled = false;

        if (this.GroupName.Text == "SONY LCD TV")
        {
            if (txtSpinModel.Text == "KD-65X80J" || txtSpinModel.Text == "KD-75X80J" ||
                txtSpinModel.Text == "KD-85X8000H" || txtSpinModel.Text == "XR-55A80J" ||
                txtSpinModel.Text == "XR-55A80J" || txtSpinModel.Text == "XR-55A80K"
                || txtSpinModel.Text == "XR-65A80K")
            {
                this.btnAvailOffer.Enabled = false;
            }
        }
        //
        this.btnRedeem.Enabled = false;
    }

    protected void ddlEMEIInfo_SelectedIndexChanged(object sender, EventArgs e)
    {
        int totalAmntFooter = 0;
        int totalNetFooter = 0;
        double dTAmnt = 0;
        double emiCharge = 0;

        if (ddlCardType1.SelectedItem.Text == "VISA" || ddlCardType1.SelectedItem.Text == "MASTER" || ddlCardType1.SelectedItem.Text == "AMEX" ||
            ddlCardType1.SelectedItem.Text == "NAGAD" || ddlCardType1.SelectedItem.Text == "BKash" || ddlCardType1.SelectedItem.Text == "OTHERS")
        {
            PopupMessage("Wrong selection.Please select EMI from CARD Type 1 to claim EMI faccility or select IPDC from CARD Type 1 to claim IPDC faccility ", btnCouponSearch);
            ddlCardType1.Focus();
            ddlEMEIInfo.SelectedIndex = 0;
            return;
        }

        if (ddlCardType1.SelectedItem.Text == "IPDC")
        {
            ddlEMEIInfo.Enabled = false;
            return;
        }

        if (ddlEMEIInfo.SelectedItem.Text == "6 Months")
        {
            ddlEMEIInfo.Enabled = false;
            return;
        }
        else if (ddlEMEIInfo.SelectedItem.Text == "12 Months")
        {
            ddlEMEIInfo.Enabled = false;
            return;
        }
        else if (ddlEMEIInfo.SelectedItem.Text == "9 Months")
        {
            ddlEMEIInfo.Enabled = false;
            return;
        }
        else if (ddlEMEIInfo.SelectedItem.Text == "18 Months")
        {
            emiCharge = 4;
            foreach (GridViewRow g1 in this.gvUsers.Rows)
            {
                if (Convert.ToInt32(g1.Cells[3].Text) != 0)
                {
                    double disPer = 0;

                    double mrp = Convert.ToDouble(g1.Cells[2].Text);
                    double campPrice = Convert.ToDouble(g1.Cells[3].Text);
                    disPer = ((mrp - campPrice) / mrp) * 100;

                    if (mrp > campPrice)
                    {
                        g1.Cells[3].Text = Math.Floor(Convert.ToInt32(g1.Cells[2].Text) - Convert.ToInt32(g1.Cells[2].Text) * (disPer - emiCharge) / 100).ToString();
                        g1.Cells[5].Text = (Convert.ToInt32(g1.Cells[4].Text) * Convert.ToInt32(g1.Cells[3].Text)).ToString();

                        g1.Cells[10].Text = g1.Cells[5].Text;
                        if (Convert.ToInt32(g1.Cells[6].Text) > 0)
                        {
                            g1.Cells[10].Text = (Convert.ToInt32(g1.Cells[10].Text) - Convert.ToInt32(g1.Cells[6].Text)).ToString();
                        }

                        if (Convert.ToInt32(g1.Cells[9].Text) > 0)
                        {
                            g1.Cells[10].Text = (Convert.ToInt32(g1.Cells[10].Text) - Convert.ToInt32(g1.Cells[9].Text)).ToString();
                        }


                    }

                    totalAmntFooter += Convert.ToInt32(g1.Cells[5].Text);
                    totalNetFooter += Convert.ToInt32(g1.Cells[10].Text);
                }
            }
        }
        else if (ddlEMEIInfo.SelectedItem.Text == "24 Months")
        {

            emiCharge = 6;
            foreach (GridViewRow g1 in this.gvUsers.Rows)
            {
                if (Convert.ToInt32(g1.Cells[3].Text) != 0)
                {
                    double disPer = 0;

                    double mrp = Convert.ToDouble(g1.Cells[2].Text);
                    double campPrice = Convert.ToDouble(g1.Cells[3].Text);
                    disPer = ((mrp - campPrice) / mrp) * 100;

                    if (mrp > campPrice)
                    {
                        g1.Cells[3].Text = Math.Floor(Convert.ToInt32(g1.Cells[2].Text) - Convert.ToInt32(g1.Cells[2].Text) * (disPer - emiCharge) / 100).ToString();
                        g1.Cells[5].Text = (Convert.ToInt32(g1.Cells[4].Text) * Convert.ToInt32(g1.Cells[3].Text)).ToString();

                        g1.Cells[10].Text = g1.Cells[5].Text;
                        if (Convert.ToInt32(g1.Cells[6].Text) > 0)
                        {
                            g1.Cells[10].Text = (Convert.ToInt32(g1.Cells[10].Text) - Convert.ToInt32(g1.Cells[6].Text)).ToString();
                        }

                        if (Convert.ToInt32(g1.Cells[9].Text) > 0)
                        {
                            g1.Cells[10].Text = (Convert.ToInt32(g1.Cells[10].Text) - Convert.ToInt32(g1.Cells[9].Text)).ToString();
                        }


                    }

                    totalAmntFooter += Convert.ToInt32(g1.Cells[5].Text);
                    totalNetFooter += Convert.ToInt32(g1.Cells[10].Text);
                }

            }
        }
        else if (ddlEMEIInfo.SelectedItem.Text == "36 Months")
        {
            emiCharge = 10;
            foreach (GridViewRow g1 in this.gvUsers.Rows)
            {
                //g1.Cells[6].Text = Convert.ToString(discountValue + redeemByRatioValue);
                if (Convert.ToInt32(g1.Cells[3].Text) != 0)
                {
                    double disPer = 0;
                    double mrp = Convert.ToDouble(g1.Cells[2].Text);
                    double campPrice = Convert.ToDouble(g1.Cells[3].Text);
                    disPer = ((mrp - campPrice) / mrp) * 100;

                    if (mrp > campPrice)
                    {
                        g1.Cells[3].Text = Math.Floor(Convert.ToInt32(g1.Cells[2].Text) - Convert.ToInt32(g1.Cells[2].Text) * (disPer - emiCharge) / 100).ToString();


                        g1.Cells[5].Text = (Convert.ToInt32(g1.Cells[4].Text) * Convert.ToInt32(g1.Cells[3].Text)).ToString();

                        g1.Cells[10].Text = g1.Cells[5].Text;
                        if (Convert.ToInt32(g1.Cells[6].Text) > 0)
                        {
                            g1.Cells[10].Text = (Convert.ToInt32(g1.Cells[10].Text) - Convert.ToInt32(g1.Cells[6].Text)).ToString();
                        }

                        if (Convert.ToInt32(g1.Cells[9].Text) > 0)
                        {
                            g1.Cells[10].Text = (Convert.ToInt32(g1.Cells[10].Text) - Convert.ToInt32(g1.Cells[9].Text)).ToString();
                        }
                    }
                    totalAmntFooter += Convert.ToInt32(g1.Cells[5].Text);
                    totalNetFooter += Convert.ToInt32(g1.Cells[10].Text);
                }
            }
        }
        else if (ddlEMEIInfo.SelectedItem.Text == "0 Month")
        {
            ddlEMEIInfo.Enabled = false;
            return;
        }
        //gvUsers.DataSource = gvUsers.DataSource;

        this.gvUsers.FooterRow.Cells[5].Text = totalAmntFooter.ToString();
        this.gvUsers.FooterRow.Cells[10].Text = totalNetFooter.ToString();

        this.txtNetAmnt.Text = totalNetFooter.ToString();
        this.txtCash.Text = totalNetFooter.ToString();
        this.txtPay.Text = totalNetFooter.ToString();


        if (ddlReference.Enabled == false)
        {
            try
            {
                if (txtAvailAmount.Text == "")
                {
                    txtAvailAmount.Text = "";
                }
                if (Convert.ToInt32(txtAvailAmount.Text) > 0)
                {
                    this.txtNetAmnt.Text = (Convert.ToInt32(txtNetAmnt.Text) - Convert.ToInt32(txtAvailAmount.Text)).ToString();
                    this.txtCash.Text = (Convert.ToInt32(txtCash.Text) - Convert.ToInt32(txtAvailAmount.Text)).ToString();
                    this.txtPay.Text = (Convert.ToInt32(txtPay.Text) - Convert.ToInt32(txtAvailAmount.Text)).ToString();
                }
            }
            catch (Exception)
            {

            }
        }
        ddlEMEIInfo.Enabled = false;
    }

    private string  GetProductGroupName(string ModelName)
    {

        

        try
        {
            SqlConnection conn = DBConnection.GetConnection();

            String strQuery = "select GroupName from Product where Model='" + ModelName + "'";
            SqlCommand cmd = new SqlCommand(strQuery, conn);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            return dt.Rows[0]["GroupName"].ToString().Trim();
        }
        catch (Exception ex)
        {
            return "";
        }
        
    }

    // save logdata into database



    public static void  LogFile(string orderNo,string invoiceNo, string comments,string requset, string respons,bool inAPI,bool IsDeliveredFromDMS,bool IsDeliveredFromOnline)
    {
        try
        {
            SqlConnection conn = DBConnection.GetConnection();

           

            if ( HttpContext.Current.Session["__logID__"]== "0")
           {
               int logID = ExecuteReturnID("Insert Into api_logData(OrderNumber,InvoiceNumber,CreatedAt,Comments)values('" + orderNo + "','" + invoiceNo + "',GetDate(),'" + comments + "');SELECT SCOPE_IDENTITY()");
               
                

                HttpContext.Current.Session["__logID__"] = logID;


               if (logID > 0)
               {
                  // Response.Write("Data Saved Succesfully");
                   //Response.Redirect(Request.Url.AbsoluteUri);
               }
               else
               {
                  // Response.Write("Data Saved Failed");
                   //Response.Redirect(Request.Url.AbsoluteUri);
               }
           }
           else
           {
               string exComments = GetComments(HttpContext.Current.Session["__logID__"].ToString());
          
            comments = exComments + ">" + comments;


            conn.Open();
            string query = (inAPI) ? "Update api_logData set Comments='" + comments + "',UpdatedAt=GetDate(),Requests='" + requset + "',Response='" + respons + "' Where Id=" + HttpContext.Current.Session["__logID__"].ToString() : (IsDeliveredFromOnline) ? "Update api_logData set IsDeliveredFromOnline=1, Comments='" + comments + "',UpdatedAt=GetDate() Where Id=" + HttpContext.Current.Session["__logID__"].ToString() : (IsDeliveredFromDMS) ? "Update api_logData set IsDeliveredFromDMS=1, Comments='" + comments + "',UpdatedAt=GetDate() Where Id=" + HttpContext.Current.Session["__logID__"].ToString() : "Update api_logData set Comments='" + comments + "',UpdatedAt=GetDate(),IsDeliveredFromDMS=1 Where Id=" + HttpContext.Current.Session["__logID__"].ToString();

            SqlCommand cmd = new SqlCommand(query, conn);
            int result = int.Parse(cmd.ExecuteNonQuery().ToString());
            conn.Close();
            if (result > 0)
            {
               // Response.Write("Data Saved Succesfully");
             
            }
            else
            {
               // Response.Write("Data Saved Failed");
                
            }
           }
           
        }
        catch (Exception ex)
          {
             
          }
    }

    public static int ExecuteReturnID(string sqlCmd)
    {
        try
        {
            SqlConnection conn = DBConnection.GetConnection();
            conn.Open();
            SqlCommand cmd = new SqlCommand(sqlCmd, conn);
            int result = int.Parse(cmd.ExecuteScalar().ToString());
            conn.Close();
            return result;
        }
        catch (Exception ex) { return 0; }
    }
  
    //getcomments
    private static string GetComments(string logID)
    {

        try
        {
            SqlConnection conn = DBConnection.GetConnection();
            String strQuery = "select Comments from api_logData where Id='" + logID + "'";
            SqlCommand cmd = new SqlCommand(strQuery, conn);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            
            return dt.Rows[0]["Comments"].ToString().Trim();
        }
        catch (Exception ex)
        {
            return "";
        }

    }

    protected void btnchkMail_Click(object sender, EventArgs e) 
    {
        fnSendMail_Invoice_new_test();

    }


    protected bool isMailValid(string email) 
    {
        string regexpression = "";

        try
        {
            regexpression = @"^[a-zA-Z][a-zA-Z0-9._%+-]*@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$";
           
        }
        catch (Exception ex)
        {
            
           
        }
        return Regex.IsMatch(email, regexpression);
    
    }


    private void fnSendMail_Invoice_new_test()
    {



        try
        {

            // Mail to Customer------------------------------------------------------------------------------------

            MailMessage mM2 = new MailMessage();
            mM2.From = new MailAddress(_mailFrom);
            bool isvalid = isMailValid(txtEmail.Text);

            if (txtEmail.Text.Trim() != "" && isvalid)
            {
                mM2.To.Add(new MailAddress("abidhasannayem@gmail.com"));
            }
            else
            {
                //mM2.To.Add(new MailAddress(txtEmail.Text));
                mM2.CC.Add(new MailAddress("nayem.codeware@gmail.com"));
            }

           
          


            mM2.Subject = "Sony-Rangs Invoice No. Test ";
            //mM2.Body = "<h1>Order Details</h1>";
            mM2.Body = "<p>Dear Valued Customer,</p>";
            mM2.Body = mM2.Body + "<p>Thank you for shopping with us.<br/>";
            mM2.Body = mM2.Body + "</p>";

            mM2.IsBodyHtml = true;
            mM2.Priority = MailPriority.High;
            SmtpClient sC1 = new SmtpClient(_mailSmtpClient);
            sC1.Port = 587;
            //sC1.Port = 2525;
            sC1.Credentials = new System.Net.NetworkCredential(_mailUserName, _mailPassword);
            //sC.EnableSsl = true;
            sC1.Send(mM2);



        }
        catch (Exception ex)
        {
            MailMessage mM2 = new MailMessage();
            mM2.From = new MailAddress(_mailFrom);
            mM2.To.Add(new MailAddress(_mailErrorTo));


            mM2.Subject = "error_email_sending";
            //mM2.Body = "<h1>Order Details</h1>";
            mM2.Body = "1." + ex.Message + "...2." + ex.InnerException;


            mM2.IsBodyHtml = true;
            mM2.Priority = MailPriority.High;
            SmtpClient sC1 = new SmtpClient(_mailSmtpClient);
            sC1.Port = 587;
            //sC1.Port = 2525;
            sC1.Credentials = new System.Net.NetworkCredential(_mailUserName, _mailPassword);
            //sC.EnableSsl = true;
            sC1.Send(mM2);
            // Deal with an exception if one is thrown by the code in the try block
            //
        }

    }


    private  string GetExchangePrice(string Model)
    {

        try
        {
            SqlConnection conn = DBConnection.GetConnection();
            String strQuery = "select Exchange_Offer_Price from tbl_Great_Exchange_Offer where Model='" + Model + "'and Status=1";
            SqlCommand cmd = new SqlCommand(strQuery, conn);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            string netAmnt = dt.Rows[0]["Exchange_Offer_Price"].ToString().Trim();
            return netAmnt;
        }
        catch (Exception ex)
        {
            return "";
        }

    }

    private string getSpecialOffer(string model)
    {
     try
        {
            SqlConnection conn = DBConnection.GetConnection();
            String strQuery = "select  SpecialPriceOffer from  SpecifiqShowroomOffer where Model='" + model + "'";
            SqlCommand cmd = new SqlCommand(strQuery, conn);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            string netAmnt = dt.Rows[0]["SpecialPriceOffer"].ToString().Trim();
            return netAmnt;
        }
        catch (Exception ex)
        {
            return "";
        }
    }

    private string GpStar(string Model)
    {

        try
        {
            SqlConnection conn = DBConnection.GetConnection();
            String strQuery = "select GP_Star_Price from tbl_Great_Exchange_Offer where Model='" + Model + "'";
            SqlCommand cmd = new SqlCommand(strQuery, conn);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            string netAmnt = dt.Rows[0]["GP_Star_Price"].ToString().Trim();
            return netAmnt;
        }
        catch (Exception ex)
        {
            return "";
        }

    }



     


            //if (ddlRefDiscount.SelectedValue == "LG Tv 6%")
            //{
            //    if (lg_lcd_model.Contains(ddlContinents.SelectedItem.Text))
            //    {


    protected void ddlRefDiscount_SelectedIndexChanged(object sender, EventArgs e) 
    {
        try
        {
            if (ddlRefDiscount.SelectedItem.Text == "Online Order" || ddlRefDiscount.SelectedItem.Text == "Great Exchange Offer")
            {
                chkTblOffer.Visible = true;
            }
            else
               chkTblOffer.Visible = false;
            

            NetAmntCalculation();
            
        }
        catch(Exception ex) { }
        
    
    }


   


    private bool NetAmntCalculation() 
    {
        if (ddlRefDiscount.SelectedValue == "Great Exchange Offer" && int.Parse(txtQty.Text.Trim().ToString()) > 0 && txtModel.Text.Trim() != "" && txtTotalAmnt.Text.ToString() != "")
        {

            string price = GetExchangePrice(txtModel.Text.ToString().Trim());
            if (price != "")
            {
                txtNet.Text = (int.Parse(price) * int.Parse(txtQty.Text.ToString())).ToString();
                txtDisAmnt.Text = (int.Parse(txtTotalAmnt.Text.ToString()) - int.Parse(txtNet.Text.ToString())).ToString();
                txtDisAmnt.Enabled = false;
            }
            else
            {
                PopupMessage("This product is not permitted for the Great Exchange Offer", btnAdd);
                return false;
            }


        }
        else if (ddlRefDiscount.SelectedValue == "GP Star Offer" && int.Parse(txtQty.Text.Trim().ToString()) > 0 && txtModel.Text.Trim() != "" && txtTotalAmnt.Text.ToString() != "")
        {
            string price = GpStar(txtModel.Text.ToString().Trim());
            if (price != "")
            {
                txtNet.Text = (int.Parse(price) * int.Parse(txtQty.Text.ToString())).ToString();
                txtDisAmnt.Text = (int.Parse(txtTotalAmnt.Text.ToString()) - int.Parse(txtNet.Text.ToString())).ToString();
                txtDisAmnt.Enabled = false;
            }
            else
            {
                PopupMessage("This product is not permitted for the GP Star Offer", btnAdd);
                return false;
            }

        }
        else if (ddlRefDiscount.SelectedValue == "GP Prime Postpaid" && int.Parse(txtQty.Text.Trim().ToString()) > 0 && txtModel.Text.Trim() != "" && txtTotalAmnt.Text.ToString() != "")
        {
            int totalPrice = int.Parse(txtTotalAmnt.Text.ToString());
            if (totalPrice >= 10000)
            {
                txtDisAmnt.Text = "1500";

            }
            else
                txtDisAmnt.Text = "0";
            txtDisAmnt_TextChanged();
        }


        else if (ddlRefDiscount.SelectedValue == "Facebook Special Campeign" && int.Parse(txtQty.Text.Trim().ToString()) > 0 && txtModel.Text.Trim() != "" && txtTotalAmnt.Text.ToString() != "") 
        {
            if (Session["UserName"].ToString() == "admin@bcdl" || Session["UserName"].ToString() == "admin@banasree") 
            {

                string price = getSpecialOffer(txtModel.Text.ToString().Trim());

                if (price != "") 
                 {
                     txtNet.Text = (int.Parse(price) * int.Parse(txtQty.Text.ToString())).ToString();
                     txtDisAmnt.Enabled = false;
                 }
            
            }
        }

        else if (ddlRefDiscount.SelectedValue == "Fantastic Four Showroom Special") 
        {

            string[] offerModel = new string[] { "KWM-KT8222GDS", "KWM-KT9222GDS", "KMW-KT1021B" };

            if (Session["UserName"].ToString() == "admin@sonartori" || Session["UserName"].ToString() == "admin@lalmatia"
                || Session["UserName"].ToString() == "admin@gulshan1" || Session["UserName"].ToString() == "admin@gulshan2")
            {
               if (offerModel.Contains(ddlContinents.SelectedItem.Text))
               {
                   if (ddlContinents.SelectedItem.Text == "KMW-KT1021B") 
                   {
                       txtNet.Text = "46900";
                       txtDisAmnt.Enabled = false;
                   }

                   if (ddlContinents.SelectedItem.Text == "KWM-KT8222GDS")
                   {
                       txtNet.Text = "22500";
                       txtDisAmnt.Enabled = false;
                   }
                   if (ddlContinents.SelectedItem.Text == "KWM-KT9222GDS")
                   {
                       txtNet.Text = "23900";
                       txtDisAmnt.Enabled = false;
                   }
            
               }
            }

        }

        else if (ddlRefDiscount.SelectedValue == "EBL 5% Discount DEC-2023")
        {
           
            if (int.Parse(txtNet.Text) > 10000)
            {
                  string disCountAmnt = (Math.Ceiling(Convert.ToInt32(this.txtTotalAmnt.Text) * 0.05)).ToString();
                    if (int.Parse(disCountAmnt) > 6000)
                    {
                        this.txtDisAmnt.Text = "6000";
                        txtDisAmnt.Enabled = false;
                    }
                    else
                    {
                        this.txtDisAmnt.Text = disCountAmnt;
                        txtDisAmnt.Enabled = false;
                    }
                  this.txtNet.Text = (Convert.ToInt32(this.txtTotalAmnt.Text) - Convert.ToInt32(this.txtDisAmnt.Text)).ToString();
               

               


            }

        }

        else
        {
            txtDisAmnt.Enabled = true;
            txtDisAmnt.Text = "0";
        }
            
        return true;
    }



    private string GetExchangePriceModel(string Model)
    {

        try
        {
            SqlConnection conn = DBConnection.GetConnection();
            String strQuery = "select model from tbl_Great_Exchange_Offer where Model='" + Model + "'and Status=1";
            SqlCommand cmd = new SqlCommand(strQuery, conn);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            string exhchangeModel = dt.Rows[0]["model"].ToString().Trim();
            return exhchangeModel;
        }
        catch (Exception ex)
        {
            return "";
        }

    }

    protected void btnCheckMail_Click(object sender, EventArgs e)
    { 
         fnSendMail_Invoice_test();
       
       
    }
    protected void chkTblOffer_CheckedChanged(object sender, EventArgs e)
    {
        if (chkTblOffer.Checked)
        {

            if (ddlRefDiscount.SelectedItem.Text=="Great Exchange Offer" && GetExchangePriceModel(txtModel.Text.ToString().Trim()) != "")
            {
                string ExchangePrice = GetExchangePrice(txtModel.Text.ToString().Trim());
                string disAmntExchange = (Math.Ceiling(Convert.ToInt32(ExchangePrice) * 0.05)).ToString();
                if (int.Parse(disAmntExchange) > 6000)
                {
                    this.txtDisAmnt.Text = "6000";
                    this.txtNet.Text = (Convert.ToInt32(ExchangePrice) - Convert.ToInt32(this.txtDisAmnt.Text)).ToString();
                    txtDisAmnt.Enabled = false;
                }
                else
                {
                    this.txtDisAmnt.Text = disAmntExchange;
                    this.txtNet.Text = (Convert.ToInt32(ExchangePrice) - Convert.ToInt32(this.txtDisAmnt.Text)).ToString();
                    txtDisAmnt.Enabled = false;
                }


            }


            else 
            {
                string disAmnt = (Math.Ceiling(Convert.ToInt32(this.txtTotalAmnt.Text) * 0.05)).ToString();
                this.txtNet.Text = (Convert.ToInt32(txtTotalAmnt.Text) - Convert.ToInt32(this.txtDisAmnt.Text)).ToString();
                txtDisAmnt.Enabled = false;
            }
           

        }
    }
}
