<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ScheduleTest.aspx.cs" Inherits="WorkSchedule.ScheduleTest" %>

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

            <asp:Table ID="ScheduleTable" BorderWidth="0px" CellSpacing="0" runat="server">
                <asp:TableRow BorderWidth="0px" runat="server">
                    <asp:TableCell runat="server" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px"></asp:TableCell>
                    <asp:TableCell runat="server" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px"></asp:TableCell>
                    <asp:TableCell runat="server" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px"></asp:TableCell>
                    <asp:TableCell runat="server" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px"></asp:TableCell>
                    <asp:TableCell runat="server" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px"></asp:TableCell>
                    <asp:TableCell runat="server" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px"></asp:TableCell>
                    <asp:TableCell runat="server" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px"></asp:TableCell>
                    <asp:TableCell runat="server" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px"></asp:TableCell>
                    <asp:TableCell runat="server" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px"></asp:TableCell>
                    <asp:TableCell runat="server" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px"></asp:TableCell>
                    <asp:TableCell runat="server" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px"></asp:TableCell>
                    <asp:TableCell runat="server" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px"></asp:TableCell>
                    <asp:TableCell runat="server" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px"></asp:TableCell>
                    <asp:TableCell runat="server" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px"></asp:TableCell>
                    <asp:TableCell runat="server" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px"></asp:TableCell>

                </asp:TableRow>
            </asp:Table>
            <!-- <asp:Table ID="Table1" runat="server">
                            <asp:TableRow Width="100px" runat="server">
                                <asp:TableCell runat="server" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px"></asp:TableCell>

                                <asp:TableCell runat="server" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px"></asp:TableCell>

                                <asp:TableCell runat="server" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px"></asp:TableCell>

                                <asp:TableCell runat="server" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px"></asp:TableCell>

                            </asp:TableRow>

                            <asp:TableRow Width="100" runat="server">
                                <asp:TableCell runat="server" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px"></asp:TableCell>

                            </asp:TableRow>

                        </asp:Table>
                        -->
            <br />
            <table>
                <tr>
                    <td>
                        <table style="border-collapse: collapse; border-spacing: 0px; border: 1px solid #000000">
                            <tr>
                                <td>xx</td>
                            </tr>
                            <tr>
                                <td style="padding: 0px; margin: 0px; border-collapse: collapse; border-spacing: 0px;">
                                    <table style="border-collapse: collapse; border-spacing: 0px;">
                                        <tr>
                                            <td style="padding: 0px; margin: 0px; border-style: solid; border-width: 1px 1px 0px 0px; border-color: #000000;">1</td>
                                            <td style="padding: 0px; margin: 0px; border-style: solid; border-width: 1px 1px 0px 0px; border-color: #000000;">2</td>
                                            <td style="padding: 0px; margin: 0px; border-style: solid; border-width: 1px 1px 0px 0px; border-color: #000000;">3</td>
                                            <td style="padding: 0px; margin: 0px; border-style: solid; border-width: 1px 0px 0px 0px; border-color: #000000;">4</td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td>
                        <table style="border-collapse: collapse; border-spacing: 0px; border: 1px solid #000000">
                            <tr>
                                <td>xx</td>
                            </tr>
                            <tr>
                                <td style="padding: 0px; margin: 0px; border-collapse: collapse; border-spacing: 0px;">
                                    <table style="border-collapse: collapse; border-spacing: 0px;">
                                        <tr>
                                            <td style="padding: 0px; margin: 0px; border-style: solid; border-width: 1px 1px 0px 0px; border-color: #000000;">1</td>
                                            <td style="padding: 0px; margin: 0px; border-style: solid; border-width: 1px 1px 0px 0px; border-color: #000000;">2</td>
                                            <td style="padding: 0px; margin: 0px; border-style: solid; border-width: 1px 1px 0px 0px; border-color: #000000;">3</td>
                                            <td style="padding: 0px; margin: 0px; border-style: solid; border-width: 1px 0px 0px 0px; border-color: #000000;">4</td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td>
                        <table style="border-collapse: collapse; border-spacing: 0px; border: 1px solid #000000">
                            <tr>
                                <td>xx</td>
                            </tr>
                            <tr>
                                <td style="padding: 0px; margin: 0px; border-collapse: collapse; border-spacing: 0px;">
                                    <table style="border-collapse: collapse; border-spacing: 0px;">
                                        <tr>
                                            <td style="padding: 0px; margin: 0px; border-style: solid; border-width: 1px 1px 0px 0px; border-color: #000000;">1</td>
                                            <td style="padding: 0px; margin: 0px; border-style: solid; border-width: 1px 1px 0px 0px; border-color: #000000;">2</td>
                                            <td style="padding: 0px; margin: 0px; border-style: solid; border-width: 1px 1px 0px 0px; border-color: #000000;">3</td>
                                            <td style="padding: 0px; margin: 0px; border-style: solid; border-width: 1px 0px 0px 0px; border-color: #000000;">4</td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td>
                        <table style="border-collapse: collapse; border-spacing: 0px; border: 1px solid #000000">
                            <tr>
                                <td>xx</td>
                            </tr>
                            <tr>
                                <td style="padding: 0px; margin: 0px; border-collapse: collapse; border-spacing: 0px;">
                                    <table style="border-collapse: collapse; border-spacing: 0px;">
                                        <tr>
                                            <td style="padding: 0px; margin: 0px; border-style: solid; border-width: 1px 1px 0px 0px; border-color: #000000;">1</td>
                                            <td style="padding: 0px; margin: 0px; border-style: solid; border-width: 1px 1px 0px 0px; border-color: #000000;">2</td>
                                            <td style="padding: 0px; margin: 0px; border-style: solid; border-width: 1px 1px 0px 0px; border-color: #000000;">3</td>
                                            <td style="padding: 0px; margin: 0px; border-style: solid; border-width: 1px 0px 0px 0px; border-color: #000000;">4</td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td>
                        <table style="border-collapse: collapse; border-spacing: 0px; border: 1px solid #000000">
                            <tr>
                                <td>xx</td>
                            </tr>
                            <tr>
                                <td style="padding: 0px; margin: 0px; border-collapse: collapse; border-spacing: 0px;">
                                    <table style="border-collapse: collapse; border-spacing: 0px;">
                                        <tr>
                                            <td style="padding: 0px; margin: 0px; border-style: solid; border-width: 1px 1px 0px 0px; border-color: #000000;">1</td>
                                            <td style="padding: 0px; margin: 0px; border-style: solid; border-width: 1px 1px 0px 0px; border-color: #000000;">2</td>
                                            <td style="padding: 0px; margin: 0px; border-style: solid; border-width: 1px 1px 0px 0px; border-color: #000000;">3</td>
                                            <td style="padding: 0px; margin: 0px; border-style: solid; border-width: 1px 0px 0px 0px; border-color: #000000;">4</td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
            </table>
            <br />


            <table style="border-collapse: collapse; border-spacing: 0px;">
                <tr>
                    <td style="padding: 0px; margin: 0px; border: 1px solid #000000">
                        <table style="border-collapse: collapse; border-spacing: 0px;">
                            <tr>
                                <td>xx</td>
                            </tr>
                            <tr>
                                <td style="padding: 0px; margin: 0px; border-collapse: collapse; border-spacing: 0px;">
                                    <table style="border-collapse: collapse; border-spacing: 0px;">
                                        <tr>
                                            <td style="padding: 0px; margin: 0px; border-style: solid; border-width: 1px 1px 0px 0px; border-color: #000000;">1</td>
                                            <td style="padding: 0px; margin: 0px; border-style: solid; border-width: 1px 1px 0px 0px; border-color: #000000;">2</td>
                                            <td style="padding: 0px; margin: 0px; border-style: solid; border-width: 1px 1px 0px 0px; border-color: #000000;">3</td>
                                            <td style="padding: 0px; margin: 0px; border-style: solid; border-width: 1px 0px 0px 0px; border-color: #000000;">4</td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td style="padding: 0px; margin: 0px; border: 1px solid #000000">
                        <table style="border-collapse: collapse; border-spacing: 0px;">
                            <tr>
                                <td>xx</td>
                            </tr>
                            <tr>
                                <td style="padding: 0px; margin: 0px; border-collapse: collapse; border-spacing: 0px;">
                                    <table style="border-collapse: collapse; border-spacing: 0px;">
                                        <tr>
                                            <td style="padding: 0px; margin: 0px; border-style: solid; border-width: 1px 1px 0px 0px; border-color: #000000;">1</td>
                                            <td style="padding: 0px; margin: 0px; border-style: solid; border-width: 1px 1px 0px 0px; border-color: #000000;">2</td>
                                            <td style="padding: 0px; margin: 0px; border-style: solid; border-width: 1px 1px 0px 0px; border-color: #000000;">3</td>
                                            <td style="padding: 0px; margin: 0px; border-style: solid; border-width: 1px 0px 0px 0px; border-color: #000000;">4</td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td style="padding: 0px; margin: 0px; border: 1px solid #000000">
                        <table style="border-collapse: collapse; border-spacing: 0px;">
                            <tr>
                                <td>xx</td>
                            </tr>
                            <tr>
                                <td style="padding: 0px; margin: 0px; border-collapse: collapse; border-spacing: 0px;">
                                    <table style="border-collapse: collapse; border-spacing: 0px;">
                                        <tr>
                                            <td style="padding: 0px; margin: 0px; border-style: solid; border-width: 1px 1px 0px 0px; border-color: #000000;">1</td>
                                            <td style="padding: 0px; margin: 0px; border-style: solid; border-width: 1px 1px 0px 0px; border-color: #000000;">2</td>
                                            <td style="padding: 0px; margin: 0px; border-style: solid; border-width: 1px 1px 0px 0px; border-color: #000000;">3</td>
                                            <td style="padding: 0px; margin: 0px; border-style: solid; border-width: 1px 0px 0px 0px; border-color: #000000;">4</td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td style="padding: 0px; margin: 0px; border: 1px solid #000000">
                        <table style="border-collapse: collapse; border-spacing: 0px;">
                            <tr>
                                <td>xx</td>
                            </tr>
                            <tr>
                                <td style="padding: 0px; margin: 0px; border-collapse: collapse; border-spacing: 0px;">
                                    <table style="border-collapse: collapse; border-spacing: 0px;">
                                        <tr>
                                            <td style="padding: 0px; margin: 0px; border-style: solid; border-width: 1px 1px 0px 0px; border-color: #000000;">1</td>
                                            <td style="padding: 0px; margin: 0px; border-style: solid; border-width: 1px 1px 0px 0px; border-color: #000000;">2</td>
                                            <td style="padding: 0px; margin: 0px; border-style: solid; border-width: 1px 1px 0px 0px; border-color: #000000;">3</td>
                                            <td style="padding: 0px; margin: 0px; border-style: solid; border-width: 1px 0px 0px 0px; border-color: #000000;">4</td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td style="padding: 0px; margin: 0px; border: 1px solid #000000">
                        <table style="border-collapse: collapse; border-spacing: 0px;">
                            <tr>
                                <td>xx</td>
                            </tr>
                            <tr>
                                <td style="padding: 0px; margin: 0px; border-collapse: collapse; border-spacing: 0px;">
                                    <table style="border-collapse: collapse; border-spacing: 0px;">
                                        <tr>
                                            <td style="padding: 0px; margin: 0px; border-style: solid; border-width: 1px 1px 0px 0px; border-color: #000000;">1</td>
                                            <td style="padding: 0px; margin: 0px; border-style: solid; border-width: 1px 1px 0px 0px; border-color: #000000;">2</td>
                                            <td style="padding: 0px; margin: 0px; border-style: solid; border-width: 1px 1px 0px 0px; border-color: #000000;">3</td>
                                            <td style="padding: 0px; margin: 0px; border-style: solid; border-width: 1px 0px 0px 0px; border-color: #000000;">4</td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
            </table>
        </div>
        <br />

        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                第一项：关于。。。。
     
        <asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click">详细</asp:LinkButton>

                <asp:Panel ID="Panel1" runat="server">
                    <br />
                    xxxx
                </asp:Panel><br />
                <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Button" />
                <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
            </ContentTemplate>
        </asp:UpdatePanel>

        <br />

    </form>
</body>
</html>
