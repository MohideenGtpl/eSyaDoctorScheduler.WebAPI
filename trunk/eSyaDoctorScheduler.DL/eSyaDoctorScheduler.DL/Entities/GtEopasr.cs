using System;
using System.Collections.Generic;

namespace eSyaDoctorScheduler.DL.Entities
{
    public partial class GtEopasr
    {
        public int BusinessKey { get; set; }
        public long AppointmentKey { get; set; }
        public int ServiceId { get; set; }
        public decimal Rate { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal NetAmount { get; set; }
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
