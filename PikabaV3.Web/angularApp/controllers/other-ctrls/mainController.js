mainApp.controller('mainController', function ($scope, $http, $cookieStore) {
    var currentCookie = $cookieStore.get('pikaba');

    if (currentCookie == undefined) {
        $scope.start = '<div ui-view="anonimous"></div>';
    } else {
        $scope.start = '<div ui-view="user"></div>';
    }
});