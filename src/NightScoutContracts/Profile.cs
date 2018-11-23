using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Meiswinkel.NightScoutReporter.NightScoutContracts
{
    [JsonObject]
    public class Profile
    {
        [JsonProperty(PropertyName = "_id")]
        public string Id
        {
            get; set;
        }

        [JsonProperty(PropertyName = "units")]
        public string Units
        {
            get; set;
        }

        public TimeZoneInfo Timezone
        {
            get; set;
        }

        [JsonProperty(PropertyName = "timezone")]
        public string TimezoneId
        {
            get; set;
        }

        [JsonProperty(PropertyName = "carbs_hr")]
        public int? CarbsHr
        {
            get; set;
        }

        [JsonProperty(PropertyName = "dia")]
        public int? Dia
        {
            get; set;
        }

        [JsonProperty(PropertyName = "delay")]
        public int? Delay
        {
            get; set;
        }

        [JsonProperty(PropertyName = "carbratio")]
        public IList<ProfileDefinition> Carbratio
        {
            get; set;
        }

        [JsonProperty(PropertyName = "basal")]
        public IList<ProfileDefinition> BasalRate
        {
            get; set;
        }

        [JsonProperty(PropertyName = "sens")]
        public IList<ProfileDefinition> Sens
        {
            get; set;
        }

        [JsonProperty(PropertyName = "target_low")]
        public IList<ProfileDefinition> TargetLow
        {
            get; set;
        }

        [JsonProperty(PropertyName = "target_high")]
        public IList<ProfileDefinition> TargetHigh
        {
            get; set;
        }
    }
}
