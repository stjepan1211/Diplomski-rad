//Define news controller
angular.module('TournamentModule').controller('newsController', ['$scope', '$http', '$stateParams', '$window', '$state', newsController]);

function newsController($scope, $http, $stateParams, $window, $state) {

    initController();

    function initController() {
        $window.alert("ispis");
    }

    $scope.ispis = function () {
        
    }

}