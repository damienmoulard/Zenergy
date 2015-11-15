zenergyApp.controller("homePageController", ["$scope", "tokenService","$http",  function ($scope, tokenService, $http) {
    $scope.name = tokenService.getUserName();

    $http({
        url: 'api/Account/ExternalLogin',
        method: 'GET'
    }).then(function successCallback(response) {
        console.log(response.data);
        $scope.users = response.data;
    }, function errorCallback(response) {
    });
}]);