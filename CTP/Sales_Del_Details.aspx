<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="Admin.master"
CodeFile="Sales_Del_Details.aspx.cs" Inherits="Sales_Del_Details" %>

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
    <link rel="stylesheet" href="../css/StylePopUp.css"/>
    <script src="../scripts/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script src="../scripts/jquery.blockUI.js" type="text/javascript"></script>

                              
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    
    <h4 class="col-sm-12 bg-primary" style="padding:5px"> Waiting for Customer Delivery ...</h4>

    <div style="width:100%; text-align:center ">        
        <asp:Label ID="lblmsg" runat="server" ForeColor="Red"></asp:Label>
    </div>

    <p>&nbsp;</p>

     <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

    <h1>
               
        <asp:Label ID="Label2" runat="server" Text="Invoice From: " ForeColor="#006600" 
            Font-Size="20pt" Font-Names="Tahoma"></asp:Label> 
        &nbsp;
        <asp:Label ID="lblFrom" runat="server" Text="0" ForeColor="#003366" 
            Font-Size="20pt" Font-Names="Tahoma"></asp:Label>

        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;        
        <asp:Label ID="Label4" runat="server" Text="Invoice # " ForeColor="Red" 
            Font-Size="20pt" Font-Names="Tahoma"></asp:Label> 
        &nbsp;
        <asp:Label ID="lblInvNo" runat="server" Text="0" ForeColor="#CC0000" 
            Font-Size="20pt" Font-Names="Tahoma"></asp:Label>
    </h1>

    <h1>
        
        <asp:Label ID="Label1" runat="server" Text="Sales Date: " ForeColor="Blue" 
            Font-Size="20" Font-Names="Tahoma"></asp:Label> 
        &nbsp;
        <asp:Label ID="lblInvDate" runat="server" Text="0" ForeColor="Blue" Font-Size="20" Font-Names="Tahoma"></asp:Label>

    </h1>
   
   <a href="Sales_Delivery_List.aspx">< Back to List</a>
    
    <br /><br />
    <div>        
        <asp:GridView runat="server" ID="gvShoppingCart" AutoGenerateColumns="false" 
            EmptyDataText="There is nothing in your shopping cart." GridLines="None" Width="100%" 
            CellPadding="2" CssClass="Grid1"             
            font-family="Arial"
            font-size= "10pt"
            ShowFooter="true" DataKeyNames="MRSRDID" 
            OnRowDataBound="gvShoppingCart_RowDataBound"
            OnRowCommand="gvShoppingCart_RowCommand"  
            >            
            <HeaderStyle HorizontalAlign="Left" BackColor="#3D7169" ForeColor="#FFFFFF" />
            <FooterStyle HorizontalAlign="Right" BackColor="#6C6B66" ForeColor="#FFFFFF" />
            <AlternatingRowStyle BackColor="#F8F8F8" />
            <Columns>
                <asp:TemplateField HeaderText="SL#" ItemStyle-Width="5%">
                    <ItemTemplate>
                        <%# Container.DataItemIndex + 1 %>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Item Code" ItemStyle-Width="10%">  
                    <ItemTemplate>
                        <asp:Label ID="lblCode" runat="server" Text='<%#Eval("Code") %>'></asp:Label>
                    </ItemTemplate>  
                </asp:TemplateField> 

                <asp:TemplateField HeaderText="Item Model" ItemStyle-Width="20%">  
                    <ItemTemplate>
                        <asp:Label ID="lblModel" runat="server" Text='<%#Eval("Model") %>'></asp:Label>
                    </ItemTemplate>  
                </asp:TemplateField> 

                <asp:TemplateField HeaderText="Item Name" ItemStyle-Width="20%">  
                    <ItemTemplate>
                        <asp:Label ID="lblPName" runat="server" Text='<%#Eval("ProdName") %>'></asp:Label>
                    </ItemTemplate>  
                </asp:TemplateField> 
                
                <asp:TemplateField HeaderText="Item Serial" ItemStyle-Width="25%">  
                    <ItemTemplate>
                        <asp:TextBox runat="server" ID="txtSLNO" Columns="10" Text='<%# Eval("SLNO") %>'>                            
                        </asp:TextBox>                        
                    </ItemTemplate>  
                </asp:TemplateField> 
                
                           
                <asp:TemplateField HeaderText="Quantity" ItemStyle-Width="10%">
                    <ItemTemplate>                        
                        <asp:Label ID="lblQty" runat="server" Text='<%#Eval("Qty") %>'></asp:Label>                                                
                    </ItemTemplate>

                    <FooterTemplate>
                        <asp:Label ID="lblTQty" runat="server" Text="Label"></asp:Label>
                    </FooterTemplate>

                </asp:TemplateField>

                <asp:TemplateField HeaderText="Amount" HeaderStyle-HorizontalAlign="Right" ItemStyle-Width="15%">
                    <HeaderStyle HorizontalAlign="Right" />
                    <ItemTemplate>                        
                        <asp:Label ID="lblAmnt" runat="server" Text='<%#Eval("tAmnt") %>'></asp:Label>                                                
                    </ItemTemplate>
                    
                    <FooterTemplate>
                        <asp:Label ID="lblTAmnt" runat="server" Text="Label"></asp:Label>
                    </FooterTemplate>

                </asp:TemplateField>

                <asp:TemplateField HeaderText="MRSRDID" InsertVisible="False">  
                    <ItemTemplate>
                        <asp:Label ID="lblDID" runat="server" Text='<%#Eval("MRSRDID") %>'></asp:Label>
                    </ItemTemplate>  
                </asp:TemplateField> 
                                                
            </Columns>
        </asp:GridView>
     
    </div>

    <div class="container"> 
        <h1>
            <a href="#" data-toggle="tooltip" title="Total Quantity of this challan ...">Total Qty :                
                <asp:Label ID="lblTItem" runat="server" Text="(0)"></asp:Label>  
            </a>
            <asp:Label ID="lblTTQty" runat="server" Text="" Visible ="false"></asp:Label>
            <br />
                       
        </h1>
    </div>

         

    <!-- Reference Number -->
    <div class="form-group"> 
        <label for="lblInvoiceNo" class="col-sm-offset-0 col-sm-1 control-label" style="font-size:12px;">Invoice #</label>
        <div class="col-sm-6">
            <asp:TextBox ID="txtInvoiceNo" CssClass="form-control" placeholder="Auto reference number" runat="server" ReadOnly ="true"></asp:TextBox>
        </div>
    </div>


    <div class="form-group" >
        <label for="lblInvoiceNo" class="col-sm-offset-0 col-sm-1 control-label" style="font-size:12px;"></label>
        <div class="col-sm-6" style="margin-bottom:7px;" ><br />
            <asp:Button ID="btnSave" CssClass="btn btn-primary" runat="server" 
                data-toggle="tooltip" title="Click here for Update this data & Print Invoice ..."
                Text="   Update & Bill Print   " OnClick="SaveData" /> 
                
             &nbsp; &nbsp;
                                            
             <asp:Button ID="btnBack" CssClass="btn btn-primary" runat="server" 
                data-toggle="tooltip" title="Click here for Back List ..."
                Text="  Back  " OnClick="btnBackCall" /> 
                                    
        </div>                                            
    </div>
    
    

    <div>&nbsp;</div>

</asp:Content>
