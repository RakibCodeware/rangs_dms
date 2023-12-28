using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Data
/// </summary>
public class Data
{
    public class MainDataFinal
    {
        //public MainData()
        //{
        //    this.data = new data();
        //}
        public MainData mainData { get; set; }
        public bool status { get; set; }
     
        public string massage { get; set; }
    }

    public class MainData
    {
        //public MainData()
        //{
        //    this.data = new data();
        //}
        public string status { get; set; }
        public data data { get; set; }
        public int order_status { get; set; }
        public string massage { get; set; }
    }

    public class data
    {
        //public int id { get; set; }
        //public int shop_id { get; set; }
        //public int customer_id { get; set; }
        //public string tracking_number { get; set; }
        //public string customer_contact { get; set; }
        //public status status { get; set; }
        //public int amount { get; set; }
        //public int discount { get; set; }
        //public int total { get; set; }
        //public int paid_total { get; set; }
        //public int delivery_fee { get; set; }
        //public int? payment_id { get; set; }
        //public string payment_status { get; set; }
        //public string payment_gateway { get; set; }
        public List<products> products { get; set; }


        //public string billing_address { get; set; }
        //public string delivery_time { get; set; }
        //public string delivery_type { get; set; }
        //public string order_date { get; set; }
        //public string order_by { get; set; }
        //public string parent_id { get; set; }
        //public int sales_tax { get; set; }
        //public string coupon_id { get; set; }
        //public string logistics_provider { get; set; }
        //public string deleted_at { get; set; }
        //public string created_at { get; set; }
        //public string updated_at { get; set; }
        //public customer customer { get; set; }
        public string customer_name { get; set; }
        public string customer_mobile { get; set; }
        public string customer_email { get; set; }

        public shipping_address shipping_address { get; set; }
        public shipping_address billing_address { get; set; }
        public string shop_name { get; set; }
        public string shop_email { get; set; }
        public int delivery_fee { get; set; }
        public int total_amount { get; set; }
        public int total_qty { get; set; }
        //public List<products> products { get; set; }


    }
    //public class status
    //{
    //    public int id { get; set; }
    //    public string name { get; set; }
    //    public int serial { get; set; }
    //    public string color { get; set; }
    //    public string created_at { get; set; }
    //    public string updated_at { get; set; }
    //}

    public class shipping_address
    {
        public string name { get; set; }
        public string email { get; set; }
        public string mobile { get; set; }
        public string district { get; set; }
        public string thana { get; set; }
        public string district_name { get; set; }
        public string thana_name { get; set; }
        public string address_one { get; set; }
        public string address_two { get; set; }
    }
    public class billing_address
    {
        public string name { get; set; }
        public string email { get; set; }
        public string mobile { get; set; }
        public string district { get; set; }
        public string thana { get; set; }
        public string district_name { get; set; }
        public string thana_name { get; set; }
        public string address_one { get; set; }
        public string address_two { get; set; }
    }
    //public class customer
    //{
    //    public int id { get; set; }
    //    public string shop_id { get; set; }
    //    public string name { get; set; }
    //    public string mobile { get; set; }
    //    public string email { get; set; }
    //    public int user_type { get; set; }
    //    public string email_verified_at { get; set; }
    //    public int is_active { get; set; }
    //    public string created_at { get; set; }
    //    public string updated_at { get; set; }
    //}

    ////public class products
    ////{
    ////    public int id { get; set; }
    ////    public string shop_id { get; set; }
    ////    public int type_id { get; set; }
    ////    public int category_id { get; set; }
    ////    public string name { get; set; }
    ////    public string slug { get; set; }
    ////    public string description { get; set; }
    ////    public int price { get; set; }
    ////    public int mrp_price { get; set; }
    ////    public int brand_id { get; set; }
    ////    public int sale_price { get; set; }
    ////    public string sku { get; set; }
    ////    public int quantity { get; set; }
    ////    public int in_stock { get; set; }
    ////    public int is_taxable { get; set; }
    ////    public string shipping_class_id { get; set; }
    ////    public string status { get; set; }
    ////    public string product_type { get; set; }
    ////    public string unit { get; set; }
    ////    public string height { get; set; }
    ////    public string width { get; set; }
    ////    public string length{ get; set; }
    ////            public image image { get; set; }
    ////    public List<gallery> gallery { get; set; }

    ////    public string max_price { get; set; }
    ////    public string min_price { get; set; }
    ////    public string max_order_qty { get; set; }
    ////    public string min_sale_price { get; set; }
    ////    public string max_sale_price { get; set; }
    ////    public string max_order { get; set; }
    ////    public string discount { get; set; }
    ////    public string discount_type { get; set; }
    ////    public string video { get; set; }
    ////    public string customize_url { get; set; }
    ////    public string product_model { get; set; }
    ////    public string product_code { get; set; }
    ////    public string specification { get; set; }
    ////    public string warranty_type { get; set; }
    ////    public string package_kg { get; set; }
    ////    public string made_by { get; set; }
    ////    public string delivery_by { get; set; }
    ////    public string search_text { get; set; }
    ////    public string origin { get; set; }
    ////    public string online_delivery { get; set; }
    ////    public string online_payment { get; set; }
    ////    public int home_page_show { get; set; }
    ////    public int most_trending { get; set; }
    ////    public int variation_slug { get; set; }
    ////    public string meta_title { get; set; }
    ////    public string meta_keyword { get; set; }
    ////    public string meta_description { get; set; }
    ////    public int delivery_cost { get; set; }
    ////    public int rating { get; set; }
    ////    public string created_at { get; set; }
    ////    public string updated_at { get; set; }
    ////    public string deleted_at { get; set; }
    ////    public pivot pivot { get; set; }
    ////            //"variation_options": []
    ////            }

    //            //public class image
    //            //{
    //            //    public string thumbnail { get; set; }
    //            //    public string original { get; set; }
    //            //    public int id { get; set; }
    //            //}
    //            //public class gallery
    //            //{
    //            //    public string thumbnail { get; set; }
    //            //    public string original { get; set; }
    //            //    public int id { get; set; }
    //            //}
    //            //public class pivot
    //            //{
    //            //    public int order_id { get; set; }
    //            //    public int product_id { get; set; }
    //            //    public string order_quantity { get; set; }
    //            //    public int unit_price { get; set; }
    //            //    public int subtotal { get; set; }
    //            //    public int variation_option_id { get; set; }
    //            //    public string created_at { get; set; }
    //            //    public string updated_at { get; set; }
    //            //}
    //public class shop
    //{
    //    public int id { get; set; }
    //    public int owner_id { get; set; }
    //    public string zone_id { get; set; }
    //    public string name { get; set; }
    //    public string email { get; set; }
    //    public string contact_person { get; set; }
    //    public string slug { get; set; }
    //    public string description { get; set; }
    //    public string cover_image { get; set; }
    //    public string logo { get; set; }
    //    public int district_id { get; set; }
    //    public int upazila_id { get; set; }
    //    public int is_active { get; set; }    

    //    public address address { get; set; }
    //    public settings settings { get; set; }
    //    public string created_at { get; set; } 
    //    public string updated_at { get; set; }
    //    public string zone { get; set; }
    //}
    //public class address {
    //    public string zip { get; set; }
    //    public string street_address { get; set; }
    //}
    //public class settings {
    //    public string contact { get; set; }
    //    public string location { get; set; }
    //    public string socials { get; set; }
    //}

    public class products
    {
        //public int id { get; set; }
        //public int order_id { get; set; }
        //public int product_id { get; set; }
        //public int variation_option_id { get; set; }
        //public int offer_id { get; set; }
        public string product_model { get; set; }
        public int unit_price { get; set; }
        public int order_quantity { get; set; }
        ////public int mrp_price { get; set; }
        public string offer_type { get; set; }
        public int discount { get; set; }
        public int subtotal { get; set; }
        public int delivery_cost { get; set; }


        //public int offer_percent { get; set; }
        //public int parent_product_id { get; set; }
        //public string created_at { get; set; }
        //public string updated_at { get; set; }
        //public List<products> Products { get; set; }

    }
}