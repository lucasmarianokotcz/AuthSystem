// Remove alertas depois de um tempo
$(document).ready(function () {
    $(".alert-success").fadeTo(2000, 1).fadeOut(500, function () {
        $(".alert-success").fadeOut(500);
    });
});