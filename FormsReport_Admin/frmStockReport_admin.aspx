<%@ Page Language="C#" MasterPageFile="~/Admin_Report.master" AutoEventWireup="true" CodeFile="frmStockReport_admin.aspx.cs" Inherits="FormsReport_Admin_frmStockReport_Admin" %>


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
                <td class="style21"></td>
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
               <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                        ErrorMessage="Not Valid." 
                        ControlToValidate="txtFrom"
                        ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                        style="font-size: xx-small; font-family: Arial, Helvetica, sans-serif" />
                
                </td>
                <td style="font-family: Arial; font-size: x-small; color: #990033" 
                    class="style32">(MM/dd/yyyy))</td>
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
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" 
                            ErrorMessage="Not Valid." 
                            ControlToValidate="txtToDate"
                            ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                            style="font-size: xx-small; font-family: Arial, Helvetica, sans-serif" />
                
                </td>
                <td style="font-family: Arial; font-size: x-small; color: #990033" 
                    class="style2">(MM/dd/yyyy))</td>
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
            
            
            <!-- Distribution Plan -->
            <tr>
                <td class="style27"></td>
                <td style="text-align: Left; font-family: Arial; " 
                    class="style13">
                    <asp:RadioButton ID="RadioButton3" runat="server" Text="Product Distribution Plan" 
                    GroupName="Sales" 
                    OnCheckedChanged="RadioButtonSales_CheckedChanged" 
                    AutoPostBack="true" />
                </td>
                <td class="style20"> :</td>
                <td style="text-align: left" class="style21">
                    <asp:DropDownList ID="ddlGroup1"
                        runat="server" AutoPostBack = "true"                        
                        BackColor="#F6F1DB"
                        Height="28px" Width="202px" TabIndex="6"
                        Visible="False" 
                        ToolTip="Please Select Product Group ...">
                        <asp:ListItem Text = "ALL" Value = ""></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
                       
            <tr>
                <td class="style27"></td>
                <td class="style33"></td>
                <td class="style20">&nbsp;</td>
                <td class="style21"></td>
            </tr>

            <tr>
                <td class="style27"></td>
                <td  align="right" class="style35" 
                    style="font-family: Arial; font-size: large; color: #000080">CTP / DEALER</td>
                <td class="style20">&nbsp;</td>
                <td align="left" class="style21">
                    <asp:DropDownList ID="ddlEntity"
                        runat="server" AutoPostBack = "true"                        
                        BackColor="#F6F1DB"
                        Height="28px" Width="202px" TabIndex="6" 
                        ToolTip="Please Select CTP / Dealer ..." 
                        onselectedindexchanged="ddlEntity_SelectedIndexChanged">
                        <asp:ListItem Text = "ALL" Value = ""></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>

            <tr>
                <td class="style27"></td>
                <td class="style35"></td>
                <td class="style20">&nbsp;</td>
                <td class="style21">
                    <asp:TextBox ID="txtEID" runat="server" Visible="False" Width="53px"></asp:TextBox>
                </td>
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

