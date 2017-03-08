//Define gallery controller
angular.module('TournamentModule').controller('mytournamentsController', ['$scope', '$http', '$stateParams', '$window', '$state', 'AuthenticationService', mytournamentsController]);

function mytournamentsController($scope, $http, $stateParams, $window, $state, AuthenticationService) {

    initController();

    function initController() {
        $scope.tournamentNotAdded = true;
    }

    $scope.myTournamentData = {
        Name: undefined,
        StartTime: undefined,
        EndTime: undefined,
        Type: undefined,
        Location: undefined,
        Matches: undefined,
        Referees: undefined,
        Teams: undefined
    }

    $scope.getSelectedTournament = {
        Name: undefined
    }

    //get all logged user tournaments
    $scope.getMyTournaments = function () {
        var username = AuthenticationService.GetUsername();
        $http.get('api/tournament/getbyusername?username=' + username)
        .then(function (response) {
            $scope.myTournamentData = response.data;
            if(response.data != undefined)
                $scope.tournamentNotAdded = false;
        }, function (response) {
            $window.alert("Error: " + response.data.Message);
        });
    }

    $scope.getSelectedTournament = function () {
        $http.get('api/tournament/get?id=' + $stateParams.tournamentId)
        .then(function (response) {
            $scope.getSelectedTournament = response.data;
        }, function (response) {
            $window.alert("Couldn't get response.");
        });
    }

    $scope.generateSchedule = function () {
        if($window.confirm("Are you sure you wanna to application generate schedule for you. You will not be able to add matches manually but you will " +
            "be able to edit matches.")) {
            if ($stateParams.tournamentId == undefined) {
                $window.alert("Tournament id is undefined.");
            }
            else {
                $http.post('api/schedule/add?tournamentId=' + $stateParams.tournamentId)
                .then(function (response) {
                    $window.alert("Schedule added successful.");
                }, function (response) {
                    $window.alert("Couldn't add schedule. Error: " + response.data.Message);
                });
            }
        }
    }

    //change state when dropdown is changed
    $scope.changeState = function (path) {
        $state.go(path, {});
    }
}