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
    /// Lógica de interacción para consAlumno.xaml
    /// </summary>
    public partial class consAlumno : Window
    {
        String alumno, grupo, claveA;
        int claveE;
        List<int> claves;

        //el constructor deja en la ventana los cambios que especifican en que sección de la base se trabaja
        public consAlumno(String clave, String grupo)
        {
            InitializeComponent();
            this.claveA = clave;
            this.grupo = grupo;
            this.alumno = obtenerAlumno();
            claves = Conexion.llenarComboCalif(cbCalif, claveA);
            setLabels();
            llenarDataGridAP(0);
            llenarDataGridEx();
            setCalifParcial();

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }


        //obtiene el nombre de un alumno dada su clave
        private String obtenerAlumno()
        {
            String resp = "";
            SqlConnection con = Conexion.agregarConexion();
            SqlCommand com = new SqlCommand(String.Format("select nombre from alumno where claveA = '{0}'", claveA), con);
            SqlDataReader rd = com.ExecuteReader();
            if (rd.Read())
            {
                resp = rd["nombre"].ToString();
            }
            return resp;
        }


        //si existen ya los tres rubros de evaluación arroja la calificación actual dadas las calificaciones existentes
        private void setCalifParcial()
        {
            String calif = "--";
            try
            {
                SqlConnection con = Conexion.agregarConexion();
                String query = String.Format("select (select AVG (evaluacion.calif)*(select porcentaje from rubroEv where claveG = '{0}' and tipo = 3) from evaluacion where evaluacion.claveA = '{1}' and tipo = 3) + ", grupo, claveA);
                query = query + String.Format("(select AVG (evaluacion.calif)*(select porcentaje from rubroEv where claveG = '{0}' and tipo = 2) as proyectos from evaluacion where evaluacion.claveA = '{1}' and tipo = 2) ", grupo, claveA);
                query = query + String.Format("+ (select AVG (evaluacion.calif)*(select porcentaje from rubroEv where claveG = '{0}' and tipo = 1) from evaluacion where evaluacion.claveA = '{1}' and tipo = 1) as calif", grupo, claveA);
                SqlCommand com = new SqlCommand(query, con);
                SqlDataReader rd = com.ExecuteReader();
                if (rd.Read())
                {
                    calif = rd["calif"].ToString();
                }
            }catch(Exception ex)
            {

            }
            lbParcial.Content = "Calificación Parcial:  " + calif;
        }


        //establece las etiquetas para la consulta con la informaci{on del alumno
        protected void setLabels()
        {
            try
            {
                lbAlumno.Content = lbAlumno.Content + alumno;
                lbGrupo.Content = lbGrupo.Content + grupo;
                SqlConnection con = Conexion.agregarConexion();
                SqlCommand com = new SqlCommand(String.Format("select edad, #lista, recursa from alumno where nombre = '{0}'", alumno), con);
                SqlDataReader rd = com.ExecuteReader();
                if (rd.Read())
                {
                    lbEdad.Content = lbEdad.Content + rd["edad"].ToString();
                    lbLista.Content = lbLista.Content + rd["#lista"].ToString();
                    if (rd["recursa"].ToString() == "False")
                    {
                        lbRecursa.Content = "";
                    }
                }
                con.Close();
            }catch(Exception ex)
            {

            }
        }

        
        //llena la grid de evaluaciones y proyectos
        protected void llenarDataGridAP(int index)
        {
            try
            {
                Alumno a = new Alumno(claveA);
                if (index == 0)
                {
                    a.buscarCalifS(a, dgAP);
                }
                else
                {
                    
                    int refe = getRefe(index);
                    int claveE = claves[index-1];
                    Evaluacion e = new Evaluacion(claveE.ToString());
                    dgAP.ItemsSource = e.buscarCalif(refe, e);
                }
            }catch(Exception ex)
            {

            }
        }


        //obtiene el tipo de evaluación seleccionado para saber que entidad es del isa
        protected int getRefe(int index)
        {
            claveE = claves[index - 1];
            SqlConnection con = Conexion.agregarConexion();
            SqlCommand com = new SqlCommand(String.Format("select tipo from evaluacion where evaluacion.claveE = {0}", claveE), con);
            SqlDataReader rd = com.ExecuteReader();
            int resp = 0;
            if (rd.Read())
                resp = rd.GetInt32(0);
            return resp;
        }


        //llena la grid de examenes
        protected void llenarDataGridEx()
        {
            Alumno a = new Alumno(claveA);
           dgEx.ItemsSource = a.buscarEx(a);
        }

        //elimina al alumno tras confirmar
        protected void btElimina_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("¿Desea eliminar al alumno?", "Confirmación", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {

                try
                {
                    SqlConnection con = Conexion.agregarConexion();
                    SqlCommand com = new SqlCommand(String.Format("select claveA from alumno where nombre = '{0}' and claveG ='{1}'", alumno, grupo), con);
                    SqlDataReader rd = com.ExecuteReader();
                    int res = 0;
                    if (rd.Read())
                    {
                        Alumno a = new Alumno(rd["claveA"].ToString());
                        res = a.eliminaAlumno(a, grupo);
                    }
                    if (res != 0)
                    {
                        MessageBox.Show("Alumno Eliminado Exitosamente");
                        mGrupo m = new mGrupo(grupo);
                        m.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("El alumno no pudo ser eliminado");
                    }
                    con.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("El alumno no pudo ser eliminado");
                }
            }
        }

        //cambia la calificación de una actividad o proyecto
        private void bCamb_Click(object sender, RoutedEventArgs e)
        {
            Evaluacion ev = new Evaluacion(claveE.ToString());
           ev.cambiarCalif(tbCalif.Text, ev);
            llenarDataGridAP(0);
            setCalifParcial();
        }

        //aumenta o resta puntos extras al examen
        private void bPuntos_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection con = Conexion.agregarConexion();
            try
            {
                float nuevo = float.Parse(tbPunt.Text);
                String query = String.Format("update evaluacion set evaluacion.calif = (select calif from evaluacion where evaluacion.tipo = 1 and claveA = '{0}')+{1} where evaluacion.tipo = 1 and claveA = '{0}'", claveA ,tbPunt.Text);
                SqlCommand com = new SqlCommand(query, con);
                int resp = com.ExecuteNonQuery();
                if (resp != 0)
                {
                    llenarDataGridEx();
                    tbPunt.Text = "0";
                }
            }catch(Exception ex)
            {

            }
            setCalifParcial();
        }

        //actualiza las grids tras cambiar una calificación

        private void cbCalif_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cbCalif.SelectedIndex;
            if (index == 0)
                bCamb.Visibility= Visibility.Hidden;
            else
                bCamb.Visibility = Visibility.Visible;
            llenarDataGridAP(index);
        }

        //regresa a la ventana previa

        private void btAtras_Click(object sender, RoutedEventArgs e)
        {
            mGrupo m = new mGrupo(grupo);
            m.Show();
            this.Close();
        }


    }


}
