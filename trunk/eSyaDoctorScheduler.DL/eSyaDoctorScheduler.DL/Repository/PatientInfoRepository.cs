using eSyaDoctorScheduler.DL.Entities;
using eSyaDoctorScheduler.DO;
using eSyaDoctorScheduler.IF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSyaDoctorScheduler.DL.Repository
{
    public class PatientInfoRepository : IPatientInfoRepository
    {
        private eSyaEnterpriseContext _context { get; set; }
        public PatientInfoRepository(eSyaEnterpriseContext context)
        {
            _context = context;
        }

        public async Task<List<DO_PatientProfile>> GetPatientSearch(string searchText)
        {
            try
            {
                var pf = await _context.GtEfoppr
                 .Where(w => ((w.FirstName + " " + w.LastName).ToUpper().Contains(searchText.ToUpper())
                                    || w.MobileNumber.ToString() == searchText || w.PatientId.ToString() == searchText || w.RUhid.ToString() == searchText))
                    .AsNoTracking()
                    .Select(r => new DO_PatientProfile
                    {
                        UHID = r.RUhid,
                        PatientID = r.PatientId,
                        FirstName = r.FirstName,
                        LastName = r.LastName,
                        Gender = r.Gender,
                        DateOfBirth = r.DateOfBirth,
                        MobileNumber = r.MobileNumber,
                        eMailID = r.EMailId
                    }).ToListAsync();

                return pf;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
