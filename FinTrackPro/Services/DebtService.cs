using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using FinTrackPro.Models;

namespace FinTrackPro.Services
{
    public class DebtService : IDebtService
    {
        private static readonly string DebtFilePath = Path.Combine(FileSystem.AppDataDirectory, "debts.json");

        public List<DebtModel> GetAllDebts()
        {
            if (!File.Exists(DebtFilePath))
            {
                return new List<DebtModel>();
            }

            var json = File.ReadAllText(DebtFilePath);
            Console.Write(json);

            var debts = JsonSerializer.Deserialize<List<DebtModel>>(json);

            if (debts == null)
                return new List<DebtModel>();

            return debts;
        }

        public List<DebtModel> GetDebtsByStatus(string status)
        {
            var debts = GetAllDebts();

            List<DebtModel> filteredDebts = new List<DebtModel>();

            foreach (var debt in debts)
            {
                if (debt.Status == status)
                {
                    filteredDebts.Add(debt);
                }
            }
            return filteredDebts;
        }

        public void AddNewDebt(DebtModel newDebt)
        {
            var debts = GetAllDebts();
            newDebt.Id = Guid.NewGuid();
            newDebt.Status = "Pending";
            debts.Add(newDebt);
            SaveDebts(debts);
        }
        
        public void UpdateDebtStatus(Guid debtId, string newStatus)
        {
            var debts = GetAllDebts();

            var debtToUpdate = debts.FirstOrDefault(d => d.Id == debtId);

            if (debtToUpdate != null)
            {
                debtToUpdate.Status = newStatus;


                SaveDebts(debts);
            }
        }

        public void MarkDebtAsPaid(Guid debtId)
        {
            var debts = GetAllDebts();

            var debtToMarkAsPaid = debts.FirstOrDefault(d => d.Id == debtId);

            if (debtToMarkAsPaid != null)
            {
                debtToMarkAsPaid.Status = "Paid";
                SaveDebts(debts);
            }
        }

        public int TotalDebtAmount()
        {
            var debts = GetAllDebts();  
            int totalDebt = 0;  

            foreach (var debt in debts)  
            {
                if (debt.Status =="Pending") { totalDebt += debt.Amount; }
            }
            return totalDebt;
        }

        public int ClearedDebt()
        {
            var debts = GetAllDebts();  
            int clearedDebt = 0;

            foreach (var debt in debts)  
            {
                if (debt.Status=="Paid")  
                {
                    clearedDebt += debt.Amount; 
                }
            }
            return clearedDebt;
        }
        public int GetHighestPendingDebt()
        {
            List<DebtModel> debts = GetAllDebts();
            // Get the highest pending debt by sorting the debts in descending order of amount
            var highestPendingDebt = debts
                .Where(d => d.Status == "Pending") // Only consider debts with "Pending" status
                .OrderByDescending(d => d.Amount) // Sort by Amount in descending order
                .FirstOrDefault(); // Get the highest debt or null if none

            return highestPendingDebt?.Amount ?? 0; // Return the amount of the highest pending debt, or 0 if none
        }

        public int GetLowestPendingDebt()
        {
            List<DebtModel> debts = GetAllDebts();
            // Get the lowest pending debt by sorting the debts in ascending order of amount
            var lowestPendingDebt = debts
                .Where(d => d.Status == "Pending") // Only consider debts with "Pending" status
                .OrderBy(d => d.Amount) // Sort by Amount in ascending order
                .FirstOrDefault(); // Get the lowest debt or null if none

            return lowestPendingDebt?.Amount ?? 0; // Return the amount of the lowest pending debt, or 0 if none
        }

        public int GetHighestPaidDebt()
        {
            List<DebtModel> debts = GetAllDebts();
            // Get the highest paid debt by sorting the debts in descending order of amount
            var highestPaidDebt = debts
                .Where(d => d.Status == "Paid") // Only consider debts with "Paid" status
                .OrderByDescending(d => d.Amount) // Sort by Amount in descending order
                .FirstOrDefault(); // Get the highest debt or null if none

            return highestPaidDebt?.Amount ?? 0; // Return the amount of the highest paid debt, or 0 if none
        }

        public int GetLowestPaidDebt()
        {
            List<DebtModel> debts = GetAllDebts();
            // Get the lowest paid debt by sorting the debts in ascending order of amount
            var lowestPaidDebt = debts
                .Where(d => d.Status == "Paid") // Only consider debts with "Paid" status
                .OrderBy(d => d.Amount) // Sort by Amount in ascending order
                .FirstOrDefault(); // Get the lowest debt or null if none

            return lowestPaidDebt?.Amount ?? 0; // Return the amount of the lowest paid debt, or 0 if none
        }



        private void SaveDebts(List<DebtModel> debts)
        {
            var json = JsonSerializer.Serialize(debts, new JsonSerializerOptions { WriteIndented = true });

            File.WriteAllText(DebtFilePath, json);
        }
    }
}
