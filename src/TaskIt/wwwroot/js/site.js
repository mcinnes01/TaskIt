$(function () {
    $(document).on('click', '.renderget', function (evt) {
        evt.preventDefault();
        evt.stopPropagation();

        var $detailsDiv = $('#' + $(this).data('target')),
            url = $(this).data('actionurl');

        $.get(url, function (data) {
            $detailsDiv.replaceWith(data);
        });
    });
});

$(function () {
    $(document).on('click', '.renderpost', function (evt) {
        evt.preventDefault();
        evt.stopPropagation();

        var $detailsDiv = $('#' + $(this).data('target')),
            url = $(this).data('actionurl');

        $.post(url, function (data) {
            $detailsDiv.replaceWith(data);
        });
    });
});

$(function () {
    $(document).on('click', '.colour-btn', function (evt) {
        evt.preventDefault();
        evt.stopPropagation();

        $('#cp1').colorpicker();
    })
});

$(function () {
    var elements = document.querySelectorAll('.editable'),
    editor = new MediumEditor(elements);
});