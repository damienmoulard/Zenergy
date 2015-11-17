﻿var zenergyApp = angular.module('zenergyApp', ['ngResource', 'ngRoute']);

zenergyApp.config(function ($routeProvider) {
    $routeProvider

        // route for the home page
        .when('/', {
            templateUrl: 'Pages/home.html',
            controller: 'homePageController'
        })

        .when('/Login', {
                templateUrl: 'Pages/login.html',
                controller: 'loginPageController'
        })

        .when('/Profile', {
            templateUrl: 'Pages/profile.html',
            controller: 'profilePageController'
        })
});



