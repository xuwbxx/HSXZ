<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DataChart.aspx.cs" Inherits="FDScanSystem.Web.DataInfo.DataChart" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
</head>
<body>
    <form id="form1" runat="server">
    <%--<asp:Chart ID="Chart1" runat="server" ImageLocation="~/TempImages/ChartPic_#SEQ(300,3)"
        ImageType="Png" BackColor="WhiteSmoke" BorderWidth="2" BackGradientStyle="TopBottom"
        BackSecondaryColor="White" Palette="BrightPastel" BorderlineDashStyle="Solid"
        BorderColor="26, 59, 105" Height="296px" Width="900">
        <Titles>
            <asp:Title ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 12pt, style=Bold" ShadowOffset="3"
                Text="时域曲线图" ForeColor="26, 59, 105">
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
                <AxisY IntervalType="Number" LineColor="64, 64, 64, 64" IsLabelAutoFit="False" >
                    <LabelStyle Format="N5" Font="Trebuchet MS, 8.25pt, style=Bold" />
                    <MajorGrid LineColor="64, 64, 64, 64" />
                </AxisY>
                <AxisX IntervalType="Seconds" IntervalAutoMode="VariableCount" IntervalOffsetType="Seconds"
                    LineColor="64, 64, 64, 64" IsLabelAutoFit="False">
                    <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                    <MajorGrid LineColor="64, 64, 64, 64" />
                </AxisX>
            </asp:ChartArea>
        </ChartAreas>
    </asp:Chart>--%>

    <asp:Chart ID="Chart2" runat="server" ImageLocation="~/TempImages/ChartPic_#SEQ(300,3)"
        ImageType="Png" BackColor="WhiteSmoke" BorderWidth="2" BackGradientStyle="TopBottom"
        BackSecondaryColor="White" Palette="BrightPastel" BorderlineDashStyle="Solid"
        BorderColor="26, 59, 105" Height="296px" Width="900">
        <Titles>
            <asp:Title ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 12pt, style=Bold" ShadowOffset="3"
                Text="时域曲线图" ForeColor="26, 59, 105">
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
                    <LabelStyle Format="N5" Font="Trebuchet MS, 8.25pt, style=Bold" />
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

     <asp:Chart ID="Chart3" runat="server" ImageLocation="~/TempImages/ChartPic_#SEQ(300,3)"
        ImageType="Png" BackColor="WhiteSmoke" BorderWidth="2" BackGradientStyle="TopBottom"
        BackSecondaryColor="White" Palette="BrightPastel" BorderlineDashStyle="Solid"
        BorderColor="26, 59, 105" Height="296px" Width="900">
        <Titles>
            <asp:Title ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 12pt, style=Bold" ShadowOffset="3"
                Text="频域曲线图" ForeColor="26, 59, 105">
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
                    <LabelStyle Format="N5" Font="Trebuchet MS, 8.25pt, style=Bold" />
                    <MajorGrid LineColor="64, 64, 64, 64" />
                </AxisY>
                <AxisX IntervalType="Number" 
                    LineColor="64, 64, 64, 64" IsLabelAutoFit="False">
                    <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                    <MajorGrid LineColor="64, 64, 64, 64" />
                </AxisX>
            </asp:ChartArea>
        </ChartAreas>
    </asp:Chart>

    </form>
</body>
</html>
