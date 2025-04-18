$(document).ready(function () {
    var $stars = $('#ratingStars i');
    var $ratingInput = $('#Rating');
    function updateStars(rating) {
        $stars.each(function () {
            var starValue = $(this).data('value');
            if (starValue <= rating) {
                $(this)
                    .removeClass('bi-star text-secondary')
                    .addClass('bi-star-fill text-warning');
            } else {
                $(this)
                    .removeClass('bi-star-fill text-warning')
                    .addClass('bi-star text-secondary');
            }
        });
    }

    $stars.on('mouseover', function () {
        var rating = $(this).data('value');
        updateStars(rating);
    });

    $stars.on('mouseout', function () {
        var currentRating = parseInt($ratingInput.val()) || 0;
        updateStars(currentRating);
    });

    $stars.on('click', function () {
        var rating = $(this).data('value');
        $ratingInput.val(rating);
        updateStars(rating);
    });
});