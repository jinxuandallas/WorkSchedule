<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoaclStroage.aspx.cs" Inherits="WorkSchedule.Test.LoaclStroage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>

    <script>
        function ccc() {
            if (!window.localStorage){
                alert("浏览器支持localstorage");
                return false;
            }else{
                var storage = window.localStorage;
                var s = storage.getItem("w_allHuiKan2");
                alert(s);
            }
        }
    </script>
</head>
<body onload="ccc()">
    <form id="form1" runat="server">
        <div>
            
        </div>
    </form>
</body>
</html>
