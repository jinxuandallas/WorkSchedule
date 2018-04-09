<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AlterSchedule.aspx.cs" Inherits="WorkSchedule.Manage.AlterSchedule" EnableEventValidation="false" MasterPageFile="~/MasterPage/MainSite.Master" %>

<asp:Content ID="ContentSchedule" ContentPlaceHolderID="ContentPlaceHolderTop" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <asp:Repeater ID="RepeaterSchedule" runat="server" OnItemDataBound="RepeaterSchedule_ItemDataBound" DataSourceID="SqlDataSource1" DataMember="DefaultView" OnItemCommand="RepeaterSchedule_ItemCommand">
                    <HeaderTemplate>
                        <table style="width: 100%">
                            <tr>
                                <td style="text-align: left ; font-size: x-large">&nbsp;&nbsp;修改工作任务（<%#DateTime.Now.Year%>年）</td>
                            </tr>
                            <tr>
                                <td>
                                    <table style="width: 100%">
                                        <tr>
                                            <td style="width: 55%"></td>
                                            <td>
                                                <table style="width: 120px">
                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <table style="width: 20px; height: 15px; background-color: #FF6600;">
                                                                <tr>
                                                                    <td></td>
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
                                                            <table style="width: 20px; height: 15px; background-color: #D04242;">
                                                                <tr>
                                                                    <td></td>
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
                                                            <table style="width: 20px; height: 15px; background-color: #3399FF;">
                                                                <tr>
                                                                    <td></td>
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
                    </HeaderTemplate>
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


                        <asp:Table ID="ScheduleTable" BorderWidth="0px" CellSpacing="0" runat="server">
                            <asp:TableRow BorderWidth="0px" runat="server">
                            </asp:TableRow>
                        </asp:Table>
                        <%--</ContentTemplate>--%>
                        <%--</asp:UpdatePanel>--%>

                        <%--<asp:UpdatePanel ID="UpdatePanel2" runat="server">--%>
                        <%--<ContentTemplate>--%>
                        <asp:Panel ID="monthPanel" runat="server" Visible="false" Width="800px">
                            <div style="width: 1000px;">
                                <div style="float: left; margin-top: 20px; margin-bottom: 30px; padding: 5px; border: 1px solid #a7a6a5">
                                    <div style="font-size: small; background-color: #ffffb0; padding: 10px 20px 10px 20px;">
                                        <br />
                                        <asp:Label ID="monthLabel" runat="server"></asp:Label>
                                        <asp:Repeater ID="RepeaterWeekSchedule" runat="server" DataSource="<%#sc.GetWeeksOfMonth(editMonth) %>">
                                            <HeaderTemplate>
                                                <br />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                第<asp:Label ID="lbWeek" runat="server" Text='<%# Eval("周数") %>'></asp:Label>周（<%#DateTime.Parse(Eval("开始日期").ToString()).ToString("M月d日") %>--<%#DateTime.Parse(Eval("结束日期").ToString()).ToString("M月d日") %>）：<br />
                                                <div style="margin: 5px">
                                                    周工作计划：<asp:TextBox ID="TextBoxWeekSchedule" Width="400px" Text='<%# sc.GetWeekSchedule(editWorkID,int.Parse( Eval("周数").ToString())) %>' runat="server"></asp:TextBox>
                                                    <br />
                                                </div>
                                                <div style="margin: 5px">
                                                    周落实情况：<asp:TextBox ID="TextBoxWeekExecution" Width="400px" Text='<%# sc.GetWeekExecution(editWorkID,int.Parse( Eval("周数").ToString())) %>' runat="server"></asp:TextBox>
                                                    <br />
                                                </div>
                                                已完成：<asp:CheckBox ID="CheckBoxState" Checked='<%# sc.GetWeekState(editWorkID, int.Parse(Eval("周数").ToString())) == 3 ? true:false %>' runat="server" />
                                                <br />
                                                <br />
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Button ID="ButtonSubmit" runat="server" Text="提交" OnClick="ButtonSubmit_Click" CommandName="SubmitWeekSchedule" />
                                                <br />
                                                <br />
                                            </FooterTemplate>
                                        </asp:Repeater>

                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                        <br />


                        <hr style="width: 1500px; text-align: left; margin-left: 0" />
                        <br />
                        <%--</div>--%>
                    </ItemTemplate>
                </asp:Repeater>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString1 %>" SelectCommand="SELECT Id, 序号, 目标名称,备注  FROM 工作 WHERE (年份 = @year) order by 序号" ProviderName="<%$ ConnectionStrings:ConnectionString1.ProviderName %>">
            <SelectParameters>
                <asp:Parameter Name="year" Type="Int32" />
            </SelectParameters>
        </asp:SqlDataSource>
    </div>
</asp:Content>
