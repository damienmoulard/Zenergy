zenergyApp.controller("cartPageController", ["$scope", "$resource", "$uibModal", "$location", "tokenService", "$route", function ($scope, $resource, $uibModal, $location, tokenService, $route) {

    if ($scope.isAuthanticated()) {


        var CartContents = $resource('api/users/:userId/basket/:productId', { userId: '@id', productId: '@id' }, {
            update: {
                method: 'PUT' // this method issues a PUT request		
            },
            removeFromBasket: {
                method: 'DELETE', isArray: true
            }
        });

        $scope.cartContents = CartContents.query({ userId: tokenService.getUserId() }, function () {
            console.log($scope.cartContents);
        });

        $scope.getTotal = function () {
            var total = 0;
            for (var i = 0; i < $scope.cartContents.length; i++) {
                var c = $scope.cartContents[i];
                if ($scope.isMember())
                    total += ((c.product.productPrice - ((c.product.productPrice * c.product.memberReduction) / 100)) * c.productQuantity);
                else
                    total += (c.product.productPrice * c.productQuantity);
            }
            return total;
        }

        $scope.deleteCC = function (c) {
            CartContents.removeFromBasket({ userId: c.userId, productId: c.productId });
            $('#tr' + c.productId).fadeOut('slow', function () {
                var index = $scope.cartContents.indexOf(c);
                $scope.cartContents.splice(index, 1);
            });
        };

        $scope.setQuantity = function (c, i) {
            c.productQuantity = c.productQuantity + i;
            c.$update({ userId: c.userId },
                function (response) {
                },
                function (response) {
                    c.productQuantity = c.productQuantity - i;
                });
        }

        $scope.proceed = function () {
            var BasketValidation = $resource('api/users/' + tokenService.getUserId() + '/basket/validate', {}, {
                update: {
                    method: 'PUT' // this method issues a PUT request		
                }
            });

            BasketValidation.update({},
                function (response) {
                    $route.reload();
                },
                function (response) {
                });
        };



    }
    else
        $location.path("/");
    }]);
