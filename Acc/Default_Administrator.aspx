<%@ Page Language="C#" MasterPageFile="Admin.master" AutoEventWireup="true" 
CodeFile="Default_Administrator.aspx.cs" Inherits="Default_Administrator" %>


<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
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

                <a href="#">
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
                <a href="#">
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
                <a href="#">
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
                            <div>Outstanding!</div>
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


    <!-- BRAND / CATEGORY WISE SALES -->
    <div class="row" style="display:none">
        <div class="col-lg-6">
            <div class="panel panel-info">
                <div class="panel-heading">
                    Brand Wise Sales on &nbsp;
                    <asp:Label ID="lblDateBrand" runat="server" Text="[Date]"></asp:Label>
                </div>
                <div class="panel-body" style="height: 300px; overflow: scroll">
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
                        Width="100%"
                        >
                        <SelectedRowStyle BackColor="BurlyWood"/>
                        <Columns>
                            <asp:TemplateField HeaderText="SL #">
                                 <ItemTemplate>
                                       <%# Container.DataItemIndex + 1 %>
                                 </ItemTemplate>
                            </asp:TemplateField>       
                            <asp:BoundField DataField="PCategory" HeaderText="Brand Name" />                                               
                            <asp:BoundField DataField="tQty" HeaderText="Sales Qty" />
                            <asp:BoundField DataField="tAmnt" HeaderText="Sales Amnt" 
                                DataFormatString="{0:###,###}"/>                            
                            
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

        <div class="col-lg-6">
            <div class="panel panel-info">
                <div class="panel-heading">
                    Category Wise Sales on &nbsp;
                    <asp:Label ID="lblDateCatWise" runat="server" Text="[Date]"></asp:Label>
                </div>
                <div class="panel-body" style="height: 300px; overflow: scroll">
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
                            Width="100%"
                            >
                            <SelectedRowStyle BackColor="BurlyWood"/>
                            <Columns>
                                <asp:TemplateField HeaderText="SL #">
                                     <ItemTemplate>
                                           <%# Container.DataItemIndex + 1 %>
                                     </ItemTemplate>
                                </asp:TemplateField>       
                                <asp:BoundField DataField="GroupName" HeaderText="Category Name" />                                               
                                <asp:BoundField DataField="tQty" HeaderText="Sales Qty" />
                                <asp:BoundField DataField="tAmnt" HeaderText="Sales Amnt" 
                                    DataFormatString="{0:###,###}"/>                            
                            
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

        

    </div>


    <!-- --------------------------------------------------------------------------------- -->
    <!-- MODEL WISE SALES -->
    <div class="row" style="display:none">
        <div class="col-lg-6">
            <div class="panel panel-info">
                <div class="panel-heading">
                    Model Wise Sales on &nbsp;
                    <asp:Label ID="lblDateModelWise" runat="server" Text="[Date]"></asp:Label>
                </div>
                <div class="panel-body" style="height: 400px; overflow: scroll">
                    <p>
                        <asp:GridView ID="gvModelWiseSales" runat="server"                        
                        AutoGenerateColumns="False"
                        DataKeyNames="Model"
                        GridLines="None"
                        AllowPaging="false"
                        CssClass="mGrid"                        
                        PagerStyle-CssClass="pgr"                                               
                        AlternatingRowStyle-CssClass="alt"                         
                        OnRowDataBound="gvModelWiseSales_RowDataBound"                        
                        Width="100%"
                        >
                        <SelectedRowStyle BackColor="BurlyWood"/>
                        <Columns>
                            <asp:TemplateField HeaderText="SL #">
                                 <ItemTemplate>
                                       <%# Container.DataItemIndex + 1 %>
                                 </ItemTemplate>
                            </asp:TemplateField>       
                            <asp:BoundField DataField="Model" HeaderText="Model" />                                               
                            <asp:BoundField DataField="tQty" HeaderText="Sales Qty" />
                            <asp:BoundField DataField="tAmnt" HeaderText="Sales Amnt" 
                                DataFormatString="{0:###,###}"/>                            
                            
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
            <div class="panel panel-info">
                <div class="panel-heading">
                    New Product List &nbsp;
                    <asp:Label ID="lblDateStock" runat="server" Text=""></asp:Label>
                </div>
                <div class="panel-body" style="height: 400px; overflow: scroll">
                    <p>
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
                

    </div>


    <!-- --------------------------------------------------------------------------------- -->
    <div class="row">
        <div class="col-lg-6">
            <div class="panel panel-info">
                <div class="panel-heading">
                    Running Campaign
                </div>
                <div class="panel-body" style="height: 100px; overflow: scroll">
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
        <div class="col-lg-6">
            <div class="panel panel-info">
                <div class="panel-heading">
                    Latest Events
                </div>
                <div class="panel-body" style="height: 100px; overflow: scroll">
                    <p></p>
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



</asp:Content>

