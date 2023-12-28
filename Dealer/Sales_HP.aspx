<%@ Page Language="C#" MasterPageFile="Admin.master" AutoEventWireup="true" 
CodeFile="Sales_HP.aspx.cs" Inherits="Sales_HP" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %> 
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
       
    <!--
    <link type="text/css" href="../css/ui-lightness/jquery-ui-1.8.19.custom.css" rel="stylesheet" />
    <script type="text/javascript" src="../js/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="../js/jquery-ui-1.8.19.custom.min.js"></script>
    -->
    
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
            $("#txtToDate").datepicker();
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
            background-color: #0E6655;
            font: bold 11px auto "Trebuchet MS", Verdana;
            font-size: 12px;
            cursor: pointer;
            width:100%;
            height:25px;
            padding: 4px;           
        }
        .cpBody
        {
            background-color: #A2D9CE;
            font: normal 11px auto Verdana, Arial;
            border: 1px gray;               
            width:100%;
            padding: 4px;
            padding-top: 7px;
        }      
        .style1
        {
            width: 2%;
        }
        .style2
        {
            width: 15%;
        }
        .style3
        {
            width: 3%;
        }
        .style4
        {
            width: 15%;
        }
        .style5
        {
            width: 10%;
        }
        .style6
        {
            width: 15%;
        }
        .style7
        {
            width: 3%;
        }
        .style8
        {
            width: 15%;
        }
        .style9
        {
            width: 2%;
        }
    </style>
                  
    
    <script>
        $(document).ready(function () {
            $('[data-toggle="tooltip"]').tooltip();
        });
    </script>
    
    <script type="text/javascript">
        jQuery(function ($) {
            $("#DOB").mask("9999-99-99");
        });
    </script>


    <link type="text/css" href="css/ui-lightness/jquery-ui-1.8.19.custom.css" rel="stylesheet" />
    <script type="text/javascript" src="js/bootstrap-combobox.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            $('.combobox').combobox();
        });
    </script>
        
        
    <script language="JavaScript" src="../js/datetimepicker.js" type="text/javascript"> </script>
            
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    
    <h2 class="col-sm-12 bg-primary" style="padding:5px; font-family: Tahoma;" align="center"> Hire Purchase Sales Entry Form ...</h2>
        
    <p>
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>                
    </p>

    <div style="float:left; padding-top:10px; width:100%;">
    <!-- MASTER DATA -->
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Panel ID="pHeader0" runat="server" CssClass="cpHeader">
                    <asp:Label ID="lblText0" runat="server" >Invoice Information</asp:Label>
                </asp:Panel>
                
                <asp:Panel ID="pBody0" runat="server" CssClass="cpBody">
                <div align="center">
        
                    <table width="100%" style="font-family: Tahoma; font-size: small">
                    
                        <tr>
                            <td width="2%"></td>
                            <td width="15%"></td>
                            <td width="3%"></td>
                            <td class="style4"></td>
                            <td width="10%"></td>
                            <td width="15%"></td>
                            <td width="3%"></td>
                            <td width="15%"></td>
                            <td width="2%"></td>                               
                        </tr>
            
                        <!-- Challan No. -->
                        <tr>
                            <td ></td>
                            <td style="text-align: Left; font-family: Tahoma; font-weight: 700; color: #990000; font-size: large;"  
                                >Invoice/Bill #</td>
                            <td > : </td>
                            <td style="text-align: left" >
                                <asp:TextBox ID="txtCHNo" runat="server" Width="200px" 
                                    style="font-weight: 700" ToolTip="Please Enter Challan Number" MaxLength="15" 
                                        BackColor="#66FFFF" BorderStyle="None" Font-Bold="True"
                                        ReadOnly="True" CssClass="form-control">
                                </asp:TextBox>
                            </td>
                
                
                            <!-- Challan Date -->
                            <td style="text-align: left;" >
                                 <asp:RequiredFieldValidator 
                                 id="RequiredFieldValidator1" runat="server" 
                                 ErrorMessage="( * )"                    
                                 ControlToValidate="txtCHNo" ForeColor="Red"></asp:RequiredFieldValidator>
                            </td>
                            <td style="text-align: left; font-family: Arial; font-size: small; " 
                                >Bill Date</td>
                            <td > : </td>
                            <td style="text-align: left; color: #FF0000;" 
                                >
                                <asp:TextBox ID="txtDate" runat="server" Width="100px" Height="25px" TabIndex="0" 
                                        ToolTip="Please Enter Challan Date" MaxLength="10">                                                 
                                </asp:TextBox> 
                
                                &nbsp; 
                                <asp:ImageButton ID="imgPopup" ImageUrl="~/images/cal.gif" ImageAlign="Bottom"
                                    runat="server" TabIndex="4" Height="16px" Width="17px" />
                                <cc1:CalendarExtender ID="Calendar1" PopupButtonID="imgPopup" runat="server" TargetControlID="txtDate"
                                    Format="MM/dd/yyyy">
                                </cc1:CalendarExtender>      
                                                                        
                                <br />
                                Date Format : (MM/dd/yyyy)
                                              
                            </td>

                            <td style="font-family: Arial; font-size: xx-small; color: #FF0000" 
                                ></td>
                
                        </tr>
            
                        <!-- SEARCH BUTTON -->
                        <tr>
                            <td ></td>
                            <td >
                                <asp:TextBox ID="txtMRSR" runat="server" Enabled="False" Font-Size="Smaller" 
                                    Width="46px" Visible="False"></asp:TextBox>
                            </td>
                            <td ></td>
                            <td >                   
                            </td>
                            <td ></td>
                            <td ></td>
                            <td ></td>
                            <td style="text-align: left" class="style51">
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" 
                                        ErrorMessage="Not Valid." 
                                        ControlToValidate="txtDate"
                                        ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                                        style="font-size: xx-small; font-family: Arial, Helvetica, sans-serif" />
                                                   
                            </td>                
                        </tr>

            
                        <tr>
                            <td ></td>
                            <td align="left">
                                Customer Contact # 
                            </td>
                            <td >:</td>
                            <td >
                   
                                <asp:TextBox ID="txtCustContact" runat="server"
                                    Width="200px" placeholder="Enter Customer Mobile #"
                                    MaxLength="11" TabIndex="1" 
                                    ToolTip="Please Enter Customer Contact Number"                        
                                    onkeypress="return numeric_only(event)"
                                    CssClass="form-control"
                                    >
                                 </asp:TextBox>
                                                     
                            </td>
                            <td >                                         
                                <asp:ImageButton ID="btnCustSearch" runat="server" Height="18px" 
                                    ImageUrl="~/Images/search.png" Width="18px" 
                                    data-toggle="tooltip" title="Click here for Search Customer ..."
                                    onclick="btnCustSearch_Click" />                                        
                            </td>
                            <td align="left">City</td>
                            <td >:</td>
                            <td style="text-align: left" >
                                <asp:DropDownList ID="ddlCity" runat="server" 
                                    Width="200px" CssClass="form-control" TabIndex="4">
                                </asp:DropDownList>
                            </td>                
                        </tr>

           
                        <!-- Customer Name -->
                        <tr>
                            <td ></td>
                            <td style="text-align: Left; font-family: Arial; " 
                                >Customer Name </td>
                            <td > : </td>        
                            <td style="text-align: left" class="style6">
                                <asp:TextBox ID="txtCustName" runat="server" Width="200px" 
                                    TabIndex="2" ToolTip="Please Enter Customer Name" 
                                    MaxLength="50" CssClass="form-control"
                                    placeholder="Enter Customer Name"
                                    >
                                </asp:TextBox></td>
                
                            <td align="left" >
                                &nbsp;</td>

                            <!-- Customer Contact # -->                
                            <td style="text-align: Left; font-family: Arial; " 
                                >Date of Birth</td>
                            <td > : </td>
                            <td style="text-align: left" >
                                <asp:TextBox ID="txtDOB" runat="server" Width="100px" TabIndex="6" 
                                        ToolTip="Please Enter Customer Date of Birth" MaxLength="10"
                                        CssClass="" Height="25px"
                                        placeholder="MM/dd/yyyy"
                                        >
                                </asp:TextBox> 
                
                                &nbsp; 
                                <asp:ImageButton ID="ImageButton1" ImageUrl="~/images/cal.gif" ImageAlign="Bottom"
                                    runat="server" Height="18px" Width="17px" />
                                <cc1:CalendarExtender ID="CalendarExtender1" PopupButtonID="ImageButton1" runat="server" TargetControlID="txtDOB"
                                    Format="MM/dd/yyyy">
                                </cc1:CalendarExtender>
                            </td>
                    
                            <td>
                                &nbsp;</td>
                        </tr>

            
                        <!-- Customer Address -->
                        <tr>
                            <td ></td>
                            <td style="text-align: Left; font-family: Arial; " 
                                >Customer Address </td>
                            <td > : </td>
                            <td style="text-align: left" class="style6">
                            <asp:TextBox ID="txtCustAdd" runat="server" Width="200px" 
                                TabIndex="3" ToolTip="Enter Customer Address" 
                                MaxLength="50" CssClass="form-control"
                                placeholder="Enter Customer Address"
                                >
                            </asp:TextBox></td>
            
                            <!-- Customer Sex -->
                            <td ></td>
                            <td style="text-align: Left; font-family: Arial; " 
                                >Sex </td>
                            <td > : </td>
                            <td style="text-align: left">
                                <asp:RadioButtonList ID="optSex" runat="server" 
                                    RepeatDirection="Horizontal">
                                    <asp:ListItem Selected="True">Male</asp:ListItem>
                                    <asp:ListItem>Female</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                
                            <td ></td>
                        </tr>
                        
                        <!-- Customer Profession -->
                        <tr>
                            <td ></td>
                            <td style="text-align: Left; font-family: Arial; " 
                                >Customer Profession </td>
                            <td > : </td>
                            <td style="text-align: left" >
                                <asp:RadioButtonList ID="optPrefession" runat="server" 
                                    RepeatDirection="Horizontal" TabIndex="6">
                                    <asp:ListItem>Business</asp:ListItem>
                                    <asp:ListItem>Service</asp:ListItem>
                                    <asp:ListItem>Others</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
            
                            <!-- Customer Organization -->
                            <td ></td>
                            <td style="text-align: Left; font-family: Arial; " 
                                >Customer Organization </td>
                            <td > : </td>
                            <td style="text-align: left" >
                                <asp:TextBox ID="txtOrg"  runat="server"                        
                                    Width="200px" 
                                    TabIndex="7" placeholder="Enter Organization"
                                    CssClass="form-control"
                                    ToolTip="Enter Customer Organization ..." MaxLength="50"></asp:TextBox></td>
                
                            <td ></td>
                        </tr>


                        <!-- Customer Email -->
                        <tr>
                            <td ></td>
                            <td style="text-align: Left; font-family: Arial; " 
                                >Customer Email </td>
                            <td > : </td>
                            <td style="text-align: left" >
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" 
                                    MaxLength="50" TabIndex="8" 
                                    placeholder="Enter Customer Email"
                                    ToolTip="Enter Customer Email Address ..." Width="200px"></asp:TextBox>
                            </td>
            
                            <!-- Designation -->
                            <td ></td>
                            <td style="text-align: Left; font-family: Arial; " 
                                >Designation </td>
                            <td > : </td>
                            <td style="text-align: left">
                                <asp:TextBox ID="txtDesg"  runat="server"                        
                                    Width="200px" 
                                    TabIndex="9" placeholder="Enter Designation"
                                    CssClass="form-control"
                                    ToolTip="Enter Customer Designation..." MaxLength="50"></asp:TextBox></td>
                
                            <td ></td>
                        </tr>


                        <!-- Customer Identification Type -->
                        <tr>
                            <td ></td>
                            <td style="text-align: Left; font-family: Arial; "
                                >ID Type </td>
                            <td > : </td>
                            <td style="text-align: left" >
                                <asp:DropDownList ID="ddlIDType" runat="server"
                                    Width="200px" CssClass="form-control" AutoPostBack="True" TabIndex="10" 
                                    onselectedindexchanged="ddlIDType_SelectedIndexChanged">
                                    <asp:ListItem>NID</asp:ListItem>
                                    <asp:ListItem>Passport</asp:ListItem>
                                    <asp:ListItem>Driving License</asp:ListItem>
                                </asp:DropDownList>
                            </td>
            
                            <!-- Designation -->
                            <td ></td>
                            <td style="text-align: Left; font-family: Arial; " 
                                >
                                <asp:Label ID="lblNIDNo" runat="server" Text="NID #"></asp:Label>
                            </td>
                            <td > : </td>
                            <td style="text-align: left" >
                                <asp:TextBox ID="txtNID"  runat="server"                        
                                    Width="200px" placeholder="Enter Identity Number"
                                    TabIndex="20" 
                                    CssClass="form-control"
                                    ToolTip="Enter Customer Designation..." MaxLength="50">
                                 </asp:TextBox>
                             </td>
                
                            <td ></td>
                        </tr>


            
                        <tr>
                            <td></td>
                            <td ></td>
                            <td></td>
                            <td >
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" 
                                    ControlToValidate="txtEmail" ErrorMessage="Invalid Email Address" 
                                    Font-Size="Smaller" ForeColor="Red" SetFocusOnError="True" 
                                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">
                                </asp:RegularExpressionValidator>
                            </td>
                            <td ></td>
                            <td ></td>
                            <td></td>                
                            <td >                    
                                &nbsp;</td>
                            <td ></td>
                        </tr>
                      
                    
                    </table>

                </div>

            </asp:Panel>
 
            <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender2" runat="server" TargetControlID="pBody0" CollapseControlID="pHeader0" ExpandControlID="pHeader0"
                Collapsed="false" TextLabelID="lblText" CollapsedText="Click to Show Master Information ..." ExpandedText="Click to Hide Master Information ..."
                CollapsedSize="0">
            </cc1:CollapsiblePanelExtender>
        </ContentTemplate>                     

    </asp:UpdatePanel>   
    </div>
        
    <!-- END MASTER DATA-->


    <div style="float:left; padding-top:10px; width:100%;">
    <!-- PRODUCT INFO -->
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <asp:Panel ID="pHeader1" runat="server" CssClass="cpHeader">
                    <asp:Label ID="Label6" runat="server" >Product Information</asp:Label>
                </asp:Panel>
                
                <asp:Panel ID="pBody2" runat="server" CssClass="cpBody">
                <div align="center">
                    <table width="100%" style="font-family: Tahoma; font-size: small">
                        <tr>
                            <td width="2%"></td>
                            <td width="15%"></td>
                            <td width="3%"></td>
                            <td width="15%"></td>
                            <td width="10%"></td>
                            <td width="15%"></td>
                            <td width="3%"></td>
                            <td width="15%"></td>
                            <td width="2%"></td>                            
                        </tr>
                        <tr>
                            <td class="style53"></td>
                            <td class="style60" align="left">Product Code</td>
                            <td class="style59"> : </td> 
                            <td class="style54" align="left">
                                <asp:TextBox ID="txtCode" runat="server" AutoPostBack="True" 
                                    Width="200px" CssClass="form-control" Height="30px"
                                    placeholder="Enter Product Code"
                                    ontextchanged="txtCode_TextChanged" TabIndex="5"></asp:TextBox>
                            </td>
                            <td class="style51"></td>
                            <td align="left">Search Model</td>
                            <td class="style26">:</td>
                            <td align="left">
                                <asp:TextBox ID="txtModel" runat="server" Height="30px" Width="200px" 
                                    placeholder="Enter Model & Search" TabIndex="10" 
                                    AutoPostBack="True" ontextchanged="txtModel_TextChanged">
                                </asp:TextBox>
                                <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" TargetControlID="txtModel"
                                     MinimumPrefixLength="1" EnableCaching="true" CompletionSetCount="1" CompletionInterval="100" 
                                     ServiceMethod="GetModel" >
                                </asp:AutoCompleteExtender>
                            </td>
                            <td class="style26"></td>
                        </tr>

                        <!-- ---------------------------------- -->
                        <!-- ---------------------------------- -->

            
                        <!-- Product Model -->
                        <tr>
                            <td class="style53"></td>
                            <td style="text-align: Left; font-family: Arial; font-weight: 700; color: #000000;" 
                                class="style60">Product Model</td>
                            <td class="style59"> : </td>              
                            <td style="text-align: left" class="style54" >                    
                                <asp:DropDownList ID="ddlContinents" class="form-control chzn-select"
                                    runat="server" AutoPostBack = "true"
                                    OnSelectedIndexChanged="ddlContinents_SelectedIndexChanged"                         
                                    Width="200px" Height="30px" TabIndex="6"
                                    ToolTip="Please Select Product Model ...">
                                    <asp:ListItem Text = "--Select Model--" Value = ""></asp:ListItem>
                                </asp:DropDownList>
                                </td>
                            <td class="style51" align="left">
                                 &nbsp;</td>

                            <!-- Product Description -->                
                            <td style="text-align: Left; font-family: Arial; " 
                                class="style58">Product Description </td>
                            <td class="style20"> : </td>
                            <td style="text-align: left" class="style51">
                                <asp:TextBox ID="txtProdDesc" runat="server" Width="200px" CssClass="form-control"
                                    Enabled="False" Height="30px"></asp:TextBox></td>
                            <td class="style20"></td>
                        </tr>

             
            
                         <!-- Unit Price -->
                        <tr>
                            <td class="style53"></td>
                            <td style="text-align: Left; font-family: Arial; color: ##003300; font-size: small;" 
                                class="style60">MRP (Tk.)</td>
                            <td class="style59"> : </td>
                            <td style="text-align: left" class="style54">
                                <asp:TextBox ID="txtUP" runat="server" Width="200px" CssClass="form-control"
                                    style="font-weight: 700" Enabled="False" Height="30px"></asp:TextBox></td>
                            <td class="style51">
                                <asp:TextBox ID="txtProdID" runat="server" Width="6px" 
                                    style="font-weight: 700" Visible="False"></asp:TextBox> 
                            </td>

                            <!-- Campaign Price -->                
                            <td style="text-align: Left; font-family: Arial; color: ##003300; font-size: small;" 
                                class="style58">Campaign Price (Tk.) </td>
                            <td class="style20"> : </td>
                            <td style="text-align: left" class="style51">
                            <asp:TextBox ID="txtCP" runat="server" Width="200px" CssClass="form-control"
                                Enabled="False" Height="30px"></asp:TextBox></td>
                            <td class="style20"></td>
                        </tr>

            
                         <!-- Quantity -->
                        <tr>
                            <td class="style53"></td>
                            <td style="text-align: Left; font-family: Arial; font-weight: 700; color: #008000; font-size: small;" 
                                class="style60">Quantity</td>
                            <td class="style59"> : </td>
                            <td style="text-align: left" class="style54">
                                <asp:TextBox ID="txtQty" runat="server" AutoPostBack="True" Width="200px" 
                                    OnTextChanged="txtQty_TextChanged"
                                    onkeypress="return numeric_only(event)"
                                    CssClass="form-control" Height="30px"
                                    placeholder="Enter Quantity"
                                    ToolTip="Please Enter Product Quantity" TabIndex="7" MaxLength="5">
                                </asp:TextBox>                    
                            </td>
                            <td class="style51" align="left">
                                &nbsp;</td>


                            <!-- Total Price -->                
                            <td style="text-align: Left; font-family: Arial; font-weight: 700; color: #00CC00; font-size: small;" 
                                class="style58">Total Price (Tk.)</td>
                            <td class="style20"> : </td>
                            <td style="text-align: left" class="style51">
                            <asp:TextBox ID="txtTotalAmnt" runat="server" Width="200px" CssClass="form-control"
                                Height="30px" Enabled="False"></asp:TextBox></td>
                            <td class="style20"></td>
                        </tr>

                        
                        <!-- Discount Code -->
                        <tr>
                            <td class="style53"></td>
                            <td style="text-align: Left; font-family: Arial; " 
                                class="style60">Discount Code</td>
                            <td class="style59"> : </td>
                            <td style="text-align: left" class="style54">
                            <asp:TextBox ID="txtDisCode" runat="server" Width="200px" CssClass="form-control"
                                Height="30px" Enabled="False"></asp:TextBox></td>
                
                            <!-- Discount Amount -->
                            <td class="style51"></td>
                            <td style="text-align: Left; font-family: Arial; color: #FF0000;" 
                                class="style58">Discount Amount</td>
                            <td class="style20"> : </td>
                            <td style="text-align: left" class="style51">
                            <asp:TextBox ID="txtDisAmnt" runat="server" AutoPostBack="True" Width="200px" 
                                OnTextChanged="txtDisAmnt_TextChanged"
                                placeholder="Enter Discount Amount" Height="30px"
                                onkeypress="return numeric_only(event)" CssClass="form-control"
                                ToolTip="Discount Amount" TabIndex="8" MaxLength="6"></asp:TextBox></td>
                            <td class="style20"></td>
                        </tr>

            
                         <!-- Discount Reference -->
                        <tr>
                            <td class="style53"></td>
                            <td style="text-align: Left; font-family: Arial; " 
                                class="style60">Ref. for Discount</td>
                            <td class="style59"> : </td>
                            <td style="text-align: left" class="style54">
                                <asp:TextBox ID="txtDisRef" runat="server" Width="200px" 
                                  CssClass="form-control" Height="30px"
                                  placeholder="Reference Name"
                                  ToolTip="Reference for Discount" TabIndex="9"></asp:TextBox>
                            </td>

                            <!-- Withdrawn/Adjustment Amount -->
                            <td class="style51"></td>
                            <td style="text-align: Left; font-family: Arial; " 
                                class="style58">With/Adjust Amount</td>
                            <td class="style20"> : </td>
                            <td style="text-align: left" class="style51">
                                <asp:TextBox ID="txtWithAdj" runat="server" AutoPostBack="True" Width="200px" 
                                    CssClass="form-control" Height="30px"
                                    placeholder="Withdrawn/Adjustment Amount"
                                    OnTextChanged="txtWithAdj_TextChanged"
                                    onkeypress="return numeric_only(event)"
                                    ToolTip="Withdrawn/Adjustment Amount..." TabIndex="10" MaxLength="6"></asp:TextBox>
                            </td>
                            <td class="style20"></td>
                        </tr>

            
                        <!-- Net Amount -->
                        <tr>
                            <td class="style53"></td>
                            <td class="style60" align="left">PayG Serial #</td>
                            <td class="style59">:</td>
                            <td class="style54" align="left">
                                <asp:TextBox ID="txtSL0" runat="server" CssClass="form-control" MaxLength="20" 
                                    TabIndex="11" placeholder="Enter Tag Serial #" Height="30px"
                                    ToolTip="Please Enter Product Tag Serial Number" Width="200px"></asp:TextBox>
                            </td>
                            <td class="style51"></td>
                            <td style="text-align: Left; font-family: Arial; font-weight: 700; color: #008000;" 
                                class="style58">Net Amount (Tk.)</td>
                            <td class="style52"> : </td>
                            <td style="text-align: left" class="style51">
                            <asp:TextBox ID="txtNet" runat="server" Width="200px" CssClass="form-control"
                                Height="30px" Enabled="False"></asp:TextBox></td>
                        </tr>

                         <!-- Product SL# -->
                        <tr>
                            <td class="style53"></td>
                            <td style="text-align: Left; font-family: Arial;" 
                                class="style60">Product SL#</td>
                            <td class="style59"> : </td>
                            <td style="text-align: left" class="style54">
                                <asp:TextBox ID="txtSL" runat="server" Width="200px" Height="30px"
                                    CssClass="form-control" placeholder="Enter Product SL#"
                                    ToolTip="Please Enter Product Serial Number" TabIndex="11" MaxLength="20"></asp:TextBox>
                            </td>

                            <!-- Remarks -->
                            <td class="style51"></td>
                            <td style="text-align: Left; font-family: Arial; " 
                                class="style58">Remarks</td>
                            <td class="style20"> : </td>
                            <td style="text-align: left" class="style51">
                            <asp:TextBox ID="txtRemarks" runat="server" Width="200px" CssClass="form-control"
                                Height="30px"
                                placeholder="Enter Note/Remarks" TabIndex="12" MaxLength="40"></asp:TextBox></td>
                            <td class="style20"></td>
                        </tr>

                       
                        <!-- Add to Data Grid -->
                        <tr>
                            <td class="style53"></td>
                            <td class="style60"></td>
                            <td class="style59"></td>
                            <td style="text-align: left" class="style54">
                                <asp:Button ID="btnAdd" runat="server" Text="Add Product in List" 
                                    OnClick="btnAdd_Click" 
                                    Width="116px" 
                                    Font-Size="X-Small" CssClass="btn btn-alart"
                                    ToolTip="Click here for add product in list ..." TabIndex="13" 
                                    BackColor="#000099" ForeColor="Aqua"/>
                            </td>                
                        </tr>
            
                        <tr>
                            <td class="style53"></td>
                            <td class="style60"></td>
                            <td class="style59"></td>
                            <td class="style54"></td>
                        </tr>
            
                    </table>
        
                    <div>
                    <asp:Label ID="lblError" ForeColor="red" runat="server" Text=""></asp:Label>
                    </div>
        
                    <table width="100%" style="font-family: Tahoma; font-size: small">
                        <tr>
                                <td class="style19">
                                    <!-- Data Grid -->                
                                    <asp:GridView ID="gvUsers" runat="server"
                                    AutoGenerateColumns="false"                        
                                    CssClass="mGrid"
                                    DataKeyNames="ProductID"
                                    EmptyDataText = "No product in list !!! Please select model and add in list."
                                    EmptyDataRowStyle-CssClass ="gvEmpty"
                                    Onrowdeleting="gvUsers_RowDelating"
                                    ShowFooter="true"
                                    OnRowDataBound="gvUsers_RowDataBound"                        
                                    GridLines="none" Width="100%">
                                    <FooterStyle Font-Bold="true" BackColor="#61A6F8" ForeColor="black" />
                                    <Columns>
                                        <asp:BoundField HeaderText="P.ID" DataField="ProductID"/>                        
                                        <asp:BoundField HeaderText="Product Model" DataField="Model" />                       
                                        <asp:BoundField HeaderText="MRP (Tk.)" DataField="MRP" />                       
                                        <asp:BoundField HeaderText="Campaign Price" DataField="CampaignPrice" />
                                        <asp:BoundField HeaderText="Qty" DataField="Qty" />
                                        <asp:BoundField HeaderText="Total Price" DataField="TotalPrice" />
                                        <asp:BoundField HeaderText="Dis Amnt" DataField="DisAmnt" />
                                        <asp:BoundField HeaderText="Dis Code" DataField="DisCode"/>
                                        <asp:BoundField HeaderText="Dis Ref" DataField="DisRef"/>
                                        <asp:BoundField HeaderText="With/Adj Amnt" DataField="WithAdjAmnt" />
                                        <asp:BoundField HeaderText="NetAmnt" DataField="NetAmnt"/>  
                                                                              
                                        <asp:BoundField HeaderText="Product SL" DataField="ProductSL" />
                                        <asp:BoundField HeaderText="Remarks" DataField="Remarks" ItemStyle-Width="5px" />
                                        <asp:TemplateField HeaderText="Action">
                                            <ItemTemplate>               
                                                <asp:ImageButton ID="ibtnDelete" runat="server"
                                                    ToolTip="Delete"                                        
                                                    ImageUrl="~/Images/btn-delete.jpg" 
                                                    CommandName="Delete"   
                                                    OnClientClick="return confirm('Are you sure you want to delete this record?');"                                                                                                          
                                                     />
                                            </ItemTemplate>
                                
                                        </asp:TemplateField>

                                    </Columns>
                                    <EmptyDataRowStyle CssClass="gvEmpty" />
                                </asp:GridView>
                            </td>
                        </tr> 
                        <tr>
                            <td>
                                <asp:Label ID="lblNetAmnt" runat="server" Text="0" Visible="False"></asp:Label>
                                <asp:TextBox ID="txtNetAmnt" runat="server" Visible="False" Width="77px"></asp:TextBox>
                            </td>
                        </tr>
            
                    </table>
  
                </div>

                <div>&nbsp;</div>

            </asp:Panel>

        <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server" TargetControlID="pBody2" CollapseControlID="pHeader1" ExpandControlID="pHeader1"
                Collapsed="false" TextLabelID="lblText" CollapsedText="Click to Show Product Details ..." ExpandedText="Click to Hide Product Details ..."
                CollapsedSize="0">
            </cc1:CollapsiblePanelExtender>
        </ContentTemplate>                     

    </asp:UpdatePanel>
    </div>
    <!-- END PRODUCT INFO -->

    <div style="float:left; padding-top:10px; width:100%;">
    <!-- Installment INFO -->
    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
            <ContentTemplate>
                <asp:Panel ID="pHeader4" runat="server" CssClass="cpHeader">
                    <asp:Label ID="Label8" runat="server" >Installment Information</asp:Label>
                </asp:Panel>
                
                <asp:Panel ID="Panel2" runat="server" CssClass="cpBody">
                    <div align="center">
        
                    <table width="100%" style="font-family: Tahoma; font-size: small">
                        <tr>
                            <td width="2%"></td>
                            <td class="style2"></td>
                            <td width="3%"></td>
                            <td width="15%"></td>
                            <td width="10%"></td>
                            <td width="15%"></td>
                            <td width="3%"></td>
                            <td width="15%"></td>
                            <td width="2%"></td>                            
                        </tr>

                        <tr>
                            <td class="style1"></td>
                            <td align="left"                                 
                                style="color: #003366; font-weight: bold; font-family: Tahoma; font-size: small;" class="style2" 
                                >
                                Duration</td>
                            <td style="text-align: center" class="style3">:</td>
                            <td align="left" colspan="5">
                                <asp:RadioButtonList ID="RadioButtonList1" runat="server" RepeatColumns="3" 
                                    RepeatDirection="Horizontal" Width="80%" AutoPostBack="True" 
                                    onselectedindexchanged="RadioButtonList1_SelectedIndexChanged">
                                    <asp:ListItem Selected="True" Value="4">4 Months</asp:ListItem>
                                    <asp:ListItem Value="5">5 Months</asp:ListItem>
                                    <asp:ListItem Value="6">6 Months</asp:ListItem>
                                    <asp:ListItem Value="7">7 Months</asp:ListItem>
                                    <asp:ListItem Value="8">8 Months</asp:ListItem>
                                    <asp:ListItem Value="9">9 Months</asp:ListItem>
                                    <asp:ListItem Value="10">10 Months</asp:ListItem>
                                    <asp:ListItem Value="11">11 Months</asp:ListItem>
                                    <asp:ListItem Value="12">12 Months</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                                            
                            <td class="style9"></td>
                        </tr>

                        <tr>
                            <td class="style1"></td>
                            <td align="left"                                  
                                style="color: #CC0000; font-weight: bold; font-family: Tahoma; font-size: small;" 
                                class="style2">
                                Down Payment</td>
                            <td style="text-align: center" class="style3">:</td>
                            <td class="style4" align="left">
                                <asp:TextBox ID="txtDownPayment" runat="server" CssClass="form-control" MaxLength="20" 
                                    TabIndex="11" Height="30px" placeholder="Enter Down Payment"
                                    onkeypress="return numeric_only(event)"
                                    ToolTip="Please Enter Customer Down Payment" Width="200px" 
                                    AutoPostBack="True" ontextchanged="txtDownPayment_TextChanged"></asp:TextBox>
                            </td>
                                            
                            <td class="style5"></td>
                            <td align="left" class="style6"
                                style="font-family: Tahoma; color: #800000; font-size: medium; font-weight: bold;">Rest Amount</td>
                            <td class="style7">:</td>
                            <td align="left" class="style8">
                                <asp:TextBox ID="txtRestAmnt" runat="server" CssClass="form-control" Height="30px" 
                                    MaxLength="20" style="font-weight: 700" TabIndex="11" 
                                    ToolTip="Rest Amount ..." Width="200px" Enabled="False"></asp:TextBox>
                            </td>
                            <td class="style9"></td>
                        </tr>


                        <tr>
                            <td class="style1"></td>
                            <td align="left" class="style2">Installment Amount</td>
                            <td class="style3">&nbsp;</td>
                            <td width="15%" class="style4">
                                <asp:TextBox ID="txtInstAmnt" runat="server" CssClass="form-control" Height="30px" 
                                    MaxLength="20" style="font-weight: 700" TabIndex="11" 
                                    ToolTip="Monthly Installment Amount..." Width="200px" Enabled="False"></asp:TextBox>
                            </td>
                            <td class="style5"></td>
                            <td class="style6" align="left">Effective From</td>
                            <td class="style7">:</td>
                            <td class="style8" align="left">
                                <asp:TextBox ID="txtEDate" runat="server" Width="100px" TabIndex="6" 
                                        ToolTip="Please Enter Customer Date of Birth" MaxLength="10"
                                        CssClass="" Height="25px"
                                        placeholder="MM/dd/yyyy"
                                        >
                                </asp:TextBox> 
                
                                &nbsp; 
                                <asp:ImageButton ID="ImageButton2" ImageUrl="~/images/cal.gif" ImageAlign="Bottom"
                                    runat="server" Height="18px" Width="17px" />
                                <cc1:CalendarExtender ID="CalendarExtender2" PopupButtonID="ImageButton2" runat="server" TargetControlID="txtEDate"
                                    Format="MM/dd/yyyy">
                                </cc1:CalendarExtender>
                            </td>
                            <td class="style9"></td>                            
                        </tr>

                        <tr>
                            <td width="2%"></td>
                            <td class="style2"></td>
                            <td width="3%"></td>
                            <td width="15%" align="left">
                                <asp:Button ID="btnGenerate" runat="server" 
                                    CssClass="btn btn-success" Text="Generate" 
                                    ToolTip="Click here for generate Installment Date &amp; Amount " 
                                    onclick="btnGenerate_Click" />
                            </td>
                            <td width="10%"></td>
                            <td width="15%"></td>
                            <td width="3%"></td>
                            <td width="15%" align="left">(MM/dd/yyyy)</td>
                            <td width="2%"></td>                            
                        </tr>

                        <tr>
                            <td width="2%"></td>
                            <td class="style2"></td>
                            <td width="3%"></td>
                            <td width="15%"></td>
                            <td width="10%"></td>
                            <td width="15%"></td>
                            <td width="3%"></td>
                            <td width="15%"></td>
                            <td width="2%"></td>                            
                        </tr>

                        <tr>
                            <td width="2%"></td>
                            <td class="style2"></td>
                            <td width="3%"></td>
                            <td colspan="5">
                                <!-- Grid -->                
                                <asp:GridView ID="GridView1" runat="server"
                                    AutoGenerateColumns="false"                        
                                    CssClass="mGrid"
                                    DataKeyNames="ID"
                                    EmptyDataText = "No Data in list !!! " 
                                    ShowFooter="false"                                                      
                                    Width="500px">
                                    <FooterStyle Font-Bold="true" BackColor="#CCFFFF" ForeColor="black" BorderStyle="NotSet" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="SL#" ItemStyle-Width="5px">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                                                                                       
                                        <asp:BoundField HeaderText="Due Date (MM/dd/yyyy)" DataField="DueDate" />
                                        <asp:BoundField HeaderText="Amount (Tk.)" DataField="Amount"/>
                                                                        
                                    </Columns>
                        
                                </asp:GridView>
                            
                            </td>
                            <td></td>
                                                                                
                        </tr>

                        <tr>
                            <td width="2%"></td>
                            <td class="style2"></td>
                            <td width="3%"></td>
                            <td width="15%"></td>
                            <td width="10%"></td>
                            <td width="15%"></td>
                            <td width="3%"></td>
                            <td width="15%"></td>
                            <td width="2%"></td>                            
                        </tr>
                                                
                    </table>

                </asp:Panel>


            <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender4" runat="server" TargetControlID="Panel2" CollapseControlID="pHeader4" ExpandControlID="pHeader4"
                Collapsed="false" TextLabelID="lblText" CollapsedText="Click to Show Installment Details ..." ExpandedText="Click to Hide Installment Details ..."
                CollapsedSize="0">
            </cc1:CollapsiblePanelExtender>
        </ContentTemplate>                     

    </asp:UpdatePanel>
    </div>
    <!-- END INSTALLMENT INFO -->

    
    <div style="float:left; padding-top:10px; width:100%;">
    <!-- GRANTOR INFO -->
    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
            <ContentTemplate>
                <asp:Panel ID="pHeader5" runat="server" CssClass="cpHeader">
                    <asp:Label ID="Label9" runat="server" >Guarantor Information</asp:Label>
                </asp:Panel>
                
                <asp:Panel ID="pBody5" runat="server" CssClass="cpBody">
                    <div align="center">
                        <table width="100%" style="font-family: Tahoma; font-size: small">
                            <tr>
                                <td width="2%"></td>
                                <td align="left"><u><b>1st Guarantor </b></u></td>
                                <td width="3%"></td>
                                <td width="15%"></td>
                                <td width="10%"></td>
                                <td align="left"><u><b>2nd Guarantor</b></u></td>
                                <td width="3%"></td>
                                <td width="15%"></td>
                                <td width="2%"></td>                            
                            </tr>

                            <tr>
                                <td width="2%"></td>
                                <td align="left">Job ID</td>
                                <td width="3%">:</td>
                                <td align="left">
                                    <asp:TextBox ID="txtGJobID1" runat="server" AutoPostBack="True" 
                                        CssClass="form-control" MaxLength="8"  Height="30px"
                                        TabIndex="14" placeholder="G1 Job ID #"
                                        ToolTip="Please Enter Guarantor Job ID ..." Width="200px"></asp:TextBox>
                                </td>
                                <td width="10%"></td>
                                <td align="left">Job ID</td>
                                <td width="3%">:</td>
                                <td width="15%">
                                    <asp:TextBox ID="txtGJobID2" runat="server" AutoPostBack="True" 
                                        CssClass="form-control" MaxLength="8" TabIndex="14" 
                                        placeholder="G2 Job ID #" Height="30px"
                                        ToolTip="Please Enter Guarantor Job ID ..." Width="200px"></asp:TextBox>
                                </td>
                                <td width="2%"></td>                            
                            </tr>

                            <tr>
                                <td width="2%"></td>
                                <td align="left">Name</td>
                                <td width="3%">:</td>
                                <td align="left">
                                    <asp:TextBox ID="txtGName1" runat="server" AutoPostBack="True" 
                                        CssClass="form-control" MaxLength="25"  Height="30px"
                                        TabIndex="14" placeholder="G1 Full Name"
                                        ToolTip="Please Enter Guarantor1 Name ..." Width="200px"></asp:TextBox>
                                </td>
                                <td width="10%"></td>
                                <td align="left">Name</td>
                                <td width="3%">:</td>
                                <td width="15%">
                                    <asp:TextBox ID="txtGName2" runat="server" AutoPostBack="True" 
                                        CssClass="form-control" MaxLength="25" TabIndex="14" 
                                        placeholder="G2 Full Name" Height="30px"
                                        ToolTip="Please Enter Guarantor2 Name ..." Width="200px"></asp:TextBox>
                                </td>
                                <td width="2%"></td>                            
                            </tr>

                            <tr>
                                <td width="2%"></td>
                                <td align="left">Address</td>
                                <td width="3%">:</td>
                                <td align="left">
                                    <asp:TextBox ID="txtGAdd1" runat="server" AutoPostBack="True" 
                                        CssClass="form-control" MaxLength="100" Height="30px"
                                        TabIndex="14" placeholder="G1 Address"
                                        ToolTip="Please Enter Guarantor 1 Address ..." Width="200px"></asp:TextBox>
                                </td>
                                <td width="10%"></td>
                                <td align="left">Address</td>
                                <td width="3%">:</td>
                                <td width="15%">
                                    <asp:TextBox ID="txtGAdd2" runat="server" AutoPostBack="True" 
                                        CssClass="form-control" MaxLength="100" TabIndex="14" 
                                        placeholder="G2 Address" Height="30px"
                                        ToolTip="Please Enter Guarantor 2 Address ..." Width="200px"></asp:TextBox>
                                </td>
                                <td width="2%"></td>                            
                            </tr>

                            <tr>
                                <td width="2%"></td>
                                <td align="left">Mobile #</td>
                                <td width="3%">:</td>
                                <td align="left">
                                    <asp:TextBox ID="txtGMobile1" runat="server" AutoPostBack="True" 
                                        CssClass="form-control" MaxLength="11" Height="30px"
                                        TabIndex="14" placeholder="G1 Mobile #"
                                        ToolTip="Please Enter Guarantor 1 Mobile # ..." Width="200px"></asp:TextBox>
                                </td>
                                <td width="10%"></td>
                                <td align="left">Mobile #</td>
                                <td width="3%">:</td>
                                <td width="15%">
                                    <asp:TextBox ID="txtGMobile2" runat="server" AutoPostBack="True" 
                                        CssClass="form-control" MaxLength="11" TabIndex="14" 
                                        placeholder="G2 Mobile #" Height="30px"
                                        ToolTip="Please Enter Guarantor 2 Mobile # ..." Width="200px"></asp:TextBox>
                                </td>
                                <td width="2%"></td>                            
                            </tr>

                            <tr>
                                <td width="2%"></td>
                                <td align="left">Relation</td>
                                <td width="3%">:</td>
                                <td align="left">
                                    <asp:TextBox ID="txtGRelation1" runat="server" AutoPostBack="True" 
                                        CssClass="form-control" MaxLength="40" Height="30px"
                                        TabIndex="14" placeholder="Relation"
                                        ToolTip="Please Enter Guarantor 1 Relation ..." Width="200px"></asp:TextBox>
                                </td>
                                <td width="10%"></td>
                                <td align="left">Relation</td>
                                <td width="3%">:</td>
                                <td width="15%">
                                    <asp:TextBox ID="txtGRelation2" runat="server" AutoPostBack="True" 
                                        CssClass="form-control" MaxLength="40" TabIndex="14" 
                                        placeholder="Relation" Height="30px"
                                        ToolTip="Please Enter Guarantor 2 Relation ..." Width="200px"></asp:TextBox>
                                </td>
                                <td width="2%"></td>                            
                            </tr>

                            <tr>
                                <td width="2%"></td>
                                <td align="left">NID #</td>
                                <td width="3%">:</td>
                                <td align="left">
                                    <asp:TextBox ID="txtGNID1" runat="server" AutoPostBack="True" 
                                        CssClass="form-control" MaxLength="20" Height="30px"
                                        TabIndex="14" placeholder="G1 NID #"
                                        ToolTip="Please Enter Guarantor 1 NID ..." Width="200px"></asp:TextBox>
                                </td>
                                <td width="10%"></td>
                                <td align="left">NID #</td>
                                <td width="3%">:</td>
                                <td width="15%">
                                    <asp:TextBox ID="txtGNID2" runat="server" AutoPostBack="True" 
                                        CssClass="form-control" MaxLength="20" TabIndex="14" 
                                        placeholder="G2 NID #" Height="30px"
                                        ToolTip="Please Enter Guarantor 2 NID ..." Width="200px"></asp:TextBox>
                                </td>
                                <td width="2%"></td>                            
                            </tr>


                            <tr>
                                <td width="2%"></td>
                                <td width="15%"></td>
                                <td width="3%"></td>
                                <td width="15%"></td>
                                <td width="10%"></td>
                                <td width="15%"></td>
                                <td width="3%"></td>
                                <td width="15%"></td>
                                <td width="2%"></td>                            
                            </tr>

                            </table>

                    </div>
                </asp:Panel>


            <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender5" runat="server" TargetControlID="pBody5" CollapseControlID="pHeader5" ExpandControlID="pHeader5"
                Collapsed="false" TextLabelID="lblText" CollapsedText="Click to Show Guarantor Details ..." ExpandedText="Click to Hide Guarantor Details ..."
                CollapsedSize="0">
            </cc1:CollapsiblePanelExtender>
        </ContentTemplate>                     

    </asp:UpdatePanel>
    </div>
    <!-- END INSTALLMENT INFO -->

    
    <div style="float:left; padding-top:10px; width:100%;">
    <!-- PAYMENT INFO -->
    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
                <asp:Panel ID="pHeader3" runat="server" CssClass="cpHeader">
                    <asp:Label ID="Label7" runat="server" >Payment Information</asp:Label>
                </asp:Panel>
                
                <asp:Panel ID="pBody3" runat="server" CssClass="cpBody">
                    <div align="center">
        
                    <table width="100%" style="font-family: Tahoma; font-size: small">
                        <tr>
                            <td width="2%"></td>
                            <td width="15%"></td>
                            <td width="3%"></td>
                            <td width="15%"></td>
                            <td width="10%"></td>
                            <td width="15%"></td>
                            <td width="3%"></td>
                            <td width="15%"></td>
                            <td width="2%"></td>                            
                        </tr>
                        <tr>
                            <td class="style35"></td>
                            <td class="style57" align="left" 
                    
                                style="color: #006600; font-weight: bold; font-family: Tahoma; font-size: large;">Cash Amount</td>
                            <td style="text-align: center" class="style36">:</td>
                            <td class="style28" align="left">
                                <asp:TextBox ID="txtCash" runat="server" Width="200px" 
                                    CssClass="form-control" placeholder="Cash Amount"
                                    onkeypress="return numeric_only(event)"
                                    ToolTip="Please Enter Cash Amount..." 
                                    MaxLength="8" ontextchanged="txtCash_TextChanged" TabIndex="14" 
                                    AutoPostBack="True"></asp:TextBox>
                            </td>
                
                            <td class="style56"></td>
                            <td class="style29" align="left" 
                                style="color: #FF0000; font-family: Tahoma; font-size: large;">Due Amount</td>
                            <td style="text-align: center" class="style20">:</td>
                            <td class="style33" align="left">
                                <asp:TextBox ID="txtDue" runat="server" Width="200px" 
                                    style="font-weight: 700" CssClass="form-control"
                                    onkeypress="return numeric_only(event)"
                                    ToolTip="Due Amount" 
                                    MaxLength="10" TabIndex="4" ReadOnly="True" BackColor="#FFCCFF">0</asp:TextBox>
                            </td>
                            <td class="style28"></td>
                        </tr>

                        <tr>
                            <td class="style35"></td>
                            <td class="style57" align="left" style="color: #008080">&nbsp;</td>
                            <td style="text-align: center" class="style36">&nbsp;</td>
                            <td class="style28" align="left">
                                <asp:DropDownList ID="ddlPayType" runat="server" BackColor="#F6F1DB"
                                    Height="24px" Width="169px" 
                                    TabIndex="15" ToolTip="Please select discount by..." 
                                    onselectedindexchanged="ddlPayType_SelectedIndexChanged" 
                                    AutoPostBack="True" Visible="False">
                                    <asp:ListItem Value = "">CASH</asp:ListItem>
                                    <asp:ListItem>CHEQUE</asp:ListItem>
                                    <asp:ListItem>AMEX</asp:ListItem>
                                    <asp:ListItem>VISA CARD</asp:ListItem>
                                    <asp:ListItem>MASTER CARD</asp:ListItem>
                                    <asp:ListItem>DD</asp:ListItem>
                                    <asp:ListItem>TT</asp:ListItem>
                                    <asp:ListItem>OTHERS</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                
                            <td class="style56"></td>
                            <td class="style29" align="left" style="color: #FF0000">&nbsp;</td>
                            <td style="text-align: center" class="style20">&nbsp;</td>
                            <td class="style33" align="left">
                                &nbsp;</td>
                            <td class="style28"></td>
                        </tr>

                        <!-- CARD AMNT -->
                        <tr>
                            <td class="style35"></td>
                            <td class="style57" align="left"                     
                    
                    
                                style="font-family: Tahoma; color: #000080; font-size: small; font-weight: normal">
                                <asp:Label ID="Label2" runat="server" Text="Card 1 Amnt" Font-Bold="True" 
                                    Font-Names="Tahoma" ForeColor="#003366"></asp:Label>
                                </td>
                            <td style="text-align: center" class="style36">:</td>
                            <td class="style28" align="left">
                                <asp:TextBox ID="txtCardAmnt1" runat="server" Width="200px" 
                                    onkeypress="return numeric_only(event)" CssClass="form-control"
                                    style="font-weight: 700" ToolTip="Amount from Card 1 ..." 
                                    MaxLength="8" Visible="True" TabIndex="15" AutoPostBack="True" 
                                    ontextchanged="txtCardAmnt1_TextChanged"></asp:TextBox>
                            </td>
                
                            <td class="style56"></td>
                            <td class="style29" align="left">
                                <asp:Label ID="Label3" runat="server" Text="Card 2 Amnt" Font-Bold="True" 
                                    Font-Names="Tahoma" ForeColor="#003366"></asp:Label>
                                </td>
                            <td style="text-align: center" class="style20">:</td>
                            <td class="style33" align="left">
                                <asp:TextBox ID="txtCardAmnt2" runat="server" Width="200px" 
                                    onkeypress="return numeric_only(event)" CssClass="form-control"
                                    style="font-weight: 700" ToolTip="Amount from Card 2 ..." 
                                    MaxLength="8" Visible="True" TabIndex="21" AutoPostBack="True" 
                                    ontextchanged="txtCardAmnt2_TextChanged"></asp:TextBox>
                            </td>
                            <td class="style28">
                                &nbsp;</td>
                        </tr>

                        <tr>
                            <td class="style35"></td>
                            <td class="style57" align="left"                     
                    
                                style="font-family: Tahoma; font-size: small; font-weight: normal">
                                <asp:Label ID="lblNo" runat="server" Text="Card #" Visible="True"></asp:Label>
                                </td>
                            <td style="text-align: center" class="style36">:</td>
                            <td class="style28" align="left">
                                <asp:TextBox ID="txtChequeNo" runat="server" Width="200px" 
                                    style="font-weight: 700" ToolTip="Card Number ..." 
                                    MaxLength="16" Visible="True" TabIndex="16"></asp:TextBox>
                            </td>
                
                            <td class="style56"></td>
                            <td class="style29" align="left">
                                <asp:Label ID="lblNo0" runat="server" Text="Card #" Visible="True"></asp:Label>
                                </td>
                            <td style="text-align: center" class="style20">:</td>
                            <td class="style33" align="left">
                                <asp:TextBox ID="txtChequeNo2" runat="server" Width="200px" 
                                    style="font-weight: 700" ToolTip="Card Number ..." 
                                    MaxLength="16" Visible="True" TabIndex="22"></asp:TextBox>
                            </td>
                            <td class="style28">
                                &nbsp;</td>
                        </tr>

                        

                        <tr>
                            <td class="style35"></td>
                            <td class="style57" align="left"                     
                    
                                style="font-family: Tahoma; font-size: small; font-weight: normal">
                                <asp:Label ID="Label1" runat="server" Text="Card Type" Visible="True"></asp:Label>
                                </td>
                            <td style="text-align: center" class="style36">:</td>
                            <td class="style28" align="left">
                                <asp:DropDownList ID="ddlCardType1" runat="server" TabIndex="17" Width="200px">
                                    <asp:ListItem>VISA</asp:ListItem>
                                    <asp:ListItem>MASTER</asp:ListItem>
                                    <asp:ListItem>AMEX</asp:ListItem>
                                    <asp:ListItem>OTHERS</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                
                            <td class="style56"></td>
                            <td class="style29" align="left">
                                <asp:Label ID="Label4" runat="server" Text="Card Type" Visible="True"></asp:Label>
                                </td>
                            <td style="text-align: center" class="style20">:</td>
                            <td class="style33" align="left">
                                <asp:DropDownList ID="ddlCardType2" runat="server" TabIndex="23" Width="200px">
                                    <asp:ListItem>VISA</asp:ListItem>
                                    <asp:ListItem>MASTER</asp:ListItem>
                                    <asp:ListItem>AMEX</asp:ListItem>
                                    <asp:ListItem>OTHERS</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td class="style28"></td>
                        </tr>


                        <tr>
                            <td class="style35"></td>
                            <td class="style57" align="left"                     
                    
                                style="font-family: Tahoma; font-size: small; font-weight: normal">
                                <asp:Label ID="lblBankName" runat="server" Text="Bank Name" Visible="True"></asp:Label>
                                </td>
                            <td style="text-align: center" class="style36">:</td>
                            <td class="style28" align="left">
                                <asp:TextBox ID="txtBankName" runat="server" Width="200px" 
                                    style="font-weight: 700" ToolTip="Pls enter bank name..." MaxLength="40" 
                                    Visible="True" TabIndex="18"></asp:TextBox>
                            </td>
                
                            <td class="style56"></td>
                            <td class="style29" align="left">
                                <asp:Label ID="lblBankName0" runat="server" Text="Bank Name" Visible="True"></asp:Label>
                                </td>
                            <td style="text-align: center" class="style20">:</td>
                            <td class="style33" align="left">
                                <asp:TextBox ID="txtBankName2" runat="server" Width="200px" 
                                    style="font-weight: 700" ToolTip="Pls enter bank name..." MaxLength="40" 
                                    Visible="True" TabIndex="24"></asp:TextBox>
                            </td>
                            <td class="style28"></td>
                        </tr>

                        <tr>
                            <td class="style35"></td>
                            <td class="style57" align="left"                     
                    
                    
                                style="font-family: Tahoma; color: #000080; font-size: small; font-weight: normal">
                                <asp:Label ID="lblSecurityCode" runat="server" Text="Security Code" Visible="True"></asp:Label>
                                </td>
                            <td style="text-align: center" class="style36">:</td>
                            <td class="style28" align="left">
                                <asp:TextBox ID="txtSecurityCode" runat="server" Width="200px" 
                                    style="font-weight: 700" ToolTip="Card Security Code ..." 
                                    MaxLength="5" Visible="True" TabIndex="19"></asp:TextBox>
                            </td>
                
                            <td class="style56"></td>
                            <td class="style29" align="left" color: #000080;>
                                <asp:Label ID="lblSecurityCode0" runat="server" Text="Security Code" 
                                    ForeColor="#003399"></asp:Label>
                                </td>
                            <td style="text-align: center" class="style20">:</td>
                            <td class="style33" align="left">
                                <asp:TextBox ID="txtSecurityCode2" runat="server" Width="200px" 
                                    style="font-weight: 700" ToolTip="Card Security Code ..." 
                                    MaxLength="5" Visible="True" TabIndex="25"></asp:TextBox>
                            </td>
                            <td class="style28"></td>
                        </tr>

                        <tr>
                            <td class="style35"></td>
                            <td class="style57" align="left"
                    
                    
                                style="font-family: Tahoma; color: #000080; font-size: small; font-weight: normal">
                                <asp:Label ID="lblSecurityCode1" runat="server" Text="Approval Code" 
                                    ForeColor="#006600"></asp:Label>
                                </td>
                            <td style="text-align: center" class="style36">:</td>
                            <td class="style28" align="left">
                                <asp:TextBox ID="txtApprovalCode1" runat="server" Width="200px" 
                                    style="font-weight: 700" ToolTip="Cheque/DD/TT Issue date..." 
                                    MaxLength="5" Visible="True" TabIndex="20"></asp:TextBox>
                            </td>                
                            <td class="style56"></td>
                            <td class="style29" align="left">
                                <asp:Label ID="lblSecurityCode2" runat="server" Text="Approval Code" 
                                    ForeColor="#006600"></asp:Label>
                                </td>
                            <td style="text-align: center" class="style20">:</td>
                            <td class="style33" align="left">
                                <asp:TextBox ID="txtApprovalCode2" runat="server" Width="200px" 
                                    style="font-weight: 700" ToolTip="Cheque/DD/TT Issue date..." 
                                    MaxLength="5" Visible="True" TabIndex="26"></asp:TextBox>
                            </td>
                            <td class="style28"></td>
                        </tr>
                        <tr>
                            <td class="style35"></td>
                            <td class="style57" align="left">
                                <asp:Label ID="lblIssueDate" runat="server" Text="Issue Date" Visible="False"></asp:Label>
                                </td>
                            <td style="text-align: center" class="style36">&nbsp;</td>
                            <td class="style28" align="left">
                                <asp:TextBox ID="txtIssueDate" runat="server" Width="200px" 
                                    style="font-weight: 700" ToolTip="Cheque/DD/TT Issue date..." 
                                    MaxLength="15" Visible="false" TabIndex="18"></asp:TextBox>
                            </td>                
                            <td class="style56"></td>
                            <td class="style29" align="left"></td>
                            <td style="text-align: center" class="style20"></td>
                            <td class="style33" align="left">
                                <asp:TextBox ID="txtPay" runat="server" Width="200px" 
                                    style="font-weight: 700" 
                                    onkeypress="return numeric_only(event)"
                                    ToolTip="Please Enter paid Amount..." 
                                    MaxLength="10" TabIndex="14" 
                                    AutoPostBack="False" Visible="False"></asp:TextBox>
                            </td>
                            <td class="style28"></td>
                        </tr>

                        <!-- Remarks -->
                        <tr>
                            <td class="style35"></td>
                            <td class="style57" align="left">
                                <asp:Label ID="Label5" runat="server" Text="Remarks/Note" Visible="True"></asp:Label>
                                </td>
                            <td style="text-align: center" class="style36">:</td>
                            <td align="left" colspan="5">
                                <asp:TextBox ID="txtNote" runat="server" Width="100%" 
                                    style="font-weight: 700" ToolTip="Write your Comments/Remarks/Note ..." 
                                    MaxLength="250" Visible="true" TabIndex="27"></asp:TextBox>
                            </td>                
                
                            <td class="style28"></td>
                        </tr>

                    </table>

                </div>
    
                <div>&nbsp;</div>

            </asp:Panel>

        <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender3" runat="server" TargetControlID="pBody3" CollapseControlID="pHeader3" ExpandControlID="pHeader3"
                Collapsed="false" TextLabelID="lblText" CollapsedText="Click to Show Payment Details ..." ExpandedText="Click to Hide Payment Details ..."
                CollapsedSize="0">
            </cc1:CollapsiblePanelExtender>
        </ContentTemplate>                     

    </asp:UpdatePanel>
    </div>
    <!-- END PAYMENT INFO -->
    <div>&nbsp;</div>

        
         
    <div>
        <table width="100%" style="font-family: Tahoma; font-size: small">             
            <!-- Line Break -->   
            <tr>
                <td  align="center"
                    style="background-image:url(../Images/header.jpg); height:0.5px; font-family: Arial;">                        
                </td>
            </tr>

            <tr>
                <td class="style28">&nbsp;</td>
            </tr>

            <tr>
                <td align="center">                    
                    <asp:Button ID="btnSave" CssClass="btn btn-primary" runat="server"  Text="Save & Bill Print" 
                        onclick="btnSave_Click" TabIndex="28" 
                        Font-Size="Small" 
                        ToolTip="Click here for save data..." Width="137px"/>
                        &nbsp;
                    <asp:Button ID="btnPrint" CssClass="btn btn-primary" runat="server" Text="Re-Print"
                        Font-Size="Small"
                        Width="88px" TabIndex="29" onclick="btnPrint_Click"/>
                        &nbsp;
                    <asp:Button ID="btnCancel" CssClass="btn btn-primary" runat="server" Text="New Bill" 
                        Font-Size="Small"
                        Width="88px" TabIndex="30" onclick="btnCancel_Click" />                    
                </td>
            </tr>
            <tr>
                <td>&nbsp;&nbsp;</td>
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

