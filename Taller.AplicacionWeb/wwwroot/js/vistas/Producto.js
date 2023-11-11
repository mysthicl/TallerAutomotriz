const modeloBase = {
    idProducto: 0,
    codigoProducto: "",
    nombre: "",
    descripcion: "",
    precio: 0,
    cantidadEnStock: 0,
    urlImagen: "",
    ganancia: 0
}

let tabladata;

$(document).ready(function () {


    tabladata = $('#tbdata').DataTable({
        responsive: true,
        "ajax": {
            "url": '/Producto/Lista',
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "idProducto", "visible": false, "searchable": false },
            {
                "data": "urlImagen", render: function (data) {
                    return `<img style="height:60px" src=${data} class="rounded mx-auto d-block"/>`
                }
            },
            { "data": "codigoProducto" },
            { "data": "nombre" },
            { "data": "descripcion" },
            { "data": "precio" },
            { "data":"ganancia" },
            { "data": "cantidadEnStock" },
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
    $("#txtId").val(modelo.idProducto)
    $("#txtCodigoProducto").val(modelo.codigoProducto)
    $("#txtNombre").val(modelo.nombre)
    $("#txtDescripcion").val(modelo.descripcion)
    $("#txtPrecio").val(modelo.precio)
    const gananciaSinPorcentaje = modelo.ganancia.replace("%", "");
    $("#txtGanancia").val(gananciaSinPorcentaje);
    $("#txtStock").val(modelo.cantidadEnStock)
    $("#imgProducto").attr("src", modelo.urlImagen)
    $("#txtFoto").val("")
    $("#modalData").modal("show")
}

$("#btnRegistro").click(function () {
    mostrarModal()
})



$("#btnAgregar").click(function () {
    limpiarCamposProducto()
})

function limpiarCamposProducto() {
    $("#txtId").val(0)
        $("#txtCodigoProducto").val("")
        $("#txtNombre").val("")
        $("#txtDescripcion").val("")
        $("#txtPrecio").val("")
        $("#txtGanancia").val("");
        $("#txtStock").val("")
        $("#imgProducto").attr("src", "https://cdn-icons-png.flaticon.com/512/1514/1514380.png")
        $("#txtFoto").val("")
        $("#modalData").modal("show")
    }


$("#btnGuardar").click(function () {

    const modelo = structuredClone(modeloBase)
    modelo["idProducto"] = parseInt($("#txtId").val())
    modelo["codigoProducto"] = $("#txtCodigoProducto").val()
    modelo["nombre"] = $("#txtNombre").val()
    modelo["descripcion"] = $("#txtDescripcion").val()
    modelo["precio"] = $("#txtPrecio").val()
    modelo["ganancia"]=$("#txtGanancia").val()
    modelo["cantidadEnStock"] = $("#txtStock").val()

    var ganancia = parseInt(modelo["ganancia"])
    

    if (ganancia <= 0 || ganancia > 100 || isNaN(ganancia)) {
        toastr.warning("", "La ganancia no debe ser menor a 0 o mayor a 100");
        return
    }

    console.log(modelo);

    const inputFoto = document.getElementById("txtImagen")

    const formData = new FormData();

    formData.append("imagen", inputFoto.files[0])
    formData.append("modelo", JSON.stringify(modelo))

    $("#modalData").find("div.modal-content").LoadingOverlay("show");

    if (modelo.idProducto == 0) {
        fetch("/Producto/Crear", {
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
                    swal("Listo", "El producto fue agregado con exito", "success")
                } else {
                    swal("Lo sentimos", responseJson.mensaje, "error")
                }
            })
    } else {
        fetch("/Producto/Editar", {
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
                    swal("Listo", "El producto fue editado", "success")
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
        text: `Eliminar el producto "${data.nombre}"`,
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

            fetch(`/Producto/Eliminar?IdProducto=${data.idProducto}`, {
                method: "DELETE"
            })
                .then(response => {
                    $(".showSweetAlert").LoadingOverlay("hide");
                    return response.ok ? response.json() : Promise.reject(response)
                })
                .then(responseJson => {
                    if (responseJson.estado == true) {

                        tabladata.row(filaSelect).remove().draw();
                        swal("Listo", "El producto fue eliminado", "success")

                    } else {
                        swal("Lo sentimos", "no se pudo eliminar el usuario por estar relacionado en otra tabla >:v", "error")

                    }
                })

        }
    })
})
