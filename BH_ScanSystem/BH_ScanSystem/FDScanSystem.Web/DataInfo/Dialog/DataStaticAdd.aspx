<%@ Page Title="" Language="C#" MasterPageFile="~/Dialog.Master" AutoEventWireup="true" CodeBehind="DataStaticAdd.aspx.cs" Inherits="FDScanSystem.Web.DataInfo.Dialog.DataStaticAdd" %>
<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
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
            <td class="td1">
                监测时间<%-- <font color="red">*</font>--%>
            </td>
            <td class="td2">
              <dx:ASPxDateEdit ID="drpDate" runat="server" ClientInstanceName="drpDate">
              </dx:ASPxDateEdit>
            </td>
            <td class="td1">时间</td>
            <td class="td2"><asp:DropDownList ID="drpHour" runat="server"></asp:DropDownList>时<asp:DropDownList ID="drpMin" runat="server"></asp:DropDownList>分</td>
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
