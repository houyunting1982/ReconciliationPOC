using System.Collections.Generic;
using ReconciliationPOC.App.Models.Accountings;

namespace ReconciliationPOC.App.Models.FakeData
{
    public class Stubs
    {
        public static string PaymentId_1 = "6e5d195e-e4a1-41cc-b45b-1e87d3d7bfc6";
        public static string PaymentId_2 = "08ae0eb1-d2ce-4ce3-8c3c-441b5ab8de70";
        public static string PaymentId_3 = "8ebb6a23-00c9-477b-a6f3-f09f63cfb31b";

        public static Payment Payment1 = new Payment { Id = PaymentId_1, Amount = 100 };
        public static Payment Payment2 = new Payment { Id = PaymentId_2, Amount = 19.95m };
        public static Payment Payment3 = new Payment { Id = PaymentId_3, Amount = 8000 };

        public static List<Payment> Payments => new List<Payment> {
            Payment1, Payment2, Payment3
        };
    }
}
