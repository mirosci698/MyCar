using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using Newtonsoft.Json;

namespace MyCar
{
    class CarCRUDOperations
    {
        private static readonly string createUri = "https://localhost:44319/Car/Create/";

        private static readonly string readUri = "https://localhost:44319/Car/Read/";

        private static readonly string updateUri = "https://localhost:44319/Car/Update/";

        private static readonly string deleteUri = "https://localhost:44319/Car/Delete/";

        private static string[] errorsRead = { "Invalid id.", "Not found." };

        private static string[] errorsDelete = { "Invalid id.", "Not found." };

        private static string[] errorsCreate = { "Invalid credentials.", "Invalid user id.", "First registration date from future.", "Purchase date before first registration date.",
        "Purchase mileage cannot be negative.", "Actual mileage cannot be lower than purchase mileage.", "Error in date format."};

        private static string[] errorsUpdate = { "Invalid credentials.", "Invalid car id.", "First registration date from future.", "Purchase date before first registration date.",
        "Purchase mileage cannot be negative.", "Actual mileage cannot be lower than purchase mileage.", "Error in date format."};
        public static string CreateCar(ComboBox comboBoxCars, Dictionary<string, string> paramDictionary, ref int counter)
        {
            RequestProvider requestProvider = new RequestProvider(createUri);
            requestProvider.Parameters = paramDictionary;
            byte[] response = requestProvider.performPost();
            string encodedResponse = Encoding.UTF8.GetString(response, 0, response.Length);
            if (errorsCreate.Contains(encodedResponse))
                return encodedResponse;
            CarWrapper carWrapper = JsonConvert.DeserializeObject<CarWrapper>(encodedResponse);
            Singleton.GetInstance().ActualUser.CarList.Add(carWrapper.Id);
            ObservableCollection<CmbElement> cmbList = (ObservableCollection<CmbElement>)comboBoxCars.ItemsSource;
            CmbElement cmbElementActual = new CmbElement { Id = counter++, Value = carWrapper.Id.ToString() };
            cmbList.Add(cmbElementActual);
            comboBoxCars.ItemsSource = cmbList;
            comboBoxCars.SelectedItem = cmbElementActual;
            return "Correct";
        }

        public static string UpdateCar(Dictionary<string, string> paramDictionary)
        {
            RequestProvider requestProvider = new RequestProvider(updateUri);
            Dictionary<string, string> parameters = paramDictionary;
            //parameters.Remove("Id");
            requestProvider.Parameters = parameters;
            byte[] response = requestProvider.performPost();
            string encodedResponse = Encoding.UTF8.GetString(response, 0, response.Length);
            if (errorsUpdate.Contains(encodedResponse))
                return encodedResponse;
            return encodedResponse;
        }

        public static Car ReadCarById(int id)
        {
            RequestProvider requestProvider = new RequestProvider(readUri);
            requestProvider.Id = id;
            byte[] response = requestProvider.performGet();
            string encodedResponse = Encoding.UTF8.GetString(response, 0, response.Length);
            if (errorsRead.Contains(encodedResponse))
            {
                MainWindow.ShowErrorInfo(encodedResponse);
                return new Car();
            }
            else
            {
                CarWrapper carWrapper = JsonConvert.DeserializeObject<CarWrapper>(encodedResponse);
                Singleton.GetInstance().ActualCar = carWrapper;
                return new Car()
                {
                    CarId = carWrapper.Id,
                    CarManufacturer = carWrapper.Manufacturer,
                    CarModel = carWrapper.Model,
                    CarFirstRegistrationDate = carWrapper.FirstRegistrationDate,
                    CarActualMileage = carWrapper.ActualMileage,
                    CarLicencePlate = carWrapper.LicencePlate,
                    CarPurchaseDate = carWrapper.PurchaseDate,
                    CarPurchaseMileage = carWrapper.PurchaseMileage,
                    CarVin = carWrapper.Vin
                };
            }
        }

        public static string DeleteCarById(int id, ComboBox comboBoxCars)
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
                CmbElement cmbElementDeleted = (CmbElement)comboBoxCars.SelectedItem;
                ObservableCollection<CmbElement> cmbList = (ObservableCollection<CmbElement>)comboBoxCars.ItemsSource;
                CmbElement cmbElementNext = cmbList.Single(i => i.Value == "<Create>");
                comboBoxCars.SelectedItem = cmbElementNext;
                cmbList.Remove(cmbElementDeleted);
                comboBoxCars.ItemsSource = cmbList;
                return encodedResponse;
            }
        }

    }
}
