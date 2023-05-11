using System;
using System.Collections.Generic;
using System.Text;

namespace eSyaDoctorScheduler.DO
{
    public class DO_PatientAppointmentDetail
    {
        public int BusinessKey { get; set; }
        public int FinancialYear { get; set; }
        public int DocumentID { get; set; }
        public int DocumentNumber { get; set; }
        public long AppointmentKey { get; set; }
        public int ClinicId { get; set; }
        public int ConsultationId { get; set; }
        public int SpecialtyID { get; set; }
        public string SpecialtyDesc { get; set; }
        public int DoctorID { get; set; }
        public string DoctorName { get; set; }
        public DateTime AppointmentDate { get; set; }
        public TimeSpan AppointmentFromTime { get; set; }
        public int Duration { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string AppointmentStatus { get; set; }
        public string ReasonforAppointment { get; set; }
        public string ReasonforCancellation { get; set; }
        public string QueueTokenKey { get; set; }
        public string VisitType { get; set; }
        public string EpisodeType { get; set; }
        public int ClinicType { get; set; }
        public int ConsultationType { get; set; }

        public string PatientID { get; set; }
        public long? UHID { get; set; }
        public string PatientFirstName { get; set; }
        public string PatientMiddleName { get; set; }
        public string PatientLastName { get; set; }
        public string Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string PatientMobileNumber { get; set; }
        public string SecondaryMobileNumber { get; set; }
        public string PatientEmailID { get; set; }
        public bool IsSponsored { get; set; }
        public int? CustomerID { get; set; }
        public int ReferredBy { get; set; }
        public bool PaymentReceived { get; set; }

        public int RequestChannel { get; set; }

        public string PatientName { get; set; }
        public int Age { get; set; }
        public string BMI { get; set; }
        public string SurgeryCode { get; set; }
        public string SurgeryName { get; set; }
        public DateTime SurgeryDate { get; set; }

        public bool ActiveStatus { get; set; }
        public string FormID { get; set; }
        public int UserID { get; set; }
        public string TerminalID { get; set; }
    }
}
