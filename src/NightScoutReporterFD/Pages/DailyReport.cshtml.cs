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
        private static readonly Uri BaseUri = new Uri("https://fmnightscout.azurewebsites.net");
        private readonly ILogger<DailyReportModel> logger;
        private readonly string[] timeLineLabels = new string[12 * 24];
        private readonly string[] mbgvs = new string[12 * 24];
        private readonly string[] sgvs = new string[12 * 24];
        private readonly string[] criticalLows = new string[12 * 24];
        private readonly string[] criticalHighs = new string[12 * 24];
        private readonly string[] warningLows = new string[12 * 24];
        private readonly string[] warningHighs = new string[12 * 24];
        private readonly string[] inTargets = new string[12 * 24];

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

        public HtmlString TimeLineLabels
        {
            get; set;
        }

        public decimal SuggestedMaxValue
        {
            get; set;
        }

        public async Task OnGetAsync()
        {
            try
            {

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
                    this.timeLineLabels[i] = String.Concat("'", parsedDay.AddMinutes(5 * i).ToString("HH:mm"), "'");
                    this.sgvs[i] = "NaN";
                    this.mbgvs[i] = "NaN";
                }

                using (var client = new NightScoutClient(BaseUri, tokenValues[0], this.logger))
                {
                    IList<Entry> entries = await client.GetEntriesAsync(
                        CancellationToken.None,
                        from: parsedDay,
                        to: parsedDay.AddDays(1).AddMilliseconds(-1),
                        maxCount: 300);

                    decimal suggestedMaxValue = 180;

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

                        if (sgvEntry.Sgv.Value > suggestedMaxValue)
                        {
                            suggestedMaxValue = sgvEntry.Sgv.Value;
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

                        if (minutesFromMidnight % 5 > 0)
                        {
                            index++;
                        }

                        this.mbgvs[index] = mbgEntry.Mbg.Value.ToString("###.##");

                        if (mbgEntry.Mbg.Value > suggestedMaxValue)
                        {
                            suggestedMaxValue = mbgEntry.Mbg.Value;
                        }
                    }

                    suggestedMaxValue = ((int)((suggestedMaxValue * 1.1m) / 10)) * 10 + 10;

                    for (int i = 0; i < 12 * 24; i++)
                    {
                        this.criticalLows[i] = "55";
                        this.warningLows[i] = "70";
                        this.inTargets[i] = "160";
                        this.warningHighs[i] = Math.Min(240, suggestedMaxValue).ToString("###.##");
                        this.criticalHighs[i] = suggestedMaxValue.ToString("###.##");
                    }

                    this.SuggestedMaxValue = suggestedMaxValue;

                    this.SerumGlucoseValues = String.Join(',', this.sgvs);
                    this.MeterBloodGlucoseValues = String.Join(',', this.mbgvs);
                    this.CriticalLows = String.Join(',', this.criticalLows);
                    this.WarningLows = String.Join(',', this.warningLows);
                    this.InTargets = String.Join(',', this.inTargets);
                    this.WarningHighs = String.Join(',', this.warningHighs);
                    this.CriticalHighs = String.Join(',', this.criticalHighs);
                    this.TimeLineLabels = new HtmlString(String.Join(',', this.timeLineLabels));
                }
             }
            catch (Exception error)
            {
                this.logger.LogError(error, "Error in OnGet: {0}", error.ToString());
                throw;
            }
        }
    }
}
