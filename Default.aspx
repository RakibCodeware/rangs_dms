<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        Welcome to Dms</h2>
    
    <div id="site_content">
        <div class="gallery">
            <ul class="images">
              <li class="show"><img width="920" height="400" src="PhotoSlide/1.jpg" alt="1"/></li>
              <li><img width="920" height="400" src="PhotoSlide/2.jpg" alt="2" /></li>
              <li><img width="920" height="400" src="PhotoSlide/3.jpg" alt="3" /></li>
            </ul>
        </div>
    </div>
    

    <p>
      <!-- javascript at the bottom for fast page loading -->
      <script type="text/javascript" src="js/jquery.js"></script>
      <script type="text/javascript" src="js/jquery.easing-sooper.js"></script>
      <script type="text/javascript" src="js/jquery.sooperfish.js"></script>
      <script type="text/javascript" src="js/image_fade.js"></script>
      <script type="text/javascript">
          $(document).ready(function () {
              $('ul.sf-menu').sooperfish();
          });
      </script>
    </p>
    

</asp:Content>



