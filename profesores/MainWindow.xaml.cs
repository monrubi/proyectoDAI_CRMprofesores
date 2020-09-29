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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace profesores
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
     

        public MainWindow()
        {
            InitializeComponent();
            //txtPassword.Password = "1234";
            
        }


        //revisa que la contraseña sea correcta para dar acceso a la base de datos
        private void Button_Click(object sender, RoutedEventArgs e)
        {
               
                SqlDataReader dr;
                SqlConnection con;
                try
                {
                    con = Conexion.agregarConexion();
                    SqlCommand cmd = new SqlCommand("select contra from usuario where usuario.claveU = "+ 1, con);
                    dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        if (dr.GetString(0).Equals(txtPassword.Password.ToString()))
                        {
                            central c = new central();
                            c.Show();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Contraseña equivocada");
                        }

                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("error: " + ex);
                }
            }
    }
}
