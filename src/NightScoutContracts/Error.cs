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

        /// <summary>
        /// Get the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Error {\n");
            sb.Append("  Code: ").Append(this.Code).Append("\n");
            sb.Append("  Message: ").Append(this.Message).Append("\n");
            sb.Append("  Fields: ").Append(this.Fields).Append("\n");
            sb.Append("}\n");

            return sb.ToString();
        }
    }
}
