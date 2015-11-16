zenergyApp.controller("homePageController", ["$scope", "tokenService","$http",  function ($scope, tokenService, $http) {
    $scope.name = tokenService.getUserName();
    var response = $http({
        url: 'api/users',
        method: 'GET',
    }).then(function successCallback(response) {
        $scope.users = response.data;
    }, function errorCallback(response) {
    });
}]);