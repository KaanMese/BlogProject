using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogProject.Models.Entities
{
    public class Subscribe
    {
        public int ID { get; set; }

        public string EmailAdress { get; set; }

        public DateTime CreDate { get; set; }
    }
}
