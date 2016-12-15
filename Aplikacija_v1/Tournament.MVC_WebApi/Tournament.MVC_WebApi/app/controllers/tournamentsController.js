//Define tournaments controller
angular.module('TournamentModule').controller('tournamentsController', ['$scope', '$http', '$stateParams', '$window', '$state', tournamentsController]);

function tournamentsController($scope, $http, $stateParams, $window, $state) {

    $scope.tournamentData = {
        StartTime: undefined,
        EndTime: undefined,
        Type: undefined,
        Locations: undefined,
        Matches: undefined,
        Referees: undefined,
        Teams: undefined
    }
    
    //get all tournaments
    $scope.getAllTournaments = function () {
        $http.get('api/tournament/getall')
        .then(function (response) {
            $scope.tournamentData = response.data;
        }, function (response) {
            return "Couldn't get response.";
        });
    }

    $scope.ispis = function () {
        $window.alert("ispis");
        console.log($scope.tournamentData);
    }

}