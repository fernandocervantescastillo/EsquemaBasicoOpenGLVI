using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using OpenTKGuion.common;
using OpenTKGuion.model;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTKGuion.controller;

namespace OpenTKGuion
{
    public partial class Form1 : Form
    {

        private Timer _timer;
        private int interval;
        Game game;
        Guion guion;


        public Form1()
        {
            InitializeComponent();
            
            game = new Game();
            guion = new Guion();

        }

        private void glControl1_Load(object sender, EventArgs e)
        {

            glControl.MakeCurrent();
            glControl.VSync = true;
           

            OnLoad1();
            glControl.Resize += glControl_Resize;
            glControl.Paint += glControl_Paint;

            interval = 1000/20;//20 FPS
            _timer = new Timer();
            _timer.Tick += Render;

            _timer.Interval = interval; ;
            _timer.Start();
        }

        private void glControl_Resize(object sender, EventArgs e)
        {
            glControl.MakeCurrent();
            if (glControl.ClientSize.Height <= 0)
                return;
            if (glControl.ClientSize.Width <= 0)
                return;
            game.OnResize(glControl.Width, glControl.Height);
        }

        private void glControl_Paint(object sender, PaintEventArgs e)
        {
            Render(sender, e);
        }

        int tiempo = 0;
        private void Render(object sender, EventArgs e)
        {
            tiempo = tiempo + interval;
            glControl.MakeCurrent();

            game.OnRenderFrame();

            if(aux == 1)
                guion.gestionar(tiempo, game);

            glControl.SwapBuffers();
        }

        public void OnLoad1()
        {
            glControl.MakeCurrent();
            game.OnLoad(glControl.Width, glControl.Height);


            cargarObjetosComboBox();
            cargarEscenario();
            cargarObjetosEscenario();
            cargarObjetosEscenarioGuion();
            cargarAcciones();
            cargarGuion();
        }

        private void glControl_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            
        }

        private void glControl_KeyDown(object sender, KeyEventArgs e)
        {
            game.OnKeyDown(e, interval);
        }

        private void glControl_MouseMove(object sender, MouseEventArgs e)
        {
            game.onMouseMove(e);
        }


        protected override void OnClosed(EventArgs e)
        {
            game.OnUnload();
            _timer.Stop();
            base.OnClosed(e);
        }


        private int longitudACortar(string t)
        {
            int c = 0;
            for(int i = t.Length - 1; i >= 0; i--)
            {
                if (t.Substring(i, 1).CompareTo("\\") == 0)
                    return c;
                c++;

            }
            return 0;
        }

        private string cortarCadena(string t)
        {
            int c = longitudACortar(t);
            string u = t.Substring(t.Length - c);
            u = u.Substring(0, u.Length - 4);
            return u;
        }

        private void cargarObjetosComboBox()
        {
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
            path = path.Remove(0, 6);
            path = path + "..\\..\\..\\files\\";

            string[] array1 = Directory.GetFiles(path);

            foreach (string name in array1)
            {
                comboBox4.Items.Add(cortarCadena(name));
            }

            if (comboBox4.Items.Count > 0)
            {
                comboBox4.Text = comboBox4.Items[0].ToString();
            }


        }

        private void cargarObjetosEscenario()
        {
            comboBox2.Items.Clear();

            if (comboBox1.SelectedItem == null)
                return;
            Escenario escenario = game.getEscenario(comboBox1.SelectedIndex);
            
            for(int i = 0; i < escenario.getCantidad(); i++)
            {
                comboBox2.Items.Add(escenario.getObjeto(i).getNombre());
            }

            if (escenario.getCantidad() > 0)
            {
                comboBox2.Text = comboBox2.Items[0].ToString();
            }
        }

        private void cargarObjetosEscenarioGuion()
        {
            comboBox6.Items.Clear();

            if (comboBox5.SelectedItem == null)
                return;
            Escenario escenario = game.getEscenario(comboBox5.SelectedIndex);

            for (int i = 0; i < escenario.getCantidad(); i++)
            {
                comboBox6.Items.Add(escenario.getObjeto(i).getNombre());
            }

            if (escenario.getCantidad() > 0)
            {
                comboBox6.Text = comboBox6.Items[0].ToString();
            }
        }

        private void cargarEscenario()
        {
            comboBox1.Items.Clear();
            comboBox5.Items.Clear();

            for (int i = 0; i < game.getCountEscenario(); i++)
            {
                comboBox1.Items.Add("Escenario " + i);
                comboBox5.Items.Add("Escenario " + i);
            }

            if (comboBox1.Items.Count > 0)
            {
                comboBox1.Text = comboBox1.Items[0].ToString();
                comboBox5.Text = comboBox1.Items[0].ToString();
            }

        }

        private void cargarAcciones()
        {
            comboBox3.Items.Add(Const.ROTAR_ESCENARIO);
            comboBox3.Items.Add(Const.AMPLIAR_ESCENARIO);
            comboBox3.Items.Add(Const.TRASLADAR_ESCENARIO);
            comboBox3.Items.Add(Const.ROTAR_OBJETO);
            comboBox3.Items.Add(Const.AMPLIAR_OBJETO);
            comboBox3.Items.Add(Const.TRASLADAR_OBJETO);
            comboBox3.Items.Add(Const.ROTAR_OBJETO_EJE);

            comboBox3.Text = comboBox3.Items[0].ToString();


            comboBox7.Items.Add(Const.ROTAR_ESCENARIO);
            comboBox7.Items.Add(Const.AMPLIAR_ESCENARIO);
            comboBox7.Items.Add(Const.TRASLADAR_ESCENARIO);
            comboBox7.Items.Add(Const.ROTAR_OBJETO);
            comboBox7.Items.Add(Const.AMPLIAR_OBJETO);
            comboBox7.Items.Add(Const.TRASLADAR_OBJETO);
            comboBox7.Items.Add(Const.ROTAR_OBJETO_EJE);

            comboBox7.Text = comboBox3.Items[0].ToString();
        }

        private void cargarGuion()
        {
            comboBox8.Items.Clear();
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
            path = path.Remove(0, 6);
            path = path + "..\\..\\..\\guiones\\";

            string[] array1 = Directory.GetFiles(path);

            foreach (string name in array1)
            {
                comboBox8.Items.Add(cortarCadena(name));
            }

            if (comboBox8.Items.Count > 0)
            {
                comboBox8.Text = comboBox8.Items[0].ToString();

                //Leemos lo que hay en ese archivo
                path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
                path = path.Remove(0, 6);
                path = path + "..\\..\\..\\guiones";
                path = path + "\\" + comboBox8.Items[0].ToString() + ".txt";

                if (!File.Exists(path))
                {
                    guion = new Guion();
                    textBox3.Text = "";
                }
                else
                {
                    string text = File.ReadAllText(path);
                    textBox3.Text = text;
                    guion = new Guion(text);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Escenario escenario = new Escenario();
            game.addEscenario(escenario);
            cargarEscenario();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null)
                return;

            if (comboBox4.SelectedItem == null)
                return;

            string key = textBox1.Text;

            if (key.CompareTo("") == 0)
            {
                label9.Text = "Ingrese nombre";
                return;
            }

            int index = comboBox1.SelectedIndex;
            Escenario escenario = game.getEscenario(index);
            string t = TextFile.getFileText(comboBox4.SelectedItem.ToString());

            if (escenario.existeObjeto(key))
            {
                label9.Text = "Ya hay un objeto con ese nombre";
                return;
            }            

            Objeto bsObj = new Objeto(t);
            bsObj.setNombre(key);
            escenario.agregarObjeto(bsObj);

            cargarObjetosEscenario();
        }

        float delta = 0.1f;

        private void button4_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null)
                return;
            
            int escenario = comboBox1.SelectedIndex;
            string accion = comboBox3.SelectedItem.ToString();
            string objeto;
            
            if (comboBox2.SelectedItem == null)
                objeto = "";
            else
                objeto = comboBox2.SelectedItem.ToString();

            guion.accionar(game, escenario, objeto, accion, delta, 0, 0);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null)
                return;

            int escenario = comboBox1.SelectedIndex;
            string accion = comboBox3.SelectedItem.ToString();
            string objeto;

            if (comboBox2.SelectedItem == null)
                objeto = "";
            else
                objeto = comboBox2.SelectedItem.ToString();

            guion.accionar(game, escenario, objeto, accion, -delta, 0, 0);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null)
                return;

            int escenario = comboBox1.SelectedIndex;
            string accion = comboBox3.SelectedItem.ToString();
            string objeto;

            if (comboBox2.SelectedItem == null)
                objeto = "";
            else
                objeto = comboBox2.SelectedItem.ToString();

            guion.accionar(game, escenario, objeto, accion, 0, delta, 0);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null)
                return;

            int escenario = comboBox1.SelectedIndex;
            string accion = comboBox3.SelectedItem.ToString();
            string objeto;

            if (comboBox2.SelectedItem == null)
                objeto = "";
            else
                objeto = comboBox2.SelectedItem.ToString();

            guion.accionar(game, escenario, objeto, accion, 0, -delta, 0);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null)
                return;

            int escenario = comboBox1.SelectedIndex;
            string accion = comboBox3.SelectedItem.ToString();
            string objeto;

            if (comboBox2.SelectedItem == null)
                objeto = "";
            else
                objeto = comboBox2.SelectedItem.ToString();

            guion.accionar(game, escenario, objeto, accion, 0, 0, delta);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null)
                return;

            int escenario = comboBox1.SelectedIndex;
            string accion = comboBox3.SelectedItem.ToString();
            string objeto;

            if (comboBox2.SelectedItem == null)
                objeto = "";
            else
                objeto = comboBox2.SelectedItem.ToString();

            guion.accionar(game, escenario, objeto, accion, 0, 0, -delta);
        }

        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            cargarObjetosEscenario();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null)
                return;
            int index = comboBox1.SelectedIndex;
            game.eliminarEscenario(index);
            cargarEscenario();
            cargarObjetosEscenario();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null)
                return;
            if (comboBox2.SelectedItem == null)
                return;
            int index = comboBox1.SelectedIndex;
            string t = comboBox2.SelectedItem.ToString();
            Escenario escenario = game.getEscenario(index);
            escenario.eliminarObjeto(t);

            cargarObjetosEscenario();
            cargarObjetosEscenarioGuion();
        }

        private void comboBox5_SelectionChangeCommitted(object sender, EventArgs e)
        {
            cargarObjetosEscenarioGuion();
        }

        private void button16_Click(object sender, EventArgs e)
        {
            float x = float.Parse(label16.Text);
            x = x + delta;
            label16.Text = x.ToString();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            float x = float.Parse(label16.Text);
            x = x - delta;
            label16.Text = x.ToString();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            float y = float.Parse(label17.Text);
            y = y + delta;
            label17.Text = y.ToString();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            float y = float.Parse(label17.Text);
            y = y - delta;
            label17.Text = y.ToString();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            float z = float.Parse(label18.Text);
            z = z + delta;
            label18.Text = z.ToString();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            float z = float.Parse(label18.Text);
            z = z - delta;
            label18.Text = z.ToString();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            label24.Text = trackBar1.Value.ToString();
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            label25.Text = trackBar2.Value.ToString();
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            label26.Text = trackBar3.Value.ToString();
        }

        private void button18_Click(object sender, EventArgs e)
        {
            string t = textBox2.Text;
            //Se verifica si se ingreso un nombre
            if(t.CompareTo("") == 0)
            {
                label27.Text = "Ingrese nombre";
                return;
            }

            //Verificamos que el nombre no existe
            
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
            path = path.Remove(0, 6);
            path = path + "..\\..\\..\\guiones\\";
            
            string[] array1 = Directory.GetFiles(path);

            foreach (string name in array1)
            {
                if (t.CompareTo(cortarCadena(name)) == 0)
                {
                    label27.Text = "Nombre repetido";
                    return;
                }
            }
            
            //Creamos el archivo y lo guardamos
            path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
            path = path.Remove(0, 6);
            path = path + "..\\..\\..\\guiones";
            path = path + "\\" + t + ".txt";

            if (!File.Exists(path))
            {
                guion = new Guion();
                t = guion.toJSon();

                StreamWriter sw = File.CreateText(path);
                sw.WriteLine(t);
                sw.Close();
            }
            else
            {
                File.WriteAllText(path, "");
            }


            label27.Text = "";

            cargarGuion();

        }

        private void button17_Click(object sender, EventArgs e)
        {
            if (comboBox8.SelectedItem == null) {
                return;
            }
                
            Accion accion = new Accion();
            accion.accion = comboBox7.SelectedItem.ToString();
            accion.delta = trackBar2.Value;
            accion.escenario = comboBox5.SelectedIndex;
            accion.objeto = comboBox6.SelectedItem.ToString();
            accion.rep = trackBar3.Value;
            accion.tEjecucion = trackBar1.Value;
            accion.valorX = float.Parse(label16.Text);
            accion.valorY = float.Parse(label17.Text);
            accion.valorZ = float.Parse(label18.Text);


            string name = comboBox8.SelectedItem.ToString();

            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
            path = path.Remove(0, 6);
            path = path + "..\\..\\..\\guiones";
            path = path + "\\" + name + ".txt";


            //lee
            /*
            string str = File.ReadAllText(path);
            guion = new Guion(str);
            */
            //guion
            guion.addAccion(accion);
            string t = guion.toJSon();

            //Escribe
            if (!File.Exists(path))
            {
                StreamWriter sw = File.CreateText(path);
                sw.WriteLine(t);
            }
            else
            {
                File.WriteAllText(path, t);
            }
            textBox3.Text = t;
        }

        private void comboBox8_SelectionChangeCommitted(object sender, EventArgs e)
        {
            string name = comboBox8.SelectedItem.ToString();

            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
            path = path.Remove(0, 6);
            path = path + "..\\..\\..\\guiones";
            path = path + "\\" + name + ".txt";

            string text = File.ReadAllText(path);
            textBox3.Text = text;
            guion = new Guion(text);

        }


        int aux = 0;
        private void button19_Click(object sender, EventArgs e)
        {
            if (aux == 0)
            {
                aux = 1;
                button19.Text = "Strop";
                button19.BackColor = System.Drawing.Color.Red;
            }
            else
            {
                aux = 0;
                button19.Text = "Play";
                button19.BackColor = System.Drawing.Color.Green;
            }

            tiempo = 0;
        }
    }
}
