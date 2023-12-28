<%@ Page Title="" Language="C#" MasterPageFile="~/DealerReports/DealerReports.master" Debug="true"  AutoEventWireup="true" CodeFile="Reports.aspx.cs" EnableEventValidation="false" Inherits="DealerReports_Reports" %>


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
        
        
        
        
  //checkbox validation Start
        function client_OnTreeNodeChecked() {
            var obj = window.event.srcElement;
            var treeNodeFound = false;
            var checkedState;
            if (obj.tagName == "INPUT" && obj.type == "checkbox") {
                var treeNode = obj;
                checkedState = treeNode.checked;
                do {
                    obj = obj.parentElement;
                } while (obj.tagName != "TABLE")
                var parentTreeLevel = obj.rows[0].cells.length;
                var parentTreeNode = obj.rows[0].cells[0];
                var tables = obj.parentElement.getElementsByTagName("TABLE");
                var numTables = tables.length
                if (numTables >= 1) {
                    for (i = 0; i < numTables; i++) {
                        if (tables[i] == obj) {
                            treeNodeFound = true;
                            i++;
                            if (i == numTables) {
                                return;
                            }
                        }
                        if (treeNodeFound == true) {
                            var childTreeLevel = tables[i].rows[0].cells.length;
                            if (childTreeLevel > parentTreeLevel) {
                                var cell = tables[i].rows[0].cells[childTreeLevel - 1];
                                var inputs = cell.getElementsByTagName("INPUT");
                                inputs[0].checked = checkedState;
                            }
                            else {
                                return;
                            }
                        }
                    }
                }
            }
        }







    
    //heckbox validation End
        
        
        
        
             
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

     
         


 <div class="content">
<div class="report_list" style="margin-top:20px;">
<h3>

    <b>Reports Name</b></h3>
    <ul>
       <li>
         <asp:RadioButton ID="btn_radio_zonewise_sales" runat="server" Text="Zone Wise Saleas Statement" 
               GroupName="Sales" 
          AutoPostBack="true" />
       </li>
       <li>
           <asp:RadioButton ID="btn_radio_dealergroup_sales" runat="server" Text="Dealer group wise sales qty" 
           GroupName="Sales" 
         AutoPostBack="true" />
       </li>

        <li>
           <asp:RadioButton ID="btn_radio_deposit" runat="server" Text="Deposit amount" 
           GroupName="Sales" 
          AutoPostBack="true" />
      </li>

<%--       <li>
           <asp:RadioButton ID="btn_radio_deposit_summary" runat="server" Text="Deposit Summary" 
           GroupName="Sales" 
         AutoPostBack="true" />
      </li>--%>

       <li>
           <asp:RadioButton ID="btn_radio_salesdetails_brandcat" runat="server" Text="Sales Details Report ( Brand & Category Wise)" 
           GroupName="Sales" 
        AutoPostBack="true" />
      </li>

       <li>
           <asp:RadioButton ID="btn_radio_sales_delivary" runat="server" Text="Sales delivery" 
           GroupName="Sales" 
        AutoPostBack="true" />
      </li>

       <li>
           <asp:RadioButton ID="btn_radio_sales_withdrawn" runat="server" Text="Sales withdrawn" 
           GroupName="Sales" 
        AutoPostBack="true" />
      </li>

      <li>
           <asp:RadioButton ID="btn_radio_country_dealer_stmnt" runat="server" Text="Countrywide dealer statement" 
           GroupName="Sales" 
        AutoPostBack="true" />
      </li>

      
       <li class="Individual_Delar">
          <a href="IndividualDealerStatementReport.aspx" class="dealer"> Individual Dealer Statetment</a>
      </li>

      </ul>

</div>

     

       <div class="tree_view">
       <h3><b>View Of Zone</b></h3>
        
        <asp:TreeView ID="tree_view" ShowCheckBoxes="All" runat="server" ShowLines="True" onclick="client_OnTreeNodeChecked();">
        
        </asp:TreeView>
    </div>
</div>

  <div>
      <asp:Button ID="btn_preview" runat="server" Text="Preview" 
          class="btn btn-primary" onclick="btn_preview_Click" />
  
</div>


<div class="border_style">
</div>

<div id="Divpoint" runat="server">

<h2 class="text-center fw-bold" id="reportCompanyanme" runat="server"></h2>
<h4  class="text-center fw-bold" id="hcompanyAdreess" runat="server"></h4>
<h4  class="text-center fw-bold" id="hcompanyrodadress" runat="server"></h4>
 <h3 id="repotname" class="tdesin m-0 text-center fw-bold" runat="server"></h3>
 <h3 class="text-center fw-bold" id="hReportsDateRange" runat="server"></h3>

   <%-- <%
     string fromdate = this.txtFrom.Text;
     string todate = this.txtToDate.Text;
     Response.Write(fromdate);
     Response.Write(todate);
    
   %>--%>
   
 
 </h3>
 <h2 class="text-center fw-bold" id="hZoneName" runat="server"></h2>
  <asp:GridView ID="gv_sales_delivery" runat="server" 
            AlternatingRowStyle-CssClass="alt" AutoGenerateColumns="False" CssClass="table" 
            BorderColor="#999999" BorderStyle="Double" BorderWidth="1px" CellPadding="2" 
            DataKeyNames="MRSRCode" GridLines="Vertical" 
            OnRowDataBound="GridView1_RowDataBound" 
          
            PagerStyle-CssClass="pgr" 
            ShowFooter="true" 
            Width="100%">
            <FooterStyle BackColor="#006666" ForeColor="White" />
            <HeaderStyle BackColor="#006666" CssClass="bg-primary"/>

            <PagerStyle CssClass="pgr"></PagerStyle>
                
            <SelectedRowStyle BackColor="#0099CC" />
            <AlternatingRowStyle CssClass="alt" BackColor="Silver"></AlternatingRowStyle>
            <Columns>
                   
              <asp:TemplateField HeaderText="SL">
                    <ItemTemplate>
                         <%#Container.DataItemIndex+1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                                
                <asp:BoundField DataField="MRSRCode" HeaderText="Invoice #" />
                <asp:BoundField DataField="RefCHNo" HeaderText="Ref. Invoice #" />
                <asp:BoundField DataField="TDate" HeaderText="Sales Date" /> 
                 <asp:BoundField DataField="ZoneName" HeaderText="From" /> 
                <asp:BoundField DataField="InSource" HeaderText="TO" />
                 
                <asp:BoundField DataField="NetSalesAmnt" HeaderText="Total Amount" />
                <asp:BoundField DataField="POCode" HeaderText="Order#" />
                <asp:BoundField DataField="sStatus" HeaderText="DMS Declare" />


            </Columns>
        </asp:GridView>
    
        
    <!--Gridview for Deposit-->
    
 <asp:GridView ID="gv_deposit" runat="server" 
                AlternatingRowStyle-CssClass="alt" AutoGenerateColumns="False" CssClass="table" 
                BorderColor="#999999" BorderStyle="Double" BorderWidth="1px" CellPadding="2" 
                DataKeyNames="CollectionNo" GridLines="Vertical" 
                OnRowDataBound="gv_deposit_RowDataBound" 
          
                PagerStyle-CssClass="pgr" 
                ShowFooter="true" 
                Width="100%" PageSize="400">
                <FooterStyle BackColor="#006666" ForeColor="White" />
                <HeaderStyle BackColor="#006666" CssClass="bg-primary"/>

                <PagerStyle CssClass="pgr"></PagerStyle>
                
                <SelectedRowStyle BackColor="#0099CC" />
                <AlternatingRowStyle CssClass="alt" BackColor="Silver"></AlternatingRowStyle>
                <Columns>
                        
                   <asp:TemplateField HeaderText="SL">
                        <ItemTemplate>
                             <%#Container.DataItemIndex+1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
              
                   <asp:BoundField DataField="CollectionNo" HeaderText="Ref #" />
                    <asp:BoundField DataField="DepositDate" HeaderText="Deposit Date" />                                 
                     <asp:BoundField DataField="ZoneName" HeaderText="From" /> 
                     <asp:BoundField DataField="Name" HeaderText="To" />
                    <asp:BoundField DataField="BankName" HeaderText="Bank Name" />    
                    <asp:BoundField DataField="DepositAmnt" HeaderText="Deposit Amount" />
                    <asp:BoundField DataField="PayType" HeaderText="Deposit Type" />                 
                    <asp:BoundField DataField="cRemarks" HeaderText="Remarks" />


                </Columns>


            </asp:GridView>


    <!--Gridview for Deposit end-->

  <!--Sales Withdrawn start -->
   <asp:GridView ID="gv_sales_withd" runat="server" 
            AlternatingRowStyle-CssClass="alt" AutoGenerateColumns="False" CssClass="table" 
            BorderColor="#999999" BorderStyle="Double" BorderWidth="1px" CellPadding="2" 
            DataKeyNames="MRSRCode" GridLines="Vertical" 
            OnRowDataBound="gv_sales_withd_RowDataBound" 
          
            PagerStyle-CssClass="pgr" 
            ShowFooter="true" 
            Width="100%" PageSize="400">
            <FooterStyle BackColor="#006666" ForeColor="White" />
            <HeaderStyle BackColor="#006666" CssClass="bg-primary"/>

            <PagerStyle CssClass="pgr"></PagerStyle>
                
            <SelectedRowStyle BackColor="#0099CC" />
            <AlternatingRowStyle CssClass="alt" BackColor="Silver"></AlternatingRowStyle>
            <Columns>
                     
             <asp:TemplateField HeaderText="SL">
                    <ItemTemplate>
                         <%#Container.DataItemIndex+1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                                
               <asp:BoundField DataField="MRSRCode" HeaderText="Withdrawn" />
                <asp:BoundField DataField="TDate" HeaderText="Withdrawn Date" />
                 
                <asp:BoundField DataField="ZoneName" HeaderText="From" /> 
                 <asp:BoundField DataField="Name" HeaderText="To" />

                <asp:BoundField DataField="NetSalesAmnt" HeaderText="Total Amount" />                
                <asp:BoundField DataField="InvoiceNo" HeaderText="Sales Invoice" />


            </Columns>
        </asp:GridView>

   <!--Sales Withdrawn end -->

  <!-- GV ZoneWise Sales Statement start-->
   <asp:GridView ID="gv_Zone_statement" runat="server" 
            AlternatingRowStyle-CssClass="alt" AutoGenerateColumns="False" CssClass="table" 
            BorderColor="#999999" BorderStyle="Double" BorderWidth="1px" CellPadding="2" 
            DataKeyNames="DelearID" GridLines="Vertical" 
            OnRowDataBound="gv_Zone_statement_RowDataBound" 
          
            PagerStyle-CssClass="pgr" 
            ShowFooter="true" 
            Width="100%" PageSize="400">
            <FooterStyle BackColor="#006666" ForeColor="White" />
            <HeaderStyle BackColor="#006666" CssClass="bg-primary"/>

            <PagerStyle CssClass="pgr"></PagerStyle>
                
            <SelectedRowStyle BackColor="#0099CC" />
            <AlternatingRowStyle CssClass="alt" BackColor="Silver"></AlternatingRowStyle>
            <Columns>

  <asp:TemplateField HeaderText="SL">
        <ItemTemplate>
             <%#Container.DataItemIndex+1 %>
        </ItemTemplate>
    </asp:TemplateField>
                                
             <asp:BoundField DataField="DelearName" HeaderText="Dealer Name" />
                <asp:BoundField DataField="OB" HeaderText="OB" /> 
                <asp:BoundField DataField="SalesAmnt" HeaderText="Sales Amount" />
                 <asp:BoundField DataField="Withdrawn" HeaderText="Withdrawn" />  
                <asp:BoundField DataField="ActualSales" HeaderText="Actual Sales" />
                <asp:BoundField DataField="Collection" HeaderText="Bank Deposit" />
                             
                <asp:BoundField DataField="DishonourAmnt" HeaderText="Dishonour Amount" />
                <asp:BoundField DataField="CB" HeaderText="CB" />


            </Columns>


        </asp:GridView>

  <!--ZoneWise Sales Statement End-->

  <!--Brand And CatagoryWise Sales Data start-->

   <asp:GridView ID="gv_brand_cat" runat="server" 
            AlternatingRowStyle-CssClass="alt" AutoGenerateColumns="False" CssClass="table" 
            BorderColor="#999999" BorderStyle="Double" BorderWidth="1px" CellPadding="2" 
            DataKeyNames="Brand" GridLines="Vertical" 
            OnRowDataBound="gv_brand_cat_RowDataBound" 
          
            PagerStyle-CssClass="pgr" 
            ShowFooter="true" 
            Width="100%" PageSize="400">
            <FooterStyle BackColor="#006666" ForeColor="White" />
            <HeaderStyle BackColor="#006666" CssClass="bg-primary"/>

            <PagerStyle CssClass="pgr"></PagerStyle>
                
            <SelectedRowStyle BackColor="#0099CC" />
            <AlternatingRowStyle CssClass="alt" BackColor="Silver"></AlternatingRowStyle>
            <Columns>

     <asp:TemplateField HeaderText="SL">
        <ItemTemplate>
             <%#Container.DataItemIndex+1 %>
        </ItemTemplate>
    </asp:TemplateField>
                
                 <asp:BoundField DataField="MRSRCode" HeaderText="Invoice" />   
                   <asp:BoundField DataField="Name" HeaderText="Dealer" />             
               <asp:BoundField DataField="Brand" HeaderText="Brand" />
             

                <asp:BoundField DataField="Catagory" HeaderText="Category" /> 
                <asp:BoundField DataField="Model" HeaderText="Model" />
                <asp:BoundField DataField="Date" HeaderText="Date" />
                <asp:BoundField DataField="UnitPrice" HeaderText="Unit Price" />                
                <asp:BoundField DataField="Qty" HeaderText="Qty" />
                <asp:BoundField DataField="TotalAmnt" HeaderText="Total Amount" />
                <asp:BoundField DataField="DiscountAmnt" HeaderText="Discount" />
                <asp:BoundField DataField="NetAmnt" HeaderText="Net Amount" />


            </Columns>


        </asp:GridView>


     
 <!--Country Wise Dealer Statement-->
  <asp:GridView ID="gv_countryWise" runat="server" 
            AlternatingRowStyle-CssClass="alt" AutoGenerateColumns="False" CssClass="table" 
            BorderColor="#999999" BorderStyle="Double" BorderWidth="1px" CellPadding="2" 
            DataKeyNames="ZoneName" GridLines="Vertical" 
            OnRowDataBound="gv_countryWise_RowDataBound" 
          
            PagerStyle-CssClass="pgr" 
            ShowFooter="true" 
            Width="100%" PageSize="400">
            <FooterStyle BackColor="#006666" ForeColor="White" />
            <HeaderStyle BackColor="#006666" CssClass="bg-primary"/>

            <PagerStyle CssClass="pgr"></PagerStyle>
                
            <SelectedRowStyle BackColor="#0099CC" />
            <AlternatingRowStyle CssClass="alt" BackColor="Silver"></AlternatingRowStyle>
            <Columns>

  <asp:TemplateField HeaderText="Sl">
        <ItemTemplate>
             <%#Container.DataItemIndex+1 %>
        </ItemTemplate>
    </asp:TemplateField>
                                
             <asp:BoundField DataField="ZoneName" HeaderText="Zone Name" />
                <asp:BoundField DataField="OB" HeaderText="OB" /> 
                <asp:BoundField DataField="SalesAmnt" HeaderText="Sales Amount" />
                 <asp:BoundField DataField="Withdrawn" HeaderText="Withdrawn" />  
                <asp:BoundField DataField="ActualSales" HeaderText="Actual Sales" />
                <asp:BoundField DataField="Collection" HeaderText="Bank Deposit" />
                             
                <asp:BoundField DataField="DishonourAmnt" HeaderText="Dishonour Amount" />
                <asp:BoundField DataField="CB" HeaderText="CB" />


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


