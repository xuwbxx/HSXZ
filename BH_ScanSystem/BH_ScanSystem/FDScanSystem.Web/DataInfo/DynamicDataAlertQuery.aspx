<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="DynamicDataAlertQuery.aspx.cs" Inherits="FDScanSystem.Web.DataInfo.DynamicDataAlertQuery" %>

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
            <asp:Literal ID="ltUrl" runat="server"></asp:Literal> > 超限采集数据管理
        </div>
    </div>
    <div class="a_t">
        <ul>
            <li>所属风机 </li>
            <li>
                <div class="hc_combox">
                    <dx:ASPxComboBox ID="cbxNode" runat="server" AutoPostBack="true" OnSelectedIndexChanged="select_Changed" Width="200px" EnableCallbackMode="false">
                    </dx:ASPxComboBox>
                </div>
            </li>
            <li>采集设备 </li>
            <li>
                <div class="hc_combox">
                    <dx:ASPxComboBox ID="cbxDevice" runat="server" Width="200px"   EnableCallbackMode="false">
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
    <div class="h_listtable">
        <div class="f_tabletool">
            <div class="hc_btn_location">
                <a class="f_btn-del" href="javascript:void(0);" onclick="Del(0);">删除</a>
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
                    HeaderStyle-HorizontalAlign="Center" Caption="采集设备">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn VisibleIndex="0" FieldName="PointName" CellStyle-HorizontalAlign="Center"
                    HeaderStyle-HorizontalAlign="Center" Caption="测点名称">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn VisibleIndex="0" FieldName="ScanValue" CellStyle-HorizontalAlign="Center"
                    HeaderStyle-HorizontalAlign="Center" Caption="监测值">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn VisibleIndex="0" FieldName="ScanLocation" CellStyle-HorizontalAlign="Center"
                    HeaderStyle-HorizontalAlign="Center" Caption="数据文件路径">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn VisibleIndex="0" FieldName="ScanTime" CellStyle-HorizontalAlign="Center"
                    HeaderStyle-HorizontalAlign="Center" Caption="采集时间">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataDateColumn VisibleIndex="0" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                    FieldName="CreateDate" Caption="创建时间">
                    <PropertiesDateEdit DisplayFormatString="yyyy/MM/dd">
                    </PropertiesDateEdit>
                </dx:GridViewDataDateColumn>
                <dx:GridViewDataTextColumn VisibleIndex="0" FieldName="ScanTime" CellStyle-HorizontalAlign="Center"
                    HeaderStyle-HorizontalAlign="Center" Caption="浏览">
                    <DataItemTemplate>
                        <a href="javascript:void(0)" onclick="ViewDetail(<%#Eval("DeviceId") %>,<%#Eval("ID") %>)">
                            浏览采集数据</a>
                              <%--<a href="DynamicDataAlertQueryDetail.aspx?device=<%#Eval("DeviceId") %>&data=<%#Eval("ID") %>">
                            浏览采集数据</a>--%>
                    </DataItemTemplate>
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
    <script language="javascript" type="text/javascript">
        function valid() {
            if (cliendGrid.GetSelectedRowCount() > 0)
            { return confirm("是否确认删除？"); }
            else {
                alert("未选择任何数据！");
                return false;
            }
        }

        function Del() {

            if (valid()) {
                return true;
            }
            else {
                return false;
            }
        }

        function ViewDetail(device, data) {
            return openChildDialog('采集数据浏览', 'DynamicDataAlertQueryDetail.aspx?device=' + device + "&data=" + data, 1200, 950);
        }


    </script>
</asp:Content>
