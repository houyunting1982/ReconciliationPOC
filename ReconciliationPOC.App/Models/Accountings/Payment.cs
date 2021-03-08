namespace ReconciliationPOC.App.Models.Accountings
{
    // Simple enough only for demonstrating the payment reconciliation process.
    public class Payment
    {
        public string Id { get; set; }
        public string BatchId { get; set; } // If BatchId is null, the payment is unbatched.
        public decimal Amount { get; set; }

        public override string ToString() {
            return $"Payment{nameof(Id)}: {Id}, {nameof(BatchId)}: {BatchId}, {nameof(Amount)}: {Amount}";
        }
    }
}
