using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace profesores
{
    class Rubro
    {
        /*Los atributos fueron implementados en formato String para evitar los problemas dados por
        la diferencia de formatos en float y boolean entre sql y c sharp. Bit recibe 1, 0 no true/false
        float va con coma no con punto.*/
        public String claveR { get; set; }
        public String tipo { get; set; }
        public String porcentaje { get; set; }
        public String claveG { get; set; }


        //constructores con clave para busquedas, completos para altas y nulos para acceder funciones
        public Rubro()
        {

        }
        public Rubro(String clave)
        {
            this.claveR = clave;
        }
        public Rubro(String clave, String tipo, String porcentaje, String claveG)
        {
            this.claveR = clave;
            this.tipo = tipo;
            this.porcentaje = porcentaje;
            this.claveG = claveG;
        }

        //recibe la clase construida con todos sus atributos genera la conección y la da de alta en la base
        public int agregarRubro(Rubro r)
        {
            try
            {
                int res = 0;
                SqlConnection con;
                con = Conexion.agregarConexion();
                SqlCommand cmd = new SqlCommand(String.Format("insert into rubroEv values({0}, {1}, {2}, '{3}')", r.claveR, r.tipo, r.porcentaje, r.claveG), con);
                res = cmd.ExecuteNonQuery();
                con.Close();
                return res;
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo dar de alta el rubro" + ex);
                return 0;
            }
        }


        //Recibe la clase con su primary key para realizar la eliminación de la base, incluyendo las entradas de las tablas que dependan de su primary key
        public int eliminarRubro(Rubro r)
        {
            try
            {
                int res = 0;
                SqlConnection con;
                con = Conexion.agregarConexion();
                SqlCommand cmd = new SqlCommand(String.Format("delete from rubroEv where claveR = '{0}'", r.claveR), con);
                res = cmd.ExecuteNonQuery();
                con.Close();
                return res;
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo eliminar el rubro" + ex);
                return 0;
            }
        }

        //modifica los porcentajes de vsalor ponderado de los 3 rubros
        public int modificar(String grupo, String porcentaje, int i)
        {
            try
            {
                SqlConnection con;
                con = Conexion.agregarConexion();
                String query = String.Format("UPDATE rubroEv SET porcentaje = CAST({0} AS float) / CAST(100 AS float) WHERE claveG = '{1}' and tipo = {2}", porcentaje, grupo, i.ToString());
                SqlCommand cmd = new SqlCommand(query, con);
                int res = cmd.ExecuteNonQuery();
                con.Close();
                return res;
            }
            catch(Exception ex)
            {
                MessageBox.Show("No se pudo eliminar el rubro" + ex);
                return 0;
            }

        }

    }
}
