<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="DeviceList.aspx.cs" Inherits="FDScanSystem.Web.Setting.DeviceList" %>

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
            <asp:Literal ID="ltUrl" runat="server"></asp:Literal> > 监测设备列表
        </div>
    </div>
    <div class="a_t">
        <ul>
            <li>所属风机 </li>
            <li>
                <div class="hc_combox">
                    <dx:ASPxComboBox ID="cbxNode" runat="server" Width="150px" EnableCallbackMode="false">
                    </dx:ASPxComboBox>
                </div>
            </li>
            <li>设备类型 </li>
            <li>
                <div class="hc_combox">
                    <dx:ASPxComboBox ID="cbxProtocol" runat="server" Width="150px" EnableCallbackMode="false">
                    </dx:ASPxComboBox>
                </div>
            </li>
            <li>设备名称 </li>
            <li>
                <div class="hc_combox">
                    <dx:ASPxTextBox ID="txtName" runat="server">
                    </dx:ASPxTextBox>
                </div>
            </li>
            <li>
                <div class="hc_button-group1">
                    <asp:Button ID="btn_Choose" CssClass="ks_btn-submit1" runat="server" Text="搜索" />
                </div>
            </li>
        </ul>
    </div>
    <div class="h_listtable">
        <div class="f_tabletool">
            <div class="hc_btn_location">
                <a class="f_btn-add" href="javascript:void(0);" onclick="SetEdit(0);">添加</a>

                <asp:LinkButton ID="btnProcess" OnClick="btnProcess_Click" CssClass="f_btn-add" runat="server" Text="发送配置指令"></asp:LinkButton>

                <asp:Button ID="btnRefresh" ClientIDMode="Static" runat="server" Style="display: none;"
                    OnClick="btnRefresh_CLick" />
            </div>
        </div>
        <dx:ASPxGridView ID="aspGrid" AutoGenerateColumns="False" KeyFieldName="ID" Width="100%"
            runat="server" ClientInstanceName="cliendGrid" OnRowDeleted="aspGrid_RowDeleted"
            OnRowDeleting="aspGrid_RowDeleting">
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
                    HeaderStyle-HorizontalAlign="Center" Caption="设备名称">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn VisibleIndex="0" FieldName="CommunicationName" CellStyle-HorizontalAlign="Center"
                    HeaderStyle-HorizontalAlign="Center" Caption="通讯设备名称">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn VisibleIndex="0" FieldName="ProtocolName" CellStyle-HorizontalAlign="Center"
                    HeaderStyle-HorizontalAlign="Center" Caption="设备类型">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn VisibleIndex="0" FieldName="DeviceIdentity" CellStyle-HorizontalAlign="Center"
                    HeaderStyle-HorizontalAlign="Center" Caption="设备标识">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataDateColumn VisibleIndex="0" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                    FieldName="CreateDate" Caption="创建时间">
                    <PropertiesDateEdit DisplayFormatString="yyyy/MM/dd">
                    </PropertiesDateEdit>
                </dx:GridViewDataDateColumn>
                <dx:GridViewDataTextColumn Caption="操作" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                    VisibleIndex="7" Width="10%">
                    <HeaderStyle HorizontalAlign="Center" />
                    <CellStyle HorizontalAlign="Center">
                    </CellStyle>
                    <DataItemTemplate>
                        <table width="100%">
                            <tr>
                                <td align="center">
                                    <a href="javascript:void(0);" onclick="SetEdit(<%#Eval("ID") %>);">编辑</a>
                                </td>
                                <td align="center">
                                    <a href="javascript:void(0)" class="a_del" title="删除" onclick="if(!window.confirm('是否确定删除？')){return false;}cliendGrid.DeleteRowByKey(<%#Eval("ID") %>); ">
                                        删除</a>
                                </td>
                            </tr>
                        </table>
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

        function SetEdit(planId) {

            return openChildDialog(planId == 0 ? '设备新增' : '设备编辑', 'dialog/DeviceEdit.aspx?device=' + planId, 800, 400);
        }


    </script>
</asp:Content>
