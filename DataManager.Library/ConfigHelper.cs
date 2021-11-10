using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataManager.Library
{
    public class ConfigHelper 
    {
        public static decimal GetTaxRate()
        {
            decimal output = 0;
            string rateText = ConfigurationManager.AppSettings["taxRate"];
            bool isValid = Decimal.TryParse(rateText, out output);
            if (isValid == false)
            {
                throw new ConfigurationErrorsException("Taksa nije postavljnea ispravno");
            }
            return output;
        }
    }
}
