function determineUser() {
    if (cookieState.role === 'seller' || cookieState.role === 'buyer') {
        getHTMLTemplate('site-menu', 'pages/other-pages/authorizeMenu.html');
    } else {
        getHTMLTemplate('site-menu', 'pages/other-pages/anonMenu.html');
    }
};

function getHTMLTemplate(id, path) {
    var xmlhttp = new XMLHttpRequest();
    xmlhttp.onreadystatechange = function () {
        if (xmlhttp.readyState === 4 && xmlhttp.status === 200) {
            $(id).innerHTML = xmlhttp.responseText;
        }
    };
    xmlhttp.open('GET', path);
    xmlhttp.send();
};

document.getElementById("headerMenu").addEventListener("load", myFunction);

function myFunction() {
    document.getElementById("home").innerHTML = "Iframe is loaded.";
}



