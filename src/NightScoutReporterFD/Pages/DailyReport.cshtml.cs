using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Meiswinkel.NightScoutReporter.NightScoutClient;
using Meiswinkel.NightScoutReporter.NightScoutContracts;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using static System.FormattableString;

namespace NightScoutReporterFD.Pages
{
    public class DailyReportModel : PageModel
    {
        public const string CriticalLow = "55";
        public const string WarningLow = "70";
        public const string InTarget = "160";

        private readonly ILogger<DailyReportModel> logger;
        private readonly string[] timeLineLabels5M = new string[12 * 24];
        private readonly string[] timeLineLabels1M = new string[60 * 24];
        private readonly string[] mbgvs = new string[12 * 24];
        private readonly string[] sgvs = new string[12 * 24];
        private readonly string[] targetBasalRateValues = new string[60 * 24];
        private readonly string[] actualBasalRateValues = new string[60 * 24];

        public DailyReportModel(ILogger<DailyReportModel> logger)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            this.logger = logger;
        }

        public string DayQueryParameter
        {
            get; set;
        }

        public string SerumGlucoseValues
        {
            get; set;
        }

        public string MeterBloodGlucoseValues
        {
            get; set;
        }

        public HtmlString TimeLineLabels5M
        {
            get; set;
        }

        public HtmlString TimeLineLabels1M
        {
            get; set;
        }

        public HtmlString TargetBasalRates
        {
            get; set;
        }

        public HtmlString ActualBasalRates
        {
            get; set;
        }

        public HtmlString DailySummary
        {
            get; set;
        }

        public HtmlString Meals
        {
            get; set;
        }

        public decimal SuggestedMaxGlucoseValue
        {
            get; set;
        }

        public decimal SuggestedMaxBasalRateValue
        {
            get; set;
        }

        public string CriticalHigh
        {
            get; set;
        }

        public string WarningHigh
        {
            get; set;
        }

        private static void SetBloodGlucoseLevels(
            DateTimeOffset parsedDay,
            IList<Entry> entries,
            string entryType,
            Func<Entry, decimal?> getGlucoseValue,
            string[] values,
            ref decimal suggestedMaxGlucoseValue)
        {
            foreach (Entry entry in entries.Where((e) => e.Type == entryType))
            {
                if (entry.Date == null || getGlucoseValue(entry) == null)
                {
                    continue;
                }

                var date = DateTimeOffset.FromUnixTimeMilliseconds(entry.Date.Value);
                int minutesFromMidnight = (int)(date - parsedDay).TotalMinutes;

                int index = minutesFromMidnight / 5;

                decimal glucoseValue = getGlucoseValue(entry).Value;
                values[index] = glucoseValue.ToString("###.##");

                if (glucoseValue > suggestedMaxGlucoseValue)
                {
                    suggestedMaxGlucoseValue = glucoseValue;
                }
            }
        }

        public async Task OnGetAsync()
        {
            try
            {
                if (!this.Request.Query.TryGetValue("Hostname", out StringValues hostnameValues) ||
                    hostnameValues.Count != 1 ||
                    !Uri.TryCreate(
                        "https://" + hostnameValues[0],
                        UriKind.Absolute,
                        out Uri baseUri))
                {
                    throw new ArgumentException(
                        "Query parameter 'Hostname' is missing or invalid.",
                        "Hostname");
                };

                if (!this.Request.Query.TryGetValue("Day", out StringValues dayValues) ||
                    dayValues.Count != 1 ||
                    dayValues[0].Length != 10 ||
                    !DateTimeOffset.TryParseExact(
                        dayValues[0],
                        format: "yyyy-MM-dd",
                        formatProvider: CultureInfo.InvariantCulture,
                        styles: DateTimeStyles.AssumeUniversal,
                        result: out DateTimeOffset parsedDay))
                {
                    throw new ArgumentException(
                        "Query parameter 'Day' is missing or invalid. It must have the format 'yyyy-MM-dd'",
                        "Day");
                };

                if (!this.Request.Query.TryGetValue("Token", out StringValues tokenValues) ||
                    tokenValues.Count != 1 ||
                    String.IsNullOrWhiteSpace(tokenValues[0]))
                {
                    throw new ArgumentException(
                        "Query parameter 'Token' is missing.",
                        "Token");
                };

                this.DayQueryParameter = parsedDay.ToString("yyyy-MM-dd");

                for (int i = 0; i < 12 * 24; i++)
                {
                    this.timeLineLabels5M[i] = String.Concat("'", parsedDay.AddMinutes(5 * i).ToString("HH:mm"), "'");
                    this.sgvs[i] = "NaN";
                    this.mbgvs[i] = "NaN";
                }

                using (var client = new NightScoutClient(baseUri, tokenValues[0], this.logger))
                {
                    Profile profile = await client.GetDefaultProfileAsync(CancellationToken.None);
                    //TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(profile.)


                    Task<IList<Entry>> entriesTask = client.GetEntriesAsync(
                        CancellationToken.None,
                        from: parsedDay,
                        to: parsedDay.AddDays(1).AddMilliseconds(-1),
                        maxCount: 300);

                    Task<IList<Treatment>> treatmentsTask = client.GetTreatmentsAsync(
                        CancellationToken.None,
                        id: null,
                        from: parsedDay,
                        to: parsedDay.AddDays(1).AddMilliseconds(-1),
                        maxCount: 300);

                    await Task.WhenAll(entriesTask, treatmentsTask);

                    IList<Entry> entries = entriesTask.Result;
                    IList<Treatment> treatments = treatmentsTask.Result;

                    decimal suggestedMaxGlucoseValue = 180;

                    SetBloodGlucoseLevels(
                        parsedDay,
                        entries,
                        entryType: "sgv",
                        getGlucoseValue: new Func<Entry, decimal?>(e => e.Sgv),
                        values:  this.sgvs,
                        suggestedMaxGlucoseValue: ref suggestedMaxGlucoseValue);

                    SetBloodGlucoseLevels(
                        parsedDay,
                        entries,
                        entryType: "mbg",
                        getGlucoseValue: new Func<Entry, decimal?>(e => e.Mbg),
                        values: this.mbgvs,
                        suggestedMaxGlucoseValue: ref suggestedMaxGlucoseValue);

                    suggestedMaxGlucoseValue = ((int)((suggestedMaxGlucoseValue * 1.1m) / 10)) * 10 + 10;

                    this.WarningHigh = Math.Min(240, suggestedMaxGlucoseValue).ToString("###.##");
                    this.CriticalHigh = suggestedMaxGlucoseValue.ToString("###.##");
                    this.SuggestedMaxGlucoseValue = suggestedMaxGlucoseValue;

                    this.SerumGlucoseValues = String.Join(',', this.sgvs);
                    this.MeterBloodGlucoseValues = String.Join(',', this.mbgvs);
                    this.TimeLineLabels5M = new HtmlString(String.Join(',', this.timeLineLabels5M));

                    for (int i = 0; i < 60 * 24; i++)
                    {
                        this.timeLineLabels1M[i] = String.Concat("'", parsedDay.AddMinutes(i).ToString("HH:mm"), "'");
                    }

                    var orderedBasalRates = new List<ProfileDefinition>(profile.BasalRate.OrderBy(br => br.TimeAsSeconds));
                    for (int i = 0; i < orderedBasalRates.Count; i++)
                    {
                        int left = (int)orderedBasalRates[i].TimeAsSeconds / 60;
                        int right;
                        if (i < orderedBasalRates.Count - 1)
                        {
                            right = (int)orderedBasalRates[i + 1].TimeAsSeconds / 60;
                        }
                        else
                        {
                            right = 60 * 24;
                        }

                        for (int n = left; n < right; n++)
                        {
                            this.targetBasalRateValues[n] = orderedBasalRates[i].Value.ToString("##0.##", CultureInfo.InvariantCulture);
                        }
                    }

                    var temporaryBasalRateTreatments = new List<Treatment>(
                        treatments.Where(t => t.EventType == "Temp Basal").OrderBy(t => t.CreatedAt));

                    var tempBasalRates = new List<TempBasal>();

                    for (int i = 0; i < temporaryBasalRateTreatments.Count; i++)
                    {
                        Treatment current = temporaryBasalRateTreatments[i];
                        if (current.Percent == null)
                        {
                            continue;
                        }

                        int start = (int)(DateTimeOffset.Parse(current.CreatedAt, CultureInfo.InvariantCulture) - parsedDay).TotalMinutes;
                        var tbr = new TempBasal
                        {
                            Start = start,
                            End = start + current.Duration.Value,
                            Factor = temporaryBasalRateTreatments[i].Percent.Value / 100,
                        };

                        if (i < temporaryBasalRateTreatments.Count - 1)
                        {
                            Treatment next = temporaryBasalRateTreatments[i + 1];
                            int nextStart = (int)(DateTimeOffset.Parse(next.CreatedAt, CultureInfo.InvariantCulture) - parsedDay).TotalMinutes;

                            tbr.End = Math.Min(tbr.End, nextStart);
                        }

                        tempBasalRates.Add(tbr);
                    }

                    for (int i = 0; i < this.targetBasalRateValues.Length; i++)
                    {
                        this.actualBasalRateValues[i] = this.targetBasalRateValues[i];
                    }

                    for (int i = 0; i < tempBasalRates.Count; i++)
                    {
                        for (int n = tempBasalRates[i].Start; n < tempBasalRates[i].End; n++)
                        {
                            decimal targetRate = Decimal.Parse(this.actualBasalRateValues[n], CultureInfo.InvariantCulture);
                            decimal adjustedRate = targetRate * (1 + tempBasalRates[i].Factor);
                            this.actualBasalRateValues[n] = adjustedRate.ToString("##0.##", CultureInfo.InvariantCulture);
                        }
                    }

                    this.TimeLineLabels1M = new HtmlString(String.Join(',', this.timeLineLabels1M));
                    this.TargetBasalRates = new HtmlString(String.Join(',', this.targetBasalRateValues));
                    this.ActualBasalRates = new HtmlString(String.Join(',', this.actualBasalRateValues));

                    var eventTypes = new HashSet<string>(
                        treatments.Select(t => t.EventType));

                    Treatment dailySummaryTreatment = treatments
                        .Where(t => t.EventType == "NightScoutReporterDailySummary")
                        .OrderBy(t => t.CreatedAt)
                        .LastOrDefault();

                    if (dailySummaryTreatment != null &&
                        !String.IsNullOrWhiteSpace(dailySummaryTreatment.Notes))
                    {
                        this.DailySummary = new HtmlString(String.Join(',', eventTypes));
                    }
                    else
                    {
                        this.DailySummary = new HtmlString("Keine");
                    }

                    List<MealBolusTreatment> mealBoulsTreatments = ToMealBolusTreatments(treatments);
                    var sb = new StringBuilder();

                    if (mealBoulsTreatments.Count > 0)
                    {
                        sb.Append("<table>")
                            .Append("<tr>")
                            .Append("<th width='15%' align='left'>Uhrzeit</th>")
                            .Append("<th width='15%' align='right'>BG (in mg/dl)</th>")
                            .Append("<th width='15%' align='right'>KH (in g)</th>")
                            .Append("<th width='25%' align='right'>IE (+/- Korrektur)</th>")
                            .Append("<th width='15%' align='right'>COB (in g)</th>")
                            .Append("<th width='15%' align='right'>IOB (in IE)</th>")
                            .Append("</tr>");

                        foreach (MealBolusTreatment t in mealBoulsTreatments)
                        {
                            string insulinText = String.Empty;

                            if (t.Insulin != null)
                            {
                                if (t.InsulinCorrection != null)
                                {
                                    insulinText = String.Format(
                                        CultureInfo.InvariantCulture,
                                        "{0} [{1}]",
                                        t.Insulin.Value.ToString("###0.00"),
                                        t.InsulinCorrection.Value.ToString("###0.00"));
                                }
                                else
                                {
                                    insulinText = t.Insulin.Value.ToString("###0.00");
                                }
                            }

                            sb
                            .Append("<tr>")
                            .Append("<td align='left' style='font-weight:bold'>").Append(t.CreatedAt.Substring(11, 8)).Append("</td>")
                            .Append("<td align='right'>")
                                .Append(t.BG != null ? t.BG.Value.ToString("###0") : String.Empty).Append("</td>")
                            .Append("<td align='right'>")
                                .Append(t.Carbs != null ?
                                    t.Carbs.Value.ToString("###0") :
                                    String.Empty).Append("</td>")
                            .Append("<td align='right'>").Append(insulinText).Append("</td>")
                            .Append("<td align='right'>")
                                .Append(t.CarbsOnBoard != null ?
                                    t.CarbsOnBoard.Value.ToString("###0") :
                                    String.Empty).Append("</td>")
                            .Append("<td align='right'>")
                                .Append(t.InsulinOnBoard != null ?
                                    t.InsulinOnBoard.Value.ToString("###0.00") :
                                    String.Empty).Append("</td>")
                            .Append("</tr>");
                        }

                        sb.Append("</table>");
                    }

                    this.Meals = new HtmlString(sb.ToString());
                }
             }
            catch (Exception error)
            {
                this.logger.LogError(error, "Error in OnGet: {0}", error.ToString());
                throw;
            }
        }

        private static List<MealBolusTreatment> ToMealBolusTreatments(IEnumerable<Treatment> treatments)
        {
            var returnValue = new List<MealBolusTreatment>();

            IEnumerable<Treatment> relevantTreatments = treatments
                .Where(t => t.EventType != null &&
                    t.EnteredBy != "sync" &&
                    (t.EventType.Contains("Bolus") || t.EventType.Contains("Meal") || t.EventType.Contains("Carb")))
                .OrderBy(t => t.CreatedAt);

            foreach (Treatment t in relevantTreatments)
            {
                long startThreshold =
                    DateTimeOffset.Parse(t.CreatedAt).AddMinutes(-10).ToUnixTimeMilliseconds();

                long endThreshold =
                    DateTimeOffset.Parse(t.CreatedAt).AddMinutes(10).ToUnixTimeMilliseconds();


                if (t.BolusCalculation == null &&
                    relevantTreatments.Any(
                        c => c.BolusCalculation != null &&
                        c.BolusCalculation.Carbs == t.Carbs &&
                        c.BolusCalculation.EventTime.ToUnixTimeMilliseconds() >= startThreshold &&
                        c.BolusCalculation.EventTime.ToUnixTimeMilliseconds() <= endThreshold))
                {
                    continue;
                }

                var mealOrBolus = new MealBolusTreatment
                {
                    BG = t.Glucose,
                    Carbs = t.Carbs,
                    Insulin = t.Insulin,
                    CreatedAt = t.CreatedAt,
                };

                if (t.BolusCalculation != null)
                {
                    mealOrBolus.CarbsOnBoard = t.BolusCalculation.CarbsOnBoard;
                    mealOrBolus.InsulinForCarbs = t.BolusCalculation.InsulinForCarbs;
                    mealOrBolus.InsulinOnBoard = t.BolusCalculation.InsulinOnBoard;
                    mealOrBolus.InsulinSensitivityFactor = t.BolusCalculation.InsulinSensitivityFactor;
                    mealOrBolus.InsulinToCarbRatio = t.BolusCalculation.InsulinToCarbRatio;

                    if (mealOrBolus.Carbs == null)
                    {
                        mealOrBolus.Carbs = t.BolusCalculation.Carbs;
                    }

                    if (t.Insulin != null)
                    {
                        mealOrBolus.InsulinCorrection = t.Insulin.Value - t.BolusCalculation.InsulinForCarbs;
                    }
                }

                returnValue.Add(mealOrBolus);
            }

            return returnValue;
        }

        private class TempBasal
        {
            public int Start
            {
                get; set;
            }

            public int End
            {
                get; set;
            }

            public decimal Factor
            {
                get; set;
            }
        }

        private class MealBolusTreatment
        {
            public string CreatedAt
            {
                get;set;
            }

            public decimal? BG
            {
                get; set;
            }

            public decimal? Carbs
            {
                get; set;
            }

            public decimal? CarbsOnBoard
            {
                get; set;
            }

            /// <summary>
            /// DIA (oder auch "Insulin-End-Time") DIA steht für "duration of insulin action", gibt also an, wie lange
            /// das Insulin im Körper aktiv ist. Bei vielen ist zwar nach 3-4 Stunden die Hauptwirkung vorbei und die
            /// Restemenge eher gering. Deswegen wird in der Praxis oder bei Bolusrechnern mit linearer
            /// Insulinwirkkurve häufig ein solcher Wert verwendet. Diese Restmenge kann sich dann z.B. beim Sport
            /// doch noch bemerkbar machen. AndroidAPS verwendet physiologischere Kurven und kann auch diese
            /// Restmengen gut berechnen. Besonders bei der Überlagerung vieler einzelner Aktionen ist dies wichtig.
            /// Daher verwendet AndroidAPS minimum 5 Stunden als DIA. Wichtiger als die exakte Länge des DIA ist das
            /// Wirkmaximum das durch Auswahl des korrekten Wirk-Profils festgelegt wird, solange der DIA genügend
            /// groß ist.
            /// </summary>
            //public decimal? DurationOfInsulinAction
            //{
            //    get; set;
            //}

            /// <summary>
            /// IC Der IC (Insulin-Carb-Ratio - Insulin-Kohlenhydrat-Faktor) bestimmt, wieviel Gramm Kohlenhydrate
            /// durch 1 IE Insulin abgedeckt werden.
            /// </summary>
            public decimal? InsulinToCarbRatio
            {
                get; set;
            }

            public decimal? Insulin
            {
                get; set;
            }

            public decimal? InsulinForCarbs
            {
                get; set;
            }

            public decimal? InsulinCorrection
            {
                get; set;
            }

            public decimal? InsulinOnBoard
            {
                get; set;
            }

            /// <summary>
            /// ISF Der Insulinsensitivitätsfaktor (ISF) gibt an, um wie viele mg/dl oder mmol/l der
            /// BZ-Wert durch 1 IE Insulin gesenkt wird.
            /// </summary>
            public decimal? InsulinSensitivityFactor
            {
                get; set;
            }
        }
    }
}
