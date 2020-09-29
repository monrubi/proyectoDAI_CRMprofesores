using System;
using System.Collections.Generic;
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
    /// Lógica de interacción para nuevaEv.xaml
    /// </summary>
    public partial class nuevaEv : Window
    {
        List<String> claveA;
        String grupo;
        int numAlumnos;
        public nuevaEv(String grupo)
        {
            InitializeComponent();
            this.grupo = grupo;
            
        }

        //deja en la ventana los cambios que especifican en que sección de la base se trabaja
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lbGrupo.Content = lbGrupo.Content + grupo;
            rbTarea.IsChecked = true;
            claveA = Conexion.obtenerClaves(grupo);
            Grupo g = new Grupo(grupo);
            numAlumnos = g.contarAlumnos(g);
        }

        //cancela el alta regresando a la ventana previa
        private void bAtras_Click(object sender, RoutedEventArgs e)
        {
            mGrupo m = new profesores.mGrupo(grupo);
            m.Show();
            this.Close();
        }

        //los siguientes métodos habilitan y deshabilitan los textos dependiendo de su propio check box
        private void cb2_Checked(object sender, RoutedEventArgs e)
        {
            if (cb2.IsChecked == true)
            {
                tbP2.IsEnabled = true;
                tbG2.IsEnabled = true;
            }
            else
            {
                tbP2.IsEnabled = false;
                tbG2.IsEnabled = false;
            }


        }

        private void cb3_Checked(object sender, RoutedEventArgs e)
        {
            if (cb3.IsChecked == true)
            {
                tbP3.IsEnabled = true;
                tbG3.IsEnabled = true;
            }
            else
            {
                tbP3.IsEnabled = false;
                tbG3.IsEnabled = false;
            }

        }

        private void cb4_Checked(object sender, RoutedEventArgs e)
        {
            if (cb4.IsChecked == true)
            {
                tbP4.IsEnabled = true;
                tbG4.IsEnabled = true;
            }
            else
            {
                tbP4.IsEnabled = false;
                tbG4.IsEnabled = false;
            }
        }

        private void cb5_Checked(object sender, RoutedEventArgs e)
        {
            if (cb5.IsChecked == true)
            {
                tbP5.IsEnabled = true;
                tbG5.IsEnabled = true;
            }
            else
            {
                tbP5.IsEnabled = false;
                tbG5.IsEnabled = false;
            }

        }

        //da de alta el proyecto nuevo sin calificar tras confirmar que tenga nombre
        private void btProyecto_Click(object sender, RoutedEventArgs e)
        {
            if (tbProNombre.Text.Equals(""))
            {
                MessageBox.Show("Debe escoger un nombre para el proyecto");
            }
            else
            {
                Evaluacion ev = new Evaluacion();
                Proyecto p;
                String fecha = datePicker.SelectedDate.Value.ToString("dd-MM-yyyy");
                int clave = ev.siguienteClave(ev);
                int alta = 0;
                for (int i = 0; i < numAlumnos; i++)
                {
                    ev = new Evaluacion(clave.ToString(), "2", "0", claveA[i]);
                    alta = ev.agregaEvaluacion(ev);
                    if (alta != 0)
                    {
                        p = new Proyecto(tbProNombre.Text, fecha, "0",clave.ToString());
                        p.agregaProyecto(p);
                        clave++;
                    }
                }
                if (alta > 0)
                {
                    MessageBox.Show("Proyecto dado de alta");
                    mGrupo m = new mGrupo(grupo);
                    m.Show();
                    this.Close();
                }
            }

        }

        //da de alta una nueva actividad tras confirmar que tenga nombre
        private void btActividad_Click(object sender, RoutedEventArgs e)
        {
            if (tbActividad.Text.Equals(""))
            {
                MessageBox.Show("Debe escoger un nombre para el proyecto");
            }
            else
            {
                String tipo = "clase";
                if (rbTarea.IsChecked == true)
                {
                    tipo = "tarea";
                }
                Evaluacion ev = new Evaluacion();
                Actividad a;
                int clave = ev.siguienteClave(ev);
                int alta = 0;
                for (int i = 0; i < numAlumnos; i++)
                {
                    ev = new Evaluacion(clave.ToString(), "3", "0", claveA[i]);
                    alta = ev.agregaEvaluacion(ev);
                    if (alta != 0)
                    {
                        a = new Actividad(tipo, tbActividad.Text, clave.ToString());
                        a.agregaActividad(a);
                        clave++;
                    }
                }
                if (alta > 0)
                {
                    Calificar c = new Calificar(grupo, tbActividad.Text, 3);
                    c.Show();
                    this.Close();
                }
            }

        }


        //da de alta el examen con sus secciones para cada alumno e inicia el proceso de calificación
        private void btExamen_Click(object sender, RoutedEventArgs e)
        {
            List<int> claves = new List<int>();
            int sec= contarSecciones();
            Evaluacion ev = new Evaluacion();
            int clave = ev.siguienteClave(ev);
            SeccionEx se;
            int alta = 0;
            for (int i = 0; i < numAlumnos; i++)
            {
                ev = new Evaluacion(clave.ToString(), "1", "0", claveA[i]);
                alta = ev.agregaEvaluacion(ev);
                claves.Add(clave);
                if (alta != 0)
                {
                    alta = ev.nuevoExamen(ev);
                    if(alta != 0)
                    {
                        se = new SeccionEx();
                        int claveS = se.siguienteClave(se);
                        se = new SeccionEx(claveS.ToString(), "0", tbP1.Text, tbG1.Text, clave.ToString());
                        se.agregaSeccion(se);
                        claveS++;
                        if (sec > 1)
                        {
                            se = new SeccionEx(claveS.ToString(), "0", tbP2.Text, tbG2.Text, clave.ToString());
                            se.agregaSeccion(se);
                            claveS++;
                        }
                        if (sec > 2)
                        {
                            se = new SeccionEx(claveS.ToString(), "0", tbP3.Text, tbG3.Text, clave.ToString());
                            se.agregaSeccion(se);
                            claveS++;
                        }
                        if (sec > 3)
                        {
                            se = new SeccionEx(claveS.ToString(), "0", tbP4.Text, tbG4.Text, clave.ToString());
                            se.agregaSeccion(se);
                            claveS++;
                        }
                        if (sec > 4)
                        {
                            se = new SeccionEx(claveS.ToString(), "0", tbP5.Text, tbG5.Text, clave.ToString());
                            se.agregaSeccion(se);
                            claveS++;
                        }

                        
                    }
                }
                clave++;
            }

            if (alta > 0)
            {
                CalificarExamen c = new CalificarExamen(sec, grupo, claves);
                c.Show();
                this.Close();
            }

        }

        //registra el número de secciones que tendrá para manejar los ciclos de las altas
        private int contarSecciones()
        {
            int resp = 1;
            if (cb2.IsChecked == true)
                resp++;
            if (cb3.IsChecked == true)
                resp++;
            if (cb4.IsChecked == true)
                resp++;
            if (cb5.IsChecked == true)
                resp++;
            return resp;
        }
    }
}
