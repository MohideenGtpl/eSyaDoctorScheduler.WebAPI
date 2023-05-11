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
    public class MasterController : ControllerBase
    {
        private readonly IMasterRepository _masterRepository;

        public MasterController(IMasterRepository masterRepository)
        {
            _masterRepository = masterRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetReferredBy()
        {
            var rs = await _masterRepository.GetReferredBy();
            return Ok(rs);
        }
        [HttpGet]
        public async Task<IActionResult> GetBusinessKey()
        {
            var rs = await _masterRepository.GetBusinessKey();
            return Ok(rs);
        }
    }
}