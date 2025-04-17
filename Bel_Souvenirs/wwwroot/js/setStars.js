$(document).ready(function () {
    var $stars = $('#ratingStars i');
    var $ratingInput = $('#NewRating');
    function updateStars(rating) {
        $stars.each(function () {
            var starValue = $(this).data('value');
            if (starValue <= rating) {
                $(this).removeClass('text-secondary').addClass('text-warning');
            } else {
                $(this).removeClass('text-warning').addClass('text-secondary');
            }
        });
    }

    $stars.on('mouseover', function () {
        var rating = $(this).data('value');
        updateStars(rating);
    });

    $stars.on('mouseout', function () {
        var currentRating = $ratingInput.val() || 0;
        updateStars(currentRating);
    });

    $stars.on('click', function () {
        var rating = $(this).data('value');
        $ratingInput.val(rating);
        updateStars(rating);
    });
});