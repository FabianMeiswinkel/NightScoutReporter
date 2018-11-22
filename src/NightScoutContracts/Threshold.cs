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
    }
}
