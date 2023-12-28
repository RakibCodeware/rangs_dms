<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="Admin.master"
CodeFile="Sales_Delivery_List.aspx.cs" Inherits="Sales_Delivery_List" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
     <script>
         $(document).ready(function () {
             $('[data-toggle="tooltip"]').tooltip();
         });
    </script>
    
    <style type="text/css">
      .hiddencol
      {
        display: none;
      }
    </style>

    <!-- FOR Customer -->
    <link rel="stylesheet" href="css/StylePopUp.css"/>
    <script src="scripts/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script src="scripts/jquery.blockUI.js" type="text/javascript"></script>

                              
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    
    <h2 class="col-sm-12 bg-primary" style="padding:5px"> Sales Delivery List...</h2>

    <p></p>
   
   <!-- GRID VIEW -->
    <div>        
        <asp:GridView ID="GridView1" runat="server" AllowPaging="True" 
            AlternatingRowStyle-CssClass="alt" AutoGenerateColumns="False" 
            BorderColor="#999999" BorderStyle="Double" BorderWidth="1px" CellPadding="2" 
            DataKeyNames="MRSRMID" GridLines="Vertical" 
            OnRowDataBound="GridView1_RowDataBound" 
            OnPageIndexChanging="OnPageIndexChanging"
            OnRowCommand="GridView1_RowCommand"
            PagerStyle-CssClass="pgr"
            font-family="Arial"
            font-size= "10pt" 
            CssClass="mGrid" 
            Width="100%" PageSize="50">
            
            <PagerStyle CssClass="pgr"></PagerStyle>
                
            <SelectedRowStyle BackColor="#0099CC" />
            <AlternatingRowStyle CssClass="alt" BackColor="#C2D69B"></AlternatingRowStyle>
            <Columns>
                <asp:TemplateField HeaderText="SL#" ItemStyle-Width="5%">
                    <ItemTemplate>
                        <%# Container.DataItemIndex + 1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                
                <asp:BoundField DataField="MRSRMID" HeaderText="Master ID" Visible="false"/>
                <asp:BoundField DataField="MRSRCode" HeaderText="Invoice #" />
                <asp:BoundField DataField="TDate" HeaderText="Invoice Date" />                                  
                <asp:BoundField DataField="SalesFrom" HeaderText="Sales From" />
                <asp:BoundField DataField="DelFrom" HeaderText="Delivery From" />
                <asp:BoundField DataField="CustName" HeaderText="Customer Name" />
                <asp:BoundField DataField="Mobile" HeaderText="Mobile #" />
                <asp:BoundField DataField="tQty" HeaderText="Total Qty" 
                    ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Right"/>
                <asp:BoundField DataField="tAmnt" HeaderText="Total Amount" 
                    ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Right"/>
                
                <asp:TemplateField ItemStyle-Width="8px">
                    <ItemTemplate >
                        <asp:Button ID="btnAddCart" runat="server" Text="Details" 
                            CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                            CommandName="ShoppingCart" ItemStyle-HorizontalAlign="Center" />
                    </ItemTemplate>
                </asp:TemplateField>
                                                                               
            </Columns>
        </asp:GridView>
        
    </div>               
    <!-- End GridView -->


    <div class="container"> 
        
    </div>

    <div>&nbsp;</div> 

    <div>&nbsp;</div>

</asp:Content>
