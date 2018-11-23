using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Meiswinkel.NightScoutReporter.NightScoutClient;
using Meiswinkel.NightScoutReporter.NightScoutContracts;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

namespace NightScoutReporterFD.Pages
{
    public class DailyReportModel : PageModel
    {
        private readonly ILogger<DailyReportModel> logger;
        private readonly string[] timeLineLabels5M = new string[12 * 24];
        private readonly string[] timeLineLabels1M = new string[60 * 24];
        private readonly string[] mbgvs = new string[12 * 24];
        private readonly string[] sgvs = new string[12 * 24];
        private readonly string[] criticalLows = new string[12 * 24];
        private readonly string[] criticalHighs = new string[12 * 24];
        private readonly string[] warningLows = new string[12 * 24];
        private readonly string[] warningHighs = new string[12 * 24];
        private readonly string[] inTargets = new string[12 * 24];
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

        public string CriticalHighs
        {
            get; set;
        }

        public string CriticalLows
        {
            get; set;
        }

        public string WarningLows
        {
            get; set;
        }

        public string WarningHighs
        {
            get; set;
        }

        public string InTargets
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

        public decimal SuggestedMaxGlucoseValue
        {
            get; set;
        }

        public decimal SuggestedMaxBasalRateValue
        {
            get; set;
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

                    foreach (Entry sgvEntry in entries.Where((e) => e.Type == "sgv"))
                    {
                        if (sgvEntry.Date == null || sgvEntry.Sgv == null)
                        {
                            continue;
                        }

                        var date = DateTimeOffset.FromUnixTimeMilliseconds(sgvEntry.Date.Value);
                        int minutesFromMidnight = (int)(date - parsedDay).TotalMinutes;

                        int index = minutesFromMidnight / 5;

                        this.sgvs[index] = sgvEntry.Sgv.Value.ToString("###.##");

                        if (sgvEntry.Sgv.Value > suggestedMaxGlucoseValue)
                        {
                            suggestedMaxGlucoseValue = sgvEntry.Sgv.Value;
                        }
                    }

                    foreach (Entry mbgEntry in entries.Where((e) => e.Type == "mbg"))
                    {
                        if (mbgEntry.Date == null || mbgEntry.Mbg == null)
                        {
                            continue;
                        }

                        var date = DateTimeOffset.FromUnixTimeMilliseconds(mbgEntry.Date.Value);
                        int minutesFromMidnight = (int)(date - parsedDay).TotalMinutes;

                        int index = minutesFromMidnight / 5;

                        this.mbgvs[index] = mbgEntry.Mbg.Value.ToString("###.##");

                        if (mbgEntry.Mbg.Value > suggestedMaxGlucoseValue)
                        {
                            suggestedMaxGlucoseValue = mbgEntry.Mbg.Value;
                        }
                    }

                    suggestedMaxGlucoseValue = ((int)((suggestedMaxGlucoseValue * 1.1m) / 10)) * 10 + 10;

                    for (int i = 0; i < 12 * 24; i++)
                    {
                        this.criticalLows[i] = "55";
                        this.warningLows[i] = "70";
                        this.inTargets[i] = "160";
                        this.warningHighs[i] = Math.Min(240, suggestedMaxGlucoseValue).ToString("###.##");
                        this.criticalHighs[i] = suggestedMaxGlucoseValue.ToString("###.##");
                    }

                    this.SuggestedMaxGlucoseValue = suggestedMaxGlucoseValue;

                    this.SerumGlucoseValues = String.Join(',', this.sgvs);
                    this.MeterBloodGlucoseValues = String.Join(',', this.mbgvs);
                    this.CriticalLows = String.Join(',', this.criticalLows);
                    this.WarningLows = String.Join(',', this.warningLows);
                    this.InTargets = String.Join(',', this.inTargets);
                    this.WarningHighs = String.Join(',', this.warningHighs);
                    this.CriticalHighs = String.Join(',', this.criticalHighs);
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
                }
             }
            catch (Exception error)
            {
                this.logger.LogError(error, "Error in OnGet: {0}", error.ToString());
                throw;
            }
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
    }
}
