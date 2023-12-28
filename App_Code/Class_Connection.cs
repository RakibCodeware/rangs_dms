using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Globalization;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

/// <summary>
/// Summary description for Class1
/// </summary>
/// 
public static class DBConnection
{
    //Live Connction

    public static SqlConnection GetConnection()
    {
        //CONNECTION FOR ORIGINAL DB
        string strCon = "Data Source=10.12.3.16;Initial Catalog=dbCID;Persist Security Info=True;User ID=sa;Password=Adminn321;Connect Timeout=0";
        return new SqlConnection(strCon);
    }

    //LocalConnection
    //public static SqlConnection GetConnection()
    //{
    //    //CONNECTION FOR ORIGINAL DB
    //    string strCon = "Data Source=DESKTOP-7FRORI9;Initial Catalog=dbCID;Persist Security Info=True;User ID=sa;Password=123;Connect Timeout=0";
    //    return new SqlConnection(strCon);
    //}
}

public static class DBConnection_ROS
{
    //Live Connction

    public static SqlConnection GetConnection()
    {
        string strCon = "Data Source=202.84.32.123;Initial Catalog=dbSonyRangsBD;Persist Security Info=False;User ID=sa;Password=Mohi@20220606;Connect Timeout=0";
        return new SqlConnection(strCon);


    }

    //LocalConnection
   // mychange
    //public static SqlConnection GetConnection()
    //{
    //    string strCon = "Data Source=DESKTOP-7FRORI9;Initial Catalog=dbSonyRangsBD;Persist Security Info=False;User ID=sa;Password=123;Connect Timeout=0";
    //    return new SqlConnection(strCon);


    //}

}


// FOR DEALER SALES MANAGEMENT
public static class DBConnectionDSM
{
    //Live Connction

    public static SqlConnection GetConnection()
    {
        string strCon = "Data Source=10.12.3.16;Initial Catalog=DelearSales;Persist Security Info=True;User Id=sa;Password=Adminn321;Connect Timeout=0";
        return new SqlConnection(strCon);
    }

    //LocalConnection
    //public static SqlConnection GetConnection()
    //{
    //    string strCon = "Data Source=DESKTOP-7FRORI9;Initial Catalog=DelearSales;Persist Security Info=True;User Id=sa;Password=123;Connect Timeout=0";
    //    return new SqlConnection(strCon);
    //}
}


public static class DBConnection_Log
{
    //Live Connction

    public static SqlConnection GetConnection()
    {
        //CONNECTION FOR ORIGINAL DB
        string strCon = "Data Source=10.12.3.16;Initial Catalog=dbDMSLog;Persist Security Info=True;User ID=sa;Password=Adminn321;Connect Timeout=0";
        return new SqlConnection(strCon);
    }

    //LocalConnection
    //public static SqlConnection GetConnection()
    //{
    //    //CONNECTION FOR ORIGINAL DB
    //    string strCon = "Data Source=DESKTOP-0JKV9HH;Initial Catalog=dbDMSLog;Persist Security Info=True;User ID=sa;Password=Adminn321;Connect Timeout=0";
    //    return new SqlConnection(strCon);
    //}
}


//CONENCTION FOR SPIN TO WIN
public static class DBConnectionSpin
{
    //Live Connction

    public static SqlConnection GetConnection()
    {
        string strCon = "Data Source=202.84.32.123;Initial Catalog=dbSpinToWin;Persist Security Info=True;User Id=sa;Password=Mohi@20220606;Connect Timeout=0";
        return new SqlConnection(strCon);
    }

    //LocalConnection
    //public static SqlConnection GetConnection()
    //{
    //    string strCon = "Data Source=DESKTOP-7FRORI9;Initial Catalog=dbSpinToWin;Persist Security Info=True;User Id=sa;Password=Mohi@20220606;Connect Timeout=0";
    //    return new SqlConnection(strCon);
    //}
}



// FOR SMS
public static class DBConnectionSMS
{
    //Live Connction


    public static SqlConnection GetConnection()
    {
        string strCon = "Data Source=202.84.32.123;Initial Catalog=dbSMS;Persist Security Info=True;User Id=sa;Password=Mohi@20220606;Connect Timeout=0";
        return new SqlConnection(strCon);
    }

    //LocalConnection
    //public static SqlConnection GetConnection()
    //{
    //    string strCon = "Data Source=DESKTOP-7FRORI9;Initial Catalog=dbSMS;Persist Security Info=True;User Id=sa;Password=Mohi@20220606;Connect Timeout=0";
    //    return new SqlConnection(strCon);
    //}
}


// CONNECTION TO HRM
public static class DBConnectionHRM
{
    //Live Connction

    public static SqlConnection GetConnection()
    {
        string strCon = "Data Source=(local);Initial Catalog=HRMS;Integrated Security=SSPI;Integrated Security=True;Trusted_Connection=Yes;User Id=sa;Password=Adminn321;Connect Timeout=0";
        return new SqlConnection(strCon);
    }

    //LocalConnection
    //public static SqlConnection GetConnection()
    //{
    //    string strCon = "Data Source=(local);Initial Catalog=HRMS;Integrated Security=SSPI;Integrated Security=True;Trusted_Connection=Yes;User Id=sa;Password=Adminn321;Connect Timeout=0";
    //    return new SqlConnection(strCon);
    //}
}

public static class ReportDBConnection
{

    public static ConnectionInfo GetConnection()
    {
        ConnectionInfo conn = new ConnectionInfo();
       // conn.ServerName = "10.12.3.16"; //live
        conn.ServerName = "DESKTOP-7FRORI9";
        conn.Password = "Adminn321";
        conn.UserID = "sa";
        conn.DatabaseName = "dbCID";

        return conn;

    }
}

public static class ReportDBConnectionSpin
{

    public static ConnectionInfo GetConnection()
    {
        ConnectionInfo conn = new ConnectionInfo();
       // conn.ServerName = "202.84.32.123"; //live
        conn.ServerName = "DESKTOP-7FRORI9";
        conn.Password = "Mohi@20220606";
        conn.UserID = "sa";
        conn.DatabaseName = "dbSpinToWin";

        return conn;

    }
}

public static class ReportDBConnectionDSM
{

    public static ConnectionInfo GetConnection()
    {
        ConnectionInfo conn = new ConnectionInfo();
        //conn.ServerName = "10.12.3.16"; //live
        conn.ServerName = "DESKTOP-0JKV9HH";
        conn.Password = "Adminn321";
        conn.UserID = "sa";
        conn.DatabaseName = "DelearSales";

        return conn;
    }
}


public static class culture
{
    public static CultureInfo cn()
    {
        CultureInfo cninfo = new CultureInfo("en-Us");
        return cninfo;
    }
}


public class vDeclare
{

    public static string sBr = "";
    public static int sBrId = 0;
    public static string sUser = "";
    public static int eID = 0;
    public static string sBillNo = "";
    public static string sBrCode = "";

    public static string sPCName = System.Environment.MachineName;

    string netBname = System.Environment.MachineName;
    string dnsName = System.Net.Dns.GetHostName();
    //sPCName == netBname;
    public static DateTime sDate;
    public static DateTime eDate;
    //public string sDate="";
    //public string eDate="";
    public static string sWebAccess = "";
    public static string sWebAccessType = "";

}

