using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppCorePostgreSQL.models
{
    public class products
    {
        public int Product_id { set; get; }
        public string Product_name { set; get; }
        public double Price { set; get; }
        public int Stock { set; get; }


    }
}
