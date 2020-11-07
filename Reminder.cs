using System;
using System.Collections.Generic;
using System.Text;

namespace MyCar
{
    class Reminder
    {
        public int ReminderId { get; set; }
        public string ReminderDate { get; set; }
        public int? ReminderMilleage { get; set; }
        public string ReminderInfo { get; set; }
        public bool ReminderIsChecked { get; set; }
    }
}
