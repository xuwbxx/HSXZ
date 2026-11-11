<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FD_Command_List.aspx.cs" Inherits="FDScanSystem.Web.Setting.FD_Command_List" %>
<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

  <div class="h-screen-tab">
        <div class="ks_breadcrumb">
              <asp:Literal ID="ltUrl" runat="server"></asp:Literal> > 远程控制面板
        </div>
    </div>
    <div class="a_t">
        <ul class="a_4">
            <li>通讯设备选择： </li>
            <li>
                <div>
                    <dx:ASPxComboBox ID="cmb_Com" AutoPostBack="true" runat="server" OnSelectedIndexChanged="cmb_Com_SelectedIndexChanged"
                        Width="150px">
                    </dx:ASPxComboBox>
                </div>
            </li>
            <li>监测设备选择：</li>
            <li>
                <div>
                    <dx:ASPxComboBox ID="cmb_Device" AutoPostBack="false" runat="server" Width="150px">
                    </dx:ASPxComboBox>
                </div>
            </li>
            <li>
                <div class="hc_button-group1">
                    <asp:Button ID="btn_Choose" CssClass="ks_btn-submit1" Width="70" runat="server" Text="搜索"
                        OnClick="btn_Choose_Click" />
                </div>
            </li>
        </ul>
    </div>
    <div class="a_m">
        <table width="100%">
            <tr>
                <td id="td2" valign="top" style="border-right: 1px solid #ccc;">
                    <div>
                        <ul class="a_ml">
                            <li>
                                <table>
                                    <tr>
                                        <td style="white-space: nowrap">
                                            开始时间：
                                        </td>
                                        <td>
                                            <dx:ASPxDateEdit ID="aspxBeginDay" Width="80%" Height="30" runat="server">
                                                <CalendarProperties TodayButtonText="今天" ClearButtonText="清除">
                                                    <HeaderStyle Spacing="1px" />
                                                    <FooterStyle Spacing="17px" />
                                                </CalendarProperties>
                                            </dx:ASPxDateEdit>
                                        </td>
                                        <td>
                                            <dx:ASPxTimeEdit ID="aspxBeginTime" Width="80%" Height="30" DisplayFormatString="HH:mm"
                                                EditFormatString="HH:mm" DateTime="2012-01-01 12:00" runat="server">
                                            </dx:ASPxTimeEdit>
                                        </td>
                                    </tr>
                                </table>
                            </li>
                            <li>
                                <table>
                                    <tr>
                                        <td style="white-space: nowrap">
                                            结束时间：
                                        </td>
                                        <td>
                                            <dx:ASPxDateEdit ID="aspxEndDay" Width="80%" Height="30" runat="server">
                                                <CalendarProperties TodayButtonText="今天" ClearButtonText="清除">
                                                    <HeaderStyle Spacing="1px" />
                                                    <FooterStyle Spacing="17px" />
                                                </CalendarProperties>
                                            </dx:ASPxDateEdit>
                                        </td>
                                        <td>
                                            <dx:ASPxTimeEdit ID="aspxEndTime" Width="80%" Height="30" DisplayFormatString="HH:mm"
                                                DateTime="2012-01-01 12:00" EditFormatString="HH:mm" runat="server">
                                            </dx:ASPxTimeEdit>
                                        </td>
                                    </tr>
                                </table>
                            </li>
                        </ul>
                        <ul class="a_mr">
                            <li>
                                <asp:LinkButton ID="btn_DateRead" runat="server" OnClick="btn_DateRead_Click"><img src="/images/a_4.gif"/><br/>指定日期读取</asp:LinkButton>
                            </li>
                            <li>
                                <asp:LinkButton ID="btn_Send" runat="server" OnClick="btn_Send_Click"><img src="/images/a_1.gif"/><br />发送采集指令</asp:LinkButton>
                            </li>
                            <li>
                                <asp:LinkButton ID="btn_Read" runat="server" OnClick="btn_Read_Click"><img src="/images/a_2.gif"/><br />读取采集记录</asp:LinkButton>
                            </li>
                            <li>
                                <asp:LinkButton ID="btn_Clear" runat="server" OnClick="btn_Clear_Click"><img src="/images/a_3.gif"/><br />清除数据</asp:LinkButton>
                            </li>
                        </ul>
                        <div class="a_mb">
                            &nbsp 指令：
                            <textarea id="txt_Command" runat="server" class="a_textarea"></textarea>
                            <button>
                                <asp:LinkButton ID="btn_SendCommand" runat="server" OnClick="btn_SendCommand_Click">发布<br />指令</asp:LinkButton>
                            </button>
                        </div>
                    </div>
                </td>
                </tr>
        </table>
    </div>
    <div class="h_listtable">
        <div class="f_tabletool">
            <div class="hc_btn_location">
                <asp:LinkButton ID="btn_Delete" CssClass="f_btn-del" runat="server" OnClientClick="return valid();"
                    OnClick="btn_Delete_Click">删除</asp:LinkButton>
                <asp:LinkButton ID="btn_HuaHeng" CssClass="f_btn-exp" runat="server" OnClick="btn_HuaHeng_Click">发送指令</asp:LinkButton>
            </div>
        </div>
        <dx:ASPxGridView ID="ASPxGridView1" KeyFieldName="ID" AutoGenerateColumns="false"
            runat="server" Width="100%"
            ClientInstanceName="grid">
            <Styles>
                <Header BackColor="#f9f9f9">
                </Header>
                <FilterRow BackColor="#f9f9f9">
                </FilterRow>
            </Styles>
            <Templates>
                <PreviewRow>
                    <table>
                        <tbody>
                            <tr>
                                <td align="left">
                                    <strong>结果 :</strong>
                                </td>
                            </tr>
                            <tr>
                                <td style="border: none;">
                                    <%# Eval("Result")%>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </PreviewRow>
            </Templates>
            <Columns>
                <dx:GridViewCommandColumn Caption="选择" Width="5%" ShowSelectCheckbox="true" VisibleIndex="0">
                    <HeaderStyle HorizontalAlign="Center" />
                </dx:GridViewCommandColumn>
                <dx:GridViewDataTextColumn FieldName="ID" ReadOnly="true" VisibleIndex="0" Visible="false">
                    <EditFormSettings Visible="False" />
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn VisibleIndex="0" Width="30%" FieldName="Command" Caption="指令">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn VisibleIndex="0" FieldName="DeviceName" Caption="监测设备">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn VisibleIndex="0" FieldName="CommunicationName" Caption="通讯设备">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn VisibleIndex="0" FieldName="CreateDate" Caption="发送日期">
                </dx:GridViewDataTextColumn>
            </Columns>
            <Settings ShowGroupPanel="false" ShowFilterBar="Hidden" ShowFilterRow="false" ShowPreview="true" />
        </dx:ASPxGridView>
    </div>

</asp:Content>
