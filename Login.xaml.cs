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
        private static readonly string loginUri = "https://localhost:44319/User/Login/";

        private static readonly string registerUri = "https://localhost:44319/User/Register/";

        private static string[] errors = { "Invalid credentials.", "Invalid password.", "Not found user with that login.", "There is user with that login." };

        private bool IsLogged { get; set; }

        public Login()
        {
            InitializeComponent();
            IsLogged = false;
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string Login = login.Text;
            string Password = password.Password;
            string trimmedLogin = String.Concat(Login.Where(c => !Char.IsWhiteSpace(c)));
            string trimmedPassword = String.Concat(Password.Where(c => !Char.IsWhiteSpace(c)));
            if (!Validation(Login, Password, trimmedLogin, trimmedPassword))
                return;
            string utfString = UserOperation(Login, Password, loginUri);
            if (errors.Contains(utfString))
                MessageBox.Show(utfString, "Error", MessageBoxButton.OK);
            else
            {
                UserWrapper userWrapper = JsonConvert.DeserializeObject<UserWrapper>(utfString);
                Singleton.GetInstance().ActualUser = userWrapper;
                IsLogged = true;
                ShowMainWindow();
            }
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            string Login = login.Text;
            string Password = password.Password;
            string trimmedLogin = String.Concat(Login.Where(c => !Char.IsWhiteSpace(c)));
            string trimmedPassword = String.Concat(Password.Where(c => !Char.IsWhiteSpace(c)));
            if (!Validation(Login, Password, trimmedLogin, trimmedPassword))
                return;
            string utfString = UserOperation(Login, Password, registerUri);
            if (errors.Contains(utfString))
                MessageBox.Show(utfString, "Error", MessageBoxButton.OK);
            else if (utfString == "Registered")
            {
                string utfStringLogin = UserOperation(Login, Password, loginUri);
                if (errors.Contains(utfStringLogin))
                    MessageBox.Show(utfStringLogin, "Error", MessageBoxButton.OK);
                else
                {
                    UserWrapper userWrapper = JsonConvert.DeserializeObject<UserWrapper>(utfStringLogin);
                    Singleton.GetInstance().ActualUser = userWrapper;
                    IsLogged = true;
                    ShowMainWindow();
                }
            }
        }

        private string UserOperation(string Login, string Password, string Uri)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("Login", Login);
            parameters.Add("Password", Password);
            RequestProvider requestProvider = new RequestProvider(Uri);
            requestProvider.Parameters = parameters;
            byte[] response = requestProvider.performPost();
            return Encoding.UTF8.GetString(response, 0, response.Length);
        }

        private bool Validation(string Login, string Password, string trimmedLogin, string trimmedPassword)
        {
            if (string.IsNullOrWhiteSpace(Login) || string.IsNullOrEmpty(Login) || !Login.Equals(trimmedLogin))
            {
                MessageBox.Show("Not valid login.", "Error", MessageBoxButton.OK);
                return false;
            }
            if (string.IsNullOrWhiteSpace(Password) || string.IsNullOrEmpty(Password) || !Password.Equals(trimmedPassword))
            {
                MessageBox.Show("Not valid password.", "Error", MessageBoxButton.OK);
                return false;
            }
            return true;
        }

        private void ShowMainWindow()
        {
            MainWindow mainWindow = new MainWindow();
            Application.Current.MainWindow = mainWindow;
            mainWindow.Show();
            Close();
        }

        public void Login_Closing(object sender, CancelEventArgs e)
        {
            if (!IsLogged)
                ClosingBehavior(sender, e);
            
        }

        private void ClosingBehavior(object sender, CancelEventArgs e)
        {
            if (MessageBox.Show("Do you want to close application?", "Exit", MessageBoxButton.YesNo) == MessageBoxResult.No)
                e.Cancel = true;
            else
                Application.Current.Shutdown();
        }
    }
}
