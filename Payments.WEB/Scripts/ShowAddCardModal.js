$(document).ready(function () {
    $('.btn-block').click(function () {
        var url = $('#addCard').data('url');
        $.get(url, function (data) {
            $("#addCard").html(data);
            $("#addCard").modal('show');
        });
    })
});