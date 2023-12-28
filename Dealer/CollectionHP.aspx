<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CollectionHP.aspx.cs" 
Inherits="CollectionHP" MasterPageFile="Admin.master" %>

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
        .style10
        {
            width: 2%;
            font-family: Tahoma;
        }
    </style>

                       
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    
    <h2 class="col-sm-12 bg-danger" 
        
        style="padding:5px; background-color: #006666; color: #FFFFFF; font-family: Tahoma;" 
        align="center"> HP Collection ...</h2>
    
    <div style="width:100%; text-align:center ">        
        <asp:Label ID="lblmsg" runat="server" Text=""></asp:Label>
    </div>
    
    <div>
        
        <table width="100%">
            <tr>
                <td class="style1"></td>
                <td class="style2">
                    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                </td>
                <td class="style3"></td>
                <td class="style4"></td>
                <td class="style5"></td>
                <td class="style6"></td>
                <td class="style7"></td>
                <td class="style8"></td>
                <td class="style9"></td>
            </tr>


            <tr>
                <td></td>
                <td class="style1">
                    
                </td>
                <td></td>
                <td>
                    <asp:CheckBox ID="chkAutoNo" runat="server" class="checkbox-inline"
                        Text="Auto Money Receipt Number " oncheckedchanged="chkAutoNo_CheckedChanged" 
                        AutoPostBack="True" />
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
                    MR No. / Ref. #</td>
                <td></td>
                <td>
                    <asp:TextBox ID="txtRefNo" CssClass="form-control" runat="server" 
                        Width="200px" TabIndex="1" MaxLength="15">
                    </asp:TextBox>
                </td>
                <td></td>
                <td>Date</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="txtFrom"  runat="server" Width="121px" TabIndex="2" 
                        ToolTip="Please Enter From Date" MaxLength="10"></asp:TextBox> 
                    <cc1:calendarextender ID="Calendar1" PopupButtonID="imgPopup" runat="server" TargetControlID="txtFrom"
                        Format="MM/dd/yyyy">
                    </cc1:calendarextender>
                
                     &nbsp;<asp:ImageButton ID="imgPopup"  ImageUrl="~/Images/cal.gif" ImageAlign="Bottom"
                        runat="server" TabIndex="2" />
                </td>
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
                <td class="style10"><strong>Sales Invoice #</strong></td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="txtInv" CssClass="form-control" runat="server" Width="200px" 
                        MaxLength="45" TabIndex="3">
                    </asp:TextBox>
                    <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" TargetControlID="txtInv"
                            MinimumPrefixLength="1" EnableCaching="true" CompletionSetCount="1" CompletionInterval="100" 
                            ServiceMethod="GetSalesInv" >
                    </asp:AutoCompleteExtender>
                </td>
                <td></td>
                <td>Customer Name</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="txtCustName" CssClass="form-control" runat="server" Width="200px" 
                        MaxLength="45"
                        TabIndex="6" Enabled="False"></asp:TextBox>
                </td>
                <td></td>
            </tr>

            <tr>
                <td></td>
                <td class="style1">Total Due</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="txtTDue" CssClass="form-control" runat="server" Width="200px" 
                        MaxLength="45"
                        TabIndex="6" Enabled="False"></asp:TextBox>
                </td>
                <td></td>
                <td>Mobile #</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="txtMobile" CssClass="form-control" runat="server" Width="200px" 
                        MaxLength="45" TabIndex="6" Enabled="False"></asp:TextBox>
                </td>
                <td></td>
            </tr>

            <tr>
                <td></td>
                <td class="style1">Running Due</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="txtRDue" CssClass="form-control" runat="server" Width="200px" 
                        MaxLength="45"
                        TabIndex="6" Enabled="False"></asp:TextBox>
                </td>
                <td></td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>
                    <asp:TextBox ID="txtAppID" runat="server" Width="50px" 
                        MaxLength="45" Visible="false"
                        TabIndex="4"></asp:TextBox>

                        &nbsp;
                    <asp:TextBox ID="txtMRSRID" runat="server" Width="50px" 
                        MaxLength="45" Visible="false"
                        TabIndex="4"></asp:TextBox>

                        &nbsp;
                    <asp:TextBox ID="txtCustID" runat="server" Width="50px" 
                        MaxLength="45" Visible="false"
                        TabIndex="4"></asp:TextBox>
                </td>
                <td></td>
            </tr>

            <tr>
                <td></td>
                <td style="font-family: Tahoma; font-size: large; color: #000080" 
                    class="style1">Collect Amount</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="txtAmnt" CssClass="form-control" 
                        onkeypress="return numeric_only(event)"
                        MaxLength="8"
                        runat="server" Width="200px" TabIndex="4">
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
                    
                    Collection Type</td>
                <td></td>
                <td>
                    <asp:DropDownList ID="ddlType" CssClass="form-control" runat="server" 
                        Height="32px" Width="200px" TabIndex="5" AutoPostBack="True" 
                        onselectedindexchanged="ddlType_SelectedIndexChanged">
                        <asp:ListItem>CASH</asp:ListItem>
                        <asp:ListItem>CHEQUE</asp:ListItem>
                        <asp:ListItem>VISA</asp:ListItem>
                        <asp:ListItem>MASTER</asp:ListItem>
                        <asp:ListItem>AMEX</asp:ListItem>
                        <asp:ListItem>bKash</asp:ListItem>
                        <asp:ListItem>Roket</asp:ListItem>
                        <asp:ListItem>Others</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
            </tr>

            <tr>
                <td class="style1"></td>
                <td class="style2">                    
                    <asp:Label ID="lblNo" runat="server" Text="Cheque #" Visible="False"></asp:Label>
                </td>
                <td></td>
                <td>
                    <asp:TextBox ID="txtChequeNo" CssClass="form-control" runat="server" 
                        Width="200px" TabIndex="6" MaxLength="15" Visible="False"></asp:TextBox>
                </td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
            </tr>

            <tr>
                <td></td>
                <td class="style2">
                    <asp:Label ID="lblBank" runat="server" Text="Bank Name" Visible="False"></asp:Label>
                </td>
                <td></td>
                <td>
                    <asp:DropDownList ID="ddlBankName" CssClass="form-control" runat="server" 
                        Height="32px" Width="200px" TabIndex="7" Visible="False">
                    </asp:DropDownList>
                </td>
                <td></td>
                <td>
                    <asp:Label ID="lblBranch" runat="server" Text="Branch Name" Visible="False"></asp:Label>
                </td>
                <td></td>
                <td>
                    <asp:TextBox ID="txtBranch" CssClass="form-control" runat="server" Width="200px" 
                        MaxLength="45"
                        TabIndex="8" Visible="False"></asp:TextBox>
                </td>
                <td></td>
            </tr>
              

            <tr>
                <td></td>
                <td class="style1">                    
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
                <td></td>
                <td class="style1">Receive By</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="txtCollectBy" CssClass="form-control" runat="server" Width="200px" 
                        MaxLength="45"
                        TabIndex="9"></asp:TextBox>
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
                        MaxLength="200"
                        Width="200px" TabIndex="10">
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
                        Text="   Save  " OnClick="btnSaveClick" Width="111px" TabIndex="11" />                        
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
    style="padding:2.5px; background-color: #669999; color: #FFFFFF;" 
    align="center">Today Collection List</h4>

    <div>        
        <asp:GridView ID="GridView1" runat="server" AllowPaging="True" 
            AlternatingRowStyle-CssClass="alt" AutoGenerateColumns="False" CssClass="table" 
            BorderColor="#999999" BorderStyle="Double" BorderWidth="1px" CellPadding="2" 
            DataKeyNames="RefNo" GridLines="Vertical" 
            OnRowDataBound="GridView1_RowDataBound" 
            PagerStyle-CssClass="pgr" Width="100%" PageSize="10">
            <HeaderStyle BackColor="#006666" CssClass="bg-primary"/>

            <PagerStyle CssClass="pgr"></PagerStyle>
                
            <SelectedRowStyle BackColor="#0099CC" />
            <AlternatingRowStyle CssClass="alt" BackColor="#669999"></AlternatingRowStyle>
            <Columns>
                <asp:TemplateField HeaderText="SL#">
                    <ItemTemplate>
                        <%# Container.DataItemIndex + 1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                                
                <asp:BoundField DataField="MRNO" HeaderText="MR#" />
                <asp:BoundField DataField="MRDate" HeaderText="Date" />  
                <asp:BoundField DataField="cType" HeaderText="Collection Type"/>                                
                <asp:BoundField DataField="ChequeNo" HeaderText="Ref #"/>
                <asp:BoundField DataField="BankName" HeaderText="Bank Name"/>
                <asp:BoundField DataField="cAmnt" HeaderText="Collection Amnt" 
                    ItemStyle-HorizontalAlign="Right">                
                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="CollectBy" HeaderText="Collect By" />
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

</asp:Content>
