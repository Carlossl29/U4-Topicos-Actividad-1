using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLite;
using System.Threading.Tasks;
using U4_Topicos_Actividad_1.Models;
using U4_Topicos_Actividad_1.ViewModels;

namespace U4_Topicos_Actividad_1.Repositories
{
    public class ActividadesRepository
    {
        SQLiteConnection conexion;
        public ActividadesRepository()
        {
            string ruta = FileSystem.AppDataDirectory + "/horario.sqlite";
            conexion = new(ruta);
            conexion.CreateTable<ActividadModel>();
        }

        public ActividadModel GetById(int id)
        {
            return conexion.Get<ActividadModel>(id);
        }

        public IEnumerable<ActividadModel> GetByDay(Dias dia)
        {
            return conexion.Table<ActividadModel>().Where(x => x.Dia == dia);
        }

        public void Insert(ActividadModel act)
        {
            conexion.Insert(act);
        }

        public void Update(ActividadModel act)
        {
            conexion.Update(act);
        }
        public void Delete(ActividadModel act)
        {
            conexion.Delete(act);
        }
    }
}
