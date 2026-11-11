<%@ Page Title="" Language="C#" MasterPageFile="~/Dialog.Master" AutoEventWireup="true" CodeBehind="FD_UserEdit.aspx.cs" Inherits="FDScanSystem.Web.Sys.FD_UserEdit" %>

<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <table class="tablist2" width="100%" cellspacing="1" cellpadding="0">
        <tr>
            <td class="td1">
                <font color="red">*</font> 用户名
            </td>
            <td class="td2">
                <dx:ASPxTextBox ID="txtName" runat="server" Width="200px">
                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithText">
                            <RequiredField IsRequired="true" ErrorText="请填写信息" />
                        </ValidationSettings>
                    </dx:ASPxTextBox>
            </td>
            <td class="td1">
                密码
            </td>
            <td class="td2">
            <dx:ASPxTextBox NullText="******" ID="txtPwd" Password="true"  runat="server" Width="200px">
                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithText">
                            <RequiredField IsRequired="true" ErrorText="请填写信息" />
                        </ValidationSettings>
                    </dx:ASPxTextBox>
            </td>
        </tr>
        <tr>
            <td class="td1">
                真实姓名
            </td>
            <td class="td2" colspan="3">
               <dx:ASPxTextBox ID="txtRealName" runat="server" Width="200px">
                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithText">
                            <RequiredField IsRequired="true" ErrorText="请填写信息" />
                        </ValidationSettings>
                    </dx:ASPxTextBox>
            </td>
        </tr>
        
        <tr>
            <td colspan="4" class="td3">
                <asp:Button ID="btnSave" runat="server" Text="保存" OnClick="btnSave_Click" class="save"
                    onmouseout="this.className='save'" onmouseover="this.className='save_hover'"
                    OnClientClick="return check('form1')" />
                <asp:Button ID="btnClose" runat="server" Text="关闭" class="close" onmouseout="this.className='close'"
                    onmouseover="this.className='close_hover'" OnClientClick="return closeChildDialog();" />
            </td>
        </tr>
    </table>

</asp:Content>
