function validarGuardado() {
    let respuesta = "";
    if ($("#idsanction_id").val().trim() == "")
        respuesta += "\n{sanction_id}";
    if ($("#description").val().trim() == "")
        respuesta += "\n{description}";
    if ($("#sanction_type").val().trim() == "")
        respuesta += "\n{sanction_type}";
    if ($("#cost").val().trim() == "")
        respuesta += "\n{cost}";
    if ($("#created_at").val().trim() == "")
        respuesta += "\n{created_at}";

    if (respuesta != "")
        alert("Los siguientes campos son obligatorios: " + respuesta);
}
