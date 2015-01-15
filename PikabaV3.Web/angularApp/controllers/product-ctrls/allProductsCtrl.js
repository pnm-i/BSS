mainApp.controller('allProductsCtrl', function ($scope, $http) {
    // Get all products
    $http.get('http://localhost:49909/api/products').success(function (result) {
        $scope.products = result;
    }).error(function (data, status, error) {
        alert(error);
    });
});