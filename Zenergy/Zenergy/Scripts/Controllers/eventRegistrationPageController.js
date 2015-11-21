

zenergyApp.controller("eventRegistrationPageController", ["$scope", "$http", "$window", "$location", function ($scope, $http, $window, $location) {

    $("#datepicker").datepicker({
        onSelect: function ()
        {
            $scope.dateSelected = $("#datepicker").datepicker('getDate');
        }
    });
     

    /*
    // Get event for this date
    var responseEvent = $http({
        url: '/api/event/' + tokenService.getUserId(),
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        }
    }).then(function successCallback(responseEvent) {
        $scope.hasError = false;
        $scope.event = {
            Id: ,
            roomName : ,
            Description : ,
            timeBegin : ,
            duration: ,            
        };
        $scope.isNotMember = $scope.user.member == null;
        //$scope.user.dateMembership = date.getMonth() + 1 + "/" + date.getDate() + "/" + date.getFullYear();
 
    });
    */

    
}]);

