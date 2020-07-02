using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using WebAppCorePostgreSQL.models;

namespace WebAppCorePostgreSQL.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // Pooling=true;Min Pool Size=0;Max Pool Size=100;Connection Lifetime=0;
       protected readonly String conStr = "Host=localhost;Port=5432;User ID=postgres;Password=@minjdb1;Pooling=true;Connection Idle Lifetime=300;MinPoolSize=1;MaxPoolSize=100;Timeout=15;SslMode=Disable;Database=postgres";
       protected NpgsqlConnection conn=null;
        public async Task<IActionResult> get()
        {

            /*
            var connString = "Host=myserver;Username=mylogin;Password=mypass;Database=mydatabase";

            await using var conn = new NpgsqlConnection(connString);
            await conn.OpenAsync();

            // Insert some data
            await using (var cmd = new NpgsqlCommand("INSERT INTO data (some_field) VALUES (@p)", conn))
            {
                cmd.Parameters.AddWithValue("p", "Hello world");
                await cmd.ExecuteNonQueryAsync();
            }

            // Retrieve all rows
            await using (var cmd = new NpgsqlCommand("SELECT some_field FROM data", conn))
            await using (var reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                    Console.WriteLine(reader.GetString(0));
            //return new Task<OkObjectResult>(reader.GetString(0));
            */
            
            try
            {
                conn = new NpgsqlConnection(conStr);
                await conn.OpenAsync();
                NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM products", conn);
                NpgsqlDataReader rd = await cmd.ExecuteReaderAsync();
                /*
                while (rd.Read()) { 
                }
               */
                if (rd.HasRows)
                {
                    List<products> list = new List<products>();
                    while (rd.Read()) {
                        
                        products prd = new products();
                        prd.Product_id = rd.GetInt32(0);
                        prd.Product_name = rd.GetString(1);
                        prd.Price = rd.GetDouble(5);
                        prd.Stock = rd.GetInt32(6);
                        list.Add(prd);
                        
                        //list.Add(new products(Product_id => rd.GetInt32(0),Product_name=> rd.GetInt32(0)));
                        
                    }
                    OkObjectResult result = Ok(new { result = true, status = HttpStatusCode.OK, data = list });
                    return result;
                }
                else
                {
                    NotFoundObjectResult result = NotFound(new { result = false, status = HttpStatusCode.NotFound, data = "Data Not Found" });
                    return result;
                    //return new Task<NotFoundObjectResult>(new { result = false, status = HttpStatusCode.NotFound, data = "Data Not Found" });
                }
            }
            catch (Exception e)
            {
                BadRequestObjectResult result = BadRequest(new { result = false, status = HttpStatusCode.BadRequest, data = e.Message });
                return result;
                //return new BadRequestObjectResult(new { result = false, status = HttpStatusCode.BadRequest, data = e.Message });
            }
            finally {
                
                conn.Close();
                conn.Dispose();
            }


        }
    }
}
