<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddOtherWorkMonthSchedule.aspx.cs" Inherits="WorkSchedule.Tools.AddOtherWorkMonthSchedule" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="Id" DataSourceID="SqlDataSource1">
                <Columns>
                    <asp:BoundField DataField="Id" HeaderText="Id" InsertVisible="False" ReadOnly="True" SortExpression="Id" />
                    <asp:BoundField DataField="工作ID" HeaderText="工作ID" SortExpression="工作ID" />
                    <asp:BoundField DataField="目标节点" HeaderText="目标节点" SortExpression="目标节点" />
                    <asp:TemplateField HeaderText="开始月份">
                        <ItemTemplate>
                            <asp:TextBox runat="server" TextMode="Number" MaxLength="2" Width="60" Text="1"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="结束月份">
                        <ItemTemplate>
                            <asp:TextBox runat="server" TextMode="Number" MaxLength="2" Width="60" Text="12"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                   
                </Columns>
            </asp:GridView>
            <br />
            &nbsp;&nbsp; <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="提交" />
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString1 %>" SelectCommand="SELECT [Id], [工作ID], [目标节点] FROM [临时目标节点] where 识别=0"></asp:SqlDataSource>
        </div>
    </form>
</body>
</html>
