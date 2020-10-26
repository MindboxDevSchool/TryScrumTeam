using System;
using System.Collections;
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
    [Authorize]
    [GlobalException]
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
        public IActionResult GetEvents([FromRoute]Guid trackId)
        {
            var userId = GetUserId();
            var events = _eventService.GetEvents(userId, trackId);
            
            var result = new GetEventsResponse()
            {
                Events = _mapper.Map<IEnumerable<EventDto>, GetEventsResponseItem[]>(events),
            };
            return Ok(result);
        }
        
        [HttpPost]
        public IActionResult CreateEvent([FromRoute]Guid trackId, [FromBody]CreateEventRequest request)
        {
            var userId = GetUserId();
            var customizations = _mapper.Map<CustomizationsDto>(request.Customizations);
            
            var @event = _eventService.CreateEvent(userId, trackId, request.CreatedAt, customizations);

            var result = _mapper.Map<CreateEventResponse>(@event);
            return Ok(result);
        }
        
        [HttpDelete]
        [Route("{eventId}")]
        public IActionResult DeleteEvent([FromRoute]Guid eventId)
        {
            var userId = GetUserId();
            
            var @event = _eventService.DeleteEvent(userId, eventId);

            var result = new DeleteEventResponse { Id = @event};
            return Ok(result);
        }
        
        [HttpPut]
        [Route("{eventId}")]
        public IActionResult EditEvent([FromRoute]Guid trackId, [FromRoute]Guid eventId, [FromBody] EditEventRequest request)
        {
            var userId = GetUserId();
            var customizations = _mapper.Map<CustomizationsDto>(request.Customizations);
            var eventDto = new EventDto(eventId, request.CreatedAt, trackId, customizations);
            var @event = _eventService.EditEvent(userId, eventDto);

            var result = _mapper.Map<EditEventResponse>(@event);
            return Ok(result);
        }

        private Guid GetUserId()
        {
            return Guid.Parse(User.FindFirstValue(JwtClaimTypes.Id));
        }
    }
}