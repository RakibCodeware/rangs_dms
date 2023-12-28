<%@ Page Language="C#" MasterPageFile="Admin.master" 
AutoEventWireup="true" CodeFile="UserSecurity.aspx.cs" Inherits="Admin_Forms_UserSecurity" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    

    <style type="text/css">
        .style19
        {
            width: 270px;
        }
        .style20
        {
            width: 392px;
        }
    </style>

</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    
    <h2>
        Software User Security
    </h2>
    
    <div>    
        <table style="border: 1px groove #008000" width="100%">
            <tr>
                <td colspan="3" align="center"                    
                    style="background-image:url('../../Images/header.jpg'); height:40px; font-family: Arial; font-size: large; text-decoration: blink;">                        
                    Software Menu for User Restriction </td>
            </tr>

            <tr>
                <td></td>
                <td>&nbsp;</td>
                <td></td>
            </tr>

            <tr>
                <td align ="right">User Name : </td>
                <td>
                    <asp:DropDownList ID="ddlUser" runat="server" Height="21px" 
                        BackColor="#F6F1DB"
                        ToolTip="Please Select User Name ..." AutoPostBack="True"
                        Width="368px" onselectedindexchanged="ddlUser_SelectedIndexChanged">
                        <asp:ListItem Text = "--Select--" Value = ""></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td></td>
            </tr>

            <tr>
                <td align ="right">User Full Name : </td>
                <td>
                      <asp:TextBox ID="txtFullName" runat="server" Height="20px" Width="195px" BackColor="#FFCCFF" 
                      BorderStyle="None" ReadOnly="True"></asp:TextBox>
                </td>
                <td></td>
            </tr>

            <tr>
                <td align ="right">User Job ID : </td>
                <td>
                      <asp:TextBox ID="txtJobID" runat="server" Height="20px" Width="195px" BackColor="#FFCCFF" 
                      BorderStyle="None" ReadOnly="True"></asp:TextBox>
                </td>
                <td></td>
            </tr>

            <tr>
                <td align ="right">Designation : </td>
                <td>
                      <asp:TextBox ID="txtDesg" runat="server" Height="20px" Width="195px" BackColor="#FFCCFF" 
                      BorderStyle="None" ReadOnly="True"></asp:TextBox>
                </td>
                <td></td>
            </tr>

            <tr>
                <td align ="right">Branch/Dept. : </td>
                <td>
                      <asp:TextBox ID="txtBranch" runat="server" Height="20px" Width="195px" BackColor="#FFCCFF" 
                      BorderStyle="None" ReadOnly="True"></asp:TextBox>
                      &nbsp;
                      <asp:TextBox ID="txtBrID" runat="server" Height="18px" Width="58px" BackColor="#FFCCFF" 
                      BorderStyle="None" ReadOnly="True" Visible="False"></asp:TextBox>
                </td>
                <td></td>
            </tr>

            <tr>
                <td></td>
                <td>&nbsp;</td>
                <td></td>
            </tr>

            <tr>
                <td></td>
                <td>
                    <asp:Label ID="Label1" runat="server" Font-Names="Tahoma" Font-Size="Small" 
                        ForeColor="#990000" 
                        Text="Please Check Menu for User Permission from below list..." 
                        Font-Bold="True" Font-Underline="True"></asp:Label>
                </td>
                <td></td>
            </tr>

            <tr>
                <td class="style19"></td>
                <td class="style20">
                    <asp:TreeView ID="TreeView1" runat="server" Width="370px"                         
                        ontreenodecheckchanged="TreeView1_TreeNodeCheckChanged" 
                        Font-Names="Tahoma" BorderColor="#000099" BorderStyle="Solid" 
                        ShowLines="True">
                      <Nodes>
                          <asp:TreeNode ShowCheckBox="True" Text="Home" Value="mnuHome">   
                          </asp:TreeNode>
                                                   
                          <asp:TreeNode ShowCheckBox="True" Text="Transaction" Value="mnuTr">
                            <asp:TreeNode ShowCheckBox="True" Text="Product Requirement (For CTP)" Value="mnuTrRequirement"/>
                            <asp:TreeNode ShowCheckBox="True" Text="Waiting List for Receive (For CTP)" Value="mnuTrWaitingList"/>
                            <asp:TreeNode ShowCheckBox="True" Text="Product Receive (For CTP)" Value="mnuTrRecCTP"/>
                            <asp:TreeNode ShowCheckBox="True" Text="Product Receive (For CIDD)" Value="mnuTrRecCIDD"/>
                            <asp:TreeNode ShowCheckBox="True" Text="Sales Entry (For CTP)" Value="mnuTrSalesCTP"/>
                            <asp:TreeNode ShowCheckBox="True" Text="Sales Entry (For CI&DD)" Value="mnuTrSalesCIDD"/>
                            <asp:TreeNode ShowCheckBox="True" Text="Product Issue/Delivery (For CIDD)" Value="mnuTrIssue"/>
                            <asp:TreeNode ShowCheckBox="True" Text="Product Transfer (For CTP)" Value="mnuTrTransferCTP"/>
                            <asp:TreeNode ShowCheckBox="True" Text="Product Transfer (For CI&DD)" Value="mnuTrTransferCIDD"/>
                            <asp:TreeNode ShowCheckBox="True" Text="Product Withdrawn (For CTP)" Value="mnuTrWithCTP"/>
                            <asp:TreeNode ShowCheckBox="True" Text="Product Withdrawn (For CIDD)" Value="mnuTrWithCIDD"/>
                          </asp:TreeNode>
                                                    
                          <asp:TreeNode ShowCheckBox="True" Text="Edit Transaction" Value="mnuTrEdit">
                            <asp:TreeNode ShowCheckBox="True" Text="Product Requirement (For CTP)" Value="mnuEditReqCTP"/>                          
                            <asp:TreeNode ShowCheckBox="True" Text="Product Receive (For CTP)" Value="mnuEditRecCTP"/>
                            <asp:TreeNode ShowCheckBox="True" Text="Product Receive (For CIDD)" Value="mnuEditRecCIDD"/>
                            <asp:TreeNode ShowCheckBox="True" Text="Sales Entry (For CTP)" Value="mnuEditSalesCTP"/>
                            <asp:TreeNode ShowCheckBox="True" Text="Sales Entry (For CI&DD)" Value="mnuEditSalesCIDD"/>
                            <asp:TreeNode ShowCheckBox="True" Text="Product Issue/Delivery (For CIDD)" Value="mnuEditIssue"/>
                            <asp:TreeNode ShowCheckBox="True" Text="Product Transfer (For CTP)" Value="mnuEditTransferCTP"/>
                            <asp:TreeNode ShowCheckBox="True" Text="Product Transfer (For CI&DD)" Value="mnuEditTransferCIDD"/>
                            <asp:TreeNode ShowCheckBox="True" Text="Product Withdrawn (For CTP)" Value="mnuEditWithCTP"/>
                            <asp:TreeNode ShowCheckBox="True" Text="Product Withdrawn (For CIDD)" Value="mnuEditWithCIDD"/>
                          </asp:TreeNode>

                          <asp:TreeNode ShowCheckBox="True" Text="Report" Value="mnuRPT">                            
                            <asp:TreeNode ShowCheckBox="True" Text="Receive Report (For CTP)" Value="mnuRPTRecCTP"/>
                            <asp:TreeNode ShowCheckBox="True" Text="Receive Report (For CI&DD)" Value="mnuRPTRecCIDD"/>
                            <asp:TreeNode ShowCheckBox="True" Text="Sales Report (For CTP)" Value="mnuRPTSalesCTP"/>
                            <asp:TreeNode ShowCheckBox="True" Text="Sales Report (For CIDD)" Value="mnuRPTSalesCIDD"/>
                            <asp:TreeNode ShowCheckBox="True" Text="Issue/Delivery Report" Value="mnuRPTIssueCIDD"/>
                            <asp:TreeNode ShowCheckBox="True" Text="Transfer Report (For CTP)" Value="mnuRPTTransferCTP"/>
                            <asp:TreeNode ShowCheckBox="True" Text="Transfer Report (For CIDD)" Value="mnuRPTTransferCIDD"/>
                            <asp:TreeNode ShowCheckBox="True" Text="Withdrawn Report (For CTP)" Value="mnuRPTWithCTP"/>
                            <asp:TreeNode ShowCheckBox="True" Text="Withdrawn Report (For CIDD)" Value="mnuRPTWithCIDD"/>

                            <asp:TreeNode ShowCheckBox="True" Text="Stock Report (For CTP)" Value="mnuRPTStockCTP"/>
                            <asp:TreeNode ShowCheckBox="True" Text="Stock Report (For CIDD)" Value="mnuRPTStockCIDD"/>
                          </asp:TreeNode>
                                                    
                          <asp:TreeNode ShowCheckBox="True" Text="Search" Value="mnuSearch">
                            <asp:TreeNode ShowCheckBox="True" Text="Product History" Value="mnuSProdHistory"/>
                            <asp:TreeNode ShowCheckBox="True" Text="Product Price Revision Statement" Value="mnuSProdPR"/>                
                          </asp:TreeNode>

                          <asp:TreeNode ShowCheckBox="True" Text="Setting" Value="mnuSetting">
                            <asp:TreeNode ShowCheckBox="True" Text="Entity Information" Value="mnuSettEntity"/>
                            <asp:TreeNode ShowCheckBox="True" Text="Product Information" Value="mnuSettProd"/>
                            <asp:TreeNode ShowCheckBox="True" Text="Employee Information" Value="mnuSettEmployee"/>

                            <asp:TreeNode ShowCheckBox="True" Text="Campaign Information" Value="mnuSettCamp"/>
                            <asp:TreeNode ShowCheckBox="True" Text="Setting Free/Gift Items" Value="mnuSettGift"/>
                            <asp:TreeNode ShowCheckBox="True" Text="Setting Discount Offer" Value="mnuSettDiscount"/>
                                                        
                            <asp:TreeNode ShowCheckBox="True" Text="Software User Information" Value="mnuSettUser"/>
                            <asp:TreeNode ShowCheckBox="True" Text="User Restriction" Value="mnuSettUserRestriction"/>
                            <asp:TreeNode ShowCheckBox="True" Text="User Change Password" Value="mnuSettChangePW"/>
                          </asp:TreeNode>
            
                            <asp:TreeNode ShowCheckBox="True" Text="Generate" Value="mnuGenerate">
                                <asp:TreeNode ShowCheckBox="True" Text="Individual Showroom Commission" Value="mnuGComm"/>
                                <asp:TreeNode ShowCheckBox="True" Text="Countrywide Commission" Value="mnuGCommCountry"/>
                                <asp:TreeNode ShowCheckBox="True" Text="Discount Code" Value="mnuGDCode"/>
                                <asp:TreeNode ShowCheckBox="True" Text="Barcode Generate & Print" Value="mnuGBCode"/>
                            </asp:TreeNode>
                            
                        </Nodes>

                    </asp:TreeView>
                </td>
                <td></td>
            </tr>

            <tr>
                <td></td>
                <td></td>
                <td></td>
            </tr>

            <tr>
                <td></td>
                <td>  
                                                      
                    <asp:Button ID="btnSave" runat="server" Height="25px" Text="Save" 
                        width="89px" onclick="btnSave_Click" TabIndex="6" 
                        Font-Size="X-Small"
                        ToolTip="Click here for save data..." BackColor="#000099" 
                        BorderColor="White" Font-Overline="False" Font-Strikeout="False" 
                        Font-Underline="False" ForeColor="Aqua"                        
                        />
                        &nbsp;
                   <asp:Button ID="btnCancel" runat="server" Height="25px" Text="Cancel" 
                        Font-Size="X-Small"
                        Width="73px" TabIndex="7" onclick="btnCancel_Click" BackColor="#003399" 
                        ForeColor="Aqua" />                    
                                        
                </td>
                <td></td>
            </tr>

        </table>

    </div> 

    <div>&nbsp;</div>

</asp:Content>