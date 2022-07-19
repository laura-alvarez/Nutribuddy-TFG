$(document).ready(function () {
    $("#citaModal").prependTo("body");

    inicializarModalCita();

    getCitasCalendario();

    $("#guardarCita").on('click', function () { guardarCita(); });

    $('#anyadirCita').on('click', function () {
        $("#guardarCita").removeClass("hidden");
        $("#guardarCita").attr('name', '0');
        $("#TipoCita").removeAttr('disabled');
        inicializarModalCita();
        $("#editarCita").addClass('hidden');
        $("#eliminarCita").addClass('hidden');
        $('#citaModal').modal();
    });

    $("#editarCita").on('click', function (e) {
        editarCita(e.currentTarget.name);
    });

    $('#eliminarCita').on('click', function (e) {
        Swal.fire({
            title: 'Are you sure?',
            text: "You won't be able to revert this!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it!'
        }).then((result) => {
            if (result.isConfirmed) {
                eliminarCita(e.currentTarget.name);
            }
        })
       
    });

    $("#comenzarCita").on('click', function (e) {
        window.location.href = '/Agenda/ComenzarCita?idCita=' + e.currentTarget.name;
    });
});

function getCitasCalendario() {
    $.ajax({
        url: "/Agenda/GetCalendarioCitas",
        type: "POST",
        data: {
            idTrabajador: 1
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert("error");
        },
        success: function (jsondata, status, xhr) {
            cargarCitasEnCalendario(jsondata.events); 
        },
        dataType: "json"
    });
}
function cargarCitasEnCalendario(data) {
    var language = getIdioma();
    new FullCalendar.Calendar(document.getElementById('calendario'), {
        headerToolbar: { 
            left: 'prev next today',
            center: 'title',
            right: 'dayGridMonth dayGridWeek listDay'
        },
        initialView: 'dayGridMonth',
        locale: language,
        height: 600,
        events: data,
        eventColor: '#378006',
        eventClick: function (info) {
            $('#citaModal').modal();
            inicializarModalCita();
            $.ajax({
                url: "/Agenda/GetDetalleCita?idCita=" + info.event.id,
                type: "GET",
                async: false,
                error: function (XHR, textStatus, errorThrown) {
                    alert("error");
                },
                success: function (data) {
                    if (data.flag == 0) {
                        var dateSplit = data.cita.Dia.split('-');
                        var fechaCita = new Date(dateSplit[0], parseInt(dateSplit[1])-1, dateSplit[2]);
                        $("#TipoCita").attr('disabled', true);
                        $('#TipoCita option[value="' + data.cita.IDTipo + '"]').prop('selected', true);
                        getTrabajadores(data.cita.IDTipo);
                        $('#CitaTrabajador option[value="' + data.cita.IDTrabajador + '"]').prop('selected', true);
                        getPacientes(data.cita.IDTrabajador);
                        $('#CitaPaciente option[value="' + data.cita.IDPaciente + '"]').prop('selected', true);
                        $("#modalTitle2").html(data.cita.Titulo);   
                        $("#CitaTitulo").val(data.cita.Titulo);
                        $("#CitaDia").val(data.cita.Dia);
                        $("#CitaHoraInicio").val(data.cita.HoraInicio);
                        $("#CitaHoraFin").val(data.cita.HoraFin);
                        $("#CitaObservaciones").val(data.cita.Observaciones);
                        $("#guardarCita").addClass("hidden");
                        var aux = new Date();
                        if (fechaCita >= aux && data.PuedeAdministrar == true) {
                            $("#editarCita").removeClass('hidden');
                            $("#editarCita").attr('name', data.cita.ID);
                            $("#eliminarCita").removeClass('hidden');
                            $("#eliminarCita").attr('name', data.cita.ID);
                        } else {
                            $("#editarCita").addClass('hidden');
                            $("#eliminarCita").addClass('hidden');
                        }
                        if (data.PuedeComenzar == true) {
                            $("#comenzarCita").removeClass('hidden');
                            $("#comenzarCita").attr('name', data.cita.ID);
                        } else {
                            $("#comenzarCita").addClass('hidden');
                        }
                        
                    }
                }
            });
            
        },

    }).render();
};

function inicializarModalCita() {
    var idioma = getIdioma();
    $("#modalTitle2").html(idioma == 'es' ? 'Añadir cita' :'Add appointment');
    $("#TipoCita").val('-1');
    $("#CitaTrabajador").val('-1');
    $("#CitaPaciente").val('-1');
    $("#CitaTitulo").val('');
    $("#CitaDia").val('');
    $("#CitaHoraInicio").val('');
    $("#CitaHoraFin").val('');
    $("#CitaObservaciones").val('');

    $("#CitaTrabajador").attr('disabled', true);
    $("#CitaPaciente").attr('disabled', true);
    $("#CitaTitulo").attr('disabled', true);
    $("#CitaDia").attr('disabled', true);
    $("#CitaHoraInicio").attr('disabled', true);
    $("#CitaHoraFin").attr('disabled', true);
    $("#CitaObservaciones").attr('disabled', true);

    $("#TipoCita").on('change', function (e) {
        e.stopImmediatePropagation();
        var value = $("#TipoCita").val();
        if (value > 0) {
            getTrabajadores(value);
            $("#CitaTrabajador").removeAttr('disabled');
        }
    });
    $("#CitaTrabajador").on('change', function (e) {
        e.stopImmediatePropagation();
        var value = $("#CitaTrabajador").val();
        if (value > 0) {
            getPacientes(value);
            $("#CitaPaciente").removeAttr('disabled');
            $("#CitaTitulo").removeAttr('disabled');;
            $("#CitaDia").removeAttr('disabled');;
            $("#CitaHoraInicio").removeAttr('disabled');;
            $("#CitaHoraFin").removeAttr('disabled');;
            $("#CitaObservaciones").removeAttr('disabled');;
        }
    });  
}

function getTrabajadores(idPuesto) {
    $("#CitaTrabajador").empty();
    $("#CitaTrabajador").append('<option value="-1">--</option>');
    if (idPuesto != -1) {
        $.ajax({
            url: "/Agenda/GetTrabajadoresPuesto?idPuesto=" + idPuesto,
            type: "GET",
            async: false,
            error: function (jqXHR, textStatus, errorThrown) {
                alert("error");
            },
            success: function (data) {
                for (var i = 0; i < data.trabajadores.length; i++) {
                    $("#CitaTrabajador").append('<option value=' + data.trabajadores[i].ID + '>' + data.trabajadores[i].Nombre + '</option>');
                }
            }
        });
    }
}

function getPacientes(idTrabajador) {
    $("#CitaPaciente").empty();
    $("#CitaPaciente").append('<option value="-1">--</option>');
    if (idTrabajador != -1) {
        $.ajax({
            url: "/Agenda/GetPacientesTrabajador?idTrabajador=" + idTrabajador,
            type: "GET",
            async: false,
            error: function (jqXHR, textStatus, errorThrown) {
                alert("error");
            },
            success: function (data) {
                for (var i = 0; i < data.pacientes.length; i++) {
                    $("#CitaPaciente").append('<option value=' + data.pacientes[i].ID + '>' + data.pacientes[i].Nombre + '</option>');
                }
            }
        });
    }
}

function guardarCita() {
    var fdCita = new FormData();
    fdCita.append("ID", $("#guardarCita").attr('name'));
    fdCita.append("IDTipo", $("#TipoCita").val());
    fdCita.append("IDTrabajador" ,$("#CitaTrabajador").val());
    fdCita.append("IDPaciente" ,$("#CitaPaciente").val());
    fdCita.append("Titulo" ,$("#CitaTitulo").val());
    fdCita.append("Dia" ,$("#CitaDia").val());
    fdCita.append("HoraInicio" ,$("#CitaHoraInicio").val());
    fdCita.append("HoraFin" ,$("#CitaHoraFin").val());
    fdCita.append("Observaciones", $("#CitaObservaciones").val());


    $.ajax({
        url: '/Agenda/GuardarCita',
        type: "POST",
        data: fdCita,
        processData: false,
        contentType: false,
        async: false,
        success: function (data) {
            Swal.fire({
                title: data.msg,
                icon: data.flag === 0 ? 'success' : 'error',
                iconColor: '#f2233a',
                backdrop: 'true',
                customClass: {
                    confirmButton: 'btnLO',
                }
            }).then((result) => {
                location.reload();
            });
        },
        error: function (xhr, status, p3, p4) {
            alert("error");
        }
    });
}

function validacionNuevaCita() {

}

function editarCita(idCita) {
    $("#CitaTitulo").removeAttr('disabled');
    $("#CitaDia").removeAttr('disabled');
    $("#CitaHoraInicio").removeAttr('disabled');
    $("#CitaHoraFin").removeAttr('disabled');
    $("#CitaObservaciones").removeAttr('disabled');
    $("#editarCita").addClass('hidden');
    $("#guardarCita").removeClass('hidden');
    $("#guardarCita").attr('name', idCita);
}

function eliminarCita(idCita) {

    $.ajax({
        url: '/Agenda/EliminarCita?idCita=' + idCita,
        type: "Delete",       
        async: false,
        success: function (data) {
            Swal.fire({
                title: data.msg,
                icon: data.flag === 0 ? 'success' : 'error',
                iconColor: '#f2233a',
                backdrop: 'true',
                customClass: {
                    confirmButton: 'btnLO',
                }
            }).then((result) => {
                location.reload();
            });
        },
        error: function (xhr, status, p3, p4) {
            alert("error");
        }
    });
}