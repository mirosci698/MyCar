using System;
using System.Collections.Generic;
using System.Text;

namespace MyCar
{
    class UserWrapper
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public List<int> CarList { get; set; }
        public UserWrapper(int Id, string Login, string Password, List<int> carList)
        {
            this.Id = Id;
            this.Login = Login;
            this.Password = Password;
            CarList = carList;
        }
    }
}
