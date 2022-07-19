$(document).ready(function () {
    cargarCitas();
    
});

function cargarCitas() {
    $.ajax({
        url: "/Paciente/GetCitasPaciente",
        type: "GET",
        async: false,
        error: function (XHR, textStatus, errorThrown) {
            alert("error");
        },
        success: function (data) {
            cargarCitasEnCalendario(data.citas);
        }
    });
}

function cargarCitasEnCalendario(data) {
    var language = getIdioma();
    new FullCalendar.Calendar(document.getElementById('calendario'), {
        headerToolbar: {
            left: 'prev next today',
            center: 'title',
            right: ''
        },
        initialView: 'dayGridMonth',
        locale: language,
        height: 600,
        events: data,
        //eventColor: '#378006',
        eventClick: function (info) {
            
                
                 $.ajax({
                url: "/Paciente/GetDetalleCita?idCita=" + info.event.id,
                type: "GET",
                async: false,
                error: function (XHR, textStatus, errorThrown) {
                    alert("error");
                },
                success: function (data) {
                    if (data.flag == 0) {                        
                        inicializarModalCita();
                        data.completada ? $("#PautasDiv").removeClass("hidden") : $("#PautasDiv").addClass("hidden");
                        $('#citaModal').modal();                    
                        $('#TipoCita').text(data.cita.Tipo);
                        $('#CitaTrabajador').text(data.cita.Trabajador);
                        $("#CitaDia").text(data.cita.Dia);
                        $("#Pautas").text(data.cita.Pautas);
                    }
                }
            });           
        },
    }).render();
};

function inicializarModalCita() {
    var idioma = getIdioma();   
    $("#TipoCita").text('');
    $("#CitaTrabajador").text('');
    $("#CitaDia").text('');
    $("#Pautas").text('');
   

    $("#TipoCita").attr('disabled', true);
    $("#CitaTrabajador").attr('disabled', true);
    $("#CitaDia").attr('disabled', true);
    $("#Pautas").attr('disabled', true);

   
}
