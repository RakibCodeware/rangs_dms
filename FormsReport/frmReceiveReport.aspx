<%@ Page Language="C#" MasterPageFile="~/CTP_Admin.master" AutoEventWireup="true" CodeFile="frmReceiveReport.aspx.cs" Inherits="FormsReport_frmReceiveReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %> 
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

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
        Receive Report
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

            
             <!-- Product Model -->
            <tr>
                <td class="style27"></td>
                <td style="text-align: Left; font-family: Arial; font-weight: 700; color: ##003300;" 
                    class="style33">
                    <asp:RadioButton ID="RadioButton1" runat="server" Text="Challan Wise" 
                    GroupName="Sales" 
                    OnCheckedChanged="RadioButtonSales_CheckedChanged" 
                    AutoPostBack="true" />
                </td>
                <td class="style20"> : </td>              
                <td style="text-align: left" class="style21" >                    
                    &nbsp;<asp:TextBox ID="txtCHNo" runat="server" Visible="False"></asp:TextBox>
                    </td>
                <td>
                    &nbsp;</td>
            </tr>

            <!-- Product Description -->
            <tr>
                <td class="style27"></td>
                <td style="text-align: Left; font-family: Arial; " 
                    class="style33">
                    <asp:RadioButton ID="RadioButton2" runat="server" Text="Receive Summary" 
                    GroupName="Sales" 
                    OnCheckedChanged="RadioButtonSales_CheckedChanged" 
                    AutoPostBack="true" />
                </td>
                <td class="style20"> &nbsp;</td>
                <td style="text-align: left" class="style21">
                    &nbsp;</td>
            </tr>
            
            

            <!-- Quantity -->
            <tr>
                <td class="style27"></td>
                <td style="text-align: Left; font-family: Arial; " 
                    class="style33">
                    <asp:RadioButton ID="RadioButton3" runat="server" Text="Product Group Wise" 
                    GroupName="Sales" 
                    OnCheckedChanged="RadioButtonSales_CheckedChanged" 
                    AutoPostBack="true" />
                </td>
                <td class="style20"> :</td>
                <td style="text-align: left" class="style21">
                    <asp:DropDownList ID="ddlGroup"
                        runat="server" AutoPostBack = "true"                        
                        BackColor="#F6F1DB"
                        Height="28px" Width="202px" TabIndex="6"
                        Visible="False" 
                        ToolTip="Please Select Product Model ...">
                        <asp:ListItem Text = "--Select Group--" Value = ""></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
                       
             <!-- Remarks -->
            <tr>
                <td class="style27"></td>
                <td style="text-align: Left; font-family: Arial; " 
                    class="style33">
                    <asp:RadioButton ID="RadioButton4" runat="server" Text="Product Model Wise" 
                    GroupName="Sales" 
                    OnCheckedChanged="RadioButtonSales_CheckedChanged" 
                    AutoPostBack="true" />
                </td>
                <td class="style20"> :</td>
                <td style="text-align: left" class="style21">
                    <asp:DropDownList ID="ddlModel"
                        runat="server" AutoPostBack = "true"                        
                        BackColor="#F6F1DB"
                        Height="28px" Width="202px" TabIndex="6"
                        Visible="False" 
                        ToolTip="Please Select Product Model ...">
                        <asp:ListItem Text = "--Select Model--" Value = ""></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>

            <!-- Product Description -->
            <tr>
                <td class="style27"></td>
                <td style="text-align: Left; font-family: Arial; " 
                    class="style33">
                    <asp:RadioButton ID="RadioButton5" runat="server" Text="Product Serial # Wise" 
                    GroupName="Sales" 
                    OnCheckedChanged="RadioButtonSales_CheckedChanged" 
                    AutoPostBack="true" />
                </td>
                <td class="style20"> : </td>
                <td style="text-align: left" class="style21">
                    <asp:TextBox ID="txtSL" runat="server" Visible="False"></asp:TextBox>
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
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td align="center">                        
                    <!-- Data Grid -->  
                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CellPadding="2"  
                        OnSelectedIndexChanged="GridView1_SelectedIndexChanged" Width="790px" 
                        AutoGenratedSelecteButton = "True" 
                        EmptyDataText = "No record found!"
                        ShowFooter="true"
                        HeaderStyle-BackColor="#3AC0F2" HeaderStyle-ForeColor="White"
                        RowStyle-BackColor="#f2f9fc" AlternatingRowStyle-BackColor="White"
                        AlternatingRowStyle-ForeColor="#000"                        
                        >
                        <Columns>
                            <asp:TemplateField Visible="true">
                                <ItemTemplate><asp:LinkButton runat="server" CommandName="select" ID="lnk_Select" Text="Details" /></ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="MRSRCode" HeaderText="Challan #" HeaderStyle-Width="15%" />
                            <asp:BoundField DataField="TDate" HeaderText="Date" HeaderStyle-Width="15%" />
                            <asp:BoundField DataField="Remarks" HeaderText="Remarks" HeaderStyle-Width="60%" />
                        </Columns>
                        <FooterStyle BackColor="#d5d5d5" ForeColor="black" />
                    </asp:GridView>

                    <asp:GridView ID="GridView3" runat="server" AutoGenerateColumns="False" 
                        CellPadding="2"  
                        Width="790px" 
                        AutoGenratedSelecteButton = "True" 
                        EmptyDataText = "No record found!"
                        ShowFooter="true"
                        HeaderStyle-BackColor="#3AC0F2" HeaderStyle-ForeColor="White"
                        RowStyle-BackColor="#f2f9fc" AlternatingRowStyle-BackColor="White"
                        AlternatingRowStyle-ForeColor="#000"  
                        OnRowDataBound="GridView3_RowDataBound"                      
                        >                                             
                        <Columns>
                            <asp:TemplateField HeaderText="Record Number">
                                 <ItemTemplate>
                                       <%# Container.DataItemIndex + 1 %>
                                 </ItemTemplate>
                             </asp:TemplateField>  
                            <asp:BoundField DataField="Model" HeaderText="Product Model" HeaderStyle-Width="30%" />
                            <asp:BoundField DataField="ProdDesc" HeaderText="Product Description" HeaderStyle-Width="40%" />
                            <asp:BoundField DataField="Qty" HeaderText="Receive Qty" HeaderStyle-Width="20%" />
                        </Columns>
                        <FooterStyle BackColor="#d5d5d5" ForeColor="black" />
                    </asp:GridView>
                    <br />
                </td>
            </tr>
            <!-- 
            <tr>
                <td colspan="1" align="center"
                    style="background-image:url(../Images/header.jpg); height:0.5px; font-family: Arial; ">                        
                </td>
            </tr>
            -->

            <tr align="center">
                <td> 
                    <asp:Label ID="lb1" runat="server" Text="Selected Challan # : " Font-Bold="True" Width="200px"></asp:Label> 
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
                        <Columns>
                            <asp:TemplateField HeaderText="Record #">
                                 <ItemTemplate>
                                       <%# Container.DataItemIndex + 1 %>
                                 </ItemTemplate>
                             </asp:TemplateField>
                        </Columns>
                    </asp:GridView> 
                       
                </td>
            </tr> 
            <tr>
                <td>                    
                                    
                </td>
            </tr>
            
            <tr>
                <td> &nbsp;</td>
            </tr>
            <tr>
                <td align="center"> .
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
            </tr>
        </table>
        
        <p>&nbsp;</p>
        <p>&nbsp;</p>

   </div>

</asp:Content>






