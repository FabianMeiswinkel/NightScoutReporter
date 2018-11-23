using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Meiswinkel.NightScoutReporter.NightScoutCommon;
using Meiswinkel.NightScoutReporter.NightScoutContracts;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TimeZoneMapper;

namespace Meiswinkel.NightScoutReporter.NightScoutClient
{
    public class NightScoutClient : IDisposable
    {
        private static readonly JsonSerializerSettings jsonSettings =
            new JsonSerializerSettings
            {
                FloatParseHandling = FloatParseHandling.Decimal,
            };

        private readonly Uri baseUri;
        private readonly HttpClient client;
        private readonly string token;
        private readonly ILogger log;

        private bool disposedValue = false; // To detect redundant calls

        public NightScoutClient(Uri baseUri, string token, ILogger log)
        {
            if (baseUri == null)
            {
                throw new ArgumentNullException(nameof(baseUri));
            }

            if (log == null)
            {
                throw new ArgumentNullException(nameof(log));
            }

            this.baseUri = baseUri;
            this.token = token;
            this.log = log;

            var clientHandler = new HttpClientHandler
            {
                AllowAutoRedirect = false,
                AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip,
                ClientCertificateOptions = ClientCertificateOption.Manual,
                MaxRequestContentBufferSize = 1 * 1024 * 1024,
                PreAuthenticate = false,
                Proxy = null,
                UseCookies = false,
                UseDefaultCredentials = false,
                UseProxy = false,
            };

            this.client = new HttpClient(clientHandler, true);
            this.client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        #region GetEntries
        public async Task<Entry> GetEntryAsync(string id, CancellationToken cancellationToken)
        {
            if (String.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            var uriBuilder = new UriBuilder(this.baseUri);

            if (!String.IsNullOrWhiteSpace(this.token))
            {
                uriBuilder.SetQueryParam("token", this.token);
            }

            uriBuilder.Path = String.Concat("/api/v1/entries/", id);

            IList<Entry> entries = await this.GetInternalAsync<Entry>(cancellationToken, uriBuilder);

            return entries.FirstOrDefault();
        }

        public Task<IList<Entry>> GetEntriesAsync(
            CancellationToken cancellationToken,
            DateTimeOffset? from,
            DateTimeOffset? to,
            int? maxCount)
        {
            return this.GetEntriesAsync(cancellationToken, EntryType.All, from, to, maxCount);
        }

        public async Task<IList<Entry>> GetEntriesAsync(
            CancellationToken cancellationToken,
            EntryType entryType = EntryType.All,
            DateTimeOffset? from = null,
            DateTimeOffset? to = null,
            int? maxCount = null)
        {
            if (maxCount != null && (maxCount.Value > 300 || maxCount.Value < 1))
            {
                throw new ArgumentOutOfRangeException(nameof(maxCount.Value));
            }

            var uriBuilder = new UriBuilder(this.baseUri);

            switch (entryType)
            {
                case EntryType.All:
                    uriBuilder.Path = "/api/v1/entries";
                    break;

                case EntryType.Calibration:
                    uriBuilder.Path = "/api/v1/entries/cal";
                    break;

                case EntryType.MeterBloodGlucoseValues:
                    uriBuilder.Path = "/api/v1/entries/mbg";
                    break;

                case EntryType.SerumGlucoseValues:
                    uriBuilder.Path = "/api/v1/entries/sgv";
                    break;

                default:
                    throw new ArgumentOutOfRangeException(
                    String.Format(CultureInfo.InvariantCulture, "Value '{0}' is not a valid {1}", entryType, typeof(EntryType).Name),
                    nameof(entryType));
            }

            if (!String.IsNullOrWhiteSpace(this.token))
            {
                uriBuilder.SetQueryParam("token", this.token);
            }

            if (from != null)
            {
                uriBuilder.SetQueryParam("find[date][$gte]", from.Value.ToUnixTimeMilliseconds().ToString());
            }

            if (to != null)
            {
                uriBuilder.SetQueryParam("find[date][$lte]", to.Value.ToUnixTimeMilliseconds().ToString());
            }

            if (maxCount != null)
            {
                uriBuilder.SetQueryParam("count", maxCount.ToString());
            }
            else
            {
                uriBuilder.SetQueryParam("count", "300");
            }

            var result = new List<Entry>(await this.GetInternalAsync<Entry>(cancellationToken, uriBuilder));

            result.Sort(new Comparison<Entry>(
                (left, right) => Comparer<long>.Default.Compare(left.Date.Value, right.Date.Value)));

            result.Sort(new Comparison<Entry>(
                (left, right) => {
                    int comparisonResult = Comparer<long>.Default.Compare(left.Date.Value, right.Date.Value);

                    if (comparisonResult != 0)
                    {
                        return comparisonResult;
                    }

                    return Comparer<string>.Default.Compare(left.Id, right.Id);
                }));

            return result;
        }
        #endregion GetEntries

        public async Task<Profile> GetDefaultProfileAsync(
            CancellationToken cancellationToken)
        {
            var uriBuilder = new UriBuilder(this.baseUri)
            {
                Path = "/api/v1/profile.json"
            };

            if (!String.IsNullOrWhiteSpace(this.token))
            {
                uriBuilder.SetQueryParam("token", this.token);
            }

            ProfilesOverview overview = (await this.GetInternalAsync<ProfilesOverview>(cancellationToken, uriBuilder)).Single();
            Console.WriteLine(overview.Id);
            Console.WriteLine(overview.Store);

            Profile profile = overview.Store.GetValue(overview.DefaultProfile).ToObject<Profile>();
            profile.Timezone = TimeZoneMap.OnlineWithFallbackValuesTZMapper.MapTZID(profile.TimezoneId);

            Console.WriteLine(profile.Id);
            Console.WriteLine(profile.Units);
            Console.WriteLine(profile.Timezone);
            Console.WriteLine(profile.TimezoneId);

            return profile;
        }

        #region GetTreatments
        public async Task<IList<Treatment>> GetTreatmentsAsync(
            CancellationToken cancellationToken,
            string id = null,
            DateTimeOffset? from = null,
            DateTimeOffset? to = null,
            int? maxCount = null)
        {
            if (maxCount != null && (maxCount.Value > 300 || maxCount.Value < 1))
            {
                throw new ArgumentOutOfRangeException(nameof(maxCount.Value));
            }

            var uriBuilder = new UriBuilder(this.baseUri)
            {
                Path = "/api/v1/treatments"
            };

            if (!String.IsNullOrWhiteSpace(this.token))
            {
                uriBuilder.SetQueryParam("token", this.token);
            }

            if (!String.IsNullOrWhiteSpace(id))
            {
                uriBuilder.SetQueryParam("find[_id]", id);
            }

            if (from != null)
            {
                uriBuilder.SetQueryParam("find[created_at][$gte]", from.Value.AddDays(-1).ToString("yyyy-MM-dd"));
            }

            if (to != null)
            {
                uriBuilder.SetQueryParam("find[created_at][$lte]", to.Value.AddDays(1).ToString("yyyy-MM-dd"));
            }

            if (maxCount != null)
            {
                uriBuilder.SetQueryParam("count", maxCount.ToString());
            }
            else
            {
                uriBuilder.SetQueryParam("count", "300");
            }

            IList<Treatment> tempResult = await this.GetInternalAsync<Treatment>(cancellationToken, uriBuilder);

            long fromEpoch = from != null ? from.Value.ToUnixTimeMilliseconds() : 0;
            long toEpoch = to != null ? to.Value.ToUnixTimeMilliseconds() : DateTimeOffset.MaxValue.ToUnixTimeMilliseconds();

            var result = new List<Treatment>();
            foreach (Treatment candidate in tempResult)
            {
                if (candidate.Date == null)
                {
                    candidate.Date = DateTimeOffset.Parse(candidate.CreatedAt).ToUnixTimeMilliseconds();
                }

                if (candidate.Date.Value >= fromEpoch &&
                    candidate.Date.Value <= toEpoch)
                {
                    result.Add(candidate);
                }
            }

            result.Sort(new Comparison<Treatment>(
                (left, right) => {
                    int comparisonResult = Comparer<long>.Default.Compare(left.Date.Value, right.Date.Value);

                    if (comparisonResult != 0)
                    {
                        return comparisonResult;
                    }

                    return Comparer<string>.Default.Compare(left.Id, right.Id);
                }));

            return result;
        }
        #endregion GetTreatments

        private async Task<IList<T>> GetInternalAsync<T>(
            CancellationToken cancellationToken,
            UriBuilder uriBuilder)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Get, uriBuilder.Uri))
            using (HttpResponseMessage response = await this.client.SendAsync(
                request,
                HttpCompletionOption.ResponseContentRead,
                cancellationToken))
            {
                if (response.Content == null)
                {
                    return new List<T>();
                }

                string responseBody = await response.Content.ReadAsStringAsync();

                if (String.IsNullOrWhiteSpace(responseBody))
                {
                    return new List<T>();
                }

                this.log.LogDebug("REQUESTHEADERS: {0}", request.Headers);
                this.log.LogDebug("REQUESTURI: {0}", request.RequestUri);
                this.log.LogDebug("RESPONSEHEADERS: {0}", response.Headers);
                this.log.LogDebug("RESPONSE: {0}", responseBody);

                return JsonConvert.DeserializeObject<IList<T>>(responseBody, jsonSettings);
            }
        }

        #region IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    this.client.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                this.disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~NightScoutClient() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            this.Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion IDisposable
    }
}
