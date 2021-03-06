﻿$(document).ready(function () {
    $('#verifyFunction').click(verifyFunction);
    $('#verifyLogic').click(verifyLogic);
    $('#loadQueue').click(loadQueue);
    $('#verifyQueue').click(verifyQueue);
    $('#loadTable').click(loadTable);
    $('#verifyTable').click(verifyTable);

    var documentStatusColumns = [{ ColumnName: "name", Title: "Name" }, { ColumnName: "processed", Title: "Processed Time" }, { ColumnName: "status", Title: "Status" }];

    function verifyLogic() {
        showStart("Verifying Logic App results");
        runTest("verifysqldata", getPayload(), "Logic App Verification", documentStatusColumns, showResult, true, true);
    }
    function verifyFunction() {
        showStart("Verify Function App");
        runTest("verifyqueue", getPayload(), "Function App Verification", documentStatusColumns, showResult, true, true);
    }
    function loadQueue() {
        showStart("Loading Queue messages");
        runTest("loadqueue", getPayload(), "Queue Load Verification", documentStatusColumns, showResult, false, true);
    }
    function verifyQueue() {
        showStart("Verify Queue Messages");
        runTest("verifyqueue", getPayload(), "Queue Verification", documentStatusColumns, showResult, true, true);
    }
    function loadTable() {
        showStart("Loading Table Rows");
        runTest("loadtable", getPayload(), "Table Load Verification", documentStatusColumns, showResult, false, true);
    }
    function verifyTable() {
        showStart("Verify Table Rows");
        runTest("verifytable", getPayload(), "Table Verification", documentStatusColumns, showResult, true, true);
    }
    function getPayload(interval, iterations) {
        return {
            storageConnection: "-1",
            sqlConnection: "-1"
        };
    }
    function showStart(text) {
        $('#currentTest').text(text);
        $('#results').text('');
    }

    function runTest(operation, payload, title, fields, callBack, showData, async) {
        var url = "/evaluate/" + operation;// + "?" + queryString + "&encryptionKey=" + encodeURIComponent(key);
        data = JSON.stringify(payload);
        $.post({
            url: url,
            data: payload,
            dataType: "json",
            async: async
        }).done(function (data) {
            callBack(data, title, fields, showData);
        }).fail(function (error) {
            $('#results').append("<div><h3>" + title + " error</h3>" + error.statusText + "</div>");
        });
    }

    function showResult(data, title, fields, showData) {
        $('#results').append("<div><h3>" + title + "</h3>" + data.message + "</div>");
        if (data.passed && showData) {
            var dataOut = "<table class='table'><thead><tr>";
            $(fields).each(function (index, field) {
                dataOut += "<th>" + field.Title + "</th>";
            });
            dataOut += "</tr></thead><tbody>";
            $(data.data).each(function (index, row) {
                dataOut += "<tr>";
                $(fields).each(function (index, field) {
                    dataOut += "<td>" + displayData(row, field.ColumnName) + "</td>";
                });
                dataOut += "</tr>";
            });
            dataOut += "</tbody></table>";
            $('#results').append(dataOut);
        }

        function displayData(row, fieldName) {
            var data = row[fieldName];
            var d = new Date(data);
            if (fieldName === "time") {
                // it is a date
                return "" + d.getUTCFullYear() + "." + d.getUTCMonth() + "." + d.getUTCDate() + " " + d.getUTCHours() + ":" + d.getUTCMinutes() + ":" + d.getUTCSeconds();

            } else if (fieldName === "reading") {
                //It is a number
                return data.toFixed(2);
            } else {
                //It is a string
                return data;
            }
        }
    }
});