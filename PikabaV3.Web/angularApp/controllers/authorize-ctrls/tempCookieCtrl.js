mainApp.controller('tempCookieCtrl', function ($scope, $cookieStore) {
    // Generate cookie Uuid
    var date = new Date().getTime();

    var currentCookie = $cookieStore.get('pikaba');
    $scope.cookieState = {};

    if (currentCookie == undefined) {
        $scope.cookieState = {
            'role': 'none',
            'uuid': 'none'
        };
    } else {
        $scope.cookieState = {
            'role': currentCookie.role,
            'uuid': currentCookie.uuid
        };
    }

    function getCookieUuid() {
        var cookieUuid = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
            var r = (date + Math.random() * 16) % 16 | 0;
            date = Math.floor(date / 16);
            return (c == 'x' ? r : (r & 0x7 | 0x8)).toString(16);
        });
        return cookieUuid;
    }

    $scope.setCookieSeller = function () {
        var cookie = {
            'uuid': getCookieUuid(),
            'role': 'seller'
        };
        $cookieStore.put('pikaba', cookie);
        $scope.cookieState.role = cookie.role;
        $scope.cookieState.uuid = cookie.uuid;
    };

    $scope.setCookieBuyer = function () {
        var cookie = {
            'uuid': getCookieUuid(),
            'role': 'buyer'
        };
        $cookieStore.put('pikaba', cookie);
        $scope.cookieState.role = cookie.role;
        $scope.cookieState.uuid = cookie.uuid;
    };

    $scope.removeCookie = function () {
        $cookieStore.remove('pikaba');
        $scope.cookieState.role = 'removed';
        $scope.cookieState.uuid = 'removed';
    };
});