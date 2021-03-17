using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebDataDemo.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebDataDemo.Option_03_Dapper_SQL
{
    [Route("api/v03/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        // GET: api/<AuthorsController>
        [HttpGet]
        public ActionResult<Author> Get()
        {
            using var conn = new SqlConnection("Server = (localdb)\\mssqllocaldb; Database = DotNetDataAccessTour; Trusted_Connection = True; MultipleActiveResultSets = true");
            var sql = "SELECT * FROM Authors";
            var authors = conn.Query(sql).ToList();

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
