<%@ Page Language="C#" AutoEventWireup="true" CodeFile="approval_list.aspx.cs" Inherits="MD_approval_list" 
    MasterPageFile="Admin.master"%>

<%@ Register namespace="AjaxControlToolkit" tagprefix="AjaxControlToolkit" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
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
    
    <style type="text/css">
        .modalBackground
        {
            background-color: Gray;
            filter: alpha(opacity=80);
            opacity: 0.8;
            z-index: 10000;
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
    
                       
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    
    <h2 class="col-sm-12 bg-primary" 
        style="padding:5px; color: #FFFFFF; background-color: #006666;"> Waiting list for Approval ...</h2>
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
                        

        </table>

    </div>

    <h4 class="col-sm-12 bg-primary" style="padding:0.5px"></h4>

    <div>        
        <asp:GridView ID="GridView1" runat="server" AllowPaging="True" 
            AlternatingRowStyle-CssClass="alt" AutoGenerateColumns="False" CssClass="mGrid" 
            BorderColor="#999999" BorderStyle="Double" BorderWidth="1px" CellPadding="2" 
            DataKeyNames="RAID" GridLines="Vertical"             
            OnPageIndexChanging="OnPageIndexChanging"
            PagerStyle-CssClass="pgr" 
            ShowFooter="true" 
            Width="100%" PageSize="50">
            
            <FooterStyle Font-Bold="True" Font-Size="Larger" ForeColor="Black" />
            
            <PagerStyle CssClass="pgr"></PagerStyle>
                
            <SelectedRowStyle BackColor="#0099CC" />
            <AlternatingRowStyle CssClass="alt" BackColor="#F7F3FF"></AlternatingRowStyle>
            <Columns>
                <asp:TemplateField HeaderText="SL#">
                    <ItemTemplate>
                        <%# Container.DataItemIndex + 1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                                
                <asp:BoundField DataField="MRSRCode" HeaderText="Invoice/Challan#" />
                <asp:BoundField DataField="TDate" HeaderText="Invoice Date" />                 
                <asp:BoundField DataField="TransactionType" HeaderText="Tr.Type" />
                <asp:BoundField DataField="eFrom" HeaderText="From" />  
                <asp:BoundField DataField="eTo" HeaderText="To" />   
                <asp:BoundField DataField="ReqFor" HeaderText="Req.For" />
                <asp:BoundField DataField="ReqReason" HeaderText="Reasone" />             
                <asp:BoundField DataField="ReqBy" HeaderText="Request From" />
                <asp:BoundField DataField="EntryDate" HeaderText="Req.Date" />
                
                <asp:TemplateField HeaderText="">
                    <ItemTemplate>
		                <asp:LinkButton ID="lnkView" Text="View"  OnClick="lnkView_Click" runat="server" >  
                        </asp:LinkButton>
	                </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="">
                    <ItemTemplate>
		                <asp:LinkButton ID="lnkApp" Text="Approved"  OnClick="lnkApp_Click" runat="server"
                            OnClientClick="return confirm('Do you want to Approve ?');" 
                            >
                        </asp:LinkButton>
	                </ItemTemplate>
                </asp:TemplateField>


                <asp:TemplateField HeaderText="">
                    <ItemTemplate>
		                <asp:LinkButton ID="lnkDel" Text="Deny"  OnClick="lnkDel_Click" runat="server">    
                        </asp:LinkButton>
	                </ItemTemplate>
                </asp:TemplateField>


            </Columns>
        </asp:GridView>
        
    </div>
        
    <div>&nbsp;</div>    
    <div></div>

        <!-- ------------------------------------------------------------------ -->
        <asp:Label ID="lblresult" runat="server"/>
        <asp:Button ID="btnShowPopup" runat="server" style="display:none" />
        
        <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="btnShowPopup" PopupControlID="pnlpopup"
            CancelControlID="btnCancel" BackgroundCssClass="modalBackground">
        </asp:ModalPopupExtender>

        <asp:Panel ID="pnlpopup" runat="server" BackColor="White" Width="600px" style="display:none; max-height: 500px; overflow: auto;">
            
            <table width="100%" style="border:3px Solid #669999; width:100%; height:100%; font-family: Tahoma; font-size: medium;" 
                cellpadding="0" cellspacing="0">
                <tr style="background-color:#669999">
                    <td colspan="5" style=" height:20px; color:White; font-weight:bold; font-size:larger" 
                        align="center">Request Details</td>
                </tr>

                <tr>
                    <td>&nbsp;</td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                </tr>
                <!-- ---------------------------------------------------------------------------------- -->
                <tr>
                    <td align="left" style=" width:2%">&nbsp;</td>
                    <td align="left" style="color: #FF0000">
                        Invoice/Challan # &nbsp;
                    </td>
                    <td>:</td>
                    <td align="left" style="color: #FF0000">
                        <asp:Label ID="lblInvNo" runat="server" Text="" Font-Size="Medium" 
                            ForeColor="#CC0000"></asp:Label>
                    </td>
                    <td align="left" style=" width:2%">
                        &nbsp;
                    </td>
                </tr>

                <tr>
                    <td align="left" style=" width:2%">&nbsp;</td>
                    <td align="left">
                        Invoice Date &nbsp;
                    </td>
                    <td>:</td>
                    <td align="left">
                        <asp:Label ID="lblInvDate" runat="server" Text="" Font-Size="Medium"></asp:Label>                            
                    </td>
                    <td align="left" style=" width:2%">
                        &nbsp;
                    </td>
                </tr>
                <!-- ---------------------------------------------------------------------------------- -->                            
                <tr>
                    <td align="left" style=" width:2%">&nbsp;</td>
                    <td align="left">
                        Transaction Type &nbsp;
                    </td>
                    <td>:</td>
                    <td align="left">
                        <asp:Label ID="lblTrType" runat="server" Text="" Font-Size="Medium"></asp:Label>                            
                    </td>
                    <td align="left" style=" width:2%">
                        &nbsp;
                    </td>
                </tr>
                
                <tr>
                    <td align="left" style=" width:2%">&nbsp;</td>
                    <td align="left">
                        From &nbsp;
                    </td>
                    <td>:</td>
                    <td align="left">
                        <asp:Label ID="lblFrom" runat="server" Text="" Font-Size="Medium"></asp:Label>                            
                    </td>
                    <td align="left" style=" width:2%">
                        &nbsp;
                    </td>
                </tr>
                <!-- ---------------------------------------------------------------------------------- -->
                <tr>
                    <td align="left" style=" width:2%">&nbsp;</td>
                    <td align="left">
                        To &nbsp;
                    </td>
                    <td>:</td>
                    <td align="left">
                        <asp:Label ID="lblTo" runat="server" Text="" Font-Size="Medium"></asp:Label>                            
                    </td>
                    <td align="left" style=" width:2%">
                        &nbsp;
                    </td>
                </tr>
                
                <tr>
                    <td align="left" style=" width:2%">&nbsp;</td>
                    <td align="left">
                        Request For &nbsp;
                    </td>
                    <td>:</td>
                    <td align="left">
                        <asp:Label ID="lblReqFor" runat="server" Text="" Font-Size="Medium"></asp:Label>                            
                    </td>
                    <td align="left" style=" width:2%">
                        &nbsp;
                    </td>
                </tr>
                <!-- ---------------------------------------------------------------------------------- -->
                <tr>
                    <td align="left" style=" width:2%">&nbsp;</td>
                    <td align="left">
                        Reason: &nbsp;
                    </td>
                    <td>:</td>
                    <td align="left">
                        <asp:Label ID="lblReason" runat="server" Text="" Font-Size="Medium"></asp:Label>                            
                    </td>
                    <td align="left" style=" width:2%">
                        &nbsp;
                    </td>
                </tr>
                
                <tr>
                    <td align="left" style=" width:2%">&nbsp;</td>
                    <td align="left">
                        Request by &nbsp;
                    </td>
                    <td>:</td>
                    <td align="left">
                        <asp:Label ID="lblReqBy" runat="server" Text="" Font-Size="Medium"></asp:Label>                            
                    </td>
                    <td align="left" style=" width:2%">
                        &nbsp;
                    </td>
                </tr>
                <!-- ---------------------------------------------------------------------------------- -->
                <tr>
                    <td align="left" style=" width:2%">&nbsp;</td>
                    <td align="left">
                        Request Date &nbsp;
                    </td>
                    <td>:</td>
                    <td align="left">
                        <asp:Label ID="lblReqDate" runat="server" Text="" Font-Size="Medium"></asp:Label>                            
                    </td>
                    <td align="left" style=" width:2%">
                        &nbsp;
                    </td>
                </tr>               
                <!-- ---------------------------------------------------------------------------------- -->    
                <tr>
                    <td></td>
                    
                    <td>
                        <asp:Button ID="btnUpdate" CssClass="btn btn-info" CommandName="Update" runat="server" Text="Update" 
                             Visible="False"/>
                        <asp:Button ID="btnCancel" CssClass="btn btn-info" runat="server" Text=" Ok " />
                    </td>
                    <td></td>
                    <td></td>
                    <td></td>
                </tr>
                <!-- ---------------------------------------------------------------------------------- -->
                <tr>
                    <td>&nbsp;</td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                </tr>

            </table>

            
        </asp:Panel>

        <!-- ------------------------------------------------------------------ -->


        <!-- ------------------------------------------------------------------ -->
        <asp:Label ID="Label1" runat="server"/>
        <asp:Button ID="Button1" runat="server" style="display:none" />
        
        <asp:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="Button1" PopupControlID="pnlpopup1"
            CancelControlID="btnCancel" BackgroundCssClass="modalBackground">
        </asp:ModalPopupExtender>

        <asp:Panel ID="pnlpopup1" runat="server" BackColor="White" Width="600px" style="display:none; max-height: 500px; overflow: auto;">
            
            <table width="100%" style="border:3px Solid #669999; width:100%; height:100%; font-family: Tahoma; font-size: medium;" 
                cellpadding="0" cellspacing="0">
                <tr style="background-color:#669999">
                    <td colspan="5" style=" height:20px; color:White; font-weight:bold; font-size:larger" 
                        align="center">Deny Confirmation</td>
                </tr>

                <tr>
                    <td>&nbsp;</td>
                    <td>
                        <asp:Label ID="lblAID" runat="server" Text="" Visible="False"></asp:Label>
                    </td>
                    <td></td>
                    <td></td>
                    <td></td>
                </tr>
                <!-- ---------------------------------------------------------------------------------- -->
                <tr>
                    <td align="left" style=" width:2%">&nbsp;</td>
                    <td align="left" style="color: #FF0000">
                        Invoice/Challan # &nbsp;
                    </td>
                    <td>:</td>
                    <td align="left" style="color: #FF0000">
                        <asp:Label ID="lblInvNo1" runat="server" Text="" Font-Size="Medium" 
                            ForeColor="#CC0000"></asp:Label>
                    </td>
                    <td align="left" style=" width:2%">
                        &nbsp;
                    </td>
                </tr>

                <tr>
                    <td align="left" style=" width:2%">&nbsp;</td>
                    <td align="left">
                        Invoice Date &nbsp;
                    </td>
                    <td>:</td>
                    <td align="left">
                        <asp:Label ID="lblInvDate1" runat="server" Text="" Font-Size="Medium"></asp:Label>                            
                    </td>
                    <td align="left" style=" width:2%">
                        &nbsp;
                    </td>
                </tr>
                <!-- ---------------------------------------------------------------------------------- -->                            
                <tr>
                    <td align="left" style=" width:2%">&nbsp;</td>
                    <td align="left">
                        Transaction Type &nbsp;
                    </td>
                    <td>:</td>
                    <td align="left">
                        <asp:Label ID="lblTrType1" runat="server" Text="" Font-Size="Medium"></asp:Label>                            
                    </td>
                    <td align="left" style=" width:2%">
                        &nbsp;
                    </td>
                </tr>
                
                <tr>
                    <td align="left" style=" width:2%">&nbsp;</td>
                    <td align="left">
                        From &nbsp;
                    </td>
                    <td>:</td>
                    <td align="left">
                        <asp:Label ID="lblFrom1" runat="server" Text="" Font-Size="Medium"></asp:Label>                            
                    </td>
                    <td align="left" style=" width:2%">
                        &nbsp;
                    </td>
                </tr>
                <!-- ---------------------------------------------------------------------------------- -->
                <tr>
                    <td align="left" style=" width:2%">&nbsp;</td>
                    <td align="left">
                        To &nbsp;
                    </td>
                    <td>:</td>
                    <td align="left">
                        <asp:Label ID="lblTo1" runat="server" Text="" Font-Size="Medium"></asp:Label>                            
                    </td>
                    <td align="left" style=" width:2%">
                        &nbsp;
                    </td>
                </tr>
                
                <tr>
                    <td align="left" style=" width:2%">&nbsp;</td>
                    <td align="left">
                        Request For &nbsp;
                    </td>
                    <td>:</td>
                    <td align="left">
                        <asp:Label ID="lblReqFor1" runat="server" Text="" Font-Size="Medium"></asp:Label>                            
                    </td>
                    <td align="left" style=" width:2%">
                        &nbsp;
                    </td>
                </tr>
                <!-- ---------------------------------------------------------------------------------- -->
                <tr>
                    <td align="left" style=" width:2%">&nbsp;</td>
                    <td align="left">
                        Reason: &nbsp;
                    </td>
                    <td>:</td>
                    <td align="left">
                        <asp:Label ID="lblReason1" runat="server" Text="" Font-Size="Medium"></asp:Label>                            
                    </td>
                    <td align="left" style=" width:2%">
                        &nbsp;
                    </td>
                </tr>
                
                <tr>
                    <td align="left" style=" width:2%">&nbsp;</td>
                    <td align="left">
                        Request by &nbsp;
                    </td>
                    <td>:</td>
                    <td align="left">
                        <asp:Label ID="lblReqBy1" runat="server" Text="" Font-Size="Medium"></asp:Label>                            
                    </td>
                    <td align="left" style=" width:2%">
                        &nbsp;
                    </td>
                </tr>
                <!-- ---------------------------------------------------------------------------------- -->
                <tr>
                    <td align="left" style=" width:2%">&nbsp;</td>
                    <td align="left">
                        Request Date &nbsp;
                    </td>
                    <td>:</td>
                    <td align="left">
                        <asp:Label ID="lblReqDate1" runat="server" Text="" Font-Size="Medium"></asp:Label>                            
                    </td>
                    <td align="left" style=" width:2%">
                        &nbsp;
                    </td>
                </tr> 
                
                <tr>
                    <td align="left" style=" width:2%">&nbsp;</td>
                    <td align="left">
                        Deny Reason &nbsp;
                    </td>
                    <td>:</td>
                    <td align="left">
                        <asp:TextBox ID="txtDenyReason" CssClass="form-control" runat="server" TextMode="MultiLine"></asp:TextBox>                     
                    </td>
                    <td align="left" style=" width:2%">
                        &nbsp;
                    </td>
                </tr> 
                
                <tr>
                    <td>&nbsp;</td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                </tr>
                              
                <!-- ---------------------------------------------------------------------------------- -->    
                <tr>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td>
                        <asp:Button ID="btnDConfirm" CssClass="btn btn-primary" CommandName="Update" 
                            runat="server" Text="Deny Confirm" OnClick="btnDConfirm_Click" Visible="True"/>                             
                        <asp:Button ID="Button3" CssClass="btn btn-primary" runat="server" Text="Cancel" />
                    </td>
                    
                    <td></td>
                </tr>
                <!-- ---------------------------------------------------------------------------------- -->
                <tr>
                    <td>&nbsp;</td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                </tr>

            </table>

            
        </asp:Panel>

        <!-- ------------------------------------------------------------------ -->


</asp:Content>