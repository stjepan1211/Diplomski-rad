//Define tournaments controller
angular.module('TournamentModule').controller('tournamentsController', ['$scope', '$http', '$stateParams', '$window', '$state','AuthenticationService', tournamentsController]);

function tournamentsController($scope, $http, $stateParams, $window, $state, AuthenticationService) {

    

    $scope.tournamentData = {
        StartTime: undefined,
        EndTime: undefined,
        Type: undefined,
        Locations: undefined,
        Matches: undefined,
        Referees: undefined,
        Teams: undefined
    }
    
    initController();

    function initController() {
        // reset login status
        AuthenticationService.CheckIsStoraged();
    };

    //get all tournaments
    $scope.getAllTournaments = function () {
        $http.get('api/tournament/getall')
        .then(function (response) {
            $scope.tournamentData = response.data;
        }, function (response) {
            return "Couldn't get response.";
        });
    }

    $scope.addTournament = function () {
        if (!AuthenticationService.Check()) {
            $window.alert("To add tournament you need to log in first.");
        }
        else {
            $state.go('addtournament');
        }
    }

    $scope.ispis = function () {
        
    }

}