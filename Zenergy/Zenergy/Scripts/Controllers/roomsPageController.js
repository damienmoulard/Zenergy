zenergyApp.controller("roomsPageController", ["$scope", "$resource", "$uibModal","$location", function ($scope, $resource, $uibModal,$location) {

    if ($scope.isAdmin()) {

        $scope.sortType = 'roomId'; // set the default sort type
        $scope.sortReverse = false;  // set the default sort order
        $scope.searchRoom = '';     // set the default search/filter term
        $scope.showInputs = false;


        var Room = $resource('api/rooms/:roomId', { roomId: '@id' }, {
            update: {
                method: 'PUT' // this method issues a PUT request
            }
        });

        $scope.rooms = Room.query(function () {
        });

        var Accessory = $resource('api/accessories/', {
        });

        $scope.accessories = Accessory.query(function () {
        });


        $scope.delete = function (r) {

            $scope.roomToDelete = r;

            var modalInstance = $uibModal.open({
                animation: true,
                templateUrl: 'deleteRoomContent.html',
                controller: 'deleteRoomModalController',
                size: 'sm',
                resolve: {
                    roomToDelete: function () {
                        return $scope.roomToDelete;
                    }
                }
            });

            modalInstance.result.then(function () {
                //Suppression du produit.
                Room.delete({ roomId: $scope.roomToDelete.roomId });
                $('#tr' + $scope.roomToDelete.roomId).fadeOut('slow', function () {
                    var index = $scope.rooms.indexOf($scope.roomToDelete);
                    $scope.rooms.splice(index, 1);
                });

            }, function () {
            });
        };

        $scope.update = function (r) {

            $scope.roomToUpdate = r;
            console.log($scope.roomToUpdate)

            var modalInstance = $uibModal.open({
                animation: true,
                templateUrl: 'updateRoomContent.html',
                controller: 'updateRoomModalController',
                size: 'lg',
                resolve: {
                    roomToUpdate: function () {
                        return $scope.roomToUpdate;
                    },
                    accessories: function () {
                        return $scope.accessories;
                    }
                }
            });

            modalInstance.result.then(function () {
                //Maj du produit.
                $scope.roomToUpdate.$update({ roomId: $scope.roomToUpdate.roomId });

            }, function () {
            });
        };


        $scope.add = function () {
            $scope.showInputs = true;
            $scope.newRoom = new Room({roomName:''});
        };

        $scope.confirm = function () {
            var r = Room.save(null, $scope.newRoom, function () {
                $scope.rooms.push(r);
                $scope.showInputs = false;
            });
        }

    }
    else
        $location.path("/")
    }]);

    zenergyApp.controller('deleteRoomModalController', function ($scope, $uibModalInstance, roomToDelete) {

        $scope.roomToDelete = roomToDelete;

        $scope.ok = function () {
            $uibModalInstance.close();
        };

        $scope.cancel = function () {
            $uibModalInstance.dismiss('cancel');
        };
    });


    zenergyApp.controller('updateRoomModalController', function ($scope, $uibModalInstance,$resource, roomToUpdate, accessories) {

        var RC = $resource('api/roomContents/', {
        });
        $scope.roomToUpdate = roomToUpdate;
        $scope.showInputsAcc = false;
        $scope.accessories = accessories;

        $scope.ok = function () {
            $uibModalInstance.close();
        };

        $scope.deleteRC = function (rc) {
            //Suppression du RoomContent
            RC.delete({ roomId: rc.roomId, accessoryId: rc.accessoryId });
            $('#trAcc' + rc.accessoryId).fadeOut('slow', function () {
                var index = $scope.roomToUpdate.roomContent.indexOf(rc);
                $scope.roomToUpdate.roomContent.splice(index, 1);
            });
        };


        $scope.addRC = function () {
            $scope.showInputsAcc = true;
            $scope.newRC = new RC({ roomId: $scope.roomToUpdate.roomId});
        };

        $scope.confirmRC = function () {
            $scope.newRC.accessoryId = $scope.newRC.accessory.accessoryId;
            var temp = $scope.newRC.accessory.accessoryName;
            delete $scope.newRC.accessory;
            var rc = RC.save(null, $scope.newRC, function () {
                rc.accessory = { accessoryName: temp };
                $scope.roomToUpdate.roomContent.push(rc);
                $scope.showInputsAcc = false;
            });
        }

    });