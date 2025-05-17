function validarGuardadoVehiculo() {
    let respuesta = "";
    if ($("#license_plate").val().trim() == "")
        respuesta += "\n{license_plate}";
    if ($("#brand").val().trim() == "")
        respuesta += "\n{brand}";
    if ($("#model").val().trim() == "")
        respuesta += "\n{model}";
    if ($("#color").val().trim() == "")
        respuesta += "\n{color}";
    if ($("#vehicle_type").val().trim() == "")
        respuesta += "\n{vehicle_type}";

    if (respuesta != "")
        alert("Los siguientes campos son obligatorios: " + respuesta);
}
