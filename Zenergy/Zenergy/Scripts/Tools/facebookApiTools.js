zenergyApp.controller("facebookApiTools", ["$scope", "$http", "$httpParamSerializerJQLike", "tokenService","$window","$location", function ($scope,$http, $httpParamSerializerJQLike, tokenService, $window,$location) {


    // This is called with the results from from FB.getLoginStatus().
    function statusChangeCallback(response) {
        if (response.status === 'connected') {
            facebookLogin(response.authResponse.accessToken);
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

    FB = null;
    (function (d, s, id) {
        var js, fjs = d.getElementsByTagName(s)[0];
        //if (d.getElementById(id)) return;
        js = d.createElement(s); js.id = id;
        js.src = "//connect.facebook.net/en_US/sdk.js";
        fjs.parentNode.insertBefore(js, fjs);
    }(document, 'script', 'facebook-jssdk'));

    function facebookLogin(token) {
        FB.api('/me', 'get', { access_token: token, fields: 'id,email,first_name,last_name' }, function (response) {
            var user = response;
            //Try to login
            $http({
                url: 'api/login',
                method: 'POST',
                data: $httpParamSerializerJQLike({ grant_type: 'password', username: user.email, password: CryptoJS.MD5(user.id).toString() }),
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded'
                }
            }).then(function successCallback(response) {
                //Success
                tokenService.saveToken(response.data.access_token,  response.data.userName, response.data.userId);
                $scope.initRoles();
                $location.path('/');
            }, function errorCallback(response) {
                //Error
                //Try to register
                $http({
                    url: '/api/Account/register',
                    method: 'POST',
                    data: { userId: 1, password: CryptoJS.MD5(user.id).toString(), lastName: user.last_name, firstName: user.first_name, mail: user.email },
                    headers: {
                        'Content-Type': 'application/json'
                    }
                }).then(function successCallback(response) {
                    $http({
                        url: 'api/login',
                        method: 'POST',
                        data: $httpParamSerializerJQLike({ grant_type: 'password', username: user.email, password: CryptoJS.MD5(user.id).toString() }),
                        headers: {
                            'Content-Type': 'application/x-www-form-urlencoded'
                        }
                    }).then(function successCallback(response) {
                        tokenService.saveToken(response.data.access_token, response.data.userName, response.data.userId);
                        $scope.initRoles();
                        $location.path('/');
                    }, function errorCallback(response) {
                    });
                }, function errorCallback(response) {
                });
                
            });
        });
    }

}]);