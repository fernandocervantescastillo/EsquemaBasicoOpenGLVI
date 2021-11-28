using Newtonsoft.Json;
using OpenTKGuion.common;
using OpenTKGuion.model;
using System.Collections.Generic;
using System.Linq;

namespace OpenTKGuion.controller
{
    class Guion
    {
        public LinkedList<Accion> l;


        public Guion()
        {
            l = new LinkedList<Accion>();
        }

        public Guion(string JSON)
        {
            Guion guion = JsonConvert.DeserializeObject<Guion>(JSON);
            if (guion == null)
            {
                guion = new Guion();
                return;
            }
            if (guion.l == null)
                this.l = new LinkedList<Accion>();
            else
                this.l = guion.l;
        }

        public void addAccion(int tEjecucion, int rep, int delta, int escenario, string objeto, string accion, float vx, float vy, float vz)
        {
            Accion a = new Accion();
           
            a.tEjecucion = tEjecucion;
            a.rep = rep;
            a.delta = delta;
            a.escenario = escenario;
            a.objeto = objeto;
            a.accion = accion;
            a.valorX = vx;
            a.valorY = vy;
            a.valorZ = vz;

            addAccion(a);
        }

        public void addAccion(Accion accion)
        {
            if(accion.rep != 0)
                l.AddLast(accion);
        }
        
        public Accion getAccion()
        {
            if (l.Count == 0)
                return null;
            Accion a = l.First();
            l.RemoveLast();
            return a;
        }

        public void gestionar(int t, Game game)
        {
            if (l.Count == 0)
                return;

            Accion a = getAccion();
            if(a.tEjecucion <= t)
            {
                accionar(game, a.escenario, a.objeto, a.accion, a.valorX, a.valorY, a.valorZ);
                a.rep = a.rep - 1;
                a.tEjecucion = a.tEjecucion + a.delta;
            }
            addAccion(a);
        }

        public void accionar(Game game, int escenario, string objeto, string accion, float vx, float vy, float vz)
        {
            Escenario _escenario = game.getEscenario(escenario);

            if (accion == Const.ROTAR_ESCENARIO)
            {
                _escenario.rotar(vx, vy, vz);
                return;
            }
            if (accion == Const.TRASLADAR_ESCENARIO)
            {
                _escenario.trasladar(vx, vy, vz);
                return;
            }
            if (accion == Const.AMPLIAR_ESCENARIO)
            {
                _escenario.ampliar(1.0f + vx, 1.0f + vy, 1.0f + vz);
                return;
            }


            Objeto _objeto = _escenario.buscarObjeto(objeto);

            if (accion == Const.ROTAR_OBJETO)
            {
                _objeto.rotar(vx, vy, vz);
                return;
            }
            if (accion == Const.TRASLADAR_OBJETO)
            {
                _objeto.trasladar(vx, vy, vz);
            }
            if (accion == Const.AMPLIAR_OBJETO)
            {
                _objeto.ampliar(1.0f + vx, 1.0f + vy, 1.0f + vz);
            }

            if (accion == Const.ROTAR_OBJETO_EJE)
            {
                _objeto.rotarEje(vx, vy, vz);
            }
        }

        public string toJSon()
        {
            string text = JsonConvert.SerializeObject(this, Formatting.Indented);
            return text;
        }

        
    }

    
}
