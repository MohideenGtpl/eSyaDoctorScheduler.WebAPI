using eSyaDoctorScheduler.DL.Entities;
using eSyaDoctorScheduler.DO;
using eSyaDoctorScheduler.DO.StaticVariables;
using eSyaDoctorScheduler.IF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSyaDoctorScheduler.DL.Repository
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private eSyaEnterpriseContext _context { get; set; }
        public AppointmentRepository(eSyaEnterpriseContext context)
        {
            _context = context;
        }

        public async Task<DO_ResponseParameter> InsertIntoDoctorSlotBooking(DO_PatientAppointmentDetail obj)
        {

            try
            {
                bool warning = false;
                string warningMessage = "";

                var wk = obj.AppointmentDate.Date.DayOfWeek.ToString();
                var wk_No = CommonMethod.GetWeekOfMonth(obj.AppointmentDate.Date);
                var dc = await _context.GtEsdos1.Where(w => w.BusinessKey == obj.BusinessKey
                            && w.DoctorId == obj.DoctorID
                            && w.DayOfWeek.ToUpper() == wk.ToUpper()
                            && ((wk_No == 1 && w.Week1) || (wk_No == 2 && w.Week2)
                                || (wk_No == 3 && w.Week3) || (wk_No == 4 && w.Week4)
                                || (wk_No == 5 && w.Week5) || (wk_No == 6 && w.Week5))
                            && (bool)w.ActiveStatus).FirstOrDefaultAsync();

                if (dc != null && dc.NoOfPatients > 0)
                {
                    var patientBooked = await _context.GtEopaph.Where(w => w.BusinessKey == obj.BusinessKey
                           && w.DoctorId == obj.DoctorID
                           && w.AppointmentDate.Date == obj.AppointmentDate.Date
                           && !w.UnScheduleWorkOrder
                           && w.AppointmentStatus !=  StatusVariables.Appointment.Cancelled).CountAsync();

                    if (patientBooked >= dc.NoOfPatients)
                    {
                        warning = true;
                        warningMessage = "Patient Limit for the day is " + dc.NoOfPatients.ToString() + ". Already booked patient count is " + patientBooked;
                    }
                }

                var endTimeSlot = obj.AppointmentFromTime.Add(new TimeSpan(0, obj.Duration, 0));

                var is_TimeSlotExits = await _context.GtEopaph.Where(w => w.BusinessKey == obj.BusinessKey
                            && w.DoctorId == obj.DoctorID
                            && w.AppointmentDate.Date == obj.AppointmentDate.Date
                            && w.AppointmentFromTime >= obj.AppointmentFromTime
                            && w.AppointmentFromTime < endTimeSlot
                            && !w.UnScheduleWorkOrder
                            && w.AppointmentStatus != StatusVariables.Appointment.Cancelled).CountAsync();
                if (is_TimeSlotExits > 0)
                {
                    return new DO_ResponseParameter() { Warning = warning, WarningMessage = warningMessage, Status = false, Message = "The Slot has been already booked" };
                }

                var slotBlocked = await _context.GtEopaps.Where(w => w.BusinessKey == obj.BusinessKey
                           && w.DoctorId == obj.DoctorID
                           && w.AppointmentDate.Date == obj.AppointmentDate.Date
                           && w.AppointmentFromTime >= obj.AppointmentFromTime
                           && w.AppointmentFromTime < endTimeSlot
                           && w.AppointmentStatus == "SL"
                           && w.CreatedBy != obj.UserID
                           && w.CreatedOn.AddMinutes(2) > System.DateTime.Now).CountAsync();
                if (slotBlocked > 0)
                {
                    return new DO_ResponseParameter() { Warning = warning, WarningMessage = warningMessage, Status = false, Message = "The Slot has been blocked." };
                }

                var timeSlotDuration = await _context.GtEsdocd.Where(w => w.DoctorId == obj.DoctorID).Select(s=>s.TimeSlotInMintues).FirstOrDefaultAsync();
                obj.Duration = timeSlotDuration;

                var qs_apSL = new GtEopaps
                {
                    BusinessKey = obj.BusinessKey,
                    SpecialtyId = obj.SpecialtyID,
                    DoctorId = obj.DoctorID,
                    AppointmentDate = obj.AppointmentDate,
                    AppointmentFromTime = obj.AppointmentFromTime,
                    Duration = obj.Duration,
                    AppointmentStatus = "SL",
                    ActiveStatus = true,
                    FormId = obj.FormID,
                    CreatedBy = obj.UserID,
                    CreatedOn = System.DateTime.Now,
                    CreatedTerminal = obj.TerminalID
                };
                _context.GtEopaps.Add(qs_apSL);
                await _context.SaveChangesAsync();

                return new DO_ResponseParameter { Warning = warning, WarningMessage = warningMessage, Status = true };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ResponseParameter> InsertIntoPatientAppointmentDetail(DO_PatientAppointmentDetail obj)
        {
            var dbContext = _context.Database.BeginTransaction();
            {
                try
                {
                    var endTimeSlot = obj.AppointmentFromTime.Add(new TimeSpan(0, obj.Duration, 0));

                    var is_TimeSlotExits = await _context.GtEopaph.Where(w => w.BusinessKey == obj.BusinessKey
                                && w.DoctorId == obj.DoctorID
                                && w.AppointmentDate.Date == obj.AppointmentDate.Date
                                && w.AppointmentFromTime >= obj.AppointmentFromTime
                                && w.AppointmentFromTime < endTimeSlot
                                && !w.UnScheduleWorkOrder
                                && w.AppointmentStatus != StatusVariables.Appointment.Cancelled).CountAsync();
                    if (is_TimeSlotExits > 0)
                    {
                        return new DO_ResponseParameter() { Status = false, Message = "The Slot has been already booked" };
                    }

                    if (obj.UHID > 0)
                    {
                        var isAlreadyBooked = await _context.GtEopaph
                            .Join(_context.GtEopapd,
                                h => new { h.BusinessKey, h.AppointmentKey },
                                d => new { d.BusinessKey, d.AppointmentKey },
                                (h, d) => new { h, d })
                            .Where(w => w.h.BusinessKey == obj.BusinessKey
                                    && w.d.Uhid == obj.UHID
                                    && w.h.DoctorId != obj.DoctorID
                                    && w.h.AppointmentDate.Date == obj.AppointmentDate.Date
                                    && w.h.AppointmentFromTime >= obj.AppointmentFromTime
                                    && w.h.AppointmentFromTime < endTimeSlot
                                    && !w.h.UnScheduleWorkOrder
                                    && w.h.AppointmentStatus != StatusVariables.Appointment.Cancelled
                                ).CountAsync();
                        if (isAlreadyBooked > 0)
                        {
                            return new DO_ResponseParameter() { Status = false, Message = "The Same MRN is booked for different doctor for same time slot." };
                        }
                    }

                    var financialYear = _context.GtEcclco.Where(w =>
                                                        w.BusinessKey == obj.BusinessKey
                                                    && DateTime.Now.Date >= w.FromDate.Date
                                                    && DateTime.Now.Date <= w.TillDate.Date)
                                        .First().FinancialYear;
                    obj.FinancialYear = (int)financialYear;

                    var dc_ap = await _context.GtDncn01
                                    .Where(w => w.BusinessKey == obj.BusinessKey
                                        && w.FinancialYear == financialYear
                                        && w.DocumentId == DocumentIdValues.op_AppointmentId).FirstOrDefaultAsync();
                    dc_ap.CurrentDocNumber = dc_ap.CurrentDocNumber + 1;
                    dc_ap.CurrentDocDate = DateTime.Now;
                    await _context.SaveChangesAsync();

                    obj.DocumentID = dc_ap.DocumentId;
                    obj.DocumentNumber = dc_ap.CurrentDocNumber;

                    var appointmentKey = long.Parse(obj.FinancialYear.ToString().Substring(2, 2) +
                        obj.BusinessKey.ToString().PadLeft(2, '0') +
                        dc_ap.DocumentId.ToString().PadLeft(3, '0') +
                        obj.DocumentNumber.ToString());

                    obj.AppointmentKey = appointmentKey;

                    var timeSlotDuration = await _context.GtEsdocd.Where(w => w.DoctorId == obj.DoctorID).Select(s => s.TimeSlotInMintues).FirstOrDefaultAsync();
                    obj.Duration = timeSlotDuration;

                    string appType = "CA";
                    if (obj.IsSponsored)
                        appType = "SA";

                    var rp = await GetAppointmentQueueToken(_context, obj);
                    if (!rp.Status)
                        return rp;
                    var qTokenKey = obj.QueueTokenKey;

                    var qs_apTk = new GtEopapq
                    {
                        BusinessKey = obj.BusinessKey,
                        TokenDate = obj.AppointmentDate.Date,
                        QueueTokenKey = qTokenKey,
                        AppointmentFromTime = obj.AppointmentFromTime,
                        AppointmentKey = obj.AppointmentKey,
                        SequeueNumber = 1,
                        PatientType = appType,
                        SpecialtyId = obj.SpecialtyID,
                        DoctorId = obj.DoctorID,
                        Uhid = obj.UHID,
                        TokenStatus = StatusVariables.Appointment.Booked,
                        ActiveStatus = true,
                        FormId = obj.FormID,
                        CreatedBy = obj.UserID,
                        CreatedOn = DateTime.Now,
                        CreatedTerminal = obj.TerminalID
                    };
                    _context.GtEopapq.Add(qs_apTk);
                    await _context.SaveChangesAsync();

                    var app_hd = new GtEopaph
                    {
                        BusinessKey = obj.BusinessKey,
                        FinancialYear = obj.FinancialYear,
                        DocumentId = obj.DocumentID,
                        DocumentNumber = obj.DocumentNumber,
                        AppointmentKey = obj.AppointmentKey,
                        ClinicId = obj.ClinicId,
                        ConsultationId = obj.ConsultationId,
                        SpecialtyId = obj.SpecialtyID,
                        DoctorId = obj.DoctorID,
                        AppointmentDate = obj.AppointmentDate,
                        AppointmentFromTime = obj.AppointmentFromTime,
                        Duration = obj.Duration,
                        AppointmentStatus = StatusVariables.Appointment.Booked,
                        ReasonforAppointment = obj.ReasonforAppointment,
                        QueueTokenKey = qTokenKey,
                        VisitType = obj.VisitType,
                        EpisodeType = obj.EpisodeType,
                        ReferredBy = obj.ReferredBy,
                        UnScheduleWorkOrder = false,
                        ActiveStatus = true,
                        FormId = obj.FormID,
                        CreatedBy = obj.UserID,
                        CreatedOn = DateTime.Now,
                        CreatedTerminal = obj.TerminalID
                    };
                    _context.GtEopaph.Add(app_hd);
                    await _context.SaveChangesAsync();

                    var app_dt = new GtEopapd
                    {
                        BusinessKey = obj.BusinessKey,
                        AppointmentKey = obj.AppointmentKey,
                        Uhid = obj.UHID,
                        PatientFirstName = obj.PatientFirstName,
                        PatientMiddleName = obj.PatientMiddleName,
                        PatientLastName = obj.PatientLastName,
                        Gender = obj.Gender,
                        DateOfBirth = obj.DateOfBirth,
                        MobileNumber = obj.PatientMobileNumber,
                        SecondaryMobileNumber = obj.SecondaryMobileNumber,
                        EmailId = obj.PatientEmailID,
                        IsSponsored = obj.IsSponsored,
                        CustomerId = obj.CustomerID,
                        ActiveStatus = true,
                        FormId = obj.FormID,
                        CreatedBy = obj.UserID,
                        CreatedOn = DateTime.Now,
                        CreatedTerminal = obj.TerminalID
                    };
                    _context.GtEopapd.Add(app_dt);

                    await _context.SaveChangesAsync();
                    dbContext.Commit();

                    return new DO_ResponseParameter { Status = true, strKey = obj.QueueTokenKey };
                }
                catch (DbUpdateException ex)
                {
                    dbContext.Rollback();
                    throw ex;
                }
                catch (Exception ex)
                {
                    dbContext.Rollback();
                    throw ex;
                }
            }
        }

        public async Task<DO_ResponseParameter> GetAppointmentQueueToken(eSyaEnterpriseContext db, DO_PatientAppointmentDetail obj)
        {
         
            var doctorCode = obj.DoctorID.ToString();

            string appType = "CA";
            if (obj.IsSponsored)
                appType = "SA";

            TimeSpan totalDuration = new TimeSpan();
            var appointmentSchedule = await GetDoctorScheduleByID(obj.BusinessKey, obj.DoctorID, obj.AppointmentDate);
            foreach (var s in appointmentSchedule)
            {
                if (s.FromTime <= obj.AppointmentFromTime && s.ToTime >= obj.AppointmentFromTime)
                {
                    totalDuration += obj.AppointmentFromTime.Subtract((TimeSpan) s.FromTime);
                    break;
                }
                else
                    totalDuration += ((TimeSpan)s.ToTime).Subtract((TimeSpan)s.FromTime);
            }

            var totalIntervalinMinutes = totalDuration.Days * 24 * 60 +
                                         totalDuration.Hours * 60 +
                                         totalDuration.Minutes;
            var slotQueue = Math.Ceiling(totalIntervalinMinutes / Convert.ToDecimal(obj.Duration)) + 1;

            while (true)
            {
                var qTokenKey = doctorCode + "-" + slotQueue.ToString().PadLeft(2, '0');

                var q_exists = await db.GtEopapq.Where(w => w.BusinessKey == obj.BusinessKey && w.DoctorId == obj.DoctorID
                    && w.TokenDate.Date == obj.AppointmentDate.Date
                    && w.QueueTokenKey == qTokenKey
                    ).FirstOrDefaultAsync();
                if (q_exists != null)
                {
                    slotQueue++;
                    continue;
                }
                else
                {
                    obj.QueueTokenKey = qTokenKey;
                    break;
                }
            }

            return new DO_ResponseParameter
            {
                Status = true
            };
        }

        public async Task<List<DO_DoctorClinicSchedule>> GetDoctorScheduleByID(int businessKey,
                   int doctorID, DateTime fromDate)
        {

            List<DO_DoctorClinicSchedule> l_sc = new List<DO_DoctorClinicSchedule>();
            var wk = fromDate.DayOfWeek.ToString();
            var wk_No = CommonMethod.GetWeekOfMonth(fromDate);

            var l_ds_1 = await _context.GtEsdocd
                    .GroupJoin(_context.GtEsdos1.Where(w => w.DayOfWeek.ToUpper() == wk.ToUpper()
                            && ((wk_No == 1 && w.Week1) || (wk_No == 2 && w.Week2)
                                || (wk_No == 3 && w.Week3) || (wk_No == 4 && w.Week4)
                                || (wk_No == 5 && w.Week5) || (wk_No == 6 && w.Week5))
                            && w.ActiveStatus),
                        d => d.DoctorId,
                        s => s.DoctorId,
                        (d, s) => new { d, s }).DefaultIfEmpty()
                    .SelectMany(d => d.s.DefaultIfEmpty(), (d, s) => new { d.d, s })
                    .GroupJoin(_context.GtEsdold.Where(w =>
                            w.ActiveStatus),
                        ds => ds.d.DoctorId,
                        l => l.DoctorId,
                        (ds, l) => new { ds, l = l.FirstOrDefault() }).DefaultIfEmpty()
                    .Where(w => w.ds.d.ActiveStatus && w.ds.d.DoctorId == doctorID
                            && !_context.GtEsdos2.Any(r => r.BusinessKey == businessKey
                                   && r.DoctorId == doctorID
                                   && r.ScheduleDate.Date == fromDate.Date
                                   && r.ActiveStatus))
                    .AsNoTracking()
                    .Select(x => new DO_DoctorClinicSchedule
                    {
                        DoctorId = x.ds.d.DoctorId,
                        DoctorName = x.ds.d.DoctorName,
                        DoctorRemarks = x.ds.d.DoctorRemarks,
                        DayOfWeek = x.ds.s != null ? x.ds.s.DayOfWeek : "",
                        ScheduleDate = fromDate,
                        NumberofPatients = x.ds.s != null ? x.ds.s.NoOfPatients : 0,
                        FromTime = x.ds.s != null ? x.ds.s.ScheduleFromTime : new TimeSpan(9, 00, 00),
                        ToTime = x.ds.s != null ? x.ds.s.ScheduleToTime : new TimeSpan(18, 00, 00),
                        IsAvailable = x.ds.s != null ? true : false,
                        IsOnLeave = x.l != null ? x.l.ActiveStatus : false
                    }).OrderBy(o => o.FromTime).ToListAsync();


            var l_ds_2 = await _context.GtEsdocd
                  .GroupJoin(_context.GtEsdos2.Where(w => w.BusinessKey == businessKey
                          && w.ScheduleDate.Date == fromDate.Date
                          && w.ActiveStatus),
                      d => d.DoctorId,
                      s => s.DoctorId,
                      (d, s) => new { d, s = s.FirstOrDefault() }).DefaultIfEmpty()
                  .GroupJoin(_context.GtEsdold.Where(w =>
                          w.ActiveStatus),
                      ds => ds.d.DoctorId,
                      l => l.DoctorId,
                      (ds, l) => new { ds, l = l.FirstOrDefault() }).DefaultIfEmpty()
              .Where(w => w.ds.d.ActiveStatus && w.ds.d.DoctorId == doctorID)
              .AsNoTracking()
              .Select(x => new DO_DoctorClinicSchedule
              {
                  DoctorId = x.ds.d.DoctorId,
                  DoctorName = x.ds.d.DoctorName,
                  DoctorRemarks = x.ds.d.DoctorRemarks,
                  DayOfWeek = "",
                  ScheduleDate = fromDate,
                  NumberofPatients = x.ds.s != null ? x.ds.s.NoOfPatients : 0,
                  FromTime = x.ds.s != null ? x.ds.s.ScheduleFromTime : new TimeSpan(9, 00, 00),
                  ToTime = x.ds.s != null ? x.ds.s.ScheduleToTime : new TimeSpan(18, 00, 00),
                  IsAvailable = x.ds.s != null ? true : false,
                  IsOnLeave = x.l != null ? x.l.ActiveStatus : false
              }).OrderBy(o => o.FromTime).ToListAsync();

            l_sc = l_ds_1.Union(l_ds_2).ToList();

            return l_sc;
        }


        public async Task<DO_ResponseParameter> UpdatePatientAppointmentDetail(DO_PatientAppointmentDetail obj)
        {
            using (var dbContext = _context.Database.BeginTransaction())
            {
                try
                {
                    var app_hd = await _context.GtEopaph.Where(w => w.BusinessKey == obj.BusinessKey && w.AppointmentKey == obj.AppointmentKey).FirstOrDefaultAsync();
                    if (app_hd.AppointmentStatus != StatusVariables.Appointment.Booked)
                    {
                        return new DO_ResponseParameter { Status = false, Message = "please check the appointment status." };
                    }

                    var app_dt = await _context.GtEopapd.Where(w => w.BusinessKey == obj.BusinessKey && w.AppointmentKey == obj.AppointmentKey).FirstOrDefaultAsync();

                    app_dt.PatientFirstName = obj.PatientFirstName;
                    app_dt.PatientLastName = obj.PatientLastName;
                    app_dt.Gender = obj.Gender;
                    app_dt.DateOfBirth = obj.DateOfBirth;
                    app_dt.MobileNumber = obj.PatientMobileNumber;
                    app_dt.SecondaryMobileNumber = obj.SecondaryMobileNumber;
                    app_dt.EmailId = obj.PatientEmailID;
                    app_dt.IsSponsored = obj.IsSponsored;

                    app_dt.ModifiedBy = obj.UserID;
                    app_dt.ModifiedOn = System.DateTime.Now;
                    app_dt.ModifiedTerminal = obj.TerminalID;

                    await _context.SaveChangesAsync();
                    dbContext.Commit();

                    return new DO_ResponseParameter { Status = true, Key = obj.AppointmentKey };
                }
                catch (DbUpdateException ex)
                {
                    dbContext.Rollback();
                    throw ex;
                }
                catch (Exception ex)
                {
                    dbContext.Rollback();
                    throw ex;
                }
            }
        }

        public async Task<DO_ResponseParameter> UpdatePatientAppointmentCancellation(DO_PatientAppointmentDetail obj)
        {
            using (var dbContext = _context.Database.BeginTransaction())
            {
                try
                {
                    var app_hd = await _context.GtEopaph.Where(w => w.BusinessKey == obj.BusinessKey && w.AppointmentKey == obj.AppointmentKey).FirstOrDefaultAsync();

                    if (app_hd.AppointmentStatus !=  StatusVariables.Appointment.Booked)
                    {
                        return new DO_ResponseParameter { Status = false, Message = "please check the appointment status." };
                    }

                    app_hd.QueueTokenKey = null;
                    app_hd.AppointmentStatus = StatusVariables.Appointment.Cancelled;
                    app_hd.ReasonforCancellation = obj.ReasonforCancellation;

                    app_hd.ModifiedBy = obj.UserID;
                    app_hd.ModifiedOn = DateTime.Now;
                    app_hd.ModifiedTerminal = obj.TerminalID;

                    var app_q = await _context.GtEopapq.Where(w => w.BusinessKey == obj.BusinessKey && w.AppointmentKey == obj.AppointmentKey).FirstOrDefaultAsync();
                    if (app_q != null)
                        _context.GtEopapq.Remove(app_q);

                    await _context.SaveChangesAsync();
                    dbContext.Commit();

                    return new DO_ResponseParameter { Status = true, Key = obj.AppointmentKey };
                }
                catch (DbUpdateException ex)
                {
                    dbContext.Rollback();
                    throw ex;
                }
                catch (Exception ex)
                {
                    dbContext.Rollback();
                    throw ex;
                }
            }
        }

        public async Task<DO_ResponseParameter> UpdatePatientAppointmentReschedule(DO_PatientAppointmentDetail obj)
        {
            using (var dbContext = _context.Database.BeginTransaction())
            {
                try
                {
                    var app_hd = await _context.GtEopaph.Where(w => w.BusinessKey == obj.BusinessKey
                                    && w.AppointmentKey == obj.AppointmentKey).FirstOrDefaultAsync();

                    if (app_hd.AppointmentStatus ==  StatusVariables.Appointment.Registered)
                        return new DO_ResponseParameter { Status = false, Message = "Registration is done for this Appointment." };

                    if (app_hd.AppointmentStatus != StatusVariables.Appointment.Booked)
                        return new DO_ResponseParameter { Status = false, Message = "please check the appointment status." };

                    if (app_hd.SpecialtyId != obj.SpecialtyID)
                        return new DO_ResponseParameter { Status = false, Message = "Rescheduling is available within specialty" };

                    if (app_hd.DoctorId != obj.DoctorID && app_hd.AppointmentDate.Date != obj.AppointmentDate.Date)
                        return new DO_ResponseParameter { Status = false, Message = "Doctor change is available for same day only" };

                    var app_dt = await _context.GtEopapd.Where(w => w.BusinessKey == obj.BusinessKey
                                && w.AppointmentKey == obj.AppointmentKey).FirstOrDefaultAsync();

                    app_hd.QueueTokenKey = null;
                    var app_q = await _context.GtEopapq.Where(w => w.BusinessKey == obj.BusinessKey && w.AppointmentKey == obj.AppointmentKey).FirstOrDefaultAsync();
                    if (app_q != null)
                        _context.GtEopapq.Remove(app_q);

                    string appType = "CA";
                    if (app_dt.IsSponsored)
                        appType = "SA";

                    obj.Duration = app_hd.Duration;

                    var rp = await GetAppointmentQueueToken(_context, obj);
                    if (!rp.Status)
                        return rp;
                    var qTokenKey = obj.QueueTokenKey;

                    var qs_apTk = new GtEopapq
                    {
                        BusinessKey = obj.BusinessKey,
                        TokenDate = obj.AppointmentDate.Date,
                        QueueTokenKey = qTokenKey,
                        AppointmentKey = obj.AppointmentKey,
                        AppointmentFromTime = obj.AppointmentFromTime,
                        SequeueNumber = 1,
                        PatientType = appType,
                        SpecialtyId = obj.SpecialtyID,
                        DoctorId = obj.DoctorID,
                        Uhid = app_dt.Uhid,
                        TokenStatus = StatusVariables.Appointment.Booked,
                        ActiveStatus = true,
                        FormId = obj.FormID,
                        CreatedBy = obj.UserID,
                        CreatedOn = System.DateTime.Now,
                        CreatedTerminal = obj.TerminalID
                    };
                    _context.GtEopapq.Add(qs_apTk);

                    app_hd.QueueTokenKey = qTokenKey;

                    app_hd.DoctorId = obj.DoctorID;
                    app_hd.AppointmentDate = obj.AppointmentDate;
                    app_hd.AppointmentFromTime = obj.AppointmentFromTime;

                    if (obj.Duration > 0)
                        app_hd.Duration = obj.Duration;

                    app_hd.UnScheduleWorkOrder = false;

                    app_hd.ModifiedBy = obj.UserID;
                    app_hd.ModifiedOn = System.DateTime.Now;
                    app_hd.ModifiedTerminal = obj.TerminalID;

                    await _context.SaveChangesAsync();
                    dbContext.Commit();

                    return new DO_ResponseParameter { Status = true, strKey = app_hd.QueueTokenKey };
                }
                catch (DbUpdateException ex)
                {
                    dbContext.Rollback();
                    throw ex;
                }
                catch (Exception ex)
                {
                    dbContext.Rollback();
                    throw ex;
                }
            }
        }

        public async Task<DO_ResponseParameter> UpdatePatientAppointmentToUnScheduleWorkOrder(DO_PatientAppointmentDetail obj)
        {
            using (var dbContext = _context.Database.BeginTransaction())
            {
                try
                {
                    var app_hd = await _context.GtEopaph.Where(w => w.BusinessKey == obj.BusinessKey && w.AppointmentKey == obj.AppointmentKey).FirstOrDefaultAsync();

                    if (app_hd.AppointmentStatus ==  StatusVariables.Appointment.Registered)
                        return new DO_ResponseParameter { Status = false, Message = "Registration is done for this Appointment." };

                    if (app_hd.AppointmentStatus != StatusVariables.Appointment.Booked)
                        return new DO_ResponseParameter { Status = false, Message = "please check the appointment status." };

                    app_hd.UnScheduleWorkOrder = true;
                    app_hd.QueueTokenKey = null;
                    var app_q = await _context.GtEopapq.Where(w => w.BusinessKey == obj.BusinessKey && w.AppointmentKey == obj.AppointmentKey).FirstOrDefaultAsync();
                    if (app_q != null)
                        _context.GtEopapq.Remove(app_q);

                    app_hd.ModifiedBy = obj.UserID;
                    app_hd.ModifiedOn = System.DateTime.Now;
                    app_hd.ModifiedTerminal = obj.TerminalID;

                    await _context.SaveChangesAsync();
                    dbContext.Commit();

                    return new DO_ResponseParameter { Status = true, Key = obj.AppointmentKey };
                }
                catch (DbUpdateException ex)
                {
                    dbContext.Rollback();
                    throw ex;
                }
                catch (Exception ex)
                {
                    dbContext.Rollback();
                    throw ex;
                }
            }
        }

        public async Task<DO_ResponseParameter> UpdateDoctorAppointmentToUnScheduleWorkOrder(DO_PatientAppointmentDetail obj)
        {
            using (var dbContext = _context.Database.BeginTransaction())
            {
                try
                {
                    var l_app = _context.GtEopaph.Where(w => w.BusinessKey == obj.BusinessKey && w.SpecialtyId == obj.SpecialtyID
                                && w.DoctorId == obj.DoctorID && w.AppointmentDate.Date == obj.AppointmentDate.Date
                                && w.AppointmentStatus == StatusVariables.Appointment.Booked);

                    foreach (var app_hd in l_app)
                    {
                        app_hd.UnScheduleWorkOrder = true;
                        app_hd.QueueTokenKey = null;
                        app_hd.ModifiedBy = obj.UserID;
                        app_hd.ModifiedOn = System.DateTime.Now;
                        app_hd.ModifiedTerminal = obj.TerminalID;

                        var app_q = await _context.GtEopapq.Where(w => w.BusinessKey == app_hd.BusinessKey && w.AppointmentKey == app_hd.AppointmentKey).FirstOrDefaultAsync();
                        if (app_q != null)
                            _context.GtEopapq.Remove(app_q);

                    }

                    await _context.SaveChangesAsync();
                    dbContext.Commit();

                    return new DO_ResponseParameter { Status = true };
                }
                catch (DbUpdateException ex)
                {
                    dbContext.Rollback();
                    throw ex;
                }
                catch (Exception ex)
                {
                    dbContext.Rollback();
                    throw ex;
                }
            }
        }

        public async Task<List<DO_PatientAppointmentDetail>> GetPatientAppointmentByDoctorDate(int businessKey, int specialtyId,
            int doctorId, DateTime fromDate, DateTime toDate)
        {
            try
            {
                var ds = await _context.GtEopaph
                 .Join(_context.GtEopapd,
                     h => new { h.BusinessKey, h.AppointmentKey },
                     d => new { d.BusinessKey, d.AppointmentKey },
                     (h, d) => new { h, d })
                    .Where(w =>
                                w.h.BusinessKey == businessKey
                                && (specialtyId == 0 || w.h.SpecialtyId == specialtyId)
                                && (doctorId == 0 || w.h.DoctorId == doctorId)
                                && w.h.AppointmentDate.Date >= fromDate.Date
                                && w.h.AppointmentDate.Date <= toDate.Date
                                && w.h.AppointmentStatus != StatusVariables.Appointment.Cancelled
                                && !w.h.UnScheduleWorkOrder
                                && w.h.ActiveStatus && w.d.ActiveStatus
                                )
                    .AsNoTracking()
                    .Select(r => new DO_PatientAppointmentDetail
                    {
                        AppointmentKey = r.h.AppointmentKey,
                        AppointmentDate = r.h.AppointmentDate,
                        AppointmentFromTime = r.h.AppointmentFromTime,
                        Duration = r.h.Duration,
                        StartDate = r.h.AppointmentDate.Date.Add(r.h.AppointmentFromTime),
                        EndDate = r.h.AppointmentDate.Date.Add(r.h.AppointmentFromTime).AddMinutes(r.h.Duration),
                        PatientFirstName = r.d.PatientFirstName,
                        PatientLastName = r.d.PatientLastName,
                        Gender = r.d.Gender,
                        DateOfBirth = r.d.DateOfBirth,
                        PatientMobileNumber = r.d.MobileNumber,
                        SecondaryMobileNumber = r.d.SecondaryMobileNumber,
                        PatientEmailID = r.d.EmailId,
                        IsSponsored = r.d.IsSponsored,
                        QueueTokenKey = r.h.QueueTokenKey,
                        AppointmentStatus = r.h.AppointmentStatus

                    }).ToListAsync();

                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DO_PatientAppointmentDetail>> GetDoctorUnScheduleWorkOrder(int businessKey, int specialtyId,
                int doctorId, DateTime fromDate)
        {
            try
            {
                var ds = await _context.GtEopaph
                 .Join(_context.GtEopapd,
                     h => new { h.BusinessKey, h.AppointmentKey },
                     d => new { d.BusinessKey, d.AppointmentKey },
                     (h, d) => new { h, d })
                    .Where(w => w.h.BusinessKey == businessKey && w.h.SpecialtyId == specialtyId
                                && w.h.DoctorId == doctorId
                                && w.h.AppointmentDate.Date >= fromDate.Date
                                && w.h.AppointmentStatus == StatusVariables.Appointment.Booked
                                && w.h.UnScheduleWorkOrder
                                && w.h.ActiveStatus && w.d.ActiveStatus)
                    .AsNoTracking()
                    .Select(r => new DO_PatientAppointmentDetail
                    {
                        AppointmentKey = r.h.AppointmentKey,
                        AppointmentDate = r.h.AppointmentDate,
                        AppointmentFromTime = r.h.AppointmentFromTime,
                        Duration = r.h.Duration,
                        StartDate = r.h.AppointmentDate.Date.Add(r.h.AppointmentFromTime),
                        EndDate = r.h.AppointmentDate.Date.Add(r.h.AppointmentFromTime).AddMinutes(r.h.Duration),
                        PatientFirstName = r.d.PatientFirstName,
                        PatientLastName = r.d.PatientLastName,
                        Gender = r.d.Gender,
                        DateOfBirth = r.d.DateOfBirth,
                        PatientMobileNumber = r.d.MobileNumber,
                        PatientEmailID = r.d.EmailId,
                        IsSponsored = r.d.IsSponsored,
                        QueueTokenKey = r.h.QueueTokenKey
                    }).ToListAsync();

                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_PatientAppointmentDetail> GetPatientAppointmentDetailByAppkey(int businessKey, decimal appointmentKey)
        {
            try
            {

                var ds = await _context.GtEopaph
                 .Join(_context.GtEopapd,
                     h => new { h.BusinessKey, h.AppointmentKey },
                     d => new { d.BusinessKey, d.AppointmentKey },
                     (h, d) => new { h, d })
                    .Where(w => w.h.BusinessKey == businessKey
                            && w.h.AppointmentKey == appointmentKey
                            && w.h.ActiveStatus && w.d.ActiveStatus)
                    .AsNoTracking()
                    .Select(r => new DO_PatientAppointmentDetail
                    {
                        AppointmentKey = r.h.AppointmentKey,
                        AppointmentDate = r.h.AppointmentDate,
                        AppointmentFromTime = r.h.AppointmentFromTime,
                        Duration = r.h.Duration,
                        StartDate = r.h.AppointmentDate.Date.Add(r.h.AppointmentFromTime),
                        EndDate = r.h.AppointmentDate.Date.Add(r.h.AppointmentFromTime).AddMinutes(r.h.Duration),
                        UHID = r.d.Uhid,
                        PatientFirstName = r.d.PatientFirstName,
                        PatientLastName = r.d.PatientLastName,
                        Gender = r.d.Gender,
                        DateOfBirth = r.d.DateOfBirth,
                        PatientMobileNumber = r.d.MobileNumber,
                        SecondaryMobileNumber = r.d.SecondaryMobileNumber,
                        PatientEmailID = r.d.EmailId,
                        IsSponsored = r.d.IsSponsored,
                        CustomerID = r.d.CustomerId,
                        QueueTokenKey = r.h.QueueTokenKey,
                        PatientID = r.d.PatientId,
                        VisitType = r.h.VisitType,
                        EpisodeType = r.h.EpisodeType,
                        ReferredBy = r.h.ReferredBy,
                        DoctorID = r.h.DoctorId,
                        ReasonforAppointment = r.h.ReasonforAppointment,
                        AppointmentStatus = r.h.AppointmentStatus

                    }).FirstOrDefaultAsync();

                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ApplicationRulesStatus> GetApplicationRuleByProcesssID(int processId,int ruleId)
        {
            try
            {

                var ds = await _context.GtEcaprl
                    .Where(w =>w.ProcessId==processId
                            && w.RuleId == ruleId)
                    .AsNoTracking()
                    .Select(r => new DO_ApplicationRulesStatus
                    {
                        RuleId=r.RuleId,
                        ProcessId=r.ProcessId,
                        RuleDesc=r.RuleDesc,
                        IsAllow=r.ActiveStatus
                    }).FirstOrDefaultAsync();

                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
