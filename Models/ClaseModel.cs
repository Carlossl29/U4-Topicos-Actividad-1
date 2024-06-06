using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLite;
using System.Threading.Tasks;
using U4_Topicos_Actividad_1.ViewModels;

namespace U4_Topicos_Actividad_1.Models
{
    public class ClaseModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [NotNull, MaxLength(100)]
        public string Nombre { get; set; } = "";
        [NotNull, MaxLength(100)]
        public string NombreMaestro { get; set; } = "";
        [NotNull, MaxLength(50)]
        public string Aula { get; set; } = "";
        [NotNull]
        public int HoraInicio { get; set; }
        [NotNull]
        public int HoraFinal { get; set; }
        [NotNull]
        public Dias Dia { get; set; }
    }
}
