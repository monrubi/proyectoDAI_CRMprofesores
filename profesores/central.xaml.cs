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
    /// Interaction logic for central.xaml
    /// </summary>
    public partial class central : Window
    {
        SqlCommand cmd;
        SqlDataReader rd;

        //el constructor deja en la ventana los cambios que especifican en que sección de la base se trabaja
        public central()
        {
            InitializeComponent();
            Conexion c = new Conexion();
            Conexion.llenarCombo(cb1);
            
        }

        //lanza una ventana para crear un grupo nuevo
        private void btNuevo_Click(object sender, RoutedEventArgs e)
        {
            altaGrupo a = new altaGrupo();
            a.Show();
            this.Close();
        }


        //toma el grupo seleccionado y lo abre para gestionarlo
        private void btAbrir_Click(object sender, RoutedEventArgs e)
        {
            mGrupo m = new mGrupo(cb1.SelectedItem.ToString());
            m.Show();
            this.Close();
        }

        //elimina el grupo y todas las tablas relacionadas tras pedir confirmación al usuario
        private void bElimina_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("¿Desea eliminar todo el grupo de alumnos?", "Confirmación", MessageBoxButton.YesNo) == MessageBoxResult.Yes)

            {
                try
                {
                    int res;
                    Grupo g = new Grupo(cb1.SelectedItem.ToString());
                    res = g.eliminaGrupo(g);
                    if (res > 0)
                    {
                        MessageBox.Show("grupo eliminado");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("El grupo no pudo ser eliminado" + ex);
                }
                cb1.Items.Clear();
                Conexion.llenarCombo(cb1);


            }
        }
    }
}
