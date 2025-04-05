// ========== Общие функции ========== //

function updateCartCount(count) {
    const $cartBadge = $('.cart-badge');
    $cartBadge.text(count);

    
    $cartBadge.addClass('animate__animated animate__bounce');
    setTimeout(() => {
        $cartBadge.removeClass('animate__animated animate__bounce');
    }, 500);


    count > 0 ? $cartBadge.show() : $cartBadge.hide();
}


function handleCartError(xhr) {
    if (xhr.status === 401) {
        window.location.href = '/Account/Login';
    } else {
        alert('Ошибка: ' + (xhr.responseJSON?.message || 'Операция не выполнена'));
    }
}

// ========== Обработчики событий ========== //

// Добавление/удаление товара (кнопки на главной странице)
$(document).on('click', '.cart-button', function (e) {
    e.preventDefault();
    e.stopPropagation();

    const button = $(this);
    const productId = button.data('product-id');
    const isRemoveAction = button.hasClass('btn-danger');
    const url = isRemoveAction ? '/Cart/RemoveFromCart' : '/Cart/AddToCart';

    button.prop('disabled', true)
        .html('<span class="spinner-border spinner-border-sm" role="status"></span>');

    $.ajax({
        url: url,
        type: 'POST',
        data: { productId: productId },
        success: function (response) {
            if (response.success) {
                button.toggleClass('btn-danger btn-primary')
                    .text(isRemoveAction ? 'Добавить в корзину' : 'Удалить из корзины');
                updateCartCount(response.cartCount);
            }
        },
        error: handleCartError,
        complete: function () {
            button.prop('disabled', false)
                .text(isRemoveAction ? 'Добавить в корзину' : 'Удалить из корзины');
        }
    });
});

// Удаление товара из корзины (страница корзины)
$(document).on('click', '.remove-from-cart-btn', function (e) {
    e.preventDefault();

    const button = $(this);
    const itemId = button.data('item-id');

    button.prop('disabled', true)
        .html('<span class="spinner-border spinner-border-sm" role="status"></span>');

    $.ajax({
        url: '/Cart/RemoveItem',
        type: 'POST',
        data: { itemId: itemId },
        success: function (response) {
            if (response.success) {
                button.closest('.list-group-item').fadeOut(300, function () {
                    $(this).remove();

                    $('.card-body .d-flex.justify-content-between.mb-2 span:last').eq(0)
                        .text(response.totalItems + ' товар(ов)');
                    $('.card-body .d-flex.justify-content-between.mb-2 span:last').eq(1)
                        .text(response.cartTotal);
                    $('.card-body .d-flex.justify-content-between.fw-bold.fs-5 span:last')
                        .text(response.cartTotal);

                    updateCartCount(response.cartCount);

                    if (response.cartCount === 0) {
                        setTimeout(() => location.reload(), 500);
                    }
                });
            }
        },
        error: handleCartError,
        complete: function () {
            button.prop('disabled', false).html('<i class="bi bi-trash"></i>');
        }
    });
});

$(document).on('click', '.plus-btn, .minus-btn', function () {
    const input = $(this).siblings('.quantity-input');
    let value = parseInt(input.val());

    if ($(this).hasClass('plus-btn')) {
        value += 1;
    } else {
        value = Math.max(1, value - 1);
    }
    input.val(value);

    const itemId = input.data('id');
    const button = $(this);

    button.prop('disabled', true);

    $.ajax({
        url: '/Cart/UpdateQuantity',
        type: 'POST',
        data: {
            itemId: itemId,
            quantity: value
        },
        success: function (response) {
            if (response.success) {
   
                const itemRow = input.closest('.list-group-item');
                itemRow.find('.col-2.text-end h5').text(response.itemTotal);

                $('.card-body .d-flex.justify-content-between.mb-2 span:last').eq(0)
                    .text(response.totalItems + ' товар(ов)');
                $('.card-body .d-flex.justify-content-between.mb-2 span:last').eq(1)
                    .text(response.cartTotal);
                $('.card-body .d-flex.justify-content-between.fw-bold.fs-5 span:last')
                    .text(response.cartTotal);
                updateCartCount(response.totalItems);

            }
        },
        error: function (xhr) {
            if (xhr.status === 401) {
                window.location.href = '/Account/Login';
            } else {
                alert('Ошибка: ' + (xhr.responseJSON?.message || 'Не удалось обновить количество'));
                input.val(value);
            }
        },
        complete: function () {
            button.prop('disabled', false);
        }
    });
});
