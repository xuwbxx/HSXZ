<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="left.ascx.cs" Inherits="FDScanSystem.Web.UC.left" %>
<div class="h-menu layout">
    <div class="h-menu-con h-show" menuid="10">
        <div class="h-menu-header">
            <i title="菜单展开/收缩"></i><span>
            <asp:Literal ID="ltUrl" runat="server"></asp:Literal>
            <%--<a href="/Index.aspx">主页</a>--%>
            </span></div>
        <div class="h-menu-scroll" style="display: none;">
            <i></i>
        </div>
        <div class="h-menu-body" style="height: 800px;">
            <ul>
             <asp:Repeater ID="menuRpt" runat="server" OnItemDataBound="rpMenu_Bind">
        <ItemTemplate>
          <li id="l1" class="active"><a href="javascript:void(0)"><i class="a-i-1"></i><span><%# Eval("Description")%></span>
                    <span class="h-flag"></span></a>
                    <ul style="display: block;">
                    <asp:Repeater ID="menuSubRpt" runat="server">
                    <ItemTemplate>
                    <li class="<%#BindCheckClass(Eval("Code")) %>"><a href="<%# Eval("Link") %>"><i class="h-i-"></i><span><%# Eval("Description")%></span></a></li>
                    </ItemTemplate>
                    </asp:Repeater>
                    </ul>
                </li>
        </ItemTemplate>
        </asp:Repeater>
     
            </ul>
        </div>
    </div>
</div>
