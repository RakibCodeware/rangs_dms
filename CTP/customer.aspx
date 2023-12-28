<%@ Page Language="C#" AutoEventWireup="true" CodeFile="customer.aspx.cs" Inherits="CTP_customer" 
    MasterPageFile="Admin.master"%>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">

</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    
    
    <h1 class="col-sm-12 bg-primary" 
        style="padding:5px; font-family: Tahoma; background-color: #000000;" 
        align="center"> 
        <asp:Label ID="lblText" runat="server" Text="Customer Information for Spin & Win"></asp:Label>
    </h1>
    <div style="width:100%; text-align:center; display:none">
        <asp:Label ID="lblmsg" runat="server" Text=""></asp:Label>
        <asp:Label ID="lblProdID" runat="server" Text="0"></asp:Label>
        <asp:Label ID="lblModel" runat="server" Text="0"></asp:Label>
    </div>
       
    <p>
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>                
    </p>
    
    
    <!-- Customer Name -->
    <div class="form-group">
    <label for="lblBrand" class="col-sm-offset-1 col-sm-3 control-label" style="font-size:12px;">Customer Name</label>
    <div class="col-sm-6">
        <asp:TextBox ID="txtName" class="form-control" placeholder="Please enter your name" runat="server"></asp:TextBox>        
    </div>
    </div>  

    <!-- Customer Contact -->
    <div class="form-group">
    <label for="lblBrand" class="col-sm-offset-1 col-sm-3 control-label" style="font-size:12px;">Contact Number</label>
    <div class="col-sm-6">
        <asp:TextBox ID="txtContact" class="form-control" 
            placeholder="Please share your contact number" 
            onkeypress="return numeric_only(event)"
            runat="server" MaxLength="11"></asp:TextBox>        
    </div>
    </div> 

    <!-- Customer DOB -->
    <div class="form-group">
        <label for="lblDOI" class="col-sm-offset-1 col-sm-3 control-label" style="font-size:12px;">Date of Birth</label>
        <div class="col-sm-2">
            <asp:DropDownList ID="ddlDay" CssClass="form-control" runat="server"> </asp:DropDownList>                    
        </div>
        <div class="col-sm-2">
            <asp:DropDownList ID="ddlMonth" CssClass="form-control" runat="server"> </asp:DropDownList>                    
        </div>
        <div class="col-sm-2">
            <asp:DropDownList ID="ddlYear" CssClass="form-control" runat="server"> </asp:DropDownList>                    
        </div>
    </div>

    <!-- Product Model -->
    <div class="form-group">
        <label for="lblRetTime" class="col-sm-offset-1 col-sm-3 control-label" 
            style="font-size:12px;">Product Model</label>
        <div class="col-md-6">
            <asp:DropDownList ID="ddlProduct" CssClass="form-control" runat="server" 
                AutoPostBack="True" onselectedindexchanged="ddlProduct_SelectedIndexChanged">
                <asp:ListItem></asp:ListItem>
            </asp:DropDownList>
        </div>                              
    </div>

    <!-- Selected Model -->
    <div class="form-group">
    <label for="lblBrand" class="col-sm-offset-1 col-sm-3 control-label" style="font-size:12px;">Selected Model</label>
    <div class="col-sm-6">
        <asp:TextBox ID="txtModel" class="form-control" placeholder="Please Select Product Model" runat="server" ReadOnly="True"></asp:TextBox>        
    </div>
    </div> 

    <!-- Product Description -->
    <div class="form-group">
    <label for="lblBrand" class="col-sm-offset-1 col-sm-3 control-label" style="font-size:12px;">Description</label>
    <div class="col-sm-6">
        <asp:TextBox ID="txtDesc" class="form-control" placeholder="Please Select Product Model" runat="server" ReadOnly="True"></asp:TextBox>        
    </div>
    </div>


    <!-- Challan Number -->
    <div class="form-group">
    <label for="lblBrand" class="col-sm-offset-1 col-sm-3 control-label" style="font-size:12px;">Promo Code </label>
    <div class="col-sm-1">
        <asp:Button ID="btnGenerate" runat="server" Text="Generate" CssClass="btn btn-danger"
            onclick="btnGenerate_Click" />
    </div>
    <div class="col-sm-5">
        <asp:TextBox ID="txtChNo" class="form-control" placeholder="Please Generate Promo Code" runat="server"></asp:TextBox>        
    </div>
    </div> 

    <!-- SAVE -->
    <div class="form-group" >
    <div class="col-sm-offset-4 col-sm-6" style="margin-bottom:7px;"><br />
        <asp:Button ID="btnSave" CssClass="btn btn-primary" runat="server" Text="   Submit   " 
        OnClick="SaveData" data-toggle="tooltip" title="Click here for Save this data ..." 
        width="100%"/>                        
    </div>                                            
    </div>
       
    <div class="row" align="center">
        <div class="col-lg-12">
            <a class="btn btn-primary" target="_blank" href="http://202.84.32.124/spinwheel.live/Default.aspx">Go To the Spin and Win Game Link</a>
        </div>
    </div>

</asp:Content>