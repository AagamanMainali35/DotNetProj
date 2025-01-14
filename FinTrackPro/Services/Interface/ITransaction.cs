using FinTrackPro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTrackPro.Services.Interface
{
    internal interface ITransaction
    {
        public bool AddTransaction(TransactionModel transaction);
        public bool UpdateTransaction(TransactionModel updatedTransaction);
        public bool DeleteTransaction(Guid id);






    }
}
