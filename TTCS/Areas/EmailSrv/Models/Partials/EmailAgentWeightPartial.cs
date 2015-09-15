using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
namespace TTCS.Areas.EmailSrv.Models
{
    [MetadataType(typeof(EEmailAgentWeightMetaData))]
    public partial class EEmailAgentWeight
    {
        [Display(Name = "客服人員")]
        public string AgentName{get;set;}
                
        public string AgentMailGroup { get; set; }
        private class EEmailAgentWeightMetaData
        {            
            public string AgentId { get; set; }

            [Display(Name = "權重")]
            public double Weight { get; set; }
        }
    }
}