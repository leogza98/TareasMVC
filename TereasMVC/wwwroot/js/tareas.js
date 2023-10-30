﻿function agregarNuevaTareaAlListado() {
    tareaListadoViewModel.tareas.push(new tareaElementoListadoViewModel({ id: 0, titulo: '' }));

    $("[name=titulo-tarea]").last().focus();

}

 async function manejarFocusoutTituloTarea(tarea) {
    const titulo = tarea.titulo();
    if (!titulo) {
        tareaListadoViewModel.tareas.pop();
        return;
    }

     const data = JSON.stringify(titulo);
     const respuesta = await fetch(urlTareas, {
         method: 'POST',
         body: data,
         headers: {
             'Content-Type': 'application/json'
         }
     });

     if (respuesta.ok) {
         const json = await respuesta.json();
         tarea.id(json.id);
     }
     else {

     }

}