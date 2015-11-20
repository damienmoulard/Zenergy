zenergyApp.controller("homePageController", ["$scope", "tokenService","$http",  function ($scope, tokenService, $http) {
    $scope.name = tokenService.getUserName();
}]);