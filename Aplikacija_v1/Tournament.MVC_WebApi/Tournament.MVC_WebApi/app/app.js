//declare module
var TournamentModule = angular.module('TournamentModule', ['ui.router', 'ngStorage', 'angular-md5', 'ngAnimate', 
    'ngMessages', '720kb.datepicker', 'LocalStorageModule', 'uiGmapgoogle-maps', 'ADM-dateTimePicker', 'ngRoute','angular-loading-bar',
    'angular-toArrayFilter','angularUtils.directives.dirPagination']);

TournamentModule.config(function ($stateProvider, $urlRouterProvider, $qProvider, localStorageServiceProvider, uiGmapGoogleMapApiProvider) {

    localStorageServiceProvider.setPrefix('TournamentModule');
    localStorageServiceProvider.setStorageType('localStorage');

    uiGmapGoogleMapApiProvider.configure({
        key: 'AIzaSyA5hrIdvl69FjzQ1XwPqIYjM2764MXm6FA',
        v: '3.26', //defaults to latest 3.X anyhow
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
        .state('detailstournament', {
            url: '/detailstournament?tournamentId&reloaded',
            views: {
                "root": {
                    templateUrl: 'app/views/detailstournament.html'
                }
            },
            controller: 'tournamentsController'
        })
        .state('players', {
            url: '/teamplayers?teamId',
            views: {
                "root": {
                    templateUrl: 'app/views/detailsplayers.html'
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
            url: '/editmytournament?tournamentId',
            views: {
                "root": {
                    templateUrl: 'app/views/editmytournament.html'
                }
            }
        })
        .state('editmytournament.tournament', {
            url: '/edittournament',
            views: {
                "editcategory": {
                    templateUrl: 'app/views/editcategory/editmytournament.html'
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
        .state('editmytournament.tournament.update', {
            url: '/edittournament',
            views: {
                "edittournament": {
                    templateUrl: 'app/views/editcategory/edittournament/updatetournament.html'
                }
            }
        })
        .state('editmytournament.match.details', {
            url: '/editmatchdetails',
            views: {
                "editmatch": {
                    templateUrl: 'app/views/editcategory/editmatch/detailsmatch.html'
                }
            }
        })
        .state('editmytournament.match.add', {
            url: '/editmatchadd',
            views: {
                "editmatch": {
                    templateUrl: 'app/views/editcategory/editmatch/addmatch.html'
                }
            }
        })
        .state('editmytournament.match.update', {
            url: '/editmatchupdate',
            views: {
                "editmatch": {
                    templateUrl: 'app/views/editcategory/editmatch/updatematch.html'
                }
            }
        })
        .state('editmytournament.match.delete', {
            url: '/editmatchdelete',
            views: {
                "editmatch": {
                    templateUrl: 'app/views/editcategory/editmatch/deletematch.html'
                }
            }
        })
        .state('editmytournament.team.delete', {
            url: '/editteamdelete',
            views: {
                "editteam": {
                    templateUrl: 'app/views/editcategory/editteam/deleteteam.html'
                }
            }
        })
        .state('editmytournament.team.add', {
            url: '/editteamadd',
            views: {
                "editteam": {
                    templateUrl: 'app/views/editcategory/editteam/addteam.html'
                }
            }
        })
        .state('editmytournament.team.details', {
            url: '/editteamdetails',
            views: {
                "editteam": {
                    templateUrl: 'app/views/editcategory/editteam/detailsteam.html'
                }
            }
        })
        .state('editmytournament.team.update', {
            url: '/editteamupdate',
            views: {
                "editteam": {
                    templateUrl: 'app/views/editcategory/editteam/updateteam.html'
                }
            }
        })
        .state('editmytournament.player.update', {
            url: '/editplayerupdate',
            views: {
                "editplayer": {
                    templateUrl: 'app/views/editcategory/editplayer/updateplayer.html'
                }
            }
        })
        .state('editmytournament.player.add', {
            url: '/editplayeradd',
            views: {
                "editplayer": {
                    templateUrl: 'app/views/editcategory/editplayer/addplayer.html'
                }
            }
        })
        .state('editmytournament.player.details', {
            url: '/editplayerdetails',
            views: {
                "editplayer": {
                    templateUrl: 'app/views/editcategory/editplayer/detailsplayer.html'
                }
            }
        })
        .state('editmytournament.player.delete', {
            url: '/editplayerdelete',
            views: {
                "editplayer": {
                    templateUrl: 'app/views/editcategory/editplayer/deleteplayer.html'
                }
            }
        })
        .state('editmytournament.referee.delete', {
            url: '/editrefereedelete',
            views: {
                "editreferee": {
                    templateUrl: 'app/views/editcategory/editreferee/deletereferee.html'
                }
            }
        })
        .state('editmytournament.referee.add', {
            url: '/editrefereeadd',
            views: {
                "editreferee": {
                    templateUrl: 'app/views/editcategory/editreferee/addreferee.html'
                }
            }
        })
        .state('editmytournament.referee.update', {
            url: '/editrefereeupdate',
            views: {
                "editreferee": {
                    templateUrl: 'app/views/editcategory/editreferee/updatereferee.html'
                }
            }
        })
        .state('editmytournament.referee.details', {
            url: '/editrefereedetails',
            views: {
                "editreferee": {
                    templateUrl: 'app/views/editcategory/editreferee/detailsreferee.html'
                }
            }
        })
        .state('editmytournament.result.details', {
            url: '/editresultdetails',
            views: {
                "editresult": {
                    templateUrl: 'app/views/editcategory/editresult/detailsresult.html'
                }
            }
        })
        .state('editmytournament.result.add', {
            url: '/editresultadd',
            views: {
                "editresult": {
                    templateUrl: 'app/views/editcategory/editresult/addresult.html'
                }
            }
        })
        .state('editmytournament.result.delete', {
            url: '/editresultdelete',
            views: {
                "editresult": {
                    templateUrl: 'app/views/editcategory/editresult/deleteresult.html'
                }
            }
        })
        .state('editmytournament.result.update', {
            url: '/editresultupdate',
            views: {
                "editresult": {
                    templateUrl: 'app/views/editcategory/editresult/updateresult.html'
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

