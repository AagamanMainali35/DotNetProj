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
               if (transaction.type == "Debit"){
                    TotalAmount = +transaction.Amount;
                }
            }
            return TotalAmount;
        }

        public int TotalCreditTransaction()
        {
            int TotalAmount = 0;
            var transactions = GetAllTransactions();
            foreach (var transaction in transactions)
            {
                if (transaction.type == "Credit")
                {
                    TotalAmount = +transaction.Amount;
                }
            }
            return TotalAmount;
        }

        public int GetHighestcredit()
        {
            List<TransactionModel> transactions = GetAllTransactions();
            // Get the highest credit transaction by sorting the transactions in descending order of amount
            var highestInflow = transactions
                .Where(t => t.type == "Credit") // Only consider credit transactions
                .OrderByDescending(t => t.Amount) // Sort by Amount in descending order
                .FirstOrDefault(); // Get the highest transaction or null if none

            return highestInflow?.Amount ?? 0; // Return the highest amount or 0 if no inflow exists
        }

        public int GetLowestcredit()
        {
            List<TransactionModel> transactions = GetAllTransactions();
            // Get the lowest credit transaction by sorting the transactions in ascending order of amount
            var lowestInflow = transactions
                .Where(t => t.type == "Credit") // Only consider credit transactions
                .OrderBy(t => t.Amount) // Sort by Amount in ascending order
                .FirstOrDefault(); // Get the lowest transaction or null if none

            return lowestInflow?.Amount ?? 0; // Return the lowest amount or 0 if no inflow exists
        }

        public int GetHighestdebit()
        {
            List<TransactionModel> transactions = GetAllTransactions();
            // Get the highest debit transaction by sorting the transactions in descending order of amount
            var highestOutflow = transactions
                .Where(t => t.type == "Debit") // Only consider debit transactions
                .OrderByDescending(t => t.Amount) // Sort by Amount in descending order
                .FirstOrDefault(); // Get the highest transaction or null if none

            return highestOutflow?.Amount ?? 0; // Return the highest amount or 0 if no outflow exists
        }

        public int GetLowestDebit()
        {
            List<TransactionModel> transactions = GetAllTransactions();
            // Get the lowest debit transaction by sorting the transactions in ascending order of amount
            var lowestOutflow = transactions
                .Where(t => t.type == "Debit") // Only consider debit transactions
                .OrderBy(t => t.Amount) // Sort by Amount in ascending order
                .FirstOrDefault(); // Get the lowest transaction or null if none

            return lowestOutflow?.Amount ?? 0; // Return the lowest amount or 0 if no outflow exists
        }


      
        public int TotalDebitTransaction()
        {
            int totalAmount = 0;
            List<TransactionModel> transactions = GetAllTransactions();
            // Iterate through all debit transactions and sum their amounts
            foreach (var transaction in transactions)
            {
                if (transaction.type == "Debit")
                {
                    totalAmount += transaction.Amount; // Sum of debit transactions
                }
            }
            return totalAmount; // Return the total amount for all debit transactions
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