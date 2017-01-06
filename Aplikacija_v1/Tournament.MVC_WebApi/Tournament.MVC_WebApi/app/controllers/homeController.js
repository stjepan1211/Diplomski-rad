//Define home controller
angular.module('TournamentModule').controller('homeController', ['$scope', '$http', '$stateParams', '$window', '$state', homeController]);

function homeController($scope, $http, $stateParams, $window, $state) {

    $scope.vrijeme = (new Date()).getTime();

    $scope.ispis = function () {
        $window.alert("ispis");
    }

}