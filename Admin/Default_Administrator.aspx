<%@ Page Language="C#" MasterPageFile="Admin.master" AutoEventWireup="true" 
CodeFile="Default_Administrator.aspx.cs" Inherits="Default_Administrator"%>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <script language="JavaScript" src="../js/datetimepicker.js" type="text/javascript"> </script>
    
    <script type="text/javascript">
        $(function () {
            $("#txtDate").datepicker();
            $("#txtFrom").datepicker();
            $("#txtToDate").datepicker();
            $("#txtFrom1").datepicker();
            $("#txtToDate1").datepicker();
            $("#txtFrom2").datepicker();
            $("#txtToDate2").datepicker();
            $("#txtPIDate").datepicker();
        });   
             
    </script>
        

</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

    <h2 class="col-sm-12 bg-primary" style="padding:5px; font-family: Tahoma;" align="center"> 
        <asp:Label ID="lblText" runat="server" Text="Label"></asp:Label>
        </h2>
        
    <p>
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>                
    </p>
       

    <div>&nbsp;</div>

    <!-- --------------------------------------------------------------------------------- -->
    <!-- /.row -->
    <!-- SALES -->
    <div class="row">
        <div class="col-lg-3 col-md-6">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <div class="row">
                        <div class="col-xs-3">
                            <i class="fa fa-comments fa-2x"></i>
                        </div>
                        <div class="col-xs-9 text-right">
                            <div class="huge">
                                <asp:Label ID="lblYSales" runat="server" Text="0"></asp:Label>
                            </div>
                            <div>Yearly Sales!</div>
                        </div>
                    </div>
                </div>

                <a href="MonthlySales.aspx">
                    <div class="panel-footer">
                        <span class="pull-left">View Details</span>
                        <span class="pull-right"><i class="fa fa-arrow-circle-right"></i></span>
                        <div class="clearfix"></div>
                    </div>
                </a>
            </div>
        </div>


        <div class="col-lg-3 col-md-6">
            <div class="panel panel-green">
                <div class="panel-heading">
                    <div class="row">
                        <div class="col-xs-3">
                            <i class="fa fa-tasks fa-2x"></i>
                        </div>
                        <div class="col-xs-9 text-right">
                            <div class="huge">
                                <asp:Label ID="lblMSales" runat="server" Text="0"></asp:Label>
                            </div>
                            <div>Monthly Sales!</div>
                        </div>
                    </div>
                </div>
                <a href="DateWiseSales.aspx">
                    <div class="panel-footer">
                        <span class="pull-left">View Details</span>
                        <span class="pull-right"><i class="fa fa-arrow-circle-right"></i></span>
                        <div class="clearfix"></div>
                    </div>
                </a>
            </div>
        </div>
        <div class="col-lg-3 col-md-6">
            <div class="panel panel-yellow">
                <div class="panel-heading">
                    <div class="row">
                        <div class="col-xs-3">
                            <i class="fa fa-shopping-cart fa-2x"></i>
                        </div>
                        <div class="col-xs-9 text-right">
                            <div class="huge">
                                <asp:Label ID="lblDSales" runat="server" Text="0"></asp:Label>
                            </div>
                            <div>Today Sales!</div>
                        </div>
                    </div>
                </div>
                <a href="ToDaySales.aspx">
                    <div class="panel-footer">
                        <span class="pull-left">View Details</span>
                        <span class="pull-right"><i class="fa fa-arrow-circle-right"></i></span>
                        <div class="clearfix"></div>
                    </div>
                </a>
            </div>
        </div>
        <div class="col-lg-3 col-md-6">
            <div class="panel panel-red">
                <div class="panel-heading">
                    <div class="row">
                        <div class="col-xs-3">
                            <i class="fa fa-support fa-4x"></i>
                        </div>
                        <div class="col-xs-9 text-right">
                            <div class="huge">
                                <asp:Label ID="lblTargetY" runat="server" Text="0"></asp:Label>
                            </div>
                            <div>Yearly Target!</div>
                        </div>
                    </div>
                </div>
                <a href="#">
                    <div class="panel-footer">
                        <span class="pull-left">View Details</span>
                        <span class="pull-right"><i class="fa fa-arrow-circle-right"></i></span>
                        <div class="clearfix"></div>
                    </div>
                </a>
            </div>
        </div>
    </div>
    <!-- /.row -->
    <!-- --------------------------------------------------------------------------------- -->


    <!-- --------------------------------------------------------------------------------- -->
    <!-- /.row -->
    <!-- Deposit -->
    <div class="row">
        <div class="col-lg-3 col-md-6">
            <div class="panel panel-info">
                <div class="panel-heading">
                    <div class="row">
                        <div class="col-xs-3">
                            <i class="fa fa-comments fa-2x"></i>
                        </div>
                        <div class="col-xs-9 text-right">
                            <div class="huge">
                                <asp:Label ID="lblYDeposit" runat="server" Text="0"></asp:Label>
                            </div>
                            <div>Yearly Deposit!</div>
                        </div>
                    </div>
                </div>

                <a href="MonthWiseDeposit.aspx">
                    <div class="panel-footer">
                        <span class="pull-left">View Details</span>
                        <span class="pull-right"><i class="fa fa-arrow-circle-right"></i></span>
                        <div class="clearfix"></div>
                    </div>
                </a>
            </div>
        </div>


        <div class="col-lg-3 col-md-6">
            <div class="panel panel-success">
                <div class="panel-heading">
                    <div class="row">
                        <div class="col-xs-3">
                            <i class="fa fa-tasks fa-2x"></i>
                        </div>
                        <div class="col-xs-9 text-right">
                            <div class="huge">
                                <asp:Label ID="lblMDeposit" runat="server" Text="0"></asp:Label>
                            </div>
                            <div>Monthly Deposit!</div>
                        </div>
                    </div>
                </div>
                <a href="DateWiseDeposit.aspx">
                    <div class="panel-footer">
                        <span class="pull-left">View Details</span>
                        <span class="pull-right"><i class="fa fa-arrow-circle-right"></i></span>
                        <div class="clearfix"></div>
                    </div>
                </a>
            </div>
        </div>
        <div class="col-lg-3 col-md-6">
            <div class="panel panel-danger">
                <div class="panel-heading">
                    <div class="row">
                        <div class="col-xs-3">
                            <i class="fa fa-shopping-cart fa-2x"></i>
                        </div>
                        <div class="col-xs-9 text-right">
                            <div class="huge">
                                <asp:Label ID="lblDDeposit" runat="server" Text="0"></asp:Label>
                            </div>
                            <div>Today Deposit!</div>
                        </div>
                    </div>
                </div>
                <a href="ToDayDeposit.aspx">
                    <div class="panel-footer">
                        <span class="pull-left">View Details</span>
                        <span class="pull-right"><i class="fa fa-arrow-circle-right"></i></span>
                        <div class="clearfix"></div>
                    </div>
                </a>
            </div>
        </div>
        <div class="col-lg-3 col-md-6">
            <div class="panel panel-red">
                <div class="panel-heading">
                    <div class="row">
                        <div class="col-xs-3">
                            <i class="fa fa-support fa-4x"></i>
                        </div>
                        <div class="col-xs-9 text-right">
                            <div class="huge">
                                <asp:Label ID="lblOutstanding" runat="server" Text="0"></asp:Label>
                            </div>
                            <div>Dealer Outstanding!</div>
                        </div>
                    </div>
                </div>
                <a href="search_zonal_statement.aspx">
                    <div class="panel-footer">
                        <span class="pull-left">View Details</span>
                        <span class="pull-right"><i class="fa fa-arrow-circle-right"></i></span>
                        <div class="clearfix"></div>
                    </div>
                </a>
            </div>
        </div>
    </div>
    <!-- /.row -->
    <!-- --------------------------------------------------------------------------------- -->


    <!-- CATEGORY / MODEL WISE SALES -->
    <div class="row">
        <div class="col-lg-6">
            <div class="panel panel-primary">
                <div class="panel-heading">                    
                    Category Wise Sales on &nbsp;
                    <asp:Label ID="lblDateCatWise" runat="server" Text="[Date]"></asp:Label>
                </div>
                
                <div class="panel-body" style="height: 400px; overflow: scroll">                    
                    
                        <table width ="100%">
                            <tr>                                
                                <td>
                                    From Date
                                </td>
                                <td>:</td>
                                <td>
                                    <asp:TextBox ID="txtFrom" runat="server" Width="90px" TabIndex="1" 
                                        ToolTip="Date Format (MM/dd/yyyy)" MaxLength="10">
                                    </asp:TextBox> 
                                    <cc1:calendarextender ID="Calendar1" PopupButtonID="imgPopup" runat="server" TargetControlID="txtFrom"
                                        Format="MM/dd/yyyy">
                                    </cc1:calendarextender>
                                                    
                                    <asp:ImageButton ID="imgPopup" ImageUrl="~/Images/cal.gif" ImageAlign="AbsMiddle"
                                        runat="server" TabIndex="1" />
                                </td>
                                <td></td>
                                <td>To Date</td>
                                <td>:</td>
                                <td>
                                    <asp:TextBox ID="txtToDate" runat="server" Width="90px" TabIndex="2" 
                                            ToolTip="Date Format (MM/dd/yyyy)" MaxLength="10"></asp:TextBox> 
                                    <cc1:calendarextender ID="Calendarextender1" PopupButtonID="imgPopup1" runat="server" TargetControlID="txtToDate"
                                        Format="MM/dd/yyyy">
                                    </cc1:calendarextender>
                
                                    <asp:ImageButton ID="imgPopup1" ImageUrl="~/Images/cal.gif" ImageAlign="AbsMiddle"
                                        runat="server" TabIndex="1" />
                                </td>
                                <td align="left">
                                    <asp:ImageButton ID="btnCatSearch" runat="server" Height="18px" 
                                        ImageUrl="~/Images/search.png" Width="18px" 
                                        data-toggle="tooltip" title="Click here for Search data ..." onclick="btnCatSearch_Click"
                                    /> 
                                </td>
                            </tr>
                        </table>
                        
                        

                    <p>
                        <asp:GridView ID="GridView2" runat="server"                        
                            AutoGenerateColumns="False"
                            DataKeyNames="GroupName"
                            GridLines="None"
                            AllowPaging="false"
                            CssClass="mGrid"                        
                            PagerStyle-CssClass="pgr"                                               
                            AlternatingRowStyle-CssClass="alt"                         
                            OnRowDataBound="GridView2_RowDataBound"                        
                            Width="100%" ShowFooter="true"
                            >
                            <SelectedRowStyle BackColor="BurlyWood"/>
                            <FooterStyle Font-Bold="true" BackColor="#D0ECE7" ForeColor="black" />
                            <Columns>
                                <asp:TemplateField HeaderText="SL #">
                                     <ItemTemplate>
                                           <%# Container.DataItemIndex + 1 %>
                                     </ItemTemplate>
                                </asp:TemplateField>       
                                <asp:BoundField DataField="GroupName" HeaderText="Category Name" />                                               
                                <asp:BoundField DataField="tQty" HeaderText="Sales Qty" />
                                <asp:BoundField DataField="tAmnt" HeaderText="Sales Amnt" />                            
                            
                            </Columns>
                        </asp:GridView>
                     </p>
                </div>
                <div class="panel-footer">
                    <!--
                    <a href="#">Details</a>
                    -->                    
                </div>
            </div>
            <!-- /.col-lg-6 -->
        </div>
        <div class="col-lg-6">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    Model Wise Sales on &nbsp;
                    <asp:Label ID="lblDateModelWise" runat="server" Text="[Date]"></asp:Label>
                </div>
                <div class="panel-body" style="height: 400px; overflow: scroll">

                        <table width ="100%">
                            <tr>                                
                                <td>
                                    From Date
                                </td>
                                <td>:</td>
                                <td>
                                    <asp:TextBox ID="txtFrom1" runat="server" Width="90px" TabIndex="1" 
                                        ToolTip="Date Format (MM/dd/yyyy)" MaxLength="10">
                                    </asp:TextBox> 
                                    <cc1:calendarextender ID="Calendarextender2" PopupButtonID="imgPopupM1" runat="server" TargetControlID="txtFrom1"
                                        Format="MM/dd/yyyy">
                                    </cc1:calendarextender>
                                                    
                                    <asp:ImageButton ID="imgPopupM1" ImageUrl="~/Images/cal.gif" ImageAlign="AbsMiddle"
                                        runat="server" TabIndex="1" />
                                </td>
                                <td></td>
                                <td>To Date</td>
                                <td>:</td>
                                <td>
                                    <asp:TextBox ID="txtToDate1" runat="server" Width="90px" TabIndex="2" 
                                            ToolTip="Date Format (MM/dd/yyyy)" MaxLength="10"></asp:TextBox> 
                                    <cc1:calendarextender ID="Calendarextender3" PopupButtonID="imgPopupM2" runat="server" TargetControlID="txtToDate1"
                                        Format="MM/dd/yyyy">
                                    </cc1:calendarextender>
                
                                    <asp:ImageButton ID="imgPopupM2" ImageUrl="~/Images/cal.gif" ImageAlign="AbsMiddle"
                                        runat="server" TabIndex="1" />
                                </td>
                                <td align="left">
                                    <asp:ImageButton ID="btnModelSearch" runat="server" Height="18px" 
                                        ImageUrl="~/Images/search.png" Width="18px" 
                                        data-toggle="tooltip" title="Click here for Search data ..." onclick="btnModelSearch_Click"
                                    /> 
                                </td>
                            </tr>
                        </table>

                    <div>
                        <asp:GridView ID="gvModelWiseSales" runat="server"                        
                        AutoGenerateColumns="False"
                        DataKeyNames="Model"
                        GridLines="None"
                        AllowPaging="false"
                        CssClass="mGrid"                        
                        PagerStyle-CssClass="pgr"                                               
                        AlternatingRowStyle-CssClass="alt"                         
                        OnRowDataBound="gvModelWiseSales_RowDataBound"                        
                        Width="100%" ShowFooter="true"
                        >
                        <SelectedRowStyle BackColor="BurlyWood"/>
                        <FooterStyle Font-Bold="true" BackColor="#D0ECE7" ForeColor="black" />
                        <Columns>
                            <asp:TemplateField HeaderText="SL #">
                                 <ItemTemplate>
                                       <%# Container.DataItemIndex + 1 %>
                                 </ItemTemplate>
                            </asp:TemplateField>       
                            <asp:BoundField DataField="Model" HeaderText="Model" />                                               
                            <asp:BoundField DataField="tQty" HeaderText="Sales Qty" />
                            <asp:BoundField DataField="tAmnt" HeaderText="Sales Amnt" />                            
                            
                        </Columns>
                    </asp:GridView>
                    </div>
                </div>
                <div class="panel-footer">
                    <!--
                    <a href="#">Details</a>
                    -->
                </div>
            </div>
            <!-- /.col-lg-4 -->
        </div>

        

    </div>


    <!-- --------------------------------------------------------------------------------- -->
    <!-- BRAND WISE SALES/ NEW PRODUCT LIST -->
    <div class="row">
        <div class="col-lg-6">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    Brand Wise Sales on &nbsp;
                    <asp:Label ID="lblDateBrand" runat="server" Text="[Date]"></asp:Label>
                </div>
                <div class="panel-body" style="height: 300px; overflow: scroll">
                    
                    <table width ="100%">
                            <tr>                                
                                <td>
                                    From Date
                                </td>
                                <td>:</td>
                                <td>
                                    <asp:TextBox ID="txtFrom2" runat="server" Width="90px" TabIndex="1" 
                                        ToolTip="Date Format (MM/dd/yyyy)" MaxLength="10">
                                    </asp:TextBox> 
                                    <cc1:calendarextender ID="Calendarextender4" PopupButtonID="imgPopupB1" runat="server" TargetControlID="txtFrom2"
                                        Format="MM/dd/yyyy">
                                    </cc1:calendarextender>
                                                    
                                    <asp:ImageButton ID="imgPopupB1" ImageUrl="~/Images/cal.gif" ImageAlign="AbsMiddle"
                                        runat="server" TabIndex="1" />
                                </td>
                                <td></td>
                                <td>To Date</td>
                                <td>:</td>
                                <td>
                                    <asp:TextBox ID="txtToDate2" runat="server" Width="90px" TabIndex="2" 
                                            ToolTip="Date Format (MM/dd/yyyy)" MaxLength="10"></asp:TextBox> 
                                    <cc1:calendarextender ID="Calendarextender5" PopupButtonID="imgPopupB2" runat="server" TargetControlID="txtToDate2"
                                        Format="MM/dd/yyyy">
                                    </cc1:calendarextender>
                
                                    <asp:ImageButton ID="imgPopupB2" ImageUrl="~/Images/cal.gif" ImageAlign="AbsMiddle"
                                        runat="server" TabIndex="1" />
                                </td>
                                <td align="left">
                                    <asp:ImageButton ID="btnBrandSearch" runat="server" Height="18px" 
                                        ImageUrl="~/Images/search.png" Width="18px" 
                                        data-toggle="tooltip" title="Click here for Search data ..." onclick="btnBrandSearch_Click"
                                    /> 
                                </td>
                            </tr>
                        </table>


                    <p>
                        <asp:GridView ID="GridView1" runat="server"                        
                        AutoGenerateColumns="False"
                        DataKeyNames="PCategory"
                        GridLines="None"
                        AllowPaging="false"
                        CssClass="mGrid"                        
                        PagerStyle-CssClass="pgr"                                               
                        AlternatingRowStyle-CssClass="alt"                         
                        OnRowDataBound="GridView1_RowDataBound"                        
                        Width="100%" ShowFooter="true"
                        >
                        <SelectedRowStyle BackColor="BurlyWood"/>
                        <FooterStyle Font-Bold="true" BackColor="#D0ECE7" ForeColor="black" />
                        <Columns>
                            <asp:TemplateField HeaderText="SL #">
                                 <ItemTemplate>
                                       <%# Container.DataItemIndex + 1 %>
                                 </ItemTemplate>
                            </asp:TemplateField>       
                            <asp:BoundField DataField="PCategory" HeaderText="Brand Name" />                                               
                            <asp:BoundField DataField="tQty" HeaderText="Sales Qty" />
                            <asp:BoundField DataField="tAmnt" HeaderText="Sales Amnt" />                            
                            
                        </Columns>
                    </asp:GridView>
                    </p>

                </div>
                <div class="panel-footer">
                    <!--
                    <a href="#">Details</a>
                    -->                   
                </div>
            </div>
            <!-- /.col-lg-4 -->
        </div>
        <!-- --------------------------------------------------------------------------------- -->
        <div class="col-lg-6">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    New Product List &nbsp;
                    <asp:Label ID="lblDateStock" runat="server" Text=""></asp:Label>
                </div>
                <div class="panel-body" style="height: 300px; overflow: scroll">
                    <div>
                        <asp:GridView ID="GridView3" runat="server"                        
                            AutoGenerateColumns="False"
                            DataKeyNames="Code"
                            GridLines="None"
                            AllowPaging="false"
                            CssClass="mGrid"                        
                            PagerStyle-CssClass="pgr"                                               
                            AlternatingRowStyle-CssClass="alt"                         
                            OnRowDataBound="GridView3_RowDataBound"                        
                            Width="100%"
                            >
                            <SelectedRowStyle BackColor="BurlyWood"/>
                            <Columns>
                                <asp:TemplateField HeaderText="SL #">
                                     <ItemTemplate>
                                           <%# Container.DataItemIndex + 1 %>
                                     </ItemTemplate>
                                </asp:TemplateField>  
                                <asp:BoundField DataField="GroupName" HeaderText="Category" />    
                                <asp:BoundField DataField="Code" HeaderText="Code" />  
                                <asp:BoundField DataField="Model" HeaderText="Model" />                                                                              
                                <asp:BoundField DataField="UnitPrice" HeaderText="MRP(Tk.)" 
                                    DataFormatString="{0:###,###}"/>
                                
                            </Columns>
                        </asp:GridView>
                     </div>
                </div>
                <div class="panel-footer">
                    <!--
                    <a href="#">Details</a>
                    -->
                </div>
            </div>
            <!-- /.col-lg-4 -->
        </div>
                

    </div>


    <!-- --------------------------------------------------------------------------------- -->
    <!-- CTP WISE SALES -->
    <div class="row">
        <div class="col-lg-6">
            <div class="panel panel-info">
                <div class="panel-heading">
                    CTP Wise Sales on &nbsp;
                    <asp:Label ID="lblCTP1" runat="server" Text="[Date]"></asp:Label>
                </div>
                <div class="panel-body" style="height: 600px; overflow: scroll">
                    
                    <table width ="100%">
                        <tr>                                
                            <td>
                                From Date
                            </td>
                            <td>:</td>
                            <td>
                                <asp:TextBox ID="txtFromDateCTP1" runat="server" Width="90px" TabIndex="1" 
                                    ToolTip="Date Format (MM/dd/yyyy)" MaxLength="10">
                                </asp:TextBox> 
                                <cc1:calendarextender ID="Calendarextender6" PopupButtonID="imgPopupCTP11" runat="server" TargetControlID="txtFromDateCTP1"
                                    Format="MM/dd/yyyy">
                                </cc1:calendarextender>
                                                    
                                <asp:ImageButton ID="imgPopupCTP11" ImageUrl="~/Images/cal.gif" ImageAlign="AbsMiddle"
                                    runat="server" TabIndex="1" />
                            </td>
                            <td></td>
                            <td>To Date</td>
                            <td>:</td>
                            <td>
                                <asp:TextBox ID="txtToDateCTP1" runat="server" Width="90px" TabIndex="2" 
                                        ToolTip="Date Format (MM/dd/yyyy)" MaxLength="10"></asp:TextBox> 
                                <cc1:calendarextender ID="Calendarextender7" PopupButtonID="imgPopupCTP12" runat="server" TargetControlID="txtToDateCTP1"
                                    Format="MM/dd/yyyy">
                                </cc1:calendarextender>
                
                                <asp:ImageButton ID="imgPopupCTP12" ImageUrl="~/Images/cal.gif" ImageAlign="AbsMiddle"
                                    runat="server" TabIndex="1" />
                            </td>
                            <td align="left">
                                <asp:ImageButton ID="btnCTP1Search" runat="server" Height="18px" 
                                    ImageUrl="~/Images/search.png" Width="18px" 
                                    data-toggle="tooltip" title="Click here for Search data ..." onclick="btnCTP1Search_Click"
                                /> 
                            </td>
                        </tr>
                    </table>

                    <p>
                        <asp:GridView ID="GridView4" runat="server"                        
                        AutoGenerateColumns="False"
                        DataKeyNames="eName"
                        GridLines="None"
                        AllowPaging="false"
                        CssClass="mGrid"                        
                        PagerStyle-CssClass="pgr"                                               
                        AlternatingRowStyle-CssClass="alt"                         
                        OnRowDataBound="GridView4_RowDataBound"                        
                        Width="100%" ShowFooter="true"
                        >
                        <SelectedRowStyle BackColor="BurlyWood"/>
                        <FooterStyle Font-Bold="true" BackColor="#D0ECE7" ForeColor="black" />
                        <Columns>
                            <asp:TemplateField HeaderText="SL #">
                                 <ItemTemplate>
                                       <%# Container.DataItemIndex + 1 %>
                                 </ItemTemplate>
                            </asp:TemplateField>       
                            <asp:BoundField DataField="eName" HeaderText="CTP Name" />                                               
                            <asp:BoundField DataField="tQty" HeaderText="Sales Qty" />
                            <asp:BoundField DataField="tAmnt" HeaderText="Sales Amnt" />                          
                            
                        </Columns>
                    </asp:GridView>
                    </p>

                </div>
                <div class="panel-footer">
                    <a href="#"></a>                    
                </div>
            </div>
            <!-- /.col-lg-4 -->
        </div>
        <!-- --------------------------------------------------------------------------------- -->
        <div class="col-lg-6">
            <div class="panel panel-info">
                <div class="panel-heading">
                    CTP Wise Target & Achivement on &nbsp;
                    <asp:Label ID="lblDateCTPWise" runat="server" Text="[Date]"></asp:Label>
                </div>
                <div class="panel-body" style="height: 600px; overflow: scroll">
                    
                    <table width ="100%">
                        <tr>                                
                            <td>
                                From Date
                            </td>
                            <td>:</td>
                            <td>
                                <asp:TextBox ID="txtFrom3" runat="server" Width="90px" TabIndex="1" 
                                    ToolTip="Date Format (MM/dd/yyyy)" MaxLength="10">
                                </asp:TextBox> 
                                <cc1:calendarextender ID="Calendarextender8" PopupButtonID="imgPopupCTP1" runat="server" TargetControlID="txtFrom3"
                                    Format="MM/dd/yyyy">
                                </cc1:calendarextender>
                                                    
                                <asp:ImageButton ID="imgPopupCTP1" ImageUrl="~/Images/cal.gif" ImageAlign="AbsMiddle"
                                    runat="server" TabIndex="1" />
                            </td>
                            <td></td>
                            <td>To Date</td>
                            <td>:</td>
                            <td>
                                <asp:TextBox ID="txtToDate3" runat="server" Width="90px" TabIndex="2" 
                                        ToolTip="Date Format (MM/dd/yyyy)" MaxLength="10"></asp:TextBox> 
                                <cc1:calendarextender ID="Calendarextender9" PopupButtonID="imgPopupCTP2" runat="server" TargetControlID="txtToDate3"
                                    Format="MM/dd/yyyy">
                                </cc1:calendarextender>
                
                                <asp:ImageButton ID="imgPopupCTP2" ImageUrl="~/Images/cal.gif" ImageAlign="AbsMiddle"
                                    runat="server" TabIndex="1" />
                            </td>
                            <td align="left">
                                <asp:ImageButton ID="btnCTPSearch" runat="server" Height="18px" 
                                    ImageUrl="~/Images/search.png" Width="18px" 
                                    data-toggle="tooltip" title="Click here for Search data ..." onclick="btnCTPSearch_Click"
                                /> 
                            </td>
                        </tr>
                    </table>

                    <p>
                        <asp:GridView ID="GridView5" runat="server"                        
                            AutoGenerateColumns="False"
                            DataKeyNames="eName"
                            GridLines="None"
                            AllowPaging="false"
                            CssClass="mGrid"                        
                            PagerStyle-CssClass="pgr"                                               
                            AlternatingRowStyle-CssClass="alt"                         
                            OnRowDataBound="GridView5_RowDataBound"                        
                            Width="100%" ShowFooter="true"
                            >
                            <SelectedRowStyle BackColor="BurlyWood"/>
                            <FooterStyle Font-Bold="true" BackColor="#D0ECE7" ForeColor="black" />
                            <Columns>
                                <asp:TemplateField HeaderText="SL #">
                                     <ItemTemplate>
                                           <%# Container.DataItemIndex + 1 %>
                                     </ItemTemplate>
                                </asp:TemplateField>       
                                <asp:BoundField DataField="eName" HeaderText="CTP Name" />                                               
                                <asp:BoundField DataField="tQty" HeaderText="Sales Qty" />
                                <asp:BoundField DataField="tAmnt" HeaderText="Sales Amnt" />                            
                            
                            </Columns>
                        </asp:GridView>
                     </p>
                </div>
                <div class="panel-footer">
                    <a href="#"></a>
                </div>
            </div>
            <!-- /.col-lg-4 -->
        </div>
                
        
    </div>
    <!-- --------------------------------------------------------------------------------- -->


    <!-- ********************************************************************************* -->
    <!-- --------------------------------------------------------------------------------- -->
    <!-- Dealer WISE SALES -->
    <div class="row">
        <div class="col-lg-6">
            <div class="panel panel-info">
                <div class="panel-heading">
                    Dealer Wise Sales on &nbsp;
                    <asp:Label ID="lblDealerWiseDate" runat="server" Text="[Date]"></asp:Label>
                </div>
                <div class="panel-body" style="height: 600px; overflow: scroll">
                    
                    <table width ="100%">
                        <tr>                                
                            <td>
                                From Date
                            </td>
                            <td>:</td>
                            <td>
                                <asp:TextBox ID="txtFromDateDealer1" runat="server" Width="90px" TabIndex="1" 
                                    ToolTip="Date Format (MM/dd/yyyy)" MaxLength="10">
                                </asp:TextBox> 
                                <cc1:calendarextender ID="Calendarextender12" PopupButtonID="ImageButton1" runat="server" TargetControlID="txtFromDateDealer1"
                                    Format="MM/dd/yyyy">
                                </cc1:calendarextender>
                                                    
                                <asp:ImageButton ID="ImageButton1" ImageUrl="~/Images/cal.gif" ImageAlign="AbsMiddle"
                                    runat="server" TabIndex="1" />
                            </td>
                            <td></td>
                            <td>To Date</td>
                            <td>:</td>
                            <td>
                                <asp:TextBox ID="txtToDateDealer1" runat="server" Width="90px" TabIndex="2" 
                                        ToolTip="Date Format (MM/dd/yyyy)" MaxLength="10"></asp:TextBox> 
                                <cc1:calendarextender ID="Calendarextender13" PopupButtonID="ImageButton2" runat="server" TargetControlID="txtToDateDealer1"
                                    Format="MM/dd/yyyy">
                                </cc1:calendarextender>
                
                                <asp:ImageButton ID="ImageButton2" ImageUrl="~/Images/cal.gif" ImageAlign="AbsMiddle"
                                    runat="server" TabIndex="1" />
                            </td>
                            <td align="left">
                                <asp:ImageButton ID="btnDealerWise" runat="server" Height="18px" 
                                    ImageUrl="~/Images/search.png" Width="18px" 
                                    data-toggle="tooltip" title="Click here for Search data ..." onclick="btnDealerWise_Click"
                                /> 
                            </td>
                        </tr>
                    </table>

                    <p>
                        <asp:GridView ID="GridView7" runat="server"                        
                        AutoGenerateColumns="False"
                        DataKeyNames="Code"
                        GridLines="None"
                        AllowPaging="false"
                        CssClass="mGrid"                        
                        PagerStyle-CssClass="pgr"                                               
                        AlternatingRowStyle-CssClass="alt"
                        Width="100%" ShowFooter="true"
                        >
                        <SelectedRowStyle BackColor="BurlyWood"/>
                        <FooterStyle Font-Bold="true" BackColor="#D0ECE7" ForeColor="black" />
                        <Columns>
                            <asp:TemplateField HeaderText="SL #">
                                 <ItemTemplate>
                                       <%# Container.DataItemIndex + 1 %>
                                 </ItemTemplate>
                            </asp:TemplateField>       
                            <asp:BoundField DataField="ZoneName" HeaderText="Area" />                                               
                            <asp:BoundField DataField="InSource" HeaderText="Dealer Name" />
                            <asp:BoundField DataField="tAmnt" HeaderText="Sales Amnt" />                          
                            
                        </Columns>
                    </asp:GridView>
                    </p>

                </div>
                <div class="panel-footer">
                    <a href="#"></a>                    
                </div>
            </div>
            <!-- /.col-lg-4 -->
        </div>
        <!-- --------------------------------------------------------------------------------- -->
        <div class="col-lg-6">
            <div class="panel panel-info">
                <div class="panel-heading">
                    Dealer Wise Target & Achivement on &nbsp;
                    <asp:Label ID="Label2" runat="server" Text="[Date]"></asp:Label>
                </div>
                <div class="panel-body" style="height: 600px; overflow: scroll">
                    
                    <table width ="100%">
                        <tr>                                
                            <td>
                                From Date
                            </td>
                            <td>:</td>
                            <td>
                                <asp:TextBox ID="TextBox3" runat="server" Width="90px" TabIndex="1" 
                                    ToolTip="Date Format (MM/dd/yyyy)" MaxLength="10">
                                </asp:TextBox> 
                                <cc1:calendarextender ID="Calendarextender14" PopupButtonID="imgPopupCTP1" runat="server" TargetControlID="txtFrom3"
                                    Format="MM/dd/yyyy">
                                </cc1:calendarextender>
                                                    
                                <asp:ImageButton ID="ImageButton4" ImageUrl="~/Images/cal.gif" ImageAlign="AbsMiddle"
                                    runat="server" TabIndex="1" />
                            </td>
                            <td></td>
                            <td>To Date</td>
                            <td>:</td>
                            <td>
                                <asp:TextBox ID="TextBox4" runat="server" Width="90px" TabIndex="2" 
                                        ToolTip="Date Format (MM/dd/yyyy)" MaxLength="10"></asp:TextBox> 
                                <cc1:calendarextender ID="Calendarextender15" PopupButtonID="imgPopupCTP2" runat="server" TargetControlID="TextBox4"
                                    Format="MM/dd/yyyy">
                                </cc1:calendarextender>
                
                                <asp:ImageButton ID="ImageButton5" ImageUrl="~/Images/cal.gif" ImageAlign="AbsMiddle"
                                    runat="server" TabIndex="1" />
                            </td>
                            <td align="left">
                                <asp:ImageButton ID="ImageButton6" runat="server" Height="18px" 
                                    ImageUrl="~/Images/search.png" Width="18px" 
                                    data-toggle="tooltip" title="Click here for Search data ..."
                                /> 
                            </td>
                        </tr>
                    </table>

                    <p>
                        <asp:GridView ID="GridView8" runat="server"                        
                            AutoGenerateColumns="False"
                            DataKeyNames="eName"
                            GridLines="None"
                            AllowPaging="false"
                            CssClass="mGrid"                        
                            PagerStyle-CssClass="pgr"                                               
                            AlternatingRowStyle-CssClass="alt"                         
                            OnRowDataBound="GridView5_RowDataBound"                        
                            Width="100%" ShowFooter="true"
                            >
                            <SelectedRowStyle BackColor="BurlyWood"/>
                            <FooterStyle Font-Bold="true" BackColor="#D0ECE7" ForeColor="black" />
                            <Columns>
                                <asp:TemplateField HeaderText="SL #">
                                     <ItemTemplate>
                                           <%# Container.DataItemIndex + 1 %>
                                     </ItemTemplate>
                                </asp:TemplateField>       
                                <asp:BoundField DataField="eName" HeaderText="CTP Name" />                                               
                                <asp:BoundField DataField="tQty" HeaderText="Sales Qty" />
                                <asp:BoundField DataField="tAmnt" HeaderText="Sales Amnt" />                            
                            
                            </Columns>
                        </asp:GridView>
                     </p>
                </div>
                <div class="panel-footer">
                    <a href="#"></a>
                </div>
            </div>
            <!-- /.col-lg-4 -->
        </div>
                
        
    </div>
    <!-- --------------------------------------------------------------------------------- -->
    <!-- ********************************************************************************* -->

    <!-- --------------------------------------------------------------------------------- -->
    <div class="row">
        <div class="col-lg-6">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    Sales Summary on &nbsp;
                    <asp:Label ID="lblSummaryDate" runat="server" Text="[Date]"></asp:Label>
                </div>
                <div class="panel-body" style="height: 190px; overflow: scroll">
                    
                    <table width ="100%">
                        <tr>                                
                            <td>
                                From Date
                            </td>
                            <td>:</td>
                            <td>
                                <asp:TextBox ID="txtFromDateSS1" runat="server" Width="90px" TabIndex="1" 
                                    ToolTip="Date Format (MM/dd/yyyy)" MaxLength="10">
                                </asp:TextBox> 
                                <cc1:calendarextender ID="Calendarextender10" PopupButtonID="imgPopupSS1" runat="server" TargetControlID="txtFromDateSS1"
                                    Format="MM/dd/yyyy">
                                </cc1:calendarextender>
                                                    
                                <asp:ImageButton ID="imgPopupSS1" ImageUrl="~/Images/cal.gif" ImageAlign="AbsMiddle"
                                    runat="server" TabIndex="1" />
                            </td>
                            <td></td>
                            <td>To Date</td>
                            <td>:</td>
                            <td>
                                <asp:TextBox ID="txtToDateSS1" runat="server" Width="90px" TabIndex="2" 
                                        ToolTip="Date Format (MM/dd/yyyy)" MaxLength="10"></asp:TextBox> 
                                <cc1:calendarextender ID="Calendarextender11" PopupButtonID="imgPopupSS2" runat="server" TargetControlID="txtToDateSS1"
                                    Format="MM/dd/yyyy">
                                </cc1:calendarextender>
                
                                <asp:ImageButton ID="imgPopupSS2" ImageUrl="~/Images/cal.gif" ImageAlign="AbsMiddle"
                                    runat="server" TabIndex="1" />
                            </td>
                            <td align="left">
                                <asp:ImageButton ID="imgBtnSS" runat="server" Height="18px" 
                                    ImageUrl="~/Images/search.png" Width="18px" 
                                    data-toggle="tooltip" title="Click here for Search data ..." onclick="imgBtnSS_Click" 
                                /> 
                            </td>
                        </tr>
                    </table>

                    <p>
                        <asp:GridView ID="GridView6" runat="server"                        
                        AutoGenerateColumns="False"
                        DataKeyNames="EntityType"
                        GridLines="None"
                        AllowPaging="false"
                        CssClass="mGrid"                        
                        PagerStyle-CssClass="pgr"                                               
                        AlternatingRowStyle-CssClass="alt"                         
                        OnRowDataBound="GridView6_RowDataBound"                        
                        Width="100%" ShowFooter="true"                        
                         >
                        <SelectedRowStyle BackColor="BurlyWood"/>
                        <FooterStyle Font-Bold="true" BackColor="#D0ECE7" ForeColor="black" />
                        <Columns>
                            <asp:TemplateField HeaderText="SL #">
                                 <ItemTemplate>
                                       <%# Container.DataItemIndex + 1 %>
                                 </ItemTemplate>
                            </asp:TemplateField>       
                            <asp:BoundField DataField="EntityType" HeaderText="Channel Type" /> 
                            <asp:BoundField DataField="tAmnt" HeaderText="Sales Amnt" />                          
                            
                        </Columns>
                    </asp:GridView>
                    </p>

                </div>
                <div class="panel-footer">
                    <a href="#"></a>                    
                </div>
            </div>
            <!-- /.col-lg-4 -->
        </div>
        <!-- --------------------------------------- -->
        <div class="col-lg-6">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    Running Campaign
                </div>
                <div class="panel-body" style="height: 170px; overflow: scroll">
                    <p>
                        <b>
                            <asp:Label ID="lblPromotion" runat="server" Text="Promotion"></asp:Label>
                        </b>
                        <br />
                        <asp:Label ID="lblPromotionDate" runat="server" Text="Promotion"></asp:Label>
                    </p>
                </div>
                <div class="panel-footer">
                    <a href="#">Details</a>
                </div>
            </div>
            <!-- /.col-lg-4 -->
        </div>

        

    </div>

   

    <div class="row">
        

    </div>
    <!-- /.row -->




    <!--
    <div class="row">
    <!-- /.col-lg-12 -->
    <!--
    <div class="col-lg-6">
        <div class="panel panel-default">
            <div class="panel-heading">
                Current Activities &nbsp;
                <asp:Label ID="lblMonth2" runat="server" Text="-"></asp:Label>
            </div>
            <!-- /.panel-heading -->

            <!--
            <div class="panel-body">
                <div class="flot-chart">
                    <div class="flot-chart-content" id="flot-pie-chart"></div>
                </div>
            </div>
            <!-- /.panel-body -->
            <!--
        </div>
        <!-- /.panel -->
        <!--
    </div>
    <!-- /.col-lg-6 -->




    <!--
    <div class="col-lg-6">
        <div class="panel panel-default">
            <div class="panel-heading">
                Upcoming Events
            </div>
            <!-- /.panel-heading -->
            <!--
            <div class="panel-body">
                <div class="flot-chart">
                    <div class="flot-chart-content" id="flot-line-chart-multi"></div>
                </div>
            </div> -->
            <!-- /.panel-body -->
            <!--
        </div> -->
        <!-- /.panel -->
    <!--
    </div>

    </div>
    -->

    <script type="text/javascript">
        $(".form_datetime").datetimepicker({ format: 'yyyy-mm-dd hh:ii' });
    </script> 

</asp:Content>

