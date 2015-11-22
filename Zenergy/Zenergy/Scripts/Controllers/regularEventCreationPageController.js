zenergyApp.controller("regularEventCreationPageController", ["$scope", "$http", "tokenService", "$window", "$location", function ($scope, $http, tokenService, $window, $location) {

    // Get activities
    var response = $http({
        url: '/api/activities/findByManagerId/' + tokenService.getUserId(),
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        }
    }).then(function successCallback(response) {
        $scope.hasError = false;
        $scope.activities = [];
        $scope.activities = $.parseJSON(JSON.stringify(response.data));
        console.log($scope.activities);
    });

    // Get rooms
    var response = $http({
        url: '/api/rooms',
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        }
    }).then(function successCallback(response) {
        $scope.hasError = false;
        $scope.rooms = [];
        $scope.rooms = $.parseJSON(JSON.stringify(response.data));
        console.log($scope.rooms);
    });

    $scope.regularEvent = {};

}]);