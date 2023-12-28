<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Search_discount_amnt.aspx.cs" 
Inherits="Search_discount_amnt" MasterPageFile="Admin.master" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    
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
    
    
    <script language="JavaScript" src="../js/datetimepicker.js" type="text/javascript"> </script>
    
    <script type="text/javascript">
        $(function () {
            $("#txtDate").datepicker();
            $("#txtFrom").datepicker();
            $("#txtToDate").datepicker();
            $("#txtPIDate").datepicker();
        });        
    </script>
    
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
    
                       
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    
    <h2 class="col-sm-12 bg-primary" 
        style="padding:5px; color: #FFFFFF; background-color: #CC0000;"> Search : Discount Amount ...</h2>
    <p></p>
    
    <div>
        
        <table width="100%">
            <tr>
                <td></td>
                <td>
                    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                </td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
            </tr>

            <tr>
                <td>&nbsp;</td>
                <td>
                    From Date
                </td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="txtFrom" runat="server" Width="110px" TabIndex="1" 
                        ToolTip="Please Enter From Date" MaxLength="10">
                    </asp:TextBox> 
                    <cc1:calendarextender ID="Calendar1" PopupButtonID="imgPopup" runat="server" TargetControlID="txtFrom"
                        Format="MM/dd/yyyy">
                    </cc1:calendarextender>
                
                    &nbsp;
                    <asp:ImageButton ID="imgPopup" ImageUrl="~/Images/cal.gif" ImageAlign="Bottom"
                        runat="server" TabIndex="1" />
                </td>
                <td></td>
                <td>To Date</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="txtToDate" runat="server" Width="110px" TabIndex="2" 
                            ToolTip="Please Enter To Date" MaxLength="10"></asp:TextBox> 
                    <cc1:calendarextender ID="Calendarextender1" PopupButtonID="imgPopup1" runat="server" TargetControlID="txtToDate"
                        Format="MM/dd/yyyy">
                    </cc1:calendarextender>
                
                &nbsp;<asp:ImageButton ID="imgPopup1" ImageUrl="~/Images/cal.gif" ImageAlign="Bottom"
                        runat="server" TabIndex="1" />
                    </td>
                <td></td>
            </tr>


            <tr>
                <td></td>
                <td></td>
                <td></td>
                <td>&nbsp;</td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
            </tr>


            <tr>
                <td></td>
                <td> CTP Name</td>
                <td>:</td>
                <td>
                    <asp:DropDownList ID="ddlEntity" runat="server" CssClass="form-control" Width="250px">
                    </asp:DropDownList>
                </td>
                <td></td>
                <td>Invoice/Order/Mobile No.</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="txtInvNo" runat="server" CssClass="form-control" 
                        Placeholder="Enter Invoice No./Order No./Mobile Number"
                        Width="100%"></asp:TextBox>
                </td>
                <td></td>
            </tr>

            <tr>
                <td></td>
                <td></td>
                <td></td>
                <td>&nbsp;</td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
            </tr>
            
            <tr>
                <td></td>
                <td>
                    <asp:Label ID="lblNetAmnt" runat="server" Text="0" Visible="False"></asp:Label>
                    <asp:TextBox ID="txtNetAmnt" runat="server" Visible="False" Width="77px"></asp:TextBox>
                </td>
                <td></td>
                <td>
                    <asp:Button ID="btnSearch" CssClass="btn btn-success" runat="server" 
                        data-toggle="tooltip" title="Click here for Search Sales Data ..."
                        Text="   Search  " OnClick="SearchData" />                        
                </td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
            </tr>


        </table>

    </div>

    <h4 class="col-sm-12 bg-primary" style="padding:0.5px"></h4>

    <div>        
        <asp:GridView ID="GridView1" runat="server" AllowPaging="True" 
            AlternatingRowStyle-CssClass="alt" AutoGenerateColumns="False" CssClass="table" 
            BorderColor="#999999" BorderStyle="Double" BorderWidth="1px" CellPadding="2" 
            DataKeyNames="MRSRCode" GridLines="Vertical" 
            OnRowDataBound="GridView1_RowDataBound" 
            OnPageIndexChanging="OnPageIndexChanging"
            PagerStyle-CssClass="pgr" 
            ShowFooter="true" 
            Width="100%" PageSize="550">
            <FooterStyle BackColor="#990000" ForeColor="White" />
            <HeaderStyle BackColor="Maroon" CssClass="bg-primary"/>

            <PagerStyle CssClass="pgr"></PagerStyle>
                
            <SelectedRowStyle BackColor="#0099CC" />
            <AlternatingRowStyle CssClass="alt" BackColor="#CCCCCC"></AlternatingRowStyle>
            <Columns>
                <asp:TemplateField HeaderText="SL#">
                    <ItemTemplate>
                        <%# Container.DataItemIndex + 1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                
                <asp:BoundField DataField="eName" HeaderText="eName" />       
                <asp:BoundField DataField="MRSRCode" HeaderText="Invoice#" />
                <asp:BoundField DataField="TDate" HeaderText="Sales.Date" />                              
                <asp:BoundField DataField="Model" HeaderText="Model" />
                <asp:BoundField DataField="UnitPrice" HeaderText="UnitPrice" />
                <asp:BoundField DataField="tQty" HeaderText="Qty" />
                <asp:BoundField DataField="TotalAmnt" HeaderText="TotalAmnt" />
                <asp:BoundField DataField="DiscountAmnt" HeaderText="DiscountAmnt" />
                <asp:BoundField DataField="NetAmnt" HeaderText="NetAmnt" />
                <asp:BoundField DataField="ProdRemarks" HeaderText="ProdRemarks" />
                <asp:BoundField DataField="DisCode" HeaderText="Dis.Code" />
                <asp:BoundField DataField="DisRef" HeaderText="Dis.Ref." />

                <asp:TemplateField HeaderText="">
                    <ItemTemplate>
		                <asp:LinkButton ID="lnkView" Text="View"  OnClick="lnkView_Click" runat="server">  
                        </asp:LinkButton>
	                </ItemTemplate>
                </asp:TemplateField>
                                 
                <asp:TemplateField HeaderText="">
                    <ItemTemplate>
		                <asp:LinkButton ID="lnkPrint" Text="Print"  OnClick="lnkPrint_Click" runat="server"
                            OnClientClick="return confirm('Do you want to Print this Invoice ?');" 
                            >
                        </asp:LinkButton>
	                </ItemTemplate>
                </asp:TemplateField>
                               
            </Columns>
        </asp:GridView>
        
    </div>
        
    <div>&nbsp;</div>
    
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
                        align="center">Sales Details </td>
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
                    <td align="left" style="color: #000000; font-size: large">Invoice # </td>
                    <td align="center">:</td>
                    <td align="left" style="color: #000080; font-size: large">
                        <asp:Label ID="lblInv" runat="server" Font-Size="Medium" Font-Bold="True"></asp:Label>
                    </td>
                    <td align="left" style=" width:2%"></td>
                    <td align="left" style="color: #000000; font-size: large">Sales Date</td>
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
                    <td align="left" style="color: #000000; ">Sex</td>
                    <td align="center">:</td>
                    <td align="left" style="color: #000080; ">
                        <asp:Label ID="lblSex" runat="server" ></asp:Label>
                    </td>
                </tr>

               
                <tr>
                    <td></td>
                    <td align="left" style="color: #000000;">Profession</td>
                    <td align="center">:</td>
                    <td align="left" style="color: #000080; ">
                        <asp:Label ID="lblProfession" runat="server"></asp:Label>
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
                    <td align="left" style="color: #000000;">Organization</td>
                    <td align="center">:</td>
                    <td align="left" style="color: #000080;">
                        <asp:Label ID="lblOrg" runat="server"></asp:Label>
                    </td>
                    <td align="left" style=" width:2%"></td>
                    <td align="left" style="color: #000000;">Designation</td>
                    <td align="center">:</td>
                    <td align="left" style="color: #000080;">
                        <asp:Label ID="lblDesg" runat="server"></asp:Label>
                    </td>
                </tr>
                              

                <tr>
                    <td></td>
                    <td align="left" style="color: #000000;">City</td>
                    <td align="center">:</td>
                    <td align="left" style="color: #000080;">
                        <asp:Label ID="lblCity" runat="server"></asp:Label>
                    </td>
                    <td align="left" style=" width:2%"></td>
                    <td align="left" style="color: #000000;">Location</td>
                    <td align="center">:</td>
                    <td align="left" style="color: #000080;">
                        <asp:Label ID="lblLoc" runat="server"></asp:Label>
                    </td>
                </tr>

                <tr>
                    <td></td>
                    <td align="left" style="color: #000000;">Date of Birth</td>
                    <td align="center">:</td>
                    <td align="left" style="color: #000080;">
                        <asp:Label ID="lblDOB" runat="server"></asp:Label>
                    </td>
                    <td align="left" style=" width:2%"></td>
                    <td align="left" style="color: #000000;">Age</td>
                    <td align="center">:</td>
                    <td align="left" style="color: #000080;">
                        <asp:Label ID="lblAge" runat="server"></asp:Label>
                    </td>
                </tr>


                <!-- GRID VIEW -->
                <tr>
                    <td></td>
                    <td align="left" colspan ="7">
                        <div style = "overflow:auto;width:100%; height:150px">
                        <asp:GridView ID="gvUsers" runat="server"
                            AutoGenerateColumns="false"                        
                            CssClass="mGrid"
                            DataKeyNames="ProductID"
                            EmptyDataText = "No product in list !!! Please select model and add in list."
                            EmptyDataRowStyle-CssClass ="gvEmpty"                            
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
                                        
                            </Columns>
                            <EmptyDataRowStyle CssClass="gvEmpty" />
                        </asp:GridView>
                        </div>
                    </td>
                    <td></td>
                </tr>


                                              
                <!-- ********************************************* -->

                <tr>
                    <td>&nbsp;</td>
                    <td>Total Amnt</td>
                    <td>:</td>
                    <td>
                        <asp:Label ID="lblTotalAmnt" runat="server" Text=""></asp:Label>                        
                    </td>
                    <td></td>
                    <td>Cash</td>
                    <td>:</td>
                    <td>
                        <asp:Label ID="lblCash" runat="server" Text=""></asp:Label>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>Card 1</td>
                    <td>:</td>
                    <td>
                        <asp:Label ID="lblCard1" runat="server" Text=""></asp:Label> 
                        <asp:Label ID="lblCardType1" runat="server" Text="" Visible="False"></asp:Label> 
                        <asp:Label ID="lblCardBank1" runat="server" Text="" Visible="False"></asp:Label>                        
                    </td>
                    <td></td>
                    <td>Card 2</td>
                    <td>:</td>
                    <td>
                        <asp:Label ID="lblCard2" runat="server" Text=""></asp:Label>
                        <asp:Label ID="lblCardType2" runat="server" Text="" Visible="False"></asp:Label> 
                        <asp:Label ID="lblCardBank2" runat="server" Text="" Visible="False"></asp:Label> 
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>Online Order #</td>
                    <td>:</td>
                    <td>
                        <asp:Label ID="lblOnlineOrderNo" runat="server" Text=""></asp:Label>                        
                    </td>
                    <td></td>
                    <td>T&C</td>
                    <td>:</td>
                    <td>
                        <asp:Label ID="lblWarrenty" runat="server" Text=""></asp:Label>
                    </td>
                    <td></td>
                </tr>

                <tr >
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
                    <td></td>
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
                        <asp:Button ID="btnSendMail" onclick="btnSendMail_Click" CssClass="btn btn-warning"  Width="100px" runat="server" Text="Invoice Mail" />
                        &nbsp;                    
                        <asp:Button ID="btnCancel" CssClass="btn btn-primary"  Width="100px" runat="server" Text="Ok" />
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
    
        
    

</asp:Content>
