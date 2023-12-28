<%@ Page Language="C#" MasterPageFile="~/CTP_Admin.master" AutoEventWireup="true" 
CodeFile="Sales_New.aspx.cs" Inherits="Forms_Sales_New" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %> 
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    
    <link type="text/css" href="../css/ui-lightness/jquery-ui-1.8.19.custom.css" rel="stylesheet" />
    <script type="text/javascript" src="../js/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="../js/jquery-ui-1.8.19.custom.min.js"></script>

    <script type="text/javascript">
        $(function () {
            $("#txtDate").datepicker();
            $("#txtToDate").datepicker();
        });        
    </script>
        
    <style type="text/css">
        .style19
        {
            width: 631px;
        }
        .grid
        {}
        .style20
        {
            width: 28px;
        }
        .style22
        {
            width: 129px;
            height: 29px;
        }
        .style24
        {
            width: 149px;
        }
        .style25
        {
            width: 146px;
        }
        .style26
        {
            width: 41px;
        }
        .style28
        {
            width: 129px;
        }
        .style29
        {
            width: 136px;
        }
        .style30
        {
            width: 47px;
        }
        .style33
        {
            width: 48px;
        }
        .style34
        {
            width: 50px;
        }
                
        .buttonStyle
        {
            background-image:-webkit-linear-gradient(top,#ffffff 0%,#3e9ad2 100%);
            background-image:-moz-linear-gradient(top,#ffffff 0%,#3e9ad2 100%);
            background-image:-o-linear-gradient(top,#ffffff 0%,#3e9ad2 100%);
            background-image:-ms-linear-gradient(top,#ffffff 0%,#3e9ad2 100%);
            background-image:linear-gradient(top,#ffffff 0%,#3e9ad2 100%);
        }
        
        .style35
    {
        width: 31px;
    }
    .style36
    {
        width: 74px;
    }
        
    </style>
        
    <script language="JavaScript" src="../js/datetimepicker.js" type="text/javascript"> </script>
            
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        Customer Sales (New Entry)
    </h2>
    <p>
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    </p>
    <div align="center">
        
        <table style="border: 1px groove #008000; width: 831px;">
            <tr>
                <td colspan="9" align="center"
                    style="background-image:url(../Images/header.jpg); height:40px; font-family: Arial; font-size: large; text-decoration: blink;">                        
                    Sales Entry Form</td>
            </tr>
            <tr>
                <td class="style26"></td><td class="style25"></td><td class="style20"></td>
                <td class="style29"></td>
            </tr>
            
            <!-- Challan No. -->
            <tr>
                <td class="style26"></td>
                <td style="text-align: Left; font-family: Arial; font-weight: 700; color: #009900;" 
                    class="style25">Invoice #</td>
                <td class="style20"> : </td>
                <td style="text-align: left" class="style29">
                <asp:TextBox ID="txtCHNo" runat="server" Width="200px" 
                    style="font-weight: 700" ToolTip="Please Enter Challan Number" MaxLength="15" 
                        BackColor="#66FFFF" BorderStyle="None" Font-Bold="True" Height="21px" 
                        ReadOnly="True"></asp:TextBox>
                </td>
                
                
                <!-- Challan Date -->
                <td style="text-align: left;">
                     <asp:RequiredFieldValidator 
                     id="RequiredFieldValidator1" runat="server" 
                     ErrorMessage="( * )"                    
                     ControlToValidate="txtCHNo" ForeColor="Red"></asp:RequiredFieldValidator>
                </td>
                <td style="text-align: left; font-family: Arial; font-size: small; " 
                    class="style24">Challan Date</td>
                <td class="style20"> : </td>
                <td style="text-align: left; font-family: Arial, Helvetica, sans-serif; font-size: x-small; color: #FF0000;" 
                    class="style22">
                    <asp:TextBox ID="txtDate" runat="server" Width="97px" TabIndex="1" 
                            ToolTip="Please Enter Challan Date" MaxLength="10">                                                 
                    </asp:TextBox> 
                
                    &nbsp; 
                    <asp:ImageButton ID="imgPopup" ImageUrl="~/images/cal.gif" ImageAlign="Bottom"
                        runat="server" TabIndex="4" />
                    <cc1:CalendarExtender ID="Calendar1" PopupButtonID="imgPopup" runat="server" TargetControlID="txtDate"
                        Format="MM/dd/yyyy">
                    </cc1:CalendarExtender>      
                                                                        
                    <br />
                    Date Format : (MM/dd/yyyy)
                                              
                </td>

                <td style="font-family: Arial; font-size: xx-small; color: #FF0000" 
                    class="style20"></td>
                
            </tr>
            
            <!-- SEARCH BUTTON -->
            <tr>
                <td class="style26"></td>
                <td class="style25">
                    <asp:TextBox ID="txtMRSR" runat="server" Enabled="False" Font-Size="Smaller" 
                        Width="46px" Visible="False"></asp:TextBox>
                </td>
                <td class="style34"></td>
                <td class="style29">
                   
                </td>
                <td class="style30"></td>
                <td class="style24"></td>
                <td class="style34"></td>
                <td style="text-align: left" class="style28">
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" 
                            ErrorMessage="Not Valid." 
                            ControlToValidate="txtDate"
                            ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                            style="font-size: xx-small; font-family: Arial, Helvetica, sans-serif" />
                                                   
                </td>                
            </tr>

           
           
            <!-- Customer Name -->
            <tr>
                <td class="style26"></td>
                <td style="text-align: Left; font-family: Arial; " 
                    class="style25">Customer Name </td>
                <td class="style20"> : </td>        
                <td style="text-align: left" class="style29">
                    <asp:TextBox ID="txtCustName" runat="server" Width="200px" 
                    style="font-weight: 700" TabIndex="2" ToolTip="Please Enter Customer Name" 
                        MaxLength="50"></asp:TextBox></td>
                
                <td align="left">
                    &nbsp;</td>

                <!-- Customer Contact # -->                
                <td style="text-align: Left; font-family: Arial; " 
                    class="style24">Customer Contact # </td>
                <td class="style20"> : </td>
                <td style="text-align: left" class="style28">
                    <asp:TextBox ID="txtCustContact" runat="server" AutoPostBack = "true"
                        Width="200px" 
                        style="font-weight: 700" MaxLength="11" TabIndex="3" 
                        ToolTip="Please Enter Customer Contact Number"                        
                        onkeypress="return numeric_only(event)"
                        OnTextChanged="txtCustContact_TextChanged"></asp:TextBox></td>
                    
                <td class="style20">
                    &nbsp;</td>
            </tr>

            
             <!-- Customer Address -->
            <tr>
                <td class="style26"></td>
                <td style="text-align: Left; font-family: Arial; " 
                    class="style25">Customer Address </td>
                <td class="style20"> : </td>
                <td style="text-align: left" class="style29">
                <asp:TextBox ID="txtCustAdd" runat="server" Width="200px" 
                    style="font-weight: 700" TabIndex="4" ToolTip="Enter Customer Address" 
                        MaxLength="50"></asp:TextBox></td>
            
                <!-- Customer Email Address -->
                <td class="style30"></td>
                <td style="text-align: Left; font-family: Arial; " 
                    class="style24">Customer Email </td>
                <td class="style20"> : </td>
                <td style="text-align: left" class="style28">
                    <asp:TextBox ID="txtEmail"  runat="server"                        
                        Width="200px" 
                        style="font-weight: 700" TabIndex="5" 
                        ToolTip="Enter Customer Email" MaxLength="50"></asp:TextBox></td>
                
                <td class="style20"></td>
            </tr>
            
            <tr>
                <td class="style26"></td>
                <td class="style25"></td>
                <td></td>
                <td class="style29"></td>
                <td class="style30"></td>
                <td class="style24"></td>
                <td></td>                
                <td class="style28">                    
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" 
                        ControlToValidate="txtEmail"
                        ErrorMessage="Invalid Email Address" 
                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                        Font-Size="Smaller" ForeColor="Red"
                        SetFocusOnError="True">
                    </asp:RegularExpressionValidator>
                </td>
                <td class="style20"></td>
            </tr>
                       
            <!-- Line Break -->            
            <tr>
                <td colspan="9" align="center"
                    style="background-image:url(../Images/header.jpg); height:0.5px; font-family: Arial;">                        
                    </td>
            </tr>
            <tr>
                <td class="style26"></td>
                <td class="style25"></td><td style="text-align: left" class="style20">
              
                &nbsp;</td>
                <td class="style29"></td>
            </tr>

            <!-- ---------------------------------- -->
            <!-- ---------------------------------- -->

            
            <!-- Product Model -->
            <tr>
                <td class="style26"></td>
                <td style="text-align: Left; font-family: Arial; font-weight: 700; color: #000000;" 
                    class="style25">Product Model</td>
                <td class="style20"> : </td>              
                <td style="text-align: left" class="style29" >                    
                    <asp:DropDownList ID="ddlContinents"
                        runat="server" AutoPostBack = "true"
                        OnSelectedIndexChanged="ddlContinents_SelectedIndexChanged" 
                        BackColor="#F6F1DB"
                        Height="26px" Width="202px" TabIndex="6"
                        ToolTip="Please Select Product Model ...">
                        <asp:ListItem Text = "--Select Model--" Value = ""></asp:ListItem>
                    </asp:DropDownList>
                    </td>
                <td class="style30" align="left">
                     &nbsp;</td>

                <!-- Product Description -->                
                <td style="text-align: Left; font-family: Arial; " 
                    class="style24">Product Description </td>
                <td class="style20"> : </td>
                <td style="text-align: left" class="style28">
                <asp:TextBox ID="txtProdDesc" runat="server" Width="200px" 
                    style="font-weight: 500" Enabled="False"></asp:TextBox></td>
                <td class="style20"></td>
            </tr>

             
            
             <!-- Unit Price -->
            <tr>
                <td class="style26"></td>
                <td style="text-align: Left; font-family: Arial; color: ##003300; font-size: small;" 
                    class="style25">MRP (Tk.)</td>
                <td class="style20"> : </td>
                <td style="text-align: left" class="style29">
                <asp:TextBox ID="txtUP" runat="server" Width="200px" 
                    style="font-weight: 700" Enabled="False"></asp:TextBox></td>
                <td class="style30">
                    <asp:TextBox ID="txtProdID" runat="server" Width="6px" 
                        style="font-weight: 700" Visible="False"></asp:TextBox> 
                </td>

                <!-- Campaign Price -->                
                <td style="text-align: Left; font-family: Arial; color: ##003300; font-size: small;" 
                    class="style24">Campaign Price (Tk.) </td>
                <td class="style20"> : </td>
                <td style="text-align: left" class="style28">
                <asp:TextBox ID="txtCP" runat="server" Width="200px" 
                    style="font-weight: 700" Enabled="False"></asp:TextBox></td>
                <td class="style20"></td>
            </tr>

            
             <!-- Quantity -->
            <tr>
                <td class="style26"></td>
                <td style="text-align: Left; font-family: Arial; font-weight: 700; color: #008000; font-size: small;" 
                    class="style25">Quantity</td>
                <td class="style20"> : </td>
                <td style="text-align: left" class="style29">
                    <asp:TextBox ID="txtQty" runat="server" AutoPostBack="True" Width="200px" 
                        style="font-weight: 700" OnTextChanged="txtQty_TextChanged"
                        onkeypress="return numeric_only(event)"
                        ToolTip="Please Enter Product Quantity" TabIndex="7" MaxLength="5"></asp:TextBox>                    
                </td>
                <td class="style30" align="left">
                    &nbsp;</td>


                <!-- Total Price -->                
                <td style="text-align: Left; font-family: Arial; font-weight: 700; color: #00CC00; font-size: small;" 
                    class="style24">Total Price (Tk.)</td>
                <td class="style20"> : </td>
                <td style="text-align: left" class="style28">
                <asp:TextBox ID="txtTotalAmnt" runat="server" Width="200px" 
                    style="font-weight: 700" Enabled="False"></asp:TextBox></td>
                <td class="style20"></td>
            </tr>

                        
            <!-- Discount Code -->
            <tr>
                <td class="style26"></td>
                <td style="text-align: Left; font-family: Arial; " 
                    class="style25">Discount Code</td>
                <td class="style20"> : </td>
                <td style="text-align: left" class="style29">
                <asp:TextBox ID="txtDisCode" runat="server" Width="200px" 
                    style="font-weight: 700" Enabled="False"></asp:TextBox></td>
                
                <!-- Discount Amount -->
                <td class="style30"></td>
                <td style="text-align: Left; font-family: Arial; color: #FF0000;" 
                    class="style24">Discount Amount</td>
                <td class="style20"> : </td>
                <td style="text-align: left" class="style28">
                <asp:TextBox ID="txtDisAmnt" runat="server" AutoPostBack="True" Width="200px" 
                    style="font-weight: 700" OnTextChanged="txtDisAmnt_TextChanged"
                    onkeypress="return numeric_only(event)"
                    ToolTip="Discount Amount" TabIndex="8" MaxLength="6"></asp:TextBox></td>
                <td class="style20"></td>
            </tr>

            
             <!-- Discount Reference -->
            <tr>
                <td class="style26"></td>
                <td style="text-align: Left; font-family: Arial; " 
                    class="style25">Ref. for Discount</td>
                <td class="style20"> : </td>
                <td style="text-align: left" class="style29">
                    <asp:TextBox ID="txtDisRef" runat="server" Width="200px" 
                      style="font-weight: 700"
                      ToolTip="Reference for Discount" TabIndex="9"></asp:TextBox>
                </td>

                <!-- Withdrawn/Adjustment Amount -->
                <td class="style30"></td>
                <td style="text-align: Left; font-family: Arial; " 
                    class="style24">With/Adjust Amount</td>
                <td class="style20"> : </td>
                <td style="text-align: left" class="style28">
                    <asp:TextBox ID="txtWithAdj" runat="server" AutoPostBack="True" Width="200px" 
                        style="font-weight: 700" 
                        OnTextChanged="txtWithAdj_TextChanged"
                        onkeypress="return numeric_only(event)"
                        ToolTip="Withdrawn/Adjustment Amount..." TabIndex="10" MaxLength="6"></asp:TextBox>
                </td>
                <td class="style20"></td>
            </tr>

            
            <!-- Net Amount -->
            <tr>
                <td class="style26"></td>
                <td class="style25"></td>
                <td class="style49"></td>
                <td class="style29"></td>
                <td class="style30"></td>
                <td style="text-align: Left; font-family: Arial; font-weight: 700; color: #008000;" 
                    class="style24">Net Amount (Tk.)</td>
                <td class="style52"> : </td>
                <td style="text-align: left" class="style28">
                <asp:TextBox ID="txtNet" runat="server" Width="200px" 
                    style="font-weight: 700" Enabled="False"></asp:TextBox></td>
            </tr>

             <!-- Product SL# -->
            <tr>
                <td class="style26"></td>
                <td style="text-align: Left; font-family: Arial;" 
                    class="style25">Product SL#</td>
                <td class="style20"> : </td>
                <td style="text-align: left" class="style29">
                    <asp:TextBox ID="txtSL" runat="server" Width="200px" 
                        style="font-weight: 700"
                        ToolTip="Please Enter Product Serial Number" TabIndex="11" MaxLength="20"></asp:TextBox>
                </td>

                <!-- Remarks -->
                <td class="style30"></td>
                <td style="text-align: Left; font-family: Arial; " 
                    class="style24">Remarks</td>
                <td class="style20"> : </td>
                <td style="text-align: left" class="style28">
                <asp:TextBox ID="txtRemarks" runat="server" Width="200px" 
                    style="font-weight: 700" TabIndex="12" MaxLength="40"></asp:TextBox></td>
                <td class="style20"></td>
            </tr>

                       
            <!-- Add to Data Grid -->
            <tr>
                <td class="style26"></td>
                <td class="style25"></td>
                <td class="style20"></td>
                <td style="text-align: left" class="style29">
                    <asp:Button ID="btnAdd" runat="server" Text="Add Product in List" OnClick="btnAdd_Click" 
                        Width="116px" Height="25px"
                        Font-Size="X-Small" 
                        ToolTip="Click here for add product in list ..." TabIndex="13" 
                        BackColor="#000099" ForeColor="Aqua"/>
                </td>                
            </tr>
            
            <tr>
                <td class="style26"></td>
                <td class="style25"></td>
                <td class="style20"></td>
                <td class="style29"></td>
            </tr>
            
        </table>
        
        <div>
        <asp:Label ID="lblError" ForeColor="red" runat="server" Text=""></asp:Label>
        </div>
        
        <table style="border: 1px groove #008000;" width="831px">
            <tr>
                    <td class="style19">
                        <!-- Data Grid -->                
                        <asp:GridView ID="gvUsers" runat="server"
                        AutoGenerateColumns="false"                        
                        CssClass="grid"
                        DataKeyNames="ProductID"
                        EmptyDataText = "No product in list !!! Please select model and add in list."
                        EmptyDataRowStyle-CssClass ="gvEmpty"
                        Onrowdeleting="gvUsers_RowDelating"
                        ShowFooter="true"
                        OnRowDataBound="gvUsers_RowDataBound"                        
                        GridLines="none" Width="800px">
                        <FooterStyle Font-Bold="true" BackColor="#61A6F8" ForeColor="black" />
                        <Columns>
                            <asp:BoundField HeaderText="Product ID" DataField="ProductID"/>                        
                            <asp:BoundField HeaderText="Product Model" DataField="Model" />                       
                            <asp:BoundField HeaderText="MRP (Tk.)" DataField="MRP" />                       
                            <asp:BoundField HeaderText="Campaign Price" DataField="CampaignPrice" />
                            <asp:BoundField HeaderText="Qty" DataField="Qty" />
                            <asp:BoundField HeaderText="Total Price" DataField="TotalPrice" />
                            <asp:BoundField HeaderText="Dis Amnt" DataField="DisAmnt" />
                            <asp:BoundField HeaderText="Dis Code" DataField="DisCode"/>
                            <asp:BoundField HeaderText="Dis Ref" DataField="DisRef"/>
                            <asp:BoundField HeaderText="With/Adj Amnt" DataField="WithAdjAmnt" />
                            <asp:BoundField HeaderText="NetAmnt" DataField="NetAmnt"/>  
                                                                              
                            <asp:BoundField HeaderText="Product SL" DataField="ProductSL" />
                            <asp:BoundField HeaderText="Remarks" DataField="Remarks" ItemStyle-Width="5px" />
                            <asp:TemplateField HeaderText="Action">
                                <ItemTemplate>               
                                    <asp:ImageButton ID="ibtnDelete" runat="server"
                                        ToolTip="Delete"                                        
                                        ImageUrl="~/Images/btn-delete.jpg" 
                                        CommandName="Delete"   
                                        OnClientClick="return confirm('Are you sure you want to delete this record?');"                                                                                                          
                                         />
                                </ItemTemplate>
                                
                            </asp:TemplateField>

                        </Columns>
                        <EmptyDataRowStyle CssClass="gvEmpty" />
                    </asp:GridView>
                </td>
            </tr> 
            <tr>
                <td>
                    <asp:Label ID="lblNetAmnt" runat="server" Text="0" Visible="False"></asp:Label>
                    <asp:TextBox ID="txtNetAmnt" runat="server" Visible="False" Width="77px"></asp:TextBox>
                </td>
            </tr>
            
        </table>
        
        <div>&nbsp;</div>

        <table style="border: 1px groove #008000" width="831px">
            <tr>
                <td class="style35"></td>
                <td class="style28" align="left"></td>
                <td style="text-align: center" class="style36">&nbsp;</td>
                <td class="style28" align="left">&nbsp;</td>                
                <td class="style28"></td>
                <td class="style29" align="left"></td>
                <td style="text-align: center" class="style20"></td>
                <td class="style33" align="left"></td>
                <td class="style28"></td>
            </tr>
            <tr>
                <td class="style35"></td>
                <td class="style28" align="left" style="color: #000080; font-weight: bold;">Pay Amount</td>
                <td style="text-align: center" class="style36">:</td>
                <td class="style28" align="left">
                    <asp:TextBox ID="txtPay" runat="server" Width="169px" 
                        style="font-weight: 700" 
                        onkeypress="return numeric_only(event)"
                        ToolTip="Please Enter paid Amount..." 
                        MaxLength="10" ontextchanged="txtPay_TextChanged" TabIndex="14">
                    </asp:TextBox>
                </td>
                
                <td class="style28"></td>
                <td class="style29" align="left" style="color: #FF0000">Due Amount</td>
                <td style="text-align: center" class="style20">:</td>
                <td class="style33" align="left">
                    <asp:TextBox ID="txtDue" runat="server" Width="134px" 
                        style="font-weight: 700"
                        onkeypress="return numeric_only(event)"
                        ToolTip="Due Amount" 
                        MaxLength="10" TabIndex="4" ReadOnly="True" BackColor="#FFCCFF"></asp:TextBox>
                </td>
                <td class="style28"></td>
            </tr>

            <tr>
                <td class="style35"></td>
                <td class="style28" align="left" style="color: #008080">Pay Type</td>
                <td style="text-align: center" class="style36">:</td>
                <td class="style28" align="left">
                    <asp:DropDownList ID="ddlPayType" runat="server" BackColor="#F6F1DB"
                        Height="24px" Width="169px" 
                        TabIndex="15" ToolTip="Please select discount by..." 
                        onselectedindexchanged="ddlPayType_SelectedIndexChanged" 
                        AutoPostBack="True">
                        <asp:ListItem Value = "">CASH</asp:ListItem>
                        <asp:ListItem>CHEQUE</asp:ListItem>
                        <asp:ListItem>AMEX</asp:ListItem>
                        <asp:ListItem>VISA CARD</asp:ListItem>
                        <asp:ListItem>MASTER CARD</asp:ListItem>
                        <asp:ListItem>DD</asp:ListItem>
                        <asp:ListItem>TT</asp:ListItem>
                        <asp:ListItem>OTHERS</asp:ListItem>
                    </asp:DropDownList>
                </td>
                
                <td class="style28"></td>
                <td class="style29" align="left" style="color: #FF0000">&nbsp;</td>
                <td style="text-align: center" class="style20">&nbsp;</td>
                <td class="style33" align="left">
                    &nbsp;</td>
                <td class="style28"></td>
            </tr>

            <tr>
                <td class="style35"></td>
                <td class="style28" align="left"                     
                    
                    style="font-family: Tahoma; color: #000080; font-size: small; font-weight: normal">
                    <asp:Label ID="lblNo" runat="server" Text="Cheque #" Visible="False"></asp:Label>
                    </td>
                <td style="text-align: center" class="style36">&nbsp;</td>
                <td class="style28" align="left">
                    <asp:TextBox ID="txtChequeNo" runat="server" Width="169px" 
                        style="font-weight: 700" ToolTip="Cheque/DD/TT/Card Number..." 
                        MaxLength="15" Visible="False" TabIndex="16"></asp:TextBox>
                </td>
                
                <td class="style28"></td>
                <td class="style29" align="left">&nbsp;</td>
                <td style="text-align: center" class="style20">&nbsp;</td>
                <td class="style33" align="left">
                    &nbsp;</td>
                <td class="style28">
                    &nbsp;</td>
            </tr>

            <tr>
                <td class="style35"></td>
                <td class="style28" align="left"                     
                    
                    style="font-family: Tahoma; color: #000080; font-size: small; font-weight: normal">
                    <asp:Label ID="lblBankName" runat="server" Text="Bank Name" Visible="False"></asp:Label>
                    </td>
                <td style="text-align: center" class="style36">&nbsp;</td>
                <td class="style28" align="left">
                    <asp:TextBox ID="txtBankName" runat="server" Width="169px" 
                        style="font-weight: 700" ToolTip="Pls enter bank name..." MaxLength="15" 
                        Visible="False" TabIndex="17"></asp:TextBox>
                </td>
                
                <td class="style28"></td>
                <td class="style29" align="left"></td>
                <td style="text-align: center" class="style20"></td>
                <td class="style33" align="left">
                    &nbsp;</td>
                <td class="style28"></td>
            </tr>

            <tr>
                <td class="style35"></td>
                <td class="style28" align="left"                     
                    
                    style="font-family: Tahoma; color: #000080; font-size: small; font-weight: normal">
                    <asp:Label ID="lblIssueDate" runat="server" Text="Issue Date" Visible="False"></asp:Label>
                    </td>
                <td style="text-align: center" class="style36">&nbsp;</td>
                <td class="style28" align="left">
                    <asp:TextBox ID="txtIssueDate" runat="server" Width="169px" 
                        style="font-weight: 700" ToolTip="Cheque/DD/TT Issue date..." 
                        MaxLength="15" Visible="False" TabIndex="18"></asp:TextBox>
                </td>
                
                <td class="style28"></td>
                <td class="style29" align="left">&nbsp;</td>
                <td style="text-align: center" class="style20">&nbsp;</td>
                <td class="style33" align="left">
                    &nbsp;</td>
                <td class="style28"></td>
            </tr>

            <tr>
                <td class="style35"></td>
                <td class="style28" align="left"
                    
                    style="font-family: Tahoma; color: #000080; font-size: small; font-weight: normal">
                    <asp:Label ID="lblSecurityCode" runat="server" Text="Security Code" Visible="False"></asp:Label>
                    </td>
                <td style="text-align: center" class="style36">&nbsp;</td>
                <td class="style28" align="left">
                    <asp:TextBox ID="txtSecurityCode" runat="server" Width="169px" 
                        style="font-weight: 700" ToolTip="Cheque/DD/TT Issue date..." 
                        MaxLength="15" Visible="False" TabIndex="18"></asp:TextBox>
                </td>                
                <td class="style28"></td>
                <td class="style29" align="left"></td>
                <td style="text-align: center" class="style20"></td>
                <td class="style33" align="left"></td>
                <td class="style28"></td>
            </tr>
            <tr>
                <td class="style35"></td>
                <td class="style28" align="left"></td>
                <td style="text-align: center" class="style36">&nbsp;</td>
                <td class="style28" align="left">&nbsp;</td>                
                <td class="style28"></td>
                <td class="style29" align="left"></td>
                <td style="text-align: center" class="style20"></td>
                <td class="style33" align="left"></td>
                <td class="style28"></td>
            </tr>
        </table>

        <div>&nbsp;</div>

        <table style="border: 1px hidden #008000;" width="831px">                
            <tr>
                <td class="style28"></td>
            </tr>
            <tr>
                <td align="center">                    
                    <asp:Button ID="btnSave" CssClass="buttonStyle" runat="server" Height="25px" Text="Save" 
                        width="68px" onclick="btnSave_Click" TabIndex="14" 
                        Font-Size="Small"
                        ToolTip="Click here for save data..."/>
                        &nbsp;
                    <asp:Button ID="btnPrint" CssClass="buttonStyle" runat="server" Height="25px" Text="Bill Print"
                        Font-Size="Small"
                        Width="78px" TabIndex="15" onclick="btnPrint_Click"/>
                        &nbsp;
                    <asp:Button ID="btnCancel" CssClass="buttonStyle" runat="server" Height="25px" Text="New Bill" 
                        Font-Size="Small"
                        Width="68px" TabIndex="16" onclick="btnCancel_Click" />                    
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
            </tr>
        </table>

        <p></p>

   </div>

</asp:Content>

