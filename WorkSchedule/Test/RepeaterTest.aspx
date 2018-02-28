<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RepeaterTest.aspx.cs" Inherits="WorkSchedule.Test.RepeaterTest" %>

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
                <HeaderTemplate>xxx<br />
                    <hr style="width:1200px" />
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            
                            第一项：关于。。。。
     
        <asp:LinkButton ID="LinkButton1" runat="server" CommandName="Detail">详细</asp:LinkButton>

                            <asp:Panel ID="Panel1" Visible="false" runat="server">
                                <br />
                                xxxx
                            </asp:Panel>
                            <br />
                            <asp:Button ID="Button1" runat="server" CommandName="button" Text="Button" />
                            <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <asp:Table ID="ScheduleTable" BorderWidth="0px" CellSpacing="0" runat="server">
                        <asp:TableRow BorderWidth="0px" runat="server">
                        </asp:TableRow>
                    </asp:Table>

                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                          <asp:Panel ID="weekPanel" Visible="false" runat="server">
                       
                                <asp:Label ID="weekLabel" BackColor="#ffff99" runat="server"></asp:Label>
                    </asp:Panel>
                    <br />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <hr style="width:1200px" />

                    <br />
                    
                </ItemTemplate>
            </asp:Repeater>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString1 %>" SelectCommand="SELECT [Id] FROM [工作]"></asp:SqlDataSource>
        </div>
    </form>
</body>
</html>
