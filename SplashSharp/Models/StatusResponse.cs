using System;
using System.Collections.Generic;
using System.Text;

namespace SplashSharp.Models
{
    public class StatusResponse
    {
        public object[] Active { get; set; }
        public int Argcache { get; set; }
        public int Fds { get; set; }
        public Leaks Leaks { get; set; }
        public int Maxrss { get; set; }
        public int Qsize { get; set; }
    }

    public class Leaks
    {
        public int Deferred { get; set; }
        public int LuaRuntime { get; set; }
        public int QTimer { get; set; }
        public int Request { get; set; }
    }

}
