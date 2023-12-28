<%@ Page Language="C#" MasterPageFile="~/CTP_Admin.master" AutoEventWireup="true" 
CodeFile="View_ProductInfo.aspx.cs" Inherits="Forms_Common_View_ProductInfo" %>


<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
        
    <style type="text/css">
           
        .mGrid { 
            width: 100%; 
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
                        
    </style>
                       
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        Product Information
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
                   Product Information</td>
            </tr>

            <tr>
                <td></td><td></td><td></td>
                <td class="style21">&nbsp;</td>
            </tr>

            <tr>
                <td></td>
                <td></td><td style="text-align: left">
              
                &nbsp;</td>
                <td class="style21" align="left">   
                    Select Product Group :   
                    <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="True" 
                        onselectedindexchanged="DropDownList1_SelectedIndexChanged"
                        Height="24px" Width="283px">
                    </asp:DropDownList>                
                </td>
            </tr>


            <tr>
                <td></td>
                <td></td>
                <td>&nbsp;</td>
                <td class="style21">&nbsp;</td>
            </tr>
                             
                               
            <!-- Grid View -->                       
            <tr>
                <td></td>
                <td></td><td style="text-align: left">              
                &nbsp;</td>

                <td>
                    <asp:GridView ID="gvCustomres" runat="server"                        
                        AutoGenerateColumns="False"
                        GridLines="None"
                        AllowPaging="false"
                        CssClass="mGrid"
                        PagerStyle-CssClass="pgr"                                               
                        AlternatingRowStyle-CssClass="alt" 
                        OnRowDataBound="gvCustomres_RowDataBound"
                        OnRowCreated="gvCustomres_RowCreated"
                        >
                        <SelectedRowStyle BackColor="BurlyWood"/>
                        <Columns>
                            <asp:BoundField DataField="Code" HeaderText="Product Code" />
                            <asp:BoundField DataField="Model" HeaderText="Product Model" />
                            <asp:BoundField DataField="ProdName" HeaderText="Product Description" />
                            <asp:BoundField DataField="UnitPrice" HeaderText="MRP (Tk.)" />
                            <asp:BoundField DataField="CampPrice" HeaderText="Campaign Price (Tk.)" />                            
                            <asp:BoundField DataField="GroupName" HeaderText="Product Group" />
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <!-- ---------------------------------- -->
                     
            <tr>
                <td></td>
                <td style="text-align: Left; font-family: Arial; font-weight: 700; color: ##003300;">
                    &nbsp;</td>
                <td> &nbsp;</td>              
                <td style="text-align: left" class="style21" >  
                                                      
                    <asp:Button ID="btnExport" runat="server" Height="25px" Text="Export To Excel" 
                        width="127px" onclick="btnExport_Click" TabIndex="7" 
                        Font-Size="X-Small"
                        ToolTip="Click here for Export data..." BackColor="#000099" 
                        BorderColor="White" Font-Overline="False" Font-Strikeout="False" 
                        Font-Underline="False" ForeColor="Aqua"                        
                        />
                        &nbsp;</td>



                <td>
                    &nbsp;</td>
            </tr>

                                                                                                          
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




