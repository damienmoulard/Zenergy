zenergyApp.controller("eventRegistrationPageController", ["$scope", "$http", "tokenService", "$window", "$location", function ($scope, $http, tokenService, $window, $location) {

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

    //function get event by date
    $scope.getEvent = function () {
        $scope.events = [];

        // var dateselected = new Date($scope.dateSelected).toJSON();

        var responseEvent = $http({
            // url: '/api/events/bydate',
            url: '/api/ponctualEvents',
            method: 'GET',
           // data: { eventdate: dateselected },
            headers: {
                'Content-Type': 'application/json'
            }
        }).then(function successCallback(responseEvent) {
            console.log(responseEvent);
            //console.log(new Date($scope.dateSelected).toJSON());

            //var for displaying or not the join button
           if (new Date($scope.dateSelected) < today) {
                $scope.pastevent = true;
            }
            else
            {
                $scope.pastevent = false;
            }
           for (var e in responseEvent.data) {               
                $scope.events.push({
                    Id: responseEvent.data[e].eventId,
                    roomName: responseEvent.data[e].event.room.roomName,
                    Description: responseEvent.data[e].event.eventDescription,
                    timeBegin: responseEvent.data[e].event.timeBegin,
                    duration: responseEvent.data[e].event.eventDurationHours,
                    price: responseEvent.data[e].event.eventPrice,
                    name: responseEvent.data[e].event.eventName,
                    activity: responseEvent.data[e].event.activity.activityName
                });
           
            }
            if ($scope.events.length == 0) {
                bootbox.alert("There is no event for this day");
            }

        });
        
    };
    //Register to the event
    $scope.joinEvent = function (eventid) {
           // reister to an event
        var responseEvent = $http({
            url: '/api/users/' + tokenService.getUserId() + '/events/' + eventid + '/registration',
             method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            }
        }).then(function successCallback(responseEvent) {
            bootbox.alert("You just join this event !");
        });
    };

    $scope.getEvent();


    
}]);

