//Define sign up controller
angular.module('TournamentModule').controller('signupController', ['$scope', '$http', '$stateParams', '$window', '$state', signupController]);

function signupController($scope, $http, $stateParams, $window, $state) {

    $scope.ispis = function () {
        $window.alert("ispis");
    }

}