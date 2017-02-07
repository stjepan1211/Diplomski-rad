//declare module
var TournamentModule = angular.module('TournamentModule', ['ui.router', 'ngStorage', 'angular-md5', 'ngAnimate', 'angular-loading-bar',
    'ngMessages', '720kb.datepicker', 'LocalStorageModule','uiGmapgoogle-maps']);

TournamentModule.config(function ($stateProvider, $urlRouterProvider, $qProvider, localStorageServiceProvider, uiGmapGoogleMapApiProvider) {

    localStorageServiceProvider.setPrefix('TournamentModule');
    localStorageServiceProvider.setStorageType('localStorage');

    uiGmapGoogleMapApiProvider.configure({
        key: 'AIzaSyA5hrIdvl69FjzQ1XwPqIYjM2764MXm6FA',
        v: '3.20', //defaults to latest 3.X anyhow
        libraries: 'weather,geometry,visualization'
    });
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
                    templateUrl: 'app/views/home.html',
                }
            },
            controller: navbarController,
            //onEnter: function() {
            //    initController();
            //},
            //onExit: function() {
               
            //}
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
        .state('mytournaments', {
            url: '/mytournaments',
            views: {
                "root": {
                    templateUrl: 'app/views/mytournaments.html'
                }
            }
        })
        .state('editmytournament', {
            url: '/editmytournament?tournamentId&tournamentName',
            views: {
                "root": {
                    templateUrl: 'app/views/editmytournament.html'
                }
            }
        })
        .state('editmytournament.match', {
            url: '/editmatch',
            views: {
                "editcategory": {
                    templateUrl: 'app/views/editcategory/editmatch.html'
                }
            }
        })
        .state('editmytournament.team', {
            url: '/editeam',
            views: {
                "editcategory": {
                    templateUrl: 'app/views/editcategory/editteam.html'
                }
            }
        })
        .state('editmytournament.player', {
            url: '/editplayer',
            views: {
                "editcategory": {
                    templateUrl: 'app/views/editcategory/editplayer.html'
                }
            }
        })
        .state('editmytournament.referee', {
            url: '/editreferee',
            views: {
                "editcategory": {
                    templateUrl: 'app/views/editcategory/editreferee.html'
                }
            }
        })
        .state('editmytournament.result', {
            url: '/editresult',
            views: {
                "editcategory": {
                    templateUrl: 'app/views/editcategory/editresult.html'
                }
            }
        })
});

TournamentModule.run(function run($rootScope, $http, $location, $localStorage, $state, AuthenticationService) {
    // keep user logged in after page refresh
    //if ($localStorage.currentUser) {
    //    $http.defaults.headers.common.Authorization = 'Bearer ' + $localStorage.currentUser.Token.TokenString;

    //    if ($http.defaults.headers.common.Authorization) {
    //        $location.path('/home');
    //    }
    //    console.log("true");
    //}



    // redirect to login page if not logged in and trying to access a restricted page
    //$rootScope.$on('$locationChangeStart', function (event, next, current) {
    //    var publicPages = ['/login'];
    //  if (!$localStorage.currentUser) {
    //        $location.path('/login');
    //    }
    //});
});

