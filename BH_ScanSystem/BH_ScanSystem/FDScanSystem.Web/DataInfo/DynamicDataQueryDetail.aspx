<%@ Page Title="" Language="C#" MasterPageFile="~/Dialog.Master" AutoEventWireup="true"
    CodeBehind="DynamicDataQueryDetail.aspx.cs" Inherits="FDScanSystem.Web.DataInfo.DynamicDataQueryDetail" %>

<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v14.1, Version=14.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>
    <%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxRoundPanel" TagPrefix="dx" %>
     <%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dx" %>
    <%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<%@ Register assembly="DevExpress.Web.v14.1, Version=14.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxPanel" tagprefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="100%">
        <tr>
            <td valign="top">
                <dx:ASPxTreeList ID="aspxTree" au ClientInstanceName="treeClientList" runat="server"
                    Width="130px" ParentFieldName="ParentId" KeyFieldName="NodeId" AutoGenerateColumns="False"
                    EnableTheming="True" Theme="DevEx" OnCustomDataCallback="aspTree_CustomCallback">
                    <SettingsBehavior AllowFocusedNode="True" AutoExpandAllNodes="True" />
                    
                    <SettingsDataSecurity AllowDelete="False" AllowEdit="False" AllowInsert="False" />
                    <Columns>
                        <dx:TreeListDataColumn  Caption="设备/测点名称" FieldName="NodeName">
                        </dx:TreeListDataColumn>
                        <dx:TreeListDataColumn Caption="通道标识" FieldName="Channel1">
                        </dx:TreeListDataColumn>
                        <dx:TreeListDataColumn Caption="备注" FieldName="Remark">
                        </dx:TreeListDataColumn>
                    </Columns>
                    <ClientSideEvents
                     CustomDataCallback="function(s, e){ LoadData();document.getElementById('value1').innerHTML = e.result; }"
                     FocusedNodeChanged="function(s,e){
                      var key = treeClientList.GetFocusedNodeKey();
                      treeClientList.PerformCustomDataCallback(key); 
                      mainClientGrid1.PerformCallback(key);   
                      mainClientGrid2.PerformCallback(key);
                         
                    }" />
                </dx:ASPxTreeList>
            </td>
            <td valign="top">
            <table border="0">
            <tr>
         <%--   <td width="120px">
             <dx:ASPxButton ID="btnXlsxExport" runat="server" Text="导出时域数据" UseSubmitBehavior="False"
                    OnClick="btnXlsxExport_Click" />
            </td >--%>
            <td width="120px">
              <dx:ASPxButton ID="ASPxButton1" runat="server" Text="导出时域数据" UseSubmitBehavior="False"
                    OnClick="btnXlsxExport1_Click" />
            </td>
            <td width="120px">
              <dx:ASPxButton ID="ASPxButton2" Visible="false" runat="server" Text="导出频域数据" UseSubmitBehavior="False"
                    OnClick="btnXlsxExport2_Click" />
            </td>
            <td align="left">
              <a href="javascript:void(0)" onclick="ViewDetail();" runat="server" id="url">原始时域报表</a>
            </td>
        
            </tr>
               <tr>
            <td colspan="3">
             <dx:ASPxCallbackPanel ID="ASPxRoundPanel1" runat="server"
        Width="100%">
        <PanelCollection>
        <dx:PanelContent ID="PanelContent1" runat="server">
         基频数据：<span id="value1" style="font-size:15px; color:blue;"></span>
        </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxCallbackPanel>
            </td>
            </tr>
            <tr>
            <td colspan="3">
             <iframe id="charFrame" frameborder="0" height="900" width="900px" marginheight="0"
                    marginwidth="0" src="DataChart.aspx"></iframe>
            </td>
            </tr>
            </table>
            </td>
        </tr>
       <%-- <tr>
        <td></td>
        <td valign="top" >
        &nbsp;&nbsp;&nbsp;&nbsp;
       
        </td>
        </tr>--%>
     <%--   <tr style="display:none;">--%>
        <tr>
            <td valign="top"  colspan="2"  align="center">
                <table width="100%">
                    <tr>
                        <td align="right">
                           <%-- <dx:ASPxGridView   ID="aspGrid" AutoGenerateColumns="False" KeyFieldName="ID" Width="880"
                                runat="server" ClientInstanceName="mainClientGrid"   OnCustomCallback="aspGrid_CustomCallback">
                                <Styles>
                                    <Header BackColor="#f9f9f9">
                                    </Header>
                                    <FilterRow BackColor="White">
                                    </FilterRow>
                                </Styles>
                                <Columns>
                                    <dx:GridViewDataTextColumn VisibleIndex="0" FieldName="ScanData" CellStyle-HorizontalAlign="Center"
                                        HeaderStyle-HorizontalAlign="Center" Caption="采集数值">
                                 <PropertiesTextEdit DisplayFormatString="F8"></PropertiesTextEdit>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn VisibleIndex="0" FieldName="ScanTime" CellStyle-HorizontalAlign="Center"
                                        HeaderStyle-HorizontalAlign="Center" PropertiesTextEdit-DisplayFormatString="yyyy/MM/dd HH:mm:ss.ffff"
                                        Caption="采集时间">
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
                            </dx:ASPxGridView>--%>

                             <dx:ASPxGridView  ID="ASPxGridView1" AutoGenerateColumns="False" KeyFieldName="ID" Width="880"
                                runat="server" ClientInstanceName="mainClientGrid1"   OnCustomCallback="aspGrid1_CustomCallback">
                                <Styles>
                                    <Header BackColor="#f9f9f9">
                                    </Header>
                                    <FilterRow BackColor="White">
                                    </FilterRow>
                                </Styles>
                                <Columns>
                                    <dx:GridViewDataTextColumn VisibleIndex="0" FieldName="ScanData" CellStyle-HorizontalAlign="Center"
                                        HeaderStyle-HorizontalAlign="Center" Caption="采集数值">
                                      <PropertiesTextEdit DisplayFormatString="F8"></PropertiesTextEdit>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn VisibleIndex="0" FieldName="ScanTime" CellStyle-HorizontalAlign="Center"
                                        HeaderStyle-HorizontalAlign="Center" PropertiesTextEdit-DisplayFormatString="yyyy/MM/dd HH:mm:ss.ffff"
                                        Caption="采集时间">
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
                            <dx:ASPxGridView  ID="ASPxGridView2" AutoGenerateColumns="False" KeyFieldName="ID" Width="880"
                                runat="server" ClientInstanceName="mainClientGrid2"   OnCustomCallback="aspGrid2_CustomCallback">
                                <Styles>
                                    <Header BackColor="#f9f9f9">
                                    </Header>
                                    <FilterRow BackColor="White">
                                    </FilterRow>
                                </Styles>
                                <Columns>
                                    <dx:GridViewDataTextColumn VisibleIndex="0" FieldName="ScanData" CellStyle-HorizontalAlign="Center"
                                        HeaderStyle-HorizontalAlign="Center" Caption="采集数值">
                                      <PropertiesTextEdit DisplayFormatString="F8"></PropertiesTextEdit>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn VisibleIndex="0" FieldName="ScanData2" CellStyle-HorizontalAlign="Center"
                                        HeaderStyle-HorizontalAlign="Center" 
                                        Caption="采集数据2">
                                        <PropertiesTextEdit DisplayFormatString="F8"></PropertiesTextEdit>
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
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <dx:ASPxGridViewExporter ID="gridExport" runat="server" GridViewID="aspGrid"></dx:ASPxGridViewExporter>
    <dx:ASPxGridViewExporter ID="gridExport1" runat="server" GridViewID="ASPxGridView1"></dx:ASPxGridViewExporter>
    <dx:ASPxGridViewExporter ID="gridExport2" runat="server" GridViewID="ASPxGridView2"></dx:ASPxGridViewExporter>
    <input type="hidden" id="hid" value="aaa" />
    <script type="text/javascript">
        function LoadData() {
            var pointId = treeClientList.GetFocusedNodeKey();
            $("#hid").val(pointId);
            jQuery("#charFrame").attr("src", "DataChart.aspx?data=<%=DataValueId %>&point=" + pointId + "&st=" + Math.random());
        }
        function ViewDetail() {
            var pointId = $("#hid").val();
           
            openChildDialog('采集数据浏览', "ScanDataValue.aspx?point=" + pointId + "&data=<%=DataValueId %>&st=" + Math.random(), 1000, 500);
        }
    </script>
</asp:Content>
