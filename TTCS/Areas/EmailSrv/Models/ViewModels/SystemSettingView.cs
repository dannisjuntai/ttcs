using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;

namespace TTCS.Areas.EmailSrv.Models
{
    public class SystemSetting
    {
        public IEnumerable<EEmailRejectReason> EmailRejectReason { get; set; }

        public IEnumerable<ERecordCloseApproach> RecordCloseApproach { get; set; }

        public EEmailScheduleSetting EmailScheduleSetting { get; set; }
    }
}
