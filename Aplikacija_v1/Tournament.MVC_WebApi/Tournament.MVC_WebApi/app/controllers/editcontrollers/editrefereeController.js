//Define edit referee controller
angular.module('TournamentModule').controller('editrefereeController', ['$scope', '$http', '$stateParams', '$window', '$state','AuthenticationService', editrefereeController]);

function editrefereeController($scope, $http, $stateParams, $window, $state, AuthenticationService) {

    initController();

    function initController() {
    }

    $scope.addRefereeData = {
        Name: undefined,
        Surname: undefined
    }

    $scope.refereesByTournamentData = {
        Name: undefined,
        Surname: undefined
    }

    $scope.editRefereeData = {
        Name: undefined,
        Surname: undefined
    }

    $scope.tournamentsByUsername = {
        Id: undefined,
        Name: undefined
    }

    $scope.changestate = function (path) {
        $state.go(path, {});
    }

    //get tournaments by username
    $scope.getTournamentsByUsername = function () {
        var username = AuthenticationService.GetUsername();
        $http.get('/api/tournament/getbyusername?username=' + username)
            .then(function (response) {
                $scope.tournamentsByUsername = response.data;
                console.log($scope.tournamentsByUsername);
            }, function (response) {
                console.log("Couldn't get response");
            })
    }
    //get referees by tournament
    $scope.getRefereesByTournament = function () {
        var tournamentId = $stateParams.tournamentId;
        $http.get('/api/referee/getrefereesbytournament?tournamentId=' + tournamentId)
            .then(function (response) {
                $scope.refereesByTournamentData = response.data;
                console.log($scope.RefereesByTournamentData);
            }, function (response) {
                console.log("Couldn't get response");
            })
    }

    //edit referee
    $scope.editReferee = function () {
        var tournamentId = $stateParams.tournamentId;
        if ($scope.editRefereeData.Name == undefined && $scope.editRefereeData.Surname == undefined) {
            $window.alert("You need to edit minimum one property.")
        }
        else {
            var referee = {
                Id: $scope.updaterefereeselected.Id,
                TournamentId: tournamentId,
                Name: $scope.editRefereeData.Name,
                Surname: $scope.editRefereeData.Surname
            }
            $http.put('api/referee/update', referee)
            .then(function (response) {
                $window.alert("Referee updated successfully.");
                $state.go('editmytournament.referee');
            }, function (response) {
                $window.alert("Can't edit referee.");
            });
        }
    }

    //add referee
    $scope.addReferee = function () {
        if ($scope.addRefereeData.Name == undefined) {
            $window.alert("Please add referee's name.");
        }
        else if ($scope.addRefereeData.Surname == undefined) {
            $window.alert("Please add referee's surname.");
        }
        else if ($scope.tournamentselected.Id == undefined) {
            $window.alert("Please select tournament.")
        }
        else {
            var referee = {
                TournamentId: $scope.tournamentselected.Id,
                Name: $scope.addRefereeData.Name,
                Surname: $scope.addRefereeData.Surname
            }
            $http.post('/api/referee/add', referee)
                .then(function (response) {
                    $window.alert("Referee added successfully.");
                    //$state.go('editmytournament.referee');
                }, function (response) {
                    $window.alert("Can't add player.");
                });
        }
    }

    $scope.deleteReferee = function () {
        if (id == undefined) {
            $window.alert("Please select referee.");
        }
        else {
            if ($window.confirm('Are you sure you want to delete this referee?')) {
                $http.delete('/api/referee/delete?id=' + id)
                    .then(function (response) {
                        $window.alert("Referee deleted successfully.");
                        $state.go('editmytournament.referee');
                    }, function (response) {
                        $window.alert("Can't delete referee.");
                    })
            }
        }
    }
}