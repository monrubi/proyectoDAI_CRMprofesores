using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace profesores
{
    /// <summary>
    /// Lógica de interacción para CalificarExamen.xaml
    /// </summary>
    public partial class CalificarExamen : Window
    {
        List<String> claveA;
        List<int> clavesE;
        int secciones;
        String grupo;
        int sec, cont;

        //el constructor deja en la ventana los cambios que especifican en que sección de la base se trabaja
        public CalificarExamen(int secciones, String grupo, List<int> clavesE)
        {
            InitializeComponent();
            this.grupo = grupo;
            this.sec = secciones;
            this.clavesE = clavesE;
            claveA = new List<string>();
            llenarClavesA();
            setNombre(claveA[0]);
            setLabels();
            cont = 0;

        }


        //obtiene las primary keys de los alumnos que estan siendo calificados
        private void llenarClavesA()
        {
            try
            {
                SqlConnection con = Conexion.agregarConexion();
                String query = String.Format("select claveA from alumno where claveG = '{0}'", grupo);
                
                SqlCommand com = new SqlCommand(query, con);
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    claveA.Add(rd["claveA"].ToString());
                }

            }
            catch (Exception ex)
            {

            }
        }

        //boton que avanza el ciclo de calificar, limpia los textos cuando aún hay alumnos y se cierra al terminar
        private void bCalif_Click(object sender, RoutedEventArgs e)
        {
            
            if (cont < claveA.Count())
            {
                float calif = 0;
                List<int> claveS = Conexion.getClaveS(clavesE[cont].ToString());
                SeccionEx ex = new SeccionEx(claveS[0].ToString());
                ex.insertaAciertos(ex, tb1.Text);
                calif = calif + (float.Parse(tb1.Text)*float.Parse(lbV1.Content.ToString())/float.Parse(lbP1.Content.ToString()));
                if (sec > 1)
                {
                    ex.claveS=claveS[1].ToString();
                    ex.insertaAciertos(ex, tb2.Text);
                    calif = calif + (float.Parse(tb2.Text) * float.Parse(lbV2.Content.ToString()) / float.Parse(lbP2.Content.ToString()));
                }
                if (sec > 2)
                {
                    ex.claveS = claveS[2].ToString();
                    ex.insertaAciertos(ex, tb3.Text);
                    calif = calif + (float.Parse(tb3.Text) * float.Parse(lbV3.Content.ToString()) / float.Parse(lbP3.Content.ToString()));
                }
                if (sec > 3)
                {
                    ex.claveS = claveS[3].ToString();
                    ex.insertaAciertos(ex, tb4.Text);
                    calif = calif + (float.Parse(tb4.Text) * float.Parse(lbV4.Content.ToString()) / float.Parse(lbP4.Content.ToString()));
                }
                if (sec > 4)
                {
                    ex.claveS = claveS[4].ToString();
                    ex.insertaAciertos(ex, tb5.Text);
                    calif = calif + (float.Parse(tb5.Text) * float.Parse(lbV5.Content.ToString()) / float.Parse(lbP5.Content.ToString()));
                }

                Evaluacion ev = new Evaluacion(clavesE[cont].ToString());
                ev.setExamen(ev, calif.ToString());
                cont++;
            }
            if (cont < claveA.Count())
            {
                limpiar();
                setNombre(claveA[cont]);
            }
            else
            {
                mGrupo m = new mGrupo(grupo);
                m.Show();
                this.Close();
            }

        }


        //pone en la etiqueta el nombre del alumno que se est{a calificando
        private void setNombre(String clave)
        {
            SqlConnection con = Conexion.agregarConexion();
            SqlCommand cmd = new SqlCommand("select nombre from alumno where alumno.claveA = '" + clave + "'", con);
            SqlDataReader rd = cmd.ExecuteReader();
            if (rd.Read())
            {
                lbAlumno.Content = "Alumno: " + rd["nombre"].ToString();
            }
        }


        //método que ayuda a limpiar los textos para el siguiente alumno a evaluar
        private void limpiar()
        {
            tb1.Text = "0";
            tb2.Text = "0";
            tb3.Text = "0";
            tb4.Text = "0";
            tb5.Text = "0";

        }


        //toma de la base de datos los parámetros de los rubros para que el profesor pueda ver qué califica
        private void setLabels()
        {
            try
            {
                SqlConnection con = Conexion.agregarConexion();
                String query = String.Format("select distinct seccionEx.valor, seccionEx.#pregun as preg, seccionEx.claveS from seccionEx, examen where seccionEx.claveE = {0} order by seccionEx.claveS", clavesE[0]);
                SqlCommand com = new SqlCommand(query, con);
                SqlDataReader rd = com.ExecuteReader();
                rd.Read();
                lbV1.Content = rd["valor"].ToString();
                lbP1.Content = rd["preg"].ToString();

                if (sec > 1)
                {
                    rd.Read();
                    lbV2.Content = rd["valor"].ToString();
                    lbP2.Content = rd["preg"].ToString();
                    tb2.IsEnabled = true;
                }
                if (sec > 2)
                {
                    rd.Read();
                    lbV3.Content = rd["valor"].ToString();
                    lbP3.Content = rd["preg"].ToString();
                    tb3.IsEnabled = true;
                }
                if (sec > 3)
                {
                    rd.Read();
                    lbV4.Content = rd["valor"].ToString();
                    lbP4.Content = rd["preg"].ToString();
                    tb4.IsEnabled = true;
                }
                if (sec > 4)
                {
                    rd.Read();
                    lbV5.Content = rd["valor"].ToString();
                    lbP5.Content = rd["preg"].ToString();
                    tb5.IsEnabled = true;
                }
            }catch(Exception ex)
            {

            }


        }
    }
}
