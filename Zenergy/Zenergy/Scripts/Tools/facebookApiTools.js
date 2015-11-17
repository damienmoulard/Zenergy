zenergyApp.controller("facebookApiTools", ["$scope", "$http", "$httpParamSerializerJQLike", "tokenService","$window","$location", function ($scope,$http, $httpParamSerializerJQLike, tokenService, $window,$location) {


    // This is called with the results from from FB.getLoginStatus().
    function statusChangeCallback(response) {
        if (response.status === 'connected') {
            facebookLogin(response.authResponse.accessToken);
            console.log(response);
        } else if (response.status === 'not_authorized') {
            document.getElementById('status').innerHTML = 'Please log ' +
              'into this app.';
        } else {
            document.getElementById('status').innerHTML = 'Please log ' +
              'into Facebook.';
        }
    }

    $window.checkLoginState = function() {
        FB.getLoginStatus(function (response) {
            statusChangeCallback(response);
        });
    }

    window.fbAsyncInit = function () {
        FB.init({
            appId: '429380533932654',
            cookie: true,  // enable cookies to allow the server to access
            // the session
            xfbml: true,  // parse social plugins on this page
            version: 'v2.2' // use version 2.2
        });


    };

    (function (d, s, id) {
        var js, fjs = d.getElementsByTagName(s)[0];
        if (d.getElementById(id)) return;
        js = d.createElement(s); js.id = id;
        js.src = "//connect.facebook.net/en_US/sdk.js";
        fjs.parentNode.insertBefore(js, fjs);
    }(document, 'script', 'facebook-jssdk'));

    function facebookLogin(token) {
        console.log('Welcome!  Fetching your information.... ');
        FB.api('/me', 'get', { access_token: token, fields: 'id,email,first_name,last_name' }, function (response) {
            var user = response;
            console.log(user);
            $http({
                url: 'api/login',
                method: 'POST',
                data: $httpParamSerializerJQLike({ grant_type: 'password', username: user.email, password: user.id }),
                //data: $httpParamSerializerJQLike({ grant_type: 'password', username: $scope.user.mail, password: CryptoJS.MD5($scope.user.password).toString() }),
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded'
                }
            }).then(function successCallback(response) {
                tokenService.saveToken(response.data.access_token, user.email);
                $location.path('/');
            }, function errorCallback(response) {
                //TODO: inscrire le user
                console.log(response);
            });
        });
    }

}]);