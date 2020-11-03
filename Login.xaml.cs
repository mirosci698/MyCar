using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Newtonsoft.Json;

namespace MyCar
{
    /// <summary>
    /// Logika interakcji dla klasy Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        private string loginUri = "https://localhost:44319/User/Login/";

        private string registerUri = "https://localhost:44319/User/Register/";

        private string[] errors = { "Invalid credentials.", "Invalid password.", "Not found user with that login.", "There is user with that login." };

        public Login()
        {
            InitializeComponent();
            
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string Login = login.Text;
            string Password = password.Text;
            string utfString = LoginOperation(Login, Password);
            if (errors.Contains(utfString))
                MessageBox.Show(utfString, "Error", MessageBoxButton.OK);
            else
            {
                UserWrapper userWrapper = JsonConvert.DeserializeObject<UserWrapper>(utfString);
                Singleton.GetInstance().ActualUser = userWrapper;
            }
        }

        private string LoginOperation(string Login, string Password)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("Login", Login);
            parameters.Add("Password", Password);
            RequestProvider requestProvider = new RequestProvider(loginUri);
            requestProvider.Parameters = parameters;
            byte[] response = requestProvider.performPost();
            return Encoding.UTF8.GetString(response, 0, response.Length);
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            string Login = login.Text;
            string Password = password.Text;
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("Login", Login);
            parameters.Add("Password", Password);
            RequestProvider requestProvider = new RequestProvider(registerUri);
            requestProvider.Parameters = parameters;
            byte[] response = requestProvider.performPost();
            string utfString = Encoding.UTF8.GetString(response, 0, response.Length);
            if (errors.Contains(utfString))
                MessageBox.Show(utfString, "Error", MessageBoxButton.OK);
            else if (utfString == "Registered")
            {
                UserWrapper userWrapper = new UserWrapper()
            }
        }

            public void Login_Closing(object sender, CancelEventArgs e)
        {
            if (MessageBox.Show("Do you want to close application?", "Exit", MessageBoxButton.YesNo) == MessageBoxResult.No)
                e.Cancel = true;
            else
                Application.Current.Shutdown();
        }
    }
}
