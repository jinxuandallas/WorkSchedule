<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageWork.aspx.cs" Inherits="WorkSchedule.Manage.ManageWork" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div style="font-size:small">
            <br />
&nbsp;&nbsp;项目名称：<asp:DropDownList ID="DropDownListWork" runat="server" DataSourceID="SqlDataSource1" DataTextField="目标名称" DataValueField="Id" Width="200px" AutoPostBack="True" OnSelectedIndexChanged="DropDownListWork_SelectedIndexChanged"></asp:DropDownList>
            <br />
            <br />
&nbsp;&nbsp;目标内容：<asp:TextBox ID="TextBoxContent" runat="server" Height="80px" TextMode="MultiLine" Width="400px"></asp:TextBox>
            <br />
            <br />
            &nbsp;&nbsp;进度识别：<asp:TextBox ID="TextBoxSchedule" runat="server" Height="80px" TextMode="MultiLine" Width="400px" BackColor="#FFFFD5"></asp:TextBox>
            <br />
            <br />
            &nbsp;&nbsp;<asp:Button ID="ButtonRecognize" runat="server" OnClick="ButtonRecognize_Click" Text="识别目标节点" />
            <br />
            <br />
            <asp:Panel ID="Panel1" runat="server" Visible="False">
                &nbsp;<asp:Label ID="LabelResult" runat="server"></asp:Label>
                <br />
                <br />
                &nbsp;&nbsp; <span style="color:red">确认无误后提交</span> <br /> &nbsp;&nbsp;
                <asp:Button ID="ButtonSubmit" runat="server" OnClick="ButtonSubmit_Click" Text="更新工作计划" />
            </asp:Panel>
            <br />&nbsp;&nbsp;<asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString1 %>" SelectCommand="SELECT [Id],CAST(序号 AS nvarchar)+ ' ' + [目标名称] as 目标名称, [目标内容] FROM [工作] WHERE ([年份] = @年份)">
                <SelectParameters>
                    <asp:Parameter Name="年份" Type="Int16" />
                </SelectParameters>
            </asp:SqlDataSource>
        </div>
    </form>
</body>
</html>
