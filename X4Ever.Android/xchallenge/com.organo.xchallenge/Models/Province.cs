using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.organo.xchallenge.Models
{
    public class Province
    {
        public int ID { get; set; }
        public int CountryId { get; set; }
        public string ProvinceCode { get; set; }
        public string ProvinceName { get; set; }
    }
}