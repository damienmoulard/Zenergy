﻿var homePageController = function ($scope,$resource) {

    $scope.mail="";
    $scope.paswword="";
    $scope.empty = "yes";


    var Users = $resource('api/users/2');
    $scope.user = Users.query();

    $scope.connexion = function(){
        if ($scope.mail && $scope.password) {
            // get authentification
            // create token and cookies ?
            $scope.empty = "no";
        }
        else
            $scope.empty = "yes";
    };
}