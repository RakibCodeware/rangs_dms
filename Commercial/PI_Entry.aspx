<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PI_Entry.aspx.cs" Inherits="PI_Entry" 
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
        style="padding:5px; background-color: #008080;" align="center"> PI (Proforma Invoice) Entry ...</h2>
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
                        
            <!-- PI No. -->
            <tr>
                <td class="style1">&nbsp;</td>
                <td style="text-align: Left; font-family: Arial; font-weight: 700; color: #009900;" 
                    class="style2">PI Number</td>
                <td class="style3"> : </td>
                <td style="text-align: left" class="style4">
                    <asp:TextBox ID="txtCHNo" runat="server" Width="200px" 
                        CssClass="form-control" BackColor="#66FFFF" Placeholder="PI Number"
                        ToolTip="Please Enter Proforma Invoice Number" 
                        MaxLength="15" Enabled="true"></asp:TextBox>
                </td>
                <td class="style5"> </td>                 
                
                <td class="style6" align="left">PI Date</td>
                <td class="style7">:</td>
                <td class="style8" align="left">
                    <asp:TextBox ID="txtDate" runat="server" Width="97px" TabIndex="0"                         
                        ToolTip="Please Enter PI Date" MaxLength="10">
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
            
            <!-- Supplier Infor -->
            <tr>
                <td class="style1"></td>
                <td style="text-align: left" class="style2">Supplier Name</td>
                <td class="style3"> :</td>
                    
                <td class="style4" height="40px">  
                    
                                         
                        <asp:DropDownList ID="ddlSupplier" Width="200px"  CssClass="form-control" runat="server">
                        </asp:DropDownList>
                        
                        <asp:Button ID="btnAddSupp" CssClass="btn btn-primary" runat="server" Text="  +  " 
                                onclick="btnAddSupp_Click" Visible="False" />
                    
                </td>
                <td class="style5"></td>
                <td class="style6"></td>
                <td class="style7"></td>
                <td class="style8" align="left">
                    <asp:Label ID="lblSAdd" runat="server" Text="[Supplier Address]"></asp:Label>
                </td>
                <td class="style9"></td>
            </tr>         
           
            <!--
            <tr>
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
            -->
            <!--
            <tr>
                <td class="style1"></td>
                <td class="style2"></td>
                <td style="text-align: left" class="style3">              
                &nbsp;</td>
                <td class="style4"></td>
                <td class="style5"></td>
                <td class="style6"></td>
                <td class="style7"></td>
                <td class="style8" align="left">
                    
                </td>
                <td class="style9"></td>
            </tr>
            -->
            <!--
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
            -->
            <br />
            <tr>
                <td colspan="9" align="center"
                    style="background-image:url(../Images/header.jpg); height:0.5px; font-family: Arial;">                        
                </td>
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
                <td class="style8"></td>
                <td class="style9"></td>
            </tr>
           
            <!-- ---------------------------------- -->

            <!-- Supplier Brand & Model -->
            <tr>
                <td class="style1"></td>
                <td style="text-align: Left; font-family: Arial; font-weight: 700; color: #003300;" 
                    class="style2">Brand</td>
                <td class="style3"> : </td>              
                <td style="text-align: left" class="style4" >  
                    <asp:DropDownList ID="ddlBrand" Width="200px" CssClass="form-control" runat="server">
                    </asp:DropDownList>                    
                </td>
                <td class="style5">
                    &nbsp;<asp:TextBox ID="txtProdID" runat="server" Width="16px" 
                        style="font-weight: 700" Visible="False"></asp:TextBox>
                </td>
                                
                <td class="style6" align="left">Supplier Model</td>
                <td class="style7">:</td>
                <td class="style8" align="left">
                    <asp:TextBox ID="txtSuppModel" runat="server" Height="30px"
                        Placeholder="Supplier Model"
                        Width="200px" CssClass="form-control" >
                    </asp:TextBox>
                </td>
                <td class="style9"></td>

            </tr>


            
             <!-- Rangs Product Model -->
            <tr>
                <td class="style1"></td>
                <td style="text-align: Left; font-family: Arial; font-weight: 700; color: #003300;" 
                    class="style2">Rangs Model</td>
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
                                
                <td class="style6" align="left">Rangs Model Code</td>
                <td class="style7">:</td>
                <td class="style8" align="left">
                    <asp:TextBox ID="txtCode" runat="server" Height="30px" Placeholder="Rangs Model"
                        Width="200px" CssClass="form-control" ontextchanged="txtCode_TextChanged">
                    </asp:TextBox>
                </td>
                <td class="style9"></td>

            </tr>

            <!-- Product Description -->
            <tr>
                <td class="style1"></td>
                <td style="text-align: Left; font-family: Arial; " 
                    class="style2" align="left">Product Description</td>
                <td class="style3"> : </td>
                <td style="text-align: left" class="style4">
                    <asp:TextBox ID="txtProdDesc" runat="server" Width="200px" 
                        CssClass="form-control" Height="30px" Placeholder="Product Description"
                        Enabled="False">
                    </asp:TextBox>                   
                </td>
                <td class="style5"></td>
                <td class="style6" align="left"></td>
                <td class="style7">:</td>
                <td class="style8" align="left">                    
                </td>
                <td class="style9"></td>
            </tr>


            <!-- Product QTY/UP -->
            <tr>
                <td class="style1"></td>
                <td style="text-align: Left; font-family: Arial; " 
                    class="style2" align="left">Import Qty</td>
                <td class="style3"> : </td>
                <td style="text-align: left" class="style4">
                    <asp:TextBox ID="txtQty" runat="server" AutoPostBack="false" Width="200px" 
                        CssClass="form-control" Height="30px" Placeholder="Import Qty"
                        onkeypress="return numeric_only(event)"
                        ToolTip="Enter Import Quantity" TabIndex="3" 
                        ></asp:TextBox>                    
                </td>
                <td class="style5"></td>
                <td class="style6" align="left">Import UP (US$)</td>
                <td class="style7">:</td>
                <td class="style8" align="left">
                    <asp:TextBox ID="txtUP" runat="server" AutoPostBack="false" Width="200px" 
                        CssClass="form-control" Height="30px"  Placeholder="Import Unit Price"
                        onkeypress="return numeric_only(event)"
                        ToolTip="Enter Import Unit Price" TabIndex="3" 
                        ></asp:TextBox> 
                </td>
                <td class="style9"></td>
            </tr>
            
            <!-- Total Price -->
            <tr>
                <td class="style1"></td>
                <td style="text-align: Left; font-family: Arial; " 
                    class="style2" align="left"></td>
                <td class="style3">  </td>
                <td style="text-align: left" class="style4">
                                       
                </td>
                <td class="style5"></td>
                <td class="style6" align="left" style="font-weight: bold">Total Amount (US$)</td>
                <td class="style7">:</td>
                <td class="style8" align="left">
                    <asp:TextBox ID="TextBox2" runat="server" AutoPostBack="false" Width="200px" 
                        CssClass="form-control" Height="30px" Placeholder="Total Amount"
                        onkeypress="return numeric_only(event)"
                        ToolTip="Total Amount (USD)" TabIndex="3" 
                        ReadOnly="True"></asp:TextBox> 
                </td>
                <td class="style9"></td>
            </tr>


            <!-- Gross Weight/Net Weight -->
            <tr>
                <td class="style1"></td>
                <td style="text-align: Left; font-family: Arial; " 
                    class="style2" align="left">Gross Weight</td>
                <td class="style3"> : </td>
                <td style="text-align: left" class="style4">
                    <asp:TextBox ID="TextBox1" runat="server" AutoPostBack="false" Width="200px" 
                        CssClass="form-control" Height="30px" Placeholder="Gross Weight"
                        onkeypress="return numeric_only(event)"
                        ToolTip="Please Enter Gross Weight" TabIndex="3" 
                        ></asp:TextBox>                    
                </td>
                <td class="style5"></td>
                <td class="style6" align="left">Net Weight</td>
                <td class="style7">:</td>
                <td class="style8" align="left">
                    <asp:TextBox ID="TextBox3" runat="server" AutoPostBack="false" Width="200px" 
                        CssClass="form-control" Height="30px" Placeholder="Net Weight"
                        onkeypress="return numeric_only(event)"
                        ToolTip="Please Enter Net Weight" TabIndex="3" 
                        ></asp:TextBox> 
                </td>
                <td class="style9"></td>
            </tr>


                       
            <!-- Remarks  -->
            <tr>
                <td class="style1"></td>
                <td style="text-align: Left; font-family: Arial;" 
                    class="style2" align="left">Remarks (If any)</td>
                <td class="style3"> : </td>
                <td style="text-align: left" class="style4" align="left">
                    <asp:TextBox ID="txtRemarks" runat="server" Width="200px" 
                        CssClass="form-control" Height="30px" Placeholder="Remarks (If any)"
                        TabIndex="5">
                    </asp:TextBox>
                </td>
                <td class="style5"></td>
                <td class="style6" align="left"></td>
                <td class="style7"></td>
                <td class="style8" align="left">
                    
                    <asp:TextBox ID="txtSL" runat="server" Width="200px" 
                        CssClass="form-control" Height="30px"
                        ToolTip="Please Enter Product Serial Number" TabIndex="4" Visible="False"></asp:TextBox>
                </td>
                <td class="style9"></td>
            </tr>

             <!-- Blank -->
             <!--
            <tr>
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
            -->
            
            <!-- Add to Data Grid -->
            <tr>
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
            
            <tr>
                <td class="style1"></td>
                <td class="style2"></td>
                <td class="style3">&nbsp;</td>
                <td class="style4"></td>
                <td class="style5"></td>
                <td class="style6"></td>
                <td class="style7"></td>
                <td class="style8"></td>
                <td class="style9"></td>
            </tr>
            
        </table>
        
        
        <div>
        <asp:Label ID="lblError" ForeColor="red" runat="server" Text=""></asp:Label>
        </div>
        
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
        </table>

        <!-- ------------------------------------------------------------------- -->
        <table>
            <tr>
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
            <tr>
                <td class="style1"></td>                
                <td class="style2">Total PI Value ($)</td>
                <td class="style3">:</td>
                <td class="style4">
                    <asp:TextBox ID="TextBox4" runat="server" CssClass="form-control" 
                        Height="30px" Placeholder="Total PI Value (USD)"></asp:TextBox>
                </td>
                <td class="style5"></td>
                <td class="style6">Adjustment Amnt ($)</td>
                <td class="style7">:</td>
                <td class="style8">
                    <asp:TextBox ID="TextBox5" CssClass="form-control"
                        Height="30px" Placeholder="Adjustment Amnt (USD)"
                        runat="server"></asp:TextBox>
                </td>
                <td class="style9"></td>
            </tr>

            <tr>
                <td class="style1"></td>
                <td class="style2">FOC (1%)</td>
                <td class="style3">:</td>
                <td class="style4">
                    <asp:RadioButtonList ID="RadioButtonList1" runat="server" Height="21px" 
                        RepeatDirection="Horizontal" Width="133px">
                        <asp:ListItem Selected="True">Yes</asp:ListItem>
                        <asp:ListItem>No</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
                <td class="style5"></td>
                <td class="style6"></td>
                <td class="style7"></td>
                <td class="style8"></td>
                <td class="style9"></td>
            </tr>

            <tr>
                <td class="style1"></td>
                <td class="style2"></td>
                <td class="style3"></td>
                <td class="style4" style="text-align: left">                    
                    <asp:Button ID="btnSave" runat="server" Text="Save" 
                        width="88px" onclick="btnSave_Click" TabIndex="7" 
                        Font-Size="Small" CssClass="btn btn-primary"
                        ToolTip="Click here for save data..."/>
                        &nbsp;
                    
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" 
                        Font-Size="Small" CssClass="btn btn-primary"
                        Width="88px" TabIndex="9" onclick="btnCancel_Click" />                    
                </td>
                <td class="style5"></td>
                <td class="style6"></td>
                <td class="style7">
                    <asp:Button ID="btnPrint" runat="server" Text="Print"
                        Font-Size="Small" CssClass="btn btn-primary"
                        Width="88px" TabIndex="8" Visible="False"/>
                    &nbsp;
                </td>
                <td class="style8"></td>
                <td class="style9"></td>
            </tr>
            <tr>
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
        <!-- ------------------------------------------------------------------- -->

        <p>&nbsp;</p>

   </div>

    <!-- *********************************************************************************** -->

    <!-- Popup Window for Unit -->
    <asp:Button ID="btnShowPopup" runat="server" style="display:none" OnClick="btnShowPopup_Click"/>   

    <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="btnAddSupp" PopupControlID="pnlpopup"
        CancelControlID="btnCancel" BackgroundCssClass="modalBackground">
    </asp:ModalPopupExtender>
              
    <asp:Panel ID="pnlpopup" runat="server" BackColor="White" Height="150px" Width="500px" Visible="false">
        <table cellpadding="0" cellspacing="0" style="border-right: #d55500 3px solid; border-top: #d55500 3px solid;
            border-left: #d55500 3px solid; width: 100%; border-bottom: #d55500 3px solid;
            height: 100%" width="100%">

            <tr style="background-color: #d55500">
                <td align="center" colspan="2" style="font-weight: bold; font-size: larger; color: white; height: 10%">                    
                    Add Supplier Name</td>
            </tr>
            <tr>
                <td align="right" colspan="2" rowspan="6" style="padding-top:5px">                    
                    <div class="form-group">
                        <span style="font-size: 12pt">
                            <label class="col-sm-offset-1 col-sm-3 control-label" for="lblUnit" style="font-size: 12px">
                                Supplier Name</label>
                        </span>
                        <div class="col-sm-5">
                            <asp:TextBox ID="txtSuppName" runat="server" CssClass="form-control" placeholder="Enter Supplier Name"></asp:TextBox><span
                                style="font-size: 9pt"> </span>                            
                        </div>
                    </div>
                    
                    <div class="form-group">
                        <span style="font-size: 12pt">
                            <label class="col-sm-offset-1 col-sm-3 control-label" for="lblUnit" style="font-size: 12px">
                                Address</label>
                        </span>
                        <div class="col-sm-5">
                            <asp:TextBox ID="txtSAdd" runat="server" CssClass="form-control" placeholder="Enter Supplier Address"></asp:TextBox><span
                                style="font-size: 9pt"> </span>                            
                        </div>
                    </div>

                    <div class="form-group">
                        <span style="font-size: 12pt">
                            <label class="col-sm-offset-1 col-sm-3 control-label" for="lblUnit" style="font-size: 12px">
                                Contact Person</label>
                        </span>
                        <div class="col-sm-5">
                            <asp:TextBox ID="txtSContactPerson" runat="server" CssClass="form-control" placeholder="Enter Supplier Contact Person Name"></asp:TextBox><span
                                style="font-size: 9pt"> </span>                            
                        </div>
                    </div>

                    <div class="form-group">
                        <span style="font-size: 12pt">
                            <label class="col-sm-offset-1 col-sm-3 control-label" for="lblUnit" style="font-size: 12px">
                                Contact Number</label>
                        </span>
                        <div class="col-sm-5">
                            <asp:TextBox ID="txtSContact" runat="server" CssClass="form-control" placeholder="Enter Contact Number"></asp:TextBox><span
                                style="font-size: 9pt"> </span>                            
                        </div>
                    </div>

                    <div class="form-group">
                        <span style="font-size: 12pt">
                            <label class="col-sm-offset-1 col-sm-3 control-label" for="lblUnit" style="font-size: 12px">
                                Email Add</label>
                        </span>
                        <div class="col-sm-5">
                            <asp:TextBox ID="txtSEmail" runat="server" CssClass="form-control" placeholder="Enter Email Address"></asp:TextBox><span
                                style="font-size: 9pt"> </span>                            
                        </div>
                    </div>

                    <div class="form-group" >
                    <div class="col-sm-offset-4 col-sm-5"style="margin-bottom:7px;">
                        <asp:Button ID="btnSaveSupp" CssClass="btn btn-primary" runat="server" Text=" Submit " OnClick="btnSaveUnit_Click" />
                        &nbsp&nbsp
                        <asp:Button ID="Button1" CssClass="btn btn-primary" runat="server" Text="Cancel" />
                    </div>
                                        
                    </div>
                </td>
            </tr>
            
        </table>
    </asp:Panel>
    <!-- *********************************************************************************** -->


    <script src="../js/jquery.min.js" type="text/javascript"></script>
	<script src="../js/chosen.jquery.js" type="text/javascript"></script>
	<script type="text/javascript">
	    $(".chzn-select").chosen(); $(".chzn-select-deselect").chosen({ allow_single_deselect: true }); 
    </script>


</asp:Content>
