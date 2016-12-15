//Define news controller
angular.module('TournamentModule').controller('newsController', ['$scope', '$http', '$stateParams', '$window', '$state', newsController]);

function newsController($scope, $http, $stateParams, $window, $state) {

    $scope.ispis = function () {
        $window.alert("ispis");
    }

}