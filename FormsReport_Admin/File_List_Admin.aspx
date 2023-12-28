<%@ Page Language="C#" MasterPageFile="~/Admin_Report.master"
AutoEventWireup="true" CodeFile="File_List_Admin.aspx.cs" Inherits="FormsReport_Admin_File_List" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
        
    <style type="text/css">
        .style19
        {
            width: 1px;
        }
        .style20
        {
            width: 16px;
        }
        .style21
        {
            width: 21px;
        }
        .style22
        {
            width: 6px;
        }
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
                <td class="style22"></td>
            </tr>
                        
            <tr>
                <td colspan="1" align="center"
                    style="background-image:url(../Images/header.jpg); height:30px; font-family: Arial; font-size: large; text-decoration: blink;">                        
                    Price                        
                   Circular &amp; Others</td>
            </tr>

            <tr>
                <td class="style22"></td>
            </tr>
     
                               
            <!-- Grid View -->                       
            <tr>
                
                <td align="center">
                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" 
                        EmptyDataText = "No files uploaded" Width="450px" BackColor="#99CCFF"
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
                <td class="style22"></td>                
            </tr>
           
            
        </table>
        
        
        <div>
        <asp:Label ID="lblError" ForeColor="red" runat="server" Text=""></asp:Label>
        </div>
        
        <p>&nbsp;</p>
        <p>&nbsp;</p>
        
   </div>

</asp:Content>





