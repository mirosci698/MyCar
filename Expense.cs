using System;
using System.Collections.Generic;
using System.Text;

namespace MyCar
{
    class Expense
    {
        public int? ExpenseId { get; set; }
        public string ExpenseDate { get; set; }
        public string ExpensePlace { get; set; }

        public int? ExpenseMileage { get; set; }

        public double? ExpensePrice { get; set; }
        public string ExpenseInfo { get; set; }
        public string ExpenseTypeInfo { get; set; }
    }
}
