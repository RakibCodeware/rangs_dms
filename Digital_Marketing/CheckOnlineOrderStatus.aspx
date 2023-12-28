<%@ Page Title="" Language="C#" MasterPageFile="~/Digital_Marketing/Admin.master" AutoEventWireup="true" CodeFile="CheckOnlineOrderStatus.aspx.cs" Inherits="Digital_Marketing_CheckOnlineOrderStatus" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">

 <script language="JavaScript" src="../js/datetimepicker.js" type="text/javascript"> </script>
    
    <script type="text/javascript">
        $(function () {
            $("#txtDate").datepicker();
            $("#txtFrom").datepicker();
            $("#txtToDate").datepicker();
            $("#txtPIDate").datepicker();
        });   

      </script>

  <div class="date_timepiker">
           
         <table width="50%">

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
                
                    &nbsp;
                    <asp:ImageButton ID="imgPopup" ImageUrl="~/Images/cal.gif" ImageAlign="Bottom"
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
                
                    &nbsp;
                    <asp:ImageButton ID="imgPopup1" ImageUrl="~/Images/cal.gif" ImageAlign="Bottom"
                        runat="server" TabIndex="1" />
                </td>
                <td></td>
            </tr>
         </table> 
       </div>

  <div>
      <asp:Button ID="btn_preview" runat="server" Text="Preview" 
          class="btn btn-primary" onclick="btn_preview_Click"/>
  
</div>

<asp:GridView ID="gvOnlineOrderStatus" runat="server" 
                AlternatingRowStyle-CssClass="alt"  CssClass="table" 
                BorderColor="#999999" BorderStyle="Double" BorderWidth="1px" CellPadding="1" 
                DataKeyNames="OrderNumber" GridLines="Vertical" 
         
                 
                PagerStyle-CssClass="pgr" 
                ShowFooter="true" 
                 PageSize="300">
                <FooterStyle BackColor="#006666" ForeColor="White" />
                <HeaderStyle BackColor="#006666" CssClass="bg-primary"/>
                   <PagerStyle ForeColor="#8C4510" 
          HorizontalAlign="Center"></PagerStyle>

                <PagerStyle CssClass="pgr"></PagerStyle>
                
                <SelectedRowStyle BackColor="#0099CC" />
                <AlternatingRowStyle CssClass="alt" BackColor="Silver"></AlternatingRowStyle>
                <Columns>
                        
                   <asp:TemplateField HeaderText="SL">
                        <ItemTemplate>
                             <%#Container.DataItemIndex+1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
              
                   <asp:BoundField DataField="OrderNumber" HeaderText="OrderNumber" />
                    <asp:BoundField DataField="InvoiceNumber" HeaderText="InvoiceNumber" />                                 
                     
                  </Columns>


            </asp:GridView>


</asp:Content>




