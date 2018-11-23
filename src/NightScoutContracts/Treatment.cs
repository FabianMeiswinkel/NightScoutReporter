using System;
using System.Text;
using Newtonsoft.Json;

namespace Meiswinkel.NightScoutReporter.NightScoutContracts
{
    public class Treatment
    {
        /// <summary>
        /// Details of the bolus calculation
        /// </summary>
        [JsonProperty(PropertyName = "boluscalc")]
        public BolusCalculation BolusCalculation
        {
            get; set;
        }

        /// <summary>
        /// Number of carbs.
        /// </summary>
        [JsonProperty(PropertyName = "carbs")]
        public decimal? Carbs
        {
            get; set;
        }

        /// <summary>
        /// The date of the event, might be set retroactively .
        /// </summary>
        [JsonProperty(PropertyName = "created_at")]
        public string CreatedAt
        {
            get; set;
        }

        /// <summary>
        /// Epoch
        /// </summary>
        [JsonProperty(PropertyName = "date")]
        public long? Date
        {
            get; set;
        }

        [JsonProperty(PropertyName = "duration")]
        public int? Duration
        {
            get; set;
        }

        /// <summary>
        /// Who entered the treatment.
        /// </summary>
        [JsonProperty(PropertyName = "enteredBy")]
        public string EnteredBy
        {
            get; set;
        }

        /// <summary>
        /// The type of treatment event.
        /// </summary>
        [JsonProperty(PropertyName = "eventType")]
        public string EventType
        {
            get; set;
        }

        /// <summary>
        /// Current glucose.
        /// </summary>
        [JsonProperty(PropertyName = "glucose")]
        public decimal? Glucose
        {
            get; set;
        }

        /// <summary>
        /// Method used to obtain glucose, Finger or Sensor.
        /// </summary>
        [JsonProperty(PropertyName = "glucoseType")]
        public string GlucoseType
        {
            get; set;
        }

        /// <summary>
        /// Internally assigned id.
        /// </summary>
        [JsonProperty(PropertyName = "_id")]
        public string Id
        {
            get; set;
        }

        /// <summary>
        /// Amount of insulin, if any.
        /// </summary>
        [JsonProperty(PropertyName = "insulin")]
        public decimal? Insulin
        {
            get; set;
        }

        /// <summary>
        /// Amount of insulin, if any.
        /// </summary>
        [JsonProperty(PropertyName = "isSMB")]
        public bool IsSuperMicroBolus
        {
            get;set;
        }

        /// <summary>
        /// Identifier of the NightScout client instance
        /// </summary>
        [JsonProperty(PropertyName = "NSCLIENT_ID")]
        public string NightScoutClientId
        {
            get; set;
        }

        /// <summary>
        /// Description/notes of treatment.
        /// </summary>
        [JsonProperty(PropertyName = "notes")]
        public string Notes
        {
            get; set;
        }

        [JsonProperty(PropertyName = "percent")]
        public decimal? Percent
        {
            get; set;
        }

        /// <summary>
        /// Identifier of the pump
        /// </summary>
        [JsonProperty(PropertyName = "pumpId")]
        public string PumpId
        {
            get; set;
        }

        /// <summary>
        /// The units for the glucose value, mg/dl or mmol.
        /// </summary>
        [JsonProperty(PropertyName = "units")]
        public string Units
        {
            get; set;
        }
    }
}
