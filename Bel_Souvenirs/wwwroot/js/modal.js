$(document).ready(function () {
    $('#orderForm').submit(function (e) {
        e.preventDefault();

        $.ajax({
            url: $(this).attr('action'),
            type: 'POST',
            data: $(this).serialize(),
            success: function (response) {
                if (response.success) {
                    $('#successMessage').text(response.message);
                    $('#HomeLink').attr('href', '/');

                    var modal = new bootstrap.Modal(document.getElementById('orderSuccessModal'));
                    modal.show();
                } else {
                    alert(response.message);
                }
            },
            error: function () {
                alert('Произошла ошибка при отправке запроса');
            }
        });
    });
});