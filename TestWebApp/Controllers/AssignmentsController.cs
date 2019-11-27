using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using TestWebApp.Models;

namespace TestWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentsController : ControllerBase
    {
        private IRepository _repository;
        public AssignmentsController(IRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IEnumerable<Assignment> Get()
        {
            return _repository.GetAll().ToList();
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Assignment assignment = _repository.Get(id);
            if (assignment == null)
                return NotFound();
            return Ok(assignment);
        }

        [HttpPost]
        public IActionResult Post([FromBody]Assignment assignment)
        {
            if (assignment == null)
            {
                return BadRequest();
            }
            assignment = _repository.Add(assignment);
            return Ok(assignment);
        }

        [HttpPut]
        public IActionResult Put([FromBody]Assignment assignment)
        {
            if (assignment == null)
            {
                return BadRequest();
            }
            assignment = _repository.Update(assignment);
            if (assignment == null)
            {
                return NotFound();
            }
            return Ok(assignment);
        }

        [HttpPut("setpriority/{id}")]
        public IActionResult SetPriority(int id, [FromQuery] int priority)
        {
            Assignment assignment = _repository.SetPriority(id, priority);
            if (assignment == null)
            {
                return NotFound();
            }
            return Ok(assignment);
        }

        [HttpPut("up/{id}")]
        public IActionResult Up(int id)
        {
            Assignment assignment = _repository.Up(id);
            if (assignment == null)
            {
                return NotFound();
            }
            return Ok(assignment);
        }

        [HttpPut("down/{id}")]
        public IActionResult Down(int id)
        {
            Assignment assignment = _repository.Down(id);
            if (assignment == null)
            {
                return NotFound();
            }
            return Ok(assignment);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Assignment assignment = _repository.Delete(id);
            if (assignment == null)
            {
                return NotFound();
            }
            return Ok(assignment);
        }
    }
}
