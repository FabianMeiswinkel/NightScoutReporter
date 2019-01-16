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
        [JsonProperty(PropertyName = "_id", NullValueHandling = NullValueHandling.Ignore)]
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
        [JsonProperty(PropertyName = "dateString", NullValueHandling = NullValueHandling.Ignore)]
        public string DateString { get; set; }

        /// <summary>
        /// dateString, prefer ISO `8601`
        /// </summary>
        [JsonProperty(PropertyName = "device", NullValueHandling = NullValueHandling.Ignore)]
        public string Device
        {
            get; set;
        }

        /// <summary>
        /// Epoch
        /// </summary>
        [JsonProperty(PropertyName = "date")]
        public long? Date { get; set; }

        /// <summary>
        /// The glucose reading. (only available for sgv types)
        /// </summary>
        [JsonProperty(PropertyName = "sgv", NullValueHandling = NullValueHandling.Ignore)]
        public uint? Sgv { get; set; }

        /// <summary>
        /// The glucose reading. (only available for mbg types)
        /// </summary>
        [JsonProperty(PropertyName = "mbg", NullValueHandling = NullValueHandling.Ignore)]
        public uint? Mbg
        {
            get; set;
        }

        /// <summary>
        /// Direction of glucose trend reported by CGM. (only available for sgv types)
        /// </summary>
        [JsonProperty(PropertyName = "direction", NullValueHandling = NullValueHandling.Ignore)]
        public string Direction { get; set; }

        /// <summary>
        /// Noise level at time of reading. (only available for sgv types)
        /// </summary>
        [JsonProperty(PropertyName = "noise", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? Noise { get; set; }

        /// <summary>
        /// The raw filtered value directly from CGM transmitter. (only available for sgv types)
        /// </summary>
        [JsonProperty(PropertyName = "filtered", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? Filtered { get; set; }

        /// <summary>
        /// dateString, prefer ISO `8601`
        /// </summary>
        [JsonProperty(PropertyName = "sysTime", NullValueHandling = NullValueHandling.Ignore)]
        public string SysTime
        {
            get; set;
        }

        /// <summary>
        /// The raw unfiltered value directly from CGM transmitter. (only available for sgv types)
        /// </summary>
        [JsonProperty(PropertyName = "unfiltered", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? Unfiltered { get; set; }

        /// <summary>
        /// The signal strength from CGM transmitter. (only available for sgv types)
        /// </summary>
        [JsonProperty(PropertyName = "rssi", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? Rssi { get; set; }
    }
}
