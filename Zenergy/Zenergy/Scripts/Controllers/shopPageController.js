zenergyApp.controller("shopPageController", ["$scope", "$resource", "$uibModal", "$location","tokenService","$http", function ($scope, $resource, $uibModal, $location, tokenService, $http) {

    if ($scope.isAuthanticated()) {

        var Category = $resource('api/categories/:categoryId', { categoryId: '@id' }, {
        });

        $scope.categories = Category.query(function () {
        });

        var Product = $resource('api/products/:productId', { productId: '@id' }, {
        });

        $scope.products = Product.query(function () {
        });

        var CartContents = $resource('api/users/:userId/basket', { userId: '@id' });

        $scope.filter = { category: { categoryId: '' } };
        $scope.changeFilter = function(c)
        {
            $scope.filter.category.categoryId = c;
        }

        /*$scope.cartContents = CartContents.query({userId : tokenService.getUserId()}, function () {
            console.log($scope.cartContents);
        });*/

        $scope.addProduct = function (p) {
            var newCC = new CartContents();

            newCC.userId = tokenService.getUserId();
            newCC.productId = p.productId;
            newCC.productQuantity = 1;
            newCC.$save({ userId: tokenService.getUserId() });
        }

    }
    else
        $location.path("/")
}]);