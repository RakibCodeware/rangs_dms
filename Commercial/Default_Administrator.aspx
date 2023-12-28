<%@ Page Language="C#" MasterPageFile="Admin.master" AutoEventWireup="true" 
CodeFile="Default_Administrator.aspx.cs" Inherits="Default_Administrator" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <script language="JavaScript" src="../js/datetimepicker.js" type="text/javascript"> </script>
    
    <script type="text/javascript">
        $(function () {
            $("#txtDate").datepicker();
            $("#txtFrom").datepicker();
            $("#txtToDate").datepicker();
            $("#txtFrom1").datepicker();
            $("#txtToDate1").datepicker();
            $("#txtFrom2").datepicker();
            $("#txtToDate2").datepicker();
            $("#txtPIDate").datepicker();
        });   
             
    </script>

    

</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

    <h2 class="col-sm-12 bg-primary" style="padding:5px; font-family: Tahoma;" align="center"> 
        <asp:Label ID="lblText" runat="server" Text="Label"></asp:Label>
        </h2>
        
    <p>
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>                
    </p>
       

    <div>&nbsp;</div>

    

   
   

    <div class="row">
        

    </div>
    
        
    <script type="text/javascript">
        $(".form_datetime").datetimepicker({ format: 'yyyy-mm-dd hh:ii' });
    </script> 

</asp:Content>

