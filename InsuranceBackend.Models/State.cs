﻿using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class State
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string IdCountry { get; set; }
    }
}
