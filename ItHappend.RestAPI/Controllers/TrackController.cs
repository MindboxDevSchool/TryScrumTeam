using System;
using System.Collections.Generic;
using System.Security.Claims;
using AutoMapper;
using ItHappend.RestAPI.Authentication;
using ItHappend.RestAPI.Filters;
using ItHappend.RestAPI.Models;
using ItHappened.Application;
using ItHappened.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ItHappend.RestAPI.Controllers
{
    [Route("tracks")]
    [Authorize]
    public class TrackController : ControllerBase
    {
        private readonly ITracksService _trackService;
        private readonly IMapper _mapper;

        public TrackController(ITracksService trackService, IMapper mapper)
        {
            _trackService = trackService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetTracks()
        {
            var userId = Guid.Parse(User.FindFirstValue(JwtClaimTypes.Id));
            var tracks = _trackService.GetTracks(userId);
            var response = new GetTracksResponse()
            {
                Tracks = _mapper.Map<IEnumerable<TrackDto>, TrackModel[]>(tracks),
            };
            return Ok(response);
        }

        [HttpPost]
        public IActionResult CreateTrack([FromBody] CreateTrackRequest request)
        {
            var createdTrack = _trackService.CreateTrack(
                Guid.Parse(User.FindFirstValue(JwtClaimTypes.Id)),
                request.Name,
                request.CreatedAt,
                _mapper.Map<IEnumerable<CustomizationType>>(request.AllowedCustomizations));

            var response = _mapper.Map<TrackModel>(createdTrack);
            return Ok(response);
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult EditTrack([FromRoute] Guid id, [FromBody] EditTrackRequest request)
        {
            var userId = Guid.Parse(User.FindFirstValue(JwtClaimTypes.Id));
            var trackToEitDto = new TrackToEditDto(
                id,
                request.Name,
                _mapper.Map<IEnumerable<CustomizationType>>(request.AllowedCustomizations));
            var editedTrack = _trackService.EditTrack(userId, trackToEitDto);
            var response = _mapper.Map<TrackModel>(editedTrack);
            return Ok(response);
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeleteTrack([FromRoute] Guid id)
        {
            var userId = Guid.Parse(User.FindFirstValue(JwtClaimTypes.Id));
            var trackId = _trackService.DeleteTrack(userId, id);
            return Ok(trackId);
        }
    }
}