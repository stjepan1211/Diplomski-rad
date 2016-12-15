//declare module
var TournamentModule = angular.module('TournamentModule', ['ui.router']);

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
});