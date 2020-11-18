using System;
using System.Collections.Generic;
using System.Text;

namespace MyCar
{
    class Singleton
    {
        private Singleton() { }

        private static Singleton _instance;

        public static Singleton GetInstance()
        {
            if (_instance == null)
            {
                _instance = new Singleton();
            }
            return _instance;
        }

        public UserWrapper ActualUser { get; set; }

        public CarWrapper ActualCar { get; set; }

        public ExpenseWrapper ActualExpense { get; set; }

        public ReminderWrapper ActualReminder { get; set; }
    }
}
