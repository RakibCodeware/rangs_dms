<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Search_Prod_history.aspx.cs" 
    Inherits="Admin_Search_Prod_history" MasterPageFile="Admin.master" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    
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
        style="padding:5px; background-color: #006666; color: #FFFFFF;">Search : Product History <small style="color:#fff"> ( Fields marked with an asterisk <span class="glyphicon glyphicon-asterisk"></span> are required )</small></h4>
    
    <div class="text-center text-danger lead">
        <asp:Label ID="lblmsg" runat="server" Text=""></asp:Label>
    
    </div>


    <div class="form-group">
        <label for="lblAdd1" class="col-sm-offset-1 col-sm-3 control-label" style="font-size:12px;">Product Serial #</label>
        <div class="col-sm-5">
            <asp:TextBox ID="txtSL" CssClass="form-control" 
                placeholder="Enter Product Serial #" runat="server" AutoPostBack="True" 
                MaxLength="40" ontextchanged="txtSL_TextChanged" ></asp:TextBox>
        </div>
    </div> 

    <!--
    <div class="form-group">
        <label for="lblAdd1" class="col-sm-offset-1 col-sm-3 control-label" style="font-size:12px;"></label>
        <div class="col-sm-5">
            <asp:Button ID="btnRegister" CssClass="btn btn-primary" runat="server" 
                Text="Submit" Height="34px" Width="113px" BackColor="#006666" onclick="btnRegister_Click" Enabled="true" />
        </div>
    </div>
    -->


    <div class="form-group">
        <label for="lblAdd1" class="col-sm-offset-1 col-sm-3 control-label" style="font-size:12px;">Product Model</label>
        <div class="col-sm-5">
            <asp:DropDownList ID="ddlModel" CssClass="form-control" runat="server">                
            </asp:DropDownList>
        </div>
    </div> 

    <div class="form-group">
        <label for="lblAdd1" class="col-sm-offset-1 col-sm-3 control-label" style="font-size:12px;"></label>
        <div class="col-sm-5">
            <asp:Button ID="btnSerch" CssClass="btn btn-primary" runat="server" 
                Text="Search" Height="34px" Width="100%" BackColor="#006666" 
                Enabled="true" onclick="btnSerch_Click" />
        </div>
    </div>

    <div class="form-group">
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


</asp:Content>