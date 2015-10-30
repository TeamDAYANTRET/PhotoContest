$(document).ready(function () {
    $.fn.addAutocomplete = function (tags) {
        this.autocomplete({
            source: tags,
            autoFocus: true
        });
    };

    $("#add-member").addAutocomplete([]);

    $("#add-member").keyup(function () {
        var value = $("#add-member").val();
        var url = $("#add-member").attr("data-url"); console.log(url);
        $.get(url, { username: value }).done(function (data) {
            var availableTags = data;

            $("#add-member").addAutocomplete(availableTags);
        });
    })
})