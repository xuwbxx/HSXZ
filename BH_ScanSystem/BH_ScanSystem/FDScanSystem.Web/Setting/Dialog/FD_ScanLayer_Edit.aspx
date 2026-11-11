<%@ Page Title="" Language="C#" MasterPageFile="~/Dialog.Master" AutoEventWireup="true"
    CodeBehind="FD_ScanLayer_Edit.aspx.cs" Inherits="FDScanSystem.Web.Setting.Dialog.FD_ScanLayer_Edit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="tablist2" width="100%" cellspacing="1" cellpadding="0">
        <tr>
            <td class="td1">
                区域名称 <font color="red">*</font>
            </td>
            <td class="td2">
                <asp:TextBox ID="txtName" runat="server">
                </asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="td1">
                所属风机
                <%--<font color="red">*</font>--%>
            </td>
            <td class="td2">
                <asp:DropDownList ID="drpNode" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <th class="td1">
                背景图上传
            </th>
            <td class="td2">
                <asp:FileUpload ID="upImage" Height="29" Width="330px" runat="server" />
                <asp:Button ID="btn_Upload" CssClass="a_addimg" runat="server" Text="上传" OnClick="btn_Upload_Click" />
                <asp:HyperLink ID="img" runat="server" Target="_blank" Height="28" Width="50px" Text="预览"
                    CssClass="lightBox">
                </asp:HyperLink>
                <script type="text/javascript">
                    jQuery(function () {
                        $('.lightBox').lightBox();
                    })
                </script>
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
