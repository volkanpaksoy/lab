using System;
using System.Collections.Generic;

namespace ElasticSearchWorkout.TestDataGenerator
{
    public enum TransactionDirection
    {
        Debit,
        Credit,
        DebitOrCredit
    }

    public class TransactionType
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public TransactionDirection Direction { get; set; }

        public static TransactionType BankGiroCredit = new TransactionType() { Code = "BGC", Description = "Bank Giro Credit", Direction = TransactionDirection.Credit };
        public static TransactionType Cashpoint = new TransactionType() { Code = "C/P", Description = "Cashpoint", Direction = TransactionDirection.Debit };
        public static TransactionType Charge = new TransactionType() { Code = "CHG", Description = "Charge", Direction = TransactionDirection.Debit };
        public static TransactionType Cheque = new TransactionType() { Code = "CHQ", Description = "Cheque", Direction = TransactionDirection.Debit };
        public static TransactionType Correction = new TransactionType() { Code = "COR", Description = "Correction", Direction = TransactionDirection.DebitOrCredit };
        public static TransactionType Credit = new TransactionType() { Code = "CR", Description = "Credit", Direction = TransactionDirection.Credit };
        public static TransactionType Cash = new TransactionType() { Code = "CSH", Description = "Cash", Direction = TransactionDirection.Debit };
        public static TransactionType CardOnCounter = new TransactionType() { Code = "CW", Description = "Card On Counter", Direction = TransactionDirection.Debit };
        public static TransactionType DirectDebit = new TransactionType() { Code = "D/D", Description = "Direct Debit", Direction = TransactionDirection.Debit };
        public static TransactionType Dividend = new TransactionType() { Code = "DIV", Description = "Dividend", Direction = TransactionDirection.Credit };
        public static TransactionType Debit = new TransactionType() { Code = "DR", Description = "Debit", Direction = TransactionDirection.Debit };
        public static TransactionType EuroCheque = new TransactionType() { Code = "EUR", Description = "Euro Cheque", Direction = TransactionDirection.Debit };
        public static TransactionType ReceiptOfMoneyViaChaps = new TransactionType() { Code = "F/FLOW", Description = "Receipt of money via CHAPS", Direction = TransactionDirection.Credit };
        public static TransactionType FasterPaymentsInwards = new TransactionType() { Code = "FPI", Description = "Faster Payments Inwards", Direction = TransactionDirection.Credit };
        public static TransactionType FasterPaymentsOutwards = new TransactionType() { Code = "FPO", Description = "Faster Payments Outwards", Direction = TransactionDirection.Debit };
        public static TransactionType InterestCreditOrDebit = new TransactionType() { Code = "INT", Description = "Interest - Credit Or Debit", Direction = TransactionDirection.DebitOrCredit };
        public static TransactionType Other = new TransactionType() { Code = "OTH", Description = "Other", Direction = TransactionDirection.DebitOrCredit };
        public static TransactionType PaymentCard = new TransactionType() { Code = "P/C", Description = "Payment Card", Direction = TransactionDirection.Debit };
        public static TransactionType StandingOrder = new TransactionType() { Code = "S/O", Description = "Standing Order", Direction = TransactionDirection.Debit };
        public static TransactionType Salary = new TransactionType() { Code = "SAL", Description = "Salary", Direction = TransactionDirection.Debit };
        public static TransactionType Transfer = new TransactionType() { Code = "TFR", Description = "Transfer", Direction = TransactionDirection.DebitOrCredit };

        public static List<TransactionType> AllTransactionTypes = new List<TransactionType>
        {
            BankGiroCredit,
            Cashpoint,
            Charge,
            Cheque,
            Correction,
            Credit,
            Cash,
            CardOnCounter,
            DirectDebit,
            Dividend,
            Debit,
            EuroCheque,
            ReceiptOfMoneyViaChaps,
            FasterPaymentsInwards,
            FasterPaymentsOutwards,
            InterestCreditOrDebit,
            Other,
            PaymentCard,
            StandingOrder,
            Salary,
            Transfer
        };

        

    }
}
