<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.master" AutoEventWireup="true" CodeFile="DealersalseReport.aspx.cs" Inherits="DealerReports_DealersalseReport" %>

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

      <h3>Dealer Sales Report With Dealer and Zone Name</h3>

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


       <div class="DealerSalesReport">
         <asp:GridView ID="gvdealerSalesReport" CssClass="table" runat="server">

          </asp:GridView>
       </div>
  
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


