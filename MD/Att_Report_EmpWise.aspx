<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Att_Report_EmpWise.aspx.cs" Inherits="Admin_Att_Report_EmpWise" MasterPageFile="Admin.master"%>

<%@ Register namespace="AjaxControlToolkit" tagprefix="AjaxControlToolkit" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %> 
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
                               
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    
    <link type="text/css" href="../css/ui-lightness/jquery-ui-1.8.19.custom.css" rel="stylesheet" />
    <script type="text/javascript" src="../js/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="../js/jquery-ui-1.8.19.custom.min.js"></script>

    <script type="text/javascript">
        $(function () {
            $("#txtFromDate").datepicker();
            $("#txtToDate").datepicker();
        });        
    </script>

    <div>&nbsp;</div>
    <div align="center">
        <asp:Label ID="Label111" runat="server" Text="Employee Wise Attendance Report" Font-Bold="True" 
            Font-Names="Tahoma" Font-Size="Large" Font-Underline="True"></asp:Label>
    </div>

    <h4 class="col-sm-12 bg-primary" style="padding:5px">Report Criteria</h4>

        <div class="text-center text-danger lead">
            <asp:Label ID="lblmsg" runat="server" Text=""></asp:Label></div>
                           
    <div align="center">
        <table width="100%">
            <tr>
                <td style="width: 9px"></td>
                <td style="width: 105px"></td>
                <td></td>
                <td style="width: 332px">&nbsp;</td>
                <td></td>
                <td></td>
            </tr>
            <tr>
                <td class="style1" style="width: 9px"></td>
                <td align="left" class="style2" style="width: 105px">Employee Name</td>
                <td class="style4">:</td>
                <td class="style3" align ="left" style="width: 332px">    	        	        	    
    	            <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="True" 
                        BackColor="#F6F1DB" Height="24px"                  
                        ToolTip="Please Select Employee Location ..." Width="288px" 
                        onselectedindexchanged="DropDownList1_SelectedIndexChanged" 
                        Font-Size="Small">
                        <asp:ListItem Text="--Select--" Value=""></asp:ListItem>
                    </asp:DropDownList>
    	        </td>
                <td align ="left">Job ID</td>
                <td align ="left">
                    <asp:TextBox ID="txtJodID" runat="server" Height="22px" Width="150px" 
                        BackColor="#9CFFCE" BorderStyle="None" Enabled="False"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="style1" style="height: 22px; width: 9px;"></td>
                <td align ="left" class="style2" style="height: 22px; width: 105px;">From Date</td>
                <td class="style4" style="height: 22px">:</td>
                <td align ="left" class="style3" style="height: 22px; width: 332px;">
                    <asp:TextBox ID="txtFromDate" runat="server" Style="z-index: 109; left: 316px; 
                        top: 165px; font-weight: normal; font-size: x-small; font-family: Arial;" Height="24px" Width="150px">
                    </asp:TextBox>
                    &nbsp;
                    
                    <asp:ImageButton ID="imgPopup" ImageUrl="~/images/cal.gif" ImageAlign="Bottom"
                        runat="server" />

                    <cc1:CalendarExtender ID="Calendar1" PopupButtonID="imgPopup" runat="server" TargetControlID="txtFromDate"
                        Format="dd-MMM-yyyy">
                    </cc1:CalendarExtender>   
                     
                    <!-- 
                    <asp:Label ID="Label5" runat="server" Text="(mm/dd/yyyy)"></asp:Label>
                    -->    
                </td>
                <td style="height: 22px" align ="left">Designation</td>
                <td style="height: 22px" align ="left">
                    <asp:TextBox ID="txtDesg" runat="server" Height="22px" Width="150px" 
                        BackColor="#9CFFCE" BorderStyle="None" Enabled="False"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="style1" style="width: 9px"></td>
                <td align ="left" class="style2" style="width: 105px">To Date</td>
                <td class="style4">:</td>
                <td align ="left" class="style3" style="width: 332px">
                    <asp:TextBox ID="txtToDate" runat="server" 
                        Style="z-index: 110; left: 180px; 
                            top: 166px; font-weight: normal; font-size: x-small; font-family: Arial;" 
                            Height="24px" Width="150px"></asp:TextBox>
                            &nbsp;   
                     <asp:ImageButton ID="imgPopup1" ImageUrl="~/Images/cal.gif" ImageAlign="Bottom"
                        runat="server" />
                    <cc1:CalendarExtender ID="CalendarExtender2" PopupButtonID="imgPopup1" runat="server" TargetControlID="txtToDate"
                        Format="dd-MMM-yyyy">
                    </cc1:CalendarExtender>                               
                            
                </td>
                <td align ="left">Job Location</td>
                <td align ="left">
                    <asp:TextBox ID="txtLocation" runat="server" Height="22px" Width="150px" 
                        BackColor="#9CFFCE" BorderStyle="None" Enabled="False"></asp:TextBox>
                </td>
            </tr>

            <tr>
                <td></td>
                <td>&nbsp;</td>
                <td></td>
                <td></td>
                <td></td>
            </tr>

            <tr>
                <td class="style1" style="width: 9px"></td>
                <td class="style2" style="width: 105px"></td>
                <td class="style4"></td>
                <td align="left" class="style3" style="width: 332px">
                    <asp:Button ID="btnSearch" CssClass="btn btn-primary" runat="server" 
                        Height="32px" Text="Search" 
                        Width="89px" onclick="btnSearch_Click" />
                </td>
                <td></td>
                <td></td>
            </tr>
            <tr>
                <td class="style5" style="width: 9px"></td>
                <td class="style6" style="width: 105px"></td>
                <td class="style7"></td>
                <td class="style8" style="width: 332px">
                    <asp:ScriptManager ID="ScriptManager1" runat="server">
                    </asp:ScriptManager>
                </td>
                <td class="style9"></td>
                <td class="style9"></td>
            </tr>
        </table>
    </div>
            
    <div>&nbsp;</div>

    <div>
        
        <asp:DataGrid ID="gvProd" runat="server" BackColor="White" 
            BorderColor="#999999" BorderStyle="Double" BorderWidth="1px" CellPadding="2" 
            CssClass="table" Font-Names="Verdana" Font-Size="Small" GridLines="Vertical" 
            Height="30%" ShowFooter="True" Width="100%">
            
            <FooterStyle BackColor="#00CC99" ForeColor="Black" />
            <SelectedItemStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#CCFFCC" ForeColor="Black" HorizontalAlign="Center" 
                Mode="NumericPages" />
            <AlternatingItemStyle BackColor="#C2D69B" />
            <ItemStyle BorderColor="Black" ForeColor="Black" />
            <HeaderStyle BackColor="green" CssClass="bg-primary" 
                HorizontalAlign="Center" />
        </asp:DataGrid>
        
    </div>

    <div>&nbsp;</div>

</asp:Content>