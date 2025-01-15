using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTrackPro.Models
{

        public class DebtModel
        {
            public Guid Id { get; set; } = Guid.NewGuid(); 
            public string Label { get; set; } 
            public string Payee { get; set; } 
            public int Amount { get; set; }
            public DateTime ?DueDate { get; set; }
            public string Tag { get; set; }
            public string Type { get; set; } = "Debt"; 
            public string Status { get; set; }

        }
    }

