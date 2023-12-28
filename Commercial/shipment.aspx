<%@ Page Language="C#" AutoEventWireup="true" CodeFile="shipment.aspx.cs" Inherits="shipment" 
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
        style="padding:5px; background-color: #000066; color: #FFFFFF;" align="center"> Shipment Information ...</h2>
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
                    class="style2">Shipment No. #</td>
                <td class="style3"> : </td>
                <td style="text-align: left" class="style4">
                    <asp:TextBox ID="txtCHNo" runat="server" Width="200px" 
                        CssClass="form-control" BackColor="#FFFFCC" Placeholder="Shipment No."
                        ToolTip="Please Enter Shipment No." 
                        MaxLength="15" Enabled="true"></asp:TextBox>
                </td>
                <td class="style5"> </td>                 
                
                <td class="style6" align="left">Shipment Date</td>
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
            
            
            <!-- Forwarder # -->
            <tr>
                <td class="style1">&nbsp;</td>
                <td style="text-align: Left;" 
                    class="style2">Forwarder Name</td>
                <td class="style3"> : </td>
                <td style="text-align: left" class="style4">
                    <asp:TextBox ID="TextBox1" runat="server" Width="200px" 
                        CssClass="form-control" Placeholder="Forwarder Name"
                        ToolTip="Please Enter Forwarder Name" 
                        MaxLength="50" Enabled="true"></asp:TextBox>
                </td>
                <td class="style5"> </td>                 
                
                <td class="style6" align="left">Forwarder Address</td>
                <td class="style7">:</td>
                <td class="style8" align="left">
                    <asp:TextBox ID="TextBox2" runat="server" Width="200px" 
                        CssClass="form-control" Placeholder="Forwarder Address"
                        ToolTip="Please Enter Forwarder Address" 
                        MaxLength="250" Enabled="true"></asp:TextBox>  
                </td>
                <td class="style9"></td>
            </tr>
            
            <!-- Forwarder -->
            <tr>
                <td class="style1">&nbsp;</td>
                <td style="text-align: Left;" 
                    class="style2">Forwarder Contact #</td>
                <td class="style3"> : </td>
                <td style="text-align: left" class="style4">
                    <asp:TextBox ID="TextBox3" runat="server" Width="200px" 
                        CssClass="form-control" Placeholder="Forwarder Contact #"
                        ToolTip="Please Enter Contact #" 
                        MaxLength="50" Enabled="true"></asp:TextBox>
                </td>
                <td class="style5"> </td>                 
                
                <td class="style6" align="left">Forwarder Email Add</td>
                <td class="style7">:</td>
                <td class="style8" align="left">
                    <asp:TextBox ID="TextBox23" runat="server" Width="200px" 
                        CssClass="form-control" Placeholder="Forwarder Email Add"
                        ToolTip="Please Enter Forwarder Email Add" 
                        MaxLength="50" Enabled="true"></asp:TextBox>
                </td>
                <td class="style9"></td>
            </tr>
            
            
            <!-- Shipping Line -->
            <tr>
                <td class="style1">&nbsp;</td>
                <td style="text-align: Left; " 
                    class="style2">Shipping Line Name</td>
                <td class="style3"> : </td>
                <td style="text-align: left" class="style4">
                    <asp:TextBox ID="TextBox4" runat="server" Width="200px" 
                        CssClass="form-control" Placeholder="Shipping Line Name"
                        ToolTip="Please Enter Shipping Line Name" 
                        MaxLength="15" Enabled="true"></asp:TextBox>
                </td>
                <td class="style5"> </td> 
                <td class="style6" align="left">Shipping Line Address</td>
                <td class="style7">:</td>
                <td class="style8" align="left">
                    <asp:TextBox ID="TextBox5" runat="server" Width="200px" 
                        CssClass="form-control" Placeholder="Shipping Line Address"
                        ToolTip="Please Enter Shipping Line Address" 
                        MaxLength="15" Enabled="true"></asp:TextBox>  
                </td>
                <td class="style9"></td>
            </tr>
            
            <!-- Container Info -->
            <tr>
                <td class="style1">&nbsp;</td>
                <td style="text-align: Left;" 
                    class="style2">No. of Container</td>
                <td class="style3"> : </td>
                <td style="text-align: left" class="style4">
                    <asp:TextBox ID="TextBox6" runat="server" Width="200px" 
                        CssClass="form-control" Placeholder="No. of Container"
                        ToolTip="Please Enter No. of Container" 
                        MaxLength="15" Enabled="true"></asp:TextBox>
                </td>
                <td class="style5"> </td>                 
                
                <td class="style6" align="left">Container Size</td>
                <td class="style7">:</td>
                <td class="style8" align="left">
                    <asp:TextBox ID="TextBox7" runat="server" Width="200px" 
                        CssClass="form-control" Placeholder="Container Size"
                        ToolTip="Please Enter Container Size" 
                        MaxLength="15" Enabled="true"></asp:TextBox>  
                </td>
                <td class="style9"></td>
            </tr>

            <!-- ETD/ETA -->
            <tr>
                <td class="style1">&nbsp;</td>
                <td style="text-align: Left;" 
                    class="style2">ETD</td>
                <td class="style3"> : </td>
                <td style="text-align: left" class="style4">
                    <asp:TextBox ID="TextBox8" runat="server" Width="200px" 
                        CssClass="form-control" Placeholder="Estimated Date of Departure"
                        ToolTip="Please Enter ETD" 
                        MaxLength="15" Enabled="true"></asp:TextBox>
                </td>
                <td class="style5"> </td>                 
                
                <td class="style6" align="left">ETA</td>
                <td class="style7">:</td>
                <td class="style8" align="left">
                    <asp:TextBox ID="TextBox9" runat="server" Width="200px" 
                        CssClass="form-control" Placeholder="Estimated Date of Arrival"
                        ToolTip="Please Enter ETA" 
                        MaxLength="15" Enabled="true"></asp:TextBox>  
                </td>
                <td class="style9"></td>
            </tr>
            
            <!-- ATD/ATA -->
            <tr>
                <td class="style1">&nbsp;</td>
                <td style="text-align: Left;" 
                    class="style2">ATD</td>
                <td class="style3"> : </td>
                <td style="text-align: left" class="style4">
                    <asp:TextBox ID="TextBox17" runat="server" Width="200px" 
                        CssClass="form-control" Placeholder="Actual Date of Departure"
                        ToolTip="Please Enter ETD" 
                        MaxLength="15" Enabled="true"></asp:TextBox>
                </td>
                <td class="style5"> </td>                 
                
                <td class="style6" align="left">ATA</td>
                <td class="style7">:</td>
                <td class="style8" align="left">
                    <asp:TextBox ID="TextBox18" runat="server" Width="200px" 
                        CssClass="form-control" Placeholder="Actual Date of Arrival"
                        ToolTip="Please Enter ETA" 
                        MaxLength="15" Enabled="true"></asp:TextBox>  
                </td>
                <td class="style9"></td>
            </tr>          
            
            <!-- Clearing Papers -->
            <tr>
                <td class="style1">&nbsp;</td>
                <td style="text-align: Left;" 
                    class="style2">Clearing Papers</td>
                <td class="style3"> : </td>
                <td align="left" class="style4" colspan="4">
                    <asp:CheckBoxList ID="CheckBoxList1" runat="server" Height="27px" Width="100%">
                        <asp:ListItem>Cl</asp:ListItem>
                        <asp:ListItem>Bill of Landing</asp:ListItem>
                        <asp:ListItem>Country of Origin</asp:ListItem>
                        <asp:ListItem>Packing List</asp:ListItem>
                        <asp:ListItem>Shipping Address</asp:ListItem>
                    </asp:CheckBoxList>
                </td>
                
                <td class="style9"></td>
            </tr>


            <!--  # -->
            <tr>
                <td class="style1">&nbsp;</td>
                <td style="text-align: Left;" 
                    class="style2">Document Sending Date</td>
                <td class="style3"> : </td>
                <td style="text-align: left" class="style4">
                    <asp:TextBox ID="TextBox12" runat="server" Width="200px" 
                        CssClass="form-control" Placeholder="Document Sending Date"
                        ToolTip="Please Enter " 
                        MaxLength="15" Enabled="true"></asp:TextBox>
                </td>
                <td class="style5"> </td>                 
                
                <td class="style6" align="left">Document Receiving Date</td>
                <td class="style7">:</td>
                <td class="style8" align="left">
                    <asp:TextBox ID="TextBox13" runat="server" Width="200px" 
                        CssClass="form-control" Placeholder="Document Receiving Date"
                        ToolTip="Please Enter " 
                        MaxLength="15" Enabled="true"></asp:TextBox>  
                </td>
                <td class="style9"></td>
            </tr>

            <!--  # -->
            <tr>
                <td class="style1">&nbsp;</td>
                <td style="text-align: Left;" 
                    class="style2">DHL No.</td>
                <td class="style3"> : </td>
                <td style="text-align: left" class="style4">
                    <asp:TextBox ID="TextBox14" runat="server" Width="200px" 
                        CssClass="form-control" Placeholder="DHL Number"
                        ToolTip="Please Enter " 
                        MaxLength="15" Enabled="true"></asp:TextBox>
                </td>
                <td class="style5"> </td>                 
                
                <td class="style6" align="left">Date of Document Retirement from Bank</td>
                <td class="style7">:</td>
                <td class="style8" align="left">
                    <asp:TextBox ID="TextBox15" runat="server" Width="200px" 
                        CssClass="form-control" Placeholder="Date of Document Retirement from Bank"
                        ToolTip="Please Enter " 
                        MaxLength="15" Enabled="true"></asp:TextBox>  
                </td>
                <td class="style9"></td>
            </tr>

            <!-- Line Break -->
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
            <!-- End Line Break -->

            <!-- --------------------------------------------------------------------------------------- -->
            <tr>
                <td colspan="9" align="center"
                    style="background-image:url(../Images/header.jpg); height:0.5px; font-family: Arial;">                        
                    </td>
            </tr>
            <!-- --------------------------------------------------------------------------------------- -->

            <!-- Line Break -->
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
            <!-- End Line Break -->

            <!--  LC# & Customs Duty -->
            <tr>
                <td class="style1">&nbsp;</td>
                <td style="text-align: Left;" 
                    class="style2">LC #</td>
                <td class="style3"> : </td>
                <td style="text-align: left" class="style4">
                    <asp:TextBox ID="TextBox10" runat="server" Width="200px" 
                        CssClass="form-control" Placeholder="LC Number"
                        ToolTip="Please Enter " 
                        MaxLength="15" Enabled="true"></asp:TextBox>
                </td>
                <td class="style5"> </td>                 
                
                <td class="style6" align="left">Customs Duty (USD)</td>
                <td class="style7">:</td>
                <td class="style8" align="left">
                    <asp:TextBox ID="TextBox11" runat="server" Width="200px" 
                        CssClass="form-control" Placeholder="Customs Duty"
                        ToolTip="Please Enter Customs Duty" 
                        MaxLength="15" Enabled="true"></asp:TextBox>  
                </td>
                <td class="style9">
                    <asp:Button ID="btnAddLC" CssClass="btn btn-primary" runat="server" Text="+" />
                </td>
            </tr>

            <!--  Misc/Port Damarage Amnt -->
            <tr>
                <td class="style1">&nbsp;</td>
                <td style="text-align: Left;" 
                    class="style2">Misc. Amount (USD)</td>
                <td class="style3"> : </td>
                <td style="text-align: left" class="style4">
                    <asp:TextBox ID="TextBox19" runat="server" Width="200px" 
                        CssClass="form-control" Placeholder="Misc. Amount"
                        ToolTip="Please Enter Misc. Amount (USD)" 
                        MaxLength="15" Enabled="true"></asp:TextBox>
                </td>
                <td class="style5"> </td>                 
                
                <td class="style6" align="left">Port Damarage (USD)</td>
                <td class="style7">:</td>
                <td class="style8" align="left">
                    <asp:TextBox ID="TextBox20" runat="server" Width="200px" 
                        CssClass="form-control" Placeholder="Port Damarage Amount"
                        ToolTip="Please Enter Port Damarage Amount" 
                        MaxLength="15" Enabled="true"></asp:TextBox>  
                </td>
                
            </tr>

            <!--  Shipping/Other Amnt -->
            <tr>
                <td class="style1">&nbsp;</td>
                <td style="text-align: Left;" 
                    class="style2">Container Shipping Charge (USD)</td>
                <td class="style3"> : </td>
                <td style="text-align: left" class="style4">
                    <asp:TextBox ID="TextBox21" runat="server" Width="200px" 
                        CssClass="form-control" Placeholder="Shipping Charge"
                        ToolTip="Please Enter " 
                        MaxLength="15" Enabled="true"></asp:TextBox>
                </td>
                <td class="style5"> </td>                 
                
                <td class="style6" align="left">Others Amount (USD)</td>
                <td class="style7">:</td>
                <td class="style8" align="left">
                    <asp:TextBox ID="TextBox22" runat="server" Width="200px" 
                        CssClass="form-control" Placeholder="Other Charge (If any)"
                        ToolTip="Please Enter " 
                        MaxLength="15" Enabled="true"></asp:TextBox>  
                </td>
                
            </tr>

            <!-- Line Break -->
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
            <!-- End Line Break -->

            <!-- --------------------------------------------------------------------------------------- -->
            <tr>
                <td colspan="9" align="center"
                    style="background-image:url(../Images/header.jpg); height:0.5px; font-family: Arial;">                        
                </td>
            </tr>
            <!-- --------------------------------------------------------------------------------------- -->

            <!-- Line Break -->
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
            <!-- End Line Break -->

            <!--  Remarks -->
            <tr>
                <td class="style1">&nbsp;</td>
                <td style="text-align: Left;" 
                    class="style2">Remarks</td>
                <td class="style3"> : </td>
                <td style="text-align: left" class="style4">
                    <asp:TextBox ID="TextBox16" runat="server" Width="200px" 
                        CssClass="form-control" Placeholder="Remarks/Note (if any)"
                        ToolTip="Please Enter " 
                        MaxLength="15" Enabled="true"></asp:TextBox>
                </td>
                <td class="style5"> </td>                 
                
                <td class="style6" align="left"></td>
                <td class="style7"></td>
                <td class="style8" align="left">
                     
                </td>
                <td class="style9"></td>
            </tr>


            
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

            
            <!-- Line Break -->
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
            <!-- End Line Break -->

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
                       
            
        </table>
        
        
        <div style="display :none ">
            <asp:Label ID="lblError" ForeColor="red" runat="server" Text=""></asp:Label>
        </div>
                        

   </div>


    <script src="../js/jquery.min.js" type="text/javascript"></script>
	<script src="../js/chosen.jquery.js" type="text/javascript"></script>
	<script type="text/javascript">
	    $(".chzn-select").chosen(); $(".chzn-select-deselect").chosen({ allow_single_deselect: true }); 
    </script>


</asp:Content>
