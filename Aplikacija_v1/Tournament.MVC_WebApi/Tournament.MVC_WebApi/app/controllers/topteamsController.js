//Define topteams controller
angular.module('TournamentModule').controller('topteamsController', ['$scope', '$http', '$stateParams', '$window', '$state', 'PromiseUtilsService', topteamsController]);

function topteamsController($scope, $http, $stateParams, $window, $state, PromiseUtilsService) {

    $scope.teamsMostPoints = {
        Name: undefined,
        TournamentId: undefined,
        Tournament: undefined,
        Points: undefined
    }

    $scope.teamsMostWins = {
        Name: undefined,
        TournamentId: undefined,
        Tournament: undefined,
        Won: undefined
    }

    $scope.teamsMostGoals = {
        Name: undefined,
        TournamentId: undefined,
        Tournament: undefined,
        GoalsScored: undefined
    }

    $scope.getMostPoints = function () {
        PromiseUtilsService.getPromiseHttpResult($http.get('api/team/getmostpoints'))
            .then(function (response) {
                $scope.teamsMostPoints = response.data;

                angular.forEach($scope.teamsMostPoints, function (value, key) {
                    PromiseUtilsService.getPromiseHttpResult($http.get('api/tournament/get?id=' + value.TournamentId))
                    .then(function (response) {
                        value.Tournament = response.data.Name;

                    }, function (response) {
                        console.log(reponse.data.Message);
                    });
                });

            }, function (response) {
                console.log(reponse.data.Message);
            });
    }

    $scope.getMostWins = function () {
        PromiseUtilsService.getPromiseHttpResult($http.get('api/team/getmostwins'))
            .then(function (response) {
                $scope.teamsMostWins = response.data;

                angular.forEach($scope.teamsMostWins, function (value, key) {
                    PromiseUtilsService.getPromiseHttpResult($http.get('api/tournament/get?id=' + value.TournamentId))
                    .then(function (response) {
                        value.Tournament = response.data.Name;

                    }, function (response) {
                        console.log(reponse.data.Message);
                    });
                });

            }, function (response) {
                console.log(reponse.data.Message);
            });
    }

    $scope.getMostGoals = function () {
        PromiseUtilsService.getPromiseHttpResult($http.get('api/team/getmostgoals'))
            .then(function (response) {
                $scope.teamsMostGoals = response.data;

            angular.forEach($scope.teamsMostGoals, function (value, key) {
                PromiseUtilsService.getPromiseHttpResult($http.get('api/tournament/get?id=' + value.TournamentId))
                .then(function (response) {
                    value.Tournament = response.data.Name;

                }, function (response) {
                    console.log(reponse.data.Message);
                });
         });

        }, function (response) {
            console.log(reponse.data.Message);
        });
    }

}