using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using U4_Topicos_Actividad_1.ViewModels;

namespace U4_Topicos_Actividad_1.Models
{
    public class VistaModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = "";
        public int HoraInicio { get; set; }
        public int HoraFinal { get; set; }
        public Tipos Tipo { get; set; }

    }
}
