<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CustomerInfo.aspx.cs" Inherits="CTP_CustomerInfo" 
    MasterPageFile="Admin.master"%>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .modalBackground
        {
            background-color: Black;
            filter: alpha(opacity=90);
            opacity: 0.8;
        }
        .modalPopup
        {
            background-color: #FFFFFF;
            border-width: 3px;
            border-style: solid;
            border-color: black;
            padding-top: 10px;
            padding-left: 10px;
            width: 300px;
            height: 140px;
        }
    </style>
        
    <br />

    <h1 class="page-header"> 
        <table width ="100%">
            <tr>
                <td align="left">Customer Information</td>
                <td align="right"><a href="CustomerInfo.aspx?action=add">Add Customer</a></td>
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
                    <asp:ListItem>Male</asp:ListItem>
                    <asp:ListItem>Female</asp:ListItem>
                    <asp:ListItem>Others</asp:ListItem>
                </asp:RadioButtonList>
            </td>
            <td align ="right">                
                <asp:DropDownList ID="DropDownList1" runat="server" Height="30px"  
                    Width="367px" ToolTip="Select Customer Type">
                    <asp:ListItem>ALL</asp:ListItem>
                    <asp:ListItem>Regular</asp:ListItem>
                    <asp:ListItem>Corporate</asp:ListItem>
                    <asp:ListItem>Others</asp:ListItem>
                </asp:DropDownList>
                
            </td>
        </tr>
        <tr>
            <td align="left">
                <asp:TextBox ID="txtSearchByPhone" placeholder="Search by Customer Name / Phone Number" 
                    CssClass="form-control" runat="server" Width="412px" 
                    AutoPostBack="True" ontextchanged="txtSearchByPhone_TextChanged"></asp:TextBox>
                <asp:Button ID="btnGo1" runat="server" Text="Go" onclick="btnGo1_Click" 
                    Visible="False" />
            </td>
            <td align ="right">                
                <asp:TextBox ID="txtSearchByName" placeholder="Search by Customer Name" 
                    CssClass="form-control" runat="server" Width="412px" Visible="False"></asp:TextBox>

                <asp:DropDownList ID="ddlCTP" runat="server" Height="30px"  Width="367px" 
                    ToolTip="Select CTP">                    
                </asp:DropDownList>
                
            </td>
        </tr>
        
        <tr>
            <td align="left">
                
            </td>
            <td align ="right">  
                <asp:DropDownList ID="ddlCityS" runat="server" Height="30px"  Width="367px" 
                    ToolTip="Select Customer City">                    
                </asp:DropDownList>
            </td>
        </tr>

        <tr>
            <td align="left">
                
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
            DataKeyNames="CustAID" CellPadding="4" ForeColor="#333333" 
            OnPageIndexChanging="GridView1_PageIndexChanging"
            OnRowDataBound="GridView1_OnRowDataBound"
            CssClass="table table-striped" GridLines="None" PageSize="100">
            <Columns>
                <asp:BoundField DataField="CustAID" HeaderText="ID" ReadOnly="True"
                    SortExpression="CustAID" Visible="False" />
                <asp:TemplateField HeaderText="SL#">
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                </asp:TemplateField> 

                <asp:BoundField DataField="CustName" HeaderText="Customer Name" SortExpression="CustName" />
                <asp:BoundField DataField="Address" HeaderText="Address" SortExpression="Address" />            
                <asp:BoundField DataField="City" HeaderText="City" SortExpression="City" />
                <asp:BoundField DataField="Mobile" HeaderText="Mobile" SortExpression="Mobile" />
                <asp:BoundField DataField="CustSex" HeaderText="Sex" SortExpression="CustSex" />
                <asp:BoundField DataField="CustType" HeaderText="Type" SortExpression="CustType" />
                 
            
                <asp:HyperLinkField DataNavigateUrlFields="CustAID" DataNavigateUrlFormatString="CustomerInfo.aspx?action=edit&amp;id={0}"
                      HeaderText="Action" Text="Edit" >
                      <ControlStyle Width="30px" />
                      <ItemStyle Width="30px" />
                </asp:HyperLinkField>
                
                <asp:HyperLinkField DataNavigateUrlFields="CustAID" DataNavigateUrlFormatString="CustomerInfo.aspx?action=view&amp;id={0}"
                      HeaderText="Action" Text="Details" >
                      <ControlStyle Width="30px" />
                      <ItemStyle Width="30px" />
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
            <td> Customer Auto ID: </td>
            <td> <asp:TextBox ID="txtAID" runat="server" Width="50%"></asp:TextBox></td>
            
        </tr>

        <tr>
            <td>Customer Type</td>
            <td>
                <asp:DropDownList ID="ddlType" runat="server" Height="30px" Width="200px">
                    <asp:ListItem>Regular</asp:ListItem>
                    <asp:ListItem>Corporate</asp:ListItem>
                    <asp:ListItem>Others</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        

        <tr>
            <td> Customer Name </td>
            <td> <asp:TextBox ID="txtName" CssClass="form-control" runat="server" ></asp:TextBox></td>
            
        </tr>

        <tr>
            <td> Customer Mobile # </td>
            <td> <asp:TextBox ID="txtMobile" CssClass="form-control" runat="server" ></asp:TextBox></td>
            
        </tr>

        <tr>
            <td valign="top">Address</td>
            <td><asp:TextBox ID="txtAdd" CssClass="form-control" runat="server" TextMode="MultiLine" ></asp:TextBox></td>
        </tr>
        
        <tr>
            <td> City </td>
            <td> 
                <asp:DropDownList ID="ddlCity" CssClass="form-control" runat="server">
                </asp:DropDownList>
            </td>
            
        </tr>

        <tr>
            <td> Email </td>
            <td> <asp:TextBox ID="txtEmail" CssClass="form-control" runat="server" ></asp:TextBox></td>
            
        </tr>

        <tr>
            <td> Customer Profession </td>
            <td> <asp:TextBox ID="txtProfession" CssClass="form-control" runat="server" ></asp:TextBox></td>
            
        </tr>
        <tr>
            <td> Organization </td>
            <td> <asp:TextBox ID="txtOrg" CssClass="form-control" runat="server" ></asp:TextBox></td>
            
        </tr>
        <tr>
            <td> Designation </td>
            <td> <asp:TextBox ID="txtDesg" CssClass="form-control" runat="server" ></asp:TextBox></td>
            
        </tr>
        <tr>
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
        <tr>
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
        <tr style="display : none">
            <td>Status </td>
            <td><asp:CheckBox ID="status" Text="Yes"  runat="server" Checked="True"></asp:CheckBox></td>
        </tr>

        <tr style="display : none"> 
            <td style="width: 100px">
                Thum:</td>
            <td style="width: 100px">                    
                <asp:FileUpload ID="ThumUpload" runat="server" />
                (Size: 260 X 260 pixel)
            </td>
        </tr>
        
        <tr style="display : none">
            <td style="width: 100px">
                Image/Banner:</td>
            <td style="width: 100px">                    
                <asp:FileUpload ID="ImageUpload" runat="server" />
                (Size: 1140 X 480 pixel)
            </td>
        </tr>
        <!-- -------------------------------------------------------------------------------- -->


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
        } 
        
    else if (Request.QueryString["action"] == "view")
   { 
    %>

    <!-- ------------------------------------------------------------------------- -->
    
    <!--
    <table width="100%"  cellpadding="0" cellspacing="0">
        -->
        <table class="table table-striped">        
                <tr>
                    <td colspan="9" 
                        style=" height:6%; color:black; font-weight:bold; font-size:x-large; font-family: Tahoma;" 
                        align="center">Sales Details </td>
                </tr>
                
                <tr>
                    <td>&nbsp;</td>
                    <td></td>
                    <td></td>
                    <td>&nbsp;</td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td>&nbsp;&nbsp;</td>
                </tr>

                <tr>
                    <td width="2%"></td>
                    <td align="left" style=" width:20%"> </td>
                    <td width="2%"></td>
                    <td align="left" style=" width:30%">
                        <asp:Label ID="lblID" runat="server" Visible="False"></asp:Label>
                    </td>
                    <td align="left" style=" width:2%">&nbsp;</td>
                    <td align="left" style=" width:15%"></td>
                    <td width="2%"></td>
                    <td align="left" style=" width:28%"></td>
                    <td width="4%"></td>
                </tr>
                
                
                <tr>
                    <td></td>
                    <td align="left" style="color: #000000;">Customer Name</td>
                    <td align="center">:</td>
                    <td align="left" style="color: #000080; ">
                        <asp:Label ID="lblCustName" runat="server" ></asp:Label>
                    </td>

                    <td align="left" style=" width:2%"></td>
                    <td align="left" style="color: #000000;">Contact # </td>
                    <td align="center">:</td>
                    <td align="left" style="color: #000080; ">
                        <asp:Label ID="lblContact" runat="server" ></asp:Label>
                    </td>
                </tr>

               
                <tr>
                    <td></td>
                    <td align="left" style="color: #000000; ">Address</td>
                    <td align="center">:</td>
                    <td align="left" style="color: #000080; ">
                        <asp:Label ID="lblAdd" runat="server" ></asp:Label>
                    </td>
                    <td align="left" style=" width:2%"></td>
                    <td align="left" style="color: #000000; ">Sex</td>
                    <td align="center">:</td>
                    <td align="left" style="color: #000080; ">
                        <asp:Label ID="lblSex" runat="server" ></asp:Label>
                    </td>
                </tr>

               
                <tr>
                    <td></td>
                    <td align="left" style="color: #000000;">Profession</td>
                    <td align="center">:</td>
                    <td align="left" style="color: #000080; ">
                        <asp:Label ID="lblProfession" runat="server"></asp:Label>
                    </td>
                    <td align="left" style=" width:2%"></td>
                    <td align="left" style="color: #000000; ">Email</td>
                    <td align="center">:</td>
                    <td align="left" style="color: #000080; ">
                        <asp:Label ID="lblEmail" runat="server"></asp:Label>
                    </td>
                </tr>
                               
                <tr>
                    <td></td>
                    <td align="left" style="color: #000000;">Organization</td>
                    <td align="center">:</td>
                    <td align="left" style="color: #000080;">
                        <asp:Label ID="lblOrg" runat="server"></asp:Label>
                    </td>
                    <td align="left" style=" width:2%"></td>
                    <td align="left" style="color: #000000;">Designation</td>
                    <td align="center">:</td>
                    <td align="left" style="color: #000080;">
                        <asp:Label ID="lblDesg" runat="server"></asp:Label>
                    </td>
                </tr>
                              

                <tr>
                    <td></td>
                    <td align="left" style="color: #000000;">City</td>
                    <td align="center">:</td>
                    <td align="left" style="color: #000080;">
                        <asp:Label ID="lblCity" runat="server"></asp:Label>
                    </td>
                    <td align="left" style=" width:2%"></td>
                    <td align="left" style="color: #000000;">Location</td>
                    <td align="center">:</td>
                    <td align="left" style="color: #000080;">
                        <asp:Label ID="lblLoc" runat="server"></asp:Label>
                    </td>
                </tr>

                <tr>
                    <td></td>
                    <td align="left" style="color: #000000;">Date of Birth</td>
                    <td align="center">:</td>
                    <td align="left" style="color: #000080;">
                        <asp:Label ID="lblDOB" runat="server"></asp:Label>
                    </td>
                    <td align="left" style=" width:2%"></td>
                    <td align="left" style="color: #000000;">Age</td>
                    <td align="center">:</td>
                    <td align="left" style="color: #000080;">
                        <asp:Label ID="lblAge" runat="server"></asp:Label>
                    </td>
                </tr>


                <!-- GRID VIEW -->
                <tr>
                    <td></td>
                    <td align="left" colspan ="7" style=" width:100%">
                        <asp:GridView ID="gvUsers" runat="server"
                            AutoGenerateColumns="false"                        
                            CssClass="mGrid"
                            DataKeyNames="MRSRMID"
                            EmptyDataText = "No product in list !!! Please select model and add in list."
                            EmptyDataRowStyle-CssClass ="gvEmpty"                            
                            ShowFooter="true"                                                  
                            GridLines="none" Width="100%">
                            <FooterStyle Font-Bold="true" BackColor="#61A6F8" ForeColor="black" />
                            <Columns>
                                <asp:BoundField HeaderText="Invoice#" DataField="MRSRCode"/>                        
                                <asp:BoundField HeaderText="Date" DataField="TDate" />                       
                                <asp:BoundField HeaderText="Sales From" DataField="eName" />  
                                <asp:BoundField HeaderText="Model" DataField="Model" />                     
                                <asp:BoundField HeaderText="Unit Price" DataField="UnitPrice" />
                                <asp:BoundField HeaderText="Qty" DataField="Qty" ItemStyle-Width="5px"/>
                                <asp:BoundField HeaderText="Total Price" DataField="TotalAmnt" />
                                <asp:BoundField HeaderText="Dis Amnt" DataField="DiscountAmnt" />                                
                                <asp:BoundField HeaderText="With/Adj Amnt" DataField="WithAdjAmnt" />
                                <asp:BoundField HeaderText="NetAmnt" DataField="NetAmnt"/>  
                                                                              
                                <asp:BoundField HeaderText="Product SL" DataField="SLNO" />
                                <asp:BoundField HeaderText="Remarks" DataField="ProdRemarks"  />
                                        
                            </Columns>
                            <EmptyDataRowStyle CssClass="gvEmpty" />
                        </asp:GridView>
                    </td>
                    <td></td>
                </tr>


                                              
                <!-- ********************************************* -->

                <tr>
                    <td>&nbsp;</td>
                    <td></td>
                    <td></td>
                    <td>&nbsp;</td>
                    <td></td>
                    <td></td>
                    <td>&nbsp;</td>
                    <td></td>
                    <td></td>
                </tr>

                <tr>
                    <td></td>                    
                    <td colspan="4">                        
                        <asp:Button ID="Button1" CssClass="btn btn-primary"  Width="100px" 
                            runat="server" Text="Ok" onclick="Button1_Click" />
                    </td>
                    <td>&nbsp;</td>
                    <td></td>
                    <td></td>
                    <td></td>                    
                </tr>

                <tr>
                    <td>&nbsp;</td>
                    <td></td>
                    <td></td>
                    <td>&nbsp;</td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                </tr>

            </table>
    <!-- ------------------------------------------------------------------------- -->

        
    
    <%
   
   } %>
   
    </div>

    

    <script type="text/javascript">
        $('.jqte-editor').jqte();
    </script>

</asp:Content>
