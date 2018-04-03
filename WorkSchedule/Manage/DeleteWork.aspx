<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeleteWork.aspx.cs" Inherits="WorkSchedule.Manage.DeleteWork" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <span style="text-align: left; font-size: x-large">删除工作任务</span>
            <hr style="width: 1500px; text-align: left; margin-left: 0" />
            <br />
            <%--<asp:Button ID="Button1" runat="server" Text="Button" OnClientClick='if(confirm("xx")) {return true;}else{return false}' OnClick="Button1_Click" />--%>
            <table style="width:100%;font-size:small">
                <tr>
                    <td style="width:50px">&nbsp;</td>
                    <td style="width:120px">项目名称</td>
                    <td><asp:DropDownList ID="DropDownListWork" runat="server" DataSourceID="SqlDataSource1" DataTextField="目标名称" DataValueField="Id" Width="200px" AutoPostBack="True" OnSelectedIndexChanged="DropDownListWork_SelectedIndexChanged"></asp:DropDownList>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td colspan="2">
                        <asp:Label ID="LabelWorkDetail" runat="server"></asp:Label>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td colspan="2">&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>
                        <asp:Button ID="Delete" runat="server"  OnClientClick='if(confirm("确定删除后工作任务将不可恢复")) {return true;}else{return false}' Text="删除" OnClick="Delete_Click" />
                    </td>
                    <td>
                        <asp:Label ID="LabelResult" runat="server"></asp:Label>
                    </td>
                    <td>&nbsp;</td>
                </tr>
            </table>
        </div>
        
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString1 %>" SelectCommand="SELECT [Id],CAST(序号 AS nvarchar)+ ' ' + [目标名称] as 目标名称, [目标内容] FROM [工作] WHERE ([年份] = @年份)">
            <SelectParameters>
                <asp:Parameter Name="年份" Type="Int16" />
            </SelectParameters>
        </asp:SqlDataSource>
        
    </form>
</body>
</html>
