<%@ Page Title="" Language="C#" MasterPageFile="~/DealerReports/DealerReports.master" AutoEventWireup="true" CodeFile="DealerAsign.aspx.cs" Inherits="DealerReports_DealerAsign" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">

<link type="text/css" href="https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/css/select2.min.css" rel="stylesheet" />
<script type="text/javascript" src="https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/js/select2.min.js"></script>

<script>
    // In your Javascript (external .js resource or <script> tag)
    $(document).ready(function () {
        $('.js-example-basic-single').select2();
    });
</script>
    <label for="cars" style="font-size:16px; width:150px;">Zone Name:</label> 


<asp:DropDownList ID="ddl_ZoneName" CssClass="ddl js-example-basic-single" runat="server"  Width="150px" Height="25px">

</asp:DropDownList>

<br />

<label for="cars" style="font-size:16px; width:150px;">Dealer Name:</label>
<asp:DropDownList ID="ddl_dealerName"  CssClass="ddl js-example-basic-single" runat="server" Width="150px" Height="25px" Margin-top="20px">

</asp:DropDownList>



  <br />

    <asp:Button ID="btn_save" runat="server" Text="Save" 
        class="ml-30 btn btn-danger" onclick="btn_save_Click" />



 <div class="col-md-8 col-md-offset-2">

      <asp:GridView ID="gridView_Zonewisedealer" runat="server" 
          AutoGenerateColumns="false"  CssClass="table"
       OnRowCommand="gridView_Zonewisedealer_RowCommand" DataKeyNames="Id">

         <Columns>
   <asp:TemplateField HeaderText="Serial">
        <ItemTemplate>
             <%#Container.DataItemIndex+1 %>
        </ItemTemplate>
    </asp:TemplateField>
         <asp:BoundField  HeaderText="Zone" DataField="Zone"/>
         <asp:BoundField  HeaderText="Dealer" DataField="Dealer"/>

              

           <asp:ButtonField CommandName="change" HeaderText="Edit" ButtonType="Button" Text="Edit" ControlStyle-CssClass="btn btn-success">
             </asp:ButtonField>

              <asp:ButtonField CommandName="remove" HeaderText="Action" ButtonType="Button" Text="Delete" ControlStyle-CssClass="btn btn-danger">
               </asp:ButtonField>

         </Columns>

      </asp:GridView>  
  </div>

    <div class="error_msg">
    <asp:Label ID="lbl_message"  runat="server" Text=""></asp:Label>
    </div>
 

</asp:Content>



