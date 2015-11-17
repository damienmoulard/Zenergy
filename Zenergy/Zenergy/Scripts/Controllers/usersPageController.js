zenergyApp.controller("usersPageController", ["$scope","$resource", function ($scope, $resource) {

    $scope.user.$promise.then(function () {

        if ($scope.user && $scope.user.admin) {
            var User = $resource('api/users/');
            $scope.users = User.query(function () {
                console.log($scope.users);
            });
        }
        else
            window.location.replace("/Home");

        $scope.test = function () {
            console.log("test");
        }

    });
}]);