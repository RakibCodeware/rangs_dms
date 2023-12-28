<%@ Page Language="C#" MasterPageFile="Admin.master" AutoEventWireup="true" 
CodeFile="Receive_New.aspx.cs" Inherits="Forms_Receive_New" %>


<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .style19
        {
            width: 92px;
        }
        .style20
        {
            width: 53px;
        }
        .style22
        {
            height: 1px;
        }
    </style>
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

    <h4 class="col-sm-12 bg-primary" style="padding:5px"> Product Receive (New Entry) ...</h4>
    <p>&nbsp;</p>

    
    <div>
        <table width="100%" style="font-family: Tahoma; font-size: small">
                        
            <tr>
                <td class="style20"></td>
                <td align="left" class="style19">                
                    <strong>Challan # </strong>  
                </td>
                <td align="center"> : </td>
                <td>   
                    <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="True" 
                        onselectedindexchanged="DropDownList1_SelectedIndexChanged"
                        Height="24px" Width="220px">
                    </asp:DropDownList>                
                </td>
                <td>
                    <asp:TextBox ID="txtCHNo" runat="server" Visible="False" Width="73px"></asp:TextBox>
                </td>
                <td></td>
                <td></td>
           </tr>
                      
           <tr>
               <td colspan="7" align="center">&nbsp;</td>
           </tr>
                       
           <tr> 
                <td colspan="7" align="center">                    
                    <asp:GridView ID="GridView1" runat="server" CellPadding="4" Font-Bold="True" 
                        Font-Names="Verdana" Font-Size="Small" ForeColor="#333333" GridLines="None"
                        EmptyDataText = "No record found!" Width="90%"
                        ShowFooter="true"  OnRowDataBound="GridView1_RowDataBound">
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                        <EditRowStyle BackColor="#999999" />
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <SortedAscendingCellStyle BackColor="#E9E7E2" />
                        <SortedAscendingHeaderStyle BackColor="#506C8C" />
                        <SortedDescendingCellStyle BackColor="#FFFDF8" />
                        <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                        <FooterStyle Font-Bold="true" BackColor="#61A6F8" ForeColor="black" />
                    </asp:GridView>                
                </td>
            </tr>
            
            <tr>
                <td colspan="7" align="center"                    
                    style="background-image:url('../Images/header.jpg'); height:0.5px; font-family: Arial;">                        
                </td>
            </tr>
             
            <tr>
                <td colspan="7" align="center" class="style22">&nbsp;</td>
            </tr>

            <tr>
                <td colspan="7" align="center">                    
                    <asp:Button ID="btnReceive" runat="server"  Text="Receive" 
                        width="88px" onclick="btnReceive_Click" TabIndex="14" 
                        Font-Size="Small" CssClass="btn btn-primary"
                        ToolTip="Click here for save data..."/>
                        &nbsp;                   
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" 
                        Width="88px" TabIndex="16" onclick="btnCancel_Click"
                        Font-Size="Small" CssClass="btn btn-primary"
                        ToolTip="Click here for cancel data..."/>                    
                </td>
            </tr>
            
            <tr>
                <td colspan="7" align="center">&nbsp;</td>
            </tr>

        </table>
    </div>

</asp:Content>


