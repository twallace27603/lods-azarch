$(document).ready(function () {

    $('#test').click(TestConnection);

    function TestConnection() {
        $.get('/evaluation/connect').done(function (data) {
            $('#testResults').html("<table class='table'><tr><td>Passed</td><td>" + data.passed + "</td></tr><tr><td>Message</td><td>" + data.message + "</td></tr></table>");
        }).fail(function (xhr, status, error) {
            $('#testResults').text("The service call failed: " + status);
        });
    }

});