﻿using eSyaDoctorScheduler.DL.Entities;
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
    public class MasterRepository : IMasterRepository
    {
        private eSyaEnterpriseContext _context { get; set; }
        public MasterRepository(eSyaEnterpriseContext context)
        {
            _context = context;
        }

        public async Task<List<DO_ApplicationCodes>> GetReferredBy()
        {
            var pt = _context.GtEcapcd
               .Where(w => w.CodeType == CodeTypeValues.ReferredBy && w.ActiveStatus)
               .Select(s => new DO_ApplicationCodes
               {
                   ApplicationCode = s.ApplicationCode,
                   CodeDesc = s.CodeDesc
               })
               .ToListAsync();
            return await pt;
        }
        public async Task<List<DO_BusinessLocation>> GetBusinessKey()
        {
            try
            {
                
                    var bk = _context.GtEcbsln
                        .Where(w => w.ActiveStatus)
                        .Select(r => new DO_BusinessLocation
                        {
                            BusinessKey = r.BusinessKey,
                            BusinessLocation = r.BusinessName + "-" + r.LocationDescription
                        }).ToListAsync();

                    return await bk;
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
