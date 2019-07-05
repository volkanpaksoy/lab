using System;

namespace ElasticSearchWorkout.TestDataGenerator
{
    public class BankStatementLine
    {
        public DateTime TransactionDate { get; set; }
        public string TransactionType { get; set; }
        public string SortCode { get; set; }
        public string AccountNumber { get; set; }
        public string TransactionDescription { get; set; }
        public decimal? DebitAmount { get; set; }
        public decimal? CreditAmount { get; set; }
        public decimal Balance { get; set; }
    }

    public static class Extensions
    {
        public static bool IsCredit(this BankStatementLine bsl)
        {
            return bsl.DebitAmount is null;
        }

        public static bool IsDebit(this BankStatementLine bsl)
        {
            return !IsCredit(bsl);
        }
    }
}
