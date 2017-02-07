//Define gallery controller
angular.module('TournamentModule').controller('mytournamentsController', ['$scope', '$http', '$stateParams', '$window', '$state', mytournamentsController]);

function mytournamentsController($scope, $http, $stateParams, $window, $state) {

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
        var myTournamentData = undefined;
        var username = 'stjepanbaricevic1211@gmail.com';
        $http.get('api/aspnetuser/getbyusername?username=' + username)
        .then(function (response) {
            $scope.tournamentNotAdded = false;
            $scope.myTournamentData = response.data.Tournaments;

        }, function (response) {
            console.log("Couldn't get response.");
            return "Couldn't get response.";
        });
    }

    //edit tournament part
    $scope.tournamentName = $stateParams.tournamentName;

    $scope.changeState = function (path) {
        $state.go(path, {});
    }
}