using System;
using System.Linq;
using Bogus;
using Bogus.Extensions;
using Bogus.Extensions.UnitedKingdom;
using System.Collections.Generic;

namespace ElasticSearchWorkout.TestDataGenerator
{
    public class TestDataGenerator
    {
        public List<BankStatementLine> Generate(DateTime startDate, DateTime endDate, decimal openingBalance, float debitTransactionRatio, int transactionDateInterval, int numberOfStatementLines)
        {
            var statementconfig = new
            {
                StartDate = startDate.Date,
                EndDate = endDate.Date,
                OpeningBalance = openingBalance,
                DebitTransactionRatio = debitTransactionRatio,
                TransactionDateInterval = transactionDateInterval,
                NumberOfStatementLines = numberOfStatementLines
            };
            
            var balance = statementconfig.OpeningBalance;
            var lastDate = statementconfig.StartDate.Date;

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


            return fakeTransactions.Generate(statementconfig.NumberOfStatementLines);
        }

    }
}
