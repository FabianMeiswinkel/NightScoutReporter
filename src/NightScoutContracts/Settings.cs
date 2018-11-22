using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Meiswinkel.NightScoutReporter.NightScoutContracts
{
    public class Settings
    {
        /// <summary>
        /// Default units for glucose measurements across the server.
        /// </summary>
        [JsonProperty(PropertyName = "units")]
        public string Units
        {
            get; set;
        }

        /// <summary>
        /// Default time format
        /// </summary>
        /// <value>Default time format</value>
        [JsonProperty(PropertyName = "timeFormat")]
        public string TimeFormat
        {
            get; set;
        }

        /// <summary>
        /// Default custom title to be displayed system wide.
        /// </summary>
        /// <value>Default custom title to be displayed system wide.</value>
        [JsonProperty(PropertyName = "customTitle")]
        public string CustomTitle
        {
            get; set;
        }

        /// <summary>
        /// Should Night mode be enabled by default?
        /// </summary>
        /// <value>Should Night mode be enabled by default?</value>
        [JsonProperty(PropertyName = "nightMode")]
        public bool? NightMode
        {
            get; set;
        }

        /// <summary>
        /// Default theme to be displayed system wide, `default`, `colors`, `colorblindfriendly`.
        /// </summary>
        /// <value>Default theme to be displayed system wide, `default`, `colors`, `colorblindfriendly`.</value>
        [JsonProperty(PropertyName = "theme")]
        public string Theme
        {
            get; set;
        }

        /// <summary>
        /// Default language code to be used system wide
        /// </summary>
        /// <value>Default language code to be used system wide</value>
        [JsonProperty(PropertyName = "language")]
        public string Language
        {
            get; set;
        }

        /// <summary>
        /// Plugins that should be shown by default
        /// </summary>
        /// <value>Plugins that should be shown by default</value>
        [JsonProperty(PropertyName = "showPlugins")]
        public string ShowPlugins
        {
            get; set;
        }

        /// <summary>
        /// If Raw BG is enabled when should it be shown? `never`, `always`, `noise`
        /// </summary>
        /// <value>If Raw BG is enabled when should it be shown? `never`, `always`, `noise`</value>
        [JsonProperty(PropertyName = "showRawbg")]
        public string ShowRawbg
        {
            get; set;
        }

        /// <summary>
        /// Enabled alarm types, can be multiple
        /// </summary>
        /// <value>Enabled alarm types, can be multiple</value>
        [JsonProperty(PropertyName = "alarmTypes")]
        public List<string> AlarmTypes
        {
            get; set;
        }

        /// <summary>
        /// Enable/Disable client-side Urgent High alarms by default, for use with `simple` alarms.
        /// </summary>
        /// <value>Enable/Disable client-side Urgent High alarms by default, for use with `simple` alarms.</value>
        [JsonProperty(PropertyName = "alarmUrgentHigh")]
        public bool? AlarmUrgentHigh
        {
            get; set;
        }

        /// <summary>
        /// Enable/Disable client-side High alarms by default, for use with `simple` alarms.
        /// </summary>
        /// <value>Enable/Disable client-side High alarms by default, for use with `simple` alarms.</value>
        [JsonProperty(PropertyName = "alarmHigh")]
        public bool? AlarmHigh
        {
            get; set;
        }

        /// <summary>
        /// Enable/Disable client-side Low alarms by default, for use with `simple` alarms.
        /// </summary>
        /// <value>Enable/Disable client-side Low alarms by default, for use with `simple` alarms.</value>
        [JsonProperty(PropertyName = "alarmLow")]
        public bool? AlarmLow
        {
            get; set;
        }

        /// <summary>
        /// Enable/Disable client-side Urgent Low alarms by default, for use with `simple` alarms.
        /// </summary>
        /// <value>Enable/Disable client-side Urgent Low alarms by default, for use with `simple` alarms.</value>
        [JsonProperty(PropertyName = "alarmUrgentLow")]
        public bool? AlarmUrgentLow
        {
            get; set;
        }

        /// <summary>
        /// Enable/Disable client-side stale data alarms by default.
        /// </summary>
        /// <value>Enable/Disable client-side stale data alarms by default.</value>
        [JsonProperty(PropertyName = "alarmTimeagoWarn")]
        public bool? AlarmTimeagoWarn
        {
            get; set;
        }

        /// <summary>
        /// Number of minutes before a stale data warning is generated.
        /// </summary>
        /// <value>Number of minutes before a stale data warning is generated.</value>
        [JsonProperty(PropertyName = "alarmTimeagoWarnMins")]
        public decimal? AlarmTimeagoWarnMins
        {
            get; set;
        }

        /// <summary>
        /// Enable/Disable client-side urgent stale data alarms by default.
        /// </summary>
        /// <value>Enable/Disable client-side urgent stale data alarms by default.</value>
        [JsonProperty(PropertyName = "alarmTimeagoUrgent")]
        public bool? AlarmTimeagoUrgent
        {
            get; set;
        }

        /// <summary>
        /// Number of minutes before a stale data warning is generated.
        /// </summary>
        /// <value>Number of minutes before a stale data warning is generated.</value>
        [JsonProperty(PropertyName = "alarmTimeagoUrgentMins")]
        public decimal? AlarmTimeagoUrgentMins
        {
            get; set;
        }

        /// <summary>
        /// Enabled features
        /// </summary>
        /// <value>Enabled features</value>
        [JsonProperty(PropertyName = "enable")]
        public List<string> Enable
        {
            get; set;
        }

        /// <summary>
        /// Gets or Sets Thresholds
        /// </summary>
        [JsonProperty(PropertyName = "thresholds")]
        public Threshold Thresholds
        {
            get; set;
        }
    }
}
