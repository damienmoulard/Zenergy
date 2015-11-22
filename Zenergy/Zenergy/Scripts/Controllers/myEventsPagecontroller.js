zenergyApp.controller("myEventsPageController", ["$scope", "$http", "tokenService", "$window", "$location", function ($scope, $http, tokenService, $window, $location) {

    $scope.pastevent = false;
    //date format
    var today = new Date();
    $scope.dateSelected = today.getMonth() + 1 + "/" + today.getDate() + "/" + today.getFullYear();

    $scope.formatDate = function (today) {
        function pad(n) {
            return n < 10 ? '0' + n : n;
        }
        return today && today.getMonth() + 1 + "/" + today.getDate() + "/" + today.getFullYear();
    };

    //initialize array of events
    $scope.events = [];

    //DateTime.Now.ToString("dd-MM-yyyy")

    //function get ponctual event
    $scope.getEvent = function () {
        $scope.events = [];
        var responseEvent = $http({
            url: '/api/users/' + tokenService.getUserId() + '/events',
            method: 'GET',
            headers: {
                'Content-Type': 'application/json'
            }
        }).then(function successCallback(responseEvent) {
            console.log(responseEvent);

            //var for displaying or not the join button
            if (new Date($scope.dateSelected) < today) {
                $scope.pastevent = true;
            }
            else {
                $scope.pastevent = false;
            }
            for (var e in responseEvent.data) {

              var date = responseEvent.data[e].ponctualEvent.eventDate;
                // date = date.getMonth() + 1 + "/" + date.getDate() + "/" + date.getFullYear();
                console.log(responseEvent.data[e].ponctualEvent.eventDate);
                var dateselected = new Date($scope.dateSelected).toJSON();
                console.log(dateselected);

                // add this event to the list if it's current date
                if (date == dateselected) {
                    $scope.events.push({
                        Id: responseEvent.data[e].eventId,
                        description: responseEvent.data[e].eventDescription,
                        timeBegin: responseEvent.data[e].timeBegin,
                        duration: responseEvent.data[e].eventDurationHours,
                        price: responseEvent.data[e].eventPrice,
                        name: responseEvent.data[e].eventName,
                    });
                }
            }
            if ($scope.events.length == 0) {
                bootbox.alert("There is no event for this day");
            }

        });

    };
    //Register to the event
    $scope.unjoinEvent = function (eventid) {
        // reister to an event
        var responseEvent = $http({
            url: '/api/users/' + tokenService.getUserId() + '/events/' + eventid + '/registration',
            method: 'DELETE',
            headers: {
                'Content-Type': 'application/json'
            }
        }).then(function successCallback(responseEvent) {
            bootbox.alert("You're are now unregistered from this event !");
        });
    };

    $scope.getEvent();
   // $scope.unjoinEvent(4);


}]);

