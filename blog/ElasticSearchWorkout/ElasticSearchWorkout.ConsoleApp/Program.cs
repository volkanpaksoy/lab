using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using BankStatementDataGenerator;
using BankStatementImport.Core;
using ConsoleTables;
using Nest;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace ElasticSearchWorkout.ConsoleApp
{
    class Program
    {
        ////////////////////////////////////////////////////////////////////////////////////////////
        //
        // Usage: This is a demo application to show some sample usage of Elasticsearch using C#
        // All regions except the first one are commented out by default. You can uncomment a region and 
        // test the code. All regions are meant to be self-contained. Make sure to comment the code
        // in the region before you proceed to another region.
        // 
        ////////////////////////////////////////////////////////////////////////////////////////////
        static void Main(string[] args)
        {
            using (var connectionSettings = new ConnectionSettings(new Uri("http://localhost:9200")))
            {
                #region Section 01: Create Elasticsearch client
                var settings = connectionSettings
                    .DefaultIndex("bankstatementindex")
                    .ThrowExceptions(true);
                IElasticClient elasticClient = new ElasticClient(settings);
                #endregion

                #region Section 02: Index single document
                //var dataGen = new TestDataGenerator();
                //var statementConfig = new
                //{
                //    StartDate = DateTime.ParseExact("20180101", "yyyyMMdd", CultureInfo.InvariantCulture),
                //    EndDate = DateTime.ParseExact("20191231", "yyyyMMdd", CultureInfo.InvariantCulture),
                //    OpeningBalance = 500.00m,
                //    DebitTransactionRatio = 0.9f,
                //    TransactionDateInterval = 3,
                //    NumberOfStatementLines = 10
                //};
                //var testData = dataGen.Generate(statementConfig.StartDate, statementConfig.EndDate, statementConfig.OpeningBalance, statementConfig.DebitTransactionRatio, statementConfig.TransactionDateInterval, statementConfig.NumberOfStatementLines);
                //elasticClient.IndexDocument<BankStatementLine>(testData.First());
                #endregion

                #region Section 03: Index many documents
                //var dataGen = new TestDataGenerator();
                //var statementConfig = new
                //{
                //    StartDate = DateTime.ParseExact("20180101", "yyyyMMdd", CultureInfo.InvariantCulture),
                //    EndDate = DateTime.ParseExact("20191231", "yyyyMMdd", CultureInfo.InvariantCulture),
                //    OpeningBalance = 500.00m,
                //    DebitTransactionRatio = 0.9f,
                //    TransactionDateInterval = 3,
                //    NumberOfStatementLines = 5000
                //};
                //var testData = dataGen.Generate(statementConfig.StartDate, statementConfig.EndDate, statementConfig.OpeningBalance, statementConfig.DebitTransactionRatio, statementConfig.TransactionDateInterval, statementConfig.NumberOfStatementLines);

                //var stopWatch = new Stopwatch();

                //// Index with a loop - naive approach
                //stopWatch.Start();
                //Console.WriteLine("Indexing in a loop");
                //testData.ForEach(x => elasticClient.IndexDocument<BankStatementLine>(testData.First()));
                //stopWatch.Stop();
                //Console.WriteLine(stopWatch.Elapsed.ToString("mm\\:ss\\.ff"));

                //// Index with BulkAll
                //stopWatch.Reset();
                //stopWatch.Start();
                //Console.WriteLine("Indexing with BulkAll");
                //var bulkAll = elasticClient.BulkAll(testData, x => x
                //    .BackOffRetries(2)
                //    .BackOffTime("30s")
                //    .RefreshOnCompleted(true)
                //    .MaxDegreeOfParallelism(4)
                //    .Size(1000));

                //bulkAll.Wait(TimeSpan.FromSeconds(60),
                //    onNext: (b) => { Console.Write("Done"); }
                //);

                //stopWatch.Stop();
                //Console.WriteLine(stopWatch.Elapsed.ToString("mm\\:ss\\.ff"));
                #endregion

                #region Section 04: BulkAll with subscribe
                //var statementConfig = new
                //{
                //    StartDate = DateTime.ParseExact("20180101", "yyyyMMdd", CultureInfo.InvariantCulture),
                //    EndDate = DateTime.ParseExact("20191231", "yyyyMMdd", CultureInfo.InvariantCulture),
                //    OpeningBalance = 500.00m,
                //    DebitTransactionRatio = 0.9f,
                //    TransactionDateInterval = 3,
                //    NumberOfStatementLines = 5000
                //};
                //var dataGen = new TestDataGenerator();
                //var testData = dataGen.Generate(statementConfig.StartDate, statementConfig.EndDate, statementConfig.OpeningBalance, statementConfig.DebitTransactionRatio, statementConfig.TransactionDateInterval, statementConfig.NumberOfStatementLines);
                //var documents = testData;
                //var waitHandle = new CountdownEvent(1);

                //var bulkAll = elasticClient.BulkAll(documents, b => b
                //    .Index("bankstatementindex")
                //    .BackOffRetries(2)
                //    .BackOffTime("30s")
                //    .RefreshOnCompleted(true)
                //    .MaxDegreeOfParallelism(4)
                //    .Size(1000)
                //);

                //bulkAll.Subscribe(new BulkAllObserver(
                //    onNext: (b) => { Console.Write("."); },
                //    onError: (e) => { throw e; },
                //    onCompleted: () => waitHandle.Signal()
                //));

                //waitHandle.Wait();
                #endregion

                #region Section 05: Showing progress and cancellation with bulk operations
                //var dataGen = new TestDataGenerator();
                //var statementConfig = new
                //{
                //    StartDate = DateTime.ParseExact("20180101", "yyyyMMdd", CultureInfo.InvariantCulture),
                //    EndDate = DateTime.ParseExact("20191231", "yyyyMMdd", CultureInfo.InvariantCulture),
                //    OpeningBalance = 500.00m,
                //    DebitTransactionRatio = 0.9f,
                //    TransactionDateInterval = 3,
                //    NumberOfStatementLines = 15000
                //};
                //var testData = dataGen.Generate(statementConfig.StartDate, statementConfig.EndDate, statementConfig.OpeningBalance, statementConfig.DebitTransactionRatio, statementConfig.TransactionDateInterval, statementConfig.NumberOfStatementLines);
                //Console.WriteLine($"Generated test data. Item count: {testData.Count}");

                //var cancellationTokenSource = new CancellationTokenSource();
                //var cancellationToken = cancellationTokenSource.Token;
                //var batchSize = 250;
                //var bulkAll = elasticClient.BulkAll(testData, x => x
                //    .BackOffRetries(2)
                //    .BackOffTime("30s")
                //    .RefreshOnCompleted(true)
                //    .MaxDegreeOfParallelism(4)
                //    .Size(batchSize), cancellationToken);
                //var totalIndexed = 0;
                //var stopWatch = new Stopwatch();
                //stopWatch.Start();
                //Task.Factory.StartNew(() =>
                //    {
                //        Console.WriteLine("Started monitor thread");
                //        var cancelled = false;
                //        while (!cancelled)
                //        {
                //            if (stopWatch.Elapsed >= TimeSpan.FromSeconds(60))
                //            {
                //                if (cancellationToken.CanBeCanceled)
                //                {
                //                    Console.WriteLine($"Cancelling. Elapsed time: {stopWatch.Elapsed.ToString("mm\\:ss\\.ff")}");
                //                    cancellationTokenSource.Cancel();
                //                    cancelled = true;
                //                }
                //            }

                //            Thread.Sleep(100);
                //        }
                //    }
                //);

                //try
                //{
                //    bulkAll.Wait(TimeSpan.FromSeconds(60),
                //        onNext: (b) =>
                //        {
                //            totalIndexed += batchSize;
                //            Console.WriteLine($"Total indexed documents: {totalIndexed}");
                //        }
                //    );
                //}
                //catch (OperationCanceledException e)
                //{
                //    Console.WriteLine($"Taking longer than allowed. Cancelled.");
                //}
                #endregion

                #region Section 06: Query the indexed documents
                //// Get the first 100 documents
                //var searchResponse = elasticClient.Search<BankStatementLine>(s => s
                //    .Query(q => q
                //        .MatchAll()
                //    )
                //    .Size(100)
                //);
                // DisplayStatementLines((List<BankStatementLine>)searchResponse.Documents);

                // Shorthand version of above query
                //var searchResponse = elasticClient.Search<BankStatementLine>(s => s
                //    .MatchAll()
                //    .Size(100)
                //);
                // DisplayStatementLines((List<BankStatementLine>)searchResponse.Documents);

                // Get transactions with date between 01/01/2018 and 10/01/2018
                //var searchResponse = elasticClient.Search<BankStatementLine>(s => s
                //    .Query(q => q
                //        .DateRange(x => x
                //            .Field(f => f.TransactionDate)
                //            .GreaterThanOrEquals(new DateTime(2018, 01, 01))
                //            .LessThanOrEquals(new DateTime(2018, 01, 10))
                //        )
                //    )
                //    .Size(10000)
                //);
                // DisplayStatementLines((List<BankStatementLine>)searchResponse.Documents);

                //Get transactions with credit amount is not null
                //var searchResponse = elasticClient.Search<BankStatementLine>(s => s
                //    .Query(q => q
                //        .Exists(e => e
                //            .Field(f => f.CreditAmount)
                //        )
                //    )
                //    .Size(10000)
                //);
                // DisplayStatementLines((List<BankStatementLine>)searchResponse.Documents);

                // Get the transactions where the balance is negative
                //var searchResponse = elasticClient.Search<BankStatementLine>(s => s
                //    .Query(q => q
                //        .Range(r => r
                //            .Field(f => f.Balance)
                //            .LessThan(0)
                //        )
                //     )
                //    .Size(1000)
                //);
                // DisplayStatementLines((List<BankStatementLine>)searchResponse.Documents);
                #endregion

                #region Section 07: Delete all documents. Uncomment to start over (or create new Docker volumes)
                //elasticClient.DeleteByQuery<BankStatementLine>(del => del
                //    .Query(q => q.QueryString(qs => qs.Query("*")))
                //);

               // elasticClient.Delete<BankStatementLine>()
                #endregion


                #region Section 08: Update

            //    elasticClient.Update<BankStatementLine>()
                #endregion

                Console.WriteLine("Completed");
                Console.Read();
                
            }
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
