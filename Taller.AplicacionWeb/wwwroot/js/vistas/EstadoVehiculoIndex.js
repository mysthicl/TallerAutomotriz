document.addEventListener("DOMContentLoaded", function () {
    const queryString = window.location.search;
    const urlParams = new URLSearchParams(queryString);
    const numberTracking = urlParams.get('numberTracking');

    fetch(`/EstadoVehiculo/Obtener?numberTracking=${numberTracking}`)
        .then(response => {
            return response.ok ? response.json() : Promise.reject(response)
        })
        .then(responseJson => {
            if (responseJson.estado) {
                const obj = responseJson.objeto

                if (obj.status === "Reparacion") {
                    document.querySelector("#descripcionReparacion").innerHTML = obj.descripcionDeLaReparacion;
                    const fecha = new Date(obj.fechaDeInicio);
                    document.querySelector("#descripcionReparacionFecha").innerHTML = fecha.toLocaleString();
                } else if (obj.status === "Revision") {
                    document.querySelector("#descripcionRevision").innerHTML = obj.descripcionDeLaReparacion;
                    document.querySelector("#descripcionRevisionFecha").innerHTML ='Fecha: '+ obj.fechaDeInicio;
                    
                } else if (obj.status === "Finalizado") {
                    document.querySelector("#descripcionFinalizado").innerHTML = obj.descripcionDeLaReparacion;
                    document.querySelector("#fechaFinalizado").innerHTML ='Fecha: '+ obj.fechaDeInicio;
                    
                }
            } else {
                swal("Lo sentimos", responseJson.mensaje, "error");
            }
        })
        .catch(error => {
            console.error(error);
        });
});
