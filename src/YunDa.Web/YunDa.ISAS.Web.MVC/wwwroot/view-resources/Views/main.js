var pageMain = pageMain || {};
$(function () {
    pageMain.accessToken = $("#accessToken", window.parent.document).val()
    pageMain.ajaxPost = function (appUrl, paramsJsonStr, ) {
        $.ajax({
            type: "post",
            url: '/api/services/app/User/GetUsers',
            data: JSON.stringify(c),
            dataType: "json",
            contentType: "application/json",
            beforeSend: function (XMLHttpRequest) {
                XMLHttpRequest.setRequestHeader("Authorization", "Bearer" + " " + accessToken);
            },
            success: function (data, textStatus) {
                alert(JSON.stringify(data));
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(textStatus + "||" + errorThrown);
            }
        });
    }
})