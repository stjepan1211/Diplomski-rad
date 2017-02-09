//Define edit team controller
angular.module('TournamentModule').controller('editteamController', ['$scope', '$http', '$stateParams', '$window', '$state', editteamController]);

function editteamController($scope, $http, $stateParams, $window, $state) {

    initController();

    function initController() {
        console.log("edit team controller initialized");
    }

    $scope.changestate = function (path) {
        $state.go(path, {});
    }
}