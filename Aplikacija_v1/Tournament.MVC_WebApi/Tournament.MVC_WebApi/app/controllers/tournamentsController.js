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
    
    $scope.tournament = {
        Type: undefined,
        Rounds: undefined
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

    $scope.getTournament = function () {
        PromiseUtilsService.getPromiseHttpResult($http.get('api/tournament/get?id=' + $stateParams.tournamentId))
        .then(function (response) {
            $scope.tournament = response.data;

            //console.log($scope.tournament.Rounds);
            switch ($scope.tournament.Rounds) {
                case 2:
                    $http.get('api/match/getmatchesbyround?tournamentId=' + $stateParams.tournamentId + '&round=' + 1)
                        .then(function (response) {
                            $scope.roundOneData = response.data;
                        }, function (response) {
                            $window.alert("Error: " + response.data.Message);
                        });
                    $http.get('api/match/getmatchesbyround?tournamentId=' + $stateParams.tournamentId + '&round=' + 2)
                    .then(function (response) {
                        $scope.roundTwoData = response.data;
                        //console.log($scope.roundTwoData);
                    }, function (response) {
                        $window.alert("Error: " + response.data.Message);
                    });
                    break;
                case 3:
                    $http.get('api/match/getmatchesbyround?tournamentId=' + $stateParams.tournamentId + '&round=' + 1)
                        .then(function (response) {
                            $scope.roundOneData = response.data;
                            //console.log($scope.roundOneData);
                        }, function (response) {
                            $window.alert("Error: " + response.data.Message);
                        });
                    $http.get('api/match/getmatchesbyround?tournamentId=' + $stateParams.tournamentId + '&round=' + 2)
                        .then(function (response) {
                            $scope.roundTwoData = response.data;
                            //console.log($scope.roundTwoData);
                        }, function (response) {
                            $window.alert("Error: " + response.data.Message);
                        });
                    $http.get('api/match/getmatchesbyround?tournamentId=' + $stateParams.tournamentId + '&round=' + 3)
                        .then(function (response) {
                            $scope.roundThreeData = response.data;
                            //console.log($scope.roundThreeData);
                        }, function (response) {
                            $window.alert("Error: " + response.data.Message);
                        });
                    break;
                case 4:
                    $http.get('api/match/getmatchesbyround?tournamentId=' + $stateParams.tournamentId + '&round=' + 1)
                        .then(function (response) {
                            $scope.roundOneData = response.data;
                            //console.log($scope.roundOneData);
                        }, function (response) {
                            $window.alert("Error: " + response.data.Message);
                        });
                    $http.get('api/match/getmatchesbyround?tournamentId=' + $stateParams.tournamentId + '&round=' + 2)
                        .then(function (response) {
                            $scope.roundTwoData = response.data;
                            //console.log($scope.roundTwoData);
                        }, function (response) {
                            $window.alert("Error: " + response.data.Message);
                        });
                    $http.get('api/match/getmatchesbyround?tournamentId=' + $stateParams.tournamentId + '&round=' + 3)
                        .then(function (response) {
                            $scope.roundThreeData = response.data;
                            //console.log($scope.roundThreeData);
                        }, function (response) {
                            $window.alert("Error: " + response.data.Message);
                        });
                    $http.get('api/match/getmatchesbyround?tournamentId=' + $stateParams.tournamentId + '&round=' + 4)
                        .then(function (response) {
                            $scope.roundFourData = response.data;
                            //console.log($scope.roundFourData);
                        }, function (response) {
                            $window.alert("Error: " + response.data.Message);
                        });
                    break;
                case 5:
                    $http.get('api/match/getmatchesbyround?tournamentId=' + $stateParams.tournamentId + '&round=' + 1)
                        .then(function (response) {
                            $scope.roundOneData = response.data;
                            //console.log($scope.roundOneData);
                        }, function (response) {
                            $window.alert("Error: " + response.data.Message);
                        });
                    $http.get('api/match/getmatchesbyround?tournamentId=' + $stateParams.tournamentId + '&round=' + 2)
                        .then(function (response) {
                            $scope.roundTwoData = response.data;
                            //console.log($scope.roundTwoData);
                        }, function (response) {
                            $window.alert("Error: " + response.data.Message);
                        });
                    $http.get('api/match/getmatchesbyround?tournamentId=' + $stateParams.tournamentId + '&round=' + 3)
                        .then(function (response) {
                            $scope.roundThreeData = response.data;
                            //console.log($scope.roundThreeData);
                        }, function (response) {
                            $window.alert("Error: " + response.data.Message);
                        });
                    $http.get('api/match/getmatchesbyround?tournamentId=' + $stateParams.tournamentId + '&round=' + 4)
                        .then(function (response) {
                            $scope.roundFourData = response.data;
                            //console.log($scope.roundFourData);
                        }, function (response) {
                            $window.alert("Error: " + response.data.Message);
                        });
                    $http.get('api/match/getmatchesbyround?tournamentId=' + $stateParams.tournamentId + '&round=' + 5)
                        .then(function (response) {
                            $scope.roundFiveData = response.data;
                            //console.log($scope.roundFiveData);
                        }, function (response) {
                            $window.alert("Error: " + response.data.Message);
                        });
                    break;
            }

        }, function (response) {
            return "Couldn't get response.";
        });
    }

    $scope.addTournament = function () {
        if (!AuthenticationService.Check()) {
            $window.alert("To add tournament you need to log in first.");
            //location.reload(true);
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

    $scope.getPenaltyWinner = function (winnerId) {
        if (winnerId == undefined) {

        }
        else {
            $http.get('api/team/get?id=' + winnerId)
            .then(function (response) {
                $scope.WinnerName = response.data.Name;
            }, function (response) {
                console.log(response.data.Message);
            })
        }
    }

    $scope.isPenalties = function (teamOneGoals, teamTwoGoals) {
        if (teamOneGoals == undefined || teamTwoGoals == undefined || teamOneGoals != teamTwoGoals) {
            return false;
        }
        else if ((teamOneGoals == teamTwoGoals)) {
            return true;
        }
    }
}