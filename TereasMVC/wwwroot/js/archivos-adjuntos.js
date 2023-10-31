let inputArchivoTarea = document.getElementById('archivoATarea');

function manejarClickArchivoAdjunto() {
    inputArchivoTarea.click();
}

async function manejarSeleccionArchivoTarea(event) {
    const archivos = event.target.files;
    const archivosArreglo = Array.from(archivos);

    const idTarea = tareaEditarVM.id;
    const forData = new FormData();
    for (var i = 0; i < archivosArreglo.length; i++) {
        forData.append("archivos", archivosArreglo[i]);
    }

    const respuesta = await fetch(`${urlArchivos}/${idTarea}`, {
        body: forData,
        method: 'POST'
    });

    if (!respuesta.ok) {
        manejarErrorApi(respuesta);
        return;
    }

    const json = await respuesta..json();
    console.log(json);

    inputArchivoTarea.value = null;
}

function prepararArchivosAdjuntos(archivosAdjuntos) {
    archivosAdjuntos.forEach(archivoAdjunto => {
        let fechaCreacion = archivoAdjunto.fechaCreacion;
        if (archivoAdjunto.fechaCreacion.indexOf('Z') === -1) {
            fechaCreacion += 'Z';
        }

        const fechaCreacionDT = new Date(fechaCreacion);
        archivoAdjunto.publicado = fechaCreacionDT.toLocaleString();

        tareaEditarVM.archivosAdjuntos.push(new archivoAdjuntoViewModel({ ...archivoAdjunto, modoEdicion: false }));
    });
}