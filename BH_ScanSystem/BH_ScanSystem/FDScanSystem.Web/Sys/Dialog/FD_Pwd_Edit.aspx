<%@ Page Title="" Language="C#" MasterPageFile="~/Dialog.Master" AutoEventWireup="true" CodeBehind="FD_Pwd_Edit.aspx.cs" Inherits="FDScanSystem.Web.Sys.Dialog.FD_Pwd_Edit" %>
<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

 <table class="tablist2" width="100%" cellspacing="1" cellpadding="0">
        <tr>
            <td class="td1">
                请输入原始密码 <font color="red">*</font>
            </td>
            <td class="td2">
                <dx:ASPxTextBox ID="txtPwd" runat="server" NullText="原始密码" Password="true" MaxLength="100"
                    >
                    <ValidationSettings Display="Static" ErrorDisplayMode="ImageWithText">
                        <RequiredField IsRequired="true" ErrorText="请输入原始密码" />
                    </ValidationSettings>
                </dx:ASPxTextBox>
            </td>
        </tr>
        <tr>
            
            <td class="td1">
                请输入新密码 <font color="red">*</font>
            </td>
            <td class="td2">
                <dx:ASPxTextBox ID="txtNewPwd2" runat="server" NullText="新密码" Password="true" MaxLength="100"
                    >
                    <ValidationSettings Display="Static" ErrorDisplayMode="ImageWithText">
                        <RequiredField IsRequired="true" ErrorText="请输入新密码" />
                    </ValidationSettings>
                </dx:ASPxTextBox>
            </td>
            
        </tr>
        <tr>
        <td class="td1">
                请再次输入新密码<font color="red">*</font>
            </td>
            <td class="td2">
                <dx:ASPxTextBox ID="txtNewPwd" runat="server" NullText="确认新密码" Password="true" MaxLength="100"
                    >
                    <ValidationSettings Display="Static" ErrorDisplayMode="ImageWithText">
                        <RequiredField IsRequired="true" ErrorText="请输入新密码" />
                    </ValidationSettings>
                </dx:ASPxTextBox>
            </td>
        
        </tr>

        <tr>
            <td colspan="4" class="td3">
                <asp:Button ID="btnSave" runat="server" Text="保存" OnClick="btnSave_Click" class="save"
                    onmouseout="this.className='save'" OnClientClick="return check('form1')" onmouseover="this.className='save_hover'" />
                <asp:Button ID="btnClose" runat="server" Text="关闭" class="close" onmouseout="this.className='close'"
                    onmouseover="this.className='close_hover'" OnClientClick="return closeChildDialog();" />
            </td>
        </tr>
    </table>

</asp:Content>
