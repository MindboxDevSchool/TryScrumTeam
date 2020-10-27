using System;
using System.Collections.Generic;
using ItHappened.Domain;

namespace ItHappend.RestAPI.Models
{
    public class GetTracksResponse
    {
        public TrackModel[] Tracks { get; set; }
    }
}