﻿using System;
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

        /// <summary>
        /// Get the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Treatment {\n");
            sb.Append("  Id: ").Append(this.Id).Append("\n");
            sb.Append("  EventType: ").Append(this.EventType).Append("\n");
            sb.Append("  CreatedAt: ").Append(this.CreatedAt).Append("\n");
            sb.Append("  Glucose: ").Append(this.Glucose).Append("\n");
            sb.Append("  GlucoseType: ").Append(this.GlucoseType).Append("\n");
            sb.Append("  Carbs: ").Append(this.Carbs).Append("\n");
            sb.Append("  Insulin: ").Append(this.Insulin).Append("\n");
            sb.Append("  Units: ").Append(this.Units).Append("\n");
            sb.Append("  Notes: ").Append(this.Notes).Append("\n");
            sb.Append("  EnteredBy: ").Append(this.EnteredBy).Append("\n");
            sb.Append("}\n");

            return sb.ToString();
        }
    }
}