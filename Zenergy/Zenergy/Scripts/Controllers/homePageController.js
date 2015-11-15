zenergyApp.controller("homePageController", ["$scope", "tokenService","$http",  function ($scope, tokenService, $http) {
    $scope.name = tokenService.getUserName();

    $http({
        url: '/api/Account/ExternalLogins?returnUrl=%2F&generateState=true',
        method: 'GET'
    }).then(function successCallback(response) {
        angular.forEach(response.data, function (value, key) {
            console.log(value);
        });
    }, function errorCallback(response) {
    });

}]);