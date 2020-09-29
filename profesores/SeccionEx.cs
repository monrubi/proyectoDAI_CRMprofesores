using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace profesores
{
    class SeccionEx
    {
        /*Los atributos fueron implementados en formato String para evitar los problemas dados por
        la diferencia de formatos en float y boolean entre sql y c sharp. Bit recibe 1, 0 no true/false
        float va con coma no con punto.*/
        public String claveS { get; set; }
        public String aciertos { get; set; }
        public String pregun { get; set; }
        public String valor { get; set; }
        public String claveE { get; set; }


        //constructores con clave para busquedas, completos para altas y nulos para acceder funciones
        public SeccionEx()
        {

        }
        public SeccionEx(String claveS)
        {
            this.claveS=claveS;
        }

        public SeccionEx(String claveS, String aciertos, String valor, String pregun, String claveE)
        {
            this.claveS = claveS;
            this.aciertos = aciertos;
            this.pregun = pregun;
            this.valor = valor;
            this.claveE = claveE;
        }


        //recibe la clase construida con todos sus atributos genera la conección y la da de alta en la base
        public int agregaSeccion(SeccionEx se)
        {
            try
            {
                int res = 0;
                SqlConnection con;
                con = Conexion.agregarConexion();
                SqlCommand cmd = new SqlCommand(String.Format("insert into seccionEx values({0}, {1}, {2}, {3}, {4})", se.claveS, se.aciertos, se.pregun, se.valor, se.claveE), con);
                res = cmd.ExecuteNonQuery();
                con.Close();
                return res;
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo dar de alta la seccion" + ex);
                return 0;
            }
        }

        //Recibe la clase con su primary key para realizar la eliminación de la base, incluyendo las entradas de las tablas que dependan de su primary key
        public int eliminaSeccion(SeccionEx se, String claveE)
        {
            try
            {
                int res = 0;
                SqlConnection con;
                con = Conexion.agregarConexion();
                SqlCommand cmd = new SqlCommand(String.Format("delete from seccionEx where claveS = {0}", se.claveS), con);
                res = cmd.ExecuteNonQuery();
                con.Close();
                return res;
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo eliminar la seccion" + ex);
                return 0;
            }
        }

        //obtiene la siguiente clave de la seccion para foliar
        public int siguienteClave(SeccionEx se)
        {
            int resp = 0;
            try
            {
                SqlConnection con;
                con = Conexion.agregarConexion();
                SqlCommand cmd = new SqlCommand("select max(claveS) from seccionEx", con);
                SqlDataReader rd = cmd.ExecuteReader();
                if (rd.Read())
                {
                    resp = rd.GetInt32(0);
                }
            }
            catch (Exception ex)
            {
                
            }
            resp ++;
            return resp;
        }

        //actualiza los aciertos que tuvo un alumno en una sección dada
        public int insertaAciertos(SeccionEx ex, String aciertos)
        {
            int resp = 0;
            SqlConnection con = Conexion.agregarConexion();
            try
            {
                String query = String.Format("update seccionEx set aciertos = {0} where claveS = {1}", aciertos, ex.claveS);
                SqlCommand com = new SqlCommand(query, con);
                resp = com.ExecuteNonQuery();
                
            }
            catch (Exception exep)
            {
                
            }
            return resp;
        }




    }

}
