<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Schedule.aspx.cs" Inherits="WorkSchedule.Schedule" MasterPageFile="~/MasterPage/MainSite.Master" %>

<asp:Content ID="ContentSchedule" ContentPlaceHolderID="ContentPlaceHolderTop" runat="server">
    <%--<asp:HiddenField  Value="<%# tool.year %>" runat="server" id="inputYear" />--%>
    <div>

        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <table style="width: 100%">
            <tr>
                <td style="text-align: center; font-size: xx-large">青岛中央商务区重点工作管理平台</td>
            </tr>
            <tr>
                <td style="text-align: center; font-size: x-large">（<%# DateTime.Now.Year %>年）</td>
            </tr>
            <tr>
                <td>
                    <table style="width: 100%">
                        <tr>
                            <td style="width: 55%; font-size: small">筛选：<asp:DropDownList ID="Filter" runat="server" AutoPostBack="True">
                                <asp:ListItem Value="SELECT Id, 序号, 目标名称,备注 FROM 工作 WHERE (年份 = @year) order by 序号">全部</asp:ListItem>
                                <asp:ListItem Value="SELECT 工作.Id, 序号, 目标名称,备注 FROM 工作 WHERE (年份 = @year) and exists (select 工作.Id from 周节点视图 where 工作.Id=工作ID and 周数=(select 周数 from 周数 where 开始日期<=convert(date, GETDATE()) and 结束日期>=convert(date, GETDATE())) and 周状态=3) order by 序号">本周已完成</asp:ListItem>
                                <asp:ListItem Value="SELECT 工作.Id, 序号, 目标名称,备注 FROM 工作 WHERE (年份 = @year) and exists (select 工作.Id from 周节点视图 where 工作.Id=工作ID and 周数=(select 周数 from 周数 where 开始日期<=convert(date, GETDATE()) and 结束日期>=convert(date, GETDATE())) and 周状态=2) order by 序号">本周未完成</asp:ListItem>
                                <asp:ListItem Value="SELECT 工作.Id, 序号, 目标名称,备注 FROM 工作 WHERE (年份 = @year) and (exists (select 工作.Id from 周节点视图 where 工作.Id=工作ID and 周数=(select 周数 from 周数 where 开始日期<=convert(date, GETDATE()) and 结束日期>=convert(date, GETDATE())) and 周状态<=1) or not exists(select 工作.Id from 周节点视图 where 工作.Id=工作ID and 周数=(select 周数 from 周数 where 开始日期<=convert(date, GETDATE()) and 结束日期>=convert(date, GETDATE())))) and exists (select 月节点.ID from 月节点,周数 where 周数.周数=(select 周数 from 周数 where 开始日期<=convert(date, GETDATE()) and 结束日期>=convert(date, GETDATE())) and month(周数.开始日期)=month(日期) and 工作.Id=月节点.工作ID) order by 序号">本周未填报</asp:ListItem>
                                <asp:ListItem Value="SELECT 工作.Id, 序号, 目标名称,备注 FROM 工作 WHERE (年份 = @year) and exists (select 工作.Id from 周节点视图,周数 where 周数.周数=(select 周数 from 周数 where 开始日期<=convert(date, GETDATE()) and 结束日期>=convert(date, GETDATE())) and 工作.Id=工作ID and month(月节点日期)=month(周数.开始日期) and 周状态=2) order by 序号">本月未完成</asp:ListItem>
                                <asp:ListItem Value="SELECT Id, 序号, 目标名称,备注 FROM 工作 WHERE (年份 = @year) and 工作类别=1 order by 序号">在建开发类项目</asp:ListItem>
                                <asp:ListItem Value="SELECT Id, 序号, 目标名称,备注 FROM 工作 WHERE (年份 = @year) and 工作类别=2 order by 序号">规划开发类项目</asp:ListItem>
                                <asp:ListItem Value="SELECT Id, 序号, 目标名称,备注 FROM 工作 WHERE (年份 = @year) and 工作类别=3 order by 序号">招商类项目</asp:ListItem>
                                <asp:ListItem Value="SELECT Id, 序号, 目标名称,备注 FROM 工作 WHERE (年份 = @year) and 工作类别=4 order by 序号">配套建设项目</asp:ListItem>
                                <asp:ListItem Value="SELECT Id, 序号, 目标名称,备注 FROM 工作 WHERE (年份 = @year) and 工作类别=5 order by 序号">内部控制</asp:ListItem>
                            </asp:DropDownList></td>
                            <td>
                                <table style="width: 120px">
                                    <tr>
                                        <td></td>
                                        <td>
                                            <table style="border: 2px solid #FF6600; padding: 1px; width: 25px; height: 25px">
                                                <tr>
                                                    <td style="background-color: #FF6600;"></td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style="width: 100px; font-size: small">首次未完成计划的工作</td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                                <table style="width: 120px">
                                    <tr>
                                        <td></td>
                                        <td>
                                            <table style="border: 2px solid #D04242; padding: 1px; width: 25px; height: 25px">
                                                <tr>
                                                    <td style="background-color: #D04242;"></td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style="width: 100px; font-size: small">连续未完成计划的工作</td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                                <table style="width: 120px">
                                    <tr>
                                        <td></td>
                                        <td>
                                            <table style="border: 2px solid #3399FF; padding: 1px; width: 25px; height: 25px">
                                                <tr>
                                                    <td style="background-color: #3399FF;"></td>
                                                </tr>
                                            </table>
                                        </td>
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
        <asp:Repeater ID="RepeaterSchedule" runat="server" OnItemDataBound="RepeaterSchedule_ItemDataBound" DataSourceID="SqlDataSource1" DataMember="DefaultView" OnItemCommand="RepeaterSchedule_ItemCommand">
            <ItemTemplate>
                <%--<div style="background-image: url('../images/DatePrompt.gif'); background-repeat: repeat-y; background-position: 220px">--%>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:Panel ID="ProjectCategory" Visible="false" runat="server">
                            <div style="width: 150px; height: 25px; font-size: large; background-color: #d7d6d5; padding: 3px; text-align: center; margin-top: 30px">
                                <asp:Label ID="lbCategoryName" runat="server"></asp:Label>
                            </div>
                            <div style="width: 1500px; height: 2px; border-top: 1px solid #d7d6d5; clear: both; margin-bottom: 15px"></div>
                            <%--<hr style="width: 1500px; text-align: left; margin-left: 0" />--%>
                        </asp:Panel>
                        第<%#Eval("序号") %>项：<span style="background-color: #ffff99"> <%#Eval("目标名称") %></span><br />
                        <span style="font-size: small">责任领导：<%# sc.GetWorkLeaders(Guid.Parse( Eval("ID").ToString())) %></span><br />
                        <span style="font-size: small"><%#Eval("备注").ToString().Trim()==""?"":"备注："+Eval("备注")+"<br />" %></span>

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
                        <asp:Panel ID="monthPanel" runat="server" Visible="false" Width="800px">
                            <div style="font-size: small; background-color: #ffffb0; float: left; margin-top: 20px; margin-bottom: 20px; padding-left: 15px; padding-right: 15px; border: 1px solid #a7a6a5;">
                                <br />
                                <asp:Label ID="monthLabel" runat="server"></asp:Label>
                            </div>
                        </asp:Panel>
                        <br />
                    </ContentTemplate>
                </asp:UpdatePanel>

                <hr style="width: 1500px; text-align: left; margin-left: 0" />
                <br />
                <%--</div>--%>
            </ItemTemplate>
        </asp:Repeater>

        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString1 %>" SelectCommand="<%# Filter.SelectedValue %>" ProviderName="<%$ ConnectionStrings:ConnectionString1.ProviderName %>">
            <%--<asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString1 %>" SelectCommand=<%# (RepeaterSchedule.Controls[0].FindControl("Filter") as DropDownList).SelectedValue %> ProviderName="<%$ ConnectionStrings:ConnectionString1.ProviderName %>">--%>
            <SelectParameters>
                <asp:Parameter Name="year" Type="Int32" />
            </SelectParameters>
        </asp:SqlDataSource>
    </div>
</asp:Content>
