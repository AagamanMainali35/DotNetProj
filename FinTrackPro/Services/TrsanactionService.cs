using FinTrackPro.Models;
using System.Text.Json;
namespace FinTrackPro.Services
{
    public class TransactionService
    {
        protected static readonly string FilePath = Path.Combine(FileSystem.AppDataDirectory, "debts.json");

        // Load transactions from the JSON file
        public List<TransactionModel> GetAllTransactions()
        {
            if (!File.Exists(FilePath)) return new List<TransactionModel>();
            var json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<List<TransactionModel>>(json) ?? new List<TransactionModel>();
        }

        // Add a new transaction to the JSON file
        public void AddTransaction(TransactionModel transaction)
        {
            var transactions = GetAllTransactions();
            transaction.Id = Guid.NewGuid();  // Assign a new unique identifier
            transactions.Add(transaction);
            SaveTransactions(transactions);
        }

        // Update an existing transaction
        public void UpdateTransaction(TransactionModel updatedTransaction)
        {
            var transactions = GetAllTransactions();
            var existingTransaction = transactions.FirstOrDefault(t => t.Id == updatedTransaction.Id);
            if (existingTransaction != null)
            {
                existingTransaction.Label = updatedTransaction.Label;
                existingTransaction.Payee = updatedTransaction.Payee; // Updated Payee
                existingTransaction.Tag = updatedTransaction.Tag; // Updated Tag
                existingTransaction.Amount = updatedTransaction.Amount;
                existingTransaction.Date = updatedTransaction.Date;
                SaveTransactions(transactions);
            }
        }

        // Delete a transaction from the JSON file
        public void DeleteTransaction(Guid id)
        {
            var transactions = GetAllTransactions();
            var transactionToDelete = transactions.FirstOrDefault(t => t.Id == id);
            if (transactionToDelete != null)
            {
                transactions.Remove(transactionToDelete);
                SaveTransactions(transactions);
            }
        }

        // Save the list of transactions to the JSON file
        private void SaveTransactions(List<TransactionModel> transactions)
        {
            var json = JsonSerializer.Serialize(transactions, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, json);
        }
    }
}