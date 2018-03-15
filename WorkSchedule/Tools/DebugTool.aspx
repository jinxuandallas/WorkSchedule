<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DebugTool.aspx.cs" Inherits="WorkSchedule.Tools.DebugTool" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:TextBox ID="TextBox1" runat="server" Width="300px"></asp:TextBox>
            <br />
            <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Button" />
            <br />
            <br />
            <asp:Button ID="ButtonRemove" runat="server" OnClick="ButtonRemove_Click" Text="去除重复的月节点" />
        </div>
    </form>
</body>
</html>
