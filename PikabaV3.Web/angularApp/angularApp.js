/*********************************** Anonymous User **************************************/
angular.module('AnonymousApp', [])
    .config(function ($stateProvider) {
        $stateProvider
            .state('anon-categories', {
                url: '/categories',
                views: { 'action': { templateUrl: 'angularApp/pages/other-pages/categories.html' } },
                controller: 'allCategoriesCtrl'
            })
            .state('anon-products', {
                url: '/products',
                views: { 'menuAction': { templateUrl: 'angularApp/pages/other-pages/products.html' } },
                controller: 'allProductsCtrl'
            })
            .state('anon-products:productId', {
                url: '/products/:productId',
                views: { 'menuAction': { templateUrl: 'angularApp/pages/other-pages/detailsProduct.html' } },
                controller: 'detailsProductCtrl'
            })
            .state('anon-login', {
                url: '/login',
                views: { 'menuAction': { templateUrl: 'angularApp/pages/other-pages/login.html' } },
                controller: 'loginCtrl'
            })
            .state('anon-register', {
                url: '/register',
                views: { 'menuAction': { templateUrl: 'angularApp/pages/other-pages/register.html' } },
                controller: 'registerCtrl'
            });
    });

/*********************************** Identity User **************************************/
angular.module('IdentityApp', [])
    .config(function ($stateProvider) {
        $stateProvider
            .state('index.identity-home', {
                url: '/home',
                views: { 'menuAction': { template: '<p>Home</p>' } }
            })
            .state('index.identity-categories', {
                url: '/categories',
                views: { 'menuAction': { template: '<p>Categories</p>' } },
                controller: 'allCategoriesCtrl'
            })
            .state('index.identity-products', {
                url: '/products',
                views: { 'menuAction': { template: '<p>Products</p>' } },
                controller: 'allProductsCtrl'
            })
            .state('index.identity-products:productId', {
                url: '/products/:productId',
                views: { 'menuAction': { template: '<p>Products id</p>' } },
                controller: 'detailsProductCtrl'
            })
            .state('index.identity-account', {
                url: '/account',
                views: { 'menuAction': { template: '<p>Account</p>' } },
                controller: 'accountCtrl'
            })
            .state('index.identity-logout', {
                url: '/logout',
                views: { 'menuAction': { template: '<p>Log out</p>' } },
                controller: 'logoutCtrl'
            });
    });

/*********************************** Main app **************************************/

var mainApp = angular.module('MainApp', ['ngCookies', 'ui.router'])
    .config(function ($stateProvider, $urlRouterProvider) {
        $urlRouterProvider.otherwise("/home");
        $stateProvider
            .state('home', {
                url: '/home',
                views: {
                    'main': {
                        templateUrl: 'angularApp/pages/other-pages/unauthorizeMenu.html'
                    }
                }
            })
            .state('anon-categories', {
                url: '/categories',
                views: { 'action': { template: 'angularApp/pages/other-pages/unauthorizeMenu.html' } }
            });

        //.state('home', {
        //    url: '/home',
        //    views: { 'main': { templateUrl: 'angularApp/pages/other-pages/unauthorizeMenu.html' } }
        //})
        //.state('anon-categories', {
        //    url: '/categories',
        //    views: {
        //        'main': { templateUrl: 'angularApp/pages/other-pages/unauthorizeMenu.html' },
        //        'action': { template: 'test' }
        //    }
        //})
        //.state('user.hi', {
        //    url: '/user/hi',
        //    views: { 'hi': { template: 'hi' } }
        //});
    });
























