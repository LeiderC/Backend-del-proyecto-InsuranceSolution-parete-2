﻿using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class MovementType
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public string Alias { get; set; }
        public bool PolicyHeader { get; set; }
    }
}
