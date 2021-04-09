using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MVC_API_DAPPER_DEMO.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_API_DAPPER_DEMO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {

        private readonly string _connectionString;

        public ProductoController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConnectionString");
        }


        [HttpGet]
       public IActionResult Get()
        {

            IEnumerable<ProductoModel> lst = null;
          
            using (var db = new MySqlConnection(_connectionString))
            {
                var sql = "select * from producto";
                lst = db.Query<ProductoModel>(sql);
            }
            return Ok(lst);
          

        }
        [HttpPost]
        public IActionResult Insert(ProductoModel model)
        {
            int result = 0;

            using (var db = new MySqlConnection(_connectionString))
            {
                var sql = "insert into producto(pro_codigo, pro_nombre, pro_descripcion, pro_precio) " +
                    "values(@pro_codigo, @pro_nombre, @pro_descripcion, @pro_precio)";

                result = db.Execute(sql, model);
            }
            return Ok(result);
        }

        [HttpPut]
        public IActionResult Edit(ProductoModel model)
        {
            int result = 0;

            using (var db = new MySqlConnection(_connectionString))
            {
                var sql = "UPDATE Producto set " +
                    "pro_codigo=@pro_codigo, " +
                    "pro_nombre=@pro_nombre, " +
                    "pro_descripcion=@pro_descripcion, " +
                    "pro_precio=@pro_precio " +
                    "where pro_codigo=@pro_codigo";

                result = db.Execute(sql, model);
            }
            return Ok(result);
        }

        [HttpDelete]
        public IActionResult DElet(ProductoModel model)
        {
            int result = 0;

            using (var db = new MySqlConnection(_connectionString))
            {
                var sql = "Delete from Producto where pro_codigo=@pro_codigo";

                result = db.Execute(sql, model);
            }
            return Ok(result);
        }

        [HttpGet("StoreProcedure")]
        public async Task<List<ProductoModel>> GetActionResult()
        {
            using (var db = new MySqlConnection(_connectionString))
            {
                using (var cmd = new MySqlCommand("sp_mostrar_datos", db))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    var response = new List<ProductoModel>();
                    await db.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while(await reader.ReadAsync())
                        {
                            response.Add(MapToValue(reader));
                        }
                    }

                    return response;

                }
            }
        }

        private ProductoModel MapToValue(System.Data.Common.DbDataReader reader)
        {
            return new ProductoModel()
            {
                pro_codigo = reader["pro_codigo"].ToString(),
                pro_nombre = reader["pro_nombre"].ToString(),
                pro_descripcion = reader["pro_descripcion"].ToString(),
                pro_precio = (decimal)reader["pro_precio"]
            };
        }
    }
}
