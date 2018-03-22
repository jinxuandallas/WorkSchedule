<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Test.aspx.cs" Inherits="WorkSchedule.Test.Test" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <style type="text/css">
        .auto-style1 {
            width: 503px;
            height: 297px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div style="background-image: url('../images/DatePrompt.gif'); background-repeat: repeat-y; background-position: 200px">
            <asp:TextBox ID="TextBox_SN" runat="server" TextMode="Number"></asp:TextBox><br />
            <asp:Button ID="Button1" runat="server" Text="Button" BackColor="#d7d6d5" OnClick="Button1_Click" />

            <div style="width: 150px; height: 20px; font-size: large; background-color: #e7e6e5; padding: 5px">招商类项目</div>
            <p>
                <img alt="" class="auto-style1" src="file:///C:/Users/Administrator/Desktop/QQ截图20180314155351.jpg" />
            </p>
            <table style="width: 1000px;">
                <tr>
                    <td></td>
                </tr>
                <tr >
                    <td style=" background-color:coral;padding:0px;height:30px">
                        <asp:Menu ID="Menu1" Height="100%" runat="server" DataSourceID="SiteMapDataSource1" Orientation="Horizontal" BackColor="#38b0e0" Font-Names="Verdana" Font-Size="0.8em" ForeColor="#284E98" StaticEnableDefaultPopOutImage="False" StaticSubMenuIndent="10px">
                            <DynamicHoverStyle BackColor="#284E98" ForeColor="White"/>
                            <DynamicMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px" Height="30px" />
                            <DynamicMenuStyle BackColor="#B5C7DE" />
                            <DynamicSelectedStyle BackColor="#507CD1"  Height="100%"/>
                            <StaticHoverStyle BackColor="#284E98" ForeColor="White" Height="30px" />
                            <StaticMenuItemStyle HorizontalPadding="5px" VerticalPadding="0px" />
                            <StaticSelectedStyle BackColor="#507CD1"/>

<%--                            <DynamicHoverStyle BackColor="#284E98" ForeColor="White" />
                            <DynamicMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px" />
                            <DynamicMenuStyle BackColor="#B5C7DE" />
                            <DynamicSelectedStyle BackColor="#507CD1" />
                            <StaticHoverStyle BackColor="#284E98" ForeColor="White" />
                            <StaticMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px" />
                            <StaticSelectedStyle BackColor="#507CD1" />--%>
                        </asp:Menu>
                    </td>
                </tr>
                <tr>
                    <td></td>
                </tr>
            </table>

            <asp:SiteMapDataSource ID="SiteMapDataSource1" runat="server" SiteMapProvider="Webtestsitemap" ShowStartingNode="False" />

            <br />
            <div>
                xx
                <br />
                <p style="margin-top: 10px; width: 100px">zzsdtfweqrewrwerewfdsfsdfsdfsdafsdvsdavsxvczxvxxzcvvxc</p>
                <br />
                <p style="margin-top: 10px; margin-bottom: 0px; width: 100px">zzsdtfweqrewrwerewfdsfsdfsdfsdafsdvsdavsxvczxvxxzcvvxc</p>
                <span style="line-height: 5px">dd</span>
            </div>
        </div>
    </form>

</body>
</html>
