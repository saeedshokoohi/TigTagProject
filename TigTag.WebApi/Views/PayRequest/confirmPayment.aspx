<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="confirmPayment.aspx.cs" Inherits="TigTag.WebApi.Views.PayRequest.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div style="text-align: center; width: 100%;height:500px;padding:100px 30px 100px 10px;background-color:darkslategrey;color:#fff; font-family: Tahoma">
            <p>
                <asp:Label runat="server" ID="lblStatus"></asp:Label>
            </p>
            <p>
                <script type="text/javascript">
                    function closeWindow()
                    {
                        open(location, '_self').close();
                    }
                </script>
                <input style="font-family:Tahoma;font-size:15px;color:darkolivegreen; width:300px;height:50px;border:none;background-color:cadetblue" type="button" onclick="closeWindow();" value="بستن" /> 
            </p>

        </div>
    </form>
</body>
</html>
