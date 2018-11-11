// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    $('#testAPI').click(testAPIClick);
    function testAPIClick() {
        var payload = {
            'Address': $('#address').val(),
            'Port': Number($('#port').val()),
            'Command': Number($('.command:checked').val()),
            'Minutes': 1
        };
        var test = JSON.stringify(payload);

        $.post({
            url: "/service/test",
            data: payload,
            dataType: "json"
        }).done(function (data) {
            var result = (data.success ? "Success: " : "Failure: ") + data.data;
            $('#results').text(result);
        }).fail(function () {
            $('#results').text('Call failed');
        });



    }
});

function setCommand(command) {
    if (command === '1') {
        $("#minutesDiv").attr("hidden","hidden" );
    } else {
        $("#minutesDiv").removeAttr("hidden");

    }
}
