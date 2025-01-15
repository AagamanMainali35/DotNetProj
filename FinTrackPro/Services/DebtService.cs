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
                totalDebt += debt.Amount; 
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


        private void SaveDebts(List<DebtModel> debts)
        {
            var json = JsonSerializer.Serialize(debts, new JsonSerializerOptions { WriteIndented = true });

            File.WriteAllText(DebtFilePath, json);
        }
    }
}
