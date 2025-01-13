using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTrackPro.Models
{
    public class ActionModel
    {
        public decimal TransactionAmount { get; set; }
        public string Label { get; set; }
        public DateTime Date { get; set; }
        public string Payee { get; set; }
        public string Tag { get; set; }
    }

}
