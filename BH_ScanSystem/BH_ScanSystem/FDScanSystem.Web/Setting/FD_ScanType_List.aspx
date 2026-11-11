<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FD_ScanType_List.aspx.cs" Inherits="FDScanSystem.Web.Setting.FD_ScanType_List" %>
<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>

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
           <%-- <a href="/Index.aspx">首页</a>--%><asp:Literal ID="ltUrl" runat="server"></asp:Literal> > 监测类型列表
        </div>
    </div>

    <div class="h_listtable">
        <div class="f_tabletool">
            <div class="hc_btn_location">
                <a class="f_btn-add" href="javascript:void(0);" onclick="SetEdit(0);">添加</a>
     
                 <asp:Button ID="btnRefresh" ClientIDMode="Static" runat="server" Style="display: none;"
                        OnClick="btnRefresh_CLick" />
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
                    <dx:GridViewDataTextColumn Visible="false" FieldName="ID">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="监测类型名称" FieldName="DictValue">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataColumn Caption="操作" Width="200">
                        <DataItemTemplate>
<a href="javascript:void(0)" class="a_edit" title="编辑" onclick="SetEdit(<%#Eval("ID") %>)">
                                            编辑</a>
<a href="javascript:void(0)" class="a_del" title="删除" onclick="if(!window.confirm('是否确定删除？')){return false;}grid.DeleteRowByKey(<%#Eval("ID") %>); ">
                                            删除</a>
    
                        </DataItemTemplate>
                    </dx:GridViewDataColumn>
                </Columns>
                   <Settings ShowGroupPanel="false" />
                <Styles>
                    <Header HorizontalAlign="Center">
                    </Header>
                    <Cell HorizontalAlign="Center">
                    </Cell>
                    <Row BackColor="#f5f9fc">
                    </Row>
                    <AlternatingRow BackColor="#f0f5f8">
                    </AlternatingRow>
                </Styles>
                <StylesEditors>
                    <TextBox Paddings-PaddingTop="5" Paddings-PaddingBottom="5">
                    </TextBox>
                    <ButtonEdit Paddings-PaddingTop="5" Paddings-PaddingBottom="5">
                    </ButtonEdit>
                    <DropDownWindow Paddings-PaddingTop="5" Paddings-PaddingBottom="5">
                    </DropDownWindow>
                </StylesEditors>
                <SettingsBehavior ConfirmDelete="true" />
                <SettingsPager Mode="ShowPager" PageSize="15" RenderMode="Lightweight">
                </SettingsPager>
                <SettingsText ConfirmDelete="是否删除所选择的记录?" EmptyDataRow="没有查询到任何符合条件的信息" CommandEdit="编辑"
                    CommandNew="新增" CommandDelete="删除" CommandCancel="取消" CommandUpdate="保存" CommandClearFilter="清除">
                </SettingsText>
            </dx:ASPxGridView>
    </div>
    <script language="javascript">
        function valid() {
            if (grid.GetSelectedRowCount() > 0)
            { return confirm("是否确认删除？"); }
            else {
                alert("未选择任何数据！");
                return false;
            }
        }

        function SetEdit(planId) {
            return openChildDialog('监测内容编辑', '/Setting/Dialog/FD_ScanType_Edit.aspx?cid=' + planId, 600, 300);
        }



    </script>

</asp:Content>
