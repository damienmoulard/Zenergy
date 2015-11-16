zenergyApp.controller("loginPageController", ["$scope", "$http", "$httpParamSerializerJQLike", "tokenService","$window", function ($scope,$http, $httpParamSerializerJQLike, tokenService, $window) {

    $scope.user = { mail: '', password: '' };
    $scope.hasError = false;

    $scope.connexion = function () {
        var response = $http({
            url: 'api/login',
            method: 'POST',
            data: $httpParamSerializerJQLike({ grant_type: 'password', username: $scope.user.mail, password: $scope.user.password }),
            //data: $httpParamSerializerJQLike({ grant_type: 'password', username: $scope.user.mail, password: CryptoJS.MD5($scope.user.password).toString() }),
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded'
            }
        }).then(function successCallback(response) {
            $scope.hasError = false;
            tokenService.saveToken(response.data.access_token, response.data.userName);
            window.location.replace("/Home");
        }, function errorCallback(response) {
            $scope.hasError = true;
            tokenService.deleteToken();
            $scope.user.mail = '';
            $scope.user.password = '';
        });
    };

    var auth_response_change_callback = function (response) {
        if (response.status === 'connected') {
            FB.api('/me', function (response) {
                console.log(response);
            });
        }
        //window.location.replace("/Home");
    }

    $window.fbAsyncInit = function () {
        // Executed when the SDK is loaded

        FB.init({

            appId: '429380533932654',
            channelUrl: 'app/channel.html',
            status: true,
            cookie: true,
            xfbml: true
        });

        FB.Event.subscribe('auth.authResponseChange', auth_response_change_callback);
    };

    (function (d) {
        // load the Facebook javascript SDK

        var js,
        id = 'facebook-jssdk',
        ref = d.getElementsByTagName('script')[0];

        if (d.getElementById(id)) {
            return;
        }

        js = d.createElement('script');
        js.id = id;
        js.async = true;
        js.src = "//connect.facebook.net/en_US/all.js";

        ref.parentNode.insertBefore(js, ref);

    }(document));

}]);