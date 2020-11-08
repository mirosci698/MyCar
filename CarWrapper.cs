using System;
using System.Collections.Generic;
using System.Text;

namespace MyCar
{
    class CarWrapper
    {
        public int Id { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public string FirstRegistrationDate { get; set; }
        public int ActualMileage { get; set; }
        public string Vin { get; set; }
        public string PurchaseDate { get; set; }
        public int PurchaseMileage { get; set; }
        public string LicencePlate { get; set; }
        public int UserId { get; set; }
        public List<int> ExpenseList { get; set; }
        public List<int> ReminderList { get; set; }

        public CarWrapper(int Id, string Manufacturer, string Model, string FirstRegistrationDate,
            int ActualMileage, string Vin, string PurchaseDate, int PurchaseMileage, 
            string LicencePlate, int userId, List<int> expenseList, List<int> reminderList)
        {
            this.Id = Id;
            this.Manufacturer = Manufacturer;
            this.Model = Model;
            this.FirstRegistrationDate = FirstRegistrationDate;
            this.ActualMileage = ActualMileage;
            this.Vin = Vin;
            this.PurchaseDate = PurchaseDate;
            this.PurchaseMileage = PurchaseMileage;
            this.LicencePlate = LicencePlate;
            UserId = userId;
            ExpenseList = expenseList;
            ReminderList = reminderList;
        }
    }
}
