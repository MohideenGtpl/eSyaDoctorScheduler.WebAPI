﻿using eSyaDoctorScheduler.DO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eSyaDoctorScheduler.IF
{
    public interface IAppointmentRepository
    {
        Task<DO_ResponseParameter> InsertIntoDoctorSlotBooking(DO_PatientAppointmentDetail obj);
        Task<DO_ResponseParameter> InsertIntoPatientAppointmentDetail(DO_PatientAppointmentDetail obj);
        Task<DO_ResponseParameter> UpdatePatientAppointmentDetail(DO_PatientAppointmentDetail obj);
        Task<DO_ResponseParameter> UpdatePatientAppointmentCancellation(DO_PatientAppointmentDetail obj);
        Task<DO_ResponseParameter> UpdatePatientAppointmentReschedule(DO_PatientAppointmentDetail obj);
        Task<DO_ResponseParameter> UpdatePatientAppointmentToUnScheduleWorkOrder(DO_PatientAppointmentDetail obj);
        Task<DO_ResponseParameter> UpdateDoctorAppointmentToUnScheduleWorkOrder(DO_PatientAppointmentDetail obj);
        Task<List<DO_PatientAppointmentDetail>> GetPatientAppointmentByDoctorDate(int businessKey, int specialtyId,
            int doctorId, DateTime fromDate, DateTime toDate);
        Task<List<DO_PatientAppointmentDetail>> GetDoctorUnScheduleWorkOrder(int businessKey, int specialtyId,
               int doctorId, DateTime fromDate);
        Task<DO_PatientAppointmentDetail> GetPatientAppointmentDetailByAppkey(int businessKey, decimal appointmentKey);
        Task<DO_ApplicationRulesStatus> GetApplicationRuleByProcesssID(int processId, int ruleId);
    }
}
