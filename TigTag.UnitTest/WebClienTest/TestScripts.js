
var serverUrl = 'http://localhost:34751/api'
function callUserSegisterApi() {

    var controllerName = '/user';
    var method = '/registerUser';
    var finalServiceUrl = serverUrl + controllerName + method;
    var user = {username:'user1',password:'pass1',emailaddress:'user1@tigtag.com'}
    $.ajax({
        url: finalServiceUrl,
        type: 'POST',
        data: JSON.stringify(user),
        contentType: "application/json;charset=utf-8",
        success: function (data) {
            WriteResponse(data);
        },
        error: function (x, y, z) {
            alert(x + '\n' + y + '\n' + z);
        }
    });



}