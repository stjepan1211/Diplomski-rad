//Define gallery controller
angular.module('TournamentModule').controller('galleryController', ['$scope', '$http', '$stateParams', '$window', '$state', galleryController]);

function galleryController($scope, $http, $stateParams, $window, $state) {

    $scope.ispis = function () {
        $window.alert("ispis");
    }

}