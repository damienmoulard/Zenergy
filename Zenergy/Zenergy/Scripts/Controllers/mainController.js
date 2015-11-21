zenergyApp.controller("mainController", ["$scope", "tokenService", "$window", "$resource", function ($scope, tokenService, $window,$resource) {

    $scope.isAuthanticated = function () {
        return tokenService.tokenExists();
    }

    $scope.logoff = function () {
        tokenService.deleteToken();
    };

    $scope.getUserName = function () {
        return tokenService.getUserName();
    };

    $scope.getUserId = function () {
        return tokenService.getUserId();
    };

    $scope.isAdmin = function () {
        return tokenService.isAdmin();
    }

    $scope.isManager = function () {
        return tokenService.isManager();
    }

    $scope.isContributor = function () {
        return tokenService.isContributor();
    }

    $scope.isMember = function () {
        return tokenService.isMember();
    }

    $scope.initRoles = function () {

        var Admin = $resource('api/admins/:userId', { userId: '@id' }, {
        });
        var Manager = $resource('api/managers/:userId', { userId: '@id' }, {
        });
        var Contributor = $resource('api/contributors/:userId', { userId: '@id' }, {
        });
        var Member = $resource('api/members/:userId', { userId: '@id' }, {
        });

        Admin.get({ userId: tokenService.getUserId() },
        function (data) {
            tokenService.setAdmin(true);
        },
        function (error) {
            tokenService.setAdmin(false);
        });
        Manager.get({ userId: tokenService.getUserId() },
        function (data) {
            tokenService.setManager(true);
        },
        function (error) {
            tokenService.setManager(false);
        });
        Contributor.get({ userId: tokenService.getUserId() },
        function (data) {
            tokenService.setContributor(true);
        },
        function (error) {
            tokenService.setContributor(false);
        });
        Member.get({ userId: tokenService.getUserId() },
        function (data) {
            tokenService.setMember(true);
        },
        function (error) {
            tokenService.setMember(false);
        });
    }


}]);
