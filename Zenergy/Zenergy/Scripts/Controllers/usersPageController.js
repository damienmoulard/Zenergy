zenergyApp.controller("usersPageController", ["$scope", "$resource", "$uibModal", function ($scope, $resource, $uibModal) {

    //if ($scope.user && $scope.user.admin) {

    $scope.sortType = 'userId'; // set the default sort type
    $scope.sortReverse = false;  // set the default sort order
    $scope.searchFish = '';     // set the default search/filter term

    $scope.users = [
    { userId: 1, firstname: 'Crab', lastname: 'Cali', mail: 'Cali@roll.fr', administrator:false },
    { userId: 3, firstname: 'Bob', lastname: 'Paul', mail: 'Bob@aaa@roll.fr', administrator: true },
    { userId: 2, firstname: 'Ami', lastname: 'Pierre', mail: 'Pierre@roll.fr', administrator: true },
    { userId: 4, firstname: 'Dude', lastname: 'Jacques', mail: 'Jacques@roll.fr', administrator: false },
    ];

    var User = $resource('api/users/:userId', { userId: '@id' }, {
        update: {
            method: 'PUT' // this method issues a PUT request
        }
    });

    $scope.users = User.query(function () {
        console.log($scope.users);
    });
   /* $scope.checkboxModel = {
        admins: [true,true,false,true],
        managers: [true, true, false, true],
        contributors: [true, true, false, true],
        members: [true, true, false, true]
    };
        /*}
        else
            window.location.replace("/Home");*/

    $scope.validate = function()
    {
        angular.forEach($scope.users, function (u, key) {
            if (!u.admin && u.adminChecked)
                u.admin = { UserId: u.userId };
            else
                u.admin = null;
            if (!u.manager && u.managerChecked)
                u.manager = { UserId: u.userId };
            else
                u.manager = null;
            if (!u.contributor && u.contributorChecked)
                u.contributor = { UserId: u.userId };
            else
                u.contributor = null;
            if (!u.member && u.memberChecked)
                u.member = { UserId: u.userId, dateMembership: new Date() };
            else
                u.contributor = null;
            //u.$update({ userId: u.userId });
        });
        console.log($scope.users);
    }

    $scope.open = function (u) {

        $scope.modifiedUser = u;

        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: 'myModalContent.html',
            controller: 'ModalInstanceCtrl',
            size: 'lg',
            /*resolve: {
                items: function () {
                    return $scope.items;
                }
            }*/
        });
    };

}]);

/*zenergyApp.controller('ModalInstanceCtrl', function ($scope, $uibModalInstance) {
    console.log($scope.modifiedUser);
    $scope.ok = function () {
        $uibModalInstance.close();
    };

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };
});*/