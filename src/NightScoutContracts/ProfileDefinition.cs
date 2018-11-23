using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Meiswinkel.NightScoutReporter.NightScoutContracts
{
    [JsonObject]
    public class ProfileDefinition
    {
        [JsonProperty(PropertyName = "time")]
        public string Time
        {
            get; set;
        }

        [JsonProperty(PropertyName = "value")]
        public decimal Value
        {
            get; set;
        }

        [JsonProperty(PropertyName = "timeAsSeconds")]
        public uint TimeAsSeconds
        {
            get; set;
        }
    }
}
