var zenergyApp = angular.module('zenergyApp', ['ngResource', 'ngRoute', 'ui.bootstrap']);

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

        .when('/Register', {
            templateUrl: 'Pages/register.html',
            controller: 'registerPageController'
        })

        .when('/Users', {
            templateUrl: 'Pages/users.html',
            controller: 'usersPageController'
        })

        .when('/Account', {
            templateUrl: 'Pages/accountManagement.html',
            controller: 'accountManagementPageController'
        })

        
        .when('/Products', {
            templateUrl: 'Pages/products.html',
            controller: 'productsPageController'
        })
});



