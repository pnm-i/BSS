mainApp.controller('loginCtrl', function ($scope, $http, $cookies) {
    // Generate cookie Uuid
    var date = new Date().getTime();
    var cookieUuid = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = (date + Math.random() * 16) % 16 | 0;
        date = Math.floor(date / 16);
        return (c == 'x' ? r : (r & 0x7 | 0x8)).toString(16);
    });

    // Login
    $scope.loginClick = function () {
        $http.post('http://localhost:49909/api/account/login/' + cookieUuid, this.login).success(function (data) {
            alert(data);
            $cookies.PikabaV3 = cookieUuid;
        }).error(function (status, error) {
            alert(error);
        });
    };
});