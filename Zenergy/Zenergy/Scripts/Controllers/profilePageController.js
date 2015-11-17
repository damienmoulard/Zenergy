zenergyApp.controller("profilePageController", ["$scope", "$window", "$http",'$location', function ($scope, $window, $http, $location)
{
    if (!$scope.isAuthanticated())
        $location.path("/Login");
    else
    {
        var response = $http({
            url: 'api/users',
            method: 'GET',
        }).then(function successCallback(response) {
            $scope.users = response.data;
        }, function errorCallback(response) {
        });
    }
}]);