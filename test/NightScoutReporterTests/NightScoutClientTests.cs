using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Meiswinkel.NightScoutReporter.NightScoutClient;
using Meiswinkel.NightScoutReporter.NightScoutContracts;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Logging.Debug;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace NightScoutReporterTests
{
    [TestClass]
    public class NightScoutClientTests
    {
        private const string TemporaryToken = "unittest-a85424f42d7a13b3";

        private static readonly Uri BaseUri = new Uri("https://fmnightscout.azurewebsites.net");
        private static readonly ILogger Log = new ConsoleLogger(
            name: "NightScoutClientTestsLogger",
            filter: null,
            includeScopes: true);

        [TestMethod]
        public async Task GetEntrySucceeds()
        {
            using (var client = new NightScoutClient(BaseUri, TemporaryToken, Log))
            {
                Entry entry = await client.GetEntryAsync("5be8bd455a6f96e567696f3c", CancellationToken.None);

                Assert.IsNotNull(entry, nameof(entry));
                Assert.AreEqual(68, entry.Sgv, nameof(entry.Sgv));
                Assert.AreEqual("2018-11-11T15:38:05.186-0800", entry.DateString, nameof(entry.DateString));
                Assert.AreEqual(1541979485186, entry.Date, nameof(entry.Date));
                Assert.AreEqual("Flat", entry.Direction, nameof(entry.Direction));

                Console.WriteLine("OUTPUT: {0}", entry);
            }
        }

        [TestMethod]
        public async Task GetEntriesSucceeds()
        {
            using (var client = new NightScoutClient(BaseUri, TemporaryToken, Log))
            {
                IList<Entry> entries = await client.GetEntriesAsync(CancellationToken.None);

                Assert.IsNotNull(entries, nameof(entries));
                Assert.AreEqual(300, entries.Count, nameof(entries.Count));

                Console.WriteLine("OUTPUT: {0}", entries);
            }
        }

        [TestMethod]
        public async Task GetTreatmentSucceeds()
        {
            using (var client = new NightScoutClient(BaseUri, TemporaryToken, Log))
            {
                Treatment treatment = (await client.GetTreatmentsAsync(CancellationToken.None,
                    id: "5be85efd8a56a01558e2233d",
                    from: DateTimeOffset.Parse("2018-11-11T16:55:41Z"),
                    to: DateTimeOffset.Parse("2018-11-11T16:55:41Z"),
                    maxCount: 1)).SingleOrDefault();

                Assert.IsNotNull(treatment, nameof(treatment));
                Assert.AreEqual("Bolus Wizard", treatment.EventType, nameof(treatment.EventType));
                Assert.IsNotNull(treatment.Insulin, nameof(treatment.Insulin));
                Assert.AreEqual(2.7m, treatment.Insulin, nameof(treatment.Insulin));
                Assert.AreEqual("2018-11-11T16:55:41Z", treatment.CreatedAt, nameof(treatment.CreatedAt));
                Assert.AreEqual(1541955341000, treatment.Date, nameof(treatment.Date));
                Assert.AreEqual(false, treatment.IsSuperMicroBolus, nameof(treatment.IsSuperMicroBolus));
                Assert.AreEqual("1541955341000", treatment.PumpId, nameof(treatment.PumpId));
                Assert.IsNotNull(treatment.Glucose, nameof(treatment.Glucose));
                Assert.AreEqual(81m, treatment.Glucose, nameof(treatment.Glucose));
                Assert.AreEqual("Manual", treatment.GlucoseType, nameof(treatment.GlucoseType));
                Assert.AreEqual("1541955350978", treatment.NightScoutClientId, nameof(treatment.NightScoutClientId));
                Assert.IsNull(treatment.Carbs, nameof(treatment.Carbs));

                Assert.IsNotNull(treatment.BolusCalculation, nameof(treatment.BolusCalculation));
                Assert.AreEqual(
                    "<Active>",
                    treatment.BolusCalculation.Profile,
                    nameof(treatment.BolusCalculation.Profile));
                Assert.AreEqual(
                    DateTimeOffset.Parse("2018-11-11T16:55:07Z"),
                    treatment.BolusCalculation.EventTime,
                    nameof(treatment.BolusCalculation.EventTime));
                Assert.AreEqual(
                    80,
                    treatment.BolusCalculation.TargetBGLow,
                    nameof(treatment.BolusCalculation.TargetBGLow));
                Assert.AreEqual(
                    110,
                    treatment.BolusCalculation.TargetBGHigh,
                    nameof(treatment.BolusCalculation.TargetBGHigh));
                Assert.AreEqual(
                    30,
                    treatment.BolusCalculation.InsulinSensitivityFactor,
                    nameof(treatment.BolusCalculation.InsulinSensitivityFactor));
                Assert.AreEqual(9,
                    treatment.BolusCalculation.InsulinToCarbRatio,
                    nameof(treatment.BolusCalculation.InsulinToCarbRatio));
                Assert.AreEqual(
                    -0.37500000000000006m, //rounded from -0.37500000000000008 - that's fine here
                    treatment.BolusCalculation.InsulinOnBoard,
                    nameof(treatment.BolusCalculation.InsulinOnBoard));
                Assert.AreEqual(
                    true,
                    treatment.BolusCalculation.BasalInsulinOnBoardUsed,
                    nameof(treatment.BolusCalculation.BasalInsulinOnBoardUsed));
                Assert.AreEqual(
                    true,
                    treatment.BolusCalculation.BolusInsulinOnBoardUsed,
                    nameof(treatment.BolusCalculation.BolusInsulinOnBoardUsed));
                Assert.AreEqual(
                    81,
                    treatment.BolusCalculation.BloodGlucose,
                    nameof(treatment.BolusCalculation.BloodGlucose));
                Assert.AreEqual(
                    0,
                    treatment.BolusCalculation.InsulinCorrectionForBloodGlucose,
                    nameof(treatment.BolusCalculation.InsulinCorrectionForBloodGlucose));
                Assert.AreEqual(
                    true,
                    treatment.BolusCalculation.InsulinCorrectionForBloodGlucoseUsed,
                    nameof(treatment.BolusCalculation.InsulinCorrectionForBloodGlucoseUsed));
                Assert.AreEqual(
                    0,
                    treatment.BolusCalculation.BloodGlucoseDiffFromTarget,
                    nameof(treatment.BolusCalculation.BloodGlucoseDiffFromTarget));
                Assert.AreEqual(
                    1.3333333333333333m,
                    treatment.BolusCalculation.InsulinForCarbs,
                    nameof(treatment.BolusCalculation.InsulinForCarbs));
                Assert.AreEqual(
                    12,
                    treatment.BolusCalculation.Carbs,
                    nameof(treatment.BolusCalculation.Carbs));
                Assert.AreEqual(
                    0,
                    treatment.BolusCalculation.CarbsOnBoard,
                    nameof(treatment.BolusCalculation.CarbsOnBoard));
                Assert.AreEqual(
                    0,
                    treatment.BolusCalculation.InsulinCorrectionForCarbsOnBoard,
                    nameof(treatment.BolusCalculation.InsulinCorrectionForCarbsOnBoard));
                Assert.AreEqual(
                    1.4m,
                    treatment.BolusCalculation.OtherCorrection,
                    nameof(treatment.BolusCalculation.OtherCorrection));
                Assert.AreEqual(
                    0,
                    treatment.BolusCalculation.InsulinCorrectionForSuperBolus,
                    nameof(treatment.BolusCalculation.InsulinCorrectionForSuperBolus));
                Assert.AreEqual(
                    -0.356m,
                    treatment.BolusCalculation.InsulinCorrectionForBloodGlucoseTrend,
                    nameof(treatment.BolusCalculation.InsulinCorrectionForBloodGlucoseTrend));
                Assert.AreEqual(
                    2.8000000000000003m, // rounded from 2.8000000000000004 - that's fine here
                    treatment.BolusCalculation.Insulin,
                    nameof(treatment.BolusCalculation.Insulin));

                Console.WriteLine("OUTPUT: {0}", treatment);
            }
        }

        [TestMethod]
        public async Task GetTreatmentsSucceeds()
        {
            using (var client = new NightScoutClient(BaseUri, TemporaryToken, Log))
            {
                IList<Treatment> treatments = await client.GetTreatmentsAsync(CancellationToken.None);

                Assert.IsNotNull(treatments, nameof(treatments));
                Assert.AreEqual(300, treatments.Count, nameof(treatments.Count));

                Console.WriteLine("OUTPUT: {0}", treatments);
            }
        }
    }
}
