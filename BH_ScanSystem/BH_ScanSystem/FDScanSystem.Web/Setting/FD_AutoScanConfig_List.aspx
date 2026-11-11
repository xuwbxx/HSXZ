<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="FD_AutoScanConfig_List.aspx.cs" Inherits="FDScanSystem.Web.Setting.FD_AutoScanConfig_List" %>

<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="h-screen-tab">
        <div class="ks_breadcrumb">
            <asp:Literal ID="ltUrl" runat="server"></asp:Literal>
            > 静态采集设备配置
        </div>
    </div>
    <div class="a_t">
        <ul>
            <li>通讯设备选择： </li>
            <li>
                <div>
                    <dx:ASPxComboBox ID="cmb_Com" AutoPostBack="true" runat="server" OnSelectedIndexChanged="cmb_Com_SelectedIndexChanged">
                    </dx:ASPxComboBox>
                </div>
            </li>
            <li>监测设备选择：</li>
            <li>
                <div>
                    <dx:ASPxComboBox ID="cmb_Device" AutoPostBack="false" runat="server">
                    </dx:ASPxComboBox>
                </div>
            </li>
            <li>
                <div class="hc_button-group1">
                    <asp:Button ID="btn_Choose" CssClass="ks_btn-submit1" Width="60" runat="server" Text="搜索"
                        OnClick="btn_Choose_Click" />
                </div>
            </li>
        </ul>
    </div>
    <div class="a_title">
        <h1>
            自动监测配置
        </h1>
    </div>
    <div class="a_form">
        <table class="a_form-table">
            <tr>
                <th class="a_form-th">
                    采集频率：次
                </th>
                <td class="a_form-td">
                    <table>
                        <tr>
                            <td>
                                <dx:ASPxSpinEdit ID="drpDay" runat="server" Height="30px" Number="0" NumberType="Integer"
                                    LargeIncrement="10" Increment="1" MinValue="0" MaxValue="30" Width="100">
                                    <SpinButtons ShowLargeIncrementButtons="true">
                                    </SpinButtons>
                                </dx:ASPxSpinEdit>
                            </td>
                            <td>
                                &nbsp&nbsp&nbsp天&nbsp&nbsp&nbsp
                            </td>
                            <td>
                                <dx:ASPxSpinEdit ID="drpHour" runat="server" Height="30px" Number="0" NumberType="Integer"
                                    LargeIncrement="10" Increment="1" MinValue="0" MaxValue="59" Width="100">
                                    <SpinButtons ShowLargeIncrementButtons="true">
                                    </SpinButtons>
                                </dx:ASPxSpinEdit>
                            </td>
                            <td>
                                &nbsp&nbsp&nbsp时&nbsp&nbsp&nbsp
                            </td>
                            <td>
                                <dx:ASPxSpinEdit ID="drpMin" runat="server" Height="30px" Number="0" NumberType="Integer"
                                    LargeIncrement="10" Increment="1" MinValue="0" MaxValue="59" Width="100">
                                    <SpinButtons ShowLargeIncrementButtons="true">
                                    </SpinButtons>
                                </dx:ASPxSpinEdit>
                            </td>
                            <td>
                                &nbsp&nbsp&nbsp分&nbsp&nbsp&nbsp
                            </td>
                            <td>
                                <dx:ASPxButton ID="btn_Time" runat="server" Text="采样频率设置" OnClick="btn_Time_Click">
                                </dx:ASPxButton>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <th class="a_form-th">
                    修改模块时间
                </th>
                <td class="a_form-td">
                    <table>
                        <tr>
                            <td>
                                <dx:ASPxDateEdit ID="aspxNowDate" Height="30" ShowShadow="false" Width="100%" runat="server">
                                    <CalendarProperties TodayButtonText="今天" ClearButtonText="清除">
                                        <HeaderStyle Spacing="1px" />
                                        <FooterStyle Spacing="17px" />
                                    </CalendarProperties>
                                </dx:ASPxDateEdit>
                            </td>
                            <td>
                                &nbsp &nbsp &nbsp
                            </td>
                            <td>
                                <dx:ASPxTimeEdit ID="aspxNowTime" Width="100" runat="server" Height="30" DisplayFormatString="HH:mm:ss"
                                    EditFormatString="HH:mm:ss" DateTime="2012-01-01 12:00:00" runat="server">
                                </dx:ASPxTimeEdit>
                            </td>
                            <td>
                                &nbsp &nbsp &nbsp
                            </td>
                            <td>
                                <dx:ASPxButton ID="btn_NowTime" runat="server" Text="模块时间设置" OnClick="btn_NowTime_Click">
                                </dx:ASPxButton>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <th class="a_form-th">
                    采样计时起点
                </th>
                <td class="a_form-td">
                    <table>
                        <tr>
                            <td>
                                <dx:ASPxTimeEdit ID="aspxTimeRead" Width="100" Height="30" runat="server" DisplayFormatString="HH:mm"
                                    EditFormatString="HH:mm" DateTime="2012-01-01 12:00">
                                </dx:ASPxTimeEdit>
                            </td>
                            <td>
                                &nbsp &nbsp &nbsp
                            </td>
                            <td>
                                <dx:ASPxButton ID="aspxEdit" Width="100" runat="server" Text="计时起点设置" OnClick="aspxEdit_Click">
                                </dx:ASPxButton>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
