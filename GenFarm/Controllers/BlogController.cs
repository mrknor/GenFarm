using Microsoft.AspNetCore.Mvc;
using GenFarm.Infrastructure;

namespace GenFarm.Controllers
{
    [ApiController]
    [Route("api/blog")]
    public class BlogController : ControllerBase
    {
        private readonly MessageQueue _messageQueue;

        public BlogController(MessageQueue messageQueue)
        {
            _messageQueue = messageQueue;
        }

        [HttpPost("generate")]
        public IActionResult GenerateBlog([FromBody] BlogRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.SEOPhrase))
            {
                return BadRequest("SEO phrase is required.");
            }

            var task = new AgentTask { TaskType = "SEOKeyword", Payload = request.SEOPhrase };
            _messageQueue.SendMessage(SerializeTask(task));
            return Ok("Blog generation initiated.");
        }

        public class BlogRequest
        {
            public string SEOPhrase { get; set; }
        }


        [HttpGet("status")]
        public IActionResult GetStatus()
        {
            var taskStatuses = _messageQueue.GetTaskStatus(); // Get current task statuses

            if (taskStatuses == null || taskStatuses.Count == 0)
            {
                return NotFound("No tasks found.");
            }

            return Ok(taskStatuses); // Return all task statuses
        }

        private string SerializeTask(AgentTask task)
        {
            return System.Text.Json.JsonSerializer.Serialize(task);
        }
    }

}
