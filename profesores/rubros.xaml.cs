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
    /// Lógica de interacción para rubros.xaml
    /// </summary>
    public partial class rubros : Window
    {
        String grupo;
        Boolean ini = false;
        //el constructor deja en la ventana los cambios que especifican en que sección de la base se trabaja
        public rubros(String grupo)
        {
            InitializeComponent();
            this.grupo = grupo;
            lbGrupo.Content = lbGrupo.Content + grupo;
            setText();
            ini = true;
            setTotal();
        }

        //pide a la base de datos el valor de los rubros y los presenta como porcentajes para el usuario
        private void setText()
        {
            try
            {
                SqlConnection con = Conexion.agregarConexion();
                SqlCommand com = new SqlCommand(String.Format("select porcentaje from rubroEv where claveG = '{0}' order by tipo", grupo), con);
                SqlDataReader rd = com.ExecuteReader();
                float aux;
                if (rd.Read())
                {
                    aux = float.Parse(rd["porcentaje"].ToString())*100;
                    txtExam.Text = aux.ToString();
                }
                if (rd.Read())
                {
                    aux = float.Parse(rd["porcentaje"].ToString()) * 100;
                    txtProy.Text = aux.ToString();
                }
                if (rd.Read())
                {
                    aux = float.Parse(rd["porcentaje"].ToString()) * 100;
                    txtTarea.Text = aux.ToString();
                }
                
                con.Close();
                rd.Close();

            }
            catch(Exception ex)
            {
                txtExam.Text = "0";
                txtTarea.Text = "0";
                txtProy.Text = "0";
            }
        }

        //regresa a la ventana anterior sin cambios
        private void bCancel_Click(object sender, RoutedEventArgs e)
        {
            mGrupo m = new mGrupo(grupo);
            m.Show();
            this.Close();
        }

        //los siguientes métodos funcionan para actualizar constantemente la suma de los porcentajes
        private void txtExam_TextChanged(object sender, EventArgs e)
        {
            setTotal();
        }

        private void txtTarea_TextChanged(object sender, TextChangedEventArgs e)
        {
            setTotal();
        }

        private void txtProy_TextChanged(object sender, TextChangedEventArgs e)
        {
            setTotal();
        }

        //pone el total en la etiqueta
        private void setTotal()
        {
            if (ini)
            {
                try
                {
                    int aux = int.Parse(txtExam.Text) + int.Parse(txtProy.Text) + int.Parse(txtTarea.Text);
                    lbTotal.Content = aux.ToString();
                }catch(Exception ex)
                {
                    lbTotal.Content = "";
                }
            }
        }

        //guarda los rubros modificados
        private void bGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (lbTotal.Content.Equals("100"))
            {
                try
                {
                    Rubro r = new Rubro();
                    int i = 1;
                    r.modificar(grupo, txtExam.Text, i);
                    i++;
                    r.modificar(grupo, txtProy.Text, i);
                    i++;
                    r.modificar(grupo, txtTarea.Text, i);
                    MessageBox.Show("Rubros guardados correctamente");
                    mGrupo m = new mGrupo(grupo);
                    m.Show();
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("No se pudieron modificar los rubros" + ex);
                }
            }
            else
                MessageBox.Show("Los rubros no suman 100%");

        }
    }
}
