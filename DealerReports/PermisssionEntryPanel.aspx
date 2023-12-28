<%@ Page Title="" Language="C#" MasterPageFile="~/DealerReports/DealerReports.master" AutoEventWireup="true" CodeFile="PermisssionEntryPanel.aspx.cs" Inherits="DealerReports_PermisssionEntryPanel" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">

<link type="text/css" href="https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/css/select2.min.css" rel="stylesheet" />
<script type="text/javascript" src="https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/js/select2.min.js"></script>
<link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css">
<script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.16.0/umd/popper.min.js"></script>



<script>
    // In your Javascript (external .js resource or <script> tag)
    $(document).ready(function () {
        $('.js-example-basic-single').select2();
    });


    function client_OnTreeNodeChecked() {
        var obj = window.event.srcElement;
        var treeNodeFound = false;
        var checkedState;
        if (obj.tagName == "INPUT" && obj.type == "checkbox") {
            var treeNode = obj;
            checkedState = treeNode.checked;
            do {
                obj = obj.parentElement;
            } while (obj.tagName != "TABLE")
            var parentTreeLevel = obj.rows[0].cells.length;
            var parentTreeNode = obj.rows[0].cells[0];
            var tables = obj.parentElement.getElementsByTagName("TABLE");
            var numTables = tables.length
            if (numTables >= 1) {
                for (i = 0; i < numTables; i++) {
                    if (tables[i] == obj) {
                        treeNodeFound = true;
                        i++;
                        if (i == numTables) {
                            return;
                        }
                    }
                    if (treeNodeFound == true) {
                        var childTreeLevel = tables[i].rows[0].cells.length;
                        if (childTreeLevel > parentTreeLevel) {
                            var cell = tables[i].rows[0].cells[childTreeLevel - 1];
                            var inputs = cell.getElementsByTagName("INPUT");
                            inputs[0].checked = checkedState;
                        }
                        else {
                            return;
                        }
                    }
                }
            }
        }
    }


</script>









<div class="content">
<div class="UserEntry" style="margin-top:5px;">
<label for="cars" style="font-size:16px; width:150px;">User Name:</label> 

 <asp:DropDownList ID="ddlUserName" CssClass="ddl js-example-basic-single" runat="server"  Width="150px" Height="25px"
  AutoPostBack="True" OnSelectedIndexChanged="ddlUserName_SelectedIndexChanged">

</asp:DropDownList>
 <asp:Button ID="Savebutton" runat="server" Text="Submit" 
        class="ml-30 btn btn-danger" onclick="btn_save_Click" />
</div>



<div class="tree_view">
       <h3><b>View Of Zone</b></h3>
        
        <asp:TreeView ID="treeview" ShowCheckBoxes="All" runat="server" ShowLines="True" onclick="client_OnTreeNodeChecked();">
        
        </asp:TreeView>
 </div>

 </div>
  

</asp:Content>





