// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
var options = "";
$.ajax({
    method: "GET",
    contentType: "application/json",
    url: "/Products/GetUnits",
    success: function (v) {
        console.log(v);
        $.each(v, function (i, v1) {
            console.log(v1);
            options += "<option value='" + v1.unitId + "'>" + v1.unitName + "</option>";
        });
    },
    error: function (error) {
        console.log(error)
    }
});