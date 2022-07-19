//Aquí van los métodos que se pueden utilizar en varios sitios de la plataforma
$(document).ready(function () {
});

function cambiarIdioma(idioma) {
    $.ajax({
        url: '/Configuracion/CambiarIdioma?idioma=' + idioma,
        type: "GET",
        async: false,
        success: function (data) {
            location.reload();
        },
    });
}

function getIdioma() {
    var idioma = 'es';
    $.ajax({
        url: '/Home/GetIdioma',
        type: "GET",
        async: false,
        success: function (data) {
            idioma = data.idioma;
        },
    });
    return idioma;
}

//FORMATEAR FECHA
function extenderA2(num) {
    return num.toString().padStart(2, '0');
}

function formatearFecha(date) {
    return [
        extenderA2(date.getDate()),
        extenderA2(date.getMonth() + 1),
        date.getFullYear(),
    ].join('-');
}