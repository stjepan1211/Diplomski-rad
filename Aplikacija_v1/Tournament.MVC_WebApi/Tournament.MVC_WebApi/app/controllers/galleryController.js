//Define gallery controller
angular.module('TournamentModule').controller('galleryController', ['$scope', '$http', '$stateParams', '$window', '$state','$timeout','PromiseUtilsService', 'QueueService', galleryController]);

function galleryController($scope, $http, $stateParams, $window, $state, $timeout, PromiseUtilsService, QueueService) {

    var INTERVAL = 10000;

    //var slides = [{ id: "image00", src: "" }];

    var slides = [{}];

    $scope.getImages = function () {
        PromiseUtilsService.getPromiseHttpResult($http.get('api/gallery/getall'))
            .then(function (response) {
                $scope.slides = response.data;

                angular.forEach($scope.slides, function(value,key){
                    $http.get('api/tournament/get?id=' + value.TournamentId)
                        .then(function(response){
                            value.Tournament = response.data.Name;
                        }, function(response){
                        
                     })
                })

                console.log($scope.slides);
            }, function (response) {
                $window.alert("Couldn't get images.")
            });
    }

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