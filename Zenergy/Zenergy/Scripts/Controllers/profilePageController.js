zenergyApp.controller("profilePageController", ["$scope","tokenService", "$window", function ($scope, tokenService, $window) {
if(!$scope.isConnected)
    window.location.replace("/Login");
}]);