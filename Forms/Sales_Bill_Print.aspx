<%@ Page Language="C#" MasterPageFile="~/Admin.master"
AutoEventWireup="true" CodeFile="Sales_Bill_Print.aspx.cs" Inherits="Forms_Sales_Bill_Print" %>

<%@ Register assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>


<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        Customer Bill
    </h2>
    
    <div>
        <table style="border: 0px groove #008000;" width="810px">            
            <tr>
                <td>                    
                    <asp:Label ID="lblBillNo" runat="server" Text="0" Visible="False"></asp:Label>
                </td>
            </tr>
                        
            <tr>
                <td align="left">
                    <asp:Button CssClass="buttonStyle" ID="btnCancel" runat="server" 
                        Height="25px" Text="Close" 
                        Font-Size="Small"
                        Width="68px" TabIndex="9" onclick="btnCancel_Click" />                    
                </td>
            </tr>
            
        </table>
    </div>

    <div>
        <CR:CrystalReportViewer ID="crv" runat="server" AutoDataBind="true" 
            BorderStyle="Ridge" 
            ReuseParameterValuesOnRefresh="True"
            GroupTreeImagesFolderUrl="" Height="50px"
            ToolPanelWidth="200px" Width="300px"
            ToolbarImagesFolderUrl=""            
            ToolPanelView="None" CssClass="ReportViewerStyle"/>
                         
        
        <CR:CrystalReportSource ID="CrystalReportSource1" runat="server">
                <Report FileName="~/Reports/Bill_N.rpt">
                </Report>
        </CR:CrystalReportSource>

    </div>

    

</asp:Content>
