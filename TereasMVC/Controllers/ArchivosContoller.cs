﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TereasMVC.Entidades;
using TereasMVC.Servicios;

namespace TereasMVC.Controllers
{
    [Route("api/archivos")]
    public class ArchivosContoller : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IAlmacenadorArchivos almacenadorArchivos;
        private readonly IServicioUsuario servicioUsuario;
        private readonly string contenedor = "archivosadjuntos";

        public ArchivosContoller(ApplicationDbContext context,
            IAlmacenadorArchivos almacenadorArchivos,
            IServicioUsuario servicioUsuario) 
        {
            this.context = context;
            this.almacenadorArchivos = almacenadorArchivos;
            this.servicioUsuario = servicioUsuario;
        }

        [HttpPost("{tareaId:int}")]
        public async Task<ActionResult<IEnumerable<ArchivoAdjunto>>> Post(int tareaId, 
            [FromForm] IEnumerable<IFormFile> archivos)
        {
            var usuarioId = servicioUsuario.ObtenerUsuarioId();

            var tarea = await context.Tareas.FirstOrDefaultAsync(t => t.Id == tareaId);

            if (tarea is null) 
            {
                return NotFound();
            }

            if (tarea.UsuarioCreacionId != usuarioId)
            {
                return Forbid();
            }

            var existenArchivosAdjuntos = await context.ArchivosAdjuntos.AnyAsync(a => a.TareaId == tareaId);

            var ordenMayor = 0;
            if (existenArchivosAdjuntos)
            {
                ordenMayor = await context.ArchivosAdjuntos
                    .Where(a => a.TareaId == tareaId).Select(a => a.Orden).MaxAsync();
            }

            var resultados = await almacenadorArchivos.Almacenar(contenedor, archivos);

            var archivosAdjuntos = resultados.Select((resultado, indice) => new ArchivoAdjunto
            {
                TareaId = tareaId,
                FechaCreacion = DateTime.UtcNow,
                Url = resultado.URL,
                Titulo = resultado.Titulo,
                Orden = ordenMayor + indice + 1
            }).ToList();

            context.AddRange(archivosAdjuntos);
            await context.SaveChangesAsync();

            return archivosAdjuntos.ToList();

        }
    }
}
