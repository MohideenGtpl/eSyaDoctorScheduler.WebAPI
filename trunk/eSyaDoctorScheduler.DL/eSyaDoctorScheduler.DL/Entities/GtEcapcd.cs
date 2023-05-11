using System;
using System.Collections.Generic;

namespace eSyaDoctorScheduler.DL.Entities
{
    public partial class GtEcapcd
    {
        public int ApplicationCode { get; set; }
        public int CodeType { get; set; }
        public string CodeDesc { get; set; }
        public string ShortCode { get; set; }
        public bool DefaultStatus { get; set; }
        public bool UsageStatus { get; set; }
        public bool ActiveStatus { get; set; }
        public string FormId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedTerminal { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedTerminal { get; set; }
    }
}
