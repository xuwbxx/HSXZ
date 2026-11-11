<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="StaticDataChart.aspx.cs" Inherits="FDScanSystem.Web.DataInfo.StaticDataChart" %>

<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="h-screen-tab">
        <div class="ks_breadcrumb">
            <asp:Literal ID="ltUrl" runat="server"></asp:Literal>
            > 静态数据管理
        </div>
    </div>
    <div class="a_t">
        <ul>
            <li>所属风机 </li>
            <li>
                <div class="hc_combox">
                    <dx:ASPxComboBox ID="cbxNode" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Node_Select"
                        Width="200px" EnableCallbackMode="false">
                    </dx:ASPxComboBox>
                </div>
            </li>
            <li>监测类型 </li>
            <li>
                <div class="hc_combox">
                    <dx:ASPxComboBox ID="cbxProtocol" runat="server" AutoPostBack="true" Width="200px"
                        EnableCallbackMode="false" OnSelectedIndexChanged="Type_Select">
                    </dx:ASPxComboBox>
                </div>
            </li>
            <li>测点名称 </li>
            <li>
                <div class="hc_combox">
                    <%--<dx:ASPxTextBox ID="txtName" runat="server" Width="200px">
                    </dx:ASPxTextBox>--%>
                    <dx:ASPxComboBox ID="cbxPoint" runat="server" Width="200px" EnableCallbackMode="false">
                    </dx:ASPxComboBox>
                </div>
            </li>
            <li>
                <div class="hc_button-group1">
                    <asp:Button ID="btn_Choose" CssClass="ks_btn-submit1" runat="server" Text="搜索" />
                </div>
            </li>
        </ul>
        <ul>
            <li>起始时间 </li>
            <li>
                <div class="hc_combox">
                    <dx:ASPxDateEdit ID="txtBeginDate" runat="server" Width="200px">
                    </dx:ASPxDateEdit>
                </div>
            </li>
            <li>结束时间</li>
            <li>
                <div class="hc_combox">
                    <dx:ASPxDateEdit ID="txtEndDate" runat="server" Width="200px">
                    </dx:ASPxDateEdit>
                </div>
            </li>
        </ul>
    </div>
    <div>
        <asp:Chart ID="chart" runat="server" Width="1050px" Height="300px" Palette="BrightPastel"
            ImageType="Png" BorderDashStyle="Solid" BackSecondaryColor="White" BackGradientStyle="TopBottom"
            BorderWidth="2" BackColor="#D3DFF0">
            <Legends>
                <asp:Legend IsTextAutoFit="true" Alignment="Center" Docking="Bottom" Name="Default"
                    BackColor="Transparent" Font="Trebuchet MS, 8.25pt, style=Bold">
                </asp:Legend>
            </Legends>
            <BorderSkin SkinStyle="Emboss"></BorderSkin>
            <Series>
            </Series>
            <ChartAreas>
                <asp:ChartArea Name="ChartArea1" BorderColor="64, 64, 64, 64" BorderDashStyle="Solid"
                    BackSecondaryColor="White" BackColor="64, 165, 191, 228" ShadowColor="Transparent"
                    BackGradientStyle="TopBottom" IsSameFontSizeForAllAxes="True">
                    <Area3DStyle PointDepth="150" PointGapDepth="150" Rotation="15" Perspective="2" Inclination="20"
                        LightStyle="Realistic" WallWidth="0" IsClustered="False"></Area3DStyle>
                    <AxisY LineColor="64, 64, 64, 64">
                        <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                        <MajorGrid LineColor="64, 64, 64, 64" />
                    </AxisY>
                    <AxisX LineColor="64, 64, 64, 64">
                        <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                        <MajorGrid LineColor="64, 64, 64, 64" />
                    </AxisX>
                </asp:ChartArea>
            </ChartAreas>
        </asp:Chart>
        <br />
        <asp:Chart Visible="false" ID="chart2" runat="server" Height="300px" Width="1050px"
            ImageLocation="~/TempImages/ChartPic_#SEQ(300,3)" Palette="BrightPastel" ImageType="Png"
            BorderDashStyle="Solid" BackSecondaryColor="White" BackGradientStyle="TopBottom"
            BorderWidth="2" BackColor="#D3DFF0">
            <Legends>
                <asp:Legend IsTextAutoFit="true" Alignment="Center" Docking="Bottom" Name="Default"
                    BackColor="Transparent" Font="Trebuchet MS, 8.25pt, style=Bold">
                </asp:Legend>
            </Legends>
            <BorderSkin SkinStyle="Emboss"></BorderSkin>
            <Series>
            </Series>
            <ChartAreas>
                <asp:ChartArea Name="ChartArea2" BorderColor="64, 64, 64, 64" BorderDashStyle="Solid"
                    BackSecondaryColor="White" BackColor="64, 165, 191, 228" ShadowColor="Transparent"
                    BackGradientStyle="TopBottom" IsSameFontSizeForAllAxes="True">
                    <Area3DStyle PointDepth="150" PointGapDepth="150" Rotation="15" Perspective="2" Inclination="20"
                        LightStyle="Realistic" WallWidth="0" IsClustered="False"></Area3DStyle>
                    <AxisY LineColor="64, 64, 64, 64">
                        <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                        <MajorGrid LineColor="64, 64, 64, 64" />
                    </AxisY>
                    <AxisX LineColor="64, 64, 64, 64">
                        <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                        <MajorGrid LineColor="64, 64, 64, 64" />
                    </AxisX>
                </asp:ChartArea>
            </ChartAreas>
        </asp:Chart>
    </div>
</asp:Content>
