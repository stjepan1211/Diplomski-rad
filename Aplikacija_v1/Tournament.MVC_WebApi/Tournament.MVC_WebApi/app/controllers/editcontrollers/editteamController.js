//Define edit team controller
angular.module('TournamentModule').controller('editteamController', ['$scope', '$http', '$stateParams', '$window', '$state', editteamController]);

function editteamController($scope, $http, $stateParams, $window, $state) {

    initController();

    function initController(){
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
        NumberOfPlayers: undefined
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
                $window.alert("Team added successfully.");
                $state.go('editmytournament.team');
            }, function (response) {
                $window.alert("Can't add tournament.");
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
                $window.alert("Team updated successfully.");
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
                        $window.alert("Team deleted successfully.");
                        $state.go('editmytournament.team');
                    }, function (response) {
                        $window.alert("Can't delete tournament.");
                    })
            }
        }
    }
    //$scope.$watch('detailsteamselected', function (item) {
    //    var json = JSON.parse(item);
    //    $window.alert(json["Name"]); //mkyong
    //    $window.alert(json.Name); //mkyong

    //    detailsteamselected.Name = json.Name;
    //    console.log(detailsteamselected.Name);
    //})

}