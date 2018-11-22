using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Meiswinkel.NightScoutReporter.NightScoutContracts
{
    [JsonObject]
    public class Entry
    {
        /// <summary>
        /// Internally assigned id.
        /// </summary>
        [JsonProperty(PropertyName = "_id")]
        public string Id
        {
            get; set;
        }

        /// <summary>
        /// sgv, mbg, cal, etc
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        /// <summary>
        /// dateString, prefer ISO `8601`
        /// </summary>
        [JsonProperty(PropertyName = "dateString")]
        public string DateString { get; set; }

        /// <summary>
        /// Epoch
        /// </summary>
        [JsonProperty(PropertyName = "date")]
        public long? Date { get; set; }

        /// <summary>
        /// The glucose reading. (only available for sgv types)
        /// </summary>
        [JsonProperty(PropertyName = "sgv")]
        public decimal? Sgv { get; set; }

        /// <summary>
        /// The glucose reading. (only available for mbg types)
        /// </summary>
        [JsonProperty(PropertyName = "mbg")]
        public decimal? Mbg
        {
            get; set;
        }

        /// <summary>
        /// Direction of glucose trend reported by CGM. (only available for sgv types)
        /// </summary>
        [JsonProperty(PropertyName = "direction")]
        public string Direction { get; set; }

        /// <summary>
        /// Noise level at time of reading. (only available for sgv types)
        /// </summary>
        [JsonProperty(PropertyName = "noise")]
        public decimal? Noise { get; set; }

        /// <summary>
        /// The raw filtered value directly from CGM transmitter. (only available for sgv types)
        /// </summary>
        [JsonProperty(PropertyName = "filtered")]
        public decimal? Filtered { get; set; }

        /// <summary>
        /// The raw unfiltered value directly from CGM transmitter. (only available for sgv types)
        /// </summary>
        [JsonProperty(PropertyName = "unfiltered")]
        public decimal? Unfiltered { get; set; }

        /// <summary>
        /// The signal strength from CGM transmitter. (only available for sgv types)
        /// </summary>
        [JsonProperty(PropertyName = "rssi")]
        public decimal? Rssi { get; set; }
    }
}
