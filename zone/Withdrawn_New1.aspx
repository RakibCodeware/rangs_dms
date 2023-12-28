<%@ Page Language="C#" MasterPageFile="Admin.master" AutoEventWireup="true" 
CodeFile="Withdrawn_New1.aspx.cs" Inherits="Forms_Withdrawn_New1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %> 
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    
    <h2 class="col-sm-12 bg-primary" style="padding:5px"> Customer Withdrawn (New Entry) ...</h2>
    <p>
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager> 
    </p>
        
    
    <!-- /.row -->
    <div class="row">
        <div class="col-lg-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    Customer Withdrawn Entry 
                </div>
                <div class="panel-body">
                    <div class="row">
                        <div class="col-lg-4">
                            <form role="form">
                                <div class="form-group">
                                    <label>Withdrawn #</label>                                    
                                    <asp:TextBox ID="txtWNo" class="form-control" runat="server"></asp:TextBox>
                                    <p class="help-block">Automatic Generate this number.</p>
                                </div>
                                <div class="form-group">
                                    <label>Product Model</label>
                                    <input class="form-control" placeholder="Enter text">
                                </div>

                                <div class="form-group">
                                    <label>Quantity</label>
                                    <input class="form-control" placeholder="Enter text">
                                </div>

                                <div class="form-group">
                                    <label>Remarks</label>
                                    <input class="form-control" placeholder="Enter text">
                                </div>
                                        
                                <button type="submit" class="btn btn-default">Add</button>
                                
                            </form>
                        </div>
                        <!-- /.col-lg-6 (nested) -->

                        <div class="col-lg-2">
                            
                        </div>

                        <div class="col-lg-4">                            
                            <form role="form">
                                <div class="form-group">
                                    <label>Withdrawn Date</label>                                                                        
                                    <asp:TextBox ID="txtDate" class="form-control" 
                                        runat="server" placeholder="MM/dd/yyyy">
                                    </asp:TextBox>
                                    
                                    <asp:ImageButton ID="imgPopup" ImageUrl="~/images/cal.gif" ImageAlign="Bottom"
                                        runat="server" TabIndex="4" Height="16px" Width="17px" 
                                        class="input-group-btn"                                        
                                        />
                                    <cc1:calendarextender ID="Calendar1" PopupButtonID="imgPopup" runat="server" TargetControlID="txtDate"
                                        Format="MM/dd/yyyy">
                                    </cc1:calendarextender>                                      
                                                                                                         
                                </div>

                                <div class="form-group">
                                    <label>Product Description</label>
                                    <input class="form-control" placeholder="Enter text">
                                </div>

                                <div class="form-group">
                                    <label>Product SL#</label>
                                    <input class="form-control" placeholder="Enter text">
                                </div>

                            </form>
                        </div>
                        <!-- /.col-lg-6 (nested) -->
                     

                    </div>
                    <!-- /.row (nested) -->
                </div>
                <!-- /.panel-body -->
            </div>
            <!-- /.panel -->
        </div>
        <!-- /.col-lg-12 -->
    </div>
    <!-- /.row -->
       

    

    <!-- jQuery -->
    <script src="../vendor/jquery/jquery.min.js"></script>

    <!-- Bootstrap Core JavaScript -->
    <script src="../vendor/bootstrap/js/bootstrap.min.js"></script>

    <!-- Metis Menu Plugin JavaScript -->
    <script src="../vendor/metisMenu/metisMenu.min.js"></script>

    <!-- Custom Theme JavaScript -->
    <script src="../dist/js/sb-admin-2.js"></script>



</asp:Content>

