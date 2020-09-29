using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace profesores
{
    /*Los atributos fueron implementados en formato String para evitar los problemas dados por
        la diferencia de formatos en float y boolean entre sql y c sharp. Bit recibe 1, 0 no true/false
        float va con coma no con punto.*/
    class Evaluacion
    {
        public String claveE { get; set; }
        public String tipo { get; set; }
        public String calif { get; set; }
        public String claveA { get; set; }

        //constructores con clave para busquedas, completos para altas y nulos para acceder funciones
        public Evaluacion()
        {

        }
        public Evaluacion(String clave)
        {
            this.claveE = clave;
        }

        public Evaluacion(String clave, String tipo, String calif, String claveA)
        {
            this.claveE = clave;
            this.tipo = tipo;
            this.calif = calif;
            this.claveA = claveA;
        }

        public int cambiarCalif(String calif, Evaluacion e)
        {
            int resp = 0;
            SqlConnection con = Conexion.agregarConexion();
            try
            {
                SqlCommand cmd = new SqlCommand(String.Format("update evaluacion set calif = {0} where claveE = {1}", calif, e.claveE), con);
                resp = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

            }
            return resp;
        }

        //recibe la clase construida con todos sus atributos genera la conección y la da de alta en la base

        public int agregaEvaluacion(Evaluacion e)
        {
            int res = 0;
            try
            {
                SqlConnection con;
                con = Conexion.agregarConexion();
                SqlCommand cmd = new SqlCommand(String.Format("insert into evaluacion values({0}, {1}, {2}, '{3}')", e.claveE, e.tipo, e.calif, e.claveA), con);
                res = cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo dar de alta el Alumno" + ex);
            }
            return res;
        }


        //registra que tipo de evaluación es y regresa el reader con la información de esa evaluación
        public SqlDataReader buscarCalif(int tipo, Evaluacion e)
        {

            SqlConnection con = Conexion.agregarConexion();
            SqlDataReader dr;
            try
            {
                String query;
                if (tipo == 2)
                    query = String.Format("SELECT CASE  WHEN evaluacion.tipo = 3 THEN 'Actividad' ELSE 'Proyecto' END AS tipo, proyecto.nombre, calif from proyecto, evaluacion where proyecto.claveE = evaluacion.claveE and evaluacion.claveE = '{0}'", claveE);
                else
                    query = String.Format("SELECT CASE WHEN evaluacion.tipo = 3 THEN 'Actividad' ELSE  'Proyecto' END AS tipo, actividad.nombre, calif from actividad, evaluacion where actividad.claveE = evaluacion.claveE and evaluacion.claveE = {0}", claveE);
                SqlCommand com = new SqlCommand(query, con);
                dr = com.ExecuteReader();
                //con.Close();
                return dr;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //obtiene la clave más alta y regresa la siguiente para foliar
        public int siguienteClave(Evaluacion e)
        {
            int resp = 0;
            try
            {
                SqlConnection con;
                con = Conexion.agregarConexion();
                SqlCommand cmd = new SqlCommand("select max(claveE) as clave from evaluacion", con);
                SqlDataReader rd = cmd.ExecuteReader();
                if (rd.Read())
                {
                    resp = rd.GetInt32(0);
                    resp = resp + 1;
                }
            }
            catch(Exception ex)
            {

            }
            return resp;
        }


        //da de alta un nuevo examen
        public int nuevoExamen(Evaluacion ev)
        {
            int resp = 0;
            SqlConnection con = Conexion.agregarConexion();
            try
            {
                SqlCommand cmd = new SqlCommand(String.Format("insert into examen values({0})", ev.claveE), con);
                resp = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

            }
            return resp;
        }


        //registra la calificación del examen
        public int setExamen(Evaluacion ev, String calif)
        {
            int resp = 0;
            SqlConnection con = Conexion.agregarConexion();
            try
            {
                SqlCommand cmd = new SqlCommand(String.Format("update evaluacion set calif = {0} where claveE = {1}", calif, ev.claveE), con);
                resp = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

            }
            return resp;
        }


    }
}
