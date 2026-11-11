<%@ Page Title="" Language="C#" MasterPageFile="~/Dialog.Master" AutoEventWireup="true"
    CodeBehind="FileUpload_Edit.aspx.cs" Inherits="FDScanSystem.Web.DataInfo.FileUpload_Edit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="tablist2" width="100%" cellspacing="1" cellpadding="0">
        <tr>
            <td class="td1">
                报告名称<%-- <font color="red">*</font>--%>
            </td>
            <td class="td2">
                <asp:TextBox ID="txtName" runat="server">
                </asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="td1">
                报告类型
                <%--<font color="red">*</font>--%>
            </td>
            <td class="td2">
                <asp:DropDownList ID="drpScanType" runat="server">
                    <asp:ListItem Value="0" Text="月报" Selected="True">
                    </asp:ListItem>
                    <asp:ListItem Value="1" Text="年报"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="td1">
                文件上传
                <%--<font color="red">*</font>--%>
            </td>
            <td class="td2">
                <%--                <asp:HyperLink ID="hlLink" Target="_blank" runat="server"></asp:HyperLink>&nbsp;&nbsp;--%>
                <asp:FileUpload ID="upFile" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="td1" style="width: 125px">
                &nbsp;
            </td>
            <td class="td2" style="width: 342px; color: Blue;">
                文件仅支持 PDF格式, 文件大小限制于2M以内
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
