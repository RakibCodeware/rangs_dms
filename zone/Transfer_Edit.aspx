<%@ Page Language="C#" MasterPageFile="Admin.master" AutoEventWireup="true" 
CodeFile="Transfer_Edit.aspx.cs" Inherits="Forms_Transfer_Edit" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
        
    <style type="text/css">
        .style19
        {
            width: 631px;
        }
        .grid
        {}
        .style21
        {
            width: 164px;
        }
        .style22
        {
            width: 164px;
            height: 29px;
        }
        .style24
        {
            width: 82px;
            height: 29px;
        }
        .style25
        {
            width: 82px;
        }
        .style27
        {
            width: 137px;
        }
        .style28
        {
            width: 43px;
        }
        .style29
        {
            width: 16px;
        }
    </style>
        
    <script language="JavaScript" src="../js/datetimepicker.js" type="text/javascript"> </script>
     
       
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    
    <p></p>
    
    <div align="center">
        
        <table width="100%" style="font-family: Tahoma; font-size: small">

            <tr>
                <td colspan="5" align="center"
                    style="background-image:url(../Images/header.jpg); height:30px; font-family: Arial; font-size: large; text-decoration: blink;">                        
                    Product Transfer Entry Form</td>
            </tr>
            <tr>
                <td class="style28"></td>
                <td class="style25">&nbsp;</td>
                <td class="style29"></td>
                <td class="style21" align="left">
                    <asp:TextBox ID="txtMRSR" runat="server" Enabled="False" Font-Size="Smaller" 
                        Width="25px" Visible="False"></asp:TextBox>
                    <asp:TextBox ID="txtMRSRID" runat="server" Enabled="False" Font-Size="Smaller" 
                        Width="16px" Visible="False"></asp:TextBox>
                </td>
            </tr>
            
            
            <!-- Challan No. -->
            <tr>
                <td class="style28"></td>
                <td style="text-align: Left; font-family: Arial; font-weight: 700; color: #009900;" 
                    class="style25">Challan #</td>
                <td class="style29"> : </td>
                <td style="text-align: left" class="style21">
                <asp:TextBox ID="txtCHNo" runat="server" Width="200px" 
                    style="font-weight: 700" ToolTip="Please Enter Challan Number" MaxLength="25"></asp:TextBox>
                </td>
                <td>
                    <asp:RequiredFieldValidator 
                     id="RequiredFieldValidator1" runat="server" 
                     ErrorMessage="Required!"                      
                     ControlToValidate="txtCHNo">
                    </asp:RequiredFieldValidator>
                </td>
            </tr>
            
            <!-- Search Button -->
            <tr>
                <td class="style28"></td>
                <td class="style25"></td>
                <td class="style29"></td>
                <td style="text-align: left" class="style27">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" 
                        Width="80px" Height="25px"
                        Font-Size="Small" 
                        ToolTip="Click here for Search Challan details ..." TabIndex="1" 
                        BackColor="#CC6600"/>
                </td>  
                
                <td class="style21"></td>
            </tr>

            <!-- Challan Date -->
            <tr>
                <td class="style28"></td>
                <td style="text-align: left; font-family: Arial; font-size: small; " 
                    class="style24">Challan Date</td>
                <td class="style29"> : </td>
                <td style="text-align: left" class="style22">
                <asp:TextBox ID="txtDate" runat="server" Width="97px" TabIndex="1" 
                        ToolTip="Please Enter Challan Date" MaxLength="10"></asp:TextBox> 
                &nbsp;
                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" 
                            ErrorMessage="Not Valid." 
                            ControlToValidate="txtDate"
                            ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                            style="font-size: xx-small; font-family: Arial, Helvetica, sans-serif" />
                
                </td>
                <td style="font-family: Arial; font-size: x-small; color: #990033" 
                    class="style2">(MM/dd/yyyy))</td>
            </tr>
           
           
            <!-- Line Break -->
            <tr>
                <td class="style28"></td>
                <td class="style25"></td><td style="text-align: left" class="style29">
              
                &nbsp;</td>
                <td class="style21">&nbsp;</td>
            </tr>
            <tr>
                <td colspan="5" align="center"
                    style="background-image:url(../Images/header.jpg); height:0.5px; font-family: Arial;">                        
                    </td>
            </tr>
            <tr>
                <td class="style28"></td>
                <td class="style25"></td><td style="text-align: left" class="style29">
              
                &nbsp;</td>
                <td class="style21">&nbsp;</td>
            </tr>
            <!-- ---------------------------------- -->

            
             <!-- Product Model -->
            <tr>
                <td class="style28"></td>
                <td style="text-align: Left; font-family: Arial; font-weight: 700; color: ##003300;" 
                    class="style25">Product Model</td>
                <td class="style29"> : </td>              
                <td style="text-align: left" class="style21" >                    
                    <asp:DropDownList ID="ddlContinents"
                        runat="server" AutoPostBack = "true"
                        OnSelectedIndexChanged="ddlContinents_SelectedIndexChanged" 
                        BackColor="#F6F1DB"
                        Height="28px" Width="202px" TabIndex="2"
                        ToolTip="Please Select Product Model ...">
                        <asp:ListItem Text = "--Select Model--" Value = ""></asp:ListItem>
                    </asp:DropDownList>
                    
                </td>
                <td>
                    &nbsp;
                    <asp:TextBox ID="txtProdID" runat="server" Width="16px" 
                        style="font-weight: 700" Visible="False"></asp:TextBox>

                </td>
            </tr>

             <!-- Product Description -->
            <tr>
                <td class="style28"></td>
                <td style="text-align: Left; font-family: Arial; " 
                    class="style25">Product Description </td>
                <td class="style29"> : </td>
                <td style="text-align: left" class="style21">
                <asp:TextBox ID="txtProdDesc" runat="server" Width="200px" 
                    style="font-weight: 500" Enabled="False"></asp:TextBox></td>
            </tr>
            
            

            <!-- Quantity -->
            <tr>
                <td class="style28"></td>
                <td style="text-align: Left; font-family: Arial; font-weight: 700; color: ##003300;" 
                    class="style25">Quantity</td>
                <td class="style29"> : </td>
                <td style="text-align: left" class="style21">
                    <asp:TextBox ID="txtQty" runat="server" AutoPostBack="True" Width="200px" 
                        style="font-weight: 700"
                        onkeypress="return numeric_only(event)"
                        ToolTip="Please Enter Product Quantity" TabIndex="3"></asp:TextBox>                    
                </td>
                <td>
                    &nbsp;</td>
            </tr>

           
            <!-- Product SL# -->
            <tr>
                <td class="style28"></td>
                <td style="text-align: Left; font-family: Arial;" 
                    class="style25">Product SL#</td>
                <td class="style29"> : </td>
                <td style="text-align: left" class="style21">
                    <asp:TextBox ID="txtSL" runat="server" Width="200px" 
                        style="font-weight: 700"
                        ToolTip="Please Enter Product Serial Number" TabIndex="4"></asp:TextBox>
                </td>
            </tr>

             <!-- Remarks -->
            <tr>
                <td class="style28"></td>
                <td style="text-align: Left; font-family: Arial; " 
                    class="style25">Remarks</td>
                <td class="style29"> : </td>
                <td style="text-align: left" class="style21">
                <asp:TextBox ID="txtRemarks" runat="server" Width="200px" 
                    style="font-weight: 700" TabIndex="5"></asp:TextBox></td>
            </tr>

            
            <!-- Add to Data Grid -->
            <tr>
                <td class="style28"></td>
                <td class="style25"></td>
                <td class="style29"></td>
                <td style="text-align: left" class="style21">
                    <asp:Button ID="btnAdd" runat="server" Text="Add" OnClick="btnAdd_Click" 
                        Width="80px" Height="25px" 
                        Font-Size="Small"
                        ToolTip="Click here for add product in list ..." TabIndex="6"/>
                </td>                
            </tr>
            
            <tr>
                <td class="style28"></td>
                <td class="style25"></td>
                <td class="style29"></td>
                <td class="style21">&nbsp;</td>
            </tr>
            
        </table>
        
        
        <div>
        <asp:Label ID="lblError" ForeColor="red" runat="server" Text=""></asp:Label>
        </div>
        
        <table width="100%" style="font-family: Tahoma; font-size: small">
            <tr>
                    <td class="style19">
                        <!-- Data Grid -->                
                        <asp:GridView ID="gvUsers" runat="server"
                        AutoGenerateColumns="false"                        
                        CssClass="mGrid"
                        DataKeyNames="ProductID"
                        EmptyDataText = "No product in list !!! Please select model and add in list."
                        EmptyDataRowStyle-CssClass ="gvEmpty"
                        Onrowdeleting="gvUsers_RowDelating"
                        ShowFooter="true"
                        OnRowDataBound="gvUsers_RowDataBound"                        
                        GridLines="none" Width="100%">
                        <FooterStyle Font-Bold="true" BackColor="#61A6F8" ForeColor="black" />
                        <Columns>
                            <asp:BoundField HeaderText="ProductID" DataField="ProductID" ItemStyle-Width="5px"/>                        
                            <asp:BoundField HeaderText="Product Model" DataField="Model" />
                            <asp:BoundField HeaderText="Quantity" DataField="Qty" ItemStyle-Width="5px"/>
                                                                                                          
                            <asp:BoundField HeaderText="Product SL" DataField="ProductSL" />
                            <asp:BoundField HeaderText="Remarks" DataField="Remarks"  />
                            <asp:TemplateField HeaderText="">
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
                </td>
            </tr>
            <tr>
                <td colspan="1" align="center"
                    style="background-image:url(../Images/header.jpg); height:0.5px; font-family: Arial; ">                        
                </td>
            </tr>
            <tr>
                <td> &nbsp;&nbsp;</td>
            </tr>
            <tr>
                <td align="center">                    
                    <asp:Button ID="btnSave" runat="server" Text="Save" 
                        width="88px" onclick="btnSave_Click" TabIndex="7" 
                        Font-Size="Small" CssClass="btn btn-primary"
                        ToolTip="Click here for save data..."/>
                        &nbsp;
                    <asp:Button ID="btnPrint" runat="server" Text="Print"
                    Font-Size="Small" CssClass="btn btn-primary"
                    Width="88px" TabIndex="8"/>
                    &nbsp;
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" 
                    Font-Size="Small" CssClass="btn btn-primary"
                    Width="88px" TabIndex="9" onclick="btnCancel_Click" />                    
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
            </tr>
        </table>

        <p>&nbsp;</p>

   </div>

</asp:Content>


