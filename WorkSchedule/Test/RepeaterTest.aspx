<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RepeaterTest.aspx.cs" Inherits="WorkSchedule.Test.RepeaterTest" %>
<%@ OutputCache Duration="3600"
               Location="Any"
               VaryByCustom="browser"
               VaryByParam="RequestID" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>

            <asp:Repeater ID="Repeater1" runat="server" OnItemDataBound="Repeater1_ItemDataBound" DataSourceID="SqlDataSource1" DataMember="DefaultView" OnItemCommand="Repeater1_ItemCommand">
                <HeaderTemplate>
                    <table style="width:100%">
                        <tr>
                            <td style="text-align:center"><span style="font-size: xx-large">青岛中央商务区重点工作管理平台</span></td>
                            
                        </tr>
                        <tr>
                            <td style="text-align:center"><span style="font-size: x-large">（<%#DateTime.Now.Year%>年）</span></td>
                        </tr>
                    </table>
                    <br />
                    <hr style="width: 1200px" />
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            第<%#Eval("序号") %>项：<span style="background-color: #ffff99"> <%#Eval("目标名称") %></span><br />
                            <span style="font-size: small">目标内容：<%#Eval("目标内容") %><br />
                                备注：<%#Eval("备注") %><br />
                                责任领导：<%# ss.GetWorkLeaders(Guid.Parse( Eval("ID").ToString())) %> </span>

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
                            <asp:Panel ID="monthPanel" runat="server">

                                <asp:Label ID="monthLabel" BackColor="#ffff99" runat="server"></asp:Label>
                            </asp:Panel>
                            <br />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <hr style="width: 1200px" />

                    <br />

                </ItemTemplate>
            </asp:Repeater>

            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString1 %>" SelectCommand="SELECT Id, 序号, 目标名称, 目标内容, 备注 FROM 工作 WHERE (年份 = DATENAME(YYYY,GETDATE()))"></asp:SqlDataSource>
        </div>
    </form>
</body>
</html>
