zenergyApp.controller("eventsManagementPageController", ["$scope", "$http", "tokenService", "$window", "$uibModal", "$location", function ($scope, $http, tokenService, $window, $uibModal, $location) {

    if ($scope.isManager()) {
        // Get manager events
        var response = $http({
            url: '/api/ponctualEvents/findByManagerId/' + tokenService.getUserId(),
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


        // modal management
        $scope.open = function (ponctual) {

            $scope.eventToUpdate = ponctual;

            var modalInstance = $uibModal.open({
                animation: true,
                templateUrl: 'updateModalContent.html',
                controller: 'UpdateModalInstanceCtrl',
                resolve: {
                    eventToUpdate: function () {
                        return $scope.eventToUpdate;
                    }
                }
            });

            modalInstance.result.then(function (eventToUpdate) {
                // Event Update
                console.log(eventToUpdate);

                var date = new Date(eventToUpdate.event.timeBegin);
                eventToUpdate.event.timeBegin = date.getHours().toString() + ":" + date.getMinutes().toString();

                eventToUpdate.event.roomId = document.getElementById("roomSelect").value;

                console.log(eventToUpdate);

                var response = $http({
                    url: '/api/ponctualEvents/' + eventToUpdate.eventId,
                    method: 'PUT',
                    data: {
                        eventId: eventToUpdate.eventId,
                        eventDate: eventToUpdate.eventDate,
                        event: {
                            eventId: eventToUpdate.event.eventId,
                            roomId: eventToUpdate.event.roomId,
                            activityId: eventToUpdate.event.activitytId,
                            eventName: eventToUpdate.event.eventName,
                            eventPrice: eventToUpdate.event.eventPrice,
                            eventDurationHours: eventToUpdate.event.eventDurationHours,
                            eventMaxPeople: eventToUpdate.event.eventMaxPeople,
                            eventDescription: eventToUpdate.event.eventDescription,
                            timeBegin: eventToUpdate.event.timeBegin
                        }
                    },
                    headers: {
                        'Content-Type': 'application/json'
                    }
                }).then(function successCallback(response) {
                    bootbox.alert("Your event is updated!", function () {
                        window.location.reload(true);
                    });
                }, function errorCallback(response) {
                    bootbox.alert("There has been an error during the update.");
                });
            }, function () {
            });
        };
    }
    else
        $location.path("/");
}]);

zenergyApp.controller('UpdateModalInstanceCtrl', function ($scope, $http, $uibModalInstance, eventToUpdate) {

    $scope.eventToUpdate = $.extend(true, {}, eventToUpdate);
    $scope.eventToUpdate.eventDate = new Date($scope.eventToUpdate.eventDate);

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

    $scope.ok = function () {
        $uibModalInstance.close($scope.eventToUpdate);
    };

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };
});