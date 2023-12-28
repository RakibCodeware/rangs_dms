<%@ Page Language="C#" MasterPageFile="Admin.master" AutoEventWireup="true" 
CodeFile="Installation_request.aspx.cs" Inherits="Installation_request" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
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
        .style34
        {
            width: 9px;
        }
        .style35
        {
            width: 9px;
            height: 36px;
        }
        .style36
        {
            width: 217px;
        }
        .style37
        {
            width: 217px;
            height: 36px;
        }
        .style38
        {
            width: 144px;
        }
    </style>

    <script>
        $(document).ready(function () {
            $('[data-toggle="tooltip"]').tooltip();
        });
    </script>
                       
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    
    <h2 class="col-sm-12 bg-primary" style="padding:5px; background-color: #006666; color: #FFFFFF;"> Installation Request ...</h2>
    <p></p>
    
    <asp:Label ID="Label1" runat="server" BackColor="#006666" CssClass="ui-bar" Text=""></asp:Label>

    <div align="center">
        
        <table width="100%" style="font-family: Tahoma; font-size: small">
            
            <tr>
                <td class="style1"></td>
                <td class="style2">&nbsp;</td>
                <td class="style3"></td>
                <td class="style4">
                    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                </td>
                <td></td>
                <td class="style5"></td>
                <td align="left" class="style6"></td>
                <td align="left" class="style7"></td>
                <td align="left" class="style8"></td>
                <td align="left" class="style9"></td>
            </tr>
            
            <!-- Challan No. -->
            <tr>
                <td></td>
                <td style="text-align: Left; font-family: Arial; font-weight: 700; color: #009900;" 
                    >Request #</td>
                <td > : </td>
                <td style="text-align: left" >
                    <asp:TextBox ID="txtCHNo" runat="server" Width="200px"  CssClass="form-control"
                        style="font-weight: 700" ToolTip="Please Enter Challan Number" MaxLength="15" 
                        Enabled="False"></asp:TextBox>
                </td>
                <td></td>
                <td ></td>
                <td align="left">
                    Request Date :</td>
                <td align="left">
                    <asp:TextBox ID="txtDate" runat="server" Width="97px" TabIndex="1" 
                        ToolTip="Please Enter Challan Date" MaxLength="10"></asp:TextBox> 
                    &nbsp; 
                    <asp:ImageButton ID="imgPopup" ImageUrl="~/Images/cal.gif" ImageAlign="Bottom"
                        runat="server" TabIndex="1" />
                    <cc1:CalendarExtender ID="Calendar1" PopupButtonID="imgPopup" runat="server" TargetControlID="txtDate"
                        Format="MM/dd/yyyy">
                    </cc1:CalendarExtender>
                </td>
                <td align="left"></td>
            </tr>
            
            <!-- Challan Date -->
            <tr>
                <td></td>
                <td style="text-align: left; font-family: Arial; font-size: small; " 
                    ></td>
                <td > &nbsp;</td>
                <td style="text-align: left" >                                    
                    <asp:TextBox ID="txtMRSR" runat="server" Enabled="False" Font-Size="Smaller" 
                        Width="46px" Visible="False"></asp:TextBox>                                    
                        &nbsp;
                    <asp:RequiredFieldValidator 
                     id="RequiredFieldValidator1" runat="server" 
                     ErrorMessage="Required!"                      
                     ControlToValidate="txtCHNo"> </asp:RequiredFieldValidator>
                </td>
                <td align="left"></td>
                <td></td>
                <td style="font-family: Arial; font-size: x-small; color: #990033" 
                    >
                </td>
                <td align="left" style="color: #FF0000">(MM/dd/yyyy)</td>
                <td align="left"></td>

            </tr>

            <!-- Challan Number & Date -->
            <tr>
                <td></td>
                <td style="text-align: left; font-family: Arial; font-size: small; " 
                    >Challan # </td>
                <td > :</td>
                <td style="text-align: left" >                                   
                    <asp:TextBox ID="txtChallanNo" runat="server" Width="300px" 
                        TabIndex="2" ToolTip="Please Enter Challan #" 
                        MaxLength="50" CssClass="form-control"
                        placeholder="Enter Challan #">
                    </asp:TextBox> 
                    
                    <asp:TextBox ID="txtMRSRID" runat="server" Enabled="False" Font-Size="Smaller" 
                            Width="36px" Visible="False"></asp:TextBox>  
                </td>
                <td>
                    <asp:ImageButton ID="ImageButton1" runat="server" Height="18px" 
                        ImageUrl="~/Images/search.png" Width="18px" 
                        data-toggle="tooltip" title="Click here for Search Customer Challan ..." 
                        onclick="ImageButton1_Click" /> 
                </td>
                <td></td>
                <td align="left">Challan Date :</td>  
                <td>
                    <asp:TextBox ID="txtChDate" runat="server"
                        placeholder="Customer Challan Date"
                        MaxLength="11" TabIndex="1" Width="300px"
                        ToolTip="Please Enter Customer Challan Date" 
                        CssClass="form-control" ReadOnly="True"></asp:TextBox> 
                </td>
                <td>                    
                    
                </td>
            </tr>

            <!-- Customer -->
            <tr>
                <td></td>
                <td style="text-align: left; font-family: Arial; font-size: small; " 
                    >Customer Name </td>
                <td > :</td>
                <td style="text-align: left" >                                   
                    <asp:TextBox ID="txtCustName" runat="server" Width="300px" 
                        TabIndex="2" ToolTip="Please Enter Customer Name" 
                        MaxLength="50" CssClass="form-control"
                        placeholder="Enter Customer Name">
                    </asp:TextBox>
                    <asp:Label ID="lblCTPEmail" runat="server" Text="" Visible="False"></asp:Label>   
                </td>
                <td></td>
                <td></td>
                <td align="left">Customer Mobile :</td>  
                <td>
                    <asp:TextBox ID="txtCustContact" runat="server"
                         placeholder="Enter Customer Mobile #"
                        MaxLength="11" TabIndex="1" Width="300px"
                        ToolTip="Please Enter Customer Contact Number"                        
                        onkeypress="return numeric_only(event)"
                        CssClass="form-control">
                    </asp:TextBox> 
                </td>
                <td>                    
                    <asp:ImageButton ID="btnCustSearch" runat="server" Height="18px" 
                        ImageUrl="~/Images/search.png" Width="18px" 
                        data-toggle="tooltip" title="Click here for Search Customer ..."
                        onclick="btnCustSearch_Click" Visible="False" /> 
                </td>
            </tr>
            
            <tr>
                <td></td>
                <td style="text-align: left; font-family: Arial; font-size: small; ">Email </td>
                <td> :</td>
                <td style="text-align: left" >                                   
                    <asp:TextBox ID="txtEmail" runat="server" Width="300px" 
                        TabIndex="2" ToolTip="Customer Email Address" 
                        MaxLength="50" CssClass="form-control"
                        placeholder="Customer Email Address">
                    </asp:TextBox> 
                                         
                </td>
                <td></td>
                <td></td>
                <td align="left"></td>  
                <td></td>
                <td></td>
            </tr>

            <!-- Customer Address -->
            <tr>
                <td></td>
                <td style="text-align: left; font-family: Arial; font-size: small; " >
                    Address </td>
                <td > :</td>
                <td style="text-align: left" colspan="5">                                   
                    <asp:TextBox ID="txtCustAdd" CssClass="form-control" runat="server"></asp:TextBox> 
                </td>                
                <td></td>
            </tr> 

            <!-- Customer District/Thana -->
            <tr>
                <td></td>
                <td style="text-align: left; font-family: Arial; font-size: small; " 
                    >Customer District </td>
                <td > :</td>
                <td style="text-align: left" >                                   
                    <asp:DropDownList ID="ddlDist" runat="server" CssClass="form-control" 
                        AutoPostBack="True" onselectedindexchanged="ddlDist_SelectedIndexChanged">
                    </asp:DropDownList>   
                </td>
                <td>&nbsp;</td>
                <td></td>
                <td align="left">Customer Thana:</td>  
                <td>
                    <asp:DropDownList ID="ddlThana" runat="server" CssClass="form-control" Width="300px">
                    </asp:DropDownList>
                </td>
                <td></td>
            </tr>   
            <!-- --------------------------------- Line Break -------------------------------------- -->
            <tr>
                <td></td>
                <td></td>
                <td style="text-align: left">              
                &nbsp;</td>
                <td align="left" style="color: #FF0000"></td>
                <td align="left"></td>
                <td align="left"></td>
                <td align="left"></td>
                <td align="left"></td>
                <td align="left"></td>
            </tr>
            <tr>
                <td colspan="9" align="center"
                    style="background-image:url(../Images/header.jpg); height:0.5px; font-family: Arial;">                        
                    </td>
            </tr>
            <!-- --------------------------------- End Line Break ---------------------------------- -->
            
            <tr>
                <td></td>
                <td></td>
                <td style="text-align: left">              
                &nbsp;</td>
                <td align="left" style="color: #FF0000"></td>
                <td align="left"></td>
                <td align="left"></td>
                <td align="left"></td>
                <td align="left"></td>
                <td align="left"></td>
            </tr>

            <!-- Vendor Name & Contact -->
            <tr style="display:none">
                <td></td>
                <td style="text-align: left; font-family: Arial; font-size: small; " 
                    >Vendor Name </td>
                <td > :</td>
                <td style="text-align: left" > 
                    <asp:DropDownList ID="ddlVendorName" CssClass="form-control" runat="server" 
                        onselectedindexchanged="ddlVendorName_SelectedIndexChanged" 
                        AutoPostBack="True">
                    </asp:DropDownList>
                </td>
                <td>
                    &nbsp;                    
                </td>
                <td></td>
                <td align="left">Vendor Contact</td>  
                <td>
                    <asp:TextBox ID="txtVMobile" runat="server" Width="300px" 
                        TabIndex="2" ToolTip="Enter Vendor Contact #" 
                        MaxLength="50" CssClass="form-control"
                        placeholder="Enter Vendor Contact #" ReadOnly="True"></asp:TextBox>
                </td>
                <td></td>
            </tr>
            
            <!-- Vendor Address -->
            <tr style="display:none">
                <td></td>
                <td style="text-align: left; font-family: Arial; font-size: small; " 
                    >Vendor Address </td>
                <td > :</td>
                <td style="text-align: left" > 
                    <asp:TextBox ID="txtVAdd" runat="server" Width="300px" 
                        TabIndex="2" ToolTip="Enter Vendor Address" 
                        MaxLength="50" CssClass="form-control"
                        placeholder="Enter Vendor Address" ReadOnly="True">
                    </asp:TextBox>
                </td>
                <td>&nbsp;</td>   
                <td></td>
                <td align="left"></td>  
                <td>                                    
                    <asp:TextBox ID="txtVAID" runat="server" Enabled="False" Font-Size="Smaller" 
                        Width="46px" Visible="False"></asp:TextBox>                                    
                    <asp:TextBox ID="txtVNickName" runat="server" Enabled="False" Font-Size="Smaller" 
                        Width="46px" Visible="False"></asp:TextBox>                                    
                </td>
                <td></td>
            </tr>   
            
            <!-- Installation Date & Time -->
            <tr style="display:none">
                <td></td>
                <td style="text-align: left; font-family: Arial; font-size: small; " 
                    >Installation Date </td>
                <td > :</td>
                <td style="text-align: left" > 
                    <asp:TextBox ID="txtInsDate" runat="server" Width="97px" TabIndex="1" 
                        ToolTip="Please Enter Installation Date" MaxLength="10"></asp:TextBox> 
                    &nbsp; 
                    <asp:ImageButton ID="ImageButton2" ImageUrl="~/Images/cal.gif" ImageAlign="Bottom"
                        runat="server" TabIndex="1" />
                    <cc1:CalendarExtender ID="CalendarExtender1" PopupButtonID="ImageButton2" runat="server" TargetControlID="txtInsDate"
                        Format="MM/dd/yyyy">
                    </cc1:CalendarExtender>
                </td>
                <td>&nbsp;</td>   
                <td></td>
                <td align="left">Installation Time</td>  
                <td>                                    
                    <asp:TextBox ID="txtInsTime" runat="server" Font-Size="Smaller" 
                        Width="300px" placeholder="Enter Approx Time" CssClass="form-control">
                        </asp:TextBox>                                    
                </td>
                <td></td>
            </tr>  
                               
            <!-- --------------------------------- Line Break -------------------------------------- -->
            <tr style="display:none">
                <td></td>
                <td></td>
                <td style="text-align: left">              
                &nbsp;</td>
                <td align="left" style="color: #FF0000">(MM/dd/yyyy)</td>
                <td align="left"></td>
                <td align="left"></td>
                <td align="left"></td>
                <td align="left"></td>
                <td align="left"></td>
            </tr>
            <tr>
                <td colspan="9" align="center"
                    style="background-image:url(../Images/header.jpg); height:0.5px; font-family: Arial;">                        
                    </td>
            </tr>
            <!-- --------------------------------- End Line Break ---------------------------------- -->
            <tr>
                <td></td>
                <td></td>
                <td style="text-align: left">              
                    &nbsp;</td>
                <td>                                        
                    <asp:TextBox ID="txtProdID" runat="server" Width="16px" 
                        style="font-weight: 700" Visible="False"></asp:TextBox>
                </td>
                <td align="left"></td>
                <td align="left"></td>
                <td align="left"></td>
                <td align="left"></td>
                <td align="left"></td>
            </tr>
            <!-- ---------------------------------- -->

            
             <!-- Product Model -->
            <!--
            <tr>
                <td></td>
                <td style="text-align: Left; font-family: Arial; font-weight: 700; color: #003300;" 
                    >Product Model</td>
                <td > : </td>              
                <td style="text-align: left">  
                    <div class="side-by-side clearfix">                                  
                        <asp:DropDownList ID="ddlContinents" class="chzn-select"
                            runat="server" AutoPostBack = "True"
                            data-placeholder="Choose a Model ..."
                            OnSelectedIndexChanged="ddlContinents_SelectedIndexChanged"                         
                            Width="300px" Height="35px" TabIndex="6"
                            ToolTip="Please Select Product Model ...">                                    
                        </asp:DropDownList> 
                    </div>                    
                </td>
                <td align="left"></td>  
                <td align="left">&nbsp;</td>
                <td align="left">Code :</td>
                <td align="left">
                    <asp:TextBox ID="txtCode" runat="server" AutoPostBack="True" 
                        CssClass="form-control"
                        ontextchanged="txtCode_TextChanged"></asp:TextBox>
                </td>
                <td align="left"></td>
                  
            </tr>
                        
            <tr>
                <td></td>
                <td style="text-align: Left; font-family: Arial; font-weight: 700;  
                    color: #003300;">Quantity</td>
                <td > : </td>
                <td style="text-align: left">
                    <asp:TextBox ID="txtQty" runat="server" Width="200px" 
                        style="font-weight: 700"
                        CssClass="form-control"
                        onkeypress="return numeric_only(event)"
                        ToolTip="Please Enter Product Quantity" TabIndex="3" 
                        ></asp:TextBox>                    
                </td>

                <td align="left"></td>
                <td align="left"></td>
                <td style="text-align: Left; font-family: Arial; " 
                    >Description </td>
                <td >
                    <asp:TextBox ID="txtProdDesc" runat="server" 
                        CssClass="form-control"
                        style="font-weight: 500" Enabled="False"></asp:TextBox>
                </td>
                <td align="left"></td>
                
            </tr>
                                  
            <tr>
                <td></td>
                <td style="text-align: Left; font-family: Arial; " 
                    >Remarks</td>
                <td> : </td>
                <td style="text-align: left">
                    <asp:TextBox ID="txtRemarks" runat="server" Width="200px" 
                        CssClass="form-control"
                        style="font-weight: 700" TabIndex="5">
                    </asp:TextBox>
                </td>
                <td>&nbsp;</td>
                <td align="left"></td>
                <td align="left"></td>
                <td align="left"></td>
                <td align="left"></td>
                <td align="left"></td>
            </tr>
                       
            <tr>
                <td ></td>
                <td ></td>
                <td ></td>
                <td >&nbsp;</td>
                <td align="left" ></td>
                <td align="left"></td>
                <td align="left"></td>
                <td align="left"></td>
                <td align="left"></td>
            </tr>           
                        
            <tr>
                <td ></td>
                <td ></td>
                <td ></td>
                <td style="text-align: left" >
                    <asp:Button ID="btnAdd" runat="server" Text="Add Product in List" OnClick="btnAdd_Click" 
                        Width="116px" Height="25px"
                        Font-Size="X-Small" CssClass="btn-success"
                        ToolTip="Click here for add product in list ..." TabIndex="13" 
                        />
                </td>  
                <td align="left" ></td>
                <td align="left" ></td>
                <td align="left" ></td>
                <td align="left"></td>
                <td align="left"></td>              
            </tr>
            
            <tr>
                <td ></td>
                <td ></td>
                <td ></td>
                <td >&nbsp;</td>
                <td align="left" ></td>
                <td align="left" ></td>
                <td align="left" ></td>
                <td align="left"></td>
                <td align="left"></td>
                
            </tr>
            -->

        </table>
        
        
        <div>
        <asp:Label ID="lblError" ForeColor="red" runat="server" Text=""></asp:Label>
        </div>
        
        <table width="100%" style="font-family: Tahoma; font-size: small">
            <tr>
                    <td class="style19">
                        <!-- Data Grid -->  
                        <!-- -------------------------------------------- -->              
                        <asp:GridView ID="gvUsers" runat="server"
                        AutoGenerateColumns="false"                                       
                        CssClass="mGrid" 
                        DataKeyNames="ProductID" ShowHeaderWhenEmpty="true"                        
                        EmptyDataText = "No product in list !!! Please select model and add in list."                        
                        Onrowdeleting="gvUsers_RowDelating"
                        ShowFooter="true"
                        OnRowDataBound="gvUsers_RowDataBound"                        
                        Width="100%"> 
                                               
                        <FooterStyle Font-Bold="true" BackColor="#CCFFFF" ForeColor="black" BorderStyle="NotSet" />
                        
                        <Columns>
                            <asp:BoundField HeaderText="ProductID" DataField="ProductID" ItemStyle-Width="35px"/>                                
                            
                            <asp:BoundField HeaderText="Product Model" DataField="Model" />
                            <asp:BoundField HeaderText="Quantity" DataField="Qty" ItemStyle-Width="30px"/>                                                                                                         
                            
                            <asp:BoundField HeaderText="Remarks" DataField="Remarks"  />
                            
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
                        <EmptyDataRowStyle CssClass="gvEmpty" />
                        <HeaderStyle BackColor="#000066" ForeColor="White" />
                    </asp:GridView>
                        <!-- -------------------------------------------- -->

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
                <td align="left"> Speical Note/Remarks:</td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="txtNote" placeholder="Special Note/Remarks" CssClass="form-control" 
                        runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td> &nbsp;</td>
            </tr>
            <tr>
                <td align="center">                    
                    <asp:Button ID="btnSave" runat="server" Text="Submit" 
                        width="100px" onclick="btnSave_Click" TabIndex="7" 
                        Font-Size="Small" CssClass="btn btn-primary"
                        ToolTip="Click here for save data..." Enabled="True"/>
                        &nbsp;
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" 
                        Font-Size="Small" CssClass="btn btn-primary"
                        Width="100px" TabIndex="9" onclick="btnCancel_Click" />                    
                </td>
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



