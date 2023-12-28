<%@ Page Language="C#" MasterPageFile="Admin.master"
AutoEventWireup="true" CodeFile="Employee_Info.aspx.cs" Inherits="Admin_Forms_Employee_Info" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
                               
    <style type="text/css">

        .style20
        {
            width: 13px;
        }
        .style21
        {
            width: 291px;
        }
        .style30
        {
            width: 176px;
            height: 29px;
        }
        .style28
        {
            width: 291px;
            height: 29px;
        }
        .mGrid
        {}
        </style>
          
    <style type="text/css">
           
         .mGrid { 
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
                        
         .style34
         {
             width: 131px;
         }
         .style35
         {
             width: 131px;
             height: 29px;
         }
         .style36
         {
             width: 176px;
         }
                        
    </style>    
                          
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    
    <p>&nbsp;</p>
        
    <div align="center">
        
        <table width="100%" style="font-family: Tahoma; font-size: small">
            <tr>
                <td colspan="5" align="center"                    
                    style="background-image:url('../../Images/header.jpg'); height:30px; font-family: Arial; font-size: large; text-decoration: blink;">                        
                    Employee Information 
                </td>
            </tr>

            <tr>
                <td class="style34">
                    <asp:TextBox ID="txtProdID" runat="server" Width="6px" 
                        style="font-weight: 700" Visible="False"></asp:TextBox> 
                </td>
                <td class="style36"></td>
                <td class="style20">&nbsp;</td>
                <td class="style21">&nbsp;</td>
            </tr>
            
            <!-- Employee Job ID -->
            <tr>
                <td class="style34"></td>
                <td style="text-align: Left; font-family: Arial; font-weight: 700; color: #009900;" 
                    class="style36">Employee Job ID</td>
                <td class="style20"> : </td>
                <td style="text-align: left" class="style21">
                <asp:TextBox ID="txtJobID" runat="server" Width="193px" 
                    style="font-weight: 700" BackColor="#FFFFCC" Enabled="True" MaxLength="20" 
                        TabIndex="1" ></asp:TextBox>
                </td>
                <td>
                    &nbsp;</td>
            </tr>

            <!-- Employee Name -->
            <tr>
                <td class="style34"></td>
                <td style="text-align: Left; font-family: Arial; font-weight: 700; color: #009900;" 
                    class="style36">Employee Name</td>
                <td class="style20"> : </td>
                <td style="text-align: left" class="style21">
                <asp:TextBox ID="txtEmpName" runat="server" Width="193px" 
                    style="font-weight: 700" BackColor="#FFFFCC" Enabled="True" MaxLength="20" 
                        TabIndex="2" ></asp:TextBox>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            
            <!-- Employee Address -->
            <tr>
                <td class="style35"></td>
                <td style="text-align: left; font-family: Arial; font-size: small; " 
                    class="style30">Employee Address</td>
                <td class="style20"> : </td>
                <td style="text-align: left" class="style28">
                <asp:TextBox ID="txtAdd" runat="server" Width="300px" TabIndex="3" 
                        ToolTip="Please Enter Product Model..." MaxLength="240" 
                        TextMode="MultiLine"></asp:TextBox>
                </td>
                
                <td style="font-family: Arial; font-size: x-small; color: #990033" 
                    class="style2">&nbsp;</td>
            </tr>

            <!-- Contact No -->
            <tr>
                <td class="style35"></td>
                <td style="text-align: left; font-family: Arial; font-size: small; " 
                    class="style30">Contact Number</td>
                <td class="style20"> : </td>
                <td style="text-align: left" class="style28">
                    <asp:TextBox ID="txtContactNo" runat="server" Width="193px" TabIndex="4" 
                        onkeypress="return numeric_only(event)"
                        ToolTip="Please Enter Product Unit Price ..." MaxLength="11" ></asp:TextBox>
                </td>
                
                <td style="font-family: Arial; font-size: x-small; color: #990033" 
                    class="style2">&nbsp;</td>
            </tr>

            
            <!-- Employee Designation -->
            <tr>
                <td class="style35"></td>
                <td style="text-align: left; font-family: Arial; font-size: small; " 
                    class="style30">Designation</td>
                <td class="style20"> : </td>
                <td style="text-align: left" class="style28">
                <asp:TextBox ID="txtDesg" runat="server" Width="193px" TabIndex="5" 
                        ToolTip="Please Enter Employee Designation ..." MaxLength="45" ></asp:TextBox>
                </td>
                
                <td style="font-family: Arial; font-size: x-small; color: #990033" 
                    class="style2">&nbsp;</td>
            </tr>                   
                 
            <!-- Department -->
            <tr>
                <td class="style35"></td>
                <td style="text-align: left; font-family: Arial; font-size: small; " 
                    class="style30">Department</td>
                <td class="style20"> : </td>
                <td style="text-align: left" class="style28">
                <asp:TextBox ID="txtDept" runat="server" Width="193px" TabIndex="6" 
                        ToolTip="Please Enter Employee Department ..." MaxLength="45" ></asp:TextBox>
                </td>
                
                <td style="font-family: Arial; font-size: x-small; color: #990033" 
                    class="style2">&nbsp;</td>
            </tr>
            
            <!-- Branch -->
            <tr>
                <td class="style35"></td>
                <td style="text-align: left; font-family: Tahoma; font-size: small; color: #008000; font-weight: bold;" 
                    class="style30">Branch</td>
                <td class="style20"> : </td>
                <td class="style35" align="left">
                    <asp:DropDownList ID="ddlBranch1" runat="server" Height="26px" 
                        BackColor="#F6F1DB"
                        ToolTip="Please Select Receive By ..." AutoPostBack="True"
                        Width="195px" onselectedindexchanged="ddlBranch1_SelectedIndexChanged">
                        <asp:ListItem Text = "--Select--" Value = ""></asp:ListItem>
                    </asp:DropDownList>
                </td>
                
                <td style="font-family: Arial; font-size: x-small; color: #990033" 
                    class="style2">&nbsp;</td>
            </tr>
            
            <tr>
                <td class="style34"></td>
                <td class="style36" align ="left">Branch Code</td><td style="text-align: left" class="style20">              
                &nbsp;</td>
                <td style="text-align: left" class="style28">                    
                      <asp:TextBox ID="txtBrCode" runat="server" Height="20px" Width="195px" BackColor="#FFCCFF" 
                      BorderStyle="None" ReadOnly="True"></asp:TextBox>
                </td>
            </tr>
                 
            <tr>
                <td class="style34">
                    <asp:TextBox ID="txtEID" runat="server" Width="16px" 
                        style="font-weight: 700" Visible="False"></asp:TextBox> 
                </td>
                <td class="style36" align ="left">Branch Address</td><td style="text-align: left" class="style20">              
                &nbsp;</td>
                <td style="text-align: left" class="style28">                    
                      <asp:TextBox ID="txtBrAdd" runat="server" Height="50px" Width="334px" BackColor="#FFCCFF" 
                      BorderStyle="None" ReadOnly="True" TextMode="MultiLine"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="style34"></td>
                <td class="style36"></td><td style="text-align: left" class="style20">              
                &nbsp;</td>
                <td class="style21"></td>
            </tr>
            <!-- ---------------------------------- -->
                     
            <tr>
                <td class="style34"></td>
                <td style="text-align: Left; font-family: Arial; font-weight: 700; color: ##003300;" 
                    class="style36">&nbsp;</td>
                <td class="style20"> &nbsp;</td>              
                <td style="text-align: left" class="style21" >  
                                                      
                    <asp:Button ID="btnSave" runat="server" Height="25px" Text="Save" 
                        width="89px" onclick="btnSave_Click" TabIndex="6" 
                        Font-Size="X-Small"
                        ToolTip="Click here for save data..." BackColor="#000099" 
                        BorderColor="White" Font-Overline="False" Font-Strikeout="False" 
                        Font-Underline="False" ForeColor="Aqua"                        
                        />
                        &nbsp;
                   <asp:Button ID="btnCancel" runat="server" Height="25px" Text="Cancel" 
                        Font-Size="X-Small"
                        Width="73px" TabIndex="7" onclick="btnCancel_Click" BackColor="#003399" 
                        ForeColor="Aqua" />                    
                                        
                </td>
                
                <td>&nbsp;</td>
            </tr>

           <tr>
                <td class="style34"></td>
                <td class="style36"></td>
                <td class="style20">&nbsp;</td>
                <td class="style21">&nbsp;</td>
            </tr>
            
            <!-- Line Break -->
            <tr>
                <td colspan="5" align="center"                    
                    style="background-image:url('../../Images/header.jpg'); height:0.5px; font-family: Arial;">                        
                </td>
            </tr>
                                                                                                          
            <tr>
                <td class="style34"></td>
                <td class="style36"></td>
                <td class="style20">&nbsp;</td>
                <td class="style21">&nbsp;</td>
            </tr>
            

            <!-- Grid View -->                       
            <tr>
                                
                <td colspan="5" align="center">   
                
                    <asp:GridView ID="gvCustomres" runat="server"                        
                        AutoGenerateColumns="False"
                        DataKeyNames="EmpID"
                        GridLines="None"
                        AllowPaging="false"
                        CssClass="mGrid"
                        PagerStyle-CssClass="pgr"                                               
                        AlternatingRowStyle-CssClass="alt" 
                        Onrowdeleting="gvCustomres_RowDelating"
                        OnRowDataBound="gvCustomres_RowDataBound"
                        Width="782px"
                        >
                        <SelectedRowStyle BackColor="BurlyWood"/>
                        <Columns>
                            <asp:TemplateField HeaderText="SL #">
                                 <ItemTemplate>
                                       <%# Container.DataItemIndex + 1 %>
                                 </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="JobID" HeaderText="Employee Job #" />
                            <asp:BoundField DataField="EmpName" HeaderText="Employee Name" />
                            <asp:BoundField DataField="eAddress" HeaderText="Address" />
                            <asp:BoundField DataField="ContactNo" HeaderText="Contact No" />
                            <asp:BoundField DataField="Designation" HeaderText="Designation" /> 
                            <asp:BoundField DataField="Department" HeaderText="Department" /> 
                            <asp:BoundField DataField="eName" HeaderText="Branch Name" />                                                      
                            
                            <asp:TemplateField HeaderText="Action">
                                <ItemTemplate>               
                                    <asp:ImageButton ID="ibtnDelete" runat="server"
                                        ToolTip="Delete"                                        
                                        ImageUrl="~/Images/btn-delete.jpg" 
                                        CommandName="Delete"   
                                        OnClientClick="return confirm('Are you sure you want to delete this record?');"                                                                                                          
                                    />
                                </ItemTemplate>                                
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <!-- ---------------------------------- -->

            <tr>
                <td class="style34"></td>
                <td class="style36"></td>
                <td class="style20">&nbsp;</td>
                <td class="style21">&nbsp;</td>
            </tr>
            
        </table>
        
                       
   </div>

   <div > &nbsp;</div>
   <div > &nbsp;</div>

</asp:Content>


