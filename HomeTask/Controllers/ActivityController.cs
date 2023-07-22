using HomeTask.View.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivityController : ControllerBase
    {
        private readonly ILogger<ActivityController> _logger;

        /// <summary>
        ///     Creates an instance of Activity controller
        /// </summary>
        /// <param name="logger"></param>
        public ActivityController(ILogger<ActivityController> logger)
        {
            _logger = logger ?? throw new ArgumentException(nameof(logger));
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Checkin")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Create([FromBody] Checkin data, CancellationToken token)
        {

            return Ok();
        }


        [HttpGet]
        [Route("GetActivities")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Get(int classroomId, CancellationToken token)
        {
            var result = new List<ActivityView>();
            return Ok(result);
        }
    }
}
