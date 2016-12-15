//Define topplayers controller
angular.module('TournamentModule').controller('topplayersController', ['$scope', '$http', '$stateParams', '$window', '$state', topplayersController]);

function topplayersController($scope, $http, $stateParams, $window, $state) {

    $scope.ispis = function () {
        $window.alert("ispis");
    }

}