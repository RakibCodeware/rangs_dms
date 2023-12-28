<%@ Page Language="C#" MasterPageFile="Admin.master" AutoEventWireup="true" 
CodeFile="Requirement.aspx.cs" Inherits="Forms_Requirement" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
        
    <style type="text/css">
        .style19
        {
            width: 631px;
        }
        .grid
        {}
        .style20
        {
            width: 13px;
        }
        .style21
        {
            width: 429px;
        }
        .style22
        {
            width: 429px;
            height: 29px;
        }
        .style24
        {
            width: 156px;
            height: 29px;
        }
        .style25
        {
            width: 156px;
        }
        .style26
        {
            width: 52px;
        }
        .style27
        {
            width: 52px;
            height: 29px;
        }
        .style28
        {
            width: 52px;
            height: 36px;
        }
        .style29
        {
            width: 156px;
            height: 36px;
        }
        .style30
        {
            width: 13px;
            height: 36px;
        }
        .style32
        {
            height: 36px;
        }
    </style>
    
    <link type="text/css" href="css/ui-lightness/jquery-ui-1.8.19.custom.css" rel="stylesheet" />
    <link href="css/grid.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="js1/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="js1/jquery-ui-1.8.19.custom.min.js"></script>

    <link type="text/css" href="css/ui-lightness/jquery-ui-1.8.19.custom.css" rel="stylesheet" />
    <script type="text/javascript" src="js/bootstrap-combobox.js"></script>

        
    <style>
		a img{border: none;}
		ol li{list-style: decimal outside;}
		div#container{width: 780px;margin: 0 auto;padding: 1em 0;}
		div.side-by-side{width: 100%;margin-bottom: 1em;}
		div.side-by-side > div{float: left;width: 50%;}
		div.side-by-side > div > em{margin-bottom: 10px;display: block;}
		.clearfix:after{content: "\0020";display: block;height: 0;clear: both;overflow: hidden;visibility: hidden;}
		
	</style>
    <link rel="stylesheet" href="../Styles/chosen.css" />

    <script type="text/javascript">
        $(function () {
            $("#txtDate").datepicker();
            $("#txtLDDate").datepicker();
            $("#txtDODate").datepicker();
            $("#txtPIDate").datepicker();
        });        
    </script>
    
    <script type="text/javascript">
        $(document).ready(function () {
            $('.combobox').combobox();
        });
    </script>
    
    
    <style type="text/css">
        body
        {
            font-family: Arial;
            font-size: 10pt;
        }
        .Grid1
        {
            border: 1px solid #ccc;
            margin-bottom: -1px;
        }
        .Grid1 th
        {
            background-color: #F7F7F7;
            color: #333;
            font-weight: bold;
        }
        .Grid1 th, .Grid1 td
        {
            padding: 5px;
            border-color: #ccc;
        }
    </style>
    
    <style type="text/css">
        .cpHeader
        {
            color: white;
            background-color: #719DDB;
            font: bold 11px auto "Trebuchet MS", Verdana;
            font-size: 12px;
            cursor: pointer;
            width:100%;
            height:25px;
            padding: 4px;           
        }
        .cpBody
        {
            background-color: #DCE4F9;
            font: normal 11px auto Verdana, Arial;
            border: 1px gray;               
            width:100%;
            padding: 4px;
            padding-top: 7px;
        }      
    </style>
    
    <style type="text/css">
      .hiddencol
      {
        display: none;
      }
        .style33
        {
            width: 10px;
        }
    </style>

    <script>
        $(document).ready(function () {
            $('[data-toggle="tooltip"]').tooltip();
        });
    </script>
                       
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    
    <h4 class="col-sm-12 bg-primary" style="padding:5px"> Product Requirement (New Entry) ...</h4>
    <p></p>
    
    <asp:Label ID="Label1" runat="server" BackColor="#0099FF" CssClass="ui-bar" Text=""></asp:Label>

    <div align="center">
        
        <table width="100%" style="font-family: Tahoma; font-size: small">
            
            <tr>
                <td class="style26"></td><td class="style25">&nbsp;</td><td class="style20"></td>
                <td class="style21">
                    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                </td>
                <td align="left" class="style33"></td>
                <td align="left"></td>
                <td align="left"></td>
                <td align="left"></td>
            </tr>
            
            <!-- Challan No. -->
            <tr>
                <td class="style26"></td>
                <td style="text-align: Left; font-family: Arial; font-weight: 700; color: #009900;" 
                    class="style25">Request #</td>
                <td class="style20"> : </td>
                <td style="text-align: left" class="style21">
                <asp:TextBox ID="txtCHNo" runat="server" Width="200px"  CssClass="form-control"
                    style="font-weight: 700" ToolTip="Please Enter Challan Number" MaxLength="15" 
                        Enabled="False"></asp:TextBox>
                </td>
                <td align="left" class="style33"></td>
                <td>
                    Request Date :</td>
                <td align="left">
                    <asp:TextBox ID="txtDate" runat="server" Width="97px" TabIndex="1" 
                        ToolTip="Please Enter Challan Date" MaxLength="10"></asp:TextBox> 
                    &nbsp; 
                    <asp:ImageButton ID="imgPopup" ImageUrl="~/Images/cal.gif" ImageAlign="Bottom"
                        runat="server" TabIndex="1" />
                    <cc1:CalendarExtender ID="Calendar1" PopupButtonID="imgPopup" runat="server" TargetControlID="txtDate"
                        Format="MM/dd/yyyy">
                    </cc1:CalendarExtender>
                </td>
                <td align="left"></td>
            </tr>
            
            <!-- Challan Date -->
            <tr>
                <td class="style27"></td>
                <td style="text-align: left; font-family: Arial; font-size: small; " 
                    class="style24"></td>
                <td class="style20"> &nbsp;</td>
                <td style="text-align: left" class="style22">
                                    
                    <asp:TextBox ID="txtMRSR" runat="server" Enabled="False" Font-Size="Smaller" 
                        Width="46px" Visible="False"></asp:TextBox>                                    
                &nbsp;<asp:RequiredFieldValidator 
                     id="RequiredFieldValidator1" runat="server" 
                     ErrorMessage="Required!"                      
                     ControlToValidate="txtCHNo"> </asp:RequiredFieldValidator>
                </td>
                <td align="left" class="style33"></td>
                <td style="font-family: Arial; font-size: x-small; color: #990033" 
                    class="style2">
                </td>
                <td align="left" style="color: #FF0000">(MM/dd/yyyy)</td>
                <td align="left"></td>

            </tr>
                  
                               
            <!-- Line Break -->
            <tr>
                <td class="style26"></td>
                <td class="style25"></td><td style="text-align: left" class="style20">
              
                &nbsp;</td>
                <td class="style21" align="left" style="color: #FF0000"></td>
            </tr>
            <tr>
                <td colspan="8" align="center"
                    style="background-image:url(../Images/header.jpg); height:0.5px; font-family: Arial;">                        
                    </td>
            </tr>
            <tr>
                <td class="style26"></td>
                <td class="style25"></td><td style="text-align: left" class="style20">
              
                &nbsp;</td>
                <td class="style21">
                                        
                    <asp:TextBox ID="txtProdID" runat="server" Width="16px" 
                        style="font-weight: 700" Visible="False"></asp:TextBox>

                    </td>
                <td align="left" class="style33"></td>
                <td align="left"></td>
                <td align="left"></td>
                <td align="left"></td>
            </tr>
            <!-- ---------------------------------- -->

            
             <!-- Product Model -->
            <tr>
                <td class="style26"></td>
                <td style="text-align: Left; font-family: Arial; font-weight: 700; color: ##003300;" 
                    class="style25">Product Model</td>
                <td class="style20"> : </td>              
                <td style="text-align: left" >  
                    <div class="side-by-side clearfix">                                  
                        <asp:DropDownList ID="ddlContinents" class="chzn-select"
                            runat="server" AutoPostBack = "True"
                            data-placeholder="Choose a Model ..."
                            OnSelectedIndexChanged="ddlContinents_SelectedIndexChanged"                         
                            Width="200px" Height="32px" TabIndex="6"
                            ToolTip="Please Select Product Model ...">                                    
                        </asp:DropDownList> 
                    </div>                    
                </td>

                <td align="left" class="style33">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;</td>
                <td align="left" class="style25">Code :
                    </td>
                <td align="left">
                    <asp:TextBox ID="txtCode" runat="server" AutoPostBack="True" 
                        ontextchanged="txtCode_TextChanged"></asp:TextBox>
                </td>
                <td align="left"></td>
                    
            </tr>

             <!-- Product Description -->
            <tr>
                <td class="style28"></td>
                <td style="text-align: Left; font-family: Arial; font-weight: 700; color: ##003300;" 
                    class="style29">Quantity</td>
                <td class="style30"> : </td>
                <td style="text-align: left" class="style21" >
                    <asp:TextBox ID="txtQty" runat="server" Width="200px" 
                        style="font-weight: 700"
                        onkeypress="return numeric_only(event)"
                        ToolTip="Please Enter Product Quantity" TabIndex="3" 
                        ></asp:TextBox>                    
                </td>

                <td align="left" class="style33"></td>
                <td style="text-align: Left; font-family: Arial; " 
                    class="style29">Product Description </td>
                <td class="style32">
                <asp:TextBox ID="txtProdDesc" runat="server" Width="200px" 
                    style="font-weight: 500" Enabled="False"></asp:TextBox></td>

            </tr>
            
            

            <!-- Quantity -->
            <tr>
                <td class="style26"></td>
                <td style="text-align: Left; font-family: Arial; " 
                    class="style25">Remarks</td>
                <td class="style20"> : </td>
                <td style="text-align: left" class="style21" >
                <asp:TextBox ID="txtRemarks" runat="server" Width="200px" 
                    style="font-weight: 700" TabIndex="5"></asp:TextBox></td>
                <td class="style33">&nbsp;</td>
                <td align="left"></td>
                <td align="left"></td>
                <td align="left"></td>
            </tr>
                       
            <tr>
                <td class="style26"></td>
                <td class="style25"></td>
                <td class="style20"></td>
                <td class="style21">&nbsp;</td>
                <td align="left" class="style33"></td>
                <td align="left"></td>
                <td align="left"></td>
            </tr>           
            
            <!-- Add to Data Grid -->
            <tr>
                <td class="style26"></td>
                <td class="style25"></td>
                <td class="style20"></td>
                <td style="text-align: left" class="style21">
                    <asp:Button ID="btnAdd" runat="server" Text="Add Product in List" OnClick="btnAdd_Click" 
                        Width="116px" Height="25px"
                        Font-Size="X-Small" CssClass="btn-success"
                        ToolTip="Click here for add product in list ..." TabIndex="13" 
                        />
                </td>  
                <td align="left" class="style33"></td>
                <td align="left"></td>
                <td align="left"></td>              
            </tr>
            
            <tr>
                <td class="style26"></td>
                <td class="style25"></td>
                <td class="style20"></td>
                <td class="style21">&nbsp;</td>
                <td align="left" class="style33"></td>
                <td align="left"></td>
                <td align="left"></td>
            </tr>
            
        </table>
        
        
        <div>
        <asp:Label ID="lblError" ForeColor="red" runat="server" Text=""></asp:Label>
        </div>
        
        <table width="100%" style="font-family: Tahoma; font-size: small">
            <tr>
                    <td class="style19">
                        <!-- Data Grid -->                
                        <asp:GridView ID="gvUsers" runat="server"
                        AutoGenerateColumns="false"                                       
                        CssClass="mGrid" 
                        DataKeyNames="ProductID" ShowHeaderWhenEmpty="true"                        
                        EmptyDataText = "No product in list !!! Please select model and add in list."                        
                        Onrowdeleting="gvUsers_RowDelating"
                        ShowFooter="true"
                        OnRowDataBound="gvUsers_RowDataBound"                        
                        Width="100%"> 
                                               
                        <FooterStyle Font-Bold="true" BackColor="#CCFFFF" ForeColor="black" BorderStyle="NotSet" />
                        
                        <Columns>
                            <asp:BoundField HeaderText="ProductID" DataField="ProductID" ItemStyle-Width="35px"/>                                
                            
                            <asp:BoundField HeaderText="Product Model" DataField="Model" />
                            <asp:BoundField HeaderText="Quantity" DataField="Qty" ItemStyle-Width="30px"/>                                                                                                         
                            
                            <asp:BoundField HeaderText="Remarks" DataField="Remarks"  />
                            <asp:TemplateField HeaderText="Action" ItemStyle-Width="6px">
                                <ItemTemplate>               
                                    <asp:ImageButton ID="ibtnDelete" runat="server"
                                        ToolTip="Delete"                                        
                                        ImageUrl="~/Images/btn-delete.jpg" 
                                        CommandName="Delete"   
                                        OnClientClick="return confirm('Are you sure you want to remove this record?');"                                                                                                          
                                         />
                                </ItemTemplate>
                                
                                <ItemStyle Width="6px"></ItemStyle>
                                
                            </asp:TemplateField>

                        </Columns>
                        <EmptyDataRowStyle CssClass="gvEmpty" />
                            <HeaderStyle BackColor="#000066" ForeColor="White" />
                    </asp:GridView>
                </td>
            </tr> 
            <tr>
                <td>
                    <asp:Label ID="lblNetAmnt" runat="server" Text="0" Visible="False"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="1" align="center"
                    style="background-image:url(../Images/header.jpg); height:0.5px; font-family: Arial; ">                        
                </td>
            </tr>
            <tr>
                <td> &nbsp;</td>
            </tr>
            <tr>
                <td align="center">                    
                    <asp:Button ID="btnSave" runat="server" Text="Submit" 
                        width="88px" onclick="btnSave_Click" TabIndex="7" 
                        Font-Size="Small" CssClass="btn btn-primary"
                        ToolTip="Click here for save data..."/>
                        &nbsp;
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" 
                        Font-Size="Small" CssClass="btn btn-primary"
                        Width="88px" TabIndex="9" onclick="btnCancel_Click" />                    
                </td>
            </tr>
            <tr>
                <td>&nbsp;
                    <asp:Label ID="lblCTPName" runat="server" Visible="False"></asp:Label>
                    <asp:Label ID="lblEID" runat="server" Visible="False"></asp:Label>
                    <asp:Label ID="lblCTPAdd" runat="server" Visible="False"></asp:Label>
                    <asp:Label ID="lblCTPEmail" runat="server" Visible="False"></asp:Label>
                    <asp:Label ID="lblCTPContact" runat="server" Visible="False"></asp:Label>
                </td>
            </tr>
        </table>

        <p></p>

   </div>

   <script src="../js/jquery.min.js" type="text/javascript"></script>
    <script src="../js/chosen.jquery.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(".chzn-select").chosen(); $(".chzn-select-deselect").chosen({ allow_single_deselect: true }); 
    </script>  

</asp:Content>



