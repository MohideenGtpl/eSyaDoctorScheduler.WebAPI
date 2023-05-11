using System;
using System.Collections.Generic;

namespace eSyaDoctorScheduler.DL.Entities
{
    public partial class GtEsspcd
    {
        public GtEsspcd()
        {
            GtEsdos1 = new HashSet<GtEsdos1>();
            GtEsdos2 = new HashSet<GtEsdos2>();
            GtEsdosc = new HashSet<GtEsdosc>();
        }

        public int SpecialtyId { get; set; }
        public string SpecialtyDesc { get; set; }
        public string Gender { get; set; }
        public string SpecialtyType { get; set; }
        public string AlliedServices { get; set; }
        public string MedicalIcon { get; set; }
        public bool ActiveStatus { get; set; }
        public string FormId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedTerminal { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedTerminal { get; set; }

        public virtual ICollection<GtEsdos1> GtEsdos1 { get; set; }
        public virtual ICollection<GtEsdos2> GtEsdos2 { get; set; }
        public virtual ICollection<GtEsdosc> GtEsdosc { get; set; }
    }
}
