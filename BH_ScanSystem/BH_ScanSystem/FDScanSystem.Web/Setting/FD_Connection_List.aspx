<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FD_Connection_List.aspx.cs" Inherits="FDScanSystem.Web.Setting.FD_Commend_List" %>
<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

  <div class="h-screen-tab">
        <div class="ks_breadcrumb">
            <asp:Literal ID="ltUrl" runat="server"></asp:Literal> > 当前连接列表
        </div>
    </div>

     <div class="h_listtable">
        <%--<div class="f_tabletool">
            <div class="hc_btn_location">
                <asp:LinkButton ID="btn_Delete" CssClass="f_btn-del" OnClientClick="return valid();"
                    runat="server" Text="删除" OnClick="btn_Delete_Click">
                </asp:LinkButton>
            </div>
        </div>--%>
        <dx:ASPxGridView ID="aspGrid" runat="server" ClientInstanceName="grid" KeyFieldName="ID" Width="100%"
                runat="server" EnableCallBacks="False" Font-Names="微软雅黑" Font-Size="13px">
            <Styles>
                <Header BackColor="#f9f9f9">
                </Header>
            </Styles>
            <Columns>
               <%-- <dx:GridViewCommandColumn Caption="选择" Width="5%" ShowSelectCheckbox="True" VisibleIndex="0">
                    <HeaderStyle HorizontalAlign="Center" />
                </dx:GridViewCommandColumn>--%>
                <dx:GridViewDataTextColumn FieldName="ID" ReadOnly="True" VisibleIndex="0" Visible="False">
                    <EditFormSettings Visible="False" />
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn VisibleIndex="0" FieldName="RemoteEndPoint" Caption="IP地址" CellStyle-HorizontalAlign="Center"
                    HeaderStyle-HorizontalAlign="Center">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn VisibleIndex="0" FieldName="ConnectionDate" Caption="连接时间" CellStyle-HorizontalAlign="Center"
                    HeaderStyle-HorizontalAlign="Center">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn VisibleIndex="0" FieldName="Status" Caption="连接状态" CellStyle-HorizontalAlign="Center"
                    HeaderStyle-HorizontalAlign="Center">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataDateColumn VisibleIndex="0" FieldName="DisConnectDate" Caption="断开时间"
                    CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                    <PropertiesDateEdit DisplayFormatString="yyyy/MM/dd">
                    </PropertiesDateEdit>
                </dx:GridViewDataDateColumn>
            </Columns>
            <SettingsPager PageSize="30">
            </SettingsPager>
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

</asp:Content>
