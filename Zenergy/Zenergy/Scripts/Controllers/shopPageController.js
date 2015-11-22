zenergyApp.controller("shopPageController", ["$scope", "$resource", "$uibModal", "$location", function ($scope, $resource, $uibModal, $location) {

    if ($scope.isAuthanticated()) {

        var Category = $resource('api/categories/:categoryId', { categoryId: '@id' }, {
        });

        $scope.categories = Category.query(function () {
        });

        var Product = $resource('api/products/:productId', { productId: '@id' }, {
            update: {
                method: 'PUT' // this method issues a PUT request
            }
        });

        $scope.products = Product.query(function () {
        });

    }
    else
        $location.path("/")
}]);