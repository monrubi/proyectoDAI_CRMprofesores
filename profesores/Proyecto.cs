using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace profesores
{
    class Proyecto
    {

        /*Los atributos fueron implementados en formato String para evitar los problemas dados por
        la diferencia de formatos en float y boolean entre sql y c sharp. Bit recibe 1, 0 no true/false
        float va con coma no con punto.*/
        public String nombre { get; set; }
        public String fecha { get; set; }

        public String calif { get; set; }
        public String claveE { get; set; }


        //constructores con clave para busquedas, completos para altas y nulos para acceder funciones
        public Proyecto(String clave)
        {
            this.claveE = clave;
        }

        public Proyecto(String clave, String nombre)
        {
            this.claveE = clave;
            this.nombre = nombre;
        }
        public Proyecto(String nombre, String fecha, String calificado, String claveE)
        {
            this.nombre = nombre;
            this.fecha = fecha;
            this.calif = calificado;
            this.claveE = claveE;
        }

        //recibe la clase construida con todos sus atributos genera la conección y la da de alta en la base
        public int agregaProyecto(Proyecto p)
        {
            int res = 0;
            try
            {
                SqlConnection con;
                con = Conexion.agregarConexion();
                SqlCommand cmd = new SqlCommand(String.Format("insert into proyecto values('{0}', '{1}', 0, {2}) ", p.nombre, p.fecha, p.claveE), con);
                res = cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo dar de alta el proyecto" + ex);
            }
            return res;
        }


        //cambia la fecha de entrega de un proyecto dado el grupo y el nombre del proyecto
        public int cambiarFecha(Proyecto p, String fecha, String grupo)
        {
            int resp = 0;
            try
            {
                SqlConnection con = Conexion.agregarConexion();
                SqlCommand com = new SqlCommand(String.Format("UPDATE proyecto SET fechaEnt = '{0}' FROM proyecto  INNER JOIN  evaluacion ON proyecto.claveE = evaluacion.claveE inner join alumno on evaluacion.claveA = alumno.claveA where alumno.claveG = '{1}' and proyecto.nombre = '{2}'", fecha, grupo, p.nombre), con);
                resp = com.ExecuteNonQuery();
                
            }catch(Exception ex)
            {
                MessageBox.Show("No se pudo cambiar la fecha");
            }
            return resp;
        }

        //registra que el proyecto ha sido calificado inhabilitando cambiar su fecha
        public void calificado(Proyecto p)
        {
            SqlConnection con = Conexion.agregarConexion();
            try
            {
                SqlCommand cmd = new SqlCommand("update proyecto set calificado = 1 where claveE = "+ p.claveE, con);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

            }
        }

    }
}
