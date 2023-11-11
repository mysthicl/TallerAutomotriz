const modeloBase = {
    idCarro: 0,
    placa: "",
    marca: "",
    modelo: "",
    ano: ""
}

let tabladata;

$(document).ready(function () {


    tabladata = $('#tbdata').DataTable({
        responsive: true,
        "ajax": {
            "url": '/Vehiculo/Lista',
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "idCarro", "visible": false, "searchable": false },
            { "data": "placa"},
            { "data": "marca" },
            { "data": "modelo" },
            { "data": "ano" },
            {
                "defaultContent": '<button class="btn btn-primary btn-editar btn-sm mr-2"><i class="fas fa-pencil-alt"></i></button>' +
                    '<button class="btn btn-danger btn-eliminar btn-sm"><i class="fas fa-trash-alt"></i></button>',
                "orderable": false,
                "searchable": false,
                "width": "80px"
            }
        ],
        order: [[0, "desc"]],

        language: {
            url: "https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json"
        },


    });

})


function mostrarModal(modelo = modeloBase) {
    $("#txtId").val(modelo.idCarro)
    $("#txtPlaca").val(modelo.placa)
    $("#txtMarca").val(modelo.marca)
    $("#txtModelo").val(modelo.modelo)
    $("#txtAnio").val(modelo.ano)
    
    $("#modalData").modal("show")
}

$("#btnRegistro").click(function () {
    mostrarModal()
})


$('#btnAgregar').click(function (){
    limpiarCamposVehiculos();
})

function limpiarCamposVehiculos(){
    $("#txtId").val(0)
    $("#txtPlaca").val("")
    $("#txtMarca").val("")
    $("#txtModelo").val("")
    $("#txtAnio").val("")
    $("#modalData").modal("show")
}


$("#btnGuardar").click(function () {

    const modelo = structuredClone(modeloBase)
    modelo["idCarro"] = parseInt($("#txtId").val())
    modelo["placa"] = $("#txtPlaca").val()
    modelo["marca"] = $("#txtMarca").val()
    modelo["modelo"] = $("#txtModelo").val()
    modelo["ano"] = $("#txtAnio").val()


    console.log(modelo);



    const formData = new FormData();

    formData.append("modelo", JSON.stringify(modelo))

    $("#modalData").find("div.modal-content").LoadingOverlay("show");

    if (modelo.idCarro == 0) {
        fetch("/Vehiculo/Crear", {
            method: "POST",
            body: formData
        })
            .then(response => {
                $("#modalData").find("div.modal-content").LoadingOverlay("hide");
                return response.ok ? response.json() : Promise.reject(response)
            })
            .then(responseJson => {
                if (responseJson.estado == true) {

                    tabladata.row.add(responseJson.objeto).draw(false)
                    $("#modalData").modal("hide")
                    swal("Listo", "El vehiculo fue agregado con exito", "success")
                } else {
                    swal("Lo sentimos", responseJson.mensaje, "error")
                }
            })
    } else {
        fetch("/Vehiculo/Editar", {
            method: "PUT",
            body: formData
        })
            .then(response => {
                $("#modalData").find("div.modal-content").LoadingOverlay("hide");
                return response.ok ? response.json() : Promise.reject(response)
            })
            .then(responseJson => {
                if (responseJson.estado == true) {

                    tabladata.row(filaSelect).data(responseJson.objeto).draw(false);

                    filaSelect = null;
                    console.log(responseJson.objeto.idRol)
                    $("#modalData").modal("hide")
                    swal("Listo", "El vehiculo fue editado", "success")
                } else {
                    swal("Lo sentimos", responseJson.mensaje, "error")
                }
            })
    }

})


let filaSelect;

$("#tbdata tbody").on("click", ".btn-editar", function () {
    if ($(this).closest("tr").hasClass("child")) {
        filaSelect = $(this).closest("tr").prev();
    } else {
        filaSelect = $(this).closest("tr");
    }

    const data = tabladata.row(filaSelect).data();
    mostrarModal(data);
})


$("#tbdata tbody").on("click", ".btn-eliminar", function () {
    let fila;
    if ($(this).closest("tr").hasClass("child")) {
        filaSelect = $(this).closest("tr").prev();
    } else {
        filaSelect = $(this).closest("tr");
    }

    const data = tabladata.row(filaSelect).data();
    console.log(data.idProducto)
    swal({
        title: "Esta seguro?",
        text: `Eliminar el vehiculo "${data.placa}"`,
        type: "warning",
        showCancelButton: true,
        confirmButtonClass: "btn-danger",
        confirmButtonText: "Si, eliminar",
        camcelButtonText: "Cancelar",
        closeOnConfirm: false,
        closeOnCancel: true

    }, function (respuesta) {
        if (respuesta) {
            $(".showSweetAlert").LoadingOverlay("show");

            fetch(`/Vehiculo/Eliminar?IdProducto=${data.idCarro}`, {
                method: "DELETE"
            })
                .then(response => {
                    $(".showSweetAlert").LoadingOverlay("hide");
                    return response.ok ? response.json() : Promise.reject(response)
                })
                .then(responseJson => {
                    if (responseJson.estado == true) {

                        tabladata.row(filaSelect).remove().draw();
                        swal("Listo", "El vehiculo fue eliminado", "success")

                    } else {
                        swal("Lo sentimos", "no se pudo eliminar el usuario por estar relacionado en otra tabla >:v", "error")

                    }
                })

        }
    })
})


