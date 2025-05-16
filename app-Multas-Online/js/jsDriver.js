function validarGuardadoConductor() {
    let respuesta = "";
    if ($("#driver_id").val().trim() == "")
        respuesta += "\n{driver_id}";
    if ($("#full_name").val().trim() == "")
        respuesta += "\n{full_name}";
    if ($("#id_number").val().trim() == "")
        respuesta += "\n{id_number}";
    if ($("#address").val().trim() == "")
        respuesta += "\n{address}";
    if ($("#phone").val().trim() == "")
        respuesta += "\n{phone}";
    if ($("#license_number").val().trim() == "")
        respuesta += "\n{license_number}";
    if ($("#registration_date").val().trim() == "")
        respuesta += "\n{registration_date}";

    if (respuesta != "")
        alert("Los siguientes campos son obligatorios: " + respuesta);
}
