using System;
using System.Collections.Generic;

namespace Registration.Models
{
    public partial class StudentInfo
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public int? MobileNo { get; set; }
        public string? Lan { get; set; }
        public string? Gender { get; set; }
        public string? Lang { get; set; }
        public DateTime? Date { get; set; }
    }
}
