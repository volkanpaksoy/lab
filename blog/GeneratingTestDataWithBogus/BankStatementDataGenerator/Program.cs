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
                DebitTransactionRatio = 0.9f
            };
            
            var balance = statementconfig.OpeningBalance;

            var commonFields = new Faker<BankStatementLine>()
                .RuleFor(x => x.AccountNumber, f => f.Finance.Account())
                .RuleFor(x => x.SortCode, f => f.Finance.SortCode())
                .Generate();

            var fakeTransactions = new Faker<BankStatementLine>()
                .RuleFor(x => x.TransactionDate, f => f.Date.Between(statementconfig.StartDate, statementconfig.EndDate))
                .RuleFor(x => x.AccountNumber, commonFields.AccountNumber)
                .RuleFor(x => x.SortCode, f => commonFields.SortCode)
                .RuleFor(x => x.TransactionDescription, f => f.Lorem.Sentence(3))
                .Rules((f, x) =>
                {
                    var debitAmount = (decimal?)f.Random.Decimal(1, 100).OrNull(f, 1.0f - statementconfig.DebitTransactionRatio);
                    if (debitAmount.HasValue) // Is it a debit transaction?
                    {
                        // We cannot have both debit and credit values in the same line
                        x.CreditAmount = null;
                        x.DebitAmount = debitAmount.Value;

                        // Adjust the total balance
                        balance -= x.DebitAmount.Value;

                        x.TransactionType = f.PickRandom(TransactionType.AllTransactionTypes
                            .Where(tt => tt.Direction == TransactionDirection.Debit || tt.Direction == TransactionDirection.DebitOrCredit)
                            .Select(tt => tt.Code));
                    }
                    else
                    {
                        // If it is not a debit transaction then it must definitely be a credit hence not calling OrNull 
                        var creditAmount = (decimal?)f.Random.Decimal(1, 100);
                        x.DebitAmount = null;
                        x.CreditAmount = creditAmount;

                        balance += x.CreditAmount.Value;

                        x.TransactionType = f.PickRandom(TransactionType.AllTransactionTypes
                            .Where(tt => tt.Direction == TransactionDirection.Credit || tt.Direction == TransactionDirection.DebitOrCredit)
                            .Select(tt => tt.Code));
                    }

                    x.Balance = balance;
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
                table.AddRow(line.TransactionDate,
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
