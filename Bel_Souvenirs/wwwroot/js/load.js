let offset = 9;
let loading = false;

function loadMoreProducts() {
    if (loading) return;
    loading = true;
    $('#loader').show();

    $.get('/Home/LoadProducts', { offset: offset }, function (data) {
        if (data.trim().length === 0) {
            $(window).off('scroll');
            $('#loader').hide();
            return;
        }

        $('#product-container').append(data);
        offset += 9;
        loading = false;
        $('#loader').hide();
    });
}

$(window).on('scroll', function () {
    if ($(window).scrollTop() + $(window).height() >= $(document).height() - 200) {
        loadMoreProducts();
    }
});
