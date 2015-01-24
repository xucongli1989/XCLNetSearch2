<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="XCLNetSearch2Web._Default" %>
 <%@ Register Assembly="XCLNetSearch" Namespace="XCLNetSearch" TagPrefix="XCL" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1">
    <title></title>
    <script src="Js/XCLNetSearch/jquery-1.5.2.min.js" type="text/javascript"></script>
    <script src="Js/DatePicker/WdatePicker.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" method="get">
        <XCL:Search runat="server" ID="search1"/>
        <br />
        <br />
        <asp:Label ID="lbSql" runat="server" ForeColor="Red"></asp:Label>
    </form>

    <br /><br /><br /><br /><br />

    <fieldset>
        <legend>列表</legend>
        <table style="width:100%;">
            <tr>
                <td>系统编号</td>
                <td>姓名</td>
                <td>所在地</td>
                <td>出生年月</td>
                <td>登记月份</td>
                <td>备注</td>
            </tr>
            <asp:Repeater ID="repList" runat="server">
                <ItemTemplate>
                    <tr>
                        <td><%#Eval("ID")%></td>
                        <td><%#Eval("Name")%></td>
                        <td><%#Eval("Area")%></td>
                        <td><%#Eval("Birth")%></td>
                        <td><%#Eval("EntryMonth")%></td>
                        <td><%#Eval("Remark")%></td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
    </fieldset>

    <script type="text/javascript">
        $("[rel='customAtt']").live("click", function () {
            alert("自定义属性触发的事件");
        });
    </script>
</body>
</html>
