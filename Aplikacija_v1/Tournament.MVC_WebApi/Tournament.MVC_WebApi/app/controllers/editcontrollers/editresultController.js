//Define edit result controller
angular.module('TournamentModule').controller('editresultController', ['$scope', '$http', '$stateParams', '$window', '$state','$route','PromiseUtilsService', editresultController]);

function editresultController($scope, $http, $stateParams, $window, $state, $route, PromiseUtilsService) {

    initController();

    function initController() {
    }

    var team1Scorers = [];
    var team2Scorers = [];
    var playersWithYellowCards = [];
    var playersWithRedCards = [];

    $scope.showTeamOneSelectScorer = false;
    $scope.showTeamTwoSelectScorer = false;
    $scope.showScorerOneAddGoals = false;
    $scope.showScorerTwoAddGoals = false;

    $scope.newObjectYellowCard = {};
    $scope.newObjectRedCard = {};

    $scope.checkBox = {
        Penalties: undefined,
        RedCards: undefined,
        YellowCards: undefined
    }

    $scope.areDependenciesAdded = {
        MatchesTeamsAdded: undefined,
        PlayersTeam1Added: undefined,
        PlayersTeam2Added: undefined
    };

    $scope.addResultData = {
        MatchId: undefined,
        TeamOneGoals: undefined,
        TeamTwoGoals: undefined
    }

    $scope.updateResultData = {
        Id: undefined,
        MatchId: undefined,
        TeamOneGoals: undefined,
        TeamTwoGoals: undefined
    }

    $scope.detailsResultData = {
        Id: undefined,
        MatchId: undefined,
        TeamOneGoals: undefined,
        TeamTwoGoals: undefined
    }

    $scope.changestate = function (path) {
        $state.go(path, {});
    }

    //check if players, matches and teams are added
    $scope.checkDependencies = function () {
        $http.get('api/result/matchesteamsadded?tournamentId=' + $stateParams.tournamentId)
            .then(function(response){
                $scope.areDependenciesAdded.MatchesTeamsAdded = true;
                //$window.alert("Teams or matches aren't added for this tournament.");
            }, function(response) {
                $window.alert("Warning: " + response.data.Message);
            })

    }

    $scope.checkTeamDependencies = function (team1Id, team2Id) {
        $http.get('api/result/playersadded?teamId=' + team1Id)
            .then(function (response) {
                $scope.areDependenciesAdded.PlayersTeam1Added = true;
            }, function (response) {
                $window.alert("Warning: " + response.data.Message);
            })
        $http.get('api/result/playersadded?teamId=' + team2Id)
            .then(function (response) {
                $scope.areDependenciesAdded.PlayersTeam2Added = true;
            }, function (response) {
                $window.alert("Warning: " + response.data.Message + ". After you add players you will can add result.");
            })
    }


    //get match data by tournament
    $scope.getMatchDataByTournament = function () {
        $http.get('/api/match/getmatchesbytournament?tournamentId=' + $stateParams.tournamentId)
        .then(function (response) {
            $scope.matches = response.data;
        }, function (response) {
            $window.alert("Couldn't get response");
        })
    }

    //add match
    $scope.addResult = function () {
        if ($scope.addResultData.TeamOneGoals == undefined || $scope.addResultData.TeamTwoGoals == undefined) {
            $window.alert("Please add team goals.");
        }
        else if ($scope.addresultmatchselected.Id == undefined) {
            $window.alert("Please select match.");
        }
        else {
            var result = {
                MatchId: $scope.addresultmatchselected.Id,
                TeamOneGoals: $scope.addResultData.TeamOneGoals,
                TeamTwoGoals: $scope.addResultData.TeamTwoGoals
            }
            $http.post('/api/result/add', result)
                .then(function (response) {
                    $window.alert("Result added successfully.");
                    //$state.go('editmytournament.team');
                }, function (response) {
                    $window.alert("Can't add result.");
                });
        }
    }

    $scope.getresultdetails = function (matchId) {

        PromiseUtilsService.getPromiseHttpResult($http.get('/api/result/getresultbymatch?matchId=' + matchId))
            .then(function (result) {
                //console.log("result", result);
                $scope.detailsResultData.TeamOneGoals = result.data[0].TeamOneGoals;
                $scope.detailsResultData.TeamTwoGoals = result.data[0].TeamTwoGoals;
                $scope.detailsResultData.Id = result.data[0].Id;
                $scope.detailsResultData.MatchId = result.data[0].MatchId;
            }, function (arguments) {
                console.log("fail", arguments);
            })
    }

    $scope.updateResult = function () {
        if ($scope.updateResultData.TeamOneGoals == undefined || $scope.updateResultData.TeamTwoGoals == undefined) {
            $window.alert("Please add team goals")
        }
        else if ($scope.detailsResultData.MatchId != $scope.updateresultmatchselected.Id) {
            $window.alert("Imamo problem");
        }
        else {
            var result = {
                Id: $scope.detailsResultData.Id,
                MatchId: $scope.detailsResultData.MatchId,
                TeamOneGoals: $scope.updateResultData.TeamOneGoals,
                TeamTwoGoals: $scope.updateResultData.TeamTwoGoals
            }
            $http.put('/api/result/update', result)
                .then(function (response) {
                    $window.alert("Result updated successfully.");
                    $state.go('editmytournament.result');
                }, function (response) {
                    $window.alert("Can't update result.");
                });
        }
    }

    $scope.deleteResult = function () {
        if ($scope.detailsResultData.Id == undefined) {
            $window.alert("Please select match to delete result");
        }
        else {
            var id = $scope.detailsResultData.Id;
            if ($window.confirm('Are you sure you want to delete this result?')) {
                $http.delete('/api/result/delete?id=' + id)
                    .then(function (response) {
                        $window.alert("Result deleted successfully.");

                        $state.go('editmytournament.result');
                    }, function (response) {
                        $window.alert("Can't delete result.");
                    })
            }
        }
    }

    //set watchers on team1 goals, when user input number, if number is greater than 0 
    //get players will be called
    $scope.$watch('addResultData.TeamOneGoals', function (value) {
        try {
            if (value != undefined && value != 0 && $scope.addresultmatchselected.Team.Id != undefined) {
                $http.get('api/player/getplayersbyteam?teamId=' + $scope.addresultmatchselected.Team.Id)
                    .then(function (response) {
                        $scope.Team1Players = response.data;
                        $scope.showTeamOneSelectScorer = true;
                    }, function (response) {
                        $window.alert("Warning: " + response.data.Message);
                    })
            }
        } catch (e) {
            $window.alert("Error: " + e);
        }
    });

    //set watchers on team1 goals, when user input number, if number is greater than 0 
    //get players will be called
    $scope.$watch('addResultData.TeamTwoGoals', function (value) {
        try {
            if (value != undefined && value != 0 && $scope.addresultmatchselected.Team1.Id != undefined) {
                $http.get('api/player/getplayersbyteam?teamId=' + $scope.addresultmatchselected.Team1.Id)
                    .then(function (response) {
                        $scope.Team2Players = response.data;
                        $scope.showTeamTwoSelectScorer = true;
                    }, function (response) {
                        $window.alert("Warning: " + response.data.Message);
                    })
            }
        } catch (e) {
            $window.alert("Error: " + e);
        }
    });

    //push team1 scorers
    $scope.addTeam1Scorer = function () {
        team1Scorers.push({
            Player: {
                Id: $scope.scorerteam1selected.Id,
                //Name: $scope.scorerteam1selected.Name,
                //Surname: $scope.scorerteam1selected.Surname,
                Goals: $scope.team1scorergoals
            }
        });
        $window.alert("Players goals added.");

    }
    //push team2 scorers
    $scope.addTeam2Scorer = function () {
        team2Scorers.push({
            Player: {
                Id: $scope.scorerteam2selected.Id,
                //Name: $scope.scorerteam2selected.Name,
                //Surname: $scope.scorerteam2selected.Surname,
                Goals: $scope.team2scorergoals
            }
        });
        $window.alert("Players goals added.");
    }

    $scope.resetTeam1Scorer = function () {
        team1Scorers.length = 0;
    }

    $scope.resetTeam2Scorer = function () {
        team2Scorers.length = 0;
    }

    $scope.getplayersyellowcards = function () {
        if ($scope.selectedTeamYellowCards != undefined) {
            $http.get('api/player/getplayersbyteam?teamId=' + $scope.selectedTeamYellowCards)
            .then(function (response) {
                $scope.getplayersforyellowcards = response.data;
            }, function (response) {
                $window.alert("Warning: " + response.data.Message);
            })
        }
    }
    $scope.getplayersredcards = function () {
        if ($scope.selectedTeamRedCards != undefined) {
            $http.get('api/player/getplayersbyteam?teamId=' + $scope.selectedTeamRedCards)
            .then(function (response) {
                $scope.getplayersforredcards = response.data;
            }, function (response) {
                $window.alert("Warning: " + response.data.Message);
            })
        }
    }

    //push team1 scorers
    $scope.addYellowCardPlayers = function () {
        playersWithYellowCards.length = 0;
        angular.forEach($scope.newObjectYellowCard, function (value, key) {
            Player = {
                Id: key,
                Value: value
            }
            playersWithYellowCards.push(Player);
        })
        $window.alert("Yellow cards for teams added.");
    }

    //push team2 scorers
    $scope.addRedCardPlayers = function () {
        playersWithRedCards.length = 0;
        angular.forEach($scope.newObjectRedCard, function (value, key) {
            Player = {
                Id: key,
                Value: value
            }
            playersWithRedCards.push(Player);
        })
        $window.alert("Red cards for teams added.");
    }

    //post all data, result, winner, (winner on penalties), yellow cards, red cards, goals
    $scope.postAll = function () {
        var result = {
            MatchId: $scope.addresultmatchselected.Id,
            TeamOneGoals: $scope.addResultData.TeamOneGoals,
            TeamTwoGoals: $scope.addResultData.TeamTwoGoals
        };
        var teamOneScorers = team1Scorers;
        var teamTwoScorers = team2Scorers;
        var playersYellowCards = playersWithYellowCards;
        var playersRedCards = playersWithRedCards;
        var penalties = {
            Were: $scope.checkBox.Penalties,
            Winner: $scope.winnerOnPenalties
        }
        //post everything as one data object
        $http({
            method: 'POST',
            url: '/api/result/add',
            data: {
                tournamentId: $stateParams.tournamentId,
                result: result,
                teamOneScorers: teamOneScorers,
                teamTwoScorers: teamTwoScorers,
                playersYellowCards: playersYellowCards,
                playersRedCards: playersRedCards,
                penalties: penalties
            }
        }).then(function (response) {
            $window.alert("Data added successful.");
        }, function (response) {
            $window.alert("Error: " + response.data.Message);
        });
    }

    $scope.changeState = function () {
        $state.reload();
    }

}