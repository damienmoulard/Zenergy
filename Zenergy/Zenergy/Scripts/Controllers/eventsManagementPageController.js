zenergyApp.controller("eventsManagementPageController", ["$scope", "$http", "tokenService", "$resource", "$window", "$uibModal", "$location", function ($scope, $http, tokenService, $resource, $window, $uibModal, $location) {

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

        // Update modal management
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

                /*$http.put('/api/ponctualEvents/' + eventToUpdate.eventId, eventToUpdate)
                .success(function (data, status, headers) {
                    bootbox.alert("Your event is updated!", function () {
                        window.location.reload(true);
                    });
                })
                .error(function () {
                    bootbox.alert("There has been an error during the update.");
                });

                /*var Event = $resource('api/regularEvents/:eventId', { eventId: '@id' }, {
                    update: {
                        method: 'PUT' // this method issues a PUT request		
                    }
                });
                Event.update({ eventId: eventToUpdate.eventId }, eventToUpdate);*/
                
            }, function () {
            });
        };

        // Delete modal management
        $scope.openDeleteModal = function (ponctual) {

            $scope.eventToDelete = ponctual;

            var modalInstance = $uibModal.open({
                animation: true,
                templateUrl: 'deleteModalContent.html',
                controller: 'DeleteModalInstanceCtrl',
                resolve: {
                    eventToDelete: function () {
                        return $scope.eventToDelete;
                    }
                }
            });

            modalInstance.result.then(function (eventToDelete) {
                // Delete Event
                console.log(eventToDelete);
                $http.delete('/api/ponctualEvents/' + eventToDelete.eventId).success(function () {
                    bootbox.alert("Your event is deleted!", function () {
                        window.location.reload(true);
                    });
                });
            }, function () {
            });
        };
    }
    else
        $location.path("/");
}]);

// Update modal controller
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

// Delete modal controller
zenergyApp.controller('DeleteModalInstanceCtrl', function ($scope, $uibModalInstance, eventToDelete) {

    $scope.eventToDelete = eventToDelete;

    $scope.deleteOk = function () {
        $uibModalInstance.close(eventToDelete);
    };

    $scope.deleteCancel = function () {
        $uibModalInstance.dismiss('cancel');
    };
});