using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DumApi.Controllers
{
    public class DumObj
    {
        [Required]
        public string Id { get; set; }
        public int Value { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ValuesController : ControllerBase
    {
        private static readonly Dictionary<string, DumObj> _values = new Dictionary<string, DumObj>();

        [HttpGet]
        public ActionResult<IEnumerable<DumObj>> Get() => _values.Values;

        [HttpGet("{id}")]
        public ActionResult<DumObj> Get(string id) => _values[id];

        [HttpPost]
        public IActionResult Post(DumObj value)
        {
            if (_values.ContainsKey(value.Id))
                return BadRequest("Object already exists.");

            _values[value.Id] = value;
            return Ok();
        }

        [HttpPut]
        public void Put(DumObj value) => _values[value.Id] = value;

        [HttpDelete("{id}")]
        public void Delete(string id) => _values.Remove(id);

        [HttpPost("clear")]
        public void Clear() => _values.Clear();
    }
}
