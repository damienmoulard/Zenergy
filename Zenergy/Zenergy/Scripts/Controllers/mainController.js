zenergyApp.controller("mainController", ["$scope", "tokenService", "$window",  function ($scope, tokenService, $window) {
    $scope.isConnected = tokenService.tokenExists();
    $scope.logoff = function () {
        tokenService.deleteToken();
        $window.location.reload();
    };
}]);
