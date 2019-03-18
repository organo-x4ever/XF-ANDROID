using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.organo.xchallenge.Models.User
{
    public class TrackerEditViewModel
    {
        public double NewValue { get; set; }
        public double OldValue { get; set; }
        public DateTime LastModifyDate { get; set; }
    }
}