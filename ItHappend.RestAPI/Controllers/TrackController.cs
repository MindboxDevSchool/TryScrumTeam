using System;
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
    [GlobalException]
    public class TrackController: ControllerBase
    {
        private readonly ITracksService _trackService;

        public TrackController(ITracksService trackService)
        {
            _trackService = trackService;
        }

        [HttpGet]
        public IActionResult GetTracks([FromBody] GetTracksRequest request)
        {
            var tracks = _trackService.GetTracks(request.UserId);
            var response = tracks.Map();
            return Ok(response);
        }

        [HttpPost]
        public IActionResult CreateTrack([FromBody] CreateTrackRequest request)
        {
            var createdTrack = _trackService.CreateTrack(
                request.UserId,
                request.Name,
                request.CreatedAt,
                request.AllowedCustomizations.Map());

            var respone = createdTrack.Map();
            
            return Ok(respone);
        }

        [HttpPut]
        public IActionResult EditTrack([FromBody] EditTrackRequest request)
        {
            var trackDto = request.Map();
            var editedTrack = _trackService.EditTrack(request.CreatorId, trackDto);
            var response = editedTrack.MapToResponse();
            return Ok(response);
        }

        [HttpPost]
        [Route("{id}")]
        public IActionResult DeleteTrack([FromRoute] Guid id, [FromBody] DeleteTrackRequest request)
        {
            var trackId = _trackService.DeleteTrack(request.UserId, id);
            return Ok(trackId);
        }
    }
}