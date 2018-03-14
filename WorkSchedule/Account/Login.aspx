<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="WorkSchedule.Account.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>

    <style type="text/css">
        .banner {
            text-align: center;
            background: url(../images/beijing.jpg);
        }
    </style>

</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div class="banner">
                <object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" codebase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=7,0,19,0" width="1002" height="220">
                    <param name="movie" value="../images/banner.swf" />
                    <param name="quality" value="high" />
                    <param name="wmode" value="transparent" />
                    <embed src="http://122.4.72.60:8000/statics/images/zyswq/banner.swf" width="1002" height="220" quality="high" pluginspage="http://www.macromedia.com/go/getflashplayer" type="application/x-shockwave-flash" wmode="transparent"></embed>
                </object>
            </div>
            <br />
            <br />
            <br />
  <%--  --%>          

            <div style="width:300px; margin:0 auto;vertical-align:middle;text-align: center;">
                <asp:Login ID="LoginUser" runat="server" BackColor="#EFF3FB" BorderColor="#B5C7DE" BorderPadding="4" BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="0.9em" ForeColor="#333333" Height="100px" Width="230px" OnAuthenticate="LoginUser_Authenticate">
                <InstructionTextStyle Font-Italic="True" ForeColor="Black" />
                <LoginButtonStyle BackColor="White" BorderColor="#507CD1" BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="0.8em" ForeColor="#284E98" />
                <TextBoxStyle Font-Size="0.8em" />
                <TitleTextStyle BackColor="#507CD1" Font-Bold="True" Font-Size="0.9em" ForeColor="White" />
            </asp:Login>
            </div>
        </div>
    </form>
</body>
</html>
