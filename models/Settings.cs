using System;
using System.Collections.Generic;
using System.Text;

namespace BaoZiLinSCKGetFeedData.models
{
    public class Settings
    {
        public string clientId { get; set; }

        public string clientSecret { get; set; }

        public string graphApiUrl { get; set; }

        public string newsFileLocation { get; set; }
    }
}
