﻿
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Search_Emi_Details.aspx.cs" 
Inherits="Search_Emi_Details" MasterPageFile="Admin.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<script runat="server">

    
    
</script>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    
    <style type="text/css">
      .hiddencol
      {
        display: none;
      }
    </style>
        
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
                        
        .style1
        {
            height: 24px;
        }
                        
    </style>
    
    
    <script language="JavaScript" src="../js/datetimepicker.js" type="text/javascript"> </script>
    
    <script type="text/javascript">
        $(function () {
            $("#txtDate").datepicker();
            $("#txtFrom").datepicker();
            $("#txtToDate").datepicker();
            $("#txtPIDate").datepicker();
        });        
    </script>
    
                       
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    
    <h2 class="col-sm-12 bg-primary" 
        style="padding:5px; color: #FFFFFF; background-color: ##006666;"> Model Wise EMI Details Information ...</h2>
    <p></p>
    
    <div>
        
        <table width="100%">
            <tr>
                <td></td>
                <td>
                    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                </td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
            </tr>

            <tr>
                <td class="style1"></td>
                <td class="style1">
                    
                </td>
                <td class="style1">:</td>
                <td class="style1">
                
                </td>
                <td class="style1"></td>
                <td class="style1">Model</td>
                <td class="style1">:</td>
                <td class="style1">
                    <asp:TextBox ID="txtModel" runat="server" Height="25px" Width="200px" 
                        CssClass="form-control"
                        placeholder="Enter Model & Search" TabIndex="10" 
                        AutoPostBack="True"> </asp:TextBox>
                    <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" TargetControlID="txtModel"
                            MinimumPrefixLength="1" EnableCaching="true" CompletionSetCount="1" CompletionInterval="100" 
                            ServiceMethod="GetModel" >
                    </asp:AutoCompleteExtender>
                                
                </td>
                <td class="style1"></td>
            </tr>

            

            <tr>
                <td></td>
                <td>
                    
                </td>
                <td></td>
                <td>&nbsp;</td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
            </tr>

            
            <tr>
                <td></td>
                <td>
                    
                </td>
                <td></td>
                <td>
                    <asp:Button ID="btnSearch" CssClass="btn btn-success" runat="server" 
                        data-toggle="tooltip" title="Click here for Search Sales Data ..."
                        Text="   Search  " OnClick="SearchData" />                        
                </td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
            </tr>


        </table>

    </div>

    <h4 class="col-sm-12 bg-primary" style="padding:0.5px"></h4>

    <h2>
        <asp:Label ID="lblModel" runat="server" Text="" Font-Names="Tahoma" 
            Font-Size="Larger" ForeColor="#000099"></asp:Label>
    </h2>

    <div>        
        <asp:GridView ID="GridView1" runat="server" AllowPaging="True" 
            AlternatingRowStyle-CssClass="alt" AutoGenerateColumns="False" CssClass="table" 
            BorderColor="#999999" BorderStyle="Double" BorderWidth="1px" CellPadding="2" 
            DataKeyNames="Model" GridLines="Vertical" 
            OnRowDataBound="GridView1_RowDataBound" 
           
            PagerStyle-CssClass="pgr" 
            ShowFooter="true" 
            Width="100%" PageSize="10">
            <FooterStyle BackColor="#006666" ForeColor="White" />
            <HeaderStyle BackColor="#006666" CssClass="bg-primary"/>

            <PagerStyle CssClass="pgr"></PagerStyle>
                
            <SelectedRowStyle BackColor="#0099CC" />
            <AlternatingRowStyle CssClass="alt" BackColor="#C2D69B"></AlternatingRowStyle>
            <Columns>
                <asp:TemplateField HeaderText="SL#">
                    <ItemTemplate>
                        <%# Container.DataItemIndex + 1 %>
                    </ItemTemplate>
                </asp:TemplateField>                                
                
                <asp:BoundField DataField="Model" HeaderText="Model" />
                <asp:BoundField DataField="PurchaseMode" HeaderText="Purchase Mode" /> 
                <asp:BoundField DataField="MRP" HeaderText="MRP" /> 
                <asp:BoundField DataField="SellingPrice" HeaderText="Selling Price" />  
                <asp:BoundField DataField="Discount" HeaderText="Discount" />  
                <asp:BoundField DataField="DiscountPercent" HeaderText="Discount Percent(%)" />   
                <asp:BoundField DataField="MonthlyEMI" HeaderText="Monthly EMI" />             
                                
            </Columns>
        </asp:GridView>
        
    </div>
        
    <div>&nbsp;</div>    
    <div></div>
    <br />

    <table class="table table-bordered table-striped" style="font-weight:bold">
    <tr>
        <td>SL NO</td>
        <td>EMI BANK </td>
        <td colspan="6">Tenure (Months)</td>
        <%--<td></td>
        <td></td>
        <td></td>
        <td></td>
        <td></td>--%>
        <td>MINIMUM LIMIT  </td>
    </tr>
    <tr>
        <td></td>
        <td></td>
        <td>3</td>
        <td>6</td>
        <td>12</td>
        <td>18</td>
        <td>24</td>
        <td>36</td>
        <td></td>
    </tr>
    <tr>
        <td>1</td>
        <td>DHAKA BANK LIMITED</td>
        <td>Yes</td>
        <td>Yes</td>
        <td>Yes</td>
        <td>Yes</td>
        <td>Yes</td>
        <td></td>
        <td>10000</td>
    </tr>
    <tr>
        <td>2</td>
        <td>AL ARAFAH ISLAMI BANK LIMITED</td>
        <td>Yes</td>
        <td>Yes</td>
        <td></td>
        <td></td>
        <td></td>
        <td></td>
        <td>10000</td>
    </tr>
    <tr>
        <td>3</td>
        <td>AB BANK LIMITED</td>
        <td>Yes</td>
        <td>Yes</td>
        <td>Yes</td>
        <td></td>
        <td></td>
        <td></td>
        <td>5000</td>
    </tr>
    <tr>
        <td>4</td>
        <td>PREMIER BANK LIMITED</td>
        <td>Yes</td>
        <td>Yes</td>
        <td>Yes</td>
        <td></td>
        <td></td>
        <td></td>
        <td>5000</td>
    </tr>
    <tr>
        <td>5</td>
        <td>ONE BANK LIMITED</td>
        <td>Yes</td>
        <td>Yes</td>
        <td>Yes</td>
        <td></td>
        <td></td>
        <td></td>
        <td>5000</td>
    </tr>
    <tr>
        <td>6</td>
        <td>NRB BANK LIMITED</td>
        <td>Yes</td>
        <td>Yes</td>
        <td>Yes</td>
        <td>Yes</td>
        <td>Yes</td>
        <td>Yes</td>
        <td>10000</td>
    </tr>
    <tr>
        <td>7</td>
        <td>MERCANTILE BANK LIMITED</td>
        <td>Yes</td>
        <td>Yes</td>
        <td>Yes</td>
        <td>Yes</td>
        <td>Yes</td>
        <td>Yes</td>
        <td>10000</td>
    </tr>
    <tr>
        <td>8</td>
        <td>NCC BANK LIMITED</td>
        <td>Yes</td>
        <td>Yes</td>
        <td>Yes</td>
        <td>Yes</td>
        <td>Yes</td>
        <td>Yes</td>
        <td>10000</td>
    </tr>
    <tr>
        <td>9</td>
        <td>SOUTHEAST BANK LIMITED</td>
        <td>Yes</td>
        <td>Yes</td>
        <td>Yes</td>
        <td></td>
        <td></td>
        <td></td>
        <td>5000</td>
    </tr>
    <tr>
        <td>10</td>
        <td>LANKA BANGLA FINANCE LIMITED</td>
        <td>Yes</td>
        <td>Yes</td>
        <td>Yes</td>
        <td>Yes</td>
        <td>Yes</td>
        <td></td>
        <td>10000</td>
    </tr>
    <tr>
        <td>11</td>
        <td>UNITED COMMERCIAL BANK LIMITED</td>
        <td>Yes</td>
        <td>Yes</td>
        <td>Yes</td>
        <td></td>
        <td></td>
        <td></td>
        <td>10000</td>
    </tr>
    <tr>
        <td>12</td>
        <td>TRUST BANK LIMITED</td>
        <td>Yes</td>
        <td>Yes</td>
        <td></td>
        <td></td>
        <td></td>
        <td></td>
        <td>5000</td>
    </tr>
    <tr>
        <td>13</td>
        <td>JAMUNA BANK LIMITED</td>
        <td>Yes</td>
        <td>Yes</td>
        <td>Yes</td>
        <td>Yes</td>
        <td>Yes</td>
        <td>Yes</td>
        <td>5000</td>
    </tr>
    <tr>
        <td>14</td>
        <td>STANDARD BANK LIMITED</td>
        <td>Yes</td>
        <td>Yes</td>
        <td>Yes</td>
        <td>Yes</td>
        <td>Yes</td>
        <td>Yes</td>
        <td>5000</td>
    </tr>
    <tr>
        <td>15</td>
        <td>CITY BANK LIMITED - AMEX</td>
        <td>Yes</td>
        <td>Yes</td>
        <td>Yes</td>
        <td>Yes</td>
        <td>Yes</td>
        <td>Yes</td>
        <td>5000</td>
    </tr>
    <tr>
        <td>16</td>
        <td>SHAJALAL ISLAMI BANK LIMITED</td>
        <td>Yes</td>
        <td>Yes</td>
        <td>Yes</td>
        <td>Yes</td>
        <td>Yes</td>
        <td></td>
        <td>5000</td>
    </tr>
    <tr>
        <td>17</td>
        <td>BRAC BANK LIMITED</td>
        <td>Yes</td>
        <td>Yes</td>
        <td>Yes</td>
        <td>Yes</td>
        <td>Yes</td>
        <td>Yes</td>
        <td>5000</td>
    </tr>
    <tr>
        <td>18</td>
        <td>SOCIAL ISLAMI BANK LIMITED</td>
        <td>Yes</td>
        <td>Yes</td>
        <td></td>
        <td></td>
        <td></td>
        <td></td>
        <td>5000</td>
    </tr>
    <tr>
        <td>19</td>
        <td>MIDLAND BANK LIMITED</td>
        <td>Yes</td>
        <td>Yes</td>
        <td></td>
        <td></td>
        <td></td>
        <td></td>
        <td>5000</td>
    </tr>
    <tr>
        <td>20</td>
        <td>DBBL</td>
        <td>Yes</td>
        <td>Yes</td>
        <td>Yes</td>
        <td>Yes</td>
        <td>Yes</td>
        <td>Yes</td>
        <td>5000</td>
    </tr>
    <tr>
        <td>21</td>
        <td>MEGHNA BANK LIMITED</td>
        <td>Yes</td>
        <td>Yes</td>
        <td>Yes</td>
        <td>Yes</td>
        <td>Yes</td>
        <td>Yes</td>
        <td>5000</td>
    </tr>
    <tr>
        <td>22</td>
        <td>COMMERCIAL BANK OF CEYLON</td>
        <td>Yes</td>
        <td>Yes</td>
        <td>Yes</td>
        <td></td>
        <td></td>
        <td></td>
        <td>5000</td>
    </tr>
    <tr>
        <td>23</td>
        <td>STANDARD CHARTERED BANK LIMITED</td>
        <td>Yes</td>
        <td>Yes</td>
        <td>Yes</td>
        <td></td>
        <td></td>
        <td></td>
        <td>5000</td>
    </tr>
    <tr>
        <td>24</td>
        <td>MUTUAL TRUST BANK LIMITED</td>
        <td>Yes</td>
        <td>Yes</td>
        <td>Yes</td>
        <td>Yes</td>
        <td>Yes</td>
        <td></td>
        <td>10000</td>
    </tr>
    <tr>
        <td>25</td>
        <td>NRB COMERCIAL BANK LIMITED</td>
        <td>Yes</td>
        <td>Yes</td>
        <td>Yes</td>
        <td>Yes</td>
        <td>Yes</td>
        <td>Yes</td>
        <td>5000</td>
    </tr>
    <tr>
        <td>26</td>
        <td>EASTERN BANK LIMITED</td>
        <td>Yes</td>
        <td>Yes</td>
        <td>Yes</td>
        <td>Yes</td>
        <td>Yes</td>
        <td>Yes</td>
        <td>5000</td>
    </tr>
    <tr>
        <td>27</td>
        <td>PRIME  BANK LIMITED</td>
        <td>Yes</td>
        <td>Yes</td>
        <td></td>
        <td></td>
        <td></td>
        <td></td>
        <td>10000</td>
    </tr>
    <tr>
        <td>28</td>
        <td>ISLAMI BANK BANGLADESH LIMITED</td>
        <td>Yes</td>
        <td>Yes</td>
        <td>Yes</td>
        <td>Yes</td>
        <td>Yes</td>
        <td></td>
        <td>10000</td>
    </tr>
    <tr>
        <td>29</td>
        <td>PUBALI BANK LIMITED</td>
        <td>Yes</td>
        <td>Yes</td>
        <td>Yes</td>
        <td>Yes</td>
        <td>Yes</td>
        <td>Yes</td>
        <td>5000</td>
    </tr>
    <tr>
        <td>30</td>
        <td>SBAC BANK LIMITED</td>
        <td>Yes</td>
        <td>Yes</td>
        <td>Yes</td>
        <td></td>
        <td></td>
        <td></td>
        <td>5000</td>
    </tr>
    <tr>
        <td>31</td>
        <td>COMMUNITY BANK BANGLADESH LIMITED</td>
        <td>Yes</td>
        <td>Yes</td>
        <td>Yes</td>
        <td>Yes</td>
        <td>Yes</td>
        <td>Yes</td>
        <td>5000</td>
    </tr>
</table>
</asp:Content>
