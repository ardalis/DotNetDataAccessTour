using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebDataDemo.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebDataDemo.Option_01_ADONET_SQL
{
    [Route("api/v01/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        // GET: api/<AuthorsController>
        [HttpGet]
        public ActionResult<Author> Get()
        {
            var authors = new List<Author>();
            using var conn = new SqlConnection("Server = (localdb)\\mssqllocaldb; Database = DotNetDataAccessTour; Trusted_Connection = True; MultipleActiveResultSets = true");
            var sql = "SELECT * FROM Authors";
            var cmd = new SqlCommand(sql, conn);
            conn.Open();
            using var reader = cmd.ExecuteReader();
            if(reader.HasRows)
            {
                while (reader.Read())
                {
                    var author = new Author();
                    author.Id = reader.GetInt32(0);
                    author.Name = reader.GetString(1);
                    authors.Add(author);
                }
            }

            return Ok(authors);
        }

        // GET api/<AuthorsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<AuthorsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<AuthorsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<AuthorsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
