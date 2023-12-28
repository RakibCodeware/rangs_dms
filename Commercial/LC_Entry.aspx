<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LC_Entry.aspx.cs" Inherits="Commercial_LC_Entry" 
    MasterPageFile="Admin.master" %>

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
            height: 35px;
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

    <link rel="stylesheet" href="../Styles/chosen.css" />


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
    
    <h2 class="col-sm-12 bg-primary" 
        style="padding:5px; background-color: #008080;" align="center"> L/C Information Entry ...</h2>
    <p>
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager> 
    </p>
            
    <div align="center">
        
        <table width="100%" style="font-family: Tahoma; font-size: small">
            <tr>
                <td width="2%"></td>
                <td width="15%"></td>
                <td width="3%"></td>
                <td class="style4">
                    <asp:TextBox ID="txtMRSR" runat="server" Enabled="False" Font-Size="Smaller" 
                        Width="46px" Visible="False"></asp:TextBox>
                </td>
                <td width="10%"></td>
                <td width="15%"></td>
                <td width="3%"></td>
                <td width="15%"></td>
                <td width="2%"></td> 
            </tr>
                        
            <!-- LC No. -->
            <tr>
                <td class="style1">&nbsp;</td>
                <td style="text-align: Left; font-family: Arial; font-weight: 700; color: #009900;" 
                    class="style2">L/C #</td>
                <td class="style3"> : </td>
                <td style="text-align: left" class="style4">
                    <asp:TextBox ID="txtCHNo" runat="server" Width="200px" 
                        CssClass="form-control" BackColor="#CCFFFF"
                        ToolTip="Please Enter LC Number" Placeholder="L/C Number"
                        MaxLength="15" Enabled="true"></asp:TextBox>
                </td>
                <td class="style5"> </td>                 
                
                <td class="style6" align="left">L/C Date</td>
                <td class="style7">:</td>
                <td class="style8" align="left">
                    <asp:TextBox ID="txtDate" runat="server" Width="97px" TabIndex="0"                         
                        ToolTip="Please Enter L/C Date" MaxLength="10">
                    </asp:TextBox> 
                     &nbsp; 
                    <asp:ImageButton ID="imgPopup" ImageUrl="~/images/cal.gif" ImageAlign="Bottom"
                        runat="server" TabIndex="4" Height="16px" Width="17px" />
                    <cc1:CalendarExtender ID="Calendar1" PopupButtonID="imgPopup" runat="server" TargetControlID="txtDate"
                        Format="MM/dd/yyyy">
                    </cc1:CalendarExtender>      
                                                                        
                    <br />
                    

                </td>
                <td class="style9"></td>
            </tr>
            
            
            <!-- File No./PI # -->
            <tr>
                <td class="style1">&nbsp;</td>
                <td style="text-align: Left;" 
                    class="style2">File #</td>
                <td class="style3"> : </td>
                <td style="text-align: left" class="style4">
                    <asp:TextBox ID="TextBox1" runat="server" Width="200px" 
                        CssClass="form-control" Placeholder="File Number"
                        ToolTip="Please Enter File Number" 
                        MaxLength="15" Enabled="true"></asp:TextBox>
                </td>
                <td class="style5"> </td>                 
                
                <td class="style6" align="left">PI Number</td>
                <td class="style7">:</td>
                <td class="style8" align="left">
                    <asp:TextBox ID="TextBox2" runat="server" Width="200px" 
                        CssClass="form-control" Placeholder="Proforma Invoice Numer"
                        style="font-weight: 700" ToolTip="Please Enter PI Number" 
                        MaxLength="15" Enabled="true"></asp:TextBox>  
                </td>
                <td class="style9"></td>
            </tr>
            
            <!-- LC Value / Sending Date -->
            <tr>
                <td class="style1">&nbsp;</td>
                <td style="text-align: Left;" 
                    class="style2">L/C Value</td>
                <td class="style3"> : </td>
                <td style="text-align: left" class="style4">
                    <asp:TextBox ID="TextBox3" runat="server" Width="200px" 
                        CssClass="form-control" Placeholder="L/C Value"
                        ToolTip="Please Enter LC Value" 
                        MaxLength="15" Enabled="true"></asp:TextBox>
                </td>
                <td class="style5"> </td>                 
                
                <td class="style6" align="left">L/C Sending Date</td>
                <td class="style7">:</td>
                <td class="style8" align="left">
                    <asp:TextBox ID="txtSendingDate" runat="server" Width="97px" TabIndex="0"                         
                        ToolTip="Please Enter L/C Sending Date" MaxLength="10">
                    </asp:TextBox> 
                     &nbsp; 
                    <asp:ImageButton ID="imgPopup4" ImageUrl="~/images/cal.gif" ImageAlign="Bottom"
                        runat="server" TabIndex="4" Height="16px" Width="17px" />
                    <cc1:CalendarExtender ID="CalendarExtender2" PopupButtonID="imgPopup4" runat="server" TargetControlID="txtSendingDate"
                        Format="MM/dd/yyyy">
                    </cc1:CalendarExtender>      
                                                                        
                    <br />                    

                </td>
                <td class="style9"></td>
            </tr>
            
            
            <!-- Bank Info # -->
            <tr>
                <td class="style1">&nbsp;</td>
                <td style="text-align: Left; " 
                    class="style2">Bank Name</td>
                <td class="style3"> : </td>
                <td style="text-align: left" class="style4">
                    <asp:TextBox ID="TextBox4" runat="server" Width="200px" 
                        CssClass="form-control" Placeholder="Bank Name"
                        ToolTip="Please Enter Bank Name" 
                        MaxLength="15" Enabled="true"></asp:TextBox>
                </td>
                <td class="style5"> </td>                 
                
                <td class="style6" align="left">Bank Address</td>
                <td class="style7">:</td>
                <td class="style8" align="left">
                    <asp:TextBox ID="TextBox5" runat="server" Width="200px" 
                        CssClass="form-control" Placeholder="Bank Address"
                        ToolTip="Please Enter Bank Address" 
                        MaxLength="15" Enabled="true"></asp:TextBox>  
                </td>
                <td class="style9"></td>
            </tr>
            
            <!-- Bank Acc # -->
            <tr>
                <td class="style1">&nbsp;</td>
                <td style="text-align: Left;" 
                    class="style2">Account #</td>
                <td class="style3"> : </td>
                <td style="text-align: left" class="style4">
                    <asp:TextBox ID="TextBox6" runat="server" Width="200px" 
                        CssClass="form-control" Placeholder="Account Number"
                        ToolTip="Please Enter Bank Account #" 
                        MaxLength="15" Enabled="true"></asp:TextBox>
                </td>
                <td class="style5"> </td>                 
                
                <td class="style6" align="left">Swift Code</td>
                <td class="style7">:</td>
                <td class="style8" align="left">
                    <asp:TextBox ID="TextBox7" runat="server" Width="200px" 
                        CssClass="form-control" Placeholder="Bank Swift Code"
                        ToolTip="Please Enter Bank Swift Code" 
                        MaxLength="15" Enabled="true"></asp:TextBox>  
                </td>
                <td class="style9"></td>
            </tr>

            <!-- IRC/BIN # -->
            <tr>
                <td class="style1">&nbsp;</td>
                <td style="text-align: Left;" 
                    class="style2">IRC #</td>
                <td class="style3"> : </td>
                <td style="text-align: left" class="style4">
                    <asp:TextBox ID="TextBox8" runat="server" Width="200px" 
                        CssClass="form-control" Placeholder="IRC Number"
                        ToolTip="Please Enter IRC #" 
                        MaxLength="15" Enabled="true"></asp:TextBox>
                </td>
                <td class="style5"> </td>                 
                
                <td class="style6" align="left">Bin #</td>
                <td class="style7">:</td>
                <td class="style8" align="left">
                    <asp:TextBox ID="TextBox9" runat="server" Width="200px" 
                        CssClass="form-control" Placeholder="Bin Number"
                        ToolTip="Please Enter Bin #" 
                        MaxLength="15" Enabled="true"></asp:TextBox>  
                </td>
                <td class="style9"></td>
            </tr>
                      
            
            <!-- Gross # -->
            <tr>
                <td class="style1">&nbsp;</td>
                <td style="text-align: Left;" 
                    class="style2">Gross Weight</td>
                <td class="style3"> : </td>
                <td style="text-align: left" class="style4">
                    <asp:TextBox ID="TextBox10" runat="server" Width="200px" 
                        CssClass="form-control" Placeholder="Gross Weight"
                        ToolTip="Please Enter " 
                        MaxLength="15" Enabled="true"></asp:TextBox>
                </td>
                <td class="style5"> </td>                 
                
                <td class="style6" align="left">Net Weight</td>
                <td class="style7">:</td>
                <td class="style8" align="left">
                    <asp:TextBox ID="TextBox11" runat="server" Width="200px" 
                        CssClass="form-control" Placeholder="Net Weight"
                        ToolTip="Please Enter " 
                        MaxLength="15" Enabled="true"></asp:TextBox>  
                </td>
                <td class="style9"></td>
            </tr>


            <!--  # -->
            <tr>
                <td class="style1">&nbsp;</td>
                <td style="text-align: Left;" 
                    class="style2">Value Per KG</td>
                <td class="style3"> : </td>
                <td style="text-align: left" class="style4">
                    <asp:TextBox ID="TextBox12" runat="server" Width="200px" 
                        CssClass="form-control" Placeholder="Value Per KG"
                        ToolTip="Please Enter " 
                        MaxLength="15" Enabled="true"></asp:TextBox>
                </td>
                <td class="style5"> </td>                 
                
                <td class="style6" align="left">H.S Code</td>
                <td class="style7">:</td>
                <td class="style8" align="left">
                    <asp:TextBox ID="TextBox13" runat="server" Width="200px" 
                        CssClass="form-control" Placeholder="H.S Code"
                        ToolTip="Please Enter " 
                        MaxLength="15" Enabled="true"></asp:TextBox>  
                </td>
                <td class="style9"></td>
            </tr>

            <!--  # -->
            <tr>
                <td class="style1">&nbsp;</td>
                <td style="text-align: Left;" 
                    class="style2">Total Qty</td>
                <td class="style3"> : </td>
                <td style="text-align: left" class="style4">
                    <asp:TextBox ID="TextBox14" runat="server" Width="200px" 
                        CssClass="form-control" Placeholder="Total Qty"
                        ToolTip="Please Enter " 
                        MaxLength="15" Enabled="true"></asp:TextBox>
                </td>
                <td class="style5"> </td>                 
                
                <td class="style6" align="left">Total Value</td>
                <td class="style7">:</td>
                <td class="style8" align="left">
                    <asp:TextBox ID="TextBox15" runat="server" Width="200px" 
                        CssClass="form-control" Placeholder="Total Value"
                        ToolTip="Please Enter " 
                        MaxLength="15" Enabled="true"></asp:TextBox>  
                </td>
                <td class="style9"></td>
            </tr>

            <!--  Remarks -->
            <tr>
                <td class="style1">&nbsp;</td>
                <td style="text-align: Left;" 
                    class="style2">Remarks</td>
                <td class="style3"> : </td>
                <td style="text-align: left" class="style4">
                    <asp:TextBox ID="TextBox16" runat="server" Width="200px" 
                        CssClass="form-control" Placeholder="Note/Remarks (If any)"
                        ToolTip="Please enter any remarks" 
                        MaxLength="15" Enabled="true"></asp:TextBox>
                </td>
                <td class="style5"> </td>                 
                
                <td class="style6" align="left"></td>
                <td class="style7"></td>
                <td class="style8" align="left">
                     
                </td>
                <td class="style9"></td>
            </tr>


            <!-- Line Break -->
            <tr style="display : none">
                <td class="style1"></td>
                <td class="style2" align="left">Shipment From</td>
                <td class="style3">:</td>
                <td class="style4" align="left">
                    <asp:DropDownList ID="ddlEntity" class="form-control" 
                        runat="server" Width="200px" Height="30px" Visible="False">
                    </asp:DropDownList>
                    <asp:TextBox ID="txtShipFrom" CssClass="form-control" Width="200px" Height="30px" runat="server"></asp:TextBox>
                </td>
                <td class="style5"></td>
                <td class="style6" align="left">Shipment Date</td>
                <td class="style7">:</td>
                <td class="style8" align="left">
                    <asp:TextBox ID="txtShipDate" runat="server" Width="97px" TabIndex="0"                         
                        ToolTip="Please Enter Shipment Date" MaxLength="10">
                    </asp:TextBox> 
                     &nbsp; 
                    <asp:ImageButton ID="ImageButton1" ImageUrl="~/images/cal.gif" ImageAlign="Bottom"
                        runat="server" TabIndex="4" Height="16px" Width="17px" />
                    <cc1:CalendarExtender ID="CalendarExtender1" PopupButtonID="ImageButton1" runat="server" TargetControlID="txtShipDate"
                        Format="MM/dd/yyyy">
                    </cc1:CalendarExtender>     
                    
                </td>
                <td class="style9"></td>
            </tr>

            
            
            <tr>
                <td class="style1"></td>
                <td class="style2"></td>
                <td style="text-align: left" class="style3">              
                &nbsp;</td>
                <td class="style4"></td>
                <td class="style5"></td>
                <td class="style6"></td>
                <td class="style7"></td>
                <td class="style8" align="left"></td>
                <td class="style9"></td>
            </tr>
           

            <tr>
                <td colspan="9" align="center"
                    style="background-image:url(../Images/header.jpg); height:0.5px; font-family: Arial;">                        
                    </td>
            </tr>

            <tr>
                <td class="style1">&nbsp;</td>
                <td style="text-align: Left;" 
                    class="style2"></td>
                <td class="style3"></td>
                <td align="left" class="style4" colspan="3">                    
                    <asp:Button ID="btnSave" runat="server" Text="Save" 
                        width="88px" onclick="btnSave_Click" TabIndex="7" 
                        Font-Size="Small" CssClass="btn btn-primary"
                        ToolTip="Click here for save data..."/>
                        &nbsp;
                    <asp:Button ID="btnPrint" runat="server" Text="Print"
                    Font-Size="Small" CssClass="btn btn-primary"
                    Width="88px" TabIndex="8" Visible="False"/>
                    
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" 
                    Font-Size="Small" CssClass="btn btn-primary"
                    Width="88px" TabIndex="9" onclick="btnCancel_Click" />                    
                </td>                
                <td class="style7"></td>
                <td class="style8" align="left"></td>
                <td class="style9"></td>
            </tr>



            <tr>
                <td class="style1"></td>
                <td class="style2"></td>
                <td class="style3"></td>               
                               
                <td class="style4">
                    <asp:TextBox ID="txtProdID" runat="server" Width="16px" 
                        style="font-weight: 700" Visible="False"></asp:TextBox>

                    </td>
                <td class="style5"></td>
                <td class="style6"></td>
                <td class="style7"></td>
                <td class="style8"></td>
                <td class="style9"></td>
            </tr>
            <!-- ---------------------------------- -->

            
             <!-- Product Model -->
            <tr style="display :none ">
                <td class="style1"></td>
                <td style="text-align: Left; font-family: Arial; font-weight: 700; color: #003300;" 
                    class="style2">Product Model</td>
                <td class="style3"> : </td>              
                <td style="text-align: left" class="style4" >  
                    <div class="side-by-side clearfix">                                  
                        <asp:DropDownList ID="ddlContinents" class="chzn-select"
                            runat="server" AutoPostBack = "True"
                            data-placeholder="Choose a Model ..."
                            OnSelectedIndexChanged="ddlContinents_SelectedIndexChanged"                         
                            Width="200px" Height="32px" TabIndex="6"
                            ToolTip="Please Select Product Model ...">                                    
                        </asp:DropDownList> 
                    </div> 
                </td>
                <td class="style5">
                    &nbsp;
                </td>
                                
                <td class="style6" align="left">Code</td>
                <td class="style7">:</td>
                <td class="style8" align="left">
                    <asp:TextBox ID="txtCode" runat="server" Height="30px"
                        Width="200px" CssClass="form-control" ontextchanged="txtCode_TextChanged">
                    </asp:TextBox>
                </td>
                <td class="style9"></td>

            </tr>

            <!-- Product Description -->
            <tr style="display :none "> 
                <td class="style1"></td>
                <td style="text-align: Left; font-family: Arial; " 
                    class="style2" align="left">Quantity</td>
                <td class="style3"> : </td>
                <td style="text-align: left" class="style4">
                    <asp:TextBox ID="txtQty" runat="server" AutoPostBack="false" Width="200px" 
                        CssClass="form-control" Height="30px"
                        onkeypress="return numeric_only(event)"
                        ToolTip="Please Enter Product Quantity" TabIndex="3" 
                        ></asp:TextBox>                    
                </td>
                <td class="style5"></td>
                <td class="style6" align="left">Product Description</td>
                <td class="style7">:</td>
                <td class="style8" align="left">
                    <asp:TextBox ID="txtProdDesc" runat="server" Width="200px" 
                        CssClass="form-control" Height="30px"
                        Enabled="False">
                    </asp:TextBox>
                </td>
                <td class="style9"></td>
            </tr>
            
            

                       
            <!-- Product SL# -->
            <tr style="display :none ">
                <td class="style1"></td>
                <td style="text-align: Left; font-family: Arial;" 
                    class="style2" align="left">Product SL#</td>
                <td class="style3"> : </td>
                <td style="text-align: left" class="style4" align="left">
                    <asp:TextBox ID="txtSL" runat="server" Width="200px" 
                        CssClass="form-control" Height="30px"
                        ToolTip="Please Enter Product Serial Number" TabIndex="4"></asp:TextBox>
                </td>
                <td class="style5"></td>
                <td class="style6" align="left">Remarks</td>
                <td class="style7">:</td>
                <td class="style8" align="left">
                    <asp:TextBox ID="txtRemarks" runat="server" Width="200px" 
                        CssClass="form-control" Height="30px"
                        style="font-weight: 700" TabIndex="5">
                    </asp:TextBox>
                </td>
                <td class="style9"></td>
            </tr>

             <!-- Remarks -->
            <tr style="display :none ">
                <td class="style1"></td>
                <td style="text-align: Left; font-family: Arial; " 
                    class="style2">&nbsp;</td>
                <td class="style3"> &nbsp;</td>
                <td style="text-align: left" class="style4">
                    &nbsp;</td>
                <td class="style5"></td>
                <td class="style6"></td>
                <td class="style7"></td>
                <td class="style8"></td>
                <td class="style9"></td>
            </tr>

            
            <!-- Add to Data Grid -->
            <tr style="display :none ">
                <td class="style1"></td>
                <td class="style2"></td>
                <td class="style3"></td>
                <td style="text-align: left" class="style4">
                    <asp:Button ID="btnAdd" runat="server" Text="Add Product in List" OnClick="btnAdd_Click" 
                        Width="116px" 
                        Font-Size="X-Small" CssClass="btn btn-primary"
                        ToolTip="Click here for add product in list ..." TabIndex="13" 
                        BackColor="#000099" ForeColor="Aqua"/>
                </td> 
                <td class="style5"></td>
                <td class="style6"></td>
                <td class="style7"></td>
                <td class="style8"></td>
                <td class="style9"></td>               
            </tr>
            
            <tr style="display :none ">
                <td class="style1"></td>
                <td class="style2"></td>
                <td class="style3"></td>
                <td class="style4">&nbsp;</td>
                <td class="style5"></td>
                <td class="style6"></td>
                <td class="style7"></td>
                <td class="style8"></td>
                <td class="style9"></td>
            </tr>
            
        </table>
        
        
        <div style="display :none ">
            <asp:Label ID="lblError" ForeColor="red" runat="server" Text=""></asp:Label>
        </div>
        
        <div style="display :none ">
        <table width="100%" style="font-family: Tahoma; font-size: small">
            <tr>
                    <td>
                        <!-- Data Grid -->                
                        <asp:GridView ID="gvUsers" runat="server"
                        AutoGenerateColumns="false"                        
                        CssClass="mGrid"
                        DataKeyNames="ProductID"
                        EmptyDataText = "No product in list !!! Please select model and add in list."                        
                        Onrowdeleting="gvUsers_RowDelating"
                        ShowFooter="true"
                        OnRowDataBound="gvUsers_RowDataBound"                        
                        Width="100%">
                        <FooterStyle Font-Bold="true" BackColor="#CCFFFF" ForeColor="black" BorderStyle="NotSet" />
                        <Columns>
                            <asp:BoundField HeaderText="Product_ID" DataField="ProductID" ItemStyle-Width="5px"/>                        
                            <asp:BoundField HeaderText="Product Model" DataField="Model" />
                            <asp:BoundField HeaderText="Quantity" DataField="Qty" ItemStyle-Width="10px"/>
                                                                                                          
                            <asp:BoundField HeaderText="Product SL" DataField="ProductSL" />
                            <asp:BoundField HeaderText="Remarks" DataField="Remarks" />
                            <asp:TemplateField HeaderText="" ItemStyle-Width="5px">
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
                        
                    </asp:GridView>
                </td>
            </tr> 
            <tr>
                <td>
                    <asp:Label ID="lblNetAmnt" runat="server" Text="0" Visible="False"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="1" align="center"
                    style="background-image:url(../Images/header.jpg); height:0.5px; font-family: Arial; ">                        
                </td>
            </tr>
            <tr>
                <td> &nbsp;</td>
            </tr>
            
            <tr>
                <td>&nbsp;</td>
            </tr>
        </table>

        </div>
        

   </div>


    <script src="../js/jquery.min.js" type="text/javascript"></script>
	<script src="../js/chosen.jquery.js" type="text/javascript"></script>
	<script type="text/javascript">
	    $(".chzn-select").chosen(); $(".chzn-select-deselect").chosen({ allow_single_deselect: true }); 
    </script>


</asp:Content>
