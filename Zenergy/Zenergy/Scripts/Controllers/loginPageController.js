var loginPageController = function ($scope, $resource) {

    $scope.empty = "yes";


    var User = $resource('api/users/:id');
    $scope.user = User.get({id:2});

    $scope.connexion = function () {
        if ($scope.mail && $scope.password) {
            // get authentification
            // create token and cookies ?
            $scope.empty = "no";
        }
        else
            $scope.empty = "yes";
    };
}