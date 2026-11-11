<%@ Page Title="" Language="C#" MasterPageFile="~/Dialog.Master" AutoEventWireup="true" CodeBehind="FD_CommunicationDevice_Edit.aspx.cs" Inherits="FDScanSystem.Web.Setting.Dialog.FD_CommunicationDevice_Edit" %>
<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<table class="tablist2" width="100%" cellspacing="1" cellpadding="0">
        <tr>
            <td class="td1">
                通讯名称<%-- <font color="red">*</font>--%>
            </td>
            <td class="td2">
                <asp:TextBox ID="txtName" runat="server" >
                </asp:TextBox>
            </td>
             <td class="td1">
                通讯代码 <%--<font color="red">*</font>--%>
            </td>
            <td class="td2">
                <asp:TextBox ID="txtCode"  runat="server" >
                </asp:TextBox>
            </td>
            </tr>
            <tr>
            <td class="td1">
                通讯类型 <%--<font color="red">*</font>--%>
            </td>
            <td class="td2">
                <asp:DropDownList ID="drpType" runat="server">
                <asp:ListItem Value="0" Text="TCP" Selected="True"></asp:ListItem>
                <asp:ListItem Value="1" Text="COM"></asp:ListItem>
                </asp:DropDownList>
            </td>
            <td class="td1">
                通讯协议 <%--<font color="red">*</font>--%>
            </td>
            <td class="td2">
                <asp:DropDownList ID="drpProtocol" runat="server">
                <asp:ListItem Value="0" Text="默认协议" Selected="True"></asp:ListItem>
                <asp:ListItem Value="1" Text="自定义协议"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
         <tr>
            <td class="td1">
                IP地址<%-- <font color="red">*</font>--%>
            </td>
            <td class="td2"  colspan="3">
                <asp:TextBox ID="txtIP" runat="server" >
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
