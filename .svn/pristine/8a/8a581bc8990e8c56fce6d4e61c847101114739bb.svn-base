using System;
using System.Collections.Generic;

namespace eSyaDoctorScheduler.DL.Entities
{
    public partial class GtEsdos1
    {
        public int BusinessKey { get; set; }
        public int ConsultationId { get; set; }
        public int ClinicId { get; set; }
        public int SpecialtyId { get; set; }
        public int DoctorId { get; set; }
        public string DayOfWeek { get; set; }
        public int SerialNo { get; set; }
        public TimeSpan ScheduleFromTime { get; set; }
        public TimeSpan ScheduleToTime { get; set; }
        public int NoOfPatients { get; set; }
        public bool Week1 { get; set; }
        public bool Week2 { get; set; }
        public bool Week3 { get; set; }
        public bool Week4 { get; set; }
        public bool Week5 { get; set; }
        public string RoomNo { get; set; }
        public bool ActiveStatus { get; set; }
        public string FormId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedTerminal { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedTerminal { get; set; }

        public virtual GtEsdocd Doctor { get; set; }
        public virtual GtEsspcd Specialty { get; set; }
    }
}
