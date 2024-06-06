using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using U4_Topicos_Actividad_1.Models;
using U4_Topicos_Actividad_1.Repositories;

namespace U4_Topicos_Actividad_1.ViewModels
{
    public enum Vistas { Horario, AgregarClase, AgregarActividad, EditarClase, EditarActividad, EliminarClase, EliminarActividad, DetallesClase, DetallesActividad }
    public enum Dias { Lunes, Martes, Miercoles, Jueves, Viernes, Sabado, Domingo}
    public enum Tipos { Clase, Actividad}
    public class HorarioViewModel : INotifyPropertyChanged
    {
        public Vistas Vista { get; set; }
        public Dias Dia { get; set; }
        public ClaseModel Clase { get; set; } = null!;
        public ActividadModel Actividad { get; set; } = null!;
        ClasesRepository clasesrepos { get; set; } = new();
        ActividadesRepository actrepos { get; set; } = new();
        public ObservableCollection<VistaModel> Horario { get; set; } = new();
        public string Error { get; set; } = "";
        public IEnumerable<int> Horas
        {
            get
            {
                return Enumerable.Range(1, 24).ToList();
            }
        }

        public Command MostrarHorarioCommand { get; set; }
        public Command VerAgregarClaseCommand { get; set; }
        public Command CancelarCommand { get; set; }
        public Command AgregarClaseCommand { get; set; }
        public Command VerDetallesCommand { get; set; }
        public Command EditarClaseCommand { get; set; }
        public Command VerEliminarCommand { get; set; }
        public Command EliminarCommand { get; set; }
        public Command VerAgregarActCommand { get; set; }
        public Command AgregarActCommand { get; set; }
        public Command VerEditarCommand { get; set; }
        public Command EditarActCommand { get; set; }

        public HorarioViewModel()
        {
            MostrarHorario(Dia);
            MostrarHorarioCommand = new Command<Dias>(MostrarHorario);
            VerAgregarClaseCommand = new Command(VerAgregarClase);
            CancelarCommand = new Command(Cancelar);
            AgregarClaseCommand = new Command(AgregarClase);
            VerDetallesCommand = new Command<VistaModel>(VerDetalles);
            EditarClaseCommand = new Command(EditarClase);
            VerEliminarCommand = new Command<VistaModel>(VerEliminar);
            EliminarCommand = new Command(Eliminar);
            VerAgregarActCommand = new Command(VerAgregarAct);
            AgregarActCommand = new Command(AgregarAct);
            VerEditarCommand = new Command<VistaModel>(VerEditar);
            EditarActCommand = new Command(EditarAct);

        }

        private void EditarAct()
        {
            if (Actividad != null)
            {
                Error = "";
                if (string.IsNullOrWhiteSpace(Actividad.Nombre))
                {
                    Error += "El nombre de la materia no puede estar vacío.\n";
                }
                if (Actividad.HoraInicio < 1 || Actividad.HoraInicio > 24 || Actividad.HoraFinal < 1 || Actividad.HoraFinal > 24 || Actividad.HoraInicio > Actividad.HoraFinal || Actividad.HoraInicio == Actividad.HoraFinal)
                {
                    Error += "Ingrese una hora válida.\n";
                }

                bool clases = clasesrepos.GetByDay(Dia).ToList().Exists(x => Actividad.HoraInicio >= x.HoraInicio && Actividad.HoraInicio < x.HoraFinal || Actividad.HoraFinal > x.HoraInicio && Actividad.HoraFinal <= x.HoraFinal || Actividad.HoraInicio < x.HoraInicio && Actividad.HoraFinal > x.HoraFinal);
                bool actividades = actrepos.GetByDay(Dia).ToList().Exists(x => (Actividad.HoraInicio >= x.HoraInicio && Actividad.HoraInicio < x.HoraFinal || Actividad.HoraFinal > x.HoraInicio && Actividad.HoraFinal <= x.HoraFinal || Actividad.HoraInicio < x.HoraInicio && Actividad.HoraFinal > x.HoraFinal) && x.Id != Actividad.Id);

                if (clases || actividades)
                {
                    Error += "Ya existe una clase o actividad en ese horario.\n";
                }
                if (string.IsNullOrWhiteSpace(Actividad.Descripcion))
                {
                    Error += "El nombre del maestro no puede estar vacío.\n";
                }

                Actualizar(nameof(Error));

                if (Error == "")
                {
                    actrepos.Update(Actividad);
                    MostrarHorario(Dia);
                    Navegar("MainPage");
                    Vista = Vistas.Horario;
                    Actualizar(nameof(Vistas));
                }
            }
        }

        private void VerEditar(VistaModel vista)
        {
            if (vista.Tipo == Tipos.Clase)
            {
                Error = "";
                ClaseModel clase = clasesrepos.GetById(vista.Id);
                Clase = new()
                {
                    Nombre = clase.Nombre,
                    NombreMaestro = clase.NombreMaestro,
                    Aula = clase.Aula,
                    Dia = clase.Dia,
                    HoraFinal = clase.HoraFinal,
                    HoraInicio = clase.HoraInicio,
                    Id = clase.Id,
                };
                Vista = Vistas.EditarClase;
                Actualizar(nameof(Error));
                Actualizar(nameof(Clase));
                Actualizar(nameof(Vista));
                Navegar("EditarClase");
            }
            else
            {
                Error = "";
                ActividadModel actividad = actrepos.GetById(vista.Id);
                Actividad = new()
                {
                    Nombre = actividad.Nombre,
                    Dia = actividad.Dia,
                    HoraFinal = actividad.HoraFinal,
                    HoraInicio = actividad.HoraInicio,
                    Descripcion = actividad.Descripcion,
                    Id = actividad.Id,
                };
                Vista = Vistas.EditarActividad;
                Actualizar(nameof(Error));
                Actualizar(nameof(Actividad));
                Actualizar(nameof(Vista));
                Navegar("EditarActividad");
            }
        }

        private void AgregarAct()
        {
            if (Actividad != null)
            {
                Error = "";
                if (string.IsNullOrWhiteSpace(Actividad.Nombre))
                {
                    Error += "El nombre de la materia no puede estar vacío.\n";
                }
                if (Actividad.HoraInicio < 1 || Actividad.HoraInicio > 24 || Actividad.HoraFinal < 1 || Actividad.HoraFinal > 24 || Actividad.HoraInicio > Actividad.HoraFinal || Actividad.HoraInicio == Actividad.HoraFinal)
                {
                    Error += "Ingrese una hora válida.\n";
                }

                bool clases = clasesrepos.GetByDay(Dia).ToList().Exists(x => Actividad.HoraInicio >= x.HoraInicio && Actividad.HoraInicio < x.HoraFinal || Actividad.HoraFinal > x.HoraInicio && Actividad.HoraFinal <= x.HoraFinal || Actividad.HoraInicio < x.HoraInicio && Actividad.HoraFinal > x.HoraFinal);
                bool actividades = actrepos.GetByDay(Dia).ToList().Exists(x => Actividad.HoraInicio >= x.HoraInicio && Actividad.HoraInicio < x.HoraFinal || Actividad.HoraFinal > x.HoraInicio && Actividad.HoraFinal <= x.HoraFinal || Actividad.HoraInicio < x.HoraInicio && Actividad.HoraFinal > x.HoraFinal);

                if (clases||actividades)
                {
                    Error += "Ya existe una clase o actividad en ese horario.\n";
                }
                if (string.IsNullOrWhiteSpace(Actividad.Descripcion))
                {
                    Error += "El nombre del maestro no puede estar vacío.\n";
                }

                Actualizar(nameof(Error));

                if (Error == "")
                {
                    actrepos.Insert(Actividad);
                    MostrarHorario(Dia);
                    Navegar("MainPage");
                    Vista = Vistas.Horario;
                    Actualizar(nameof(Vistas));
                }
            }
        }

        private void VerAgregarAct()
        {
            Vista = Vistas.AgregarActividad;
            Error = "";
            Actividad = new()
            {
                Dia = Dia,
            };
            Actualizar(nameof(Error));
            Actualizar(nameof(Actividad));
            Actualizar(nameof(Vista));
            Navegar("AgregarActividad");
        }

        private void Eliminar()
        {
            if (Vista==Vistas.EliminarClase)
            {
                clasesrepos.Delete(Clase);
            }
            if (Vista == Vistas.EliminarActividad)
            {
                actrepos.Delete(Actividad);
            }
            Vista = Vistas.Horario;
            MostrarHorario(Dia);
            Actualizar(nameof(Vista));
        }

        private void VerEliminar(VistaModel vista)
        {
            if (vista.Tipo == Tipos.Clase)
            {
                ClaseModel clase = clasesrepos.GetById(vista.Id);
                Clase = clase;
                Vista = Vistas.EliminarClase;
            }
            else
            {
                ActividadModel actividad = actrepos.GetById(vista.Id);
                Actividad = actividad;
                Vista = Vistas.EliminarActividad;
            }
            Actualizar(nameof(Clase));
            Actualizar(nameof(Actividad));
            Actualizar(nameof(Vista));
        }

        private void EditarClase()
        {
            if (Clase != null)
            {
                Error = "";
                if (string.IsNullOrWhiteSpace(Clase.Nombre))
                {
                    Error += "El nombre de la materia no puede estar vacío.\n";
                }
                if (Clase.HoraInicio < 1 || Clase.HoraInicio > 24 || Clase.HoraFinal < 1 || Clase.HoraFinal > 24 || Clase.HoraInicio > Clase.HoraFinal || Clase.HoraInicio == Clase.HoraFinal)
                {
                    Error += "Ingrese una hora válida.\n";
                }

                bool clases = clasesrepos.GetByDay(Dia).ToList().Exists(x => (Clase.HoraInicio >= x.HoraInicio && Clase.HoraInicio < x.HoraFinal || Clase.HoraFinal > x.HoraInicio && Clase.HoraFinal <= x.HoraFinal || Clase.HoraInicio < x.HoraInicio && Clase.HoraFinal > x.HoraFinal) && x.Id != Clase.Id);
                bool actividades = actrepos.GetByDay(Dia).ToList().Exists(x => Clase.HoraInicio >= x.HoraInicio && Clase.HoraInicio < x.HoraFinal || Clase.HoraFinal > x.HoraInicio && Clase.HoraFinal <= x.HoraFinal || Clase.HoraInicio < x.HoraInicio && Clase.HoraFinal > x.HoraFinal);

                if (clases || actividades)
                {
                    Error += "Ya existe una clase en ese horario.\n";
                }
                if (string.IsNullOrWhiteSpace(Clase.NombreMaestro))
                {
                    Error += "El nombre del maestro no puede estar vacío.\n";
                }
                if (string.IsNullOrWhiteSpace(Clase.Aula))
                {
                    Error += "El aula no puede estar vacía.\n";
                }

                Actualizar(nameof(Error));

                if (Error == "")
                {
                    clasesrepos.Update(Clase);
                    MostrarHorario(Dia);
                    Navegar("MainPage");
                    Vista = Vistas.Horario;
                    Actualizar(nameof(Vistas));
                }
            }
        }

        private void VerDetalles(VistaModel vista)
        {
            if (vista.Tipo == Tipos.Clase)
            {
                ClaseModel clase = clasesrepos.GetById(vista.Id);
                Clase = clase;
                Vista = Vistas.DetallesClase;
            }
            else
            {
                ActividadModel actividad = actrepos.GetById(vista.Id);
                Actividad = actividad;
                Vista = Vistas.DetallesActividad;
            }
            Actualizar(nameof(Clase));
            Actualizar(nameof(Actividad));
            Actualizar(nameof(Vista));
        }

        private void AgregarClase()
        {
            if (Clase != null)
            {
                Error = "";
                if (string.IsNullOrWhiteSpace(Clase.Nombre))
                {
                    Error += "El nombre de la materia no puede estar vacío.\n";
                }
                if (Clase.HoraInicio < 1 || Clase.HoraInicio > 24 || Clase.HoraFinal < 1 || Clase.HoraFinal> 24 || Clase.HoraInicio > Clase.HoraFinal || Clase.HoraInicio == Clase.HoraFinal)
                {
                    Error += "Ingrese una hora válida.\n";
                }

                bool clases = clasesrepos.GetByDay(Dia).ToList().Exists(x => Clase.HoraInicio >= x.HoraInicio && Clase.HoraInicio < x.HoraFinal || Clase.HoraFinal > x.HoraInicio && Clase.HoraFinal <= x.HoraFinal || Clase.HoraInicio < x.HoraInicio && Clase.HoraFinal > x.HoraFinal);
                bool actividades = actrepos.GetByDay(Dia).ToList().Exists(x => Clase.HoraInicio >= x.HoraInicio && Clase.HoraInicio < x.HoraFinal || Clase.HoraFinal > x.HoraInicio && Clase.HoraFinal <= x.HoraFinal || Clase.HoraInicio < x.HoraInicio && Clase.HoraFinal > x.HoraFinal);

                if (clases || actividades)
                {
                    Error += "Ya existe una clase o actividad en ese horario.\n";
                }
                if (string.IsNullOrWhiteSpace(Clase.NombreMaestro))
                {
                    Error += "El nombre del maestro no puede estar vacío.\n";
                }
                if (string.IsNullOrWhiteSpace(Clase.Aula))
                {
                    Error += "El aula no puede estar vacía.\n";
                }

                Actualizar(nameof(Error));

                if (Error == "")
                {
                    clasesrepos.Insert(Clase);
                    MostrarHorario(Dia);
                    Navegar("MainPage");
                    Vista = Vistas.Horario;
                    Actualizar(nameof(Vistas));
                }
            }
        }

        private void VerAgregarClase()
        {
            Vista = Vistas.AgregarClase;
            Error = "";
            Clase = new()
            {
                Dia = Dia,
            };
            Actualizar(nameof(Error));
            Actualizar(nameof(Clase));
            Actualizar(nameof(Vista));
            Navegar("AgregarClase");
        }

        private void Cancelar()
        {
            Vista = Vistas.Horario;
            Navegar("MainPage");
            Actualizar(nameof(Vista));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void MostrarHorario(Dias dia)
        {
            var clases = clasesrepos.GetByDay(dia);
            var actividades = actrepos.GetByDay(dia);

            List<VistaModel> lista = new();

            foreach (var c in clases)
            {
                VistaModel clase = new()
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    HoraInicio = c.HoraInicio,
                    HoraFinal = c.HoraFinal,
                    Tipo = Tipos.Clase
                };
                lista.Add(clase);
            }

            foreach (var a in actividades)
            {
                VistaModel actividad = new()
                {
                    Id = a.Id,
                    Nombre = a.Nombre,
                    HoraInicio = a.HoraInicio,
                    HoraFinal = a.HoraFinal,
                    Tipo = Tipos.Actividad
                };
                lista.Add(actividad);
            }

            var listaordenada = lista.OrderBy(x=>x.HoraInicio).ToList();

            Horario.Clear();

            foreach (var item in listaordenada)
            {
                Horario.Add(item);
            }

            Dia = dia;
            Actualizar(nameof(Dia));
        }

        void Navegar(string ruta)
        {
            Shell.Current.GoToAsync("//" + ruta);
        }

        void Actualizar(string nombre)
        {
            PropertyChanged?.Invoke(this, new(nombre));
        }
    }
}
