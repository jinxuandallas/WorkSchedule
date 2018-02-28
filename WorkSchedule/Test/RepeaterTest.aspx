<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RepeaterTest.aspx.cs" Inherits="WorkSchedule.Test.RepeaterTest" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Repeater ID="Repeater1" runat="server" OnItemDataBound="Repeater1_ItemDataBound" DataSourceID="SqlDataSource1" DataMember="DefaultView">
                <%--<HeaderTemplate>xxx</HeaderTemplate>--%>
                <ItemTemplate>
                    <asp:Table ID="ScheduleTable" BorderWidth="0px" CellSpacing="0" runat="server">
                        <asp:TableRow BorderWidth="0px" runat="server">
                        </asp:TableRow>
                    </asp:Table>
                    <br />
                </ItemTemplate>
            </asp:Repeater>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString1 %>" SelectCommand="SELECT [Id] FROM [工作]"></asp:SqlDataSource>
        </div>
    </form>
</body>
</html>
