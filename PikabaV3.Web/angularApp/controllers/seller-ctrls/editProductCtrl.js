mainApp.controller('editProductCtrl', function ($scope, $http, $cookies) {

    $http.get('http://localhost:49909/api/product/' + $stateParams.productId).success(function (response) {
        $scope.product = response;
    }).error(function (data, status, error) {
        alert(error);
    });

    var currentCookie = $cookies.PikabaV3;
    $scope.updateProduct = function () {
        $http.put('http://localhost:49909/api/product/' + currentCookie, $scope.product).success(function () {
        }).error(function (status, error) {
            alert(error);
        });
    };
});