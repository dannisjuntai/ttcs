using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TTCS.App_Start
{
    public class Def
    {
        public const string JsonMimeType = "application/json";
        public const int MaxRetry = 3;
        public const int RetryWait = 3 * 1000;
    }
}