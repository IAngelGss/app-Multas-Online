function validarGuardadoAgente() {
    let respuesta = "";
    if ($("#full_name").val().trim() == "")
        respuesta += "\n{full_name}";
    if ($("#id_number").val().trim() == "")
        respuesta += "\n{id_number}";
    if ($("#rank_level").val().trim() == "")
        respuesta += "\n{rank_level}";


    if (respuesta != "")
        alert("Los siguientes campos son obligatorios: " + respuesta);
}
