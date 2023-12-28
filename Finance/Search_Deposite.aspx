﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Search_Deposite.aspx.cs" 
Inherits="Search_Deposite_info" MasterPageFile="Admin.master"
ValidateRequest="false" EnableEventValidation = "false" %>

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
                        
        .style1
        {
            height: 21px;
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
    
    <h2 class="col-sm-12 bg-danger" 
        style="padding:5px; background-color: #800080; color: #FFFFFF;"> Search : Deposite Information ...</h2>
    
    <div style="width:100%; text-align:center ">        
        <asp:Label ID="lblmsg" runat="server" Text=""></asp:Label>
    </div>
    
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
                        ToolTip="Please Enter From Date" MaxLength="10"></asp:TextBox> 
                    <cc1:calendarextender ID="Calendar1" PopupButtonID="imgPopup" runat="server" TargetControlID="txtFrom"
                        Format="MM/dd/yyyy">
                    </cc1:calendarextender>
                
                &nbsp;<asp:ImageButton ID="imgPopup" ImageUrl="~/Images/cal.gif" ImageAlign="Bottom"
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
                <td>
                    
                </td>
                <td></td>
                <td style="font-family: Tahoma; color: #FF0000">(MM/dd/yyyy)</td>
                <td></td>
                <td></td>
                <td></td>
                <td style="font-family: Tahoma; color: #FF0000">(MM/dd/yyyy)</td>
                <td></td>
            </tr>

            <tr>
                <td></td>
                <td>
                    
                </td>
                <td></td>
                <td>&nbsp;</td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
            </tr>


            <tr>
                <td class="style1"></td>
                <td class="style1">
                    
                    Bank Name</td>
                <td class="style1">:</td>
                <td class="style1">
                    <asp:DropDownList ID="ddlBank" CssClass="form-control" 
                        runat="server" Width="270px">
                    </asp:DropDownList>
                </td>
                <td class="style1"></td>
                <td class="style1">Deposit Type</td>
                <td class="style1" align="center">:</td>
                <td class="style1">
                    <asp:DropDownList ID="ddlDepositeType" runat="server" 
                        CssClass="form-control" Width="270px">
                        <asp:ListItem>ALL</asp:ListItem>
                        <asp:ListItem>CASH</asp:ListItem>
                        <asp:ListItem>CARD</asp:ListItem>
                        <asp:ListItem>CHEQUE</asp:ListItem>
                        <asp:ListItem>REQUISITION</asp:ListItem>
                        <asp:ListItem>Online Payment</asp:ListItem>
                        <asp:ListItem>bKash</asp:ListItem>
                        <asp:ListItem>Rocket</asp:ListItem>
                        <asp:ListItem>JV</asp:ListItem>   
                    </asp:DropDownList>
                </td>
                <td class="style1"></td>
            </tr>
            
            <tr>
                <td></td>
                <td> Entity Name</td>
                <td>:</td>
                <td>
                    <asp:DropDownList ID="ddlEntity" runat="server" CssClass="form-control" Width="270px">
                    </asp:DropDownList>
                </td>
                <td></td>
                <td>Search By</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="txtInvNo" runat="server" CssClass="form-control" 
                        Placeholder="Ref. No./ Card No./ Amount"
                        Width="100%"></asp:TextBox>
                </td>
                <td></td>
            </tr>
                    

            <tr>
                <td></td>
                <td>Accounts Approval</td>
                <td>:</td>
                <td>                   
                    <asp:DropDownList ID="ddlApproval" runat="server" 
                        CssClass="form-control" Width="270px">
                        <asp:ListItem Value="3">ALL</asp:ListItem>
                        <asp:ListItem Value="1">Approved</asp:ListItem>
                        <asp:ListItem Value="2">Dishonour</asp:ListItem>
                        <asp:ListItem Value="0">Pending</asp:ListItem>                        
                    </asp:DropDownList>
                </td>
                
                <td>&nbsp;</td>
                
                <td></td>
                <td></td>
                <td>
                    <asp:RadioButtonList ID="rbEntityType" runat="server" 
                        RepeatDirection="Horizontal" Width="100%">
                        <asp:ListItem Selected="True">ALL</asp:ListItem>
                        <asp:ListItem>CTP</asp:ListItem>
                        <asp:ListItem>Dealer</asp:ListItem>
                        <asp:ListItem>Corporate</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
                <td></td>
            </tr>

            <tr>
                <td class="style1"></td>
                <td class="style1">                    
                    POS Machine Name</td>
                <td class="style1">:</td>
                <td class="style1">
                    <asp:DropDownList ID="ddlBankName1" CssClass="form-control" 
                        runat="server" Width="270px">
                    </asp:DropDownList>
                </td>
                <td class="style1"></td>
                <td class="style1"></td>
                <td class="style1" align="center"></td>
                <td class="style1">
                   
                </td>
                <td class="style1"></td>
            </tr>

            <tr>
                <td></td>
                <td>
                    
                </td>
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
                    
                </td>
                <td></td>
                <td>
                    <asp:Button ID="btnSearch" CssClass="btn btn-primary" runat="server" 
                        data-toggle="tooltip" title="Click here for Search Sales Data ..."
                        Text="   Search  " OnClick="SearchData" />                        
                </td>
                <td></td>
                <td></td>
                <td></td>
                <td>
                    
                </td>
                <td></td>
            </tr>


        </table>

    </div>

    <h4 class="col-sm-12 bg-primary" style="padding:0.5px"></h4>

    <div>        
        <asp:GridView ID="GridView1" runat="server" AllowPaging="True" 
            AlternatingRowStyle-CssClass="alt" AutoGenerateColumns="False" CssClass="table" 
            BorderColor="#999999" BorderStyle="Double" BorderWidth="1px" CellPadding="2" 
            DataKeyNames="RefNo" GridLines="Vertical" 
            OnRowDataBound="GridView1_RowDataBound" 
            OnPageIndexChanging="OnPageIndexChanging"
            PagerStyle-CssClass="pgr" 
            ShowFooter="true" 
            Width="100%" PageSize="2000">
            <FooterStyle BackColor="#006666" ForeColor="White" />
            <HeaderStyle BackColor="#006666" CssClass="bg-primary"/>

            <PagerStyle CssClass="pgr"></PagerStyle>
                
            <SelectedRowStyle BackColor="#0099CC" />
            <AlternatingRowStyle CssClass="alt" BackColor="#C2D69B"></AlternatingRowStyle>
            <Columns>
                <asp:TemplateField HeaderText="SL#">
                    <ItemTemplate>
                        <%# Container.DataItemIndex + 1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                                
                <%--<asp:BoundField DataField="RefNo" HeaderText="Deposit #" />
                <asp:BoundField DataField="DepositDate" HeaderText="Deposit Date" /> 
                <asp:BoundField DataField="BankName" HeaderText="Bank Name" />                
                <asp:BoundField DataField="BranchName" HeaderText="Branch" />
                <asp:BoundField DataField="DepositAmnt" HeaderText="Deposit Amnt" />
                <asp:BoundField DataField="DepositType" HeaderText="Deposit Type" />
                <asp:BoundField DataField="DepositBy" HeaderText="Deposit By" />
                <asp:BoundField DataField="CardNo" HeaderText="Card No" />
                <asp:BoundField DataField="Remarks" HeaderText="Remarks" />
                <asp:BoundField DataField="eName" HeaderText="CTP Name" />
                <asp:BoundField DataField="aStatus" HeaderText="Accounts Approval" /> --%>

                <asp:BoundField DataField="RefNo" HeaderText="Deposit #" />
                <asp:BoundField DataField="DepositDate" HeaderText="Deposit Date" /> 
                <asp:BoundField DataField="BankName" HeaderText="Bank Name" />                
                <asp:BoundField DataField="BranchName" HeaderText="Branch" />
                <asp:BoundField DataField="DepositAmnt" HeaderText="Deposit Amnt" />
                 <asp:BoundField DataField="IpdcAmnt" HeaderText="IPDC Amnt" />
                <asp:BoundField DataField="DepositType" HeaderText="Deposit Type" />
                <asp:BoundField DataField="CardNo" HeaderText="Card No" />
                <asp:BoundField DataField="POSMachine" HeaderText="POS Machine" />
                <asp:BoundField DataField="ApprovalCode" HeaderText="Approval Code" />
                <asp:BoundField DataField="DepositBy" HeaderText="Deposit By" />
                <asp:BoundField DataField="Remarks" HeaderText="Remarks" />
                <asp:BoundField DataField="eName" HeaderText="CTP Name" />
                <asp:BoundField DataField="aStatus" HeaderText="Accounts Approval" />
                <asp:BoundField DataField="EMEITenor" HeaderText="EMEI Tenor" />
                <asp:BoundField DataField="DealerName" HeaderText="Dealer Name" />

                 <%--<asp:TemplateField HeaderText="">
                    <ItemTemplate>
		                <asp:LinkButton ID="lnkEdit" Text="View"  OnClick="lnkEdit_Click" runat="server">
                        </asp:LinkButton>
	                </ItemTemplate>
                </asp:TemplateField>--%>
                


            </Columns>
        </asp:GridView>
        
    </div>
        
    <div>&nbsp;</div>    

    <div>
        <asp:Button ID="btnDownloadToExcel" runat="server" Text="Download To Excel" Height="30px" 
                    CssClass="btn btn-primary" onclick="btnDownloadToExcel_Click" />
        &nbsp;
        <asp:Button ID="btnExportToPDF" runat="server" Text="Download To PDF" Height="30px" 
                    CssClass="btn btn-primary" onclick="btnExportToPDF_Click" />
    </div>


    <!-- ***************************************************************************************** -->

    <asp:Button ID="btnShowPopup" runat="server" style="display:none" />
        
        <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="btnShowPopup" PopupControlID="pnlpopup"
            CancelControlID="btnCancel" BackgroundCssClass="modalBackground">
        </asp:ModalPopupExtender>

        <asp:Panel ID="pnlpopup" runat="server" BackColor="White" Height="450px" Width="550px" style="display:none">
            <table width="100%" style="border:Solid 3px #D55500; width:100%; height:100%" cellpadding="0" cellspacing="0">
                
                <tr style="background-color:#D55500">
                    <td colspan="3" style=" height:10%; color:White; font-weight:bold; font-size:larger" align="center">Deposit Edit/Update </td>
                </tr>
                
                <tr>
                    <td>&nbsp;</td>
                    <td></td>
                    <td></td>
                </tr>

                <tr>
                    <td align="left" style=" width:10%"></td>
                    <td align="left" style=" width:30%"> </td>
                    <td>
                        <asp:Label ID="lblID" runat="server" Visible="False"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="left" style=" width:10%"></td>
                    <td align="left" style="color: #000080; font-size: large">Reference # </td>
                    <td align="left" style="color: #000080; font-size: large">
                        <asp:Label ID="lblRefNo" runat="server" Font-Size="Medium" Font-Bold="True"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="left" style=" width:10%"></td>
                    <td align="left">Deposit Date :</td>
                    <td>
                        <asp:Label ID="lblDDate" runat="server"/>
                    </td>
                </tr>
                
                <tr>
                    <td align="left" style=" width:10%"></td>
                    <td align="left">Bank Name :</td>
                    <td>
                        <asp:Label ID="lblBankName" runat="server"/>
                    </td>
                </tr>

                <tr>
                    <td align="left" style=" width:10%"></td>
                    <td align="left">Branch Name :</td>
                    <td>                        
                        <asp:Label ID="lblBrName" runat="server"/>
                    </td>
                </tr>
                
                <tr>
                    <td align="left" style=" width:10%"></td>
                    <td align="left">Deposit Amnt :</td>
                    <td>
                        <asp:Label ID="lblAmnt1" runat="server"/>
                    </td>
                </tr>
                
                <tr>
                    <td align="left" style=" width:10%"></td>
                    <td align="left">Deposit Type :</td>
                    <td>
                        <asp:Label ID="lblDepositeType" runat="server"/>
                    </td>
                </tr>

                <tr>
                    <td align="left" style=" width:10%"></td>
                    <td align="left">Card No (If any) :</td>
                    <td>
                        <asp:Label ID="lblCardNo" runat="server"/>
                    </td>
                </tr>

                <tr>
                    <td align="left" style=" width:10%"></td>
                    <td align="left">Approval Code (If any) :</td>
                    <td>
                        <asp:Label ID="lblAppCode" runat="server"/>
                    </td>
                </tr>

                <tr>
                    <td align="left" style=" width:10%"></td>
                    <td align="left">POS Machine :</td>
                    <td>
                        <asp:Label ID="lblPOSName" runat="server"/>
                    </td>
                </tr>
                
                <tr>
                    <td align="left" style=" width:10%"></td>
                    <td align="left">Deposit By :</td>
                    <td>
                        <asp:Label ID="lblDBy" runat="server"/>
                    </td>
                </tr>

                <tr>
                    <td align="left" style=" width:10%"></td>
                    <td align="left">Remarks :</td>
                    <td>
                        <asp:Label ID="lblRemarks1" runat="server"/>
                    </td>
                </tr>

                <tr>
                    <td align="left" style=" width:10%"></td>
                    <td align="left">Accounts status</td>
                    <td>
                        <asp:RadioButtonList ID="RadioButtonList1" runat="server" Height="16px" 
                            RepeatDirection="Horizontal" Width="207px">
                            <asp:ListItem Value="1">Received</asp:ListItem>
                            <asp:ListItem Value="2">Dishonour</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td align="left" style=" width:10%"></td>
                    <td align="left">Accounts Note</td>
                    <td>
                        <asp:TextBox ID="txtNote" runat="server"  CssClass="form-control" 
                            TextMode="MultiLine" MaxLength="500"></asp:TextBox>
                    </td>
                </tr>

                                              
                <!-- ********************************************* -->

                <tr>
                    <td>&nbsp;</td>
                    <td></td>
                    <td></td>
                </tr>

                <tr>
                    <td></td>
                    <td></td>
                    <td>
                        <asp:Button ID="btnUpdate" CommandName="Update" runat="server" Text="Update" 
                            onclick="btnUpdate_Click" />
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" />
                    </td>
                </tr>

                <tr>
                    <td>&nbsp;</td>
                    <td></td>
                    <td></td>
                </tr>

            </table>

        </asp:Panel>
    </div>


    <div> &nbsp;</div>

    <div></div>

</asp:Content>
