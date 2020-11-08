using System;
using System.Collections.Generic;
using System.Text;

namespace MyCar
{
    class Car
    {
        public int? CarId { get; set; }

        public string CarManufacturer { get; set; }

        public string CarModel { get; set;  }

        public string CarFirstRegistrationDate { get; set; }

        public int? CarActualMileage { get; set; }

        public string CarVin { get; set; }

        public string CarPurchaseDate { get; set; }
        public int? CarPurchaseMileage { get; set; }
        public string CarLicencePlate { get; set; }
    }
}
