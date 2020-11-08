using System;
using System.Collections.Generic;
using AutoMapper;
using ItHappend.RestAPI.Extensions;
using ItHappend.RestAPI.Models;
using ItHappened.Application;
using ItHappened.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ItHappend.RestAPI.Controllers
{
    [Authorize]
    [Route("tracks/{trackId}/events")]
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;
        private readonly IMapper _mapper;

        public EventController(IEventService eventService, IMapper mapper)
        {
            _eventService = eventService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetEvents([FromRoute] Guid trackId, [FromQuery]int? take, [FromQuery]int? skip)
        {
            var userId = User.GetUserId();
            var events = _eventService.GetEvents(userId, trackId, take, skip);

            var result = new GetEventsResponse()
            {
                Events = _mapper.Map<IEnumerable<EventDto>, EventModel[]>(events),
            };
            return Ok(result);
        }

        [HttpGet]
        [Route("{eventId}")]
        public IActionResult GetEvent([FromRoute] Guid trackId, [FromRoute] Guid eventId)
        {
            var userId = User.GetUserId();
            var eventDto = _eventService.GetEvent(userId, trackId, eventId);
            var response = _mapper.Map<EventModel>(eventDto);
            return Ok(response);
        }

        [HttpPost]
        public IActionResult CreateEvent([FromRoute] Guid trackId, [FromBody] CreateEventRequest request)
        {
            var userId = User.GetUserId();
            var customizations = _mapper.Map<CustomizationsDto>(request.Customizations);

            var @event = _eventService.CreateEvent(userId, trackId, request.CreatedAt, customizations);

            var result = _mapper.Map<EventModel>(@event);
            return Ok(result);
        }

        [HttpDelete]
        [Route("{eventId}")]
        public IActionResult DeleteEvent([FromRoute] Guid eventId)
        {
            var userId = User.GetUserId();

            var @event = _eventService.DeleteEvent(userId, eventId);

            var result = new DeleteEventResponse {Id = @event};
            return Ok(result);
        }

        [HttpPut]
        [Route("{eventId}")]
        public IActionResult EditEvent([FromRoute] Guid trackId, [FromRoute] Guid eventId,
            [FromBody] EditEventRequest request)
        {
            var userId = User.GetUserId();
            var customizations = _mapper.Map<CustomizationsDto>(request.Customizations);
            var eventDto = new EventToEditDto(eventId, customizations);
            var @event = _eventService.EditEvent(userId, eventDto);

            var result = _mapper.Map<EventModel>(@event);
            return Ok(result);
        }
    }
}