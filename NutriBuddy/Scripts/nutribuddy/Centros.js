var IdCentro = 0;
var PrevNPlan = -1;
$(document).ready(function () {
    var url = window.location.href;
    if (url.includes('ZonaPrivada/Home') || url.includes('ZonaPrivada?menu=6')) {
        getCentros();
        $('#anyadirCentro').on('click', function () {
            window.location.href = '/Centros/FichaCentro?id=0';
        });
    }
    if (url.includes('FichaCentro')) {
        PrevNPlan = $("#plan").val();
        $('#plan').on('change', function () {
            if (checkTrabajadoresActuales()) {
                PrevNPlan = $("#plan").val();
                changeMaxTrabajadores();
            } else {
                //ERROR Y NO CAMBIA
                $("#plan").val(PrevNPlan);
                Swal.fire({
                    icon: 'error',
                    title: 'Oops...',
                    text: 'No puedes elegir el plan porque tienes más trabajadores de los permitidos, da de baja algún trabajador antes de cambiar el plan'                    
                })
            }
        });
        $('#puestosTC').select2({
            disabled: true
        });
        $('#puestosTC').val('1');
        $('#puestosTC').trigger('change');

        $('#guardarCentro').on('click', function () {
            guardarCentro();
        });

        $('#volverListado').on('click', function () {
            window.location.href = '/ZonaPrivada/ZonaPrivada?menu=6';
        });
        
    }
});

var estructuraTabla = [
    { data: 'ID' },
    { data: 'Nombre' },
    { data: 'Plan' },
    { data: 'Provincia' },
    { data: 'Email' },
    { data: 'Telefono' },
    { data: 'Responsable' },   
    {
        data: 'ID', orderable: false, render: 
        function(data, type, row, meta) {                
            return '<span id="editar_' + data + '" title="Editar centro" onclick="editarCentro(' + data + ')"><i class="fa-solid fa-pencil editar iconoTabla" ></i ></span>'+
                '<span id="eliminar_' + data + '" title="Eliminar centro" onclick="eliminarCentro(' + data +')"><i class="fa-solid fa-trash-can eliminar iconoTabla"></i></span> ';
        }
        
    },
];

function getCentros() {  
    $.ajax({
        url: '/Centros/GetCentros',
        type: "GET",
        async: false,
        success: function (data) {
            iniciarTabla('#tablaCentros', estructuraTabla, data.Centros);
        },
    });
}

function checkTrabajadoresActuales(idPlanNew) {
    var IDCentro = $("#idCentro").val();
    var nTrabajadores = 0;
    $.ajax({
        url: '/Centros/GetTrabajadoresActuales?idCentro=' + IDCentro + '&idPlan=' + idPlanNew,
        type: "GET",
        async: false,
        success: function (data) {
            nTrabajadores = data.resta;
        },
    });
    return (nTrabajadores >= 0);    
}

function changeMaxTrabajadores() {
    var idPlan = $("#plan").val();
    if (idPlan === "4") {
        $("#maxTrabajadores").attr('disabled', false);
        $("#maxTrabajadores").attr('max', 100);
        $("#maxTrabajadores").val(100);
    } else if (idPlan === "2") {
        $("#maxTrabajadores").attr('max', 10);
        $("#maxTrabajadores").val(10);
        $("#maxTrabajadores").attr('disabled', true);
    } else if (idPlan === "1") {
        $("#maxTrabajadores").attr('max', 1);
        $("#maxTrabajadores").val(1);
        $("#maxTrabajadores").attr('disabled', true);
    } else {
        $("#maxTrabajadores").val(0);
        $("#maxTrabajadores").attr('disabled', true);
    }
}

function guardarCentro() {
    if (Validador()) {
        var formDataCentro = new FormData();
        formDataCentro.append("IDCentro", $("#idCentro").val());
        formDataCentro.append("Nombre", $("#nombre").val());
        formDataCentro.append("CIF", $("#CIF").val());
        formDataCentro.append("Descripcion", $("#descripcion").val());
        formDataCentro.append("IDPlan", $("#plan").val());
        formDataCentro.append("MaxTrabajadores", $("#maxTrabajadores").val());
        formDataCentro.append("IDProvincia", $("#provincia").val());
        formDataCentro.append("Direccion", $("#direccion").val());
        formDataCentro.append("Localidad", $("#localidad").val());
        formDataCentro.append("CP", $("#cp").val());
        formDataCentro.append("Telefono", $("#telefono").val());
        formDataCentro.append("Email", $("#centroEmail").val());
        formDataCentro.append("Website", $("#website").val());
        formDataCentro.append("Consultation_F2F", $('#presencial').is(":checked"));
        formDataCentro.append("Consultation_Online", $('#online').is(":checked"));
        formDataCentro.append("NombreR", $("#NombreTC").val());
        formDataCentro.append("ApellidosR", $("#ApellidosTC").val());
        formDataCentro.append("SSR", $("#SSR").val());
        formDataCentro.append("DNIR", $("#DNITC").val());
        formDataCentro.append("NacionalidadR", $("#NacionalidadTC").val());
        formDataCentro.append("IDProvinciaR", $("#ProvinciaTC").val());
        formDataCentro.append("LocalidadR", $("#LocalidadTC").val());
        formDataCentro.append("EmailR", $("#EmailTC").val());
        formDataCentro.append("TelefonoR", $("#TelefonoTC").val());

        $.ajax({
            url: '/Centros/GuardarCentro',
            type: "POST",
            data: formDataCentro,
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
                    window.location.href = '/ZonaPrivada/ZonaPrivada?menu=6';
                });
            },
            error: function (xhr, status, p3, p4) {
                alert("error");
            }
        });
    } else {
        Swal.fire({
            title: 'Por favor, completa todos los campos',
            icon: 'error',
            iconColor: '#f2233a',
            backdrop: 'true',
            customClass: {
                confirmButton: 'btnLO',
            }
        });
    }
}

function eliminarCentro(idCentro) {
    Swal.fire({
        title: '¿Seguro que quieres dar de baja el centro?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Sí',
        cancelButtonText: 'No',
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/Centros/EliminarCentro?id=' + idCentro,
                type: 'DELETE',
                success: function (data) {
                    Swal.fire({
                        title: data.msg,
                        icon: 'success',
                        iconColor: '#f2233a',
                        backdrop: 'true',
                        customClass: {
                            confirmButton: 'btnLO',
                        }
                    }).then((result) => {
                        location.reload();
                    });
                }
            });
        }
    })
}

function editarCentro(id) {
    window.location.href = '/Centros/FichaCentro?id='+id;
}

function Validador() {
    let correcto = true;
    if (!validarSoloLetras($("#nombre").val())) {
        backgroundInputValidated("#nombre");
        correcto = false;
    }
    if (!validadorCIF($("#CIF").val())) {
        backgroundInputValidated("#CIF");
        correcto = false;
    }
    if (!validarSoloLetras($("#localidad").val())) {
        backgroundInputValidated("#localidad");
        correcto = false;
    }
    let aux = $("#cp").val();
    if (!validarSoloNumeros(aux) || aux.length > 5 || aux.length<0) {
        backgroundInputValidated("#cp");
        correcto = false;
    }
    aux = $("#telefono").val();
    if (!validarSoloNumeros(aux) || aux.length > 9 || aux.length < 0) {
        backgroundInputValidated("#telefono");
        correcto = false;
    }
    if (!validarEmail($("#centroEmail").val())) {
        backgroundInputValidated("#centroEmail");
        correcto = false;
    }
    if (!$('#presencial').is(":checked") && !$('#online').is(":checked")) {
        correcto = false;
    }
    if (!validarSoloLetras($("#NombreTC").val())) {
        backgroundInputValidated("#NombreTC");
        correcto = false;
    }
    if (!validarSoloLetras($("#ApellidosTC").val())) {
        backgroundInputValidated("#ApellidosTC");
        correcto = false;
    }
    aux = $("#SSR").val();
    if (!validarSoloNumeros(aux) || aux.length > 12 || aux.length < 0) {
        backgroundInputValidated("#SSR");
        correcto = false;
    }
    aux = $("#DNITC").val();
    if (!validadorDNI(aux) || aux.length > 9 || aux.length < 0) {
        backgroundInputValidated("#DNITC");
        correcto = false;
    }
    if (!validarSoloLetras($("#NacionalidadTC").val())) {
        backgroundInputValidated("#NacionalidadTC");
        correcto = false;
    }
    if ($("#ProvinciaTC").val() == -1) {
        backgroundInputValidated("#ProvinciaTC");
        correcto = false;
    }
    if (!validarSoloLetras($("#LocalidadTC").val())) {
        backgroundInputValidated("#LocalidadTC");
        correcto = false;
    }
    if (!validarEmail($("#EmailTC").val())) {
        backgroundInputValidated("#EmailTC");
        correcto = false;
    }
    aux = $("#TelefonoTC").val();
    if (!validarSoloNumeros(aux) || aux.length > 9 || aux.length < 0) {
        backgroundInputValidated("#TelefonoTC");
        correcto = false;
    }
    return correcto;
}

function backgroundInputValidated(id) {
    $(id).css("background-color", "#dd8b85");
}

