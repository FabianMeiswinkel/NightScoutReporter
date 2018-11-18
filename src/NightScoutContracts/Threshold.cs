using System.Text;
using Newtonsoft.Json;

namespace Meiswinkel.NightScoutReporter.NightScoutContracts
{
    public class Threshold
    {
        /// <summary>
        /// High BG range.
        /// </summary>
        [JsonProperty(PropertyName = "bg_high")]
        public int? BgHigh
        {
            get; set;
        }

        /// <summary>
        /// Top of target range.
        /// </summary>
        [JsonProperty(PropertyName = "bg_target_top")]
        public int? BgTargetTop
        {
            get; set;
        }

        /// <summary>
        /// Bottom of target range.
        /// </summary>
        [JsonProperty(PropertyName = "bg_target_bottom")]
        public int? BgTargetBottom
        {
            get; set;
        }

        /// <summary>
        /// Low BG range.
        /// </summary>
        [JsonProperty(PropertyName = "bg_low")]
        public int? BgLow
        {
            get; set;
        }

        /// <summary>
        /// Get the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Threshold {\n");
            sb.Append("  BgHigh: ").Append(this.BgHigh).Append("\n");
            sb.Append("  BgTargetTop: ").Append(this.BgTargetTop).Append("\n");
            sb.Append("  BgTargetBottom: ").Append(this.BgTargetBottom).Append("\n");
            sb.Append("  BgLow: ").Append(this.BgLow).Append("\n");
            sb.Append("}\n");

            return sb.ToString();
        }
    }
}
