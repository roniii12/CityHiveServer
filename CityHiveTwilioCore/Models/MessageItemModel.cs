using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityHiveTwilioCore.Models
{
    public class MessageItemModel
    {
        public string Body { get; set; }
        public string To { get; set; }
        public DateTime? Date { get; set; }
    }
}
