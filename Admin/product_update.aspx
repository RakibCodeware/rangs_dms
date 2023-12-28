<%@ Page Language="C#" AutoEventWireup="true" CodeFile="product_update.aspx.cs" Inherits="admin_product_update" 
    MasterPageFile="Admin.master" ValidateRequest="false"%>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
      <style type="text/css">
        body
        {
            font-family: Arial;
            font-size: 10pt;
        }
        table
        {
            border: 1px solid #ccc;
            border-collapse: collapse;
            background-color: #fff;
        }
        table th
        {
            background-color: #B8DBFD;
            color: #333;
            font-weight: bold;
        }
        table th, table td
        {
            padding: 5px;
            border: 1px solid #ccc;
        }
        table, table table td
        {
            border: 0px solid #ccc;
        }
    </style>
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent" >
  
    
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>


    <div>

    <h1>Update Product Price As per Campaign</h1>

    <table width ="100%">
        <tr>
            <td>Running Campaign:</td>
            <td align="left">
                <asp:DropDownList ID="ddlCampaign" runat="server" Height="30px"  Width="337px">
                </asp:DropDownList>
            </td>
            <td align ="right"> 
                <asp:Label ID="lblCampID" runat="server" Text="0" Visible="False"></asp:Label>
                <asp:Label ID="lblCampCode" runat="server" Text="0" Visible="False"></asp:Label>
            </td>
        </tr>

        <tr>
            <td>Category:</td>
            <td align="left">
                <asp:DropDownList ID="DropDownList1" runat="server" Height="30px"  Width="337px">
                </asp:DropDownList>
            </td>
            <td align ="right"> 
                <asp:Button ID="btnSearch" runat="server" Text="Search" Height="30px" 
                    CssClass="btn btn-primary" onclick="btnSearch_Click" />
            </td>
        </tr>
    </table>

    </div>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

        <div style="padding: 10px; width: 100%">

            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" OnRowDataBound="OnRowDataBound"
                    DataKeyNames="ProductID" OnRowEditing="OnRowEditing" OnRowCancelingEdit="OnRowCancelingEdit"
                    PageSize="150" AllowPaging="true" OnPageIndexChanging="OnPaging" OnRowUpdating="OnRowUpdating"
                    EmptyDataText="No records has been added." Width="100%">
                    <Columns>
                        
                        <asp:TemplateField HeaderText="SL#" ItemStyle-Width="5%">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField> 

                        
                        <asp:TemplateField HeaderText="Model">
                            <ItemTemplate>
                                <asp:Label ID="lblTitle" runat="server" Text='<%# Eval("Model") %>'></asp:Label>
                            </ItemTemplate>                            
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Unit Price" ItemStyle-Width="100">
                            <ItemTemplate>
                                <asp:Label ID="lblMRP" runat="server" Text='<%# Eval("UnitPrice") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtMRP" runat="server" Text='<%# Eval("UnitPrice") %>' Width="90"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Campaign Price" ItemStyle-Width="100">
                            <ItemTemplate>
                                <asp:Label ID="lblPrice" runat="server" Text='<%# Eval("tCampPrice") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtPrice" runat="server" Text='<%# Eval("tCampPrice") %>' Width="90"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="Dealer Price" ItemStyle-Width="100">
                            <ItemTemplate>
                                <asp:Label ID="lblDP" runat="server" Text='<%# Eval("DealerPrice") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtDP" runat="server" Text='<%# Eval("DealerPrice") %>' Width="90"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Special Price(Del)" ItemStyle-Width="100">
                            <ItemTemplate>
                                <asp:Label ID="lblDSP" runat="server" Text='<%# Eval("DealerSpecialPrice") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtDSP" runat="server" Text='<%# Eval("DealerSpecialPrice") %>' Width="90"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="EXP Price(Del)" ItemStyle-Width="100">
                            <ItemTemplate>
                                <asp:Label ID="lblDealerOfferExp" runat="server" Text='<%# Eval("DealerSpecialPriceExp") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtDealerOfferExp" runat="server" Text='<%# Eval("DealerSpecialPriceExp") %>' Width="90"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                                                                                                
                        <asp:CommandField ButtonType="Link" ShowEditButton="true"
                            ItemStyle-Width="150" />

                    </Columns>
                </asp:GridView>

       
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>

  

    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script type="text/javascript" src="scripts/jquery.blockUI.js"></script>
    <script type="text/javascript">
        $(function () {
            BlockUI("");
            $.blockUI.defaults.css = {};
        });
        function BlockUI(elementID) {
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_beginRequest(function () {
                $("#" + elementID).block({ message: '<div align = "center">' + '<img src="images/loadingAnim.gif"/></div>',
                    css: {},
                    overlayCSS: { backgroundColor: '#000000', opacity: 0.6, border: '3px solid #63B2EB' }
                });
            });
            prm.add_endRequest(function () {
                $("#" + elementID).unblock();
            });
        };
    </script>


</asp:Content>