var cookieState = initCookieState(getCookie('PikabaV3'));
refreshViewCookieElements();
setClickHandlers();

function generateCookieUuid() {
    var date = new Date().getTime();
    var cookieUuid = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = (date + Math.random() * 16) % 16 | 0;
        date = Math.floor(date / 16);
        return (c == 'x' ? r : (r & 0x7 | 0x8)).toString(16);
    });
    return cookieUuid;
}

function setCookie(cookieName, role, uuid, hours) {
    var dateExp = new Date();
    dateExp.setHours(dateExp.getHours() + hours);
    document.cookie = cookieName + '=' + role + ',' + uuid + ';expires=' + dateExp;
}

function getCookie(name) {
    var matches = document.cookie.match(new RegExp(
        "(?:^|; )" + name.replace(/([\.$?*|{}\(\)\[\]\\\/\+^])/g, '\\$1') + "=([^;]*)"
        ));

    if (matches == undefined) {
        return undefined;
    } else {
        var cookieArr = matches[1].split(',');
        return { role: cookieArr[0], uuid: cookieArr[1] };
    }
}

function deleteCookie(cookieName) {
    setCookie(cookieName, '', '', -1000);
    determineUser();
}

function setCookieSeller() {
    var tempCookie = {
        'uuid': generateCookieUuid(),
        'role': 'seller'
    };
    setCookie('PikabaV3', tempCookie.role, tempCookie.uuid, 10);
    cookieState.role = tempCookie.role;
    cookieState.uuid = tempCookie.uuid;
    refreshViewCookieElements();
    determineUser();
};

function setCookieBuyer() {
    var tempCookie = {
        'uuid': generateCookieUuid(),
        'role': 'buyer'
    };
    setCookie('PikabaV3', tempCookie.role, tempCookie.uuid, 10);
    cookieState.role = tempCookie.role;
    cookieState.uuid = tempCookie.uuid;
    refreshViewCookieElements();
    determineUser();
};

function removeCookie() {
    deleteCookie('PikabaV3');
    cookieState.role = 'removed';
    cookieState.uuid = 'removed';
    refreshViewCookieElements();
    determineUser();
};

function refreshViewCookieElements() {
    $('cookie-state-role').innerHTML = cookieState.role;
    $('cookie-state-uuid').innerHTML = cookieState.uuid;
}

function initCookieState(currCookie) {
    if (currCookie == undefined) {
        return {
            'role': 'none',
            'uuid': 'none'
        };
    } else {
        return {
            'role': currCookie.role,
            'uuid': currCookie.uuid
        };
    }
}

function setClickHandlers() {
    $('set-cookie-seller').onclick = setCookieSeller;
    $('set-cookie-buyer').onclick = setCookieBuyer;
    $('remove-cookie').onclick = removeCookie;
}