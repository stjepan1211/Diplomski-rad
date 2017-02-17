//Define edit result controller
angular.module('TournamentModule').controller('editresultController', ['$scope', '$http', '$stateParams', '$window', '$state','$route','PromiseUtilsService', editresultController]);

function editresultController($scope, $http, $stateParams, $window, $state, $route, PromiseUtilsService) {

    initController();

    function initController() {
    }

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
}