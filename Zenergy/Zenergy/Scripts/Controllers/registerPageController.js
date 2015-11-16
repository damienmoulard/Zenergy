zenergyApp.controller("registerPageController",["$scope","$http", function($scope, $http){
  
    $scope.user = {mail: '', password: '', passwordBis: '',  lastName:'', firstName:'', adr1:'', adr2:'', pc:'', town:'', phone:''};
    $scope.hasError = false;
    $scope.register = function () {
        var response = $http({
            url: 'api/Account/register',
            method: 'POST',
            data: $httpParamSerializerJQLike({ userId: 1, password: CryptoJS.MD5($scope.user.password).toString(), lastName: $scope.user.lastName, firstName: $scope.user.firstName, adr1: $scope.user.adr1, adr2: $scope.user.adr2, pc: $scope.user.pc, town: $scope.user.town, mail: $scope.user.mail, phone: $scope.user.phone }),
            // password: CryptoJS.MD5($scope.user.password).toString() }),
            headers: {
                'Content-Type': 'application/json'
            }
        }).then(function successCallback(response) {
            $scope.hasError = false;
            tokenService.saveToken(response.data.access_token, response.data.userName);
            window.location.replace("/Login");
        }, function errorCallback(response) {
            $scope.hasError = true;
            tokenService.deleteToken();
            $scope.user.mail = '';
            $scope.user.password = '';
        });

    };
}]);