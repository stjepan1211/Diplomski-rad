//declare module
var TournamentModule = angular.module('TournamentModule', ['ui.router', 'ngStorage', 'angular-md5', 'angular-loading-bar']);

TournamentModule.config(function ($stateProvider, $urlRouterProvider, $qProvider) {

    //
    $qProvider.errorOnUnhandledRejections(false);

    //default route
    $urlRouterProvider.otherwise('/home');

    //define states
    $stateProvider
        .state('home', {
            url: '/home',
            views: {
                "root": {
                    templateUrl: 'app/views/home.html'
                }
            }
        })
        .state('tournaments', {
            url: '/tournaments',
            views: {
                "root": {
                    templateUrl: 'app/views/tournaments.html'
                }
            }
        })
        .state('gallery', {
            url: '/gallery',
            views: {
                "root": {
                    templateUrl: 'app/views/gallery.html'
                }
            }
        })
        .state('topteams', {
            url: '/topteams',
            views: {
                "root": {
                    templateUrl: 'app/views/topteams.html'
                }
            }
        })
        .state('topplayers', {
            url: '/topplayers',
            views: {
                "root": {
                    templateUrl: 'app/views/topplayers.html'
                }
            }
        })
        .state('news', {
            url: '/news',
            views: {
                "root": {
                    templateUrl: 'app/views/news.html'
                }
            }
        })
        .state('signup', {
            url: '/signup',
            views: {
                "root": {
                    templateUrl: 'app/views/signup.html'
                }
            }
        })
        .state('login', {
            url: '/login',
            views: {
                "root": {
                    templateUrl: 'app/views/login.html'
                }
            }
        })
        .state('addtournament', {
            url: '/addtournament',
            views: {
                "root": {
                    templateUrl: 'app/views/addtournament.html'
                }
            }
        })
});

TournamentModule.run(function run($rootScope, $http, $location, $localStorage, $state, AuthenticationService) {
    // keep user logged in after page refresh
    if ($localStorage.currentUser) {
        $http.defaults.headers.common.Authorization = 'Bearer ' + $localStorage.currentUser.token;


        if ($http.defaults.headers.common.Authorization) {
            $location.path('/home');
        }
    }

    // redirect to login page if not logged in and trying to access a restricted page
    $rootScope.$on('$locationChangeStart', function (event, next, current) {
        var publicPages = ['/login'];
        var restrictedPage = publicPages.indexOf($location.path()) === -1;
        if (restrictedPage && !$localStorage.currentUser) {
            $location.path('/login');
        }
    });
});