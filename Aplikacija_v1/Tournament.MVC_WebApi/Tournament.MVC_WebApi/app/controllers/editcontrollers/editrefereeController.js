//Define edit referee controller
angular.module('TournamentModule').controller('editrefereeController', ['$scope', '$http', '$stateParams', '$window', '$state', editrefereeController]);

function editrefereeController($scope, $http, $stateParams, $window, $state) {

    initController();

    function initController() {
        console.log("edit referee controller initialized");
    }

    $scope.changestate = function (path) {
        $state.go(path, {});
    }
}