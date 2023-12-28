<%@ Page Title="" Language="C#" MasterPageFile="~/DealerReports/DealerReports.master" AutoEventWireup="true" CodeFile="Organogram_Entryfrom.aspx.cs" Inherits="DealerReports_Organogram_Entryfrom" %>

     
    <asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" Runat="Server">
    

 <label for="cars" style="font-size:16px; width:150px;">Parent Zone:</label>
        <asp:DropDownList  ID="Parent_zone" runat="server" Width="150px" Height="25px"
            onselectedindexchanged="Parent_zone_SelectedIndexChanged">
            
        </asp:DropDownList>
    <br />
      <label for="fname" style="font-size:16px; width:150px;margin-top:10px;">Zone name:</label>
        <asp:TextBox ID="ZoneName" runat="server"></asp:TextBox>

    <br />
      <label for="fname" style="font-size:16px; width:150px;margin-top:10px;">Note:</label>
  
       <asp:TextBox ID="Note" runat="server"></asp:TextBox>
      <br/>

        <asp:Button  ID="Savebutton" runat="server" class="ml-300 btn btn-danger" Text="Save" onclick="Savebutton_Click" /> 




          <div class="col-md-8 col-md-offset-2" id="tbl_design">

              <asp:GridView ID="gridZone" runat="server" AutoGenerateColumns="false" 
                 OnRowCommand="gridZone_RowCommand" DataKeyNames="ZoneId"
                 CssClass="table" >

                   <Columns>
                    <asp:BoundField HeaderText="ZoneId" DataField="ZoneId" />
                   <asp:BoundField HeaderText="ParentZoneName" DataField="ParentName"  />
                    <asp:BoundField HeaderText="ZoneName" DataField="ZoneName" />
                      <asp:BoundField HeaderText="Note" DataField="Note"/>
                      <asp:ButtonField CommandName="change" HeaderText="Edit" ButtonType="Button" Text="Edit" ControlStyle-CssClass="btn btn-success">
                      </asp:ButtonField>

                         <asp:ButtonField CommandName="remove" HeaderText="Delete" ButtonType="Button" Text="Delete" ControlStyle-CssClass="btn btn-danger">
                      </asp:ButtonField>
                   </Columns>
                 
               
              </asp:GridView>
          </div>
         
         <div class="heading_item">
         <h3>Organogram View</h3>
         </div>


         <div class="tree_view">
        <asp:TreeView ID="TreeView1"  runat="server">
        
        </asp:TreeView>
        </div>

    </asp:Content>


  


