<%@ Page Title="" Language="C#" MasterPageFile="~/DealerReports/DealerReports.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="DealerReports_Default" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" Runat="Server">
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
                            <div>Outstanding!</div>
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


    <!-- BRAND / CATEGORY WISE SALES -->
   


    <!-- --------------------------------------------------------------------------------- -->
    <!-- MODEL WISE SALES -->
  


    <!-- --------------------------------------------------------------------------------- -->
    
      

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

