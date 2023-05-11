using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eSyaDoctorScheduler.DO;
using eSyaDoctorScheduler.IF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eSyaDoctorScheduler.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentRepository _appointmentRepository;

        public AppointmentController(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        [HttpPost]
        public async Task<IActionResult> InsertIntoDoctorSlotBooking(DO_PatientAppointmentDetail obj)
        {
            var rs = await _appointmentRepository.InsertIntoDoctorSlotBooking(obj);
            return Ok(rs);
        }

        [HttpPost]
        public async Task<IActionResult> InsertIntoPatientAppointmentDetail(DO_PatientAppointmentDetail obj)
        {
            var rs = await _appointmentRepository.InsertIntoPatientAppointmentDetail(obj);
            return Ok(rs);
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePatientAppointmentDetail(DO_PatientAppointmentDetail obj)
        {
            var rs = await _appointmentRepository.UpdatePatientAppointmentDetail(obj);
            return Ok(rs);
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePatientAppointmentCancellation(DO_PatientAppointmentDetail obj)
        {
            var rs = await _appointmentRepository.UpdatePatientAppointmentCancellation(obj);
            return Ok(rs);
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePatientAppointmentReschedule(DO_PatientAppointmentDetail obj)
        {
            var rs = await _appointmentRepository.UpdatePatientAppointmentReschedule(obj);
            return Ok(rs);
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePatientAppointmentToUnScheduleWorkOrder(DO_PatientAppointmentDetail obj)
        {
            var rs = await _appointmentRepository.UpdatePatientAppointmentToUnScheduleWorkOrder(obj);
            return Ok(rs);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateDoctorAppointmentToUnScheduleWorkOrder(DO_PatientAppointmentDetail obj)
        {
            var rs = await _appointmentRepository.UpdateDoctorAppointmentToUnScheduleWorkOrder(obj);
            return Ok(rs);
        }

        [HttpGet]
        public async Task<IActionResult> GetPatientAppointmentByDoctorDate(int businessKey, int specialtyId,
            int doctorId, DateTime fromDate, DateTime toDate)
        {
            var rs = await _appointmentRepository.GetPatientAppointmentByDoctorDate(businessKey, specialtyId,
             doctorId, fromDate, toDate);
            return Ok(rs);
        }

        [HttpGet]
        public async Task<IActionResult> GetDoctorUnScheduleWorkOrder(int businessKey, int specialtyId,
                int doctorId)
        {
            DateTime fromDate = DateTime.Now;
            var rs = await _appointmentRepository.GetDoctorUnScheduleWorkOrder(businessKey, specialtyId,
             doctorId, fromDate);
            return Ok(rs);
        }

        [HttpGet]
        public async Task<IActionResult> GetPatientAppointmentDetailByAppkey(int businessKey, decimal appointmentKey)
        {
            var rs = await _appointmentRepository.GetPatientAppointmentDetailByAppkey(businessKey, appointmentKey);
            return Ok(rs);
        }
        [HttpGet]
        public async Task<IActionResult> GetApplicationRuleByProcesssID(int processId, int ruleId)
        {
            var rs = await _appointmentRepository.GetApplicationRuleByProcesssID(processId, ruleId);
            return Ok(rs);
        }
    }
}