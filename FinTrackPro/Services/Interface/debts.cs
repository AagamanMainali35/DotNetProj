using System;
using System.Collections.Generic;
using FinTrackPro.Models;

namespace FinTrackPro.Services
{
    public interface IDebtService
    {
        List<DebtModel> GetDebtsByStatus(string status);
        void AddNewDebt(DebtModel newDebt);
        void UpdateDebtStatus(Guid debtId, string newStatus);
        void MarkDebtAsPaid(Guid debtId);
    }
}
