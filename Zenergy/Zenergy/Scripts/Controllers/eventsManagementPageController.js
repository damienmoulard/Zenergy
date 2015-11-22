zenergyApp.controller("eventsManagementPageController", ["$scope", "$http", "tokenService", "$window", "$location", function ($scope, $http, tokenService, $window, $location) {

    // Get manager events
    // TODO : get THIS MANAGER events
    var response = $http({
        url: '/api/ponctualEvents',
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        }
    }).then(function successCallback(response) {
        $scope.hasError = false;
        $scope.ponctualEvents = [];
        $scope.ponctualEvents = $.parseJSON(JSON.stringify(response.data));
        console.log($scope.ponctualEvents);
    });

    $scope.updateEvent = function (eventRow) {
        console.log(eventRow);

        // Store the event rw in the events array
        $scope.eventToUpdate = eventRow;
        // $scope.eventToUpdate = ponctualEvents[eventRow];
        // comme ça on met ça en paramettre du put, on se fait plus chier avec les index

        // Show modal to update infos
        $('#myModal').modal('show');
        
        // Set the function for the modal "ok" button
        $(document).ready(function () {
            $('#commitUpdateButton').click(function () {
                commitUpdateEvent();
            });
        });
    };

    // Add membership
    commitUpdateEvent = function () {
        // launch http request with updated event

        /*var today = new Date();

        var response = $http({
            url: '/api/members/',
            method: 'POST',
            data: {
                userId: tokenService.getUserId(),
                dateMembership: today
            },
            headers: {
                'Content-Type': 'application/json'
            }
        }).then(function successCallback(response) {
            $scope.hasError = false;
            window.location.reload(true);
        }, function errorCallback(response) {
            $scope.hasError = true;
        });*/
    };
}]);