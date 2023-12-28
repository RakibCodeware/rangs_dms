<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CustomerOrder.aspx.cs" Inherits="CustomerOrder" 
MasterPageFile="Admin.master"  ValidateRequest="false"%>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server" >
    

    <!-- ------------------------------------------------------- -->
    <style type="text/css">
        .modalBackground
        {
            background-color: Black;
            filter: alpha(opacity=90);
            opacity: 0.8;
        }
        .modalPopup
        {
            background-color: #FFFFFF;
            border-width: 3px;
            border-style: solid;
            border-color: black;
            padding-top: 10px;
            padding-left: 10px;
            width: 300px;
            height: 140px;
        }
    </style>
    <style type="text/css">
      .hiddencol
      {
        display: none;
      }
    </style>
        
    <style type="text/css">
           
        .mGrid { 
            width: 100%; 
            background-color: #fff; 
            margin: 5px 0 10px 0; 
            border: solid 1px #525252; 
            border-collapse:collapse; 
        }
        .mGrid td { 
            padding: 2px; 
            border: solid 1px #c1c1c1; 
            color: #717171; 
        }
        .mGrid th { 
            padding: 4px 2px; 
            color: #fff; 
            background: #424242 url(grd_head.png) repeat-x top; 
            border-left: solid 1px #525252; 
            font-size: 0.9em; 
        }
        .mGrid .alt { background: #fcfcfc url(grd_alt.png) repeat-x top; }
        .mGrid .pgr { background: #424242 url(grd_pgr.png) repeat-x top; }
        .mGrid .pgr table { margin: 5px 0; }
        .mGrid .pgr td { 
            border-width: 0; 
            padding: 0 6px; 
            border-left: solid 1px #666; 
            font-weight: bold; 
            color: #fff; 
            line-height: 12px; 
         }   
        .mGrid .pgr a { color: #666; text-decoration: none; }
        .mGrid .pgr a:hover { color: #000; text-decoration: none; }


       .highlight
        {
            background-color: #ffeb95;
            cursor: pointer;
        }
        .normal
        {
            background-color: white;
            cursor: pointer;
        }
                        
    </style>

    <!-- ------------------------------------------------------- -->
           
    <h2 class="col-sm-12 bg-primary" 
        style="padding:5px; color: #FFFFFF; background-color: #008080;"> Online Order Information ...</h2>
    <p></p>

    <asp:Label ID="errorMsg" runat="server" Text=""></asp:Label>
    
    <table width ="100%">
        <tr>
            <td align="left">
                From Date: </td>

            <td align="left">
                <asp:TextBox ID="txtFrom" runat="server" Width="110px" TabIndex="1" 
                    ToolTip="Please Enter From Date" MaxLength="10">
                </asp:TextBox> 
                <cc1:calendarextender ID="Calendar1" PopupButtonID="imgPopup" runat="server" TargetControlID="txtFrom"
                    Format="MM/dd/yyyy">
                </cc1:calendarextender>
                
                &nbsp;
                <asp:ImageButton ID="imgPopup" ImageUrl="~/images/cal.gif" ImageAlign="Bottom"
                    runat="server" TabIndex="1" />  
            </td>
            <td>&nbsp;</td>
            <td>To Date:</td>
            <td align="left">
                <asp:TextBox ID="txtToDate" runat="server" Width="110px" TabIndex="2" 
                            ToolTip="Please Enter To Date" MaxLength="10"></asp:TextBox> 
                    <cc1:calendarextender ID="Calendarextender1" PopupButtonID="imgPopup1" runat="server" TargetControlID="txtToDate"
                        Format="MM/dd/yyyy">
                    </cc1:calendarextender>
                
                &nbsp;<asp:ImageButton ID="imgPopup1" ImageUrl="~/images/cal.gif" ImageAlign="Bottom"
                        runat="server" TabIndex="1" />   
            </td>

            <td align ="right">  
                <asp:Button ID="btnSearch" runat="server" Text="Search" Height="30px" 
                    CssClass="btn btn-primary" onclick="btnSearch_Click" />
            </td>
        </tr>

        <tr>
            <td align="left">Search By</td>
            <td>                
                <asp:TextBox ID="txtContact" class="form-control" placeholder="Order Number/Customer Name/Contact/Email" runat="server"></asp:TextBox>
            </td>
            <td>&nbsp;</td>
            <td></td>
            <td></td>
        </tr>

        <tr>
            <td align="left">CTP Name</td>
            <td>                
                <asp:DropDownList ID="ddlCTP" runat="server" class="form-control"> 
                </asp:DropDownList>
            </td>
            <td>&nbsp;</td>
            <td align="left">Status</td>
            <td>                
                <asp:DropDownList ID="ddlStatus" runat="server" class="form-control">
                    <asp:ListItem Value="5">ALL</asp:ListItem>
                    <asp:ListItem Value="0">Processing</asp:ListItem>
                    <asp:ListItem Value="1">Quality Check</asp:ListItem>
                    <asp:ListItem Value="2">Dispatched</asp:ListItem>
                    <asp:ListItem Value="3">Delivered</asp:ListItem>
                    <asp:ListItem Value="4">Cancel</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>

        

    </table>


    <div class="table-responsive">   
    
    <br />
       <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

      <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="DelID"            
            EmptyDataText="There are no data records to display."
            CssClass="table table-striped" GridLines="None">

            <Columns>
                <asp:TemplateField HeaderText="SL#">
                    <ItemTemplate>
                        <%# Container.DataItemIndex + 1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                                
                <asp:BoundField DataField="DelNo" HeaderText="Order #" SortExpression="DelNo" />
                <asp:BoundField DataField="DelDate" HeaderText="Date" SortExpression="DelDate" />
                
                <asp:BoundField DataField="CustName" HeaderText="CustName" SortExpression="CustName" />
                <asp:BoundField DataField="CustEmail" HeaderText="Email" SortExpression="CustEmail" />
                <asp:BoundField DataField="CustContact" HeaderText="Contact#" SortExpression="CustContact" />
                
                <asp:BoundField DataField="TotalQty" HeaderText="Qty" SortExpression="TotalQty" />
                <asp:BoundField DataField="TotalAmnt" HeaderText="Amount" SortExpression="TotalAmnt" />

                <asp:BoundField DataField="DelFrom" HeaderText="CTP" SortExpression="DelFrom" />

                <asp:BoundField DataField="tStatus" HeaderText="Status" SortExpression="tStatus" />
                                          
                              
                 <asp:TemplateField HeaderText="">
                    <ItemTemplate>
		                <asp:LinkButton ID="lnkView" Text="View"  OnClick="lnkView_Click" runat="server">  
                        </asp:LinkButton>
	                </ItemTemplate>
                </asp:TemplateField>
                                                             
          </Columns>
      </asp:GridView>
      
            
      <asp:AccessDataSource ID="AccessDataSource2" runat="server" DataFile="~/App_Data/db.mdb"          
          
          SelectCommand="SELECT `AID`, `CustName`, `EmailAdd`, `ContactNo`, `Address`, `UserName`, `PW` FROM `tbCustomerInfo` ">
                             
      </asp:AccessDataSource>
          
    </div>

    <!-- ================================================================================= -->
    <!-- ***************************************************************************************** -->
        <asp:Button ID="btnShowPopup" runat="server" style="display:none" />
        
        <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="btnShowPopup" PopupControlID="pnlpopup"
            CancelControlID="btnCancel" BackgroundCssClass="modalBackground">
        </asp:ModalPopupExtender>

        <asp:Panel ID="pnlpopup" runat="server"  BackColor="White" Height="550px" Width="700px" style="display:none">

            <table width="100%"  cellpadding="0" cellspacing="0">
                
                <tr style="background-color:#D55500">
                    <td colspan="9" 
                        style=" height:10%; color:White; font-weight:bold; font-size:x-large; font-family: Tahoma;" 
                        align="center">Order Details </td>
                </tr>
                
                <tr>
                    <td>&nbsp;</td>
                    <td></td>
                    <td></td>
                    <td>&nbsp;</td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td>&nbsp;&nbsp;</td>
                </tr>

                <tr>
                    <td width="2%"></td>
                    <td align="left" style=" width:20%"> </td>
                    <td width="2%"></td>
                    <td align="left" style=" width:30%">
                        <asp:Label ID="lblID" runat="server" Visible="False"></asp:Label>
                    </td>
                    <td align="left" style=" width:2%">&nbsp;</td>
                    <td align="left" style=" width:15%"></td>
                    <td width="2%"></td>
                    <td align="left" style=" width:28%"></td>
                    <td width="4%"></td>
                </tr>
                <tr>
                    <td></td>
                    <td align="left" style="color: #000000; font-size: large">Order # </td>
                    <td align="center">:</td>
                    <td align="left" style="color: #000080; font-size: large">
                        <asp:Label ID="lblInv" runat="server" Font-Size="Medium" Font-Bold="True"></asp:Label>
                    </td>
                    <td align="left" style=" width:2%"></td>
                    <td align="left" style="color: #000000; font-size: large">Order Date</td>
                    <td align="center">:</td>
                    <td align="left" style="color: #000080; ">
                        <asp:Label ID="lblDate" runat="server" Font-Bold="True"></asp:Label>

                    </td>
                    <td></td>
                </tr>
                
                <tr>
                    <td></td>
                    <td align="left" style="color: #000000;">Customer Name</td>
                    <td align="center">:</td>
                    <td align="left" style="color: #000080; ">
                        <asp:Label ID="lblCustName" runat="server" ></asp:Label>
                        <asp:Label ID="lblCustID" runat="server" Visible="False"></asp:Label>
                    </td>

                    <td align="left" style=" width:2%"></td>
                    <td align="left" style="color: #000000;">Contact # </td>
                    <td align="center">:</td>
                    <td align="left" style="color: #000080; ">
                        <asp:Label ID="lblContact" runat="server" ></asp:Label>
                    </td>
                </tr>

               
                <tr>
                    <td></td>
                    <td align="left" style="color: #000000; ">Address</td>
                    <td align="center">:</td>
                    <td align="left" style="color: #000080; ">
                        <asp:Label ID="lblAdd" runat="server" ></asp:Label>
                    </td>
                    <td align="left" style=" width:2%"></td>
                    <td align="left" style="color: #000000; ">Email</td>
                    <td align="center">:</td>
                    <td align="left" style="color: #000080; ">
                        <asp:Label ID="lblEmail" runat="server"></asp:Label>
                    </td>
                </tr>
                                   

                <tr>
                    <td></td>
                    <td align="left" style="color: #000000;">City/Dist</td>
                    <td align="center">:</td>
                    <td align="left" style="color: #000080;">
                        <asp:Label ID="lblDist" runat="server"></asp:Label>
                    </td>
                    <td align="left" style=" width:2%"></td>
                    <td align="left" style="color: #000000;">Thana</td>
                    <td align="center">:</td>
                    <td align="left" style="color: #000080;">
                        <asp:Label ID="lblThana" runat="server"></asp:Label>
                    </td>
                </tr>

                

                <!-- GRID VIEW -->
                <tr>
                    <td></td>
                    <td align="left" colspan ="7">
                        <div style = "overflow:auto;width:100%; height:130px">
                            
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    
                                    <asp:GridView ID="gvUsers" runat="server"
                                        AutoGenerateColumns="false"                        
                                        CssClass="mGrid"
                                        DataKeyNames="DelDID"
                                        EmptyDataText = "No product in list !!!"
                                        EmptyDataRowStyle-CssClass ="gvEmpty"                            
                                        ShowFooter="true"                                                  
                                        GridLines="none" Width="100%">
                                        <FooterStyle Font-Bold="true" BackColor="#61A6F8" ForeColor="black" />
                                        <Columns>
                                            <asp:BoundField HeaderText="DelDID" DataField="DelDID"/>                        
                                            <asp:BoundField HeaderText="Model" DataField="title" /> 
                                            <asp:BoundField HeaderText="Desc" DataField="titleDesc" />                       
                                            <asp:BoundField HeaderText="Sale Price" DataField="SalePrice" />    
                                            <asp:BoundField HeaderText="Qty" DataField="tQty" />
                                            <asp:BoundField HeaderText="Total Price" DataField="tAmnt" />                                         
                                        </Columns>
                                        <EmptyDataRowStyle CssClass="gvEmpty" />
                                    </asp:GridView>

                                </ContentTemplate>
                            </asp:UpdatePanel>

                        </div>
                    </td>
                    <td></td>
                </tr>


                                              
                <!-- ********************************************* -->
                <tr>
                    <td>&nbsp;</td>
                    <td>Total Amount</td>
                    <td>:</td>
                    <td>
                        <asp:Label ID="lblTotalAmnt" runat="server" Text="0"></asp:Label>
                        <asp:Label ID="lblShipping" runat="server" Visible="False" Text="0"></asp:Label>
                        <asp:Label ID="lblTax" runat="server" Visible="False" Text="0"></asp:Label>
                    </td>
                    <td></td>
                    <td>Payment Type</td>
                    <td>:</td>
                    <td>
                        <asp:Label ID="lblPaymentMethod" runat="server" Text=""></asp:Label>
                    </td>                    
                    <td></td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>Delivery Type</td>
                    <td>:</td>
                    <td>
                        <asp:Label ID="lblDelType" runat="server" Text=""></asp:Label>
                    </td>
                    <td></td>
                    <td>Delivery From</td>
                    <td>:</td>
                    <td>
                        <asp:Label ID="lblEName" runat="server" Text=""></asp:Label>
                        <asp:Label ID="lblCTPEmail" runat="server" Visible="False" Text=""></asp:Label>
                        <asp:Label ID="lblCTPAdd" runat="server" Visible="False" Text=""></asp:Label>
                    </td>                    
                    <td></td>
                </tr>

                <tr>
                    <td>&nbsp;</td>
                    <td>Status</td>
                    <td>:</td>
                    <td colspan="6">
                        <asp:RadioButtonList ID="RadioButtonList1" runat="server" 
                            RepeatDirection="Horizontal" Width="100%">
                            <asp:ListItem Value="0" Selected="True">&nbsp;In Progress</asp:ListItem>
                            <asp:ListItem Value="1">&nbsp;Quality Check</asp:ListItem>
                            <asp:ListItem Value="2">&nbsp;Dispatched</asp:ListItem>
                            <asp:ListItem Value="3">&nbsp;Delivered</asp:ListItem>
                            <asp:ListItem Value="4">&nbsp;Cancel</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                    
                </tr>

                <tr>
                    <td>&nbsp;</td>
                    <td>Remarks/Cancel Reason</td>
                    <td>:</td>
                    <td>
                        <asp:TextBox ID="txtNote" CssClass="form-control" runat="server" MaxLength="200"></asp:TextBox>
                    </td>
                    <td></td>
                    <td></td>
                    <td>&nbsp;</td>
                    <td></td>
                    <td></td>
                </tr>

                <tr>
                    <td>&nbsp;</td>
                    <td></td>
                    <td></td>
                    <td>&nbsp;</td>
                    <td></td>
                    <td></td>
                    <td>&nbsp;</td>
                    <td></td>
                    <td></td>
                </tr>

                <tr>
                    <td></td>                    
                    <td colspan="4">                        
                        <asp:Button ID="btnUpdate" onclick="btnUpdate_Click" CssClass="btn btn-primary"  Width="100px" runat="server" Text="Update" />
                        &nbsp;
                        <asp:Button ID="btnSendMail" onclick="btnSendMail_Click" CssClass="btn btn-warning"  Width="100px" runat="server" Text="Mail Re-Send" />
                        &nbsp;
                        <asp:Button ID="btnCancel" CssClass="btn btn-primary"  Width="100px" runat="server" Text="Close" />
                    </td>
                    <td>&nbsp;</td>
                    <td></td>
                    <td></td>
                    <td></td>                    
                </tr>

                <tr>
                    <td>&nbsp;</td>
                    <td></td>
                    <td></td>
                    <td>&nbsp;</td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                </tr>

            </table>

        </asp:Panel>
        <!-- ***************************************************************************************** -->
        
        <div></div>
    <!-- ================================================================================= -->

    


    <script type="text/javascript">
   	    $('.jqte-editor').jqte();
    </script>

</asp:Content>