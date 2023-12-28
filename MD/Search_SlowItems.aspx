<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Search_SlowItems.aspx.cs" 
Inherits="Search_SlowItems" MasterPageFile="Admin.master" %>


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
            background-color: Gray;
            filter: alpha(opacity=80);
            opacity: 0.8;
            z-index: 10000;
        }
    </style>

    
                       
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    
    <h2 class="col-sm-12 bg-primary" 
        style="padding:5px; color: #FFFFFF; background-color: #006666;"> Search : Slow Items List ...</h2>
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
                    No Sales on last
                </td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="txtDay" runat="server" Width="60px" TabIndex="1" 
                        ToolTip="" MaxLength="10" Text="60" Style="text-align :center ">
                    </asp:TextBox> 
                    Days &nbsp;
                </td>
                <td></td>
                <td>Order By</td>
                <td>:</td>
                <td>
                    <asp:DropDownList ID="ddlOrderBy" 
                        CssClass="form-control" Width="300px"
                        runat="server" AutoPostBack="True" 
                        onselectedindexchanged="ddlOrderBy_SelectedIndexChanged">
                        <asp:ListItem>Model</asp:ListItem>
                        <asp:ListItem>Date</asp:ListItem>
                        <asp:ListItem>Stock</asp:ListItem>
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
                <td> Category</td>
                <td>:</td>
                <td>
                    <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control" Width="300px">
                    </asp:DropDownList>
                </td>
                <td></td>
                <td></td>
                <td></td>
                <td>
                    <asp:RadioButtonList ID="RadioButtonList1" runat="server" 
                        RepeatDirection="Horizontal" Width="300px" AutoPostBack="True" 
                        onselectedindexchanged="RadioButtonList1_SelectedIndexChanged">
                        <asp:ListItem Selected="True" Value="ASC">Ascending</asp:ListItem>
                        <asp:ListItem Value="DESC">Descending</asp:ListItem>
                    </asp:RadioButtonList>
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
                    <asp:Button ID="btnSearch" CssClass="btn btn-primary" runat="server" 
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
        <asp:GridView ID="GridView1" runat="server"                        
            AutoGenerateColumns="False"
            DataKeyNames="Model"
            GridLines="None"
            AllowPaging="false"
            CssClass="mGrid"                        
            PagerStyle-CssClass="pgr"                                               
            AlternatingRowStyle-CssClass="alt"  
            AllowSorting="true" OnSorting="OnSorting"
            Width="100%" ShowFooter="true"                        
                >
            <SelectedRowStyle BackColor="BurlyWood"/>
            <FooterStyle Font-Bold="true" BackColor="#D0ECE7" ForeColor="black" />
            <Columns>
                <asp:TemplateField HeaderText="SL #" HeaderStyle-Width="5%">
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                </asp:TemplateField>       
                <asp:BoundField DataField="GroupName" HeaderText="Category" />                                               
                <asp:BoundField DataField="Model" HeaderText="Model" />
                <asp:BoundField DataField="maxDate" HeaderText="Last Sales Date" />                          
                <asp:BoundField DataField="bQty" HeaderText="Stock" />

                <asp:TemplateField HeaderText="" HeaderStyle-Width="5%">
                    <ItemTemplate>
		                <asp:LinkButton ID="lnkStock" Text="Details"  OnClick="lnkStock_Click" 
                            runat="server" >   
                        </asp:LinkButton>
	                </ItemTemplate>
                </asp:TemplateField>
                                

            </Columns>
        </asp:GridView>
        
    </div>
        
    <div>&nbsp;</div>  
    
    <div>
        <asp:Button ID="btnExport" runat="server" Text="Export To Excel" 
            OnClick = "ExportToExcel" Height="28px" Visible="False" Width="161px" 
            BackColor="#000099" Font-Size="X-Small" ForeColor="Aqua" /> 
    </div>
    <div>&nbsp;</div>
    
    <!-- ------------------------------------------------------------------ -->
    <asp:Label ID="lblresult" runat="server"/>
        <asp:Button ID="btnShowPopup" runat="server" style="display:none" />
        
        <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="btnShowPopup" PopupControlID="pnlpopup"
            CancelControlID="btnCancel" BackgroundCssClass="modalBackground">
        </asp:ModalPopupExtender>

        <asp:Panel ID="pnlpopup" runat="server" BackColor="White" Width="600px" style="display:none; max-height: 400px; overflow: auto;">
            <table width="100%" style="border:3px Solid #669999; width:100%; height:100%" 
                cellpadding="0" cellspacing="0">
                <tr style="background-color:#669999">
                    <td colspan="3" style=" height:20px; color:White; font-weight:bold; font-size:larger" align="center">Stock Report</td>
                </tr>

                <tr>
                    <td>&nbsp;</td>
                    <td></td>
                    <td></td>
                </tr>

                <tr>
                    <td align="left" style=" width:2%">&nbsp;</td>
                    <td align="left" style="color: #FF0000">
                        Model: &nbsp;
                        <asp:Label ID="lblModel" runat="server" Text="Model" Font-Size="Medium" 
                            ForeColor="#CC0000"></asp:Label>
                    </td>
                    <td align="left" style=" width:2%">
                        &nbsp;
                    </td>
                </tr>

                <tr>
                    <td align="left" style=" width:2%">&nbsp;</td>
                    <td align="left" style=" width:96%">

                        <asp:Panel ID="Panel1" runat="server">
                        
                        <asp:GridView ID="GridView2" runat="server"                        
                            AutoGenerateColumns="False"
                            DataKeyNames="Model"
                            GridLines="None"
                            AllowPaging="false"
                            CssClass="mGrid"                        
                            PagerStyle-CssClass="pgr"                                               
                            AlternatingRowStyle-CssClass="alt"                              
                            Width="100%" ShowFooter="true"  >                      
                                
                            <SelectedRowStyle BackColor="BurlyWood"/>
                            <FooterStyle Font-Bold="true" BackColor="#D0ECE7" ForeColor="black" />
                            <Columns>
                                <asp:TemplateField HeaderText="SL #" HeaderStyle-Width="5%">
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                </asp:TemplateField>                       
                                <asp:BoundField DataField="eName" HeaderText="CTP/Area Name" />                                                      
                                <asp:BoundField DataField="bQty" HeaderText="Stock" />
                                
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
                        <asp:Button ID="btnCancel" CssClass="btn btn-info" runat="server" Text="Ok" />
                    </td>
                    <td></td>
                </tr>
                
                <tr>
                    <td>&nbsp;</td>
                    <td></td>
                    <td></td>
                </tr>

            </table>

            
        </asp:Panel>

    <!-- ------------------------------------------------------------------ -->
    
      
    
</asp:Content>
