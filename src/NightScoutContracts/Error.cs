using System;
using System.Text;
using Newtonsoft.Json;

namespace Meiswinkel.NightScoutReporter.NightScoutContracts
{
    [JsonObject]
    public class Error
    {
        /// <summary>
        /// Gets or Sets Code
        /// </summary>
        [JsonProperty(PropertyName = "code")]
        public int? Code { get; set; }

        /// <summary>
        /// Gets or Sets Message
        /// </summary>
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        /// <summary>
        /// Gets or Sets Fields
        /// </summary>
        [JsonProperty(PropertyName = "fields")]
        public object Fields { get; set; }
    }
}
