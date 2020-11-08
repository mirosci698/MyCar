using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Converters;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;

namespace MyCar
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int counter = 1;

        private bool isCreating;

        private Car carForBinding;

        public MainWindow()
        {
            InitializeComponent();
            List<int> carList = Singleton.GetInstance().ActualUser.CarList;
            ObservableCollection<CmbElement> cmbList = new ObservableCollection<CmbElement>();
            CmbElement cmbElementDefault = new CmbElement { Id = 0, Value = "<Create>" };
            cmbList.Add(cmbElementDefault);
            foreach (int number in carList)
                cmbList.Add(new CmbElement { Id = counter++, Value = number.ToString() });
            comboBoxCars.ItemsSource = cmbList;
            comboBoxCars.SelectedItem = cmbElementDefault;
        }

        public static void ShowErrorInfo(string encodedResponse)
        {
            MessageBox.Show(encodedResponse, "Error", MessageBoxButton.OK);
        }

        private void comboBoxCars_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = (CmbElement)comboBoxCars.SelectedItem;
            string value = item.Value;
            if (value == "<Create>")
            {
                setButtonsForCreating();
                carForBinding = new Car();
            }
            else
            {
                try
                {
                    setButtonsForUpdatingAndDeleting();
                    Car car = CarCRUDOperations.ReadCarById(Int32.Parse(value));
                    carForBinding = car;
                }
                catch (Exception)
                {
                    ShowErrorInfo("Error in reading proper id.");
                    CmbElement cmbElementDefault = new CmbElement { Id = 0, Value = "<Create>" };
                    comboBoxCars.SelectedItem = cmbElementDefault;
                    return;
                }
                
            }
            CarTab.DataContext = carForBinding;
        }

        private void setButtonsForCreating()
        {
            isCreating = true;
            buttonCreate.IsEnabled = true;
            buttonUpdate.IsEnabled = false;
            buttonDelete.IsEnabled = false;
        }

        private void setButtonsForUpdatingAndDeleting()
        {
            isCreating = false;
            buttonCreate.IsEnabled = false;
            buttonUpdate.IsEnabled = true;
            buttonDelete.IsEnabled = true;
        }

        private Dictionary<string, string> getDictionaryFromCar(Car car)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            result.Add("Id", car.CarId.ToString());
            result.Add("Manufacturer", car.CarManufacturer);
            result.Add("Model", car.CarModel);
            result.Add("FirstRegistrationDate", car.CarFirstRegistrationDate);
            result.Add("LicencePlate", car.CarLicencePlate);
            result.Add("Vin", car.CarVin);
            result.Add("PurchaseDate", car.CarPurchaseDate);
            result.Add("PurchaseMileage", car.CarPurchaseMileage.ToString());
            result.Add("ActualMileage", car.CarActualMileage.ToString());
            result.Add("UserId", Singleton.GetInstance().ActualUser.Id.ToString());
            foreach (var key in result)
            {
                if (key.Value != null)
                    key.Value.Replace(" ", "%20");
            }
            return result;
        }

        private void buttonCreate_Click(object sender, RoutedEventArgs e)
        {
            string result = CarCRUDOperations.CreateCar(comboBoxCars, getDictionaryFromCar(carForBinding), ref counter);
            if (result != "Correct")
                MessageBox.Show(result, "Error", MessageBoxButton.OK);
        }

        private void buttonUpdate_Click(object sender, RoutedEventArgs e)
        {
            string result = CarCRUDOperations.UpdateCar(getDictionaryFromCar(carForBinding));
            if (result != "Update")
                MessageBox.Show(result, "Error", MessageBoxButton.OK);
            else
                MessageBox.Show(result, "Info", MessageBoxButton.OK);
        }

        private void buttonDelete_Click(object sender, RoutedEventArgs e)
        {
            string result = CarCRUDOperations.DeleteCarById(Singleton.GetInstance().ActualCar.Id, comboBoxCars);
            if (result != "Removed")
                MessageBox.Show(result, "Error", MessageBoxButton.OK);
            else
                MessageBox.Show(result, "Info", MessageBoxButton.OK);
        }
    }
}
