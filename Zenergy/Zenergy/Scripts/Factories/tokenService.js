zenergyApp.factory('tokenService', function ($window) {
    return {
        saveToken: function (token, username) {
            $window.sessionStorage.token = token;
            $window.sessionStorage.username = username;
        },
        getToken: function () {
            return $window.sessionStorage.token;
        },
        getUserName: function () {
            return $window.sessionStorage.username;
        },
        deleteToken: function () {
            delete $window.sessionStorage.token;
            delete $window.sessionStorage.username;
        },
        tokenExists: function () {
            return $window.sessionStorage.token !== undefined;
        }
    }
});