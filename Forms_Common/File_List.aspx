<%@ Page Language="C#" MasterPageFile="~/CTP_Admin.master"
AutoEventWireup="true" CodeFile="File_List.aspx.cs" Inherits="Forms_Common_File_List" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
        
    <style type="text/css">
       
    </style>
                       
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        Official Circular
    </h2>
    <p></p>
    
    <div align="center">
        
        <table style="border: 1px groove #008000" width="810px">
                        
            <tr>
                <td></td><td></td><td></td>
                <td class="style21"></td>
            </tr>
                        
            <tr>
                <td colspan="5" align="center"
                    style="background-image:url(../Images/header.jpg); height:30px; font-family: Arial; font-size: large; text-decoration: blink;">                        
                   Circular</td>
            </tr>

            <tr>
                <td></td><td></td><td></td>
                <td class="style21">&nbsp;</td>
            </tr>
     
                               
            <!-- Grid View -->                       
            <tr>
                <td></td>
                <td></td><td style="text-align: left">              
                &nbsp;</td>

                <td align="center">
                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" 
                        EmptyDataText = "No files uploaded" Width="402px" BackColor="#99CCFF"
		                Font-Names="Arial">
                            <Columns>
                                <asp:BoundField DataField="Text" HeaderText="File Name" />
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkDownload" Text = "Download" CommandArgument = '<%# Eval("Value") %>' runat="server" OnClick = "DownloadFile"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>                    
                            </Columns>
		                <FooterStyle BackColor="#FFCC00" BorderStyle="Outset" />
                            <HeaderStyle BackColor="#FF9999" />
                    </asp:GridView>
                </td>
            </tr>
            <!-- ---------------------------------- -->
                                                                                                                       
            <tr>
                <td></td>
                <td></td>
                <td>&nbsp;</td>
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





