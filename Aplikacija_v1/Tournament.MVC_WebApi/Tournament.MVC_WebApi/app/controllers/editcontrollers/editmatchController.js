//Define edit match controller
angular.module('TournamentModule').controller('editmatchController', ['$scope', '$http', '$stateParams', '$window', '$state', editmatchController]);

function editmatchController($scope, $http, $stateParams, $window, $state) {

    initController();

    function initController() {
        console.log("edit match controller initialized");
    }

    $scope.changestate = function (path) {
        $state.go(path, {});
    }

    $scope.onTimeSet = function (newDate, oldDate) {
        console.log(newDate);
        console.log(oldDate);
    }
}