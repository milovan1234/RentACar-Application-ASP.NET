using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyWebApp.Models
{
    public class Automobile
    {
        public int Id { get; set; }
        [Required]
        public string CarBrand { get; set; }
        [Required]
        public string CarModel { get; set; }
        [Required]
        //Godina proizvodnje
        public int ProductYear { get; set; }
        [Required]
        //Kubikaza
        public int Cubicase { get; set; }

        //Broj vrata
        public NumberOfDoor NumberOfDoor { get; set; }
        public int NumberOfDoorId { get; set; }

        //Karoserija
        public CarBody CarBody { get; set; }
        public int CarBodyId { get; set; }

        //Menjac
        public Gearshift Gearshift { get; set; }
        public int GearshiftId { get; set; }
        public string ImagePath { get; set; }

        public string this[string propertyName]
        {
            get
            {
                switch (propertyName)
                {
                    case "CarBrand":
                        return CarBrand.ToString();
                    case "CarModel":
                        return CarModel.ToString();
                    case "Id":
                        return Id.ToString();
                    case "ProductYear":
                        return ProductYear.ToString();
                    case "Cubicase":
                        return Cubicase.ToString();
                    case "NumberOfDoorId":  
                        return NumberOfDoorId.ToString();
                    case "CarBodyId":
                        return CarBodyId.ToString();
                    case "GearshiftId":
                        return GearshiftId.ToString();
                    default:
                        return "";
                }
            }
        }
    }
}