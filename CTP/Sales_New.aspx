<%@ Page Language="C#" MasterPageFile="Admin.master" AutoEventWireup="true" 
CodeFile="Sales_New.aspx.cs" Inherits="Forms_Sales_New" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %> 
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
        
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">

    <link type="text/css" href="css/ui-lightness/jquery-ui-1.8.19.custom.css" rel="stylesheet" />
    <script type="text/javascript" src="js/bootstrap-combobox.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.7.0/jquery.min.js"></script>

    <style>
		a img{border: none;}
		ol li{list-style: decimal outside;}
		div#container{width: 780px;margin: 0 auto;padding: 1em 0;}
		div.side-by-side{width: 100%;margin-bottom: 1em;}
		div.side-by-side > div{float: left;width: 50%;}
		div.side-by-side > div > em{margin-bottom: 10px;display: block;}
		.clearfix:after{content: "\0020";display: block;height: 0;clear: both;overflow: hidden;visibility: hidden;}
		
		
		
	span#MainContent_lblDelivary_charge
	            {
                    font-weight: 800;
                    margin-left: 128px;
                }
		
		
		
	</style>

    <link rel="stylesheet" href="../Styles/chosen.css" />
        
    
    <script type="text/javascript">
        $(function () {
            $("#txtDate").datepicker();
            $("#txtToDate").datepicker();
        });        
    </script>
    
    <script type="text/javascript">
        function fnAllowNumeric() {
            if ((event.keyCode < 48 || event.keyCode > 57) && event.keyCode != 8 && event.keyCode != 189) {
                event.keyCode = 0;
                alert("Please Enter Number Only");
                return false;
            }
        }
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
        .style1{ width: 2%; }.style2{ width: 15%;}.style3{width: 3%;}.style4{width: 25%;}
        .style5{width: 10%;}.style6{width: 15%;}.style7{width: 3%;}.style8{width: 25%;}
        .style9{width: 2%;}.required {color: Red;}
        
        
      
    .error-message {
        color: red;
        margin-top: 5px;
       }

    </style>

    <link type="text/css" href="css/ui-lightness/jquery-ui-1.8.19.custom.css" rel="stylesheet" />
    <script type="text/javascript" src="js/bootstrap-combobox.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            $('[data-toggle="tooltip"]').tooltip();
            $('.combobox').combobox();
            $("#DOB").mask("9999-99-99");
        });
    </script>



    <script language="JavaScript" src="../js/datetimepicker.js" type="text/javascript"> </script>
            
</asp:Content>


<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    
    <h2 class="col-sm-12 bg-primary" style="padding:5px; font-family: Tahoma;" align="center"> Sales Entry Form ...</h2>
        
    <p>
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>                
    </p>

    <div style="float:left; padding-top:10px; width:100%;">

    <!-- MASTER DATA -->
    <asp:Button  runat="server" ID="btnCheckMail" Text="CHEK" 
            onclick="btnCheckMail_Click"/>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Panel ID="pHeader0" runat="server" CssClass="cpHeader">
                    <asp:Label ID="lblText0" runat="server" >Customer Information</asp:Label>
                </asp:Panel>
                
                <asp:Panel ID="pBody0" runat="server" CssClass="cpBody">
                <div align="center">
        
                    <table width="100%" style="font-family: Tahoma; font-size: small">
                    
                        <tr>
                            <td width="2%"></td>
                            <td width="15%"></td>
                            <td width="3%"></td>
                            <td width="25%"></td>
                            <td width="10%"></td>
                            <td width="15%"></td>
                            <td width="3%"></td>
                            <td width="25%"></td>
                            <td width="2%"></td>                            
                        </tr>
            
                        <!-- Challan No. -->
                        <tr>
                            <td ></td>
                            <td style="text-align: Left; font-family: Tahoma; font-weight: 700; color: #990000; font-size: large;" 
                                >Invoice/Bill #</td>
                            <td > : </td>
                            <td style="text-align: left">
                                <asp:TextBox ID="txtCHNo" runat="server" Width="200px" 
                                    style="font-weight: 700" ToolTip="Please Enter Challan Number" MaxLength="15" 
                                        BackColor="#66FFFF" BorderStyle="None" Font-Bold="True"
                                        ReadOnly="True" CssClass="form-control">
                                </asp:TextBox>
                            </td>
                
                
                            <!-- Challan Date -->
                            <td style="text-align: left;">
                                 <asp:RequiredFieldValidator 
                                 id="RequiredFieldValidator1" runat="server" 
                                 ErrorMessage="( * )"                    
                                 ControlToValidate="txtCHNo" ForeColor="Red"></asp:RequiredFieldValidator>
                            </td>
                            <td style="text-align: left; font-family: Arial; font-size: small; " 
                                >Bill Date</td>
                            <td > : </td>
                            <td style="text-align: left; color: #FF0000;" >                                
                                <asp:TextBox ID="txtDate" runat="server" Width="100px" Height="25px"
                                    ToolTip="Please Enter Challan Date" MaxLength="10" Enabled="False"></asp:TextBox> 
                
                                &nbsp; 
                                <asp:ImageButton ID="imgPopup" ImageUrl="~/images/cal.gif" ImageAlign="Bottom"
                                    runat="server" Height="18px" Width="17px" Enabled="False" />
                                <cc1:CalendarExtender ID="Calendar1" PopupButtonID="imgPopup" runat="server" TargetControlID="txtDate"
                                    Format="MM/dd/yyyy">
                                </cc1:CalendarExtender>      
                                                                        
                                <br />
                                (MM/dd/yyyy)
                                              
                            </td>

                            <td style="font-family: Arial; font-size: xx-small; color: #FF0000" 
                                ></td>
                
                        </tr>
            
                        
                        <tr>
                            <td class="style53"></td>
                            <td class="style60">
                                <asp:TextBox ID="txtMRSR" runat="server" Enabled="False" Font-Size="Smaller" 
                                    Width="46px" Visible="False"></asp:TextBox>
                            </td>
                            <td class="style59"></td>
                            <td class="style54">                   
                            </td>
                            <td class="style51"></td>
                            <td class="style58"></td>
                            <td class="style34"></td>
                            <td style="text-align: left" class="style51">
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" 
                                        ErrorMessage="Not Valid." 
                                        ControlToValidate="txtDate"
                                        ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                                        style="font-size: xx-small; font-family: Arial, Helvetica, sans-serif" />
                                                   
                            </td>                
                        </tr>

                        <!-- SEARCH BUTTON -->
                        <tr>
                            <td></td>
                            <td align="left">
                                Customer Contact # 
                            </td>
                            <td >:</td>
                            <td >
                   
                                <asp:TextBox ID="txtCustContact" runat="server"
                                    placeholder="Enter Customer Mobile #"
                                    MaxLength="11" TabIndex="1" 
                                    ToolTip="Please Enter Customer Contact Number"                        
                                    onkeypress="return numeric_only(event)"
                                    CssClass="form-control"
                                    >
                                 </asp:TextBox>                                   
                            </td>
                            <td>                                         
                                <asp:ImageButton ID="btnCustSearch" runat="server" Height="18px" 
                                    ImageUrl="~/Images/search.png" Width="18px" 
                                    data-toggle="tooltip" title="Click here for Search Customer ..."
                                    onclick="btnCustSearch_Click" />                                        
                            </td>
                            <td align="left">City</td>
                            <td >:</td>
                            <td style="text-align: left">
                                <asp:DropDownList ID="ddlCity" runat="server" 
                                    CssClass="form-control"
                                    placeholder="Select Customer City" TabIndex="5">
                                </asp:DropDownList>
                            </td>                
                        </tr>

           
                        <!-- Customer Name -->
                        <tr>
                            <td class="style53"></td>
                            <td style="text-align: Left; font-family: Arial; " 
                                class="style60">Customer Name </td>
                            <td class="style59"> : </td>        
                            <td style="text-align: left" class="style54">
                                <asp:TextBox ID="txtCustName" runat="server"  
                                    TabIndex="2" ToolTip="Please Enter Customer Name" 
                                    MaxLength="50" CssClass="form-control"
                                    placeholder="Enter Customer Name">
                                </asp:TextBox></td>
                
                            <td align="left" class="style51">
                                &nbsp;</td>

                            <!-- Customer DOB -->                
                            <td style="text-align: Left; font-family: Arial; " 
                                class="style58">Date of Birth</td>
                            <td class="style20"> : </td>
                            <td style="text-align: left" >
                                <asp:TextBox ID="txtDOB" runat="server" TabIndex="6" 
                                        ToolTip="Please Enter Customer Date of Birth" MaxLength="10"
                                        CssClass="" Height="25px"
                                        placeholder="MM/dd/yyyy"
                                        ></asp:TextBox> 
                
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
                            <td class="style53"></td>
                            <td style="text-align: Left; font-family: Arial; " 
                                class="style60">Customer Address </td>
                            <td class="style59"> : </td>
                            <td style="text-align: left" class="style54">
                            <asp:TextBox ID="txtCustAdd" runat="server"  
                                TabIndex="3" ToolTip="Enter Customer Address" 
                                MaxLength="150" CssClass="form-control"
                                placeholder="Enter Customer Address">
                            </asp:TextBox></td>
            
                            <!-- Customer Sex -->
                            <td class="style51"></td>
                            <td style="text-align: Left; font-family: Arial; " 
                                class="style58">Sex </td>
                            <td class="style20"> : </td>
                            <td style="text-align: left" >
                                <asp:RadioButtonList ID="optSex" runat="server" width="80%"
                                    RepeatDirection="Horizontal">
                                    <asp:ListItem Selected="True">Male</asp:ListItem>
                                    <asp:ListItem>Female</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                
                            <td class="style20"></td>
                        </tr>
                        
                        <!-- Customer Profession -->
                        <tr>
                            <td class="style53"></td>
                            <td style="text-align: Left; font-family: Arial; " 
                                class="style60">Customer Profession </td>
                            <td class="style59"> : </td>
                            <td style="text-align: left" class="style54">
                                <asp:RadioButtonList ID="optProfession" runat="server" 
                                    RepeatDirection="Horizontal" width="200px">
                                    <asp:ListItem>Business</asp:ListItem>
                                    <asp:ListItem>Service</asp:ListItem>
                                    <asp:ListItem Selected="True">Others</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
            
                            <!-- Customer Organization -->
                            <td class="style51"></td>
                            <td style="text-align: Left; font-family: Arial; " 
                                class="style58">Customer Organization </td>
                            <td class="style20"> : </td>
                            <td style="text-align: left" class="style51">
                                <asp:TextBox ID="txtOrg"  runat="server"                        
                                    placeholder="Enter Organization"
                                    TabIndex="7" 
                                    CssClass="form-control"
                                    ToolTip="Enter Customer Organization ..." MaxLength="50"></asp:TextBox></td>
                
                            <td class="style20"></td>
                        </tr>


                        <!-- Customer Email -->
                        <tr>
                            <td class="style53"></td>
                            <td style="text-align: Left; font-family: Arial; " 
                                class="style60">Customer Email </td>
                            <td class="style59"> : </td>
                            <td style="text-align: left" class="style54">
                         <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" 
                                MaxLength="50" TabIndex="4" 
                                ToolTip="Enter Customer Email Address ..." 
                                placeholder="Enter Email Address">
                            </asp:TextBox>
                           


                            </td>
            
                            <!-- Designation -->
                            <td class="style51"></td>
                            <td style="text-align: Left; font-family: Arial; " 
                                class="style58">Designation </td>
                            <td class="style20"> : </td>
                            <td style="text-align: left" class="style51">
                                <asp:TextBox ID="txtDesg"  runat="server"                        
                                    placeholder="Enter Designation"
                                    TabIndex="8" 
                                    CssClass="form-control"
                                    ToolTip="Enter Customer Designation..." MaxLength="50"></asp:TextBox></td>
                
                            <td class="style20"></td>
                        </tr>

                        <asp:Button runat="server" ClientIDMode="Static" ID="btnchkMail" OnClick="btnchkMail_Click" />

                        <tr>
                            <td class="style53"></td>
                            <td class="style60"></td>
                            <td class="style59"></td>
                            <td class="style54">
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" 
                                    ControlToValidate="txtEmail" ErrorMessage="Invalid Email Address" 
                                    Font-Size="Smaller" ForeColor="Red" SetFocusOnError="True" 
                                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">
                                </asp:RegularExpressionValidator>
                            </td>
                            <td class="style51"></td>
                            <td class="style58"></td>
                            <td></td>                
                            <td class="style51">                    
                                &nbsp;</td>
                            <td class="style20"></td>
                        </tr>
                        
                        <tr style="display:none">
                            <td>&nbsp;</td>
                            <td>CTP Name</td>
                            <td>:</td>
                            <td>                        
                                <asp:Label ID="lblCTPName" runat="server" Text=""></asp:Label>
                                <asp:Label ID="lblEID" runat="server" Visible="False"></asp:Label>
                            </td>
                            <td></td>
                            <td>CTP Address</td>
                            <td>:</td>
                            <td>
                                <asp:Label ID="lblCTPAdd" runat="server" Text=""></asp:Label>
                            </td>
                            
                        </tr>

                        <tr style="display:none">
                            <td>&nbsp;</td>
                            <td>CTP Email</td>
                            <td>:</td>
                            <td>
                                <asp:Label ID="lblCTPEmail" runat="server" Text=""></asp:Label>
                            </td>
                            <td></td>
                            <td>CTP Phone</td>
                            <td>:</td>
                            <td>
                                <asp:Label ID="lblCTPContact" runat="server" Text=""></asp:Label>
                            </td>
                            
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
                            <td width="15%">
                                <asp:Label ID="lblIncentiveType" runat="server" Text="0" Visible="False"></asp:Label>
                                <asp:Label ID="lblBLIPAmnt" runat="server" Text="0" Visible="False"></asp:Label>
                                <asp:Label ID="lblIncentiveAmnt" runat="server" Text="0" Visible="False"></asp:Label>
                            </td>
                            <td width="3%">
                                <asp:Label ID="lblGetIncentive" runat="server" Text="0" Visible="False"></asp:Label>
                            </td>
                            <td width="25%">
                                <asp:Label ID="lblWPPrice" runat="server" Text="0" Visible="False"></asp:Label>
                                <asp:Label ID="lblBLIPofWP" runat="server" Text="0" Visible="False"></asp:Label>
                            </td>
                            <td width="10%">
                                <asp:Label ID="lblWPIncentive" runat="server" Text="0" Visible="False"></asp:Label>
                                <asp:Label ID="lblWPMinQty" runat="server" Text="0" Visible="False"></asp:Label>
                            </td>
                            <td width="15%">
                                <asp:Label ID="lblUP" runat="server" Text="0" Visible="False"></asp:Label>
                            </td>
                            <td width="3%"></td>
                            <td width="25%"></td>
                            <td width="2%"></td>                            
                        </tr>

                        <tr>
                            <td class="style53"></td>
                            <td class="style60" align="left">Product Code</td>
                            <td class="style59"> : </td> 
                            <td class="style54" align="left">
                                <asp:TextBox ID="txtCode" runat="server" AutoPostBack="True" 
                                    Width="200px" CssClass="form-control"
                                    placeholder="Enter Product Code"
                                    onkeypress="return numeric_only(event)"
                                    ontextchanged="txtCode_TextChanged" TabIndex="9">
                                </asp:TextBox>
                            </td>
                            <td class="style51"></td>
                            <td align="left">Search Model</td>
                            <td class="style26">:</td>
                            <td align="left">
                                <asp:TextBox ID="txtModel" runat="server" Height="25px" Width="200px" 
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
            
                        <!-- Product Model -->
                        <tr>
                            <td ></td>
                            <td style="text-align: Left; font-family: Arial; font-weight: 700; color: #000000;" 
                                >Product Model</td>
                            <td > : </td>              
                            <td>                                                  
                                <asp:DropDownList ID="ddlContinents" class="chzn-select"
                                    runat="server" AutoPostBack = "True"
                                    data-placeholder="Choose a Model ..."
                                    OnSelectedIndexChanged="ddlContinents_SelectedIndexChanged"                         
                                    Width="200px" Height="32px" TabIndex="11"
                                    ToolTip="Please Select Product Model ...">                                    
                                </asp:DropDownList>                                
                            </td>
                            <td align="left">
                                 &nbsp;</td>

                            <!-- Product Description -->                
                            <td style="text-align: Left; font-family: Arial; " >
                                Product Description </td>
                            <td > : </td>
                            <td style="text-align: left" >
                                <asp:TextBox ID="txtProdDesc" runat="server" Width="200px" CssClass="form-control"
                                    style="font-weight: 500" Enabled="False"></asp:TextBox></td>
                            <td ></td>
                        </tr>            
            
                         <!-- Unit Price -->
                        <tr>
                            <td class="style53"></td>
                            <td style="text-align: Left; font-family: Arial; color: #003300; font-size: small;" 
                                class="style60">MRP (Tk.)</td>
                            <td class="style59"> : </td>
                            <td style="text-align: left" class="style54">
                            <asp:TextBox ID="txtUP" runat="server" Width="200px" CssClass="form-control"
                                style="font-weight: 700" Enabled="False"></asp:TextBox></td>
                            <td class="style51">
                                <asp:TextBox ID="txtProdID" runat="server" Width="6px" 
                                    style="font-weight: 700" Visible="False"></asp:TextBox> 
                                <asp:TextBox ID="txtBLIPAmnt" runat="server" Width="6px" 
                                    style="font-weight: 700" Visible="False"></asp:TextBox> 
                            </td>

                            <!-- Campaign Price -->                
                            <td style="text-align: Left; font-family: Arial; color: #003300; font-size: small;" 
                                class="style58">Campaign Price (Tk.) </td>
                            <td class="style20"> : </td>
                            <td style="text-align: left" class="style51">
                            <asp:TextBox ID="txtCP" runat="server" Width="200px" CssClass="form-control"
                                style="font-weight: 700" Enabled="False"></asp:TextBox></td>
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
                                    CssClass="form-control" placeholder="Enter Quantity"
                                    ToolTip="Please Enter Product Quantity" TabIndex="12" MaxLength="5">
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
                                style="font-weight: 700" Enabled="False"></asp:TextBox></td>
                            <td class="style20"></td>
                        </tr>

                        
                        <!-- Discount Code -->
                        <tr>
                            <%--<td class="style53"></td>
                            <td style="text-align: Left; font-family: Arial; " 
                                class="style60">Discount Code</td>
                            <td class="style59"> : </td>
                            <td style="text-align: left" class="style54">
                            <asp:TextBox ID="txtDisCode" runat="server" Width="200px" CssClass="form-control"
                                placeholder="Enter Promo code"
                                style="font-weight: 700" Enabled="True"></asp:TextBox></td>--%>
                
                            <!-- Discount Reference -->

                            <td class="style53"></td>
                            <td style="text-align: Left; font-family: Arial; font-weight: 700; color: #b22222; font-size: small;" 
                                class="style60">Ref. for Discount</td>
                            <td class="style59"> : </td>
                            <td style="text-align: left; width:200px;" class="style54">
                                <asp:DropDownList ID="ddlRefDiscount" runat="server" 
                                                  CssClass="form-control" style = "width: 200px;"
                                                  placeholder="Select Ref. Source" TabIndex="5" OnSelectedIndexChanged="ddlRefDiscount_SelectedIndexChanged" AutoPostBack="true">
                                   <%-- <asp:ListItem Text="Select Ref. Source" Value="" />
                                    <asp:ListItem Text="Online Order" Value="Online Order" />
                                    <asp:ListItem Text="Free Gift" Value="Free Gift" />
                                    <asp:ListItem Text="GM Sir (Marketing)" Value="GM Sir" />
                                    <asp:ListItem Text="DGM Sir (Sales)" Value="" />--%>
                                </asp:DropDownList>
                                <asp:CheckBox runat="server" ID="chkTblOffer" Text="Tbl 5% DisCount" AutoPostBack="true" ViewStateMode="Enabled" ClientIDMode="Static"
                                    oncheckedchanged="chkTblOffer_CheckedChanged"/>
                            </td>    

                            <!-- Discount Amount -->
                            <td class="style51">
                                <asp:Label ID="lblDisPerPromoCode" runat="server" Text="0" Visible="False"></asp:Label>
                                <asp:Label ID="lblDisAmntPromoCode" runat="server" Text="0" Visible="False"></asp:Label>
                            </td>
                            <td style="text-align: Left; font-family: Arial; color: #FF0000;" 
                                class="style58">Discount Amount</td>
                            <td class="style20"> : </td>
                            <td style="text-align: left" class="style51">
                                <asp:TextBox ID="txtDisAmnt" runat="server" AutoPostBack="True" Width="200px" 
                                    OnTextChanged="txtDisAmnt_TextChanged"
                                    placeholder="Discount Amount" Enabled="true"
                                    onkeypress="fnAllowNumeric();" CssClass="form-control"
                                    ToolTip="Discount Amount" TabIndex="13" MaxLength="6">
                                </asp:TextBox>
                            </td>
                            <td class="style20"></td>
                        </tr>


                        <tr>
                            <!-- Online Order No -->
                            <td class="style51"></td>
                            <td style="text-align: Left; font-family: Arial; " 
                                class="style58">Online Order# / Voucher Code</td>
                            <td class="style20"> : </td>
                            <td style="text-align: left" class="style51">
                                <asp:TextBox ID="txtOnlineOrder" runat="server" AutoPostBack="True" Width="200px" 
                                             CssClass="form-control"
                                             placeholder="Order/Code No" TabIndex="15"></asp:TextBox>
                            </td>
                            <!-- Withdrawn/Adjustment Amount -->
                            <td class="style51"></td>
                            <td style="text-align: Left; font-family: Arial; " 
                                class="style58">With/Adjust Amount</td>
                            <td class="style20"> : </td>
                            <td style="text-align: left" class="style51">
                                <asp:TextBox ID="txtWithAdj" runat="server" AutoPostBack="True" Width="200px" 
                                    CssClass="form-control" 
                                    ReadOnly="true"
                                    placeholder="With/Adjust Amnt"
                                    OnTextChanged="txtWithAdj_TextChanged"
                                    onkeypress="return numeric_only(event)"
                                    ToolTip="Withdrawn/Adjustment Amount..." TabIndex="15" MaxLength="6"></asp:TextBox>
                            </td>
                       </tr>
                        <tr>
                        <!-- Net Amount -->
                            <td class="style53"></td>
                            <td class="style60"></td>
                            <td class="style59"></td>
                            <td class="style54"></td>
                            <td class="style51"></td>
                            <td style="text-align: Left; font-family: Arial; font-weight: 700; color: #008000;" 
                                class="style58">Net Amount (Tk.)</td>
                            <td class="style52"> : </td>
                            <td style="text-align: left" class="style51">
                            <asp:TextBox ID="txtNet" runat="server" Width="200px" CssClass="form-control"
                                style="font-weight: 700" Enabled="False"></asp:TextBox></td>
                        </tr>

                         <!-- Product SL# -->
                        <tr>
                            <td class="style53"></td>
                            <td style="text-align: Left; font-family: Arial;" 
                                class="style60">Product SL#</td>
                            <td class="style59"> : </td>
                            <td style="text-align: left" class="style54">
                                <asp:TextBox ID="txtSL" runat="server" Width="200px" 
                                    CssClass="form-control"
                                    ToolTip="Please Enter Product Serial Number" 
                                    placeholder="Enter Product SL#"
                                    TabIndex="16" MaxLength="20">
                                </asp:TextBox>
                            </td>

                            
                            <td class="style51"></td>
                            <td style="text-align: Left; font-family: Arial; " 
                                class="style58">&nbsp;</td>
                            <td class="style20"> &nbsp;</td>
                            <td style="text-align: left" class="style51">
                                <%--<asp:HiddenField ID="GroupName" runat="server" />--%>
                                <asp:Label ID="GroupName" runat="server" Text="Label" visible="false"></asp:Label>
                            </td>
                            <td class="style20"></td>
                        </tr>
                        <!-- Remarks -->
                      <%--  <tr>
                            <td class="style53"></td>
                            <td style="text-align: left" class="style60" >Remarks</td>
                            <td class="style59">:</td>
                            <td class="style54" colspan="5">
                                <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" 
                                    MaxLength="50" placeholder="Sales Offer/Discount Reason" TabIndex="17" 
                                    Width="700px">
                            </asp:TextBox>
                            </td>
                            
                            <td class="style54"></td>
                            <td class="style54"></td>
                        </tr>--%>

                        <tr>
                            <td class="style53"></td>
                            <td class="style60"></td>
                            <td class="style59"></td>
                            <td class="style54">&nbsp;</td>
                            <td class="style54"></td>
                            <td class="style54"></td>
                            <td class="style54"></td>
                            <td class="style54"></td>
                            <td class="style54"></td>
                        </tr>
                       
                        <!-- Add to Data Grid -->
                        <tr>
                            <td class="style53"></td>
                            <td class="style60"></td>
                            <td class="style59"></td>
                            <td style="text-align: left" >
                                <asp:Button ID="btnAdd" runat="server" Text="Add Product in List" 
                                    OnClick="btnAdd_Click"
                                    OnClientClick="return onAddButtonClick();"                                     
                                    CssClass="btn btn-success"
                                    ToolTip="Click here for add product in list ..." TabIndex="18" 
                                    />
                            </td>                
                        </tr>
            
                        <tr>
                            <td class="style53"></td>
                            <td class="style60"></td>
                            <td class="style59"></td>
                            <td class="style54"></td>
                            <td class="style54"></td>
                            <td class="style54"></td>
                            <td class="style54"></td>
                            <td class="style54"></td>
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

                                        <asp:BoundField HeaderText="BLAmnt" DataField="BLIPAmnt" ItemStyle-Width="" />
                                        <asp:BoundField HeaderText="Inc" DataField="IncentiveAmnt" ItemStyle-Width="" />
                                        <asp:BoundField HeaderText="IncType" DataField="IncentiveType" ItemStyle-Width="" />
                                        <asp:BoundField HeaderText="" DataField="CustShowPrice" ItemStyle-Width="" Visible="false"/>

                                        


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

                   <asp:Label runat="server" CssClass="Delivary_charge" ID="lblDelivary_charge" Text="Delivary Cahrge"></asp:Label>
  
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

    <!-- VOUCHER INFO -->
       
        <asp:UpdatePanel ID="UpdatePanel4" runat="server" Visible="True">
            <ContentTemplate>
                <asp:Panel ID="Panel1" runat="server" CssClass="cpHeader" BackColor="Black">
                    <asp:Label ID="Label8" runat="server" Font-Bold="True" Font-Size="14pt" 
                        ForeColor="Orange" >Voucher Information (Redeem Point Here ...)</asp:Label>
                </asp:Panel>
                
                <asp:Panel ID="Panel2" runat="server" CssClass="cpBody">
                <div align="center">
        
                    <table width="100%" style="font-family: Tahoma; font-size: small">
                    
                        <tr>
                            <td width="2%"></td>
                            <td width="15%"></td>
                            <td width="3%"></td>
                            <td width="25%"></td>
                            <td width="10%"></td>
                            <td width="15%"></td>
                            <td width="3%"></td>
                            <td width="25%"></td>
                            <td width="2%"></td>                            
                        </tr>

                         <!-- SEARCH BUTTON -->
                        <tr>
                            <td></td>
                            <td align="left">
                                Customer Contact # 
                            </td>
                            <td >:</td>
                            <td >
                   
                                <asp:TextBox ID="txtCustomerMobile" runat="server"
                                    placeholder="Enter Customer Mobile #"
                                    MaxLength="11" TabIndex="1" 
                                    ToolTip="Please Enter Customer Contact Number"                        
                                    onkeypress="return numeric_only(event)"
                                    CssClass="form-control"
                                    >
                                 </asp:TextBox>                                   
                            </td>
                            <td>                                         
                                <asp:ImageButton ID="btnVoucherSearch" runat="server" Height="18px" 
                                    ImageUrl="~/Images/search.png" Width="18px" 
                                    data-toggle="tooltip" title="Click here for Search Customer ..." 
                                    onclick="btnVoucherSearch_Click"/>                                        
                            </td>
                   
                            <!-- Redeem Point -->                
                            <td style="text-align: Left; font-family: Arial; padding-top:5px;" 
                                class="style58">Redeem Point</td>
                            <td class="style20"> : </td>
                            <td style="text-align: left; padding-top:5px;" >
                                <asp:TextBox ID="txtRedeemPoint" runat="server" TabIndex="6"
                                        MaxLength="2" CssClass="form-control" Width="200px"></asp:TextBox> 
                                <%--<asp:RangeValidator ID="txtRedeemPointReg" runat="server" ControlToValidate="txtRedeemPoint" CssClass="required"
                                       ErrorMessage="Maximum 10 point can be redeem in 1 voucher"  MaximumValue="9" MinimumValue="0"></asp:RangeValidator>--%>
                            </td>       
                        </tr>
            
                        <!-- Available Point -->
                        <tr>
                            <td class="style53"></td>

                             <td align="left"><p style="color:Green;">Available Point</p></td>
                            <td >:</td>
                            <td style="text-align: left; padding-top:5px;" >
                                <asp:TextBox ID="txtAvailablePoint" runat="server" TabIndex="6" ReadOnly="true"
                                        style="text-align: center;" CssClass="form-control" Width="200px"></asp:TextBox> 
                            </td>
                            <td> &nbsp;</td>
                             <td style="text-align: Left; font-family: Arial; " 
                                class="style58"></td>
                            <td class="style20"> </td>
                            <td style="text-align: left; padding-top:5px;" >
                                 <asp:Button ID="btnRedeem" runat="server" Text="Add Redeem Point"                                    
                                    CssClass="btn btn-success" ToolTip="Click here for add product in list ..." 
                                     TabIndex="18" onclick="btnRedeem_Click"/>
                            </td> 
                        </tr>

                    
                    </table>

                </div>

            </asp:Panel>
 
        <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender4" runat="server" TargetControlID="Panel2" CollapseControlID="Panel1" ExpandControlID="Panel1"
                Collapsed="false" TextLabelID="lblText" CollapsedText="Click to Show Voucher Info ..." ExpandedText="Click to Hide Voucher Info..."
                CollapsedSize="0">
            </cc1:CollapsiblePanelExtender>
        </ContentTemplate>                   

    </asp:UpdatePanel>
       <!-- END VOUCHER INFO -->
       <!-- Spin Wheel -->
        <asp:UpdatePanel ID="UpdatePanel7" runat="server" Visible="False">
            <ContentTemplate>
                <asp:Panel ID="Panel7" runat="server" CssClass="cpHeader" BackColor="Black">
                    <asp:Label ID="Label12" runat="server" Font-Bold="True" Font-Size="14pt" 
                        ForeColor="Orange" >Goal and Score</asp:Label>
                </asp:Panel>
                
                <asp:Panel ID="Panel8" runat="server" CssClass="cpBody">
                <div align="center">
        
                    <table width="100%" style="font-family: Tahoma; font-size: small">
                    
                        <tr>
                            <td width="2%"></td>
                            <td width="15%"></td>
                            <td width="3%"></td>
                            <td width="25%"></td>
                            <td width="10%"></td>
                            <td width="15%"></td>
                            <td width="3%"></td>
                            <td width="25%"></td>
                            <td width="2%"></td>                            
                        </tr>

                         <!-- SEARCH BUTTON -->
                        <tr>
                            <td></td>
                            <td align="left">
                                Coupon Code 
                            </td>
                            <td >:</td>
                            <td >
                   
                                <asp:TextBox ID="TxtSpinCouponNumber" runat="server"
                                    placeholder="Enter Coupon Number #"
                                    MaxLength="11" TabIndex="1" 
                                    ToolTip="Please Enter Coupon Number"                        
                                    onkeypress="return numeric_only(event)"
                                    CssClass="form-control"
                                    >
                                 </asp:TextBox>                                   
                            </td>
                            <td>                                         
                                <asp:ImageButton ID="btnCouponSearch" runat="server" Height="18px" 
                                    ImageUrl="~/Images/search.png" Width="18px" 
                                    data-toggle="tooltip" title="Click here for Search Coupon ..." 
                                    onclick="btnCouponSearch_Click"/>                                        
                            </td>
                   
                            <!-- Redeem Point -->                
                            <td style="text-align: Left; font-family: Arial; padding-top:5px;" 
                                class="style58">Coupon Discount</td>
                            <td class="style20"> : </td>
                            <td style="text-align: left; padding-top:5px;" >
                                <asp:TextBox ID="txtSpinCpouponDiscountAmnt" runat="server" TabIndex="6"
                                        MaxLength="2" CssClass="form-control" Width="200px"></asp:TextBox> 
                                <%--<asp:RangeValidator ID="txtRedeemPointReg" runat="server" ControlToValidate="txtRedeemPoint" CssClass="required"
                                       ErrorMessage="Maximum 10 point can be redeem in 1 voucher"  MaximumValue="9" MinimumValue="0"></asp:RangeValidator>--%>
                            </td>       
                        </tr>
            
                        <!-- Available Point -->
                        <tr>
                            <td class="style53"></td>

                             <td align="left"><p style="color:Green;">Validated For</p></td>
                            <td >:</td>
                            <td style="text-align: left; padding-top:5px;" >
                                <asp:TextBox ID="txtSpinModel" runat="server" TabIndex="6" ReadOnly="true"
                                        style="text-align: center;" CssClass="form-control" Width="200px" Visible="true"></asp:TextBox> 
                            </td>
                            <td> &nbsp;</td>
                             <td style="text-align: Left; font-family: Arial; " 
                                class="style58"></td>
                            <td class="style20"> </td>
                            <td style="text-align: left; padding-top:5px;" >
                                 <asp:Button ID="btnSpinCoupon" runat="server" Text="Add Coupon Discount"                                    
                                    CssClass="btn btn-success" ToolTip="Click here for add product in list ..." 
                                     TabIndex="18" onclick="btnSpinCoupon_Click"/>
                            </td> 
                        </tr>
                        <tr>
                            <td class="style53"></td>

                             <td align="left"><p style="color:Green;">Product SL(Of Spinned Model)</p></td>
                            <td >:</td>
                            <td style="text-align: left; padding-top:5px;">
                                <asp:TextBox ID="SpinModelProdSLNo" runat="server" TabIndex="6" ReadOnly="false"
                                        style="text-align: center;" CssClass="form-control" Width="200px" Visible="true"></asp:TextBox> 
                            </td>
                            <td> &nbsp;</td>
                             <td style="text-align: Left; font-family: Arial; " 
                                class="style58"></td>
                            <td class="style20"> </td>
                            <td style="text-align: left; padding-top:5px;" >
                                
                            </td> 
                        </tr>
                    
                    </table>

                </div>

            </asp:Panel>
 
        <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender6" runat="server" TargetControlID="Panel8" CollapseControlID="Panel7" ExpandControlID="Panel7"
                Collapsed="false" TextLabelID="lblText" CollapsedText="Click to Show Voucher Info ..." ExpandedText="Click to Hide Voucher Info..."
                CollapsedSize="0">
            </cc1:CollapsiblePanelExtender>
        </ContentTemplate>                   

    </asp:UpdatePanel>
       <!-- Spin Wheel -->
       <!-- GENERATE DIGITAL COUPON -->
       
<%--       <asp:UpdatePanel ID="UpdatePanel7" runat="server" Visible="false">
            <ContentTemplate>
                <asp:Panel ID="Panel1" runat="server" CssClass="cpHeader">
                    <asp:Label ID="Label8" runat="server" >Digital Cash Return (Generate Coupon Here ...)</asp:Label>
                </asp:Panel>
                
                <asp:Panel ID="Panel2" runat="server" CssClass="cpBody">
                <div align="center">
        
                    <table width="100%" style="font-family: Tahoma; font-size: small">
                    
                        <tr>
                            <td width="2%"></td>
                            <td width="15%"></td>
                            <td width="3%"></td>
                            <td width="25%"></td>
                            <td width="10%"></td>
                            <td width="15%"></td>
                            <td width="3%"></td>
                            <td width="25%"></td>
                            <td width="2%"></td>                            
                        </tr>

                         <!-- SEARCH BUTTON -->
                        <tr>
                             <td class="style53"></td>
                            <td class="style60" align="left"></td>
                            <td class="style54" align="right">                                                                 
                                <asp:ImageButton ID="btnGenerateCoupon" runat="server" Height="60px" 
                                    ImageUrl="~/Images/discount_coupon.png" Width="150px" OnClick="btnGenerateCoupon_OnClick"
                                    data-toggle="tooltip" title="Click here for Generate Coupon ..."/>                                        
                            </td>
                            <td></td>
                            <td>
                            </td>
                            <!-- Redeem Point -->                
                            <td style="text-align: Left; font-family: Arial; padding-top:5px;" 
                                class="style58">Coupon Code</td>
                            <td class="style20"> : </td>
                            <td style="text-align: left; padding-top:5px;" >
                                <asp:TextBox ID="txtCouponCode" runat="server" TabIndex="6" ReadOnly="true"
                                             style="text-align: center; text-decoration: brown;"  MaxLength="1" 
                                             CssClass="form-control" Width="200px"></asp:TextBox>
                            </td>       
                        </tr>
            
                        <!-- Available Point -->
                        <tr>
                            <td class="style53"></td>
                            <td class="style20"> </td>
                            <td style="text-align: left; padding-top:5px;" >
                                 <asp:Button ID="btnRedeem" runat="server" Text="Redeem Coupon" Visible="False"                                    
                                    CssClass="btn btn-success" TabIndex="18" OnClick="btnRedeem_OnClick"/>
                            </td> 
                            
                            <td> &nbsp;</td>
                            <td style="text-align: Left; font-family: Arial; " 
                                class="style58"></td>
                            <td align="left"><p style="color:Green;">Discount Amount</p></td>
                            <td >:</td>
                            <td style="text-align: left; padding-top:5px;" >
                                <asp:TextBox ID="txtDiscountAmount" runat="server" TabIndex="6" ReadOnly="true"
                                             style="text-align: center;" CssClass="form-control" Width="200px"></asp:TextBox> 
                            </td>
                        </tr>

                    
                    </table>

                </div>

            </asp:Panel>
 
        <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender4" runat="server" TargetControlID="Panel2" CollapseControlID="Panel1" ExpandControlID="Panel1"
                Collapsed="false" TextLabelID="lblText" CollapsedText="Click to Show Voucher Info ..." ExpandedText="Click to Hide Voucher Info..."
                CollapsedSize="0">
            </cc1:CollapsiblePanelExtender>
        </ContentTemplate>                   

    </asp:UpdatePanel>--%>
       
       <!-- END DIGITAL COUPON -->

        <!-- GENERATE GP/B.Link/Nagad Offer -->
       
    <asp:UpdatePanel ID="UpdatePanel5" runat="server" Visible="true">
            <ContentTemplate>
                <asp:Panel ID="Panel3" runat="server" CssClass="cpHeader">
                    <asp:Label ID="Label10" runat="server">Avail Offer Here..</asp:Label>
                </asp:Panel>
                
                <asp:Panel ID="Panel4" runat="server" CssClass="cpBody">
                <div align="center">
        
                    <table width="100%" style="font-family: Tahoma; font-size: small">
                    
                        <tr>
                            <td width="2%"></td>
                            <td width="15%"></td>
                            <td width="3%"></td>
                            <td width="25%"></td>
                            <td width="10%"></td>
                            <td width="15%"></td>
                            <td width="3%"></td>
                            <td width="25%"></td>
                            <td width="2%"></td>                            
                        </tr>

                         <!-- SEARCH BUTTON -->
                        <tr>
                            <td class="style53"></td>
                            <td class="style60" align="left"></td>
                            <td class="style59"></td> 
                            <td class="style54" align="left">  
                                <asp:DropDownList ID="ddlReference" runat="server" AutoPostBack="True" 
                                                  CssClass="form-control" style = "width: 200px;"
                                                  placeholder="Select Ref. Source" TabIndex="5" 
                                    onselectedindexchanged="ddlReference_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>  
                            <!-- Redeem Point -->      
                            <td></td>
                            <td class="padding-top:5px;" align="left">Avail Amount</td>
                            <td class="style59">:</td>
                            <td class="style54" align="left">
                                <asp:TextBox ID="txtAvailAmount" runat="server" TabIndex="6" ReadOnly="true"
                                             style="text-align: center;" CssClass="form-control" Width="200px"></asp:TextBox> 
                            </td>   
                             <td class="style26"></td>	  
                        </tr>
                        <tr>
                            <td class="style53"></td>
                            <td class="style60"></td>
                            <td class="style59"></td>
                            <td class="style54">&nbsp;</td>
                            <td class="style54"></td>
                            <td class="style54"></td>
                            <td class="style54"></td>
                            <td class="style54"></td>
                            <td class="style54"></td>
                        </tr>
                        <!-- Available Point -->
                        <tr>
                            <td class="style53"></td>
                            <td class="style60" align="left"></td>
                            <td class="style59"></td> 
                            <td class="style54" align="left">  
                             <asp:Button ID="btnAvailOffer" runat="server" Text="Avail Offer"                                
                                    CssClass="btn btn-success"
                                    ToolTip="Click here for avail offer..." TabIndex="18" onclick="btnAvailOffer_Click" 
                                    />     
                             </td>
                            
                            <td> &nbsp;</td>
                            <td style="text-align: Left; font-family: Arial; " 
                                class="style58"></td>
                            <td align="left"></td>
                            <td></td>
                            <td style="text-align: left; padding-top:5px;" >
                            </td>
                        </tr>

                    
                    </table>

                </div>

            </asp:Panel>
 
        <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender5" runat="server" TargetControlID="Panel4" CollapseControlID="Panel3" ExpandControlID="Panel3"
                Collapsed="false" TextLabelID="lblText" CollapsedText="Click to Show Voucher Info ..." ExpandedText="Click to Hide Voucher Info..."
                CollapsedSize="0">
            </cc1:CollapsiblePanelExtender>
        </ContentTemplate>                   

    </asp:UpdatePanel>
       
       <!-- END DIGITAL COUPON -->

   </div>   

      


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
                            <td width="25%"></td>
                            <td width="10%"></td>
                            <td width="15%"></td>
                            <td width="3%"></td>
                            <td width="25%"></td>
                            <td width="2%"></td>                            
                        </tr>
                        <tr>
                            <td ></td>
                            <td align="left"                     
                                
                                style="color: #006600; font-weight: bold; font-family: Tahoma; font-size: large;" 
                                class="style2">Cash Amount</td>
                            <td style="text-align: center">:</td>
                            <td align="left">
                                <asp:TextBox ID="txtCash" runat="server" Width="200px" 
                                    CssClass="form-control"
                                    placeholder="Enter Cash Amount"
                                    onkeypress="return numeric_only(event)"
                                    ToolTip="Please Enter Cash Amount..." 
                                    MaxLength="8" ontextchanged="txtCash_TextChanged" TabIndex="19" 
                                    AutoPostBack="True"></asp:TextBox>
                            </td>
                
                            <td ></td>
                            <td align="left" 
                                style="color: #FF0000; font-family: Tahoma; font-size: large;">Due Amount</td>
                            <td style="text-align: center" >:</td>
                            <td align="left">
                                <asp:TextBox ID="txtDue" runat="server" Width="200px" 
                                    style="font-weight: 700" CssClass="form-control"
                                    onkeypress="return numeric_only(event)"
                                    ToolTip="Due Amount" 
                                    MaxLength="10" ReadOnly="True" BackColor="#FFCCFF">0</asp:TextBox>
                            </td>
                            <td></td>
                        </tr>

                        <tr>
                            <td ></td>
                            <td align="left" style="color: #008080" class="style2">&nbsp;</td>
                            <td style="text-align: center" >&nbsp;</td>
                            <td align="left">
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
                
                            <td ></td>
                            <td align="left" style="color: #FF0000">&nbsp;</td>
                            <td style="text-align: center" >&nbsp;</td>
                            <td align="left">
                                &nbsp;</td>
                            <td ></td>
                        </tr>
                        <tr>
                            <td class="style1"></td>
                            <td align="left"  class="style2"                    
                                style="font-family: Tahoma; color: #000080; font-size: small; font-weight: normal">
                                <asp:Label ID="Label15" runat="server" Text="Payment Mode 1" Font-Bold="True" 
                                    Font-Names="Tahoma" ForeColor="Red" Font-Size="12pt"></asp:Label>
                            </td>
                            <td style="text-align: center" class="style3">:</td>
                            <td align="left" class="style4">
                                
                            </td>
                
                            <td class="style5"></td>
                            <td align="left" class="style6">
                                <asp:Label ID="Label16" runat="server" Text="Payment Mode 2" Font-Bold="True" 
                                    Font-Names="Tahoma" ForeColor="Red" Font-Size="12pt"></asp:Label>
                                </td>
                            <td style="text-align: center" class="style7">:</td>
                            <td align="left" class="style8">
                                
                            </td>
                            <td class="style9"> &nbsp;</td>
                        </tr>

                        <!-- CARD AMNT -->
                        <tr>
                            <td class="style1"></td>
                            <td align="left"  class="style2"                    
                                style="font-family: Tahoma; color: #000080; font-size: small; font-weight: normal">
                                <asp:Label ID="Label2" runat="server" Text="Card 1 Amnt/IPDC Amnt" Font-Bold="True" 
                                    Font-Names="Tahoma" ForeColor="#003366"></asp:Label>
                            </td>
                            <td style="text-align: center" class="style3">:</td>
                            <td align="left" class="style4">
                                <asp:TextBox ID="txtCardAmnt1" runat="server" Width="200px" 
                                    onkeypress="return numeric_only(event)" CssClass="form-control"
                                    ToolTip="Amount from Card 1 ..." 
                                    placeholder="Card 1 Amount"
                                    MaxLength="8" Visible="True" TabIndex="20" AutoPostBack="True" 
                                    ontextchanged="txtCardAmnt1_TextChanged"></asp:TextBox>
                            </td>
                
                            <td class="style5"></td>
                            <td align="left" class="style6">
                                <asp:Label ID="Label3" runat="server" Text="Card 2 Amnt" Font-Bold="True" 
                                    Font-Names="Tahoma" ForeColor="#003366"></asp:Label>
                                </td>
                            <td style="text-align: center" class="style7">:</td>
                            <td align="left" class="style8">
                                <asp:TextBox ID="txtCardAmnt2" runat="server" Width="200px" 
                                    onkeypress="return numeric_only(event)" CssClass="form-control"
                                    ToolTip="Amount from Card 2 ..." 
                                    placeholder="Card 2 Amount"
                                    MaxLength="8" Visible="True" TabIndex="26" AutoPostBack="True" 
                                    ontextchanged="txtCardAmnt2_TextChanged"></asp:TextBox>
                            </td>
                            <td class="style9"> &nbsp;</td>
                        </tr>

                        <tr>
                            <td ></td>
                            <td align="left"  
                                style="font-family: Tahoma; font-size: small; font-weight: normal" 
                                class="style2">
                                <asp:Label ID="lblNo" runat="server" Text="Card/IPDC #" Visible="True"></asp:Label>
                                </td>
                            <td style="text-align: center" >:</td>
                            <td align="left">
                                <asp:TextBox ID="txtChequeNo" runat="server" Width="200px" 
                                    ToolTip="Card Number ..." CssClass="form-control"
                                    placeholder="Card 1 Number"
                                    onkeypress="return numeric_only(event)"
                                    MaxLength="16" Visible="True" TabIndex="21">
                                </asp:TextBox>
                            </td>
                
                            <td ></td>
                            <td align="left">
                                <asp:Label ID="lblNo0" runat="server" Text="Card #" Visible="True"></asp:Label>
                                </td>
                            <td style="text-align: center" >:</td>
                            <td align="left">
                                <asp:TextBox ID="txtChequeNo2" runat="server" Width="200px" 
                                    ToolTip="Card Number ..." CssClass="form-control"
                                    placeholder="Card 2 Number"
                                    onkeypress="return numeric_only(event)"
                                    MaxLength="16" Visible="True" TabIndex="27"></asp:TextBox>
                            </td>
                            <td> &nbsp;</td>
                        </tr>
                                               

                        <tr>
                            <td ></td>
                            <td align="left"   
                                style="font-family: Tahoma; font-size: small; font-weight: normal" 
                                class="style2">
                                <asp:Label ID="Label1" runat="server" Text="Card Type" Visible="True"></asp:Label>
                                </td>
                            <td style="text-align: center">:</td>
                            <td align="left">
                                <asp:DropDownList ID="ddlCardType1" CssClass="form-control" runat="server" TabIndex="22" Width="200px">
                                    <asp:ListItem>VISA</asp:ListItem>
                                    <asp:ListItem>EMEI</asp:ListItem>
                                    <asp:ListItem>MASTER</asp:ListItem>
                                    <asp:ListItem>AMEX</asp:ListItem>
                                    <asp:ListItem>NAGAD</asp:ListItem>
                                    <asp:ListItem>BKash</asp:ListItem>                                    
                                    <asp:ListItem>OTHERS</asp:ListItem>
                                    <asp:ListItem>IPDC</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                
                            <td ></td>
                            <td align="left">
                                <asp:Label ID="Label4" runat="server" Text="Card Type" Visible="True"></asp:Label>
                                </td>
                            <td style="text-align: center">:</td>
                            <td align="left">
                                <asp:DropDownList ID="ddlCardType2" CssClass="form-control" runat="server" TabIndex="28" Width="200px">
                                    <asp:ListItem>VISA</asp:ListItem>                                   
                                    <asp:ListItem>MASTER</asp:ListItem>
                                    <asp:ListItem>AMEX</asp:ListItem>
                                    <asp:ListItem>NAGAD</asp:ListItem>
                                    <asp:ListItem>BKash</asp:ListItem>
                                    <asp:ListItem>OTHERS</asp:ListItem>
                                    
                                </asp:DropDownList>
                            </td>
                            <td></td>
                        </tr>
                        <tr>
                            <td ></td>
                            <td align="left"   
                                style="font-family: Tahoma; font-size: small; font-weight: normal" 
                                class="style2">
                                <asp:Label ID="Label13" runat="server" Text="EMEI Tenure" Visible="True"></asp:Label>
                                </td>
                            <td style="text-align: center">:</td>
                            <td align="left">

                            <asp:DropDownList ID="ddlEMEIInfo" runat="server" AutoPostBack="True" 
                                                  CssClass="form-control" style = "width: 200px;"
                                                  placeholder="Select EMEI Tenure" TabIndex="5" 
                                    onselectedindexchanged="ddlEMEIInfo_SelectedIndexChanged">
                                </asp:DropDownList>

                                <%--<asp:DropDownList ID="ddlEMEIInfo" CssClass="form-control" runat="server" 
                                    TabIndex="22" Width="200px" 
                                    onselectedindexchanged="ddlEMEIInfo_SelectedIndexChanged">
                                <asp:ListItem>0 Month</asp:ListItem>
                                    <asp:ListItem>3 Month</asp:ListItem>
                                    <asp:ListItem>6 Month</asp:ListItem>
                                    <asp:ListItem>12 Month</asp:ListItem>
                                    <asp:ListItem>18 Month</asp:ListItem>
                                    <asp:ListItem>24 Month</asp:ListItem>
                                    <asp:ListItem>36 Month</asp:ListItem>
                                </asp:DropDownList>--%>
                            </td>
                
                            <td ></td>
                            <td align="left">
                                <asp:Label ID="Label14" runat="server" Text="" Visible="True"></asp:Label>
                                </td>
                            <td style="text-align: center">:</td>
                            <td align="left">                                
                            </td>
                            <td></td>
                        </tr>
                        <tr>
                            <td ></td>
                            <td align="left"                     
                                
                                style="color: #006600; font-weight: bold; font-family: Tahoma; font-size: large;" 
                                class="style2">EMI Down Payment</td>
                            <td style="text-align: center">:</td>
                            <td align="left">
                                <asp:TextBox ID="txtEMIDownPayment" runat="server" Width="200px" 
                                    CssClass="form-control"
                                    placeholder="Enter EMI Down Payment"
                                    onkeypress="return numeric_only(event)"
                                    ToolTip="Enter EMI Down Payment ..." 
                                    MaxLength="8" TabIndex="19" 
                                    AutoPostBack="True"></asp:TextBox>
                            </td>
                
                            <td ></td>
                            <td align="left" 
                                style="color: #FF0000; font-family: Tahoma; font-size: large;"></td>
                            <td style="text-align: center" >:</td>
                            <td align="left">
                                
                            </td>
                            <td>
                                
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td align="left"
                                style="font-family: Tahoma; font-size: small; font-weight: normal" 
                                class="style2">
                                <asp:Label ID="lblBankName" runat="server" Text="Bank Name" Visible="True"></asp:Label>
                            </td>
                            <td style="text-align: center">:</td>
                            <td align="left">
                                <asp:TextBox ID="txtBankName" runat="server" Width="200px" CssClass="form-control"
                                    style="font-weight: 700" ToolTip="Pls enter bank name..." MaxLength="40" 
                                    Visible="True" TabIndex="23"></asp:TextBox>
                            </td>
                
                            <td ></td>
                            <td align="left">
                                <asp:Label ID="lblBankName0" runat="server" Text="Bank Name" Visible="True"></asp:Label>
                                </td>
                            <td style="text-align: center">:</td>
                            <td align="left">
                                <asp:TextBox ID="txtBankName2" runat="server" Width="200px" CssClass="form-control"
                                    style="font-weight: 700" ToolTip="Pls enter bank name..." MaxLength="40" 
                                    Visible="True" TabIndex="29"></asp:TextBox>
                            </td>
                            <td></td>
                        </tr>

                        <tr>
                            <td></td>
                            <td align="left"     
                                
                                style="font-family: Tahoma; color: #000080; font-size: small; font-weight: normal" 
                                class="style2">
                                <asp:Label ID="lblSecurityCode" runat="server" Text="Security Code" Visible="True"></asp:Label>
                            </td>
                            <td style="text-align: center">:</td>
                            <td align="left">
                                <asp:TextBox ID="txtSecurityCode" runat="server" Width="200px" 
                                    style="font-weight: 700" ToolTip="Card Security Code ..." 
                                    MaxLength="5" Visible="True" TabIndex="24"></asp:TextBox>
                            </td>
                
                            <td></td>
                            <td align="left" color: #000080;>
                                <asp:Label ID="lblSecurityCode0" runat="server" Text="Security Code" 
                                    ForeColor="#003399"></asp:Label>
                            </td>
                            <td style="text-align: center">:</td>
                            <td align="left">
                                <asp:TextBox ID="txtSecurityCode2" runat="server" Width="200px" 
                                    style="font-weight: 700" ToolTip="Card Security Code ..." 
                                    MaxLength="5" Visible="True" TabIndex="30"></asp:TextBox>
                            </td>
                            <td></td>
                        </tr>

                        <tr>
                            <td></td>
                            <td align="left"
                                
                                style="font-family: Tahoma; color: #000080; font-size: small; font-weight: normal" 
                                class="style2">
                                <asp:Label ID="lblSecurityCode1" runat="server" Text="Approval Code" 
                                    ForeColor="#006600"></asp:Label>
                            </td>
                            <td style="text-align: center">:</td>
                            <td align="left">
                                <asp:TextBox ID="txtApprovalCode1" runat="server" Width="200px"
                                    style="font-weight: 700" ToolTip="Cheque/DD/TT Issue date..." 
                                    MaxLength="5" Visible="True" TabIndex="25"></asp:TextBox>
                            </td>                
                            <td ></td>
                            <td align="left">
                                <asp:Label ID="lblSecurityCode2" runat="server" Text="Approval Code" 
                                    ForeColor="#006600"></asp:Label>
                            </td>
                            <td style="text-align: center">:</td>
                            <td align="left">
                                <asp:TextBox ID="txtApprovalCode2" runat="server" Width="200px" 
                                    style="font-weight: 700" ToolTip="Cheque/DD/TT Issue date..." 
                                    MaxLength="5" Visible="True" TabIndex="31"></asp:TextBox>
                            </td>
                            <td></td>
                        </tr>
                        <tr>
                            <td></td>
                            <td align="left" class="style2">
                                <asp:Label ID="lblIssueDate" runat="server" Text="Issue Date" Visible="False"></asp:Label>
                            </td>
                            <td style="text-align: center">&nbsp;</td>
                            <td align="left">
                                <asp:TextBox ID="txtIssueDate" runat="server" Width="200px" 
                                    style="font-weight: 700" ToolTip="Cheque/DD/TT Issue date..." 
                                    MaxLength="15" Visible="false" TabIndex="18"></asp:TextBox>
                            </td>                
                            <td ></td>
                            <td align="left"></td>
                            <td style="text-align: center"></td>
                            <td align="left">
                                <asp:TextBox ID="txtPay" runat="server" Width="200px" 
                                    style="font-weight: 700" 
                                    onkeypress="return numeric_only(event)"
                                    ToolTip="Please Enter paid Amount..." 
                                    MaxLength="10" TabIndex="14" 
                                    AutoPostBack="False" Visible="False"></asp:TextBox>
                            </td>
                            <td></td>
                        </tr>

                        

                        <tr>
                            <td></td>
                            <td align="left" class="style2">
                                Ref. By</td>
                            <td style="text-align: center">:</td>
                            <td align="left">
                                <asp:TextBox ID="txtRefBy" runat="server" Width="200px" CssClass="form-control"
                                    style="font-weight: 700" ToolTip="Sales Reference By ..." 
                                    MaxLength="15" Visible="true" TabIndex="18"></asp:TextBox>
                            </td>                
                            <td ></td>
                            <td align="left" style="color: #000080">Sales By (Job ID#)</td>
                            <td style="text-align: center">:</td>
                            <td align="left">
                                <asp:TextBox ID="txtJobID" runat="server" Width="200px" 
                                    style="font-weight: 700" CssClass="form-control"
                                    onkeypress="return numeric_only(event)"
                                    ToolTip="Please Enter Employee Job ID ..." 
                                    MaxLength="6" TabIndex="14" 
                                    AutoPostBack="False" Visible="true"></asp:TextBox>
                            </td>
                            <td>
                                <asp:ImageButton ID="btnSearchSalesBy" runat="server" data-toggle="tooltip" 
                                    Height="18px" ImageUrl="~/Images/search.png" onclick="btnSearchSalesBy_Click" 
                                    title="Click here for Search Employee who sales this product ..." 
                                    Width="18px" />
                            </td>
                        </tr>

                        
                        <!-- Delivery From -->
                        <tr>
                            <td></td>
                            <td align="left" class="style2">
                                Delivery From</td>
                            <td style="text-align: center">:</td>
                            <td align="left">
                                <asp:DropDownList ID="ddlEntity" runat="server" class="form-control" 
                                    Height="30px" Width="200px">
                                </asp:DropDownList>
                            </td>                
                            <td ></td>
                            <td align="left" style="color: #000080">&nbsp;</td>
                            <td style="text-align: center"></td>
                            <td align="left">
                                <asp:Label ID="lblSalesBy" runat="server" Text="-"></asp:Label>
                            </td>
                            <td></td>
                        </tr>

                       
                        <tr>
                            <td ></td>
                            <td align="left" class="style2" style="color: #FF0000; font-size: 16px">
                                Online Order No.</td>
                            <td style="text-align: center" >&nbsp;</td>
                            <td align="left">
                                <asp:TextBox ID="txtOrderNo" CssClass="form-control" runat="server" MaxLength="13" 
                                    style="font-weight: 700" TabIndex="18" ToolTip="Online Order Number ..." 
                                    Visible="true" Width="200px"></asp:TextBox>
                            </td>                
                            
                            <td></td>
                            <td align="left" style="color: #000080">Reference Challan#</td>
                            <td style="text-align: center">:</td>
                            <td align="left">
                                <asp:TextBox ID="txtRefChNo" CssClass="form-control" runat="server" MaxLength="15" 
                                    style="font-weight: 700" TabIndex="18" ToolTip="Reference Challan No ..." 
                                    Visible="true" Width="200px"></asp:TextBox>
                            </td>
                            <td></td>
                        </tr>

                        <%--<tr>
                            <td ></td>
                            <td align="left" class="style2" style="color: #FF0000; font-size: 12px">
                                Source Of Information
                            </td>
                            <td style="text-align: center" >:</td>
                            <td align="left">
                                <asp:DropDownList ID="ddlSource" CssClass="form-control" runat="server" Width="200px">
                                    <asp:ListItem>Facebook</asp:ListItem>
                                    <asp:ListItem>Google</asp:ListItem>
                                    <asp:ListItem>Instagram</asp:ListItem>
                                    <asp:ListItem>LinkdIn</asp:ListItem>
                                    <asp:ListItem>Twitter</asp:ListItem>
                                    <asp:ListItem>News Paper</asp:ListItem>
                                    <asp:ListItem>TV</asp:ListItem>
                                    <asp:ListItem>Leaflet</asp:ListItem>
                                    <asp:ListItem>Mouth to Mouth</asp:ListItem>
                                    <asp:ListItem>None</asp:ListItem>
                                </asp:DropDownList>
                            </td>                
                            
                            <td></td>
                            <td align="left" style="color: #000080"></td>
                            <td style="text-align: center"></td>
                            <td align="left">
                                
                            </td>
                            <td></td>
                        </tr>--%>

                        <tr>
                            <td ></td>
                            <td align="left" class="style2">
                                &nbsp;</td>
                            <td style="text-align: center" >&nbsp;</td>
                            <td align="left">
                                &nbsp;</td>                
                            
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                        </tr>

                        <tr>
                            <td ></td>
                            <td align="left" class="style2">
                                &nbsp;</td>
                            <td style="text-align: center" >&nbsp;</td>
                            <td align="left" colspan="4">
                                &nbsp;</td>                
                            
                            <td></td>
                        </tr>

                        <!-- Terms & Condition -->
                        <tr>
                            <td ></td>
                            <td align="left" class="style2">
                                <asp:Label ID="Label9" runat="server" Text="Terms & Condition" 
                                    Font-Names="Tahoma" ForeColor="Red"></asp:Label>
                                </td>
                            <td style="text-align: center" >:</td>
                            <td align="left" colspan="4">
                                <asp:CheckBoxList ID="chkTC" runat="server" 
                                    RepeatColumns="1" 
                                    TabIndex="11"  
                                    >
                                </asp:CheckBoxList>
                                <asp:TextBox ID="txtTC" runat="server" Width="100%" 
                                    ToolTip="Write your Comments/Remarks/Note ..." 
                                    MaxLength="1050" Visible="False" TabIndex="32" TextMode="MultiLine" 
                                    Height="55px" ReadOnly="False"></asp:TextBox>
                            </td>                
                            
                            <td></td>
                        </tr>


                         <!-- Remarks -->
                        <tr>
                            <td ></td>
                            <td align="left" class="style2">
                                <asp:Label ID="Label5" runat="server" Text="Remarks/Note" Visible="True"></asp:Label>
                                </td>
                            <td style="text-align: center" >:</td>
                            <td align="left" colspan="4">
                                <asp:TextBox ID="txtNote" runat="server" Width="100%" 
                                    style="font-weight: 700" ToolTip="Write your Comments/Remarks/Note ..." 
                                    MaxLength="250" Visible="true" TabIndex="32">
                                </asp:TextBox>
                            </td>                
                            
                            <td></td>
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

    <div style="float:left; padding-top:10px; width:100%;"> 
        <!-- Source of Customer INFO -->
    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
            <ContentTemplate>
                <asp:Panel ID="Panel5" runat="server" CssClass="cpHeader">
                    <asp:Label ID="Label11" runat="server" >Source of Information</asp:Label>
                </asp:Panel>
                <asp:Panel ID="Panel6" runat="server" CssClass="cpBody">
                    <div align="center">
                        <table width="100%" style="font-family: Tahoma; font-size: small">
                        <tr>
                            <td width="2%"></td>
                            <td width="15%"></td>
                            <td width="3%"></td>
                            <td width="50%"></td>
                            <td width="10%"></td>
                            <td width="15%"></td>
                            <td width="3%"></td>
                            <td width="25%"></td>
                            <td width="2%"></td>                            
                        </tr>
                            <tr>
                            <td ></td>
                            <td align="left" class="style2" style="color: Green; font-size: 16px">
                                Source Of Information
                            </td>
                            <td style="text-align: center" >:</td>
                            <td align="left">             
                                         
                                <asp:DropDownList ID="ddlSource" CssClass="form-control" runat="server" Width="70%">
                                    <asp:ListItem>None</asp:ListItem>
                                    <asp:ListItem>Facebook/Instagram/Twitter</asp:ListItem>                                    
                                    <asp:ListItem>Newspaper Ad</asp:ListItem>
                                    <asp:ListItem>TVC</asp:ListItem>
                                    <asp:ListItem>YouTube</asp:ListItem>
                                    <asp:ListItem>Old Customer, repeat Purchase.</asp:ListItem>
                                    <asp:ListItem>Friends/Family Suggestion</asp:ListItem>                                    
                                    <asp:ListItem>Brand preference</asp:ListItem>
                                    <asp:ListItem>Follow up/Push/Corporate Sales</asp:ListItem>                                    
                                    <asp:ListItem>Leaflets/Banners/Miking</asp:ListItem>
                                    <asp:ListItem>Walk in Customer</asp:ListItem>                                                                 
                                </asp:DropDownList>
                            </td>                
                            
                            <td></td>
                            <td align="left" style="color: #000080"></td>
                            <td style="text-align: center"></td>
                            <td align="left">
                                
                            </td>
                            <td></td>
                        </tr>
                        </table>
                        
                    </div>
    
                <div>&nbsp;</div>

            </asp:Panel>
            </ContentTemplate>                     

    </asp:UpdatePanel>
    </div>
         
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
                        onclick="btnSave_Click" TabIndex="33" 
                        Font-Size="Small" 
                        ToolTip="Click here for save data..." Width="137px"/>
                        &nbsp;
                    <asp:Button ID="btnPrint" CssClass="btn btn-primary" runat="server" Text="Re-Print"
                        Font-Size="Small"
                        Width="88px" TabIndex="34" onclick="btnPrint_Click"/>
                        &nbsp;
                    <asp:Button ID="btnCancel" CssClass="btn btn-primary" runat="server" Text="New Bill" 
                        Font-Size="Small"
                        Width="88px" TabIndex="35" onclick="btnCancel_Click" />
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

