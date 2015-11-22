zenergyApp.controller("ponctualEventCreationPageController", ["$scope", "$http", "tokenService", "$window", "$location", function ($scope, $http, tokenService, $window, $location) {

    // Get activities
    var response = $http({
        url: '/api/activities/findByManagerId/' + tokenService.getUserId(),
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        }
    }).then(function successCallback(response) {
        $scope.hasError = false;
        $scope.activities = [];
        $scope.activities = $.parseJSON(JSON.stringify(response.data));
        console.log($scope.activities);
    });

    // Get rooms
    var response = $http({
        url: '/api/rooms',
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        }
    }).then(function successCallback(response) {
        $scope.hasError = false;
        $scope.rooms = [];
        $scope.rooms = $.parseJSON(JSON.stringify(response.data));
        console.log($scope.rooms);
    });

    $scope.ponctualEvent = { eventId: 1, eventDate: new Date(), event: { eventId: 1, roomId: '', activityId: '', eventName: '', eventPrice: '', eventDurationHours: '', eventMaxPeople: '', eventDescription: '', timeBegin: '' } };

    $scope.createEvent = function () {
        //console.log($scope.ponctualEvent);

        $scope.hasError = false;
        $scope.dateError = false;

        var today = new Date();
        if ($scope.ponctualEvent.eventDate <= today) {
            $scope.hasError = true;
            $scope.dateError = true;
        }

        var date = new Date($scope.ponctualEvent.event.timeBegin);
        $scope.ponctualEvent.event.timeBegin = date.getHours().toString() + ":" + date.getMinutes().toString();
        
        $scope.ponctualEvent.event.roomId = document.getElementById("roomSelect").value;
        $scope.ponctualEvent.event.activityId = document.getElementById("actSelect").value;

        if (!$scope.hasError)
        {
            var response = $http({
                url: '/api/ponctualEvents',
                method: 'POST',
                data: $scope.ponctualEvent,
                headers: {
                    'Content-Type': 'application/json'
                }
            }).then(function successCallback(response) {
                bootbox.alert("Your event is now created!", function () {
                    $location.path("/#/MyEvents");
                });
            }, function errorCallback(response) {
                bootbox.alert("There has been an error during the event creation. Please check your event information.");
            });
        }
    }
}]);