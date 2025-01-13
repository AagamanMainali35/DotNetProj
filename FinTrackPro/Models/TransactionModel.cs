using System;
namespace FinTrackPro.Models
{
    public class TransactionModel
    {
        public Guid Id { get; set; }
        public string? Label { get; set; } 
        public string? Payee { get; set; } 
        public string? Tag { get; set; }
        public string? Note { get; set; }
        public string? CustomTag { get; set; }
        public string? type { get; set; } 
        public int Amount { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
    }
}
