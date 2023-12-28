<%@ Page Language="C#" MasterPageFile="Admin.master" AutoEventWireup="true" CodeFile="frmSalesReport_admin.aspx.cs" Inherits="FormsReport_Admin_frmSalesReport_Admin" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

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
            width: 53px;
        }
        .style28
        {
            width: 53px;
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
        .style34
        {
            width: 224px;
            height: 14px;
        }
        .style35
        {
            width: 224px;
        }
    </style>
    
    
    <link type="text/css" href="css/ui-lightness/jquery-ui-1.8.19.custom.css" rel="stylesheet" />
    <link href="css/grid.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="js1/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="js1/jquery-ui-1.8.19.custom.min.js"></script>

    <link type="text/css" href="css/ui-lightness/jquery-ui-1.8.19.custom.css" rel="stylesheet" />
    <script type="text/javascript" src="js/bootstrap-combobox.js"></script>
        
    <script language="JavaScript" src="../js/datetimepicker.js" type="text/javascript"> </script>
    
    <style type="text/css">
        .style19
        {
            width: 631px;
        }
        .grid
        {}
        .style20
        {
            width: 13px;
        }
        .style21
        {
            width: 429px;
        }
        .style22
        {
            width: 429px;
            height: 29px;
        }
        .style24
        {
            width: 156px;
            height: 29px;
        }
        .style25
        {
            width: 156px;
        }
        .style26
        {
            width: 52px;
        }
        .style27
        {
            width: 52px;
            height: 29px;
        }
        .style28
        {
            width: 52px;
            height: 36px;
        }
        .style29
        {
            width: 156px;
            height: 36px;
        }
        .style30
        {
            width: 13px;
            height: 36px;
        }
        .style32
        {
            height: 36px;
        }
    </style>
    
    <link type="text/css" href="css/ui-lightness/jquery-ui-1.8.19.custom.css" rel="stylesheet" />
    <link href="css/grid.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="js1/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="js1/jquery-ui-1.8.19.custom.min.js"></script>

    <link type="text/css" href="css/ui-lightness/jquery-ui-1.8.19.custom.css" rel="stylesheet" />
    <script type="text/javascript" src="js/bootstrap-combobox.js"></script>

        
    <style>
		a img{border: none;}
		ol li{list-style: decimal outside;}
		div#container{width: 780px;margin: 0 auto;padding: 1em 0;}
		div.side-by-side{width: 100%;margin-bottom: 1em;}
		div.side-by-side > div{float: left;width: 50%;}
		div.side-by-side > div > em{margin-bottom: 10px;display: block;}
		.clearfix:after{content: "\0020";display: block;height: 0;clear: both;overflow: hidden;visibility: hidden;}
		
	</style>
    <link rel="stylesheet" href="../Styles/chosen.css" />

    <script type="text/javascript">
        $(function () {
            $("#txtDate").datepicker();
            $("#txtLDDate").datepicker();
            $("#txtDODate").datepicker();
            $("#txtPIDate").datepicker();
        });        
    </script>
    
    <script type="text/javascript">
        $(document).ready(function () {
            $('.combobox').combobox();
        });
    </script>
    
    
    <style type="text/css">
        body
        {
            font-family: Arial;
            font-size: 10pt;
        }
        .Grid1
        {
            border: 1px solid #ccc;
            margin-bottom: -1px;
        }
        .Grid1 th
        {
            background-color: #F7F7F7;
            color: #333;
            font-weight: bold;
        }
        .Grid1 th, .Grid1 td
        {
            padding: 5px;
            border-color: #ccc;
        }
    </style>
    
    <style type="text/css">
        .cpHeader
        {
            color: white;
            background-color: #719DDB;
            font: bold 11px auto "Trebuchet MS", Verdana;
            font-size: 12px;
            cursor: pointer;
            width:100%;
            height:25px;
            padding: 4px;           
        }
        .cpBody
        {
            background-color: #DCE4F9;
            font: normal 11px auto Verdana, Arial;
            border: 1px gray;               
            width:100%;
            padding: 4px;
            padding-top: 7px;
        }      
    </style>
    
    <style type="text/css">
      .hiddencol
      {
        display: none;
      }
        .style33
        {
            width: 10px;
        }
    </style>

    <script>
        $(document).ready(function () {
            $('[data-toggle="tooltip"]').tooltip();
        });
    </script>

</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    
    <p>&nbsp;</p>
    
    <div align="center">
        
        <table width="100%" style="font-family: Tahoma; font-size: small">
            <tr>
                <td colspan="5" align="center"
                    style="background-image:url(../Images/header.jpg); height:40px; font-family: Arial; font-size: large; text-decoration: blink;">                        
                    Please select Report Type</td>
            </tr>
            <tr>
                <td class="style27">&nbsp;</td><td class="style35"></td><td class="style20"></td>
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
                    <cc1:calendarextender ID="Calendar1" PopupButtonID="imgPopup" runat="server" TargetControlID="txtFrom"
                        Format="MM/dd/yyyy">
                    </cc1:calendarextender>
                
                </td>
                <td style="font-family: Arial; font-size: x-small; color: #990033" 
                    class="style32">(MM/dd/yyyy))</td>
            </tr>

            
            <!-- To Date -->
            <tr>
                <td class="style27"></td>
                <td style="text-align: left; font-family: Arial; font-size: small; " 
                    class="style35">To Date</td>
                <td class="style20"> : </td>
                <td style="text-align: left" class="style21">
                    <asp:TextBox ID="txtToDate" runat="server" Width="97px" TabIndex="2" 
                            ToolTip="Please Enter To Date" MaxLength="10"></asp:TextBox> 
                    &nbsp;                                   
                    <asp:ImageButton ID="imgPopup1" ImageUrl="~/Images/cal.gif" ImageAlign="Bottom"
                        runat="server" TabIndex="1" />
                    <cc1:calendarextender ID="Calendarextender1" PopupButtonID="imgPopup1" runat="server" TargetControlID="txtToDate"
                        Format="MM/dd/yyyy">
                    </cc1:calendarextender>
                
                </td>
                <td style="font-family: Arial; font-size: x-small; color: #990033" 
                    class="style2">(MM/dd/yyyy))</td>
            </tr>
           
           
            <!-- Line Break -->
            <tr>
                <td class="style27"></td>
                <td class="style35"></td><td style="text-align: left" class="style20">
              
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
                <td class="style35"></td><td style="text-align: left" class="style20">
                    &nbsp;
                </td>
                <td class="style21"></td>
            </tr>
            <!-- ---------------------------------- -->

            
             <!-- Product Model -->
            <tr>
                <td class="style27"></td>
                <td style="text-align: Left; font-family: Arial; font-weight: 700; color: #003300;" 
                    class="style35">
                    <asp:RadioButton ID="RadioButton1" runat="server" Text="Challan Wise" 
                    GroupName="Sales" 
                    OnCheckedChanged="RadioButtonSales_CheckedChanged" 
                    AutoPostBack="true" />
                </td>
                <td class="style20"> &nbsp;</td>              
                <td style="text-align: left" class="style21" >                    
                    &nbsp;<asp:TextBox ID="txtCHNo" runat="server" Visible="False"></asp:TextBox>
                    </td>
                <td>
                    &nbsp;</td>
            </tr>

            <!-- Sales Summary (Challan Wise) -->
            <tr>
                <td class="style27"></td>
                <td style="text-align: Left; font-family: Arial; " 
                    class="style35">
                    <asp:RadioButton ID="RadioButton2" runat="server" Text="Sales Summary (Challan Wise)" 
                    GroupName="Sales" 
                    OnCheckedChanged="RadioButtonSales_CheckedChanged" 
                    AutoPostBack="true" />
                </td>
                <td class="style20"> &nbsp;</td>
                <td style="text-align: left" class="style21">
                    &nbsp;</td>
            </tr>
            
            <!-- Sales Summary (Product Wise) -->
            <tr>
                <td class="style27"></td>
                <td style="text-align: Left; font-family: Arial; " 
                    class="style35">
                    <asp:RadioButton ID="RadioButton6" runat="server" Text="Sales Summary (Product Wise)" 
                    GroupName="Sales" 
                    OnCheckedChanged="RadioButtonSales_CheckedChanged" 
                    AutoPostBack="true" Checked="True" />
                </td>
                <td class="style20"> &nbsp;</td>
                <td style="text-align: left" class="style21">
                    &nbsp;</td>
            </tr>

            <!-- Sales Summary (Product Category Wise) -->
            <tr>
                <td class="style27"></td>
                <td style="text-align: Left; font-family: Arial; " 
                    class="style35">
                    <asp:RadioButton ID="RadioButton7" runat="server" Text="Sales Summary (Category Wise)" 
                    GroupName="Sales" 
                    OnCheckedChanged="RadioButtonSales_CheckedChanged" 
                    AutoPostBack="true" />
                </td>
                <td class="style20"> &nbsp;</td>
                <td style="text-align: left" class="style21">
                    &nbsp;</td>
            </tr>

            <!-- Group Wise -->
            <tr>
                <td class="style27"></td>
                <td style="text-align: Left; font-family: Arial; " 
                    class="style35">
                    <asp:RadioButton ID="RadioButton3" runat="server" Text="Product Group Wise" 
                    GroupName="Sales" 
                    OnCheckedChanged="RadioButtonSales_CheckedChanged" 
                    AutoPostBack="true" />
                </td>
                <td class="style20"> &nbsp;</td>
                <td style="text-align: left" class="style21">
                    <asp:DropDownList ID="ddlGroup"
                        runat="server" AutoPostBack = "true"                        
                        BackColor="#F6F1DB"
                        Height="28px" Width="202px" TabIndex="6"
                        Visible="False" class="chzn-select"
                        ToolTip="Please Select Product Group ...">
                        
                    </asp:DropDownList>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
                       
             <!-- Remarks -->
            <tr>
                <td class="style27"></td>
                <td style="text-align: Left; font-family: Arial; " 
                    class="style35">
                    <asp:RadioButton ID="RadioButton4" runat="server" Text="Product Model Wise" 
                    GroupName="Sales" 
                    OnCheckedChanged="RadioButtonSales_CheckedChanged" 
                    AutoPostBack="true" />
                </td>
                <td class="style20"> &nbsp;</td>
                <td style="text-align: left" class="style21">
                    <asp:DropDownList ID="ddlModel"
                        runat="server" AutoPostBack = "true"                        
                        BackColor="#F6F1DB"
                        Height="28px" Width="202px" TabIndex="6"
                        Visible="False" class="chzn-select"
                        ToolTip="Please Select Product Model ...">
                        
                    </asp:DropDownList>
                </td>
            </tr>

            <!-- Product SL# Wise -->
            <tr>
                <td class="style27"></td>
                <td style="text-align: Left; font-family: Arial; " 
                    class="style35">
                    <asp:RadioButton ID="RadioButton5" runat="server" Text="Product Serial # Wise" 
                    GroupName="Sales" 
                    OnCheckedChanged="RadioButtonSales_CheckedChanged" 
                    AutoPostBack="true" />
                </td>
                <td class="style20"> &nbsp;</td>
                <td style="text-align: left" class="style21">
                    <asp:TextBox ID="txtSL" runat="server" Visible="False"></asp:TextBox>
                </td>
            </tr>

            <!-- Raw Data -->
            <tr>
                <td class="style27"></td>
                <td style="text-align: Left; font-family: Arial; " 
                    class="style35">
                    <asp:RadioButton ID="RadioButton8" runat="server" Text="Sales Raw Data" 
                    GroupName="Sales" 
                    OnCheckedChanged="RadioButtonSales_CheckedChanged" 
                    AutoPostBack="true" Visible="True" />
                </td>
                <td class="style20"> &nbsp;</td>
                <td style="text-align: left" class="style21">
                    &nbsp;</td>
            </tr>

            <tr>
                <td class="style27"></td>
                <td class="style35">
                    <asp:TextBox ID="txtEID" runat="server" Visible="False" Width="53px"></asp:TextBox>
                </td>
                <td class="style20">&nbsp;</td>
                <td class="style21" align="left">
                    <asp:DropDownList ID="ddlEntity"
                        runat="server" AutoPostBack = "true"                        
                        BackColor="#F6F1DB" class="chzn-select"
                        Height="28px" Width="202px" TabIndex="6" 
                        ToolTip="Please Select CTP / Dealer ..." 
                        onselectedindexchanged="ddlEntity_SelectedIndexChanged"
                        visible="false"
                        >
                        
                    </asp:DropDownList>
                </td>
            </tr>

            

            <tr>
                <td class="style27"></td>
                <td class="style35"></td>
                <td class="style20">&nbsp;</td>
                <td class="style21">
                    &nbsp;</td>
            </tr>

            <!-- Add to Data Grid -->
            <tr>
                <td class="style27"></td>
                <td style="text-align: left" class="style35">
                    &nbsp;
                    <asp:Button ID="btnAdd" runat="server" Text="View" OnClick="btnAdd_Click" 
                        Width="88px" CssClass="btn btn-primary"
                        Font-Size="Small"
                        ToolTip="Click here for Search condition wise ..." TabIndex="6"/>
                    &nbsp;
                    <asp:Button ID="btnClear" runat="server" Text="Clear" OnClick="btnClear_Click" 
                        Width="88px" CssClass="btn btn-primary"
                        Font-Size="Small"
                        ToolTip="Click here for all text & others ..." TabIndex="6"/>
                
                </td>
                <td class="style20"></td>
                <td style="text-align: left" class="style21">
                    &nbsp;</td>                
            </tr>
            
            <tr>
                <td class="style27"></td>
                <td class="style35"></td>
                <td class="style20">&nbsp;</td>
                <td class="style21"></td>
            </tr>
            
        </table>
                
        <div>
        <asp:Label ID="lblError" ForeColor="red" runat="server" Text=""></asp:Label>
        </div>
        
        <table width="100%" style="font-family: Tahoma; font-size: small">
            <tr>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td align="center">                        
                    <!-- Data Grid -->  
                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CellPadding="2"  
                        OnSelectedIndexChanged="GridView1_SelectedIndexChanged" Width="100%" 
                        AutoGenratedSelecteButton = "True" 
                        EmptyDataText = "No record found!"
                        ShowFooter="true" DataKeyNames="MRSRCode"
                        HeaderStyle-BackColor="#3AC0F2" HeaderStyle-ForeColor="White"
                        RowStyle-BackColor="#f2f9fc" AlternatingRowStyle-BackColor="White"
                        AlternatingRowStyle-ForeColor="#000"
                        OnRowDataBound="GridView1_RowDataBound"                        
                        >
                        <Columns>
                            <asp:TemplateField Visible="true">
                                <ItemTemplate><asp:LinkButton runat="server" CommandName="select" ID="lnk_Select" Text="Details" /></ItemTemplate>
                            </asp:TemplateField>
                                                        
                            <asp:BoundField DataField="MRSRCode" HeaderText="Challan #" />
                            <asp:BoundField DataField="TDate" HeaderText="Date" />
                            <asp:BoundField DataField="NetSalesAmnt" HeaderText="Amount (Tk.)" />
                            <asp:BoundField DataField="Entity" HeaderText="Entity" />

                            <asp:TemplateField HeaderText="">
                                <ItemTemplate>
		                            <asp:LinkButton ID="lnkPrint" Text="Print"  OnClick="lnkPrint_Click" runat="server"
                                        OnClientClick="return confirm('Do you want to Print this Invoice ?');" 
                                        >
                                    </asp:LinkButton>
	                            </ItemTemplate>
                            </asp:TemplateField>

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
            <tr  align="center">
                <td> 
                    <asp:Label ID="lb1" runat="server" Text="Selected Challan # : " Font-Bold="True" Width="200px"></asp:Label> 
                    <asp:Label ID="lbl_id" runat="server" ForeColor="#CC3300"></asp:Label>                
                <br />
                </td>                
            </tr>
            <tr>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td align="center">
                    <asp:GridView ID="GridView2" runat="server" CellPadding="2"  
                        Font-Names="Verdana" Font-Size="Small" ForeColor="#333333"
                        EmptyDataText = "No record found!"
                        OnRowDataBound="GridView2_RowDataBound" Width="100%"
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
                       
                    <asp:GridView ID="GridView3" runat="server" CellPadding="2"  
                        Font-Names="Verdana" Font-Size="Small" ForeColor="#333333"
                        EmptyDataText = "No record found!"  Width="100%"                   
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
                    <asp:Button ID="btnExport" runat="server" Text="Export To Excel" 
                        OnClick = "ExportToExcel" Height="28px" Visible="False" Width="161px" 
                        BackColor="#000099" Font-Size="X-Small" ForeColor="Aqua" />                
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

   </div>

   <script src="../js/jquery.min.js" type="text/javascript"></script>
    <script src="../js/chosen.jquery.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(".chzn-select").chosen(); $(".chzn-select-deselect").chosen({ allow_single_deselect: true }); 
    </script> 


</asp:Content>

