<%@ Page Title="" Language="C#" MasterPageFile="~/Dialog.Master" AutoEventWireup="true" CodeBehind="DataStaticEdit.aspx.cs" Inherits="FDScanSystem.Web.DataInfo.Dialog.DataStaticEdit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <table class="tablist2" width="100%" cellspacing="1" cellpadding="0">
        <tr>
            <td class="td1">
                监测值1<%-- <font color="red">*</font>--%>
            </td>
            <td class="td2">
                <asp:TextBox ID="txtValue5" runat="server">
                </asp:TextBox>
            </td>
            <td class="td1">
               监测值2
                <%--<font color="red">*</font>--%>
            </td>
            <td class="td2">
                <asp:TextBox ID="txtValue6" runat="server">
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
