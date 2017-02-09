//Define edit result controller
angular.module('TournamentModule').controller('editresultController', ['$scope', '$http', '$stateParams', '$window', '$state', editresultController]);

function editresultController($scope, $http, $stateParams, $window, $state) {

    initController();

    function initController() {
        console.log("edit result controller initialized");
    }

    $scope.changestate = function (path) {
        $state.go(path, {});
    }
}