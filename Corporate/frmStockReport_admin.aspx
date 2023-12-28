<%@ Page Language="C#" MasterPageFile="Admin.master" AutoEventWireup="true" CodeFile="frmStockReport_admin.aspx.cs" Inherits="FormsReport_Admin_frmStockReport_Admin" %>

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
            width: 429px;
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
                    style="background-image:url(../Images/header.jpg); height:30px; font-family: Arial; font-size: large; text-decoration: blink; color: #FFFFFF;">                        
                    Stock Report </td>
            </tr>
            <tr>
                <td class="style27"></td>
                <td class="style21">&nbsp;</td>
                <td class="style20"></td>
                <td class="style21">
                    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                </td>
                <td></td>
            </tr>
            
            <!-- From Date -->
            <tr>
                <td class="style28"></td>
                <td style="text-align: left; font-family: Arial; font-size: small; " 
                    class="style31">From Date</td>
                <td class="style30"> : </td>
                <td style="text-align: left" class="style31">
                <asp:TextBox ID="txtFrom" runat="server" Width="110px" TabIndex="1" 
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
                    class="style21">To Date</td>
                <td class="style20"> : </td>
                <td style="text-align: left" class="style21">
                    <asp:TextBox ID="txtToDate" runat="server" Width="110px" TabIndex="2" 
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
                <td class="style21"></td><td style="text-align: left" class="style20">
              
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
                <td class="style21"></td><td style="text-align: left" class="style20">
                    &nbsp;
                </td>
                <td class="style21"></td>
            </tr>
            <!-- ---------------------------------- -->
            
            <!-- Group Wise -->
            <tr>
                <td class="style27"></td>
                <td style="text-align: Left; font-family: Arial; " 
                    class="style21">
                    <asp:RadioButton ID="RadioButton1" runat="server" Text="Product Group Wise" 
                    GroupName="Sales" 
                    OnCheckedChanged="RadioButtonSales_CheckedChanged" 
                    AutoPostBack="true" />
                </td>
                <td class="style20"> &nbsp;</td>
                <td style="text-align: left" class="style21">
                    <asp:DropDownList ID="ddlGroup"
                        runat="server" AutoPostBack = "true"                        
                        class="chzn-select" BackColor="#F6F1DB"
                        Height="28px" Width="252px" TabIndex="6"
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
                    class="style21">
                    <asp:RadioButton ID="RadioButton2" runat="server" Text="Product Model Wise" 
                    GroupName="Sales" 
                    OnCheckedChanged="RadioButtonSales_CheckedChanged" 
                    AutoPostBack="true" />
                </td>
                <td class="style20"> &nbsp;</td>
                <td style="text-align: left" class="style21">
                    <asp:DropDownList ID="ddlModel" class="chzn-select"
                        runat="server" AutoPostBack = "true"                        
                        BackColor="#F6F1DB"
                        Height="28px" Width="252px" TabIndex="6"
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
                    class="style21">
                    <asp:RadioButton ID="RadioButton3" runat="server" Text="Product Distribution Plan" 
                    GroupName="Sales" 
                    OnCheckedChanged="RadioButtonSales_CheckedChanged" 
                    AutoPostBack="true" />
                </td>
                <td class="style20"> &nbsp;</td>
                <td style="text-align: left" class="style21">
                    <asp:DropDownList ID="ddlGroup1" class="chzn-select"
                        runat="server" AutoPostBack = "true"                        
                        BackColor="#F6F1DB"
                        Height="28px" Width="252px" TabIndex="6"
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
                <td class="style21"></td>
                <td class="style20">&nbsp;</td>
                <td class="style21"></td>
            </tr>

            <tr>
                <td class="style27"></td>
                <td  align="left" class="style21" 
                    style="font-family: Arial; font-size: large; color: #000080">
                    Entity Name</td>
                <td class="style20">&nbsp;</td>
                <td align="left" class="style21">
                    <asp:DropDownList ID="ddlEntity"
                        runat="server" AutoPostBack = "true"                        
                        BackColor="#F6F1DB"
                        Height="28px" Width="252px" TabIndex="6" 
                        ToolTip="Please Select CTP / Dealer ..." 
                        onselectedindexchanged="ddlEntity_SelectedIndexChanged" Visible="True">
                        
                    </asp:DropDownList>
                </td>

            </tr>

            <tr>
                <td class="style27"></td>
                <td class="style21"></td>
                <td class="style20">&nbsp;</td>
                <td class="style21"></td>
            </tr>

            <!-- VIEW -->
            <tr>
                <td class="style27"></td>
                <td style="text-align: left" class="style21">
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
                    <asp:TextBox ID="txtEID" runat="server" Visible="False" Width="53px"></asp:TextBox>
                </td>                
            </tr>
            
            <tr>
                <td class="style27"></td>
                <td class="style21">&nbsp;</td>
                <td class="style20">&nbsp;</td>
                <td class="style21"></td>
            </tr>
            
            <tr>
                <td colspan="5" align="center"
                    style="background-image:url(../Images/header.jpg); height:0.5px; font-family: Arial;">                        
                </td>
            </tr>

        </table>
                
        <div>
        <asp:Label ID="lblError" ForeColor="red" runat="server" Text=""></asp:Label>
        </div>
        
        <table width="100%" style="font-family: Tahoma; font-size: small">
                        
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
                        ShowFooter="true" width="80%">
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
   
   <script src="../js/jquery.min.js" type="text/javascript"></script>
    <script src="../js/chosen.jquery.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(".chzn-select").chosen(); $(".chzn-select-deselect").chosen({ allow_single_deselect: true }); 
    </script> 
    
</asp:Content>

