<%@ Page Title="" Language="C#" MasterPageFile="Admin.master" AutoEventWireup="true" CodeFile="AcountsReport.aspx.cs" Inherits="DealerReports_AcountsReport" %>

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
      
    <div class="header_content">

      <h3>Please Select Report Type</h3>

    </div>

   
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



    <div style="padding:0" class="container">
    
        <div style="margin:10px 0;padding:0;" class="row">

            <div style="display:flex; align-items:center; padding:0;" class="col-lg-6">

            <div style="margin:0 12px">
             <span>Model : </span>
            </div>
               

                <div>
                <asp:TextBox ID="txtModel" runat="server" Height="25px" Width="200px" 
                CssClass="form-control"
                placeholder="Enter Model & Search" TabIndex="10" 
                AutoPostBack="True"> </asp:TextBox>

                <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" TargetControlID="txtModel"
                    MinimumPrefixLength="1" EnableCaching="true" CompletionSetCount="1" CompletionInterval="100" 
                    ServiceMethod="GetModel" >
                </asp:AutoCompleteExtender>
                </div>               
                

                                
            </div>

             <div class="col-lg-6">
            
            </div>

            <div class="col-lg-6">
            
            </div>

        
        </div>
    
    </div>

       
 <div class="content">
<div class="report_list" style="margin-top:20px;">
<h3>

    <b>Reports Name</b></h3>
    <ul>


      <li>
           <asp:RadioButton ID="btn_radio_salesReport" runat="server" Text="Sales Report" 
           GroupName="Sales" 
        AutoPostBack="true" />
      </li>

      
      <li>
           <asp:RadioButton ID="btn_radio_inventory" runat="server" Text="Inventory Report" 
           GroupName="Sales" 
        AutoPostBack="true" />
      </li>


      </ul>

</div>

     

</div>

  <div>
      <asp:Button ID="btn_preview" runat="server" Text="Preview" 
          class="btn btn-primary" onclick="btn_preview_Click" />
  
</div>
    <asp:GridView ID="gv_salesReport" runat="server">
    </asp:GridView>


    

 <%--<asp:GridView ID="gv_salesReport" runat="server"
            AlternatingRowStyle-CssClass="alt" AutoGenerateColumns="False" CssClass="table" 
            BorderColor="#999999" BorderStyle="Double" BorderWidth="1px" CellPadding="2" 
            DataKeyNames="MRSRCode" GridLines="Vertical" 
            OnRowDataBound="gv_salesReport_RowDataBound" 
          
            PagerStyle-CssClass="pgr" 
            ShowFooter="true" 
            Width="100%" PageSize="400">
            <FooterStyle BackColor="#006666" ForeColor="White" />
            <HeaderStyle BackColor="#006666" CssClass="bg-primary"/>

            <PagerStyle CssClass="pgr"></PagerStyle>
                
            <SelectedRowStyle BackColor="#0099CC" />
            <AlternatingRowStyle CssClass="alt" BackColor="Silver"></AlternatingRowStyle>
            <Columns>
                   
              <asp:TemplateField HeaderText="Serial">
                    <ItemTemplate>
                         <%#Container.DataItemIndex+1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                                
                <asp:BoundField DataField="CTP" HeaderText="CTP " />
                <asp:BoundField DataField="Location" HeaderText="Location" />
                <asp:BoundField DataField="Model" HeaderText="Model" /> 
                 <asp:BoundField DataField="Month" HeaderText="Month" /> 
                <asp:BoundField DataField="SalesQty" HeaderText="SalesQty" />
                 
                <asp:BoundField DataField="SalesAmnt" HeaderText="SalesAmnt" />
                <asp:BoundField DataField="WithQty" HeaderText="WithQty#" />
                <asp:BoundField DataField="WithdrawnAmnt" HeaderText="WithdrawnAmnt" />
                <asp:BoundField DataField="Qty" HeaderText="Qty" />
                <asp:BoundField DataField="GrossSales" HeaderText="GrossSales" />

            </Columns>
        </asp:GridView>
--%>

   <%-- <asp:GridView ID="gvInventoryReport" CssClass="table" Width="500px" runat="server">

    </asp:GridView>--%>
    <div class="Inventory">
     <asp:GridView ID="gvInventoryReport" runat="server" 
                AlternatingRowStyle-CssClass="alt" AutoGenerateColumns="False" CssClass="table" 
                BorderColor="#999999" BorderStyle="Double" BorderWidth="1px" CellPadding="1" 
                DataKeyNames="model" GridLines="Vertical" 
                OnRowDataBound="gvInventoryReport_RowDataBound" 
                 
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
              
                   <asp:BoundField DataField="model" HeaderText="Model" />
                    <asp:BoundField DataField="BrandName" HeaderText="BrandName" />                                 
                     <asp:BoundField DataField="OpCIDD" HeaderText="OpCIDD" /> 
                     <asp:BoundField DataField="OpCTP" HeaderText="OpCtp" />
                    <asp:BoundField DataField="OpCKDSKD" HeaderText="OpCKDSKD" />    
                    <asp:BoundField DataField="OpCBU" HeaderText="OpCBU" />
                    <asp:BoundField DataField="OpeningTotalQty" HeaderText="OpTotalQty" />                 
                    <asp:BoundField DataField="PurchaseQty" HeaderText="PurQty" />

                       <asp:BoundField DataField="WithDrawnQty" HeaderText="WithQty" /> 
                     <asp:BoundField DataField="SalesQty" HeaderText="SalesQty" />
                    <asp:BoundField DataField="ClosCidd" HeaderText="ClCIDD" />    
                    <asp:BoundField DataField="ClCtp" HeaderText="ClCTP" />
                    <asp:BoundField DataField="cloCKDSKD" HeaderText="ClCKDSKD" />                 
                    <asp:BoundField DataField="CloCBU" HeaderText="CloCBU" />
                        <asp:BoundField DataField="closingstock" HeaderText="cloStock" />


                </Columns>


            </asp:GridView>

</div>

        
    <div class="btn_export">
        <div class="btn_expot_excel">
           <asp:Button ID="btn_export_excel" runat="server" Text="Export to Excel" 
              onclick="btn_export_excel_Click" />
        </div>

        <div class="btn_export_pdf">
           <asp:Button ID="btn_export_pdf" runat="server"  Text="Export Pdf" class="btn-warning"
            onclick="btn_export_pdf_Click" />
        </div>

    </div>

</asp:Content>

