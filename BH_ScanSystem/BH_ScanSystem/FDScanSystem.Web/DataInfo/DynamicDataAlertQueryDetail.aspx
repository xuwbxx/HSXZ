<%@ Page Title="" Language="C#" MasterPageFile="~/Dialog.Master" AutoEventWireup="true"
    CodeBehind="DynamicDataAlertQueryDetail.aspx.cs" Inherits="FDScanSystem.Web.DataInfo.DynamicDataAlertQueryDetail" %>

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
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <%--  <table width="100%">
        <tr>
            <td valign="top" align="center">
                <asp:Chart ID="Chart1" runat="server" ImageLocation="~/TempImages/ChartPic_#SEQ(300,3)"
                    ImageType="Png" BackColor="WhiteSmoke" BorderWidth="2" BackGradientStyle="TopBottom"
                    BackSecondaryColor="White" Palette="BrightPastel" BorderlineDashStyle="Solid"
                    BorderColor="26, 59, 105" Height="296px" Width="900">
                    <Titles>
                        <asp:Title ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 12pt, style=Bold" ShadowOffset="3"
                            Text="监测点曲线图" ForeColor="26, 59, 105">
                        </asp:Title>
                    </Titles>
                    <Legends>
                        <asp:Legend Enabled="False" IsTextAutoFit="False" Name="Default" BackColor="Transparent"
                            Font="Trebuchet MS, 8.25pt, style=Bold">
                        </asp:Legend>
                    </Legends>
                    <BorderSkin SkinStyle="Emboss"></BorderSkin>
                    <Series>
                        <asp:Series Name="Series1" ChartType="FastLine" ShadowColor="Black" BorderWidth="3"
                            BorderColor="180, 26, 59, 105">
                        </asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1" BorderColor="64, 64, 64, 64" BorderDashStyle="Solid"
                            BackSecondaryColor="White" BackColor="Gainsboro" ShadowColor="Transparent" BackGradientStyle="TopBottom">
                            <Area3DStyle Rotation="10" Perspective="10" Inclination="15" IsRightAngleAxes="False"
                                WallWidth="0" IsClustered="False"></Area3DStyle>
                            <AxisY IntervalType="Number" LineColor="64, 64, 64, 64" IsLabelAutoFit="False">
                                <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                                <MajorGrid LineColor="64, 64, 64, 64" />
                            </AxisY>
                            <AxisX IntervalType="Seconds" IntervalAutoMode="VariableCount" IntervalOffsetType="Seconds"
                                LineColor="64, 64, 64, 64" IsLabelAutoFit="False">
                                <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                                <MajorGrid LineColor="64, 64, 64, 64" />
                            </AxisX>
                        </asp:ChartArea>
                    </ChartAreas>
                </asp:Chart>
            </td>
        </tr>
        <tr>
            <td valign="top" align="center">
                <dx:ASPxGridView ID="aspGrid" AutoGenerateColumns="False" KeyFieldName="ID" Width="100%"
                    runat="server" ClientInstanceName="mainClientGrid" OnCustomCallback="aspGrid_CustomCallback">
                    <Styles>
                        <Header BackColor="#f9f9f9">
                        </Header>
                        <FilterRow BackColor="White">
                        </FilterRow>
                    </Styles>
                    <Columns>
                        <dx:GridViewDataTextColumn VisibleIndex="0" FieldName="ScanData" CellStyle-HorizontalAlign="Center"
                            HeaderStyle-HorizontalAlign="Center" Caption="采集数值">
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
            </td>
        </tr>
    </table>--%>

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
              <dx:ASPxButton ID="ASPxButton2" runat="server" Text="导出频域数据" UseSubmitBehavior="False"
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
        <tr style="display:none;">
            <td valign="top"  colspan="2"  align="center">
                <table width="100%">
                    <tr>
                        <td align="right">
                           <%-- <dx:ASPxGridView Visible="true"  ID="aspGrid" AutoGenerateColumns="False" KeyFieldName="ID" Width="880"
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

                             <dx:ASPxGridView Visible="true" ID="ASPxGridView1" AutoGenerateColumns="False" KeyFieldName="ID" Width="880"
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
                            <dx:ASPxGridView Visible="true" ID="ASPxGridView2" AutoGenerateColumns="False" KeyFieldName="ID" Width="880"
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
