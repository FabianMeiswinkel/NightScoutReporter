using System.Text;
using Newtonsoft.Json;

namespace Meiswinkel.NightScoutReporter.NightScoutContracts
{
    [JsonObject]
    public class Profile
    {
        [JsonProperty(PropertyName = "sens")]
        public int? Sens { get; set; }

        [JsonProperty(PropertyName = "dia")]
        public int? Dia { get; set; }

        [JsonProperty(PropertyName = "carbratio")]
        public int? Carbratio { get; set; }

        [JsonProperty(PropertyName = "carbs_hr")]
        public int? CarbsHr { get; set; }

        /// <summary>
        /// Internally assigned id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Get the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Profile {\n");
            sb.Append("  Sens: ").Append(this.Sens).Append("\n");
            sb.Append("  Dia: ").Append(this.Dia).Append("\n");
            sb.Append("  Carbratio: ").Append(this.Carbratio).Append("\n");
            sb.Append("  CarbsHr: ").Append(this.CarbsHr).Append("\n");
            sb.Append("  Id: ").Append(this.Id).Append("\n");
            sb.Append("}\n");

            return sb.ToString();
        }
    }
}
