using System;
using System.Collections.Generic;

namespace eSyaDoctorScheduler.DL.Entities
{
    public partial class GtEsdocd
    {
        public GtEsdocd()
        {
            GtEsdold = new HashSet<GtEsdold>();
            GtEsdos1 = new HashSet<GtEsdos1>();
            GtEsdos2 = new HashSet<GtEsdos2>();
            GtEsdosc = new HashSet<GtEsdosc>();
        }

        public int DoctorId { get; set; }
        public string DoctorName { get; set; }
        public string DoctorShortName { get; set; }
        public string Gender { get; set; }
        public string Qualification { get; set; }
        public string DoctorRegnNo { get; set; }
        public int Isdcode { get; set; }
        public string MobileNumber { get; set; }
        public string EmailId { get; set; }
        public int DoctorClass { get; set; }
        public int DoctorCategory { get; set; }
        public bool AllowConsultation { get; set; }
        public bool IsRevenueShareApplicable { get; set; }
        public bool AllowSms { get; set; }
        public string LanguageKnown { get; set; }
        public string Experience { get; set; }
        public string TraiffFrom { get; set; }
        public int TimeSlotInMintues { get; set; }
        public string DoctorRemarks { get; set; }
        public string Password { get; set; }
        public bool ActiveStatus { get; set; }
        public string FormId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedTerminal { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedTerminal { get; set; }

        public virtual ICollection<GtEsdold> GtEsdold { get; set; }
        public virtual ICollection<GtEsdos1> GtEsdos1 { get; set; }
        public virtual ICollection<GtEsdos2> GtEsdos2 { get; set; }
        public virtual ICollection<GtEsdosc> GtEsdosc { get; set; }
    }
}
