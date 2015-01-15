mainApp.controller('editBuyerAccountCtrl', function ($scope, $http, $stateParams) {
    $http.get('http://localhost:49909/api/user/' + $stateParams.userId).success(function (response) {
        $scope.user = response;
    }).error(function (data, status, error) {
        alert(error);
    });

    $scope.saveEditedAccount = function () {
        $http.put('http://localhost:49909/api/user/' + $stateParams.userId, $scope.user).success(function (response) {
            $scope.result = response;
        }).error(function (data, status, error) {
            alert(error);
        });
    };
});