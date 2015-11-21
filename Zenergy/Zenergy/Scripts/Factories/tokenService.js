zenergyApp.factory('tokenService', function ($window) {
    return {
        saveToken: function (token, username, userid) {
            $window.sessionStorage.token = token;
            $window.sessionStorage.username = username;
            $window.sessionStorage.setItem("userId", userid);
        },
        getToken: function () {
            return $window.sessionStorage.token;
        },
        getUserName: function () {
            return $window.sessionStorage.username;
        },
        getUserId: function () {
            return $window.sessionStorage.getItem("userId");
        },
        deleteToken: function () {
            delete $window.sessionStorage.token;
            delete $window.sessionStorage.username;
            delete $window.sessionStorage.userId;
        },
        tokenExists: function () {
            return $window.sessionStorage.token !== undefined;
        }
    }
});