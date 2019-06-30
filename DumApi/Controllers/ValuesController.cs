using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DumApi.Controllers
{
    public class MyValue
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
        private static readonly Dictionary<string, MyValue> _values = new Dictionary<string, MyValue>();

        [HttpGet]
        public ActionResult<IEnumerable<MyValue>> Get() => _values.Values;

        [HttpGet("{id}")]
        public ActionResult<MyValue> Get(string id) => _values[id];

        [HttpPost]
        public IActionResult Post(MyValue value)
        {
            if (_values.ContainsKey(value.Id))
                return BadRequest("Object already exists.");

            _values[value.Id] = value;
            return Ok();
        }

        [HttpPut]
        public void Put(MyValue value) => _values[value.Id] = value;

        [HttpDelete("{id}")]
        public void Delete(string id) => _values.Remove(id);

        [HttpPost("clear")]
        public void Clear() => _values.Clear();
    }
}
