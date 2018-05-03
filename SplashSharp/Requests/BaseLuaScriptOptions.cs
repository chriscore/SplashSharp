using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SplashSharp.Requests
{
    public abstract class BaseLuaScriptOptions : BaseRenderRequestOptions
    {
        protected BaseLuaScriptOptions()
        {
            InternalProperties = new Dictionary<string, object>();
        }

        public object this[string key]
        {
            get => InternalProperties[key];
            set => InternalProperties[key] = value;
        }

        /// <summary>
        /// The Lua automation script to run
        /// </summary>
        [JsonRequired]
        public string LuaSource { get; set; }

        internal Dictionary<string, object> InternalProperties { get; set; }
    }
}
