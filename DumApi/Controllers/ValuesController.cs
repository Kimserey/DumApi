using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace DumpApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ValuesController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet("{id}")]
        public ActionResult<int> Get(int id)
        {
            return id;
        }

        [HttpPost]
        public ActionResult<string> Post([FromBody] string value)
        {
            return value;
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        [HttpDelete("{id}")]
        public ActionResult<int> Delete(int id)
        {
            return id;
        }
    }
}
