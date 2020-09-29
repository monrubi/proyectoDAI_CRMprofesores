using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace profesores
{
    class Grupo
    {
        /*Los atributos fueron implementados en formato String para evitar los problemas dados por
        la diferencia de formatos en float y boolean entre sql y c sharp. Bit recibe 1, 0 no true/false
        float va con coma no con punto.*/
        public String claveG { get; set; }
        public String grado { get; set; }
        public String letra { get; set; }

        //constructores con clave para busquedas, completos para altas y nulos para acceder funciones
        public Grupo(String claveG)
        {
            this.claveG = claveG;
        }

        public Grupo(String grado, String letra)
        {
            this.claveG = grado + letra;
            this.grado = grado;
            this.letra = letra;
        }

        //recibe la clase construida con todos sus atributos genera la conección y la da de alta en la base
        public int agregarGrupo(Grupo g)
        {
            try
            {
                int res = 0;
                SqlConnection con;
                con = Conexion.agregarConexion();
                SqlCommand cmd = new SqlCommand(String.Format("insert into grupo values('{0}', '{1}', '{2}')", g.claveG, g.grado, g.letra), con);
                res = cmd.ExecuteNonQuery();
                con.Close();
                return res;
            }catch(Exception ex)
            {
                MessageBox.Show("No se pudo dar de alta el grupo" + ex);
                return 0;
            }
        }

        //Recibe la clase con su primary key para realizar la eliminación de la base, incluyendo las entradas de las tablas que dependan de su primary key
        public int eliminaGrupo(Grupo g)
        {
            try
            {
                int res = 0;
                SqlConnection con;
                con = Conexion.agregarConexion();
                SqlCommand cmd = new SqlCommand(String.Format("delete seccionEx from seccionEx INNER JOIN evaluacion ON evaluacion.claveE = seccionEx.claveE INNER JOIN alumno ON alumno.claveA = evaluacion.claveA where alumno.claveG = '{0}'", g.claveG), con);
                res = cmd.ExecuteNonQuery();
                cmd = new SqlCommand(String.Format("delete examen from  examen INNER JOIN evaluacion ON evaluacion.claveE = examen.claveE INNER JOIN alumno ON alumno.claveA = evaluacion.claveA where alumno.claveG = '{0}'", g.claveG), con);
                res = cmd.ExecuteNonQuery();
                cmd = new SqlCommand(String.Format("delete actividad from actividad INNER JOIN evaluacion ON evaluacion.claveE = actividad.claveE INNER JOIN alumno ON alumno.claveA = evaluacion.claveA where alumno.claveG = '{0}'", g.claveG), con);
                res = cmd.ExecuteNonQuery();
                cmd = new SqlCommand(String.Format("delete proyecto from proyecto INNER JOIN evaluacion ON evaluacion.claveE = proyecto.claveE INNER JOIN alumno ON alumno.claveA = evaluacion.claveA where alumno.claveG = '{0}'", g.claveG), con);
                res = cmd.ExecuteNonQuery();
                cmd = new SqlCommand(String.Format("delete evaluacion from evaluacion INNER JOIN alumno ON alumno.claveA = evaluacion.claveA where alumno.claveG = '{0}'", g.claveG), con);
                res = cmd.ExecuteNonQuery();
                cmd = new SqlCommand(String.Format("delete from alumno where claveG = '{0}'", g.claveG), con);
                res = cmd.ExecuteNonQuery();
                cmd = new SqlCommand(String.Format("delete from rubroEv where claveG = '{0}'", g.claveG), con);
                res = cmd.ExecuteNonQuery();
                cmd = new SqlCommand(String.Format("delete from grupo where claveG = '{0}'", g.claveG), con);
                res = cmd.ExecuteNonQuery();
                con.Close();
                return res;
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo eliminar el grupo" + ex);
                return 0;
            }
        }

        //obtiene el número de alumnos en un grupo
        public int contarAlumnos(Grupo g)
        {
            int resp = 0;
            try
            {
                SqlConnection con;
                con = Conexion.agregarConexion();
                SqlCommand cmd = new SqlCommand(String.Format("select count(*) from alumno where alumno.claveG = '{0}'", g.claveG), con);
                SqlDataReader rd = cmd.ExecuteReader();
                if (rd.Read())
                {
                    resp = rd.GetInt32(0);
                }
            }
            catch(Exception ex)
            {

            }
            return resp;
        }


    }
}
