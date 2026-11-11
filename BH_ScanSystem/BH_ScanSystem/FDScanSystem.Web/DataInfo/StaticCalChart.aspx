<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="StaticCalChart.aspx.cs" Inherits="FDScanSystem.Web.DataInfo.StaticCalChart" %>

<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/dialog.js" type="text/javascript"></script>
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
                    <dx:ASPxComboBox ID="cbxNode" ClientInstanceName="cbxNode" runat="server" AutoPostBack="True"
                        Width="200px" EnableCallbackMode="false" OnSelectedIndexChanged="cbxNode_Select">
                    </dx:ASPxComboBox>
                </div>
            </li>
            <li>监测类型 </li>
            <li>
                <div class="hc_combox">
                    <dx:ASPxComboBox ID="cbxProtocol" ClientInstanceName="cbxProtocol" runat="server" AutoPostBack="true"
                        Width="200px" EnableCallbackMode="false" OnSelectedIndexChanged="cbxProtocol_select">
                    </dx:ASPxComboBox>
                </div>
            </li>
            <li>测点名称 </li>
            <li>
                <div class="hc_combox">

                       <dx:ASPxTextBox ClientInstanceName="txtPoint" ID="txtPoint" ReadOnly="true"  Width="150px" NullText="请输入搜索的关键字" runat="server">
                        <ClientSideEvents GotFocus="function(){SelectPoint();}" />
                        </dx:ASPxTextBox>
                 <%--   <input id="txtPoint" disabled="disabled" name="txtPointList" type="text" />--%>
                        <asp:HiddenField ID="hfPoint" ClientIDMode="Static" runat="server" />
                    <%--  <dx:ASPxTextBox ID="txtPointList" ClientEnabled="false" ClientInstanceName="txtPointList"
                        runat="server" Width="200px">
                    </dx:ASPxTextBox>--%>

<%--                    <input type="button" value="撤销" onclick="jQuery('#txtPointList').val('');jQuery('#txtPoint').val('');"
                        id="Button1" />
                    <input type="hidden" name="hidPoint" value="<%=PointId %>" id="hidPoint" />--%>
                </div>
            </li>
            <li>
                <div class="hc_button-group1">
                    <asp:Button ID="btn_Choose" CssClass="ks_btn-submit1" runat="server" Text="查看" />
                </div>
            </li>
        </ul>
        <ul>
            <li>起始日期 </li>
            <li>
                <div class="hc_combox">
                    <dx:ASPxDateEdit ID="txtBeginDate" runat="server" Width="200px">
                    </dx:ASPxDateEdit>
                </div>
            </li>
             <li>起始小时</li>
            <li>
                <div class="hc_combox">
                    <dx:ASPxComboBox ID="txtBeginHour" runat="server" Width="100px">
                    </dx:ASPxComboBox>
                </div>
            </li>
            <li>结束日期</li>
            <li>
                <div class="hc_combox">
                    <dx:ASPxDateEdit ID="txtEndDate" runat="server" Width="200px">
                    </dx:ASPxDateEdit>
                </div>
            </li>
           
            <li>结束小时</li>
            <li>
                <div class="hc_combox">
                    <dx:ASPxComboBox ID="txtEndHour" runat="server" Width="100px">
                    </dx:ASPxComboBox>
                </div>
            </li>
        </ul>
    </div>
    <div>
        <%--   <iframe id="charFrame" frameborder="0" height="900" width="900px" marginheight="0"
            marginwidth="0" src="DataChart.aspx"></iframe>--%>
        <center>
            <span style="font-size: 20px;">
                <asp:Literal ID="ltTitle" runat="server"></asp:Literal></span></center>
        <br />
        <asp:Chart ID="chart" runat="server" Width="1050px" Height="300px" Palette="BrightPastel"
            ImageType="Png" BorderDashStyle="Solid" BackSecondaryColor="White" BackGradientStyle="TopBottom"
            BorderWidth="1" BackColor="#D3DFF0">
            <Legends>
                <asp:Legend IsTextAutoFit="true" Alignment="Center" Docking="Bottom" Name="Default"
                    BackColor="Transparent" Font="Trebuchet MS, 8.25pt, style=Bold">
                </asp:Legend>
            </Legends>
            <BorderSkin SkinStyle="Emboss"></BorderSkin>
            <Series>
            </Series>
            <ChartAreas>
                <asp:ChartArea Name="ChartArea1" BorderColor="64, 64, 64, 64" BorderDashStyle="Solid"
                    BackSecondaryColor="White" BackColor="64, 165, 191, 228" ShadowColor="Transparent"
                    BackGradientStyle="TopBottom" IsSameFontSizeForAllAxes="True">
                    <Area3DStyle PointDepth="150" PointGapDepth="150" Rotation="15" Perspective="2" Inclination="20"
                        LightStyle="Realistic" WallWidth="0" IsClustered="False"></Area3DStyle>
                    <AxisY LineColor="64, 64, 64, 64">
                        <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                        <MajorGrid LineColor="64, 64, 64, 64" />
                    </AxisY>
                    <AxisX LineColor="64, 64, 64, 64">
                        <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                        <MajorGrid LineColor="64, 64, 64, 64" />
                    </AxisX>
                </asp:ChartArea>
            </ChartAreas>
        </asp:Chart>
        <br />
        <center>
            <span style="font-size: 20px;">
                <asp:Literal ID="ltTitle2" runat="server"></asp:Literal></span></center>
        <asp:Chart Visible="false" ID="chart2" runat="server" Height="300px" Width="1050px"
            ImageLocation="~/TempImages/ChartPic_#SEQ(300,3)" Palette="BrightPastel" ImageType="Png"
            BorderDashStyle="Solid" BackSecondaryColor="White" BackGradientStyle="TopBottom"
            BorderWidth="2" BackColor="#D3DFF0">
            <Legends>
                <asp:Legend IsTextAutoFit="true" Alignment="Center" Docking="Bottom" Name="Default"
                    BackColor="Transparent" Font="Trebuchet MS, 8.25pt, style=Bold">
                </asp:Legend>
            </Legends>
            <BorderSkin SkinStyle="Emboss"></BorderSkin>
            <Series>
            </Series>
            <ChartAreas>
                <asp:ChartArea Name="ChartArea2" BorderColor="64, 64, 64, 64" BorderDashStyle="Solid"
                    BackSecondaryColor="White" BackColor="64, 165, 191, 228" ShadowColor="Transparent"
                    BackGradientStyle="TopBottom" IsSameFontSizeForAllAxes="True">
                    <Area3DStyle PointDepth="150" PointGapDepth="150" Rotation="15" Perspective="2" Inclination="20"
                        LightStyle="Realistic" WallWidth="0" IsClustered="False"></Area3DStyle>
                    <AxisY LineColor="64, 64, 64, 64">
                        <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                        <MajorGrid LineColor="64, 64, 64, 64" />
                    </AxisY>
                    <AxisX LineColor="64, 64, 64, 64">
                        <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                        <MajorGrid LineColor="64, 64, 64, 64" />
                    </AxisX>
                </asp:ChartArea>
            </ChartAreas>
        </asp:Chart>
    </div>
    <script type="text/javascript">
//        var OnSelectGoodsCallback = function (PointId, PointName) {

//        }

//        function TriggerChooseGoods(ctrl, hid) {
//            //  window.alert(cbxNode.GetValue());
//            // return;
//            OnSelectGoodsCallback = function (PointId, PointName) {
//                jQuery(ctrl).val(PointName);
//                jQuery("#" + hid).val(PointId);
//                jQuery("#btnAddGoods").removeAttr("disabled");
//                //jQuery(ctrl).attr("disabled", true);
//                layer.closeAll();

//                //                $.post("StaticCalChart.aspx", { Action: "post", ctl00$ContentPlaceHolder1$txtPoint: PointId });


//            }
//            // ShowIframeDialog("PointSelect.aspx?node=" + cbxNode.GetValue() + "&type=" + cbxProtocol.GetValue(), 700, 400, "选择测点");
//            openChildDialog('选择测点', 'PointSelect.aspx?node=' + cbxNode.GetValue() + "&type=" + cbxProtocol.GetValue(), 900, 1200);
//            //            jQuery("#txtPoint").val(jQuery("#" + hid).val());




        //        }

        function SelectPoint() {
            OnPointSelect = function (arr) {
               
                                if (arr.length > 0) {
                                    var aa = "";
                                    var id = "";
                                    for (var i = 0; i < arr.length; i++) {
                                        aa = aa + arr[i].name + ",";
                                        id = id + arr[i].id + ",";
                                    }

                                    txtPoint.SetText(aa);
                                    $("#hfPoint").val(id);
                                }
                                else {
                                    txtPoint.SetText('');
                                    $("#hfPoint").val('');
                                }
            }
           // ShowDialog('PointSelect.aspx?point=' + $("#<%=hfPoint.ClientID %>").val() + "&node=" + cbxNode.GetValue() + "&type=" + cbxProtocol.GetValue(), 600, 550, '选择测点');
            openChildDialog('选择测点', 'PointSelect.aspx?point=' + $("#<%=hfPoint.ClientID %>").val() + "&node=" + cbxNode.GetValue() + "&type=" + cbxProtocol.GetValue(), 900, 1200);

        }

        

    </script>
</asp:Content>
