﻿using System;
using System.Collections.Generic;
using ItHappened.Domain;

namespace ItHappend.RestAPI.Models
{
    public class CreateTrackRequest
    {
        public string Name { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public string[] AllowedCustomizations { get; set; }
    }
}