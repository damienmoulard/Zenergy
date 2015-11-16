zenergyApp.controller("registerPageController", ["$scope", "$http", "$httpParamSerializerJQLike", "tokenService", "$window", function ($scope, $http, $httpParamSerializerJQLike, tokenService, $window) {

    $scope.user = { mail: '', password: '', passwordBis: '', firstName :'', lastName:'', adr1:'', adr2:'', pc:'', town:'', phone:''};
    $scope.hasError = false;
    $scope.signIn = function () {
        var response = $http({
            url: 'api/Account/register',
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