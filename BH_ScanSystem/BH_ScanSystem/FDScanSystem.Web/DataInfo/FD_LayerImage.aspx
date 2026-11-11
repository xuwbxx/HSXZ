<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="FD_LayerImage.aspx.cs" Inherits="FDScanSystem.Web.DataInfo.FD_LayerImage" %>

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
            > 风机区域显示
        </div>
    </div>
    <div class="a_t">
        <ul>
            <li>风机名称：</li>
            <li>
                <div class="hc_combox">
                    <dx:ASPxComboBox ID="drpNode" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpLayer_Select">
                    </dx:ASPxComboBox>
                </div>
            </li>
            <li>监测内容 </li>
            <li>
                <div class="hc_combox">
                    <dx:ASPxComboBox ID="drpScanType" EnableCallbackMode="false" AutoPostBack="false"
                        runat="server">
                    </dx:ASPxComboBox>
                </div>
            </li>
            <li>监测区域 </li>
            <li>
                <div class="hc_combox">
                    <dx:ASPxComboBox ID="drpLayer" EnableCallbackMode="false" AutoPostBack="false" runat="server">
                    </dx:ASPxComboBox>
                </div>
            </li>
            <li>
                <div class="hc_button-group1">
                    <asp:Button ID="btn_Choose" CssClass="ks_btn-submit1" Width="100" runat="server"
                        Text="查看" OnClick="btn_Choose_Click" />
                </div>
            </li>
        </ul>
    </div>
    <div class="a_items">
        <h5>
        </h5>
        <div style="width: 100%;">
            <asp:Literal ID="ltDiv" runat="server"></asp:Literal>
            <asp:Image ID="img_Layer" runat="server" Width="100%" Height="400" />
        </div>
        <h5>
        </h5>
    </div>
</asp:Content>
