zenergyApp.controller("productsPageController", ["$scope", "$resource", "$uibModal","$location", function ($scope, $resource, $uibModal,$location) {

    if ($scope.isAdmin()) {

        $scope.sortType = 'productId'; // set the default sort type
        $scope.sortReverse = false;  // set the default sort order
        $scope.searchProduct = '';     // set the default search/filter term
        $scope.filterByCateg = '';     // set the default search/filter term
        $scope.showInputs = false;
        $scope.addButton = "Add";


        var Product = $resource('api/products/:productId', { productId: '@id' }, {
        });

        $scope.products = Product.query(function () {
        });

        var Category = $resource('api/categories/:categoryId', { categoryId: '@id' }, {
        });

        $scope.categories = Category.query(function () {
        });


        
        $scope.delete = function (p) {

            $scope.productToDelete = p;

            var modalInstance = $uibModal.open({
                animation: true,
                templateUrl: 'deleteProductContent.html',
                controller: 'deleteProductModalController',
                size: 'sm',
                resolve: {
                    productToDelete: function () {
                        return $scope.productToDelete;
                    }
                }
            });

            modalInstance.result.then(function () {
                //Suppression du produit.
                Product.delete({ productId: $scope.productToDelete.productId });
                $('#tr' + $scope.productToDelete.productId).fadeOut('slow', function () {
                    var index = $scope.products.indexOf($scope.productToDelete);
                    $scope.products.splice(index, 1);
                });

            }, function () {
            });
        };

        $scope.update = function (p) {

            $scope.productToUpdate = p;

            var modalInstance = $uibModal.open({
                animation: true,
                templateUrl: 'updateProductContent.html',
                controller: 'updateProductModalController',
                size: 'lg',
                resolve: {
                    productToUpdate: function () {
                        return $scope.productToUpdate;
                    },
                    categories: function () {
                        return $scope.categories;
                    }
                }
            });

            modalInstance.result.then(function () {
                //Maj du produit.
                $scope.productToUpdate.categoryId = $scope.productToUpdate.category.categoryId;
                $scope.productToUpdate.$update({ productId: $scope.productToUpdate.productId });

            }, function () {
            });
        };


        $scope.add = function () {
            $scope.showInputs = true;
            $scope.newProduct = new Product({productName:''});
        };

        $scope.confirm = function () {
            var p = Product.save(null, $scope.newProduct, function () {
                $scope.products.push(p);
                $scope.showInputs = false;
            });
        }

    }
    else
        $location.path("/")
    }]);

    zenergyApp.controller('deleteProductModalController', function ($scope, $uibModalInstance, productToDelete) {

        $scope.productToDelete = productToDelete;

        $scope.ok = function () {
            $uibModalInstance.close();
        };

        $scope.cancel = function () {
            $uibModalInstance.dismiss('cancel');
        };
    });


    zenergyApp.controller('updateProductModalController', function ($scope, $uibModalInstance, productToUpdate, categories) {

        $scope.productToUpdate = productToUpdate;
        $scope.categories = categories;

        $scope.ok = function () {
            $uibModalInstance.close();
        };

    });