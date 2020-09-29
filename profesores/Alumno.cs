using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace profesores
{
    class Alumno
    {
        /*Los atributos fueron implementados en formato String para evitar los problemas dados por
        la diferencia de formatos en float y boolean entre sql y c sharp. Bit recibe 1, 0 no true/false
        float va con coma no con punto.*/
        public String claveA { get; set; }
        public String edad { get; set; }
        public String recursa { get; set; }
        public String nombre { get; set; }
        public String lista { get; set; }
        public String claveG { get; set; }

        //constructores con clave para busquedas, completos para altas y nulos para acceder funciones
        public Alumno()
        {

        }
        public Alumno(String clave)
        {
            this.claveA = clave;
        }
        public Alumno(String edad, String recursa, String nombre, String lista, String claveG)
        {
            this.claveA = claveG + lista;
            this.edad = edad;
            this.recursa = recursa;
            this.nombre = nombre;
            this.lista = lista;
            this.claveG = claveG;
        }

        //recibe la clase construida con todos sus atributos genera la conección y la da de alta en la base
        public int agregaAlumno(Alumno a)
        {
            try{
                int res = 0;
                SqlConnection con;
                con = Conexion.agregarConexion();
                SqlCommand cmd = new SqlCommand(String.Format("insert into alumno values('{0}', {1}, {2}, '{3}', {4}, '{5}')", a.claveA, a.edad, a.recursa, a.nombre, a.lista, a.claveG), con);
                res = cmd.ExecuteNonQuery();
                con.Close();
                return res;
            }catch (Exception ex)
            {
                MessageBox.Show("No se pudo dar de alta el Alumno" + ex);
                return 0;
            }

        }

        //Recibe la clase con su primary key para realizar la eliminación de la base, incluyendo las entradas de las tablas que dependan de su primary key
        public int eliminaAlumno(Alumno a, String grupo)
        {
            try
            {
                int res = 0;
                SqlConnection con;
                con = Conexion.agregarConexion();
                SqlCommand cmd = new SqlCommand(String.Format("delete seccionEx from seccionEx INNER JOIN evaluacion ON evaluacion.claveE = seccionEx.claveE INNER JOIN alumno ON alumno.claveA = evaluacion.claveA where alumno.claveA = '{0}'", a.claveA), con);
                res = cmd.ExecuteNonQuery();
                cmd = new SqlCommand(String.Format("delete examen from  examen INNER JOIN evaluacion ON evaluacion.claveE = examen.claveE INNER JOIN alumno ON alumno.claveA = evaluacion.claveA where alumno.claveA = '{0}'", a.claveA), con);
                res = cmd.ExecuteNonQuery();
                cmd = new SqlCommand(String.Format("delete actividad from actividad INNER JOIN evaluacion ON evaluacion.claveE = actividad.claveE INNER JOIN alumno ON alumno.claveA = evaluacion.claveA where alumno.claveA = '{0}'", a.claveA), con);
                res = cmd.ExecuteNonQuery();
                cmd = new SqlCommand(String.Format("delete proyecto from proyecto INNER JOIN evaluacion ON evaluacion.claveE = proyecto.claveE INNER JOIN alumno ON alumno.claveA = evaluacion.claveA where alumno.claveA = '{0}'", a.claveA), con);
                res = cmd.ExecuteNonQuery();
                cmd = new SqlCommand(String.Format("delete evaluacion from evaluacion INNER JOIN alumno ON alumno.claveA = evaluacion.claveA where alumno.claveA = '{0}'", a.claveA), con);
                res = cmd.ExecuteNonQuery();
                cmd = new SqlCommand(String.Format("delete from alumno where claveA = '{0}'", a.claveA), con);
                res = cmd.ExecuteNonQuery();
                con.Close();
                return res;
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo eliminar el alumno" + ex);
                return 0;
            }
        }


        //Método que una las entidades actividades y proyecto de la base de dtaos para generar un reporte
        public void buscarCalifS(Alumno a, DataGrid dg)
        {

            SqlConnection con = Conexion.agregarConexion();
            SqlDataReader dr;
            try
            {
                String query = String.Format("SELECT CASE  WHEN evaluacion.tipo = 3 THEN 'Actividad' ELSE 'Proyecto' END AS tipo, proyecto.nombre, calif from proyecto, evaluacion where proyecto.claveE = evaluacion.claveE and evaluacion.claveA = '{0}'union SELECT CASE WHEN evaluacion.tipo = 3 THEN 'Actividad' ELSE  'Proyecto' END AS tipo, actividad.nombre, calif from actividad, evaluacion where actividad.claveE = evaluacion.claveE and evaluacion.claveA = '{0}'", a.claveA);
                SqlCommand com = new SqlCommand(query, con);
                dr = com.ExecuteReader();
                dg.ItemsSource = dr;

                /*Estamos conscientes de que las conecciones abiertas generan vulnerabilidades
                 * sin embargo, al cerrar la conección ya no se puede llenar el grid, aún si se cierra
                 * después de ponerla como fuente, suponemos que es porque se pierde la conección,
                 * por ello tuvimos que dejarla abierta*/
                //con.Close();
                //dr.Close();
                

            }
            catch (Exception ex)
            {

            }
        }

        //Método para que el alumno, quien conoce su propia clave, busque las secciones del examen que le corresponden
        public SqlDataReader buscarEx(Alumno a)
        {
            SqlConnection con = Conexion.agregarConexion();
            SqlDataReader dr;
            try
            {
                String query = String.Format("select seccionEx.aciertos*seccionEx.valor/seccionEx.#pregun as puntaje from seccionEx, evaluacion where seccionEx.claveE = evaluacion.claveE and evaluacion.claveA = '{0}' union select calif from evaluacion where evaluacion.tipo = 1 and claveA = '{0}'", a.claveA);
                SqlCommand com = new SqlCommand(query, con);
                dr = com.ExecuteReader();
                return dr;
            }
            catch (Exception ex)
            {
                return null;
            }
        }





    }
}
