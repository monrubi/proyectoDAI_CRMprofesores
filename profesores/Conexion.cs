using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace profesores
{
    class Conexion
    {
        
        public static SqlConnection agregarConexion()
        {
            SqlConnection cnn;
            try
            {

                cnn = new SqlConnection("Data Source=DANIELRUBI18D8;Initial Catalog=profesores;User ID=sa;Password=sqladmin");
                cnn.Open();
            }
            catch (Exception ex)
            {
                cnn = null;
                MessageBox.Show("no se pudo conectar" + ex);
            }
            return cnn;
        }


        public static void llenarCombo(ComboBox cb)
        {
            SqlCommand cmd;
            SqlDataReader rd;
            try
            {
                SqlConnection con;
                con = Conexion.agregarConexion();
                cmd = new SqlCommand("select claveG from grupo", con);
                rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    cb.Items.Add(rd["claveG"].ToString());
                }
                cb.SelectedIndex = 0;
                rd.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("no se pudo llenar el combo" + ex);
            }
        }

        public static List<int> llenarComboProyecto(ComboBox cb, String grupo)
        {
            List<int> aux = new List<int>();
            SqlCommand cmd;
            SqlDataReader rd;
            try
            {
                SqlConnection con;
                con = Conexion.agregarConexion();
                cmd = new SqlCommand("select distinct proyecto.nombre from proyecto, evaluacion, alumno where proyecto.claveE = evaluacion.claveE and evaluacion.claveA = alumno.claveA and alumno.claveG = '" + grupo + "' intersect select proyecto.nombre from proyecto where proyecto.calificado = 'false'", con);
                rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    cb.Items.Add(rd["nombre"].ToString());
                }
                cb.SelectedIndex = 0;
                rd.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("no se pudo llenar el combo" + ex);
            }
            return aux;
        }

        public static List<String> llenarComboAlumn(ComboBox cb, String grupo)
        {
            List<String> claves = new List<String>();
            SqlCommand cmd;
            SqlDataReader rd;
            try
            {
                SqlConnection con;
                con = Conexion.agregarConexion();
                cmd = new SqlCommand(String.Format("select nombre, claveA from alumno where alumno.claveG = '{0}'", grupo), con);
                rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    cb.Items.Add(rd["nombre"].ToString());
                    claves.Add(rd["claveA"].ToString());
                }
                cb.SelectedIndex = 0;
                rd.Close();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("no se pudo llenar el combo" + ex);
            }
            return claves;
        }

        //regresa una lista con las claves de los alumnos dado un grupo

        public static List<String> obtenerClaves(String grupo)
        {
            List<String> claves = new List<String>();
            SqlCommand cmd;
            SqlDataReader rd;
            try
            {
                SqlConnection con;
                con = Conexion.agregarConexion();
                cmd = new SqlCommand(String.Format("select claveA from alumno where alumno.claveG = '{0}'", grupo), con);
                rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    claves.Add(rd["claveA"].ToString());
                }
                rd.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show("no se pudo llenar el combo" + ex);
            }
            return claves;
        }



        public static List<int> llenarComboCalif(ComboBox cb, String alumno)
        {
            SqlCommand cmd;
            SqlDataReader rd;
            List<int> clave = new List<int>();
            try
            {
                cb.Items.Add("Todas");
                SqlConnection con;
                con = Conexion.agregarConexion();
                cmd = new SqlCommand(String.Format("select nombre, actividad.claveE from actividad,evaluacion where actividad.claveE = evaluacion.claveE and evaluacion.claveA = '{0}' union select nombre, proyecto.claveE from proyecto,evaluacion where proyecto.claveE = evaluacion.claveE and evaluacion.claveA = '{0}'", alumno), con);
                rd = cmd.ExecuteReader();

                while (rd.Read())
                {
                    cb.Items.Add(rd["nombre"].ToString());
                    clave.Add(rd.GetInt32(1));

                }
                cb.SelectedIndex = 0;
                con.Close();
               rd.Close();
               return clave;
            }
            catch (Exception ex)
            {
                MessageBox.Show("no se pudo llenar el combo" + ex);
                return clave;
            }
        }

        public static List<int> llenarComboEva(ComboBox cb, String grupo)
        {
            List<int> tipos = new List<int>();
            
            SqlConnection con = Conexion.agregarConexion();
            try
            {
                
                SqlCommand com = new SqlCommand("select * from evaluacion, alumno where evaluacion.claveA = alumno.claveA and alumno.claveG = '"+grupo+"' and evaluacion.tipo = 1", con);
                SqlDataReader rd = com.ExecuteReader();
                if (rd.Read())
                {
                    cb.Items.Add("Examen");
                    tipos.Add(1);
                }

                con.Close();
                SqlConnection con1 = Conexion.agregarConexion();
                SqlCommand com1 = new SqlCommand("select distinct proyecto.nombre from alumno, evaluacion, proyecto where alumno.claveA = evaluacion.claveA and evaluacion.claveE = proyecto.claveE and alumno.claveG = '" + grupo + "'", con1);
                SqlDataReader rd1 = com1.ExecuteReader();
                while (rd1.Read())
                {
                    cb.Items.Add(rd1.GetString(0));
                    tipos.Add(2);
                }
                con1.Close();
                SqlConnection con2 = Conexion.agregarConexion();
                SqlCommand com2 = new SqlCommand("select distinct actividad.nombre from alumno, evaluacion, actividad where alumno.claveA = evaluacion.claveA and evaluacion.claveE = actividad.claveE and alumno.claveG = '" + grupo + "'", con2);
                SqlDataReader rd2 = com2.ExecuteReader();
                while (rd2.Read())
                {
                    cb.Items.Add(rd2["nombre"].ToString());
                    tipos.Add(3);
                }

                con2.Close();
                

            }
            catch (Exception ex)
            {

            }

            return tipos;
        }

        //método para llenar el grid de evaluación
        public static SqlDataReader llenarEva(DataGrid dg, String query)
        {
            SqlDataReader rd;
            try
            {
                SqlConnection con = Conexion.agregarConexion();
                SqlCommand com = new SqlCommand(query, con);
                rd = com.ExecuteReader();


            }
            catch (Exception ex)
            {
                rd = null;
            }
            return rd;
        }


        //obtiene las claves de las secciones dado un examen
        public static List<int> getClaveS(String claveE)
        {
            SqlCommand cmd;
            SqlDataReader rd;
            List<int> claveS = new List<int>();
            try
            {
                SqlConnection con;
                con = Conexion.agregarConexion();
                cmd = new SqlCommand(String.Format("select claveS from seccionEx where claveE = {0}", claveE), con);
                rd = cmd.ExecuteReader();

                while (rd.Read())
                {
                    claveS.Add(rd.GetInt32(0));
                }
                con.Close();
                rd.Close();

            }
            catch (Exception ex)
            {

            }
            return claveS;
        }
    }
}
