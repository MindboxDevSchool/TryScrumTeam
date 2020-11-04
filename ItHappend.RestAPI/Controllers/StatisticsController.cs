using System;
using ItHappend.RestAPI.Extensions;
using ItHappend.RestAPI.Models;
using ItHappened.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ItHappend.RestAPI.Controllers
{
    [Authorize]
    [Route("statistics")]
    public class StatisticsController: ControllerBase
    {
        private readonly IStatisticsService _statisticsService;

        public StatisticsController(IStatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }

        [HttpGet]
        [Route("track={trackId}")]
        public IActionResult GetTrackStatistics([FromRoute] Guid trackId)
        {
            var userId = User.GetUserId();

            var trackStatistics = _statisticsService.GetTrackStatistics(userId, trackId);

            var result = new GetTrackStatisticsResponse {TrackStatistics = trackStatistics};
            return Ok(result);
        }
    }
}