mainApp.controller('sellerAccountCtrl', function ($scope, $http, $stateParams) {
    $http.get('http://localhost:49909/api/seller/' + $stateParams.userId).success(function (response) {
        $scope.user = response;
    }).error(function (data, status, error) {
        alert(error);
    });
});