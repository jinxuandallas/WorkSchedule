<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InitDB.aspx.cs" Inherits="WorkSchedule.Tools.InitDB" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
        <br />
    <form id="form1" runat="server">
        <table><tr><td>
            <asp:Button ID="AddStaf" runat="server" OnClick="AddStaf_Click" Text="添加工作人员" Width="100px" />
            </td></tr>
            <tr><td>&nbsp;</td></tr>
            <tr><td>
            <asp:Button ID="ImportWorks" runat="server" OnClick="ImportWorks_Click" Text="导入工作" Width="100px" />
            </td></tr>
            <tr><td>&nbsp;</td></tr>
            <tr><td>
            <asp:Button ID="BuildWorkLeader" runat="server" Text="构建工作责任领导表" Width="150px" OnClick="BuildWorkLeader_Click" />
            </td></tr>
            <tr><td>&nbsp;</td></tr>
            <tr><td>
            <asp:Button ID="BuildTempMonthTable" runat="server" Text="构建临时月节点表" Width="150px" OnClick="BuildTempMonthTable_Click" />
            </td></tr>
            <tr><td>&nbsp;</td></tr>
            <tr><td>
            <asp:Button ID="BuildMonthSchedule" runat="server" Text="构建月节点表" Width="100px" OnClick="BuildMonthSchedule_Click" />
            </td></tr>
        </table>
    </form>
</body>
</html>
