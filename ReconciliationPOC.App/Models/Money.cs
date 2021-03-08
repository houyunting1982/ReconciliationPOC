namespace ReconciliationPOC.App.Models
{
    public class MoneyAmount
    {
        public string Value { get; set; }

        public string Code { get; set; }

        public override string ToString() {
            return $"{nameof(Value)}: {Value}, {nameof(Code)}: {Code}";
        }
    }
}
