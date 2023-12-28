<%@ Page Title="" Language="C#" MasterPageFile="~/DealerReports/DealerReports.master" AutoEventWireup="true" 
EnableEventValidation="false" CodeFile="IndividualDealerStatementReport.aspx.cs" Inherits="DealerReports_IndividualDealerStatementReport" %>


<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">

<link type="text/css" href="https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/css/select2.min.css" rel="stylesheet" />
<script type="text/javascript" src="https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/js/select2.min.js"></script>
<script type="text/javascript" src="https://cdn.jsdelivr.net/npm/jspdf@1.5.3/dist/jspdf.min.js"></script>

<style type="text/css">
    
    .align-items-center
    {
        display:flex;
        align-items:center;
        
        }
    
    .py-3
    {
        
        padding:5px 0;
        }
    
    label
    {
        margin-bottom: 0;
        }
    
.mb-3
{
    margin-bottom:20px;
    
    }
    
    .form-control
    {
        background-color:#fff !important;
        }

             .dealerName
             {
                 margin:30px 0;
                 }
            .dealerName {
    align-items: center;
    gap:20px !important;
    .main
     {
     border:2px solid red;
     }
}
.dealerName h3
{
    margin:0;
    padding:0;
    }
    
    .d-flex
    {
        display:flex;
        
        }
       .border {
            border: 2px solid #000;
            width: 435px;
          }
    .justify-content-end
    {
                
        justify-content:end;
        }
    .gx-3
    {
        gap:10px;
        }
    .mt-5
    {
        margin-top:20px;
                        
        }
</style>

<script type="text/javascript">
    // In your Javascript (external .js resource or <script> tag)
    $(document).ready(function () {
        $('.js-example-basic-single').select2();
    });
</script>




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


       <div class="dealerName">
        <h3>Please Select Dealer Name : </h3>
           <asp:DropDownList ID="ddlDealerName" CssClass="js-example-basic-single form-control" runat="server">
          </asp:DropDownList>
      </div>


      
    
    <div class="row">

      <div class="col-lg-4 mb-3">
          <label>Delivery Amount</label>
          <asp:Label ID="lblSalesAmount" runat="server"></asp:Label>
       </div>

      <div class="col-lg-4 mb-3">
          <label>Deposit Amount</label>
           <asp:Label ID="lblDeposit" runat="server"></asp:Label>
      </div>

      <div class="col-lg-4 mb-3">
           <label>Withdrawn Amount</label>
            <asp:Label ID="lblWithdrawn" runat="server"></asp:Label>
      </div>

 </div>






 <asp:Panel ID="PagePdf" runat="server">
<div id="divPoint" runat="server" class="main">
  

      <div>
       <asp:Label ID="lblOBSales" runat="server" Visible="False"></asp:Label>
       <asp:Label ID="lblOBCollection" runat="server" Visible="False"></asp:Label>
       <asp:Label ID="lblOBDis" runat="server" Visible="False"></asp:Label>
       <asp:Label ID="lblOBWith" runat="server" Visible="False"></asp:Label>
      
      </div>

      <div>
          <h2 class="text-center fw-bold" id="reportCompanyanme" runat="server"></h2>
        <h4  class="text-center fw-bold" id="hcompanyAdreess" runat="server"></h4>
        <h4  class="text-center fw-bold" id="hcompanyrodadress" runat="server"></h4>
         <h3 id="repotname" class="tdesin m-0 text-center fw-bold" runat="server"></h3>
         <h3 class="text-center fw-bold" id="hReportsDateRange" runat="server"></h3>
          <h3 class="fw-bold" id="dealername" runat="server"></h3>
     </div>



    <div class="row">

      <div class="col-lg-6">
           <h4 >Sales Delivery
        <asp:Label ID="lblDate1" runat="server"></asp:Label>
    </h4>
     <asp:GridView ID="gvSales" runat="server" 
            AlternatingRowStyle-CssClass="alt" AutoGenerateColumns="False" CssClass="table" 
            DataKeyNames="MRSRCode"
            OnRowDataBound="gvSales_RowDataBound"             
            ShowFooter="true">
            <FooterStyle BackColor="#006666" ForeColor="White" />
            <HeaderStyle BackColor="#006666" CssClass="bg-primary"/>
            <Columns>
                <asp:TemplateField HeaderText="SL#">
                    <ItemTemplate>
                        <%# Container.DataItemIndex + 1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                                
                <asp:BoundField DataField="MRSRCode" HeaderText="Invoice #" />
                <asp:BoundField DataField="TDate" HeaderText="Sales Date" /> 
                <asp:BoundField DataField="NetSalesAmnt" HeaderText="Total Amnt" />              
                 
            </Columns>
        </asp:GridView>
    </div>



      

      <div class="col-lg-6">
           <h4>Deposit Amount
        <asp:Label ID="lblDate3" runat="server"></asp:Label>
    </h4>
            <asp:GridView ID="gvDeposit" runat="server" 
            AlternatingRowStyle-CssClass="alt" AutoGenerateColumns="False" CssClass="table" 
            BorderColor="#999999" BorderStyle="Double" BorderWidth="1px" CellPadding="2" 
            DataKeyNames="CollectionNo" GridLines="Vertical" 
            OnRowDataBound="gvDeposit_RowDataBound"             
            PagerStyle-CssClass="pgr" 
            ShowFooter="true" 
            Width="100%">
            <FooterStyle BackColor="#006666" ForeColor="White" />
            <HeaderStyle BackColor="#006666" CssClass="bg-primary"/>

            <PagerStyle CssClass="pgr"></PagerStyle>
                
            <SelectedRowStyle BackColor="#0099CC" />
            <AlternatingRowStyle CssClass="alt" BackColor="Silver"></AlternatingRowStyle>
            <Columns>
                <asp:TemplateField HeaderText="SL#">
                    <ItemTemplate>
                        <%# Container.DataItemIndex + 1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                                
                <asp:BoundField DataField="CollectionNo" HeaderText="Ref #" />
                <asp:BoundField DataField="DepositDate" HeaderText="Deposit Date" />                   
                <asp:BoundField DataField="DepositAmnt" HeaderText="Deposit Amnt" />       
                                                 
            </Columns>
        </asp:GridView>
     </div>


     <div class="col-lg-6">
                   <h4>Sales Withdrawn
        <asp:Label ID="lblDate2" runat="server"></asp:Label>
    </h4>
      <asp:GridView ID="gvWithdran" runat="server" 
            AlternatingRowStyle-CssClass="alt" AutoGenerateColumns="False" CssClass="table" 
            BorderColor="#999999" BorderStyle="Double" BorderWidth="1px" CellPadding="2" 
            DataKeyNames="MRSRCode" GridLines="Vertical" 
            OnRowDataBound="gvWithdran_RowDataBound"            
            PagerStyle-CssClass="pgr" 
            ShowFooter="true" 
            Width="100%" >
            <FooterStyle BackColor="#006666" ForeColor="White" />
            <HeaderStyle BackColor="#006666" CssClass="bg-primary"/>

            <PagerStyle CssClass="pgr"></PagerStyle>
                
            <SelectedRowStyle BackColor="#0099CC" />
            <AlternatingRowStyle CssClass="alt" BackColor="Silver"></AlternatingRowStyle>
            <Columns>
                <asp:TemplateField HeaderText="SL#">
                    <ItemTemplate>
                        <%# Container.DataItemIndex + 1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                                
                <asp:BoundField DataField="MRSRCode" HeaderText="Ref #" />
                <asp:BoundField DataField="TDate" HeaderText="Withdrawn Date" /> 
                <asp:BoundField DataField="NetSalesAmnt" HeaderText="Withdrawn Amnt" />               
                                                 
            </Columns>
        </asp:GridView>
     </div>
    
     
    </div>





  <div class="footer_row">
          
    <label>Opening Balance :</label>
    <asp:Label ID="lblOb" runat="server"></asp:Label>
         
     <label class="footer_style">Closing Amount :</label>
    <asp:Label ID="lblClosing" runat="server"></asp:Label>
     
 </div>
      
          
   </div>
   
   </asp:Panel>
 <div class="mt-5">
     <!--Preview button-->
      <div class="col-lg-12 mb-3">
            <asp:Button ID="btn_preview" runat="server" Text="Preview" 
          class="btn btn-primary" onclick="btn_preview_Click" />

          <!--Excel button-->
      <div class="btn_export">
        <div class="btn_expot_excel">
           <asp:Button ID="btn_export_excel" runat="server" Text="Export to Excel" 
              onclick="btn_export_excel_Click" />
        </div>

        <!--Pdf Button-->
        <div class="btn_export_pdf">
           <asp:Button ID="btn_export_pdf" runat="server"  Text="Export Pdf" class="btn-warning"
            onclick="btn_export_pdf_Click" />
        </div>
      </div>
    </div>
</div>


  


</asp:Content>


