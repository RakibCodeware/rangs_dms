<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Att_Report_WithPhoto.aspx.cs" Inherits="Admin_Att_Report_WithPhoto" MasterPageFile="Admin.master"%>

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
        <asp:Label ID="Label111" runat="server" Text="Attendance Report with Photo" Font-Bold="True" 
            Font-Names="Tahoma" Font-Size="Large" Font-Underline="True"></asp:Label>
    </div>

    <h4 class="col-sm-12 bg-primary" style="padding:5px">Report Criteria</h4>

        <div class="text-center text-danger lead">
            <asp:Label ID="lblmsg" runat="server" Text=""></asp:Label></div>
                           
    <div align="center">
        <table width="100%">
            <tr>
                <td style="height: 17px"></td>
                <td style="height: 17px; width: 91px;"></td>
                <td style="height: 17px"></td>
                <td style="height: 17px; width: 189px;"></td>
                <td style="height: 17px; width: 49px;"></td>
                <td style="height: 17px; width: 108px;"></td>
                <td style="height: 17px; width: 125px;"></td>
                <td style="height: 17px"></td>
            </tr>
            <tr>
                <td class="style1" style="height: 24px"></td>
                <td align="left" class="style2" style="height: 15px; width: 91px;">&nbsp;Code</td>
                <td class="style4" style="height: 15px">:</td>
                <td class="style3" align ="left" style="height: 15px; width: 189px;">    	        	        	    
    	            <asp:TextBox ID="txtCode" runat="server" Height="21px" Width="300px" 
                        ReadOnly="True" BackColor="#99FF66" Font-Size="Small" BorderStyle="None"></asp:TextBox>
    	        </td>
                <td style="height: 15px; width: 49px;"></td>
                <td align="left" style="height: 15px; width: 108px;">Total Employee</td>
                <td align="left" style="height: 15px; width: 125px;">
                    <asp:TextBox ID="txtTEmp" runat="server" BackColor="#00CCFF" BorderStyle="None" 
                        Height="21px" ReadOnly="True" Font-Size="Large" Width="67px" 
                        style="text-align:center">0</asp:TextBox>
                </td>
                <td style="height: 15px"></td>
            </tr>
            <tr>
                <td class="style1" style="height: 24px"></td>
                <td align="left" class="style2" style="width: 91px; height: 19px;">CTP Name</td>
                <td class="style4" style="height: 19px">:</td>
                <td class="style3" align ="left" style="width: 189px; height: 19px;">    	        	        	    
    	            <asp:TextBox ID="txtName" runat="server" Height="21px" Width="300px" 
                        ReadOnly="True" BackColor="#FFFF66" Font-Size="Small" BorderStyle="None"></asp:TextBox>
    	        </td>
                <td style="width: 49px; height: 19px;"></td>
                <td align="left" style="width: 108px; height: 19px;">Total Present</td>
                <td align="left" style="width: 125px; height: 19px;">
                    <asp:TextBox ID="txtTPresent" runat="server" BackColor="#00CC00" BorderStyle="None" 
                        Height="21px" ReadOnly="True" Font-Size="Large" Width="67px" 
                        style="text-align:center">0</asp:TextBox>
                </td>
                <td style="height: 19px"></td>
            </tr>

            <tr>
                <td class="style1" style="height: 22px"></td>
                <td align="left" class="style2" style="width: 91px; height: 22px;">&nbsp;</td>
                <td class="style4" style="height: 22px">&nbsp;</td>
                <td class="style3" align ="left" style="width: 189px; height: 22px;">    	        	        	    
    	            <asp:DropDownList ID="ddlLoc" runat="server" AutoPostBack="False" 
                        BackColor="#F6F1DB" Height="21px"                  
                        ToolTip="Please Select Employee Location ..." Width="288px" TabIndex="1" 
                        Enabled="False" Visible="False">
                        <asp:ListItem Text="--Select--" Value=""></asp:ListItem>
                    </asp:DropDownList>
    	        </td>
                <td style="width: 49px; height: 22px;"></td>
                <td align="left" style="width: 108px; height: 22px;">On Leave</td>
                <td align="left" style="width: 125px; height: 22px;">
                    <asp:TextBox ID="txtTLeave" runat="server" BackColor="#CCFF33" BorderStyle="None" 
                        Height="21px" ReadOnly="True" Font-Size="Large" Width="67px" 
                        style="text-align:center">0</asp:TextBox>
                </td>
                <td style="height: 22px">
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;</td>
            </tr>

            <tr>
                <td class="style1" style="height: 8px"></td>
                <td align ="left" class="style2" style="height: 8px; width: 91px;">From Date</td>
                <td class="style4" style="height: 8px">:</td>
                <td align ="left" class="style3" style="height: 8px; width: 189px;">
                    <asp:TextBox ID="txtFromDate" runat="server" Style="z-index: 109; left: 316px; 
                        top: 165px; font-weight: normal; font-size: x-small; font-family: Arial;" 
                        Height="21px" Width="150px" TabIndex="2"></asp:TextBox>
                    &nbsp;
                    
                    <asp:ImageButton ID="imgPopup" ImageUrl="~/images/cal.gif" ImageAlign="Bottom"
                        runat="server" TabIndex="4" />

                    <cc1:CalendarExtender ID="Calendar1" PopupButtonID="imgPopup" runat="server" TargetControlID="txtFromDate"
                        Format="dd-MMM-yyyy">
                    </cc1:CalendarExtender>    
                              
                    
                    <!-- 
                    <asp:Label ID="Label5" runat="server" Text="(mm/dd/yyyy)"></asp:Label>
                    -->    
                </td>
                <td style="height: 8px; width: 49px;">&nbsp; &nbsp;</td>
                <td align="left" style="height: 8px; width: 108px;">On Tour</td>
                <td align="left" style="width: 125px; height: 8px;">
                    <asp:TextBox ID="txtTTour" runat="server" BackColor="#66FF99" BorderStyle="None" 
                        Height="21px" ReadOnly="True" Font-Size="Large" Width="67px" 
                        style="text-align:center">0</asp:TextBox>
                </td>
                <td style="height: 8px"></td>
            </tr>
            <tr>
                <td style="height: 9px"></td>
                <td align ="left" class="style2" style="width: 91px; height: 9px;">To Date</td>
                <td style="height: 9px">:</td>
                <td align ="left" class="style3" style="width: 189px; height: 9px;">
                    <asp:TextBox ID="txtToDate" runat="server" 
                        Style="z-index: 110; left: 180px; 
                            top: 166px; font-weight: normal; font-size: x-small; font-family: Arial;" 
                            Height="21px" Width="150px" TabIndex="3"></asp:TextBox>
                            &nbsp;   
                     <asp:ImageButton ID="imgPopup1" ImageUrl="~/Images/cal.gif" ImageAlign="Bottom"
                        runat="server" TabIndex="6" />
                    <cc1:CalendarExtender ID="CalendarExtender1" PopupButtonID="imgPopup1" runat="server" TargetControlID="txtToDate"
                        Format="dd-MMM-yyyy">
                    </cc1:CalendarExtender> 
                            
                </td>
                <td style="width: 49px; height: 9px;"></td>
                <td align="left" style="width: 108px; height: 9px;">Total Absent</td>
                <td align="left" style="width: 125px; height: 9px;">
                    <asp:TextBox ID="txtTAbs" runat="server" BackColor="#FF99CC" BorderStyle="None" 
                        Height="21px" ReadOnly="True" Font-Size="Large" Width="67px" 
                        style="text-align:center">0</asp:TextBox>
                </td>
                <td style="height: 9px"></td>
            </tr>

            <tr>
                <td style="height: 22px"></td>
                <td style="width: 91px; height: 19px;"></td>
                <td style="height: 19px"></td>
                <td style="width: 189px; height: 19px;"></td>
                <td style="width: 49px; height: 19px;"></td>
                <td align="left" style="height: 1px; width: 110px; color: #FF0000;">Late Comer</td>
                <td align="left" style="width: 125px; height: 19px;">
                    <asp:TextBox ID="txtTLate" runat="server" BackColor="Yellow" BorderStyle="None" 
                        Height="21px" ReadOnly="True" Font-Size="Large" Width="67px" 
                        style="text-align:center">0</asp:TextBox>
                </td>
            </tr>

            <tr>
                <td class="style1"></td>
                <td class="style2" style="width: 91px"></td>
                <td class="style4"></td>
                <td align="left" class="style3" style="width: 189px">
                    <asp:Button ID="btnSearch" CssClass="btn btn-primary" runat="server" 
                        Height="32px" Text="Search" 
                        Width="89px" onclick="btnSearch_Click" TabIndex="4" /> 
                </td>
                <td style="width: 49px"></td>
                <td style="width: 108px"></td>
                <td style="width: 125px"></td>
                <td></td>
            </tr>
            <tr>
                <td class="style5"></td>
                <td class="style6" style="width: 91px"></td>
                <td class="style7"></td>
                <td class="style8" style="width: 189px">
                    <asp:ScriptManager ID="ScriptManager1" runat="server">
                    </asp:ScriptManager>
                </td>
                <td class="style9" style="width: 49px"></td>
                <td class="style9" style="width: 108px"></td>
                <td style="width: 125px"></td>
                <td></td>
            </tr>
        </table>
    </div>
            
    <div>&nbsp;</div>
    
    <h4 class="col-sm-12 bg-primary" style="padding:5px">Report With Photo
        <asp:Label ID="lblDateTime" runat="server" Text="-"></asp:Label>
    </h4>
                
    <div>&nbsp;</div>

    <!-- GRID VIEW -->
    <div align="center">
        <asp:GridView ID="gvImages" CssClass="table" runat="server" AutoGenerateColumns="False"
        BackColor="White" BorderColor="#999999" BorderStyle="Double"
        CellPadding="2" Font-Names="Verdana"
        Font-Size="Small" GridLines="Vertical" Height="30%" Width="100%"
        >
        <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />        
            <HeaderStyle CssClass="bg-primary" />
            <AlternatingRowStyle BackColor="Gainsboro" />
            <Columns>
                <asp:BoundField HeaderText = "Job ID" DataField="EmpCod" />
                <asp:BoundField HeaderText = "Employee Name" DataField="EmpName" />
                <asp:BoundField HeaderText = "Designation" DataField="Desg" />
                <asp:BoundField HeaderText = "Date" DataField="Dt" />
                <asp:BoundField HeaderText = "In Time" DataField="EntryTime" />
                <asp:TemplateField HeaderText="In Image">
                    <ItemTemplate>
                        <asp:Image ID="Image1" runat="server" ImageUrl='<%# "ImageHandler.ashx?ImID="+ Eval("AID") %>' Height="100px" Width="100px"/>
                    </ItemTemplate>
                </asp:TemplateField>
                        
                <asp:BoundField HeaderText = "Out Time" DataField="ExitTime" />
                <asp:TemplateField HeaderText="Out Image">
                    <ItemTemplate>
                        <asp:Image ID="Image1" runat="server" ImageUrl='<%# "ImageHandler1.ashx?ImID="+ Eval("AID") %>' Height="100px" Width="100px"/>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
            
    <div>&nbsp;</div>
    
    <!-- FOR Absent List -->
    <div>&nbsp;<asp:Label ID="lblAbsentCaption" runat="server" Text="Absent List" 
            Font-Size="Medium" Font-Underline="True" ForeColor="Red" 
            Font-Names="Tahoma" Visible="False"></asp:Label>
    </div>

    <div>        
        <asp:DataGrid ID="gvAbsent" runat="server" BackColor="White" 
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

    <div>&nbsp;</div>

</asp:Content>

