using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using ItHappend.RestAPI.Extensions;
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
        public IActionResult GetTracks([FromQuery]int? take, [FromQuery]int? skip)
        {
            var userId = User.GetUserId();
            var tracks = _trackService.GetTracks(userId, take, skip);
            
            var mappedTracks = new List<TrackModel>();

            foreach (var track in tracks)
            {
                var mappedTrack = new TrackModel();
                mappedTrack.Id = track.Id;
                mappedTrack.Name = track.Name;
                mappedTrack.CreatedAt = track.CreatedAt;
                try
                {
                    mappedTrack.AllowedCustomizations = track.AllowedCustomizations.Select(c => c.ToString()).ToArray();
                }
                catch
                {
                    mappedTrack.AllowedCustomizations = Array.Empty<string>();
                }
                mappedTracks.Add(mappedTrack);
            }
            
            var response = new GetTracksResponse()
            {
                Tracks = mappedTracks.ToArray(),
            };
            return Ok(response);
        }

        [HttpPost]
        public IActionResult CreateTrack([FromBody] CreateTrackRequest request)
        {
            var createdTrack = _trackService.CreateTrack(
                User.GetUserId(),
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
            var userId = User.GetUserId();
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
            var userId = User.GetUserId();
            var trackId = _trackService.DeleteTrack(userId, id);
            return Ok(trackId);
        }
    }
}