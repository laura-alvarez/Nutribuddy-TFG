$(document).ready(function () {
    pedirDatosPaciente();
});

function pedirDatosPaciente() {
    var idioma = getIdioma();
    $.ajax({
        url: '/ZonaPrivada/GetDatosResumenPaciente',
        type: "GET",
        async: false,
        success: function (data) {
            if (data.flag == 0) {
                $("#noContenidoMes").addClass('hidden');
                $("#contenidoMes").removeClass('hidden');
                crearGraficaProgreso(data.datosG.Datos, data.datosG.Etiquetas, idioma);
                $("#diasDeporte").text(data.diasDeporte);
                $("#emocionMsg").text(data.emocion.Nombre);
                document.getElementById("emocionImg").src = "../Content/Img/Emociones/" + data.emocion.NombreEN + "_ic.svg";
                $("#suenyoMSg").text(data.suenyo.Nombre);
            } else {
                $("#contenidoMes").addClass('hidden');
                $("#noContenidoMes").removeClass('hidden');
            }
        },
    });
}

function crearGraficaProgreso(datos, categorias, idioma) {
    var options = {
        chart: {
            type: 'line'
        },
        stroke: {
            curve: 'smooth',
        },
        colors: ['#F44336'],
        series: [{
            name: 'kg',
            data: datos//[100.6, 100]
        }],
        xaxis: {
            categories: categorias,// ['1-6', 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31],
            title: {
                text: idioma == 'es'?'Días mes':'Month days',
            },
        },
        yaxis: [{
            title: {
                text: idioma == 'es' ?'Evolución peso (kg)':'Weight progress (kg)',
            },
        }]
    };

    var chart = new ApexCharts(document.querySelector("#graficaProgreso"), options);

    chart.render();
}