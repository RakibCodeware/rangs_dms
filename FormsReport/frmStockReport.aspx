<%@ Page Language="C#" MasterPageFile="~/CTP_Admin.master" AutoEventWireup="true" CodeFile="frmStockReport.aspx.cs" Inherits="FormsReport_frmStockReport" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
        
    <style type="text/css">
        .grid
        {}
        .style20
        {
            width: 13px;
        }
        .style21
        {
            width: 164px;
        }
        .style27
        {
            width: 145px;
        }
        .style28
        {
            width: 145px;
            height: 14px;
        }
        .style30
        {
            width: 13px;
            height: 14px;
        }
        .style31
        {
            width: 164px;
            height: 14px;
        }
        .style32
        {
            width: 93px;
            height: 14px;
        }
        .style33
        {
            width: 163px;
        }
        .style34
        {
            width: 163px;
            height: 14px;
        }
    </style>
        
    <script language="JavaScript" src="../js/datetimepicker.js" type="text/javascript"> </script>
    
    <link type="text/css" href="../css/ui-lightness/jquery-ui-1.8.19.custom.css" rel="stylesheet" />
    <script type="text/javascript" src="../js/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="../js/jquery-ui-1.8.19.custom.min.js"></script>

    <script type="text/javascript">
        $(function () {
            $("#txtDate").datepicker();
            $("#txtFrom").datepicker();
            $("#txtToDate").datepicker();
        });        
    </script>
           
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        Welcome to Stock Report
    </h2>
    <p></p>
    
    <div align="center">
        
        <table style="border: 1px groove #008000" width="810px">
            <tr>
                <td colspan="5" align="center"
                    style="background-image:url(../Images/header.jpg); height:40px; font-family: Arial; font-size: large; text-decoration: blink;">                        
                    Please select Report Type</td>
            </tr>
            <tr>
                <td class="style27"></td><td class="style33"></td><td class="style20"></td>
                <td class="style21">
                   <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                </td>
            </tr>
            
            <!-- From Date -->
            <tr>
                <td class="style28"></td>
                <td style="text-align: left; font-family: Arial; font-size: small; " 
                    class="style34">From Date</td>
                <td class="style30"> : </td>
                <td style="text-align: left" class="style31">
                <asp:TextBox ID="txtFrom" runat="server" Width="97px" TabIndex="1" 
                        ToolTip="Please Enter From Date" MaxLength="10"></asp:TextBox> 
                    &nbsp;
                    <asp:ImageButton ID="imgPopup" ImageUrl="~/Images/cal.gif" ImageAlign="Bottom"
                        runat="server" TabIndex="1" />
                    <cc1:CalendarExtender ID="Calendar1" PopupButtonID="imgPopup" runat="server" TargetControlID="txtFrom"
                        Format="MM/dd/yyyy">
                    </cc1:CalendarExtender>       
                </td>
                <td style="font-family: Arial; font-size: x-small; color: #990033" 
                    class="style32">&nbsp;</td>
            </tr>

            
            <!-- To Date -->
            <tr>
                <td class="style27"></td>
                <td style="text-align: left; font-family: Arial; font-size: small; " 
                    class="style33">To Date</td>
                <td class="style20"> : </td>
                <td style="text-align: left" class="style21">
                    <asp:TextBox ID="txtToDate" runat="server" Width="97px" TabIndex="2" 
                            ToolTip="Please Enter To Date" MaxLength="10"></asp:TextBox> 
                    &nbsp;
                    <asp:ImageButton ID="imgPopup1" ImageUrl="~/Images/cal.gif" ImageAlign="Bottom"
                        runat="server" TabIndex="1" />
                    <cc1:CalendarExtender ID="CalendarExtender1" PopupButtonID="imgPopup1" runat="server" TargetControlID="txtToDate"
                        Format="MM/dd/yyyy">
                    </cc1:CalendarExtender>                
                </td>
                <td style="font-family: Arial; font-size: x-small; color: #990033" 
                    class="style2">&nbsp;</td>
            </tr>
           
           
            <!-- Line Break -->
            <tr>
                <td class="style27"></td>
                <td class="style33"></td><td style="text-align: left" class="style20">
              
                &nbsp;</td>
                <td class="style21"></td>
            </tr>
            <tr>
                <td colspan="5" align="center"
                    style="background-image:url(../Images/header.jpg); height:0.5px; font-family: Arial;">                        
                </td>
            </tr>
            <tr>
                <td class="style27"></td>
                <td class="style33"></td><td style="text-align: left" class="style20">
                    &nbsp;
                </td>
                <td class="style21"></td>
            </tr>
            <!-- ---------------------------------- -->
            
            <!-- Group Wise -->
            <tr>
                <td class="style27"></td>
                <td style="text-align: Left; font-family: Arial; " 
                    class="style33">
                    <asp:RadioButton ID="RadioButton1" runat="server" Text="Product Group Wise" 
                    GroupName="Sales" 
                    OnCheckedChanged="RadioButtonSales_CheckedChanged" 
                    AutoPostBack="true" />
                </td>
                <td class="style20"> :</td>
                <td style="text-align: left" class="style21">
                    <asp:DropDownList ID="ddlGroup"
                        runat="server" AutoPostBack = "true"                        
                        CssClass="ddl" BackColor="#F6F1DB"
                        Height="28px" Width="202px" TabIndex="6"
                        Visible="False" 
                        ToolTip="Please Select Product Group ...">
                        <asp:ListItem Text = "--Select Group--" Value = ""></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
                       
             <!-- Model Wise -->
            <tr>
                <td class="style27"></td>
                <td style="text-align: Left; font-family: Arial; " 
                    class="style33">
                    <asp:RadioButton ID="RadioButton2" runat="server" Text="Product Model Wise" 
                    GroupName="Sales" 
                    OnCheckedChanged="RadioButtonSales_CheckedChanged" 
                    AutoPostBack="true" />
                </td>
                <td class="style20"> :</td>
                <td style="text-align: left" class="style21">
                    <asp:DropDownList ID="ddlModel"
                        runat="server" AutoPostBack = "true"                        
                        CssClass="ddl" BackColor="#F6F1DB"
                        Height="28px" Width="202px" TabIndex="6"
                        Visible="False" 
                        ToolTip="Please Select Product Model ...">
                        <asp:ListItem Text = "--Select Model--" Value = ""></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
                       
            <tr>
                <td class="style27"></td>
                <td class="style33"></td>
                <td class="style20">&nbsp;</td>
                <td class="style21"></td>
            </tr>

            <!-- Add to Data Grid -->
            <tr>
                <td class="style27"></td>
                <td style="text-align: left" class="style35">
                    &nbsp;
                    <asp:Button ID="btnAdd" runat="server" Text="View" OnClick="btnAdd_Click" 
                        Width="70px" Height="25px" 
                        Font-Size="Small"
                        ToolTip="Click here for Search condition wise ..." TabIndex="6"/>
                    &nbsp;
                    <asp:Button ID="btnClear" runat="server" Text="Clear" OnClick="btnClear_Click" 
                        Width="70px" Height="25px" 
                        Font-Size="Small"
                        ToolTip="Click here for all text & others ..." TabIndex="6"/>
                
                </td>
                <td class="style20"></td>
                <td style="text-align: left" class="style21">
                    &nbsp;</td>                
            </tr>
            
            <tr>
                <td class="style27"></td>
                <td class="style33"></td>
                <td class="style20">&nbsp;</td>
                <td class="style21"></td>
            </tr>
            
        </table>
                
        <div>
        <asp:Label ID="lblError" ForeColor="red" runat="server" Text=""></asp:Label>
        </div>
        
        <table style="border: 1px groove #008000;" width="810px">
                        
            <tr>
                <td colspan="1" align="center">
                    
                </td>
            </tr>
            <tr align="center">
                <td> 
                    <asp:Label ID="lbl_id" runat="server" ForeColor="#CC3300"></asp:Label>                
                <br />
                </td>                
            </tr>
            <tr>
                <td>&nbsp;</td>
            </tr>
            <tr align="center">
                <td>
                    <asp:GridView ID="GridView2" runat="server" CellPadding="2"  
                        Font-Names="Verdana" Font-Size="Small" ForeColor="#333333"
                        EmptyDataText = "No record found!"
                        OnRowDataBound="GridView2_RowDataBound"
                        ShowFooter="true" >
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                        <EditRowStyle BackColor="#999999" />
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                        <SelectedRowStyle BackColor="#E2DED6" ForeColor="#333333" />
                        <SortedAscendingCellStyle BackColor="#E9E7E2" />
                        <SortedAscendingHeaderStyle BackColor="#506C8C" />
                        <SortedDescendingCellStyle BackColor="#FFFDF8" />
                        <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                        <FooterStyle BackColor="#61A6F8" ForeColor="black" />
                    </asp:GridView> 
                       
                </td>
            </tr> 
                                    
            <tr>
                <td> &nbsp;</td>
            </tr>
            
            <tr>
                <td>&nbsp;</td>
            </tr>
        </table>
        
        <p></p>

   </div>

</asp:Content>

