using System;
using System.Collections.Generic;
using System.Security.Claims;
using ItHappend.RestAPI.Authentication;
using ItHappend.RestAPI.Extensions;
using ItHappend.RestAPI.Filters;
using ItHappend.RestAPI.Models;
using ItHappened.Application;
using ItHappened.Domain;
using ItHappened.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ItHappend.RestAPI.Controllers
{
    [Route("tracks")]
    //[Authorize]
    public class TrackController: ControllerBase
    {
        private readonly ITracksService _trackService;
        private readonly ITrackRepository _trackRepository;

        public TrackController(ITracksService trackService,ITrackRepository trackRepository)
        {
            _trackService = trackService;
            _trackRepository = trackRepository;
        }

        [HttpGet]
        public IActionResult GetTracks()
        {
            var userId = Guid.Parse(User.FindFirstValue(JwtClaimTypes.Id));
            var tracks = _trackService.GetTracks(userId);
            var response = tracks.Map();
            return Ok(response);
        }

        [HttpPost]
        public IActionResult CreateTrack([FromBody] CreateTrackRequest request)
        {
            var createdTrack = _trackService.CreateTrack(
                Guid.Parse(User.FindFirstValue(JwtClaimTypes.Id)),
                request.Name,
                request.CreatedAt,
                request.AllowedCustomizations.Map());

            var respone = createdTrack.Map();
            
            return Ok(respone);
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult EditTrack([FromRoute]Guid id, [FromBody] EditTrackRequest request)
        {
            var userId = Guid.Parse(User.FindFirstValue(JwtClaimTypes.Id));
            var trackDto = request.Map(id);
            var editedTrack = _trackService.EditTrack(userId, trackDto);
            var response = editedTrack.MapToResponse();
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

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetById([FromRoute] Guid id)
        {
            var track = _trackRepository.TryGetTrackById(id);
            return Ok(track);
        }
        [HttpGet]
        [Route("test")]
        public IActionResult GetById()
        {
            var c = new List<CustomizationType>(){CustomizationType.Comment};
            var g = Guid.NewGuid();
            var newTrack = new Track(g,"fsfs",new DateTime(),g,c );
            var track = _trackRepository.TryCreate(newTrack);
            var track1 = _trackRepository.TryGetTracksByUser(g);
            return Ok(track1);
        }
    }
}