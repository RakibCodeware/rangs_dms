<%@ Page Language="C#" AutoEventWireup="true" CodeFile="File_Uploadn.aspx.cs" Inherits="Admin_File_Uploadn" MasterPageFile="Admin.master"%>

<%@ Register namespace="AjaxControlToolkit" tagprefix="AjaxControlToolkit" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %> 
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server" >

    <script type="text/javascript" language="javascript">
        function confirmmsg() {
            if (confirm('Do you want to save ?')) { return true; } else { return false; }
        }    
    </script>

    <h4 class="col-sm-12 bg-primary" style="padding:5px">File/Document Upload</h4>

    <div align="center">         
        <asp:FileUpload ID="FileUpload1" runat="server" />
        <asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="UploadFile" />
    </div>
        
    <div align="center"> 
        
            <asp:GridView ID="GridView1" runat="server" CssClass="table" AutoGenerateColumns="false" 
                EmptyDataText = "No files uploaded" Width="100%" BackColor="#99CCFF">
                
                <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />        
                <HeaderStyle CssClass="bg-primary" BackColor="#009933" />
                <AlternatingRowStyle BackColor="Gainsboro" />

                <Columns>
                    <asp:BoundField DataField="Text" HeaderText="File Name" />
                    
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID = "lnkDelete" Text = "Delete" CommandArgument = '<%# Eval("Value") %>' runat = "server" OnClick = "DeleteFile" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        
    </div>

    <div>&nbsp;</div>           

</asp:Content>