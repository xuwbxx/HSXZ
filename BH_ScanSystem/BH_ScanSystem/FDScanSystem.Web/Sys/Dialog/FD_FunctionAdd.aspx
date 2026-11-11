<%@ Page Title="" Language="C#" MasterPageFile="~/Dialog.Master" AutoEventWireup="true" CodeBehind="FD_FunctionAdd.aspx.cs" Inherits="FDScanSystem.Web.FD_FunctionAdd" %>
<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<asp:Content ID="head1" runat="server" ContentPlaceHolderID="head">
</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
  
  

     <table class="tablist2" width="100%" cellspacing="1" cellpadding="0">
        <tr>
            <td class="td1">
                <font color="red">*</font> 功能名称
            </td>
            <td class="td2">
                <dx:ASPxTextBox ID="txtName" runat="server" Width="200px">
                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithText">
                            <RequiredField IsRequired="true" ErrorText="请填写信息" />
                        </ValidationSettings>
                    </dx:ASPxTextBox>
            </td>
            <td class="td1">
                显示顺序
            </td>
            <td class="td2">
            <dx:ASPxTextBox ID="txtSort" runat="server" Width="200px">
                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithText">
                            <RequiredField IsRequired="true" ErrorText="请填写信息" />
                        </ValidationSettings>
                    </dx:ASPxTextBox>
            </td>
        </tr>
        <tr>
            <td class="td1">
                功能地址
            </td>
            <td class="td2">
                 <dx:ASPxTextBox ID="txtUrl" runat="server" Width="200px">
                       <%-- <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithText">
                            <RequiredField IsRequired="true" ErrorText="请填写信息" />
                        </ValidationSettings>--%>
                    </dx:ASPxTextBox>
            </td>
            <td class="td1">
                父节点
            </td>
            <td class="td2" colspan="3">
               <asp:DropDownList ID="cbxGroup" runat="server" Width="200px"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="td1">
                <font color="red">*</font> 权限编码
            </td>
            <td  class="td2">
                <dx:ASPxTextBox ID="txtCode" runat="server" Width="200px">
                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="请输入权限编码" IsRequired="True" />
                    </ValidationSettings>
                </dx:ASPxTextBox>
            </td>
            <td class="td1">
                是否为菜单
            </td>
            <td class="td2">
                <asp:CheckBox ID="chkIsMenu" Width="95%" runat="server" Text="是否显示为菜单" />
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
