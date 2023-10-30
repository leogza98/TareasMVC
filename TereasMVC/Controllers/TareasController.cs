using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TereasMVC.Entidades;
using TereasMVC.Servicios;

namespace TereasMVC.Controllers
{
    [Route("api/tareas")]
    public class TareasController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IServicioUsuario servicioUsuario;

        public TareasController(ApplicationDbContext context,IServicioUsuario servicioUsuario)
        {
            this.context = context;
            this.servicioUsuario = servicioUsuario;
        }

        [HttpPost]
        public async Task<ActionResult<Tarea>> Post([FromBody] string titulo)
        {
            var usuarioId = servicioUsuario.ObtenerUsuarioId();

            var existenTareas = await context.Tareas.AnyAsync(t => t.UsuarioCreacionId == usuarioId);

            var ordenMayor = 0;
            if (existenTareas)
            {
                ordenMayor = await context.Tareas.Where(t => t.UsuarioCreacionId == usuarioId).Select(t => t.Orden).MaxAsync();
            }

            var tarea = new Tarea
            {
                Titulo = titulo,
                UsuarioCreacionId = usuarioId,
                FechaCreacion = DateTime.UtcNow,
                Orden = ordenMayor + 1
            };

            context.Add(tarea);
            await context.SaveChangesAsync();

            return tarea;
        }

    }
}
