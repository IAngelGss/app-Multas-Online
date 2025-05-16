function validarGuardadoAgente() {
    let respuesta = "";
    if ($("#officer_id").val().trim() == "")
        respuesta += "\n{officer_id}";
    if ($("#full_name").val().trim() == "")
        respuesta += "\n{full_name}";
    if ($("#id_number").val().trim() == "")
        respuesta += "\n{id_number}";
    if ($("#rank_level").val().trim() == "")
        respuesta += "\n{rank_level}";
    if ($("#created_at").val().trim() == "")
        respuesta += "\n{created_at}";

    if (respuesta != "")
        alert("Los siguientes campos son obligatorios: " + respuesta);
}
