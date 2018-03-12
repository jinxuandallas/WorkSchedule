<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Schedule.aspx.cs" Inherits="WorkSchedule.Schedule" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <%--<asp:HiddenField  Value="<%# tool.year %>" runat="server" id="inputYear" />--%>
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>

            <asp:Repeater ID="RepeaterSchedule" runat="server" OnItemDataBound="RepeaterSchedule_ItemDataBound" DataSourceID="SqlDataSource1" DataMember="DefaultView" OnItemCommand="RepeaterSchedule_ItemCommand">
                <HeaderTemplate>
                    <table style="width: 100%">
                        <tr>
                            <td style="text-align: center"><span style="font-size: xx-large">青岛中央商务区重点工作管理平台</span></td>

                        </tr>
                        <tr>
                            <td style="text-align: center"><span style="font-size: x-large">（<%#DateTime.Now.Year%>年）</span></td>
                        </tr>
                        <tr>
                            <td>
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 55%"></td>
                                        <td >
                                            <table style="width:120px">
                                                <tr>
                                                    <td></td>
                                                    <td ><table style="width: 20px;height:15px; background-color: #FF6600;"><tr><td></td></tr></table></td>
                                                    <td style="width: 100px; font-size: small">第一次未完成的工作</td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td>
                                            <table style="width:120px">
                                                <tr>
                                                    <td></td>
                                                    <td ><table style="width: 20px;height:15px; background-color: #D04242;"><tr><td></td></tr></table></td>
                                                    <td style="width: 100px; font-size: small">连续未完成计划的工作</td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td >
                                            <table style="width:120px">
                                                <tr>
                                                    <td></td>
                                                    <td ><table style="width: 20px;height:15px; background-color: #3399FF;"><tr><td></td></tr></table></td>
                                                    <td style="width: 100px; font-size: small">按计划完成的工作</td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <hr style="width: 1500px; text-align: left; margin-left: 0" />
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            第<%#Eval("序号") %>项：<span style="background-color: #ffff99"> <%#Eval("目标名称") %></span><br />
                            <span style="font-size: small">责任领导：<%# ss.GetWorkLeaders(Guid.Parse( Eval("ID").ToString())) %></span>

                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
                            <asp:Table ID="ScheduleTable" BorderWidth="0px" CellSpacing="0" runat="server">
                                <asp:TableRow BorderWidth="0px" runat="server">
                                </asp:TableRow>
                            </asp:Table>
                            <%--</ContentTemplate>--%>
                            <%--</asp:UpdatePanel>--%>

                            <%--<asp:UpdatePanel ID="UpdatePanel2" runat="server">--%>
                            <%--<ContentTemplate>--%>
                            <asp:Panel ID="monthPanel" runat="server" Visible="false">
                                <br />
                                <div style="font-size: small; background-color: #ffff99; width: 500px;">
                                   <asp:Label ID="monthLabel" BackColor="#ffff99" runat="server"></asp:Label>
                                </div>
                            </asp:Panel>
                            <br />
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <hr style="width: 1500px; text-align: left; margin-left: 0" />
                    <br />
                </ItemTemplate>
            </asp:Repeater>

            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString1 %>" SelectCommand="SELECT Id, 序号, 目标名称  FROM 工作 WHERE (年份 = @year)" ProviderName="<%$ ConnectionStrings:ConnectionString1.ProviderName %>">
                <SelectParameters>
                    <asp:Parameter  Name="year" Type="Int32"  />
                </SelectParameters>
            </asp:SqlDataSource>
        </div>
    </form>


</body>
</html>
