zenergyApp.controller("accountManagementPageController", ["$scope", "$http", "tokenService", "$window", "$location", function ($scope, $http, tokenService, $window, $location) {

    // Get user data
    var response = $http({
        url: '/api/users/' + tokenService.getUserId(),
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        }
    }).then(function successCallback(response) {
        $scope.hasError = false;
        $scope.user = { mail: response.data.mail, password: response.data.password, lastName: response.data.lastname, firstName: response.data.firstname, adr1: response.data.adr1, adr2: response.data.adr2, pc: response.data.pc, town: response.data.town, phone: response.data.phone, member: response.data.member };
        $scope.isNotMember = $scope.user.member == null;
        if (!$scope.isNotMember) {
            var date = new Date(response.data.member.dateMembership);
            $scope.user.dateMembership = date.getMonth()+1 + "/" + date.getDate() + "/" + date.getFullYear();
        }
    });

    $scope.hasError = false;
    $scope.passNotMatch = false;

    // Modify account info
    $scope.changeInformations = function () {
        if (!$scope.hasError) {
            var response = $http({
                url: '/api/users/' + tokenService.getUserId(),
                method: 'PUT',
                data: { userId: tokenService.getUserId(), password: $scope.user.password, lastName: $scope.user.lastName, firstName: $scope.user.firstName, adr1: $scope.user.adr1, adr2: $scope.user.adr2, pc: $scope.user.pc, town: $scope.user.town, mail: $scope.user.mail, phone: $scope.user.phone },
                headers: {
                    'Content-Type': 'application/json'
                }
            }).then(function successCallback(response) {
                $scope.hasError = false;
                bootbox.alert("Your account is updated!", function () {
                    window.location.reload(true);
                });
            }, function errorCallback(response) {
                $scope.hasError = true;
            });
        }
    };

    // Change password
    $scope.changePassword = function () {

        if ($scope.user.newPassword != $scope.user.newPasswordBis) {
            $scope.passNotMatch = true;
        }
        else {
            $scope.passNotMatch = false;
        }

        if (!$scope.passNotMatch) {
            var response = $http({
                url: '/api/users/' + tokenService.getUserId(),
                method: 'PUT',
                data: { userId: tokenService.getUserId(), password: CryptoJS.MD5($scope.user.newPassword).toString(), lastName: $scope.user.lastName, firstName: $scope.user.firstName, adr1: $scope.user.adr1, adr2: $scope.user.adr2, pc: $scope.user.pc, town: $scope.user.town, mail: $scope.user.mail, phone: $scope.user.phone },
                headers: {
                    'Content-Type': 'application/json'
                }
            }).then(function successCallback(response) {
                $scope.hasError = false;
                window.location.reload(true);
                bootbox.alert("Your account is updated!", function () {
                    window.location.reload(true);
                });
            }, function errorCallback(response) {
                $scope.hasError = true;
            });
        }
    };

    // Add membership
    $scope.becomeMember = function () {
        var today = new Date();

        var response = $http({
            url: '/api/members/',
            method: 'POST',
            data: { userId: tokenService.getUserId(), dateMembership: today },
            headers: {
                'Content-Type': 'application/json'
            }
        }).then(function successCallback(response) {
            $scope.hasError = false;
            window.location.reload(true);
        }, function errorCallback(response) {
            $scope.hasError = true;
        });
    };
}]);