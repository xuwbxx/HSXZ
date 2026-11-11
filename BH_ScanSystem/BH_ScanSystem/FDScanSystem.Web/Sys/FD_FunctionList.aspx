<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FD_FunctionList.aspx.cs" Inherits="FDScanSystem.Web.FD_FunctionList" %>
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
            <%--<a href="/Index.aspx">首页</a>--%><asp:Literal ID="ltUrl" runat="server"></asp:Literal> > 功能列表
        </div>
    </div>
 <%--   <div class="a_t">
        <ul>
            <li>所属项目选择 </li>
            <li>
                <div class="hc_combox">
                    <dx:ASPxComboBox ID="cmb_Chosen" runat="server" Width="400px" EnableCallbackMode="false">
                    </dx:ASPxComboBox>
                </div>
            </li>
            <li>
                <div class="hc_button-group1">
                    <asp:Button ID="btn_Choose" CssClass="ks_btn-submit1" Width="35" runat="server" OnClick="btn_Choose_Click"
                        Text="搜索" />
                </div>
            </li>
        </ul>
    </div>--%>
    <div class="h_listtable">
        <div class="f_tabletool">
            <div class="hc_btn_location">
                <%--<asp:LinkButton ID="btn_Add" CssClass="f_btn-add" runat="server" OnClick="btn_Add_Click"
                    Text="添加">
                </asp:LinkButton>--%>
                <a class="f_btn-add" href="javascript:void(0);" onclick="SetEdit(0);">添加</a>
             <%--   <asp:LinkButton ID="btn_Delete" CssClass="f_btn-del" Text="删除" OnClick="Del_Click"
                    runat="server" OnClientClick="return valid();">
                </asp:LinkButton>--%>
                 <asp:Button ID="btnRefresh" ClientIDMode="Static" runat="server" Style="display: none;"
                        OnClick="btnRefresh_CLick" />
            </div>
        </div>
        <dx:ASPxGridView ID="ASPxGridView1"  AutoGenerateColumns="False" KeyFieldName="ID"
            Width="100%" runat="server" ClientInstanceName="grid" OnRowDeleted="aspGrid_RowDeleted"
                OnRowDeleting="aspGrid_RowDeleting"  >
            <Styles>
                <Header BackColor="#f9f9f9">
                </Header>
                <FilterRow BackColor="White">
                </FilterRow>
            </Styles>
            <Columns>
              <%--  <dx:GridViewCommandColumn Caption="选择" Width="5%" ShowSelectCheckbox="True" VisibleIndex="0">
                    <HeaderStyle HorizontalAlign="Center" />
                </dx:GridViewCommandColumn>--%>
                <dx:GridViewDataTextColumn Visible="false" FieldName="ID">
                    </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn VisibleIndex="0" FieldName="FunctionName" CellStyle-HorizontalAlign="Center"
                    HeaderStyle-HorizontalAlign="Center" Caption="功能名称">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn VisibleIndex="0" FieldName="FunctionCode" CellStyle-HorizontalAlign="Center"
                    HeaderStyle-HorizontalAlign="Center" Caption="功能编号">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn VisibleIndex="0" FieldName="FunctionUrl" CellStyle-HorizontalAlign="Center"
                    HeaderStyle-HorizontalAlign="Center" Caption="功能地址">
                </dx:GridViewDataTextColumn>
                 <dx:GridViewDataTextColumn VisibleIndex="0" FieldName="ParentName" CellStyle-HorizontalAlign="Center"
                    HeaderStyle-HorizontalAlign="Center" Caption="父功能">
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

                     <a href="javascript:void(0);" onclick="SetEdit(<%#Eval("ID") %>);">编辑</a>
                      <a href="javascript:void(0)" class="a_del" title="删除" onclick="if(!window.confirm('是否确定删除？')){return false;}grid.DeleteRowByKey(<%#Eval("ID") %>); ">
                                           删除</a>
    

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
            return openChildDialog('功能编辑', 'Dialog/FD_FunctionAdd.aspx?ID=' + planId, 600, 400);
        }


    </script>

</asp:Content>
