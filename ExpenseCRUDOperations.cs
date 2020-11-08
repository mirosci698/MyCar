using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Linq;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace MyCar
{
    class ExpenseCRUDOperations
    {
        private static readonly string createUri = "https://localhost:44319/Expense/Create/";

        private static readonly string readUri = "https://localhost:44319/Expense/Read/";

        private static readonly string updateUri = "https://localhost:44319/Expense/Update/";

        private static readonly string deleteUri = "https://localhost:44319/Expense/Delete/";

        private static string[] errorsRead = { "Invalid id.", "Not found." };

        private static string[] errorsDelete = { "Invalid id.", "Not found." };

        private static string[] errorsCreate = { "Invalid credentials.", "Invalid car id.", "Wrong date.", "Mileage cannot be lower than purchase mileage.",
        "Price cannot be lower than zero.", "Error in date format."};

        private static string[] errorsUpdate = { "Invalid credentials.", "Invalid car id.", "Wrong date.", "Mileage cannot be lower than purchase mileage.",
        "Price cannot be lower than zero.", "Error in date format."};

        public static string CreateExpense(ComboBox comboBoxExpenses, Dictionary<string, string> paramDictionary, ref int counter)
        {
            RequestProvider requestProvider = new RequestProvider(createUri);
            requestProvider.Parameters = paramDictionary;
            byte[] response = requestProvider.performPost();
            string encodedResponse = Encoding.UTF8.GetString(response, 0, response.Length);
            if (errorsCreate.Contains(encodedResponse))
                return encodedResponse;
            ExpenseWrapper expenseWrapper = JsonConvert.DeserializeObject<ExpenseWrapper>(encodedResponse);
            Singleton.GetInstance().ActualCar.ExpenseList.Add(expenseWrapper.Id);
            ObservableCollection<CmbElement> cmbList = (ObservableCollection<CmbElement>)comboBoxExpenses.ItemsSource;
            CmbElement cmbElementActual = new CmbElement { Id = counter++, Value = expenseWrapper.Id.ToString() };
            cmbList.Add(cmbElementActual);
            comboBoxExpenses.ItemsSource = cmbList;
            comboBoxExpenses.SelectedItem = cmbElementActual;
            return "Correct";
        }

        public static string UpdateExpense(Dictionary<string, string> paramDictionary)
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

        public static Expense ReadExpenseById(int id)
        {
            RequestProvider requestProvider = new RequestProvider(readUri);
            requestProvider.Id = id;
            byte[] response = requestProvider.performGet();
            string encodedResponse = Encoding.UTF8.GetString(response, 0, response.Length);
            if (errorsRead.Contains(encodedResponse))
            {
                MainWindow.ShowErrorInfo(encodedResponse);
                return new Expense();
            }
            else
            {
                ExpenseWrapper expenseWrapper = JsonConvert.DeserializeObject<ExpenseWrapper>(encodedResponse);
                Singleton.GetInstance().ActualExpense = expenseWrapper;
                return new Expense()
                {
                    ExpenseId = expenseWrapper.Id,
                    ExpenseDate = expenseWrapper.Date,
                    ExpensePlace = expenseWrapper.Place,
                    ExpenseInfo = expenseWrapper.Info,
                    ExpenseMileage = expenseWrapper.Mileage,
                    ExpensePrice = (double)expenseWrapper.Price,
                    ExpenseTypeInfo = expenseWrapper.TypeInfo
                };
            }
        }

        public static string DeleteExpenseById(int id, ComboBox comboBoxExpenses)
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
                CmbElement cmbElementDeleted = (CmbElement)comboBoxExpenses.SelectedItem;
                ObservableCollection<CmbElement> cmbList = (ObservableCollection<CmbElement>)comboBoxExpenses.ItemsSource;
                CmbElement cmbElementNext = cmbList.Single(i => i.Value == "<Create>");
                comboBoxExpenses.SelectedItem = cmbElementNext;
                cmbList.Remove(cmbElementDeleted);
                comboBoxExpenses.ItemsSource = cmbList;
                Singleton.GetInstance().ActualCar.ExpenseList.Remove(id);
                return encodedResponse;
            }
        }
    }
}
