//Define tournaments controller
angular.module('TournamentModule').controller('tournamentsController', ['$scope', '$http', '$stateParams', '$window', '$state', '$route', '$document',
    'AuthenticationService', 'PromiseUtilsService', tournamentsController]);

function tournamentsController($scope, $http, $stateParams, $window, $state, $route, $document, AuthenticationService, PromiseUtilsService) {

    //initController();

    $scope.tournamentData = {
        StartTime: undefined,
        EndTime: undefined,
        Type: undefined,
        Locations: undefined,
        Matches: undefined,
        Referees: undefined,
        Teams: undefined
    }
    
    $scope.teamData = {
        Id: undefined,
        TournamentId: undefined,
        Name: undefined,
        MatchesPlayed: undefined, 
        Won: undefined,
        Lost: undefined,
        Draw: undefined,
        NumberOfPlayers: undefined,
        NumberOfMatches: undefined,
        Points: undefined
    }

    $scope.refereeData = {
        Name: undefined,
        Surname: undefined
    }

    $scope.playerData = {
        Name: undefined,
        Surname: undefined,
        Goals: undefined,
        RedCards: undefined,
        YellowCards: undefined,
        GamesPlayed: undefined
    }

    $scope.locationData = {
        Place: undefined,
        Longitude: undefined,
        Latitude: undefined
    }

    initController();

    function initController() {
        // reset login status
        AuthenticationService.CheckIsStoraged();
    };

    //get all tournaments
    $scope.getAllTournaments = function () {
        $http.get('api/tournament/getall')
        .then(function (response) {
            $scope.tournamentData = response.data;
        }, function (response) {
            return "Couldn't get response.";
        });
    }

    $scope.addTournament = function () {
        if (!AuthenticationService.Check()) {
            $window.alert("To add tournament you need to log in first.");
            location.reload(true);
        }
        else {
            $state.go('addtournament');
        }
    }

    $scope.getTeamsByTournament = function () {
        var id = $stateParams.tournamentId;
        $http.get('api/team/getallwheretournamentid?tournamentId=' + id)
            .then(function (response) {
                $scope.teamData = response.data;
            }, function (response) {
                $window.alert("Error: " + response.data.Message);
            });
    }

    $scope.getRefereesByTournament = function () {
        var id = $stateParams.tournamentId;
        $http.get('api/referee/getrefereesbytournament?tournamentId=' + id)
            .then(function (response) {
                $scope.refereeData = response.data;
            }, function (response) {
                $window.alert("Error: " + response.data.Message);
            });
    }

    $scope.getPlayersByTeam = function () {
        var id = $stateParams.teamId;
        $http.get('api/player/getplayersbyteam?teamId=' + id)
            .then(function (response) {
                $scope.playerData = response.data;
            }, function (response) {
                $window.alert("Error: " + response.data.Message);
            });
    }

    $scope.getMatchesData = function () {
        var id = $stateParams.tournamentId;
        $http.get('api/match/getmatchesbytournament?tournamentId=' + id)
            .then(function (response) {
                $scope.matchData = response.data;
            }, function (response) {
                $window.alert("Error: " + response.data.Message);
            });
    }

    $scope.getLocationData = function () {
        var id = $stateParams.tournamentId;
        PromiseUtilsService.getPromiseHttpResult($http.get('/api/location/getbytournament?tournamentId=' + id))
            .then(function (result) {
                $scope.locationData = result.data;
                //initial and handle objects and events on google map
                angular.extend($scope, {
                    map: {
                        center: {
                            latitude: $scope.locationData[0].Latitude,
                            longitude: $scope.locationData[0].Longitude
                        },
                        zoom: 11,
                        marker: {
                            id: Date.now(),
                            coords: {
                                latitude: $scope.locationData[0].Latitude,
                                longitude: $scope.locationData[0].Longitude
                            }
                        },
                        markersEvents: {
                            click: function (marker, eventName, model) {
                                $scope.map.window.model = model;
                                $scope.map.window.show = true;
                                $scope.address = $scope.locationData[0].Place;
                            }
                        },
                        window: {
                            marker: {},
                            show: false,
                            closeClick: function () {
                                this.show = false;
                            },
                            options: {}
                        }
                    }
                });
            }, function (arguments) {
                console.log("fail", arguments);
            })
    }

    $scope.sort = function (keyname) {
        $scope.sortKey = keyname;   //set the sortKey to the param passed
        $scope.reverse = !$scope.reverse; //if true make it false and vice versa
    }
}