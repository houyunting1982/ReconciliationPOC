using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReconciliationPOC.App.Models.Accountings;
using ReconciliationPOC.App.Models.FakeData;
using ReconciliationPOC.App.Models.Reconciliation;

namespace ReconciliationPOC.App.Services
{
    public class AutoBatchingService
    {
        private readonly ReconciledPaymentClient client;

        public AutoBatchingService(ReconciledPaymentClient client) {
            this.client = client;
        }

        public async Task ExecuteAsync(string type = null, DateTime? start = null, DateTime? end = null) {
            var reconciledReport = await client.GetReportsAsync(type, start, end);
            var unMatchedTransactions = new List<ReconciledTransaction>();
            foreach (var report in reconciledReport)
            {
                var joinedPayments = (from reconciledTransaction in report.Transactions
                    join payment in Stubs.Payments on reconciledTransaction.PaymentId equals payment.Id into joinedTable
                    from subPayment in joinedTable.DefaultIfEmpty()
                    select (
                        Transaction: reconciledTransaction,
                        Payment: subPayment
                    )).ToList();

                unMatchedTransactions.AddRange(joinedPayments.Where(i => i.Payment == null).Select(i => i.Transaction));
                GenerateBatch(joinedPayments.Where(i => i.Payment != null), report.Type.ToUpper(), report.Date);
            }

            SendUnmatchedTransactionsReport(unMatchedTransactions);
        }

        private void SendUnmatchedTransactionsReport(List<ReconciledTransaction> unMatchedTransactions) {
            Console.WriteLine($"Total {unMatchedTransactions.Count} unmatched transactions:");
            foreach (var trans in unMatchedTransactions)
            {
                Console.WriteLine($"  {trans}");
            }
        }

        private void GenerateBatch(IEnumerable<(ReconciledTransaction, Payment)> matchedTransactionPayments, string type, DateTime date) {
            var groupedTransaction = matchedTransactionPayments.GroupBy(i => i.Item1.SettlementNumber);
            foreach (var group in groupedTransaction)
            {
                // 1. Create a new batch
                var batch = new Batch {
                    Id = Guid.NewGuid().ToString(),
                    Name = $"AutoBatch_{type}_{group.Key}_{date:d}",
                    CreatedOn = DateTime.UtcNow
                };

                // Just for demo purpose. Print the Batch information
                Console.WriteLine($"Batch [{batch.Name}] (Id: {batch.Id}) created AT {batch.CreatedOn:u}: ");
                Console.WriteLine("  Contains the following payment(s):");
                // Update the BatchId field of all matched payment entities.
                foreach (var item in group)
                {
                    item.Item2.BatchId = batch.Id;
                    // Just for demo purpose. Print the Payment information
                    Console.WriteLine($"    {item.Item2}");
                }

                Console.WriteLine();
            }
        }
    }
}
