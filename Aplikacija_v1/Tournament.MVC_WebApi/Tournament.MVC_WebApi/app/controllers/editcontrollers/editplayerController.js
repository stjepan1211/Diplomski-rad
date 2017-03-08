//Define edit player controller
angular.module('TournamentModule').controller('editplayerController', ['$scope', '$http', '$stateParams', '$window', '$state', editplayerController]);

function editplayerController($scope, $http, $stateParams, $window, $state) {

    initController();

    function initController() {
    }

    $scope.getPlayerDataByTournament = {
        Id: undefined,
        Name: undefined,
        Surname: undefined,
        Goals: undefined,
        YellowCards: undefined,
        RedCards: undefined,
        GamesPlayed: undefined
    }

    $scope.getPlayerDataByTeam = {
        Id: undefined,
        Name: undefined,
        Surname: undefined,
        Goals: undefined,
        YellowCards: undefined,
        RedCards: undefined,
        GamesPlayed: undefined
    }

    $scope.getTeamDataByTournament = {
        Name: undefined,
        MatchesPlayed: undefined,
        Won: undefined,
        Lost: undefined,
        NumberOfPlayers: undefined
    }

    $scope.addPlayerData = {
        TeamId: undefined,
        Name: undefined,
        Surname: undefined
    }

    $scope.updatePlayerData = {
        Name: undefined,
        Surname: undefined,
        Goals: undefined,
        YellowCards: undefined,
        RedCards: undefined,
        GamesPlayed: undefined
    }

    $scope.deleteplayerselected = {
        Id: undefined
    }

    $scope.changestate = function (path) {
        $state.go(path, {});
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
    //get players for selected team
    $scope.getPlayers = function (teamId) {
        $http.get('/api/player/getplayersbyteam?teamId=' + teamId)
           .then(function (response) {
               $scope.getPlayerDataByTeam = response.data;
               console.log($scope.getPlayerDataByTeam);
           }, function (response) {
               console.log("Couldn't get response");
           })
    }

    //get players for selected tournament
    $scope.getPlayerDataByTournament = function () {
        $http.get('/api/player/getplayersbytournament?tournamentId=' + $stateParams.tournamentId)
            .then(function (response) {
                $scope.getPlayerDataByTournament = response.data;
                //console.log($scope.$scope.getTeamDataByTournament);
            }, function (response) {
                console.log("Couldn't get response");
            })
    }

    //add player
    $scope.addPlayer = function () {
        if ($scope.addPlayerData.Name == undefined) {
            $window.alert("Please add player's name.");
        }
        else if ($scope.addPlayerData.Surname == undefined) {
            $window.alert("Please add player's surname.");
        }
        else if ($scope.teamselected.Id == undefined) {
            $window.alert("Please select player's team.")
        }
        else {
            var player = {
                TeamId: $scope.teamselected.Id,
                Name: $scope.addPlayerData.Name,
                Surname: $scope.addPlayerData.Surname
            }
            $http.post('/api/player/add', player)
                .then(function (response) {
                    $window.alert("Player added successfully.");
                    //$state.go('editmytournament.team');
                }, function (response) {
                    $window.alert("Warning: " + response.data.Message);
                });
        }
    }
    //update player
    $scope.updatePlayer = function () {
        if ($scope.updateteamselected.Id == undefined || $scope.updatePlayerData.Name == undefined || $scope.updatePlayerData.Surname == undefined ||
            $scope.updatePlayerData.Goals == undefined || $scope.updatePlayerData.YellowCards == undefined || $scope.updatePlayerData.RedCards == undefined ||
            $scope.updatePlayerData.GamesPlayed == undefined) {
            $window.alert("You need to set all properties to update player.");
        }
        else
        {
            var player = {
                Id: $scope.updateplayerselected.Id,
                TeamId: $scope.updateteamselected.Id,
                Name: $scope.updatePlayerData.Name,
                Surname: $scope.updatePlayerData.Surname,
                Goals: $scope.updatePlayerData.Goals,
                YellowCards: $scope.updatePlayerData.YellowCards,
                RedCards: $scope.updatePlayerData.RedCards,
                GamesPlayed: $scope.updatePlayerData.GamesPlayed
            }
            console.log(player)
            $http.put('api/player/update', player)
                .then(function (response) {
                    $window.alert("Player updated successfully.");
                    $state.go('editmytournament.player');
                }, function (response) {
                    $window.alert("Can't add tournament.");
                });
        }
    }

    $scope.deletePlayer = function () {
        var id = $scope.deleteplayerselected.Id;
        console.log($scope.deleteplayerselected.Id);
        if (id == undefined) {
            $window.alert("Please select player.");
        }
        else {
            if ($window.confirm('Are you sure you want to delete this player?')) {
                $http.delete('/api/player/delete?id=' + id)
                    .then(function (response) {
                        $window.alert("Player deleted successfully.");
                        $state.go('editmytournament.team');
                    }, function (response) {
                        $window.alert("Can't delete player.");
                    })
            }
        }
    }

}