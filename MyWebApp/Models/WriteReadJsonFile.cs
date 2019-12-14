using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace MyWebApp.Models
{
    public class WriteReadJsonFile
    {

        public static bool WriteList<T>(List<T> list, string fileName)
        {

            try
            {
                string json = JsonConvert.SerializeObject(list, Formatting.Indented, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects });
                File.WriteAllText(@"D:\Users\Milovan Srejic\source\repos\MyWebApp\MyWebApp\Files/" + fileName, json);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static bool ReadList<T>(ref List<T> list, string fileName)
        {
            string file = @"D:\Users\Milovan Srejic\source\repos\MyWebApp\MyWebApp\Files/" + fileName;
            if (!File.Exists(file))
            {
                File.Create(file);
                return true;
            }            
            else
                try
                {
                    string json = File.ReadAllText(@"D:\Users\Milovan Srejic\source\repos\MyWebApp\MyWebApp\Files/" + fileName);
                    list = JsonConvert.DeserializeObject<List<T>>(json, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects });
                    return true;
                }
                catch
                {
                    return false;
                }
        }
    }
}