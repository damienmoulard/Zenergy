zenergyApp.controller("mainController", ["$scope", "tokenService", "$window",  function ($scope, tokenService, $window) {

    $scope.isAuthanticated = function()
    {
        return tokenService.tokenExists();
    }

    $scope.logoff = function () {
        tokenService.deleteToken();
    };

    $scope.getUserName = function () {
        return tokenService.getUserName();
    };

}]);
