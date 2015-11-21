zenergyApp.controller("usersPageController", ["$scope", "$resource", "$uibModal", function ($scope, $resource, $uibModal) {

    //if ($scope.user && $scope.user.admin) {

    $scope.sortType = 'userId'; // set the default sort type
    $scope.sortReverse = false;  // set the default sort order
    $scope.searchFish = '';     // set the default search/filter term


    var User = $resource('api/users/:userId', { userId: '@id' }, {
        update: {
            method: 'PUT' // this method issues a PUT request
        }
    });
    var Admin = $resource('api/admins/:userId', { userId: '@id' }, {
    });
    var Manager = $resource('api/managers/:userId', { userId: '@id' }, {
    });
    var Contributor = $resource('api/contributors/:userId', { userId: '@id' }, {
    });
    var Member = $resource('api/members/:userId', { userId: '@id' }, {
    });


    $scope.users = User.query(function () {
    });

    $scope.validate = function()
    {
        angular.forEach($scope.users, function (u, key) {

            if (u.adminChecked)
                Admin.save({userId : u.userId}) //admin role added
            else
                Admin.delete({ userId: u.userId }); //admin role removed

            if (u.managerChecked)
                Manager.save({ userId: u.userId });//manager role added
            else
                Manager.delete({ userId: u.userId }); //manager role removed

            if (u.contributorChecked)
                Contributor.save({ userId: u.userId });//contributor role added
            else
                Contributor.delete({ userId: u.userId }); //contributor role removed

            if (u.memberChecked)
                Member.save({ userId: u.userId });//member role added
            else if (u.memberChecked)
                Member.delete({ userId: u.userId });//member role removed

        });
    }




    $scope.open = function (u) {

        $scope.userTodelete = u;

        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: 'myModalContent.html',
            controller: 'ModalInstanceCtrl',
            size: 'sm',
            resolve: {
                userTodelete: function () {
                    return $scope.userTodelete;
                }
            }
        });

        modalInstance.result.then(function () {
            //Suppression de l'utilisateur.
            User.delete({ userId: $scope.userTodelete.userId });
            $('#tr' + $scope.userTodelete.userId).fadeOut('slow', function () {
                var index = $scope.users.indexOf($scope.userTodelete);
                $scope.users.splice(index, 1);
            });

        }, function () {
        });
    };

}]);

zenergyApp.controller('ModalInstanceCtrl', function ($scope, $uibModalInstance, userTodelete) {

    console.log(userTodelete);
    $scope.userTodelete = userTodelete;

    $scope.ok = function () {
        $uibModalInstance.close();
    };

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };
});