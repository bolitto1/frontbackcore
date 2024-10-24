using Entidades.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Data;

namespace BACKEND1.Repositorios
{
    public class ClienteDAL
    {
        private readonly string _cadenaConexion;

        public ClienteDAL(string cadenaConexion)
        {
            _cadenaConexion = cadenaConexion;
        }


        public async Task<IActionResult> Consulta(Cliente Cli, int crud)
        {
            try
            {
                var jsonResponse = new { Resultado = "Error" };
                string json = System.Text.Json.JsonSerializer.Serialize(Cli);
                using (SqlConnection con = new SqlConnection(_cadenaConexion))
                {
                    using (var command = new SqlCommand("Clientes_SP01", con))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@jsonData", json);
                        command.Parameters.AddWithValue("@crud", crud);
                        await con.OpenAsync();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (reader.HasRows)
                            {
                                while (await reader.ReadAsync())
                                {
                                    var resultado = reader.GetString(0);
                                    var respuestaUsuario = JsonConvert.DeserializeObject<Entidades.Models.Cliente[]>(resultado);

                                    return new OkObjectResult(respuestaUsuario);
                                }
                            }
                            else
                            {
                                return new NotFoundResult();
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new BadRequestResult();
            }
            return new BadRequestResult();
        }//fin de metodo


        public async Task<IActionResult> ObtenerTodos(Cliente Cli, int crud)
        {
            try
            {
                var jsonResponse = new { Resultado = "Error" };
                string json = System.Text.Json.JsonSerializer.Serialize(Cli);
                using (SqlConnection con = new SqlConnection(_cadenaConexion))
                {
                    using (var command = new SqlCommand("Clientes_SP01", con))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@jsonData", json);
                        command.Parameters.AddWithValue("@crud", crud);
                        await con.OpenAsync();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (reader.HasRows)
                            {
                                while (await reader.ReadAsync())
                                {
                                    var resultado = reader.GetString(0);
                                    var respuestaUsuario = JsonConvert.DeserializeObject<Entidades.Models.Cliente[]>(resultado);

                                    return new OkObjectResult(respuestaUsuario);
                                }
                            }
                            else
                            {
                                return new NotFoundResult();
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new BadRequestResult();
            }
            return new BadRequestResult();
        }//fin de metodo
    }
}
