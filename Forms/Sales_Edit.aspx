<%@ Page Language="C#" MasterPageFile="~/CTP_Admin.master" AutoEventWireup="true" 
CodeFile="Sales_Edit.aspx.cs" Inherits="Forms_Sales_Edit" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
        
    <style type="text/css">
        .grid
        {}
        .style20
        {
            width: 12px;
        }
        .style22
        {
            width: 154px;
            height: 29px;
        }
        .style27
        {
            width: 135px;
        }
        .style29
        {
            width: 158px;
            height: 29px;
        }
        .style34
        {
            width: 140px;
        }
        .style43
        {
            width: 23px;
        }
        .style44
        {
            width: 158px;
        }
        .style45
        {
            width: 154px;
        }
        .style47
        {
            width: 26px;
        }
        .style48
        {
            width: 23px;
            height: 15px;
        }
        .style49
        {
            height: 15px;
        }
        .style51
        {
            width: 158px;
            height: 15px;
        }
        .style52
        {
            width: 12px;
            height: 15px;
        }
        .style53
        {
            width: 154px;
            height: 15px;
        }
        .style54
        {
            width: 53px;
            height: 15px;
        }
        .style55
        {
            width: 53px;
            height: 29px;
        }
        .style57
        {
            width: 53px;
        }
        </style>
        
    <script language="JavaScript" src="../js/datetimepicker.js" type="text/javascript"> </script>
            
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        Customer Sales (Edit Previous Entry)
    </h2>
    <p></p>
    
    <div align="center">
        
        <table style="border: 1px groove #008000; width: 831px;">
            <tr>
                <td colspan="9" align="center"
                    style="background-image:url(../Images/header.jpg); height:40px; font-family: Arial; font-size: large; text-decoration: blink;">                        
                    Sales Edit/Update Form</td>
            </tr>
            <tr>
                <td class="style43"></td><td class="style34"></td><td class="style20"></td>
                <td class="style27"></td>
            </tr>
            
            <!-- Challan No. -->
            <tr>
                <td class="style43"></td>
                <td style="text-align: Left; font-family: Arial; font-weight: 700; color: #009900;" 
                    class="style34">Challan #</td>
                <td class="style20"> : </td>
                <td style="text-align: left" class="style27">
                <asp:TextBox ID="txtCHNo" runat="server" Width="200px" 
                    style="font-weight: 700" ToolTip="Please Enter Challan Number" MaxLength="20"></asp:TextBox>
                    
                    <asp:TextBox ID="txtMRSR" runat="server" Enabled="False" Font-Size="Smaller" 
                        Width="46px" Visible="False"></asp:TextBox>
                    <asp:TextBox ID="txtMRSRID" runat="server" Enabled="False" Font-Size="Smaller" 
                        Width="46px" Visible="False"></asp:TextBox>
                </td>
                
                <!-- Challan Date -->
                <td class="style55"></td>
                <td style="text-align: left; font-family: Arial; font-size: small; " 
                    class="style29">Challan Date</td>
                <td class="style20"> : </td>
                <td style="text-align: left; font-family: Arial, Helvetica, sans-serif; font-size: x-small; color: #FF0000;" 
                    class="style22">
                    <asp:TextBox ID="txtDate" runat="server" Width="97px" TabIndex="1" 
                            ToolTip="Please Enter Challan Date" MaxLength="10">                                                 
                    </asp:TextBox> 
                
                    &nbsp; 
                     <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" 
                                ErrorMessage="Not Valid." 
                                ControlToValidate="txtDate"
                                ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                                style="font-size: xx-small; font-family: Arial, Helvetica, sans-serif" />
                                                   
                    <br />
                    Date Format : (MM/dd/yyyy)
                                              
                </td>

                <td style="font-family: Arial; font-size: xx-small; color: #FF0000" 
                    class="style47"></td>
                
            </tr>
            
            <!-- SEARCH BUTTON -->
            <tr>
                <td class="style43"></td>
                <td class="style34"></td>
                <td class="style20"></td>
                <td style="text-align: left" class="style27">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" 
                        Width="80px" Height="25px"
                        Font-Size="Small" 
                        ToolTip="Click here for Search Challan details ..." TabIndex="1" 
                        BackColor="#CC6600"/>
                </td>                
            </tr>

           
           
            <!-- Customer Contact No. -->
            <tr>
                <td class="style43"></td>
                <td style="text-align: Left; font-family: Arial; " 
                    class="style34">Customer Contact # </td>
                <td class="style20"> : </td>        
                <td style="text-align: left" class="style27">
                    <asp:TextBox ID="txtCustContact" runat="server" AutoPostBack = "true"
                        Width="200px" 
                        style="font-weight: 700" MaxLength="11" TabIndex="2" 
                        ToolTip="Please Enter Customer Contact Number"                        
                        onkeypress="return numeric_only(event)"
                        OnTextChanged="txtCustContact_TextChanged"></asp:TextBox></td>
                
                <td class="style57" align="left">
                    &nbsp;</td>


                <!-- Customer Name -->                
                <td style="text-align: Left; font-family: Arial; " 
                    class="style44">Customer Name </td>
                <td class="style20"> : </td>
                <td style="text-align: left" class="style45">
                <asp:TextBox ID="txtCustName" runat="server" Width="200px" 
                    style="font-weight: 700" TabIndex="3" ToolTip="Please Enter Customer Name" 
                        MaxLength="50"></asp:TextBox></td>
                    
                <td align="left">
                    &nbsp;</td>
            </tr>

            
             <!-- Customer Address -->
            <tr>
                <td class="style43"></td>
                <td style="text-align: Left; font-family: Arial; " 
                    class="style34">Customer Address </td>
                <td class="style20"> : </td>
                <td style="text-align: left" class="style27">
                <asp:TextBox ID="txtCustAdd" runat="server" Width="200px" 
                    style="font-weight: 700" TabIndex="4" ToolTip="Enter Customer Address" 
                        MaxLength="50"></asp:TextBox></td>
            
                <!-- Customer Email Address -->
                <td class="style57"></td>
                <td style="text-align: Left; font-family: Arial; " 
                    class="style44">Customer Email </td>
                <td class="style20"> : </td>
                <td style="text-align: left" class="style45">
                    <asp:TextBox ID="txtEmail"  runat="server"                        
                        Width="200px" 
                        style="font-weight: 700" TabIndex="5" 
                        ToolTip="Enter Customer Email" MaxLength="50"></asp:TextBox></td>
                
                <td class="style47"></td>
            </tr>
            
            <tr>
                <td class="style43"></td>
                <td></td>
                <td></td>
                <td></td>
                <td class="style57"></td>
                <td class="style44"></td>
                <td></td>                
                <td class="style45">                    
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" 
                        ControlToValidate="txtEmail"
                        ErrorMessage="Invalid Email Address" 
                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                        Font-Size="Smaller" ForeColor="Red"
                        SetFocusOnError="True">
                    </asp:RegularExpressionValidator>
                </td>
                <td class="style47"></td>
            </tr>
                       
            <!-- Line Break -->            
            <tr>
                <td colspan="9" align="center"
                    style="background-image:url(../Images/header.jpg); height:0.5px; font-family: Arial;">                        
                    </td>
            </tr>
            <tr>
                <td class="style43"></td>
                <td class="style34"></td><td style="text-align: left" class="style20">
              
                &nbsp;</td>
                <td class="style27"></td>
            </tr>
            <!-- ---------------------------------- -->

            
            <!-- Product Model -->
            <tr>
                <td class="style43"></td>
                <td style="text-align: Left; font-family: Arial; font-weight: 700; color: #000000;" 
                    class="style34">Product Model</td>
                <td class="style20"> : </td>              
                <td style="text-align: left" class="style27" >                    
                    <asp:DropDownList ID="ddlContinents"
                        runat="server" AutoPostBack = "true"
                        OnSelectedIndexChanged="ddlContinents_SelectedIndexChanged" 
                        BackColor="#F6F1DB"
                        Height="26px" Width="202px" TabIndex="6"
                        ToolTip="Please Select Product Model ...">
                        <asp:ListItem Text = "--Select Model--" Value = ""></asp:ListItem>
                    </asp:DropDownList>
                    </td>
                <td class="style57" align="left">
                     &nbsp;</td>

                <!-- Product Description -->                
                <td style="text-align: Left; font-family: Arial; " 
                    class="style44">Product Description </td>
                <td class="style20"> : </td>
                <td style="text-align: left" class="style45">
                <asp:TextBox ID="txtProdDesc" runat="server" Width="200px" 
                    style="font-weight: 500" Enabled="False"></asp:TextBox></td>
                <td class="style47"></td>
            </tr>

             
            
             <!-- Unit Price -->
            <tr>
                <td class="style43"></td>
                <td style="text-align: Left; font-family: Arial; color: ##003300; font-size: small;" 
                    class="style34">MRP (Tk.)</td>
                <td class="style20"> : </td>
                <td style="text-align: left" class="style27">
                <asp:TextBox ID="txtUP" runat="server" Width="200px" 
                    style="font-weight: 700" Enabled="False"></asp:TextBox></td>
                
                <!-- Campaign Price -->
                <td class="style57">
                    <asp:TextBox ID="txtProdID" runat="server" Width="16px" 
                        style="font-weight: 700" Visible="False"></asp:TextBox> 
                </td>
                <td style="text-align: Left; font-family: Arial; color: ##003300; font-size: small;" 
                    class="style44">Campaign Price (Tk.) </td>
                <td class="style20"> : </td>
                <td style="text-align: left" class="style45">
                <asp:TextBox ID="txtCP" runat="server" Width="200px" 
                    style="font-weight: 700" Enabled="False"></asp:TextBox></td>
                <td class="style47"></td>
            </tr>

            
             <!-- Quantity -->
            <tr>
                <td class="style43"></td>
                <td style="text-align: Left; font-family: Arial; font-weight: 700; color: #008000; font-size: small;" 
                    class="style34">Quantity</td>
                <td class="style20"> : </td>
                <td style="text-align: left" class="style27">
                    <asp:TextBox ID="txtQty" runat="server" AutoPostBack="True" Width="200px" 
                        style="font-weight: 700" OnTextChanged="txtQty_TextChanged"
                        onkeypress="return numeric_only(event)"
                        ToolTip="Please Enter Product Quantity" TabIndex="7" MaxLength="5"></asp:TextBox>                    
                </td>
                <td class="style57" align="left">
                    &nbsp;</td>
                

                <!-- Total Price -->                
                <td style="text-align: Left; font-family: Arial; font-weight: 700; color: #00CC00; font-size: small;" 
                    class="style44">Total Price (Tk.)</td>
                <td class="style20"> : </td>
                <td style="text-align: left" class="style45">
                <asp:TextBox ID="txtTotalAmnt" runat="server" Width="200px" 
                    style="font-weight: 700" Enabled="False"></asp:TextBox></td>
                <td class="style47"></td>
            </tr>

                        
            <!-- Discount Code -->
            <tr>
                <td class="style43"></td>
                <td style="text-align: Left; font-family: Arial; " 
                    class="style34">Discount Code</td>
                <td class="style20"> : </td>
                <td style="text-align: left" class="style27">
                <asp:TextBox ID="txtDisCode" runat="server" Width="200px" 
                    style="font-weight: 700" Enabled="False"></asp:TextBox></td>
                
                <!-- Discount Amount -->
                <td class="style57"></td>
                <td style="text-align: Left; font-family: Arial; color: #FF0000;" 
                    class="style44">Discount Amount</td>
                <td class="style20"> : </td>
                <td style="text-align: left" class="style45">
                <asp:TextBox ID="txtDisAmnt" runat="server" AutoPostBack="True" Width="200px" 
                    style="font-weight: 700" OnTextChanged="txtDisAmnt_TextChanged"
                    onkeypress="return numeric_only(event)"
                    ToolTip="Discount Amount" TabIndex="8" MaxLength="6"></asp:TextBox></td>
                <td class="style47"></td>
            </tr>

            
             <!-- Discount Reference -->
            <tr>
                <td class="style43"></td>
                <td style="text-align: Left; font-family: Arial; " 
                    class="style34">Ref. for Discount</td>
                <td class="style20"> : </td>
                <td style="text-align: left" class="style27">
                    <asp:TextBox ID="txtDisRef" runat="server" Width="200px" 
                      style="font-weight: 700"
                      ToolTip="Reference for Discount" TabIndex="9"></asp:TextBox>
                </td>

                <!-- Withdrawn/Adjustment Amount -->
                <td class="style57"></td>
                <td style="text-align: Left; font-family: Arial; " 
                    class="style44">With/Adjust Amount</td>
                <td class="style20"> : </td>
                <td style="text-align: left" class="style45">
                    <asp:TextBox ID="txtWithAdj" runat="server" AutoPostBack="True" Width="200px" 
                        style="font-weight: 700" 
                        OnTextChanged="txtWithAdj_TextChanged"
                        onkeypress="return numeric_only(event)"
                        ToolTip="Withdrawn/Adjustment Amount..." TabIndex="10" MaxLength="6"></asp:TextBox>
                </td>
                <td class="style47"></td>
            </tr>

            
            <!-- Net Amount -->
            <tr>
                <td class="style48"></td>
                <td class="style49"></td>
                <td class="style49"></td>
                <td class="style49"></td>
                <td class="style54"></td>
                <td style="text-align: Left; font-family: Arial; font-weight: 700; color: #008000;" 
                    class="style51">Net Amount (Tk.)</td>
                <td class="style52"> : </td>
                <td style="text-align: left" class="style53">
                <asp:TextBox ID="txtNet" runat="server" Width="200px" 
                    style="font-weight: 700" Enabled="False"></asp:TextBox></td>
            </tr>

             <!-- Product SL# -->
            <tr>
                <td class="style43"></td>
                <td style="text-align: Left; font-family: Arial;" 
                    class="style34">Product SL#</td>
                <td class="style20"> : </td>
                <td style="text-align: left" class="style27">
                    <asp:TextBox ID="txtSL" runat="server" Width="200px" 
                        style="font-weight: 700"
                        ToolTip="Please Enter Product Serial Number" TabIndex="11" MaxLength="20"></asp:TextBox>
                </td>

                <!-- Remarks -->
                <td class="style57"></td>
                <td style="text-align: Left; font-family: Arial; " 
                    class="style44">Remarks</td>
                <td class="style20"> : </td>
                <td style="text-align: left" class="style45">
                <asp:TextBox ID="txtRemarks" runat="server" Width="200px" 
                    style="font-weight: 700" TabIndex="12" MaxLength="40"></asp:TextBox></td>
                <td class="style47"></td>
            </tr>

                       
            <!-- Add to Data Grid -->
            <tr>
                <td class="style43"></td>
                <td class="style34"></td>
                <td class="style20"></td>
                <td style="text-align: left" class="style27">
                    <asp:Button ID="btnAdd" runat="server" Text="Add" OnClick="btnAdd_Click" 
                        Width="80px" Height="25px"
                        Font-Size="Small" 
                        ToolTip="Click here for add product in list ..." TabIndex="13" 
                        BackColor="#CC6600"/>
                </td>                
            </tr>
            
            <tr>
                <td class="style43"></td>
                <td class="style34"></td>
                <td class="style20"></td>
                <td class="style27"></td>
            </tr>
            
        </table>
        
        
        <div>
        <asp:Label ID="lblError" ForeColor="red" runat="server" Text=""></asp:Label>
        </div>
        
        <table style="border: 0px groove #008000; width: 831px;">
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
                    GridLines="none" Width="826px">
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
                <td colspan="1" align="center" >
                    <asp:Label ID="lblNetAmnt" runat="server" Text="0" Visible="False"></asp:Label>
                </td>
            </tr>
        </table>
        
        <p></p>

        <table style="border: 1px groove #000000; width: 831px;">
            <tr>
                <td>&nbsp;</td> 
                <td>&nbsp;</td>
                <td>&nbsp;</td> 
                <td>&nbsp;&nbsp;&nbsp; &nbsp;</td>              
                <td>                    
                    <asp:Button ID="btnSave" runat="server" Height="25px" Text="Save" 
                        width="68px" onclick="btnSave_Click" TabIndex="14" 
                        Font-Size="Small"
                        ToolTip="Click here for save data..."/>
                        &nbsp;
                    
                    <asp:Button ID="btnCancel" runat="server" Height="25px" Text="Cancel" 
                    Font-Size="Small"
                        Width="68px" TabIndex="16" onclick="btnCancel_Click" />                    
                </td>
                                                
            </tr>
           
        </table>

        <p></p>

   </div>

</asp:Content>


