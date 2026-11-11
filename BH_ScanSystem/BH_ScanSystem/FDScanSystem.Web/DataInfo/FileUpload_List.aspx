<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="FileUpload_List.aspx.cs" Inherits="FDScanSystem.Web.DataInfo.FileUpload_List" %>

<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="h-screen-tab">
        <div class="ks_breadcrumb">
            <asp:Literal ID="ltUrl" runat="server"></asp:Literal>
            > 测点管理列表
        </div>
    </div>
    <div class="a_t">
        <ul>
            <li>报告名称：</li>
            <li>
                <div class="hc_combox">
                    <dx:ASPxTextBox ID="txtName" runat="server">
                    </dx:ASPxTextBox>
                </div>
            </li>
            <li>报告类型：</li>
            <li>
                <div class="hc_combox">
                    <dx:ASPxComboBox ID="drpScanType" runat="server">
                        <Items>
                            <dx:ListEditItem Value="0" Selected="true" Text="月报" />
                            <dx:ListEditItem Value="1" Text="年报" />
                        </Items>
                    </dx:ASPxComboBox>
                </div>
            </li>
            <li>
                <div class="hc_button-group1">
                    <asp:Button ID="btn_Choose" CssClass="ks_btn-submit1" Width="100" runat="server"
                        Text="搜索" OnClick="btn_Choose_Click" />
                </div>
            </li>
        </ul>
    </div>
    <div class="h_listtable">
        <div class="f_tabletool">
            <div class="hc_btn_location">
                <a class="f_btn-add" href="javascript:void(0);" onclick="SetEdit(0);">添加</a>
                <asp:Button ID="btnRefresh" ClientIDMode="Static" runat="server" Style="display: none;"
                    OnClick="btnRefresh_CLick" />
                <%-- <asp:LinkButton ID="btn_Delete" CssClass="f_btn-add" runat="server" Text="删除" OnClick="btn_Delete_Click"
                    OnClientClick="return delt();">
                </asp:LinkButton>--%>
            </div>
        </div>
        <dx:ASPxGridView ID="aspGrid" ClientInstanceName="grid" KeyFieldName="ID" Width="100%"
            runat="server" EnableCallBacks="False" Font-Names="微软雅黑" Font-Size="13px" OnRowDeleted="aspGrid_RowDeleted"
            OnRowDeleting="aspGrid_RowDeleting" AutoGenerateColumns="False">
            <Styles>
                <Header BackColor="#f9f9f9">
                </Header>
                <FilterRow BackColor="White">
                </FilterRow>
            </Styles>
            <Columns>
                <dx:GridViewDataTextColumn FieldName="ID" ReadOnly="True" VisibleIndex="0" Visible="False">
                    <EditFormSettings Visible="False" />
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn VisibleIndex="0" FieldName="FileName" Caption="报告名称" CellStyle-HorizontalAlign="Center"
                    HeaderStyle-HorizontalAlign="Center">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn VisibleIndex="0" FieldName="TypeName" Caption="监测类型" CellStyle-HorizontalAlign="Center"
                    HeaderStyle-HorizontalAlign="Center">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataDateColumn VisibleIndex="0" FieldName="CreateDate" Caption="创建时间"
                    CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                    <PropertiesDateEdit DisplayFormatString="yyyy/MM/dd">
                    </PropertiesDateEdit>
                </dx:GridViewDataDateColumn>
                <dx:GridViewDataColumn Caption="操作" HeaderStyle-HorizontalAlign="Center">
                    <DataItemTemplate>
                        <a href="<%# Eval("FileLocation") %>" class="a_edit" title="编辑">下载</a> <a href="javascript:void(0)"
                            class="a_edit" title="编辑" onclick="SetEdit(<%#Eval("ID") %>)">编辑</a> <a href="javascript:void(0)"
                                class="a_del" title="删除" onclick="if(!window.confirm('是否确定删除？')){return false;}grid.DeleteRowByKey(<%#Eval("ID") %>); ">
                                删除</a>
                    </DataItemTemplate>
                    <CellStyle HorizontalAlign="Center">
                    </CellStyle>
                </dx:GridViewDataColumn>
            </Columns>
            <Settings ShowGroupPanel="false" ShowFilterBar="Hidden" ShowFilterRow="false" />
            <SettingsPager PageSize="13">
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
    <script language="javascript">
        function delt()
        { return confirm("确定删除此测点及其所有数据？"); }

        function SetEdit(planId) {
            return openChildDialog('报告上传编辑', '/DataInfo/FileUpload_Edit.aspx?cid=' + planId, 600, 400);
        }


    </script>
</asp:Content>
