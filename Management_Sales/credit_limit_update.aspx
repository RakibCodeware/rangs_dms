<%@ Page Language="C#" AutoEventWireup="true" CodeFile="credit_limit_update.aspx.cs" Inherits="credit_limit" 
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

  

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

    <h4 class="col-sm-12 bg-primary" style="padding:5px"> Dealer Credit Limit Information ...</h4>
    <p></p>

    <div>

    <table width ="100%">
        <tr>
            <td>Zone/Area:</td>
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
                    DataKeyNames="TID" OnRowEditing="OnRowEditing" OnRowCancelingEdit="OnRowCancelingEdit"
                    PageSize="250" AllowPaging="true" OnPageIndexChanging="OnPaging" OnRowUpdating="OnRowUpdating"
                    EmptyDataText="No records has been added." Width="100%">
                    <Columns>
                        
                        <asp:TemplateField HeaderText="SL#" ItemStyle-Width="5%">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField> 

                        <asp:TemplateField HeaderText="Code">
                            <ItemTemplate>
                                <asp:Label ID="lblCode" runat="server" Text='<%# Eval("Code") %>'></asp:Label>
                            </ItemTemplate>                            
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="Dealer Name">
                            <ItemTemplate>
                                <asp:Label ID="lblTitle" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                            </ItemTemplate>                            
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Credit Limit (Tk)" ItemStyle-Width="150">
                            <ItemTemplate>
                                <asp:Label ID="lblMRP" runat="server" Text='<%# Eval("TAmount") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtMRP" runat="server" Text='<%# Eval("TAmount") %>' Width="140"></asp:TextBox>
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