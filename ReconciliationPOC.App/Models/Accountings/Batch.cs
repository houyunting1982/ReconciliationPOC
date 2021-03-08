using System;

namespace ReconciliationPOC.App.Models.Accountings
{
    // Simple enough only for demonstrating the payment reconciliation process.
    public class Batch
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedOn { get; set; }

        public override string ToString() {
            return $"{nameof(Id)}: {Id}, {nameof(Name)}: {Name}, Created At : {CreatedOn}";
        }
    }
}
