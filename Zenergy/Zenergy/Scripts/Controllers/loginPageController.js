zenergyApp.controller("loginPageController", ["$scope", "$http", "$httpParamSerializerJQLike", "tokenService","$window", "$location", function ($scope,$http, $httpParamSerializerJQLike, tokenService, $window, $location) {

    $scope.user = { mail: '', password: '' };
    $scope.hasError = false;

    $scope.connexion = function () {
        var response = $http({
            url: 'api/login',
            method: 'POST',
            data: $httpParamSerializerJQLike({ grant_type: 'password', username: $scope.user.mail, password: CryptoJS.MD5($scope.user.password).toString() }),
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded'
            }
        }).then(function successCallback(response) {
            $scope.hasError = false;
            tokenService.saveToken(response.data.access_token, response.data.userName, response.data.userId);
            $location.path('/');
        }, function errorCallback(response) {
            $scope.hasError = true;
            tokenService.deleteToken();
            $scope.user.mail = '';
            $scope.user.password = '';
        });
    };

}]);