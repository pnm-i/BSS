mainApp.controller('deleteBuyerAccountCtrl', function ($scope, $http, $stateParams, $location) {
    $scope.deleteAccountYes = function () {
        $http.delete('http://localhost:49909/api/user/' + $stateParams.userId).success(function (response) {
            $scope.result = response;
        }).error(function (data, status, error) {
            alert(error);
        });
        $scope.deleteAccountNo = function () {
            //$location.path('/');
            alert('change url');
        };
    };
});