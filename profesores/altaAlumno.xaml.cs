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
    /// Lógica de interacción para altaAlumno.xaml
    /// </summary>
    public partial class altaAlumno : Window
    {
        String grupo;

        //el constructor deja en la ventana los cambios que especifican en que sección de la base se trabaja
        public altaAlumno(String grupo)
        {
            InitializeComponent();
            this.grupo = grupo;
            lbGrupo.Content = lbGrupo.Content + grupo;
            setLista();
        }

        //busca el número de clave más alta para incrementarla como folio
        private void setLista()
        {
            SqlCommand cmd;
            SqlDataReader rd;
            try
            {
                SqlConnection con = Conexion.agregarConexion();
                cmd = new SqlCommand(String.Format("select max(#lista) as #lista from alumno where alumno.claveG = '{0}'",grupo), con);
                rd = cmd.ExecuteReader();
                if (rd.Read())
                {
                    int aux = Int32.Parse(rd["#lista"].ToString());
                    aux = aux+1;
                    txtLista.Text = aux.ToString();
                }
                
            }catch (Exception ex)
            {
                txtLista.Text = "1";
            }
            

        }


        //cierra la ventana y regresa a la de modificaci{on de grupo
        private void bCancel_Click(object sender, RoutedEventArgs e)
        {
            mGrupo m = new mGrupo(grupo);
            m.Show();
            this.Close();
        }


        //Genera repetitivamente altas de alumno hasta que el usuario cierre el ciclo
        private void bAlta_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (tbEdad.Text != "" && tbNombre.Text != "")
                {
                    int res;
                    int rec = 0;
                    if (cbRec.IsChecked == true)
                    {
                        rec = 1;
                    }
                    Alumno a = new Alumno(tbEdad.Text, rec.ToString(), tbNombre.Text, txtLista.Text, grupo);
                    res = a.agregaAlumno(a);

                    if (MessageBox.Show("Alumno dado de alta \n¿Desea dar otro alumno de alta?", "Confirmación", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        clearText();
                    else
                    {
                        mGrupo m = new mGrupo(grupo);
                        m.Show();
                        this.Close();
                    }
                }
                else
                    MessageBox.Show("Favor llenar todos los campos");
            }
            catch (Exception ex)
            {
                MessageBox.Show("el alumno no pudo darse de alta" + ex);
            }
        }

        //limpia las cajas de textos para dejarlas listas para el siguiente alta
        private void clearText()
        {
            setLista();
            tbNombre.Text = "";
            tbEdad.Text = "";
            cbRec.IsChecked = false;
        }

    }
}
