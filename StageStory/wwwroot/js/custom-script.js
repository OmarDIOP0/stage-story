var token = $('input[name="__RequestVerificationToken"]').val();
function ajaxManager(type, url, successHandler, errorHandler, data = null) {
    $.ajax({
        type,
        url,
        headers: { 'RequestVerificationToken': token },
        data,
        success: successHandler,
        error: errorHandler
    });
}