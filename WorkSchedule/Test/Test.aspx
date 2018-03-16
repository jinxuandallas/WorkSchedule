<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Test.aspx.cs" Inherits="WorkSchedule.Test.Test" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
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
        <div>
             <asp:TextBox ID="TextBox_SN" runat="server" TextMode="Number"></asp:TextBox><br />
            <asp:Button ID="Button1" runat="server" Text="Button" BackColor="#d7d6d5" OnClick="Button1_Click" />

            <div style="width:150px;height:20px; font-size:large;background-color:#e7e6e5;padding:5px">招商类项目</div>
        </div>
    </form>
    <p>
        <img alt="" class="auto-style1" src="file:///C:/Users/Administrator/Desktop/QQ截图20180314155351.jpg" /></p>
</body>
</html>
