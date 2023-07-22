using HomeTask.Data.Models;
using HomeTask.Service.Interfaces;
using HomeTask.View.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace HomeTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivityController : ControllerBase
    {
        private readonly ILogger<ActivityController> _logger;
        private readonly IActivityService _activityService;

        /// <summary>
        ///     Creates an instance of Activity controller
        /// </summary>
        /// <param name="logger"></param>
        public ActivityController(ILogger<ActivityController> logger, IActivityService activityService)
        {
            _logger = logger ?? throw new ArgumentException(nameof(logger));
            _activityService = activityService ?? throw new ArgumentException(nameof(activityService));
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

            if (data != null && !ModelState.IsValid)
            {
                return Problem(detail: "Invalid Checkin ",
                    statusCode: StatusCodes.Status400BadRequest);
            }

            var result = await _activityService.ActivityCheckin(data.ActivityId, data.ActivityId, data.StudentPassword, token);

            if (result.HttpStatusCode != (int)HttpStatusCode.OK)
            {
                return Problem(detail: result.Message,
                    statusCode: result.HttpStatusCode);
            }

            return Ok(result);
        }


        [HttpGet]
        [Route("GetActivities")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Get(string classroomId, CancellationToken token)
        {
            if (string.IsNullOrEmpty(classroomId))
            {
                return Problem(detail: "Invalid Classroom Id record",
                    statusCode: StatusCodes.Status400BadRequest);
            }

            var result = await _activityService.GetActivities(classroomId, token);

            if (result.HttpStatusCode != (int)HttpStatusCode.OK)
            {
                return Problem(detail: result.Message,
                    statusCode: result.HttpStatusCode);
            }

            return Ok(result);
        }
    }
}
