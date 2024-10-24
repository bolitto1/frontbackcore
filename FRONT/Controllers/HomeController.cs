using Entidades.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;

namespace FRONT.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetCliente(string cedula)
        {
            var clienteApiUrl = _configuration.GetValue<string>("ApiSettings:ClienteApiUrl");
            clienteApiUrl = clienteApiUrl.Replace("{cedula}", cedula);

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(clienteApiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var cliente = await response.Content.ReadFromJsonAsync<Cliente>();
                    return Ok(cliente);
                }
                else
                {
                    var code = StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
                    return code;
                }
            }
        } 


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {

                var clienteApiUrl = _configuration.GetValue<string>("ApiSettings:ClienteApiUrlTodos");

                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync(clienteApiUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        var cliente = await response.Content.ReadFromJsonAsync<Cliente[]>();
                        return Ok(cliente);
                    }
                    else
                    {
                        var code = StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
                        return code;
                    }
                }
            }
            catch (Exception ex) {
                Console.Write(ex.Message);
                return BadRequest(ex.Message);
            }
            
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new Models.ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}