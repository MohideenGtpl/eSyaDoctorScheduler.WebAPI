using System;
using System.Collections.Generic;

namespace eSyaDoctorScheduler.DL.Entities
{
    public partial class GtEopapq
    {
        public int BusinessKey { get; set; }
        public DateTime TokenDate { get; set; }
        public string QueueTokenKey { get; set; }
        public TimeSpan AppointmentFromTime { get; set; }
        public long AppointmentKey { get; set; }
        public int SequeueNumber { get; set; }
        public string PatientType { get; set; }
        public int SpecialtyId { get; set; }
        public int DoctorId { get; set; }
        public long? Uhid { get; set; }
        public string PatientId { get; set; }
        public string PatientName { get; set; }
        public string TokenStatus { get; set; }
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
