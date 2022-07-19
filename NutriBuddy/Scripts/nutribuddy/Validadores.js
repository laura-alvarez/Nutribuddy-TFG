function validarSoloLetras(text) {
    return /^(([a-zA-Z]|(á|é|í|ó|ú|Á|É|Í|Ó|Ú|ñ|Ñ|ç|Ç|ü|Ü)) ?)+$/g.test(text);    
}

function validarSoloNumeros(text) {
    return /^(([0-9]) ?)+$/g.test(text);
}

function validarEmail(text) {
    return /^(\w|\d|\.)+@(\w|\d)+\.(\w|\d)+(\.(\w|\d)+)*$/g.test(text);
}

function validadorCIF(text) {
    return /^[A-Z]{1}[0-9]{8}$/g.test(text);   
}

function validadorDNI(text) {
    return /^[0-9]{8}[A-Z]{1}$/g.test(text);
}

