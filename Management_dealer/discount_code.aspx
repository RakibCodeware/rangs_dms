<%@ Page Language="C#" AutoEventWireup="true" CodeFile="discount_code.aspx.cs" 
    Inherits="discount_code" MasterPageFile="Admin.master" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    
    <script>
        function allowOnlyNumber(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }
    </script>

    <style>
        #customers {
          font-family: "Trebuchet MS", Arial, Helvetica, sans-serif;
          border-collapse: collapse;
          width: 100%;
        }

        #customers td, #customers th {
          border: 1px solid #ddd;
          padding: 8px;
        }

        #customers tr:nth-child(even){background-color: #f2f2f2;}

        #customers tr:hover {background-color: #ddd;}

        #customers th {
          padding-top: 12px;
          padding-bottom: 12px;
          text-align: left;
          background-color: #CB2C26;
          color: white;
        }
    </style>


    <div>
        <table>
            <tr>
                <td>
                    <asp:TextBox ID="txtProdID" Enabled="false" CssClass="form-control" 
                        placeholder="Auto" runat="server" Visible="False" Height="16px" 
                        Width="46px" >
                    </asp:TextBox>
                </td>
                <td>
                    <asp:TextBox ID="txtCTPID" Enabled="false" CssClass="form-control" 
                        placeholder="Auto" runat="server" Height="16px" Width="63px" 
                        Visible="False" ></asp:TextBox>
                </td>
                <td><asp:ScriptManager ID="ScriptManager1" runat="server">
                    </asp:ScriptManager></td>
                <td></td>
                <td></td>
            </tr>
        </table>
        
    </div>

    <br />
    <h4 class="col-sm-12 bg-primary" 
        style="padding:5px; background-color: #006666; color: #FFFFFF;">Generate Discount Code <small style="color:#fff"> ( Fields marked with an asterisk <span class="glyphicon glyphicon-asterisk"></span> are required )</small></h4>
    
    <div class="text-center text-danger lead">
        <asp:Label ID="lblmsg" runat="server" Text=""></asp:Label>
    
    </div>


    <div class="form-group">
        <label for="lblAdd1" class="col-sm-offset-1 col-sm-3 control-label" style="font-size:12px;">Discount Amount</label>
        <div class="col-sm-5">
            <asp:TextBox ID="txtDisAmnt" CssClass="form-control" 
                placeholder="Enter Discount Amount" runat="server" 
                onkeypress="return allowOnlyNumber(event);"
                MaxLength="6">
            </asp:TextBox>
        </div>
    </div> 

       

    <div class="form-group">
        <label for="lblAdd1" class="col-sm-offset-1 col-sm-3 control-label" style="font-size:12px;"></label>
        <div class="col-sm-5">
            <asp:Button ID="btnGenerate" CssClass="btn btn-primary" runat="server" 
                Text="Generate" Height="34px" Width="100%" BackColor="#006666" 
                onclick="btnGenerate_Click" TabIndex="1" />

            <asp:Label ID="lblDisID" runat="server" Visible="False"></asp:Label>
            <asp:Label ID="lblNewVal" runat="server" Text="" Visible="False"></asp:Label>

        </div>
    </div>

    <div class="form-group">
        <label for="lblAdd1" class="col-sm-offset-1 col-sm-3 control-label" style="font-size:12px;">Discount Code</label>
        <div class="col-sm-5">
            <asp:TextBox ID="txtDisCode" style="text-align:center" CssClass="form-control" 
                runat="server" ReadOnly="True" BackColor="Black" ForeColor="Yellow" 
                TabIndex="2"></asp:TextBox>
        </div>
    </div> 

    <div class="form-group">
        <label for="lblAdd1" class="col-sm-offset-1 col-sm-3 control-label" style="font-size:12px;">Reference By</label>
        <div class="col-sm-5">
            <asp:TextBox ID="txtRefBy" CssClass="form-control" runat="server" TabIndex="3"></asp:TextBox>
        </div>
    </div> 

    <div class="form-group">
        <label for="lblAdd1" class="col-sm-offset-1 col-sm-3 control-label" style="font-size:12px;">CTP Name</label>
        <div class="col-sm-5">
            <asp:DropDownList ID="ddlCTP" CssClass="form-control" runat="server" 
                TabIndex="4" AutoPostBack="True">                
            </asp:DropDownList>
            <asp:Label ID="lblEID" runat="server" Text="0" Visible="False"></asp:Label>
        </div>
    </div> 

    <div class="form-group">
        <label for="lblAdd1" class="col-sm-offset-1 col-sm-3 control-label" style="font-size:12px;"></label>
        <div class="col-sm-5">
            <asp:Button ID="btnSubmit" CssClass="btn btn-primary" runat="server" 
                Text="Submit" Height="34px" Width="100%" BackColor="#CC0000" 
                Enabled="true" onclick="btnSubmit_Click" TabIndex="5" />
        </div>
    </div>

    <!-- -------------------- Start TABLE ------------------------------- -->
    <div class="form-group" style="display:none">
        <table id="customers">
            <tr>
                <th width="6%">#</th>
                <th>Date</th> 
                <th>Challan#</th> 
                <th>Transaction Type</th> 
                <th>From</th> 
                <th>To</th>                                    
            </tr>

            <asp:Literal ID="Literal1" runat="server"></asp:Literal>


        </table>
    </div>
    <!-- -------------------- END TABLE --------------------------------- -->

</asp:Content>