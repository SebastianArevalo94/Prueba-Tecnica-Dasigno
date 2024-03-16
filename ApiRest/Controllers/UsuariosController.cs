using ApiRest.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ApiRest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {

        private readonly ApiRestContext _context;

        public UsuariosController(ApiRestContext context)
        {
            _context = context;
        }

        //Obtener todos los usuarios
        [HttpPost]
        [Route("GetUsers")]
        public IActionResult GetUsers([FromBody] Paginacion paginacion)
        {
            try
            {
                var users = _context.Usuarios.ToList();

                if (paginacion.NumeroPagina == 0 || paginacion.RegistrosPorPagina == 0)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new { message = $"La configuracion de la paginacion no puede estar en 0" });
                }

                int totalPaginas = (int)Math.Ceiling((double)_context.Usuarios.Count() / paginacion.RegistrosPorPagina);

                //En caso de que el numero de pagina solicitado sea menor o igual a 0, o el numero de pagina sea mayor al total de paginas, es decir una pagina inexistente, retornar error
                if (paginacion.NumeroPagina <= 0 || paginacion.NumeroPagina > totalPaginas)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new { message = $"Pagina incorrecta, para {paginacion.RegistrosPorPagina} registros por pagina, existen {totalPaginas} paginas." });
                }

                var pages = users.Skip((paginacion.NumeroPagina - 1) * paginacion.RegistrosPorPagina).Take(paginacion.RegistrosPorPagina).ToList();

                return StatusCode(StatusCodes.Status200OK, new
                {
                    message = "Success GetUsers!",
                    data = pages
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }

        }

        //Obtener usuario por id
        [HttpGet]
        [Route("GetUserById/{Id:int}")]
        public IActionResult GetUserById(int Id)
        {
            try
            {
                var user = _context.Usuarios.Find(Id);

                if (user == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new { message = "User not found!" });
                }

                return StatusCode(StatusCodes.Status200OK, new
                {
                    message = "Success GetUserById!",
                    data = user
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }

        }

        //Crear usuario
        [HttpPost]
        [Route("CreateUser")]
        public IActionResult CreateUser([FromBody] Usuario usuario)
        {
            try
            {
                //Validacion de campos

                if (usuario.PrimerNombre.IsNullOrEmpty())
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new { message = "El Primer Nombre es obligatorio" });
                }
                else if (usuario.PrimerApellido.IsNullOrEmpty())
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new { message = "El Primer Apellido es obligatorio" });
                }
                else if (usuario.Sueldo == 0)
                {
                    return StatusCode(StatusCodes.Status403Forbidden, new { message = "El Sueldo no puede ser 0" });
                }

                _context.Usuarios.Add(usuario);
                _context.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new
                {
                    message = "Success Create User",
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }


        //Obtener Usuario por primer nombre
        [HttpPost]
        [Route("GetByFirstName/{FirstName}")]
        public IActionResult GetByFirstName(string FirstName, [FromBody] Paginacion paginacion)
        {
            try
            {
                var users = _context.Usuarios.Where(user => user.PrimerNombre.ToLower().Contains(FirstName.ToLower())).ToList();

                if (paginacion.NumeroPagina == 0 || paginacion.RegistrosPorPagina == 0)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new { message = "La configuracion de la paginacion no puede estar en 0" });
                }

                if (users.Count == 0)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new { message = $"No se encontraron usuarios con nombre '{FirstName}'" });
                }

                int totalPaginas = (int)Math.Ceiling((double)users.Count() / paginacion.RegistrosPorPagina);

                //En caso de que el numero de pagina solicitado sea menor o igual a 0, o el numero de pagina sea mayor al total de paginas, es decir una pagina inexistente, retornar error
                if (paginacion.NumeroPagina <= 0 || paginacion.NumeroPagina > totalPaginas)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new { message = $"Pagina incorrecta, para {paginacion.RegistrosPorPagina} registros por pagina, existen {totalPaginas} paginas." });
                }

                var pages = users.Skip((paginacion.NumeroPagina - 1) * paginacion.RegistrosPorPagina).Take(paginacion.RegistrosPorPagina).ToList();

                return StatusCode(StatusCodes.Status200OK, new
                {
                    message = "Success GetByFirstName!",
                    data = pages
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        //Obtener Usuario por primer apellido
        [HttpPost]
        [Route("GetByFirstLastName/{LastName}")]
        public IActionResult GetByFirstLastName(string LastName, [FromBody] Paginacion paginacion)
        {
            try
            {
                var users = _context.Usuarios.Where(user => user.PrimerApellido.ToLower().Contains(LastName.ToLower())).ToList();

                if (paginacion.NumeroPagina == 0 || paginacion.RegistrosPorPagina == 0)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new { message = "La configuracion de la paginacion no puede estar en 0" });
                }

                if (users.Count == 0)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new { message = $"No se encontraron usuarios con apellido '{LastName}'" });
                }

                int totalPaginas = (int)Math.Ceiling((double)users.Count() / paginacion.RegistrosPorPagina);

                //En caso de que el numero de pagina solicitado sea menor o igual a 0, o el numero de pagina sea mayor al total de paginas, es decir una pagina inexistente, retornar error
                if (paginacion.NumeroPagina <= 0 || paginacion.NumeroPagina > totalPaginas)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new { message = $"Pagina incorrecta, para {paginacion.RegistrosPorPagina} registros por pagina, existen {totalPaginas} paginas." });
                }

                var pages = users.Skip((paginacion.NumeroPagina - 1) * paginacion.RegistrosPorPagina).Take(paginacion.RegistrosPorPagina).ToList();

                return StatusCode(StatusCodes.Status200OK, new
                {
                    message = "Success GetByFirstName!",
                    data = pages
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        //Actualizar Usuario
        [HttpPut]
        [Route("UpdateUser")]

        public IActionResult UpdateUser(Usuario newData)
        {
            try
            {
                var user = _context.Usuarios.Find(newData.Id);

                //En caso de que el usuario no se encuentre en la bd
                if (user == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new { message = "User not found" });
                }

                user.Id = newData.Id;

                if (newData.PrimerNombre.IsNullOrEmpty())
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new { message = "El primer nombre no puede quedar vacio" });
                }
                else if (newData.PrimerApellido.IsNullOrEmpty())
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new { message = "El primer apellido no puede quedar vacio" });
                }

                if (newData.Sueldo <= 0)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new { message = "El sueldo no puede quedar en 0" });
                }

                user.PrimerNombre = newData.PrimerNombre;
                user.SegundoNombre = newData.SegundoNombre;
                user.PrimerApellido = newData.PrimerApellido;
                user.SegundoApellido = newData.SegundoApellido;
                user.FechaNacimiento = newData.FechaNacimiento;            
                user.Sueldo = newData.Sueldo;
                user.FechaModificacion = DateTime.Now;

                _context.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new
                {
                    message = "Success Update User!"
                });

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        //Eliminar usuario por ID
        [HttpDelete]
        [Route("DeleteUserById/{Id:int}")]

        public IActionResult DeleteUserById(int Id)
        {
            try
            {
                var user = _context.Usuarios.Find(Id);

                //En caso de que el usuario no se encuentre en la bd
                if (user == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new { message = "User not found" });
                }

                _context.Usuarios.Remove(user);
                _context.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new
                {
                    message = "Success Delete User!"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }
    }
}
