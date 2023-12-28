<%@ Page Language="C#" AutoEventWireup="true" 
CodeFile="ChangePassword.aspx.cs" Inherits="Forms_ChangePassword" MasterPageFile="DealerReports.master"%>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
        
    <style type="text/css">
        .grid
        {}
        .style20
        {
            width: 13px;
        }
        .style21
        {
            width: 291px;
        }
        .style28
        {
            width: 291px;
            height: 29px;
        }
        .style30
        {
            width: 115px;
            height: 29px;
        }
        .style31
        {
            width: 231px;
        }
        .style32
        {
            width: 231px;
            height: 29px;
        }
        .style33
        {
            width: 115px;
        }
        .style34
        {
            width: 231px;
            height: 21px;
        }
        .style35
        {
            width: 115px;
            height: 21px;
        }
        .style36
        {
            width: 13px;
            height: 21px;
        }
        .style37
        {
            width: 291px;
            height: 21px;
        }
    </style>
                       
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h4 class="col-sm-12 bg-primary" style="padding:5px"> USER CHANGE PASSWORD ...</h4>
    <p></p>
    
    <div align="center">
        
        <table style="border: 1px groove #008000" width="810px">
            <tr>
                <td colspan="5" align="center"
                    style="background-image:url(../Images/header.jpg); height:40px; font-family: Arial; font-size: large; text-decoration: blink;">                        
                    So ... you want to change your password ?</td>
            </tr>
            <tr>
                <td class="style34" align="left" 
                    
                    
                    style="color: #000080; font-family: Arial; font-size: small; font-weight: bold">
                    </td>
                <td class="style35"  align="left"
                    
                    
                    
                    style="font-family: Arial; color: #000080; font-size: small; font-weight: bold">
                    </td>
                <td class="style36"></td>
                <td class="style37" align="left" 
                    
                    style="font-family: Arial; font-size: small; font-weight: bold; color: #000080">No Problem !</td>
            </tr>
            
            <tr>
                <td class="style31" align="left">&nbsp;</td>
                <td class="style33" align="left">&nbsp;</td><td class="style20"></td>
                <td class="style21" align="left">-
                    <span style="color: rgb(40, 71, 117); font-family: Verdana; font-size: 9px; font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal; line-height: normal; orphans: auto; text-align: left; text-indent: 0px; text-transform: none; white-space: normal; widows: auto; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(238, 238, 238); display: inline !important; float: none;">
                    Type your old password.</span></td>
            </tr>

            <tr>
                <td class="style31" align="left">&nbsp;</td>
                <td class="style33" align="left">&nbsp;</td><td class="style20"></td>
                <td class="style21" align="left">-
                    <span style="color: rgb(40, 71, 117); font-family: Verdana; font-size: 9px; font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal; line-height: normal; orphans: auto; text-align: left; text-indent: 0px; text-transform: none; white-space: normal; widows: auto; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(238, 238, 238); display: inline !important; float: none;">
                    Type your new password twice.</span></td>
            </tr>
            <tr>
                <td class="style31"></td><td class="style33"></td><td class="style20"></td>
                <td class="style21"></td>
            </tr>


            <!-- Line Break -->
            <tr>
                <td colspan="5" align="center"
                    style="background-image:url(../Images/header.jpg); height:0.5px; font-family: Arial;">                        
                    </td>
            </tr>

            <tr>
                <td class="style31"></td>
                <td class="style33"></td><td style="text-align: left" class="style20">
              
                &nbsp;</td>
                <td class="style21"></td>
            </tr>


            <!-- User Name -->
            <tr>
                <td class="style31"></td>
                <td style="text-align: Left; font-family: Arial; font-weight: 700; color: #009900;" 
                    class="style33">User Name</td>
                <td class="style20"> : </td>
                <td style="text-align: left" class="style21">
                <asp:TextBox ID="txtUser" runat="server" Width="193px" 
                    style="font-weight: 700" BackColor="#FFFFCC" Enabled="False" ></asp:TextBox>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            
            <!-- Old Password -->
            <tr>
                <td class="style32"></td>
                <td style="text-align: left; font-family: Arial; font-size: small; " 
                    class="style30">Old Password</td>
                <td class="style20"> : </td>
                <td style="text-align: left" class="style28">
                <asp:TextBox ID="txtOldPass" runat="server" TextMode="Password" Width="193px" TabIndex="1" 
                        ToolTip="Please Enter Old Password" MaxLength="10"></asp:TextBox>
                </td>
                
                <td style="font-family: Arial; font-size: x-small; color: #990033" 
                    class="style2">&nbsp;</td>
            </tr>

            <!-- New Password -->
            <tr>
                <td class="style32"></td>
                <td style="text-align: left; font-family: Arial; font-size: small; " 
                    class="style30">New Password</td>
                <td class="style20"> : </td>
                <td style="text-align: left" class="style28">
                <asp:TextBox ID="txtNewPass" runat="server" Width="193px" TabIndex="1" 
                        ToolTip="Please Enter New Password" MaxLength="10" TextMode="Password"></asp:TextBox>
                </td>
                
                <td style="font-family: Arial; font-size: x-small; color: #990033" 
                    class="style2">&nbsp;</td>
            </tr>

            <!-- Confirm Password -->
            <tr>
                <td class="style32"></td>
                <td style="text-align: left; font-family: Arial; font-size: small; " 
                    class="style30">Confirm Password</td>
                <td class="style20"> : </td>
                <td style="text-align: left" class="style28">
                <asp:TextBox ID="txtConfirmPass" runat="server" Width="193px" TabIndex="1" 
                        ToolTip="Please Enter Confirm Password" MaxLength="10" TextMode="Password" 
                        ></asp:TextBox>
                </td>
                
                <td style="font-family: Arial; font-size: x-small; color: #990033" 
                    class="style2">
                    <asp:Label ID="lblPasswordMatch" runat="server" ForeColor="#FF3300" 
                        Text="Password" Visible="False"></asp:Label>
                </td>
            </tr>
                  
                               
            <!-- Line Break -->
           
            
            <tr>
                <td class="style31"></td>
                <td class="style33"></td><td style="text-align: left" class="style20">
              
                &nbsp;</td>
                <td class="style21"></td>
            </tr>
            <!-- ---------------------------------- -->
                     
            <tr>
                <td class="style31"></td>
                <td style="text-align: Left; font-family: Arial; font-weight: 700; color: ##003300;" 
                    class="style33">&nbsp;</td>
                <td class="style20"> &nbsp;</td>              
                <td style="text-align: left" class="style21" >  
                                                      
                    <asp:Button ID="btnSave" runat="server" Height="25px" Text="Change My Password" 
                        width="127px" onclick="btnSave_Click" TabIndex="7" 
                        Font-Size="X-Small"
                        ToolTip="Click here for save data..." BackColor="#000099" 
                        BorderColor="White" Font-Overline="False" Font-Strikeout="False" 
                        Font-Underline="False" ForeColor="Aqua"                        
                        />
                        &nbsp;<asp:Button ID="btnCancel" runat="server" Height="25px" Text="Cancel" 
                    Font-Size="X-Small"
                        Width="62px" TabIndex="9" onclick="btnCancel_Click" BackColor="#003399" 
                        ForeColor="Aqua" />                    
                                        
                </td>



                <td>
                    &nbsp;</td>
            </tr>

                                                                                                          
            <tr>
                <td class="style31"></td>
                <td class="style33"></td>
                <td class="style20">&nbsp;</td>
                <td class="style21">&nbsp;</td>
            </tr>
           
            
        </table>
        
        
        <div>
        <asp:Label ID="lblError" ForeColor="red" runat="server" Text=""></asp:Label>
        </div>
        
        <p>&nbsp;</p>
        <p>&nbsp;</p>
        
   </div>

</asp:Content>



