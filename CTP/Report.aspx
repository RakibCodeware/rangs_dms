﻿<%@ Page Language="C#" MasterPageFile="Admin.master"
AutoEventWireup="true" CodeFile="Report.aspx.cs" Inherits="Report" %>

<%@ Register assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>


<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <script type="text/javascript">
        function Print() {
            var dvReport = document.getElementById("dvReport");
            var frame1 = dvReport.getElementsByTagName("iframe")[0];
            if (navigator.appName.indexOf("Internet Explorer") != -1 || navigator.appVersion.indexOf("Trident") != -1) {
                frame1.name = frame1.id;
                window.frames[frame1.id].focus();
                window.frames[frame1.id].print();
            } else {
                var frameDoc = frame1.contentWindow ? frame1.contentWindow : frame1.contentDocument.document ? frame1.contentDocument.document : frame1.contentDocument;
                frameDoc.print();
            }
        }  
        
function btnPrint_onclick() {

}

    </script> 

</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
        
    <h2 class="col-sm-12 bg-primary" style="padding:5px"> Report ...</h2>

    <input id="btnPrint" type="button" value="Print" onclick="Print()" onclick="return btnPrint_onclick()" />
    &nbsp;&nbsp;&nbsp;
    <asp:HyperLink ID="HyperLink1" runat="server" 
        NavigateUrl="~/CTP/Sales_New.aspx" Font-Names="Tahoma">Back to Sales Entry</asp:HyperLink>

    <div>
        <table style="border: 0px groove #008000;" width="810px">            
            <tr>
                <td>                    
                    <asp:Label ID="lblBillNo" runat="server" Text="0" Visible="False"></asp:Label>
                </td>
            </tr>
                        
            <tr>
                <td align="left">
                    
                    &nbsp;
                    </td>
            </tr>
            
        </table>
    </div>

    <div id="dvReport">
        <CR:CrystalReportViewer ID="crv" runat="server" 
            EnableDatabaseLogonPrompt="False" 
            EnableParameterPrompt="False" ToolPanelView="None" />
                     
    </div>

    <div>&nbsp;</div>
    <div>
        <asp:Button CssClass="buttonStyle" ID="btnCancel" runat="server" 
            Height="25px" Text="Close" 
            Font-Size="Small"
            Width="68px" TabIndex="9" onclick="btnCancel_Click" />                    
    </div>
    <div>&nbsp;</div>

</asp:Content>
