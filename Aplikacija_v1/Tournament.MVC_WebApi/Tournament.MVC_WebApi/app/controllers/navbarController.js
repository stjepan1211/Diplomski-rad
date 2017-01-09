//Define navbar controller
angular.module('TournamentModule').controller('navbarController', ['$scope', '$http', '$stateParams', '$window', '$state', 'AuthenticationService', navbarController]);

function navbarController($scope, $http, $stateParams, $window, $state, AuthenticationService) {

    $scope.da = false;

}