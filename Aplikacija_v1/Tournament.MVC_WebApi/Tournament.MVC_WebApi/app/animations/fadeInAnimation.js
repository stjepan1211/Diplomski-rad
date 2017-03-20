TournamentModule.animation('.fade-in-animation', function ($window) {
    return {
        enter: function (element, done) {
            TweenMax.fromTo(element, 1, { opacity: 0 }, { opacity: 1, onComplete: done });
        },

        leave: function (element, done) {
            TweenMax.to(element, 1, { opacity: 0, onComplete: done });
        }
    };
});