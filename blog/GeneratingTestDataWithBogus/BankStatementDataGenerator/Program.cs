using System;
using System.Linq;
using System.Globalization;
using BankStatementImport.Core;
using Bogus;
using Bogus.Extensions;
using Bogus.Extensions.UnitedKingdom;
using System.Collections.Generic;
using ConsoleTables;

namespace BankStatementDataGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var statementconfig = new
            {
                StartDate = DateTime.ParseExact("20190601", "yyyyMMdd", CultureInfo.InvariantCulture),
                EndDate = DateTime.ParseExact("20190630", "yyyyMMdd", CultureInfo.InvariantCulture),
                OpeningBalance = 500.00m,
                DebitTransactionRatio = 0.9f,
                TransactionDateInterval = 3
            };
            
            var balance = statementconfig.OpeningBalance;
            var lastDate = statementconfig.StartDate;

            var commonFields = new Faker<BankStatementLine>()
                .RuleFor(x => x.AccountNumber, f => f.Finance.Account())
                .RuleFor(x => x.SortCode, f => f.Finance.SortCode())
                .Generate();

            var fakeTransactions = new Faker<BankStatementLine>()
                .StrictMode(true)
                .RuleFor(x => x.TransactionDate, f =>
                {
                    lastDate = lastDate.AddDays(f.Random.Double(0, statementconfig.TransactionDateInterval));
                    if (lastDate.Date > statementconfig.EndDate)
                    {
                        lastDate = statementconfig.EndDate;
                    }
                    return lastDate;
                })
                .RuleFor(x => x.AccountNumber, commonFields.AccountNumber)
                .RuleFor(x => x.SortCode, f => commonFields.SortCode)
                .RuleFor(x => x.TransactionDescription, f => f.Lorem.Sentence(3))
                .RuleFor(x => x.DebitAmount, f =>
                {
                    return (decimal?)f.Random.Decimal(1, 100).OrNull(f, 1.0f - statementconfig.DebitTransactionRatio);
                })
                .RuleFor(x => x.CreditAmount, (f, x) =>
                {
                    return x.IsCredit() ? (decimal?)f.Random.Decimal(1, 100) : null;
                })
                .RuleFor(x => x.TransactionType, (f, x) =>
                {
                    if (x.IsCredit())
                    {
                        return RandomTxCode(TransactionDirection.Credit); ;
                    }
                    else
                    {
                        return RandomTxCode(TransactionDirection.Debit);
                    }

                    string RandomTxCode(TransactionDirection direction)
                    {
                        return f.PickRandom(TransactionType.AllTransactionTypes
                            .Where(tt => tt.Direction == direction || tt.Direction == TransactionDirection.DebitOrCredit)
                            .Select(tt => tt.Code));
                        }
                })
                .RuleFor(x => x.Balance, (f, x) =>
                {
                    if (x.IsCredit())
                        balance += x.CreditAmount.Value;
                    else
                        balance -= x.DebitAmount.Value;

                    return balance;
                });


            var statementLines = fakeTransactions.GenerateBetween(10, 10);

            DisplayStatementLines(statementLines);

            Console.Read();
        }

        static void DisplayStatementLines(List<BankStatementLine> statementLines)
        {
            var table = new ConsoleTable("Transaction Date", "Transaction Type", "Sort Code", "Account Number", "Transaction Description", "Debit Amount", "Credit Amount", "Balance");
            
            foreach (var line in statementLines)
            {
                table.AddRow(line.TransactionDate.ToString("dd/MM/yyyy"),
                    line.TransactionType,
                    line.SortCode,
                    line.AccountNumber,
                    line.TransactionDescription,
                    line.DebitAmount?.ToString("#.##"),
                    line.CreditAmount?.ToString("#.##"),
                    line.Balance.ToString("#.##"));
            }

            table.Write();
        }
    }
}
