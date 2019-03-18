using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.organo.x4ever.Models
{
    public class Menu
    {
        public Int16 ID { get; set; }
        public string MenuTitle { get; set; }
        public string MenuType { get; set; }
        public short MenuTypeCode { get; set; }
        public string MenuIcon { get; set; }
        public bool MenuIconVisible { get; set; }
        public bool MenuActive { get; set; }
        public DateTime ModifyDate { get; set; }
    }
}