﻿using Microsoft.AspNetCore.Mvc;
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

    public class ValuesController : ControllerBase
    {
        private readonly Dictionary<string, DumObj> _values = new Dictionary<string, DumObj>();

        [HttpGet]
        public ActionResult<IEnumerable<DumObj>> Get() => _values.Values;

        [HttpGet("{id}")]
        public ActionResult<DumObj> Get(string id) => _values[id];

        [HttpPost]
        public void Post(DumObj value) => _values[value.Id] = value;

        [HttpDelete("{id}")]
        public void Delete(string id) => _values.Remove(id);
    }
}
