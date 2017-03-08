//Define edit tournament controller
angular.module('TournamentModule').controller('edittournamentController', ['$scope', '$http', '$stateParams', '$window', '$state', edittournamentController]);

function edittournamentController($scope, $http, $stateParams, $window, $state) {

    initController();

    function initController() {
    }


    $scope.edittournamentData = {
        Id: undefined,
        Name: undefined,
        NumberOfTeams: undefined,
        Type: undefined,
        StartTime: undefined,
        EndTime: undefined
    }
    
    $scope.gettournamentdata = function () {
        $http.get('api/tournament/get?id=' + $stateParams.tournamentId)
            .then(function (response) {
                $scope.tournamentData = response.data;
                //console.log($scope.tournamentData);
            }, function (response) {
                $window.alert("Error: " + response.data.Message);
            })
    }

    $scope.changestate = function (path) {
        $state.go(path, {});
    }

    $scope.editMatch = function () {
        if ($stateParams.tournamentId == undefined || $scope.edittournamentData.Name == undefined || $scope.edittournamentData.NumberOfTeams == undefined
            || $scope.edittournamentData.Type == undefined || $scope.edittournamentData.StartTime == undefined || $scope.edittournamentData.EndTime == undefined) {
            $window.alert("You need to add all properties.");
        }
        else if ($scope.tournamentData.IsScheduled == true) {
            $window.alert("You cant't update scheduled tournament. You can manually change matches.");
        }
        else {
            var tournament = {
                Id: $stateParams.tournamentId,
                Name: $scope.edittournamentData.Name,
                NumberOfTeams: $scope.edittournamentData.NumberOfTeams,
                Type: $scope.edittournamentData.Type,
                StartTime: $scope.edittournamentData.StartTime,
                EndTime: $scope.edittournamentData.EndTime
            }
            $http.put('api/tournament/update', tournament)
                .then(function (response) {
                    $window.alert("Tournament " + $scope.tournamentData.Name + " updated successful.");
                }, function (response) {
                    $window.alert("Error: " + response.data.Message);
                })
        }
    }
}