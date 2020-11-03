using System;
using System.Collections.Generic;
using System.Text;

namespace MyCar
{
    class ReminderWrapper
    {
        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public int? Milleage { get; set; }
        public string Info { get; set; }
        public bool IsChecked { get; set; }
        public int CarId { get; set; }

        public ReminderWrapper(int Id, DateTime Date, int Milleage, string Info,
            bool IsChecked, int carId)
        {
            this.Id = Id;
            this.Date = Date;
            this.Milleage = Milleage;
            this.Info = Info;
            this.IsChecked = IsChecked;
            CarId = carId;
        }
    }
}
