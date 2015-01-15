mainApp.controller('detailsProductCtrl', function ($scope, $http, $stateParams) {
    // Get details one product
    $http.get('http://localhost:49909/api/product/' + $stateParams.productId).success(function (result) {
        $scope.product = result;
    }).error(function (data, status, error) {
        alert(error);
    });
});