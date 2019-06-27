using System;
using CsvHelper.Configuration.Attributes;

namespace BankStatementImport.Core
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
}
