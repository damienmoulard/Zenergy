zenergyApp.directive('samepassword', function () {

    return {
        require: 'ngModel',
        link: function (scope, elm, attrs, ctrl) {
            ctrl.$validators.samepassword = function (modelValue, viewValue) {
                if (ctrl.$isEmpty(modelValue)) {
                    // consider empty models to be invalid
                    return false;
                }

                if (viewvalue != $scope.user) {
                    // it is valid
                    return true;
                }

                // it is invalid
                return false;
            };
        }
    }

});


zenergyApp.controller("registerPageController", ["$scope", "$http", "$window", function ($scope, $http, $window) {
  
    $scope.user = {mail: '', password: '', passwordBis: '',  lastName:'', firstName:'', adr1:'', adr2:'', pc:'', town:'', phone:''};
    $scope.hasError = false;
        
    $scope.register = function () {
        var response = $http({
            url: 'api/Account/register',
            method: 'POST',
            data: { userId: 1, password: CryptoJS.MD5($scope.user.password).toString(), lastName: $scope.user.lastName, firstName: $scope.user.firstName, adr1: $scope.user.adr1, adr2: $scope.user.adr2, pc: $scope.user.pc, town: $scope.user.town, mail: $scope.user.mail, phone: $scope.user.phone },
            headers: {
                'Content-Type': 'application/json'
            }
        }).then(function successCallback(response) {
            $scope.hasError = false;
            window.location.replace("/Login");
        }, function errorCallback(response) {
            $scope.hasError = true;
            $scope.user.mail = '';
            $scope.user.password = '';
        });

    };
}]);