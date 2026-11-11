<%@ Page Title="" Language="C#" MasterPageFile="~/Dialog1.Master" AutoEventWireup="true"
    CodeBehind="PointSelect.aspx.cs" Inherits="FDScanSystem.Web.DataInfo.PointSelect" %>

<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script src="/Scripts/Dialog1.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--<asp:CheckBoxList ID="chkPoint" runat="server" RepeatDirection="Vertical">
    </asp:CheckBoxList>--%>
   <%-- <table width="100%" border="0" cellpadding="0" cellspacing="6">
        <tr>
            <td>
                <tr>
                    <td valign="top" colspan="7" align="left">
                        <ul id="selected" style="width: 650px; font-size: 11px; margin-left: 0px; padding: 0px;
                            line-height: 20px;">
                            <asp:Repeater ID="rptDataSource" runat="server">
                                <ItemTemplate>
                                    <li style='list-style: none; border: 1px solid #666666; background: #CCC; padding: 2px;
                                        margin-right: 2px; font-weight: 700; float: left;'>
                                        <%#Eval("PointName") %>
                                    </li>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ul>
                        <input type="hidden" id="pointlist" name="pointlist" />
                        <script type="text/javascript">
                            function Add(id, name) {
                                jQuery("#selected").append("<li style='list-style:none;border:1px solid #666666;background:#CCC;padding:2px;margin-right:2px;font-weight:700;float:left;'>" + name + ";</li>")
                                jQuery("#pointlist").val(jQuery("#pointlist").val() + "," + name);
                                jQuery("#selectedHisory").val(jQuery("#selectedHisory").val() + "," + id);
                            }
                        </script>
                        <input id="selectedHisory" name="selectedHistory" type="hidden" value="<%=SelectedIds %>" />
                    </td>
                </tr>
            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxGridView ID="mainGrid" KeyFieldName="ID" ClientInstanceName="mainClientGrid"
                    Width="650" runat="server" OnCustomCallback="mainGrid_CustomCallback">
                    <Columns>
                        <dx:GridViewDataColumn Caption="测点名称" VisibleIndex="0" FieldName="PointName">
                        </dx:GridViewDataColumn>
                        <dx:GridViewDataColumn Caption="传感器名称" VisibleIndex="0" FieldName="Remark">
                        </dx:GridViewDataColumn>
                        <dx:GridViewDataColumn Caption="选择" VisibleIndex="0">
                            <DataItemTemplate>
                                <a href="javascript:void(0)" onclick="OnGoodsSelect(<%#Eval("ID") %>,'<%#Eval("PointName") %>')">
                                    选择</a>
                            </DataItemTemplate>
                        </dx:GridViewDataColumn>
                    </Columns>
                    <SettingsPager Mode="ShowPager" PageSize="15">
                    </SettingsPager>
                </dx:ASPxGridView>
            </td>
        </tr>
    </table>
    <script type="text/javascript">

        function OnGoodsSelect(PointId, PointName) {
            Add(PointId, PointName);

            window.parent.OnSelectGoodsCallback(jQuery("#selectedHisory").val(), jQuery("#pointlist").val());


        }

        function AutoScroll() {
            jQuery(document).scrollTop(0);
        }
    </script>--%>

    <div class="toolbar">
        <table cellpadding="0" cellspacing="2">
            <tr>
                <td>
                    <dx:ASPxButton ID="btnSave" Image-Url="/Images/yes.png" Height="28px" runat="server"
                        Text="保存">
                        <Paddings PaddingTop="0" PaddingBottom="0" />
                        <ClientSideEvents Click="function(s,e){window.setTimeout(function (){SelectPoint();},100);e.processOnServer=false;}" />
                    </dx:ASPxButton>
                </td>
                <td>

                    <dx:ASPxButton ID="btnCancel" Image-Url="/Images/no.png" Height="28px" runat="server"
                        Text="返回">
                        <ClientSideEvents Click="function(s,e){closeChildDialog();e.processOnServer=false;}" />
                        <Paddings PaddingTop="0" PaddingBottom="0" />
                    </dx:ASPxButton>
                </td>
            </tr>
        </table>
    </div>
    <div class="l_list-table" style="margin-top: 0px;">
        <div class="l_list-table-con">
            <table width="100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td valign="top">
                        <dx:ASPxGridView ID="mainGrid" ClientInstanceName="clientDataGrid" EnableCallBacks="false"
                            KeyFieldName="ID" Width="100%" runat="server">
                            <Columns>
                                <dx:GridViewDataColumn Caption="选择" VisibleIndex="0">
                                    <DataItemTemplate>
                                        <input type="checkbox" id='<%#Eval("ID") %>' value='<%#Eval("PointName") %>' name="cbUser" />
                                    </DataItemTemplate>
                                </dx:GridViewDataColumn>
                                 <dx:GridViewDataColumn Caption="测点名称" VisibleIndex="0" FieldName="PointName">
                        </dx:GridViewDataColumn>
                        <dx:GridViewDataColumn Caption="传感器编码" VisibleIndex="0" FieldName="Remark">
                        </dx:GridViewDataColumn>
                            </Columns>
                        <SettingsPager PageSize="1000"></SettingsPager>
                         
                            
                            <Styles Header-HorizontalAlign="Center" Header-Paddings-Padding="5" Cell-HorizontalAlign="Center">
                                <Header Paddings-Padding="5px" Font-Bold="true" HorizontalAlign="Center">
                                </Header>
                                <Cell Paddings-Padding="3px">
                                </Cell>
                            </Styles>
                        </dx:ASPxGridView>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <script type="text/javascript">
        $("input[name='cbUser']").each(function () {
            $(this).click(function () {
                if (('<%= Request["type"] %>').toLowerCase() == 's') {
                    if ($(this).prop('checked')) {
                        $("input[name='cbUser']").removeAttr("checked");
                        $(this).prop("checked", true);
                    }
                }
            });
            var str = '<%=Point %>';
            var strs = new Array();
            strs = str.split(",");

            for (i = 0; i < strs.length; i++) {
                if ($(this).attr("id") == strs[i]) {
                    $(this).prop("checked", true);
                }
            }


        });

        function SelectPoint() {
            var arr = [];

            $("input[name='cbUser']:checked").each(function () {
                var obj = {};
                obj.id = $(this).attr("id");
                obj.name = $(this).val();
                arr.push(obj);
            });

            window.parent.OnPointSelect(arr);
            closeChildDialog();
        }

        function AutoScroll() {
            jQuery(document).scrollTop(0);
        }

    </script>

</asp:Content>
