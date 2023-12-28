<%@ Page Language="C#" MasterPageFile="Admin.master" AutoEventWireup="true"
CodeFile="Requirement_Edit.aspx.cs" Inherits="Forms_Requirement_Edit" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
        
    <style type="text/css">
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
    
    <style type="text/css">
           
        .mGrid { 
            background-color: #fff; 
            margin: 5px 0 10px 0; 
            border: solid 1px #525252; 
            border-collapse:collapse; 
        }
        .mGrid td { 
            padding: 2px; 
            border: solid 1px #c1c1c1; 
            color: #717171; 
        }
        .mGrid th { 
            padding: 4px 2px; 
            color: #fff; 
            background: #424242 url(grd_head.png) repeat-x top; 
            border-left: solid 1px #525252; 
            font-size: 0.9em; 
        }
        .mGrid .alt { background: #fcfcfc url(grd_alt.png) repeat-x top; }
        .mGrid .pgr { background: #424242 url(grd_pgr.png) repeat-x top; }
        .mGrid .pgr table { margin: 5px 0; }
        .mGrid .pgr td { 
            border-width: 0; 
            padding: 0 6px; 
            border-left: solid 1px #666; 
            font-weight: bold; 
            color: #fff; 
            line-height: 12px; 
         }   
        .mGrid .pgr a { color: #666; text-decoration: none; }
        .mGrid .pgr a:hover { color: #000; text-decoration: none; }


       .highlight
        {
            background-color: #ffeb95;
            cursor: pointer;
        }
        .normal
        {
            background-color: white;
            cursor: pointer;
        }
                        
         </style>
                       
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    
    <h4 class="col-sm-12 bg-primary" style="padding:5px"> Product Requirement (Edit/Update) ...</h4>
    <p></p>
    
    <div align="center">
        
        <table width="100%" style="font-family: Tahoma; font-size: small">
            <tr>
                <td colspan="5" align="center"></td>
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
                    style="font-weight: 700" ToolTip="Please Enter Challan Number" MaxLength="20"></asp:TextBox>
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
        
        <table width="100%" style="font-family: Tahoma; font-size: small">
            <tr>
                    <td >
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
                        Width="100%">
                        <FooterStyle Font-Bold="true" BackColor="#61A6F8" ForeColor="black" />
                        <Columns>
                            <asp:BoundField HeaderText="ProductID" DataField="ProductID" ItemStyle-Width="5px"/>                        
                            <asp:BoundField HeaderText="Product Model" DataField="Model" />
                            <asp:BoundField HeaderText="Quantity" DataField="Qty" ItemStyle-Width="5px"/>                                                                                                         
                            
                            <asp:BoundField HeaderText="Remarks" DataField="Remarks"  />
                            <asp:TemplateField HeaderText="Action" ItemStyle-Width="5px">
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
                    <asp:Button ID="btnSave" runat="server" Text="Save" 
                        width="88px" onclick="btnSave_Click" TabIndex="7" 
                        Font-Size="Small" CssClass="btn btn-primary"
                        ToolTip="Click here for save data..."/>
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

        <p></p>

   </div>

</asp:Content>



