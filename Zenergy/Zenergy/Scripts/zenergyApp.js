var zenergyApp = angular.module('zenergyApp', ['ngResource', 'ngRoute', 'ui.bootstrap', 'mp.datePicker']);

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
        .when('/Events', {
            templateUrl: 'Pages/eventRegistration.html',
            controller: 'eventRegistrationPageController'
        })

        .when('/MyEvents', {
            templateUrl: 'Pages/eventsManagement.html',
            controller: 'eventsManagementPageController'
        })

        .when('/CreateEvent', {
            templateUrl: 'Pages/ponctualEventCreation.html',
            controller: 'ponctualEventCreationPageController'
        })

        .when('/Products', {
            templateUrl: 'Pages/products.html',
            controller: 'productsPageController'
        })

        .when('/Rooms', {
            templateUrl: 'Pages/rooms.html',
            controller: 'roomsPageController'
        })

        .when('/Shop', {
            templateUrl: 'Pages/shop.html',
            controller: 'shopPageController'
        })

        .when('/Cart', {
            templateUrl: 'Pages/cart.html',
            controller: 'cartPageController'
        })
});



