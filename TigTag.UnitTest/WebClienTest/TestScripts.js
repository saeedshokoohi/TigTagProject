
var serverUrl = 'http://localhost:34751/api'
//var serverUrl = 'http://82.102.10.243/tigtag/api'
//var serverUrl = 'http://localhost/tigtag/api'
function callUserSegisterApi() {

    var controllerName = '/user';
    var method = '/registerUser';
    var username = $('#username').val();
    var passwrod = $('#password').val();
    var finalServiceUrl = serverUrl + controllerName + method;
    var user = {username:username,password:passwrod,emailaddress:'user7@tigtag.com',phoneNumber:'0913589963434'}
    $.ajax({
        url: finalServiceUrl,
        type: 'POST',
        data: JSON.stringify(user),
        contentType: "application/json;charset=utf-8",
        success: function (data) {
            $('#message').html(JSON.stringify(data));
            callGetUsersService();
        },
        error: function (x, y, z) {
            $('#message').html(JSON.stringify(x), JSON.stringify(y), JSON.stringify(z));

        }
    });



}

function login() {

    var controllerName = '/user';
    var method = '/login';
    var username = $('#username').val();
    var passwrod = $('#password').val();
    var finalServiceUrl = serverUrl + controllerName + method;
    var user = { username: username, password: passwrod }
    $.ajax({
        url: finalServiceUrl,
        type: 'POST',
        data: JSON.stringify(user),
        contentType: "application/json;charset=utf-8",
        success: function (data) {
            $('#message').html(JSON.stringify(data));
          
        },
        error: function (x, y, z) {
            $('#message').html(JSON.stringify(x), JSON.stringify(y), JSON.stringify(z));

        }
    });



}
function callGetUsersService() {

    var controllerName = '/user';
    var method = '/getRegisteredUser';
    var finalServiceUrl = serverUrl + controllerName + method;
    $.ajax({
        url: finalServiceUrl,
        type: 'Get',
        
        contentType: "application/json;charset=utf-8",
        success: function (data) {
            debugger;
            var list = '';
            $.each(data,function (n, v) {
                list=list+'<li>'+JSON.stringify(n)+JSON.stringify(v)+'</li>'
            });
            $('#userlist').html(list);
          //  ('#userlist').html(JSON.stringify(data));

        },
        error: function (x, y, z) {
            $('#message').html(JSON.stringify(x), JSON.stringify(y), JSON.stringify(z));
        }
    });



}