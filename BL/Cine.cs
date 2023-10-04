using Microsoft.EntityFrameworkCore;

namespace BL
{
    public class Cine
    {
        public static ML.Result GetAll()
        {
            ML.Result resultado = new ML.Result();

            try
            {
                using (DL.DtorresCineContext conexion = new DL.DtorresCineContext())
                {
                    var query = conexion.Cines.FromSqlRaw("CineGetAll").ToList();

                    if (query.Count > 0)
                    {
                        resultado.Objects = new List<object>();

                        foreach (var cineResult in query)
                        {
                            ML.Cine cine = new ML.Cine();
                            cine.Zona = new ML.Zona();

                            cine.Id = cineResult.IdCine;
                            cine.Nombre = cineResult.Nombre;
                            cine.Direccion = cineResult.Direccion;
                            cine.Zona.Id = cineResult.IdZona.Value;
                            cine.Zona.Nombre = cineResult.NombreCine;
                            cine.Ventas = cineResult.Ventas.Value;

                            resultado.Objects.Add(cine);
                        }

                        resultado.Correct = true;
                    }
                    else
                    {
                        resultado.Correct = false;
                        resultado.ErrorMessage = "No se encontraron datos de algun cine";
                    }
                }
            }
            catch (Exception ex)
            {
                resultado.Correct = false;
                resultado.ErrorMessage = ex.Message;
                resultado.Ex = ex;
            }

            return resultado;
        }
    }
}