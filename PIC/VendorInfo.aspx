<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VendorInfo.aspx.cs" Inherits="VendorInfo" 
    MasterPageFile="Admin.master"%>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    
        
    <br />

    <h1 class="page-header"> 
        <table width ="100%">
            <tr>
                <td align="left">Vendor Information</td>
                <td align="right"><a href="VendorInfo.aspx?action=add">Add Vendor</a></td>
            </tr>
        </table>        
    </h1>
    
    <asp:Label ID="errorMsg" runat="server" Text=""></asp:Label>


    <div class="table-responsive">

    <% if (Request.QueryString["action"] == null)
        { 
    %>
    

    <table width ="100%">
        <tr>
            <td align="left">
                <asp:RadioButtonList ID="RadioButtonList1" runat="server" Height="16px" 
                    RepeatDirection="Horizontal" Width="298px" AutoPostBack="True" 
                    onselectedindexchanged="RadioButtonList1_SelectedIndexChanged">
                    <asp:ListItem Selected="True">ALL</asp:ListItem>
                    <asp:ListItem>Own</asp:ListItem>
                    <asp:ListItem>Franchise</asp:ListItem>                    
                </asp:RadioButtonList>
            </td>
            <td align ="right">                
                <asp:TextBox ID="txtSearchByPhone" placeholder="Search by Vendor Name / Phone Number" 
                    CssClass="form-control" runat="server" Width="412px" 
                    >
                </asp:TextBox>
                
            </td>
            <td align ="right">  
                <asp:Button ID="btnSearch" runat="server" Text="Search" Height="30px" 
                    CssClass="btn btn-primary" onclick="btnSearch_Click" />
            </td>
        </tr>
                

    </table>

    <br />
    &nbsp;<br />

        
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
            EmptyDataText="There are no data records to display." AllowPaging="True"  
            DataKeyNames="VAID" CellPadding="4" ForeColor="#333333" 
            OnPageIndexChanging="GridView1_PageIndexChanging"
            OnRowDataBound="GridView1_OnRowDataBound"
            CssClass="table table-striped" GridLines="None" PageSize="100">
            <Columns>
                <asp:BoundField DataField="VAID" HeaderText="ID" ReadOnly="True"
                    SortExpression="VAID" Visible="False" />
                <asp:TemplateField HeaderText="SL#">
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                </asp:TemplateField> 

                <asp:BoundField DataField="VName" HeaderText="Vendor Name" SortExpression="VName" />
                <asp:BoundField DataField="VAddress" HeaderText="Address" SortExpression="VAddress" />  
                <asp:BoundField DataField="VContact" HeaderText="Mobile" SortExpression="VContact" />
                <asp:BoundField DataField="VRef" HeaderText="Reference" SortExpression="VRef" />                
                
                <asp:ImageField DataImageUrlField="VPhoto" HeaderText="Photo" DataImageUrlFormatString="../image/vendor/{0}"
                    NullImageUrl="../image/no-image.png">
                    <ControlStyle Width="30px" />
                    <ItemStyle Width="30px" />
                </asp:ImageField> 

                <asp:ImageField DataImageUrlField="VNID" HeaderText="NID/License" DataImageUrlFormatString="../image/vendor/doc/{0}"
                    NullImageUrl="../image/no-image.png">
                    <ControlStyle Width="30px" />
                    <ItemStyle Width="30px" />
                </asp:ImageField>
            
                <asp:HyperLinkField DataNavigateUrlFields="VAID" DataNavigateUrlFormatString="VendorInfo.aspx?action=edit&amp;id={0}"
                      HeaderText="Action" Text="Edit" >
                      <ControlStyle Width="30px" />
                      <ItemStyle Width="30px" />
                </asp:HyperLinkField>
                
                <asp:HyperLinkField DataNavigateUrlFields="VAID" DataNavigateUrlFormatString="VendorInfo.aspx?action=delete&amp;id={0}" Text="Delete" >
                    <ItemStyle Width="40px" />
                </asp:HyperLinkField>

                <asp:HyperLinkField DataNavigateUrlFields="VAID" DataNavigateUrlFormatString="" Text="View" >
                    <ItemStyle Width="40px" />
                </asp:HyperLinkField>                     

                              
            </Columns>
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#999999" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />

            <SelectedRowStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
                            <SortedAscendingCellStyle BackColor="#EDF6F6" />
                            <SortedAscendingHeaderStyle BackColor="#0D4AC4" />
                            <SortedDescendingCellStyle BackColor="#D6DFDF" />
                            <SortedDescendingHeaderStyle BackColor="#002876" />

        </asp:GridView>

        <div>
            <asp:Button ID="btnExport" runat="server" Text="Export To Excel" 
                OnClick = "ExportToExcel" Height="28px" Visible="True" Width="161px" 
                BackColor="#000099" Font-Size="X-Small" ForeColor="Aqua" /> 
        </div>  
        
        <br />

    <%
       
    }
       else if (Request.QueryString["action"] == "add" || Request.QueryString["action"] == "edit")
   { 
    %>
   <br />
   
   <h2 class="sub-header">Add</h2>
   <asp:TextBox ID="ID" runat="server" Text="" Visible="False"  ></asp:TextBox>

    <table class="table table-striped">
        
        <tr style="display:none">
            <td> Vendor Auto ID: </td>
            <td> <asp:TextBox ID="txtAID" runat="server" Width="50%"></asp:TextBox></td>
            
        </tr>

        <tr>
            <td>Vendor Type</td>
            <td>
                <asp:DropDownList ID="ddlType" CssClass="form-control" runat="server" Height="30px" Width="200px">
                    <asp:ListItem>Own</asp:ListItem>
                    <asp:ListItem>Franchise</asp:ListItem>                    
                </asp:DropDownList>
            </td>
        </tr>
        

        <tr>
            <td> Vendor Full Name </td>
            <td> <asp:TextBox ID="txtName" CssClass="form-control" runat="server" ></asp:TextBox></td>
            
        </tr>

        <tr>
            <td> Vendor Nick Name </td>
            <td> <asp:TextBox ID="txtNickName" CssClass="form-control" runat="server" ></asp:TextBox></td>
            
        </tr>

        <tr>
            <td> Vendor Mobile # </td>
            <td> <asp:TextBox ID="txtMobile" CssClass="form-control" runat="server" ></asp:TextBox></td>
            
        </tr>

        <tr>
            <td valign="top">Address</td>
            <td><asp:TextBox ID="txtAdd" CssClass="form-control" runat="server" TextMode="MultiLine" ></asp:TextBox></td>
        </tr>
        
        <tr>
            <td> NID Number </td>
            <td> <asp:TextBox ID="txtNIDNo" CssClass="form-control" runat="server" ></asp:TextBox></td>            
        </tr>
                
        <tr>
            <td> Reference </td>
            <td> <asp:TextBox ID="txtRef" CssClass="form-control" runat="server" ></asp:TextBox></td>            
        </tr>

        <!-- ----------------------------------------------------------------- -->
        <tr style ="display:none">
            <td> Customer Date of Birth </td>
            <td> 
                <div class="form-group">
                    <label for="lblDOI" class="col-sm-offset-0 col-sm-0 control-label" style="font-size:12px;"></label>
                    <div class="col-sm-2">
                        <asp:DropDownList ID="ddlDay" CssClass="form-control" runat="server"> </asp:DropDownList>                    
                    </div>
                    <div class="col-sm-2">
                        <asp:DropDownList ID="ddlMonth" CssClass="form-control" runat="server"> </asp:DropDownList>                    
                    </div>
                    <div class="col-sm-2">
                        <asp:DropDownList ID="ddlYear" CssClass="form-control" runat="server"> </asp:DropDownList>                    
                    </div>
                </div>
                
                <asp:TextBox ID="txtDOB" CssClass="form-control" runat="server" 
                    Visible="False" ></asp:TextBox>
            </td>
            
        </tr>
        <!-- ----------------------------------------------------------------- -->


        <tr style="display : none">
            <td> Sex </td>
            <td> 
                <asp:RadioButtonList ID="optSex" runat="server" Height="16px" 
                    RepeatDirection="Horizontal" Width="298px">
                    <asp:ListItem Selected="True">Male</asp:ListItem>
                    <asp:ListItem>Female</asp:ListItem>
                    <asp:ListItem>Others</asp:ListItem>
                </asp:RadioButtonList>
            </td>
            
        </tr>

        <!-- -------------------------------------------------------------------------------- -->
        <tr style="display : none">
            <td>Order</td>
            <td><asp:TextBox ID="order" runat="server"></asp:TextBox></td>
        </tr>
        

        <tr> 
            <td style="width: 100px">
                Vendor Photo:</td>
            <td style="width: 100px">                    
                <asp:FileUpload ID="ImageUpload" runat="server" />
                (Dimension: 450px x 570px & Size: maximum 500kb)
            </td>
        </tr>
        
        <tr>
            <td style="width: 100px">
                Document Image (NID/Trade License):</td>
            <td style="width: 100px">                    
                <asp:FileUpload ID="ImageUpload2" runat="server" />
                (Size: maximum 500kb)
            </td>
        </tr>
        <!-- -------------------------------------------------------------------------------- -->

        <tr>
            <td>Status </td>
            <td><asp:CheckBox ID="status" Text="Yes"  runat="server" Checked="True"></asp:CheckBox></td>
        </tr>

        <tr>
            <td>
            </td>
            
            <td>
            <asp:HiddenField ID="parent" Value="0" runat="server"/>
            <asp:Button ID="btnSave" CssClass="btn btn-primary" runat="server" Text="Save" OnClick="btnSave_Click" />
            <asp:Button ID="btnCancel" CssClass="btn btn-primary" runat="server" Text="Cancel" onclick="btnCancel_Click" />
            </td>
        </tr>
    </table>
   <%
   
   } %>

   
    </div>

    

    <script type="text/javascript">
        $('.jqte-editor').jqte();
    </script>

</asp:Content>
