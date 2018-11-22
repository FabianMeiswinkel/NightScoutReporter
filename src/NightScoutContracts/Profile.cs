using System.Text;
using Newtonsoft.Json;

namespace Meiswinkel.NightScoutReporter.NightScoutContracts
{
    [JsonObject]
    public class Profile
    {
        [JsonProperty(PropertyName = "carbratio")]
        public int? Carbratio
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

        [JsonProperty(PropertyName = "sens")]
        public int? Sens
        {
            get; set;
        }

        /// <summary>
        /// Internally assigned id
        /// </summary>
        public string Id
        {
            get; set;
        }
    }
}
