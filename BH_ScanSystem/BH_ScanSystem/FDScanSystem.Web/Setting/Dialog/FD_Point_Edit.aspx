<%@ Page Title="" Language="C#" MasterPageFile="~/Dialog.Master" AutoEventWireup="true"
    CodeBehind="FD_Point_Edit.aspx.cs" Inherits="FDScanSystem.Web.Setting.Dialog.FD_Point_Edit" %>

<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="tablist2" width="100%" cellspacing="1" cellpadding="0">
        <tr>
            <td class="td1">
                测点名称<%-- <font color="red">*</font>--%>
            </td>
            <td class="td2">
                <asp:TextBox ID="txtName" runat="server">
                </asp:TextBox>
            </td>
            <td class="td1">
                传感器编号
                <%--<font color="red">*</font>--%>
            </td>
            <td class="td2">
                <asp:TextBox ID="txtRemark" runat="server">
                </asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="td1">
                通道1
                <%--<font color="red">*</font>--%>
            </td>
            <td class="td2">
                <asp:TextBox ID="txtChannel1" runat="server">
                </asp:TextBox>
            </td>
            <td class="td1">
                通道2
                <%--<font color="red">*</font>--%>
            </td>
            <td class="td2">
                <asp:TextBox ID="txtChannel2" runat="server">
                </asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="td1">
                单位名称1
                <%--<font color="red">*</font>--%>
            </td>
            <td class="td2">
                <asp:TextBox ID="txtUnit1" runat="server">
                </asp:TextBox>
            </td>
            <td class="td1">
                单位名称2
                <%--<font color="red">*</font>--%>
            </td>
            <td class="td2">
                <asp:TextBox ID="txtUnit2" runat="server">
                </asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="td1">
                最大值1
                <%--<font color="red">*</font>--%>
            </td>
            <td class="td2">
                <asp:TextBox ID="txtMax1" runat="server">
                </asp:TextBox>
            </td>
            <td class="td1">
                最大值2
                <%--<font color="red">*</font>--%>
            </td>
            <td class="td2">
                <asp:TextBox ID="txtMax2" runat="server">
                </asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="td1">
                最小值1
                <%--<font color="red">*</font>--%>
            </td>
            <td class="td2">
                <asp:TextBox ID="txtMin1" runat="server">
                </asp:TextBox>
            </td>
            <td class="td1">
                最小值2
                <%--<font color="red">*</font>--%>
            </td>
            <td class="td2">
                <asp:TextBox ID="txtMin2" runat="server">
                </asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="td1">
                坐标X
            </td>
            <td class="td2">
                <asp:TextBox ID="txtX" runat="server">
                </asp:TextBox>
            </td>
            <td class="td1">
                坐标Y
            </td>
            <td class="td2">
                <asp:TextBox ID="txtY" runat="server">
                </asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="td1">
                监测内容
                <%--<font color="red">*</font>--%>
            </td>
            <td class="td2">
                <asp:DropDownList ID="drpScanType" runat="server">
                </asp:DropDownList>
            </td>
            <td class="td1">
                所属风机
                <%--<font color="red">*</font>--%>
            </td>
            <td class="td2">
                <asp:DropDownList ID="drpNode" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpLayer_Select">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="td1">
                所属区域
            </td>
            <td class="td2">
                <asp:DropDownList ID="drpLayer" runat="server">
                </asp:DropDownList>
            </td>
            <td class="td1">
                对应传感器<%-- <font color="red">*</font>--%>
            </td>
            <td class="td2">
                <asp:DropDownList ID="drpScanPoint" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="td1">
                通讯设备
                <%--<font color="red">*</font>--%>
            </td>
            <td class="td2">
                <asp:DropDownList ID="drpCom" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpDevice_Select">
                </asp:DropDownList>
            </td>
            <td class="td1">
                监测设备
                <%--<font color="red">*</font>--%>
            </td>
            <td class="td2">
                <asp:DropDownList ID="drpDevice" AutoPostBack="false" OnSelectedIndexChanged="drpD_Select"
                    runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="td1">
                初始值<%-- <font color="red">*</font>--%>
            </td>
            <td class="td2">
                <asp:TextBox ID="txtOther" runat="server">
                </asp:TextBox>
            </td>
            <td class="td1">
                灵敏度<%-- <font color="red">*</font>--%>
            </td>
            <td class="td2">
                <asp:TextBox ID="txtOther2" runat="server">
                </asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="td1">
                初始值1<%-- <font color="red">*</font>--%>
            </td>
            <td class="td2">
                <asp:TextBox ID="txtpara1" runat="server">
                </asp:TextBox>
            </td>
            <td class="td1">
                初始值2<%-- <font color="red">*</font>--%>
            </td>
            <td class="td2">
                <asp:TextBox ID="txtpara2" runat="server">
                </asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="td1">
                初始值3<%-- <font color="red">*</font>--%>
            </td>
            <td class="td2">
                <asp:TextBox ID="txtpara3" runat="server">
                </asp:TextBox>
            </td>
            <td class="td1">
                初始值4<%-- <font color="red">*</font>--%>
            </td>
            <td class="td2">
                <asp:TextBox ID="txtpara4" runat="server">
                </asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="td1">
                初始值5<%-- <font color="red">*</font>--%>
            </td>
            <td class="td2">
                <asp:TextBox ID="txtpara5" runat="server">
                </asp:TextBox>
            </td>
            <td class="td1">
                初始值6<%-- <font color="red">*</font>--%>
            </td>
            <td class="td2">
                <asp:TextBox ID="txtpara6" runat="server">
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
    <script type="text/javascript">
    
    
    </script>
</asp:Content>
