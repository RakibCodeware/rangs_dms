<%@ Page Language="C#" MasterPageFile="Admin.master" 
AutoEventWireup="true" CodeFile="Product_Info.aspx.cs" Inherits="Admin_Forms_Product_Info" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">

      <style type="text/css">
        .grid
        {}
        .style20
        {
            width: 13px;
        }
        .style27
        {
            width: 32px;
        }
        .style28
        {
            width: 32px;
            height: 14px;
        }
        .style30
        {
            width: 13px;
            height: 14px;
        }
        .style32
        {
            width: 93px;
            height: 14px;
        }
        .style36
        {
            width: 175px;
            height: 14px;
        }
        .style37
        {
            width: 175px;
        }
        .style39
        {
            width: 63px;
            height: 14px;
        }
        .style40
        {
            width: 63px;
        }
    </style>
        
    <script language="JavaScript" src="../js/datetimepicker.js" type="text/javascript"> </script>
    
    <script type="text/javascript">
        $(function () {
            $("#txtDate").datepicker();
            $("#txtFrom").datepicker();
            $("#txtToDate").datepicker();
            $("#txtPIDate").datepicker();
        });        
    </script>
                              
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

    <h4 class="col-sm-12 bg-primary" style="padding:5px"> Product Information ...</h4>
    <p></p>
       
    
    <div align="center">
               
   </div>

</asp:Content>
