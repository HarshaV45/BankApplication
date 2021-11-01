using System;
using System.Collections.Generic;
using System.Text;

namespace TechnovertAtm.CLI.Enums
{
    public enum StaffOptions
    {
        NewAccount=1, 
        UpdateAccount,
        DeleteAccount,
        TransactionHistory,
        RevertTransaction,
        NewCurrency,
        AccountDetails,
        Logout
    }
}
