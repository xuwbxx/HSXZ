<%@ Page Title="" Language="C#"  AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="FDScanSystem.Web.Index" %>
<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <%--    <meta content="text/html; charset=utf-8" http-equiv="Content-Type" />--%>
    <title>海上风电远程无线监测系统</title>
    <link href="/Styles/user.css" rel="Stylesheet" type="text/css" />
    <link href="/Styles/components.css" rel="Stylesheet" type="text/css" />
    <link href="/Styles/frame.css" rel="Stylesheet" type="text/css" />
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="/scripts/layer/layer.min.js" type="text/javascript"></script>
    <script src="/Scripts/Utilities.js" type="text/javascript"></script>
    <script src="/Scripts/Dialog1.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.mousewheel.js" type="text/javascript"></script>
</head>
<body>
    <div class="h-header" region="north">
        <div class="h-logo">
            <a href="#">
                <img src="/images/logo4.png" /></a>
        </div>
        <div class="h-toolbar">
            <a href="#"><i class="h-i-user"></i><span>用户中心</span><b class="h-caret"></b></a>
            <ul>
                <li><span>
                    <asp:Literal ID="ltr_UserName" runat="server"></asp:Literal>
                </span></li>
                <li><a href="javascript:void(0);" onclick="SetPwd();" title="修改密码"><i class="h-i-user"></i><span>修改密码</span></a></li>
                <li>
                <a href="javascript:void(0);" onclick="SetWe();" title="联系我们"><i class="h-i-user"></i><span>联系我们</span></a>
                </li>
                <li>
                    <asp:Literal ID="ltr_EditNotice" runat="server"></asp:Literal>
                </li>
                <li><a href="/LogOut.aspx" title="退出"><i class="h-i-logout"></i><span>退出</span></a></li>
            </ul>
        </div>
    </div>
    <div class="h-main"  region="center">
        <form id="Form1" runat="server">
          <div class="h-screen1 layout" fit="true">

  <div class="h-screen-tab">
        <div class="ks_breadcrumb">   
           首页
        </div>
    </div>
       <div class="a_items">
        <h5>
        </h5>
        <div style="width: 100%;">
            <asp:Literal ID="ltDiv" runat="server"></asp:Literal>
            <asp:Image ID="img_Layer" runat="server" Width="100%" Height="400" />
            <img src="" width="" height="" style="display:none;" />
        </div>
        <h5>
        </h5>
    </div>
    </div>
     </form>
    </div>
          
          </body>
          </html>
