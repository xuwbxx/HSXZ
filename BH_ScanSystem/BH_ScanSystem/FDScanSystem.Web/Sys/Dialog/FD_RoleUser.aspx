<%@ Page Title="" Language="C#" MasterPageFile="~/Dialog.Master" AutoEventWireup="true" CodeBehind="FD_RoleUser.aspx.cs" Inherits="FDScanSystem.Web.Sys.Dialog.FD_RoleUser" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

 <table class="tablist2" width="100%" cellspacing="1" cellpadding="0">
        <tr>
            <td class="td2">
                角色列表: 当前用户【<asp:Label ID="lbUser" runat="server"></asp:Label>
                】 &nbsp;&nbsp;&nbsp;<asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click"
                    OnClientClick="return confirm('您确定保存吗？');" Text="保存" class="save" onmouseout="this.className='save'"
                    onmouseover="this.className='save_hover'" />
                <asp:Button ID="btnClose" runat="server" Text="关闭" class="close" onmouseout="this.className='close'"
                    onmouseover="this.className='close_hover'" OnClientClick="return closeChildDialog();" />
            </td>
        </tr>
    </table>
    <table class="tablist2" width="100%" cellspacing="1" cellpadding="0">
        <tr>
            <td class="td2">
                <asp:CheckBoxList ID="cklRole" runat="server" RepeatDirection="Vertical" DataTextField="ROLENAME"
                    DataValueField="ID">
                </asp:CheckBoxList>
            </td>
        </tr>
    </table>

</asp:Content>
