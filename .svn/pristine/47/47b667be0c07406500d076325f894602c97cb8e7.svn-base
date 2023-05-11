using System;
using System.Collections.Generic;

namespace eSyaDoctorScheduler.DL.Entities
{
    public partial class GtEcbsln
    {
        public GtEcbsln()
        {
            GtEcclco = new HashSet<GtEcclco>();
        }

        public int BusinessId { get; set; }
        public int SegmentId { get; set; }
        public int BusinessKey { get; set; }
        public string LocationCode { get; set; }
        public string LocationDescription { get; set; }
        public string BusinessName { get; set; }
        public byte[] EBusinessKey { get; set; }
        public int Isdcode { get; set; }
        public int CityCode { get; set; }
        public string CurrencyCode { get; set; }
        public int TaxIdentification { get; set; }
        public string ESyaLicenseType { get; set; }
        public byte[] EUserLicenses { get; set; }
        public byte[] EActiveUsers { get; set; }
        public byte[] ENoOfBeds { get; set; }
        public bool? TolocalCurrency { get; set; }
        public bool TocurrConversion { get; set; }
        public bool TorealCurrency { get; set; }
        public bool IsBookOfAccounts { get; set; }
        public int BusinessSegmentId { get; set; }
        public bool ActiveStatus { get; set; }
        public string FormId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedTerminal { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedTerminal { get; set; }

        public virtual ICollection<GtEcclco> GtEcclco { get; set; }
    }
}
