<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="FDScanSystem.Web.Error" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>访问权限不足</title>
    <style>
        body
        {
            background: #f9fee8; /* background-image: url('images/background.jpg');*/
            margin: 0;
            padding: 20px;
            text-align: left;
            font-family: Arial, Helvetica, sans-serif;
            font-size: 14px;
            color: #666666;
        }
        .error_page
        {
            padding: 15px;
            margin: auto;
        }
        .error_page h1
        {
            margin: 20px 0 0;
        }
        .error_page p
        {
            margin: 10px 0;
            padding: 0;
        }
        a
        {
            color: #9caa6d;
            text-decoration: none;
        }
        a:hover
        {
            color: #9caa6d;
            text-decoration: underline;
        }
    </style>
</head>
<body class="login">
    <div class="error_page" style="float:left;">
        <img alt="Kidmondo_face_sad" align="middle" src="/images/kidmondo_face_sad.png">
    </div>
    <div class="error_page" style="margin-left:10px;">
        <h1>
            非常抱歉...</h1>
        <p>
            您所在的用户组不能够访问该页面</p>
    </div>
</body>
</html>