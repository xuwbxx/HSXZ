<%@ Page Title="" Language="C#" MasterPageFile="~/Dialog.Master" AutoEventWireup="true"
    CodeBehind="FD_ScanNode_Edit.aspx.cs" Inherits="FDScanSystem.Web.Setting.Dialog.FD_ScanNode_Edit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="tablist2" width="100%" cellspacing="1" cellpadding="0">
        <tr>
            <td class="td1">
                风机名称 <font color="red">*</font>
            </td>
            <td class="td2">
                <asp:TextBox ID="txtName" runat="server">
                </asp:TextBox>
            </td>
        </tr>
         <tr>
            <td class="td1">
                坐标X
            </td>
            <td class="td2">
                <asp:TextBox ID="txtX" Text="0" runat="server">
                </asp:TextBox>
            </td>
        </tr>
         <tr>
            <td class="td1">
                坐标Y
            </td>
            <td class="td2">
                <asp:TextBox ID="txtY" Text="0" runat="server">
                </asp:TextBox>
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
