<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ScanPeriodSetting.aspx.cs" Inherits="FDScanSystem.Web.Setting.ScanPeriodSetting" %>

<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxTabControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="a_title">
        <h1>
            采集周期配置
        </h1>
    </div>
    <div style="padding-left: 20px; padding-top: 10px; height: 30px;">
        <dx:ASPxTabControl ID="aspTab" ActiveTabIndex="0" Theme="DevEx" Width="100%" runat="server">
            <Tabs>
                <dx:Tab Text="静态监测采集周期配置" TabImage-IconID="actions_clip_16x16" Name="tabStatic">
                </dx:Tab>
                <dx:Tab Text="动态监测采集周期配置" NavigateUrl="ScanPeriodSetting2.aspx" TabImage-IconID="actions_convert_16x16" Name="tabDynamic">
                </dx:Tab>
            </Tabs>
        </dx:ASPxTabControl>
    </div>
    <div class="a_form">
        <table class="a_form-table">
            <tr>
                <th class="a_form-th">
                    <span class="req">*</span>起始日期(默认时间为00:00:00)
                </th>
                <td class="a_form-td">
                    <dx:ASPxCalendar ID="txtBeginDate" runat="server" Width="300px">
                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithText">
                            <RequiredField IsRequired="true" ErrorText="请输入起始日期" />
                        </ValidationSettings>
                    </dx:ASPxCalendar>
                </td>
            </tr>
            <tr>
                <th class="a_form-th">
                    <span class="req">*</span>间隔时间（分钟）
                </th>
                <td class="a_form-td">
                    <dx:ASPxSpinEdit ID="txtPeriod" runat="server" Height="30px" LargeIncrement="5" MaxValue="10000"
                        MinValue="3" NullText="3" Number="0" NumberType="Integer" Theme="DevEx" Width="250px">
                        <SpinButtons ShowLargeIncrementButtons="True">
                        </SpinButtons>
                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithText">
                            <RequiredField IsRequired="true" ErrorText="请输入间隔时间" />
                        </ValidationSettings>
                    </dx:ASPxSpinEdit>
                </td>
            </tr>
            <%--  <tr>
                <th class="a_form-th">
                    表头
                </th>
                <td class="a_form-td">
                    <input type="text" class="a_input-select" />
                    <span class="a_form-v"><span class="a_active"><i class="a_active-corner"></i>sadf dsaf
                        dsfa </span></span>
                </td>
            </tr>
            <tr>
                <th class="a_form-th">
                    表头
                </th>
                <td class="a_form-td">
                    <input type="text" class="a_input-date" /><span class="a_date-space">-</span><input
                        type="text" class="a_input-date" />
                    <span class="a_form-v"><span class="a_success">OK</span></span>
                </td>
            </tr>
            <tr>
                <th class="a_form-th">
                    表头
                </th>
                <td class="a_form-td">
                    <textarea class="a_textarea"></textarea>
                    <span class="a_form-v"><span class="a_success">OK</span></span>
                </td>
            </tr>
            <tr>
                <th class="a_form-th">
                    表头
                </th>
                <td class="a_form-td">
                    <input type="radio" class="a_input-radio" /><label>adfadsfsdfasdf</label><input type="radio"
                        class="a_input-radio" /><label>adfadsfsdfasdf</label>
                    <span class="a_form-v"><span class="a_success">OK</span></span>
                </td>
            </tr>
            <tr>
                <th class="a_form-th">
                    表头
                </th>
                <td class="a_form-td">
                    <input type="checkbox" class="a_input-checkbox" /><label>adfadsfsdfasdf</label><input
                        type="checkbox" class="a_input-checkbox" /><label>adfadsfsdfasdf</label>
                </td>
            </tr>
            <tr>
                <th class="a_form-th">
                    表头
                </th>
                <td class="a_form-td">
                    <div class="a_addimg">
                        <a href="#">
                            <img src="webapp/components/images/noimg.jpg" /></a></div>
                    <p>
                        最多上传十张照片，每张图片大小最大为5M</p>
                </td>
            </tr>--%>
            <tr>
                <th class="a_form-th">
                    &nbsp;
                </th>
                <td class="a_form-td">
                    <dx:ASPxButton ID="btnSubmit" Image-IconID="save_save_16x16" runat="server" Text="保存" Theme="iOS" 
                        onclick="btnSubmit_Click">
                    </dx:ASPxButton>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
