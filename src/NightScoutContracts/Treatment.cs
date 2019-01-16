using System;
using System.Globalization;
using System.Text;
using Newtonsoft.Json;

namespace Meiswinkel.NightScoutReporter.NightScoutContracts
{
    public class Treatment
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
        /// The type of treatment event.
        /// </summary>
        [JsonProperty(PropertyName = "eventType", NullValueHandling = NullValueHandling.Ignore)]
        public string EventType
        {
            get; set;
        }

        /// <summary>
        /// The date of the event, might be set retroactively .
        /// </summary>
        [JsonProperty(PropertyName = "created_at", NullValueHandling = NullValueHandling.Ignore)]
        public string CreatedAt
        {
            get; set;
        }

        /// <summary>
        /// Current glucose.
        /// </summary>
        [JsonProperty(PropertyName = "glucose", NullValueHandling = NullValueHandling.Ignore)]
        public string GlucoseJsonText
        {
            get; set;
        }

        /// <summary>
        /// Current glucose.
        /// </summary>
        [JsonIgnore]
        public decimal? Glucose
        {
            get
            {
                if (String.IsNullOrWhiteSpace(this.GlucoseJsonText))
                {
                    return null;
                }

                return Decimal.Parse(this.GlucoseJsonText, CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// Method used to obtain glucose, Finger or Sensor.
        /// </summary>
        [JsonProperty(PropertyName = "glucoseType", NullValueHandling = NullValueHandling.Ignore)]
        public string GlucoseType
        {
            get; set;
        }

        /// <summary>
        /// Number of carbs.
        /// </summary>
        [JsonProperty(PropertyName = "carbs", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? Carbs
        {
            get; set;
        }

        /// <summary>
        /// Amount of insulin, if any.
        /// </summary>
        [JsonProperty(PropertyName = "insulin", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? Insulin
        {
            get; set;
        }

        /// <summary>
        /// The units for the glucose value, mg/dl or mmol.
        /// </summary>
        [JsonProperty(PropertyName = "units", NullValueHandling = NullValueHandling.Ignore)]
        public string Units
        {
            get; set;
        }

        /// <summary>
        /// Description/notes of treatment.
        /// </summary>
        [JsonProperty(PropertyName = "notes", NullValueHandling = NullValueHandling.Ignore)]
        public string Notes
        {
            get; set;
        }

        /// <summary>
        /// Who entered the treatment.
        /// </summary>
        [JsonProperty(PropertyName = "enteredBy", NullValueHandling = NullValueHandling.Ignore)]
        public string EnteredBy
        {
            get; set;
        }

        /// <summary>
        /// Details of the bolus calculation
        /// </summary>
        [JsonProperty(PropertyName = "boluscalc", NullValueHandling = NullValueHandling.Ignore)]
        public BolusCalculation BolusCalculation
        {
            get; set;
        }

        /// <summary>
        /// Epoch
        /// </summary>
        [JsonProperty(PropertyName = "date", NullValueHandling = NullValueHandling.Ignore)]
        public long? Date
        {
            get; set;
        }

        [JsonProperty(PropertyName = "duration", NullValueHandling = NullValueHandling.Ignore)]
        public int? Duration
        {
            get; set;
        }

        /// <summary>
        /// Amount of insulin, if any.
        /// </summary>
        [JsonProperty(PropertyName = "isSMB", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool IsSuperMicroBolus
        {
            get;set;
        }

        /// <summary>
        /// Identifier of the NightScout client instance
        /// </summary>
        [JsonProperty(PropertyName = "NSCLIENT_ID", NullValueHandling = NullValueHandling.Ignore)]
        public string NightScoutClientId
        {
            get; set;
        }

        [JsonProperty(PropertyName = "percent", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? Percent
        {
            get; set;
        }

        /// <summary>
        /// Identifier of the pump
        /// </summary>
        [JsonProperty(PropertyName = "pumpId", NullValueHandling = NullValueHandling.Ignore)]
        public string PumpId
        {
            get; set;
        }
    }
}
