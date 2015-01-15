mainApp.controller('editSellerAccountCtrl', function ($scope, $http, $stateParams) {
    $http.get('http://localhost:49909/api/seller/' + $stateParams.userId).success(function (response) {
        $scope.user = response;
    }).error(function (data, status, error) {
        alert(error);
    });

    $scope.saveEditedAccount = function () {
        $http.put('http://localhost:49909/api/seller/' + $stateParams.userId, $scope.user).success(function (response) {
            $scope.result = response;
        }).error(function (data, status, error) {
            alert(error);
        });
    };
});