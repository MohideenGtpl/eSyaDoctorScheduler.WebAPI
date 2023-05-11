using System;
using System.Collections.Generic;

namespace eSyaDoctorScheduler.DL.Entities
{
    public partial class GtEsdold
    {
        public int DoctorId { get; set; }
        public DateTime OnLeaveFrom { get; set; }
        public DateTime OnLeaveTill { get; set; }
        public int NoOfDays { get; set; }
        public string Comments { get; set; }
        public bool ActiveStatus { get; set; }
        public string FormId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedTerminal { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedTerminal { get; set; }

        public virtual GtEsdocd Doctor { get; set; }
    }
}
