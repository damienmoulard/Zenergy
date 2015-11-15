zenergyApp.controller("loginPageController", ["$scope", "$http", "$httpParamSerializerJQLike", "tokenService","$window", function ($scope,$http, $httpParamSerializerJQLike, tokenService, $window) {

    $scope.user = { mail: '', password: '' };
    $scope.hasError = false;
    $http({
        url: '/api/Account/ExternalLogins?returnUrl=%2F&generateState=true',
        method: 'GET'
    }).then(function successCallback(response) {
        $scope.networks = response.data;
    }, function errorCallback(response) {
    });


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
}]);