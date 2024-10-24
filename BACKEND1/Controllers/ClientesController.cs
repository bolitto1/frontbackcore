using BACKEND1.Repositorios;
using Entidades.Models;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class ClientesController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly ClienteDAL _clienteDAL;

    public ClientesController(IConfiguration config, ClienteDAL clienteDAL)
    {
        _config = config;
        _clienteDAL = clienteDAL ?? throw new ArgumentNullException(nameof(clienteDAL));
    }

    [HttpGet("{cedula}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Consulta([FromRoute] string cedula)
    {
        if (string.IsNullOrEmpty(cedula))
        {
            return BadRequest("La cédula no puede estar vacía");
        }

        try
        {
            IActionResult response;
            var cliente = new Cliente { Cedula = cedula }; // Asegúrate que la propiedad en la clase Cliente sea 'Cedula'
            var result = await _clienteDAL.Consulta(cliente, 1);

            if (result is OkObjectResult okResult)
            {
                var respuestaClientes = okResult.Value as Cliente[];
                if (respuestaClientes != null && respuestaClientes.Length > 0)
                {
                    // Devolver el primer cliente encontrado
                    response = Ok(respuestaClientes[0]);
                }
                else
                {
                    var responseObject = new Dictionary<string, string>
                {
                    { "ErrorCode", "si" },
                    { "Error", "Cliente no existe" }
                };
                    response = Unauthorized(responseObject);
                }
            }
            else if (result is NotFoundResult)
            {
                response = NotFound();
            }
            else
            {
                response = StatusCode(500, "Error interno del servidor");
            }

            return response;
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno: {ex.Message}");
        }
    }


    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ObtenerTodos()
    {
         
        try
        {
            IActionResult response;
            var cliente = new Cliente { Cedula = "" }; 
            var result = await _clienteDAL.ObtenerTodos(cliente, 10);

            if (result is OkObjectResult okResult)
            {
                var respuestaClientes = okResult.Value as Cliente[];
                if (respuestaClientes != null && respuestaClientes.Length > 0)
                {
                    // Devolver el primer cliente encontrado
                    response = Ok(respuestaClientes);
                }
                else
                {
                    var responseObject = new Dictionary<string, string>
                {
                    { "ErrorCode", "si" },
                    { "Error", "Cliente no existe" }
                };
                    response = Unauthorized(responseObject);
                }
            }
            else if (result is NotFoundResult)
            {
                response = NotFound();
            }
            else
            {
                response = StatusCode(500, "Error interno del servidor");
            }

            return response;
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno: {ex.Message}");
        }
    }
}