<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LogIn.aspx.cs" Inherits="LogIn" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head >
  <meta charset="utf-8">
  <meta http-equiv="X-UA-Compatible" content="IE=edge">
  <title>DMS | Log in</title>
  <!-- Tell the browser to be responsive to screen width -->
  <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport">
  <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap/5.2.3/css/bootstrap.min.css" integrity="sha512-SbiR/eusphKoMVVXysTKG/7VseWii+Y3FdHrt0EpKgpToZeemhqHeZeLWLhJutz/2ut2Vw1uQEj2MbRF+TVBUA==" crossorigin="anonymous" referrerpolicy="no-referrer" />
  <!-- Font Awesome -->
  <link rel="stylesheet" href="bower_components/font-awesome/css/font-awesome.min.css">
  <!-- Ionicons -->
  <link rel="stylesheet" href="bower_components/Ionicons/css/ionicons.min.css">
  <!-- Theme style -->
  <link rel="stylesheet" href="dist/css/AdminLTE.min.css">
  <!-- iCheck -->
  <link rel="stylesheet" href="plugins/iCheck/square/blue.css">

  <!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
  <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
  <!--[if lt IE 9]>
  <script src="https://oss.maxcdn.com/html5shiv/3.7.3/html5shiv.min.js"></script>
  <script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
  <![endif]-->

  <!-- Google Font -->
  <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,600,700,300italic,400italic,600italic">

  <link href="favicon.ico" rel="DMS">
  <link rel="icon" href="favicon.ico" />

</head>


<body class="hold-transition login-page">
  
  <div class="login-box">
      <div class="login-logo">
        <a href="#">
            <asp:Image ID="Image1" runat="server" Height="100%" 
              ImageUrl="~/image/SonyRangs_Logo.png" Width="100%" />
        </a>
      </div>
      <!-- /.login-logo -->

  <div class="login-box-body">
    <p class="login-box-msg">Sign in to start your POS by DMS </p>

    
    <form id="form1" runat="server"  class="form-signin" role="form">

      <div class="form-group has-feedback">        
        <asp:TextBox ID="UserName1" runat="server" cssclass="form-control" placeholder="Username"></asp:TextBox>          
        <span class="glyphicon glyphicon-envelope form-control-feedback"></span>
      </div>
      <div class="form-group has-feedback">
        <asp:TextBox ID="Password" runat="server" TextMode="Password" cssclass="form-control" placeholder="Password"> </asp:TextBox>
        <span class="glyphicon glyphicon-lock form-control-feedback"></span>
      </div>
      <div class="row">
        <!--
        <div class="col-xs-8">
          <div class="checkbox icheck">
            <label>
              <input type="checkbox"> Remember Me
            </label>
          </div>
        </div> -->
        <!-- /.col -->
        <div class="col-xs-4">          
          <asp:Button ID="btnLogin" runat="server" OnClick="btnLogin_Click" Text="Login"  cssclass="btn btn-primary btn-block btn-flat" />
        </div>
        <!-- /.col -->
      </div>
    </form>

    <div class="form-group">                                                   
        <asp:Label  ID="lblMessage" runat="server"></asp:Label> 
    </div>

    <div class="social-auth-links text-center">
      
      <a href="https://www.facebook.com/sonyrangsbd/" class="btn btn-block btn-social btn-facebook btn-flat">
        <i class="fa fa-facebook"></i> Sign in our Facebook</a>
      <a href="#" class="btn btn-block btn-social btn-google btn-flat"><i class="fa fa-google-plus">
        </i> Sign in our Google+</a>
    </div>
    <!-- /.social-auth-links -->

    <!--
    <a href="#">I forgot my password</a><br>
    <a href="#" class="text-center">Register a new membership</a>
    -->

    <div class="form-group">                                                   
        <p style="font-family: Tahoma; font-size: x-small">Copyright &copy; <span></span> <a href="http://rangs.com.bd/">Rangs Electronics Ltd.</a>  | All Rights Reserved</p>
    </div>

  </div>
  <!-- /.login-box-body -->
</div>
<!-- /.login-box -->

<!-- jQuery 3 -->
<script src="../../bower_components/jquery/dist/jquery.min.js"></script>
<!-- Bootstrap 3.3.7 -->
<script src="../../bower_components/bootstrap/dist/js/bootstrap.min.js"></script>
<!-- iCheck -->
<script src="../../plugins/iCheck/icheck.min.js"></script>
<script>
    $(function () {
        $('input').iCheck({
            checkboxClass: 'icheckbox_square-blue',
            radioClass: 'iradio_square-blue',
            increaseArea: '20%' // optional
        });
    });
</script>
    
</body>
</html>

