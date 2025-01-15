using FinTrackPro.Models;
using System.Text.Json;
using FinTrackPro.Services.Interface;
namespace FinTrackPro.Services
{
   public class TransactionService :  ITransaction
    {
        protected static readonly string FilePath = Path.Combine(FileSystem.AppDataDirectory, "Transactions.json");

        // Load transactions from the JSON file
        public List<TransactionModel> GetAllTransactions()
        {
            if (!File.Exists(FilePath)) return new List<TransactionModel>();
            var json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<List<TransactionModel>>(json) ?? new List<TransactionModel>();
        }

        public bool AddTransaction(TransactionModel transaction)
        {
            var transactions = GetAllTransactions();
            transaction.Id = Guid.NewGuid(); 
            transactions.Add(transaction);
            SaveTransactions(transactions);
            return true;
        }


        // Update an existing transaction
        public bool UpdateTransaction(TransactionModel updatedTransaction)
        {
            var transactions = GetAllTransactions();
            var existingTransaction = transactions.FirstOrDefault(t => t.Id == updatedTransaction.Id);
            if (existingTransaction != null)
            {
                existingTransaction.Label = updatedTransaction.Label;
                existingTransaction.Payee = updatedTransaction.Payee; 
                existingTransaction.Tag = updatedTransaction.Tag; // Updated Tag
                existingTransaction.Amount = updatedTransaction.Amount;
                existingTransaction.Date = updatedTransaction.Date;
                SaveTransactions(transactions);
                return true;
            }
            else
            {
                return false;
            }
            
        }

        public bool DeleteTransaction(Guid id)
        {
            var transactions = GetAllTransactions();
            var transactionToDelete = transactions.FirstOrDefault(t => t.Id == id);
            if (transactionToDelete != null)
            {
                transactions.Remove(transactionToDelete);
                SaveTransactions(transactions);
                return true;
            }
            else
            {
                return false;
            }
           
        }

        public List<TransactionModel> GetTopTransactions(int count)
        {
            var transactions = GetAllTransactions();
            return transactions
                    .OrderByDescending(t => t.Amount) // Sort by Amount in descending order
                    .Take(count) // Take the top 'count' transactions
                    .ToList();
        }


        public int TotalTransaction()
        {
            int TotalAmount = 0;
            var transactions = GetAllTransactions();
            foreach(var transaction in transactions)
            {
                TotalAmount += transaction.Amount;
            }
            return TotalAmount;
        }


        // Save the list of transactions to the JSON file
        private bool SaveTransactions(List<TransactionModel> transactions)
        {
            var json = JsonSerializer.Serialize(transactions, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, json);
            return true ;
        }
    }
}