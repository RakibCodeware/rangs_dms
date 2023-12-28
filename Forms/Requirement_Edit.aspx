<%@ Page Language="C#" MasterPageFile="~/CTP_Admin.master" AutoEventWireup="true"
CodeFile="Requirement_Edit.aspx.cs" Inherits="Forms_Requirement_Edit" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

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
            width: 260px;
        }
        .style22
        {
            width: 260px;
            height: 29px;
        }
        .style24
        {
            width: 140px;
            height: 29px;
        }
        .style26
        {
            width: 30px;
        }
        .style27
        {
            width: 55px;
        }
        .style28
        {
            width: 112px;
            height: 29px;
        }
        .style29
        {
            width: 112px;
        }
        .style30
        {
            width: 140px;
        }
    </style>
                       
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        Product Requirement (Edit/Update)
    </h2>
    <p></p>
    
    <div align="center">
        
        <table style="border: 1px groove #008000" width="810px">
            <tr>
                <td colspan="5" align="center"
                    style="background-image:url(../Images/header.jpg); height:40px; font-family: Arial; font-size: large; text-decoration: blink;">                        
                    Product Requirement Edit Previous Entry</td>
            </tr>
            <tr>
                <td class="style29"></td><td class="style30"></td><td class="style26"></td>
                <td class="style21" align="left">
                    <asp:TextBox ID="txtMRSR" runat="server" Enabled="False" Font-Size="Smaller" 
                        Width="16px" Visible="False"></asp:TextBox>
                    <asp:TextBox ID="txtMRSRID" runat="server" Enabled="False" Font-Size="Smaller" 
                        Width="16px" Visible="False"></asp:TextBox>
                </td>
            </tr>
            
            <!-- Challan No. -->
            <tr>
                <td class="style29"></td>
                <td style="text-align: Left; font-family: Arial; font-weight: 700; color: #009900;" 
                    class="style30">Request #</td>
                <td class="style26"> : </td>
                <td style="text-align: left" class="style21">
                <asp:TextBox ID="txtCHNo" runat="server" Width="200px" 
                    style="font-weight: 700" ToolTip="Please Enter Challan Number" MaxLength="15"></asp:TextBox>
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
                <td class="style29"></td>
                <td class="style30"></td>
                <td class="style26"></td>
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
                    class="style24">Request Date</td>
                <td class="style26"> : </td>
                <td style="text-align: left" class="style22">
                <asp:TextBox ID="txtDate" runat="server" Width="97px" TabIndex="1" 
                        ToolTip="Please Enter Challan Date" MaxLength="10"></asp:TextBox> 
                &nbsp;</td>
                
                <td style="font-family: Arial; font-size: x-small; color: #990033" 
                    class="style2">(MM/dd/yyyy))
                </td>

            </tr>
            
            
                               
            <!-- Line Break -->
            <tr>
                <td class="style29"></td>
                <td class="style30"></td><td style="text-align: left" class="style26">
              
                &nbsp;</td>
                <td class="style21"></td>
                <td class="style21"></td>
            </tr>
            <tr>
                <td colspan="5" align="center"
                    style="background-image:url(../Images/header.jpg); height:0.5px; font-family: Arial;">                        
                    </td>
            </tr>
            <tr>
                <td class="style29"></td>
                <td class="style30"></td><td style="text-align: left" class="style26">
              
                &nbsp;</td>
                <td class="style21"></td>
            </tr>
            <!-- ---------------------------------- -->

            
             <!-- Product Model -->
            <tr>
                <td class="style29"></td>
                <td style="text-align: Left; font-family: Arial; font-weight: 700; color: ##003300;" 
                    class="style30">Product Model</td>
                <td class="style26"> : </td>              
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
                    <asp:TextBox ID="txtProdID" runat="server" Width="16px" 
                        style="font-weight: 700" Visible="False"></asp:TextBox>

                </td>
            </tr>

             <!-- Product Description -->
            <tr>
                <td class="style29"></td>
                <td style="text-align: Left; font-family: Arial; " 
                    class="style30">Product Description </td>
                <td class="style26"> : </td>
                <td style="text-align: left" class="style21">
                <asp:TextBox ID="txtProdDesc" runat="server" Width="200px" 
                    style="font-weight: 500" Enabled="False"></asp:TextBox></td>
            </tr>
            
            

            <!-- Quantity -->
            <tr>
                <td class="style29"></td>
                <td style="text-align: Left; font-family: Arial; font-weight: 700; color: ##003300;" 
                    class="style30">Quantity</td>
                <td class="style26"> : </td>
                <td style="text-align: left" class="style21">
                    <asp:TextBox ID="txtQty" runat="server" AutoPostBack="True" Width="200px" 
                        style="font-weight: 700"
                        onkeypress="return numeric_only(event)"
                        ToolTip="Please Enter Product Quantity" TabIndex="3"></asp:TextBox>                    
                </td>
                <td>
                    &nbsp;</td>
            </tr>
                       
             <!-- Remarks -->
            <tr>
                <td class="style29"></td>
                <td style="text-align: Left; font-family: Arial; " 
                    class="style30">Remarks</td>
                <td class="style26"> : </td>
                <td style="text-align: left" class="style21">
                <asp:TextBox ID="txtRemarks" runat="server" Width="200px" 
                    style="font-weight: 700" TabIndex="5"></asp:TextBox></td>
            </tr>

            
            <!-- Add to Data Grid -->
            <tr>
                <td class="style29"></td>
                <td class="style30"></td>
                <td class="style26"></td>
                <td style="text-align: left" class="style21">
                    <asp:Button ID="btnAdd" runat="server" Text="Add" OnClick="btnAdd_Click" 
                        Width="80px" Height="25px" 
                        Font-Size="Small"
                        ToolTip="Click here for add product in list ..." TabIndex="6"/>
                </td>                
            </tr>
            
            <tr>
                <td class="style29"></td>
                <td class="style30"></td>
                <td class="style26"></td>
                <td class="style21"></td>
            </tr>
            
        </table>
        
        
        <div>
        <asp:Label ID="lblError" ForeColor="red" runat="server" Text=""></asp:Label>
        </div>
        
        <table style="border: 1px groove #008000;" width="810px">
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
                            <asp:BoundField HeaderText="Qty" DataField="Qty" />                                                                                                         
                            
                            <asp:BoundField HeaderText="Remarks" DataField="Remarks" ItemStyle-Width="5px" />
                            <asp:TemplateField HeaderText="Action">
                                <ItemTemplate>               
                                    <asp:ImageButton ID="ibtnDelete" runat="server"
                                        ToolTip="Delete"                                        
                                        ImageUrl="~/Images/btn-delete.jpg" 
                                        CommandName="Delete"   
                                        OnClientClick="return confirm('Are you sure you want to remove this record?');"                                                                                                          
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
                <td> &nbsp;</td>
            </tr>
            <tr>
                <td align="center">                    
                    <asp:Button ID="btnSave" runat="server" Height="25px" Text="Save" 
                        width="68px" onclick="btnSave_Click" TabIndex="7" 
                        Font-Size="Small"
                        ToolTip="Click here for save data..."/>
                        &nbsp;
                    <asp:Button ID="btnPrint" runat="server" Height="25px" Text="Print"
                    Font-Size="Small"
                    Width="68px" TabIndex="8"/>
                    &nbsp;
                    <asp:Button ID="btnCancel" runat="server" Height="25px" Text="Cancel" 
                    Font-Size="Small"
                        Width="68px" TabIndex="9" onclick="btnCancel_Click" />                    
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
            </tr>
        </table>

        <p></p>

   </div>

</asp:Content>



