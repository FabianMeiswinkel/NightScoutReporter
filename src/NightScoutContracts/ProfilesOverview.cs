using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Meiswinkel.NightScoutReporter.NightScoutContracts
{
    [JsonObject]
    public class ProfilesOverview
    {
        /// <summary>
        /// Internally assigned id.
        /// </summary>
        [JsonProperty(PropertyName = "_id")]
        public string Id
        {
            get; set;
        }

        [JsonProperty(PropertyName = "defaultProfile")]
        public string DefaultProfile
        {
            get; set;
        }

        [JsonProperty(PropertyName = "units")]
        public string Units
        {
            get; set;
        }

        [JsonProperty(PropertyName = "store")]
        public JObject Store
        {
            get; set;
        }
    }
}
