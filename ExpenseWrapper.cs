using System;
using System.Collections.Generic;
using System.Text;

namespace MyCar
{
    class ExpenseWrapper
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Place { get; set; }
        public int Mileage { get; set; }
        public decimal Price { get; set; }
        public string Info { get; set; }
        public string TypeInfo { get; set; }
        public int CarId { get; set; }

        public ExpenseWrapper(int Id, DateTime Date, string Place, int Mileage,
            decimal Price, string Info, string TypeInfo, int expenseId)
        {
            this.Id = Id;
            this.Date = Date;
            this.Place = Place;
            this.Mileage = Mileage;
            this.Price = Price;
            this.Info = Info;
            this.TypeInfo = TypeInfo;
            CarId = expenseId;
        }
    }
}
