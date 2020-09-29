using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace profesores
{
    class Actividad
    {
        //Los atributos fueron implementados en formato String para evitar los problemas dados por 
        //la diferencia de formatos en float y boolean entre sql y c sharp. Bit recibe 1, 0 no true/false
        //float va con coma no con punto.
        public String tipo { get; set; }
        public String nombre { get; set; }
        public String claveE { get; set; }

        //constructores con clave para busquedas y completos para altas
        public Actividad(String clave)
        {
            
            this.claveE = clave;
        }

        public Actividad (String tipo, String nombre, String claveE)
        {
            this.tipo = tipo;
            this.nombre = nombre;
            this.claveE = claveE;
        }

        //recibe la clase construida con todos sus atributos genera la conección y la da de alta en la base
        public int agregaActividad(Actividad a)
        {
            try
            {

                int res = 0;
                SqlConnection con;
                con = Conexion.agregarConexion();
                SqlCommand cmd = new SqlCommand(String.Format("insert into actividad values('{0}', '{1}', {2})", a.tipo, a.nombre, a.claveE), con);
                res = cmd.ExecuteNonQuery();
                con.Close();
                return res;
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo dar de alta la actividad" + ex);
                return 0;
            }
    
        }

        //Recibe la clase con su primary key para realizar la eliminación de la base
        public int eliminaActividad(Actividad a)
        {
            try
            {
                int res = 0;
                SqlConnection con;
                con = Conexion.agregarConexion();
                SqlCommand cmd = new SqlCommand(String.Format("delete from actividad where claveE = {0}", a.claveE), con);
                res = cmd.ExecuteNonQuery();
                con.Close();
                return res;
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo eliminar la actividad" + ex);
                return 0;
            }
        }

    }


}
