using System.Security.Claims;

namespace TereasMVC.Servicios
{
    public interface IServicioUsuario
    {
        string ObtenerUsuarioId();
    }

    public class ServicioUsuario : IServicioUsuario
    {
        private HttpContext httpContext;

        public ServicioUsuario(IHttpContextAccessor httpContextAccessor)
        {
            httpContext = httpContextAccessor.HttpContext;
        }

        public string ObtenerUsuarioId()
        {
            if (httpContext.User.Identity.IsAuthenticated) 
            {
                var idClaim = httpContext.User.Claims
                    .Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault();

                return idClaim.Value;
            }
            else
            {
                throw new Exception("El usuario no esta autenticado");
            }
        }
    }
}
