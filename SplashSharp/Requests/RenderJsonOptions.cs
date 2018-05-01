using System;
using System.Collections.Generic;
using System.Text;

namespace SplashSharp.Requests
{
    public class RenderJsonOptions : RenderJpegOptions
    {
        /// <summary>
        /// Will include response HTML in output if true.
        /// Defaults to false
        /// </summary>
        public bool? Html { get; set; }

        /// <summary>
        /// Will include PNG in output if true.
        /// Defaults to false
        /// </summary>
        public bool? Png { get; set; }

        /// <summary>
        /// Will include JPEG in output if true.
        /// Defaults to false
        /// </summary>
        public bool? Jpeg { get; set; }

        /// <summary>
        /// Will include information about child iframes if true.
        /// Defaults to false
        /// </summary>
        public bool? IFrames { get; set; }

        /// <summary>
        /// Will include result of executed JavaScript final statement in output if true.
        /// Defaults to false
        /// </summary>
        public bool? Script { get; set; }
        
        /// <summary>
        /// Will include JavaScript console messages in output if true.
        /// Defaults to false
        /// </summary>
        public bool? Console { get; set; }

        /// <summary>
        /// Will include history of requests and responses for the main frame if true.
        /// Can be used to get HTTP codes and headers 
        /// Defaults to false
        /// </summary>
        public bool? History { get; set; }
        
        /// <summary>
        /// Will include HAR data in response if true.
        /// Defaults to false
        /// </summary>
        public bool? Har { get; set; }

        /// <summary>
        /// Includes response body in HAR records when true.
        /// Has no effect when Har and History are false.
        /// Defaults to false
        /// </summary>
        public bool? ResponseBody { get; set; }
    }
}
