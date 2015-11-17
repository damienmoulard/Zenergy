zenergyApp.controller("mainController", ["$scope", "tokenService", "$window", "$resource",  function ($scope, tokenService, $window,$resource) {
  

    if (tokenService.tokenExists()) {
        var User = $resource('api/users/:userId', { userId: '@id' });
        //TODO : remplacer 2 par le vrai id
        $scope.user = User.get({ userId: 2 }, function () {
        });
    }


    $scope.logoff = function () {
        tokenService.deleteToken();
        $window.location.reload();
        $scope.user = null;
    };

}]);
