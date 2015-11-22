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
        setAdmin(bool)
        {
            $window.sessionStorage.setItem("isAdmin", bool);
        },
        setManager(bool) {
            $window.sessionStorage.setItem("isManager", bool);
        },
        setContributor(bool) {
            $window.sessionStorage.setItem("isContributor", bool);
        },
        setMember(bool) {
            $window.sessionStorage.setItem("isMember", bool);
        },
        isAdmin()
        {
            return $window.sessionStorage.getItem("isAdmin");
        },
        isManager() {
            return $window.sessionStorage.getItem("isManager");
        },
        isContributor() {
            return $window.sessionStorage.getItem("isContributor");
        },
        isMember() {
            return $window.sessionStorage.getItem("isMember");
        },
        deleteToken: function () {
            delete $window.sessionStorage.clear();
        },
        tokenExists: function () {
            return $window.sessionStorage.token !== undefined;
        }
    }
});