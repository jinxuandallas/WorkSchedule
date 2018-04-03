<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InputSchedule.aspx.cs" Inherits="WorkSchedule.Input.InputSchedule" EnableEventValidation="false" MasterPageFile="~/MasterPage/MainSite.Master" %>

<asp:Content ID="ContentInput" ContentPlaceHolderID="ContentPlaceHolderTop" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <asp:Repeater ID="RepeaterSchedule" runat="server" OnItemDataBound="RepeaterSchedule_ItemDataBound" DataSourceID="SqlDataSource1" DataMember="DefaultView" OnItemCommand="RepeaterSchedule_ItemCommand">
                    <HeaderTemplate>
                        <table style="width: 100%">
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
                                                        <td style="width: 100px; font-size: small">首次未完成的工作</td>
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
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                第<%#Eval("序号") %>项：<span style="background-color: #ffff99"> <%#Eval("目标名称") %></span><br />
                                <span style="font-size: small">目标内容：<%#Eval("目标内容") %><br />
                                    责任领导：<%# ss.GetWorkLeaders(Guid.Parse( Eval("ID").ToString())) %></span><br />
                                <span style="font-size: small"><%#Eval("备注").ToString().Trim()==""?"":"备注："+Eval("备注")+"<br />" %></span>
                            </ContentTemplate>
                        </asp:UpdatePanel>


                        <asp:Table ID="ScheduleTable" BorderWidth="0px" CellSpacing="0" runat="server">
                            <asp:TableRow BorderWidth="0px" runat="server">
                            </asp:TableRow>
                        </asp:Table>
                        <%--</ContentTemplate>--%>
                        <%--</asp:UpdatePanel>--%>

                        <%--<asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>--%>
                        <asp:Panel ID="monthPanel" runat="server" Visible="false" Width="1000px">
                            <div style="width: 1000px;">
                                <div style="float: left; margin-top: 20px; margin-bottom: 30px; padding: 5px; border: 1px solid #a7a6a5">
                                    <div style="font-size: small; background-color: #ffffb0; padding: 10px 20px 10px 20px;">
                                        <br />
                                        <asp:Label ID="monthLabel" runat="server"></asp:Label>
                                        <asp:Repeater ID="RepeaterWeekSchedule" runat="server" DataSource="<%#tool.GetWeeksOfMonth(editMonth) %>" >
                                            <HeaderTemplate><br /></HeaderTemplate>
                                            <ItemTemplate>
                                                第<asp:Label ID="lbWeek" runat="server" Text='<%# Eval("周数") %>'></asp:Label>周（<%#DateTime.Parse(Eval("开始日期").ToString()).ToString("M月d日") %>--<%#DateTime.Parse(Eval("结束日期").ToString()).ToString("M月d日") %>）：<br />
                                                周工作计划：<asp:TextBox ID="TextBoxWeekSchedule" Width="400px" Text='<%# tool.GetWeekSchedule(editWorkID,int.Parse( Eval("周数").ToString())) %>' runat="server"></asp:TextBox>
                                                <br />
                                                周落实情况：<asp:TextBox ID="TextBoxWeekExecution" Width="400px" Text='<%# tool.GetWeekExecution(editWorkID,int.Parse( Eval("周数").ToString())) %>' runat="server"></asp:TextBox>
                                                <br />
                                                已完成：<asp:CheckBox ID="CheckBoxState" Checked='<%# tool.GetWeekState(editWorkID, int.Parse(Eval("周数").ToString())) == 3 ? true:false %>' runat="server" />
                                                <br />
                                                <br />
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Button ID="ButtonSubmit" runat="server" Text="提交" OnClick="ButtonSubmit_Click" CommandName="SubmitWeekSchedule" />
                                                <br /><br />
                                            </FooterTemplate>
                                        </asp:Repeater>
                                        
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                        <br />
                        <%-- </ContentTemplate>
                    </asp:UpdatePanel>--%>
                        <hr style="width: 1500px; text-align: left; margin-left: 0" />
                        <br />
                    </ItemTemplate>
                </asp:Repeater>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString1 %>" SelectCommand="SELECT distinct(工作.Id), 序号, 目标名称,工作.目标内容,工作.备注  FROM 工作,工作责任领导视图,月节点 WHERE 工作责任领导视图.年份 = @year and (信息管理用户ID=@用户ID or 用户ID=@用户ID) and 工作责任领导视图.工作ID=工作.Id and 月节点.工作ID=工作.Id and MONTH(月节点.日期)<=month(getdate())+1" ProviderName="<%$ ConnectionStrings:ConnectionString1.ProviderName %>">
        <SelectParameters>
            <asp:Parameter Name="year" Type="Int32" />
            <asp:SessionParameter Name="用户ID" SessionField="UserID" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>
