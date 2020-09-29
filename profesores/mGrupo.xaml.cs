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
    /// Lógica de interacción para mGrupo.xaml
    /// </summary>
    public partial class mGrupo : Window
    {
        List<String> claveA;
        List<int> calificado;
        String grupo;

        //el constructor deja en la ventana los cambios que especifican en que sección de la base se trabaja
        public mGrupo(String grupo)
        {
            InitializeComponent();
            lbGrupo.Content = lbGrupo.Content + grupo;
            this.grupo = grupo;
            claveA = Conexion.llenarComboAlumn(cbAlumno, grupo);
            calificado = Conexion.llenarComboProyecto(cbP, grupo);

        }

        //abre la ventana para dar de alta alumnos
        private void bAdminA_Click(object sender, RoutedEventArgs e)
        {
            altaAlumno a = new altaAlumno(grupo);
            a.Show();
            this.Close();
        }

        //regresa a la ventana central para acceder a un grupo
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            central c = new central();
            c.Show();
            this.Close();
        }

        //elimina el grupo de alumnos llamando la funcion de la clase grupo
        private void bEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("¿Desea eliminar todo el grupo de alumnos?", "Confirmación", MessageBoxButton.YesNo) == MessageBoxResult.Yes)

            {
                try
                {
                    int res;
                    Grupo g = new Grupo(grupo);
                    res = g.eliminaGrupo(g);
                    if (res > 0)
                    {
                        MessageBox.Show("grupo eliminado");
                        central c = new central();
                        c.Show();
                        this.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("El grupo no pudo ser eliminado" + ex);
                }


            }

        }


        //abre la ventana para gestionar rubros
        private void bRubro_Click(object sender, RoutedEventArgs e)
        {
            rubros r = new rubros(grupo);
            r.Show();
            this.Close();
        }


        //abre la ventana para consultar alumno que esta seleccionado en el combo box
        private void button4_Click(object sender, RoutedEventArgs e)
        {

            if (claveA.Count > 0)
            {
                consAlumno m = new consAlumno(claveA[cbAlumno.SelectedIndex], grupo);
                m.Show();
                this.Close();
            }
            else
                MessageBox.Show("No hay alumnos dados de alta");
        }

        //abre la ventana que busca las calificaciones por grupo no por alumno
        private void bCalif_Click(object sender, RoutedEventArgs e)
        {
            consultarCalif c = new consultarCalif(grupo);
            c.Show();
            this.Close();
        }

        //abre la ventana para dar de alta una nueva evaluaciòn
        private void button3_Click(object sender, RoutedEventArgs e)
        {
            nuevaEv n = new nuevaEv(grupo);
            n.Show();
            this.Close();
        }

        //Modifica la fecha de entrega del proyecto
        private void fechaProy_Click(object sender, RoutedEventArgs e)
        {
            Proyecto p = new Proyecto("0", cbP.SelectedItem.ToString());
            try
            {
                String fecha = datePicker.SelectedDate.Value.ToString("dd-MM-yyyy");
                p.cambiarFecha(p, fecha, grupo);
                MessageBox.Show("Fecha modificada");
                cbP.SelectedIndex = 0;
            } catch (Exception ex)
            {
                MessageBox.Show("Seleccione una fecha");
            }
        }

        //abre la ventana para calificar un proyecto en todos sus alumnos
        private void califProy_Click(object sender, RoutedEventArgs e)
        {
            Calificar c = new Calificar(grupo, cbP.SelectedItem.ToString(), 2);
            c.Show();
            this.Close();
        }


    }
}



