<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FOB_Entry.aspx.cs" Inherits="PIC_FOB_Entry" 
    MasterPageFile="Admin.master" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <script language="JavaScript" src="../js/datetimepicker.js" type="text/javascript"> </script>
    
    <script type="text/javascript">
        $(function () {
            $("#txtDate").datepicker();
            $("#txtFrom").datepicker();
            $("#txtToDate").datepicker();            
        });   
             
    </script>      

</asp:Content>


<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    
    <h2 class="col-sm-12 bg-primary" style="padding:5px; color: #FFFFFF; background-color: #006666;"> 
        FOB Price Entry ...
    </h2>
    <p></p>



</asp:Content>
