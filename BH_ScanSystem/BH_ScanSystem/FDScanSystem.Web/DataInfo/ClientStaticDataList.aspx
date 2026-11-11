<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ClientStaticDataList.aspx.cs" Inherits="FDScanSystem.Web.DataInfo.ClientStaticDataList" %>
<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            width: 100px;
        }
    </style>
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
                        OnSelectedIndexChanged="Type_Select" EnableCallbackMode="false">
                    </dx:ASPxComboBox>
                </div>
            </li>
            <li>测点名称 </li>
            <li>
                <div class="hc_combox">
                    <%--<dx:ASPxTextBox ID="txtName" runat="server" Width="200px">
                    </dx:ASPxTextBox>--%>
                    <dx:ASPxComboBox ID="cbxPoint" ClientInstanceName="cbxPoint" runat="server" Width="200px" EnableCallbackMode="false">
                    </dx:ASPxComboBox>
                </div>
            </li>
            <li>
                <div class="hc_button-group1">
                    <asp:Button ID="btn_Choose" CssClass="ks_btn-submit1" runat="server" Text="搜索" />
                    <asp:Button ID="Button1" ClientIDMode="Static" runat="server" Style="display: none;"
                        OnClick="btnRefresh_CLick" />
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
    <div class="h_listtable">
        <div class="f_tabletool">
            <div class="hc_btn_location">
<%--                <a class="f_btn-add" href="javascript:void(0);" onclick="Add();">新增</a>
                <a class="f_btn-del" href="javascript:void(0);" onclick="Del(0);">删除</a>--%>
                <%-- <a class="f_btn-exp" href="javascript:void(0);" onclick="Export(0);">导出</a>--%>
                <asp:LinkButton ID='lkExport' Text="导出" runat="server" class="f_btn-exp" UseSubmitBehavior="False"
                    OnClick="btnXlsxExport1_Click"></asp:LinkButton>
                <asp:Button ID="btnRefresh" ClientIDMode="Static" runat="server" Style="display: none;"
                    OnClick="btnRefresh_CLick" />
            </div>
        </div>
        <dx:ASPxGridView ID="aspGrid" AutoGenerateColumns="False" KeyFieldName="ID" Width="100%"
            runat="server" ClientInstanceName="cliendGrid">
            <Styles>
                <Header BackColor="#f9f9f9">
                </Header>
                <FilterRow BackColor="White">
                </FilterRow>
            </Styles>
            <Columns>
                <dx:GridViewDataTextColumn VisibleIndex="0" FieldName="NodeName" CellStyle-HorizontalAlign="Center"
                    HeaderStyle-HorizontalAlign="Center" Caption="所属风机">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn VisibleIndex="0" FieldName="DeviceName" CellStyle-HorizontalAlign="Center"
                    HeaderStyle-HorizontalAlign="Center" Caption="监测设备">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn VisibleIndex="0" FieldName="ProtocolName" CellStyle-HorizontalAlign="Center"
                    HeaderStyle-HorizontalAlign="Center" Caption="监测点名称">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn VisibleIndex="0" FieldName="ScanValue5" CellStyle-HorizontalAlign="Center"
                    HeaderStyle-HorizontalAlign="Center" Caption="监测值1">
                  <PropertiesTextEdit DisplayFormatString="F4"></PropertiesTextEdit>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn VisibleIndex="0" FieldName="ScanValue6" CellStyle-HorizontalAlign="Center"
                    HeaderStyle-HorizontalAlign="Center" Caption="监测值2">
                     <PropertiesTextEdit DisplayFormatString="F4"></PropertiesTextEdit>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn VisibleIndex="0" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                    FieldName="CreateDate" Caption="创建时间">
                    <%--                <propertiesdateedit DisabledDates="" displayformatstring="yyyy/MM/dd hh:mm:ss">
                    </propertiesdateedit>--%>
                </dx:GridViewDataTextColumn>



            </Columns>
            <Settings ShowGroupPanel="false" ShowFilterBar="Hidden" ShowFilterRow="false" />
            <SettingsPager PageSize="30">
            </SettingsPager>
            <Settings ShowFilterRow="false" />
            <StylesEditors>
                <CalendarHeader Spacing="1px">
                </CalendarHeader>
                <ProgressBar Height="25px">
                </ProgressBar>
            </StylesEditors>
        </dx:ASPxGridView>
    </div>
    <dx:ASPxGridViewExporter ID="gridExport" runat="server" GridViewID="aspGrid">
    </dx:ASPxGridViewExporter>
    <script language="javascript" type="text/javascript">
        function valid() {
            if (cliendGrid.GetSelectedRowCount() > 0)
            { return confirm("是否确认删除？"); }
            else {
                alert("未选择任何数据！");
                return false;
            }
        }

        function Add(planId) {
            // window.alert(cbxPoint.GetValue().toString());
            return openChildDialog('监测数据新增', '/DataInfo/Dialog/DataStaticAdd.aspx?pid=' + cbxPoint.GetValue().toString(), 600, 300);
        }

        function Del() {

            if (valid()) {
                return true;
            }
            else {
                return false;
            }
        }

        function SetEdit() {
            return openChildDialog('监测数据编辑', '/DataInfo/Dialog/DataStaticEdit.aspx?cid=' + planId, 600, 300);
        }

        function Export() {

        }


    </script>
</asp:Content>
