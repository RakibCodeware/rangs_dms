<%@ Page Language="C#" MasterPageFile="~/CTP_Admin.master" AutoEventWireup="true" 
CodeFile="Receive_List.aspx.cs" Inherits="Forms_Receive_List" %>


<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>



<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    
    
    <h2>
        Product Receive Challan List
    </h2>
    
            
    <div>        
        <br />      
        <table width="600px" align="center">
            <tr>
                <td colspan="2" align="center"
                    style="background-image:url(../Images/header.jpg); height:40px; font-family: Arial; font-size: large; text-decoration: blink;">                        
                    Waiting for Receive</td>
            </tr>
            <tr>
                <td colspan="2">
                <asp:GridView ID="gvEmployeeDetails" runat="server" AutoGenerateColumns="false" ShowFooter="true" Width="600px"
                     OnRowDataBound="gvEmployeeDetails_OnRowDataBound">
                    <Columns>
                        <asp:TemplateField ItemStyle-Width="20px">
                            <ItemTemplate>
                                <a href="JavaScript:divexpandcollapse('div<%# Eval("MRSRCode") %>');">
                                    <img id="imgdiv<%# Eval("MRSRCode") %>" width="9px" border="0" src="../Images/plus.gif"
                                        alt="" /></a>                        
                            </ItemTemplate>
                            <ItemStyle Width="20px" VerticalAlign="Middle"></ItemStyle>
                        </asp:TemplateField>
                        <asp:BoundField DataField="sno" HeaderText="SL#" />
                        <asp:TemplateField HeaderText="Challan #">
                        <ItemTemplate>
                         <asp:Label ID="lblCHNo" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "MRSRCode") %>'></asp:Label>
                        </ItemTemplate>
                        </asp:TemplateField>                
                        <asp:BoundField DataField="TDate" HeaderText="Date" />
                        <asp:BoundField DataField="eName" HeaderText="From" />                        
                        <asp:TemplateField>
                            <ItemTemplate>
                            <tr>
                            <td colspan="100%">
                                <div id="div<%# Eval("MRSRCode") %>"  style="overflow:auto; display:none; position: relative; left: 15px; overflow: auto">
                                <asp:GridView ID="gv_Child" runat="server" Width="95%" AutoGenerateColumns="false" DataKeyNames="MRSRCode"
                                OnRowDataBound="gv_Child_OnRowDataBound">
                                <Columns>
                                <asp:TemplateField ItemStyle-Width="20px">
                                    <ItemTemplate>
                                        <a href="JavaScript:divexpandcollapse('div1<%# Eval("ProductID") %>');">
                                            <img id="imgdiv1<%# Eval("ProductID") %>" width="9px" border="0" src="../Images/plus.gif"
                                                alt="" /></a>                        
                                    </ItemTemplate>
                                    <ItemStyle Width="20px" VerticalAlign="Middle"></ItemStyle>
                                </asp:TemplateField> 
                                <asp:TemplateField HeaderText="Product ID" Visible="false">
                                    <ItemTemplate>
                                     <asp:Label ID="lblProdID" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "ProductID") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField> 
                                <asp:BoundField DataField="Model" HeaderText="Model"/>
                                <asp:BoundField DataField="ProdName" HeaderText="Product Desc"/>
                                <asp:BoundField DataField="SLNO" HeaderText="Serial #"/>
                                <asp:BoundField DataField="tQty" HeaderText="Qty"/>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <tr>
                                            <td colspan="100%">
                                                <div id="div1<%# Eval("ProductID") %>"  style="overflow:auto; display:none; position: relative; left: 15px; overflow: auto">
                                                <asp:GridView ID="gv_NestedChild" runat="server" Width="95%" AutoGenerateColumns="false">
                                         
                                                    <Columns>
                                                    <asp:BoundField DataField="ProdName" HeaderText="Description"/>
                                                    
                                                    </Columns>
                                                    <HeaderStyle BackColor="#95B4CA" ForeColor="White" />
                                                </asp:GridView>
                                                </div> 
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                </Columns>
                                <HeaderStyle BackColor="#4D92C1" ForeColor="White" />
                                </asp:GridView>
                                </div> 
                            </td>
                            </tr>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <HeaderStyle BackColor="#0063A6" ForeColor="White" />
                </asp:GridView>     
                </td>
            </tr>
            <tr>
                <td colspan="2" style="background-image:url(../Images/header.jpg); height:30px"></td>
            </tr>
        </table> 
    </div>


</asp:Content>

