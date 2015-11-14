zenergyApp.controller("homePageController", ["$scope", "tokenService",  function ($scope, tokenService) {
    $scope.name = tokenService.getUserName();
}]);