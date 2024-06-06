using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using U4_Topicos_Actividad_1.ViewModels;

namespace U4_Topicos_Actividad_1.Models
{
    public class ActividadModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [NotNull, MaxLength(100)]
        public string Nombre { get; set; } = "";
        [NotNull, MaxLength(255)]
        public string Descripcion { get; set; } = "";
        [NotNull]
        public int HoraInicio { get; set; }
        [NotNull]
        public int HoraFinal { get; set; }
        [NotNull]
        public Dias Dia { get; set; }
    }
}
