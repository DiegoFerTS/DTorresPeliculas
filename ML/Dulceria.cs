using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class Dulceria
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public string Descripcion { get; set; }
        public string Imagen { get; set; }
        public List<object> Productos { get; set; }

        // Este atributo no esta en la base de datos
        public int Cantidad { get; set; }
    }
}
