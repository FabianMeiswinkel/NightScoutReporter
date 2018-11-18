using System;
using Newtonsoft.Json;

namespace Meiswinkel.NightScoutReporter.NightScoutContracts
{
    [JsonObject]
    public class BolusCalculation
    {
        [JsonProperty(PropertyName = "basaliobused")]
        public bool BasalInsulinOnBoardUsed
        {
            get; set;
        }

        [JsonProperty(PropertyName = "bolusiobused")]
        public bool BolusInsulinOnBoardUsed
        {
            get; set;
        }

        [JsonProperty(PropertyName = "bg")]
        public decimal BloodGlucose
        {
            get; set;
        }

        [JsonProperty(PropertyName = "bgdiff")]
        public decimal BloodGlucoseDiffFromTarget
        {
            get; set;
        }

        [JsonProperty(PropertyName = "carbs")]
        public decimal Carbs
        {
            get; set;
        }

        [JsonProperty(PropertyName = "cob")]
        public decimal CarbsOnBoard
        {
            get; set;
        }

        [JsonProperty(PropertyName = "eventTime")]
        public DateTimeOffset EventTime
        {
            get; set;
        }

        [JsonProperty(PropertyName = "insulin")]
        public decimal Insulin
        {
            get; set;
        }

        [JsonProperty(PropertyName = "insulinbg")]
        public decimal InsulinCorrectionForBloodGlucose
        {
            get; set;
        }

        [JsonProperty(PropertyName = "insulintrend")]
        public decimal InsulinCorrectionForBloodGlucoseTrend
        {
            get; set;
        }

        [JsonProperty(PropertyName = "insulinbgused")]
        public bool InsulinCorrectionForBloodGlucoseUsed
        {
            get; set;
        }

        [JsonProperty(PropertyName = "insulincob")]
        public decimal InsulinCorrectionForCarbsOnBoard
        {
            get; set;
        }

        [JsonProperty(PropertyName = "insulinsuperbolus")]
        public decimal InsulinCorrectionForSuperBolus
        {
            get; set;
        }

        [JsonProperty(PropertyName = "insulincarbs")]
        public decimal InsulinForCarbs
        {
            get; set;
        }

        [JsonProperty(PropertyName = "iob")]
        public decimal InsulinOnBoard
        {
            get; set;
        }

        [JsonProperty(PropertyName = "isf")]
        public decimal InsulinSensitivityFactor
        {
            get; set;
        }

        [JsonProperty(PropertyName = "ic")]
        public decimal InsulinToCarbRatio
        {
            get; set;
        }

        [JsonProperty(PropertyName = "othercorrection")]
        public decimal OtherCorrection
        {
            get; set;
        }

        /// <summary>
        /// Profile used for the bolus calculation
        /// </summary>
        [JsonProperty(PropertyName = "profile")]
        public string Profile
        {
            get; set;
        }

        [JsonProperty(PropertyName = "targetBGHigh")]
        public decimal TargetBGHigh
        {
            get; set;
        }

        [JsonProperty(PropertyName = "targetBGLow")]
        public decimal TargetBGLow
        {
            get;set;
        }
    }
}
