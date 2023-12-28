<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Leave.aspx.cs"
MasterPageFile="Admin.master" Inherits="Leave" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
        
    <style type="text/css">
        .grid
        {}
        .style20
        {
            width: 13px;
        }
        .style27
        {
            width: 32px;
        }
        .style28
        {
            width: 32px;
            height: 14px;
        }
        .style30
        {
            width: 13px;
            height: 14px;
        }
        .style32
        {
            width: 93px;
            height: 14px;
        }
        .style36
        {
            width: 175px;
            height: 14px;
        }
        .style37
        {
            width: 175px;
        }
        .style40
        {
            width: 162px;
        }
        .style41
        {
            width: 162px;
            height: 14px;
        }
    </style>
        
    <script language="JavaScript" src="../js/datetimepicker.js" type="text/javascript"> </script>
    
    <script type="text/javascript">
        $(function () {
            $("#txtDate").datepicker();
            $("#txtFrom").datepicker();
            $("#txtToDate").datepicker();
            $("#txtPIDate").datepicker();
        });        
    </script>
           
</asp:Content>



<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    
        
    <h2 class="col-sm-12 bg-primary" style="padding:5px" align="center"> Leave Application ...</h2>
    <p></p> 
        
    <div align="center">
    <h2 style="font-family: Arial; color: #003366; text-decoration: underline"> Employee Leave Application Form </h2>
        <table style="border: thin groove #008000">
            <tr>
                <td class="style5"></td><td class="style4"></td>
            </tr>
            
            <tr>
                <td style="text-align: right; font-family: Arial; font-weight: 700; color: #009900;" 
                    class="style5">Employee Job Id : </td>
                <td style="text-align: left" class="style4">
                <asp:TextBox ID="txtJobId" runat="server" Width="200px" 
                    style="font-weight: 700"></asp:TextBox></td>
            </tr>
            
            <tr>
                <td style="text-align: right; font-family: Arial; font-size: small; color: #CC0000;" 
                    class="style5">Employee Password : </td>
                <td style="text-align: left" class="style4">
                <asp:TextBox ID="txtPassword" TextMode="Password" runat="server" Width="200px" 
                    style="color: #FF0000"></asp:TextBox>
                    &nbsp;</td>
            </tr>
            
            <tr>
                <td class="style5"></td><td style="text-align: left" class="style4">
                <asp:Button ID="btnSearch" runat="server" 
                    Height="20px" onclick="btnSearch_Click" 
                    Text="Check Employee Validity" Width="198px" />
                </td>
            </tr>
            
            <tr>
                <td style="text-align: right; font-family: Arial; color: #003300; font-size: small;" 
                    class="style5">
                    <span class="style1">Employee Name : </span> </td>
                <td style="text-align: left" class="style4">
                <asp:TextBox ID="txtName" runat="server" Width="200px" 
                    style="font-weight: 700; background-color: #CCCCFF;" ReadOnly="True"></asp:TextBox></td>
            </tr>
            
            <tr>
                <td style="text-align: right; font-family: Arial; color: #003300; font-size: small;" 
                    class="style5">Designation : </td>
                <td style="text-align: left" class="style4">
                <asp:TextBox ID="txtDesg" runat="server" Width="200px" 
                    style="font-weight: 700; background-color: #CCCCFF;" ReadOnly="True"></asp:TextBox></td>
            </tr>
            
            <tr>
                <td style="text-align: right; font-family: Arial; color: #003300; font-size: small;" 
                    class="style5">Department : </td>
                <td style="text-align: left" class="style4">
                <asp:TextBox ID="txtDept" runat="server" Width="200px" 
                    style="font-weight: 700; background-color: #CCCCFF;" ReadOnly="True"></asp:TextBox></td>
            </tr>
            
            <tr>
                <td style="text-align: right; font-family: Arial; color: #003300; font-size: small;" 
                    class="style5">Location : </td>
                <td style="text-align: left" class="style4">
                <asp:TextBox ID="txtLocation" runat="server" Width="200px" 
                    style="font-weight: 700; background-color: #CCCCFF;" ReadOnly="True"></asp:TextBox></td>
            </tr>
            
            <tr>
                <td style="text-align: right; font-family: Arial; color: #0066CC; font-size: small;" 
                    class="style5">Email : </td>
                <td style="text-align: left" class="style4">
                <asp:TextBox ID="txtEmail" runat="server" Width="200px" 
                    style="font-weight: 700; color: #0066CC; background-color: #CCCCFF;" 
                        ReadOnly="True"></asp:TextBox></td>
            </tr>
            
            <tr>
                <td style="text-align: right; font-family: Arial; color: #003300; font-size: small;" 
                    class="style5">Phone : </td>
                <td style="text-align: left" class="style4">
                <asp:TextBox ID="txtphone" runat="server" Width="200px" 
                    style="font-weight: 700; background-color: #CCCCFF;" ReadOnly="True"></asp:TextBox></td>
            </tr>
            
            <tr>
                <td style="text-align: right" class="style5"></td>
                <td style="text-align: left" 
                    class="style4"></td>
            </tr>
            
            <tr>
                <td style="text-align: right; font-family: Arial; color: #003300; font-size: small; " 
                    class="style5">Leave Type : </td>
                <td style="text-align: left" class="style4">
                <asp:DropDownList ID="DropDownList1" runat="server" Height="22px" Width="204px">
                    <asp:ListItem>Casual</asp:ListItem>
                    <asp:ListItem>Privilege</asp:ListItem>
                    <asp:ListItem>Sick</asp:ListItem>
                </asp:DropDownList>
                </td>
            </tr>
            
            <tr>
                <td style="text-align: right; font-family: Arial; color: #003300; font-size: small; " 
                    class="style5">From Date : </td>
                <td style="text-align: left" class="style4">
                <asp:TextBox ID="txtFromDate" runat="server" Width="130px"></asp:TextBox>                       
                <asp:RegularExpressionValidator ID="regexpName" runat="server"     
                                ErrorMessage="This is not validate format." 
                                ControlToValidate="txtFromDate"     
                                
                        ValidationExpression="^(0[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)\d\d$" 
                        style="font-size: xx-small; font-family: Arial, Helvetica, sans-serif" />
                </td>
                <td style="font-family: Arial; font-size: x-small; color: #990033" 
                    class="style2">(dd/MM/yyyy)</td>
            </tr>  
              
            <tr>    
                <td style="text-align: right; font-family: Arial; color: #003300; font-size: small; " 
                    class="style5">To Date : </td>
                <td style="text-align: left" class="style4">
                <asp:TextBox ID="txtToDate" runat="server" Width="128px"></asp:TextBox>
                <asp:RegularExpressionValidator ID="regexpName1" runat="server"     
                                ErrorMessage="This is not validate format." 
                                ControlToValidate="txtToDate"     
                                
                        ValidationExpression="^(0[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)\d\d$" 
                        style="font-size: xx-small; font-family: Arial, Helvetica, sans-serif" />
                </td>
                <td style="font-family: Arial; font-size: x-small; color: #990033" 
                    class="style2">(dd/MM/yyyy)</td>
            </tr>
            
            <tr>
                <td style="text-align: right; font-family: Arial; font-size: small; color: #003300; " 
                    class="style5">Reason/Purpose of Leave : </td>
                <td style="text-align: left" class="style4"><asp:TextBox ID="txtPurpose" runat="server" Width="200px"></asp:TextBox></td>
            </tr>
            
            <tr>
                <td style="text-align: right; font-family: Arial; font-size: small; color: #003300; " 
                    class="style5">Address during leave period : </td>
                <td style="text-align: left" class="style4"><asp:TextBox ID="txtAdd" runat="server" Width="200px"></asp:TextBox></td>
            </tr>
            
            <tr>    
                <td style="text-align: right; font-family: Arial; font-size: small; color: #003300; " 
                    class="style5">Date of joining after leave : </td>
                <td style="text-align: left" class="style4">
                <asp:TextBox ID="txtJoinDate" runat="server" Width="128px"></asp:TextBox>
                <asp:RegularExpressionValidator ID="regexpName2" runat="server"     
                                ErrorMessage="This is not validate format." 
                                ControlToValidate="txtJoinDate"     
                                
                        ValidationExpression="^(0[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)\d\d$" 
                        style="font-family: Arial, Helvetica, sans-serif; font-size: xx-small" />
                </td>
                <td style="font-family: Arial; font-size: x-small; color: #990033" 
                    class="style2">(dd/MM/yyyy)</td>
            </tr>
            
            <tr>
                <td style="text-align: right;vertical-align:top; font-family: Arial; " 
                    class="style5">Remarks : </td>
                <td class="style4">
                <asp:TextBox  ID="txtMessage" runat="server" TextMode="MultiLine" Height="60px" 
                    Width="276px" style="text-align: left"></asp:TextBox></td>
            </tr>
            
            <tr>
            <td class="style5"></td>
            <td style="text-align: left" class="style4">
                <asp:Button ID="btnSend" runat="server" Text="Send" OnClick="btnSend_Click" Width="100px" /></td>
            </tr>
            
            <tr>
                <td class="style5"></td><td class="style4"></td>
            </tr>
            
        </table>
    
        <asp:Label ID="lblError" ForeColor="red" runat="server" Text=""></asp:Label>
        <p>&nbsp;</p>
        <p>&nbsp;</p>
        <p style="text-align: right">
            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/LogIn.aspx" 
                style="font-family: Arial, Helvetica, sans-serif; font-size: xx-small; text-align: right">Home</asp:HyperLink>
        </p>
    </div>
    
        
    
</asp:Content>
