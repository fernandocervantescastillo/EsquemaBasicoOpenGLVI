using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTKGuion.model
{
    class Accion
    {
        public int tEjecucion { get; set; }
        public int rep { get; set; }
        public int delta { get; set; }
        public int escenario { get; set; }
        public string objeto { get; set; }
        public string accion { get; set; }
        public float valorX { get; set; }
        public float valorY { get; set; }
        public float valorZ { get; set; }

    }
}
