//Define gallery controller
angular.module('TournamentModule').controller('mytournamentsController', ['$scope', '$http', '$stateParams', '$window', '$state', 'AuthenticationService', mytournamentsController]);

function mytournamentsController($scope, $http, $stateParams, $window, $state, AuthenticationService) {

    initController();

    function initController() {
        $scope.tournamentNotAdded = true;
    }

    $scope.myTournamentData = {
        StartTime: undefined,
        EndTime: undefined,
        Type: undefined,
        Location: undefined,
        Matches: undefined,
        Referees: undefined,
        Teams: undefined
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
            $window.alert("Couldn't get response.");
            return "Couldn't get response.";
        });
    }

    //edit tournament part
    $scope.tournamentName = $stateParams.tournamentName;
    //change state when dropdown is changed
    $scope.changeState = function (path) {
        $state.go(path, {});
    }
}