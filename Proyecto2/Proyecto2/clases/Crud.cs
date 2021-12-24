using Proyecto2.model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto2.clases
{
    class Crud
    {
        Conexion conn = new Conexion();



        public Task<List<UbicacionModel>> getReadUbicacion()
        {
            return conn.GetConnectionAsync().Table<UbicacionModel>().ToListAsync();
        }

        public Task<UbicacionModel> getUbicacionId(int id)
        {
            return conn
                .GetConnectionAsync()
                .Table<UbicacionModel>()
                .Where(item => item.id == id)
                .FirstOrDefaultAsync();
        }

        public Task<int> getUbicacionUpdateId(UbicacionModel ubicacion)
        {
            return conn
                .GetConnectionAsync()
                .UpdateAsync(ubicacion);

        }

        public Task<int> Delete(UbicacionModel ubicacion)
        {
            return conn
                .GetConnectionAsync()
                .DeleteAsync(ubicacion);
        }
    }
}