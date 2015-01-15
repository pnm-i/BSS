mainApp.controller('allCategoriesCtrl', function ($scope, $http) {
    // Get all categiries
    $http.get('http://localhost:49909/api/categories').success(function (result) {
        $scope.categoryall = result;
    }).error(function (data, status, error) {
        alert(error);
    });
});