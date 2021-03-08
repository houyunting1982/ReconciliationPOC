using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using ReconciliationPOC.App.Models.Reconciliation;

namespace ReconciliationPOC.App
{
    public class ReconciledPaymentClient : IDisposable
    {
        private readonly HttpClient _client;

        private const string BaseUrl = "http://paymentservice-poc.herokuapp.com";
            //"http://localhost:5000";

        public ReconciledPaymentClient() {
            _client = new HttpClient {
                BaseAddress = new Uri(BaseUrl)
            };
        }

        public async Task<ReconciledReport> GetReportByIdAsync(string id) {
            var response = await _client.GetAsync($"/api/Reconciliations/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ReconciledReport>();
        }

        public async Task<IEnumerable<ReconciledReport>> GetReportsAsync(string type = null,
            DateTime? start = null,
            DateTime? end = null
        ) {
            var parameters = new NameValueCollection();
            if (!string.IsNullOrEmpty(type))
            {
                parameters["type"] = type;
            }

            if (start.HasValue)
            {
                parameters["start"] = start.ToString();
            }

            if (end.HasValue)
            {
                parameters["end"] = end.ToString();
            }
            var response = await _client.GetAsync($"/api/Reconciliations?{ToQueryString(parameters)}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IEnumerable<ReconciledReport>>();
        }

        private static string ToQueryString(NameValueCollection nvc) {
            var array = (
                from key in nvc.AllKeys
                from value in nvc.GetValues(key)
                select $"{HttpUtility.UrlEncode(key)}={HttpUtility.UrlEncode(value)}"
            ).ToArray();
            return $"{string.Join("&", array)}";
        }

        private void ReleaseUnmanagedResources() {
            // TODO release unmanaged resources here
        }

        private void Dispose(bool disposing) {
            ReleaseUnmanagedResources();
            if (disposing)
            {
                _client?.Dispose();
            }
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
