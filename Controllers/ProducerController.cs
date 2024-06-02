using Microsoft.AspNetCore.Mvc;
using ProducerImplementation.ProducerService;
using System.Text.Json;
using ProducerImplementation.Models;
using backgroundImplementation.Data;
using Kafkaproducerimplemenation.DTO;

namespace ProducerImplementation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProducerController : ControllerBase
    {
        private readonly Producer _producer;
        private readonly ApplicationDbcontext  _application;

        public ProducerController(Producer producer, ApplicationDbcontext application)
        {
            _producer = producer;
            _application = application;
        }
        [HttpPost]
        public async Task<IActionResult> Addprince(Princesrequest request)
        {
            var prince = new prince()
            {
                Name = request.Name,
                Description = request.Description,
            };

            _application.Princes.Add(prince);
            await _application.SaveChangesAsync(); 

            var message = JsonSerializer.Serialize(request);

            await _producer.ProduceAsync("princesAdded", message);

            return Ok("Prince added successfully.");
        }
    }
}

