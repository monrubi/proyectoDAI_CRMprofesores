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
    /// Lógica de interacción para Calificar.xaml
    /// </summary>
    public partial class Calificar : Window
    {
        List<String> claveA, clavesE;
        String grupo, nombre;
        int tipo, cont;

        //el constructor deja en la ventana los cambios que especifican en que sección de la base se trabaja
        public Calificar(String grupo, String nombre, int tipo)
        {
            InitializeComponent();
            this.grupo = grupo;
            this.nombre = nombre;
            this.tipo = tipo;
            lbGrupo.Content = lbGrupo.Content + grupo;
            claveA = new List<string>();
            clavesE = new List<string>();
            llenarClavesA();
            llenarClavesE();
            setNombre(claveA[0]);
            cont = 0;
        }


        //obtiene las claves de los alumnos en el grupo desde la base para calificar las evaluaciones relacionadas a ellos
        private void llenarClavesA()
        {
            try
            {
                SqlConnection con = Conexion.agregarConexion();
                String query;
                if (tipo == 2)
                {
                    query = String.Format("select alumno.claveA from alumno, evaluacion, proyecto where alumno.claveA = evaluacion.claveA and evaluacion.claveE = proyecto.claveE and proyecto.nombre = '{0}' intersect select alumno.claveA from alumno, evaluacion, proyecto where alumno.claveA = evaluacion.claveA and evaluacion.claveE = proyecto.claveE and alumno.claveG = '{1}'", nombre, grupo);
                }
                else
                {
                    query = String.Format("select alumno.claveA from alumno, evaluacion, actividad where alumno.claveA = evaluacion.claveA and evaluacion.claveE = actividad.claveE and actividad.nombre = '{0}' intersect select alumno.claveA from alumno, evaluacion, actividad where alumno.claveA = evaluacion.claveA and evaluacion.claveE = actividad.claveE and alumno.claveG = '{1}'", nombre, grupo);
                }
                SqlCommand com = new SqlCommand(query, con);
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    claveA.Add(rd["claveA"].ToString());
                }

            }catch (Exception ex){

            }
        }

        //obtiene las claves de la evaluacion que ser{a calificada relacionadas a los alumnos del grupo en el que se está
        private void llenarClavesE()
        {
            try
            {
                SqlConnection con = Conexion.agregarConexion();
                String tabla;
                if (tipo == 2)
                    tabla = "proyecto";
                else
                    tabla = "actividad";
                SqlCommand com = new SqlCommand(String.Format("select evaluacion.claveE from alumno, evaluacion, {0} where alumno.claveA = evaluacion.claveA and evaluacion.claveE = {0}.claveE and {0}.nombre = '{1}' intersect select evaluacion.claveE from alumno, evaluacion, {0} where alumno.claveA = evaluacion.claveA and evaluacion.claveE = {0}.claveE and alumno.claveG = '{2}'", tabla, nombre, grupo ), con);
                SqlDataReader rd = com.ExecuteReader();
                while (rd.Read())
                {
                    clavesE.Add(rd["claveE"].ToString());
                }
                con.Close();

            }
            catch (Exception ex)
            {

            }
        }


        //ejecuta el cambio en la evaluacion que ya existe calificada en 0 por alumno tomando los parámetros del profesor
        //hace ciclos limpiando los textos para el siguiente alumno hasta que se han calificado todos
        private void bCalif_Click(object sender, RoutedEventArgs e)
        {
            
            if(cont < claveA.Count())
            {
                Evaluacion ev = new Evaluacion(clavesE[cont]);
                ev.cambiarCalif(tbCalif.Text, ev);
                if (tipo == 2)
                {
                    Proyecto p = new Proyecto(clavesE[cont]);
                    p.calificado(p);
                }
                
                cont++;
            }
            if (cont < claveA.Count())
            {
                tbCalif.Text = "0";
                setNombre(claveA[cont]);
            }
            else
            {
                mGrupo m = new mGrupo(grupo);
                m.Show();
                this.Close();
            }

        }


        //cambia el nombre en el label para que el profesor sepa  a quien califica
        private void setNombre(String clave)
        {
            SqlConnection con = Conexion.agregarConexion();
            SqlCommand cmd = new SqlCommand("select nombre from alumno where alumno.claveA = '"+ clave +"'", con);
            SqlDataReader rd = cmd.ExecuteReader();
            if (rd.Read())
            {
                lbAlumno.Content = "Alumno: " + rd["nombre"].ToString();
            }
        }

    }
}
