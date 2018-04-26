using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SplashSharp.Models
{
    public class GarbageCollectionResponse
    {
        [JsonProperty("cached_args_removed")]
        public long? CachedArgsRemoved { get; set; }

        [JsonProperty("pyobjects_collected")]
        public long? PyObjectsCollected { get; set; }

        public string Status { get; set; }
    }
}
