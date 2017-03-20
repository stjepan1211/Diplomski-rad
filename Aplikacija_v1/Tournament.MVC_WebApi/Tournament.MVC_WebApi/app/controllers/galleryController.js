//Define gallery controller
angular.module('TournamentModule').controller('galleryController', ['$scope', '$http', '$stateParams', '$window', '$state','$timeout', 'QueueService', galleryController]);

function galleryController($scope, $http, $stateParams, $window, $state, $timeout, QueueService) {

    var INTERVAL = 10000;

    var slides = [{ id: "image00", src: "http://www.mali-nogomet.hr/img/full/66561_6352.jpg" },
        { id: "image02", src: "http://www.palmbeachsoccerclub.com.au/wp-content/uploads/2016/01/pb5.jpg" }];



    function setCurrentSlideIndex(index) {
        $scope.currentIndex = index;
    }

    function isCurrentSlideIndex(index) {
        return $scope.currentIndex === index;
    }

    function nextSlide() {
        $scope.currentIndex = ($scope.currentIndex < $scope.slides.length - 1) ? ++$scope.currentIndex : 0;
        $timeout(nextSlide, INTERVAL);
    }

    function setCurrentAnimation(animation) {
        $scope.currentAnimation = animation;
    }

    function isCurrentAnimation(animation) {
        return $scope.currentAnimation === animation;
    }

    function loadSlides() {
        QueueService.loadManifest(slides);
    }

    $scope.$on('queueProgress', function (event, queueProgress) {
        $scope.$apply(function () {
            $scope.progress = queueProgress.progress * 100;
        });
    });

    $scope.$on('queueComplete', function (event, slides) {
        $scope.$apply(function () {
            $scope.slides = slides;
            $scope.loaded = true;

            $timeout(nextSlide, INTERVAL);
        });
    });

    $scope.progress = 0;
    $scope.loaded = false;
    $scope.currentIndex = 0;
    $scope.currentAnimation = 'slide-left-animation';

    $scope.setCurrentSlideIndex = setCurrentSlideIndex;
    $scope.isCurrentSlideIndex = isCurrentSlideIndex;
    $scope.setCurrentAnimation = setCurrentAnimation;
    $scope.isCurrentAnimation = isCurrentAnimation;

    loadSlides();
}