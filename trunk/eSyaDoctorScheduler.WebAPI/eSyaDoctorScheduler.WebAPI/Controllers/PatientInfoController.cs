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
    public class PatientInfoController : ControllerBase
    {
        private readonly IPatientInfoRepository _patientInfoRepository;

        public PatientInfoController(IPatientInfoRepository patientInfoRepository)
        {
            _patientInfoRepository = patientInfoRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetPatientSearch(string searchText)
        {
            var rs = await _patientInfoRepository.GetPatientSearch(searchText);
            return Ok(rs);
        }
    }
}