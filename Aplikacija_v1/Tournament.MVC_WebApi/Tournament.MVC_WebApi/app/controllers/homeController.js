//Define home controller
angular.module('TournamentModule').controller('homeController', ['$scope', '$http', '$stateParams', '$window', '$state', homeController]);

function homeController($scope, $http, $stateParams, $window, $state) {

    $scope.getInProgressTouraments = function () {
        $http.get('api/tournament/getinprogresstournaments')
            .then(function (response) {
                $scope.tournamentInProgressData = response.data;
            }, function (response) {
                console.log("Can't get in progress tournaments.");
            });
    }

    $scope.getTouramentsAnnouncements = function () {
        $http.get('api/tournament/gettournamentannouncements')
            .then(function (response) {
                $scope.tournamentAnnouncements = response.data;
            }, function (response) {
                console.log("Can't get in progress tournaments.");
            });
    }

}