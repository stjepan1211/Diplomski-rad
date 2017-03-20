//Define edit team controller
angular.module('TournamentModule').controller('editteamController', ['$scope', '$http', '$stateParams', '$window', '$state','PromiseUtilsService', editteamController]);

function editteamController($scope, $http, $stateParams, $window, $state, PromiseUtilsService) {

    initController();

    function initController(){
    }

    $scope.tournament = {
        Type: undefined
    }

    $scope.addTeamData = {
        Name: undefined,
        NumberOfPlayers: undefined
    }

    $scope.updateTeamData = {
        Name: undefined,
        NumberOfPlayers: undefined
    }

    $scope.getTeamDataByTournament = {
        Name: undefined,
        MatchesPlayed: undefined,
        Won: undefined,
        Lost: undefined,
        Draw: undefined,
        NumberOfPlayers: undefined,
        NumberOfMatches: undefined,
        Points: undefined
    }

    $scope.detailsteamselected = {
        Name: undefined,
        MatchesPlayed: undefined,
        Won: undefined,
        Lost: undefined,
        NumberOfPlayers: undefined
    }

    $scope.changestate = function (path) {
        $state.go(path, {});
    }

    //post method - add team
    $scope.addTeam = function () {
        if ($scope.addTeamData.Name == undefined) {
            $window.alert("Please add team name.");
        }
        else if ($scope.addTeamData.NumberOfPlayers == undefined) {
            $window.alert("Please add number of players.")
        }
        var team = {
            TournamentId: $stateParams.tournamentId,
            Name: $scope.addTeamData.Name,
            NumberOfPlayers: $scope.addTeamData.NumberOfPlayers
        }
        $http.post('/api/team/add', team)
            .then(function (response) {
                $window.alert("Team added successfull.");
                //$state.go('editmytournament.team');
            }, function (response) {
                $window.alert("Warning: " + response.data.Message);
            });
    }
    //get teams for selected tournaments
    $scope.getTeamDataByTournament = function () {
        $http.get('/api/team/getallwheretournamentid?tournamentId=' + $stateParams.tournamentId)
            .then(function (response) {
                $scope.getTeamDataByTournament = response.data;
                //console.log($scope.getTeamDataByTournament);
            }, function (response) {
                console.log("Couldn't get response");
            })
    }
    

    //get league type of tournament for adding league cup teams
    $scope.getLeagueTournaments = function () {
        $http.get('api/tournament/getleaguetournaments')
            .then(function (response) {
                $scope.leagueTournaments = response.data;
                //console.log($scope.leagueTournaments);
            }, function (response) {
                console.log("Couldn't get response");
            })
    }

    $scope.getTeamsToAdd = function(tournamentId, numberOfTeams){
        $http.get('api/team/getleaguewinners?tournamentId=' + tournamentId + '&numberOfTeams=' + numberOfTeams)
           .then(function (response) {
               $scope.leagueWinner = response.data;
               //console.log($scope.leagueWinner);
           }, function (response) {
               $window.alert(response.data.Message);
           })
    }

    $scope.updateTeam = function () {

        if ($scope.updateTeamData.Name == undefined && $scope.updateTeamData.NumberOfPlayers == undefined) {
            $window.alert("You need to edit minimum one property.");
        }
        else if ($scope.selectedTeamUpdateItem.Id == undefined) {
            $window.alert("Please select tournament.");
        }
        else {
            var team = {
                Id: $scope.selectedTeamUpdateItem.Id,
                Name: $scope.updateTeamData.Name,
                NumberOfPlayers: $scope.updateTeamData.NumberOfPlayers
            }

            $http.put('api/team/update', team)
            .then(function (response) {
                $window.alert("Team updated successfull.");
                $state.go('editmytournament.team');
            }, function (response) {
                $window.alert("Can't add tournament.");
            });
        }

    }

    $scope.deleteTeam = function () {
        var id = $scope.selectedTeamDeleteItem.Id;
        console.log($scope.selectedTeamDeleteItem.Id);
        if (id == undefined) {
            $window.alert("Please select team.");
        }
        else {
            if ($window.confirm('Are you sure you want to delete this team?')) {
                $http.delete('/api/team/delete?id=' + id)
                    .then(function (response) {
                        $window.alert("Team deleted successfull.");
                        $state.go('editmytournament.team');
                    }, function (response) {
                        $window.alert("Can't delete tournament.");
                    })
            }
        }
    }

    //get tournament
    $scope.getTournament = function () {
        $http.get('/api/tournament/get?id=' + $stateParams.tournamentId)
            .then(function (response) {
                $scope.tournament = response.data;
                //console.log($scope.tournament);
            }, function (response) {
                console.log("Couldn't get response");
            })
    }

    $scope.addLeagueCupTeams = function () {
            if ($scope.numberofteams.checked == 2) {
                var team1 = {
                    Id: $scope.leagueWinner[0].Id,
                    TournamentId: $stateParams.tournamentId,
                    Name: $scope.leagueWinner[0].Name,
                    NumberOfPlayers: $scope.leagueWinner[0].NumberOfPlayers,
                    MatchesPlayed: $scope.leagueWinner[0].MatchesPlayed,
                    NumberOfMatches: $scope.leagueWinner[0].NumberOfMatches,
                    Won: $scope.leagueWinner[0].Won,
                    Lost: $scope.leagueWinner[0].Lost,
                    Draw: $scope.leagueWinner[0].Lost,
                    Points: $scope.leagueWinner[0].Points,
                    //Players: $scope.leagueWinner[0].Players
                }

                PromiseUtilsService.getPromiseHttpResult($http.post('/api/team/add', team1))
                .then(function (response) {
                    $window.alert("Team1 added successfull.");
                    var team2 = {
                        Id: $scope.leagueWinner[1].Id,
                        TournamentId: $stateParams.tournamentId,
                        Name: $scope.leagueWinner[1].Name,
                        NumberOfPlayers: $scope.leagueWinner[1].NumberOfPlayers,
                        MatchesPlayed: $scope.leagueWinner[1].MatchesPlayed,
                        NumberOfMatches: $scope.leagueWinner[1].NumberOfMatches,
                        Won: $scope.leagueWinner[1].Won,
                        Lost: $scope.leagueWinner[1].Lost,
                        Draw: $scope.leagueWinner[1].Lost,
                        Points: $scope.leagueWinner[1].Points,
                        Players: $scope.leagueWinner[1].Players
                    }

                    $http.post('/api/team/add', team2)
                    .then(function (response) {
                        $window.alert("Team added successfull.");
                    }, function (response) {
                        $window.alert("Warning: " + response.data.Message);
                    })
                    //$state.go('editmytournament.team');
                }, function (response) {
                    $window.alert("Warning: " + response.data.Message);
                });
            }
            else {
                var team = {
                    Id: $scope.leagueWinner.Id,
                    TournamentId: $stateParams.tournamentId,
                    Name: $scope.leagueWinner.Name,
                    NumberOfPlayers: $scope.leagueWinner.NumberOfPlayers,
                    MatchesPlayed: $scope.leagueWinner.MatchesPlayed,
                    NumberOfMatches: $scope.leagueWinner.NumberOfMatches,
                    Won: $scope.leagueWinner.Won,
                    Lost: $scope.leagueWinner.Lost,
                    Draw: $scope.leagueWinner.Lost,
                    Points: $scope.leagueWinner.Points,
                    //Players: $scope.leagueWinner.Players
                }

                PromiseUtilsService.getPromiseHttpResult($http.post('/api/team/add', team))
                .then(function (response) {
                    $window.alert("Team added successfull.");
                    //$state.go('editmytournament.team');
                }, function (response) {
                    $window.alert("Warning: " + response.data.Message);
                });
            }
    }
}