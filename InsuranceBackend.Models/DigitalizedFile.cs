﻿using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class DigitalizedFile
    {
        public int Id { get; set; }
        public int IdCustomer { get; set; }
        public int IdPolicy { get; set; }
        public int IdTask { get; set; }
        public int IdTypeDigitalizedFile { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string UID { get; set; }
        public string FileRoute { get; set; }
    }
}