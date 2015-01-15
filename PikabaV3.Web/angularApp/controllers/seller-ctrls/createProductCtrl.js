mainApp.controller('createProductCtrl', function ($scope, $http, $cookies) {
    var currentCookie = $cookies.PikabaV3;
    $scope.createProduct = function () {
        $http.post('http://localhost:49909/api/product/' + currentCookie, this.product).success(function () {
        }).error(function (status, error) {
            alert(error);
        });
    };
});