//Define edit player controller
angular.module('TournamentModule').controller('editplayerController', ['$scope', '$http', '$stateParams', '$window', '$state', editplayerController]);

function editplayerController($scope, $http, $stateParams, $window, $state) {

    initController();

    function initController() {
        console.log("edit player controller initialized");
    }

    $scope.changestate = function (path) {
        $state.go(path, {});
    }
}