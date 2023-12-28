<%@ Page Language="C#" MasterPageFile="Admin.master"
AutoEventWireup="true" CodeFile="UserInfo.aspx.cs" Inherits="Admin_Forms_UserInfo" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
                               
    <style type="text/css">

        .style20
        {
            width: 13px;
        }
        .style21
        {
            width: 291px;
        }
        .style30
        {
            width: 176px;
            height: 29px;
        }
        .style28
        {
            width: 291px;
            height: 29px;
        }
        .mGrid
        {}
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
                        
         .style34
         {
             width: 131px;
         }
         .style35
         {
             width: 131px;
             height: 29px;
         }
         .style36
         {
             width: 194px;
         }
                        
    </style>  
        
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    
    <p>&nbsp;</p>
    
    <div align="center">
        
        <table width="100%" style="font-family: Tahoma; font-size: small">
            <tr>
                <td colspan="5" align="center"                    
                    style="background-image:url(../Images/header.jpg); height:30px; font-family: Arial; font-size: large; text-decoration: blink; color: #FFFFFF;">                        
                    User Information </td>
            </tr>

            <tr>
                <td class="style34"></td>
                <td class="style36"></td>
                <td class="style20">&nbsp;</td>
                <td class="style21">&nbsp;</td>
            </tr>
                           
            <tr>
                <td class="style34">
                    <asp:TextBox ID="txtProdID" runat="server" Width="6px" 
                        style="font-weight: 700" Visible="False"></asp:TextBox> 
                </td>
                <td class="style36" align="left" 
                    style="font-family: Tahoma; color: #008000; font-size: large">Employee Name</td><td style="text-align: left" class="style20">                
                    &nbsp;</td>
                <td style="text-align: left" class="style21">
                    <asp:DropDownList ID="ddlEmp" runat="server" Height="26px" 
                        BackColor="#F6F1DB"
                        ToolTip="Please Select Employee Name ..." AutoPostBack="True"
                        Width="195px" onselectedindexchanged="ddlEmp_SelectedIndexChanged">
                        <asp:ListItem Text = "--Select--" Value = ""></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            
            <!-- Employee ID -->
            <tr>
                <td class="style35"></td>
                <td style="text-align: left; font-family: Tahoma; font-size: small; color: #000080; font-weight: normal;" 
                    class="style30">Employee ID</td>
                <td class="style20"> : </td>
                <td class="style35" align="left">
                      <asp:TextBox ID="txtEmpID" runat="server" Height="20px" Width="195px" BackColor="#FFCCFF" 
                      BorderStyle="None" ReadOnly="false"></asp:TextBox>
                </td>
                
                <td style="font-family: Arial; font-size: x-small; color: #990033" 
                    class="style2">&nbsp;</td>
            </tr>

            <!-- Designation -->
            <tr>
                <td class="style35"></td>
                <td style="text-align: left; font-family: Tahoma; font-size: small; color: #000080; font-weight: normal;" 
                    class="style30">Designation</td>
                <td class="style20"> : </td>
                <td class="style35" align="left">
                      <asp:TextBox ID="txtDesg" runat="server" Height="20px" Width="195px" BackColor="#FFCCFF" 
                      BorderStyle="None" ReadOnly="false"></asp:TextBox>
                </td>
                
                <td style="font-family: Arial; font-size: x-small; color: #990033" 
                    class="style2">&nbsp;</td>
            </tr>

            <!-- Branch -->
            <tr>
                <td class="style35">
                    <asp:TextBox ID="txtBrCode" runat="server" Width="6px" 
                        style="font-weight: 700" Visible="False"></asp:TextBox> 
                </td>
                <td style="text-align: left; font-family: Tahoma; font-size: small; color: #000080; font-weight: bold;" 
                    class="style30">Branch</td>
                <td class="style20"> : </td>
                <td class="style35" align="left">
                      <asp:TextBox ID="txtBr" runat="server" Height="20px" Width="195px" BackColor="#FFCCFF" 
                      BorderStyle="None" ReadOnly="false"></asp:TextBox>
                </td>
                
                <td style="font-family: Arial; font-size: x-small; color: #990033" 
                    class="style2">&nbsp;</td>
            </tr>
            
            

            <tr>
                <td class="style34"></td>
                <td class="style36"></td><td style="text-align: left" class="style20">              
                &nbsp;</td>
                <td class="style21"></td>
            </tr>

            <!-- Software User -->
            <tr>
                <td class="style35"></td>
                <td style="text-align: left; font-family: Tahoma; font-size: small; color: #008000; font-weight: bold;" 
                    class="style30">User Name</td>
                <td class="style20"> : </td>
                <td style="text-align: left" class="style28">
                <asp:TextBox ID="txtUser" runat="server" Width="193px" TabIndex="5" 
                        ToolTip="Please Enter Software User Name ..." MaxLength="45" ></asp:TextBox>
                </td>
                
                <td style="font-family: Arial; font-size: x-small; color: #990033" 
                    class="style2">&nbsp;</td>
            </tr>
            
            <!-- Password -->
            <tr>
                <td class="style35"></td>
                <td style="text-align: left; font-family: Tahoma; font-size: small; color: #FF0000;" 
                    class="style30">Password</td>
                <td class="style20"> : </td>
                <td style="text-align: left" class="style28">
                    <asp:TextBox ID="txtPass" runat="server" Width="193px" TabIndex="5" 
                        ToolTip="Please Enter Password ..." MaxLength="45" TextMode="Password" 
                        > </asp:TextBox>
                </td>
                
                <td style="font-family: Arial; font-size: x-small; color: #990033" 
                    class="style2">&nbsp;</td>
            </tr>
            
            <!-- Confirm Password -->
            <tr>
                <td class="style35"></td>
                <td style="text-align: left; font-family: Tahoma; font-size: small; color: #FF0000;" 
                    class="style30">Confirm Password</td>
                <td class="style20"> : </td>
                <td style="text-align: left" class="style28">
                <asp:TextBox ID="txtPassConfirm" runat="server" Width="193px" TabIndex="5" 
                        ToolTip="Please Enter Confirm Password ..." MaxLength="45" 
                        TextMode="Password"></asp:TextBox>
                </td>
                
                <td style="font-family: Arial; font-size: x-small; color: #990033" 
                    class="style2">&nbsp;</td>
            </tr>
            
            <!-- Blank Row -->
            <tr>
                <td class="style34"></td>
                <td class="style36"></td><td style="text-align: left" class="style20">              
                &nbsp;</td>
                <td class="registrationFormAlert" id="divCheckPasswordMatch"></td>
                 
            </tr>

            <!-- User Type -->
            <tr>
                <td class="style35">                   
                </td>
                <td style="text-align: left; font-family: Tahoma; font-size: small; color: #000080; font-weight: bold;" 
                    class="style30">User Type</td>
                <td class="style20"> : </td>
                <td class="style35" align="left">
                      <asp:DropDownList ID="ddlUserType" runat="server" Height="19px" Width="193px" 
                          onselectedindexchanged="ddlUserType_SelectedIndexChanged" 
                          AutoPostBack="True">
                          <asp:ListItem>Admin</asp:ListItem>
                          <asp:ListItem>Normal</asp:ListItem>
                          <asp:ListItem>CTP</asp:ListItem>
                          <asp:ListItem>Management</asp:ListItem>
                          <asp:ListItem>PIC</asp:ListItem>
                          <asp:ListItem>Other</asp:ListItem>
                      </asp:DropDownList>
                </td>
                
                <td style="font-family: Arial; font-size: x-small; color: #990033" 
                    class="style2">&nbsp;</td>
            </tr>
                        
                              
            <tr>
                <td class="style34"></td>
                <td class="style36"></td><td style="text-align: left" class="style20">              
                &nbsp;</td>
                <td class="style21"></td>
            </tr>
            <!-- ---------------------------------- -->
                     
            <tr>
                <td class="style34"></td>
                <td style="text-align: Left; font-family: Arial; font-weight: 700; color: ##003300;" 
                    class="style36">&nbsp;</td>
                <td class="style20"> &nbsp;</td>              
                <td style="text-align: left" class="style21" >  
                                                      
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
                
                <td>&nbsp;</td>
            </tr>

           <tr>
                <td class="style34"></td>
                <td class="style36"></td>
                <td class="style20">&nbsp;</td>
                <td class="style21">&nbsp;</td>
            </tr>
            
            <!-- Line Break -->
            <tr>
                <td colspan="5" align="center"                    
                    style="background-image:url(../Images/header.jpg); height:0.5px; font-family: Arial;">                        
                </td>
            </tr>
                                                                                                          
            <tr>
                <td class="style34"></td>
                <td class="style36"></td>
                <td class="style20">&nbsp;</td>
                <td class="style21">&nbsp;</td>
            </tr>
            

            <!-- Grid View -->                       
            <tr>
                                
                <td colspan="5" align="center">   
                
                    <asp:GridView ID="gvCustomres" runat="server"                        
                        AutoGenerateColumns="False"
                        DataKeyNames="UserName"
                        GridLines="None"
                        AllowPaging="false"
                        CssClass="mGrid"
                        PagerStyle-CssClass="pgr"                                               
                        AlternatingRowStyle-CssClass="alt" 
                        Onrowdeleting="gvCustomres_RowDelating"
                        OnRowDataBound="gvCustomres_RowDataBound"
                        Width="100%"
                        >
                        <SelectedRowStyle BackColor="BurlyWood"/>
                        <Columns>
                            <asp:TemplateField HeaderText="SL #">
                                 <ItemTemplate>
                                       <%# Container.DataItemIndex + 1 %>
                                 </ItemTemplate>
                            </asp:TemplateField>       
                            <asp:BoundField DataField="FullName" HeaderText="Employee Name" />
                            <asp:BoundField DataField="Designation" HeaderText="Designation" />                     
                            <asp:BoundField DataField="UserName" HeaderText="User Name" />
                            <asp:BoundField DataField="Passward" HeaderText="User Password" />
                            <asp:BoundField DataField="WebAccess" HeaderText="Web Access" />
                            <asp:BoundField DataField="UserType" HeaderText="User Type" />
                            <asp:BoundField DataField="eName" HeaderText="CTP Name" />
                            <asp:BoundField DataField="Active" HeaderText="Active" />
                            
                            
                            <asp:TemplateField HeaderText="">
                                <ItemTemplate>               
                                    <asp:ImageButton ID="ibtnEdit" runat="server"
                                        ToolTip="Edit Previous Record"                                        
                                        ImageUrl="~/Images/btn-edit.png" 
                                        CommandName="Edit"   
                                        OnClientClick="return confirm('Are you sure you want to edit this record?');"                                                                                                          
                                    />
                                </ItemTemplate>                                
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="">
                                <ItemTemplate>               
                                    <asp:ImageButton ID="ibtnDelete" runat="server"
                                        ToolTip="Delete any record ??"                                        
                                        ImageUrl="~/Images/btn-delete.jpg" 
                                        CommandName="Delete"   
                                        OnClientClick="return confirm('Are you sure you want to delete this record?');"                                                                                                          
                                    />
                                </ItemTemplate>                                
                            </asp:TemplateField>


                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <!-- ---------------------------------- -->

            <tr>
                <td class="style34"></td>
                <td class="style36"></td>
                <td class="style20">&nbsp;</td>
                <td class="style21">&nbsp;</td>
            </tr>
            
        </table>
        
                       
   </div>

   <div > &nbsp;</div>
   <div > &nbsp;</div>

</asp:Content>


