const modeloBase = {
    idUsuario: 0,
    nombreDeUsuario: "",
    contrasena: "",
    idRol: 0,
    urlFoto: "",
    FechaRegistro:"",
}
let tabladata;

$(document).ready(function () {

    fetch("/Usuario/ListaRoles")
        .then(response => {
            return response.ok ? response.json() : Promise.reject(response)
        })
        .then(responseJson => {
            if (responseJson.length > 0) {
                responseJson.forEach((item) => {
                    $("#cboRol").append(
                        $("<option>").val(item.idRol).text(item.nombreRol)
                    )
                })


            }
        })

    tabladata = $('#tbdata').DataTable({
        responsive: true,
        "ajax": {
            "url": '/Usuario/Lista',
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "idUsuario", "visible": false, "searchable": false },
            {
                "data": "urlFoto", render: function (data) {
                    return `<img style="height:60px" src=${data} class="rounded mx-auto d-block"/>`
                }
            },
            { "data": "nombreDeUsuario" },
            { "data": "nombreRol" },
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
    $("#txtId").val(modelo.idUsuario)
    $("#txtNombre").val(modelo.nombreDeUsuario)
    $("#txtContra").val(modelo.contrasena)
    $("#cboRol").val(modelo.idRol == 0 ? $("#cboRol option:first").val() : modelo.idRol)
    if (modelo.urlFoto === "") {
        $("#imgUsuario").attr("src", "../img/auto5.png")
    } else {
        $("#imgUsuario").attr("src", modelo.urlFoto)
    }

    $("#txtFoto").val("")
    $("#modalData").modal("show")
}

function limpiarCampos() {
    $("#txtId").val("0");
    $("#txtNombre").val("");
    $("#txtContra").val("");
    $("#cboRol").val("");
    $("#imgUsuario").attr("src", "https://cdn-icons-png.flaticon.com/512/6073/6073873.png");
    $("#txtFoto").val("");
}

$("#btnCancelar").click(function () {
    limpiarCampos();
    $("#modalData").modal("hide");
});

$("#btnClose").click(function () {
    limpiarCampos();
    $("#modalData").modal("hide");
});
$("#btnRegistro").click(function () {
    limpiarCampos();
    mostrarModal()
})


$("#btnGuardar").click(function () {


    const inputs = $("input.input-validar, select.input-validar").serializeArray();

    var inputs_sin_valor = inputs.filter(function (item) {
        return item.value === "" || item.value === null;
    });

    var select_sin_valor = $('select.input-validar').filter(function () {
        return this.value === "";
    });

    if (inputs_sin_valor.length > 0 || select_sin_valor.length > 0) {
        var mensajeError = "";

        if (inputs_sin_valor.length > 0) {
            mensajeError += `Debe completar el campo "${inputs_sin_valor[0].name}"\n`;
        }

        if (select_sin_valor.length > 0) {
            mensajeError += `Debe seleccionar una opción en el campo Rol"\n`;
        }

        toastr.warning("", mensajeError);
        $(`[name="${inputs_sin_valor[0].name}"], [name="${select_sin_valor.attr('name')}"]`).focus();
        return;
    }


    const modelo = structuredClone(modeloBase)
    modelo["idUsuario"] = parseInt($("#txtId").val())
    modelo["nombreDeUsuario"] = $("#txtNombre").val()
    modelo["contrasena"] = $("#txtContra").val()
    modelo["idRol"] = $("#cboRol").val()
    modelo["fechaRegistro"] = new Date();

    console.log(modelo);

    const inputFoto = document.getElementById("txtFoto")

    const formData = new FormData();

    formData.append("foto", inputFoto.files[0])
    formData.append("modelo", JSON.stringify(modelo))

    $("#modalData").find("div.modal-content").LoadingOverlay("show");

    if (modelo.idUsuario == 0) {


        fetch("/Usuario/Crear", {
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
                    swal("Listo", "El usuario fue creado", "success")
                } else {
                    swal("Lo sentimos", responseJson.mensaje, "error")
                }
            })
    } else {
        fetch("/Usuario/Editar", {
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
                    swal("Listo", "El usuario fue editado", "success")
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
    console.log(data.idUsuario)
    swal({
        title: "Esta seguro?",
        text: `Eliminar al usuario "${data.nombreDeUsuario}"`,
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

            fetch(`/Usuario/Eliminar?IdUsuario=${data.idUsuario}`, {
                method: "DELETE"
            })
                .then(response => {
                    $(".showSweetAlert").LoadingOverlay("hide");
                    return response.ok ? response.json() : Promise.reject(response)
                })
                .then(responseJson => {
                    if (responseJson.estado == true) {

                        tabladata.row(filaSelect).remove().draw();
                        swal("Listo", "El usuario fue eliminado", "success")

                    } else {
                        swal("Lo sentimos", "no se pudo eliminar el usuario por estar relacionado en otra tabla >:v", "error")

                    }
                })

        }
    })
})
