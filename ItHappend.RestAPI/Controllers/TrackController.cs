using System;
using System.Security.Claims;
using ItHappend.RestAPI.Authentication;
using ItHappend.RestAPI.Extensions;
using ItHappend.RestAPI.Filters;
using ItHappend.RestAPI.Models;
using ItHappened.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ItHappend.RestAPI.Controllers
{
    [Route("tracks")]
    [Authorize]
    public class TrackController : ControllerBase
    {
        private readonly ITracksService _trackService;

        public TrackController(ITracksService trackService)
        {
            _trackService = trackService;
        }

        [HttpGet]
        public IActionResult GetTracks()
        {
            var userId = User.GetUserId();
            var tracks = _trackService.GetTracks(userId);
            var response = tracks.Map();
            return Ok(response);
        }

        [HttpPost]
        public IActionResult CreateTrack([FromBody] CreateTrackRequest request)
        {
            var createdTrack = _trackService.CreateTrack(
                User.GetUserId(),
                request.Name,
                request.CreatedAt,
                request.AllowedCustomizations.Map());

            var response = createdTrack.Map();

            return Ok(response);
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult EditTrack([FromRoute] Guid id, [FromBody] EditTrackRequest request)
        {
            var userId = User.GetUserId();
            var trackDto = request.Map(id);
            var editedTrack = _trackService.EditTrack(userId, trackDto);
            var response = editedTrack.MapToResponse();
            return Ok(response);
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeleteTrack([FromRoute] Guid id)
        {
            var userId = User.GetUserId();
            var trackId = _trackService.DeleteTrack(userId, id);
            return Ok(trackId);
        }
    }
}