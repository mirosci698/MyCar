using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using Newtonsoft.Json;
using System.Linq;
using System.Collections.ObjectModel;

namespace MyCar
{
    class ReminderCRUDOperations
    {
        private static readonly string createUri = "https://localhost:44319/Reminder/Create/";

        private static readonly string readUri = "https://localhost:44319/Reminder/Read/";

        private static readonly string updateUri = "https://localhost:44319/Reminder/Update/";

        private static readonly string deleteUri = "https://localhost:44319/Reminder/Delete/";

        private static string[] errorsRead = { "Invalid id.", "Not found." };

        private static string[] errorsDelete = { "Invalid id.", "Not found." };

        private static string[] errorsCreate = { "Invalid credentials.", "Invalid car id.", "Reminder cannot occur in the past.", "Actual mileage cannot be lower than purchase mileage.",
        "Error in date format."};

        private static string[] errorsUpdate = { "Invalid credentials.", "Invalid car id.", "Reminder cannot occur in the past.", "Actual mileage cannot be lower than purchase mileage.",
        "Error in date format."};

        public static string CreateReminder(ComboBox comboBoxReminders, Dictionary<string, string> paramDictionary, ref int counter)
        {
            RequestProvider requestProvider = new RequestProvider(createUri);
            requestProvider.Parameters = paramDictionary;
            byte[] response = requestProvider.performPost();
            string encodedResponse = Encoding.UTF8.GetString(response, 0, response.Length);
            if (errorsCreate.Contains(encodedResponse))
                return encodedResponse;
            ReminderWrapper reminderWrapper = JsonConvert.DeserializeObject<ReminderWrapper>(encodedResponse);
            Singleton.GetInstance().ActualCar.ReminderList.Add(reminderWrapper.Id);
            ObservableCollection<CmbElement> cmbList = (ObservableCollection<CmbElement>)comboBoxReminders.ItemsSource;
            CmbElement cmbElementActual = new CmbElement { Id = counter++, Value = reminderWrapper.Id.ToString() };
            cmbList.Add(cmbElementActual);
            comboBoxReminders.ItemsSource = cmbList;
            comboBoxReminders.SelectedItem = cmbElementActual;
            return "Correct";
        }

        public static string UpdateReminder(Dictionary<string, string> paramDictionary)
        {
            RequestProvider requestProvider = new RequestProvider(updateUri);
            Dictionary<string, string> parameters = paramDictionary;
            requestProvider.Parameters = parameters;
            byte[] response = requestProvider.performPost();
            string encodedResponse = Encoding.UTF8.GetString(response, 0, response.Length);
            if (errorsUpdate.Contains(encodedResponse))
                return encodedResponse;
            return encodedResponse;
        }

        public static Reminder ReadReminderById(int id)
        {
            RequestProvider requestProvider = new RequestProvider(readUri);
            requestProvider.Id = id;
            byte[] response = requestProvider.performGet();
            string encodedResponse = Encoding.UTF8.GetString(response, 0, response.Length);
            if (errorsRead.Contains(encodedResponse))
            {
                MainWindow.ShowErrorInfo(encodedResponse);
                return new Reminder();
            }
            else
            {
                ReminderWrapper reminderWrapper = JsonConvert.DeserializeObject<ReminderWrapper>(encodedResponse);
                Singleton.GetInstance().ActualReminder = reminderWrapper;
                return new Reminder()
                {
                    ReminderId = reminderWrapper.Id,
                    ReminderDate = reminderWrapper.Date,
                    ReminderInfo = reminderWrapper.Info,
                    ReminderIsChecked = reminderWrapper.IsChecked,
                    ReminderMilleage = reminderWrapper.Milleage
                };
            }
        }

        public static string DeleteReminderById(int id, ComboBox comboBoxReminders)
        {
            RequestProvider requestProvider = new RequestProvider(deleteUri);
            requestProvider.Id = id;
            byte[] response = requestProvider.performGet();
            string encodedResponse = Encoding.UTF8.GetString(response, 0, response.Length);
            if (errorsDelete.Contains(encodedResponse))
            {
                return encodedResponse;
            }
            else
            {
                CmbElement cmbElementDeleted = (CmbElement)comboBoxReminders.SelectedItem;
                ObservableCollection<CmbElement> cmbList = (ObservableCollection<CmbElement>)comboBoxReminders.ItemsSource;
                CmbElement cmbElementNext = cmbList.Single(i => i.Value == "<Create>");
                comboBoxReminders.SelectedItem = cmbElementNext;
                cmbList.Remove(cmbElementDeleted);
                comboBoxReminders.ItemsSource = cmbList;
                Singleton.GetInstance().ActualCar.ReminderList.Remove(id);
                return encodedResponse;
            }
        }

    }
}
