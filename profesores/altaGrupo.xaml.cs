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
    /// Interaction logic for altaGrupo.xaml
    /// </summary>
    /// 
    //el constructor deja en la ventana los cambios que especifican en que sección de la base se trabaja
    public partial class altaGrupo : Window
    {
        public altaGrupo()
        {
            InitializeComponent();
            txtLetra.CharacterCasing = CharacterCasing.Upper;
            txtLetra.MaxLength = 1;
            txtGrado.MaxLength = 1;
        }


        //regresa a la ventana central sin cambios
        private void bCancel_Click(object sender, RoutedEventArgs e)
        {
            central c = new central();
            c.Show();
            this.Close();
        }


        //crea un grupo y lo instancia con rubros de evaluación implementados en 0
        private void bCrear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int res;
                Grupo g = new Grupo(txtGrado.Text, txtLetra.Text);
                res = g.agregarGrupo(g);
                if (res > 0)
                {
                    SqlConnection con = Conexion.agregarConexion();
                    SqlCommand com = new SqlCommand("select max(claveR) as claveR from rubroEv", con);
                    SqlDataReader rd = com.ExecuteReader();
                    int clave;
                    if (rd.Read())
                    {
                        clave = Int32.Parse(rd["claveR"].ToString());
                    }
                    else
                        clave = 0;

                    Rubro r;
                    for (int i = 1; i < 4; i++)
                    {
                        clave = clave + 1;
                        r = new Rubro(clave.ToString(), i.ToString(), "0", g.claveG);
                        r.agregarRubro(r);
                    }
                    MessageBox.Show("Grupo dado de alta");
                    mGrupo m = new mGrupo(g.claveG);
                    m.Show();
                    this.Close();                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("el grupo no pudo darse de alta" + ex);
            }
            
        }
    }
}
