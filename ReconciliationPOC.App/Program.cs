using System;
using System.Threading.Tasks;
using ReconciliationPOC.App.Services;

namespace ReconciliationPOC.App
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using var client = new ReconciledPaymentClient();
            var autoBatchService = new AutoBatchingService(client);
            await autoBatchService.ExecuteAsync();
        }
    }
}
