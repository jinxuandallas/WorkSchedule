<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InputThisWeekSchedule.aspx.cs" EnableEventValidation="false" Inherits="WorkSchedule.Input.InputThisWeekSchedule" %>

<%@ Import Namespace="System.Globalization" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:Repeater ID="RepeaterThisWeek" OnItemCommand="RepeaterThisWeek_ItemCommand" runat="server" DataSourceID="SqlDataSource1">
                        <ItemTemplate>
                            第<%#Eval("序号") %>项：<span style="background-color: #ffff99"> <%#Eval("目标名称") %></span><br />
                            <span style="font-size: small">目标内容：<%#Eval("目标内容") %><br />
                                责任领导：<%# ss.GetWorkLeaders(Guid.Parse( Eval("ID").ToString())) %></span><br />
                            <span style="font-size: small"><%#Eval("备注").ToString().Trim()==""?"":"备注："+Eval("备注")+"<br />" %></span>
                            <div style="width: 1000px;">
                                <div style="float: left; margin-top: 20px; margin-bottom: 30px; padding: 5px; border: 1px solid #a7a6a5">
                                    <div style="font-size: small; background-color: #ffffb0; padding: 10px;">
                                        <%# DateTime.Now.Month %>月计划：“<%# Eval("目标节点") %>”<br />
                                        <%--第<asp:Label ID="lbWeek" runat="server" Text='<%#(new GregorianCalendar()).GetWeekOfYear(DateTime.Now,CalendarWeekRule.FirstDay, DayOfWeek.Monday) %>'></asp:Label>周：<br />--%>
                                        <%# thisWeek %>周工作计划：<asp:TextBox ID="TextBoxWeekSchedule" Width="400px" runat="server"></asp:TextBox>
                                        <br />
                                        <%#thisWeek %>周落实情况：<asp:TextBox ID="TextBoxWeekExecution" Width="400px" runat="server"></asp:TextBox>
                                        <br />
                                        <asp:CheckBox ID="CheckBoxState" Text="已完成" runat="server" />
                                        <br />
                                        <%# thisWeek==53?"下一":(thisWeek+1).ToString() %>周工作计划：<asp:TextBox ID="TextBoxNextWeekSchedule" Width="400px" runat="server"></asp:TextBox>
                                        <br />
                                        <br />
                                        <asp:CheckBox ID="CheckBoxModify" Text="修改上周计划" AutoPostBack="true" OnCheckedChanged="CheckBoxModify_CheckedChanged" Enabled='<%# thisWeek==1?false:true %>' runat="server" />
                                        <br />
                                        <asp:PlaceHolder ID="PlaceHolderModify" runat="server" Visible="false">xxxx
                                        <br />
                                        </asp:PlaceHolder>
                                        <br />
                                        <asp:Button ID="ButtonSubmit" runat="server" CommandName="SubmitSchedule" CommandArgument='<%# Eval("Id") %>' Text="提交" /><asp:Label ID="LabelStatus" runat="server"></asp:Label>
                                    </div>
                                </div>
                            </div>
                            <hr style="width: 1500px; text-align: left; margin-left: 0" />
                            <br />

                        </ItemTemplate>
                    </asp:Repeater>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString1 %>" SelectCommand="SELECT distinct(工作.Id), 序号, 目标名称,工作.目标内容,工作.备注,月节点.目标节点  FROM 工作,工作责任领导视图,月节点 WHERE 工作责任领导视图.年份 = @year and (信息管理用户ID=@用户ID or 用户ID=@用户ID) and 工作责任领导视图.工作ID=工作.Id and 月节点.工作ID=工作.Id and MONTH(月节点.日期)=month(getdate())" ProviderName="<%$ ConnectionStrings:ConnectionString1.ProviderName %>">
            <SelectParameters>
                <asp:Parameter Name="year" Type="Int32" />
                <asp:SessionParameter Name="用户ID" SessionField="UserID" />
            </SelectParameters>
        </asp:SqlDataSource>
    </form>
</body>
</html>
