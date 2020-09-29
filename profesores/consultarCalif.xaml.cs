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
    /// Lógica de interacción para consultarCalif.xaml
    /// </summary>
    public partial class consultarCalif : Window
    {
        List<String> idA;
        List<int> idE;
        String grupo;


        public consultarCalif(String grupo)
        {
            InitializeComponent();
            this.grupo = grupo;

        }

        //deja en la ventana los cambios que especifican en que sección de la base se trabaja
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lbGrupo.Content = lbGrupo.Content + grupo;
            idE = Conexion.llenarComboEva(cbE, grupo);
            idA = Conexion.llenarComboAlumn(cbA, grupo);
            cbE.SelectedIndex = 0;
        }


        //revisa el tipo de evaluacion que es y llena el grid
        private void llenarGrid(int index)
        {
            int tipo = idE[index];
            String query;
            switch (tipo)
            {
                case 1:
                    query = "select alumno.nombre, evaluacion.calif from evaluacion, alumno where evaluacion.claveA = alumno.claveA and alumno.claveG = '"+grupo+"' and evaluacion.tipo = 1";
                    break;
                case 2:
                    query = "select distinct alumno.nombre, evaluacion.calif from alumno, evaluacion, proyecto where proyecto.claveE = evaluacion.claveE and evaluacion.claveA = alumno.claveA and alumno.claveG = '"+grupo+"' and proyecto.nombre = '"+cbE.SelectedItem+"'";
                    break;
                case 3:
                    query = "select distinct alumno.nombre, evaluacion.calif from alumno, evaluacion, actividad where actividad.claveE = evaluacion.claveE and evaluacion.claveA = alumno.claveA and alumno.claveG = '"+ grupo+"' and actividad.nombre = '"+cbE.SelectedItem+"'";
                    break;
                default:
                    query = "";
                    break;
            }

            dgReporte.ItemsSource = Conexion.llenarEva(dgReporte, query);

        }

        //vuelve a la ventana que gestiona Grupos
        private void bAtras_Click(object sender, RoutedEventArgs e)
        {
            mGrupo m = new profesores.mGrupo(grupo);
            m.Show();
            this.Close();
        }


        //envìa a la ventana para consultar alumno
        private void bAlumno_Click(object sender, RoutedEventArgs e)
        {
            consAlumno c = new consAlumno(idA[cbA.SelectedIndex], grupo);
            c.Show();
            this.Close();
        }

        //registra si se cambio en el combo box
        private void cbE_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            llenarGrid(cbE.SelectedIndex);
        }
    }
}
