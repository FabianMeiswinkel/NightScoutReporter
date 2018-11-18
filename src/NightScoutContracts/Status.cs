using System.Text;
using Newtonsoft.Json;

namespace Meiswinkel.NightScoutReporter.NightScoutContracts
{
    public class Status
    {
        /// <summary>
        /// Whether or not the REST API is enabled.
        /// </summary>
        [JsonProperty(PropertyName = "apiEnabled")]
        public bool? ApiEnabled
        {
            get; set;
        }

        /// <summary>
        /// Whether or not the careportal is enabled in the API.
        /// </summary>
        [JsonProperty(PropertyName = "careportalEnabled")]
        public bool? CareportalEnabled
        {
            get; set;
        }

        /// <summary>
        /// The git identifier for the running instance of the app.
        /// </summary>
        [JsonProperty(PropertyName = "head")]
        public string Head
        {
            get; set;
        }

        /// <summary>
        /// Nightscout (static)
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name
        {
            get; set;
        }

        /// <summary>
        /// The version label of the app.
        /// </summary>
        [JsonProperty(PropertyName = "version")]
        public string Version
        {
            get; set;
        }

        /// <summary>
        /// Gets or Sets Settings
        /// </summary>
        [JsonProperty(PropertyName = "settings")]
        public Settings Settings
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
            sb.Append("class Status {\n");
            sb.Append("  ApiEnabled: ").Append(this.ApiEnabled).Append("\n");
            sb.Append("  CareportalEnabled: ").Append(this.CareportalEnabled).Append("\n");
            sb.Append("  Head: ").Append(this.Head).Append("\n");
            sb.Append("  Name: ").Append(this.Name).Append("\n");
            sb.Append("  Version: ").Append(this.Version).Append("\n");
            sb.Append("  Settings: ").Append(this.Settings).Append("\n");
            sb.Append("}\n");

            return sb.ToString();
        }
    }
}
