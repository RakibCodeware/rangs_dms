<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="Admin.master"
CodeFile="Requirement_List.aspx.cs" Inherits="CTP_Requirement_List" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

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
    
    <h2 class="col-sm-12 bg-primary" 
        style="padding:5px; background-color: #CC0000; color: #FFFFFF;"> My Requirement List ...</h2>
    <p>
        &nbsp;</p>
        <div>
        
        <table width="100%">
            <tr>
                <td></td>
                <td>
                    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                </td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
            </tr>

            <tr>
                <td></td>
                <td colspan="5">                    
                    <asp:RadioButtonList ID="RadioButtonList1" runat="server" 
                        RepeatDirection="Horizontal" Width="80%">
                        <asp:ListItem Selected="True">ALL</asp:ListItem>
                        <asp:ListItem>Pending</asp:ListItem>
                        <asp:ListItem>Delivered</asp:ListItem>
                    </asp:RadioButtonList>                    
                </td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
            </tr>

            <tr>
                <td></td>
                <td>
                    &nbsp;
                </td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
            </tr>

            <tr>
                <td>&nbsp;</td>
                <td>
                    From Date
                </td>
                <td>:</td>
                <td>
                <asp:TextBox ID="txtFrom" runat="server" Width="110px" TabIndex="1" 
                        ToolTip="Please Enter From Date" MaxLength="10"></asp:TextBox> 
                    <cc1:calendarextender ID="Calendar1" PopupButtonID="imgPopup" runat="server" TargetControlID="txtFrom"
                        Format="MM/dd/yyyy">
                    </cc1:calendarextender>
                
                &nbsp;<asp:ImageButton ID="imgPopup" ImageUrl="~/Images/cal.gif" ImageAlign="Bottom"
                        runat="server" TabIndex="1" />
                    </td>
                <td></td>
                <td>To Date</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="txtToDate" runat="server" Width="110px" TabIndex="2" 
                            ToolTip="Please Enter To Date" MaxLength="10"></asp:TextBox> 
                    <cc1:calendarextender ID="Calendarextender1" PopupButtonID="imgPopup1" runat="server" TargetControlID="txtToDate"
                        Format="MM/dd/yyyy">
                    </cc1:calendarextender>
                
                &nbsp;<asp:ImageButton ID="imgPopup1" ImageUrl="~/Images/cal.gif" ImageAlign="Bottom"
                        runat="server" TabIndex="1" />
                    </td>
                <td></td>
            </tr>

            <tr>
                <td></td>
                <td>
                    
                </td>
                <td></td>
                <td style="color: #FF0000">(MM/dd/yyyy)</td>
                <td></td>
                <td></td>
                <td></td>
                <td style="color: #FF0000">(MM/dd/yyyy)</td>
                <td></td>
            </tr>

            <tr>
                <td></td>
                <td>
                    
                </td>
                <td></td>
                <td>&nbsp;</td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
            </tr>

            
            <tr>
                <td></td>
                <td>
                    
                </td>
                <td></td>
                <td>
                    <asp:Button ID="btnSearch" CssClass="btn btn-primary" runat="server" 
                        data-toggle="tooltip" title="Click here for Search Sales Data ..."
                        Text="   Search  " OnClick="SearchData" />                        
                </td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
            </tr>


        </table>

    </div>

    <h4 class="col-sm-12 bg-primary" style="padding:0.5px"></h4>

    <div>&nbsp;</div>

   <!-- GRID VIEW -->
    <div>        
        <asp:GridView ID="GridView1" runat="server" AllowPaging="True" 
            AlternatingRowStyle-CssClass="alt" AutoGenerateColumns="False" CssClass="table" 
            BorderColor="#999999" BorderStyle="Double" BorderWidth="1px" CellPadding="2" 
            DataKeyNames="ReqAID" GridLines="Vertical" 
            OnRowDataBound="GridView1_RowDataBound" 
            OnPageIndexChanging="OnPageIndexChanging"
            OnRowCommand="GridView1_RowCommand"
            PagerStyle-CssClass="pgr"
            font-family="Arial"
            font-size= "10pt" 
            Width="100%" PageSize="10" Font-Names="Tahoma">
            <HeaderStyle BackColor="#CC0000" CssClass="bg-primary"/>

            <PagerStyle CssClass="pgr"></PagerStyle>
                
            <SelectedRowStyle BackColor="#0099CC" />
            <AlternatingRowStyle CssClass="alt" BackColor="#FFCCCC"></AlternatingRowStyle>
            <Columns>
                <asp:TemplateField HeaderText="SL#" ItemStyle-Width="5%">
                    <ItemTemplate>
                        <%# Container.DataItemIndex + 1 %>
                    </ItemTemplate>

<ItemStyle Width="5%"></ItemStyle>
                </asp:TemplateField>
                
                <asp:BoundField DataField="ReqAID" HeaderText="Master ID" Visible="false"/>
                <asp:BoundField DataField="ReqNo" HeaderText="Requirement #" 
                    ItemStyle-Width="15%">
<ItemStyle Width="15%"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="ReqDate" HeaderText="Date" ItemStyle-Width="15%">                                  
<ItemStyle Width="15%"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="ReqBy" HeaderText="Requirement By" 
                    ItemStyle-Width="15%">
<ItemStyle Width="15%"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="Remarks" HeaderText="Remarks" />
                <asp:BoundField DataField="tQty" HeaderText="Total Qty" 
                    ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Right">
<ItemStyle HorizontalAlign="Right" Width="10%"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="dStatus" HeaderText="Status" />
                
                <asp:TemplateField ItemStyle-Width="8px">
                    <ItemTemplate >
                        <asp:Button ID="btnAddCart" runat="server" Text="Details" 
                            CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                            CommandName="ShoppingCart" ItemStyle-HorizontalAlign="Center" />
                    </ItemTemplate>

<ItemStyle Width="8px"></ItemStyle>
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

