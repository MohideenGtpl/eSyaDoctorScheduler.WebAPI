using System;
using System.Collections.Generic;
using System.Text;

namespace eSyaDoctorScheduler.DO
{
    public class DO_ResponseParameter
    {
        public bool Status { get; set; }
        public string StatusCode { get; set; }
        public string Message { get; set; }
        public bool Warning { get; set; }
        public string WarningMessage { get; set; }
        public string ErrorCode { get; set; }
        public int ID { get; set; }
        public long Key { get; set; }

        public string strKey { get; set; }
    }
}
