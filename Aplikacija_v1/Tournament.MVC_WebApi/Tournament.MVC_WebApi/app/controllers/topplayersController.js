//Define topplayers controller
angular.module('TournamentModule').controller('topplayersController', ['$scope', '$http', '$stateParams', '$window', '$state', 'PromiseUtilsService', topplayersController]);

function topplayersController($scope, $http, $stateParams, $window, $state, PromiseUtilsService) {


    $scope.topTwentyPlayers = {
        Name: undefined,
        Surname: undefined,
        Team: undefined,
        TournamentId: undefined,
        Tournament: undefined,
        Goals: undefined,
        YellowCards: undefined,
        RedCards: undefined
    }

    $scope.gettoptwentyplayers = function () {
        $http.get('api/player/gettoptwenty')
            .then(function (response) {
                $scope.topTwentyPlayers = response.data;
            
            }, function (response) {
                $window.alert("Could't get players. " + response.data.Message);
            });
    }

}