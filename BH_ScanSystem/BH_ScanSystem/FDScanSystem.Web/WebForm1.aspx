<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="FDScanSystem.Web.WebForm1" %>

<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v14.1, Version=14.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <dx:aspxtreelist id="aspxTree" runat="server" parentfieldname="ParentId" keyfieldname="NodeId">
                    <Columns>
                        <dx:TreeListDataColumn Caption="设备/测点名称" FieldName="NodeName">
                        </dx:TreeListDataColumn>
                        <dx:TreeListDataColumn Caption="通道标识" FieldName="Channel1">
                        </dx:TreeListDataColumn>
                        <dx:TreeListDataColumn Caption="预警值（Min）" FieldName="AlertMin">
                        </dx:TreeListDataColumn>
                        <dx:TreeListDataColumn Caption="预警值（Max）" FieldName="AlertMax">
                        </dx:TreeListDataColumn>
                        <dx:TreeListDataColumn Caption="备注" FieldName="Remark">
                        </dx:TreeListDataColumn>
                    </Columns>
                </dx:aspxtreelist>
    </form>
</body>
</html>
