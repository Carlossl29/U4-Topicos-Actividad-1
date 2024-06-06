using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using U4_Topicos_Actividad_1.Models;
using U4_Topicos_Actividad_1.ViewModels;

namespace U4_Topicos_Actividad_1.Repositories
{
    public class ClasesRepository
    {
        SQLiteConnection conexion;

        public ClasesRepository()
        {
            string ruta = FileSystem.AppDataDirectory + "/horario.sqlite";
            conexion = new(ruta);
            conexion.CreateTable<ClaseModel>();
        }

        public ClaseModel GetById(int id)
        {
            return conexion.Get<ClaseModel>(id);
        }
        public IEnumerable<ClaseModel> GetByDay(Dias dia)
        {
            return conexion.Table<ClaseModel>().Where(x => x.Dia == dia);
        }

        public void Insert(ClaseModel clase)
        {
            conexion.Insert(clase);
        }

        public void Update(ClaseModel clase)
        {
            conexion.Update(clase);
        }
        public void Delete(ClaseModel clase)
        {
            conexion.Delete(clase);
        }
    }
}
