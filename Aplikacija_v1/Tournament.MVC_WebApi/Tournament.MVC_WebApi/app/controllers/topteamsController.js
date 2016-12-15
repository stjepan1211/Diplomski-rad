//Define topteams controller
angular.module('TournamentModule').controller('topteamsController', ['$scope', '$http', '$stateParams', '$window', '$state', topteamsController]);

function topteamsController($scope, $http, $stateParams, $window, $state) {

    $scope.ispis = function () {
        $window.alert("ispis");
    }

}