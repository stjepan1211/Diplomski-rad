//Define edit match controller
angular.module('TournamentModule').controller('editmatchController', ['$scope', '$http', '$stateParams', '$window', '$state',editmatchController]);

function editmatchController($scope, $http, $stateParams, $window, $state) {

    initController();

    function initController() {
    }

    $scope.updatematchselected = {
        Id:undefined
    };

    $scope.refereesByTournamentData = {
        Id: undefined,
        Name: undefined,
        Surname: undefined
    }

    $scope.getTeamDataByTournament = {
        Id: undefined,
        Name: undefined,
        NumberOfPlayers: undefined
    }

    $scope.matches = {

    };

    $scope.MatchDetailsDataByTournament = {
        Referee: undefined,
        TeamOne: undefined,
        TeamTwo: undefined,
        DateTime: undefined,
        Round: undefined
    }

    $scope.changestate = function (path) {
        $state.go(path, {});
    }

    $scope.onTimeSet = function (newDate, oldDate) {
        console.log(newDate);
        console.log(oldDate);
    }



    //get referees for the match
    $scope.getRefereesByTournament = function () {
        var tournamentId = $stateParams.tournamentId;
        $http.get('/api/referee/getrefereesbytournament?tournamentId=' + tournamentId)
            .then(function (response) {
                $scope.refereesByTournamentData = response.data;
            }, function (response) {
                $window.alert("Couldn't get response");
            })
    }

    //get teams for the match
    $scope.getTeamDataByTournament = function () {
        $http.get('/api/team/getallwheretournamentid?tournamentId=' + $stateParams.tournamentId)
            .then(function (response) {
                $scope.getTeamDataByTournament = response.data;
                //console.log($scope.getTeamDataByTournament);
            }, function (response) {
                $window.alert("Couldn't get response");
            })
    }

    ////post match
    $scope.addMatch = function () {
        if ($scope.addrefereeselected == undefined || $scope.addteamoneselected == undefined || $scope.addteamtwoselected == undefined
            || $scope.datetime == undefined) {
            $window.alert("You need to select all properties.");
        }
        else if ($scope.addteamoneselected.Id == $scope.addteamtwoselected.Id) {
            $window.alert("Please select different teams.")
        }
        else {
            var match = {
                TournametId: $stateParams.tournamentId,
                RefereeId: $scope.addrefereeselected.Id,
                TeamOneId: $scope.addteamoneselected.Id,
                TeamTwoId: $scope.addteamtwoselected.Id,
                DateTime: $scope.datetime
            }
            $http.post('/api/match/add', match)
                .then(function (response) {
                    $window.alert("Match added successfully.");
                    //$state.go('editmytournament.team');
                }, function (response) {
                    $window.alert("Can't add match.");
                });
        }
    }

    ////get match data by tournament
    $scope.getMatchDataByTournament = function () {
        $http.get('/api/match/getmatchesbytournament?tournamentId=' + $stateParams.tournamentId)
        .then(function (response) {
            $scope.matches = response.data;
        }, function (response) {
            $window.alert("Couldn't get response");
        })
    }

    //update match
    $scope.editMatch = function () {
        if ($scope.updatematchselected == undefined || $scope.selectdatetime == undefined || $scope.updaterefereeselected == undefined
            || $scope.updateteamoneselected == undefined || $scope.updateteamtwoselected == undefined) {
            $window.alert("You need to select all properties.");
        }
        else if ($scope.updateteamtwoselected.Id == $scope.updateteamoneselected.Id) {
            $window.alert("Please select different teams.")
        }
        else {
            var match = {
                Id: $scope.updatematchselected.Id,
                TournametId: $stateParams.tournamentId,
                RefereeId: $scope.updaterefereeselected.Id,
                TeamOneId: $scope.updateteamoneselected.Id,
                TeamTwoId: $scope.updateteamtwoselected.Id,
                DateTime: $scope.selectdatetime
            }
            $http.put('/api/match/update', match)
                .then(function (response) {
                    $window.alert("Match updated successfully.");
                    $state.go('editmytournament.match');
                }, function (response) {
                    $window.alert("Can't add match.");
                });
        }
    }

    $scope.deleteMatch = function () {
        if ($scope.deletematchselected.Id == undefined) {
            $window.alert("Please select match.");
        }
        else {
            var id = $scope.deletematchselected.Id;
            if ($window.confirm('Are you sure you want to delete this match?')) {
                $http.delete('/api/match/delete?id=' + id)
                    .then(function (response) {
                        $window.alert("Match deleted successfully.");
                        $state.go('editmytournament.match');
                    }, function (response) {
                        $window.alert("Can't delete referee.");
                    })
            }
        }
    }
}