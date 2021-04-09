using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_API_DAPPER_DEMO.Models
{
    public class ProductoModel
    {
        public string pro_codigo { get; set; }
        public string pro_nombre { get; set; }
        public string pro_descripcion { get; set; }
        public decimal pro_precio { get; set; }
    }
}

