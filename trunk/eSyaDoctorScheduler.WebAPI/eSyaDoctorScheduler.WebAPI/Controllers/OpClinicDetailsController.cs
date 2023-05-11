using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eSyaDoctorScheduler.IF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eSyaDoctorScheduler.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OpClinicDetailsController : ControllerBase
    {
        private readonly IOpClinicDetailsRepository _opClinicDetailsRepository;

        public OpClinicDetailsController(IOpClinicDetailsRepository opClinicDetailsRepository)
        {
            _opClinicDetailsRepository = opClinicDetailsRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetEpisodeType()
        {
            var rs = await _opClinicDetailsRepository.GetEpisodeType();
            return Ok(rs);
        }

        [HttpGet]
        public async Task<IActionResult> GetClinicTypes(int businessKey)
        {
            var rs = await _opClinicDetailsRepository.GetClinicTypes(businessKey);
            return Ok(rs);
        }

        [HttpGet]
        public async Task<IActionResult> GetSpecialty(int businessKey)
        {
            var rs = await _opClinicDetailsRepository.GetSpecialty(businessKey);
            return Ok(rs);
        }

        [HttpGet]
        public async Task<IActionResult> GetDoctorScheduleListByClinicTypeSpecialtyDate(int businessKey,
        int clinicType, int consultationType,
        int specialtyId, DateTime scheduleDate)
        {
            var rs = await _opClinicDetailsRepository.GetDoctorScheduleListByClinicTypeSpecialtyDate(businessKey,
                    clinicType, consultationType, specialtyId, scheduleDate);
            return Ok(rs);
        }

        [HttpGet]
        public async Task<IActionResult> GetDoctorScheduleListForDoctorDateRange(int businessKey,
             int clinicType, int consultationType,
             int specialtyId, int doctorId, DateTime fromDate, DateTime toDate)
        {
            var rs = await _opClinicDetailsRepository.GetDoctorScheduleListForDoctorDateRange(businessKey,
             clinicType, consultationType,
             specialtyId, doctorId, fromDate, toDate);
            return Ok(rs);
        }

        //[HttpGet]
        //public async Task<IActionResult> GetDoctorScheduleByID(int businessKey,
        //            int doctorID, DateTime fromDate)
        //{
        //    var rs = await _opClinicDetailsRepository.GetDoctorScheduleByID(businessKey,
        //            doctorID, fromDate);
        //    return Ok(rs);
        //}

        [HttpGet]
        public async Task<IActionResult> GetDoctorListByClinicType(int businessKey,
          int clinicType, int consultationType,
          int specialtyId)
        {
            var rs = await _opClinicDetailsRepository.GetDoctorListByClinicType(businessKey,
                clinicType, consultationType,
                specialtyId);
            return Ok(rs);
        }

        [HttpGet]
        public async Task<IActionResult> GetDoctorListWeeklyScheduleTime(int businessKey,
          int clinicType, int consultationType,
          int specialtyId, int doctorId)
        {
            var rs = await _opClinicDetailsRepository.GetDoctorListWeeklyScheduleTime(businessKey,
                clinicType, consultationType,
                specialtyId, doctorId);
            return Ok(rs);
        }

    }
}