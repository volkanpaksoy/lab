using System;
using BankStatementImport.Core;

namespace BankStatementDataGenerator
{
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
