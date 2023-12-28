﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ToDaySales.aspx.cs" 
Inherits="CTP_ToDaySales" MasterPageFile="Admin.master"%>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    
    <style type="text/css">
      .hiddencol
      {
        display: none;
      }
    </style>
        
    <style type="text/css">
           
        .mGrid { 
            width: 100%; 
            background-color: #fff; 
            margin: 5px 0 10px 0; 
            border: solid 1px #525252; 
            border-collapse:collapse; 
        }
        .mGrid td { 
            padding: 2px; 
            border: solid 1px #c1c1c1; 
            color: #717171; 
        }
        .mGrid th { 
            padding: 4px 2px; 
            color: #fff; 
            background: #424242 url(grd_head.png) repeat-x top; 
            border-left: solid 1px #525252; 
            font-size: 0.9em; 
        }
        .mGrid .alt { background: #fcfcfc url(grd_alt.png) repeat-x top; }
        .mGrid .pgr { background: #424242 url(grd_pgr.png) repeat-x top; }
        .mGrid .pgr table { margin: 5px 0; }
        .mGrid .pgr td { 
            border-width: 0; 
            padding: 0 6px; 
            border-left: solid 1px #666; 
            font-weight: bold; 
            color: #fff; 
            line-height: 12px; 
         }   
        .mGrid .pgr a { color: #666; text-decoration: none; }
        .mGrid .pgr a:hover { color: #000; text-decoration: none; }


       .highlight
        {
            background-color: #ffeb95;
            cursor: pointer;
        }
        .normal
        {
            background-color: white;
            cursor: pointer;
        }
                        
        .style1
        {
            height: 21px;
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
    
    <h2 class="col-sm-12 bg-danger" 
        style="padding:5px; background-color: #008080; color: #FFFFFF;"> ToDay Sales ...</h2>
    
    <div style="width:100%; text-align:left">  
        <asp:HyperLink ID="HyperLink1" runat="server" 
            NavigateUrl="Default_Administrator.aspx">Back to Dashboard</asp:HyperLink>   
    </div>

    <div style="width:100%; text-align:center ">    
        <asp:Label ID="lblmsg" runat="server" Text=""></asp:Label>
    </div>
    
    <h4 class="col-sm-12 bg-primary" style="padding:0.5px"></h4>
    
    <div>        
        <asp:GridView ID="GridView1" runat="server" AllowPaging="True" 
            AlternatingRowStyle-CssClass="alt" AutoGenerateColumns="False" CssClass="table" 
            BorderColor="#999999" BorderStyle="Double" BorderWidth="1px" CellPadding="2" 
            DataKeyNames="MRSRCode" GridLines="Vertical" 
            OnRowDataBound="GridView1_RowDataBound" 
            OnPageIndexChanging="OnPageIndexChanging"
            PagerStyle-CssClass="pgr" 
            ShowFooter="true" 
            Width="100%" PageSize="50">
            <FooterStyle BackColor="#006666" ForeColor="White" />
            <HeaderStyle BackColor="#006600" CssClass="bg-primary"/>

            <PagerStyle CssClass="pgr"></PagerStyle>
                
            <SelectedRowStyle BackColor="#0099CC" />
            <AlternatingRowStyle CssClass="alt" BackColor="#C2D69B"></AlternatingRowStyle>
            <Columns>
                <asp:TemplateField HeaderText="SL#">
                    <ItemTemplate>
                        <%# Container.DataItemIndex + 1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                                
                <asp:BoundField DataField="MRSRCode" HeaderText="Invoice #" />
                <asp:BoundField DataField="TDate" HeaderText="Sales Date" /> 
                <asp:BoundField DataField="NetSalesAmnt" HeaderText="Total Amnt" />                
                <asp:BoundField DataField="CashAmnt" HeaderText="Cash Pay" />
                <asp:BoundField DataField="CardAmnt" HeaderText="Card Pay" />
               <%-- <asp:BoundField DataField="CustName" HeaderText="Customer Name" />
                <asp:BoundField DataField="Mobile" HeaderText="Contact #" />--%>
                                 
                <asp:TemplateField HeaderText="Invoice">
                    <ItemTemplate>
		                <asp:LinkButton ID="lnkPrint" Text="Print"  OnClick="lnkPrint_Click" runat="server"
                            OnClientClick="return confirm('Do you want to Print this Invoice ?');" 
                            >
                        </asp:LinkButton>
	                </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        
    </div>
        
    <div>&nbsp;</div>    
    <div></div>

</asp:Content>