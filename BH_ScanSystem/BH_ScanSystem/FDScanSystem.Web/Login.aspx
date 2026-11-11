<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="FDScanSystem.Web.Login" %>


<head>
    <style type="text/css">
        .logt
        {
            font-family: '微软雅黑' , '楷体';
            font-size: 16px;
        }
        .logt .fiename
        {
            font-weight: bold;
            color: #fff;
            width: 75px;
        }
        .logt .fieinput
        {
            border: 1px solid #074785;
            height: 25px;
            width: 160px;
        }
    </style>
    <title>海上风电远程无线监测系统</title>
</head>
<body style="background-color: #4182C2; padding-top: 41px; padding-left: 128px; overflow-x: hidden;
    overflow-y: hidden;">
    <form id="Form1" runat="server">
    <div style="background: url('images/lbg.jpg'); margin: 0px auto;/* width: 1000px; */height: 625px;
        background-repeat: no-repeat; background-position: left center;">
        <div style="width: 500px; padding-top: 256px; padding-left: 330px;">
            <table width="360px" class="logt" cellspacing="0" style="height: 150px;" cellpadding="0">
                <tr>
                    <td class="fiename">
                        用户名：
                    </td>
                    <td>
                        <asp:TextBox ID="txt_User" runat="server" CssClass="fieinput"></asp:TextBox>
                    </td>
                    <td rowspan="2">
                        <input id="Image1" type="image" src="images/lbtn.png" runat="server" onserverclick="btn_Login_Click" />
                    </td>
                </tr>
                <tr>
                    <td class="fiename">
                        密&nbsp;&nbsp;&nbsp;码：
                    </td>
                    <td>
                        <asp:TextBox ID="txt_Password" TextMode="Password" runat="server" CssClass="fieinput"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    </form>
</body>

