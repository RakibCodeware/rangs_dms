<%@ Page Language="C#" AutoEventWireup="true" CodeFile="payment.aspx.cs" 
Inherits="payment" MasterPageFile="Admin.master" %>

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
            width: 237px;
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
    
    <h2 class="col-sm-12 bg-danger" 
        
        style="padding:5px; background-color: #800080; color: #FFFFFF; font-family: Tahoma;" 
        align="center"> Payment Entry ...</h2>
    
    <div style="width:100%; text-align:center ">        
        <asp:Label ID="lblmsg" runat="server" Text=""></asp:Label>
    </div>
    
    <div>
        
        <table width="100%">
            <tr>
                <td></td>
                <td class="style1">
                    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                </td>
                <td></td>
                <td>
                    <asp:TextBox ID="txtAID" runat="server" Visible="False"></asp:TextBox>
                </td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
            </tr>


            <tr>
                <td></td>
                <td class="style1">
                    
                </td>
                <td></td>
                <td>
                    <asp:CheckBox ID="chkAutoNo" runat="server" class="checkbox-inline"
                        Text="Auto Deposit Number " oncheckedchanged="chkAutoNo_CheckedChanged" 
                        AutoPostBack="True" Checked="True" />
                </td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
            </tr>

            <tr>
                <td></td>
                <td style="font-family: Tahoma; font-size: large; color: #800080" 
                    class="style1">
                    Payment Slip/Ref. #</td>
                <td></td>
                <td>
                    <asp:TextBox ID="txtRefNo" CssClass="form-control" runat="server" 
                        Width="270px" TabIndex="1" MaxLength="15" ToolTip="Auto Deposit Number">
                    </asp:TextBox>
                </td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
            </tr>

            <tr>
                <td>&nbsp;</td>
                <td class="style1">
                    Payment Date
                </td>
                <td>:</td>
                <td CssClass="form-control">
                <asp:TextBox ID="txtFrom"  runat="server" Width="121px" TabIndex="2" 
                        ToolTip="Please Enter From Date" MaxLength="10"></asp:TextBox> 
                    <cc1:calendarextender ID="Calendar1" PopupButtonID="imgPopup" runat="server" TargetControlID="txtFrom"
                        Format="MM/dd/yyyy">
                    </cc1:calendarextender>
                
                &nbsp;<asp:ImageButton ID="imgPopup"  ImageUrl="~/Images/cal.gif" ImageAlign="Bottom"
                        runat="server" TabIndex="2" />
                    </td>
                <td></td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>
                
                &nbsp;</td>
                <td></td>
            </tr>

            <tr>
                <td></td>
                <td class="style1">
                    
                </td>
                <td></td>
                <td style="color: #FF0000">(MM/dd/yyyy)</td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
            </tr>

            <tr>
                <td></td>
                <td class="style1">
                    
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
                <td class="style1">Bank Name</td>
                <td>:</td>
                <td>
                    <asp:DropDownList ID="ddlBankName" CssClass="form-control" runat="server" 
                        Height="32px" Width="270px" 
                        TabIndex="3">
                    </asp:DropDownList>
                </td>
                <td></td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td></td>
            </tr>
            
            <tr>
                <td></td>
                <td class="style1"> Branch Name</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="txtBranch" CssClass="form-control" runat="server" Width="270px" 
                        MaxLength="45" Placeholder="Barch Name" TabIndex="4">                        
                    </asp:TextBox>
                </td>
                <td></td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td></td>
            </tr>

            <tr>
                <td></td>
                <td class="style1"> Account Number</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="TextBox2" CssClass="form-control" runat="server" Width="270px" 
                        MaxLength="45" Placeholder="Account Number" TabIndex="4">                        
                    </asp:TextBox>
                </td>
                <td></td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td></td>
            </tr>

            <tr>
                <td></td>
                <td class="style1"> Swift Code</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="TextBox3" CssClass="form-control" runat="server" Width="270px" 
                        MaxLength="45" Placeholder="Bank Swift Code" TabIndex="4">                        
                    </asp:TextBox>
                </td>
                <td></td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td></td>
            </tr>

            <tr>
                <td></td>
                <td class="style1"></td>
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
                <td style="font-family: Tahoma; font-size: large; color: #000080" 
                    class="style1">PI #</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="TextBox1" CssClass="form-control"                         
                        MaxLength="20" placeholder="Proforma Invoice Number"
                        runat="server" Width="121px"  TabIndex="5">
                    </asp:TextBox>
                </td>
                <td></td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>
                    &nbsp;</td>
                <td></td>
            </tr>

            <tr>
                <td></td>
                <td style="font-family: Tahoma; font-size: large; color: #000080" 
                    class="style1">Payment Amount</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="txtAmnt" CssClass="form-control" 
                        onkeypress="return numeric_only(event)"
                        MaxLength="8" placeholder="Payment Amount"
                        runat="server" Width="121px" TabIndex="5">
                    </asp:TextBox>
                </td>
                <td></td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td></td>
            </tr>

            <tr>
                <td></td>
                <td style="font-family: Tahoma; font-size: medium; color: #000080" 
                    class="style1">Pay Type</td>
                <td>:</td>
                <td>
                    <asp:DropDownList ID="ddlDepositeType" runat="server" 
                        CssClass="form-control" Width="270px">
                        <asp:ListItem>CASH</asp:ListItem>
                        <asp:ListItem>TT</asp:ListItem>
                        <asp:ListItem>CHEQUE</asp:ListItem>
                        
                    </asp:DropDownList>
                </td>
                <td></td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>
                    &nbsp;</td>
                <td></td>
            </tr>

            <tr>
                <td></td>
                <td class="style1">
                    
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
                <td class="style1">Payment By</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="txtDepositBy" CssClass="form-control" runat="server" Width="270px" 
                        MaxLength="45" Placeholder="Payment By"
                        TabIndex="6">
                    </asp:TextBox>
                </td>
                <td></td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>
                    &nbsp;</td>
                <td></td>
            </tr>

            <tr>
                <td></td>
                <td class="style1">Remarks</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="txtRemarks" CssClass="form-control" runat="server" 
                        MaxLength="200" Placeholder="Remarks/Note (If any)"
                        Width="270px" TabIndex="7">
                    </asp:TextBox>
                </td>
                <td></td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>
                    &nbsp;</td>
                <td></td>
            </tr>


            <tr>
                <td></td>
                <td class="style1">
                    
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
                <td class="style1"></td>     
                <td></td>
                <td>
                    <asp:Button ID="btnSave" CssClass="btn btn-primary" runat="server" 
                        data-toggle="tooltip" title="Click here for Search Sales Data ..."
                        Text="   Save  " OnClick="btnSaveClick" Width="111px" TabIndex="8" />                        
                </td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
            </tr>


        </table>

    </div>

    <h4 class="col-sm-12 bg-primary" 
    style="padding:2.5px; background-color: #800080; color: #FFFFFF;" 
    align="center">Today Payment List</h4>

    <div>        
        <asp:GridView ID="GridView1" runat="server" AllowPaging="True" 
            AlternatingRowStyle-CssClass="alt" AutoGenerateColumns="False" CssClass="table" 
            BorderColor="#999999" BorderStyle="Double" BorderWidth="1px" CellPadding="2" 
            DataKeyNames="RefNo" GridLines="Vertical" 
            OnRowDataBound="GridView1_RowDataBound" 
            PagerStyle-CssClass="pgr" Width="100%" PageSize="50">
            <HeaderStyle BackColor="#000066" CssClass="bg-primary" ForeColor="White"/>

            <PagerStyle CssClass="pgr"></PagerStyle>
                
            <SelectedRowStyle BackColor="#0099CC" />
            <AlternatingRowStyle CssClass="alt" BackColor="#6699FF"></AlternatingRowStyle>
            <Columns>
                <asp:TemplateField HeaderText="SL#">
                    <ItemTemplate>
                        <%# Container.DataItemIndex + 1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                                
                <asp:BoundField DataField="RefNo" HeaderText="Ref #" />
                <asp:BoundField DataField="DepositDate" HeaderText="Date" />                                  
                <asp:BoundField DataField="BankName" HeaderText="Bank Name"/>
                <asp:BoundField DataField="BranchName" HeaderText="Branch Name"/>
                <asp:BoundField DataField="DepositAmnt" HeaderText="Pay Amnt" 
                    ItemStyle-HorizontalAlign="Right">                
                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="DepositType" HeaderText="Type" />
                <asp:BoundField DataField="DepositBy" HeaderText="Payment By" />
                <asp:BoundField DataField="Remarks" HeaderText="Remarks"/>
                
                <asp:TemplateField HeaderText="">
                    <ItemTemplate>
		                <asp:LinkButton ID="lnkEdit" Text="Edit"  OnClick="lnkEdit_Click" runat="server"
                            OnClientClick="return confirm('Do you want to edit this record?');" 
                            >
                        </asp:LinkButton>
	                </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="">
                    <ItemTemplate>
		                <asp:LinkButton ID="lnkDel" Text="Delete"  OnClick="lnkDel_Click" runat="server"
                            OnClientClick="return confirm('Do you want to Delete this record?');" >
                            </asp:LinkButton>
	                </ItemTemplate>
                </asp:TemplateField>
                
            </Columns>
        </asp:GridView>
        
    </div>

    <div> &nbsp;</div>

    <!-- ***************************************************************************************** -->

    <asp:Button ID="btnShowPopup" runat="server" style="display:none" />
        
        <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="btnShowPopup" PopupControlID="pnlpopup"
            CancelControlID="btnCancel" BackgroundCssClass="modalBackground">
        </asp:ModalPopupExtender>

        <asp:Panel ID="pnlpopup" runat="server" BackColor="White" Height="350px" Width="550px" style="display:none">
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
                    <td align="left">Payment Date :</td>
                    <td>
                        <asp:Label ID="lblDDate" runat="server"/>
                    </td>
                </tr>
                
                <tr>
                    <td align="left" style=" width:10%"></td>
                    <td align="left">Bank Name :</td>
                    <td>
                        <asp:DropDownList ID="DropDownList1" Width="270px" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>

                <tr>
                    <td align="left" style=" width:10%"></td>
                    <td align="left">Branch Name :</td>
                    <td>
                        <asp:TextBox ID="txtBrName" Width="270px" runat="server"/>
                    </td>
                </tr>
                
                <tr>
                    <td align="left" style=" width:10%"></td>
                    <td align="left">Payment Amnt :</td>
                    <td>
                        <asp:TextBox ID="txtAmnt1" Width="270px" runat="server"/>
                    </td>
                </tr>
                
                <tr>
                    <td align="left" style=" width:10%"></td>
                    <td align="left">Payment Type :</td>
                    <td>
                        <asp:DropDownList ID="ddlDType" runat="server" 
                            CssClass="form-control" Width="270px">
                            <asp:ListItem>CASH</asp:ListItem>
                            <asp:ListItem>CHEQUE</asp:ListItem>
                            <asp:ListItem>TT</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                
                <tr>
                    <td align="left" style=" width:10%"></td>
                    <td align="left">Payment By :</td>
                    <td>
                        <asp:TextBox ID="txtDBy" Width="270px" runat="server"/>
                    </td>
                </tr>

                <tr>
                    <td align="left" style=" width:10%"></td>
                    <td align="left">Remarks :</td>
                    <td>
                        <asp:TextBox ID="txtRemarks1" Width="270px" runat="server"/>
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
                        <asp:Button ID="btnUpdate" CommandName="Update" runat="server" Text="Update" onclick="btnUpdate_Click"/>
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
    


    <div> &nbsp;</div>

</asp:Content>
