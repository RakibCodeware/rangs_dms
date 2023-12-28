<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Search_Installation.aspx.cs" 
Inherits="Search_Installation" MasterPageFile="Admin.master" %>

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
        style="padding:5px; color: #FFFFFF; background-color: #006600;"> Search : Installation Information ...</h2>
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
                <td>Vendor Name</td>
                <td></td>
                <td>
                    <asp:DropDownList ID="ddlVendor" runat="server" CssClass="form-control" 
                        Width="250px">
                    </asp:DropDownList>
                </td>
                <td></td>
            </tr>


            <tr>
                <td></td>
                <td> Product Category</td>
                <td>:</td>
                <td>
                    <asp:DropDownList ID="ddlCat" runat="server" CssClass="form-control" Width="250px">
                    </asp:DropDownList>
                </td>
                <td></td>
                <td>Product Model</td>
                <td></td>
                <td>
                    <asp:DropDownList ID="ddlModel" runat="server" CssClass="form-control" 
                        Width="250px">
                    </asp:DropDownList>
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
            AlternatingRowStyle-CssClass="alt" AutoGenerateColumns="False" CssClass="mGrid" 
            BorderColor="#999999" BorderStyle="Double" BorderWidth="1px" CellPadding="2" 
            DataKeyNames="IAID" GridLines="Vertical" 
            OnRowDataBound="GridView1_RowDataBound" 
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
                                
                <asp:BoundField DataField="RefNo" HeaderText="Ref. #" />
                <asp:BoundField DataField="RefDate" HeaderText="Date" /> 
                <asp:BoundField DataField="InvNo" HeaderText="Invoice#" />
                <asp:BoundField DataField="eName" HeaderText="CTP Name" />
                <asp:BoundField DataField="VName" HeaderText="Vendor Name" />  
                <asp:BoundField DataField="CustName" HeaderText="Customer Name" />                
                <asp:BoundField DataField="CustMobile" HeaderText="Contact#" />
                <asp:BoundField DataField="InstDateAprx1" HeaderText="Inst.Date" />
                <asp:BoundField DataField="Model" HeaderText="Model" />
                <asp:BoundField DataField="tQty" HeaderText="Qty" />
                <asp:BoundField DataField="InsTag" HeaderText="Status" />

                <asp:TemplateField HeaderText="">
                    <ItemTemplate>
		                <asp:LinkButton ID="lnkView" Text="View"  OnClick="lnkView_Click" runat="server">
                        </asp:LinkButton>
	                </ItemTemplate>
                </asp:TemplateField>
                               
                <asp:TemplateField HeaderText="">
                    <ItemTemplate>
		                <asp:LinkButton ID="lnkEdit" Text="Edit"  OnClick="lnkEdit_Click" runat="server">
                        </asp:LinkButton>
	                </ItemTemplate>
                </asp:TemplateField>


                <asp:TemplateField HeaderText="">
                    <ItemTemplate>
		                <asp:LinkButton ID="lnkDel" Text="Cancel"  OnClick="lnkDel_Click" runat="server"
                            OnClientClick="return confirm('Do you want to Cancel this Installation ?');" >                            
                        </asp:LinkButton>
	                </ItemTemplate>
                </asp:TemplateField>


            </Columns>
        </asp:GridView>
        
    </div>

    <div>
        <asp:Button ID="btnExport" runat="server" Text="Export To Excel" 
            OnClick = "ExportToExcel" Height="28px" Visible="True" Width="161px" 
            BackColor="#000099" Font-Size="X-Small" ForeColor="Aqua" /> 
    </div>  
        
        
        
    <div>&nbsp;</div>    
    <div></div>

        <!-- ---------------- VIEW ------------------------------------------------------- -->
        <asp:Label ID="lblresult" runat="server"/>
        <asp:Button ID="btnShowPopup" runat="server" style="display:none" />
        
        <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="btnShowPopup" PopupControlID="pnlpopup"
            CancelControlID="btnCancel" BackgroundCssClass="modalBackground">
        </asp:ModalPopupExtender>

        <asp:Panel ID="pnlpopup" runat="server" BackColor="White" Width="600px" style="display:none; max-height: 800px; overflow: auto;">
            
            <table width="100%" style="border:3px Solid #669999; width:100%; height:100%; font-family: Tahoma; font-size: medium;" 
                cellpadding="0" cellspacing="0">
                <tr style="background-color:#669999">
                    <td colspan="5" style=" height:20px; color:White; font-weight:bold; font-size:larger" 
                        align="center">Installation Details</td>
                </tr>

                <tr>
                    <td>&nbsp;</td>
                    <td>
                        <asp:Label ID="lblAID" runat="server" Text="0" Visible="False"></asp:Label>
                    </td>
                    <td></td>
                    <td></td>
                    <td></td>
                </tr>
                <!-- ---------------------------------------------------------------------------------- -->
                <tr>
                    <td align="left" style=" width:2%">&nbsp;</td>
                    <td align="left" style="color: #FF0000; width:30%">
                        Ref.No. # &nbsp;
                    </td>
                    <td align="center" style=" width:2%">:</td>
                    <td align="left" style="color: #FF0000">
                        <asp:Label ID="lblRefNo" runat="server" Text="" Font-Size="Medium" 
                            ForeColor="#CC0000"></asp:Label>
                    </td>
                    <td align="left" style=" width:2%">
                        &nbsp;
                    </td>
                </tr>

                <tr>
                    <td align="left" style=" width:2%">&nbsp;</td>
                    <td align="left">
                        Ref.Date &nbsp;
                    </td>
                    <td>:</td>
                    <td align="left">
                        <asp:Label ID="lblRefDate" runat="server" Text="" Font-Size="Medium"></asp:Label>                            
                    </td>
                    <td align="left" style=" width:2%">
                        &nbsp;
                    </td>
                </tr>
                <!-- ---------------------------------------------------------------------------------- -->                            
                <tr>
                    <td align="left" style=" width:2%">&nbsp;</td>
                    <td align="left">
                        Invoice No. &nbsp;
                    </td>
                    <td>:</td>
                    <td align="left">
                        <asp:Label ID="lblInv" runat="server" Text="" Font-Size="Medium"></asp:Label>                            
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

                <tr>
                    <td align="left" style=" width:2%">&nbsp;</td>
                    <td align="left">
                        CTP Name &nbsp;
                    </td>
                    <td>:</td>
                    <td align="left">
                        <asp:Label ID="lblCTP" runat="server" Text="" Font-Size="Medium"></asp:Label>                            
                    </td>
                    <td align="left" style=" width:2%">
                        &nbsp;
                    </td>
                </tr>
                <!-- ---------------------------------------------------------------------------------- -->
                <tr>
                    <td align="left" style=" width:2%">&nbsp;</td>
                    <td align="left">
                        Vendor Name &nbsp;
                    </td>
                    <td>:</td>
                    <td align="left">
                        <asp:Label ID="lblVName" runat="server" Text="" Font-Size="Medium"></asp:Label>                            
                    </td>
                    <td align="left" style=" width:2%">
                        &nbsp;
                    </td>
                </tr>

                <tr>
                    <td align="left" style=" width:2%">&nbsp;</td>
                    <td align="left">
                        Vendor Address &nbsp;
                    </td>
                    <td>:</td>
                    <td align="left">
                        <asp:Label ID="lblVAdd" runat="server" Text="" Font-Size="Medium"></asp:Label>                            
                    </td>
                    <td align="left" style=" width:2%">
                        &nbsp;
                    </td>
                </tr>

                <tr>
                    <td align="left" style=" width:2%">&nbsp;</td>
                    <td align="left">
                        Vendor Contact &nbsp;
                    </td>
                    <td>:</td>
                    <td align="left">
                        <asp:Label ID="lblVContact" runat="server" Text="" Font-Size="Medium"></asp:Label>                            
                    </td>
                    <td align="left" style=" width:2%">
                        &nbsp;
                    </td>
                </tr>
                
                <tr>
                    <td align="left" style=" width:2%">&nbsp;</td>
                    <td align="left">
                        Customer Name &nbsp;
                    </td>
                    <td>:</td>
                    <td align="left">
                        <asp:Label ID="lblCustName" runat="server" Text="" Font-Size="Medium"></asp:Label>                            
                    </td>
                    <td align="left" style=" width:2%">
                        &nbsp;
                    </td>
                </tr>

                <tr>
                    <td align="left" style=" width:2%">&nbsp;</td>
                    <td align="left">
                        Customer Address &nbsp;
                    </td>
                    <td>:</td>
                    <td align="left">
                        <asp:Label ID="lblCustAdd" runat="server" Text="" Font-Size="Medium"></asp:Label>                            
                    </td>
                    <td align="left" style=" width:2%">
                        &nbsp;
                    </td>
                </tr>

                <!-- ---------------------------------------------------------------------------------- -->
                <tr>
                    <td align="left" style=" width:2%">&nbsp;</td>
                    <td align="left">
                        Contact # &nbsp;
                    </td>
                    <td>:</td>
                    <td align="left">
                        <asp:Label ID="lblContact" runat="server" Text="" Font-Size="Medium"></asp:Label>                            
                    </td>
                    <td align="left" style=" width:2%">
                        &nbsp;
                    </td>
                </tr>
                
                <tr>
                    <td align="left" style=" width:2%">&nbsp;</td>
                    <td align="left">
                        Inst.Date (Approx)&nbsp;
                    </td>
                    <td>:</td>
                    <td align="left">
                        <asp:Label ID="lblInsDate" runat="server" Text="" Font-Size="Medium"></asp:Label>                            
                    </td>
                    <td align="left" style=" width:2%">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td align="left" style=" width:2%">&nbsp;</td>
                    <td align="left">
                        Inst.Time (Approx)&nbsp;
                    </td>
                    <td>:</td>
                    <td align="left">
                        <asp:Label ID="lblInsTime" runat="server" Text="" Font-Size="Medium"></asp:Label>                            
                    </td>
                    <td align="left" style=" width:2%">
                        &nbsp;
                    </td>
                </tr>
                <!-- ---------------------------------------------------------------------------------- -->
                <tr>
                    <td align="left" style=" width:2%">&nbsp;</td>
                    <td align="left">
                        Status &nbsp;
                    </td>
                    <td>:</td>
                    <td align="left">
                        <asp:Label ID="lblStatus" runat="server" Text="" Font-Size="Medium"></asp:Label>                         
                    </td>
                    <td align="left" style=" width:2%">
                        &nbsp;
                    </td>
                </tr>               
                <!-- ---------------------------------------------------------------------------------- -->  
                
                <tr>
                    <td align="left" style=" width:2%">&nbsp;</td>
                    <td align="left" colspan="3">

                        <asp:Panel ID="Panel1" runat="server">
                        
                        <asp:GridView ID="GridView2" runat="server"                        
                            AutoGenerateColumns="False" 
                            DataKeyNames="Model" BorderColor="#999999" BorderStyle="Double"
                            GridLines="Vertical"
                            AllowPaging="false"
                            CssClass="mGrid" BorderWidth="1px" CellPadding="2"                        
                            PagerStyle-CssClass="pgr"                                               
                            AlternatingRowStyle-CssClass="alt"                              
                            Width="100%" ShowFooter="true"  >                      
                                                              
                            <SelectedRowStyle BackColor="BurlyWood"/>
                            <FooterStyle Font-Bold="true" BackColor="#D0ECE7" ForeColor="black" />
                            <Columns>
                                <asp:TemplateField HeaderText="SL#" HeaderStyle-Width="5%">
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                </asp:TemplateField>                       
                                <asp:BoundField DataField="Model" HeaderText="Model" />                                                      
                                <asp:BoundField DataField="tQty" HeaderText="Quantity" />
                                
                            </Columns>
                        </asp:GridView>

                        </asp:Panel>

                    </td>
                    <td align="left" style=" width:2%">
                        &nbsp;
                    </td>
                </tr>
                
                  
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

        <!-- ----------------- END VIEW ------------------------------------------------- -->


        


</asp:Content>
