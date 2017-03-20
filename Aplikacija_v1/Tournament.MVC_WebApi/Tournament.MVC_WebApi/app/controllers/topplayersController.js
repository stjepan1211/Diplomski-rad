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
        PromiseUtilsService.getPromiseHttpResult($http.get('api/player/gettoptwenty'))
            .then(function (response) {
                $scope.topTwentyPlayers = response.data;

                angular.forEach($scope.topTwentyPlayers, function (value, key) {
                    PromiseUtilsService.getPromiseHttpResult($http.get('api/team/get?id=' + value.TeamId))
                    .then(function (response) {
                        value.Team = response.data.Name;
                        value.TournamentId = response.data.TournamentId;

                        angular.forEach($scope.topTwentyPlayers, function (value, key) {
                            PromiseUtilsService.getPromiseHttpResult($http.get('api/tournament/get?id=' + value.TournamentId))
                            .then(function (response) {
                                value.Tournament = response.data.Name;
                                console.log(response.data.Name);
                                console.log(value.Tournament);
                            }, function (response) {
                                //$window.alert("Could't get tournament. " + response.data.Message);
                            });
                        });

                    }, function (response) {
                        $window.alert("Could't get team. " + response.data.Message);
                    });
                });
            
            }, function (response) {
                $window.alert("Could't get players. " + response.data.Message);
            });
    }

}