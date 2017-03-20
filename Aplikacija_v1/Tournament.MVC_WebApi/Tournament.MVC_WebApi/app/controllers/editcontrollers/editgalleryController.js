angular.module('TournamentModule').controller('editgalleryController', ['$scope', '$http', '$stateParams', '$window', '$state', 'AuthenticationService', editgalleryController]);

function editgalleryController($scope, $http, $stateParams, $window, $state, AuthenticationService) {

    $scope.imageData = {
        TournamentId: undefined,
        Url: undefined
    }

    //get tournaments by username
    $scope.getTournamentsByUsername = function () {
        var username = AuthenticationService.GetUsername();
        $http.get('/api/tournament/getbyusername?username=' + username)
            .then(function (response) {
                $scope.tournamentsByUsername = response.data;
                //console.log($scope.tournamentsByUsername);
            }, function (response) {
                console.log("Couldn't get response");
            })
    }

    $scope.changestate = function (path) {
        $state.go(path, {});
    }

    $scope.addImage = function () {
        var gallery = {
            TournamentId: $scope.tournamentselected.Id,
            Url: $scope.imageData.Url
        }

        $http.post('api/gallery/add', gallery)
            .then(function (response) {
                $window.alert("Image added successful.");
            }, function (response) {
                $window.alert("Warning: " + response.data.Message);
            });
    }

    $scope.getImagesByTournament = function () {
        $http.get('api/gallery/getallwheretournamentid?tournamentId=' + $stateParams.tournamentId)
            .then(function (response) {
                $scope.tournamentImages = response.data;
            }, function (response) {
                $window.alert("Warning: " + response.data.Message);
            });
    }

    $scope.deleteImage = function () {
        console.log($scope.imageselected);
        console.log($scope.imageselected.Id);
        if ($scope.imageselected.Id == undefined) {
            $window.alert("Please select image.");
        }
        else {
            if ($window.confirm('Are you sure you want to delete this image?')) {
                $http.delete('/api/gallery/delete?id=' + $scope.imageselected.Id)
                    .then(function (response) {
                        $window.alert("Image deleted successfully.");
                        $state.go('editmytournament.gallery');
                    }, function (response) {
                        $window.alert("Can't delete image." + response.data.Message);
                    })
            }
        }
    }
}