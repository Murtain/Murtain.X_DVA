// Write your Javascript code.

$('#sel-gender').change(function () {
    $('#gender').val($('#sel-gender').val());
    $('#form-gender').submit();
});