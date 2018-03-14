<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddScheduleTool.aspx.cs" Inherits="WorkSchedule.AddWorkSchedule.AddScheduleTool" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div>
            <%#DateTime.Now.Year%>年：<br />
            第<asp:DropDownList ID="DropDownListWork" runat="server" AutoPostBack="True" DataSourceID="SqlDataSourceDropDownListWork" DataTextField="序号" DataValueField="Id" OnSelectedIndexChanged="DropDownListWork_SelectedIndexChanged"></asp:DropDownList>
            项工作：<asp:SqlDataSource ID="SqlDataSourceDropDownListWork" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString1 %>" SelectCommand="SELECT 序号, Id FROM 工作 WHERE (年份 = DATENAME(YYYY,GETDATE()) )"></asp:SqlDataSource>

            <asp:Label ID="LabelWork" runat="server"></asp:Label>
            <br />
            <br />
            第<asp:DropDownList ID="DropDownListMonth" DataSource="<%#t.GetExistTaskMonths(Guid.Parse(DropDownListWork.SelectedValue)) %>" runat="server" OnSelectedIndexChanged="DropDownListMonth_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>月目标节点：
            
            <asp:Label ID="LabelMonthTask" runat="server"></asp:Label>
            <br />
            <br />
            <br />
            周工作计划：
            <br />
            <asp:Repeater ID="Repeater1" runat="server" DataSource="<%#t.GetWeeksOfMonth(int.Parse(DropDownListMonth.SelectedValue)) %>">
                <ItemTemplate>
                    第<asp:Label ID="lbWeek" runat="server" Text='<%# Eval("周数") %>'></asp:Label>周（<%#DateTime.Parse(Eval("开始日期").ToString()).ToShortDateString() %>--<%#DateTime.Parse(Eval("结束日期").ToString()).ToShortDateString() %>）：<br />
                    周工作计划：<asp:TextBox ID="TextBoxWeekSchedule" Width="200px" runat="server"></asp:TextBox>
                    <br />
                    周落实情况：<asp:TextBox ID="TextBoxWeekExecution" Width="200px" runat="server"></asp:TextBox>
                    <br />
                    已完成：<asp:CheckBox ID="CheckBoxState" runat="server" />
                    <br />
                </ItemTemplate>
                <FooterTemplate>
                    <br />
                    <asp:Button ID="ButtonSubmit" runat="server" Text="提交" OnClick="ButtonSubmit_Click" />
                </FooterTemplate>
            </asp:Repeater>
        </div>
    </form>
</body>
</html>
