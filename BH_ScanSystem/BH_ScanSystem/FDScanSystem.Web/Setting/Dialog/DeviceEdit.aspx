<%@ Page Title="" Language="C#" MasterPageFile="~/Dialog.Master" AutoEventWireup="true"
    CodeBehind="DeviceEdit.aspx.cs" Inherits="FDScanSystem.Web.Setting.Dialog.DeviceEdit" %>

<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="tablist2" width="100%" cellspacing="1" cellpadding="0">
        <tr>
            <td class="td1">
                <font color="red">*</font>所属风机
            </td>
            <td class="td2">
                <dx:ASPxComboBox ID="cbxNode" runat="server" Width="200px">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="请选择所属风机" IsRequired="true" />
                    </ValidationSettings>
                </dx:ASPxComboBox>
            </td>
            <td class="td1">
                <font color="red">*</font>设备名称
            </td>
            <td class="td2">
                <dx:ASPxTextBox ID="txtDeviceName" runat="server" Width="200px">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="请输入设备名称" IsRequired="true" />
                    </ValidationSettings>
                </dx:ASPxTextBox>
            </td>
        </tr>
        <tr>
            <td class="td1">
                <font color="red">*</font>通讯设备
            </td>
            <td class="td2">
                <dx:ASPxComboBox ID="cbxCommunication" runat="server" Width="200px">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="请选择通讯设备" IsRequired="true" />
                    </ValidationSettings>
                </dx:ASPxComboBox>
            </td>
            <td class="td1">
                <font color="red">*</font>设备类型
            </td>
            <td class="td2">
                <dx:ASPxComboBox ID="cbxDeviceProtocol" runat="server" Width="200px">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="请选择设备类型" IsRequired="true" />
                    </ValidationSettings>
                </dx:ASPxComboBox>
            </td>
        </tr>
        <tr>
            <td class="td1">
                <font color="red">*</font>设备标识<br />
                (如MCU即00001,
                <br />
                加速采集即USB-4716,BID#0)
            </td>
            <td class="td2">
                <dx:ASPxTextBox ID="txtDeviceIdentity" Width="200px" runat="server">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="请输入设备标识" IsRequired="true" />
                    </ValidationSettings>
                </dx:ASPxTextBox>
            </td>
            <td class="td1">
                采集周期
            </td>
            <td class="td2">
                <table width="300px">
                    <tr>
                        <td class="td1">
                            采样间隔(毫秒 ms)
                        </td>
                        <td class="td1">
                            采样长度(时间 毫秒 ms)
                        </td>
                    </tr>
                    <tr>
                        <td class="td2">
                            <dx:ASPxTextBox ID="txtPeriod" Width="80px" runat="server">
                            </dx:ASPxTextBox>
                        </td>
                        <td class="td2">
                            <dx:ASPxTextBox ID="txtHowLong" Width="80px" runat="server">
                            </dx:ASPxTextBox>
                        </td>
                    </tr>
                </table>
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
