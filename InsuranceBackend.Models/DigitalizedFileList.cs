using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class DigitalizedFileList: DigitalizedFile
    {
        public int TotalRecords { get; set; }
        public string DigitalizedFileTypeDesc { get; set; }
    }
}
